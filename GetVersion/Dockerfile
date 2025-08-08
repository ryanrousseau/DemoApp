FROM public.ecr.aws/lambda/dotnet:9 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /src
COPY ["GetVersion.csproj", "GetVersion/"]
RUN dotnet restore "GetVersion/GetVersion.csproj"

WORKDIR "/src/GetVersion"
COPY . .
RUN dotnet build "GetVersion.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "GetVersion.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .

ENV ASPNETCORE_HTTP_PORTS=80

CMD [ "GetVersion::GetVersion.Function::FunctionHandler" ]
