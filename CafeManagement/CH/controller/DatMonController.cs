using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CafeManagement.CH.dao;
using CH.Controller;
using CH.dao;
using CH.Model;
using CH.View;

namespace CH.controller
{
    public class DatMonController
    {
        private DatMonView view;
        private ThucDonDAO menuDao;
        private HoaDonDAO hoaDonDao;

        private double currentTotal = 0;

        private HoaDonController hoaDonController;
        private KhachHangDAO khachHangDAO;
        private KhachHangController khachHangController;

        public DatMonController(
            DatMonView view,
            HoaDonController hoaDonController)
        {
            this.view = view;
            this.hoaDonController = hoaDonController;

            menuDao = new ThucDonDAO();
            hoaDonDao = new HoaDonDAO();
            khachHangDAO = new KhachHangDAO();

            view.ClearCart();

            LoadMenu();
            LoadDanhMuc();

            // EVENT
            view.AddXoaListener((s, e) => XoaHetGioHang());

            view.AddThanhToanListener((s, e) =>
            {
                MoPopupThanhToan();
            });

            view.GetCbDanhMuc().SelectedIndexChanged += (s, e) =>
            {
                LoadMenu();
            };

            view.GetTxtSearch().TextChanged += (s, e) =>
            {
                LoadMenu();
            };
        }

        // ================= LOAD DANH MỤC =================
        private void LoadDanhMuc()
        {
            ComboBox cb = view.GetCbDanhMuc();

            cb.Items.Clear();

            cb.Items.Add("Danh mục");
            cb.Items.Add("Tất cả");

            DanhMucDAO dao = new DanhMucDAO();

            foreach (string[] dm in dao.GetAll())
            {
                cb.Items.Add(dm[1]);
            }

            cb.SelectedIndex = 0;
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
                UnicodeCategory uc =
                    CharUnicodeInfo.GetUnicodeCategory(c);

                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().ToLower();
        }

        // ================= LOAD MENU =================
        public void LoadMenu()
        {
            view.ClearMenu();

            List<MonAn> list = menuDao.GetAll();

            string keyword = view.GetTxtSearch().Text.Trim();
            if (keyword == "🔍 Tìm kiếm món ăn..." || keyword.Contains("🔍"))
            {
                keyword = "";
            }

            string danhMuc =
                view.GetCbDanhMuc().SelectedItem != null
                ? view.GetCbDanhMuc().SelectedItem.ToString()
                : "Danh mục";

            foreach (MonAn m in list)
            {
                bool matchName =
                    RemoveAccent(m.TenMon)
                    .Contains(RemoveAccent(keyword));

                bool matchDanhMuc =
                    danhMuc == "Danh mục"
                    || danhMuc == "Tất cả"
                    || (
                        m.TenDanhMuc != null &&
                        RemoveAccent(m.TenDanhMuc)
                        == RemoveAccent(danhMuc)
                    );

                if (matchName && matchDanhMuc)
                {
                    view.AddMonCard(
                        m.MaMon,
                        m.TenMon,
                        m.DonGia,
                        m.HinhAnh
                    );
                }
            }

            view.Refresh();
        }

        // ================= XOÁ GIỎ HÀNG =================
        private void XoaHetGioHang()
        {
            if (view.GetCartData().Count == 0)
                return;

            DialogResult rs = MessageBox.Show(
                "Bạn có muốn làm trống giỏ hàng?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (rs == DialogResult.Yes)
            {
                view.ClearCart();
            }
        }

        // ================= UPDATE TỔNG TIỀN =================
        private void UpdateTongTien()
        {
            currentTotal = 0;

            List<object[]> data = view.GetCartData();

            foreach (object[] item in data)
            {
                int sl = Convert.ToInt32(item[2]);

                double gia =
                    Convert.ToDouble(item[3]);

                currentTotal += sl * gia;
            }

            view.SetTongTien(currentTotal);
        }

        // ================= THANH TOÁN =================
        private void MoPopupThanhToan()
        {
            List<object[]> data = view.GetCartData();

            if (data.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!");
                return;
            }

            UpdateTongTien();

            XacNhanThanhToanDialog dialog =
                new XacNhanThanhToanDialog(
                    currentTotal,
                    view.GetCartData()
                );

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string tenKhach = dialog.GetTenKhach();
                string sdt = dialog.GetSDT();

                if (string.IsNullOrWhiteSpace(tenKhach))
                {
                    MessageBox.Show("Nhập tên khách!");
                    return;
                }

                LuuHoaDonVaoDB(tenKhach, sdt);
            }
        }

