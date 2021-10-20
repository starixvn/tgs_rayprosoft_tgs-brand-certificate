using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tgs.BrandCertificateService
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddBrandCertificate((options, sp) =>
				{
					var config = sp.GetRequiredService<IConfiguration>();

					config.GetSection("Brand").Bind(options);
				})
				.AddBrandCertificateRepositories((sp, builder) =>
				{
					var config = sp.GetRequiredService<IConfiguration>();

					builder.UseNpgsql(config.GetConnectionString("Tgs"));
				});

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(
					"v1",
					new()
					{
						Title = "Tgs BrandCertificate Service",
						Version = "v1"
					});

				var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var path = Path.Combine(AppContext.BaseDirectory, xml);

				if (File.Exists(path))
				{
					c.IncludeXmlComments(path);
				}
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint(
					"/swagger/v1/swagger.yaml",
					"Tgs BrandCertificate Service");
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapSwagger();
			});
		}
	}
}
