using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mms.Api.Jobs;
using Mms.Api.Services;
using Mms.Database;
using Rssdp;
using Rssdp.Infrastructure;
using Serilog;
using Serilog.Events;
using WildApricot;

namespace Mms.Api
{
	public class Program
	{
		private static SsdpDevicePublisher SsdpPublisher4;
		public static string SsdpDescription { get; private set; }
		public static List<string> BackupServers { get; private set; }

		public static int Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			try {
				Log.Information("Building Web Application");

				var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
					Args = args,
					WebRootPath = "webroot",
				});

				builder.Host.UseSerilog((context, services, configuration) => configuration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console());

				builder.WebHost.UseUrls("http://*:80");

				AreaFundingDatabase.ConnectionString = builder.Configuration.GetConnectionString("area_funding");
				AccessControlDatabase.ConnectionString = builder.Configuration.GetConnectionString("access_control");
				BillingDatabase.ConnectionString = builder.Configuration.GetConnectionString("billing");
				WildApricotClient.ApiKey = builder.Configuration["WildApricot:ApiKey"];
				var waClientId = builder.Configuration["WildApricot:ClientId"];
				var waClientSecret = builder.Configuration["WildApricot:ClientSecret"];
				var qbClientId = builder.Configuration["QuickBooks:ClientId"];
				var qbClientSecret = builder.Configuration["QuickBooks:ClientSecret"];
				var reverseProxyNetwork = builder.Configuration["ReverseProxyNetwork"];
				BackupServers = builder.Configuration.GetSection("BackupServers").Get<List<string>>();

