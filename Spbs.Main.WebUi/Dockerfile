FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Spbs.Main.WebUi/Spbs.Main.WebUi.csproj", "Spbs.Main.WebUi/"]
RUN dotnet restore "Spbs.Main.WebUi/Spbs.Main.WebUi.csproj"
COPY . .
WORKDIR "/src/Spbs.Main.WebUi"
RUN dotnet build "Spbs.Main.WebUi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spbs.Main.WebUi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spbs.Main.WebUi.dll"]
