namespace Tgs.BrandCertificate;

public interface ITgsRepository
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandStatus?> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<VipLevel> QueryVipLevelsAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BrandMaintenanceInfo> QueryBrandMaintenanceInfosAsync(string? brandCode, CancellationToken cancellationToken = default);

	ValueTask BatchUpdateBrandMaintenanceTimeAsync(Guid[] brandIds, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceModel> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceInfoAsync(Guid brandMaintenanceId, string Content, CancellationToken cancellationToken = default);

	ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default);
}