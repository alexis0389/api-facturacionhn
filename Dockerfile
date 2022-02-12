FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Facturacion.csproj", "Facturacion/"]
RUN dotnet restore "Facturacion/Facturacion.csproj"
WORKDIR "/src/Facturacion"
COPY . .
RUN dotnet build "Facturacion.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Facturacion.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./firebase/facturacion-a4770-firebase-adminsdk-hq2h7-ddb5dfdcd0.json .
CMD dotnet /app/Facturacion.dll