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

            // ===== HEADER =====
            Label lblTitle = new Label();
            lblTitle.Text = "Quản Lý Nhân Viên";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = COLOR_PRIMARY;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);

            Label lblSub = new Label();
            lblSub.Text = "Quản lý thông tin, chức vụ và tài khoản đăng nhập";
            lblSub.Font = new Font("Segoe UI", 10);
            lblSub.ForeColor = Color.Gray;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(32, 65);

            // ===== SEARCH =====
            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.Size = new Size(500, 35);
            txtSearch.Location = new Point(35, 110);
            txtSearch.Text = "🔍 Tìm kiếm theo tên nhân viên...";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text.Contains("🔍"))
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Tìm kiếm theo tên nhân viên...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            // ===== BUTTON THÊM =====
            BtnThem = new Button();
            BtnThem.Text = "+ Thêm Nhân Viên";
            BtnThem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            BtnThem.BackColor = COLOR_PRIMARY;
            BtnThem.ForeColor = Color.White;
            BtnThem.FlatStyle = FlatStyle.Flat;
            BtnThem.FlatAppearance.BorderSize = 0;
            BtnThem.Size = new Size(180, 40);
            BtnThem.Location = new Point(700, 105);

            // ===== TABLE =====
            Table = new DataGridView();
            Table.Location = new Point(35, 170);
            Table.Size = new Size(980, 500);

            Table.BackgroundColor = Color.White;
            Table.BorderStyle = BorderStyle.None;

            Table.AllowUserToAddRows = false;
            Table.RowHeadersVisible = false;

            Table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Table.MultiSelect = false;

            Table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Table.RowTemplate.Height = 40;

            Table.ColumnHeadersHeight = 45;
            Table.EnableHeadersVisualStyles = false;

            Table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            Table.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            Table.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            Table.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(204, 243, 229);

            Table.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(0, 105, 92);

            // ===== COLUMNS =====
            Table.Columns.Add("MaNV", "Mã NV");
            Table.Columns.Add("TenNV", "Họ tên");
            Table.Columns.Add("NgaySinh", "Ngày sinh");
            Table.Columns.Add("GioiTinh", "Giới tính");
            Table.Columns.Add("ChucVu", "Chức vụ");
            Table.Columns.Add("SDT", "SĐT");

            DataGridViewButtonColumn actionCol =
                new DataGridViewButtonColumn();

            actionCol.HeaderText = "Hành động";
            actionCol.Text = "✎  🗑";
            actionCol.UseColumnTextForButtonValue = true;

            Table.Columns.Add(actionCol);

            // ===== HIDDEN BUTTONS =====
            BtnSua = new Button();
            BtnXoa = new Button();

            // ===== EVENT CLICK TABLE =====
            Table.CellClick += Table_CellClick;

            // ===== ADD CONTROLS =====
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSub);
            this.Controls.Add(txtSearch);
            this.Controls.Add(BtnThem);
            this.Controls.Add(Table);
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
            DialogForm.Size = new Size(500, 720);
            DialogForm.StartPosition = FormStartPosition.CenterScreen;
            DialogForm.BackColor = Color.White;

            int top = 20;

            txtMaNV = CreateTextBox(false);
            txtTenNV = CreateTextBox();
            txtChucVu = CreateTextBox();
            txtSDT = CreateTextBox();
            txtDiaChi = CreateTextBox();
            txtUsername = CreateTextBox();
            txtPassword = CreateTextBox();

            dtNgaySinh = new DateTimePicker();
            dtNgaySinh.Format = DateTimePickerFormat.Short;
            dtNgaySinh.Size = new Size(380, 35);

            rdoNam = new RadioButton();
            rdoNam.Text = "Nam";

            rdoNu = new RadioButton();
            rdoNu.Text = "Nữ";

            cboRole = new ComboBox();
            cboRole.Items.AddRange(new string[]
            {
                "NHÂN VIÊN",
                "ADMIN"
            });

            cboRole.SelectedIndex = 0;

            AddInput(DialogForm, "Mã nhân viên", txtMaNV, ref top);
            AddInput(DialogForm, "Họ tên", txtTenNV, ref top);
            AddInput(DialogForm, "Ngày sinh", dtNgaySinh, ref top);

            FlowLayoutPanel genderPanel = new FlowLayoutPanel();
            genderPanel.Size = new Size(380, 35);
            genderPanel.Controls.Add(rdoNam);
            genderPanel.Controls.Add(rdoNu);

            AddInput(DialogForm, "Giới tính", genderPanel, ref top);

            AddInput(DialogForm, "Chức vụ", txtChucVu, ref top);
            AddInput(DialogForm, "SĐT", txtSDT, ref top);
            AddInput(DialogForm, "Địa chỉ", txtDiaChi, ref top);
            AddInput(DialogForm, "Username", txtUsername, ref top);
            AddInput(DialogForm, "Password", txtPassword, ref top);
            AddInput(DialogForm, "Quyền hạn", cboRole, ref top);

            BtnLuu = new Button();
            BtnLuu.Text = "Xác nhận lưu";
            BtnLuu.BackColor = COLOR_ACCENT;
            BtnLuu.ForeColor = Color.White;
            BtnLuu.FlatStyle = FlatStyle.Flat;
            BtnLuu.FlatAppearance.BorderSize = 0;
            BtnLuu.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            BtnLuu.Size = new Size(380, 45);
            BtnLuu.Location = new Point(40, top + 20);

            DialogForm.Controls.Add(BtnLuu);
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