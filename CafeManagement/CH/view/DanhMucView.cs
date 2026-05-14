using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public class DanhMucView : UserControl
    {
        // ===== COMPONENTS =====
        private TextBox txtMaDM, txtTenDM, txtSearch;
        private DataGridView table;
        private Form dialogForm;

        // Khai báo public để Controller có thể gán sự kiện Click
        public Button BtnThem;
        public Button BtnLuu;
        public Button BtnSua; // Nút ẩn để Controller xử lý logic Sửa
        public Button BtnXoa; // Nút ẩn để Controller xử lý logic Xóa

        // ===== COLORS =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);

        public DanhMucView()
        {
            InitializeUI();
            InitDialogForm();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // ----- HEADER -----
            Label lblTitle = new Label
            {
                Text = "Quản Lý Danh Mục",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(30, 20)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý và phân loại các nhóm thực đơn trong hệ thống",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(32, 65)
            };

            // ----- SEARCH -----
            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(500, 35),
                Location = new Point(35, 110),
                Text = "🔍 Tìm kiếm danh mục...",
                ForeColor = Color.Gray
            };

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };

            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm danh mục..."; txtSearch.ForeColor = Color.Gray; }
            };

            // ----- BUTTON THÊM -----
            BtnThem = new Button
            {
                Text = "+ Thêm Danh Mục",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(700, 105),
                Cursor = Cursors.Hand
            };
            BtnThem.FlatAppearance.BorderSize = 0;

            // ----- TABLE -----
            table = new DataGridView
            {
                Location = new Point(35, 170),
                Size = new Size(980, 500),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 },
                EnableHeadersVisualStyles = false
            };

            table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);
            table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            table.ColumnHeadersHeight = 45;

            table.Columns.Add("MaDM", "Mã Danh Mục");
            table.Columns.Add("TenDM", "Tên Danh Mục");

            // Cột Nút Hành động
            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
            {
                HeaderText = "Hành động",
                Text = "✎  🗑",
                UseColumnTextForButtonValue = true,

                Name = "btnAction"
            };
            table.Columns.Add(actionCol);

            // Khởi tạo các nút ẩn để Controller bắt sự kiện
            BtnSua = new Button();
            BtnXoa = new Button();

            // SỰ KIỆN CLICK VÀO CỘT HÀNH ĐỘNG
            table.CellContentClick += (s, e) => {
                if (e.RowIndex < 0) return;

                // Nếu click vào cột thứ 2 (cột Hành động)
                if (e.ColumnIndex == 2)
                {
                    // Hiển thị menu nhanh
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem itemSua = new ToolStripMenuItem("Sửa");
                    ToolStripMenuItem itemXoa = new ToolStripMenuItem("Xóa");

                    itemSua.Click += (sender, ev) => BtnSua.PerformClick();
                    itemXoa.Click += (sender, ev) => BtnXoa.PerformClick();

                    menu.Items.AddRange(new ToolStripItem[] { itemSua, itemXoa });
                    menu.Show(Cursor.Position);
                }
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSub);
            this.Controls.Add(txtSearch);
            this.Controls.Add(BtnThem);
            this.Controls.Add(table);
        }

        private void InitDialogForm()
        {
            dialogForm = new Form
            {
                Text = "Thông tin danh mục",
                Size = new Size(450, 350),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            FlowLayoutPanel mainContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(30, 20, 30, 20)
            };

            txtMaDM = CreateTextBox(false);
            txtTenDM = CreateTextBox();

            AddInputModern(mainContainer, "Mã danh mục", txtMaDM);
            AddInputModern(mainContainer, "Tên danh mục", txtTenDM);

            BtnLuu = new Button
            {
                Text = "XÁC NHẬN LƯU",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(375, 50),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 30, 0, 0)
            };
            BtnLuu.FlatAppearance.BorderSize = 0;

            mainContainer.Controls.Add(BtnLuu);
            dialogForm.Controls.Add(mainContainer);
        }

        private void AddInputModern(Control parent, string title, Control input)
        {
            Label lbl = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 3)
            };
            input.Width = 375;
            input.Margin = new Padding(0, 0, 0, 15);
            parent.Controls.Add(lbl);
            parent.Controls.Add(input);
        }

        private TextBox CreateTextBox(bool enabled = true) => new TextBox
        {
            Font = new Font("Segoe UI", 11),
            Enabled = enabled,
            BorderStyle = BorderStyle.FixedSingle
        };

        // ===== GETTERS & SETTERS (Giữ nguyên để không lỗi Controller) =====
        public DataGridView GetTable() => table;
        public Button GetBtnThem() => BtnThem;
        public Button GetBtnLuu() => BtnLuu;
        public TextBox GetTxtSearch() => txtSearch;
        public Form GetDialogForm() => dialogForm;
        public string GetMaDM() => txtMaDM.Text;
        public string GetTenDM() => txtTenDM.Text;
        public void SetForm(string ma, string ten) { txtMaDM.Text = ma; txtTenDM.Text = ten; }
        public void ClearForm() { txtMaDM.Text = "Tự động sinh"; txtTenDM.Clear(); }
        public void AddRow(object[] row) => table.Rows.Add(row[0], row[1]);
        public void ClearTable() => table.Rows.Clear();
    }
}