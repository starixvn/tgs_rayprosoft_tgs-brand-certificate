using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients
{
	internal class BrandCertificateMiddleware
	{
		private readonly RequestDelegate _next;

		public BrandCertificateMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IBrandCertificateClient brandCertificateClient)
		{
			if (context.Request.Headers.TryGetValue("Brand-Token", out var values))
			{
				var jwt = values.FirstOrDefault();

				if (!string.IsNullOrWhiteSpace(jwt))
				{
					var securityKey = await brandCertificateClient.GetSystemValidateSecurityKeyAsync();

					var handler = new JwtSecurityTokenHandler();

					try
					{
						var principal = handler.ValidateToken(
							jwt,
							new TokenValidationParameters
							{
								ValidateIssuer = true,
								ValidateAudience = false,
								ValidateLifetime = false,
								ValidIssuers = new[] { "tgs" },
								NameClaimType = ClaimTypes.NameIdentifier,
								IssuerSigningKey = securityKey
							},
							out _);

						var brandId = Guid.Parse(principal.Identity!.Name!);

						context.Features.Set<IBrandFeature>(new BrandFeature
						{
							BrandId = brandId
						});
					}
					catch (Exception)
					{
					}
				}
			}
			await _next(context);
		}
	}
}
