#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["SmsService/SmsService.csproj", "SmsService/"]
COPY ["Tgs.WebApi.Toolkits/Tgs.WebApi.Toolkits.csproj", "Tgs.WebApi.Toolkits/"]
COPY ["SmsService.Core/SmsService.Core.csproj", "SmsService.Core/"]
COPY ["SmsService.Repositories/SmsService.Repositories.csproj", "SmsService.Repositories/"]
RUN dotnet restore "SmsService/SmsService.csproj"
COPY . .
WORKDIR "/src/SmsService"
RUN dotnet build "SmsService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "SmsService.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TGS.SmsService.dll"]