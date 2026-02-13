FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Memento.API/Memento.API.csproj Memento.API/
COPY Memento.Domain/Memento.Domain.csproj Memento.Domain/
COPY Memento.Infrastructure/Memento.Infrastructure.csproj Memento.Infrastructure/
COPY Memento.Services/Memento.Services.csproj Memento.Services/
RUN dotnet restore 'Memento.API/Memento.API.csproj'

COPY . .
RUN dotnet build 'Memento.API/Memento.API.csproj' -c Release -o /app/build

FROM build AS publish
RUN dotnet publish 'Memento.API/Memento.API.csproj' -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
EXPOSE 5295
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Memento.API.dll" ]
