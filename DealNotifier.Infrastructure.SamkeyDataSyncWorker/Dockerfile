#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SamkeyDataSyncWorker/SamkeyDataSyncWorker.csproj", "SamkeyDataSyncWorker/"]
COPY ["DealNotifier.Core.Application/DealNotifier.Core.Application.csproj", "DealNotifier.Core.Application/"]
COPY ["DealNotifier.Core.Domain/DealNotifier.Core.Domain.csproj", "DealNotifier.Core.Domain/"]
COPY ["DealNotifier.Infrastructure.Persistence/DealNotifier.Infrastructure.Persistence.csproj", "DealNotifier.Infrastructure.Persistence/"]
RUN dotnet restore "SamkeyDataSyncWorker/SamkeyDataSyncWorker.csproj"
COPY . .
WORKDIR "/src/SamkeyDataSyncWorker"
RUN dotnet build "SamkeyDataSyncWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SamkeyDataSyncWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SamkeyDataSyncWorker.dll"]