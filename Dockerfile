FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Saltimer.Api/Saltimer.Api.csproj", "Saltimer.Api/"]
RUN dotnet restore "Saltimer.Api/Saltimer.Api.csproj"
COPY . .
WORKDIR "/src/Saltimer.Api"
RUN dotnet build "Saltimer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Saltimer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet API.dll