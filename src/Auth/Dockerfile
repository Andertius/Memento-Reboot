FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /app

COPY Memento.Auth/Memento.Auth.csproj Memento.Auth/
RUN dotnet restore './Memento.Auth/Memento.Auth.csproj' --arch $TARGETARCH

COPY . .
RUN dotnet build './Memento.Auth/Memento.Auth.csproj' -c $BUILD_CONFIGURATION -o /app/build --arch $TARGETARCH

FROM build AS publish
RUN dotnet publish './Memento.Auth/Memento.Auth.csproj' -c $BUILD_CONFIGURATION -o /app/publish --arch $TARGETARCH

FROM mcr.microsoft.com/dotnet/aspnet:10.0
EXPOSE 5091
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Memento.Auth.dll" ]
