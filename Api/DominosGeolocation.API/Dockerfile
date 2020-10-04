#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Api/DominosGeolocation.API/DominosGeolocation.API.csproj", "Api/DominosGeolocation.API/"]
COPY ["Logging/DominosGeolocation.Logging/DominosGeolocation.Logging.csproj", "Logging/DominosGeolocation.Logging/"]
COPY ["Data/DominosGeolocation.Data/DominosGeolocation.Data.csproj", "Data/DominosGeolocation.Data/"]
COPY ["Helper/DominosGeolocation.Helper/DominosGeolocation.Helper.csproj", "Helper/DominosGeolocation.Helper/"]
RUN dotnet restore "Api/DominosGeolocation.API/DominosGeolocation.API.csproj"
COPY . .
WORKDIR "/src/Api/DominosGeolocation.API"
RUN dotnet build "DominosGeolocation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DominosGeolocation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DominosGeolocation.API.dll"]
