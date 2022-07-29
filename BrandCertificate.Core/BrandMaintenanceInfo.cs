using System.Text.Json.Serialization;

namespace Tgs.BrandCertificate
{
	public record BrandMaintenanceInfo
	{
		public string BrandName { get; set; } = default!;
		public string BrandCode { get; set; } = default!;
		public string LicenseeName { get; set; } = default!;
		public Guid BrandId { get; set; }

		[JsonConverter(typeof(TimeOnlyConverter))]
		public TimeOnly? BeginTime { get; set; }

		[JsonConverter(typeof(TimeOnlyConverter))]
		public TimeOnly? EndTime { get; set; }

		[JsonPropertyName("Id")]
		public Guid? BrandMaintenanceId { get; set; }

		public BrandStatus BrandStatus { get; set; }
	}
}