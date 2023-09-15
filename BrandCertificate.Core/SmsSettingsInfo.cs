using System.Text.Json;

namespace Tgs.BrandCertificate;
public record SmsSettingsInfo(string Code, JsonDocument? VendorSettings);
