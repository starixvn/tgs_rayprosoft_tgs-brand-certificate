using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests;

internal class TestBrandCertificateApplication : WebApplicationFactory<Program>
{
	private readonly Action<IWebHostBuilder>? _webHostConfigure;

	public TestBrandCertificateApplication(Action<IWebHostBuilder>? webHostConfigure = null)
	{
		_webHostConfigure = webHostConfigure;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		_ = builder
			.ConfigureAppConfiguration(builder => builder.Sources.Clear())
			.ConfigureLogging(logging => logging.ClearProviders().AddConsole());

		_ = builder.ConfigureServices(services => services
			.AddSingleton<IConfigureOptions<OtlpExporterOptions>>(
				sp => new ConfigureNamedOptions<OtlpExporterOptions>(
					Options.DefaultName,
					exporterOptions =>
					{
						exporterOptions.TimeoutMilliseconds = 0;
						exporterOptions.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
						exporterOptions.Endpoint = new Uri("http://otel.rpgp.cc:4317");
					}))
			.Replace(ServiceDescriptor.Singleton(sp =>
			{
				var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
				_ = dbContextOptionsBuilder
					.UseInMemoryDatabase(Guid.NewGuid().ToString())
					.ConfigureWarnings(x => x.Ignore(
						InMemoryEventId.TransactionIgnoredWarning));

				return new TgsContext(dbContextOptionsBuilder.Options);
			}))
			.AddOptions<BrandCertificateOptions>()
			.Configure(options =>
			{
				options.SecurityKeyPath = "Assets/testkey.pem";
			})
			.Services
			.AddSingleton(sp =>
			{
				var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
				_ = dbContextOptionsBuilder
					.UseInMemoryDatabase(Guid.NewGuid().ToString())
					.ConfigureWarnings(x => x.Ignore(
						InMemoryEventId.TransactionIgnoredWarning));

				return new TgsContext(dbContextOptionsBuilder.Options);
			}));

		_webHostConfigure?.Invoke(builder);
	}
}
