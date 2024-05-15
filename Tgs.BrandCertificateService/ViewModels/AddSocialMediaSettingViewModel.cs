using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class AddSocialMediaSettingViewModel
{
	public required string Name { get; set; }

	public required SocialType Type { get; set; }

	public required Uri Url { get; set; }
}
