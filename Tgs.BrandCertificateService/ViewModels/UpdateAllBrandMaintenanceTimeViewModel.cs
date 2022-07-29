using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels
{
	public class UpdateAllBrandMaintenanceTimeViewModel
	{
		[JsonPropertyName("begintime")]
		public string BeginTime { get; set; } = default!;

		[JsonPropertyName("endtime")]
		public string EndTime { get; set; } = default!;
	}
}