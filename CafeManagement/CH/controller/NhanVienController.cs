using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CH.dao;
using CH.Model;
using CH.View;

namespace CH.Controller
{
    public class NhanVienController
    {
        private NhanVienView view;
        private NhanVienDAO nhanVienDAO;
        private bool isEdit = false;

        public NhanVienController(NhanVienView view)
        {
            this.view = view;
            this.nhanVienDAO = new NhanVienDAO();

            LoadDataToView("");

            // ===== TÌM KIẾM =====
            view.TxtSearch.TextChanged += (s, e) =>
            {
                string keyword = view.TxtSearch.Text;

                if (keyword.Contains("🔍"))
                    LoadDataToView("");
                else
                    LoadDataToView(keyword.Trim());
            };

            // ===== THÊM =====
            view.BtnThem.Click += (s, e) =>
            {
                isEdit = false;

                view.ClearForm();
                view.DialogForm.Text = "Thêm nhân viên mới";
                view.DialogForm.ShowDialog();
            };

            // ===== SỬA =====
            view.BtnSua.Click += (s, e) =>
            {
                if (view.Table.SelectedRows.Count > 0)
                {
                    isEdit = true;

                    string maNV = view.Table.SelectedRows[0].Cells[0].Value.ToString();

                    NhanVien nv = nhanVienDAO.GetByID(maNV);

                    if (nv != null)
                    {
                        view.FillForm(nv);
                        view.DialogForm.Text = "Chỉnh sửa thông tin nhân viên";
                        view.DialogForm.ShowDialog();
                    }
                }
            };

            // ===== XÓA =====
            view.BtnXoa.Click += (s, e) =>
            {
                if (view.Table.SelectedRows.Count > 0)
                {
                    string maNV = view.Table.SelectedRows[0].Cells[0].Value.ToString();

                    DialogResult confirm = MessageBox.Show(
                        "Bạn có chắc chắn muốn xóa nhân viên " + maNV + "?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        if (nhanVienDAO.Delete(maNV))
                        {
                            MessageBox.Show("Xóa thành công!");
                            LoadDataToView("");
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại!");
                        }
                    }
                }
            };

            // ===== LƯU =====
            view.BtnLuu.Click += (s, e) =>
            {
                NhanVien nv = view.GetNhanVienInfo();

                // ===== VALIDATE =====
                if (string.IsNullOrWhiteSpace(nv.TenNV) ||
                    string.IsNullOrWhiteSpace(nv.NgaySinh) ||
                    string.IsNullOrWhiteSpace(nv.ChucVu) ||
                    string.IsNullOrWhiteSpace(nv.SoDienThoai) ||
                    string.IsNullOrWhiteSpace(nv.Username) ||
                    string.IsNullOrWhiteSpace(nv.Password))
                {
                    MessageBox.Show(
                        "Vui lòng nhập đầy đủ thông tin có dấu (*)",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Validate tên
                if (!Regex.IsMatch(nv.TenNV, @"^[\p{L} ]+$"))
                {
                    MessageBox.Show(
                        "Tên nhân viên không được chứa ký tự đặc biệt!"
                    );
                    return;
                }

                // Validate giới tính
                if (!view.RdoNam.Checked && !view.RdoNu.Checked)
                {
                    MessageBox.Show("Vui lòng chọn giới tính!");
                    return;
                }

                // Validate SĐT
                if (!Regex.IsMatch(nv.SoDienThoai, @"^0\d{9}$"))
                {
                    MessageBox.Show(
                        "SĐT không hợp lệ (Phải có 10 chữ số và bắt đầu bằng 0)!"
                    );
                    return;
                }

                // Password
                if (!isEdit && string.IsNullOrWhiteSpace(nv.Password))
                {
                    MessageBox.Show(
                        "Mật khẩu cho nhân viên mới không được để trống!"
                    );
                    return;
                }

                // ===== CHECK TRÙNG =====
                if (!isEdit)
                {
                    if (nhanVienDAO.IsExistsSdt(nv.SoDienThoai))
                    {
                        MessageBox.Show(
                            "Số điện thoại này đã thuộc về nhân viên khác!"
                        );
                        return;
                    }

                    if (nhanVienDAO.IsExistsUsername(nv.Username))
                    {
                        MessageBox.Show(
                            "Tên đăng nhập (Username) đã tồn tại!"
                        );
                        return;
                    }
                }
                else
                {
                    NhanVien oldNv = nhanVienDAO.GetByID(nv.MaNV);

                    if (oldNv != null)
                    {
                        // Check SĐT
                        if (oldNv.SoDienThoai != nv.SoDienThoai &&
                            nhanVienDAO.IsExistsSdt(nv.SoDienThoai))
                        {
                            MessageBox.Show(
                                "Số điện thoại mới bị trùng với nhân viên khác!"
                            );
                            return;
                        }

                        // Check Username
                        if (oldNv.Username != nv.Username &&
                            nhanVienDAO.IsExistsUsername(nv.Username))
                        {
                            MessageBox.Show(
                                "Username mới đã tồn tại trên hệ thống!"
                            );
                            return;
                        }
                    }
                }

                // ===== LƯU DB =====
                bool thanhCong = isEdit
                    ? nhanVienDAO.Update(nv)
                    : nhanVienDAO.Insert(nv);

                if (thanhCong)
                {
                    MessageBox.Show(
                        (isEdit ? "Cập nhật" : "Thêm mới") +
                        " nhân viên thành công!"
                    );

                    view.DialogForm.Close();

                    LoadDataToView("");
                }
                else
                {
                    MessageBox.Show(
                        "Thao tác thất bại. Vui lòng kiểm tra lại Database!"
                    );
                }
            };
        }

        // ===== LOAD DATA =====
        private void LoadDataToView(string keyword)
        {
            view.ClearTable();

            List<NhanVien> list;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                list = nhanVienDAO.GetAll();
            }
            else
            {
                list = nhanVienDAO.Search(keyword);
            }

            foreach (NhanVien nv in list)
            {
                view.AddRow(new object[]
                {
                    nv.MaNV,
                    nv.TenNV,
                    nv.NgaySinh,
                    nv.GioiTinh,
                    nv.ChucVu,
                    nv.SoDienThoai,
                    ""
                });
            }
            view.Table.ClearSelection();
            view.Table.CurrentCell = null;
        }
    }
}