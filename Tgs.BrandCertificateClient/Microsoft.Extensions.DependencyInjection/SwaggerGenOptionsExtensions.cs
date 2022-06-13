using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerGenOptionsExtensions
{
	public static void AddBrandCertificateSecurityDefinition(this SwaggerGenOptions options)
	{
		options.AddSecurityDefinition("Brand", new OpenApiSecurityScheme
		{
			Name = "brand-token",
			Description = "Brand certificate",
			In = ParameterLocation.Header
		});

		options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			[new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Brand"
				}
			}] = Array.Empty<string>()
		});
	}
}
