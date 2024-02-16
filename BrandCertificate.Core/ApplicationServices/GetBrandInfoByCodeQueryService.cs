using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandInfoByCodeQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandInfoByCodeQuery, BrandInfo?>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<BrandInfo?> GetAsync(GetBrandInfoByCodeQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandInfoByCodeAsync(query.BrandCode, cancellationToken);
}
