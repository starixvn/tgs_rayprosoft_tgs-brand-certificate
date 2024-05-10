using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandFullInfoQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandFullInfoQuery, BrandFullInfo?>
{
	public ValueTask<BrandFullInfo?> GetAsync(GetBrandFullInfoQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetBrandFullInfoAsync(query.BrandId, cancellationToken);
}
