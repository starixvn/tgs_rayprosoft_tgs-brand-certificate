namespace Tgs.BrandCertificate.Clients;
public record SocialMediaSetting(
	Guid Id,
	string Name,
	SocialType Type,
	Uri Url);
