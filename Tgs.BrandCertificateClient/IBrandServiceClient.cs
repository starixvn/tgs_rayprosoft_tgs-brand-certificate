namespace Tgs.BrandCertificate.Clients;

public interface IBrandServiceClient
{
	ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default);
}
