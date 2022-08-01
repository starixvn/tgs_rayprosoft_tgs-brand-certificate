namespace Tgs.BrandCertificate.Clients
{
	public record BrandMaintenanceInfo
	{
		public string BrandName { get; set; } = default!;
		public string BrandCode { get; set; } = default!;
		public string LicenseeName { get; set; } = default!;
		public Guid BrandId { get; set; }
		public string? BeginTime { get; set; }
		public string? EndTime { get; set; }
		public Guid? BrandMaintenanceId { get; set; }
		public BrandStatus BrandStatus { get; set; }
	}
}