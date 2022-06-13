namespace Tgs.BrandCertificate;

public interface ITgsRepository
{
	public ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId);
}
