using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class ListBrandPlayerContractDisplayQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListBrandPlayerContractDisplayQuery, BrandPlayerContractDisplayInfo>
{
	public IAsyncEnumerable<BrandPlayerContractDisplayInfo> QueryAsync(
		ListBrandPlayerContractDisplayQuery query, 
		CancellationToken cancellationToken = default)
		=> tgsRepository.QueryBrandPlayerContractDisplayAsync(query.BrandId, cancellationToken);
}