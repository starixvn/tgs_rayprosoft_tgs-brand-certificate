using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels;

public record UpdateBrandMaintenanceContentViewModel
{
	[JsonPropertyName("content")]
	public string? Content { get; set; }
}
