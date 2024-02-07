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

	IAsyncEnumerable<VipLevel> QueryVipLevelsAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? content, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<bool> UpdateBrandPlayerRegisterFlowSettingAsync(Guid brandId, JsonDocument playerRegisterFlowSettings, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandStatusAsync(
		Guid brandId,
		BrandStatus status,
		string modifier,
		CancellationToken cancellationToken = default);

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
}
