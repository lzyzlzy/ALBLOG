FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ALBLOG/ALBLOG.Web.csproj", "ALBLOG/"]
COPY ["ALBLOG.Domain.Service/ALBLOG.Domain.Service.csproj", "ALBLOG.Domain.Service/"]
COPY ["ALBLOG.Domain.Repository/ALBLOG.Domain.Repository.csproj", "ALBLOG.Domain.Repository/"]
COPY ["ALBLOG.Domain.Model/ALBLOG.Domain.Model.csproj", "ALBLOG.Domain.Model/"]
COPY ["Common.MongoDBClient/Common.MongoDBClient.csproj", "Common.MongoDBClient/"]
COPY ["Common.Utility/Common.Utility.csproj", "Common.Utility/"]
COPY ["Constant/ALBLOG.Constant.csproj", "Constant/"]
COPY ["ALBLOG.Domain.Dto/ALBLOG.Domain.Dto.csproj", "ALBLOG.Domain.Dto/"]
RUN dotnet restore "ALBLOG/ALBLOG.Web.csproj"
COPY . .
WORKDIR "/src/ALBLOG"
RUN dotnet build "ALBLOG.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ALBLOG.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ALBLOG.Web.dll"]