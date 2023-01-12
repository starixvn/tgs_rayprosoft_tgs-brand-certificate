using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandIdsByTimeSpanMinutesQueryService : IQueryService<GetBrandIdsByTimeSpanMinutesQuery, Guid>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandIdsByTimeSpanMinutesQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<Guid> QueryAsync(GetBrandIdsByTimeSpanMinutesQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandIdsByTimeSpanMinutesAsync(query.TimeSpan, cancellationToken);
}
