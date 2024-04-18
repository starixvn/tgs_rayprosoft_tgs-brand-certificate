using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandProductFeeInfoQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandProductFeeInfoQuery, BrandProductFeeInfo?>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<BrandProductFeeInfo?> GetAsync(GetBrandProductFeeInfoQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandProductFeeAsync(query.BrandId, cancellationToken);
}
