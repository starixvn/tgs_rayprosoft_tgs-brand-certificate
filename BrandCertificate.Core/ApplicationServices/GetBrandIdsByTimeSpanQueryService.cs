using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandIdsByTimeSpanQueryService : IQueryService<GetBrandIdsByTimeSpanQuery, Guid>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandIdsByTimeSpanQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<Guid> QueryAsync(GetBrandIdsByTimeSpanQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandIdsByTimeSpanAsync(query.TimeSpan, cancellationToken);
}
