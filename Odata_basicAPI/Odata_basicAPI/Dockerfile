#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Odata_basicAPI/Odata_basicAPI.csproj", "Odata_basicAPI/"]
RUN dotnet restore "Odata_basicAPI/Odata_basicAPI.csproj"
COPY . .
WORKDIR "/src/Odata_basicAPI"
RUN dotnet build "Odata_basicAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Odata_basicAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odata_basicAPI.dll"]