using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels
{
	public record UpdateBrandMaintenanceViewModel
	{
		[JsonPropertyName("content")]
		public string Content { get; set; } = default!;
	}
}