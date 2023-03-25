namespace Tgs.BrandCertificate;

public record BrandListItem(
	Guid BrandId,
	string BrandName,
	Guid LicenseeId,
	string LicenseeName,
	BrandStatus Status,
	string? Description,
	string? OperationalEmail,
	DateTime CreatedOn,
	DateTime UpdatedOn);