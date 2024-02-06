using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandPaymentSettingsQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandPaymentSettingsQuery, BrandPaymentSettings?>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<BrandPaymentSettings?> GetAsync(GetBrandPaymentSettingsQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandPaymentSettingsAsync(query.BrandId, cancellationToken);
}
