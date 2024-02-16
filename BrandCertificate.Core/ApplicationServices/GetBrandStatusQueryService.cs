using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandStatusQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandStatusQuery, BrandStatus>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public async ValueTask<BrandStatus> GetAsync(GetBrandStatusQuery query, CancellationToken cancellationToken = default)
	{
		var brandStatusInfo = await _tgsRepository
			.GetBrandStatusInfoAsync(query.BrandId, cancellationToken)
			.ConfigureAwait(false);

		if (brandStatusInfo is null)
			return BrandStatus.Inactive;

		var status = brandStatusInfo.ResellerStatus;

		if (brandStatusInfo.LicenseeStatus is not BrandStatus.Active)
			status = brandStatusInfo.LicenseeStatus;

		if (brandStatusInfo.BrandStatus is not BrandStatus.Active)
			status = brandStatusInfo.BrandStatus;

		return status;
	}
}
