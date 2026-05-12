using System;
using System.Windows.Forms;
using CafeManagement.CH.dao;
using CH.dao;
using CH.Model;
using CH.view;
using CH.View;

namespace CH.Controller
{
    public class LoginController
    {
        private LoginView view;
        private NhanVienDAO nhanVienDAO;

        public LoginController(LoginView view)
        {
            this.view = view;
            this.nhanVienDAO = new NhanVienDAO();

            InitEvents();
        }

        private void InitEvents()
        {
            // Sự kiện nút đăng nhập
            view.GetBtnLogin().Click += (s, e) => HandleLogin();

            // Nhấn Enter ở ô mật khẩu
            view.GetTxtPassword().KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    HandleLogin();
                }
            };
        }

        private void HandleLogin()
        {
            string username = view.GetTxtUsername().Text.Trim();
            string password = view.GetTxtPassword().Text;

            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập tài khoản và mật khẩu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Kiểm tra kết nối database
            var conn = DBConnection.GetConnection();

            if (conn == null)
            {
                MessageBox.Show(
                    "Không thể kết nối cơ sở dữ liệu!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Kiểm tra đăng nhập
            NhanVien nv = nhanVienDAO.LoginNV(username, password);

            if (nv != null)
            {
                // ================= SESSION =================
                Session.Username = nv.Username;
                Session.Role = nv.Role;
                Session.TenNV = nv.TenNV;

                // ================= MAIN VIEW =================
                MainView mainView = new MainView();

                mainView.SetRole(nv.Role);

                mainView.Show();

                view.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Tài khoản hoặc mật khẩu không đúng!",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}