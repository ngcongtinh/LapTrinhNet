using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CafeManagement.CH.dao
{
    public class AdminDAO
    {
        // =======================================================
        // 1. LOGIN: Kiểm tra trong bảng NhanVien
        // =======================================================
        public string Login(string username, string password)
        {
            string role = null;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                // Truy vấn vào các cột Username, Password của bảng NhanVien
                string sql = "SELECT Role FROM NhanVien WHERE Username=@username AND Password=@password";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    // Lấy quyền (ADMIN hoặc NHÂN VIÊN)
                    role = rs["Role"].ToString();
                }

                rs.Close();
                cmd.Dispose();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return role;
        }

        // =======================================================
        // 2. TẠO DỮ LIỆU MẪU CHO BẢNG NHÂN VIÊN
        // =======================================================
        public void InsertDefaultUsersIfEmpty()
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                // Kiểm tra xem bảng NhanVien có dữ liệu chưa
                string countSql = "SELECT COUNT(*) FROM NhanVien";

                MySqlCommand countCmd = new MySqlCommand(countSql, cons);

                int count = Convert.ToInt32(countCmd.ExecuteScalar());

                if (count == 0)
                {
                    // Vì bảng NhanVien có nhiều cột NOT NULL
                    // nên phải insert đầy đủ thông tin

                    string sql = @"INSERT INTO NhanVien
                    (MaNV, TenNV, NgaySinh, GioiTinh, ChucVu, SoDienThoai, DiaChi, Username, Password, Role)
                    VALUES
                    ('NV01', 'Nguyễn Quản Lý', '1990-01-01', 'Nam', 'Cửa hàng trưởng', '0901234567', 'Hà Nội', 'admin', '123', 'ADMIN'),

                    ('NV02', 'Trần Nhân Viên', '1995-05-05', 'Nữ', 'Thu ngân', '0909876543', 'Hồ Chí Minh', 'staff', '123', 'NHÂN VIÊN')";

                    MySqlCommand cmd = new MySqlCommand(sql, cons);

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("✓ Đã khởi tạo dữ liệu mẫu trong bảng NhanVien (admin/123 và staff/123)");
                }

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Lỗi khi tạo dữ liệu mẫu nhân viên");
            }
        }
    }
}