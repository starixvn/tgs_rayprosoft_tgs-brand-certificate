using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandMaintenanceInfoQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandMaintenanceInfoQuery, BrandMaintenanceModel>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<BrandMaintenanceModel> GetAsync(GetBrandMaintenanceInfoQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandMaintenanceInfoByIdAsync(query.BrandMaintenanceId, cancellationToken);
}
