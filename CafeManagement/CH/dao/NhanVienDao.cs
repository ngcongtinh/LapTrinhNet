using System;
using System.Collections.Generic;
using CafeManagement.CH.dao;
using CH.Model;
using MySql.Data.MySqlClient;

namespace CH.dao
{
    public class NhanVienDAO
    {
        // =======================================================
        // 1. LẤY DANH SÁCH NHÂN VIÊN
        // =======================================================
        public List<NhanVien> GetAll()
        {
            List<NhanVien> list = new List<NhanVien>();

            string sql = "SELECT * FROM NhanVien ORDER BY MaNV ASC";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    list.Add(MapResultSetToEntity(rs));
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        // =======================================================
        // 2. LẤY NHÂN VIÊN THEO ID
        // =======================================================
        public NhanVien GetByID(string maNV)
        {
            string sql = "SELECT * FROM NhanVien WHERE MaNV=@MaNV";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaNV", maNV);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    NhanVien nv = MapResultSetToEntity(rs);

                    rs.Close();
                    cons.Close();

                    return nv;
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        // =======================================================
        // 3. THÊM NHÂN VIÊN
        // =======================================================
        public bool Insert(NhanVien nv)
        {
            // Sinh mã tự động
            if (nv.MaNV == null || nv.MaNV.Equals("Tự động sinh"))
            {
                nv.MaNV = GetNewID();
            }

            string sql = @"
            INSERT INTO NhanVien
            (MaNV, TenNV, NgaySinh, GioiTinh, ChucVu,
             SoDienThoai, DiaChi, Username, Password, Role)
            VALUES
            (@MaNV, @TenNV, @NgaySinh, @GioiTinh, @ChucVu,
             @SoDienThoai, @DiaChi, @Username, @Password, @Role)";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
                cmd.Parameters.AddWithValue("@TenNV", nv.TenNV);
                cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                cmd.Parameters.AddWithValue("@Username", nv.Username);

                cmd.Parameters.AddWithValue(
                    "@Password",
                    string.IsNullOrEmpty(nv.Password) ? "123" : nv.Password
                );

                cmd.Parameters.AddWithValue("@Role", nv.Role);

                int row = cmd.ExecuteNonQuery();

                cons.Close();

                return row > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 4. CẬP NHẬT NHÂN VIÊN
        // =======================================================
        public bool Update(NhanVien nv)
        {
            string sql = @"
            UPDATE NhanVien
            SET TenNV=@TenNV,
                NgaySinh=@NgaySinh,
                GioiTinh=@GioiTinh,
                ChucVu=@ChucVu,
                SoDienThoai=@SoDienThoai,
                DiaChi=@DiaChi,
                Username=@Username,
                Role=@Role,
                Password=@Password
            WHERE MaNV=@MaNV";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@TenNV", nv.TenNV);
                cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);
                cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                cmd.Parameters.AddWithValue("@Username", nv.Username);
                cmd.Parameters.AddWithValue("@Role", nv.Role);
                cmd.Parameters.AddWithValue("@Password", nv.Password);
                cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);

                int row = cmd.ExecuteNonQuery();

                cons.Close();

                return row > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 5. XÓA NHÂN VIÊN
        // =======================================================
        public bool Delete(string maNV)
        {
            string sql = "DELETE FROM NhanVien WHERE MaNV=@MaNV";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaNV", maNV);

                int row = cmd.ExecuteNonQuery();

                cons.Close();

                return row > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // =======================================================
        // 6. TÌM KIẾM
        // =======================================================
        public List<NhanVien> Search(string keyword)
        {
            List<NhanVien> list = new List<NhanVien>();

            string sql = @"
            SELECT * FROM NhanVien
            WHERE TenNV LIKE @keyword
               OR MaNV LIKE @keyword
               OR SoDienThoai LIKE @keyword";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                string query = "%" + keyword + "%";

                cmd.Parameters.AddWithValue("@keyword", query);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    list.Add(MapResultSetToEntity(rs));
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        // =======================================================
        // 7. SINH MÃ NHÂN VIÊN
        // =======================================================
        public string GetNewID()
        {
            string newID = "NV01";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT MAX(MaNV) FROM NhanVien";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string maxID = result.ToString();

                    if (maxID.StartsWith("NV"))
                    {
                        int so = int.Parse(maxID.Substring(2)) + 1;

                        newID = string.Format("NV{0:00}", so);
                    }
                }

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return newID;
        }

        // =======================================================
        // 8. ĐĂNG NHẬP
        // =======================================================
        public NhanVien LoginNV(string username, string password)
        {
            string sql =
                "SELECT * FROM NhanVien WHERE Username=@Username AND Password=@Password";

            try
            {
                MySqlConnection conn = DBConnection.GetConnection();

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    NhanVien nv = MapResultSetToEntity(rs);

                    rs.Close();
                    conn.Close();

                    return nv;
                }

                rs.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        // =======================================================
        // MAP RESULTSET -> OBJECT
        // =======================================================
        private NhanVien MapResultSetToEntity(MySqlDataReader rs)
        {
            NhanVien nv = new NhanVien();

            nv.MaNV = rs["MaNV"].ToString();
            nv.TenNV = rs["TenNV"].ToString();
            nv.NgaySinh = rs["NgaySinh"].ToString();
            nv.GioiTinh = rs["GioiTinh"].ToString();
            nv.ChucVu = rs["ChucVu"].ToString();
            nv.SoDienThoai = rs["SoDienThoai"].ToString();
            nv.DiaChi = rs["DiaChi"].ToString();
            nv.Username = rs["Username"].ToString();
            nv.Password = rs["Password"].ToString();
            nv.Role = rs["Role"].ToString();

            return nv;
        }

        // =======================================================
        // KIỂM TRA SỐ ĐIỆN THOẠI TỒN TẠI
        // =======================================================
        public bool IsExistsSdt(string sdt)
        {
            try
            {
                using (MySqlConnection conn =
                    DBConnection.GetConnection())
                {
                    string sql =
                        "SELECT COUNT(*) FROM NhanVien WHERE SoDienThoai=@sdt";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@sdt", sdt);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        // =======================================================
        // KIỂM TRA USERNAME TỒN TẠI
        // =======================================================
        public bool IsExistsUsername(string user)
        {
            try
            {
                using (MySqlConnection conn =
                    DBConnection.GetConnection())
                {
                    string sql =
                        "SELECT COUNT(*) FROM NhanVien WHERE Username=@user";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@user", user);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }
    }
}