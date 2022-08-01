using System.Text.Json.Serialization;
using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels
{
	public class BrandMaintenanceInfoViewModel
	{
		public string BrandName { get; set; } = default!;
		public string BrandCode { get; set; } = default!;
		public string LicenseeName { get; set; } = default!;
		public Guid BrandId { get; set; }

		public TimeOnly? BeginTime { get; set; }

		public TimeOnly? EndTime { get; set; }

		[JsonPropertyName("Id")]
		public Guid? BrandMaintenanceId { get; set; }

		public BrandStatus BrandStatus { get; set; }
	}
}