namespace Tgs.BrandCertificate;

public record BrandStatusInfo(
	BrandStatus ResellerStatus,
	BrandStatus LicenseeStatus,
	BrandStatus BrandStatus);