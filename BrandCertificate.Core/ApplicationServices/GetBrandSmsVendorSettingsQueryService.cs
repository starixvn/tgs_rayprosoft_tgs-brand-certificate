using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandSmsVendorSettingsQueryService : IGetService<GetBrandSmsVendorSettingsQuery, SmsSettingsInfo?>
{
	private readonly ITgsRepository _tgsRepository;

	public GetBrandSmsVendorSettingsQueryService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask<SmsSettingsInfo?> GetAsync(GetBrandSmsVendorSettingsQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandSmsSettingsAsync(query.BrandId, cancellationToken);
}
