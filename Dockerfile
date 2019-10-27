FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ["src/Runtime/FamilyGuy.csproj", "src/Runtime/"]
COPY ["src/Infrastructure/FamilyGuy.Infrastructure/FamilyGuy.Infrastructure.csproj", "src/Infrastructure/FamilyGuy.Infrastructure/"]
COPY ["src/Infrastructure/FamilyGuy.Persistence/FamilyGuy.Persistence.csproj", "src/Infrastructure/FamilyGuy.Persistence/"]
COPY ["src/Application/FamilyGuy.Accounts/FamilyGuy.Accounts.csproj", "src/Application/FamilyGuy.Accounts/"]
COPY ["src/Contracts/FamilyGuy.Contracts/FamilyGuy.Contracts.csproj", "src/Contracts/FamilyGuy.Contracts/"]
COPY ["src/API/FamilyGuy.UserApi/FamilyGuy.UserApi.csproj", "src/API/FamilyGuy.UserApi/"]
COPY ["src/Process/FamilyGuy.Processes/FamilyGuy.Processes.csproj", "src/Process/FamilyGuy.Processes/"]
RUN dotnet restore "src/Runtime/FamilyGuy.csproj" 

COPY . .

WORKDIR /app/src/Runtime
RUN dotnet build "FamilyGuy.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /family-guy
COPY ./docker/opt /
RUN chmod +x /entrypoint.sh

COPY --from=build /app/build ./
EXPOSE 80

ENTRYPOINT [ "/entrypoint.sh" ]

CMD ["dotnet", "./FamilyGuy.dll"]