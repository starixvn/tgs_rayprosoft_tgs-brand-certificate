namespace Tgs.BrandCertificate.ApplicationServices;
public record SetSocialMediaSettingOrderCommand(
	Guid SettingId,
	int NewOrder,
	string Modifier);
