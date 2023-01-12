using System.Text.Json;

namespace Tgs.BrandCertificate;

public interface ITgsRepository
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandStatus?> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<VipLevel> QueryVipLevelsAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandMaintenanceInfo> QueryBrandMaintenanceInfosAsync(string? brandCode, CancellationToken cancellationToken = default);

	ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceModel> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? Content, CancellationToken cancellationToken = default);

	ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<bool> UpdateBrandPlayerRegisterFlowSettingAsync(Guid brandId, JsonDocument PlayerRegisterFlowSettings, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes, CancellationToken cancellationToken = default);

	ValueTask<int?> GetTimeSpanMinutesByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);
}
