#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["SmsService/SmsService.csproj", "SmsService/"]
COPY ["SmsService.Core/SmsService.Core.csproj", "SmsService.Core/"]
COPY ["SmsService.Repositories/SmsService.Repositories.csproj", "SmsService.Repositories/"]
RUN dotnet restore "SmsService/SmsService.csproj"
COPY . .
WORKDIR "/src/SmsService"
RUN dotnet build "SmsService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "SmsService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TGS.SmsService.dll"]