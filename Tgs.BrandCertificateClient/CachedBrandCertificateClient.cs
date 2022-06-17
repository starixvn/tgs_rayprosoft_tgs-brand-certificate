using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients;

internal class CachedBrandCertificateClient : IBrandCertificateClient
{
	private static readonly SemaphoreSlim _LockObject = new(1, 1);
	private readonly IBrandCertificateClient _baseClient;
	private readonly IMemoryCache _memoryCache;

	public CachedBrandCertificateClient(IBrandCertificateClient baseClient, IMemoryCache memoryCache)
	{
		_baseClient = baseClient ?? throw new ArgumentNullException(nameof(baseClient));
		_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
	}

	public Task<string?> MakeJsonWebTokenAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> _baseClient.MakeJsonWebTokenAsync(brandId, cancellationToken);

	public async Task<SecurityKey> GetSystemValidateSecurityKeyAsync(CancellationToken cancellationToken = default)
	{
		if (_memoryCache.TryGetValue<SecurityKey>("Brand:Varify:Key", out var securityKey))
			return securityKey;

		await _LockObject.WaitAsync(cancellationToken).ConfigureAwait(false);

		try
		{
			if (_memoryCache.TryGetValue("Brand:Varify:Key", out securityKey))
				return securityKey!;

			securityKey = await _baseClient
				.GetSystemValidateSecurityKeyAsync(cancellationToken)
				.ConfigureAwait(false);

			_ = _memoryCache.Set("Brand:Varify:Key", securityKey, DateTimeOffset.Now.AddHours(1));

			return securityKey;
		}
		finally
		{
			_ = _LockObject.Release();
		}
	}
}
