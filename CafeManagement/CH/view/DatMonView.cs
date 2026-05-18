using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CH.View
{
    public partial class DatMonView : UserControl
    {
        // ================= GIỎ HÀNG =================
        private FlowLayoutPanel pnlCartList;
        private List<object[]> listGioHang = new List<object[]>();

        private Button btnXoaMon;
        private Button btnThanhToan;
        private Label lblTongTien;

        private FlowLayoutPanel pnlMenuGrid;

        private ComboBox cbDanhMuc;
        private TextBox txtSearch;

        // ===== MODERN THEME COLORS =====
        private readonly Color COLOR_PRIMARY = Color.FromArgb(38, 70, 83);      // Dark Slate Blue
        private readonly Color COLOR_ACCENT = Color.FromArgb(16, 185, 129);      // Emerald Green
        private readonly Color COLOR_ACCENT_HOVER = Color.FromArgb(5, 150, 105); // Darker Emerald
        private readonly Color COLOR_BG_LIGHT = Color.FromArgb(243, 244, 246);   // Cool Light Gray Background
        private readonly Color COLOR_BORDER = Color.FromArgb(226, 232, 240);     // Light Slate Gray Border
        private readonly Color COLOR_CARD_HOVER = Color.FromArgb(248, 250, 252); // Soft Gray-Blue on Hover
        private readonly Color COLOR_TEXT_DARK = Color.FromArgb(30, 41, 59);     // Deep Charcoal Gray
        private readonly Color COLOR_TEXT_MUTED = Color.FromArgb(100, 116, 139);  // Slate Gray Muted

        public DatMonView()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = COLOR_BG_LIGHT;
            this.Padding = new Padding(20);

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

            // ================= LEFT: THỰC ĐƠN CARD =================
            Panel pnlLeftContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 0, 15, 0)
            };

            // Custom elevated border style for card effect
            Panel borderPanelLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            pnlLeftContainer.Controls.Add(borderPanelLeft);

            // Title "THỰC ĐƠN CỦA QUÁN"
            Label lblMenuTitle = new Label
            {
                Text = "DANH SÁCH THỰC ĐƠN",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft
            };
            borderPanelLeft.Controls.Add(lblMenuTitle);

            // Top panel for Search & Category filter
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(0, 10, 0, 10)
            };

            cbDanhMuc = new ComboBox
            {
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                Location = new Point(0, 12),
                FlatStyle = FlatStyle.Flat
            };
            cbDanhMuc.Items.Add("Danh mục");
            cbDanhMuc.Items.Add("Tất cả");

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Width = 260,
                Location = new Point(200, 12),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = COLOR_TEXT_MUTED
            };

            // Placeholder for search textbox
            txtSearch.Text = "🔍 Tìm kiếm món ăn...";
            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text.Contains("🔍")) { txtSearch.Text = ""; txtSearch.ForeColor = COLOR_TEXT_DARK; }
            };
            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Tìm kiếm món ăn..."; txtSearch.ForeColor = COLOR_TEXT_MUTED; }
            };

            pnlTop.Controls.AddRange(new Control[] { cbDanhMuc, txtSearch });
            borderPanelLeft.Controls.Add(pnlTop);

            // Grid Flow Layout Panel
            pnlMenuGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(5)
            };
            borderPanelLeft.Controls.Add(pnlMenuGrid);
            pnlMenuGrid.BringToFront(); // Ensure it sits below pnlTop

            // ================= RIGHT: GIỎ HÀNG CARD =================
            Panel pnlRightContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Panel borderPanelRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            pnlRightContainer.Controls.Add(borderPanelRight);

            // Title "GIỎ HÀNG HIỆN TẠI"
            Label lblCartTitle = new Label
            {
                Text = "ĐƠN HÀNG HIỆN TẠI",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft
            };
            borderPanelRight.Controls.Add(lblCartTitle);

            // List of cart items
            pnlCartList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(10),
                BorderStyle = BorderStyle.None
            };
            borderPanelRight.Controls.Add(pnlCartList);
            pnlCartList.BringToFront();

            // Checkout Footer panel
            Panel pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 110,
                BackColor = Color.White,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Divider line above checkout
            Panel hr = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = COLOR_BORDER };
            pnlFooter.Controls.Add(hr);

            lblTongTien = new Label
            {
                Text = "Tổng thanh toán: 0 VNĐ",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY
            };

            Panel pnlBtn = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(0, 5, 0, 0)
            };

            btnThanhToan = new Button
            {
                Text = "THANH TOÁN",
                BackColor = COLOR_ACCENT,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 170,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(pnlBtn.Width - 170, 4),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.MouseEnter += (s, e) => btnThanhToan.BackColor = COLOR_ACCENT_HOVER;
            btnThanhToan.MouseLeave += (s, e) => btnThanhToan.BackColor = COLOR_ACCENT;

            btnXoaMon = new Button
            {
                Text = "🗑 DỌN GIỎ HÀNG",
                BackColor = Color.White,
                ForeColor = Color.FromArgb(220, 38, 38), // Red-600
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Width = 140,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(0, 4),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnXoaMon.FlatAppearance.BorderColor = Color.FromArgb(239, 68, 68);
            btnXoaMon.MouseEnter += (s, e) => { btnXoaMon.BackColor = Color.FromArgb(254, 242, 242); };
            btnXoaMon.MouseLeave += (s, e) => { btnXoaMon.BackColor = Color.White; };

            pnlBtn.Controls.AddRange(new Control[] { btnThanhToan, btnXoaMon });
            pnlBtn.SizeChanged += (s, e) =>
            {
                btnThanhToan.Left = pnlBtn.Width - btnThanhToan.Width;
            };

            pnlFooter.Controls.Add(pnlBtn);
            pnlFooter.Controls.Add(lblTongTien);
            borderPanelRight.Controls.Add(pnlFooter);

            mainLayout.Controls.Add(pnlLeftContainer, 0, 0);
            mainLayout.Controls.Add(pnlRightContainer, 1, 0);

            this.Controls.Add(mainLayout);
        }

        // ================= RENDER CART =================
        private void RenderCart()
        {
            pnlCartList.Controls.Clear();

            double tong = 0;

            foreach (object[] item in listGioHang)
            {
                pnlCartList.Controls.Add(CreateCartItemUI(item));
                tong += Convert.ToInt32(item[2]) * Convert.ToDouble(item[3]);
            }

            SetTongTien(tong);
        }

        // ================= CART ITEM UI ROW =================
        private Panel CreateCartItemUI(object[] item)
        {
            Panel row = new Panel
            {
                Width = 350,
                Height = 85,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(5)
            };

            // Custom border for the item row card
            Panel innerCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            row.Controls.Add(innerCard);

            // PictureBox with fallback handling
            PictureBox pic = new PictureBox
            {
                Size = new Size(55, 55),
                Location = new Point(5, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = COLOR_BG_LIGHT
            };

            try
            {
                string path = item[4]?.ToString();
                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                    pic.Image = Image.FromFile(path);
            }
            catch { }

            // Item Name Label
            Label lblName = new Label
            {
                Text = item[0].ToString(),
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                Location = new Point(70, 5),
                AutoSize = true
            };

            // Size Selector
            ComboBox cbSize = new ComboBox
            {
                Width = 50,
                Location = new Point(70, 28),
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat
            };
            cbSize.Items.AddRange(new string[] { "S", "M", "L" });
            cbSize.SelectedItem = item[1].ToString();

            cbSize.SelectedIndexChanged += (s, e) =>
            {
                string size = cbSize.SelectedItem.ToString();
                double giaGoc = Convert.ToDouble(item[5]);

                double giaMoi = giaGoc;

                if (size == "M")
                    giaMoi += 10000;
                else if (size == "L")
                    giaMoi += 20000;

                item[1] = size;
                item[3] = giaMoi;

                RenderCart();
            };

            // Item Price Label
            Label lblPrice = new Label
            {
                Text = $"{Convert.ToDouble(item[3]):N0} đ",
                ForeColor = COLOR_PRIMARY,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                Location = new Point(70, 54),
                AutoSize = true
            };

            // Quantity Control Buttons
            Button btnMinus = CreateSmallBtn("-");
            btnMinus.Location = new Point(205, 23);

            Label lblQty = new Label
            {
                Text = item[2].ToString(),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                Location = new Point(238, 27),
                Size = new Size(24, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnPlus = CreateSmallBtn("+");
            btnPlus.Location = new Point(265, 23);

            // Delete item button
            Button btnDelete = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(156, 163, 175), // Gray-400
                Width = 28,
                Height = 28,
                Location = new Point(310, 23),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.MouseEnter += (s, e) => btnDelete.ForeColor = Color.FromArgb(220, 38, 38);
            btnDelete.MouseLeave += (s, e) => btnDelete.ForeColor = Color.FromArgb(156, 163, 175);

            btnMinus.Click += (s, e) =>
            {
                int sl = Convert.ToInt32(item[2]);

                if (sl > 1)
                {
                    item[2] = sl - 1;
                    RenderCart();
                }
            };

            btnPlus.Click += (s, e) =>
            {
                item[2] = Convert.ToInt32(item[2]) + 1;
                RenderCart();
            };

            btnDelete.Click += (s, e) =>
            {
                listGioHang.Remove(item);
                RenderCart();
            };

            innerCard.Controls.AddRange(new Control[] { pic, lblName, cbSize, lblPrice, btnMinus, lblQty, btnPlus, btnDelete });

            return row;
        }

        // ================= FLAT SMALL BUTTONS =================
        private Button CreateSmallBtn(string text)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 28,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                BackColor = COLOR_BG_LIGHT,
                ForeColor = COLOR_TEXT_DARK,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(229, 231, 235); }; // Gray-200
            btn.MouseLeave += (s, e) => { btn.BackColor = COLOR_BG_LIGHT; };
            return btn;
        }

        // ================= MENU FOOD/DRINK CARD =================
        public void AddMonCard(string maMon, string ten, double gia, string hinhAnh)
        {
            Panel card = new Panel
            {
                Width = 160,
                Height = 200,
                BackColor = Color.White,
                Margin = new Padding(12),
                Cursor = Cursors.Hand
            };

            // Custom border design for menu card
            Panel innerCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            card.Controls.Add(innerCard);

            PictureBox pic = new PictureBox
            {
                Size = new Size(130, 115),
                Location = new Point(15, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            try
            {
                if (!string.IsNullOrEmpty(hinhAnh) && System.IO.File.Exists(hinhAnh))
                    pic.Image = Image.FromFile(hinhAnh);
            }
            catch { }

            Label lblTen = new Label
            {
                Text = ten,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 26
            };

            Label lblGia = new Label
            {
                Text = $"{gia:N0} đ",
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = COLOR_ACCENT,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 22
            };

            innerCard.Controls.Add(pic);
            innerCard.Controls.Add(lblGia);
            innerCard.Controls.Add(lblTen);

            // Register beautiful hover states for micro-animations
            Action<object, EventArgs> onHoverIn = (s, e) =>
            {
                innerCard.BackColor = COLOR_CARD_HOVER;
                pic.BackColor = COLOR_CARD_HOVER;
            };

            Action<object, EventArgs> onHoverOut = (s, e) =>
            {
                innerCard.BackColor = Color.White;
                pic.BackColor = Color.White;
            };

            card.MouseEnter += new EventHandler(onHoverIn);
            innerCard.MouseEnter += new EventHandler(onHoverIn);
            pic.MouseEnter += new EventHandler(onHoverIn);
            lblTen.MouseEnter += new EventHandler(onHoverIn);
            lblGia.MouseEnter += new EventHandler(onHoverIn);

            card.MouseLeave += new EventHandler(onHoverOut);
            innerCard.MouseLeave += new EventHandler(onHoverOut);
            pic.MouseLeave += new EventHandler(onHoverOut);
            lblTen.MouseLeave += new EventHandler(onHoverOut);
            lblGia.MouseLeave += new EventHandler(onHoverOut);

            // Add item to cart on click
            EventHandler onClickItem = (s, e) =>
            {
                AddMonToGio(maMon, ten, gia, hinhAnh);
            };

            card.Click += onClickItem;
            innerCard.Click += onClickItem;
            pic.Click += onClickItem;
            lblTen.Click += onClickItem;
            lblGia.Click += onClickItem;

            pnlMenuGrid.Controls.Add(card);
        }

        // ================= ADD ITEM TO BASKET =================
        public void AddMonToGio(string maMon, string ten, double gia, string hinhAnh)
        {
            foreach (object[] item in listGioHang)
            {
                if (item[0].ToString() == ten && item[1].ToString() == "S")
                {
                    item[2] = Convert.ToInt32(item[2]) + 1;
                    RenderCart();
                    return;
                }
            }

            listGioHang.Add(new object[]
            {
                ten,
                "S",
                1,
                gia,
                hinhAnh,
                gia
            });

            RenderCart();
        }

        // ================= GETTER METHODS =================
        public List<object[]> GetCartData()
        {
            return listGioHang;
        }

        public void ClearMenu()
        {
            pnlMenuGrid.Controls.Clear();
        }

        public ComboBox GetCbDanhMuc()
        {
            return cbDanhMuc;
        }

        public TextBox GetTxtSearch()
        {
            return txtSearch;
        }

        public void SetTongTien(double tien)
        {
            lblTongTien.Text = $"Tổng thanh toán: {tien:N0} đ";
        }

        public void AddXoaListener(EventHandler ev)
        {
            btnXoaMon.Click += ev;
        }

        public void AddThanhToanListener(EventHandler ev)
        {
            btnThanhToan.Click += ev;
        }

        public void ClearCart()
        {
            listGioHang.Clear();
            RenderCart();
        }
    }
}