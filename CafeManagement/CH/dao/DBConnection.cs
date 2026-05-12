using System;
using MySql.Data.MySqlClient;

namespace CafeManagement.CH.dao
{
    public class DBConnection
    {
        private static readonly string HOST = "localhost";
        private static readonly string PORT = "3306";
        private static readonly string DB_NAME = "quanlycuahang2";
        private static readonly string USER = "root";
        private static readonly string PASS = "123456";

        // Kết nối Server MySQL
        private static readonly string SERVER_CONNECTION =
            $"server={HOST};port={PORT};user={USER};password={PASS};";

        // Kết nối Database
        private static readonly string DB_CONNECTION =
            $"server={HOST};port={PORT};database={DB_NAME};user={USER};password={PASS};charset=utf8;";

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return cons;
        }

        // =======================================================
        // 2. KHỞI TẠO DATABASE
        // =======================================================
        public static void InitializeDatabase()
        {
            try
            {
                // =======================================================
                // 1. TẠO DATABASE
                // =======================================================

                MySqlConnection serverConn = new MySqlConnection(SERVER_CONNECTION);
                serverConn.Open();

                MySqlCommand serverCmd = new MySqlCommand(
                    $"CREATE DATABASE IF NOT EXISTS {DB_NAME}",
                    serverConn
                );

                serverCmd.ExecuteNonQuery();

                Console.WriteLine("Kiem tra Database: " + DB_NAME);

                serverConn.Close();

                // =======================================================
                // 2. KẾT NỐI DATABASE
                // =======================================================

                MySqlConnection dbConn = new MySqlConnection(DB_CONNECTION);
                dbConn.Open();

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
                // TẠO BẢNG KHO
                // =======================================================

                string sqlKho = @"
                CREATE TABLE IF NOT EXISTS Kho (
                    MaHH VARCHAR(20) PRIMARY KEY,
                    TenHH VARCHAR(100),
                    SoLuong INT,
                    GiaNhap DOUBLE,
                    GiaBan DOUBLE
                )";

                dbCmd = new MySqlCommand(sqlKho, dbConn);
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
                // TẠO BẢNG THUCDON
                // =======================================================

                string sqlThucDon = @"
                CREATE TABLE IF NOT EXISTS ThucDon (
                    MaMon VARCHAR(20) PRIMARY KEY,
                    TenMon VARCHAR(100),
                    DonGia DOUBLE,
                    DonViTinh VARCHAR(20),
                    HinhAnh VARCHAR(255),
                    TenDanhMuc VARCHAR(100)
                )";

                dbCmd = new MySqlCommand(sqlThucDon, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG HOADON
                // =======================================================

                string sqlHoaDon = @"
                CREATE TABLE IF NOT EXISTS HoaDon (
                    MaHD VARCHAR(20) PRIMARY KEY,
                    TenNV VARCHAR(100),
                    TenKH VARCHAR(100),
                    NgayLap VARCHAR(20),
                    TongTien DOUBLE
                )";

                dbCmd = new MySqlCommand(sqlHoaDon, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // TẠO BẢNG CHITIETHOADON
                // =======================================================

                string sqlCTHD = @"
                CREATE TABLE IF NOT EXISTS ChiTietHoaDon (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    MaHD VARCHAR(20),
                    TenMon VARCHAR(100),
                    SoLuong INT,
                    DonGia DOUBLE,
                    Size VARCHAR(5),
                    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) ON DELETE CASCADE
                )";

                dbCmd = new MySqlCommand(sqlCTHD, dbConn);
                dbCmd.ExecuteNonQuery();

                // =======================================================
                // DỮ LIỆU MẪU NHÂN VIÊN
                // =======================================================

                string checkNV = "SELECT COUNT(*) FROM NhanVien";

                dbCmd = new MySqlCommand(checkNV, dbConn);

                int countNV = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countNV == 0)
                {
                    string insertNV = @"
                    INSERT INTO NhanVien VALUES
                    ('NV01','Admin','01/01/1990','Nam','Quan ly','0901','HN','admin','123','ADMIN'),

                    ('NV02','Staff','01/01/2000','Nu','Nhan vien','0902','HCM','staff','123','NHÂN VIÊN')
                    ";

                    dbCmd = new MySqlCommand(insertNV, dbConn);
                    dbCmd.ExecuteNonQuery();
                }

                // =======================================================
                // DỮ LIỆU MẪU KHO
                // =======================================================

                string checkKho = "SELECT COUNT(*) FROM Kho";

                dbCmd = new MySqlCommand(checkKho, dbConn);

                int countKho = Convert.ToInt32(dbCmd.ExecuteScalar());

                if (countKho == 0)
                {
                    string insertKho = @"
                    INSERT INTO Kho VALUES
                    ('HH01','Ca phe',100,10000,0),

                    ('HH02','Tra sua',100,15000,0),

                    ('HH03','Banh',100,20000,0)
                    ";

                    dbCmd = new MySqlCommand(insertKho, dbConn);
                    dbCmd.ExecuteNonQuery();
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
                    (MaMon,TenMon,DonGia,DonViTinh,HinhAnh,TenDanhMuc)
                    VALUES

                    ('M01','Ca phe den',20000,'Ly','','Nuoc'),

                    ('M02','Tra sua',30000,'Ly','','Nuoc'),

                    ('M03','Banh ngot',25000,'Cai','','Do an')
                    ";

                    dbCmd = new MySqlCommand(insertMenu, dbConn);
                    dbCmd.ExecuteNonQuery();
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