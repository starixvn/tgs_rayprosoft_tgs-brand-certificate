using Microsoft.AspNetCore.Mvc;

namespace Tgs.BrandCertificateService.ViewModels
{
	public record GetBrandMaintenanceInfosViewModel
	{
		[FromQuery(Name = "brandcode")]
		public string? BrandCode { get; set; }
	}
}