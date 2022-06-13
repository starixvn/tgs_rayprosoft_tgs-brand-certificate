namespace Tgs.BrandCertificate.Clients;

public interface IBrandServiceClient
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);
}
