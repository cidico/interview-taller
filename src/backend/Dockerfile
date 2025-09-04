FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE ${PORT:-8080}

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Interview.BackendApi/Interview.BackendApi.csproj", "."]
RUN dotnet restore "Interview.BackendApi.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "Interview.BackendApi/Interview.BackendApi.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "Interview.BackendApi/Interview.BackendApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG ASPNETCORE_ENVIRONMENT=Development
ARG PORT
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Development}
ENV PORT=${PORT:-8080}
ENTRYPOINT ["dotnet", "Interview.BackendApi.dll"]