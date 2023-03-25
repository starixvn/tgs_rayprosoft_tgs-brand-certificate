namespace Tgs.BrandCertificate.Clients;

public class BrandListQuery
{
	public Guid? ResellerId { get; init; }

	public Guid? LicenseeId { get; init; }

	public Guid? BrandId { get; init; }

	public string? BrandCode { get; init; }

	public BrandStatus? Status { get; init; }
}