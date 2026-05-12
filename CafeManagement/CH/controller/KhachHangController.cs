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
    public class KhachHangController
    {
        private KhachHangView view;
        private KhachHangDAO dao;

        private bool isEdit = false;

        public KhachHangController(KhachHangView view)
        {
            this.view = view;
            this.dao = new KhachHangDAO();

            // Load dữ liệu
            LoadData("");

            // ================= SEARCH =================
            view.TxtSearch.TextChanged += (s, e) =>
            {
                string keyword = view.GetTxtSearch().Text.Trim();

                if (keyword == "🔍 Tìm tên hoặc mã khách hàng...")
                    keyword = "";

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
                int row = view.TableKhachHang.CurrentRow?.Index ?? -1;

                if (row >= 0)
                {
                    isEdit = true;

                    string maKH = view.TableKhachHang.Rows[row]
                        .Cells[0].Value.ToString();

                    KhachHang kh = dao.GetById(maKH);

                    if (kh != null)
                    {
                        view.FillForm(kh);

                        view.DialogForm.Text = "Cập Nhật Khách Hàng";
                        view.DialogForm.ShowDialog();
                    }
                }
            };

            // ================= XÓA =================
            view.BtnXoa.Click += (s, e) =>
            {
                int row = view.TableKhachHang.CurrentRow?.Index ?? -1;

                if (row >= 0)
                {
                    string maKH = view.TableKhachHang.Rows[row]
                        .Cells[0].Value.ToString();

                    string tenKH = view.TableKhachHang.Rows[row]
                        .Cells[1].Value.ToString();

                    DialogResult rs = MessageBox.Show(
                        $"Bạn có chắc muốn xóa khách hàng [{tenKH}] ?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (rs == DialogResult.Yes)
                    {
                        if (dao.Delete(maKH))
                        {
                            MessageBox.Show(
                                "Xóa khách hàng thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            LoadData("");
                        }
                        else
                        {
                            MessageBox.Show(
                                "Không thể xóa khách hàng!",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            };

            // ================= LƯU =================
            view.BtnLuu.Click += (s, e) =>
            {
                KhachHang kh = view.GetKhachHangInfo();

                string ten = kh.TenKH?.Trim() ?? "";
                string loai = kh.TheLoai?.Trim() ?? "";
                string email = kh.Email?.Trim() ?? "";
                string sdt = kh.SoDienThoai?.Trim() ?? "";
                string diaChi = kh.DiaChi?.Trim() ?? "";

                // Validate rỗng
                if (ten == "" ||
                    loai == "" ||
                    email == "" ||
                    sdt == "" ||
                    diaChi == "")
                {
                    MessageBox.Show(
                        "Vui lòng nhập đầy đủ thông tin!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Validate tên
                if (!Regex.IsMatch(ten, @"^[\p{L} ]+$"))
                {
                    MessageBox.Show("Tên không hợp lệ!");
                    return;
                }

                // Validate giới tính
                if (!view.RdoNam.Checked &&
                    !view.RdoNu.Checked)
                {
                    MessageBox.Show("Vui lòng chọn giới tính!");
                    return;
                }

                // Validate email
                if (!Regex.IsMatch(
                        email,
                        @"^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\.[a-z]{2,6}$"))
                {
                    MessageBox.Show("Email không hợp lệ!");
                    return;
                }

                // Validate SĐT
                if (!Regex.IsMatch(sdt, @"^0\d{9}$"))
                {
                    MessageBox.Show(
                        "Số điện thoại phải đủ 10 số và bắt đầu bằng 0!"
                    );
                    return;
                }

                // ================= CHECK TRÙNG =================
                if (!isEdit)
                {
                    if (dao.IsExistsSdt(sdt))
                    {
                        MessageBox.Show(
                            "Số điện thoại đã tồn tại!"
                        );
                        return;
                    }
                }
                else
                {
                    KhachHang oldKh = dao.GetById(kh.MaKH);

                    if (oldKh != null &&
                        oldKh.SoDienThoai != kh.SoDienThoai)
                    {
                        if (dao.IsExistsSdt(sdt))
                        {
                            MessageBox.Show(
                                "Số điện thoại mới đã tồn tại!"
                            );
                            return;
                        }
                    }
                }

                // ================= SAVE =================
                if (isEdit)
                {
                    if (dao.Update(kh))
                    {
                        MessageBox.Show(
                            "Cập nhật thành công!"
                        );

                        view.DialogForm.Close();

                        LoadData("");
                    }
                    else
                    {
                        MessageBox.Show(
                            "Cập nhật thất bại!"
                        );
                    }
                }
                else
                {
                    if (dao.Insert(kh))
                    {
                        MessageBox.Show(
                            "Thêm khách hàng thành công!"
                        );

                        view.DialogForm.Close();

                        LoadData("");
                    }
                    else
                    {
                        MessageBox.Show(
                            "Không thể thêm khách hàng!"
                        );
                    }
                }
            };
        }

        // ================= LOAD DATA =================
        private void LoadData(string keyword)
        {
            view.ClearTable();

            string searchKey = RemoveAccent(
                keyword.ToLower().Trim()
            );

            List<KhachHang> list = dao.GetAll();

            foreach (KhachHang kh in list)
            {
                string ten = RemoveAccent(
                    kh.TenKH.ToLower()
                );

                string ma = kh.MaKH.ToLower();

                string sdt = kh.SoDienThoai;

                bool match =
                    string.IsNullOrEmpty(searchKey) ||
                    ten.Contains(searchKey) ||
                    ma.Contains(searchKey) ||
                    sdt.Contains(searchKey);

                if (match)
                {
                    view.AddRow(kh);
                }
            }
        }

        // ================= REMOVE ACCENT =================
        private string RemoveAccent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            string normalized = text.Normalize(
                NormalizationForm.FormD
            );

            Regex regex = new Regex(
                "\\p{IsCombiningDiacriticalMarks}+"
            );

            string result = regex.Replace(normalized, "");

            result = result.Replace('đ', 'd')
                           .Replace('Đ', 'D');

            return result;
        }

        // Public reload
        public void LoadData()
        {
            LoadData("");
        }
    }
}