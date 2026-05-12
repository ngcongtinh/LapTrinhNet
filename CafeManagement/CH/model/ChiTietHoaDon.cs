using System;

namespace CH.Model
{
    public class ChiTietHoaDon
    {
        private string tenMon;
        private string size;
        private int soLuong;
        private double donGia;
        private double thanhTien;
        public double ThanhTien => thanhTien;

        public ChiTietHoaDon(string tenMon, string size, int soLuong, double donGia)
        {
            this.tenMon = tenMon;
            this.size = size;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.thanhTien = soLuong * donGia;
        }

        public object[] ToObjectArray()
        {
            return new object[]
            {
                tenMon,
                size,
                soLuong,
                donGia.ToString("#,##0"),
                thanhTien.ToString("#,##0")
            };
        }
    }
}