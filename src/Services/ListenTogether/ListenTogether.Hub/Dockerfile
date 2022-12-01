#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/ListenTogether/ListenTogether.Hub/ListenTogether.Hub.csproj", "src/Services/ListenTogether/ListenTogether.Hub/"]
COPY ["src/Services/ListenTogether/ListenTogether.Infrastructure/ListenTogether.Infrastructure.csproj", "src/Services/ListenTogether/ListenTogether.Infrastructure/"]
COPY ["src/Services/ListenTogether/ListenTogether.Application/ListenTogether.Application.csproj", "src/Services/ListenTogether/ListenTogether.Application/"]
COPY ["src/Services/ListenTogether/ListenTogether.Domain/ListenTogether.Domain.csproj", "src/Services/ListenTogether/ListenTogether.Domain/"]
RUN dotnet restore "src/Services/ListenTogether/ListenTogether.Hub/ListenTogether.Hub.csproj"
COPY . .
WORKDIR "/src/src/Services/ListenTogether/ListenTogether.Hub"
RUN dotnet build "ListenTogether.Hub.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "ListenTogether.Hub.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ListenTogether.Hub.dll"]