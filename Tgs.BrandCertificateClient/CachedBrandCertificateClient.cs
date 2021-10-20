using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients
{
	internal class CachedBrandCertificateClient : IBrandCertificateClient
	{
		private readonly static SemaphoreSlim _LockObject = new SemaphoreSlim(1, 1);
		private readonly IBrandCertificateClient _baseClient;
		private readonly IMemoryCache _memoryCache;

		public CachedBrandCertificateClient(IBrandCertificateClient baseClient, IMemoryCache memoryCache)
		{
			_baseClient = baseClient ?? throw new ArgumentNullException(nameof(baseClient));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

		public Task<string?> MakeJsonWebTokenAsync(Guid brandId)
			=> _baseClient.MakeJsonWebTokenAsync(brandId);

		public async Task<SecurityKey> GetSystemValidateSecurityKeyAsync()
		{
			if (_memoryCache.TryGetValue<SecurityKey>("Brand:Varify:Key", out var securityKey))
				return securityKey;

			await _LockObject.WaitAsync().ConfigureAwait(false);

			try
			{
				if (_memoryCache.TryGetValue("Brand:Varify:Key", out securityKey))
					return securityKey!;

				securityKey = await _baseClient
					.GetSystemValidateSecurityKeyAsync()
					.ConfigureAwait(false);

				_memoryCache.Set("Brand:Varify:Key", securityKey, DateTimeOffset.Now.AddHours(1));

				return securityKey;
			}
			finally
			{
				_LockObject.Release();
			}
		}
	}
}
