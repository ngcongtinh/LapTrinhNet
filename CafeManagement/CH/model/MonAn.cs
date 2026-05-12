using System;

namespace CH.Model
{
    public class MonAn
    {
        private string maMon;
        private string tenMon;
        private double donGia;
        private string donViTinh;
        private string hinhAnh;
        private string tenDanhMuc;
        private string maDanhMuc; // lưu ID

        public MonAn()
        {
        }

        // Constructor
        public MonAn(string maMon, string tenMon, double donGia,
                     string donViTinh, string hinhAnh, string tenDanhMuc)
        {
            this.maMon = maMon;
            this.tenMon = tenMon;
            this.donGia = donGia;
            this.donViTinh = donViTinh;
            this.hinhAnh = hinhAnh;
            this.tenDanhMuc = tenDanhMuc;
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

        public double DonGia
        {
            get { return donGia; }
            set { donGia = value; }
        }

        public string DonViTinh
        {
            get { return donViTinh; }
            set { donViTinh = value; }
        }

        public string HinhAnh
        {
            get { return hinhAnh; }
            set { hinhAnh = value; }
        }

        public string TenDanhMuc
        {
            get { return tenDanhMuc; }
            set { tenDanhMuc = value; }
        }

        public string MaDanhMuc
        {
            get { return maDanhMuc; }
            set { maDanhMuc = value; }
        }

        // Chuyển sang object[] để hiển thị DataGridView
        public object[] ToObjectArray()
        {
            return new object[]
            {
                maMon,
                tenMon,
                donGia.ToString("#,##0"),
                donViTinh,
                tenDanhMuc,
                hinhAnh
            };
        }
    }
}