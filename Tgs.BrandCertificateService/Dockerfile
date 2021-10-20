#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["Tgs.BrandCertificateService/Tgs.BrandCertificateService.csproj", "Tgs.BrandCertificateService/"]
COPY ["BrandCertificate.Core/BrandCertificate.Core.csproj", "BrandCertificate.Core/"]
COPY ["BrandCertificate.Repositories/BrandCertificate.Repositories.csproj", "BrandCertificate.Repositories/"]
RUN dotnet restore "Tgs.BrandCertificateService/Tgs.BrandCertificateService.csproj"
COPY . .
WORKDIR "/src/Tgs.BrandCertificateService"
RUN dotnet build "Tgs.BrandCertificateService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.BrandCertificateService.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.BrandCertificateService.dll"]