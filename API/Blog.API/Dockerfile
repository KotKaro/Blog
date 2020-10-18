#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["API/Blog.API/Blog.API.csproj", "API/Blog.API/"]
COPY ["API/Blog.Auth.Abstractions/Blog.Auth.Abstractions.csproj", "API/Blog.Auth.Abstractions/"]
COPY ["API/Blog.Application/Blog.Application.csproj", "API/Blog.Application/"]
COPY ["API/Blog.Domain/Blog.Domain.csproj", "API/Blog.Domain/"]
COPY ["API/Blog.Infrastructure/Blog.Infrastructure.csproj", "API/Blog.Infrastructure/"]
COPY ["API/Blog.DataAccess/Blog.DataAccess.csproj", "API/Blog.DataAccess/"]
COPY ["API/Blog.Auth/Blog.Auth.csproj", "API/Blog.Auth/"]
RUN dotnet restore "API/Blog.API/Blog.API.csproj"
COPY . .
WORKDIR "/src/API/Blog.API"
RUN dotnet build "Blog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.API.csproj" -c Release -o /app/publish
COPY "**/Blog.API.xml" "/app"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.API.dll"]