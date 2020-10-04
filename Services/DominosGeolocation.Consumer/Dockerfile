#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/DominosGeolocation.Consumer/DominosGeolocation.Consumer.csproj", "Services/DominosGeolocation.Consumer/"]
COPY ["Data/DominosGeolocation.Data/DominosGeolocation.Data.csproj", "Data/DominosGeolocation.Data/"]
COPY ["Logging/DominosGeolocation.Logging/DominosGeolocation.Logging.csproj", "Logging/DominosGeolocation.Logging/"]
COPY ["Helper/DominosGeolocation.Helper/DominosGeolocation.Helper.csproj", "Helper/DominosGeolocation.Helper/"]
RUN dotnet restore "Services/DominosGeolocation.Consumer/DominosGeolocation.Consumer.csproj"
COPY . .
WORKDIR "/src/Services/DominosGeolocation.Consumer"
RUN dotnet build "DominosGeolocation.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DominosGeolocation.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DominosGeolocation.Consumer.dll"]