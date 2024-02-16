using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandFullInfoQueryService : IGetService<GetBrandFullInfoQuery, BrandFullInfo?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandFullInfoQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<BrandFullInfo?> GetAsync(GetBrandFullInfoQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandFullInfoAsync(query.BrandId, cancellationToken);
}
