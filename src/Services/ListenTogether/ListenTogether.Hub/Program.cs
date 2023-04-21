using Azure.Identity;
using ListenTogether.Application;
using ListenTogether.Hub;
using ListenTogether.Infrastructure;
using ListenTogether.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var credential = new ChainedTokenCredential(new AzureDeveloperCliCredential(), new DefaultAzureCredential());
builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"]), credential);
builder.AddOrleans();

var serviceCollection = builder.Services;
serviceCollection.AddHub();
serviceCollection.AddApplication();
serviceCollection.AddInfrastructure(builder.Configuration);

var app = builder.Build();
await EnsureDbAsync(app.Services);

app.UseCors();
app.MapHub<ListenTogetherHub>("/listentogether");
app.MapGet("/", () => "Listen Together Hub");
app.MapOrleansDashboard();
app.Run();

static async Task EnsureDbAsync(IServiceProvider sp)
{
    await using var db = sp.CreateScope().ServiceProvider.GetRequiredService<ListenTogetherDbContext>();
    await db.Database.MigrateAsync();
}