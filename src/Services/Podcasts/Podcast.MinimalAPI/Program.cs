using Asp.Versioning;
using Asp.Versioning.Conventions;
using Azure.Monitor.OpenTelemetry.Exporter;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Podcast.API.Routes;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Http;
using Podcast.Infrastructure.Http.Feeds;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Database and storage related-services
var connectionString = builder.Configuration.GetConnectionString("PodcastDb")!;
builder.Services.AddSqlServer<PodcastDbContext>(connectionString);
var queueConnectionString = builder.Configuration.GetConnectionString("FeedQueue");
builder.Services.AddSingleton(new QueueClient(queueConnectionString, "feed-queue"));
builder.Services.AddHttpClient<IFeedClient, FeedClient>();
builder.Services.AddTransient<JitterHandler>();
builder.Services.AddHttpClient<ShowClient>().AddHttpMessageHandler<JitterHandler>();

// Authentication and authorization-related services
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

// OpenAPI and versioning-related services
builder.Services.AddSwaggerGen();
builder.Services.Configure<SwaggerGeneratorOptions>(opts =>
{
    opts.InferSecuritySchemes = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddCors(setup =>
{
    setup.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Rate-limiting and output caching-related services
builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("feeds", options =>
{
    options.PermitLimit = 5;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.QueueLimit = 0;
    options.Window = TimeSpan.FromSeconds(2);
    options.AutoReplenishment = false;
}));

var serviceName = typeof(Program).Assembly.FullName;
var serviceVersion = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
var serviceResource =
        ResourceBuilder
         .CreateDefault()
         .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

builder.Services.AddOpenTelemetryTracing(b =>
    b.AddSource("dotnet-podcasts")
     .SetResourceBuilder(serviceResource)
     .AddJaegerExporter(o =>
     {
         // because we chose to use _endpoint_ we must use binaryThrift protocol
         o.Endpoint = new Uri(builder.Configuration.GetSection("Jaeger").GetValue<string>("Endpoint"));
         o.Protocol = OpenTelemetry.Exporter.JaegerExportProtocol.HttpBinaryThrift;
     })
     .AddAzureMonitorTraceExporter(o =>
     {
         o.ConnectionString = builder.Configuration.GetConnectionString("AzureMonitor");
     })
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()
);

builder.Services.AddOpenTelemetryMetrics(metrics =>
{
    metrics
    .SetResourceBuilder(serviceResource)
    .AddPrometheusExporter()
    .AddAzureMonitorMetricExporter(o =>
    {
        o.ConnectionString = builder.Configuration.GetConnectionString("AzureMonitor");
    })
    .AddAspNetCoreInstrumentation()
    .AddRuntimeInstrumentation()
    .AddProcessInstrumentation()    
    .AddHttpClientInstrumentation();
});
builder.Services.AddOutputCache();

var app = builder.Build();

await EnsureDbAsync(app.Services);


// Register required middlewares
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetPodcast Api v1");
});
app.UseCors();
app.UseRateLimiter();
app.UseOutputCache();

app.MapPrometheusScrapingEndpoint();

var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(1.0)
                    .HasApiVersion(2.0)
                    .ReportApiVersions()
                    .Build();

var shows = app.MapGroup("/shows");
var categories = app.MapGroup("/categories");
var episodes = app.MapGroup("/episodes");

var blocking = app.MapGroup("/blocking");

shows
    .MapShowsApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0)
    .MapToApiVersion(2.0);

categories
    .MapCategoriesApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

episodes
    .MapEpisodesApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

blocking
    .MapBlockingApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0)
    .MapToApiVersion(2.0);

var feeds = app.MapGroup("/feeds");
feeds
    .MapFeedsApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(2.0)
    .RequireRateLimiting("feeds");

app.Run();

static async Task EnsureDbAsync(IServiceProvider sp)
{
    await using var db = sp.CreateScope().ServiceProvider.GetRequiredService<PodcastDbContext>();
    await db.Database.MigrateAsync();
}