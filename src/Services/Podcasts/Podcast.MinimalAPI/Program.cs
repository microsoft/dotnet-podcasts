using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Http.Feeds;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Podcast.API.Routes;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Database and storage related-services
var connectionString = builder.Configuration.GetConnectionString("PodcastDb");
builder.Services.AddSqlServer<PodcastDbContext>(connectionString);
var queueConnectionString = builder.Configuration.GetConnectionString("FeedQueue");
builder.Services.AddSingleton(new QueueClient(queueConnectionString, "feed-queue"));
builder.Services.AddHttpClient<IFeedClient, FeedClient>();

// Authentication and authorization-related services
// builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorizationBuilder().AddPolicy("modify_feeds", policy => policy.RequireScope("API.Access"));

// OpenAPI and versioning-related services
builder.Services.AddSwaggerGen();
builder.Services.Configure<SwaggerGeneratorOptions>(opts => {
    opts.InferSecuritySchemes = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
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
builder.Services.AddOutputCache();

var app = builder.Build();

await EnsureDbAsync(app.Services);

// Register required middlewares
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Podcasts Minimal API");
});
app.UseCors();
app.UseRateLimiter();
app.UseOutputCache();

var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(1.0)
                    .HasApiVersion(2.0)
                    .ReportApiVersions()
                    .Build();

var shows = app.MapGroup("/shows");
var categories = app.MapGroup("/categories");
var episodes = app.MapGroup("/episodes");

shows
    .MapShowsApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0)
    .MapToApiVersion(2.0)
    .CacheOutput();

categories
    .MapCategoriesApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

episodes
    .MapEpisodesApi()
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

var feedIngestionEnabled = app.Configuration.GetValue<bool>("Features:FeedIngestion");

if (feedIngestionEnabled)
{
    var feeds = app.MapGroup("/feeds");
    feeds.MapFeedsApi().WithApiVersionSet(versionSet).MapToApiVersion(2.0).RequireRateLimiting("feeds");
}

app.Run();

static async Task EnsureDbAsync(IServiceProvider sp)
{
    await using var db = sp.CreateScope().ServiceProvider.GetRequiredService<PodcastDbContext>();
    await db.Database.MigrateAsync();
}