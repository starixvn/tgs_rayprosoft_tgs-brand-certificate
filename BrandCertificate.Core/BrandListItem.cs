namespace Tgs.BrandCertificate;

public record BrandListItem(
	Guid BrandId,
	string BrandCode,
	string BrandName,
	BrandStatus Status,
	string? Description,
	string? OperationalEmail,
	DateTime CreatedOn,
	DateTime UpdatedOn);