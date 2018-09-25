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
using Microsoft.Extensions.Configuration;
using Rssdp;
using Rssdp.Infrastructure;

namespace Mms.Api
{
	public class Program
	{
		private static SsdpDevicePublisher SsdpPublisher4;
		private static SsdpDevicePublisher SsdpPublisher6;
		public static string SsdpDescription { get; private set; }

		public static int Main(string[] args)
		{
			PublishDevice();

			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.UseUrls("http://*:80")
				.Build();

			host.Run();

			return 0;
		}

		// Call this method from somewhere to actually do the publish.
		private static void PublishDevice()
		{
			var ip4 = GetLocalIp4Address();
			var ip6 = GetLocalIp6Address();
			var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			var deviceDefinition4 = new SsdpRootDevice()
			{
				Location = new Uri($"http://{ip4}/home/service"),
				PresentationUrl = new Uri($"http://{ip4}/"),
				FriendlyName = "Milwaukee Makerspace Api",
				Manufacturer = "Milwaukee Makerspace",
				ModelName = "Milwaukee Makerspace Api",
				Uuid = "6111f321-2cee-455e-b203-4abfaf14b516",
				ManufacturerUrl = new Uri("https://milwaukeemakerspace.org/"),
				ModelUrl = new Uri("https://github.com/DanDude0/MilwaukeeMakerspaceApi/"),
				ModelNumber = version,
			};

			SsdpPublisher4 = new SsdpDevicePublisher(new SsdpCommunicationsServer(new SocketFactory(ip4)));
			SsdpPublisher4.StandardsMode = SsdpStandardsMode.Relaxed;
			SsdpPublisher4.AddDevice(deviceDefinition4);

			var deviceDefinition6 = new SsdpRootDevice()
			{
				Location = new Uri($"http://[{ip6}]/home/service"),
				PresentationUrl = new Uri($"http://[{ip6}]/"),
				FriendlyName = "Milwaukee Makerspace Api",
				Manufacturer = "Milwaukee Makerspace",
				ModelName = "Milwaukee Makerspace Api",
				Uuid = "6111f321-2cee-455e-b203-4abfaf14b516",
				ManufacturerUrl = new Uri("https://milwaukeemakerspace.org/"),
				ModelUrl = new Uri("https://github.com/DanDude0/MilwaukeeMakerspaceApi/"),
				ModelNumber = version,
			};

			SsdpDescription = deviceDefinition6.ToDescriptionDocument();

			SsdpPublisher6 = new SsdpDevicePublisher(new SsdpCommunicationsServer(new SocketFactory(ip6)));
			SsdpPublisher6.StandardsMode = SsdpStandardsMode.Relaxed;
			SsdpPublisher6.AddDevice(deviceDefinition6);
		}

		private static string GetLocalIp6Address()
		{
			UnicastIPAddressInformation mostSuitableIp = null;

			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

			foreach (var network in networkInterfaces)
			{
				if (network.OperationalStatus != OperationalStatus.Up)
					continue;

				var properties = network.GetIPProperties();

				if (properties.GatewayAddresses.Count == 0)
					continue;

				foreach (var address in properties.UnicastAddresses)
				{
					if (address.Address.AddressFamily != AddressFamily.InterNetworkV6)
						continue;

					if (IPAddress.IsLoopback(address.Address))
						continue;

					if (!address.IsDnsEligible)
					{
						if (mostSuitableIp == null)
							mostSuitableIp = address;
						continue;
					}

					// The best IP is the IP got from DHCP server
					if (address.PrefixOrigin != PrefixOrigin.Dhcp)
					{
						if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
							mostSuitableIp = address;
						continue;
					}

					return address.Address.ToString();
				}
			}

			return mostSuitableIp != null
				? mostSuitableIp.Address.ToString()
				: "";
		}

		private static string GetLocalIp4Address()
		{
			UnicastIPAddressInformation mostSuitableIp = null;

			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

			foreach (var network in networkInterfaces)
			{
				if (network.OperationalStatus != OperationalStatus.Up)
					continue;

				var properties = network.GetIPProperties();

				if (properties.GatewayAddresses.Count == 0)
					continue;

				foreach (var address in properties.UnicastAddresses)
				{
					if (address.Address.AddressFamily != AddressFamily.InterNetwork)
						continue;

					if (IPAddress.IsLoopback(address.Address))
						continue;

					if (!address.IsDnsEligible)
					{
						if (mostSuitableIp == null)
							mostSuitableIp = address;
						continue;
					}

					// The best IP is the IP got from DHCP server
					if (address.PrefixOrigin != PrefixOrigin.Dhcp)
					{
						if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
							mostSuitableIp = address;
						continue;
					}

					return address.Address.ToString();
				}
			}

			return mostSuitableIp != null
				? mostSuitableIp.Address.ToString()
				: "";
		}
	}
}
