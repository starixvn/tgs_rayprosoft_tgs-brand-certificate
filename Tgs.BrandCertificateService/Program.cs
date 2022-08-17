using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("/config/appsettings.json", true);

builder.Logging
	.AddOpenTelemetry(options =>
	{
		options.IncludeFormattedMessage = true;
		options.ParseStateValues = true;
		_ = options.AddFluentdForwardExporter(fluentOptions =>
		{
			var nodeIp = builder.Configuration.GetValue<string>("NODE_IP");
			fluentOptions.Host = nodeIp;
			fluentOptions.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME");
			fluentOptions.UseMessagePack();
		});
	});

builder.Services
	.AddBrandCertificate((options, sp) =>
	{
		var config = sp.GetRequiredService<IConfiguration>();

		config.GetSection("Brand").Bind(options);
	})
	.AddBrandCertificateRepositories((sp, builder) =>
	{
		var config = sp.GetRequiredService<IConfiguration>();

		_ = builder.UseNpgsql(config.GetConnectionString("Tgs"));
	})
	.AddMemoryCache()
	.AddBrandCertificateCache();

builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers(options=> 
	{
		options.UseDateOnlyTimeOnlyStringConverters();
	})
	.AddJsonOptions(options => 
	{
		options.UseDateOnlyTimeOnlyStringConverters();
	});

builder.Services
	.AddHealthChecks();

builder.Services
	.AddSwaggerGen(c =>
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
			c.IncludeXmlComments(path);

		c.ApplyWebApiToolkitSupport();
		c.UseDateOnlyTimeOnlyStringConverters();
	});

builder.Services
	.AddOptions<OtlpExporterOptions>()
	.Configure<IConfiguration>((options, conifg) =>
	{
		var nodeIp = conifg.GetValue<string>("NODE_IP");

		if (!string.IsNullOrWhiteSpace(nodeIp))
			options.Endpoint = new Uri($"http://{nodeIp}:4317");

		conifg.GetSection("Opentelemetry:Tracing").Bind(options);
	})
	.Services
	.AddProfileViewer()
	.AddOpenTelemetryTracing(builder => builder
		.SetResourceBuilder(ResourceBuilder.CreateDefault()
			.AddEnvironmentVariableDetector())
		.AddSource("Tgs.*")
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddNpgsql()
		.AddProfileViewExporter()
		.AddOtlpExporter())
	.Configure<AspNetCoreInstrumentationOptions>(options => options.Filter =
		httpContext => !httpContext.Request.Path.HasValue
			|| !httpContext.Request.Path.StartsWithSegments("/swagger")
			&& !httpContext.Request.Path.StartsWithSegments("/healthz")
			&& !httpContext.Request.Path.StartsWithSegments("/metrics")
			&& !new[] { "html", "ico", "png", "jpg" }.Any(
				ext => httpContext.Request.Path.Value.EndsWith($".{ext}")));

var app = builder.Build();

app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint(
		"/swagger/v1/swagger.yaml",
		"Tgs BrandCertificate Service");
});

app.MapControllers();
app.MapSwagger();
app.MapHealthChecks("/healthz");
app.MapProfileViewer();

app.Run();