        // ================= LƯU HÓA ĐƠN =================
        private void LuuHoaDonVaoDB(
            string tenKhach,
            string sdt)
        {
            MySqlConnection conn = null;
            MySqlTransaction trans = null;

            try
            {
                conn = DBConnection.GetConnection();


                trans = conn.BeginTransaction();

                string maKH = null;

                // ================= KHÁCH HÀNG =================
                if (!string.IsNullOrWhiteSpace(sdt))
                {
                    string sqlCheck =
                        "SELECT MaKH FROM KhachHang " +
                        "WHERE SoDienThoai = @sdt";

                    MySqlCommand cmdCheck =
                        new MySqlCommand(sqlCheck, conn, trans);

                    cmdCheck.Parameters.AddWithValue(
                        "@sdt",
                        sdt
                    );

                    object result = cmdCheck.ExecuteScalar();

                    if (result != null)
                    {
                        maKH = result.ToString();
                    }
                    else
                    {
                        maKH = khachHangDAO.GetNewID();

                        string sqlInsertKH =
                            "INSERT INTO KhachHang " +
                            "(MaKH, TenKH, TheLoai, GioiTinh, SoDienThoai) " +
                            "VALUES (@makh,@tenkh,@theloai,@gioitinh,@sdt)";

                        MySqlCommand cmdKH =
                            new MySqlCommand(sqlInsertKH, conn, trans);

                        cmdKH.Parameters.AddWithValue("@makh", maKH);
                        cmdKH.Parameters.AddWithValue("@tenkh", tenKhach);
                        cmdKH.Parameters.AddWithValue("@theloai", "Vãng lai");
                        cmdKH.Parameters.AddWithValue("@gioitinh", "Khác");
                        cmdKH.Parameters.AddWithValue("@sdt", sdt);

                        cmdKH.ExecuteNonQuery();
                    }
                }

                // ================= HÓA ĐƠN =================
                string maHD = hoaDonDao.GetNewID();

                string ngayLap =
                    DateTime.Now.ToString("dd/MM/yyyy");

                string sqlHD =
                    "INSERT INTO HoaDon " +
                    "(MaHD, MaNV, MaKH, NgayLap, TongTien) " +
                    "VALUES (@mahd, " +
                    "(SELECT MaNV FROM NhanVien WHERE TenNV = @tennv LIMIT 1), " +
                    "@makh, @ngaylap, @tongtien)";

                MySqlCommand cmdHD =
                    new MySqlCommand(sqlHD, conn, trans);

                cmdHD.Parameters.AddWithValue("@mahd", maHD);
                cmdHD.Parameters.AddWithValue("@tennv", Session.TenNV);
                cmdHD.Parameters.AddWithValue("@makh", string.IsNullOrEmpty(maKH) ? (object)DBNull.Value : maKH);
                cmdHD.Parameters.AddWithValue("@ngaylap", ngayLap);
                cmdHD.Parameters.AddWithValue("@tongtien", currentTotal);

                cmdHD.ExecuteNonQuery();

                // ================= CHI TIẾT HÓA ĐƠN =================
                string sqlCT =
                    "INSERT INTO ChiTietHoaDon " +
                    "(MaHD, MaMon, Size, SoLuong, DonGia) " +
                    "VALUES (@mahd, (SELECT MaMon FROM ThucDon WHERE TenMon = @tenmon LIMIT 1), @size, @soluong, @dongia)";

                foreach (object[] item in view.GetCartData())
                {
                    MySqlCommand cmdCT =
                        new MySqlCommand(sqlCT, conn, trans);

                    cmdCT.Parameters.AddWithValue("@mahd", maHD);
                    cmdCT.Parameters.AddWithValue("@tenmon", item[0].ToString());
                    cmdCT.Parameters.AddWithValue("@size", item[1].ToString());
                    cmdCT.Parameters.AddWithValue("@soluong",
                        Convert.ToInt32(item[2]));
                    cmdCT.Parameters.AddWithValue("@dongia",
                        Convert.ToDouble(item[3]));

                    cmdCT.ExecuteNonQuery();
                }

                // ================= COMMIT =================
                trans.Commit();

                MessageBox.Show(
                    "Thanh toán thành công! Mã HĐ: " + maHD
                );

                view.ClearCart();

                LoadMenu();

                if (hoaDonController != null)
                {
                    hoaDonController.LoadData("");
                }

                if (khachHangController != null)
                {
                    khachHangController.LoadData();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    trans?.Rollback();
                }
                catch { }

                MessageBox.Show(
                    "Lỗi thanh toán!\n" + ex.Message
                );
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        // ================= PUBLIC METHODS =================
        public void ReloadDanhMuc()
        {
            LoadDanhMuc();
        }

        public void SetKhachHangController(
            KhachHangController controller)
        {
            khachHangController = controller;
        }
    }
}