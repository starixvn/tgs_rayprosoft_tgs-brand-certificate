using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandBetLimitIdQueryService : IGetService<GetBrandBetLimitIdQuery, int?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandBetLimitIdQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<int?> GetAsync(GetBrandBetLimitIdQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandBetLimitIdAsync(query.BrandId, cancellationToken);
}
