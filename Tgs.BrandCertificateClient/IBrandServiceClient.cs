namespace Tgs.BrandCertificate.Clients;

public interface IBrandServiceClient
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandStatus> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<VipLevelInfo> GetVipLevelInfosAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceInfos> GetBrandMaintenanceInfosAsync(string brandCode, Paging Paging, CancellationToken cancellationToken = default);

	ValueTask BatchUpdateBrandMaintenanceTimeAsync(Guid[] brandIds, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default);

	ValueTask<BrandMaintenanceResult> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default);

	ValueTask UpdateBrandMaintenanceInfoAsync(Guid brandMaintenanceId, string Content, CancellationToken cancellationToken = default);
}