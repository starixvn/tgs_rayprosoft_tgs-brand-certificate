using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandAgentDividendInfoQueryService : IQueryService<GetBrandAgentDividendInfoQuery, BrandAgentDividendInfo>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandAgentDividendInfoQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<BrandAgentDividendInfo> QueryAsync(GetBrandAgentDividendInfoQuery query,
		CancellationToken cancellationToken = default)
	{
		return _tgsRepository.GetBrandAgentDividendInfosAsync(cancellationToken);
	}
}
