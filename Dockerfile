FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /.

COPY *.sln .
COPY src/Manager/*.csproj Manager/
COPY src/Data/*.csproj Data/
COPY test/ManagerTests/*.csproj ManagerTests/

COPY . .

RUN dotnet build -c release

FROM build AS publish
RUN dotnet publish -c release --no-build -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV AllowedHosts=*
EXPOSE 80

COPY --from=publish /out .
ENTRYPOINT ["dotnet", "Manager.dll"]
