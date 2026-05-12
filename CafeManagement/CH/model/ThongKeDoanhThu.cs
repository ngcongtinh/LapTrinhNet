using System;

namespace CH.Model
{
    public class ThongKeDoanhThu
    {
        private string thoiGian; // Ngày, Tháng, Năm
        private double tongDoanhThu;

        public ThongKeDoanhThu(string thoiGian, double tongDoanhThu)
        {
            this.thoiGian = thoiGian;
            this.tongDoanhThu = tongDoanhThu;
        }

        // Hiển thị lên DataGridView
        public object[] ToObjectArray()
        {
            return new object[]
            {
                thoiGian,
                tongDoanhThu.ToString("#,##0") + " VNĐ"
            };
        }

        // Getters & Setters
        public string ThoiGian
        {
            get { return thoiGian; }
            set { thoiGian = value; }
        }

        public double TongDoanhThu
        {
            get { return tongDoanhThu; }
            set { tongDoanhThu = value; }
        }
    }
}