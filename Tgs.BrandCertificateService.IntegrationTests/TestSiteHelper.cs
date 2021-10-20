using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests
{
	internal static class TestSiteHelper
	{
		public static IHost GetBrandCertificateTestHost()
			=> GetBrandCertificateTestHost(_ => { });

		public static IHost GetBrandCertificateTestHost(Action<IServiceCollection> configureServices)
			=> Host.CreateDefaultBuilder(Array.Empty<string>())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder
						.UseStartup<Startup>()
						.ConfigureAppConfiguration((ctx, builder) =>
						{
							builder.Sources.Clear();
						})
						.UseTestServer()
						.ConfigureTestServices(services =>
						{
							services
								.AddOptions<BrandCertificateOptions>()
								.Configure(options =>
								{
									options.SecurityKeyPath = "Assets/testkey.pem";
								})
								.Services
								.AddSingleton(sp =>
								{
									var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
									dbContextOptionsBuilder
										.UseInMemoryDatabase(Guid.NewGuid().ToString())
										.ConfigureWarnings(x => x.Ignore(
											InMemoryEventId.TransactionIgnoredWarning));

									return new TgsContext(dbContextOptionsBuilder.Options);
								});
							;

							configureServices(services);
						});
				})
				.Build();
	}
}
