namespace Tgs.BrandCertificate.Clients;

public record BrandPlayerContractDisplayInfo
{
	public string Name { get; set; } = default!;
	public bool IsVisualized { get; set; }
}