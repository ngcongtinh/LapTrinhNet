using System;
using System.Collections.Generic;
using CafeManagement.CH.dao;
using CH.Model;
using MySql.Data.MySqlClient;

namespace CH.dao
{
    public class HoaDonDAO
    {
        // =======================================================
        // 1. LẤY DANH SÁCH HÓA ĐƠN
        // =======================================================
        public List<HoaDon> GetAll()
        {
            List<HoaDon> list = new List<HoaDon>();

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT * FROM HoaDon ORDER BY MaHD DESC";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    HoaDon hd = new HoaDon(
                        rs["MaHD"].ToString(),
                        rs["TenNV"].ToString(),
                        rs["TenKH"].ToString(),
                        rs["NgayLap"].ToString(),
                        Convert.ToDouble(rs["TongTien"])
                    );

                    list.Add(hd);
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
        // 2. LẤY CHI TIẾT HÓA ĐƠN
        // =======================================================
        public List<ChiTietHoaDon> GetChiTiet(string maHD)
        {
            List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT * FROM ChiTietHoaDon WHERE MaHD=@maHD";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@maHD", maHD);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    ChiTietHoaDon ct = new ChiTietHoaDon(
                        rs["TenMon"].ToString(),
                        rs["Size"].ToString(),
                        Convert.ToInt32(rs["SoLuong"]),
                        Convert.ToDouble(rs["DonGia"])
                    );

                    list.Add(ct);
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
        // 3. THÊM HÓA ĐƠN
        // =======================================================
        public bool Add(HoaDon hd)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"INSERT INTO HoaDon
                (MaHD, TenNV, TenKH, NgayLap, TongTien)
                VALUES
                (@MaHD, @TenNV, @TenKH, @NgayLap, @TongTien)";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaHD", hd.MaHD);
                cmd.Parameters.AddWithValue("@TenNV", hd.TenNV);
                cmd.Parameters.AddWithValue("@TenKH", hd.TenKH);
                cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
                cmd.Parameters.AddWithValue("@TongTien", hd.TongTien);

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
        // 4. SỬA HÓA ĐƠN
        // =======================================================
        public bool Update(HoaDon hd)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"UPDATE HoaDon
                SET TenNV=@TenNV,
                    TenKH=@TenKH,
                    NgayLap=@NgayLap
                WHERE MaHD=@MaHD";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@TenNV", hd.TenNV);
                cmd.Parameters.AddWithValue("@TenKH", hd.TenKH);
                cmd.Parameters.AddWithValue("@NgayLap", hd.NgayLap);
                cmd.Parameters.AddWithValue("@MaHD", hd.MaHD);

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
        // 5. XÓA HÓA ĐƠN
        // =======================================================
        public bool Delete(string maHD)
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "DELETE FROM HoaDon WHERE MaHD=@MaHD";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@MaHD", maHD);

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
        // 6. THÊM DỮ LIỆU MẪU
        // =======================================================
        public void AddSampleDataIfEmpty()
        {
            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string checkSql = "SELECT COUNT(*) FROM HoaDon";

                MySqlCommand cmd = new MySqlCommand(checkSql, cons);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 0)
                {
                    cmd = new MySqlCommand(
                        "INSERT INTO HoaDon VALUES ('HD001', 'Nguyễn Văn A', 'Trần Thị B', '01/12/2025', 130000)",
                        cons
                    );
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(
                        "INSERT INTO HoaDon VALUES ('HD002', 'Lê Văn C', 'Khách vãng lai', '02/12/2025', 20000)",
                        cons
                    );
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(
                        "INSERT INTO ChiTietHoaDon(MaHD, TenMon, Size, SoLuong, DonGia) VALUES ('HD001', 'Gà rán', 'S', 2, 35000)",
                        cons
                    );
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(
                        "INSERT INTO ChiTietHoaDon(MaHD, TenMon, Size, SoLuong, DonGia) VALUES ('HD001', 'Khoai tây chiên', 'M', 1, 60000)",
                        cons
                    );
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(
                        "INSERT INTO ChiTietHoaDon(MaHD, TenMon, Size, SoLuong, DonGia) VALUES ('HD002', 'Pepsi', 'S', 2, 10000)",
                        cons
                    );
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Đã thêm dữ liệu mẫu cho Hóa đơn.");
                }

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // =======================================================
        // 7. TẠO MÃ HÓA ĐƠN MỚI
        // =======================================================
        public string GetNewID()
        {
            string newID = "HD001";

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT MaHD FROM HoaDon ORDER BY MaHD DESC LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    string lastID = rs["MaHD"].ToString();

                    if (lastID.Length >= 4)
                    {
                        string prefix = lastID.Substring(0, 2);

                        string numberPart = lastID.Substring(2);

                        try
                        {
                            int number = int.Parse(numberPart);

                            number++;

                            newID = prefix + number.ToString("D3");
                        }
                        catch
                        {
                            Console.WriteLine("Lỗi parse mã cũ: " + lastID);

                            newID = "HD" + DateTime.Now.Ticks;
                        }
                    }
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
        // 8. TỔNG DOANH THU
        // =======================================================
        public double SumAllTongTien()
        {
            double total = 0;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT SUM(TongTien) FROM HoaDon";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    total = Convert.ToDouble(result);
                }

                cons.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return total;
        }

        // =======================================================
        // 9. ĐẾM HÓA ĐƠN
        // =======================================================
        public int CountAll()
        {
            int count = 0;

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = "SELECT COUNT(*) FROM HoaDon";

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
        // 10. THỐNG KÊ DOANH THU THEO NGÀY
        // =======================================================
        public List<ThongKeDoanhThu> GetDoanhThuTheoNgay(
            string dateFrom,
            string dateTo)
        {
            List<ThongKeDoanhThu> list =
                new List<ThongKeDoanhThu>();

            try
            {
                MySqlConnection cons = DBConnection.GetConnection();

                string sql = @"
                SELECT NgayLap,
                       SUM(TongTien) AS TongDoanhThu
                FROM HoaDon
                WHERE STR_TO_DATE(NgayLap, '%d/%m/%Y')
                BETWEEN STR_TO_DATE(@dateFrom, '%d/%m/%Y')
                AND STR_TO_DATE(@dateTo, '%d/%m/%Y')
                GROUP BY NgayLap
                ORDER BY STR_TO_DATE(NgayLap, '%d/%m/%Y')";

                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                cmd.Parameters.AddWithValue("@dateTo", dateTo);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    ThongKeDoanhThu tk =
                        new ThongKeDoanhThu(
                            rs["NgayLap"].ToString(),
                            Convert.ToDouble(rs["TongDoanhThu"])
                        );

                    list.Add(tk);
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
    }
}