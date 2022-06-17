using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandFrontendInfoQueryService : IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandFrontendInfoQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<BrandFrontendInfo?> GetAsync(GetBrandFrontendInfoQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandFrontendInfoAsync(query.BrandId, cancellationToken);
}
