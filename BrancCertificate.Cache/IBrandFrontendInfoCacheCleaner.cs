namespace Tgs.BrandCertificate.Cache;

internal interface IBrandFrontendInfoCacheCleaner
{
	void Clean(Guid brandId);
}
