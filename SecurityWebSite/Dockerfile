#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root

RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get install -y ffmpeg

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SecurityWebSite.csproj", "."]
RUN dotnet restore "./././SecurityWebSite.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./SecurityWebSite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SecurityWebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN mkdir /app/Thumbnails

RUN mkdir /app/Pages
COPY ./Pages/* /app/Pages

RUN mkdir /app/Styles
COPY ./Styles/* /app/Styles

RUN mkdir /app/Scripts
COPY ./Scripts/* /app/Scripts

RUN mkdir cert
RUN chmod 777 ./cert

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecurityWebSite.dll"]