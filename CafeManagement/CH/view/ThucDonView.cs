using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CH.Model;

namespace CH.View
{
    public partial class ThucDonView : UserControl
    {
        private TextBox txtMaMon, txtTenMon, txtDonGia, txtDVT, txtSearch;
        private ComboBox cboDanhMuc;
        private Label lblHinhAnh;
        private Button btnChonAnh, btnThem, btnSua, btnXoa, btnReset, btnLuu;

        private string duongDanAnh = "";

        private DataGridView table;
        private Form dialogForm;

        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);
        private readonly Color COLOR_BG = Color.White;
        public DataGridView Table => table;
        public TextBox TxtSearch => txtSearch;
        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Form DialogForm => dialogForm;

        public ThucDonView()
        {
            InitUI();
        }

        private void InitUI()
        {
            this.BackColor = COLOR_BG;
            this.Dock = DockStyle.Fill;

            // ================= HEADER =================
            Panel pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 90;

            Label lblTitle = new Label();
            lblTitle.Text = "Quản Lý Menu Cà Phê";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = COLOR_PRIMARY;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 10);

            Label lblSubTitle = new Label();
            lblSubTitle.Text = "Quản lý danh sách các loại cà phê và đồ uống";
            lblSubTitle.Font = new Font("Segoe UI", 10);
            lblSubTitle.ForeColor = Color.Gray;
            lblSubTitle.AutoSize = true;
            lblSubTitle.Location = new Point(22, 50);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubTitle);

            btnSua = new Button();
            btnXoa = new Button();
            btnReset = new Button();
            btnSua.Text = "Sửa";
            btnXoa.Text = "Xóa";
            btnReset.Text = "Reset";

            btnSua.Size = new Size(100, 40);
            btnXoa.Size = new Size(100, 40);
            btnReset.Size = new Size(100, 40);

            btnSua.Location = new Point(700, 15);
            btnXoa.Location = new Point(810, 15);
            btnReset.Location = new Point(920, 15);

            Panel pnlAction = new Panel();
            pnlAction.Controls.Add(btnSua);
            pnlAction.Controls.Add(btnXoa);
            pnlAction.Controls.Add(btnReset);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.Size = new Size(450, 40);
            txtSearch.Location = new Point(20, 15);
            txtSearch.Text = "🔍 Tìm kiếm theo tên món...";
            txtSearch.ForeColor = Color.Gray;

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
                    txtSearch.Text = "🔍 Tìm kiếm theo tên món...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            btnThem = new Button();
            btnThem.Text = "+ Thêm Món";
            btnThem.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnThem.BackColor = COLOR_PRIMARY;
            btnThem.ForeColor = Color.White;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Size = new Size(160, 40);
            btnThem.Location = new Point(500, 15);

            pnlAction.Controls.Add(txtSearch);
            pnlAction.Controls.Add(btnThem);

            // ================= TABLE =================
            table = new DataGridView();
            table.Dock = DockStyle.Fill;
            table.BackgroundColor = Color.White;
            table.BorderStyle = BorderStyle.None;
            table.RowHeadersVisible = false;
            table.AllowUserToAddRows = false;
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.Font = new Font("Segoe UI", 10);
            table.RowTemplate.Height = 55;

            table.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            table.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(242, 245, 248);

            table.EnableHeadersVisualStyles = false;

            table.Columns.Add("MaMon", "Mã món");

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "Anh";
            imgCol.HeaderText = "Ảnh";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            table.Columns.Add(imgCol);

            table.Columns.Add("TenMon", "Tên món");
            table.Columns.Add("Gia", "Giá");
            table.Columns.Add("DVT", "ĐVT");
            table.Columns.Add("DanhMuc", "Danh mục");

            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn();
            actionCol.Name = "Action";
            actionCol.HeaderText = "Hành động";
            actionCol.Text = "✎   🗑";
            actionCol.UseColumnTextForButtonValue = true;
            table.Columns.Add(actionCol);

            // ================= ADD CONTROL =================
            this.Controls.Add(table);
            this.Controls.Add(pnlAction);
            this.Controls.Add(pnlHeader);

            InitDialogForm();

        }

        // ================= FORM DIALOG =================
        private void InitDialogForm()
        {
            dialogForm = new Form();
            dialogForm.Text = "Thêm món";
            dialogForm.Size = new Size(420, 620);
            dialogForm.StartPosition = FormStartPosition.CenterScreen;
            dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialogForm.MaximizeBox = false;
            dialogForm.BackColor = Color.White;

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(25);

            int top = 20;

            txtMaMon = CreateTextBox(false);
            AddInput(panel, "Mã món", txtMaMon, ref top);

            txtTenMon = CreateTextBox(true);
            AddInput(panel, "Tên món", txtTenMon, ref top);

            txtDonGia = CreateTextBox(true);
            AddInput(panel, "Giá", txtDonGia, ref top);

            txtDVT = CreateTextBox(true);
            AddInput(panel, "Đơn vị tính", txtDVT, ref top);

            cboDanhMuc = new ComboBox();
            cboDanhMuc.Font = new Font("Segoe UI", 10);
            cboDanhMuc.Items.Clear();
            cboDanhMuc.Items.Add("-- Chọn danh mục --");
            cboDanhMuc.SelectedIndex = 0;


            AddInput(panel, "Danh mục", cboDanhMuc, ref top);

            // Ảnh
            lblHinhAnh = new Label();
            lblHinhAnh.Text = "Chưa có ảnh";
            lblHinhAnh.BorderStyle = BorderStyle.FixedSingle;
            lblHinhAnh.TextAlign = ContentAlignment.MiddleCenter;
            lblHinhAnh.Size = new Size(120, 120);
            lblHinhAnh.Location = new Point(25, top);

            btnChonAnh = new Button();
            btnChonAnh.Text = "Chọn ảnh";
            btnChonAnh.Location = new Point(170, top + 40);

            btnChonAnh.Click += (s, e) => ChonAnh();

            panel.Controls.Add(lblHinhAnh);
            panel.Controls.Add(btnChonAnh);

            top += 150;

            btnLuu = new Button();
            btnLuu.Text = "Xác nhận lưu";
            btnLuu.BackColor = COLOR_ACCENT;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLuu.Size = new Size(320, 45);
            btnLuu.Location = new Point(25, top);

            panel.Controls.Add(btnLuu);

            dialogForm.Controls.Add(panel);
        }

        // ================= HELPER =================
        private TextBox CreateTextBox(bool editable)
        {
            TextBox txt = new TextBox();
            txt.Font = new Font("Segoe UI", 10);
            txt.Width = 320;
            txt.Height = 35;
            txt.ReadOnly = !editable;
            return txt;
        }

        private void AddInput(Control parent, string title, Control input, ref int top)
        {
            Label lbl = new Label();
            lbl.Text = title;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(25, top);

            input.Location = new Point(25, top + 25);

            parent.Controls.Add(lbl);
            parent.Controls.Add(input);

            top += 80;
        }

        // ================= IMAGE =================
        private void ChonAnh()
        {
            OpenFileDialog fd = new OpenFileDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                duongDanAnh = fd.FileName;

                Image img = Image.FromFile(duongDanAnh);
                lblHinhAnh.Image = new Bitmap(img, new Size(120, 120));
                lblHinhAnh.Text = "";
            }
        }

        // ================= DATA =================
        public MonAn GetMonAnInfo()
        {
            double gia = 0;

            double.TryParse(txtDonGia.Text.Replace(",", ""), out gia);

            return new MonAn(
                txtMaMon.Text,
                txtTenMon.Text,
                gia,
                txtDVT.Text,
                duongDanAnh,
                cboDanhMuc.Text
            );
        }

        public void FillForm(MonAn m)
        {
            txtMaMon.Text = m.MaMon;
            txtTenMon.Text = m.TenMon;
            txtDonGia.Text = m.DonGia.ToString("N0");
            txtDVT.Text = m.DonViTinh;

            cboDanhMuc.SelectedItem = m.TenDanhMuc;

            duongDanAnh = m.HinhAnh;

            if (!string.IsNullOrEmpty(duongDanAnh))
            {
                Image img = Image.FromFile(duongDanAnh);
                lblHinhAnh.Image = new Bitmap(img, new Size(120, 120));
                lblHinhAnh.Text = "";
            }
        }

        public void ClearForm()
        {
            txtMaMon.Text = "Tự động sinh";
            txtTenMon.Clear();
            txtDonGia.Clear();
            txtDVT.Clear();

            cboDanhMuc.SelectedIndex = -1;

            duongDanAnh = "";

            lblHinhAnh.Image = null;
            lblHinhAnh.Text = "Chưa có ảnh";
        }

        public void AddRow(MonAn m)
        {
            Image img = null;

            if (!string.IsNullOrEmpty(m.HinhAnh))
            {
                img = new Bitmap(Image.FromFile(m.HinhAnh), new Size(45, 45));
            }

            table.Rows.Add(
                m.MaMon,
                img,
                m.TenMon,
                string.Format("{0:N0} đ", m.DonGia),
                m.DonViTinh,
                m.TenDanhMuc,
                ""
            );
        }

        public void ClearTable()
        {
            table.Rows.Clear();
        }

        // ================= GETTER =================
        public DataGridView GetTable() => table;

        public ComboBox GetCboDanhMuc() => cboDanhMuc;

        public TextBox GetTxtSearch() => txtSearch;

        public int GetSelectedRow() => table.CurrentCell.RowIndex;

        public Form GetDialogForm() => dialogForm;

        public Button GetBtnLuu() => btnLuu;

        // ================= EVENT =================
        public void AddThemListener(EventHandler ev)
        {
            btnThem.Click += ev;
        }

        public void AddSuaListener(EventHandler ev)
        {
            btnSua.Click += ev;
        }

        public void AddXoaListener(EventHandler ev)
        {
            btnXoa.Click += ev;
        }
    }
}