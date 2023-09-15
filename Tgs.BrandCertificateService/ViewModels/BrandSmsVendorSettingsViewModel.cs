using System.Text.Json;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandSmsVendorSettingsViewModel
{
	public required string Code { get; set; }
	public JsonDocument? SmsSettings { get; set; }
}
