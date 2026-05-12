using System;
using System.Collections.Generic;
using CafeManagement.CH.dao;
using CH.Model;
using MySql.Data.MySqlClient;

namespace CH.dao
{
    public class ThucDonDAO
    {
        // =======================================================
        // 1. LẤY TẤT CẢ MÓN ĂN
        // =======================================================
        public List<MonAn> GetAll()
        {
            List<MonAn> list = new List<MonAn>();

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                SELECT MaMon,
                       TenMon,
                       DonGia,
                       DonViTinh,
                       TenDanhMuc,
                       HinhAnh
                FROM ThucDon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    list.Add(new MonAn(
                        rs["MaMon"].ToString(),
                        rs["TenMon"].ToString(),
                        Convert.ToDouble(rs["DonGia"]),
                        rs["DonViTinh"].ToString(),
                        rs["HinhAnh"].ToString(),
                        rs["TenDanhMuc"].ToString()
                    ));
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
        // 2. THÊM MÓN ĂN
        // =======================================================
        public bool Add(MonAn m)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                INSERT INTO ThucDon
                (MaMon, TenMon, DonGia, DonViTinh, TenDanhMuc, HinhAnh)
                VALUES
                (@MaMon, @TenMon, @DonGia, @DonViTinh, @TenDanhMuc, @HinhAnh)";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaMon", m.MaMon);
                cmd.Parameters.AddWithValue("@TenMon", m.TenMon);
                cmd.Parameters.AddWithValue("@DonGia", m.DonGia);
                cmd.Parameters.AddWithValue("@DonViTinh", m.DonViTinh);
                cmd.Parameters.AddWithValue("@TenDanhMuc", m.TenDanhMuc);
                cmd.Parameters.AddWithValue("@HinhAnh", m.HinhAnh);

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
        // 3. SỬA MÓN ĂN
        // =======================================================
        public bool Update(MonAn m)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                UPDATE ThucDon
                SET TenMon=@TenMon,
                    DonGia=@DonGia,
                    DonViTinh=@DonViTinh,
                    TenDanhMuc=@TenDanhMuc,
                    HinhAnh=@HinhAnh
                WHERE MaMon=@MaMon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@TenMon", m.TenMon);
                cmd.Parameters.AddWithValue("@DonGia", m.DonGia);
                cmd.Parameters.AddWithValue("@DonViTinh", m.DonViTinh);
                cmd.Parameters.AddWithValue("@TenDanhMuc", m.TenDanhMuc);
                cmd.Parameters.AddWithValue("@HinhAnh", m.HinhAnh);
                cmd.Parameters.AddWithValue("@MaMon", m.MaMon);

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
        // 4. XÓA MÓN ĂN
        // =======================================================
        public bool Delete(string maMon)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "DELETE FROM ThucDon WHERE MaMon=@MaMon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaMon", maMon);

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
        // 5. SINH MÃ TỰ ĐỘNG
        // =======================================================
        public string GetNewID()
        {
            string newID = "M01";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                SELECT MaMon
                FROM ThucDon
                ORDER BY LENGTH(MaMon) DESC, MaMon DESC
                LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    string lastID = rs.GetString(0);

                    int num = int.Parse(lastID.Substring(1)) + 1;

                    newID = "M" + (num < 10 ? "0" + num : num.ToString());
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception)
            {
            }

            return newID;
        }

        // =======================================================
        // 6. ĐẾM TỔNG SỐ MÓN ĂN
        // =======================================================
        public int CountAll()
        {
            int count = 0;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT COUNT(*) FROM ThucDon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return count;
        }

        // =======================================================
        // 7. LẤY MÓN ĂN THEO MÃ
        // =======================================================
        public MonAn GetByID(string ma)
        {
            MonAn mon = null;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                SELECT MaMon,
                       TenMon,
                       DonGia,
                       DonViTinh,
                       TenDanhMuc,
                       HinhAnh
                FROM ThucDon
                WHERE MaMon=@MaMon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaMon", ma);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    mon = new MonAn(
                        rs["MaMon"].ToString(),
                        rs["TenMon"].ToString(),
                        Convert.ToDouble(rs["DonGia"]),
                        rs["DonViTinh"].ToString(),
                        rs["HinhAnh"].ToString(),
                        rs["TenDanhMuc"].ToString()
                    );
                }

                rs.Close();
                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return mon;
        }

        // =======================================================
        // 8. KIỂM TRA TÊN MÓN TỒN TẠI
        // =======================================================
        public bool IsExistsTenMon(string tenMon)
        {
            bool exists = false;

            string sql =
                "SELECT COUNT(*) FROM ThucDon WHERE TenMon=@TenMon";

            try
            {
                using (MySqlConnection cons =
                    DBConnection.GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(sql, cons);

                    cmd.Parameters.AddWithValue("@TenMon", tenMon);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    exists = count > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return exists;
        }
    }
}