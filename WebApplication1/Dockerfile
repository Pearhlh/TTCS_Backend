#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApplication1/TourishApi.csproj", "WebApplication1/"]
RUN dotnet restore "WebApplication1/TourishApi.csproj"
COPY . .
WORKDIR "/src/WebApplication1"
RUN dotnet build "TourishApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TourishApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]