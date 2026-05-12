using CH.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class TrangChuView : UserControl
    {
        // ===== LABEL THỐNG KÊ =====
        private Label lblTongDoanhThu;
        private Label lblSoHoaDon;
        private Label lblSoKhachHang;
        private Label lblSoMonAn;

        // ===== BUTTON =====
        private Button btnLamMoi;

        // ===== USER INFO =====
        private Label lblGreeting;
        private Label lblDate;
        private Label lblRole;
        private Label lblMessage;

        // ===== COLOR =====
        private readonly Color PRIMARY_COLOR = Color.FromArgb(0, 91, 110);
        private readonly Color TEXT_COLOR = Color.FromArgb(50, 50, 50);

        public TrangChuView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(240, 245, 249);

            // ================= HEADER =================
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.Padding = new Padding(20, 20, 20, 10);
            headerPanel.BackColor = this.BackColor;

            Label lblTitle = new Label();
            lblTitle.Text = "TỔNG QUAN CỬA HÀNG";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = PRIMARY_COLOR;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 10);

            btnLamMoi = CreateRefreshButton("Làm mới dữ liệu");
            btnLamMoi.Location = new Point(900, 10);

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnLamMoi);

            this.Controls.Add(headerPanel);

            // ================= MAIN PANEL =================
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            mainPanel.BackColor = this.BackColor;

            // ================= STATS PANEL =================
            TableLayoutPanel statsPanel = new TableLayoutPanel();
            statsPanel.ColumnCount = 4;
            statsPanel.RowCount = 1;
            statsPanel.Height = 140;
            statsPanel.Dock = DockStyle.Top;
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            lblTongDoanhThu = new Label();
            lblSoHoaDon = new Label();
            lblSoKhachHang = new Label();
            lblSoMonAn = new Label();

            statsPanel.Controls.Add(
                CreateCard("DOANH THU", lblTongDoanhThu,
                Color.FromArgb(255, 159, 67)), 0, 0);

            statsPanel.Controls.Add(
                CreateCard("HÓA ĐƠN", lblSoHoaDon,
                Color.FromArgb(52, 152, 219)), 1, 0);

            statsPanel.Controls.Add(
                CreateCard("KHÁCH HÀNG", lblSoKhachHang,
                Color.FromArgb(46, 204, 113)), 2, 0);

            statsPanel.Controls.Add(
                CreateCard("MÓN ĂN", lblSoMonAn,
                Color.FromArgb(155, 89, 182)), 3, 0);

            mainPanel.Controls.Add(statsPanel);

            // ================= CENTER PANEL =================
            Panel centerPanel = new Panel();
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.BackColor = Color.White;
            centerPanel.Padding = new Padding(20);
            centerPanel.Margin = new Padding(0, 20, 0, 0);

            lblGreeting = new Label();
            lblGreeting.Text = "Chào Admin!";
            lblGreeting.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblGreeting.ForeColor = PRIMARY_COLOR;
            lblGreeting.AutoSize = true;
            lblGreeting.Location = new Point(350, 80);

            lblDate = new Label();
            lblDate.Text = "Hôm nay là: " +
                    DateTime.Now.ToString("dd/MM/yyyy");

            lblDate.Font = new Font("Segoe UI", 18);
            lblDate.ForeColor = TEXT_COLOR;
            lblDate.AutoSize = true;
            lblDate.Location = new Point(330, 140);

            lblRole = new Label();
            lblRole.Text = "Vai trò: ";
            lblRole.Font = new Font("Segoe UI", 18);
            lblRole.ForeColor = TEXT_COLOR;
            lblRole.AutoSize = true;
            lblRole.Location = new Point(390, 190);

            lblMessage = new Label();
            lblMessage.Text = "Chúc bạn làm việc hiệu quả";
            lblMessage.Font = new Font("Segoe UI", 18, FontStyle.Italic);
            lblMessage.ForeColor = Color.DarkGreen;
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(300, 250);

            centerPanel.Controls.Add(lblGreeting);
            centerPanel.Controls.Add(lblDate);
            centerPanel.Controls.Add(lblRole);
            centerPanel.Controls.Add(lblMessage);

            mainPanel.Controls.Add(centerPanel);

            this.Controls.Add(mainPanel);
        }

        // ================= CARD =================
        private Panel CreateCard(string title, Label valueLabel, Color color)
        {
            Panel card = new Panel();
            card.BackColor = Color.White;
            card.Margin = new Padding(10);
            card.BorderStyle = BorderStyle.FixedSingle;

            Panel colorBar = new Panel();
            colorBar.BackColor = color;
            colorBar.Dock = DockStyle.Left;
            colorBar.Width = 8;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gray;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 25);

            valueLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            valueLabel.ForeColor = TEXT_COLOR;
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(20, 60);

            card.Controls.Add(colorBar);
            card.Controls.Add(lblTitle);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ================= BUTTON =================
        private Button CreateRefreshButton(string text)
        {
            Button btn = new Button();

            btn.Text = text;
            btn.Size = new Size(180, 45);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = Color.FromArgb(53, 79, 78);
            btn.ForeColor = Color.White;

            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(0, 180, 160);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(53, 79, 78);
            };

            return btn;
        }

        // ================= GETTER =================
        public Label GetLblTongDoanhThu()
        {
            return lblTongDoanhThu;
        }

        public Label GetLblSoHoaDon()
        {
            return lblSoHoaDon;
        }

        public Label GetLblSoKhachHang()
        {
            return lblSoKhachHang;
        }

        public Label GetLblSoMonAn()
        {
            return lblSoMonAn;
        }

        public Button GetBtnLamMoi()
        {
            return btnLamMoi;
        }

        // ================= USER INFO =================
        public void SetUserInfo(string username, string role)
        {
            lblGreeting.Text = "Chào " + Session.TenNV + "!";
            lblRole.Text = "Vai trò: " + role.ToUpper();
        }

        public void RefreshDate()
        {
            lblDate.Text = "Hôm nay là: " +
                DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}