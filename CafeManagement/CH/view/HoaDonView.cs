using CH.Model;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CH.View
{
    public partial class HoaDonView : UserControl
    {
        private TextBox txtSearch;
        private DataGridView dgvHoaDon;

        // Event cho Controller xử lý
        public event EventHandler XemChiTietClicked;

        // Màu giao diện
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);
        private readonly Color COLOR_BG = Color.White;

        public HoaDonView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.BackColor = COLOR_BG;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            // ================= HEADER =================
            Panel pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 80;

            Label lblTitle = new Label();
            lblTitle.Text = "Lịch Sử Hóa Đơn";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = COLOR_PRIMARY;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 0);

            Label lblSub = new Label();
            lblSub.Text = "Tra cứu danh sách và chi tiết các hóa đơn";
            lblSub.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSub.ForeColor = Color.Gray;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(2, 42);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // ================= SEARCH =================
            Panel pnlSearch = new Panel();
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Height = 60;

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.Size = new Size(400, 40);
            txtSearch.Location = new Point(0, 10);
            txtSearch.Text = "🔍 Tìm kiếm mã hóa đơn hoặc tên khách...";
            txtSearch.ForeColor = Color.Gray;

            SetupPlaceholder();

            pnlSearch.Controls.Add(txtSearch);

            // ================= TABLE =================
            dgvHoaDon = new DataGridView();
            dgvHoaDon.Dock = DockStyle.Fill;
            dgvHoaDon.BackgroundColor = Color.White;
            dgvHoaDon.BorderStyle = BorderStyle.None;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.AllowUserToDeleteRows = false;
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersVisible = false;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.RowTemplate.Height = 45;
            dgvHoaDon.Font = new Font("Segoe UI", 10);

            // Header style
            dgvHoaDon.EnableHeadersVisualStyles = false;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHoaDon.ColumnHeadersHeight = 45;

            dgvHoaDon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            dgvHoaDon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            // Columns
            dgvHoaDon.Columns.Add("MaHD", "Mã hóa đơn");
            dgvHoaDon.Columns.Add("TenNV", "Nhân viên lập");
            dgvHoaDon.Columns.Add("TenKH", "Khách hàng");
            dgvHoaDon.Columns.Add("NgayLap", "Ngày lập");
            dgvHoaDon.Columns.Add("TongTien", "Tổng tiền");

            // Button column
            DataGridViewButtonColumn btnDetail = new DataGridViewButtonColumn();
            btnDetail.Name = "ChiTiet";
            btnDetail.HeaderText = "Chi tiết";
            btnDetail.Text = "👁";
            btnDetail.UseColumnTextForButtonValue = true;

            dgvHoaDon.Columns.Add(btnDetail);

            dgvHoaDon.CellClick += DgvHoaDon_CellClick;

            // ================= ADD =================
            this.Controls.Add(dgvHoaDon);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);
        }

        /// <summary>
        /// Placeholder Search
        /// </summary>
        private void SetupPlaceholder()
        {
            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text.Contains("🔍"))
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Tìm kiếm mã hóa đơn hoặc tên khách...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
        }

        /// <summary>
        /// Click nút xem chi tiết
        /// </summary>
        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                dgvHoaDon.Columns[e.ColumnIndex].Name == "ChiTiet")
            {
                XemChiTietClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        // ================= PUBLIC METHODS =================

        public void AddRow(HoaDon hd)
        {
            dgvHoaDon.Rows.Add(
                hd.MaHD,
                hd.TenNV,
                hd.TenKH,
                hd.NgayLap,
                string.Format("{0:N0} đ", hd.TongTien),
                "👁"
            );
        }

        public void ClearTable()
        {
            dgvHoaDon.Rows.Clear();
        }

        public int GetSelectedRow()
        {
            if (dgvHoaDon.CurrentRow == null)
                return -1;

            return dgvHoaDon.CurrentRow.Index;
        }

        // ================= GETTERS =================

        public TextBox TxtSearch
        {
            get { return txtSearch; }
        }

        public DataGridView TableHoaDon
        {
            get { return dgvHoaDon; }
        }
    }
}