#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/DominosGeolocation.InsertMq/DominosGeolocation.InsertMq.csproj", "Services/DominosGeolocation.InsertMq/"]
COPY ["Logging/DominosGeolocation.Logging/DominosGeolocation.Logging.csproj", "Logging/DominosGeolocation.Logging/"]
COPY ["Data/DominosGeolocation.Data/DominosGeolocation.Data.csproj", "Data/DominosGeolocation.Data/"]
COPY ["Helper/DominosGeolocation.Helper/DominosGeolocation.Helper.csproj", "Helper/DominosGeolocation.Helper/"]
RUN dotnet restore "Services/DominosGeolocation.InsertMq/DominosGeolocation.InsertMq.csproj"
COPY . .
WORKDIR "/src/Services/DominosGeolocation.InsertMq"
RUN dotnet build "DominosGeolocation.InsertMq.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DominosGeolocation.InsertMq.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DominosGeolocation.InsertMq.dll"]