using System.Text.Json;

namespace Tgs.BrandCertificate.Clients;
public record BrandFrontendInfo(
	Uri? DesktopLogoUrl,
	Uri? MobileLogoUrl,
	Uri? LoadingUrl,
	JsonDocument? PlayerRegisterFlowSettings);
