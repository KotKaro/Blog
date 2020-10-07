#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["KKotBlogApi/Blog.API.csproj", "KKotBlogApi/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
COPY ["Blog.DataAccess/Blog.DataAccess.csproj", "Blog.DataAccess/"]
COPY ["Blog.Auth/Blog.Auth.csproj", "Blog.Auth/"]
RUN dotnet restore "KKotBlogApi/Blog.API.csproj"
COPY . .
WORKDIR "/src/KKotBlogApi"
RUN dotnet build "Blog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.API.dll"]
