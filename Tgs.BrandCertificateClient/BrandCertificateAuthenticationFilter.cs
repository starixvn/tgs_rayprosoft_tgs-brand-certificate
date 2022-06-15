using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tgs.BrandCertificate.Clients;

internal class BrandCertificateAuthenticationFilter : IAsyncAuthorizationFilter
{
	public Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var brandFeature = context.HttpContext.Features.Get<IBrandFeature>();

		if (brandFeature == null)
			context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

		return Task.CompletedTask;
	}
}
