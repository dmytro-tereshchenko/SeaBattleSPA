#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SeaBattle.AuthorizationService/SeaBattle.AuthorizationService.csproj", "SeaBattle.AuthorizationService/"]
RUN dotnet restore "SeaBattle.AuthorizationService/SeaBattle.AuthorizationService.csproj"
COPY . .
WORKDIR "/src/SeaBattle.AuthorizationService"
RUN dotnet build "SeaBattle.AuthorizationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeaBattle.AuthorizationService.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
EXPOSE 44715
EXPOSE 44367
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeaBattle.AuthorizationService.dll"]