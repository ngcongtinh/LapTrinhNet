using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CH.dao;
using CH.Model;
using CH.View;

namespace CH.Controller
{
    public class DoanhThuController
    {
        private DoanhThuView view;
        private HoaDonDAO dao;

        public DoanhThuController(DoanhThuView view)
        {
            this.view = view;
            this.dao = new HoaDonDAO();

            view.BtnXemBaoCao.Click += ReportListener;

            XemBaoCao();
        }

        private void XemBaoCao()
        {
            string tuNgay = view.TxtTuNgay.Text.Trim();
            string denNgay = view.TxtDenNgay.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(tuNgay) || string.IsNullOrEmpty(denNgay))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ Từ Ngày và Đến Ngày.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Kiểm tra định dạng DD/MM/YYYY
            Regex datePattern = new Regex(@"^\d{2}/\d{2}/\d{4}$");

            if (!datePattern.IsMatch(tuNgay) || !datePattern.IsMatch(denNgay))
            {
                MessageBox.Show(
                    "Định dạng ngày không hợp lệ. Vui lòng nhập theo định dạng DD/MM/YYYY.",
                    "Lỗi Định Dạng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Kiểm tra ngày hợp lệ
            DateTime tuDate;
            DateTime denDate;

            bool validTuNgay = DateTime.TryParseExact(
                tuNgay,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out tuDate
            );

            bool validDenNgay = DateTime.TryParseExact(
                denNgay,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out denDate
            );

            if (!validTuNgay || !validDenNgay)
            {
                MessageBox.Show(
                    "Ngày không hợp lệ.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Kiểm tra từ ngày <= đến ngày
            if (tuDate > denDate)
            {
                MessageBox.Show(
                    "Từ Ngày phải nhỏ hơn hoặc bằng Đến Ngày.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Clear table
            DataGridView dgv = view.DgvDoanhThu;
            dgv.Rows.Clear();

            // Load dữ liệu
            List<ThongKeDoanhThu> list = dao.GetDoanhThuTheoNgay(tuNgay, denNgay);

            double tongDoanhThu = 0;

            foreach (ThongKeDoanhThu item in list)
            {
                dgv.Rows.Add(item.ToObjectArray());

                tongDoanhThu += item.TongDoanhThu;
            }

            // Hiển thị tổng doanh thu
            view.LblTongDoanhThu.Text =
                "TỔNG DOANH THU: " + tongDoanhThu.ToString("N0") + " VNĐ";
        }

        // ================= EVENT =================
        private void ReportListener(object sender, EventArgs e)
        {
            XemBaoCao();
        }
    }
}