using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandListQueryViewModel
{
	[FromQuery]
	public Guid? ResellerId { get; set; }

	[FromQuery]
	public Guid? LicenseeId { get; set; }

	[FromQuery]
	public Guid? BrandId { get; set; }

	[FromQuery]
	public string? BrandCode { get; set; }

	[FromQuery(Name = "status")]
	public BrandStatus? BrandStatus { get; set; }
}
