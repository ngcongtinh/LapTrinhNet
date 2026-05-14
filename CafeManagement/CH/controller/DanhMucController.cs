using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CafeManagement.CH.dao;
using CH.controller;
using CH.dao;
using CH.View;

namespace CH.Controller
{
    public class DanhMucController
    {
        private DanhMucView view;
        private DanhMucDAO dao;
        private ThucDonController thucDonController;
        private DatMonController datMonController;
        private bool isEdit = false;

        public void SetDatMonController(DatMonController c) => this.datMonController = c;
        public void SetThucDonController(ThucDonController c) => this.thucDonController = c;

        public DanhMucController(DanhMucView view)
        {
            this.view = view;
            this.dao = new DanhMucDAO();

            LoadDataToView();

            // Event: Thêm mới
            view.BtnThem.Click += (s, e) => {
                isEdit = false;
                view.ClearForm();
                view.GetDialogForm().Text = "Thêm Danh Mục Mới";
                view.GetDialogForm().ShowDialog();
            };

            // Event: Sửa (Được gọi từ Menu chuột phải trong View)
            view.BtnSua.Click += (s, e) => {
                if (view.GetTable().SelectedRows.Count > 0)
                {
                    isEdit = true;
                    var row = view.GetTable().SelectedRows[0];
                    string ma = row.Cells["MaDM"].Value.ToString();
                    string ten = row.Cells["TenDM"].Value.ToString();

                    view.SetForm(ma, ten);
                    view.GetDialogForm().Text = "Chỉnh Sửa Danh Mục";
                    view.GetDialogForm().ShowDialog();
                }
            };

            // Event: Xóa (Được gọi từ Menu chuột phải trong View)
            view.BtnXoa.Click += (s, e) => {
                if (view.GetTable().SelectedRows.Count > 0)
                {
                    string ma = view.GetTable().SelectedRows[0].Cells["MaDM"].Value.ToString();
                    DeleteData(ma);
                }
            };

            // Event: Lưu
            view.BtnLuu.Click += SaveData;

            // Event: Tìm kiếm
            view.GetTxtSearch().TextChanged += (s, e) =>
            {
                string keyword = view.GetTxtSearch().Text.Trim();
                if (keyword == "🔍 Tìm kiếm danh mục..." || string.IsNullOrEmpty(keyword))
                {
                    LoadDataToView();
                }
                else
                {
                    LoadDataWithFilter(keyword);
                }
            };
        }

        private void SaveData(object sender, EventArgs e)
        {
            string ten = view.GetTenDM().Trim();
            string ma = view.GetMaDM();

            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trùng tên (Ngoại trừ trường hợp đang sửa chính nó)
            if (dao.IsExistsTenDanhMuc(ten))
            {
                if (!isEdit || (isEdit && !ten.Equals(GetTenCuFromTable(ma))))
                {
                    MessageBox.Show("Tên danh mục này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            bool success = isEdit ? dao.Update(ma, ten) : dao.Insert(TaoMaTuDong(), ten);

            if (success)
            {
                view.GetDialogForm().Close();
                LoadDataToView();
                RefreshOtherScreens();
            }
            else
            {
                MessageBox.Show("Thao tác thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteData(string ma)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục {ma}",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dao.Delete(ma))
                {
                    LoadDataToView();
                    RefreshOtherScreens();
                }
                else
                {
                    MessageBox.Show("Không thể xóa danh mục này (có thể đang chứa món ăn)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadDataToView()
        {
            view.ClearTable();
            foreach (string[] dm in dao.GetAll()) view.AddRow(dm);
            view.GetTable().ClearSelection();
            view.GetTable().CurrentCell = null;
        }

        private void LoadDataWithFilter(string keyword)
        {
            view.ClearTable();
            string key = RemoveAccent(keyword.ToLower());
            foreach (string[] dm in dao.GetAll())
            {
                if (RemoveAccent(dm[1].ToLower()).Contains(key) || dm[0].ToLower().Contains(key))
                    view.AddRow(dm);
            }

        }

        private string GetTenCuFromTable(string ma)
        {
            foreach (DataGridViewRow row in view.GetTable().Rows)
            {
                if (row.Cells["MaDM"].Value?.ToString() == ma)
                    return row.Cells["TenDM"].Value?.ToString();
            }
            return "";
        }

        private string TaoMaTuDong()
        {
            var list = dao.GetAll();
            int max = 0;
            foreach (var dm in list)
            {
                if (dm[0].Length > 2 && int.TryParse(dm[0].Substring(2), out int so))
                {
                    if (so > max) max = so;
                }
            }
            return $"DM{(max + 1):00}";
        }

        private void RefreshOtherScreens()
        {
            thucDonController?.LoadDanhMucToComboBox();
            datMonController?.ReloadDanhMuc();
        }

        private string RemoveAccent(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in normalized)
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            return sb.ToString().Replace('đ', 'd').Replace('Đ', 'D');
        }
    }
}