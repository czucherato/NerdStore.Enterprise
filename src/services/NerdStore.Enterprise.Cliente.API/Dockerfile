#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./services/NerdStore.Enterprise.Cliente.API/NerdStore.Enterprise.Cliente.API.csproj", "services/NerdStore.Enterprise.Cliente.API/"]
COPY ["./building-blocks/NerdStore.Enterprise.Core/NerdStore.Enterprise.Core.csproj", "building-blocks/NerdStore.Enterprise.Core/"]
COPY ["./building-blocks/NerdStore.Enterprise.WebAPI.Core/NerdStore.Enterprise.WebAPI.Core.csproj", "building-blocks/NerdStore.Enterprise.WebAPI.Core/"]
COPY ["./building-blocks/NerdStore.Enterprise.MessageBus/NerdStore.Enterprise.MessageBus.csproj", "building-blocks/NerdStore.Enterprise.MessageBus/"]
RUN dotnet restore "services/NerdStore.Enterprise.Cliente.API/NerdStore.Enterprise.Cliente.API.csproj"
COPY . .
WORKDIR "services/NerdStore.Enterprise.Cliente.API"
RUN dotnet build "NerdStore.Enterprise.Cliente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NerdStore.Enterprise.Cliente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NerdStore.Enterprise.Cliente.API.dll"]