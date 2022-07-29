using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels
{
	public record BrandMaintenanceViewModel
	{
		public int TotalCount { get; set; }
		public BrandMaintenanceInfo[] Items { get; set; } = default!;
	}
}