using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rssdp;
using Rssdp.Infrastructure;

namespace Mms.Api
{
	public class Program
	{
		private static SsdpDevicePublisher SsdpPublisher4;
		public static string SsdpDescription { get; private set; }

		public static int Main(string[] args)
		{
			var host = new HostBuilder()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseKestrel()
					.UseStartup<Startup>()
					.UseUrls("http://*:80")
					.UseStaticWebAssets();
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConsole();
					logging.SetMinimumLevel(LogLevel.Warning);
				})
				.Build();

			var task = host.RunAsync();

			PublishDevice();

			task.GetAwaiter().GetResult();

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

				Console.WriteLine($"Publishing SSDP on {ip4}");

				SsdpPublisher4 = new SsdpDevicePublisher(new SsdpCommunicationsServer(new SocketFactory(ip4)));
				SsdpPublisher4.StandardsMode = SsdpStandardsMode.Relaxed;
				SsdpPublisher4.AddDevice(deviceDefinition4);

				SsdpDescription = deviceDefinition4.ToDescriptionDocument();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
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
