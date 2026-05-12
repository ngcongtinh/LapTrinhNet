using System;
using System.Globalization;
using System.Windows.Forms;
using CH.dao;
using CH.view;
using CH.View;

namespace CH.Controller
{
    public class TrangChuController
    {
        private TrangChuView view;

        // DAO
        private HoaDonDAO hoaDonDAO;
        private KhachHangDAO khachHangDAO;
        private ThucDonDAO thucDonDAO;

        public TrangChuController(TrangChuView view)
        {
            this.view = view;

            hoaDonDAO = new HoaDonDAO();
            khachHangDAO = new KhachHangDAO();
            thucDonDAO = new ThucDonDAO();

            // Load dữ liệu khi mở
            LoadStatistics();

            // Event nút làm mới
            this.view.GetBtnLamMoi().Click += (s, e) =>
            {
                LoadStatistics();
            };
        }

        private void LoadStatistics()
        {
            try
            {
                // ===== LẤY DỮ LIỆU =====
                double tongDoanhThu = hoaDonDAO.SumAllTongTien();

                int soHoaDon = hoaDonDAO.CountAll();

                int soKhachHang = khachHangDAO.CountAll();

                int soMonAn = thucDonDAO.CountAll();

                // ===== FORMAT =====
                string doanhThuFormat =
                    tongDoanhThu.ToString("#,##0", CultureInfo.InvariantCulture);

                // ===== HIỂN THỊ =====
                view.GetLblTongDoanhThu().Text =
                    doanhThuFormat + " VNĐ";

                view.GetLblSoHoaDon().Text =
                    soHoaDon.ToString();

                view.GetLblSoKhachHang().Text =
                    soKhachHang.ToString();

                view.GetLblSoMonAn().Text =
                    soMonAn.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải thống kê: " + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}