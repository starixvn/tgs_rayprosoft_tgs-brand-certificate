using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandListQueryViewModel
{
	[FromQuery(Name = "brandId")]
	public Guid[] BrandIds { get; set; } = Array.Empty<Guid>();

	[FromQuery]
	public string? Keyword { get; set; }

	[FromQuery(Name = "status")]
	public BrandStatus? BrandStatus { get; set; }
}
