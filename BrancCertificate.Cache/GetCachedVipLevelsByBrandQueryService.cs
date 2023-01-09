using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.Cache;

internal class GetCachedVipLevelsByBrandQueryService : IQueryService<GetVipLevelsByBrandQuery, VipLevel>
{
	private readonly IQueryService<GetVipLevelsByBrandQuery, VipLevel> _sourceService;
	private readonly IMemoryCache _memoryCache;

	public GetCachedVipLevelsByBrandQueryService(
		IQueryService<GetVipLevelsByBrandQuery, VipLevel> sourceService,
		IMemoryCache memoryCache)
	{
		_sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
		_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
	}

	public async IAsyncEnumerable<VipLevel> QueryAsync(
		GetVipLevelsByBrandQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var key = $"VipLevels:{query.BrandId}";

		var items = await _memoryCache.GetOrCreateAsync(key, async entry =>
		{
			entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10D);

			var result = await _sourceService
				.QueryAsync(query, cancellationToken)
				.ToArrayAsync()
				.ConfigureAwait(false);

			return result;
		}).ConfigureAwait(false)!;

		foreach (var item in items!)
			yield return item;
	}
}
