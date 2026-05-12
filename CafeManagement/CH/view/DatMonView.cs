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

        // 🎨 COFFEE THEME
        private Color coffeeDark = Color.FromArgb(111, 78, 55);
        private Color coffee = Color.FromArgb(141, 110, 99);
        private Color coffeeLight = Color.FromArgb(215, 204, 200);
        private Color coffeeBg = Color.FromArgb(239, 235, 233);

        public DatMonView()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = coffeeBg;

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            // ================= LEFT =================
            GroupBox grpMenu = new GroupBox();
            grpMenu.Text = "THỰC ĐƠN";
            grpMenu.Dock = DockStyle.Fill;
            grpMenu.ForeColor = coffeeDark;
            grpMenu.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Panel pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.BackColor = coffeeBg;

            Panel pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 45;

            cbDanhMuc = new ComboBox();
            cbDanhMuc.Width = 160;
            cbDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDanhMuc.Items.Add("Danh mục");
            cbDanhMuc.Items.Add("Tất cả");

            txtSearch = new TextBox();
            txtSearch.Dock = DockStyle.Right;
            txtSearch.Width = 250;

            pnlTop.Controls.Add(cbDanhMuc);
            pnlTop.Controls.Add(txtSearch);

            pnlMenuGrid = new FlowLayoutPanel();
            pnlMenuGrid.Dock = DockStyle.Fill;
            pnlMenuGrid.AutoScroll = true;
            pnlMenuGrid.BackColor = coffeeBg;

            pnlLeft.Controls.Add(pnlMenuGrid);
            pnlLeft.Controls.Add(pnlTop);

            grpMenu.Controls.Add(pnlLeft);

            // ================= RIGHT =================
            GroupBox grpCart = new GroupBox();
            grpCart.Text = "GIỎ HÀNG";
            grpCart.Dock = DockStyle.Fill;
            grpCart.ForeColor = coffeeDark;
            grpCart.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Panel pnlRight = new Panel();
            pnlRight.Dock = DockStyle.Fill;

            pnlCartList = new FlowLayoutPanel();
            pnlCartList.Dock = DockStyle.Fill;
            pnlCartList.FlowDirection = FlowDirection.TopDown;
            pnlCartList.WrapContents = false;
            pnlCartList.AutoScroll = true;
            pnlCartList.BackColor = Color.White;

            Panel pnlFooter = new Panel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 90;
            pnlFooter.BackColor = coffeeBg;

            lblTongTien = new Label();
            lblTongTien.Text = "Tổng tiền: 0 VNĐ";
            lblTongTien.Dock = DockStyle.Top;
            lblTongTien.Height = 35;
            lblTongTien.TextAlign = ContentAlignment.MiddleRight;
            lblTongTien.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            FlowLayoutPanel pnlBtn = new FlowLayoutPanel();
            pnlBtn.Dock = DockStyle.Bottom;
            pnlBtn.FlowDirection = FlowDirection.RightToLeft;
            pnlBtn.Height = 45;

            btnThanhToan = new Button();
            btnThanhToan.Text = "THANH TOÁN";
            btnThanhToan.BackColor = coffeeLight;
            btnThanhToan.Width = 130;
            btnThanhToan.Height = 35;

            btnXoaMon = new Button();
            btnXoaMon.Text = "XOÁ HẾT";
            btnXoaMon.BackColor = coffeeLight;
            btnXoaMon.Width = 100;
            btnXoaMon.Height = 35;

            pnlBtn.Controls.Add(btnThanhToan);
            pnlBtn.Controls.Add(btnXoaMon);

            pnlFooter.Controls.Add(pnlBtn);
            pnlFooter.Controls.Add(lblTongTien);

            pnlRight.Controls.Add(pnlCartList);
            pnlRight.Controls.Add(pnlFooter);

            grpCart.Controls.Add(pnlRight);

            mainLayout.Controls.Add(grpMenu, 0, 0);
            mainLayout.Controls.Add(grpCart, 1, 0);

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

        // ================= CART ITEM =================
        private Panel CreateCartItemUI(object[] item)
        {
            Panel row = new Panel();
            row.Width = 420;
            row.Height = 90;
            row.BorderStyle = BorderStyle.FixedSingle;
            row.BackColor = Color.White;

            // Ảnh
            PictureBox pic = new PictureBox();
            pic.Size = new Size(60, 60);
            pic.Location = new Point(5, 10);
            pic.SizeMode = PictureBoxSizeMode.Zoom;

            try
            {
                string path = item[4]?.ToString();

                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                    pic.Image = Image.FromFile(path);
            }
            catch { }

            // Tên món
            Label lblName = new Label();
            lblName.Text = item[0].ToString();
            lblName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblName.Location = new Point(75, 5);
            lblName.AutoSize = true;

            // Size
            ComboBox cbSize = new ComboBox();
            cbSize.Items.AddRange(new string[] { "S", "M", "L" });
            cbSize.SelectedItem = item[1].ToString();
            cbSize.Width = 50;
            cbSize.Location = new Point(75, 30);

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

            // Giá
            Label lblPrice = new Label();
            lblPrice.Text = $"{Convert.ToDouble(item[3]):N0} VNĐ";
            lblPrice.ForeColor = Color.Brown;
            lblPrice.Location = new Point(75, 60);
            lblPrice.AutoSize = true;

            // Minus
            Button btnMinus = CreateSmallBtn("-");
            btnMinus.Location = new Point(250, 28);

            // Quantity
            Label lblQty = new Label();
            lblQty.Text = item[2].ToString();
            lblQty.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblQty.Location = new Point(290, 32);
            lblQty.AutoSize = true;

            // Plus
            Button btnPlus = CreateSmallBtn("+");
            btnPlus.Location = new Point(320, 28);

            // Delete
            Button btnDelete = new Button();
            btnDelete.Text = "X";
            btnDelete.ForeColor = Color.Red;
            btnDelete.Width = 35;
            btnDelete.Height = 30;
            btnDelete.Location = new Point(365, 28);

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

            row.Controls.Add(pic);
            row.Controls.Add(lblName);
            row.Controls.Add(cbSize);
            row.Controls.Add(lblPrice);
            row.Controls.Add(btnMinus);
            row.Controls.Add(lblQty);
            row.Controls.Add(btnPlus);
            row.Controls.Add(btnDelete);

            return row;
        }

        // ================= SMALL BUTTON =================
        private Button CreateSmallBtn(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 30;
            btn.Height = 28;
            return btn;
        }

        // ================= MENU CARD =================
        public void AddMonCard(string maMon, string ten, double gia, string hinhAnh)
        {
            Panel card = new Panel();
            card.Width = 150;
            card.Height = 180;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.BackColor = Color.White;
            card.Margin = new Padding(10);

            PictureBox pic = new PictureBox();
            pic.Size = new Size(120, 110);
            pic.Location = new Point(15, 10);
            pic.SizeMode = PictureBoxSizeMode.Zoom;

            try
            {
                if (!string.IsNullOrEmpty(hinhAnh) && System.IO.File.Exists(hinhAnh))
                    pic.Image = Image.FromFile(hinhAnh);
            }
            catch { }

            Label lblTen = new Label();
            lblTen.Text = ten;
            lblTen.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTen.TextAlign = ContentAlignment.MiddleCenter;
            lblTen.Dock = DockStyle.Bottom;
            lblTen.Height = 25;

            Label lblGia = new Label();
            lblGia.Text = $"{gia:N0} VNĐ";
            lblGia.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblGia.TextAlign = ContentAlignment.MiddleCenter;
            lblGia.Dock = DockStyle.Bottom;
            lblGia.Height = 20;

            card.Controls.Add(pic);
            card.Controls.Add(lblGia);
            card.Controls.Add(lblTen);

            card.Click += (s, e) =>
            {
                AddMonToGio(maMon, ten, gia, hinhAnh);
            };

            pic.Click += (s, e) =>
            {
                AddMonToGio(maMon, ten, gia, hinhAnh);
            };

            pnlMenuGrid.Controls.Add(card);
        }

        // ================= GIỎ HÀNG =================
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

        // ================= GETTER =================
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
            lblTongTien.Text = $"Tổng tiền: {tien:N0} VNĐ";
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