using System.Text.Json;

namespace Tgs.BrandCertificate.Clients;

public interface IBrandServiceClient
{
	IAsyncEnumerable<BrandCurrencyInfo> GetAllBrandCurrenciesAsync(CancellationToken cancellationToken = default);

	ValueTask<int?> GetBrandBetLimitIdAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes, CancellationToken cancellationToken = default);

	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandInfo?> GetBrandInfoByCodeAsync(string brandCode, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceResult> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandMaintenanceInfo> GetBrandMaintenanceInfosAsync(string brandCode, CancellationToken cancellationToken = default);

	ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandAgentDividendInfo> GetBrandAgentDividendInfosAsync(CancellationToken cancellationToken = default);

	ValueTask<SmsVendorInfo<TSmsSettings>?> GetBrandSmsSettingsAsync<TSmsSettings>(
		Guid brandId,
		JsonSerializerOptions? jsonSerializerOptions = null,
		CancellationToken cancellationToken = default);

	ValueTask<BrandStatus> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandListItem> QueryBrandListAsync(BrandListQuery query, CancellationToken cancellationToken = default);

	ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? content, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandStatusAsync(
		Guid brandId,
		BrandStatus status,
		string modifier,
		CancellationToken cancellationToken = default);

	ValueTask<bool?> GetBrandPlatformAccountFundAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask SetBrandPlatformAccountFundAsync(Guid brandId, bool enabled, string modifier, CancellationToken cancellationToken = default);
	
	ValueTask<Guid> CreateBrandAsync(
		CreateBrandRequest request,
		CancellationToken cancellationToken = default);

	ValueTask<Guid> UpdateBrandAsync(
		UpdateBrandRequest request,
		CancellationToken cancellationToken = default);

	ValueTask<BrandFullInfo?> GetBrandFullInfoAsync(Guid brandId, CancellationToken cancellationToken = default);
}
