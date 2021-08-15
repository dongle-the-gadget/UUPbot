# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy everything and build
COPY . .
RUN dotnet restore /app/UnofficialUUPDumpBot/UnofficialUUPDumpBot.csproj
RUN dotnet publish /app/UnofficialUUPDumpBot/UnofficialUUPDumpBot.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "UnofficialUUPDumpBot.dll"]