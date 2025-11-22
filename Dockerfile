FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj và restore
COPY *.csproj ./
RUN dotnet restore

# Copy code và build
COPY . ./
RUN dotnet publish -c Release -o out

# --- BƯỚC MỚI: TỰ ĐỘNG ĐỔI TÊN FILE DLL ---
# Tìm bất kỳ file .dll nào trong thư mục out trùng tên project và đổi thành app.dll
# Cách này xử lý được cả tên có dấu # hay tên cũ/mới
WORKDIR /app/out
RUN mv *.dll app.dll || true

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Bây giờ chúng ta luôn chạy file tên là app.dll
ENTRYPOINT ["dotnet", "app.dll"]