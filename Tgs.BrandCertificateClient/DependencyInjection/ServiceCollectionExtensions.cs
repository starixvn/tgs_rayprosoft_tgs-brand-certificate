using Microsoft.Extensions.Options;
using Tgs.BrandCertificate.Clients;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBrandCertificateClient(
		this IServiceCollection services,
		Action<BrandCertificateClientOptions, IServiceProvider> configure)
		=> services
			.AddOptions<BrandCertificateClientOptions>()
			.Configure(configure)
			.Services
			.AddHttpClient<BrandCertificateClient>()
			.ConfigureHttpClient((sp, httpClient) =>
			{
				var options = sp.GetRequiredService<IOptions<BrandCertificateClientOptions>>().Value;

				httpClient.BaseAddress = options.BrandCertificateBaseUrl;
			})
			.Services
			.AddMemoryCache()
			.AddTransient<IBrandCertificateClient>(
				sp => ActivatorUtilities.CreateInstance<CachedBrandCertificateClient>(
					sp,
					sp.GetRequiredService<BrandCertificateClient>()));

	public static IServiceCollection AddBrandServiceClient(
		this IServiceCollection services,
		Action<BrandCertificateClientOptions, IServiceProvider> configure)
		=> services
			.AddOptions<BrandCertificateClientOptions>()
			.Configure(configure)
			.Services
			.AddHttpClient<IBrandServiceClient, BrandServiceClient>()
			.ConfigureHttpClient((sp, httpClient) =>
			{
				var options = sp.GetRequiredService<IOptions<BrandCertificateClientOptions>>().Value;

				httpClient.BaseAddress = options.BrandCertificateBaseUrl;
			})
			.Services;
}
