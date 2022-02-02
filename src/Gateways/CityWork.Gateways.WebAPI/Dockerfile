#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateways/CityWork.Gateways.WebAPI/CityWork.Gateways.WebAPI.csproj", "src/Gateways/CityWork.Gateways.WebAPI/"]
RUN dotnet restore "src/Gateways/CityWork.Gateways.WebAPI/CityWork.Gateways.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Gateways/CityWork.Gateways.WebAPI"
RUN dotnet build "CityWork.Gateways.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityWork.Gateways.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CityWork.Gateways.WebAPI.dll"]