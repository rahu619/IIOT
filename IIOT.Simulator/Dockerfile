#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IIOT.Simulator/IIOT.Simulator.csproj", "IIOT.Simulator/"]
RUN dotnet restore "IIOT.Simulator/IIOT.Simulator.csproj"
COPY . .
WORKDIR "/src/IIOT.Simulator"
RUN dotnet build "IIOT.Simulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IIOT.Simulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IIOT.Simulator.dll"]