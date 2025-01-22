FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src

COPY . .
RUN dotnet publish src/Sales -c $BUILD_CONFIGURATION -o /app/publish --arch $TARGETARCH /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sales.dll"]