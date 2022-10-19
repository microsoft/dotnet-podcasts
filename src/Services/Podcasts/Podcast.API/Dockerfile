#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Podcasts/Podcast.API/Podcast.API.csproj", "src/Services/Podcasts/Podcast.API/"]
COPY ["src/Services/Podcasts/Podcast.Infrastructure/Podcast.Infrastructure.csproj", "src/Services/Podcasts/Podcast.Infrastructure/"]
RUN dotnet restore "src/Services/Podcasts/Podcast.API/Podcast.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Podcasts/Podcast.API"
RUN dotnet build "Podcast.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Podcast.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Podcast.API.dll"]