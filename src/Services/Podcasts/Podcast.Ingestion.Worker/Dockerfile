#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Podcasts/Podcast.Ingestion.Worker/Podcast.Ingestion.Worker.csproj", "src/Services/Podcasts/Podcast.Ingestion.Worker/"]
COPY ["src/Services/Podcasts/Podcast.Infrastructure/Podcast.Infrastructure.csproj", "src/Services/Podcasts/Podcast.Infrastructure/"]
RUN dotnet restore "src/Services/Podcasts/Podcast.Ingestion.Worker/Podcast.Ingestion.Worker.csproj"
COPY . .
WORKDIR "/src/src/Services/Podcasts/Podcast.Ingestion.Worker"
RUN dotnet build "Podcast.Ingestion.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Podcast.Ingestion.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Podcast.Ingestion.Worker.dll"]