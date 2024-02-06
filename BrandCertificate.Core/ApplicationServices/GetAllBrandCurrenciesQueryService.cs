using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetAllBrandCurrenciesQueryService(ITgsRepository tgsRepository) : IQueryService<GetAllBrandCurrenciesQuery, BrandCurrencyInfo>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public IAsyncEnumerable<BrandCurrencyInfo> QueryAsync(
		GetAllBrandCurrenciesQuery query,
		CancellationToken cancellationToken = default)
		=> _tgsRepository.GetAllBrandCurrenciesAsync(cancellationToken);
}
