namespace Tgs.BrandCertificate.Clients;

public record BrandListItem(
	Guid BrandId,
	string BrandName,
	Guid LicenseeId,
	string LicenseeName,
	Guid ResellerId,
	string ResellerName,
	BrandStatus Status,
	string? Description,
	string? OperationalEmail,
	DateTime CreatedOn,
	DateTime UpdatedOn);