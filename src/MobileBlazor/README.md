# .NET MAUI Blazor podcast app for Android, iOS, macOS, and Windows
The *.sln* is located in [/src/MobileBlazor/Podcast.DotnetMaui.Blazor.sln](Podcast.DotnetMaui.Blazor.sln)
The startup project is **NetPodsMauiBlazor**

This solution has four projects:
- **NetPodsMauiBlazor**: Maui App based on default template of .NET Maui Blazor App.
- References to Blazor projects
  - **[/src/Web/Components/Podcast.Components.csproj](/src/Web/Components/Podcast.Components.csproj)**: Razor Class Library project with shared components.
  - **[/src/Web/Pages/Podcast.Pages.csproj](/src/Web/Pages/Podcast.Pages.csproj)**: Razor Class Library project with shared pages.
  - **[/src/Web/Shared/Podcast.Shared.csproj](/src/Web/Shared/Podcast.Shared.csproj)**: Shared class library project with services.