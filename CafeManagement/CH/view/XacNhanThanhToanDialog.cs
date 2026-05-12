using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class XacNhanThanhToanDialog : Form
    {
        private TextBox txtTenKhach;
        private TextBox txtSDT;

        private Label lblTongTienFinal;

        private Button btnXacNhanIn;

        private DataGridView dgvReview;


        public XacNhanThanhToanDialog(
            double tongTien,
            List<object[]> gioHang)
        {
            Text = "Xác nhận Thanh Toán";

            Size = new Size(400, 550);

            StartPosition = FormStartPosition.CenterParent;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            MaximizeBox = false;
            MinimizeBox = false;

            BackColor = Color.White;

            InitializeUI(gioHang, tongTien);
        }

        private void InitializeUI(List<object[]> gioHangRows, double tongTien)
        {
            // =========================
            // HEADER
            // =========================
            Panel pnlHead = new Panel();
            pnlHead.Dock = DockStyle.Top;
            pnlHead.Height = 140;
            pnlHead.Padding = new Padding(10);

            Label lblTenKhach = new Label();
            lblTenKhach.Text = "Tên khách hàng:";
            lblTenKhach.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTenKhach.AutoSize = true;
            lblTenKhach.Location = new Point(10, 10);

            txtTenKhach = new TextBox();
            txtTenKhach.Text = "Khách vãng lai";
            txtTenKhach.Font = new Font("Segoe UI", 10);
            txtTenKhach.Size = new Size(340, 30);
            txtTenKhach.Location = new Point(10, 35);

            Label lblSDT = new Label();
            lblSDT.Text = "Số điện thoại (Để tìm kiếm):";
            lblSDT.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(10, 75);

            txtSDT = new TextBox();
            txtSDT.Font = new Font("Segoe UI", 10);
            txtSDT.Size = new Size(340, 30);
            txtSDT.Location = new Point(10, 100);

            pnlHead.Controls.Add(lblTenKhach);
            pnlHead.Controls.Add(txtTenKhach);

            pnlHead.Controls.Add(lblSDT);
            pnlHead.Controls.Add(txtSDT);

            Controls.Add(pnlHead);

            // =========================
            // CENTER - GIỎ HÀNG
            // =========================
            dgvReview = new DataGridView();

            dgvReview.Dock = DockStyle.Fill;

            dgvReview.BackgroundColor = Color.White;

            dgvReview.AllowUserToAddRows = false;
            dgvReview.ReadOnly = true;

            dgvReview.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvReview.RowTemplate.Height = 35;

            dgvReview.Font = new Font("Segoe UI", 10);

            dgvReview.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            // Copy columns
            // Tạo cột
            dgvReview.Columns.Add("TenMon", "Tên món");
            dgvReview.Columns.Add("Size", "Size");
            dgvReview.Columns.Add("SoLuong", "Số lượng");
            dgvReview.Columns.Add("DonGia", "Đơn giá");

            // Load dữ liệu
            foreach (object[] item in gioHangRows)
            {
                dgvReview.Rows.Add(
                    item[0],
                    item[1],
                    item[2],
                    string.Format("{0:N0} VNĐ", item[3])
                );
            }

            Controls.Add(dgvReview);

            // =========================
            // FOOTER
            // =========================
            Panel pnlFoot = new Panel();

            pnlFoot.Dock = DockStyle.Bottom;

            pnlFoot.Height = 110;

            pnlFoot.Padding = new Padding(10);

            lblTongTienFinal = new Label();

            lblTongTienFinal.Text =
                "Tổng cộng: " +
                string.Format("{0:N0} VNĐ", tongTien);

            lblTongTienFinal.Font =
                new Font("Segoe UI", 16, FontStyle.Bold);

            lblTongTienFinal.ForeColor = Color.Black;

            lblTongTienFinal.TextAlign =
                ContentAlignment.MiddleCenter;

            lblTongTienFinal.Dock = DockStyle.Top;

            lblTongTienFinal.Height = 40;

            btnXacNhanIn = new Button();

            btnXacNhanIn.Text = "XÁC NHẬN & IN";

            btnXacNhanIn.BackColor =
                Color.FromArgb(0, 100, 0);

            btnXacNhanIn.ForeColor = Color.White;

            btnXacNhanIn.Font =
                new Font("Segoe UI", 11, FontStyle.Bold);

            btnXacNhanIn.FlatStyle = FlatStyle.Flat;

            btnXacNhanIn.FlatAppearance.BorderSize = 0;

            btnXacNhanIn.Size = new Size(220, 45);

            btnXacNhanIn.Location = new Point(80, 50);

            btnXacNhanIn.Cursor = Cursors.Hand;
            btnXacNhanIn.DialogResult = DialogResult.OK;
            AcceptButton = btnXacNhanIn;

            pnlFoot.Controls.Add(lblTongTienFinal);

            pnlFoot.Controls.Add(btnXacNhanIn);

            Controls.Add(pnlFoot);
        }

        // =========================
        // GETTERS
        // =========================
        public string GetTenKhach()
        {
            return txtTenKhach.Text.Trim();
        }

        public string GetSDT()
        {
            return txtSDT.Text.Trim();
        }

        public Button GetBtnXacNhanIn()
        {
            return btnXacNhanIn;
        }

        // =========================
        // EVENT
        // =========================
        public void AddXacNhanListener(EventHandler ev)
        {
            btnXacNhanIn.Click += ev;
        }
    }
}