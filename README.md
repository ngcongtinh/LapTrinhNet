# HƯỚNG DẪN SỬ DỤNG HỆ THỐNG QUẢN LÝ QUÁN CAFE (CAFEMANAGEMENT)
*Phiên bản: 2.0 (Giao diện Coffee/Cream & CSDL Chuẩn hóa có Khóa ngoại)*

Chào mừng bạn đến với tài liệu hướng dẫn sử dụng phần mềm **CafeManagement**. Ứng dụng được xây dựng trên nền tảng **Windows Forms (.NET)** tích hợp hệ quản trị cơ sở dữ liệu **MySQL local**, mang lại trải nghiệm chuyên nghiệp, mượt mà và trực quan nhất cho việc vận hành quán cafe.

---

## MỤC LỤC
1. [Yêu Cầu Hệ Thống & Khởi Động](#1-yêu-cầu-hệ-thống--khởi-động)
2. [Hướng Dẫn Đăng Nhập](#2-hướng-dẫn-đăng-nhập)
3. [Sơ Đồ Tính Năng Trên Giao Diện](#3-sơ-đồ-tính-năng-trên-giao-diện)
4. [Hướng Dẫn Sử Dụng Chi Tiết](#4-hướng-dẫn-sử-dụng-chi-tiết)
   - [4.1 Bán hàng & Đặt món (Đặt Món View)](#41-bán-hàng--đặt-món-đặt-món-view)
   - [4.2 Quản lý Thực đơn (Thực Đơn View)](#42-quản-lý-thực-đơn-thực-đơn-view)
   - [4.3 Quản lý Hóa đơn & Báo cáo Doanh thu (Doanh Thu View)](#43-quản-lý-hóa-đơn--báo-cáo-doanh-thu-doanh-thu-view)
   - [4.4 Quản lý Nhân viên & Khách hàng](#44-quản-lý-nhân-viên--khách-hàng)
5. [Cấu Trúc Cơ Sở Dữ Liệu & Thiết Lập Liên Kết Khóa Ngoại](#5-cấu-trúc-cơ-sở-dữ-liệu--thiết-lập-liên-kết-khóa-ngoại)

---

## 1. YÊU CẦU HỆ THỐNG & KHỞI ĐỘNG

### A. Yêu cầu môi trường
* **Hệ điều hành**: Windows 10/11 hoặc Windows Server.
* **Bộ phát triển**: `.NET SDK 6.0` trở lên.
* **Cơ sở dữ liệu**: MySQL Server (phiên bản 5.7 hoặc 8.0 trở lên).

### B. Thiết lập chuỗi kết nối (Connection String)
Tất cả thông tin kết nối CSDL được quản lý tập trung tại file cấu hình:
📁 `d:\DATASON\data son\VS Code\.NET\CafeManagement\LapTrinhNet\CafeManagement\appsettings.json`

Cấu trúc file như sau (kết nối đến máy cục bộ của bạn):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=quanlycuahang2;User=root;Password=070704;"
  }
}
```

### C. Lệnh khởi động dự án
Mở Terminal tại thư mục `d:\DATASON\data son\VS Code\.NET\CafeManagement\LapTrinhNet\CafeManagement` và chạy lệnh:
```bash
dotnet run
```
> [!TIP]
> Ứng dụng được lập trình tự động hóa cực kỳ thông minh: Trong lần đầu tiên kết nối, hệ thống sẽ **tự động kiểm tra và tạo mới database `quanlycuahang2`**, đồng thời tạo đầy đủ 6 bảng chuẩn hóa và nạp dữ liệu mẫu ban đầu mà bạn không cần phải import file `.sql` thủ công!

---

## 2. HƯỚNG DẪN ĐĂNG NHẬP

Khi ứng dụng khởi động thành công, màn hình Đăng nhập sẽ hiển thị:

* **Tài khoản mặc định (Quyền Admin)**: 
  * **Tên đăng nhập**: `admin`
  * **Mật khẩu**: `123`
* Nhập đúng thông tin và bấm **Đăng nhập** để vào giao diện quản trị chính.

---

## 3. SƠ ĐỒ TÍNH NĂNG TRÊN GIAO DIỆN

Giao diện chính được thiết kế đồng bộ theo tông màu **Coffee & Cream (Nâu & Kem)** hiện đại, sang trọng với thanh menu điều hướng nằm bên trái:

1. **Trang Chủ (Dashboard)**: Nơi hiển thị logo, lời chào và trạng thái hoạt động của nhân viên đang đăng nhập.
2. **Đặt Món (POS Bán Hàng)**: Màn hình thao tác chính của nhân viên phục vụ để tạo order, tính tiền và in hóa đơn.
3. **Thực Đơn**: Quản lý danh mục, các món ăn/thức uống có trong quán.
4. **Hóa Đơn & Doanh Thu**: Xem lịch sử hóa đơn bán hàng và biểu đồ/bảng thống kê doanh số.
5. **Nhân Viên**: Quản lý danh sách nhân sự của quán và phân quyền truy cập.
6. **Khách Hàng**: Quản lý thông tin thành viên thân thiết để tích điểm hoặc áp dụng ưu đãi.

---

## 4. HƯỚNG DẪN SỬ DỤNG CHI TIẾT

### 4.1 Bán hàng & Đặt món (Đặt Món View)
Đây là màn hình hoạt động nhiều nhất của nhân viên thu ngân:
1. **Chọn Danh mục**: Phía trên cùng có bộ lọc nhanh theo Danh mục (ví dụ: *Nước*, *Đồ ăn*).
2. **Chọn Món**: Click vào thẻ món ăn (có hình ảnh minh họa sống động, tên món và đơn giá). Món ăn sẽ tự động được thêm vào giỏ hàng bên phải.
3. **Điều chỉnh số lượng**: Tại bảng giỏ hàng, bạn có thể chỉnh sửa số lượng tăng/giảm trực tiếp.
4. **Chọn Khách hàng**: 
   * Nếu là khách vãng lai, hệ thống mặc định chọn trống (không lưu thông tin cá nhân).
   * Nếu là khách hàng thành viên, chọn từ ComboBox Khách hàng để hưởng ưu đãi thành viên.
5. **Thanh toán**: Bấm **Thanh toán** để hoàn tất:
   * Hệ thống tự động tính tổng tiền.
   * Lưu hóa đơn vào bảng `HoaDon` và chi tiết các món vào `ChiTietHoaDon`.
   * Ghi nhận mã nhân viên (`MaNV`) thực hiện giao dịch dựa trên phiên làm việc (`Session`).

---

### 4.2 Quản lý Thực đơn (Thực Đơn View)
Hỗ trợ Admin cập nhật menu của quán linh hoạt:
* **Xem danh sách**: Menu hiển thị dạng danh sách lưới chuyên nghiệp gồm ảnh minh họa, mã món, tên món, đơn giá, đơn vị tính và tên danh mục liên kết.
* **Thêm món mới**:
  1. Nhập Mã món (`MaMon` - khóa chính, không trùng lặp) và Tên món.
  2. Nhập đơn giá và đơn vị tính (ví dụ: *Ly*, *Đĩa*).
  3. Chọn Danh mục tương ứng (ví dụ: *Nuoc* hoặc *Do an*).
  4. Nhập đường dẫn hình ảnh (Hỗ trợ URL trực tuyến hoặc đường dẫn ảnh cục bộ).
  5. Bấm **Thêm**.
* **Sửa / Xóa**: Chọn món cần chỉnh sửa trên GridView, thay đổi thông tin và bấm **Cập nhật** (hoặc **Xóa** nếu không còn phục vụ món này).
> [!IMPORTANT]
> Toàn bộ các thao tác Thêm/Sửa thực đơn đều được đồng bộ khóa ngoại an toàn dưới cơ sở dữ liệu bằng các câu lệnh liên kết ID tự động, tránh hiện tượng lỗi logic dữ liệu.

---

### 4.3 Quản lý Hóa đơn & Báo cáo Doanh thu (Doanh Thu View)
* **Truy xuất hóa đơn**: Xem toàn bộ lịch sử giao dịch bán hàng của quán với chi tiết ngày giờ lập, nhân viên thực hiện, khách hàng mua và tổng tiền.
* **Xem chi tiết**: Click vào một hóa đơn bất kỳ, bảng chi tiết bên cạnh sẽ hiển thị rõ ràng danh sách món ăn, số lượng và thành tiền của hóa đơn đó.
* **Thống kê Doanh thu**:
  1. Chọn **Khoảng thời gian** (Từ ngày -> Đến ngày) cần thống kê.
  2. Hệ thống sẽ truy vấn dữ liệu từ MySQL, hiển thị tổng doanh thu đạt được và vẽ biểu đồ tăng trưởng trực quan để giúp bạn đánh giá hiệu quả kinh doanh.

---

### 4.4 Quản lý Nhân viên & Khách hàng
* **Nhân viên**: Thêm mới nhân sự, gán mã nhân viên (`MaNV`), họ tên, số điện thoại, chức vụ. Thông tin này được liên kết trực tiếp với tài khoản đăng nhập hệ thống.
* **Khách hàng**: Lưu trữ thông tin thành viên (Mã KH, Tên KH, Số điện thoại, Điểm tích lũy). Giúp xây dựng các chương trình chăm sóc khách hàng chuyên nghiệp.

---

## 5. CẤU TRÚC CƠ SỞ DỮ LIỆU & THIẾT LẬP LIÊN KẾT KHÓA NGOẠI

Ứng dụng sử dụng cấu hình **CSDL Chuẩn hóa 3NF** cực kỳ tối ưu, loại bỏ hoàn toàn việc lưu trữ chữ thuần túy (string) không liên kết của phiên bản cũ. Gồm 6 bảng chính sau:

```mermaid
erDiagram
    NhanVien ||--o{ HoaDon : "lap"
    KhachHang ||--o{ HoaDon : "thuoc"
    DanhMuc ||--o{ ThucDon : "chua"
    HoaDon ||--{ ChiTietHoaDon : "co"
    ThucDon ||--{ ChiTietHoaDon : "nam_trong"

    NhanVien {
        string MaNV PK
        string TenNV
        string ChucVu
    }
    KhachHang {
        string MaKH PK
        string TenKH
        string SoDienThoai
    }
    DanhMuc {
        string MaDanhMuc PK
        string TenDanhMuc
    }
    ThucDon {
        string MaMon PK
        string TenMon
        double DonGia
        string MaDanhMuc FK
    }
    HoaDon {
        string MaHD PK
        datetime NgayLap
        string MaNV FK
        string MaKH FK
    }
    ChiTietHoaDon {
        int ID PK
        string MaHD FK
        string MaMon FK
        int SoLuong
        double DonGia
    }
```

### Ưu điểm vượt trội của kiến trúc CSDL mới:
1. **Ràng buộc Khóa Ngoại cứng (`FOREIGN KEY`)**: Đảm bảo không bao giờ xảy ra lỗi dữ liệu mồ côi (ví dụ: Không thể lưu hóa đơn của một nhân viên không tồn tại, hoặc không thể xóa một danh mục đang có chứa các món ăn đang bán).
2. **Truy vấn JOIN tối ưu**: DAO layer áp dụng `LEFT JOIN` giúp kéo chính xác tên hiển thị lên giao diện cực kỳ nhanh chóng mà không làm tăng dung lượng lưu trữ CSDL.
3. **Tính bảo mật cao**: Che giấu toàn bộ thông tin nhạy cảm của hệ thống cơ sở dữ liệu trên môi trường log.

*Chúc bạn có những trải nghiệm tuyệt vời khi vận hành quán Cafe của mình với phần mềm **CafeManagement**!*
