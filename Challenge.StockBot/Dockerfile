FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StockBot/StockBot.csproj", "StockBot/"]
RUN dotnet restore "StockBot/StockBot.csproj"
COPY . .
WORKDIR "/src/StockBot"
RUN dotnet build "StockBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockBot.dll"]
