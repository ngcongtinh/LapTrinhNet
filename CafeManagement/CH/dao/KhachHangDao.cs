using System;
using System.Collections.Generic;
using CafeManagement.CH.dao;
using CH.Model;
using MySql.Data.MySqlClient;

namespace CH.dao
{
    public class KhachHangDAO
    {
        // =======================================================
        // 1. LẤY TẤT CẢ KHÁCH HÀNG
        // =======================================================
        public List<KhachHang> GetAll()
        {
            List<KhachHang> list = new List<KhachHang>();

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT * FROM KhachHang";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    KhachHang kh = new KhachHang();

                    kh.MaKH = rs["MaKH"].ToString();
                    kh.TenKH = rs["TenKH"].ToString();
                    kh.TheLoai = rs["TheLoai"].ToString();
                    kh.GioiTinh = rs["GioiTinh"].ToString();
                    kh.Email = rs["Email"].ToString();
                    kh.SoDienThoai = rs["SoDienThoai"].ToString();
                    kh.DiaChi = rs["DiaChi"].ToString();

                    list.Add(kh);
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
        // 2. LẤY KHÁCH HÀNG THEO MÃ
        // =======================================================
        public KhachHang GetById(string maKH)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT * FROM KhachHang WHERE MaKH=@MaKH";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaKH", maKH);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    KhachHang kh = new KhachHang();

                    kh.MaKH = rs["MaKH"].ToString();
                    kh.TenKH = rs["TenKH"].ToString();
                    kh.TheLoai = rs["TheLoai"].ToString();
                    kh.GioiTinh = rs["GioiTinh"].ToString();
                    kh.Email = rs["Email"].ToString();
                    kh.SoDienThoai = rs["SoDienThoai"].ToString();
                    kh.DiaChi = rs["DiaChi"].ToString();

                    rs.Close();
                    cons.Close();

                    return kh;
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
        // 3. THÊM KHÁCH HÀNG
        // =======================================================
        public bool Insert(KhachHang kh)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();


                // Tự động sinh mã
                if (string.IsNullOrEmpty(kh.MaKH)
                    || kh.MaKH.Equals("Tự động sinh"))
                {
                    kh.MaKH = GetNewID();
                }

                string sql = @"
                INSERT INTO KhachHang
                (MaKH, TenKH, TheLoai, GioiTinh, Email, SoDienThoai, DiaChi)
                VALUES
                (@MaKH, @TenKH, @TheLoai, @GioiTinh, @Email, @SoDienThoai, @DiaChi)
                ";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);
                cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
                cmd.Parameters.AddWithValue("@TheLoai", kh.TheLoai);
                cmd.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);

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
        // 4. SỬA KHÁCH HÀNG
        // =======================================================
        public bool Update(KhachHang kh)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                UPDATE KhachHang
                SET TenKH=@TenKH,
                    TheLoai=@TheLoai,
                    GioiTinh=@GioiTinh,
                    Email=@Email,
                    SoDienThoai=@SoDienThoai,
                    DiaChi=@DiaChi
                WHERE MaKH=@MaKH";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@TenKH", kh.TenKH);
                cmd.Parameters.AddWithValue("@TheLoai", kh.TheLoai);
                cmd.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);

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
        // 5. XÓA KHÁCH HÀNG
        // =======================================================
        public bool Delete(string maKH)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "DELETE FROM KhachHang WHERE MaKH=@MaKH";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaKH", maKH);

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
        // 6. TẠO MÃ KHÁCH HÀNG MỚI
        // =======================================================
        public string GetNewID()
        {
            string newID = "KH001";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql =
                    "SELECT MaKH FROM KhachHang ORDER BY MaKH DESC LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    string lastID = rs["MaKH"].ToString();

                    string numberPart = lastID.Substring(2);

                    int number = int.Parse(numberPart);

                    number++;

                    newID = string.Format("KH{0:000}", number);
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return newID;
        }

        // =======================================================
        // 7. ĐẾM TẤT CẢ KHÁCH HÀNG
        // =======================================================
        public int CountAll()
        {
            int count = 0;

            try
            {
                using (MySqlConnection conn =
                    DBConnection.GetConnection())

                {

                    string sql = "SELECT COUNT(*) FROM KhachHang";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return count;
        }

        // =======================================================
        // 8. KIỂM TRA SỐ ĐIỆN THOẠI TỒN TẠI
        // =======================================================
        public bool IsExistsSdt(string sdt)
        {
            bool exists = false;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql =
                    "SELECT COUNT(*) FROM KhachHang WHERE SoDienThoai=@sdt";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@sdt", sdt);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                exists = count > 0;

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return exists;
        }
    }
}