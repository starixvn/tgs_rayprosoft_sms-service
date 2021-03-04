#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["SmsService/SmsService.csproj", "SmsService/"]
COPY ["SmsService.Core/SmsService.Core.csproj", "SmsService.Core/"]
COPY ["SmsSender/SmsSender.csproj", "SmsSender/"]
RUN dotnet restore "SmsService/SmsService.csproj"
COPY . .
WORKDIR "/src/SmsService"
RUN dotnet build "SmsService.csproj" -c ${BuildConfiguration} -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "SmsService.csproj" -c ${BuildConfiguration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmsService.dll"]