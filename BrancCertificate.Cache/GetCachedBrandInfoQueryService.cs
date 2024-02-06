using Microsoft.Extensions.Caching.Memory;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.Cache;

internal class GetCachedBrandInfoQueryService(
	IGetService<GetBrandInfoQuery, BrandInfo?> sourceService,
	IMemoryCache memoryCache) : IGetService<GetBrandInfoQuery, BrandInfo?>
{
	private readonly IGetService<GetBrandInfoQuery, BrandInfo?> _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
	private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

	public async ValueTask<BrandInfo?> GetAsync(GetBrandInfoQuery query, CancellationToken cancellationToken = default)
	{
		var key = $"Brand:{query.BrandId}";

		return await _memoryCache.GetOrCreateAsync(key, async entry =>
		{
			entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10D);

			return await _sourceService
				.GetAsync(query, cancellationToken)
				.ConfigureAwait(false);
		}).ConfigureAwait(false);
	}
}
