using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using CH.Model;

namespace CH.View
{
    public class KhachHangView : UserControl
    {
        private TextBox txtMaKH, txtTenKH, txtTheLoai, txtEmail, txtSDT, txtDiaChi, txtSearch;
        private RadioButton rdoNam, rdoNu;
        private DataGridView table;
        private DataTable model;

        private Form dialogForm;

        private Button btnThem, btnSua, btnXoa, btnLuu;

        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);
        private readonly Color COLOR_ACCENT = Color.FromArgb(42, 157, 143);
        private readonly Color COLOR_BG = Color.White;

        public KhachHangView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Dock = DockStyle.Fill;
            BackColor = COLOR_BG;
            Padding = new Padding(30, 30, 30, 30);

            // ================= HEADER =================
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70
            };

            Label lblTitle = new Label
            {
                Text = "Quản Lý Khách Hàng",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblSub = new Label
            {
                Text = "Theo dõi thông tin, phân loại và lịch sử khách hàng",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(3, 45)
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // ================= ACTION BAR =================
            Panel pnlAction = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60
            };

            txtSearch = CreateSearchField();
            txtSearch.Location = new Point(0, 5);

            btnThem = CreateButton("+ Thêm Khách Hàng", COLOR_PRIMARY);
            btnThem.Size = new Size(180, 42);
            btnThem.Location = new Point(800, 5);
            btnSua = CreateButton("Sửa", COLOR_PRIMARY);
            btnXoa = CreateButton("Xóa", Color.Red);

            btnSua.Size = new Size(100, 42);
            btnXoa.Size = new Size(100, 42);

            btnSua.Location = new Point(1000, 5);
            btnXoa.Location = new Point(1120, 5);

            pnlAction.Controls.Add(btnSua);
            pnlAction.Controls.Add(btnXoa);

            pnlAction.Controls.Add(txtSearch);
            pnlAction.Controls.Add(btnThem);

            // ================= TABLE =================
            table = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                Font = new Font("Segoe UI", 11),
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 45 }
            };

            table.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 248);
            table.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            table.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 243, 229);
            table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 105, 92);

            model = new DataTable();
            model.Columns.Add("Mã KH");
            model.Columns.Add("Họ tên");
            model.Columns.Add("Loại KH");
            model.Columns.Add("Giới tính");
            model.Columns.Add("Email");
            model.Columns.Add("SĐT");
            model.Columns.Add("Địa chỉ");

            table.DataSource = model;

            // Hover Effect
            table.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    table.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(245, 248, 250);
                }
            };

            table.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                    !table.Rows[e.RowIndex].Selected)
                {
                    table.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.White;
                }
            };

            // ================= MAIN =================
            Controls.Add(table);
            Controls.Add(pnlAction);
            Controls.Add(pnlHeader);

            btnSua = new Button();
            btnXoa = new Button();

            InitDialogForm();
        }

        // =====================================================
        // DIALOG FORM
        // =====================================================

        private void InitDialogForm()
        {
            dialogForm = new Form
            {
                Size = new Size(500, 650),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White
            };

            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            int top = 20;

            txtMaKH = CreateTextField(false);
            txtTenKH = CreateTextField(true);
            txtTheLoai = CreateTextField(true);
            txtEmail = CreateTextField(true);
            txtSDT = CreateTextField(true);
            txtDiaChi = CreateTextField(true);

            AddInput(container, "Mã khách hàng", txtMaKH, ref top);
            AddInput(container, "Họ và tên", txtTenKH, ref top);
            AddInput(container, "Phân loại", txtTheLoai, ref top);

            // Gender
            Label lblGender = CreateLabel("Giới tính");
            lblGender.Location = new Point(0, top);

            Panel pnlGender = new Panel
            {
                Location = new Point(0, top + 25),
                Size = new Size(300, 40)
            };

            rdoNam = new RadioButton
            {
                Text = "Nam",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 5)
            };

            rdoNu = new RadioButton
            {
                Text = "Nữ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(100, 5)
            };

            pnlGender.Controls.Add(rdoNam);
            pnlGender.Controls.Add(rdoNu);

            container.Controls.Add(lblGender);
            container.Controls.Add(pnlGender);

            top += 80;

            AddInput(container, "Email", txtEmail, ref top);
            AddInput(container, "Số điện thoại", txtSDT, ref top);
            AddInput(container, "Địa chỉ", txtDiaChi, ref top);

            btnLuu = CreateButton("Xác nhận Lưu", COLOR_ACCENT);
            btnLuu.Size = new Size(380, 45);
            btnLuu.Location = new Point(0, top + 20);

            container.Controls.Add(btnLuu);

            dialogForm.Controls.Add(container);
        }

        // =====================================================
        // SUPPORT UI
        // =====================================================

        private TextBox CreateSearchField()
        {
            TextBox txt = new TextBox
            {
                Width = 750,
                Height = 40,
                Font = new Font("Segoe UI", 11),
                Text = "🔍 Tìm tên hoặc mã khách hàng...",
                ForeColor = Color.Gray
            };

            txt.Enter += (s, e) =>
            {
                if (txt.Text.Contains("🔍"))
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = "🔍 Tìm tên hoặc mã khách hàng...";
                    txt.ForeColor = Color.Gray;
                }
            };

            return txt;
        }

        private TextBox CreateTextField(bool enabled)
        {
            TextBox txt = new TextBox
            {
                Width = 380,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                Enabled = enabled
            };

            return txt;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true
            };
        }

        private void AddInput(Control parent, string title, Control input, ref int top)
        {
            Label lbl = CreateLabel(title);
            lbl.Location = new Point(0, top);

            input.Location = new Point(0, top + 25);

            parent.Controls.Add(lbl);
            parent.Controls.Add(input);

            top += 80;
        }

        private Button CreateButton(string text, Color bg)
        {
            Button btn = new Button
            {
                Text = text,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        // =====================================================
        // METHODS
        // =====================================================

        public void AddRow(KhachHang kh)
        {
            model.Rows.Add(
                kh.MaKH,
                kh.TenKH,
                kh.TheLoai,
                kh.GioiTinh,
                kh.Email,
                kh.SoDienThoai,
                kh.DiaChi
            );
        }

        public void ClearTable()
        {
            model.Rows.Clear();
        }

        public void FillForm(KhachHang kh)
        {
            txtMaKH.Text = kh.MaKH;
            txtTenKH.Text = kh.TenKH;
            txtTheLoai.Text = kh.TheLoai;
            txtEmail.Text = kh.Email;
            txtSDT.Text = kh.SoDienThoai;
            txtDiaChi.Text = kh.DiaChi;

            if (kh.GioiTinh == "Nam")
                rdoNam.Checked = true;
            else
                rdoNu.Checked = true;
        }

        public void ClearForm()
        {
            txtMaKH.Text = "Tự động sinh";
            txtTenKH.Clear();
            txtTheLoai.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();

            rdoNam.Checked = false;
            rdoNu.Checked = false;
        }

        public KhachHang GetKhachHangInfo()
        {
            return new KhachHang(
                txtMaKH.Text,
                txtTenKH.Text,
                txtTheLoai.Text,
                rdoNam.Checked ? "Nam" : "Nữ",
                txtEmail.Text,
                txtSDT.Text,
                txtDiaChi.Text
            );
        }

        // =====================================================
        // GETTERS
        // =====================================================

        public DataGridView GetTable() => table;

        public Form GetDialogForm() => dialogForm;

        public Button GetBtnThem() => btnThem;

        public Button GetBtnSua() => btnSua;

        public Button GetBtnXoa() => btnXoa;

        public Button GetBtnLuu() => btnLuu;

        public TextBox GetTxtSearch() => txtSearch;

        public RadioButton GetRdoNam() => rdoNam;

        public RadioButton GetRdoNu() => rdoNu;
        public TextBox TxtSearch => txtSearch;
        public DataGridView TableKhachHang => table;
        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Form DialogForm => dialogForm;
        public RadioButton RdoNam => rdoNam;
        public RadioButton RdoNu => rdoNu;

        public void SetMaKH(string ma)
        {
            txtMaKH.Text = ma;
        }
    }
}