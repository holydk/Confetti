FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

ARG NODE_VERSION
ARG NODE_DOWNLOAD_URL
ARG NODE_DOWNLOAD_SHA
RUN curl -SL "$NODE_DOWNLOAD_URL" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs \
    && node -v \
    && npm i --global gulp-cli \
    && gulp -v

COPY . .
WORKDIR /src/Services/Identity/Identity.Api/src/
RUN  npm i && dotnet restore "Identity.Api.csproj"

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/dist

FROM base AS final
COPY --from=publish /app/dist .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]