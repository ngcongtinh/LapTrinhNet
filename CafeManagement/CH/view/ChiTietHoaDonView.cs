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
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblMaHD);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(970, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.lblTitle.Location = new System.Drawing.Point(180, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(245, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết hóa đơn";
            // 
            // lblMaHD
            // 
            this.lblMaHD.AutoSize = true;
            this.lblMaHD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblMaHD.Location = new System.Drawing.Point(20, 50);
            this.lblMaHD.Name = "lblMaHD";
            this.lblMaHD.Size = new System.Drawing.Size(83, 23);
            this.lblMaHD.TabIndex = 1;
            this.lblMaHD.Text = "Mã HĐ: ...";
            // 
            // tableChiTiet
            // 
            this.tableChiTiet.AllowUserToAddRows = false;
            this.tableChiTiet.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tableChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tableChiTiet.ColumnHeadersHeight = 29;
            this.tableChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.tableChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableChiTiet.EnableHeadersVisualStyles = false;
            this.tableChiTiet.Location = new System.Drawing.Point(0, 0);
            this.tableChiTiet.Name = "tableChiTiet";
            this.tableChiTiet.ReadOnly = true;
            this.tableChiTiet.RowHeadersWidth = 51;
            this.tableChiTiet.RowTemplate.Height = 30;
            this.tableChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableChiTiet.Size = new System.Drawing.Size(970, 326);
            this.tableChiTiet.TabIndex = 1;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Controls.Add(this.lblKhachHang);
            this.pnlFooter.Controls.Add(this.lblTongTien);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 326);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(970, 70);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblKhachHang
            // 
            this.lblKhachHang.AutoSize = true;
            this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblKhachHang.Location = new System.Drawing.Point(20, 25);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(122, 23);
            this.lblKhachHang.TabIndex = 0;
            this.lblKhachHang.Text = "Khách hàng: ...";
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.Location = new System.Drawing.Point(350, 25);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(123, 25);
            this.lblTongTien.TabIndex = 1;
            this.lblTongTien.Text = "Tổng tiền: ...";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Tên món ăn";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Size";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Số lượng";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Đơn giá";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Thành tiền";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 125;
            // 
            // ChiTietHoaDonView
            // 
            this.ClientSize = new System.Drawing.Size(970, 396);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.tableChiTiet);
            this.Controls.Add(this.pnlFooter);
            this.Name = "ChiTietHoaDonView";
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