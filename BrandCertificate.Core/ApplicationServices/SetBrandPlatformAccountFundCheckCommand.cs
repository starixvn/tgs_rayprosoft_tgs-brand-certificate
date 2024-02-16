namespace Tgs.BrandCertificate.ApplicationServices;

public record SetBrandPlatformAccountFundCheckCommand(
	Guid BrandId,
	bool Enableld,
	string Modifier);
