using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetVipLevelsByBrandQueryService(ITgsRepository tgsRepository) : IQueryService<GetVipLevelsByBrandQuery, VipLevel>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public IAsyncEnumerable<VipLevel> QueryAsync(GetVipLevelsByBrandQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.QueryVipLevelsAsync(query.BrandId, cancellationToken);
}
