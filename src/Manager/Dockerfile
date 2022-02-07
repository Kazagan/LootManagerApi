FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Manager/Manager.csproj", "Manager/"]
RUN dotnet restore "src/Manager/Manager.csproj"
COPY . .
WORKDIR "/src/Manager"
RUN dotnet build "Manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manager.dll"]
