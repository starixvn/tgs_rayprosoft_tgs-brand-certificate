using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class GetBrandMaintenanceInfoQueryService : IGetService<GetBrandMaintenanceInfoQuery, BrandMaintenanceModel>
	{
		private readonly ITgsRepository _tgsRepository;

		public GetBrandMaintenanceInfoQueryService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask<BrandMaintenanceModel> GetAsync(GetBrandMaintenanceInfoQuery query, CancellationToken cancellationToken = default)
			=> _tgsRepository.GetBrandMaintenanceInfoByIdAsync(query.BrandMaintenanceId, cancellationToken);
	}
}