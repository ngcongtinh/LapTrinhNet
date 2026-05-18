using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class DoanhThuView : UserControl
    {
        private DataGridView dgvDoanhThu;
        private TextBox txtTuNgay;
        private TextBox txtDenNgay;
        private Button btnXemBaoCao;
        private Label lblValueTong;

        // ===== MODERN COLOR THEME =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);      // Dark Slate Blue
        private readonly Color COLOR_ACCENT = Color.FromArgb(16, 185, 129);      // Emerald Green
        private readonly Color COLOR_ACCENT_HOVER = Color.FromArgb(5, 150, 105); // Darker Emerald
        private readonly Color COLOR_BG_LIGHT = Color.FromArgb(243, 244, 246);   // Cool Light Gray Background
        private readonly Color COLOR_BORDER = Color.FromArgb(226, 232, 240);     // Light Slate Gray Border
        private readonly Color COLOR_TEXT_DARK = Color.FromArgb(30, 41, 59);     // Deep Charcoal Gray
        private readonly Color COLOR_TEXT_MUTED = Color.FromArgb(100, 116, 139);  // Slate Gray Muted
        private readonly Color COLOR_GRID_SELECTED = Color.FromArgb(237, 245, 244); // Soft Sage Green Highlight

        public DoanhThuView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.BackColor = COLOR_BG_LIGHT;
            this.Padding = new Padding(25);
            this.Dock = DockStyle.Fill;

            // ================= MAIN CONTAINER =================
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_BG_LIGHT
            };

            // ================= TOP BLOCK (HEADER & WIDGET & FILTER) =================
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 265,
                BackColor = COLOR_BG_LIGHT
            };

            // ===== TITLE PANEL =====
            Panel pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = COLOR_BG_LIGHT
            };

            Label lblTitle = new Label
            {
                Text = "BÁO CÁO DOANH THU",
                Font = new Font("Segoe UI Semibold", 20, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Theo dõi thống kê hiệu quả kinh doanh, doanh số và dòng tiền cửa hàng theo thời gian",
                Font = new Font("Segoe UI", 10),
                ForeColor = COLOR_TEXT_MUTED,
                AutoSize = true,
                Location = new Point(2, 38)
            };

            pnlTitle.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // ===== REVENUE STAT CARD WIDGET =====
            Panel pnlCard = CreateStatCard("DOANH THU TRONG KỲ");
            pnlCard.Location = new Point(0, 75);

            // ===== DATE RANGE FILTER =====
            Panel pnlFilter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = COLOR_BG_LIGHT
            };

            Label lblFilter = new Label
            {
                Text = "Khoảng thời gian thống kê:",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                AutoSize = true,
                Location = new Point(0, 16)
            };

            txtTuNgay = CreateStyledTextBox("01/01/2025");
            txtTuNgay.Location = new Point(210, 11);

            txtDenNgay = CreateStyledTextBox("31/12/2026");
            txtDenNgay.Location = new Point(380, 11);

            btnXemBaoCao = CreatePrimaryButton("Xem báo cáo");
            btnXemBaoCao.Location = new Point(550, 9);

            pnlFilter.Controls.AddRange(new Control[] { lblFilter, txtTuNgay, txtDenNgay, btnXemBaoCao });

            pnlTop.Controls.AddRange(new Control[] { pnlFilter, pnlCard, pnlTitle });

            // ================= BOTTOM BLOCK: TABLE CARD CONTAINER =================
            Panel pnlTableContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 15, 0, 0)
            };

            // ===== REVENUE DATA GRID =====
            dgvDoanhThu = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 44 },
                Font = new Font("Segoe UI", 10),
                EnableHeadersVisualStyles = false,
                GridColor = COLOR_BG_LIGHT
            };
            pnlTableContainer.Controls.Add(dgvDoanhThu);

            dgvDoanhThu.ColumnHeadersHeight = 44;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.BackColor = COLOR_BG_LIGHT;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.ForeColor = COLOR_TEXT_DARK;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);

            dgvDoanhThu.DefaultCellStyle.SelectionBackColor = COLOR_GRID_SELECTED;
            dgvDoanhThu.DefaultCellStyle.SelectionForeColor = COLOR_PRIMARY;
            dgvDoanhThu.DefaultCellStyle.ForeColor = COLOR_TEXT_DARK;

            dgvDoanhThu.Columns.Add("NgayGiaoDich", "Ngày Giao Dịch");
            dgvDoanhThu.Columns.Add("DoanhThu", "Doanh Thu Thu Về (VNĐ)");

            // Alternating Row Styling
            dgvDoanhThu.CellFormatting += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.RowIndex % 2 == 0)
                        dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    else
                        dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
                }
            };

            // Clean modern cell row hover effect
            dgvDoanhThu.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
                }
            };

            dgvDoanhThu.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    if (e.RowIndex % 2 == 0)
                        dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    else
                        dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
                }
            };

            // ================= PACKING IT ALL UP =================
            pnlMain.Controls.Add(pnlTableContainer);
            pnlMain.Controls.Add(pnlTop);

            this.Controls.Add(pnlMain);
        }

        // ================= WIDGET CARD =================
        private Panel CreateStatCard(string title)
        {
            Panel card = new Panel
            {
                Size = new Size(680, 125),
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_MUTED,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            lblValueTong = new Label
            {
                Text = "TỔNG DOANH THU: 0 VNĐ",
                Font = new Font("Segoe UI Semibold", 26, FontStyle.Bold),
                ForeColor = COLOR_ACCENT,
                AutoSize = true,
                Location = new Point(22, 52)
            };

            card.Controls.AddRange(new Control[] { lblTitle, lblValueTong });

            return card;
        }

        // ================= DATE TEXTBOX =================
        private TextBox CreateStyledTextBox(string text)
        {
            TextBox txt = new TextBox
            {
                Text = text,
                Size = new Size(150, 36),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = COLOR_TEXT_DARK
            };

            return txt;
        }

        // ================= SUBMIT BUTTON =================
        private Button CreatePrimaryButton(string text)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(140, 34),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(29, 53, 63);
            btn.MouseLeave += (s, e) => btn.BackColor = COLOR_PRIMARY;

            return btn;
        }

        // ================= GETTERS (Keep intact for Controller compatibility) =================
        public DataGridView DgvDoanhThu
        {
            get { return dgvDoanhThu; }
        }

        public TextBox TxtTuNgay
        {
            get { return txtTuNgay; }
        }

        public TextBox TxtDenNgay
        {
            get { return txtDenNgay; }
        }

        public Button BtnXemBaoCao
        {
            get { return btnXemBaoCao; }
        }

        public Label LblTongDoanhThu
        {
            get { return lblValueTong; }
        }
    }
}