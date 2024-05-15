using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients;

internal class BrandCertificateMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<BrandCertificateMiddleware> _logger;

	public BrandCertificateMiddleware(RequestDelegate next, ILogger<BrandCertificateMiddleware> logger)
	{
		_next = next;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task InvokeAsync(HttpContext context, IBrandCertificateClient brandCertificateClient)
	{
		if (context.Request.Headers.TryGetValue("brand-token", out var values))
		{
			var jwt = values.FirstOrDefault();

			if (!string.IsNullOrWhiteSpace(jwt))
			{
				_logger.LogDebug("Receive brand-token: {jwt}", jwt);

				var securityKey = await brandCertificateClient.GetSystemValidateSecurityKeyAsync(
					context.RequestAborted).ConfigureAwait(false);

				var handler = new JwtSecurityTokenHandler();

				try
				{
					var issuers = new[] { "tgs" };
					var result = await handler.ValidateTokenAsync(
						jwt,
						new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidateAudience = false,
							ValidateLifetime = false,
							ValidIssuers = issuers,
							NameClaimType = ClaimTypes.NameIdentifier,
							IssuerSigningKey = securityKey
						}).ConfigureAwait(false);

					var brandId = Guid.Parse(result.ClaimsIdentity.Name!);

					context.Features.Set<IBrandFeature>(new BrandFeature
					{
						BrandId = brandId
					});

					_logger.LogDebug("Catch brand id is {brandId}", brandId);
				}
				catch (Exception ex)
				{
					if (_logger.IsEnabled(LogLevel.Debug))
						_logger.LogError(ex, "Valid brand-token occur error!");
				}
			}
		}
		await _next(context).ConfigureAwait(false);
	}
}
