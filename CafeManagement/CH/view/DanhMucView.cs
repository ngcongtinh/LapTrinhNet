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

        // Public buttons for Controller binding
        public Button BtnThem;
        public Button BtnLuu;
        public Button BtnSua; // Hidden button for Controller edit logic
        public Button BtnXoa; // Hidden button for Controller delete logic

        // ===== MODERN DESIGN COLORS =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);      // Dark Slate Blue
        private readonly Color COLOR_ACCENT = Color.FromArgb(16, 185, 129);      // Emerald Green
        private readonly Color COLOR_ACCENT_HOVER = Color.FromArgb(5, 150, 105); // Darker Emerald
        private readonly Color COLOR_BG_LIGHT = Color.FromArgb(243, 244, 246);   // Cool Light Gray Background
        private readonly Color COLOR_BORDER = Color.FromArgb(226, 232, 240);     // Light Slate Gray Border
        private readonly Color COLOR_TEXT_DARK = Color.FromArgb(30, 41, 59);     // Deep Charcoal Gray
        private readonly Color COLOR_TEXT_MUTED = Color.FromArgb(100, 116, 139);  // Slate Gray Muted
        private readonly Color COLOR_GRID_SELECTED = Color.FromArgb(237, 245, 244); // Soft Sage Green Highlight

        public DanhMucView()
        {
            InitializeUI();
            InitDialogForm();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = COLOR_BG_LIGHT;
            this.Padding = new Padding(25);

            // Container Panel for Elevated Card Look
            Panel cardContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(25)
            };
            this.Controls.Add(cardContainer);

            // ----- HEADER BLOCK -----
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White
            };
            cardContainer.Controls.Add(pnlHeader);

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH MỤC",
                Font = new Font("Segoe UI Semibold", 20, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý và phân loại các nhóm thực đơn trong hệ thống kinh doanh cửa hàng",
                Font = new Font("Segoe UI", 10),
                ForeColor = COLOR_TEXT_MUTED,
                AutoSize = true,
                Location = new Point(2, 40)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // ----- FILTER & ACTION PANEL -----
            Panel pnlActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White,
                Padding = new Padding(0, 10, 0, 15)
            };
            cardContainer.Controls.Add(pnlActions);

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Width = 400,
                Location = new Point(0, 12),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = COLOR_TEXT_MUTED
            };
            txtSearch.Text = "🔍 Tìm kiếm danh mục...";

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = COLOR_TEXT_DARK; }
            };

            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm danh mục..."; txtSearch.ForeColor = COLOR_TEXT_MUTED; }
            };

            BtnThem = new Button
            {
                Text = "+ THÊM DANH MỤC",
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 190,
                Height = 36,
                Location = new Point(pnlActions.Width - 190, 10),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            BtnThem.FlatAppearance.BorderSize = 0;
            BtnThem.MouseEnter += (s, e) => BtnThem.BackColor = Color.FromArgb(29, 53, 63);
            BtnThem.MouseLeave += (s, e) => BtnThem.BackColor = COLOR_PRIMARY;

            pnlActions.Controls.AddRange(new Control[] { txtSearch, BtnThem });
            pnlActions.SizeChanged += (s, e) =>
            {
                BtnThem.Left = pnlActions.Width - BtnThem.Width;
            };

            // ----- GRID TABLE -----
            table = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 44 },
                EnableHeadersVisualStyles = false,
                GridColor = COLOR_BG_LIGHT
            };
            cardContainer.Controls.Add(table);
            table.BringToFront();

            // Table styling details
            table.DefaultCellStyle.SelectionBackColor = COLOR_GRID_SELECTED;
            table.DefaultCellStyle.SelectionForeColor = COLOR_PRIMARY;
            table.DefaultCellStyle.ForeColor = COLOR_TEXT_DARK;
            table.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            
            table.ColumnHeadersDefaultCellStyle.BackColor = COLOR_BG_LIGHT;
            table.ColumnHeadersDefaultCellStyle.ForeColor = COLOR_TEXT_DARK;
            table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            table.ColumnHeadersHeight = 44;

            table.Columns.Add("MaDM", "Mã Danh Mục");
            table.Columns.Add("TenDM", "Tên Danh Mục");

            // Alternating Row Styling
            table.CellFormatting += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.RowIndex % 2 == 0)
                        table.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    else
                        table.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
                }
            };

            // Action Column (⚙ Lựa chọn)
            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
            {
                HeaderText = "⚙ Lựa chọn",
                Text = "✎ Sửa / Xóa",
                UseColumnTextForButtonValue = true,
                Width = 150,
                Name = "btnAction",
                FlatStyle = FlatStyle.Flat
            };
            actionCol.DefaultCellStyle.BackColor = COLOR_BG_LIGHT;
            actionCol.DefaultCellStyle.ForeColor = COLOR_PRIMARY;
            actionCol.DefaultCellStyle.SelectionBackColor = COLOR_GRID_SELECTED;
            actionCol.DefaultCellStyle.SelectionForeColor = COLOR_PRIMARY;
            table.Columns.Add(actionCol);

            // Initialize hidden buttons for Controller event binding
            BtnSua = new Button();
            BtnXoa = new Button();

            // Grid action cell content click handler
            table.CellContentClick += (s, e) => {
                if (e.RowIndex < 0) return;

                if (e.ColumnIndex == 2)
                {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem itemSua = new ToolStripMenuItem("✎ Chỉnh sửa danh mục");
                    ToolStripMenuItem itemXoa = new ToolStripMenuItem("🗑 Xóa bỏ danh mục");

                    itemSua.Click += (sender, ev) => BtnSua.PerformClick();
                    itemXoa.Click += (sender, ev) => BtnXoa.PerformClick();

                    menu.Items.AddRange(new ToolStripItem[] { itemSua, itemXoa });
                    menu.Show(Cursor.Position);
                }
            };
        }

        private void InitDialogForm()
        {
            dialogForm = new Form
            {
                Text = "Hồ sơ danh mục",
                Size = new Size(450, 380),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Modern Top Title Banner for Hộp Thoại Dialog
            Panel pnlBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = COLOR_PRIMARY,
                Padding = new Padding(20, 12, 20, 12)
            };

            Label lblBannerTitle = new Label
            {
                Text = "HỒ SƠ DANH MỤC",
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 12)
            };

            Label lblBannerSub = new Point(15, 36) != Point.Empty ? new Label
            {
                Text = "Nhập thông tin phân loại nhóm thực đơn bên dưới",
                Font = new Font("Segoe UI", 9),
                ForeColor = COLOR_BG_LIGHT,
                AutoSize = true,
                Location = new Point(15, 36)
            } : null;

            pnlBanner.Controls.AddRange(new Control[] { lblBannerTitle, lblBannerSub });
            dialogForm.Controls.Add(pnlBanner);

            FlowLayoutPanel mainContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(30, 90, 30, 20) // Top padding to offset absolute banner height
            };

            txtMaDM = CreateTextBox(false);
            txtTenDM = CreateTextBox();

            AddInputModern(mainContainer, "Mã danh mục (Tự động sinh)", txtMaDM);
            AddInputModern(mainContainer, "Tên danh mục", txtTenDM);

            BtnLuu = new Button
            {
                Text = "XÁC NHẬN LƯU",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(375, 46),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 20, 0, 0)
            };
            BtnLuu.FlatAppearance.BorderSize = 0;
            BtnLuu.MouseEnter += (s, e) => BtnLuu.BackColor = COLOR_ACCENT_HOVER;
            BtnLuu.MouseLeave += (s, e) => BtnLuu.BackColor = COLOR_ACCENT;

            mainContainer.Controls.Add(BtnLuu);
            dialogForm.Controls.Add(mainContainer);
            pnlBanner.BringToFront(); // Keeps title panel on top
        }

        private void AddInputModern(Control parent, string title, Control input)
        {
            Label lbl = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 4)
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
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = enabled ? Color.White : COLOR_BG_LIGHT
        };

        // ===== GETTERS & SETTERS (Keep intact for Controller compatibility) =====
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