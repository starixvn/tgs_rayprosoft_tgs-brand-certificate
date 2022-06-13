namespace Tgs.BrandCertificate;

public interface ITgsRepository
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);

	ValueTask<BrandFrontendInfo?> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default);
}
