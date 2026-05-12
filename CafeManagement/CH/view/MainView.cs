using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CH.controller;
using CH.Controller;
using CH.Model;

namespace CH.View
{
    public partial class MainView : Form
    {
        // ==========================================
        // MODERN COLOR PALETTE (Bảng màu chuyên nghiệp)
        // ==========================================
        private readonly Color COLOR_SIDEBAR = Color.FromArgb(31, 41, 55);      // Gray-800
        private readonly Color COLOR_SIDEBAR_DARK = Color.FromArgb(17, 24, 39); // Gray-900
        private readonly Color COLOR_ACCENT = Color.FromArgb(16, 185, 129);     // Emerald-500
        private readonly Color COLOR_TEXT_MENU = Color.FromArgb(156, 163, 175); // Gray-400
        private readonly Color COLOR_HEADER = Color.White;
        private readonly Color COLOR_BG_CONTENT = Color.FromArgb(243, 244, 246);// Gray-100

        private Panel pnlContent, pnlSidebar, pnlHeader, pnlLogo;
        private Label lblRole, lblUser;
        private Dictionary<string, Button> menuButtons = new Dictionary<string, Button>();

        // Views
        private TrangChuView trangChuView;
        private DatMonView datMonView;
        private ThucDonView thucDonView;
        private NhanVienView nhanVienView;
        private KhachHangView khachHangView;
        private HoaDonView hoaDonView;
        private DoanhThuView doanhThuView;
        private DanhMucView danhMucView;

        public MainView()
        {
            InitUI();
            InitContent();

            ShowView("Trang chủ");
            UpdateActiveButton("Trang chủ");
        }

        private void InitUI()
        {
            this.Text = "DINO COFFEE - HỆ THỐNG QUẢN LÝ";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = COLOR_BG_CONTENT;
            this.Font = new Font("Segoe UI", 10);

            // --- SIDEBAR ---
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = COLOR_SIDEBAR
            };

            // Logo Section nằm trên cùng
            pnlLogo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = COLOR_SIDEBAR_DARK
            };
            lblRole = new Label
            {
                Text = "ADMIN",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlLogo.Controls.Add(lblRole);

            // Quan trọng: Thêm Logo vào Sidebar TRƯỚC
            pnlSidebar.Controls.Add(pnlLogo);

            // Menu Container nằm ngay dưới Logo
            FlowLayoutPanel menuFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, // Chiếm toàn bộ phần còn lại dưới Logo
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true, // Phòng trường hợp màn hình nhỏ vẫn cuộn được menu
                Padding = new Padding(0, 0, 0, 0)
            };

            // Thêm các nút vào menuFlow
            var menus = new[] {
                new { T = "Trang chủ", I = "🏠" },
                new { T = "Đặt Món",   I = "☕" },
                new { T = "Thực đơn",  I = "📖" },
                new { T = "Danh mục",  I = "🗂️" },
                new { T = "Nhân viên", I = "👤" },
                new { T = "Khách hàng",I = "👥" },
                new { T = "Hóa đơn",   I = "🧾" },
                new { T = "Doanh thu", I = "📊" },
                new { T = "Thoát",     I = "🚪" }
            };

            foreach (var m in menus)
            {
                Button btn = CreateMenuButton(m.I, m.T);
                menuFlow.Controls.Add(btn);
                menuButtons.Add(m.T, btn);
            }

            // Thêm menuFlow vào Sidebar SAU CÙNG để nó nằm dưới pnlLogo
            pnlSidebar.Controls.Add(menuFlow);
            menuFlow.BringToFront(); // Đảm bảo nó nổi lên trên nếu có control nào khác

