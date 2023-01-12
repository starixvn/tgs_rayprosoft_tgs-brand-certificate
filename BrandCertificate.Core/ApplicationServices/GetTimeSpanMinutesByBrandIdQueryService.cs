using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetTimeSpanMinutesByBrandIdQueryService : IGetService<GetTimeSpanMinutesByBrandIdQuery, int?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetTimeSpanMinutesByBrandIdQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<int?> GetAsync(GetTimeSpanMinutesByBrandIdQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetTimeSpanMinutesByBrandIdAsync(query.BrandId, cancellationToken);
}
