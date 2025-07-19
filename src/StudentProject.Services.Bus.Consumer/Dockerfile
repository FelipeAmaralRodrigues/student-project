# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudentProject.Services.Bus.Consumer/StudentProject.Services.Bus.Consumer.csproj", "StudentProject.Services.Bus.Consumer/"]
COPY ["StudentProject.Domain/StudentProject.Domain.csproj", "StudentProject.Domain/"]
COPY ["StudentProject.Infra.CrossCutting.Bus/StudentProject.Infra.CrossCutting.Bus.csproj", "StudentProject.Infra.CrossCutting.Bus/"]
COPY ["StudentProject.Infra.Data/StudentProject.Infra.Data.csproj", "StudentProject.Infra.Data/"]
RUN dotnet restore "StudentProject.Services.Bus.Consumer/StudentProject.Services.Bus.Consumer.csproj"
COPY . .
WORKDIR "/src/StudentProject.Services.Bus.Consumer"
RUN dotnet build "StudentProject.Services.Bus.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudentProject.Services.Bus.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentProject.Services.Bus.Consumer.dll"]