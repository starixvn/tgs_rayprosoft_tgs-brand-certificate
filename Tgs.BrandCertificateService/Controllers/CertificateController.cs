using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;
using Tgs.WebApi.Toolkits;

namespace Tgs.BrandCertificateService.Controllers;

[Route("cert")]
[ApiController]
public class CertificateController : ControllerBase
{
	[HttpGet("jwk")]
	[Produces(MediaTypeNames.Application.Json)]
	public async Task<JsonDocument> GetJsonWebKeyAsync(
		[FromServices] IGetService<GetJsonWebKeyQuery, string> getJwkService)
		=> JsonDocument.Parse(
			await getJwkService
				.GetAsync(new GetJsonWebKeyQuery(), HttpContext.RequestAborted)
				.ConfigureAwait(false));

	[HttpPost("{brandId}")]
	[Produces(MediaTypeNames.Text.Plain)]
	[FailStatusCode(StatusCodes.Status204NoContent, "Can't find brand.")]
	public async ValueTask<string?> MakeTokenAsync(
		[FromServices] IGetService<GetBrandInfoQuery, BrandInfo?> getBrandService,
		[FromServices] IGetService<MakeBrandTokenCommand, string> generateTokenService,
		Guid brandId)
	{
		var brandInfo = await getBrandService.GetAsync(new GetBrandInfoQuery(brandId), HttpContext.RequestAborted);

		return brandInfo == null
			? null
			: await generateTokenService
				.GetAsync(new MakeBrandTokenCommand(brandInfo.Id, brandInfo.Code), HttpContext.RequestAborted)
				.ConfigureAwait(false);
	}
}
