using System.Text.Json;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandFrontendViewModel
{
	public Uri? DesktopLogoUrl { get; set; }

	public Uri? MobileLogoUrl { get; set; }

	public Uri? LoadingUrl { get; set; }

	public JsonDocument? PlayerRegisterFlowSettings { get; set; }
}
