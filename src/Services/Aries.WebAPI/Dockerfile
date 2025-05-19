FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["aries.contabilidad.2.0.sln", "."]
COPY ["src/Core/AriesContador.Core/AriesContador.Core.csproj", "src/Core/AriesContador.Core/"]
COPY ["src/Core/AriesContador.Data/AriesContador.Data.csproj", "src/Core/AriesContador.Data/"]
COPY ["src/Services/Aries.WebAPI/Aries.WebAPI.csproj", "src/Services/Aries.WebAPI/"]
COPY ["src/UI/Aries.Contabilidad/Aries.Contabilidad.csproj", "src/UI/Aries.Contabilidad/"]

RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV ASPNETCORE_URLS="http://*:5000"
ENTRYPOINT ["dotnet", "Aries.WebAPI.dll"]