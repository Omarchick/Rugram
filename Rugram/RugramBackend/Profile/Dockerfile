FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Profile/Profile.csproj", "Profile/"]
RUN dotnet restore "Profile/Profile.csproj"
COPY . .
WORKDIR "/src/Profile"
RUN dotnet build "Profile.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Profile.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.dll"]
