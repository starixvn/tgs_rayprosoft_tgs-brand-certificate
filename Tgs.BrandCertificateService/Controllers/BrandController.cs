using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService.ViewModels;
using Tgs.ServiceUtility;
using TGS.Paging;

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

	/// <summary>
	/// Gets the brand maintenance infos asynchronous.
	/// </summary>
	/// <param name="queryService">The query service.</param>
	/// <param name="getService">The get service.</param>
	/// <param name="request">The request.</param>
	/// <param name="paging">The paging.</param>
	/// <returns></returns>
	[HttpGet("maintenance")]
	public async ValueTask<BrandMaintenanceViewModel> GetBrandMaintenanceInfosAsync(
		[FromServices] IQueryService<GetBrandMaintenanceInfosQuery, BrandMaintenanceInfo> queryService,
		[FromServices] IGetService<GetBrandMaintenanceInfosQuery, int> getService,
		[FromQuery] GetBrandMaintenanceInfosViewModel request,
		[FromQuery] PagingViewModel paging)
	{
		var sourcePaging = new Paging<BrandMaintenanceInfo>
		{
			Page = paging.Page,
			RowCount = paging.Rows
		};

		if (paging.ShowAll == true)
		{
			sourcePaging.Page = -1;
			sourcePaging.RowCount = -1;
		}

		if (!string.IsNullOrWhiteSpace(paging.SortBy))
			sourcePaging.SortByPropertyName(paging.SortBy, paging.IsDesc);

		var query = new GetBrandMaintenanceInfosQuery(request.BrandCode, sourcePaging);

		var items = await queryService.QueryAsync(query, HttpContext.RequestAborted).ToArrayAsync().ConfigureAwait(false);

		var totalCount = await getService.GetAsync(query, HttpContext.RequestAborted).ConfigureAwait(false);

		return new BrandMaintenanceViewModel
		{
			Items = items,
			TotalCount = totalCount
		};
	}

	/// <summary>
	/// Batche update brand maintenance time asynchronous.
	/// </summary>
	/// <param name="commandService">The command service.</param>
	/// <param name="request">The request.</param>
	/// <returns></returns>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///				"brandids": [
	///					"4e86719f-243f-46f6-b010-5c758b7285af"
	///				],
	///				"begintime": "09:00:00",
	///				"endtime": "10:30:00"
	///			}
	/// </remarks>
	[HttpPut("maintenance/updatealltime")]
	public ValueTask UpdateAllBrandMaintenanceTimeAsync(
		[FromServices] ICommandService<UpdateAllBrandMaintenanceTimeCommand> commandService,
		[FromBody] UpdateAllBrandMaintenanceTimeViewModel request)
		=> commandService.ExecuteAsync(new UpdateAllBrandMaintenanceTimeCommand(TimeOnly.Parse(request.BeginTime), TimeOnly.Parse(request.EndTime)));

	[HttpGet("maintenance/{id}", Name = "GetBrandMaintenanceInfoById")]
	public async ValueTask<BrandMaintenanceResult> GetBrandMaintenanceInfoByIdAsync(
		[FromServices] IGetService<GetBrandMaintenanceInfoQuery, BrandMaintenanceModel> getService,
		Guid id)
	{
		var query = new GetBrandMaintenanceInfoQuery(id);

		var info = await getService.GetAsync(query).ConfigureAwait(false);

		return new BrandMaintenanceResult(info.Content);
	}

	[HttpPut("maintenance/content/{id}")]
	public ValueTask UpdateBrandMaintenanceContentAsync(
		[FromServices] ICommandService<UpdateBrandMaintenanceContentCommand> commandService,
		[FromBody] UpdateBrandMaintenanceContentViewModel request,
		Guid id)
		=> commandService.ExecuteAsync(new UpdateBrandMaintenanceContentCommand(
			BrandMaintenanceId: id,
			Content: request.Content));

	[HttpPut("maintenance/time/{brandId}")]
	public ValueTask UpdateBrandMaintenanceTimeAsync(
		[FromServices] ICommandService<UpdateBrandMaintenanceTimeCommand> commandService,
		[FromBody] UpdateBrandMaintenanceTimeViewModel request,
		Guid brandId)
		=> commandService.ExecuteAsync(new UpdateBrandMaintenanceTimeCommand(
			brandId,
			TimeOnly.Parse(request.BeginTime),
			TimeOnly.Parse(request.EndTime)));

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