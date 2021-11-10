#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY IIOT.sln ./
COPY IIOT.MessageConsumer.Data/IIOT.MessageConsumer.Data.csproj ./IIOT.MessageConsumer.Data/
COPY IIOT.MessageBroker/IIOT.MessageBroker.csproj ./IIOT.MessageBroker/
COPY IIOT.Simulator/IIOT.Simulator.csproj ./IIOT.Simulator/

RUN dotnet restore
COPY . .
WORKDIR /src/IIOT.MessageConsumer.Data
RUN dotnet build -c Release -o /app

WORKDIR /src/IIOT.MessageBroker
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebAPIProject.dll"]