# Мультистейдж-сборка для микросервиса на .NET 9
ARG PROJECT=Microservice.Template.Web
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Копируем только csproj и выполняем restore отдельным слоем (для кэширования)
COPY ../src/${PROJECT}/${PROJECT}.csproj src/${PROJECT}/
COPY ../src/Microservice.Template.Core/Microservice.Template.Core.csproj src/Microservice.Template.Core/
COPY ../src/Microservice.Template.UseCases/Microservice.Template.UseCases.csproj src/Microservice.Template.UseCases/
COPY ../src/Microservice.Template.Infrastructure/Microservice.Template.Infrastructure.csproj src/Microservice.Template.Infrastructure/
COPY ../src/Microservice.Template.ServiceDefaults/Microservice.Template.ServiceDefaults.csproj src/Microservice.Template.ServiceDefaults/
RUN dotnet restore src/${PROJECT}/${PROJECT}.csproj
# Копируем остальной код и публикуем
COPY .. .
WORKDIR /src/src/${PROJECT}
RUN dotnet publish "${PROJECT}.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Проверка работоспособности контейнера
HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 CMD curl --fail http://localhost:80/health || exit 1
ENTRYPOINT ["dotnet", "Microservice.Template.Web.dll"] 