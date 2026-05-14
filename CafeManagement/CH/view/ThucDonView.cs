using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CH.Model;
using MySqlX.XDevAPI.Relational;

namespace CH.View
{
    public partial class ThucDonView : UserControl
    {
        // ===== COMPONENTS =====
        private TextBox txtMaMon;
        private TextBox txtTenMon;
        private TextBox txtDonGia;
        private TextBox txtDVT;
        private TextBox txtSearch;
        private ComboBox cboDanhMuc;
        private Label lblHinhAnh;
        private Button btnChonAnh;

        public Button BtnThem;
        public Button BtnSua;
        public Button BtnXoa;
        public Button BtnLuu;

        public DataGridView Table;
        public Form DialogForm;
        private string duongDanAnh = "";

        // ===== COLORS (Thống nhất với NhanVienView) =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);

        public ThucDonView()
        {
            InitUI();
            InitDialogForm();
        }

        private void InitUI()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30); // Tạo khoảng cách bao quanh cho đẹp

            // --- 1. PANEL HEADER (Tiêu đề) ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80 };

            Label lblTitle = new Label
            {
                Text = "Quản Lý Thực Đơn",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Quản lý danh sách món ăn, đơn giá và hình ảnh thực đơn",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(2, 45)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });

            // --- 2. PANEL ACTION (Tìm kiếm & Nút thêm) ---
            Panel pnlAction = new Panel { Dock = DockStyle.Top, Height = 60 };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(400, 35),
                Location = new Point(0, 10),
                Text = "🔍 Tìm kiếm theo tên món...",
                ForeColor = Color.Gray
            };

            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };
            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm theo tên món..."; txtSearch.ForeColor = Color.Gray; }
            };

            BtnThem = new Button
            {
                Text = "+ Thêm Món Mới",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = COLOR_PRIMARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                // Anchor giúp nút luôn ở bên phải khi kéo rộng màn hình
                Location = new Point(pnlAction.Width - 180, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            BtnThem.FlatAppearance.BorderSize = 0;

            pnlAction.Controls.AddRange(new Control[] { txtSearch, BtnThem });

            // --- 3. BẢNG DỮ LIỆU (Dock Fill - Tự động to ra) ---
            Table = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 60 }, // Chiều cao dòng lớn để hiện ảnh
                ColumnHeadersHeight = 45,
                EnableHeadersVisualStyles = false
            };

            Table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            Table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            Table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            Table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            // Cấu trúc cột (Giữ nguyên thứ tự để không lỗi Index CellClick)
            Table.Columns.Add("MaMon", "Mã món");

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn
            {
                HeaderText = "Hình ảnh",
                Name = "Anh",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 80
            };
            Table.Columns.Add(imgCol);

            Table.Columns.Add("TenMon", "Tên món");
            Table.Columns.Add("Gia", "Đơn giá");
            Table.Columns.Add("DVT", "ĐVT");
            Table.Columns.Add("DanhMuc", "Danh mục");

            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
            {
                HeaderText = "Hành động",
                Text = "✎  🗑",
                UseColumnTextForButtonValue = true,
                Name = "btnAction",
                Width = 80
            };
            Table.Columns.Add(actionCol);

            BtnSua = new Button();
            BtnXoa = new Button();

            Table.CellClick += Table_CellClick;

            // --- 4. THÊM VÀO VIEW (Thứ tự rất quan trọng) ---
            this.Controls.Add(Table);      // Fill trước
            this.Controls.Add(pnlAction);   // Panel ở giữa
            this.Controls.Add(pnlHeader);   // Panel trên cùng
        }

        private void Table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 6) // Cột hành động
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem edit = new ToolStripMenuItem("Sửa");
                ToolStripMenuItem delete = new ToolStripMenuItem("Xóa");

                edit.Click += (s, ev) => BtnSua.PerformClick();
                delete.Click += (s, ev) => BtnXoa.PerformClick();

                menu.Items.AddRange(new ToolStripItem[] { edit, delete });
                menu.Show(Cursor.Position);
            }
        }

        // ===== DIALOG MODAL =====
        private void InitDialogForm()
        {
            DialogForm = new Form();
            DialogForm.Text = "Chi tiết món ăn";
            DialogForm.Size = new Size(500, 650);
            DialogForm.StartPosition = FormStartPosition.CenterScreen;
            DialogForm.BackColor = Color.White;
            DialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            DialogForm.MaximizeBox = false;

            FlowLayoutPanel mainContainer = new FlowLayoutPanel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.FlowDirection = FlowDirection.TopDown;
            mainContainer.WrapContents = false;
            mainContainer.AutoScroll = true;
            mainContainer.Padding = new Padding(30, 20, 30, 20);

            // Khởi tạo controls
            txtMaMon = CreateTextBox(false);
            txtTenMon = CreateTextBox();
            txtDonGia = CreateTextBox();
            txtDVT = CreateTextBox();

            cboDanhMuc = new ComboBox();
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Width = 400;
            cboDanhMuc.Font = new Font("Segoe UI", 11);

            // Phần hình ảnh
            lblHinhAnh = new Label();
            lblHinhAnh.Size = new Size(150, 150);
            lblHinhAnh.BorderStyle = BorderStyle.FixedSingle;
            lblHinhAnh.Text = "Chưa có ảnh";
            lblHinhAnh.TextAlign = ContentAlignment.MiddleCenter;
            lblHinhAnh.BackColor = Color.FromArgb(245, 245, 245);
            lblHinhAnh.Margin = new Padding(0, 10, 0, 10);

            btnChonAnh = new Button();
            btnChonAnh.Text = "Chọn hình ảnh";
            btnChonAnh.Size = new Size(150, 35);
            btnChonAnh.BackColor = Color.LightGray;
            btnChonAnh.FlatStyle = FlatStyle.Flat;
            btnChonAnh.Click += (s, e) => ChonAnh();

            // Add vào container
            AddInputModern(mainContainer, "Mã món ăn", txtMaMon);
            AddInputModern(mainContainer, "Tên món", txtTenMon);
            AddInputModern(mainContainer, "Đơn giá", txtDonGia);
            AddInputModern(mainContainer, "Đơn vị tính", txtDVT);
            AddInputModern(mainContainer, "Danh mục", cboDanhMuc);

            mainContainer.Controls.Add(CreateLabelModern("Hình ảnh minh họa"));
            mainContainer.Controls.Add(lblHinhAnh);
            mainContainer.Controls.Add(btnChonAnh);

            // Nút Lưu
            BtnLuu = new Button();
            BtnLuu.Text = "XÁC NHẬN LƯU";
            BtnLuu.BackColor = COLOR_ACCENT;
            BtnLuu.ForeColor = Color.White;
            BtnLuu.FlatStyle = FlatStyle.Flat;
            BtnLuu.FlatAppearance.BorderSize = 0;
            BtnLuu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            BtnLuu.Size = new Size(400, 50);
            BtnLuu.Cursor = Cursors.Hand;
            BtnLuu.Margin = new Padding(0, 30, 0, 40);

            mainContainer.Controls.Add(BtnLuu);
            DialogForm.Controls.Add(mainContainer);
        }

        // ===== HELPERS =====
        private void AddInputModern(Control parent, string title, Control input)
        {
            parent.Controls.Add(CreateLabelModern(title));
            input.Margin = new Padding(0, 0, 0, 15);
            input.Width = 400;
            parent.Controls.Add(input);
        }

        private Label CreateLabelModern(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 3)
            };
        }

        private TextBox CreateTextBox(bool enabled = true)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Enabled = enabled,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void ChonAnh()
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.jpeg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnh = ofd.FileName;
                    LoadAnh(duongDanAnh);
                }
            }
        }

        private void LoadAnh(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using (var img = Image.FromFile(path))
                    {
                        lblHinhAnh.Image = new Bitmap(img, lblHinhAnh.Size);
                        lblHinhAnh.Text = "";
                    }
                }
                else
                {
                    lblHinhAnh.Image = null;
                    lblHinhAnh.Text = "Chưa có ảnh";
                }
            }
            catch { lblHinhAnh.Text = "Lỗi ảnh"; }
        }

        // ===== METHODS FOR CONTROLLER =====
        public MonAn GetMonAnInfo()
        {
            double gia = 0;
            double.TryParse(txtDonGia.Text, out gia);
            return new MonAn(txtMaMon.Text, txtTenMon.Text, gia, txtDVT.Text, duongDanAnh, cboDanhMuc.Text);
        }

        public void FillForm(MonAn m)
        {
            txtMaMon.Text = m.MaMon;
            txtTenMon.Text = m.TenMon;
            txtDonGia.Text = m.DonGia.ToString();
            txtDVT.Text = m.DonViTinh;
            cboDanhMuc.SelectedItem = m.TenDanhMuc;
            duongDanAnh = m.HinhAnh;
            LoadAnh(duongDanAnh);
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
            Image miniImg = null;
            if (!string.IsNullOrEmpty(m.HinhAnh) && File.Exists(m.HinhAnh))
            {
                using (var img = Image.FromFile(m.HinhAnh)) miniImg = new Bitmap(img, new Size(50, 50));
            }
            Table.Rows.Add(m.MaMon, miniImg, m.TenMon, m.DonGia.ToString("N0") + " đ", m.DonViTinh, m.TenDanhMuc);
        }

        public void ClearTable() => Table.Rows.Clear();
        public ComboBox GetCboDanhMuc() => cboDanhMuc;
        public TextBox TxtSearch => txtSearch;
    }
}