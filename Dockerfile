# Giai đoạn 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file csproj và khôi phục thư viện
# Dấu * đại diện cho tên file, giúp tránh lỗi gõ sai tên dự án phức tạp
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source code và build bản Release
COPY . ./
RUN dotnet publish -c Release -o out

# Giai đoạn 2: Chạy ứng dụng (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Cấu hình cổng cho Render (Render mặc định check cổng 10000 nhưng .NET thường dùng 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# --- QUAN TRỌNG: KIỂM TRA TÊN FILE DLL ---
# Mặc định Visual Studio giữ nguyên tên file là "ASM_C#4.dll".
# Nhưng nếu deploy bị lỗi, hãy thử đổi thành "ASM_C_4.dll"
# Sửa dòng cuối cùng này:
ENTRYPOINT ["dotnet", "ASM_C_4.dll"]