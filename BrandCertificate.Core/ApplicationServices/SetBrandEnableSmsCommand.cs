namespace Tgs.BrandCertificate.ApplicationServices;

public record SetBrandEnableSmsCommand(
	Guid BrandId,
	bool EnableSms,
	string Modifier);