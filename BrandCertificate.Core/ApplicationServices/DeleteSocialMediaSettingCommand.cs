namespace Tgs.BrandCertificate.ApplicationServices;
public record DeleteSocialMediaSettingCommand(
	Guid BrandId,
	Guid SettingId);
