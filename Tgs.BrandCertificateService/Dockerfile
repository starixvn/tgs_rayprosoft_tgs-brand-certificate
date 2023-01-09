#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["Tgs.BrandCertificateService/Tgs.BrandCertificateService.csproj", "Tgs.BrandCertificateService/"]
COPY ["BrancCertificate.Cache/BrancCertificate.Cache.csproj", "BrancCertificate.Cache/"]
COPY ["BrandCertificate.Core/BrandCertificate.Core.csproj", "BrandCertificate.Core/"]
COPY ["BrandCertificate.Common/BrandCertificate.Common.csproj", "BrandCertificate.Common/"]
COPY ["BrandCertificate.Repositories/BrandCertificate.Repositories.csproj", "BrandCertificate.Repositories/"]
RUN dotnet restore "Tgs.BrandCertificateService/Tgs.BrandCertificateService.csproj"
COPY . .
WORKDIR "/src/Tgs.BrandCertificateService"
RUN dotnet build "Tgs.BrandCertificateService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.BrandCertificateService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.BrandCertificateService.dll"]