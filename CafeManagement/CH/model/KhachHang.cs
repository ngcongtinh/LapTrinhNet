using System;

namespace CH.Model
{
    public class KhachHang
    {
        private string maKH;
        private string tenKH;
        private string theLoai;
        private string gioiTinh;
        private string email;
        private string soDienThoai;
        private string diaChi;

        public KhachHang()
        {
        }

        public KhachHang(string maKH, string tenKH, string theLoai, string gioiTinh,
                         string email, string soDienThoai, string diaChi)
        {
            this.maKH = maKH;
            this.tenKH = tenKH;
            this.theLoai = theLoai;
            this.gioiTinh = gioiTinh;
            this.email = email;
            this.soDienThoai = soDienThoai;
            this.diaChi = diaChi;
        }

        // Getters & Setters
        public string MaKH
        {
            get { return maKH; }
            set { maKH = value; }
        }

        public string TenKH
        {
            get { return tenKH; }
            set { tenKH = value; }
        }

        public string TheLoai
        {
            get { return theLoai; }
            set { theLoai = value; }
        }

        public string GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string SoDienThoai
        {
            get { return soDienThoai; }
            set { soDienThoai = value; }
        }

        public string DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; }
        }

        // Chuyển đổi sang object[] để hiển thị DataGridView
        public object[] ToObjectArray()
        {
            return new object[]
            {
                maKH,
                tenKH,
                theLoai,
                gioiTinh,
                email,
                soDienThoai,
                diaChi
            };
        }
    }
}