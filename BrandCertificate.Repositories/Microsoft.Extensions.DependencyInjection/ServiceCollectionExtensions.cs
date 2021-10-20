using System;
using Microsoft.EntityFrameworkCore;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Repositories;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBrandCertificateRepositories(
			this IServiceCollection services,
			Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsConfigure)
			=> services
				.AddDbContext<TgsContext>(dbContextOptionsConfigure)
				.AddTransient<ITgsRepository, TgsRepository>();
	}
}
