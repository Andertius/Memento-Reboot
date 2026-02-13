FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Memento.Auth/Memento.Auth.csproj Memento.Auth/
RUN dotnet restore './Memento.Auth/Memento.Auth.csproj'

COPY . .
RUN dotnet build './Memento.Auth/Memento.Auth.csproj' -c Release -o /app/build

FROM build AS publish
RUN dotnet publish './Memento.Auth/Memento.Auth.csproj' -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
EXPOSE 5091
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Memento.Auth.dll" ]
