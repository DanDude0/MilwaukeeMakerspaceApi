using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Mms.Api
{
	public class AuthorizingHandler : DelegatingHandler
	{
		private readonly OAuthOptions _options;
		public AuthorizingHandler(HttpMessageHandler inner, OAuthOptions options)
			: base(inner)
		{
			_options = options;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (request.RequestUri == new Uri(_options.TokenEndpoint)) {
				var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(_options.ClientId + ":" + _options.ClientSecret));

				request.Headers.Add("Authorization", $"Basic {credentials}");
			}
			return base.SendAsync(request, cancellationToken);
		}
	}
}
