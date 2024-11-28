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
	[HttpGet("{brandId:Guid}", Name = "GetBrandInfo")]
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
				Status = brandInfo.Status,
				Currency = brandInfo.Currency,
				PlayerIdentityEnabled = brandInfo.PlayerIdentityEnabled,
				SmsEnabled = brandInfo.SmsEnabled,
				SmsRegisterFlowEnabled = brandInfo.SmsRegisterFlowEnabled,
				BankInfoRegisterFlowEnabled = brandInfo.BankInfoRegisterFlowEnabled,
				EmailRegisterFlowEnabled = brandInfo.EmailRegisterFlowEnabled,
				TimeSpanMinutes = brandInfo.TimeSpanMinutes
			};
	}

	[HttpGet("{brandCode}", Name = "GetBrandInfoByCode")]
	public async Task<BrandViewModel?> GetBrandInfoByCodeAsync(
		[FromServices] IGetService<GetBrandInfoByCodeQuery, BrandInfo?> getBrandService,
		string brandCode)
	{
		var query = new GetBrandInfoByCodeQuery(brandCode);

		var brandInfo = await getBrandService
			.GetAsync(query, HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return brandInfo is null
			? null
			: new BrandViewModel
			{
				Id = brandInfo.Id,
				Code = brandInfo.Code,
				Status = brandInfo.Status,
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

	[HttpGet("sms/{brandId}", Name = "GetBrandSmsSettings")]
	public async ValueTask<BrandSmsVendorSettingsViewModel?> GetBrandSmsSettingsAsync(
		[FromServices] IGetService<GetBrandSmsVendorSettingsQuery, SmsSettingsInfo?> getService,
		Guid brandId)
	{
		var query = new GetBrandSmsVendorSettingsQuery(brandId);

		var info = await getService.GetAsync(
			query,
			HttpContext.RequestAborted).ConfigureAwait(false);

		return info is null
			? null
			: new BrandSmsVendorSettingsViewModel
			{
				Code = info.Code,
				SmsSettings = info.VendorSettings
			};
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

	[HttpGet("platform-account-fund/{brandId:Guid}", Name = "GetBrandPlatformAccountFundCheckFlag")]
	public ValueTask<bool?> GetBrandPlatformAccountFundCheckFlagAsync(
		[FromServices] IGetService<GetBrandPlatformAccountFundCheckFlagQuery, bool?> getService,
		Guid brandId)
	{
		var query = new GetBrandPlatformAccountFundCheckFlagQuery(brandId);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}

	[HttpPatch("platform-account-fund/{brandId:Guid}", Name = "SetBrandPlatformAccountFundCheck")]
	public ValueTask SetBrandPlatformAccountFundCheckAsync(
		[FromServices] ICommandService<SetBrandPlatformAccountFundCheckCommand> setService,
		[FromHeader(Name = "Modifier")] string modifier,
		Guid brandId,
		[FromBody] bool isEnabled)
	{
		var command = new SetBrandPlatformAccountFundCheckCommand(brandId, isEnabled, modifier);

		return setService.ExecuteAsync(command, HttpContext.RequestAborted);
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
				LicenseeCode = info.LicenseeCode,
				LicenseeName = info.LicenseeName,
				ResellerId = info.ResellerId,
				ResellerCode = info.ResellerCode,
				ResellerName = info.ResellerName
			});

	[HttpGet("currencies", Name = "GetAllBrandCurrencies")]
	public IAsyncEnumerable<BrandCurrencyViewModel> GetAllBrandCurrenciesAsync(
		[FromServices] IQueryService<GetAllBrandCurrenciesQuery, BrandCurrencyInfo> queryService)
		=> queryService.QueryAsync(new(), HttpContext.RequestAborted)
			.Select(info => new BrandCurrencyViewModel
			{
				BrandId = info.BrandId,
				CurrencyCode = info.Currency
			});

	[HttpGet("agent-dividends", Name = "GetBrandAgentDividendInfos")]
	public IAsyncEnumerable<BrandAgentDividendInfoViewModel> GetBrandAgentDividendInfosAsync(
		[FromServices] IQueryService<GetBrandAgentDividendInfoQuery, BrandAgentDividendInfo> getService)
	{
		var query = new GetBrandAgentDividendInfoQuery();

		return getService
			.QueryAsync(query, HttpContext.RequestAborted)
			.Select(x => new BrandAgentDividendInfoViewModel
			{
				Id = x.Id,
				Code = x.Code,
				Status = x.Status,
				AgentDividendType = x.AgentDividendType,
				SettlementIntervalType = x.SettlementIntervalType,
				AutoSentAgentBonus = x.AutoSentAgentBonus,
				CheckDividendActiveCount = x.CheckDividendActiveCount,
				PlatformCharge = x.PlatformCharge,
				PreviousSettlementFrom = x.PreviousSettlementFrom,
				PreviousSettlementTo = x.PreviousSettlementTo,
				CurrentSettlementFrom = x.CurrentSettlementFrom,
				CurrentSettlementTo = x.CurrentSettlementTo,
				NextSettlementFrom = x.NextSettlementFrom,
				NextSettlementTo = x.NextSettlementTo,
				SettlementDay = x.SettlementDay
			});
	}

	[HttpPatch("agent-dividend-time-interval", Name = "UpdateBrandAgentDividendTimeInterval")]
	public ValueTask UpdateBrandAgentDividendTimeIntervalAsync(
		[FromServices] ICommandService<UpdateBrandAgentDividendTimeIntervalCommand> commandService,
		[FromBody] UpdateBrandAgentDividendTimeIntervalRequest request)
	{
		return commandService.ExecuteAsync(new UpdateBrandAgentDividendTimeIntervalCommand(request.BrandId,
			request.PreviousSettlementFrom,
			request.PreviousSettlementTo,
			request.CurrentSettlementFrom,
			request.CurrentSettlementTo,
			request.NextSettlementFrom,
			request.NextSettlementTo));
	}

	[HttpPut(Name = "UpdateBrand")]
	public ValueTask<Guid> UpdateBrandAsync(
	[FromServices] IGetService<UpdateBrandQuery, Guid> getService,
	[FromBody] UpdateBrandRequest request)
	{
		var query = new UpdateBrandQuery(
			request.Id,
			request.Name,
			request.Description,
			request.OperationalEmail,
			request.PlatformCharge,
			request.EnablePlayerIdentity,
			request.SettlementIntervalType,
			request.SettlementDay,
			request.CheckDividendActiveCount,
			request.AutoClearWageringRequirement,
			request.AutoSendAgentBonus,
			request.EnablePlayerAgent,
			request.AutoClearWageringRequirementBalance,
			request.DepositHandlingFeeRateTenPercentile,
			request.WithdrawHandlingFeeRateTenPercentile,
			request.EditorUsername,
			request.AreaCodes);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}

	[HttpPost(Name = "CreateBrand")]
	public ValueTask<Guid> CreateBrandAsync(
		[FromServices] IGetService<CreateBrandQuery, Guid> getService,
		[FromBody] CreateBrandRequest request)
	{
		var query = new CreateBrandQuery(request.Code,
			request.Name,
			request.Description,
			request.OperationalEmail,
			request.PlatformCharge,
			request.TimeSpanMinutes,
			request.EnablePlayerIdentity,
			request.LicenseeId,
			request.SettlementIntervalType,
			request.SettlementDay,
			request.AgentDividendType,
			request.CheckDividendActiveCount,
			request.AutoClearWageringRequirement,
			request.AutoSendAgentBonus,
			request.EnablePlayerAgent,
			request.AutoClearWageringRequirementBalance,
			request.DepositHandlingFeeRateTenPercentile,
			request.WithdrawHandlingFeeRateTenPercentile,
			request.EditorUsername,
			request.AreaCodes);

		return getService.GetAsync(query, HttpContext.RequestAborted);
	}

	[HttpGet("full-info/{brandId:Guid}", Name = "GetBrandFullInfo")]
	public async Task<BrandFullInfoViewModel?> GetBrandFullInfoAsync(
		[FromServices] IGetService<GetBrandFullInfoQuery, BrandFullInfo?> getBrandService,
		Guid brandId)
	{
		var query = new GetBrandFullInfoQuery(brandId);

		var brandInfo = await getBrandService
			.GetAsync(query, HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return brandInfo is null
			? null
			: new BrandFullInfoViewModel
			{
				Id = brandInfo.Id,
				LicenseeId = brandInfo.LicenseeId,
				LicenseeName = brandInfo.LicenseeName,
				ResellerId = brandInfo.ResellerId,
				ResellerName = brandInfo.ResellerName,
				Code = brandInfo.Code,
				Name = brandInfo.Name,
				Description = brandInfo.Description,
				OperationalEmail = brandInfo.OperationalEmail,
				Status = brandInfo.Status,
				AgentDividendType = brandInfo.AgentDividendType,
				SettlementIntervalType = brandInfo.SettlementIntervalType,
				Currency = brandInfo.Currency,
				ClientId = brandInfo.ClientId,
				SecretKey = brandInfo.SecretKey,
				ApiKey = brandInfo.ApiKey,
				SmsEnabled = brandInfo.SmsEnabled,
				PlayerIdentityEnabled = brandInfo.PlayerIdentityEnabled,
				SmsRegisterFlowEnabled = brandInfo.SmsRegisterFlowEnabled,
				BankInfoRegisterFlowEnabled = brandInfo.BankInfoRegisterFlowEnabled,
				EmailRegisterFlowEnabled = brandInfo.EmailRegisterFlowEnabled,
				CheckDividendActiveCount = brandInfo.CheckDividendActiveCount,
				EnablePlayerIdentity = brandInfo.EnablePlayerIdentity,
				AutoClearWageringRequirement = brandInfo.AutoClearWageringRequirement,
				AutoSendAgentBonus = brandInfo.AutoSendAgentBonus,
				EnablePlayerAgent = brandInfo.EnablePlayerAgent,
				AutoClearWageringRequirementBalance = brandInfo.AutoClearWageringRequirementBalance,
				DepositHandlingFeeRateTenPercentile = brandInfo.DepositHandlingFeeRateTenPercentile,
				WithdrawHandlingFeeRateTenPercentile = brandInfo.WithdrawHandlingFeeRateTenPercentile,
				PlatformCharge = brandInfo.PlatformCharge,
				CreatedOn = brandInfo.CreatedOn,
				UpdatedOn = brandInfo.UpdatedOn,
				SettlementDay = brandInfo.SettlementDay,
				TimeSpanMinutes = brandInfo.TimeSpanMinutes
			};
	}

	[HttpGet("product-fee/{brandId:Guid}", Name = "GetBrandProductFeeInfo")]
	public async Task<BrandProductFeeInfoViewModel?> GetBrandProductFeeInfoAsync(
	[FromServices] IGetService<GetBrandProductFeeInfoQuery, BrandProductFeeInfo?> getBrandService,
	Guid brandId)
	{
		var query = new GetBrandProductFeeInfoQuery(brandId);

		var brandInfo = await getBrandService
			.GetAsync(query, HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return brandInfo is null
			? null
			: new BrandProductFeeInfoViewModel
			{
				BrandId = brandId,
				CardGamePercentage = brandInfo.CardGamePercentage,
				LiveDealerPercentage = brandInfo.LiveDealerPercentage,
				LotteryPercentage = brandInfo.LotteryPercentage,
				SlotPercentage = brandInfo.SlotPercentage,
				SportsPercentage = brandInfo.SportsPercentage,
				VirtualSportsPercentage = brandInfo.VirtualSportsPercentage,
			};
	}

	[HttpPut("product-fee", Name = "UpdateBrandProductFee")]
	public ValueTask UpdateBrandProductFeeAsync(
		[FromServices] ICommandService<UpdateBrandProductFeeCommand> getService,
		[FromBody] CreateUpdateBrandProductFeeRequest request)
	{
		var query = new UpdateBrandProductFeeCommand(
			request.BrandId,
			request.CardGamePercentage,
			request.LiveDealerPercentage,
			request.LotteryPercentage,
			request.SlotPercentage,
			request.SportsPercentage,
			request.VirtualSportsPercentage,
			request.EditorUsername);

		return getService.ExecuteAsync(query, HttpContext.RequestAborted);
	}

	[HttpPatch("enable-sms/{brandId:Guid}", Name = "SetBrandEnableSms")]
	public ValueTask SetBrandEnableSmsAsync(
		[FromServices] ICommandService<SetBrandEnableSmsCommand> commandService,
		[FromHeader(Name = "Modifier")] string modifier,
		Guid brandId,
		[FromBody] bool enableSms)
	{
		var command = new SetBrandEnableSmsCommand(
			brandId,
			enableSms,
			modifier);

		return commandService.ExecuteAsync(command, HttpContext.RequestAborted);
	}	
}