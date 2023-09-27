FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

#COPY ./souschef.server.sln .
COPY ./souschef.server.csproj .
RUN dotnet restore 

# Copy everything from here into the /app on docker 
COPY . .
# Restore as distinct layers
# Build and publish a release
RUN dotnet publish "souschef.server.csproj" --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000
EXPOSE 5001
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "souschef.server.dll"]