            // --- 2. HEADER ---
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = COLOR_HEADER,
                Padding = new Padding(20, 0, 20, 0)
            };

            // Kẻ dòng dưới Header cho sang trọng
            Panel hr = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            pnlHeader.Controls.Add(hr);

            lblUser = new Label
            {
                Text = "Xin chào, Admin",
                Font = new Font("Segoe UI Semibold", 10),
                ForeColor = COLOR_SIDEBAR,
                AutoSize = true,
                Location = new Point(20, 25)
            };

            Button btnLogout = new Button
            {
                Text = "Đăng xuất",
                Size = new Size(110, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(220, 38, 38),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
            btnLogout.Location = new Point(pnlHeader.Width - 130, 17);
            btnLogout.Click += (s, e) => HandleLogout();

            // Responsive logout position
            pnlHeader.SizeChanged += (s, e) => { btnLogout.Left = pnlHeader.Width - 130; };

            pnlHeader.Controls.AddRange(new Control[] { lblUser, btnLogout });

            // --- 3. CONTENT ---
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_BG_CONTENT,
                Padding = new Padding(20) // Tạo lề cho các View bên trong
            };

            this.Controls.AddRange(new Control[] { pnlContent, pnlHeader, pnlSidebar });
        }

        private Button CreateMenuButton(string icon, string text)
        {
            Button btn = new Button
            {
                Text = $"{icon}   {text}", // Chỉ cần 1 khoảng cách nhỏ
                Width = 260,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = COLOR_TEXT_MENU,
                Font = new Font("Segoe UI", 11),
                TextAlign = ContentAlignment.MiddleLeft,
                // Dùng Padding để đẩy cả icon và chữ vào trong một khoảng bằng nhau
                Padding = new Padding(30, 0, 0, 0),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) => { if (btn.BackColor != COLOR_ACCENT) btn.BackColor = Color.FromArgb(55, 65, 81); };
            btn.MouseLeave += (s, e) => { if (btn.BackColor != COLOR_ACCENT) btn.BackColor = COLOR_SIDEBAR; };

            btn.Click += (s, e) => {
                if (text == "Thoát")
                {
                    if (MessageBox.Show("Bạn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) Application.Exit();
                }
                else
                {
                    ShowView(text);
                    UpdateActiveButton(text);
                }
            };
            return btn;
        }

        private void InitContent()
        {
            // Views
            trangChuView = new TrangChuView();
            datMonView = new DatMonView();
            thucDonView = new ThucDonView();
            nhanVienView = new NhanVienView();
            khachHangView = new KhachHangView();
            hoaDonView = new HoaDonView();
            doanhThuView = new DoanhThuView();
            danhMucView = new DanhMucView();

            // Controllers
            new TrangChuController(trangChuView);
            new DoanhThuController(doanhThuView);
            new NhanVienController(nhanVienView);

            HoaDonController hoaDonController = new HoaDonController(hoaDonView);
            KhachHangController khachHangController = new KhachHangController(khachHangView);
            DatMonController datMonController = new DatMonController(datMonView, hoaDonController);
            datMonController.SetKhachHangController(khachHangController);
            ThucDonController thucDonController = new ThucDonController(thucDonView, datMonController);
            DanhMucController danhMucController = new DanhMucController(danhMucView);

            danhMucController.SetThucDonController(thucDonController);
            danhMucController.SetDatMonController(datMonController);

            // Add views
            string[] names = { "Trang chủ", "Đặt Món", "Thực đơn", "Nhân viên", "Khách hàng", "Hóa đơn", "Doanh thu", "Danh mục" };
            Control[] views = { trangChuView, datMonView, thucDonView, nhanVienView, khachHangView, hoaDonView, doanhThuView, danhMucView };

            for (int i = 0; i < names.Length; i++)
            {
                AddView(views[i], names[i]);
            }
        }

        private void AddView(Control view, string name)
        {
            view.Dock = DockStyle.Fill;
            view.Visible = false;
            view.Name = name;
            view.BackColor = Color.White; // Ép nền view về trắng để nổi bật trên nền xám content
            pnlContent.Controls.Add(view);
        }

        private void ShowView(string name)
        {
            foreach (Control c in pnlContent.Controls) c.Visible = false;
            Control view = pnlContent.Controls[name];
            if (view != null)
            {
                view.Visible = true;
                view.BringToFront();
            }
        }

        private void UpdateActiveButton(string active)
        {
            foreach (var item in menuButtons)
            {
                bool isActive = (item.Key == active);
                item.Value.BackColor = isActive ? COLOR_ACCENT : COLOR_SIDEBAR;
                item.Value.ForeColor = isActive ? Color.White : COLOR_TEXT_MENU;
                item.Value.Font = new Font("Segoe UI", 11, isActive ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        public void SetRole(string role)
        {
            lblRole.Text = role;
            lblUser.Text = "👤  " + Session.TenNV + " (" + role + ")";

            if (role == "NHÂN VIÊN")
            {
                HideMenu("Thực đơn");
                HideMenu("Nhân viên");
                HideMenu("Danh mục");
                HideMenu("Doanh thu");
            }
            trangChuView.SetUserInfo(role, role);
        }

        private void HideMenu(string name)
        {
            if (menuButtons.ContainsKey(name)) menuButtons[name].Visible = false;
        }

        private void HandleLogout()
        {
            if (MessageBox.Show("Đăng xuất khỏi hệ thống?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                LoginView loginView = new LoginView();
                new LoginController(loginView);
                loginView.Show();
            }
        }
    }
}