using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public class DanhMucView : UserControl
    {
        private TextBox txtMaDM, txtTenDM, txtSearch;
        private DataGridView table;
        private Form dialogForm;
        private Button btnThem, btnLuu;

        // Bảng màu hiện đại
        private readonly Color COLOR_PRIMARY = Color.FromArgb(45, 52, 71);    // Xanh Navy
        private readonly Color COLOR_ACCENT = Color.FromArgb(0, 150, 136);    // Teal (Lưu/Thêm)
        private readonly Color COLOR_DANGER = Color.FromArgb(231, 76, 60);    // Đỏ (Xóa)
        private readonly Color COLOR_LIGHT = Color.FromArgb(245, 247, 250);   // Xám nhạt (Header)

        public DanhMucView()
        {
            InitializeUI();
            InitDialogForm();
            SetupSearchPlaceholder();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Padding = new Padding(25);

            // ----- HEADER -----
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 90 };

            Label lblTitle = new Label
            {
                Text = "DANH MỤC SẢN PHẨM",
                Font = new Font("Segoe UI Semibold", 20),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(-4, 10)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý và phân loại các nhóm thực đơn trong hệ thống",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                AutoSize = true,
                Location = new Point(0, 50)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // ----- ACTION BAR -----
            Panel pnlAction = new Panel { Dock = DockStyle.Top, Height = 60 };

            txtSearch = new TextBox
            {
                Text = "🔍 Tìm kiếm danh mục...",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 12),
                Size = new Size(350, 35),
                Location = new Point(0, 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnThem = CreateButton("+ THÊM MỚI", COLOR_ACCENT);
            btnThem.Size = new Size(150, 35);
            btnThem.Location = new Point(365, 10);

            pnlAction.Controls.AddRange(new Control[] { txtSearch, btnThem });

            // ----- TABLE (DATA GRID) -----
            table = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 50 },
                GridColor = Color.FromArgb(240, 240, 240),
                EnableHeadersVisualStyles = false,
                ReadOnly = true
            };

            // Header Style
            table.ColumnHeadersDefaultCellStyle.BackColor = COLOR_LIGHT;
            table.ColumnHeadersDefaultCellStyle.ForeColor = COLOR_PRIMARY;
            table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11);
            table.ColumnHeadersHeight = 45;

            // Columns
            table.Columns.Add("MaDM", "Mã ID");
            table.Columns.Add("TenDM", "Tên Danh Mục");

            // Nút Sửa trực tiếp trên dòng
            DataGridViewButtonColumn btnSuaCol = new DataGridViewButtonColumn
            {
                HeaderText = "Chỉnh sửa",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                Name = "btnSua",
                FlatStyle = FlatStyle.Flat
            };
            btnSuaCol.DefaultCellStyle.ForeColor = COLOR_ACCENT;
            btnSuaCol.DefaultCellStyle.SelectionForeColor = COLOR_ACCENT;
            table.Columns.Add(btnSuaCol);

            // Nút Xóa trực tiếp trên dòng
            DataGridViewButtonColumn btnXoaCol = new DataGridViewButtonColumn
            {
                HeaderText = "Thao tác",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Name = "btnXoa",
                FlatStyle = FlatStyle.Flat
            };
            btnXoaCol.DefaultCellStyle.ForeColor = COLOR_DANGER;
            btnXoaCol.DefaultCellStyle.SelectionForeColor = COLOR_DANGER;
            table.Columns.Add(btnXoaCol);

            this.Controls.Add(table);
            this.Controls.Add(pnlAction);
            this.Controls.Add(pnlHeader);
        }

        private void InitDialogForm()
        {
            dialogForm = new Form
            {
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Text = "Thông tin danh mục"
            };

            Label lblTen = new Label
            {
                Text = "Tên danh mục:",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10)
            };

            txtTenDM = new TextBox
            {
                Location = new Point(30, 60),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 12)
            };

            txtMaDM = new TextBox { Visible = false }; // Lưu mã ngầm khi sửa

            btnLuu = CreateButton("XÁC NHẬN LƯU", COLOR_PRIMARY);
            btnLuu.Location = new Point(30, 150);
            btnLuu.Size = new Size(320, 45);

            dialogForm.Controls.AddRange(new Control[] { lblTen, txtTenDM, btnLuu });
        }

        private Button CreateButton(string text, Color bg)
        {
            return new Button
            {
                Text = text,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void SetupSearchPlaceholder()
        {
            txtSearch.Enter += (s, e) => { if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm danh mục..."; txtSearch.ForeColor = Color.Gray; } };
        }

        // Getters & Setters
        public DataGridView GetTable() => table;
        public Button GetBtnThem() => btnThem;
        public Button GetBtnLuu() => btnLuu;
        public TextBox GetTxtSearch() => txtSearch;
        public Form GetDialogForm() => dialogForm;
        public string GetMaDM() => txtMaDM.Text;
        public string GetTenDM() => txtTenDM.Text;
        public void SetForm(string ma, string ten) { txtMaDM.Text = ma; txtTenDM.Text = ten; }
        public void ClearForm() { txtMaDM.Text = ""; txtTenDM.Text = ""; }
        public void AddRow(object[] row) => table.Rows.Add(row[0], row[1]);
        public void ClearTable() => table.Rows.Clear();
    }
}