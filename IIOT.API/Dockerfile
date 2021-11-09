#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IIOT.API/IIOT.API.csproj", "IIOT.API/"]
COPY ["IIOT.MessageConsumer/IIOT.MessageConsumer.csproj", "IIOT.MessageConsumer/"]
COPY ["IIOT.MessageConsumer.Data/IIOT.MessageConsumer.Data.csproj", "IIOT.MessageConsumer.Data/"]
COPY ["IIOT.MessageConsumer.Service/IIOT.MessageConsumer.Service.csproj", "IIOT.MessageConsumer.Service/"]
RUN dotnet restore "IIOT.API/IIOT.API.csproj"
COPY . .
WORKDIR "/src/IIOT.API"
RUN dotnet build "IIOT.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IIOT.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IIOT.API.dll"]