using System;

namespace CH.Model
{
    public class DatMon
    {
        private string maMon;
        private string tenMon;
        private int soLuong;
        private double donGia;

        public DatMon()
        {
        }

        public DatMon(string maMon, string tenMon, int soLuong, double donGia)
        {
            this.maMon = maMon;
            this.tenMon = tenMon;
            this.soLuong = soLuong;
            this.donGia = donGia;
        }

        // --- Getters & Setters ---
        public string MaMon
        {
            get { return maMon; }
            set { maMon = value; }
        }

        public string TenMon
        {
            get { return tenMon; }
            set { tenMon = value; }
        }

        public int SoLuong
        {
            get { return soLuong; }
            set { soLuong = value; }
        }

        public double DonGia
        {
            get { return donGia; }
            set { donGia = value; }
        }

        // Hàm tính thành tiền
        public double ThanhTien
        {
            get { return soLuong * donGia; }
        }

        // Chuyển sang mảng object để hiển thị DataGridView
        public object[] ToObjectArray()
        {
            return new object[]
            {
                tenMon,
                soLuong,
                donGia.ToString("#,##0"),
                ThanhTien.ToString("#,##0")
            };
        }
    }
}