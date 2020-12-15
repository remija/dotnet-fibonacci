FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build 
#mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /src 
COPY . . 
RUN dotnet publish "./src/Fibonacci.Web/Fibonacci.Web.csproj" -c Release -r linux-x64 /p:PublishSingleFile=true -o /publish 

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0 AS final 
#mcr.microsoft.com/dotnet/core/runtime-deps:3.1
WORKDIR /app 
COPY --from=build /publish . 
ENTRYPOINT ["/app/Fibonacci.Web"]
