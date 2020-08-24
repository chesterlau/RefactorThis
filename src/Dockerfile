FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /worksrc
COPY ["src/ProductManagement.API/ProductManagement.API.csproj", "worksrc/ProductManagement.API/"]
RUN dotnet restore "worksrc/ProductManagement.API/ProductManagement.API.csproj"
COPY . .
WORKDIR "/worksrc/src"
RUN dotnet build "ProductManagement.API/ProductManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductManagement.API/ProductManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManagement.API.dll"]
