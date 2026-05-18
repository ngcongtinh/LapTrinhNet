using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CafeManagement.CH.dao
{
    public class DBConnection
    {
        private static IConfigurationRoot _configuration;
        private static string _connectionString;

        static DBConnection()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _configuration = builder.Build();
                _connectionString = _configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine(">>> Loaded Connection String: " + System.Text.RegularExpressions.Regex.Replace(_connectionString, "Password=[^;]+", "Password=******", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Error loading appsettings.json: " + ex.Message);
            }
        }

        // Kết nối Database
        private static string DB_CONNECTION => _connectionString;

        // =======================================================
        // 1. KẾT NỐI DATABASE
        // =======================================================
        public static MySqlConnection GetConnection()
        {
            MySqlConnection cons = null;

            try
            {
                cons = new MySqlConnection(DB_CONNECTION);
                cons.Open();
                // Console.WriteLine(">>> Database Connected Successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(">>> Database Connection Failed: " + e.Message);
            }

            return cons;
        }

        // =======================================================
        // 2. KHỞI TẠO DATABASE
        // =======================================================
        public static void InitializeDatabase()
        {
            Console.WriteLine("--- Bat dau khoi tao Database ---");
            try
            {
                // =======================================================
                // 1. KẾT NỐI DATABASE VÀ TỰ ĐỘNG TẠO DB NẾU CHƯA CÓ
                // =======================================================
                var connStringBuilder = new MySqlConnectionStringBuilder(DB_CONNECTION);
                string dbName = connStringBuilder.Database;
                
                // Tạo kết nối tạm không chỉ định DB để tạo DB nếu chưa tồn tại
                connStringBuilder.Database = "";
                using (var tempConn = new MySqlConnection(connStringBuilder.ConnectionString))
                {
                    tempConn.Open();
                    using (var createCmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {dbName}", tempConn))
                    {
                        createCmd.ExecuteNonQuery();
                    }
                }

                MySqlConnection dbConn = new MySqlConnection(DB_CONNECTION);
                dbConn.Open();
                Console.WriteLine("Ket noi thanh cong den MySQL.");

                // DROP TABLE IF EXISTS to force schema migration to normalized version
                // string dropSql = "DROP TABLE IF EXISTS ChiTietHoaDon, HoaDon, ThucDon, DanhMuc, Kho, KhachHang, NhanVien;";
                // using (var dropCmd = new MySqlCommand(dropSql, dbConn))
                // {
                //     dropCmd.ExecuteNonQuery();
                //     Console.WriteLine(">>> Dropped old tables to upgrade to normalized schema with Foreign Keys.");
                // }

                // Clean up warehouse table (Kho) as requested
                using (var dropCmd = new MySqlCommand("DROP TABLE IF EXISTS Kho;", dbConn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                MySqlCommand dbCmd;

                // =======================================================
                // TẠO BẢNG NHANVIEN
                // =======================================================

                string sqlNhanVien = @"
                CREATE TABLE IF NOT EXISTS NhanVien (
                    MaNV VARCHAR(20) PRIMARY KEY,
                    TenNV VARCHAR(100),
                    NgaySinh VARCHAR(20),
                    GioiTinh VARCHAR(10),
                    ChucVu VARCHAR(50),
                    SoDienThoai VARCHAR(15),
                    DiaChi VARCHAR(255),
                    Username VARCHAR(50),
                    Password VARCHAR(50),
                    Role VARCHAR(20)
                )";

                dbCmd = new MySqlCommand(sqlNhanVien, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG KHACHHANG
                // =======================================================

                string sqlKhachHang = @"
                CREATE TABLE IF NOT EXISTS KhachHang (
                    MaKH VARCHAR(20) PRIMARY KEY,
                    TenKH VARCHAR(100),
                    TheLoai VARCHAR(20),
                    GioiTinh VARCHAR(10),
                    Email VARCHAR(100),
                    SoDienThoai VARCHAR(15),
                    DiaChi VARCHAR(255)
                )";

                dbCmd = new MySqlCommand(sqlKhachHang, dbConn);
                dbCmd.ExecuteNonQuery();



                // =======================================================
                // TẠO BẢNG DANHMUC
                // =======================================================

                string sqlDanhMuc = @"
                CREATE TABLE IF NOT EXISTS DanhMuc (
                    MaDanhMuc VARCHAR(20) PRIMARY KEY,
                    TenDanhMuc VARCHAR(100)
                )";

                dbCmd = new MySqlCommand(sqlDanhMuc, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG THUCDON (Liên kết Khóa Ngoại tới DanhMuc)
                // =======================================================

                string sqlThucDon = @"
                CREATE TABLE IF NOT EXISTS ThucDon (
                    MaMon VARCHAR(20) PRIMARY KEY,
                    TenMon VARCHAR(100),
                    DonGia DOUBLE,
                    DonViTinh VARCHAR(20),
                    HinhAnh VARCHAR(255),
                    MaDanhMuc VARCHAR(20),
                    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc) ON DELETE SET NULL
                )";

                dbCmd = new MySqlCommand(sqlThucDon, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG HOADON (Liên kết Khóa Ngoại tới NhanVien và KhachHang)
                // =======================================================

                string sqlHoaDon = @"
                CREATE TABLE IF NOT EXISTS HoaDon (
                    MaHD VARCHAR(20) PRIMARY KEY,
                    MaNV VARCHAR(20),
                    MaKH VARCHAR(20),
                    NgayLap VARCHAR(20),
                    TongTien DOUBLE,
                    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL,
                    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE SET NULL
                )";

                dbCmd = new MySqlCommand(sqlHoaDon, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG CHITIETHOADON (Liên kết Khóa Ngoại tới HoaDon và ThucDon)
                // =======================================================

                string sqlCTHD = @"
                CREATE TABLE IF NOT EXISTS ChiTietHoaDon (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    MaHD VARCHAR(20),
                    MaMon VARCHAR(20),
                    SoLuong INT,
                    DonGia DOUBLE,
                    Size VARCHAR(5),
                    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) ON DELETE CASCADE,
                    FOREIGN KEY (MaMon) REFERENCES ThucDon(MaMon) ON DELETE SET NULL
                )";

                dbCmd = new MySqlCommand(sqlCTHD, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // DỮ LIỆU MẪU NHÂN VIÊN
                // =======================================================

                string checkNV = "SELECT COUNT(*) FROM NhanVien WHERE Username = 'admin'";

                dbCmd = new MySqlCommand(checkNV, dbConn);

                int countNV = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countNV == 0)
                {
                    string insertNV = @"
                    INSERT INTO NhanVien (MaNV, TenNV, NgaySinh, GioiTinh, ChucVu, SoDienThoai, DiaChi, Username, Password, Role)
                    VALUES ('NV01','Admin','01/01/1990','Nam','Quan ly','0901','HN','admin','123','ADMIN')
                    ";

                    dbCmd = new MySqlCommand(insertNV, dbConn);
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine(">>> Created admin: admin / 123");
                }
                else
                {
                    string updatePass = "UPDATE NhanVien SET Password = '123' WHERE Username = 'admin'";
                    dbCmd = new MySqlCommand(updatePass, dbConn);
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine(">>> Verified admin account.");
                }

                // =======================================================
                // DỮ LIỆU MẪU DANH MỤC
                // =======================================================

                string checkDM = "SELECT COUNT(*) FROM DanhMuc";

                dbCmd = new MySqlCommand(checkDM, dbConn);

                int countDM = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countDM == 0)
                {
                    string insertDM = @"
                    INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc) VALUES
                    ('Nuoc', 'Nuoc'),
                    ('Do an', 'Do an')
                    ";

                    dbCmd = new MySqlCommand(insertDM, dbConn);
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine(">>> Created sample categories.");
                }

                // =======================================================
                // DỮ LIỆU MẪU KHÁCH HÀNG
                // =======================================================

                string checkKH = "SELECT COUNT(*) FROM KhachHang";

                dbCmd = new MySqlCommand(checkKH, dbConn);

                int countKH = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countKH == 0)
                {
                    string insertKH = @"
                    INSERT INTO KhachHang (MaKH, TenKH, TheLoai, GioiTinh, SoDienThoai) VALUES
                    ('KH01', 'Trần Thị B', 'Thường', 'Nữ', '0902'),
                    ('KH02', 'Khách vãng lai', 'Vãng lai', 'Khác', '0903')
                    ";

                    dbCmd = new MySqlCommand(insertKH, dbConn);
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine(">>> Created sample customers.");
                }



                // =======================================================
                // DỮ LIỆU MẪU THỰC ĐƠN
                // =======================================================

                string checkMenu = "SELECT COUNT(*) FROM ThucDon";

                dbCmd = new MySqlCommand(checkMenu, dbConn);

                int countMenu = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countMenu == 0)
                {
                    string insertMenu = @"
                    INSERT INTO ThucDon
                    (MaMon,TenMon,DonGia,DonViTinh,HinhAnh,MaDanhMuc)
                    VALUES
                    ('M01','Ca phe den',20000,'Ly','https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500','Nuoc'),
                    ('M02','Tra sua',30000,'Ly','https://images.unsplash.com/photo-1552611052-33e04de081de?w=500','Nuoc'),
                    ('M03','Banh ngot',25000,'Cai','https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500','Do an')
                    ";

                    dbCmd = new MySqlCommand(insertMenu, dbConn);
                    dbCmd.ExecuteNonQuery();
                }
                else
                {
                    string updateImg = @"
                    UPDATE ThucDon SET HinhAnh = 'https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500' WHERE MaMon = 'M01';
                    UPDATE ThucDon SET HinhAnh = 'https://images.unsplash.com/photo-1552611052-33e04de081de?w=500' WHERE MaMon = 'M02';
                    UPDATE ThucDon SET HinhAnh = 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500' WHERE MaMon = 'M03';";
                    dbCmd = new MySqlCommand(updateImg, dbConn);
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine(">>> Updated images for existing products.");
                }

                Console.WriteLine("Khoi tao Database thanh cong!");

                dbConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Loi ket noi hoac khoi tao Database!");
            }
        }
    }
}