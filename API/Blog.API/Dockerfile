#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS build
WORKDIR /src

COPY ["API/Blog.API/Blog.API.csproj", "API/Blog.API/"]
COPY ["API/Blog.API/StaticFiles", "API/Blog.API/StaticFiles/"]
COPY ["API/Blog.Application/Blog.Application.csproj", "API/Blog.Application/"]
COPY ["API/Blog.Domain/Blog.Domain.csproj", "API/Blog.Domain/"]
COPY ["API/Blog.Infrastructure/Blog.Infrastructure.csproj", "API/Blog.Infrastructure/"]
COPY ["API/Blog.Infrastructure.DataAccess/Blog.Infrastructure.DataAccess.csproj", "API/Blog.Infrastructure.DataAccess/"]
COPY ["API/Blog.Infrastructure.Auth/Blog.Infrastructure.Auth.csproj", "API/Blog.Infrastructure.Auth/"]
COPY ["API/Blog.Infrastructure.Auth.Abstractions/Blog.Infrastructure.Auth.Abstractions.csproj", "API/Blog.Infrastructure.Auth.Abstractions/"]
RUN dotnet restore "API/Blog.API/Blog.API.csproj" --no-cache
COPY . .
WORKDIR "/src/API/Blog.API"

FROM build AS publish
RUN dotnet publish "Blog.API.csproj" -c Release -o /app/publish --no-restore

#RUN dotnet dev-certs https --clean

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.API.dll"]