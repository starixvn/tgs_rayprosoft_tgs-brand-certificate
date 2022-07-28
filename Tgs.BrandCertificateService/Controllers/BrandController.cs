using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
	[HttpGet("{brandId}", Name = "GetBrandInfo")]
	public async Task<BrandViewModel?> GetBrandInfoAsync(
		[FromServices] IGetService<GetBrandInfoQuery, BrandInfo?> getBrandService,
		Guid brandId)
	{
		var query = new GetBrandInfoQuery(brandId);

		var brandInfo = await getBrandService
			.GetAsync(query, HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return brandInfo is null
			? null
			: new BrandViewModel
			{
				Id = brandInfo.Id,
				Code = brandInfo.Code,
				Currency = brandInfo.Currency,
				PlayerIdentityEnabled = brandInfo.PlayerIdentityEnabled,
				SmsEnabled = brandInfo.SmsEnabled,
				SmsRegisterFlowEnabled = brandInfo.SmsRegisterFlowEnabled,
				BankInfoRegisterFlowEnabled = brandInfo.BankInfoRegisterFlowEnabled,
				EmailRegisterFlowEnabled = brandInfo.EmailRegisterFlowEnabled
			};
	}

	[HttpGet("front/{brandId}", Name = "GetBrandFrontendInfo")]
	public async Task<BrandFrontendViewModel> GetBrandFrontendInfoAsync(
		[FromServices] IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo> getBrandFrontendService,
		Guid brandId)
	{
		var query = new GetBrandFrontendInfoQuery(brandId);

		var info = await getBrandFrontendService
			.GetAsync(query, HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return info is null
			? new BrandFrontendViewModel()
			: new BrandFrontendViewModel
			{
				DesktopLogoUrl = info.DesktopLogoUrl,
				MobileLogoUrl = info.MobileLogoUrl,
				LoadingUrl = info.LoadingUrl,
				PlayerRegisterFlowSettings = info.PlayerRegisterFlowSettings
			};
	}

	[HttpGet("status/{brandId}", Name = "GetBrandStatus")]
	public ValueTask<BrandStatus> GetBrandStatusAsync(
		[FromServices] IGetService<GetBrandStatusQuery, BrandStatus> getService,
		Guid brandId)
	{
		var query = new GetBrandStatusQuery(brandId);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}

	[HttpGet("payment/{brandId}", Name = "GetBrandPaymentSettings")]
	public async Task<BrandPaymentSettingsViewModel?> GetBrandPaymentSettingsAsync(
		[FromServices] IGetService<GetBrandPaymentSettingsQuery, BrandPaymentSettings?> getService,
		Guid brandId)
	{
		var query = new GetBrandPaymentSettingsQuery(brandId);

		var result = await getService
			.GetAsync(query, HttpContext.RequestAborted).AsTask()
			.ConfigureAwait(false);

		return result is null
			? null
			: new BrandPaymentSettingsViewModel
			{
				PaymentDepositLockEnabled = result.PaymentDepositLockEnabled
			};
	}
}
