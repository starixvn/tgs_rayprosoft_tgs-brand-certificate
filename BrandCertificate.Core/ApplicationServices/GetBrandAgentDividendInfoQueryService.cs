using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandAgentDividendInfoQueryService(ITgsRepository tgsRepository)
	: IQueryService<GetBrandAgentDividendInfoQuery, BrandAgentDividendInfo>
{
	public IAsyncEnumerable<BrandAgentDividendInfo> QueryAsync(
		GetBrandAgentDividendInfoQuery query,
		CancellationToken cancellationToken = default)
		=> tgsRepository.GetBrandAgentDividendInfosAsync(cancellationToken);
}
