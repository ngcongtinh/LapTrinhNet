using CH.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class TrangChuView : UserControl
    {
        private Label lblTongDoanhThu, lblSoHoaDon, lblSoKhachHang, lblSoMonAn;
        private Button btnLamMoi;
        private Label lblGreeting, lblDate, lblRole, lblMessage;

        private readonly Color PRIMARY_COLOR = Color.FromArgb(0, 91, 110);
        private readonly Color TEXT_COLOR = Color.FromArgb(50, 50, 50);
        private readonly Color BG_COLOR = Color.FromArgb(240, 245, 249); // Màu nền xám nhạt như mẫu

        public TrangChuView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = BG_COLOR;

            // ================= HEADER =================
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.White; // Header mẫu thường là trắng hoặc tiệp màu nền nhưng có shadow

            Label lblTitle = new Label();
            lblTitle.Text = "TỔNG QUAN CỬA HÀNG";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = PRIMARY_COLOR;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 25);

            btnLamMoi = CreateRefreshButton("Làm mới dữ liệu");
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Đẩy nút sang phải
            btnLamMoi.Location = new Point(this.Width - 200, 20);

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnLamMoi);
            this.Controls.Add(headerPanel);

            // ================= CONTENT CONTAINER =================
            // Dùng để chứa Stats và Center panel, tạo khoảng cách Padding
            Panel container = new Panel();
            container.Dock = DockStyle.Fill;
            container.Padding = new Padding(20);
            this.Controls.Add(container);
            container.BringToFront();

            // ================= STATS PANEL (4 CARDS) =================
            TableLayoutPanel statsPanel = new TableLayoutPanel();
            statsPanel.Dock = DockStyle.Top;
            statsPanel.Height = 220; // Tăng chiều cao để card trông thoáng hơn
            statsPanel.ColumnCount = 4;
            statsPanel.RowCount = 1;
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            statsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            lblTongDoanhThu = new Label();
            lblSoHoaDon = new Label();
            lblSoKhachHang = new Label();
            lblSoMonAn = new Label();

            statsPanel.Controls.Add(CreateCard("DOANH THU", lblTongDoanhThu, Color.FromArgb(255, 159, 67)), 0, 0);
            statsPanel.Controls.Add(CreateCard("HÓA ĐƠN", lblSoHoaDon, Color.FromArgb(52, 152, 219)), 1, 0);
            statsPanel.Controls.Add(CreateCard("KHÁCH HÀNG", lblSoKhachHang, Color.FromArgb(46, 204, 113)), 2, 0);
            statsPanel.Controls.Add(CreateCard("MÓN ĂN", lblSoMonAn, Color.FromArgb(155, 89, 182)), 3, 0);

            container.Controls.Add(statsPanel);

            // ================= CENTER INFO PANEL =================
            // Dùng FlowLayoutPanel để các dòng chữ tự động căn giữa dễ hơn
            FlowLayoutPanel centerPanel = new FlowLayoutPanel();
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.FlowDirection = FlowDirection.TopDown;
            centerPanel.WrapContents = false;
            centerPanel.Padding = new Padding(0, 50, 0, 0); // Đẩy nội dung xuống một chút

            // Hàm hỗ trợ căn giữa label trong FlowPanel
            Action<Label> AddCenteredLabel = (lbl) => {
                lbl.AutoSize = true;
                lbl.Margin = new Padding((this.Width / 2) - 150, 10, 0, 0); // Ước lượng căn giữa
                // Tuy nhiên cách chuẩn nhất là gán Anchor hoặc dùng Label.TextAlign nếu FixedSize
                lbl.Anchor = AnchorStyles.None;
                centerPanel.Controls.Add(lbl);
            };

            lblGreeting = new Label { Text = "Chào Admin!", Font = new Font("Segoe UI", 28, FontStyle.Bold), ForeColor = PRIMARY_COLOR, AutoSize = true };
            lblDate = new Label { Text = "Hôm nay là: " + DateTime.Now.ToString("dd/MM/yyyy"), Font = new Font("Segoe UI", 16), ForeColor = TEXT_COLOR, AutoSize = true };
            lblRole = new Label { Text = "Vai trò: ADMIN", Font = new Font("Segoe UI", 16), ForeColor = TEXT_COLOR, AutoSize = true };
            lblMessage = new Label { Text = "Chúc bạn làm việc hiệu quả", Font = new Font("Segoe UI", 16, FontStyle.Italic), ForeColor = Color.Green, AutoSize = true };

            // Căn lề cho các dòng chữ (Dùng Center để giống mẫu)
            centerPanel.Controls.Add(lblGreeting);
            centerPanel.Controls.Add(lblDate);
            centerPanel.Controls.Add(lblRole);
            centerPanel.Controls.Add(lblMessage);

            // Căn chỉnh thủ công vì FlowPanel cần tính toán lại
            centerPanel.SizeChanged += (s, e) => {
                foreach (Control c in centerPanel.Controls)
                {
                    c.Margin = new Padding((centerPanel.Width - c.Width) / 2, 10, 0, 0);
                }
            };

            container.Controls.Add(centerPanel);
            centerPanel.BringToFront();
        }

        private Panel CreateCard(string title, Label valueLabel, Color color)
        {
            Panel card = new Panel();
            card.BackColor = Color.White;
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(10);
            card.Padding = new Padding(0);

            // Thanh màu bên trái
            Panel colorBar = new Panel();
            colorBar.BackColor = color;
            colorBar.Dock = DockStyle.Left;
            colorBar.Width = 6;

            Label lblT = new Label();
            lblT.Text = title;
            lblT.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblT.ForeColor = Color.DarkGray;
            lblT.AutoSize = true;
            lblT.Location = new Point(20, 30);

            valueLabel.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            valueLabel.ForeColor = TEXT_COLOR;
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(20, 75);
            valueLabel.Text = "0"; // Giá trị mặc định

            card.Controls.Add(lblT);
            card.Controls.Add(valueLabel);
            card.Controls.Add(colorBar);

            return card;
        }

        private Button CreateRefreshButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(160, 40);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(53, 79, 78);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 150, 136);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(53, 79, 78);

            return btn;
        }

        // ================= CÁC HÀM CẬP NHẬT DỮ LIỆU =================
        public void SetUserInfo(string username, string role)
        {
            lblGreeting.Text = "Chào " + (Session.TenNV ?? "Admin") + "!";
            lblRole.Text = "Vai trò: " + role.ToUpper();
        }

        public void RefreshDate()
        {
            lblDate.Text = "Hôm nay là: " + DateTime.Now.ToString("dd/MM/yyyy");
        }

        // Getters
        public Label GetLblTongDoanhThu() => lblTongDoanhThu;
        public Label GetLblSoHoaDon() => lblSoHoaDon;
        public Label GetLblSoKhachHang() => lblSoKhachHang;
        public Label GetLblSoMonAn() => lblSoMonAn;
        public Button GetBtnLamMoi() => btnLamMoi;
    }
}