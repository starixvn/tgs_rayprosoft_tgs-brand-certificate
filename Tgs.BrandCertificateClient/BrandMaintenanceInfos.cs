namespace Tgs.BrandCertificate.Clients
{
	public record BrandMaintenanceInfos
	{
		public BrandMaintenanceInfo[] Items { get; set; } = default!;
		public int TotalCount { get; set; }
	}
}