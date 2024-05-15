using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class SocialMediaSettingViewModel
{
	public required Guid Id { get; set; }

	public required string Name { get; set; }

	public required SocialType Type { get; set; }

	public required Uri Url { get; set; }

	public required int Order { get; set; }
}
