using Tgs.BrandCertificate.Clients;

namespace Microsoft.AspNetCore.Mvc
{
	public static class MvcOptionsExtensions
	{
		public static void AddBrandCertificateFilters(this MvcOptions options)
		{
			options.Filters.Add<BrandCertificateAuthenticationFilter>();
		}
	}
}
