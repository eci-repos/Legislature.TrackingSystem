FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["global.json", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["src/Legislature.TrackingSystem.Domain/Legislature.TrackingSystem.Domain.csproj", "src/Legislature.TrackingSystem.Domain/"]
COPY ["src/Legislature.TrackingSystem.Application/Legislature.TrackingSystem.Application.csproj", "src/Legislature.TrackingSystem.Application/"]
COPY ["src/Legislature.TrackingSystem.Infrastructure/Legislature.TrackingSystem.Infrastructure.csproj", "src/Legislature.TrackingSystem.Infrastructure/"]
COPY ["src/Legislature.TrackingSystem.Web.Client/Legislature.TrackingSystem.Web.Client.csproj", "src/Legislature.TrackingSystem.Web.Client/"]
COPY ["src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj", "src/Legislature.TrackingSystem.Web/"]

RUN dotnet restore "src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj"

COPY . .

# Do NOT pass --no-restore here. The prior restore runs with only the .csproj files present
# (before COPY . .), so --no-restore would reuse that partial restore and skip regenerating the
# Microsoft.AspNetCore.App.Internal.Assets static web assets (_framework/blazor.web.js and
# _framework/blazor.server.js). Without blazor.web.js the served page cannot boot the interactive
# WebAssembly client. Re-running restore here (packages are cached in the layer above) emits them.
RUN dotnet publish "src/Legislature.TrackingSystem.Web/Legislature.TrackingSystem.Web.csproj" \
    --configuration Release \
    --output /app/publish

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .

# OCI image metadata for provenance and registry tooling.
LABEL org.opencontainers.image.title="Legislature.TrackingSystem.Web" \
      org.opencontainers.image.description="DOR Legislative Tracking System web application" \
      org.opencontainers.image.source="https://github.com/legislature-tracking-system/Legislature.TrackingSystem" \
      org.opencontainers.image.licenses="Proprietary"

# Install curl for the container healthcheck, then drop to the non-root 'app' user (built into the
# aspnet image) for least privilege. The app never runs as root.
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
USER app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "Legislature.TrackingSystem.Web.dll"]
