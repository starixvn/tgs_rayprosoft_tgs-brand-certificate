using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("bet-limit")]
[ApiController]
public class BetLimitController : ControllerBase
{
	[HttpGet("{brandId}", Name = "GetBrandBetLimitId")]
	public ValueTask<int?> GetBrandBetLimitIdAsync(
		[FromServices] IGetService<GetBrandBetLimitIdQuery, int?> getService,
		Guid brandId)
	{
		var query = new GetBrandBetLimitIdQuery(brandId);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}
}
