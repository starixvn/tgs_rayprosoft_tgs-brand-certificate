using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandIdsByTimeSpanMinutesQueryService(ITgsRepository tgsRepository) : IQueryService<GetBrandIdsByTimeSpanMinutesQuery, Guid>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public IAsyncEnumerable<Guid> QueryAsync(GetBrandIdsByTimeSpanMinutesQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandIdsByTimeSpanMinutesAsync(query.TimeSpan, cancellationToken);
}
