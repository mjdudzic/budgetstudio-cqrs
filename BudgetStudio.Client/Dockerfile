FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BudgetStudio.Client/BudgetStudio.Client.csproj", "BudgetStudio.Client/"]
RUN dotnet restore "BudgetStudio.Client/BudgetStudio.Client.csproj"
COPY . .
WORKDIR "/src/BudgetStudio.Client"
RUN dotnet build "BudgetStudio.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetStudio.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetStudio.Client.dll"]