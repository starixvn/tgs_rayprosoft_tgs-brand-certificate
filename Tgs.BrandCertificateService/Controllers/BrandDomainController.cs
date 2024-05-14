using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandDomainController : ControllerBase
{
	[HttpGet("list/{brandId:guid}", Name = "GetBrandDomains")]
	public IAsyncEnumerable<string> GetBrandDomainsAsync(
		[FromServices] IQueryService<GetBrandDomainsQuery, string> getBrandService,
		Guid brandId)
	{
		var query = new GetBrandDomainsQuery(brandId);

		return getBrandService
			.QueryAsync(query, HttpContext.RequestAborted);
	}
}
