using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WildApricot
{
	public partial class WildApricotClient
	{
		public static string ApiKey;

		private readonly string token;
		public readonly double accountId;

		public WildApricotClient()
		{
			// From generated
			BaseUrl = "https://api.wildapricot.org/v2.3";
			_httpClient = new HttpClient();
			Initialize();

			// Get OAuth token using key
			var authToken = Encoding.ASCII.GetBytes($"APIKEY:{ApiKey}");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

			var body = new StringContent("grant_type=client_credentials&scope=auto");
			body.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

			var tokenRequest = _httpClient.PostAsync("https://oauth.wildapricot.org/auth/token", body).Result;
			
			tokenRequest.EnsureSuccessStatusCode();

			var reply = JsonConvert.DeserializeObject<dynamic>(tokenRequest.Content.ReadAsStringAsync().Result);

			token = reply.access_token;
			accountId = reply.Permissions[0].AccountId;

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}
	}
}
