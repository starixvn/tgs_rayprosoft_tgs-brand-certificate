using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels;

public class UpdateAllBrandMaintenanceTimeViewModel
{
	[JsonPropertyName("begintime")]
	public TimeOnly BeginTime { get; set; }

	[JsonPropertyName("endtime")]
	public TimeOnly EndTime { get; set; }
}
