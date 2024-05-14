namespace Tgs.BrandCertificate.ApplicationServices;
public record AddSocialMediaSettingCommand(
	Guid BrandId,
	string Name,
	SocialType Type,
	Uri Url,
	string Modifier);
