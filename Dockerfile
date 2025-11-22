# Giai đoạn Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj và restore
COPY *.csproj ./
RUN dotnet restore

# Build code
COPY . ./
RUN dotnet publish -c Release -o out

# Giai đoạn Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Chạy file đúng tên đã đổi trong Properties
ENTRYPOINT ["dotnet", "ASM_C_4.dll"]