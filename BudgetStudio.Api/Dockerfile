FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BudgetStudio.Api/BudgetStudio.Api.csproj", "BudgetStudio.Api/"]
RUN dotnet restore "BudgetStudio.Api/BudgetStudio.Api.csproj"
COPY . .
WORKDIR "/src/BudgetStudio.Api"
RUN dotnet build "BudgetStudio.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetStudio.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetStudio.Api.dll"]