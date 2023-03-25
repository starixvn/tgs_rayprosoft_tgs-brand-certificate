namespace Tgs.BrandCertificate.ApplicationServices;

public record BrandListQuery(
	Guid? ResellerId,
	Guid? LicenseeId,
	Guid? BrandId,
	string? BrandCode,
	BrandStatus? Status);

