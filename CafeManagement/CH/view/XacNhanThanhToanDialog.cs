using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class XacNhanThanhToanDialog : Form
    {
        private TextBox txtTenKhach, txtSDT;
        private Label lblTongTienFinal;
        private Button btnXacNhanIn;
        private DataGridView dgvReview;

        // Bảng màu hiện đại
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);
        private readonly Color COLOR_BG = Color.FromArgb(248, 249, 250);

        public XacNhanThanhToanDialog(double tongTien, List<object[]> gioHang)
        {
            this.Text = "HÓA ĐƠN TẠM TÍNH";
            this.Size = new Size(450, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = COLOR_BG;
            this.Font = new Font("Segoe UI", 10);

            InitializeUI(gioHang, tongTien);
        }

        private void InitializeUI(List<object[]> gioHangRows, double tongTien)
        {
            // =========================
            // HEADER - THÔNG TIN KHÁCH
            // =========================
            Panel pnlHead = new Panel { Dock = DockStyle.Top, Height = 160, Padding = new Padding(25, 20, 25, 0) };

            Label lblHeaderTitle = new Label
            {
                Text = "THÔNG TIN KHÁCH HÀNG",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(25, 15)
            };

            txtTenKhach = CreateStyledTextBox("Khách vãng lai", new Point(25, 45), 380);

            Label lblSDTTitle = new Label
            {
                Text = "SỐ ĐIỆN THOẠI",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(25, 90)
            };

            txtSDT = CreateStyledTextBox("", new Point(25, 115), 380);

            pnlHead.Controls.AddRange(new Control[] { lblHeaderTitle, txtTenKhach, lblSDTTitle, txtSDT });

            // =========================
            // CENTER - CHI TIẾT ĐƠN HÀNG
            // =========================
            Panel pnlGridContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 10, 25, 10) };

            dgvReview = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(240, 240, 240),
                ReadOnly = true
            };

            // Header Grid Style
            dgvReview.ColumnHeadersHeight = 40;
            dgvReview.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 242, 245);
            dgvReview.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvReview.RowTemplate.Height = 35;

            dgvReview.Columns.Add("TenMon", "Món");
            dgvReview.Columns.Add("Size", "Size");
            dgvReview.Columns.Add("SL", "SL");
            dgvReview.Columns.Add("Gia", "Thành tiền");

            dgvReview.Columns["SL"].FillWeight = 40;
            dgvReview.Columns["Size"].FillWeight = 40;

            foreach (object[] item in gioHangRows)
            {
                dgvReview.Rows.Add(item[0], item[1], item[2], string.Format("{0:N0}", item[3]));
            }

            pnlGridContainer.Controls.Add(dgvReview);

            // =========================
            // FOOTER - TỔNG TIỀN & NÚT
            // =========================
            Panel pnlFoot = new Panel { Dock = DockStyle.Bottom, Height = 150, BackColor = Color.White, Padding = new Padding(25, 15, 25, 15) };

            // Đường kẻ ngăn cách mỏng
            Panel line = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(224, 224, 224) };

            lblTongTienFinal = new Label
            {
                Text = "Tổng cộng: " + string.Format("{0:N0} VNĐ", tongTien),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Top,
                Height = 50
            };

            btnXacNhanIn = new Button
            {
                Text = "XÁC NHẬN & IN HÓA ĐƠN",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Bottom,
                Height = 55,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnXacNhanIn.FlatAppearance.BorderSize = 0;

            pnlFoot.Controls.AddRange(new Control[] { lblTongTienFinal, line, btnXacNhanIn });

            // Thêm vào Form theo thứ tự
            this.Controls.Add(pnlGridContainer);
            this.Controls.Add(pnlHead);
            this.Controls.Add(pnlFoot);

            this.AcceptButton = btnXacNhanIn;
        }

        // Hàm tạo TextBox phong cách hiện đại
        private TextBox CreateStyledTextBox(string placeholder, Point loc, int width)
        {
            return new TextBox
            {
                Text = placeholder,
                Location = loc,
                Width = width,
                Font = new Font("Segoe UI", 12),
                BorderStyle = BorderStyle.FixedSingle,
            };
        }

        // =========================
        // GETTERS
        // =========================
        public string GetTenKhach() => txtTenKhach.Text.Trim();
        public string GetSDT() => txtSDT.Text.Trim();
        public Button GetBtnXacNhanIn() => btnXacNhanIn;

        public void AddXacNhanListener(EventHandler ev)
        {
            btnXacNhanIn.Click += ev;
        }
    }
}