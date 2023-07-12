#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5072

ENV ASPNETCORE_URLS=http://+:5072
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["01 - Presentation/API/WebAPI/WebAPI.csproj", "01 - Presentation/API/WebAPI/"]
COPY ["04 - Infrastructure/4.3 - CrossCutting/IOC/IOC.csproj", "04 - Infrastructure/4.3 - CrossCutting/IOC/"]
COPY ["02 - Application/Application.Services/Application.Services.csproj", "02 - Application/Application.Services/"]
COPY ["03 - Domain/Domain.Contract/Domain.Contract.csproj", "03 - Domain/Domain.Contract/"]
COPY ["02 - Application/Application/Application.csproj", "02 - Application/Application/"]
COPY ["04 - Infrastructure/4.1 - Data/Data.SQLServer/Data.SQLServer.csproj", "04 - Infrastructure/4.1 - Data/Data.SQLServer/"]
COPY ["03 - Domain/Domain.Core/Domain.Core.csproj", "03 - Domain/Domain.Core/"]
COPY ["04 - Infrastructure/4.1 - Data/Data.Repository/Data.Repository.csproj", "04 - Infrastructure/4.1 - Data/Data.Repository/"]
COPY ["04 - Infrastructure/4.2 - Messaging/Messaging.RabbitMQ/Messaging.RabbitMQ.csproj", "04 - Infrastructure/4.2 - Messaging/Messaging.RabbitMQ/"]
RUN dotnet restore "01 - Presentation/API/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/01 - Presentation/API/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
