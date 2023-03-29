using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class BrandRelationQueryService : IQueryService<BrandRelationQuery, BrandRelationInfo>
{
	private readonly ITgsRepository _tgsRepository;

	public BrandRelationQueryService(
		ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}


	public IAsyncEnumerable<BrandRelationInfo> QueryAsync(
		BrandRelationQuery query, 
		CancellationToken cancellationToken = default) 
		=> _tgsRepository.GetBrandRelationsAsync(cancellationToken);
}