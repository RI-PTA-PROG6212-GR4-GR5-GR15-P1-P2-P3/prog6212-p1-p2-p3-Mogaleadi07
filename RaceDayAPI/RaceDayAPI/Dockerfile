# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDayAPI/RaceDayAPI.csproj", "RaceDayAPI/"]
RUN dotnet restore "RaceDayAPI/RaceDayAPI.csproj"
COPY . .
WORKDIR "/src/RaceDayAPI"
RUN dotnet build "RaceDayAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaceDayAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayAPI.dll"]