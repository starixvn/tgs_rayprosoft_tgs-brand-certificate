using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetAllBrandCurrenciesQueryService : IQueryService<GetAllBrandCurrenciesQuery, BrandCurrencyInfo>
{
	private readonly ITgsRepository _tgsRepository;

	public GetAllBrandCurrenciesQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<BrandCurrencyInfo> QueryAsync(
		GetAllBrandCurrenciesQuery query,
		CancellationToken cancellationToken = default)
		=> _tgsRepository.GetAllBrandCurrenciesAsync(cancellationToken);
}
