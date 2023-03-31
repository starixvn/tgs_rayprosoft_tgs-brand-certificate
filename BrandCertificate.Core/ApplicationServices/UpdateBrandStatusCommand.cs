namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandStatusCommand(
	Guid BrandId,
	BrandStatus Status,
	string Modifier);