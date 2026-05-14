using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CafeManagement.CH.dao;
using CH.controller;
using CH.dao;
using CH.Model;
using CH.view;
using CH.View;

namespace CH.Controller
{
    public class ThucDonController
    {
        private ThucDonView view;
        private ThucDonDAO dao;
        private DatMonController datMonController;
        private DanhMucDAO danhMucDAO = new DanhMucDAO();

        private string currentMaMon = null;
        private bool isEdit = false;

        public ThucDonController(ThucDonView view, DatMonController datMonController)
        {
            this.view = view;
            this.dao = new ThucDonDAO();
            this.datMonController = datMonController;

            LoadData("");
            LoadDanhMucToComboBox();

            // ===== SEARCH =====
            view.TxtSearch.TextChanged += (s, e) =>
            {
                string keyword = view.TxtSearch.Text.Trim();

                if (keyword == "🔍 Tìm kiếm theo tên món...")
                    keyword = "";

                LoadData(keyword);
            };

            // ===== THÊM =====
            view.BtnThem.Click += (s, e) =>
            {
                isEdit = false;

                view.ClearForm();

                view.DialogForm.Text = "Thêm món mới";
                view.DialogForm.StartPosition = FormStartPosition.CenterScreen;
                view.DialogForm.ShowDialog();
            };

            // ===== SỬA =====
            view.BtnSua.Click += (s, e) =>
            {
                if (view.Table.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn món để sửa!");
                    return;
                }

                isEdit = true;

                string ma = view.Table.CurrentRow.Cells[0].Value.ToString();
                currentMaMon = ma;

                MonAn m = dao.GetByID(ma);

                if (m != null)
                {
                    view.FillForm(m);

                    view.DialogForm.Text = "Chỉnh sửa món";
                    view.DialogForm.StartPosition = FormStartPosition.CenterScreen;
                    view.DialogForm.ShowDialog();
                }
            };

            // ===== LƯU =====
            view.BtnLuu.Click += (s, e) =>
            {
                MonAn m = view.GetMonAnInfo();

                // Validate tên món
                if (string.IsNullOrWhiteSpace(m.TenMon))
                {
                    MessageBox.Show("Tên món không được để trống!");
                    return;
                }

                if (!Regex.IsMatch(m.TenMon, @"^[\p{L}\s]+$"))
                {
                    MessageBox.Show("Tên món không được chứa ký tự đặc biệt!");
                    return;
                }

                // Validate giá
                if (m.DonGia <= 0)
                {
                    MessageBox.Show("Giá phải lớn hơn 0!");
                    return;
                }

                // Validate đơn vị tính
                if (string.IsNullOrWhiteSpace(m.DonViTinh))
                {
                    MessageBox.Show("Đơn vị tính không được để trống!");
                    return;
                }

                // Validate danh mục
                if (view.GetCboDanhMuc().SelectedIndex <= 0)
                {
                    MessageBox.Show("Vui lòng chọn danh mục!");
                    return;
                }

                // ===== CHECK TRÙNG =====
                if (!isEdit)
                {
                    if (dao.IsExistsTenMon(m.TenMon.Trim()))
                    {
                        MessageBox.Show("Tên món ăn đã tồn tại!");
                        return;
                    }
                }
                else
                {
                    MonAn oldMon = dao.GetByID(currentMaMon);

                    if (oldMon != null &&
                        !oldMon.TenMon.Equals(m.TenMon.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        if (dao.IsExistsTenMon(m.TenMon.Trim()))
                        {
                            MessageBox.Show("Tên món mới bị trùng!");
                            return;
                        }
                    }
                }

                // ===== SAVE =====
                bool success;

                if (isEdit)
                {
                    m.MaMon = currentMaMon;
                    success = dao.Update(m);
                }
                else
                {
                    m.MaMon = dao.GetNewID();
                    success = dao.Add(m);
                }

                if (success)
                {
                    MessageBox.Show(
                        (isEdit ? "Cập nhật" : "Thêm mới")
                        + " món ăn thành công!"
                    );

                    view.DialogForm.Close();

                    LoadData("");

                    Reload();

                    currentMaMon = null;
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            };

            // ===== XÓA =====
            view.BtnXoa.Click += (s, e) =>
            {
                if (view.Table.CurrentRow == null)
                {
                    MessageBox.Show("Chọn món cần xoá!");
                    return;
                }

                string ma = view.Table.CurrentRow.Cells[0].Value.ToString();

                DialogResult result = MessageBox.Show(
                    "Xóa món " + ma + " ?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (dao.Delete(ma))
                    {
                        Reload();

                        MessageBox.Show("Xóa thành công!");

                        LoadData("");
                    }
                }
            };

            // ===== CLICK TABLE =====
            view.Table.SelectionChanged += (s, e) =>
            {
                if (view.Table.CurrentRow != null)
                {
                    try
                    {
                        string ma = view.Table.CurrentRow.Cells[0].Value.ToString();

                        MonAn m = dao.GetByID(ma);

                        if (m != null)
                        {
                            view.FillForm(m);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };
        }

        // ===== REMOVE ACCENT =====
        private string RemoveAccent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            var normalized = text.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);

                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString()
                     .Replace('đ', 'd')
                     .Replace('Đ', 'D')
                     .Normalize(NormalizationForm.FormC);
        }

        // ===== LOAD DANH MỤC =====
        public void LoadDanhMucToComboBox()
        {
            view.GetCboDanhMuc().Items.Clear();

            view.GetCboDanhMuc().Items.Add("");

            foreach (string[] dm in danhMucDAO.GetAll())
            {
                view.GetCboDanhMuc().Items.Add(dm[1]);
            }
        }

        // ===== LOAD DATA =====
        private void LoadData(string keyword)
        {
            view.ClearTable();

            string searchKey = RemoveAccent(keyword.ToLower());

            foreach (MonAn m in dao.GetAll())
            {
                string tenMonKoDau = RemoveAccent(m.TenMon.ToLower());

                if (string.IsNullOrEmpty(searchKey)
                    || tenMonKoDau.Contains(searchKey))
                {
                    view.AddRow(m);
                }
            }
            view.Table.ClearSelection();
            view.Table.CurrentCell = null;
        }

        // ===== RELOAD =====
        private void Reload()
        {
            LoadData("");

            view.ClearForm();

            if (datMonController != null)
            {
                datMonController.LoadMenu();
            }
        }
    }
}