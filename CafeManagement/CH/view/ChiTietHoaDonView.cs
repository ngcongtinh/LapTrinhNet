using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CH.Model;

namespace CH.view
{
    public partial class ChiTietHoaDonView : Form
    {
        // =======================================================
        // COMPONENTS
        // =======================================================
        private DataGridView tableChiTiet;

        private Label lblMaHD;
        private Label lblKhachHang;
        private Panel pnlHeader;
        private Label lblTitle;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private Panel pnlFooter;
        private Label lblTongTien;

        // =======================================================
        // CONSTRUCTOR
        // =======================================================
        public ChiTietHoaDonView()
        {
            InitializeComponent();

            this.Text = "Chi tiết hóa đơn";

            this.Size = new Size(600, 500);

            this.StartPosition = FormStartPosition.CenterParent;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;

            this.MinimizeBox = false;

            this.BackColor = Color.White;
        }

        // =======================================================
        // KHỞI TẠO GIAO DIỆN
        // =======================================================
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMaHD = new System.Windows.Forms.Label();
            this.tableChiTiet = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableChiTiet)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // --- pnlHeader ---
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblMaHD);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 80;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 77, 77);
            this.lblTitle.Location = new System.Drawing.Point(180, 10);
            this.lblTitle.Text = "CHI TIẾT HÓA ĐƠN";

            // lblMaHD
            this.lblMaHD.AutoSize = true;
            this.lblMaHD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblMaHD.Location = new System.Drawing.Point(20, 50);
            this.lblMaHD.Text = "Mã hóa đơn: ...";

            // --- pnlFooter ---
            this.pnlFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlFooter.Controls.Add(this.lblKhachHang);
            this.pnlFooter.Controls.Add(this.lblTongTien);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 60;

            // lblKhachHang
            this.lblKhachHang.AutoSize = true;
            this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblKhachHang.Location = new System.Drawing.Point(20, 20);
            this.lblKhachHang.Text = "Khách hàng: ...";

            // lblTongTien
           
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.Black;
            this.lblTongTien.Location = new System.Drawing.Point(380, 18);
            this.lblTongTien.Text = "Tổng tiền: 0 VNĐ";

            // --- tableChiTiet ---
            this.tableChiTiet.AllowUserToAddRows = false;
            this.tableChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.tableChiTiet.BorderStyle = BorderStyle.None;
            this.tableChiTiet.Dock = System.Windows.Forms.DockStyle.Fill; // Chiếm phần còn lại ở giữa
            this.tableChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.tableChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động dãn cột
            this.tableChiTiet.RowHeadersVisible = false; // Ẩn cột đầu dòng cho gọn

            // Cấu hình các cột
            this.dataGridViewTextBoxColumn1.HeaderText = "Tên món";
            this.dataGridViewTextBoxColumn1.FillWeight = 150; // Cột này rộng hơn
            this.dataGridViewTextBoxColumn2.HeaderText = "Size";
            this.dataGridViewTextBoxColumn3.HeaderText = "Số lượng";
            this.dataGridViewTextBoxColumn4.HeaderText = "Đơn giá";
            this.dataGridViewTextBoxColumn5.HeaderText = "Thành tiền";

            this.tableChiTiet.Columns.Clear();
            this.tableChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5
    });

            // --- ChiTietHoaDonView (Form chính) ---
            this.ClientSize = new System.Drawing.Size(650, 500); // Kích thước hợp lý

            // QUAN TRỌNG: Thứ tự thêm control ảnh hưởng đến Dock
            this.Controls.Add(this.tableChiTiet); // Add table trước để Dock.Fill chiếm chỗ giữa
            this.Controls.Add(this.pnlHeader);   // Header Dock Top
            this.Controls.Add(this.pnlFooter);   // Footer Dock Bottom

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableChiTiet)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        // =======================================================
        // HIỂN THỊ CHI TIẾT HÓA ĐƠN
        // =======================================================
        public void SetDetails(
            string maHD,
            string tenKH,
            double tongTien,
            List<ChiTietHoaDon> listChiTiet)
        {
            lblMaHD.Text = "Mã hóa đơn: " + maHD;

            lblKhachHang.Text = "Khách hàng: " + tenKH;

            tableChiTiet.Rows.Clear();

            double tongTienTinhToan = 0;

            foreach (ChiTietHoaDon item in listChiTiet)
            {
                object[] row = item.ToObjectArray();

                tableChiTiet.Rows.Add(row);

                try
                {
                    double thanhTien = Convert.ToDouble(item.ThanhTien);

                    tongTienTinhToan += thanhTien;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            lblTongTien.Text =
                "Tổng tiền: " +
                string.Format("{0:N0} VNĐ", tongTienTinhToan);
        }
    }
}