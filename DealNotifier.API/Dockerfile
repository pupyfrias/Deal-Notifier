#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["DealNotifier.Core.Application/DealNotifier.Core.Application.csproj", "DealNotifier.Core.Application/"]
COPY ["DealNotifier.Core.Domain/DealNotifier.Core.Domain.csproj", "DealNotifier.Core.Domain/"]
COPY ["DealNotifier.Infrastructure.Email/DealNotifier.Infrastructure.Email.csproj", "DealNotifier.Infrastructure.Email/"]
COPY ["DealNotifier.Infrastructure.Identity/DealNotifier.Infrastructure.Identity.csproj", "DealNotifier.Infrastructure.Identity/"]
COPY ["DealNotifier.Infrastructure.Persistence/DealNotifier.Infrastructure.Persistence.csproj", "DealNotifier.Infrastructure.Persistence/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]