using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.BrandCertificateService.ViewModels
{
	public record UpdateBrandPlayerRegisterFlowSettingViewModel
	{
		[JsonPropertyName("registersettings")]
		public JsonDocument PlayerRegisterFlowSettings { get; set; } = default!;
	}
}