using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mms.Database;

namespace Mms.Api
{
	public class Startup
	{
		public Startup(IHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();

			AreaFundingDatabase.ConnectionString = Configuration.GetConnectionString("area_funding");
			AccessControlDatabase.ConnectionString = Configuration.GetConnectionString("access_control");
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddControllersWithViews();
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddMvc(options => options.InputFormatters.Insert(0, new RawStringInputFormatter()));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(routes =>
			{
				routes.MapControllerRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}/{key?}");
			});
		}
	}
}
