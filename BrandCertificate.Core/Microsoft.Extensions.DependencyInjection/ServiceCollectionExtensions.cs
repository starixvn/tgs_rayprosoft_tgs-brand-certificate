using System;
using System.Reflection;
using Tgs.BrandCertificateService;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBrandCertificate(
			this IServiceCollection services,
			Action<BrandCertificateOptions, IServiceProvider> configure)
			=> services
				.AddOptions<BrandCertificateOptions>()
				.Configure(configure)
				.Services
				.AddSingleton<BrandCertificateManager>()
				.RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);
	}
}
