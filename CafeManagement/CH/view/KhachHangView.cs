using System;
using System.Drawing;
using System.Windows.Forms;
using CH.Model;

namespace CH.View
{
    public class KhachHangView : UserControl
    {
 
        private TextBox txtMaKH, txtTenKH, txtTheLoai, txtEmail, txtSDT, txtDiaChi, txtSearch;
        private RadioButton rdoNam, rdoNu;

        public DataGridView Table;
        public Form DialogForm;

        public Button BtnThem, BtnSua, BtnXoa, BtnLuu;

        // ===== THEME COLORS =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);     // Dark Slate Blue
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);    // Vibrant Sage Green
        private readonly Color COLOR_BG_LIGHT = Color.FromArgb(248, 249, 250);  // Soft White/Light Gray
        private readonly Color COLOR_BORDER = Color.FromArgb(226, 232, 240);    // Cool Gray Border
        private readonly Color COLOR_TEXT_DARK = Color.FromArgb(30, 41, 59);    // Charcoal Gray Text
        private readonly Color COLOR_TEXT_MUTED = Color.FromArgb(100, 116, 139); // Muted Slate Gray

        public KhachHangView()
        {
            InitUI();
            InitDialogForm();
        }

        private void InitUI()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30); 

           
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80 };

            Label lblTitle = new Label
            {
                Text = "Quản Lý Khách Hàng",
                Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý thông tin, phân loại và lịch sử giao dịch khách hàng",
                Font = new Font("Segoe UI", 10),
                ForeColor = COLOR_TEXT_MUTED,
                AutoSize = true,
                Location = new Point(2, 48)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // --- 2. PANEL ACTION (Chứa thanh tìm kiếm và nút thêm) ---
            Panel pnlAction = new Panel { Dock = DockStyle.Top, Height = 65 };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(420, 35),
                Location = new Point(0, 12),
                Text = "🔍 Tìm kiếm theo tên hoặc mã khách hàng...",
                ForeColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = COLOR_TEXT_DARK; }
            };

            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm theo tên hoặc mã khách hàng..."; txtSearch.ForeColor = Color.Gray; }
            };

            BtnThem = new Button
            {
                Text = "+ THÊM KHÁCH HÀNG",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 42),
                // Anchor giúp nút luôn bám sát lề phải khi kéo to cửa sổ
                Location = new Point(pnlAction.Width - 200, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            BtnThem.FlatAppearance.BorderSize = 0;

            BtnThem.MouseEnter += (s, e) => BtnThem.BackColor = Color.FromArgb(48, 85, 101);
            BtnThem.MouseLeave += (s, e) => BtnThem.BackColor = COLOR_PRIMARY;

            pnlAction.Controls.AddRange(new Control[] { txtSearch, BtnThem });

            // --- 3. TABLE (Chiếm phần diện tích còn lại) ---
            Table = new DataGridView
            {
                Dock = DockStyle.Fill, // Tự động to ra theo diện tích trống
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 48 }, // Tăng chiều cao dòng cho dễ nhìn
                ColumnHeadersHeight = 48,
                EnableHeadersVisualStyles = false,
                GridColor = COLOR_BORDER
            };

            Table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(75, 85, 99);
            Table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);

            Table.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            Table.DefaultCellStyle.ForeColor = COLOR_TEXT_DARK;
            Table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 245, 244);
            Table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(38, 70, 83);
            Table.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            Table.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);

            // Cột dữ liệu
            Table.Columns.Add("MaKH", "Mã KH");
            Table.Columns.Add("TenKH", "Họ tên");
            Table.Columns.Add("Loai", "Phân loại");
            Table.Columns.Add("GioiTinh", "Giới tính");
            Table.Columns.Add("SDT", "Số điện thoại");
            Table.Columns.Add("Email", "Email");
            Table.Columns.Add("DiaChi", "Địa chỉ");

            Table.Columns["MaKH"].FillWeight = 85;
            Table.Columns["TenKH"].FillWeight = 140;
            Table.Columns["Loai"].FillWeight = 110;
            Table.Columns["GioiTinh"].FillWeight = 75;
            Table.Columns["SDT"].FillWeight = 110;
            Table.Columns["Email"].FillWeight = 135;
            Table.Columns["DiaChi"].FillWeight = 135;

            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
            {
                HeaderText = "Hành động",
                Text = "⚙  Lựa chọn",
                UseColumnTextForButtonValue = true,
                Name = "btnAction",
                Width = 100
            };
            Table.Columns.Add(actionCol);
            Table.Columns["btnAction"].FillWeight = 95;

            BtnSua = new Button();
            BtnXoa = new Button();

            // --- 4. THÊM VÀO VIEW (Thứ tự Add Table trước rồi đến Panels sau là chuẩn Winforms Dock) ---
            this.Controls.Add(Table);
            this.Controls.Add(pnlAction);
            this.Controls.Add(pnlHeader);
        }

        private void InitDialogForm()
        {
            DialogForm = new Form
            {
                Text = "Thông tin khách hàng",
                Size = new Size(500, 650),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White
            };

            Panel pnlDialogHeader = new Panel
            {
                Size = new Size(500, 80),
                BackColor = COLOR_PRIMARY,
                Dock = DockStyle.Top
            };
            Label lblDialogTitle = new Label
            {
                Text = "HỒ SƠ KHÁCH HÀNG",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 20),
                AutoSize = true
            };
            Label lblDialogSub = new Label
            {
                Text = "Vui lòng nhập đầy đủ các thông tin bên dưới",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(200, 220, 225),
                Location = new Point(25, 48),
                AutoSize = true
            };
            pnlDialogHeader.Controls.AddRange(new Control[] { lblDialogTitle, lblDialogSub });

            FlowLayoutPanel mainContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(35, 20, 35, 20)
            };

            txtMaKH = CreateTextBox(false);
            txtTenKH = CreateTextBox();
            txtTheLoai = CreateTextBox();
            txtSDT = CreateTextBox();
            txtEmail = CreateTextBox();
            txtDiaChi = CreateTextBox();

            rdoNam = new RadioButton { Text = "Nam", AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = COLOR_TEXT_DARK };
            rdoNu = new RadioButton { Text = "Nữ", AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = COLOR_TEXT_DARK };

            // Thêm các ô nhập liệu
            AddInputModern(mainContainer, "Mã khách hàng", txtMaKH);
            AddInputModern(mainContainer, "Họ tên *", txtTenKH);
            AddInputModern(mainContainer, "Phân loại (VIP / Vãng lai)", txtTheLoai);

            mainContainer.Controls.Add(CreateLabelModern("Giới tính"));
            FlowLayoutPanel genderPanel = new FlowLayoutPanel { Size = new Size(410, 35), Margin = new Padding(0, 0, 0, 15) };
            genderPanel.Controls.AddRange(new Control[] { rdoNam, rdoNu });
            mainContainer.Controls.Add(genderPanel);

            AddInputModern(mainContainer, "Số điện thoại *", txtSDT);
            AddInputModern(mainContainer, "Email", txtEmail);
            AddInputModern(mainContainer, "Địa chỉ", txtDiaChi);

            BtnLuu = new Button
            {
                Text = "XÁC NHẬN LƯU",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(410, 52),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 20, 0, 40)
            };
            BtnLuu.FlatAppearance.BorderSize = 0;

            BtnLuu.MouseEnter += (s, e) => BtnLuu.BackColor = Color.FromArgb(52, 175, 160);
            BtnLuu.MouseLeave += (s, e) => BtnLuu.BackColor = COLOR_ACCENT;

            mainContainer.Controls.Add(BtnLuu);
            
            DialogForm.Controls.Add(mainContainer);
            DialogForm.Controls.Add(pnlDialogHeader);
        }

        // ===== CÁC HÀM BỔ TRỢ =====
        private void AddInputModern(Control parent, string title, Control input)
        {
            parent.Controls.Add(CreateLabelModern(title));
            input.Margin = new Padding(0, 0, 0, 15);
            input.Width = 410;
            parent.Controls.Add(input);
        }

        private Label CreateLabelModern(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            ForeColor = COLOR_PRIMARY,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 4)
        };

        private TextBox CreateTextBox(bool enabled = true) => new TextBox
        {
            Font = new Font("Segoe UI", 11),
            Enabled = enabled,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = enabled ? Color.White : Color.FromArgb(243, 244, 246),
            ForeColor = enabled ? COLOR_TEXT_DARK : COLOR_TEXT_MUTED
        };

        // ===== METHODS CHO CONTROLLER =====
        public void AddRow(KhachHang kh) => Table.Rows.Add(kh.MaKH, kh.TenKH, kh.TheLoai, kh.GioiTinh, kh.SoDienThoai,kh.Email,kh.DiaChi);
        public void ClearTable() => Table.Rows.Clear();
        public TextBox GetTxtSearch() => txtSearch;
        public void FillForm(KhachHang kh)
        {
            txtMaKH.Text = kh.MaKH; txtTenKH.Text = kh.TenKH; txtTheLoai.Text = kh.TheLoai;
            txtSDT.Text = kh.SoDienThoai; txtEmail.Text = kh.Email; txtDiaChi.Text = kh.DiaChi;
            if (kh.GioiTinh == "Nam") rdoNam.Checked = true; else rdoNu.Checked = true;
        }
        public void ClearForm()
        {
            txtMaKH.Text = "Tự động sinh"; txtTenKH.Clear(); txtTheLoai.Clear();
            txtSDT.Clear(); txtEmail.Clear(); txtDiaChi.Clear();
            rdoNam.Checked = rdoNu.Checked = false;
        }
        public KhachHang GetKhachHangInfo() => new KhachHang(
            txtMaKH.Text, txtTenKH.Text, txtTheLoai.Text,
            rdoNam.Checked ? "Nam" : "Nữ",
            txtEmail.Text, txtSDT.Text, txtDiaChi.Text
        );
    }
}