namespace Tgs.BrandCertificate.Clients;

public record BrandListItem(
	Guid BrandId,
	BrandStatus Status,
	string? Description,
	string? OperationalEmail,
	DateTime CreatedOn,
	DateTime UpdatedOn);