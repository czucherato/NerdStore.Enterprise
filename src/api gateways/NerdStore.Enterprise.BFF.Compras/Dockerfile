#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./api gateways/NerdStore.Enterprise.BFF.Compras/NerdStore.Enterprise.BFF.Compras.csproj", "api gateways/NerdStore.Enterprise.BFF.Compras/"]
COPY ["./building-blocks/NerdStore.Enterprise.Core/NerdStore.Enterprise.Core.csproj", "building-blocks/NerdStore.Enterprise.Core/"]
COPY ["./building-blocks/NerdStore.Enterprise.WebAPI.Core/NerdStore.Enterprise.WebAPI.Core.csproj", "building-blocks/NerdStore.Enterprise.WebAPI.Core/"]
RUN dotnet restore "src/api gateways/NerdStore.Enterprise.BFF.Compras/NerdStore.Enterprise.BFF.Compras.csproj"
COPY . .
WORKDIR "api gateways/NerdStore.Enterprise.BFF.Compras"
RUN dotnet build "NerdStore.Enterprise.BFF.Compras.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NerdStore.Enterprise.BFF.Compras.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NerdStore.Enterprise.BFF.Compras.dll"]