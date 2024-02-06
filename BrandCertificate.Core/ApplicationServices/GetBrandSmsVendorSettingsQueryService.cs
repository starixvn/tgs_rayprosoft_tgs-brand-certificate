using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandSmsVendorSettingsQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandSmsVendorSettingsQuery, SmsSettingsInfo?>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask<SmsSettingsInfo?> GetAsync(GetBrandSmsVendorSettingsQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandSmsSettingsAsync(query.BrandId, cancellationToken);
}
