using System.Text.Json.Serialization;
using FluentdForward.OpenTelemetry.MessagePack;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NATS.Client.Core;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry();

builder.Logging
	.AddOpenTelemetry(options =>
	{
		options.IncludeFormattedMessage = true;
		options.ParseStateValues = true;
		_ = options.AddFluentdForwardExporter(fluentOptions =>
		{
			var nodeIp = builder.Configuration.GetValue<string>("Otel:NodeIp")!;
			fluentOptions.Host = nodeIp;
			fluentOptions.Tag = builder.Configuration.GetValue<string>("Otel:ServiceName")!;
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
	.AddKeyedSingleton(
		"Nats",
		(sp, _) => new NatsConnection(NatsOpts.Default with
		{
			Url = builder.Configuration.GetConnectionString("Nats")!
		}))
	.AddMessageQueue()
	.AddNatsMessageQueue(config => config
		.ConfigureResolveConnection(sp => sp.GetRequiredKeyedService<NatsConnection>("Nats"))
		.AddHandler(
			Tgs.BrandCertificate.Cache.MessageQueue.ProcessorTypes.BrandFrontendInfoChangedEventHandlerType,
			"tgs.brand.frontendinfo.changed"))
	.AddNatsGlobPatternExchange("*");

builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers()
	.AddJsonOptions(options => options
		.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services
	.AddHealthChecks();


builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1",
		new OpenApiInfo
		{
			Title = "Tgs BrandCertificate Service",
			Version = "v1"
		});

	c.CustomSchemaIds(type => type.FullName.Replace('.', '_'));

	var xml = "Tgs.BrandCertificate.Service.xml";
	var path = Path.Combine(AppContext.BaseDirectory, xml);

	if (System.IO.File.Exists(path))
		c.IncludeXmlComments(path);
});

var excludeExtensions = new[] { "html", "ico", "png", "jpg" };
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
	.ConfigureResource(_ => _.AddService(builder.Configuration.GetValue<string>("Otel:ServiceName"))
		.AddEnvironmentVariableDetector())
	.WithTracing(tracerProviderBuilder => tracerProviderBuilder
		.AddSource("Tgs.*")
		.AddSource("Valhalla.*")
		.AddAspNetCoreInstrumentation(options => options.Filter =
		httpContext => !httpContext.Request.Path.HasValue
			|| !httpContext.Request.Path.StartsWithSegments("/swagger")
			&& !httpContext.Request.Path.StartsWithSegments("/healthz")
			&& !httpContext.Request.Path.StartsWithSegments("/metrics")
			&& !excludeExtensions.Any(
				ext => httpContext.Request.Path.Value.EndsWith($".{ext}")))
		.AddHttpClientInstrumentation()
		.AddNpgsql()
		.AddProfileViewExporter()
		.AddOtlpExporter(options => options.Endpoint = builder.Configuration.GetValue<Uri>("Otel:Collector")))
	.WithMetrics(meterProviderBuilder => meterProviderBuilder
		.AddRuntimeInstrumentation()
		.AddAspNetCoreInstrumentation()
		.AddHttpClientInstrumentation()
		.AddOtlpExporter());

var app = builder.Build();

app.MapControllers();
app.MapHealthChecks("/healthz");
app.MapProfileViewer();


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tgs BrandCertificate Service"));

SentrySdk.CaptureMessage($"Started result Tgs-BrandCertificateService at {DateTime.Now.ToString()}");

app.Run();