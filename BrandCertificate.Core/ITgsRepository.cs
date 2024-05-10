using System.Text.Json;

namespace Tgs.BrandCertificate;

public interface ITgsRepository
{
	IAsyncEnumerable<BrandCurrencyInfo> GetAllBrandCurrenciesAsync(CancellationToken cancellationToken = default);

	ValueTask<int?> GetBrandBetLimitIdAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes, CancellationToken cancellationToken = default);

	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandInfo?> GetBrandInfoByCodeAsync(string brandCode, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceModel> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandRelationInfo> GetBrandRelationsAsync(CancellationToken cancellationToken = default);

	ValueTask<SmsSettingsInfo?> GetBrandSmsSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandStatusInfo?> GetBrandStatusInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandAgentDividendInfo> GetBrandAgentDividendInfosAsync(CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandListItem> QueryAllBrandsAsync(CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandMaintenanceInfo> QueryBrandMaintenanceInfosAsync(string? brandCode, CancellationToken cancellationToken = default);

	ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? content, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<bool> UpdateBrandPlayerRegisterFlowSettingAsync(Guid brandId, JsonDocument playerRegisterFlowSettings, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandStatusAsync(
		Guid brandId,
		BrandStatus status,
		string modifier,
		CancellationToken cancellationToken = default);

	ValueTask<bool?> GetBrandPlatformAccountFundCheckFlagAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask SetBrandPlatformAccountFundCheckFlagAsync(Guid brandId, bool enabled, string modifier, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandAgentDividendTimeIntervalAsync(
		Guid brandId,
		DateTime previousSettlementFrom,
		DateTime previousSettlementTo,
		DateTime currentSettlementFrom,
		DateTime currentSettlementTo,
		DateTime nextSettlementFrom,
		DateTime nextSettlementTo,
		CancellationToken cancellationToken = default);

	ValueTask CreateBrandAsync(CreateBrandInfo createBrand, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandAsync(UpdateBrandInfo updateBrand, CancellationToken cancellationToken = default);

	ValueTask<BrandFullInfo?> GetBrandFullInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<string> GetBrandDomainsAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandProductFeeInfo?> GetBrandProductFeeAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask CreateBrandProductFeeAsync(
		Guid brandId,
		decimal cardGamePercentage,
		decimal liveDealerPercentage,
		decimal lotteryPercentage,
		decimal slotPercentage,
		decimal sportsPercentage,
		decimal virtualSportsPercentage,
		string modifier,
		CancellationToken cancellationToken = default);

	ValueTask UpdateBrandProductFeeAsync(
		Guid brandId,
		decimal cardGameRebatePercentage,
		decimal liveDealerRebatePercentage,
		decimal lotteryRebatePercentage,
		decimal slotRebatePercentage,
		decimal sportsRebatePercentage,
		decimal virtualSportsRebatePercentage,
		string modifier,
		CancellationToken cancellationToken = default);

	ValueTask InsertBrandSocialMediaSettingAsync(SocialMediaSetting setting, string modifier, CancellationToken cancellationToken = default);

	ValueTask DeleteBrandSocialMediaSettingAsync(Guid settingId, CancellationToken cancellationToken = default);

	ValueTask<SocialMediaSetting?> GetSocialMediaSettingAsync(Guid settingId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SocialMediaSetting> QueryBrandSocialMediaSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);
}
