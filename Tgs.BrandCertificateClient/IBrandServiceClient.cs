namespace Tgs.BrandCertificate.Clients;

public interface IBrandServiceClient
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandStatus> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<VipLevelInfo> GetVipLevelInfosAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandMaintenanceInfo> GetBrandMaintenanceInfosAsync(string brandCode, CancellationToken cancellationToken = default);

	ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceResult> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? Content, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes, CancellationToken cancellationToken = default);

	ValueTask<int?> GetTimeSpanMinutesByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);
}
