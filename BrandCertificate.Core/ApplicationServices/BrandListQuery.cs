namespace Tgs.BrandCertificate.ApplicationServices;

public record BrandListQuery(
	IEnumerable<Guid> BrandIds,
	string? BrandCode,
	string? BrandName,
	BrandStatus? Status);

