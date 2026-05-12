using System;

namespace CH.Model
{
    public class HoaDon
    {
        private string maHD;
        private string tenNV; // Tên nhân viên lập
        private string tenKH; // Tên khách hàng
        private string ngayLap;
        private double tongTien;

        public HoaDon()
        {
        }

        public HoaDon(string maHD, string tenNV, string tenKH, string ngayLap, double tongTien)
        {
            this.maHD = maHD;
            this.tenNV = tenNV;
            this.tenKH = tenKH;
            this.ngayLap = ngayLap;
            this.tongTien = tongTien;
        }

        // Getters & Setters
        public string MaHD
        {
            get { return maHD; }
            set { maHD = value; }
        }

        public string TenNV
        {
            get { return tenNV; }
            set { tenNV = value; }
        }

        public string TenKH
        {
            get { return tenKH; }
            set { tenKH = value; }
        }

        public string NgayLap
        {
            get { return ngayLap; }
            set { ngayLap = value; }
        }

        public double TongTien
        {
            get { return tongTien; }
            set { tongTien = value; }
        }

        public object[] ToObjectArray()
        {
            return new object[]
            {
                maHD,
                tenNV,
                tenKH,
                ngayLap,
                tongTien.ToString("#,##0 VNĐ")
            };
        }
    }
}