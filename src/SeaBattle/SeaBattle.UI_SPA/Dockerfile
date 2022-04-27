FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y build-essential nodejs

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src

RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y build-essential nodejs

COPY ["SeaBattle.UI_SPA/SeaBattle.UI_SPA.csproj", "SeaBattle.UI_SPA/"]
RUN dotnet restore "SeaBattle.UI_SPA/SeaBattle.UI_SPA.csproj"
COPY . .

WORKDIR "/src/SeaBattle.UI_SPA"
RUN dotnet build "SeaBattle.UI_SPA.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "SeaBattle.UI_SPA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeaBattle.UI_SPA.dll"]
