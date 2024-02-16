using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class BrandListQueryService(
	ITgsRepository tgsRepository) : IQueryService<BrandListQuery, BrandListItem>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public IAsyncEnumerable<BrandListItem> QueryAsync(
		BrandListQuery query,
		CancellationToken cancellationToken = default)
	{
		var brands = _tgsRepository.QueryAllBrandsAsync(cancellationToken);

		if (query.BrandIds.Any())
			brands = brands.Where(b => query.BrandIds.Contains(b.BrandId));

		if (query.Status is not null)
			brands = brands.Where(b => b.Status == query.Status);

		if (!string.IsNullOrEmpty(query.Keyword))
		{
			brands = brands
				.Where(b => b.BrandCode.Contains(query.Keyword, StringComparison.OrdinalIgnoreCase)
							|| b.BrandName.Contains(query.Keyword, StringComparison.OrdinalIgnoreCase));
		}

		return brands;
	}
}
