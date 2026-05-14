using CH.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class NhanVienView : UserControl
    {
        // ===== COMPONENTS =====
        private TextBox txtMaNV;
        private TextBox txtTenNV;
        private DateTimePicker dtNgaySinh;
        private TextBox txtChucVu;
        private TextBox txtSDT;
        private TextBox txtDiaChi;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtSearch;

        private RadioButton rdoNam;
        private RadioButton rdoNu;

        private ComboBox cboRole;

        public Button BtnThem;
        public Button BtnSua;
        public Button BtnXoa;
        public Button BtnLuu;

        public DataGridView Table;
        public Form DialogForm;

        // ===== COLORS =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);

        public NhanVienView()
        {
            InitUI();
            InitDialogForm();
        }

        private void InitUI()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30); // Tạo khoảng cách cho toàn bộ View

            // --- 1. PANEL HEADER (Tiêu đề) ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80 };

            Label lblTitle = new Label
            {
                Text = "Quản Lý Nhân Viên",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý thông tin, chức vụ và tài khoản đăng nhập",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(2, 45)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // --- 2. PANEL ACTION (Tìm kiếm & Nút thêm) ---
            Panel pnlAction = new Panel { Dock = DockStyle.Top, Height = 60 };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(400, 35),
                Location = new Point(0, 10),
                Text = "🔍 Tìm kiếm theo tên nhân viên...",
                ForeColor = Color.Gray
            };

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };
            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm theo tên nhân viên..."; txtSearch.ForeColor = Color.Gray; }
            };

            BtnThem = new Button
            {
                Text = "+ Thêm Nhân Viên",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                // Anchor giúp nút luôn bám lề phải
                Location = new Point(pnlAction.Width - 180, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            BtnThem.FlatAppearance.BorderSize = 0;

            pnlAction.Controls.AddRange(new Control[] { txtSearch, BtnThem });

            // --- 3. BẢNG DỮ LIỆU (Dock Fill - Tự động co dãn) ---
            Table = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 },
                ColumnHeadersHeight = 45,
                EnableHeadersVisualStyles = false
            };

            Table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            Table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Table.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            Table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            Table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            // Thêm các cột
            Table.Columns.Add("MaNV", "Mã NV");
            Table.Columns.Add("TenNV", "Họ tên");
            Table.Columns.Add("NgaySinh", "Ngày sinh");
            Table.Columns.Add("GioiTinh", "Giới tính");
            Table.Columns.Add("ChucVu", "Chức vụ");
            Table.Columns.Add("SDT", "SĐT");

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

            Table.CellClick += Table_CellClick;

            // --- 4. THÊM VÀO VIEW (Thứ tự quan trọng: Fill thêm trước, Top thêm sau) ---
            this.Controls.Add(Table);
            this.Controls.Add(pnlAction);
            this.Controls.Add(pnlHeader);
        }

        private void Table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 6)
            {
                ContextMenuStrip menu = new ContextMenuStrip();

                ToolStripMenuItem edit =
                    new ToolStripMenuItem("Sửa");

                ToolStripMenuItem delete =
                    new ToolStripMenuItem("Xóa");

                edit.Click += (s, ev) =>
                {
                    BtnSua.PerformClick();
                };

                delete.Click += (s, ev) =>
                {
                    BtnXoa.PerformClick();
                };

                menu.Items.Add(edit);
                menu.Items.Add(delete);

                menu.Show(Cursor.Position);
            }
        }

        // ===== DIALOG =====
        private void InitDialogForm()
        {
            DialogForm = new Form();
            DialogForm.Text = "Thông tin nhân viên";
            DialogForm.Size = new Size(500, 600); // Giảm chiều cao form chính
            DialogForm.StartPosition = FormStartPosition.CenterScreen;
            DialogForm.BackColor = Color.White;
            DialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogForm.MaximizeBox = false;

            // Sử dụng FlowLayoutPanel để các control tự động xếp chồng và có thanh cuộn nếu thiếu chỗ
            FlowLayoutPanel mainContainer = new FlowLayoutPanel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.FlowDirection = FlowDirection.TopDown;
            mainContainer.WrapContents = false;
            mainContainer.AutoScroll = true; // QUAN TRỌNG: Tạo thanh cuộn khi nội dung quá dài
            mainContainer.Padding = new Padding(30, 20, 30, 20);

            // Khởi tạo các components
            txtMaNV = CreateTextBox(false);
            txtTenNV = CreateTextBox();
            txtChucVu = CreateTextBox();
            txtSDT = CreateTextBox();
            txtDiaChi = CreateTextBox();
            txtUsername = CreateTextBox();
            txtPassword = CreateTextBox();
            txtPassword.PasswordChar = '●'; // Ẩn mật khẩu

            dtNgaySinh = new DateTimePicker();
            dtNgaySinh.Format = DateTimePickerFormat.Short;
            dtNgaySinh.Width = 400;
            dtNgaySinh.Font = new Font("Segoe UI", 11);

            rdoNam = new RadioButton { Text = "Nam", AutoSize = true, Font = new Font("Segoe UI", 10) };
            rdoNu = new RadioButton { Text = "Nữ", AutoSize = true, Font = new Font("Segoe UI", 10) };

            cboRole = new ComboBox();
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Width = 400;
            cboRole.Font = new Font("Segoe UI", 11);
            cboRole.Items.AddRange(new string[] { "NHÂN VIÊN", "ADMIN" });
            cboRole.SelectedIndex = 0;

            // Thêm các ô nhập liệu vào Container
            AddInputModern(mainContainer, "Mã nhân viên", txtMaNV);
            AddInputModern(mainContainer, "Họ tên", txtTenNV);
            AddInputModern(mainContainer, "Ngày sinh", dtNgaySinh);

            // Gender Panel
            Label lblGender = CreateLabelModern("Giới tính");
            mainContainer.Controls.Add(lblGender);
            FlowLayoutPanel genderPanel = new FlowLayoutPanel { Size = new Size(400, 35), Margin = new Padding(0, 0, 0, 15) };
            genderPanel.Controls.Add(rdoNam);
            genderPanel.Controls.Add(rdoNu);
            mainContainer.Controls.Add(genderPanel);

            AddInputModern(mainContainer, "Chức vụ", txtChucVu);
            AddInputModern(mainContainer, "SĐT", txtSDT);
            AddInputModern(mainContainer, "Địa chỉ", txtDiaChi);
            AddInputModern(mainContainer, "Tên đăng nhập", txtUsername);
            AddInputModern(mainContainer, "Mật khẩu", txtPassword);
            AddInputModern(mainContainer, "Quyền hạn", cboRole);

            // Nút Lưu
            BtnLuu = new Button();
            BtnLuu.Text = "XÁC NHẬN LƯU";
            BtnLuu.BackColor = COLOR_ACCENT;
            BtnLuu.ForeColor = Color.White;
            BtnLuu.FlatStyle = FlatStyle.Flat;
            BtnLuu.FlatAppearance.BorderSize = 0;
            BtnLuu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            BtnLuu.Size = new Size(400, 50);
            BtnLuu.Cursor = Cursors.Hand;
            BtnLuu.Margin = new Padding(0, 20, 0, 40);

            mainContainer.Controls.Add(BtnLuu);
            DialogForm.Controls.Add(mainContainer);
        }

        // Các hàm bổ trợ để code sạch hơn
        private void AddInputModern(Control parent, string title, Control input)
        {
            Label lbl = CreateLabelModern(title);
            input.Margin = new Padding(0, 0, 0, 15); // Khoảng cách dưới mỗi ô nhập
            input.Width = 400;

            parent.Controls.Add(lbl);
            parent.Controls.Add(input);
        }

        private Label CreateLabelModern(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 3)
            };
        }

        // ===== HELPER =====
        private TextBox CreateTextBox(bool enabled = true)
        {
            TextBox txt = new TextBox();

            txt.Font = new Font("Segoe UI", 10);
            txt.Size = new Size(380, 35);
            txt.Enabled = enabled;

            return txt;
        }

        private void AddInput(
            Form form,
            string title,
            Control control,
            ref int top)
        {
            Label lbl = new Label();

            lbl.Text = title;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(40, top);

            control.Location = new Point(40, top + 25);

            form.Controls.Add(lbl);
            form.Controls.Add(control);

            top += 80;
        }

        // ===== METHODS =====
        public void AddRow(object[] row)
        {
            Table.Rows.Add(row);
        }

        public void ClearTable()
        {
            Table.Rows.Clear();
        }

        public void ClearForm()
        {
            txtMaNV.Text = "Tự động sinh";
            txtTenNV.Clear();
            txtChucVu.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            rdoNam.Checked = false;
            rdoNu.Checked = false;
        }

        public void FillForm(NhanVien nv)
        {
            txtMaNV.Text = nv.MaNV;
            txtTenNV.Text = nv.TenNV;
            txtChucVu.Text = nv.ChucVu;
            txtSDT.Text = nv.SoDienThoai;
            txtDiaChi.Text = nv.DiaChi;
            txtUsername.Text = nv.Username;

            if (nv.GioiTinh == "Nam")
                rdoNam.Checked = true;
            else
                rdoNu.Checked = true;

            DateTime date;

            if (DateTime.TryParse(nv.NgaySinh, out date))
            {
                dtNgaySinh.Value = date;
            }

            cboRole.SelectedItem = nv.Role;
        }

        public NhanVien GetNhanVienInfo()
        {
            string gt = rdoNam.Checked ? "Nam" : "Nữ";

            return new NhanVien(
                txtMaNV.Text,
                txtTenNV.Text,
                dtNgaySinh.Value.ToString("dd/MM/yyyy"),
                gt,
                txtChucVu.Text,
                txtSDT.Text,
                txtDiaChi.Text,
                txtUsername.Text,
                txtPassword.Text,
                cboRole.SelectedItem.ToString()
            );
        }

        // ===== GETTERS =====
        public TextBox TxtSearch => txtSearch;

        public RadioButton RdoNam => rdoNam;
        public RadioButton RdoNu => rdoNu;
    }
}