				if (!string.IsNullOrWhiteSpace(waClientId) && !string.IsNullOrWhiteSpace(waClientId)) {
					builder.Services.AddAuthentication(options =>
					{
						options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
						options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
						options.DefaultChallengeScheme = "WildApricot";
					})
						.AddCookie(options =>
						{
							options.LoginPath = "/login";
							options.LogoutPath = "/logout";
							options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
							options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
						})
						.AddOAuth("WildApricot", "WildApricot", options =>
						{
							options.AuthorizationEndpoint = "https://members.milwaukeemakerspace.org/sys/login/OAuthLogin";
							options.Scope.Add("contacts_me");
							//options.Scope.Add("identify");
							//options.Scope.Add("email");

							options.CallbackPath = "/logincallback";

							options.ClientId = waClientId;
							options.ClientSecret = waClientSecret;

							options.TokenEndpoint = "https://oauth.wildapricot.org/auth/token";

							var innerHandler = new HttpClientHandler();
							options.BackchannelHttpHandler = new AuthorizingHandler(innerHandler, options);

							options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "Id");
							options.ClaimActions.MapJsonKey(ClaimTypes.Name, "DisplayName");
							options.ClaimActions.MapJsonKey(ClaimTypes.Email, "Email");
							options.ClaimActions.MapJsonKey(ClaimTypes.Webpage, "Url");

							options.AccessDeniedPath = "/loginfailed";

							options.Events = new OAuthEvents {
								OnCreatingTicket = async context =>
								{
									var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
									request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
									request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

									var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

									var userString = await response.Content.ReadAsStringAsync();

									var user = JsonDocument.Parse(userString).RootElement;

									if (userString.Contains("\"AdministrativeRoleTypes\":[\"AccountAdministrator\"],"))
										context.Identity.AddClaim(new Claim("AccountAdministrator", "True"));

									context.RunClaimActions(user);
								}
							};

							try {
								var wildApricot = new WildApricotClient();

								options.UserInformationEndpoint = $"https://api.wildapricot.org/v2.2/accounts/{wildApricot.accountId}/contacts/me";
							}
							catch {
								// If we can't load wild apricot for remote services, don't kill local services.
								return;
							}
						});
					builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
				}

				builder.Services.AddRazorPages();
				builder.Services.AddServerSideBlazor();
				builder.Services.AddControllersWithViews();
				builder.Services.AddRouting(options => options.LowercaseUrls = true);
				builder.Services.AddMvc(options => options.InputFormatters.Insert(0, new RawStringInputFormatter()));
				builder.Services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new OpenApiInfo { Title = "MmsApi", Version = "v1" }); });
				builder.Services.AddSingleton<AttemptService>();
				builder.Services.AddSingleton<InvoiceService>();
				builder.Services.AddSingleton<ReportService>();

				builder.Services.AddScheduler(options =>
				{
					options.AddJob<PullMembersFromWildApricot>();
					options.AddUnobservedTaskExceptionHandler(sp =>
					{
						return
							(sender, args) =>
							{
								Log.Error(args.Exception, "Unhandled Exception Running Job");
								args.SetObserved();
							};
					});
				});

				builder.WebHost.UseStaticWebAssets();

				var app = builder.Build();

				app.UseSerilogRequestLogging();

				if (!string.IsNullOrWhiteSpace(reverseProxyNetwork)) {
					try {
						app.UseForwardedHeaders(new ForwardedHeadersOptions {
							ForwardedHeaders = ForwardedHeaders.All,
							RequireHeaderSymmetry = false,
							ForwardLimit = null,
							KnownNetworks = { Microsoft.AspNetCore.HttpOverrides.IPNetwork.Parse(builder.Configuration["ReverseProxyNetwork"]) },
						});
					}
					catch (Exception ex) {
						Log.Fatal(ex, $"Error Enabling Reverse Proxy for network: {reverseProxyNetwork}");
					}
				}

				if (app.Environment.IsDevelopment()) {
					app.UseDeveloperExceptionPage();
				}
				else {
					app.UseExceptionHandler("/Error");
				}

				app.UseStaticFiles();
				app.UseRouting();
				app.UseCookiePolicy(new CookiePolicyOptions {
					MinimumSameSitePolicy = SameSiteMode.None,
					Secure = CookieSecurePolicy.Always
				});
				app.UseAuthentication();
				app.UseAuthorization();

				app.MapBlazorHub();
				app.MapFallbackToPage("/_Host");
				app.MapControllerRoute(
						"default",
						"{controller}/{action=Index}");

				app.UseSwagger();
				app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "MmsApi v1"); });

				var task = app.RunAsync();

				PublishDevice();

				task.GetAwaiter().GetResult();
			}
			catch (Exception ex) {
				Log.Fatal(ex, "Application terminated unexpectedly");
			}

			Log.CloseAndFlush();

			return 0;
		}

		// Call this method from somewhere to actually do the publish.
		private static void PublishDevice()
		{
			try {
				var ip4 = GetLocalIp4Address();
				var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

				var deviceDefinition4 = new SsdpRootDevice() {
					Location = new Uri($"http://{ip4}/info/service"),
					PresentationUrl = new Uri($"http://{ip4}/"),
					FriendlyName = "Milwaukee Makerspace Api",
					Manufacturer = "Milwaukee Makerspace",
					ModelName = "Milwaukee Makerspace Api",
					Uuid = "6111f321-2cee-455e-b203-4abfaf14b516",
					ManufacturerUrl = new Uri("https://milwaukeemakerspace.org/"),
					ModelUrl = new Uri("https://github.com/DanDude0/MilwaukeeMakerspaceApi/"),
					ModelNumber = version,
				};

				// Have to bind to all addresses on Linux, or broadcasts don't work!
				if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)) {
					ip4 = IPAddress.Any.ToString();
				}

				Log.Information($"Publishing SSDP on {ip4}");

				SsdpPublisher4 = new SsdpDevicePublisher(new SsdpCommunicationsServer(new SocketFactory(ip4)));
				SsdpPublisher4.StandardsMode = SsdpStandardsMode.Relaxed;
				SsdpPublisher4.AddDevice(deviceDefinition4);

				SsdpDescription = deviceDefinition4.ToDescriptionDocument();
			}
			catch (Exception ex) {
				Log.Fatal(ex, "Error publishing device over SSDP");
			}
		}

		private static string GetLocalIp4Address()
		{
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

			foreach (var network in networkInterfaces) {
				if (network.OperationalStatus != OperationalStatus.Up)
					continue;

				var properties = network.GetIPProperties();

				if (properties.GatewayAddresses.Count == 0)
					continue;

				foreach (var address in properties.UnicastAddresses) {
					if (address.Address.AddressFamily != AddressFamily.InterNetwork)
						continue;

					if (IPAddress.IsLoopback(address.Address))
						continue;

					return address.Address.ToString();
				}
			}

			throw new Exception("No IP Address Found for SSDP");
		}
	}
}
