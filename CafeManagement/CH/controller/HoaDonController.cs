using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CH.dao;
using CH.Model;
using CH.view;
using CH.View;

namespace CH.Controller
{
    public class HoaDonController
    {
        private HoaDonView view;
        private HoaDonDAO dao;

        public HoaDonController(HoaDonView view)
        {
            this.view = view;
            this.dao = new HoaDonDAO();

            // 1. Load dữ liệu ban đầu
            LoadData("");

            // 2. Tìm kiếm realtime
            view.TxtSearch.TextChanged += (s, e) =>
            {
                Search();
            };

            // 3. Xem chi tiết hóa đơn
            view.TableHoaDon.CellClick += DgvHoaDon_CellClick;
        }

        // ================= SEARCH =================
        private void Search()
        {
            string keyword = view.TxtSearch.Text;

            // Nếu là placeholder
            if (keyword.Contains("🔍"))
            {
                LoadData("");
            }
            else
            {
                LoadData(keyword);
            }
        }

        // ================= LOAD DATA =================
        public void LoadData(string keyword)
        {
            view.TableHoaDon.Rows.Clear();

            string searchKey = RemoveAccent(keyword.ToLower().Trim());

            List<HoaDon> list = dao.GetAll();

            foreach (HoaDon hd in list)
            {
                string maHD = hd.MaHD.ToLower();
                string tenKH = RemoveAccent(hd.TenKH.ToLower());
                string tenNV = RemoveAccent(hd.TenNV.ToLower());

                if (
                    string.IsNullOrEmpty(searchKey)
                    || maHD.Contains(searchKey)
                    || tenKH.Contains(searchKey)
                    || tenNV.Contains(searchKey)
                )
                {
                    view.AddRow(hd);
                }
            }
        }

        // ================= XEM CHI TIẾT =================
        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Cột icon mắt (ví dụ cột cuối)
            if (e.ColumnIndex == view.TableHoaDon.Columns["ChiTiet"].Index)
            {
                ShowHoaDonChiTiet();
            }
        }

        private void ShowHoaDonChiTiet()
        {
            int row = view.TableHoaDon.CurrentRow.Index;

            if (row < 0)
                return;

            // Lấy dữ liệu
            string maHD = view.TableHoaDon.Rows[row].Cells[0].Value.ToString();
            string tenKH = view.TableHoaDon.Rows[row].Cells[2].Value.ToString();

            string strTien = view.TableHoaDon.Rows[row]
                .Cells[4]
                .Value
                .ToString()
                .Replace(".", "")
                .Replace("đ", "")
                .Replace("VNĐ", "")
                .Trim();

            double tongTien = double.Parse(strTien);

            // Lấy chi tiết
            List<ChiTietHoaDon> details = dao.GetChiTiet(maHD);

            // Mở form chi tiết
            ChiTietHoaDonView dialog = new ChiTietHoaDonView();

            dialog.SetDetails(maHD, tenKH, tongTien, details);

            dialog.StartPosition = FormStartPosition.CenterParent;

            dialog.ShowDialog();
        }

        // ================= REMOVE ACCENT =================
        private string RemoveAccent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            string normalized = text.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);

                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace('đ', 'd')
                .Replace('Đ', 'D');
        }
    }
}