FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["PokemonApi/PokemonApi.csproj", "PokemonApi/"]
RUN dotnet restore "PokemonApi/PokemonApi.csproj"
COPY . .
WORKDIR "/src/PokemonApi"
RUN dotnet build "PokemonApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokemonApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonApi.dll"]