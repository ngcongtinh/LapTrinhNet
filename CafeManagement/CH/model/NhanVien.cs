namespace CH.Model
{
    public class NhanVien
    {
        private string maNV;
        private string tenNV;
        private string ngaySinh;
        private string gioiTinh;
        private string chucVu;
        private string soDienThoai;
        private string diaChi;
        private string username;
        private string password;
        private string role;

        public NhanVien()
        {
        }

        public NhanVien(string maNV, string tenNV, string ngaySinh,
                        string gioiTinh, string chucVu, string soDienThoai,
                        string diaChi, string username, string password, string role)
        {
            this.maNV = maNV;
            this.tenNV = tenNV;
            this.ngaySinh = ngaySinh;
            this.gioiTinh = gioiTinh;
            this.chucVu = chucVu;
            this.soDienThoai = soDienThoai;
            this.diaChi = diaChi;
            this.username = username;
            this.password = password;
            this.role = role;
        }

        // Getters & Setters
        public string MaNV
        {
            get { return maNV; }
            set { maNV = value; }
        }

        public string TenNV
        {
            get { return tenNV; }
            set { tenNV = value; }
        }

        public string NgaySinh
        {
            get { return ngaySinh; }
            set { ngaySinh = value; }
        }

        public string GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; }
        }

        public string ChucVu
        {
            get { return chucVu; }
            set { chucVu = value; }
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

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        public object[] ToObjectArray()
        {
            return new object[]
            {
                maNV,
                tenNV,
                ngaySinh,
                gioiTinh,
                chucVu,
                soDienThoai,
                diaChi
            };
        }
    }
}