using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CH.dao;
using CH.Model;
using CH.View;

namespace CH.Controller
{
    public class KhachHangController
    {
        private KhachHangView view;
        private KhachHangDAO dao;
        private bool isEdit = false;

        public KhachHangController(KhachHangView view)
        {
            this.view = view;
            this.dao = new KhachHangDAO();

            // Load dữ liệu ban đầu
            LoadData("");

            // Đăng ký các sự kiện
            InitEvents();
        }

        private void InitEvents()
        {
            // ================= SEARCH =================
            view.GetTxtSearch().TextChanged += (s, e) =>
            {
                string keyword = view.GetTxtSearch().Text.Trim();
                // Loại bỏ placeholder khi tìm kiếm
                if (keyword.Contains("🔍")) keyword = "";
                LoadData(keyword);
            };

            // ================= THÊM =================
            view.BtnThem.Click += (s, e) =>
            {
                isEdit = false;
                view.ClearForm();
              
                view.DialogForm.Text = "Thêm Khách Hàng Mới";
                view.DialogForm.ShowDialog();
            };

            // ================= SỬA =================
            view.BtnSua.Click += (s, e) =>
            {
                // Sử dụng tên biến 'Table' mới
                if (view.Table.CurrentRow == null) return;

                isEdit = true;
                string maKH = view.Table.CurrentRow.Cells[0].Value.ToString();
                KhachHang kh = dao.GetById(maKH);

                if (kh != null)
                {
                    view.FillForm(kh);
                    view.DialogForm.Text = "Cập Nhật Khách Hàng";
                    view.DialogForm.ShowDialog();
                }
            };

            // ================= XÓA =================
            view.BtnXoa.Click += (s, e) =>
            {
                if (view.Table.CurrentRow == null) return;

                string maKH = view.Table.CurrentRow.Cells[0].Value.ToString();
                string tenKH = view.Table.CurrentRow.Cells[1].Value.ToString();

                DialogResult rs = MessageBox.Show(
                    $"Bạn có chắc muốn xóa khách hàng [{tenKH}]?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (rs == DialogResult.Yes)
                {
                    if (dao.Delete(maKH))
                    {
                        MessageBox.Show("Đã xóa khách hàng thành công.");
                        LoadData("");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không thể xóa khách hàng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            // ================= LƯU =================
            view.BtnLuu.Click += (s, e) => HandleSave();

            // Bắt sự kiện Click vào cột Action trên Table (Nếu bạn dùng nút vẽ trong cell)
            view.Table.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                // Giả sử cột nút bấm là cột cuối cùng
                if (e.ColumnIndex == view.Table.ColumnCount - 1)
                {
                    ShowActionMenu();
                }
            };
        }

        private void ShowActionMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem edit = new ToolStripMenuItem("Sửa thông tin");
            ToolStripMenuItem delete = new ToolStripMenuItem("Xóa khách hàng");

            edit.Click += (s, ev) => view.BtnSua.PerformClick();
            delete.Click += (s, ev) => view.BtnXoa.PerformClick();

            menu.Items.AddRange(new ToolStripItem[] { edit, delete });
            menu.Show(Cursor.Position);
        }

        private void HandleSave()
        {
            KhachHang kh = view.GetKhachHangInfo();

            // Validate dữ liệu (Đồng bộ logic validate với NhanVien)
            if (string.IsNullOrWhiteSpace(kh.TenKH) || string.IsNullOrWhiteSpace(kh.SoDienThoai))
            {
                MessageBox.Show("Vui lòng nhập các thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(kh.SoDienThoai, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (10 số, bắt đầu bằng 0)!");
                return;
            }

            // Kiểm tra trùng SĐT
            if (!isEdit)
            {
                if (dao.IsExistsSdt(kh.SoDienThoai))
                {
                    MessageBox.Show("Số điện thoại này đã được đăng ký!");
                    return;
                }

                if (dao.Insert(kh))
                {
                    MessageBox.Show("Thêm mới khách hàng thành công.");
                    view.DialogForm.Close();
                    LoadData("");
                }
            }
            else
            {
                // Kiểm tra trùng SĐT khi sửa (nếu SĐT bị thay đổi)
                KhachHang old = dao.GetById(kh.MaKH);
                if (old != null && old.SoDienThoai != kh.SoDienThoai && dao.IsExistsSdt(kh.SoDienThoai))
                {
                    MessageBox.Show("Số điện thoại mới đã tồn tại!");
                    return;
                }

                if (dao.Update(kh))
                {
                    MessageBox.Show("Cập nhật thông tin thành công.");
                    view.DialogForm.Close();
                    LoadData("");
                }
            }
        }

        public void LoadData(string keyword = "")
        {
            view.ClearTable();
            string searchKey = RemoveAccent(keyword.ToLower().Trim());
            List<KhachHang> list = dao.GetAll();

            foreach (KhachHang kh in list)
            {
                string ten = RemoveAccent(kh.TenKH.ToLower());
                string ma = kh.MaKH.ToLower();
                string sdt = kh.SoDienThoai;

                if (string.IsNullOrEmpty(searchKey) || ten.Contains(searchKey) || ma.Contains(searchKey) || sdt.Contains(searchKey))
                {
                    view.AddRow(kh);
                }
            }
        }

        private string RemoveAccent(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string normalized = text.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string result = regex.Replace(normalized, "").Replace('đ', 'd').Replace('Đ', 'D');
            return result;
        }
    }
}