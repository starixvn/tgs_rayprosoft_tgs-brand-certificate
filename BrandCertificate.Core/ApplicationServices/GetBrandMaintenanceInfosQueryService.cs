using Tgs.ServiceUtility;
using TGS.Paging;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class GetBrandMaintenanceInfosQueryService : IQueryService<GetBrandMaintenanceInfosQuery, BrandMaintenanceInfo>, IGetService<GetBrandMaintenanceInfosQuery, int>
	{
		private readonly ITgsRepository _tgsRepository;

		public GetBrandMaintenanceInfosQueryService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask<int> GetAsync(GetBrandMaintenanceInfosQuery query, CancellationToken cancellationToken = default)
		{
			var data = _tgsRepository.QueryBrandMaintenanceInfosAsync(query.BrandCode, cancellationToken);
			return data.CountAsync(cancellationToken: cancellationToken);
		}

		public IAsyncEnumerable<BrandMaintenanceInfo> QueryAsync(GetBrandMaintenanceInfosQuery query, CancellationToken cancellationToken = default)
		{
			var data = _tgsRepository.QueryBrandMaintenanceInfosAsync(query.BrandCode, cancellationToken);

			return data.ApplyOrderBy(query.Paging).ApplyPaging(query.Paging);
		}
	}
}