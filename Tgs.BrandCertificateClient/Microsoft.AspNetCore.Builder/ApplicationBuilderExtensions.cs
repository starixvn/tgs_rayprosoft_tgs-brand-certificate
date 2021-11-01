using Tgs.BrandCertificate.Clients;

namespace Microsoft.AspNetCore.Builder
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseBrandCertificate(this IApplicationBuilder app)
		{
			app.UseMiddleware<BrandCertificateMiddleware>();

			return app;
		}
	}
}
