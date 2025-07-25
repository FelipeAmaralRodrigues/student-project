# Acesse https://aka.ms/customizecontainer para saber como personalizar seu cont�iner de depura��o e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depura��o mais r�pida.

# Esta fase � usada durante a execu��o no VS no modo r�pido (Padr�o para a configura��o de Depura��o)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# Esta fase � usada para compilar o projeto de servi�o
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

# Esta fase � usada para publicar o projeto de servi�o a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudentProject.Services.Bus.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase � usada na produ��o ou quando executada no VS no modo normal (padr�o quando n�o est� usando a configura��o de Depura��o)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentProject.Services.Bus.Consumer.dll"]