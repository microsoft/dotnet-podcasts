var builder = WebApplication.CreateBuilder(args);
builder.AddOrleans();

var serviceCollection = builder.Services;
serviceCollection.AddHub();
serviceCollection.AddApplication();
serviceCollection.AddInfrastructure(builder.Configuration);

await using var app = builder.Build();
await EnsureDbAsync(app.Services);

app.UseCors();
app.MapHub<ListenTogetherHub>("/listentogether");
app.MapGet("/", () => "Listen Together Hub");
app.MapOrleansDashboard();
app.Run();

static async Task EnsureDbAsync(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    await using var db = scope.ServiceProvider.GetRequiredService<ListenTogetherDbContext>();
    await db.Database.MigrateAsync();
}