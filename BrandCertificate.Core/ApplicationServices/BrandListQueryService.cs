using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class BrandListQueryService : IQueryService<BrandListQuery, BrandListItem>
{
	private readonly ITgsRepository _tgsRepository;

	public BrandListQueryService(
		ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<BrandListItem> QueryAsync(
		BrandListQuery query,
		CancellationToken cancellationToken = default)
		=> _tgsRepository.QueryBrandListAsync(query.BrandCode, query.Status, cancellationToken);
}