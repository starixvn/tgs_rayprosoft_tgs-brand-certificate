using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetVipLevelsByBrandQueryService : IQueryService<GetVipLevelsByBrandQuery, VipLevel>
{
	private readonly ITgsRepository _tgsRepository;

	public GetVipLevelsByBrandQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public IAsyncEnumerable<VipLevel> QueryAsync(GetVipLevelsByBrandQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.QueryVipLevelsAsync(query.BrandId, cancellationToken);
}
