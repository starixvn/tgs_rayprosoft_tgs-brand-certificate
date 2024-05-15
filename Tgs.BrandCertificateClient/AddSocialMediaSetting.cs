namespace Tgs.BrandCertificate.Clients;
public record AddSocialMediaSetting(
	string Name,
	SocialType Type,
	Uri Url);
