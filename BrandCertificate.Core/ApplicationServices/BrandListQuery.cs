namespace Tgs.BrandCertificate.ApplicationServices;

public record BrandListQuery(
	IEnumerable<Guid> BrandIds,
	string? Keyword,
	BrandStatus? Status);

