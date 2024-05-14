namespace Tgs.BrandCertificate;
public record SocialMediaSetting(
	Guid Id,
	Guid BrandId,
	string Name,
	SocialType Type,
	Uri Url,
	int Order);