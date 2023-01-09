using System.Text.Json;

namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandPlayerRegisterFlowSettingCommand(
	Guid BrandId,
	JsonDocument PlayerRegisterFlowSettings);
