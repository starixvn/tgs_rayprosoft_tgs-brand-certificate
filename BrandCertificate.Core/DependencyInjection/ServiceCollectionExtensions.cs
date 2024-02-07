using System.Reflection;
using Tgs.BrandCertificate;
using Tgs.BrandCertificateService;
using Tgs.Core.Infrastructures.Security;

namespace Microsoft.Extensions.DependencyInjection;

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
			.RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
			.AddTransient<ICredentialGenerator, CredentialGenerator>()
			.AddTransient<IHashProvider, HashProvider>()
			.AddTransient<IPasswordHandler, PasswordHandler>()
			.AddSerialNumberGenerator();
}
