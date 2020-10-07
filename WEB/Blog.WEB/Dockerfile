# escape=`
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs

WORKDIR /app
COPY WEB/Blog.WEB/ClientApp .
RUN npm install 
RUN npm run-script build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WEB/Blog.WEB/Blog.WEB.csproj", "WEB/Blog.WEB/"]
RUN dotnet restore "WEB/Blog.WEB/Blog.WEB.csproj"
COPY . .
WORKDIR "/src/WEB/Blog.WEB"
RUN dotnet build "Blog.WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.WEB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist ClientApp/dist
ENTRYPOINT ["dotnet", "Blog.WEB.csproj"]