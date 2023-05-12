using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandInfoByCodeQueryService : IGetService<GetBrandInfoByCodeQuery, BrandInfo?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandInfoByCodeQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<BrandInfo?> GetAsync(GetBrandInfoByCodeQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandInfoByCodeAsync(query.BrandCode, cancellationToken);
}
