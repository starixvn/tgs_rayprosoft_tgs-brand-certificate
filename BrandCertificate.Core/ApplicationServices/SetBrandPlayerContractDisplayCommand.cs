namespace Tgs.BrandCertificate.ApplicationServices;

public record SetBrandPlayerContractDisplayCommand(
	Guid BrandId,
	string Name,
	bool IsVisualized,
	string Modifier);