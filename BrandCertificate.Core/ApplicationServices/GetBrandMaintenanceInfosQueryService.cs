using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandMaintenanceInfosQueryService : IQueryService<GetBrandMaintenanceInfosQuery, BrandMaintenanceInfo>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandMaintenanceInfosQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<BrandMaintenanceInfo> QueryAsync(GetBrandMaintenanceInfosQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.QueryBrandMaintenanceInfosAsync(query.BrandCode, cancellationToken);
}
