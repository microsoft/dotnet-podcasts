using Podcast.Components;
using Podcast.Pages.Data;
using Podcast.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<PodcastService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PodcastApi:BaseAddress"]!);
});
builder.Services.AddScoped<ThemeInterop>();
builder.Services.AddScoped<AudioInterop>();
builder.Services.AddScoped<LocalStorageInterop>();
builder.Services.AddScoped<ClipboardInterop>();
builder.Services.AddScoped<SubscriptionsService>();
builder.Services.AddScoped<ListenLaterService>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddScoped<ListenTogetherHubClient>(_ =>
    new ListenTogetherHubClient(builder.Configuration["ListenTogetherHub"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("{*path:nonfile}", "/_Host");
app.MapFallbackToPage("/", "/Landing");

app.Run();
