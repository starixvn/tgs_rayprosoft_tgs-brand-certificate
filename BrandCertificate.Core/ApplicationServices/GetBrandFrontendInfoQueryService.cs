using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandFrontendInfoQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<BrandFrontendInfo> GetAsync(GetBrandFrontendInfoQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandFrontendInfoAsync(query.BrandId, cancellationToken);
}
