namespace Tgs.BrandCertificate.ApplicationServices;

public record BrandListQuery(
	string? BrandCode,
	BrandStatus? Status);

