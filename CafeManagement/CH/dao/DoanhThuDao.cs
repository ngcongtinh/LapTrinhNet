using System;
using System.Collections.Generic;
using CH.Model;
using MySql.Data.MySqlClient;

namespace CafeManagement.CH.dao
{
    public class DoanhThuDAO
    {
        // =======================================================
        // LẤY DOANH THU THEO KHOẢNG THỜI GIAN
        // =======================================================
        public List<ThongKeDoanhThu> GetDoanhThuTheoKhoangThoiGian(string tuNgay, string denNgay)
        {
            List<ThongKeDoanhThu> list = new List<ThongKeDoanhThu>();

            MySqlConnection cons = DBConnection.GetConnection();

            // SQL: Lấy ngày lập hóa đơn và tổng doanh thu
            string sql = @"
            SELECT NgayLap, SUM(TongTien) AS TongDoanhThu
            FROM HoaDon
            WHERE STR_TO_DATE(NgayLap, '%d/%m/%Y') BETWEEN @tuNgay AND @denNgay
            GROUP BY NgayLap
            ORDER BY STR_TO_DATE(NgayLap, '%d/%m/%Y') ASC";

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, cons);

                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader rs = cmd.ExecuteReader();

                while (rs.Read())
                {
                    string ngay = rs["NgayLap"].ToString();

                    double tongDoanhThu =
                        Convert.ToDouble(rs["TongDoanhThu"]);

                    // Tạo object thống kê doanh thu
                    ThongKeDoanhThu tk =
                        new ThongKeDoanhThu(ngay, tongDoanhThu);

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