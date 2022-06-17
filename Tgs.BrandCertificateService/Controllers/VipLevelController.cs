using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("[controller]")]
[ApiController]
public class VipLevelController : ControllerBase
{
	[HttpGet("{brandId}", Name = "ListVipLevelsByBrand")]
	public IAsyncEnumerable<VipLevelViewModel> ListVipLevelsAsync(
		[FromServices] IQueryService<GetVipLevelsByBrandQuery, VipLevel> queryService,
		Guid brandId)
	{
		var query = new GetVipLevelsByBrandQuery(brandId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(vip => new VipLevelViewModel
			{
				Id = vip.Id,
				Level = vip.Level
			});
	}
}
