using System;
using Microsoft.AspNetCore.Http;
using Tgs.BrandCertificate.Clients;

namespace Microsoft.AspNetCore.Mvc
{
	public static class HttpContextExtensions
	{
		public static Guid? GetBrandId(this HttpContext httpContext)
		{
			var brandFeature = httpContext.Features.Get<IBrandFeature>();

			return brandFeature?.BrandId;
		}
	}
}
