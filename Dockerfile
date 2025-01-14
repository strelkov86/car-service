FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["SibintekTask/SibintekTask.API.csproj", "SibintekTask/"]
COPY ["SibintekTask.Application/SibintekTask.Application.csproj", "SibintekTask.Application/"]
COPY ["SibintekTask.Core/SibintekTask.Core.csproj", "SibintekTask.Core/"]
COPY ["SibintekTask.Persistence.EF/SibintekTask.Persistence.EF.csproj", "SibintekTask.Persistence.EF/"]
COPY ["SibintekTask.Infrastructure.Logging/SibintekTask.Infrastructure.Logging.csproj", "SibintekTask.Infrastructure.Logging/"]
RUN dotnet restore "SibintekTask/SibintekTask.API.csproj"

COPY . .

WORKDIR "/src/SibintekTask"
RUN dotnet build "SibintekTask.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SibintekTask.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SibintekTask.API.dll"]