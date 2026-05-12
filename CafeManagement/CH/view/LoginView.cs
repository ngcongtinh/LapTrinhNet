using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public class LoginView : Form
    {
        private readonly Color PRIMARY_COLOR = Color.FromArgb(0, 120, 215);
        private readonly Color BACKGROUND_COLOR = Color.FromArgb(240, 242, 245);
        private readonly Color PANEL_COLOR = Color.White;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;

        public LoginView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Đăng nhập hệ thống";
            Size = new Size(500, 420);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = BACKGROUND_COLOR;

            // ================= CONTAINER =================
            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BACKGROUND_COLOR
            };

            Controls.Add(container);

            // ================= CARD =================
            Panel card = new Panel
            {
                Size = new Size(400, 300),
                BackColor = PANEL_COLOR,
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Location = new Point(
                (ClientSize.Width - card.Width) / 2,
                (ClientSize.Height - card.Height) / 2
            );

            container.Controls.Add(card);

            // ================= TITLE =================
            Label lblTitle = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ QUÁN DINOCOFFEE",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = PRIMARY_COLOR,
                AutoSize = false,
                Width = 360,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20)
            };

            card.Controls.Add(lblTitle);

            // ================= USERNAME =================
            txtUsername = CreateTextField();
            txtUsername.Location = new Point(40, 90);
            txtUsername.Text = "admin";

            Label lblUser = CreateLabel("Tên đăng nhập");
            lblUser.Location = new Point(40, 70);

            card.Controls.Add(lblUser);
            card.Controls.Add(txtUsername);

            // ================= PASSWORD =================
            txtPassword = CreateTextField();
            txtPassword.Location = new Point(40, 165);
            txtPassword.PasswordChar = '•';

            Label lblPass = CreateLabel("Mật khẩu");
            lblPass.Location = new Point(40, 145);

            card.Controls.Add(lblPass);
            card.Controls.Add(txtPassword);

            // ================= SHOW PASSWORD =================
            chkShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Font = new Font("Segoe UI", 10),
                BackColor = PANEL_COLOR,
                AutoSize = true,
                Location = new Point(40, 205)
            };

            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.PasswordChar =
                    chkShowPassword.Checked ? '\0' : '•';
            };

            card.Controls.Add(chkShowPassword);

            // ================= LOGIN BUTTON =================
            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = PRIMARY_COLOR,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(320, 45),
                Location = new Point(40, 240),
                Cursor = Cursors.Hand
            };

            btnLogin.FlatAppearance.BorderSize = 0;

            // Hover effect
            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(30, 144, 255);
            };

            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = PRIMARY_COLOR;
            };

            card.Controls.Add(btnLogin);

            AcceptButton = btnLogin;
        }

        // =====================================================
        // SUPPORT UI
        // =====================================================

        private TextBox CreateTextField()
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Width = 320,
                Height = 35
            };
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true
            };
        }

        // =====================================================
        // GETTERS
        // =====================================================

        public Button GetBtnLogin()
        {
            return btnLogin;
        }

        public TextBox GetTxtUsername()
        {
            return txtUsername;
        }

        public TextBox GetTxtPassword()
        {
            return txtPassword;
        }
    }
}