FROM public.ecr.aws/lambda/dotnet:9 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /src
COPY ["API.Lambda.csproj", "API.Lambda/"]
RUN dotnet restore "API.Lambda/API.Lambda.csproj"

WORKDIR "/src/API.Lambda"
COPY . .
RUN dotnet build "API.Lambda.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "API.Lambda.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .

ENV ASPNETCORE_HTTP_PORTS=80

CMD [ "API.Lambda::API.Lambda.Function::FunctionHandler" ]
