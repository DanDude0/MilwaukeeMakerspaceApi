using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mms.Api.Services;
using Mms.Database;
using WildApricot;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Mms.Api
{
	public class Startup
	{
		private string waClientId;
		private string waClientSecret;

		public Startup(IHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();

			AreaFundingDatabase.ConnectionString = Configuration.GetConnectionString("area_funding");
			AccessControlDatabase.ConnectionString = Configuration.GetConnectionString("access_control");
			BillingDatabase.ConnectionString = Configuration.GetConnectionString("billing");
			WildApricotClient.ApiKey = Configuration.GetConnectionString("waApiKey");
			waClientId = Configuration.GetConnectionString("waClientId");
			waClientSecret = Configuration.GetConnectionString("waClientSecret");
			//DinkToPdfAll.LibraryLoader.Load();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.

			// If they're not using OAuth, don't try to load it. Will break other things.
			if (!string.IsNullOrWhiteSpace(waClientId) && !string.IsNullOrWhiteSpace(waClientId)) {
				services.AddAuthentication(options =>
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
			}

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddControllersWithViews();
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddMvc(options => options.InputFormatters.Insert(0, new RawStringInputFormatter()));
			services.AddSingleton<AttemptService>();
			services.AddSingleton<InvoiceService>();
			services.AddSingleton<ReportService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions {
				ForwardedHeaders = ForwardedHeaders.All,
				RequireHeaderSymmetry = false,
				ForwardLimit = null,
				KnownNetworks = { new IPNetwork(IPAddress.Parse("192.168.86.0"), 24) },
			}); ;

			//if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
			//}
			//else {
			//	app.UseExceptionHandler("/Error");
			//}

			app.UseStaticFiles();
			app.UseRouting();
			app.UseCookiePolicy(new CookiePolicyOptions {
				MinimumSameSitePolicy = SameSiteMode.None,
				Secure = CookieSecurePolicy.Always
			});
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
				endpoints.MapControllerRoute(
					"default",
					"{controller}/{action=Index}");
			});
		}
	}
}
