using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mvc.Client.Controllers;

public class AuthenticationController : Controller
{
	[HttpGet("/login")]
	[HttpPost("/login")]
	public IActionResult SignIn()
	{
		// Instruct the middleware corresponding to the requested external identity
		// provider to redirect the user agent to its own authorization endpoint.
		// Note: the authenticationScheme parameter must match the value configured in Startup.cs
		return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "WildApricot");
	}

	[HttpGet("/logout")]
	[HttpPost("/logout")]
	public IActionResult SignOutCurrentUser()
	{
		// Instruct the cookies middleware to delete the local cookie created
		// when the user agent is redirected from the external identity provider
		// after a successful authentication flow (e.g Google or Facebook).
		return SignOut(new AuthenticationProperties { RedirectUri = "/" },
			CookieAuthenticationDefaults.AuthenticationScheme);
	}

	private static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(HttpContext context)
	{
		ArgumentNullException.ThrowIfNull(context);

		var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

		return (from scheme in await schemes.GetAllSchemesAsync()
				where !string.IsNullOrEmpty(scheme.DisplayName)
				select scheme).ToArray();
	}

	private static async Task<bool> IsProviderSupportedAsync(HttpContext context, string provider)
	{
		ArgumentNullException.ThrowIfNull(context);

		return (from scheme in await GetExternalProvidersAsync(context)
				where string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase)
				select scheme).Any();
	}
}
