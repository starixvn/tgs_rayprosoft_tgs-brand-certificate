using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandPlatformAccountFundCheckFlagQueryService(ITgsRepository tgsRepository)
	: IGetService<GetBrandPlatformAccountFundCheckFlagQuery, bool?>
{
	public ValueTask<bool?> GetAsync(
		GetBrandPlatformAccountFundCheckFlagQuery query,
		CancellationToken cancellationToken = default)
		=> tgsRepository.GetBrandPlatformAccountFundCheckFlagAsync(query.BrandId, cancellationToken);
}
