# BÁO CÁO DỰ ÁN: XÂY DỰNG WEBSITE QUẢN LÝ BÁN THỨC ĂN NHANH
**Môn học:** Xây dựng ứng dụng C# ASP.NET Core MVC - Nâng cao
**Lớp:** SD19311
**Sinh viên thực hiện:** Phùng Xuân Dương
**MSSV:** PS38124

---

## MỤC LỤC
1. [Giới thiệu dự án](#1-giới-thiệu-dự-án)
2. [Phân tích yêu cầu khách hàng](#2-phân-tích-yêu-cầu-khách-hàng)
3. [Thiết kế ứng dụng](#3-thiết-kế-ứng-dụng)
4. [Thực hiện dự án](#4-thực-hiện-dự-án)
5. [Kiểm thử và Đóng gói](#5-kiểm-thử-phần-mềm-và-sửa-lỗi)
6. [Kết luận](#6-kết-luận)

---

## 1. Giới thiệu dự án

### 1.1. Tổng quan
Dự án xây dựng website quản lý và bán thức ăn nhanh, phục vụ nhu cầu đặt món trực tuyến của khách hàng và quản lý cửa hàng của nhân viên/admin.

### 1.2. Yêu cầu dự án
* **Đối với Admin:** Toàn quyền sử dụng phần mềm, quản lý nhân viên, thống kê.
* **Đối với Nhân viên:** Sử dụng các chức năng bán hàng, quản lý đơn hàng (trừ quản lý nhân viên).
* **Đối với Khách hàng:** Xem món ăn, đặt hàng, thanh toán.

### 1.3. Lập kế hoạch dự án

| TT | Hạng mục | Bắt đầu | Kết thúc | Kết quả |
|----|----------|---------|----------|---------|
| 1 | Phân tích yêu cầu | 05/11/2024 | 05/11/2024 | Hoàn thành |
| 1.1 | Vẽ sơ đồ Use Case | 08/11/2024 | 08/11/2024 | Hoàn thành |
| 2 | Thiết kế ứng dụng | 10/11/2024 | 10/11/2024 | Hoàn thành |
| 2.1 | Thiết kế CSDL & ERD | 11/11/2024 | 11/11/2024 | Hoàn thành |
| 3 | Thực hiện dự án (Code) | 12/11/2024 | 12/11/2024 | Hoàn thành |
| 3.1 | Tạo giao diện UI | 13/11/2024 | 13/11/2024 | Hoàn thành |
| 4 | Kiểm thử & Đóng gói | ... | ... | Đang thực hiện |

---

## 2. Phân tích yêu cầu khách hàng

### 2.1. Sơ đồ Use Case
Hệ thống phân quyền rõ ràng giữa Admin và Nhân viên/Khách hàng.

![Sơ đồ Use Case](LINK_ANH_USE_CASE_CUA_BAN)

### 2.2. Đặc tả yêu cầu hệ thống (SRS)

#### Các chức năng chính:
1.  **Đăng nhập/Đăng ký:** Xác thực người dùng, phân quyền truy cập.
2.  **Trang chủ:** Hiển thị danh sách món ăn, banner, thanh điều hướng.
3.  **Quản lý Món ăn:** Thêm, Xóa, Sửa, Xem chi tiết, Quản lý hình ảnh.
4.  **Quản lý Danh mục:** Phân loại món ăn (Gà rán, Đồ uống...).
5.  **Quản lý Hóa đơn:** Xem chi tiết đơn hàng, cập nhật trạng thái, lịch sử giao dịch.
6.  **Quản lý Nhân viên (Admin):** Thêm/Sửa/Xóa nhân viên, phân quyền.
7.  **Báo cáo thống kê (Admin):** Thống kê doanh thu, món bán chạy, xuất file báo cáo.
8.  **Giỏ hàng & Thanh toán:** Thêm món vào giỏ, tính tổng tiền, thanh toán online/tiền mặt.

### 2.3. Sơ đồ triển khai
Mô hình triển khai Client - Server:
* **Server:** Máy chủ chứa Database (SQL Server/PostgreSQL) và source code Backend.
* **Client:** Trình duyệt web của người dùng/nhân viên truy cập vào hệ thống.

![Sơ đồ triển khai](LINK_ANH_SO_DO_TRIEN_KHAI)

---

## 3. Thiết kế ứng dụng

### 3.1. Công nghệ sử dụng
* **Framework:** ASP.NET Core MVC (.NET 8.0)
* **Database:** SQL Server (Local) / PostgreSQL (Production)
* **ORM:** Entity Framework Core (Code First)
* **Công cụ:** Visual Studio, SSMS, Draw.io

### 3.2. Cơ sở dữ liệu (ERD)
Sơ đồ quan hệ thực thể của dự án bao gồm các bảng: `AspNetUsers`, `Products`, `Orders`, `OrderDetails`, `Categories`, `Brands`, `Combos`.

![Sơ đồ ERD](LINK_ANH_ERD_CUA_BAN)

#### Chi tiết thực thể User (AspNetUsers):
| Thuộc tính | Kiểu dữ liệu | Mô tả |
|------------|--------------|-------|
| Id | String (GUID) | Mã User |
| UserName | String | Tên đăng nhập |
| PasswordHash | String | Mật khẩu (đã mã hóa) |
| Email | String | Email người dùng |
| PhoneNumber | String | Số điện thoại |

### 3.3. Sơ đồ tổ chức giao diện
Hệ thống được tổ chức theo luồng: Đăng nhập -> Trang chủ -> Các trang chức năng (Quản lý, Bán hàng).

![Sơ đồ giao diện](LINK_ANH_SO_DO_GIAO_DIEN)

---

## 4. Thực hiện dự án

### 4.1. Giao diện người dùng (UI)

#### Trang chủ
Hiển thị banner quảng cáo, danh sách các món ăn nổi bật (Best Seller), danh mục sản phẩm.
![Giao diện Trang chủ](LINK_ANH_TRANG_CHU)

#### Trang danh sách món ăn (Admin)
Giao diện quản lý cho phép Admin xem danh sách dạng bảng, có nút Thêm, Sửa, Xóa.
![Quản lý món ăn](LINK_ANH_QUAN_LY_MON_AN)

#### Trang Giỏ hàng
Hiển thị các món đã chọn, cho phép tăng giảm số lượng hoặc xóa món.
![Giỏ hàng](LINK_ANH_GIO_HANG)

#### Trang Đăng nhập / Đăng ký
Giao diện xác thực người dùng.
![Đăng nhập](LINK_ANH_DANG_NHAP)

### 4.2. Xây dựng Cơ sở dữ liệu
Sử dụng kỹ thuật **Code First** trong Entity Framework Core để ánh xạ các Class C# (Model) thành bảng trong Database.
* Sử dụng `DbContext` để quản lý kết nối.
* Sử dụng `Migrations` để cập nhật thay đổi cấu trúc bảng.

---

## 5. Kiểm thử phần mềm và sửa lỗi
Đã thực hiện kiểm thử các chức năng:
* [x] Đăng ký / Đăng nhập / Đăng xuất.
* [x] Hiển thị sản phẩm trang chủ.
* [x] Thêm sản phẩm vào giỏ hàng.
* [x] Đặt hàng và lưu vào CSDL.
* [x] Gửi email xác nhận đơn hàng.
* [x] Quản lý thêm/sửa/xóa sản phẩm (Admin).

---

## 6. Kết luận

### 6.1. Kết quả đạt được (Thuận lợi)
* Hoàn thành các chức năng cốt lõi của một website bán hàng.
* Giao diện thân thiện, dễ sử dụng.
* Áp dụng thành công công nghệ ASP.NET Core MVC hiện đại.
* Triển khai thành công lên môi trường Internet (Render & Supabase).

### 6.2. Khó khăn & Hướng phát triển
* **Khó khăn:** Gặp một số vấn đề về cấu hình Deploy (Docker, IPv6 Mail), xử lý bất đồng bộ khi gửi mail.
* **Hướng phát triển:** Tích hợp thanh toán Online (Momo/VNPAY), thêm chức năng chat trực tuyến hỗ trợ khách hàng.

---
© 2024 Phùng Xuân Dương - PS38124
