using System.Reflection;
using FluentdForward.OpenTelemetry.MessagePack;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("/config/appsettings.json", true)
	.AddEnvironmentVariables();

builder.Logging
	.AddOpenTelemetry(options =>
	{
		options.IncludeFormattedMessage = true;
		options.ParseStateValues = true;
		_ = options.AddFluentdForwardExporter(fluentOptions =>
		{
			var nodeIp = builder.Configuration.GetValue<string>("NODE_IP")!;
			fluentOptions.Host = nodeIp;
			fluentOptions.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;
			fluentOptions.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
				"MachineName",
				() => Environment.MachineName));
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
	.AddMessageQueue()
	.AddNatsMessageQueue(config => config
		.ConfigQueueOptions((options, sp) =>
		{
			var config = sp.GetRequiredService<IConfiguration>();

			options.Url = config.GetConnectionString("Nats")!;
		})
		.AddHandler(
			Tgs.BrandCertificate.Cache.MessageQueue.ProcessorTypes.BrandFrontendInfoChangedEventHandlerType,
			"tgs.brand.frontendinfo.changed"))
	.AddNatsGlobPatternExchange("*");

builder.Services
	.AddDateOnlyTimeOnlyStringConverters()
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers();

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
	.Configure<IConfiguration>((options, config) =>
	{
		var nodeIp = config.GetValue<string>("NODE_IP");

		if (!string.IsNullOrWhiteSpace(nodeIp))
			options.Endpoint = new Uri($"http://{nodeIp}:4317");

		config.GetSection("Opentelemetry:Tracing").Bind(options);
	})
	.Services
	.AddProfileViewer()
	.AddOpenTelemetry()
	.ConfigureResource(_ => ResourceBuilder.CreateDefault()
		.AddEnvironmentVariableDetector())
	.WithTracing(tracerProviderBuilder => tracerProviderBuilder
		.AddSource("Tgs.*")
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddNpgsql()
		.AddProfileViewExporter()
		.AddOtlpExporter())
	.WithMetrics(meterProviderBuilder => meterProviderBuilder
		.AddRuntimeInstrumentation()
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddOtlpExporter())
	.Services
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
