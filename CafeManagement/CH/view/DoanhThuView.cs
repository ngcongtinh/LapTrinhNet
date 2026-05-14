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

        // ===== THEME =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);
        private readonly Color COLOR_BG = Color.FromArgb(248, 249, 250);
        private readonly Color COLOR_HOVER = Color.FromArgb(245, 248, 250);
        private readonly Color COLOR_CARD = Color.White;

        public DoanhThuView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.BackColor = COLOR_BG;
            this.Padding = new Padding(30);
            this.Dock = DockStyle.Fill;

            // ================= MAIN =================
            Panel pnlMain = new Panel();
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.BackColor = COLOR_BG;

            // ================= HEADER =================
            Panel pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 240;
            pnlTop.BackColor = COLOR_BG;

            // ===== TITLE =====
            Panel pnlTitle = new Panel();
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Height = 70;

            Label lblTitle = new Label();
            lblTitle.Text = "Báo Cáo Doanh Thu";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = COLOR_PRIMARY;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 0);

            Label lblSub = new Label();
            lblSub.Text = "Theo dõi hiệu quả kinh doanh và dòng tiền theo thời gian";
            lblSub.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSub.ForeColor = Color.Gray;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(3, 42);

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);

            // ===== CARD =====
            Panel pnlCard = CreateStatCard("DOANH THU TRONG KỲ");
            pnlCard.Location = new Point(0, 85);

            // ===== FILTER =====
            Panel pnlFilter = new Panel();
            pnlFilter.Dock = DockStyle.Bottom;
            pnlFilter.Height = 60;
            pnlFilter.BackColor = COLOR_BG;

            Label lblFilter = new Label();
            lblFilter.Text = "Khoảng thời gian:";
            lblFilter.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblFilter.ForeColor = Color.FromArgb(60, 60, 60);
            lblFilter.AutoSize = true;
            lblFilter.Location = new Point(0, 18);

            txtTuNgay = CreateStyledTextBox("01/01/2025");
            txtTuNgay.Location = new Point(160, 12);

            txtDenNgay = CreateStyledTextBox("31/12/2026");
            txtDenNgay.Location = new Point(330, 12);

            btnXemBaoCao = CreatePrimaryButton("Xem báo cáo");
            btnXemBaoCao.Location = new Point(500, 10);

            pnlFilter.Controls.Add(lblFilter);
            pnlFilter.Controls.Add(txtTuNgay);
            pnlFilter.Controls.Add(txtDenNgay);
            pnlFilter.Controls.Add(btnXemBaoCao);

            pnlTop.Controls.Add(pnlFilter);
            pnlTop.Controls.Add(pnlCard);
            pnlTop.Controls.Add(pnlTitle);

            // ================= TABLE =================
            dgvDoanhThu = new DataGridView();
            dgvDoanhThu.Dock = DockStyle.Fill;
            dgvDoanhThu.BackgroundColor = Color.White;
            dgvDoanhThu.BorderStyle = BorderStyle.None;
            dgvDoanhThu.AllowUserToAddRows = false;
            dgvDoanhThu.AllowUserToDeleteRows = false;
            dgvDoanhThu.ReadOnly = true;
            dgvDoanhThu.RowHeadersVisible = false;
            dgvDoanhThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDoanhThu.RowTemplate.Height = 45;
            dgvDoanhThu.Font = new Font("Segoe UI", 11);

            dgvDoanhThu.ColumnHeadersHeight = 45;
            dgvDoanhThu.EnableHeadersVisualStyles = false;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            dgvDoanhThu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            dgvDoanhThu.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            dgvDoanhThu.Columns.Add("NgayGiaoDich", "Ngày Giao Dịch");
            dgvDoanhThu.Columns.Add("DoanhThu", "Doanh Thu Thu Về");

            // Hover effect
            dgvDoanhThu.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = COLOR_HOVER;
                }
            };

            dgvDoanhThu.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dgvDoanhThu.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            };

            // ================= ADD =================
            pnlMain.Controls.Add(dgvDoanhThu);
            pnlMain.Controls.Add(pnlTop);

            this.Controls.Add(pnlMain);
        }

        // ================= STAT CARD =================
        // ================= STAT CARD (Đã fix rộng hơn) =================
        private Panel CreateStatCard(string title)
        {
            Panel card = new Panel();
            // Tăng Width lên 650 hoặc cao hơn tùy màn hình để không bị tràn chữ
            card.Size = new Size(650, 130);
            card.BackColor = COLOR_CARD;
         

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold); // Tăng size tiêu đề
            lblTitle.ForeColor = Color.Gray;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(25, 20);

            lblValueTong = new Label();
            lblValueTong.Text = "TỔNG DOANH THU: 0 VNĐ"; // Gán text mặc định có đủ độ dài
            lblValueTong.Font = new Font("Segoe UI", 26, FontStyle.Bold); // Font to rõ rệt
            lblValueTong.ForeColor = COLOR_ACCENT;
            lblValueTong.AutoSize = true;
            lblValueTong.Location = new Point(22, 55);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValueTong);

            return card;
        }

        // ================= TEXTBOX =================
        private TextBox CreateStyledTextBox(string text)
        {
            TextBox txt = new TextBox();
            txt.Text = text;
            txt.Size = new Size(150, 35);
            txt.Font = new Font("Segoe UI", 11);
            txt.BorderStyle = BorderStyle.FixedSingle;

            return txt;
        }

        // ================= BUTTON =================
        private Button CreatePrimaryButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(130, 38);
            btn.BackColor = COLOR_PRIMARY;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            return btn;
        }

        // ================= GETTERS =================
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