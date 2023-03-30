using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificate.Repositories.Entities;
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
				EmailRegisterFlowEnabled = brandInfo.EmailRegisterFlowEnabled,
				TimeSpanMinutes = brandInfo.TimeSpanMinutes
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

	[HttpPut("status/{brandId}/{status}", Name = "UpdateBrandStatus")]
	public ValueTask UpdateBrandStatusAsync(
		[FromServices] ICommandService<UpdateBrandStatusCommand> commandService,
		Guid brandId,
		BrandStatus status,
		[FromHeader(Name = "Modifier")] string modifier)
	{
		var command = new UpdateBrandStatusCommand(
			brandId,
			status,
			modifier);

		return commandService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	/// <summary>
	/// Gets the brand maintenance infos asynchronous.
	/// </summary>
	/// <param name="queryService">The query service.</param>
	/// <param name="request">The request.</param>
	/// <returns></returns>
	[HttpGet("maintenance")]
	public IAsyncEnumerable<BrandMaintenanceInfoViewModel> GetBrandMaintenanceInfosAsync(
		[FromServices] IQueryService<GetBrandMaintenanceInfosQuery, BrandMaintenanceInfo> queryService,
		[FromQuery] GetBrandMaintenanceInfosViewModel request)
	{
		var query = new GetBrandMaintenanceInfosQuery(request.BrandCode);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(info => new BrandMaintenanceInfoViewModel
			{
				BrandMaintenanceId = info.BrandMaintenanceId,
				BrandId = info.BrandId,
				BrandCode = info.BrandCode,
				BeginTime = info.BeginTime,
				EndTime = info.EndTime,
				BrandStatus = info.BrandStatus,
				BrandName = info.BrandName,
				LicenseeName = info.LicenseeName
			});
	}

	/// <summary>
	/// Batch update brand maintenance time asynchronous.
	/// </summary>
	/// <param name="commandService">The command service.</param>
	/// <param name="request">The request.</param>
	/// <returns></returns>
	/// <remarks>
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
		=> commandService.ExecuteAsync(new UpdateAllBrandMaintenanceTimeCommand(request.BeginTime, request.EndTime));

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
			request.BeginTime,
			request.EndTime));

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
				PaymentDepositLockEnabled = result.PaymentDepositLockEnabled,
				AutoClearWageringRequirement = result.AutoClearWageringRequirement,
				ClearWageringRequirementBalance = result.ClearWageringRequirementBalance
			};
	}

	[HttpPut("register/{brandId}", Name = "UpdateBrandPlayerRegisterFlowSetting")]
	public ValueTask UpdateBrandPlayerRegisterFlowSettingAsync(
		[FromServices] ICommandService<UpdateBrandPlayerRegisterFlowSettingCommand> commandService,
		[FromBody] UpdateBrandPlayerRegisterFlowSettingViewModel request,
		Guid brandId)
		=> commandService.ExecuteAsync(new UpdateBrandPlayerRegisterFlowSettingCommand(
			brandId,
			request.PlayerRegisterFlowSettings));

	[HttpGet("id/{timeSpanMinutes}")]
	public IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(
		[FromServices] IQueryService<GetBrandIdsByTimeSpanMinutesQuery, Guid> queryService,
		int timeSpanMinutes)
	{
		var query = new GetBrandIdsByTimeSpanMinutesQuery(timeSpanMinutes);

		return queryService.QueryAsync(query, HttpContext.RequestAborted);
	}

	[HttpGet(Name = "QueryBrandList")]
	public IAsyncEnumerable<BrandListItemViewModel> QueryBrandListAsync(
		[FromServices] IQueryService<BrandListQuery, BrandListItem> queryService,
		[FromQuery] BrandListQueryViewModel viewModel)
	{
		var query = new BrandListQuery(
			viewModel.BrandIds,
			viewModel.Keyword,
			viewModel.BrandStatus);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(item => new BrandListItemViewModel
			{ 
				BrandId = item.BrandId,
				Status = item.Status,
				Description = item.Description,
				OperationalEmail = item.OperationalEmail,
				CreatedOn = item.CreatedOn,
				UpdatedOn = item.UpdatedOn
			});
	}

	[HttpGet("relations", Name = "GetBrandRelations")]
	public IAsyncEnumerable<BrandRelationViewModel> GetBrandRelationsAsync(
		[FromServices] IQueryService<BrandRelationQuery, BrandRelationInfo> queryService)
		=> queryService.QueryAsync(new BrandRelationQuery(), HttpContext.RequestAborted)
			.Select(info => new BrandRelationViewModel
			{
				BrandId = info.BrandId,
				BrandCode = info.BrandCode,
				BrandName = info.BrandName,
				LicenseeId = info.LicenseeId,
				LicenseeCode = info .LicenseeCode,
				LicenseeName = info.LicenseeName,
				ResellerId = info.ResellerId,
				ResellerCode = info.ResellerCode,
				ResellerName = info.ResellerName
			});
}