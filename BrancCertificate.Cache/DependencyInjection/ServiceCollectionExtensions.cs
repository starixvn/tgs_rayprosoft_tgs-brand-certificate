using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificate.Cache;
using Tgs.ServiceUtility;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBrandCertificateCache(this IServiceCollection services)
		=> services
			.Decorate<IGetService<GetBrandInfoQuery, BrandInfo?>, GetCachedBrandInfoQueryService>()
			.Decorate<IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo?>, GetCachedBrandFrontendInfoQueryService>()
			.Decorate<IQueryService<GetVipLevelsByBrandQuery, VipLevel>, GetCachedVipLevelsByBrandQueryService>();
}
