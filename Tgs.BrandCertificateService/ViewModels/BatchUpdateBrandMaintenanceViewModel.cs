using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels
{
	public class BatchUpdateBrandMaintenanceViewModel
	{
		[JsonPropertyName("brandids")]
		public Guid[] BrandIds { get; set; } = default!;

		[JsonPropertyName("begintime")]
		public string BeginTime { get; set; } = default!;

		[JsonPropertyName("endtime")]
		public string EndTime { get; set; } = default!;
	}
}