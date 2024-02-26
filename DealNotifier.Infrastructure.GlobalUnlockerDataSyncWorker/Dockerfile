#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["T-UnlockDataSyncWorker/T-UnlockDataSyncWorker.csproj", "T-UnlockDataSyncWorker/"]
COPY ["DealNotifier.Core.Application/DealNotifier.Core.Application.csproj", "DealNotifier.Core.Application/"]
COPY ["DealNotifier.Core.Domain/DealNotifier.Core.Domain.csproj", "DealNotifier.Core.Domain/"]
COPY ["DealNotifier.Infrastructure.Persistence/DealNotifier.Infrastructure.Persistence.csproj", "DealNotifier.Infrastructure.Persistence/"]
RUN dotnet restore "T-UnlockDataSyncWorker/T-UnlockDataSyncWorker.csproj"
COPY . .
WORKDIR "/src/T-UnlockDataSyncWorker"
RUN dotnet build "T-UnlockDataSyncWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "T-UnlockDataSyncWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "T-UnlockDataSyncWorker.dll"]