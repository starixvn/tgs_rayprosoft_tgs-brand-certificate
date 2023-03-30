namespace Tgs.BrandCertificate.Clients;

public class BrandListQuery
{
	public IEnumerable<Guid> BrandIds { get; init; } = Enumerable.Empty<Guid>();

	public string? Keyword { get; init; }

	public BrandStatus? Status { get; init; }
}
