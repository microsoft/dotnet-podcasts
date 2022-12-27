FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update -y
RUN apt-get install -y python3
RUN dotnet workload install wasm-tools
WORKDIR /src
COPY ["src/Web/Server/Podcast.Server.csproj", "src/Web/Server/"]
COPY ["src/Web/Shared/Podcast.Shared.csproj", "src/Web/Shared/"]
COPY ["src/Web/Client/Podcast.Client.csproj", "src/Web/Client/"]
COPY ["src/Web/Pages/Podcast.Pages.csproj", "src/Web/Pages/"]
COPY ["src/Web/Components/Podcast.Components.csproj", "src/Web/Components/"]
RUN dotnet restore "src/Web/Server/Podcast.Server.csproj"
COPY . .
WORKDIR "/src/src/Web/Server"
RUN dotnet build "Podcast.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Podcast.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Podcast.Server.dll"]