using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;

namespace Tgs.BrandCertificateService.ViewModels
{
	public class UpdateAllBrandMaintenanceTimeViewModel
	{
		[JsonPropertyName("begintime")]
		public TimeOnly BeginTime { get; set; }

		[JsonPropertyName("endtime")]
		public TimeOnly EndTime { get; set; }
	}
}