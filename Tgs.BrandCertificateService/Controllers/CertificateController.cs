using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using TGS.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CertificateController : ControllerBase
	{
		[HttpGet("jwk")]
		public async Task<JsonDocument> GetJsonWebKeyAsync(
			[FromServices] IGetService<GetJsonWebKeyQuery, string> getJwkService)
			=> JsonDocument.Parse(
				await getJwkService
						.GetAsync(new GetJsonWebKeyQuery())
						.ConfigureAwait(false));

		[HttpPost]
		public async ValueTask<string?> MakeTokenAsync(
			[FromServices] IGetService<GetBrandInfoQuery, BrandInfo?> getBrandService,
			[FromServices] IGetService<MakeBrandTokenCommand, string> generateTokenService,
			Guid brandId)
		{
			var brandInfo = await getBrandService.GetAsync(new GetBrandInfoQuery(brandId));

			if (brandInfo == null)
				return null;

			return await generateTokenService
				.GetAsync(new MakeBrandTokenCommand(brandInfo.Id, brandInfo.Code))
				.ConfigureAwait(false);
		}
	}
}
