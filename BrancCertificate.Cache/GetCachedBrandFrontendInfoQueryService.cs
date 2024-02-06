using Microsoft.Extensions.Caching.Memory;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.Cache;

internal class GetCachedBrandFrontendInfoQueryService(
	IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo> sourceService,
	IMemoryCache memoryCache) : IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo>, IBrandFrontendInfoCacheCleaner
{
	private readonly IGetService<GetBrandFrontendInfoQuery, BrandFrontendInfo> _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
	private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

	public void Clean(Guid brandId)
	{
		var key = $"BrandFrontend:{brandId}";

		_memoryCache.Remove(key);
	}

	public async ValueTask<BrandFrontendInfo> GetAsync(
		GetBrandFrontendInfoQuery query,
		CancellationToken cancellationToken = default)
	{
		var key = $"BrandFrontend:{query.BrandId}";

		return (await _memoryCache.GetOrCreateAsync(key, async entry =>
		{
			entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10D);

			return await _sourceService
				.GetAsync(query, cancellationToken)
				.ConfigureAwait(false);
		}).ConfigureAwait(false))!;
	}
}
