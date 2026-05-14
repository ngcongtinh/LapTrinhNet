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

        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);

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
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý thông tin, phân loại và lịch sử giao dịch khách hàng",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(2, 45)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // --- 2. PANEL ACTION (Chứa thanh tìm kiếm và nút thêm) ---
            Panel pnlAction = new Panel { Dock = DockStyle.Top, Height = 60 };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(400, 35),
                Location = new Point(0, 10),
                Text = "🔍 Tìm kiếm theo tên hoặc mã khách hàng...",
                ForeColor = Color.Gray
            };

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };

            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm theo tên hoặc mã khách hàng..."; txtSearch.ForeColor = Color.Gray; }
            };

            BtnThem = new Button
            {
                Text = "+ Thêm Khách Hàng",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                // Anchor giúp nút luôn bám sát lề phải khi kéo to cửa sổ
                Location = new Point(pnlAction.Width - 180, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            BtnThem.FlatAppearance.BorderSize = 0;

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
                RowTemplate = { Height = 45 }, // Tăng chiều cao dòng cho dễ nhìn
                ColumnHeadersHeight = 45,
                EnableHeadersVisualStyles = false
            };

            Table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            Table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Table.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            Table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            Table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            // Cột dữ liệu
            Table.Columns.Add("MaKH", "Mã KH");
            Table.Columns.Add("TenKH", "Họ tên");
            Table.Columns.Add("Loai", "Phân loại");
            Table.Columns.Add("GioiTinh", "Giới tính");
            Table.Columns.Add("SDT", "Số điện thoại");
            Table.Columns.Add("Email", "Email");
            Table.Columns.Add("DiaChi", "Địa chỉ");

            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
            {
                HeaderText = "Hành động",
                Text = "✎  🗑",
                UseColumnTextForButtonValue = true,
                Name = "btnAction",
                Width = 80
            };
            Table.Columns.Add(actionCol);

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
                Size = new Size(480, 600),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White
            };

            FlowLayoutPanel mainContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(30, 20, 30, 20)
            };

            txtMaKH = CreateTextBox(false);
            txtTenKH = CreateTextBox();
            txtTheLoai = CreateTextBox();
            txtSDT = CreateTextBox();
            txtEmail = CreateTextBox();
            txtDiaChi = CreateTextBox();

            rdoNam = new RadioButton { Text = "Nam", AutoSize = true, Font = new Font("Segoe UI", 10) };
            rdoNu = new RadioButton { Text = "Nữ", AutoSize = true, Font = new Font("Segoe UI", 10) };

            // Thêm các ô nhập liệu
            AddInputModern(mainContainer, "Mã khách hàng", txtMaKH);
            AddInputModern(mainContainer, "Họ tên", txtTenKH);
            AddInputModern(mainContainer, "Phân loại(Vip/Vãng lai)", txtTheLoai);

            mainContainer.Controls.Add(CreateLabelModern("Giới tính"));
            FlowLayoutPanel genderPanel = new FlowLayoutPanel { Size = new Size(400, 35), Margin = new Padding(0, 0, 0, 15) };
            genderPanel.Controls.AddRange(new Control[] { rdoNam, rdoNu });
            mainContainer.Controls.Add(genderPanel);

            AddInputModern(mainContainer, "Số điện thoại", txtSDT);
            AddInputModern(mainContainer, "Email", txtEmail);
            AddInputModern(mainContainer, "Địa chỉ", txtDiaChi);

            BtnLuu = new Button
            {
                Text = "XÁC NHẬN LƯU",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(400, 50),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 20, 0, 40)
            };
            BtnLuu.FlatAppearance.BorderSize = 0;

            mainContainer.Controls.Add(BtnLuu);
            DialogForm.Controls.Add(mainContainer);
        }

        // ===== CÁC HÀM BỔ TRỢ (Đã đồng bộ hóa 100% với NhanVienView) =====
        private void AddInputModern(Control parent, string title, Control input)
        {
            parent.Controls.Add(CreateLabelModern(title));
            input.Margin = new Padding(0, 0, 0, 15);
            input.Width = 400;
            parent.Controls.Add(input);
        }

        private Label CreateLabelModern(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(64, 64, 64),
            AutoSize = true,
            Margin = new Padding(0, 5, 0, 3)
        };

        private TextBox CreateTextBox(bool enabled = true) => new TextBox
        {
            Font = new Font("Segoe UI", 10),
            Size = new Size(380, 35),
            Enabled = enabled,
            BackColor = enabled ? Color.White : Color.FromArgb(245, 245, 245)
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