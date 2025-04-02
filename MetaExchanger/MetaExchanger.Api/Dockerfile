FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MetaExchanger.Api/MetaExchanger.Api.csproj", "MetaExchanger.Api/"]
COPY ["MetaExchanger.Contracts/MetaExchanger.Contracts.csproj", "MetaExchanger.Contracts/"]
COPY ["MetaExchanger.Application/MetaExchanger.Application.csproj", "MetaExchanger.Application/"]
RUN dotnet restore "./MetaExchanger.Api/MetaExchanger.Api.csproj"
COPY . .
WORKDIR "/src/MetaExchanger.Api"
RUN dotnet build "./MetaExchanger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MetaExchanger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY MetaExchanger.Application/Infrastructure/order_books_data.txt ./MetaExchanger.Application/Infrastructure/myfile.txt

EXPOSE 8080

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetaExchanger.Api.dll"]