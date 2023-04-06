#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Api_For_Cosmo_DB/Api_For_Cosmo_DB.csproj", "Api_For_Cosmo_DB/"]
RUN dotnet restore "Api_For_Cosmo_DB/Api_For_Cosmo_DB.csproj"
COPY . .
WORKDIR "/src/Api_For_Cosmo_DB"
RUN dotnet build "Api_For_Cosmo_DB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api_For_Cosmo_DB.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api_For_Cosmo_DB.dll"]