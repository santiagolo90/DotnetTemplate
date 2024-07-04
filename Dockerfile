FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copia los archivos .csproj y restaura las dependencias
COPY DotnetTemplate/*.csproj DotnetTemplate/
COPY DotnetTemplate.Application/*.csproj DotnetTemplate.Application/
COPY DotnetTemplate.Infraestructure/*.csproj DotnetTemplate.Infraestructure/
COPY DotnetTemplate.EntityFramework.Shared/*.csproj DotnetTemplate.EntityFramework.Shared/

RUN dotnet restore DotnetTemplate/DotnetTemplate.csproj

# Copia el resto de los archivos y construye el proyecto
COPY . .

WORKDIR /src/DotnetTemplate
RUN dotnet build "DotnetTemplate.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "./DotnetTemplate.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "DotnetTemplate.dll"]