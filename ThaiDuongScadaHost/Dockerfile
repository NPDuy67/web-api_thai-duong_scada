#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ThaiDuongScadaHost/ThaiDuongScada.Host.csproj", "ThaiDuongScadaHost/"]
COPY ["ThaiDuongScada.Domain/ThaiDuongScada.Domain.csproj", "ThaiDuongScada.Domain/"]
COPY ["ThaiDuongScada.Infrastructure/ThaiDuongScada.Infrastructure.csproj", "ThaiDuongScada.Infrastructure/"]
RUN dotnet restore "ThaiDuongScadaHost/ThaiDuongScada.Host.csproj"
COPY . .
WORKDIR "/src/ThaiDuongScadaHost"
RUN dotnet build "ThaiDuongScada.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThaiDuongScada.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThaiDuongScada.Host.dll"]