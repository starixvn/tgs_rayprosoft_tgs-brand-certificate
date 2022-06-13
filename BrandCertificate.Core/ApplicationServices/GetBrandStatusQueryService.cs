using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandStatusQueryService : IGetService<GetBrandStatusQuery, BrandStatus>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandStatusQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public async ValueTask<BrandStatus> GetAsync(GetBrandStatusQuery query, CancellationToken cancellationToken = default)
		=> await _tgsRepository
			.GetBrandStatusAsync(query.BrandId, cancellationToken)
			.ConfigureAwait(false) ?? BrandStatus.Inactive;
}
