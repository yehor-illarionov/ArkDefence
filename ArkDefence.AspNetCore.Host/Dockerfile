FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ArkDefence.AspNetCore.Host/ArkDefence.AspNetCore.Host.csproj", "ArkDefence.AspNetCore.Host/"]
RUN dotnet restore "ArkDefence.AspNetCore.Host/ArkDefence.AspNetCore.Host.csproj"
COPY . .
WORKDIR "/src/ArkDefence.AspNetCore.Host"
RUN dotnet build "ArkDefence.AspNetCore.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ArkDefence.AspNetCore.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ArkDefence.AspNetCore.Host.dll"]