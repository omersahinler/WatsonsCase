FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80

COPY src/Domain/WatsonsCase.Domain.Core/*.csproj ./Domain/WatsonsCase.Domain.Core/
COPY src/Domain/WatsonsCase.Domain/*.csproj ./Domain/WatsonsCase.Domain/
COPY src/Infrastructure/WatsonsCase.Infrastructure/*.csproj ./Infrastructure/WatsonsCase.Infrastructure/
COPY src/Application/WatsonsCase.Application/*.csproj ./Application/WatsonsCase.Application/
COPY src/Presentation/WatsonsCase.Api/*.csproj ./Presentation/WatsonsCase.Api/

COPY ./*.sln .

RUN dotnet restore Presentation/WatsonsCase.Api/*.csproj
COPY . .
RUN dotnet publish src/Presentation/WatsonsCase.Api/*.csproj -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "WatsonsCase.Api.dll"]