using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace KetNoiQUANLY
{
    public partial class Form2 : Form
    {
        private static readonly Color MauSidebar = ColorTranslator.FromHtml("#1E3A5F");
        private static readonly Color MauSidebarHover = ColorTranslator.FromHtml("#2B4C7E");

        private readonly List<Button> menuButtons = new List<Button>();
        private Label lblSoMatHang, lblTonThap, lblSoHoaDon, lblDoanhThu;
        private Button btnThemHang, btnNhapHang, btnLapHoaDon, btnXoa;

        // Cot khoa cua tung bang, dung de xoa dung dong. Neu ten cot trong
        // database cua ban khac, sua lai cho khop (vi du MaHD, MaHang...).
        private static readonly Dictionary<string, string[]> ColKhoa = new Dictionary<string, string[]>
        {
            { "HangHoa",            new[] { "MaHang" } },
            { "LoaiHang",           new[] { "MaLoai" } },
            { "NhaCungCap",         new[] { "MaNCC" } },
            { "KhachHang",          new[] { "MaKH" } },
            { "PhieuNhap",          new[] { "MaPN" } },
            { "ChiTietNhap",        new[] { "MaPN", "MaHang" } },
            { "HoaDon",             new[] { "MaHD" } },
            { "ChiTietHoaDon",      new[] { "MaHD", "MaHang" } },
        };
        private Button menuHienTai;   // menu dang chon, de nap lai bang sau khi them/nhap/ban

        // =====================================================
        // TEN COT TRONG DATABASE - SUA LAI CHO KHOP VOI BANG CUA BAN
        // =====================================================
        // HoaDon(MaHD, NgayLap, MaKH)
        // ChiTietHoaDon(MaHD, MaHang, SoLuong, DonGiaBan)
        // PhieuNhap(MaPN, NgayNhap, MaNCC)
        // ChiTietNhap(MaPN, MaHang, SoLuong, DonGiaNhap)
        // KhachHang(MaKH, TenKH)   NhaCungCap(MaNCC, TenNCC)
        // LoaiHang(MaLoai, TenLoai)

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
            this.FormClosed += Form2_FormClosed;
            XayDungGiaoDien();
        }

        // =====================================================
        // DUNG GIAO DIEN BANG CODE
        // =====================================================
        private void XayDungGiaoDien()
        {
            this.Text = "QUẢN LÝ HÀNG HÓA - DỮ LIỆU";
            this.ClientSize = new Size(1100, 650);
            Theme.StyleForm(this);

            cboBang.Visible = false;
            btnTai.Visible = false;

            var pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background };
            var pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 220, BackColor = MauSidebar };
            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlSidebar);

            // ---------- SIDEBAR ----------
            var lblLogo = new Label
            {
                Text = "QL HÀNG HÓA",
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold)
            };
            pnlSidebar.Controls.Add(lblLogo);

            string[,] menu =
            {
                { "Hàng hóa",          "HangHoa" },
                { "Loại hàng",         "LoaiHang" },
                { "Nhà cung cấp",      "NhaCungCap" },
                { "Khách hàng",        "KhachHang" },
                { "Phiếu nhập",        "PhieuNhap" },
                { "Chi tiết nhập",     "ChiTietNhap" },
                { "Hóa đơn",           "HoaDon" },
                { "Chi tiết hóa đơn",  "ChiTietHoaDon" },
                { "Tổng tiền hóa đơn", "vw_TongTienHoaDon" }
            };

            for (int i = 0; i < menu.GetLength(0); i++)
            {
                var btn = new Button
                {
                    Text = menu[i, 0],
                    Tag = menu[i, 1],
                    Location = new Point(0, 90 + i * 46),
                    Size = new Size(220, 44),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = MauSidebar,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(22, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = MauSidebarHover;
                btn.Click += (s, e) => ChonMenu((Button)s);
                menuButtons.Add(btn);
                pnlSidebar.Controls.Add(btn);
            }

            btnVeForm1.Text = "←  QUAY VỀ FORM1";
            Theme.StyleButton(btnVeForm1, false);
            btnVeForm1.Size = new Size(190, 40);
            btnVeForm1.Location = new Point(15, 590);
            btnVeForm1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlSidebar.Controls.Add(btnVeForm1);

            // ---------- KHUNG CHINH ----------
            lblThongBao.Dock = DockStyle.None;
            lblThongBao.AutoSize = true;
            lblThongBao.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblThongBao.ForeColor = ColorTranslator.FromHtml("#111827");
            lblThongBao.BackColor = Color.Transparent;
            lblThongBao.Location = new Point(20, 18);
            pnlMain.Controls.Add(lblThongBao);

            txtTimKiem.Font = new Font("Segoe UI", 11F);
            txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
            txtTimKiem.PlaceholderText = "Nhập tên hoặc mã hàng...";
            txtTimKiem.Size = new Size(190, 30);
            txtTimKiem.Location = new Point(360, 20);
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlMain.Controls.Add(txtTimKiem);

            btnTim.Text = "TÌM HÀNG";
            Theme.StyleButton(btnTim);
            btnTim.Size = new Size(130, 34);
            btnTim.Location = new Point(560, 18);
            btnTim.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlMain.Controls.Add(btnTim);
            this.AcceptButton = btnTim;

            btnXoa = new Button
            {
                Text = "XÓA DÒNG",
                Size = new Size(100, 34),
                Location = new Point(700, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#DC2626"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += btnXoa_Click;
            pnlMain.Controls.Add(btnXoa);

            // ---------- HANG NUT THAO TAC (moi) ----------
            btnThemHang = TaoNutThaoTac(pnlMain, "+ THÊM HÀNG MỚI", 20, "#2563EB");
            btnNhapHang = TaoNutThaoTac(pnlMain, "NHẬP THÊM HÀNG", 190, "#F59E0B");
            btnLapHoaDon = TaoNutThaoTac(pnlMain, "LẬP HÓA ĐƠN", 360, "#10B981");
            btnThemHang.Click += btnThemHang_Click;
            btnNhapHang.Click += btnNhapHang_Click;
            btnLapHoaDon.Click += btnLapHoaDon_Click;

            // 4 the thong ke
            lblSoMatHang = TaoThe(pnlMain, 0, "SỐ MẶT HÀNG", "#2563EB");
            lblTonThap = TaoThe(pnlMain, 1, "HÀNG TỒN KHO THẤP (<50)", "#F59E0B");
            lblSoHoaDon = TaoThe(pnlMain, 2, "SỐ HÓA ĐƠN", "#10B981");
            lblDoanhThu = TaoThe(pnlMain, 3, "TỔNG DOANH THU", "#8B5CF6");

            // Luoi du lieu
            dgvDuLieu.Location = new Point(20, 215);
            dgvDuLieu.Size = new Size(840, 415);
            dgvDuLieu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.StyleGrid(dgvDuLieu);
            pnlMain.Controls.Add(dgvDuLieu);
        }

        private Button TaoNutThaoTac(Panel parent, string text, int x, string mauHex)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 62),
                Size = new Size(160, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml(mauHex),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            parent.Controls.Add(btn);
            return btn;
        }

        private Label TaoThe(Panel parent, int index, string tieuDe, string mauHex)
        {
            Color mau = ColorTranslator.FromHtml(mauHex);

            var card = new Panel
            {
                BackColor = Color.White,
                Size = new Size(198, 90),
                Location = new Point(20 + index * 214, 110)
            };
            var strip = new Panel { BackColor = mau, Dock = DockStyle.Left, Width = 6 };
            var lblTitle = new Label
            {
                Text = tieuDe,
                AutoSize = false,
                Location = new Point(18, 12),
                Size = new Size(175, 22),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
            var lblValue = new Label
            {
                Text = "0",
                AutoSize = false,
                Location = new Point(18, 38),
                Size = new Size(176, 40),
                ForeColor = mau,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold)
            };

            card.Controls.Add(strip);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            parent.Controls.Add(card);
            return lblValue;
        }

        // =====================================================
        // XU LY SU KIEN CO BAN
        // =====================================================
        private void Form2_Load(object sender, EventArgs e)
        {
            CapNhatThe();
            ChonMenu(menuButtons[0]);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is Form1)
                {
                    f.Show();
                    break;
                }
            }
        }

        private void ChonMenu(Button chon)
        {
            menuHienTai = chon;
            foreach (var b in menuButtons)
                b.BackColor = (b == chon) ? Theme.Primary : MauSidebar;

            lblThongBao.Text = chon.Text;
            TaiDuLieu("SELECT * FROM " + chon.Tag.ToString());
        }

        private object LayGiaTri(string sql)
        {
            using (var conn = DB.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        private void CapNhatThe()
        {
            try
            {
                lblSoMatHang.Text = LayGiaTri("SELECT COUNT(*) FROM HangHoa").ToString();
                lblTonThap.Text = LayGiaTri("SELECT COUNT(*) FROM HangHoa WHERE SoLuongTon < 50").ToString();
                lblSoHoaDon.Text = LayGiaTri("SELECT COUNT(*) FROM HoaDon").ToString();

                decimal doanhThu = Convert.ToDecimal(
                    LayGiaTri("SELECT ISNULL(SUM(SoLuong * DonGiaBan), 0) FROM ChiTietHoaDon"));
                lblDoanhThu.Text = doanhThu.ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thống kê: " + ex.Message);
            }
        }

        private void TaiDuLieu(string sql, params SqlParameter[] ps)
        {
            try
            {
                using (var conn = DB.GetConnection())
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    if (ps != null) da.SelectCommand.Parameters.AddRange(ps);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDuLieu.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            foreach (var b in menuButtons) b.BackColor = MauSidebar;
            lblThongBao.Text = "Kết quả tìm kiếm";

            TaiDuLieu(
                "SELECT * FROM HangHoa WHERE TenHang LIKE @kw OR MaHang LIKE @kw",
                new SqlParameter("@kw", "%" + txtTimKiem.Text.Trim() + "%"));
        }

        private void btnVeForm1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTai_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        // =====================================================
        // TIEN ICH CHUNG CHO CAC HOP THOAI
        // =====================================================

        // Sau moi thao tac: cap nhat the thong ke + nap lai bang dang xem
        private void LamMoiSauThaoTac()
        {
            CapNhatThe();
            if (menuHienTai != null) ChonMenu(menuHienTai);
        }

        // Chay nhieu lenh SQL trong 1 transaction: loi la rollback het
        private void ChayTransaction(Action<SqlConnection, SqlTransaction> hanhDong)
        {
            using (var conn = DB.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        hanhDong(conn, tran);
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private int ChayLenh(SqlConnection conn, SqlTransaction tran, string sql, params SqlParameter[] ps)
        {
            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                if (ps != null) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }

        // Sinh ma tu tang, vd: HD06 (prefix + so, it nhat 2 chu so)
        private string SinhMa(SqlConnection conn, SqlTransaction tran, string bang, string cot, string prefix)
        {
            string sql = "SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(" + cot + ", " + (prefix.Length + 1) + ", 20) AS int)), 0) + 1 " +
                         "FROM " + bang + " WITH (UPDLOCK, HOLDLOCK) WHERE " + cot + " LIKE @p + '%'";
            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@p", prefix);
                int so = Convert.ToInt32(cmd.ExecuteScalar());
                return prefix + so.ToString("D2");
            }
        }

        private void NapCombo(ComboBox cbo, string sql, string cotGiaTri, string cotHienThi)
        {
            using (var conn = DB.GetConnection())
            using (var da = new SqlDataAdapter(sql, conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.DataSource = dt;
                cbo.ValueMember = cotGiaTri;
                cbo.DisplayMember = cotHienThi;
            }
        }

        private NumericUpDown TaoSo(decimal max, decimal min = 0, decimal value = 0)
        {
            return new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                ThousandsSeparator = true,
                DecimalPlaces = 0
            };
        }

        // Tao hop thoai nhap lieu: moi dong gom nhan + control
        private Form TaoDialog(string tieuDe, params (string nhan, Control ctl)[] dong)
        {
            var f = new Form
            {
                Text = tieuDe,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = new Font("Segoe UI", 10F),
                ClientSize = new Size(400, 30 + dong.Length * 40 + 70)
            };

            int y = 20;
            foreach (var (nhan, ctl) in dong)
            {
                f.Controls.Add(new Label { Text = nhan, Location = new Point(20, y + 4), Size = new Size(125, 24) });
                ctl.Location = new Point(150, y);
                ctl.Width = 230;
                f.Controls.Add(ctl);
                y += 40;
            }

            var ok = new Button
            {
                Text = "LƯU",
                DialogResult = DialogResult.OK,
                Size = new Size(110, 36),
                Location = new Point(150, y + 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#2563EB"),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false
            };
            ok.FlatAppearance.BorderSize = 0;
            var huy = new Button
            {
                Text = "HỦY",
                DialogResult = DialogResult.Cancel,
                Size = new Size(110, 36),
                Location = new Point(270, y + 10)
            };
            f.Controls.Add(ok);
            f.Controls.Add(huy);
            f.AcceptButton = ok;
            f.CancelButton = huy;
            return f;
        }

        // =====================================================
        // 1) THEM HANG HOA MOI  -> so mat hang + ton kho tang
        // =====================================================
        private void btnThemHang_Click(object sender, EventArgs e)
        {
            var txtMa = new TextBox();
            var txtTen = new TextBox();
            var cboLoai = new ComboBox();
            var txtDvt = new TextBox();
            var numGiaNhap = TaoSo(1000000000);
            var numGiaBan = TaoSo(1000000000);
            var numSoLuong = TaoSo(1000000);

            try { NapCombo(cboLoai, "SELECT MaLoai, MaLoai + N' - ' + TenLoai AS Hien FROM LoaiHang", "MaLoai", "Hien"); }
            catch (Exception ex) { MessageBox.Show("Không tải được loại hàng: " + ex.Message); return; }

            using (var dlg = TaoDialog("Thêm hàng hóa mới",
                ("Mã hàng", txtMa), ("Tên hàng", txtTen), ("Loại hàng", cboLoai),
                ("Đơn vị tính", txtDvt), ("Giá nhập", numGiaNhap), ("Giá bán", numGiaBan),
                ("Số lượng ban đầu", numSoLuong)))
            {
                while (true)
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    if (txtMa.Text.Trim() == "" || txtTen.Text.Trim() == "" || cboLoai.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng nhập mã hàng, tên hàng và chọn loại hàng.");
                        continue;
                    }
                    if (numGiaBan.Value < numGiaNhap.Value &&
                        MessageBox.Show("Giá bán đang thấp hơn giá nhập. Vẫn lưu?", "Xác nhận",
                            MessageBoxButtons.YesNo) == DialogResult.No)
                        continue;
                    break;
                }

                try
                {
                    ChayTransaction((conn, tran) =>
                    {
                        object tonTai;
                        using (var chk = new SqlCommand("SELECT COUNT(*) FROM HangHoa WHERE MaHang = @ma", conn, tran))
                        {
                            chk.Parameters.AddWithValue("@ma", txtMa.Text.Trim());
                            tonTai = chk.ExecuteScalar();
                        }
                        if (Convert.ToInt32(tonTai) > 0)
                            throw new Exception("Mã hàng '" + txtMa.Text.Trim() + "' đã tồn tại.");

                        ChayLenh(conn, tran,
                            "INSERT INTO HangHoa (MaHang, TenHang, MaLoai, DonViTinh, GiaNhap, GiaBan, SoLuongTon) " +
                            "VALUES (@ma, @ten, @loai, @dvt, @gn, @gb, @sl)",
                            new SqlParameter("@ma", txtMa.Text.Trim()),
                            new SqlParameter("@ten", txtTen.Text.Trim()),
                            new SqlParameter("@loai", cboLoai.SelectedValue),
                            new SqlParameter("@dvt", txtDvt.Text.Trim()),
                            new SqlParameter("@gn", numGiaNhap.Value),
                            new SqlParameter("@gb", numGiaBan.Value),
                            new SqlParameter("@sl", (int)numSoLuong.Value));
                    });

                    MessageBox.Show("Đã thêm hàng hóa thành công!");
                    LamMoiSauThaoTac();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm hàng: " + ex.Message);
                }
            }
        }

        // =====================================================
        // 2) NHAP THEM HANG -> lap phieu nhap, ton kho tang
        // =====================================================
        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            var cboHang = new ComboBox();
            var cboNCC = new ComboBox();
            var numSoLuong = TaoSo(1000000, 1, 1);
            var numGiaNhap = TaoSo(1000000000);

            try
            {
                NapCombo(cboHang,
                    "SELECT MaHang, MaHang + N' - ' + TenHang + N' (tồn ' + CAST(SoLuongTon AS nvarchar(20)) + N')' AS Hien FROM HangHoa",
                    "MaHang", "Hien");
                NapCombo(cboNCC, "SELECT MaNCC, MaNCC + N' - ' + TenNCC AS Hien FROM NhaCungCap", "MaNCC", "Hien");
            }
            catch (Exception ex) { MessageBox.Show("Không tải được danh sách: " + ex.Message); return; }

            // Chon hang -> tu dien gia nhap hien tai
            EventHandler doiHang = (s, ev) =>
            {
                if (cboHang.SelectedValue == null) return;
                try
                {
                    object gia = LayGiaTriThamSo("SELECT GiaNhap FROM HangHoa WHERE MaHang = @ma", cboHang.SelectedValue);
                    if (gia != null && gia != DBNull.Value) numGiaNhap.Value = Convert.ToDecimal(gia);
                }
                catch { }
            };
            cboHang.SelectedIndexChanged += doiHang;
            doiHang(null, EventArgs.Empty);

            using (var dlg = TaoDialog("Nhập thêm hàng",
                ("Hàng hóa", cboHang), ("Nhà cung cấp", cboNCC),
                ("Số lượng nhập", numSoLuong), ("Giá nhập", numGiaNhap)))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                if (cboHang.SelectedValue == null || cboNCC.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa và nhà cung cấp.");
                    return;
                }

                try
                {
                    ChayTransaction((conn, tran) =>
                    {
                        string maPN = SinhMa(conn, tran, "PhieuNhap", "MaPN", "PN");

                        ChayLenh(conn, tran,
                            "INSERT INTO PhieuNhap (MaPN, NgayNhap, MaNCC) VALUES (@pn, @ngay, @ncc)",
                            new SqlParameter("@pn", maPN),
                            new SqlParameter("@ngay", DateTime.Now),
                            new SqlParameter("@ncc", cboNCC.SelectedValue));

                        ChayLenh(conn, tran,
                            "INSERT INTO ChiTietNhap (MaPN, MaHang, SoLuong, DonGiaNhap) VALUES (@pn, @hang, @sl, @gia)",
                            new SqlParameter("@pn", maPN),
                            new SqlParameter("@hang", cboHang.SelectedValue),
                            new SqlParameter("@sl", (int)numSoLuong.Value),
                            new SqlParameter("@gia", numGiaNhap.Value));

                        // Ton kho tang
                        ChayLenh(conn, tran,
                            "UPDATE HangHoa SET SoLuongTon = SoLuongTon + @sl, GiaNhap = @gia WHERE MaHang = @hang",
                            new SqlParameter("@sl", (int)numSoLuong.Value),
                            new SqlParameter("@gia", numGiaNhap.Value),
                            new SqlParameter("@hang", cboHang.SelectedValue));
                    });

                    MessageBox.Show("Nhập hàng thành công! Tồn kho đã được cộng thêm.");
                    LamMoiSauThaoTac();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhập hàng: " + ex.Message);
                }
            }
        }

        // =====================================================
        // 3) LAP HOA DON -> ton kho giam, so hoa don + doanh thu tang
        // =====================================================
        private void btnLapHoaDon_Click(object sender, EventArgs e)
        {
            var cboHang = new ComboBox();
            var cboKH = new ComboBox();
            var numSoLuong = TaoSo(1000000, 1, 1);
            var lblGia = new Label { Text = "0 đ", ForeColor = ColorTranslator.FromHtml("#8B5CF6"), AutoSize = false, Height = 24 };

            try
            {
                NapCombo(cboHang,
                    "SELECT MaHang, MaHang + N' - ' + TenHang + N' (tồn ' + CAST(SoLuongTon AS nvarchar(20)) + N')' AS Hien FROM HangHoa",
                    "MaHang", "Hien");
                NapCombo(cboKH, "SELECT MaKH, MaKH + N' - ' + TenKH AS Hien FROM KhachHang", "MaKH", "Hien");
            }
            catch (Exception ex) { MessageBox.Show("Không tải được danh sách: " + ex.Message); return; }

            // Hien thanh tien tam tinh
            EventHandler tinhTien = (s, ev) =>
            {
                if (cboHang.SelectedValue == null) return;
                try
                {
                    object gia = LayGiaTriThamSo("SELECT GiaBan FROM HangHoa WHERE MaHang = @ma", cboHang.SelectedValue);
                    decimal g = (gia == null || gia == DBNull.Value) ? 0 : Convert.ToDecimal(gia);
                    lblGia.Text = (g * numSoLuong.Value).ToString("N0") + " đ  (đơn giá " + g.ToString("N0") + ")";
                }
                catch { }
            };
            cboHang.SelectedIndexChanged += tinhTien;
            numSoLuong.ValueChanged += tinhTien;
            tinhTien(null, EventArgs.Empty);

            using (var dlg = TaoDialog("Lập hóa đơn bán hàng",
                ("Khách hàng", cboKH), ("Hàng hóa", cboHang),
                ("Số lượng bán", numSoLuong), ("Thành tiền", lblGia)))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                if (cboHang.SelectedValue == null || cboKH.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng và hàng hóa.");
                    return;
                }

                try
                {
                    ChayTransaction((conn, tran) =>
                    {
                        // Khoa dong hang va kiem tra ton kho
                        int ton; decimal giaBan;
                        using (var cmd = new SqlCommand(
                            "SELECT SoLuongTon, GiaBan FROM HangHoa WITH (UPDLOCK) WHERE MaHang = @ma", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@ma", cboHang.SelectedValue);
                            using (var rd = cmd.ExecuteReader())
                            {
                                if (!rd.Read()) throw new Exception("Không tìm thấy hàng hóa.");
                                ton = Convert.ToInt32(rd["SoLuongTon"]);
                                giaBan = Convert.ToDecimal(rd["GiaBan"]);
                            }
                        }
                        if (numSoLuong.Value > ton)
                            throw new Exception("Không đủ hàng! Tồn kho hiện chỉ còn " + ton + ".");

                        string maHD = SinhMa(conn, tran, "HoaDon", "MaHD", "HD");

                        ChayLenh(conn, tran,
                            "INSERT INTO HoaDon (MaHD, NgayBan, MaKH) VALUES (@hd, @ngay, @kh)",
                            new SqlParameter("@hd", maHD),
                            new SqlParameter("@ngay", DateTime.Now),
                            new SqlParameter("@kh", cboKH.SelectedValue));

                        ChayLenh(conn, tran,
                            "INSERT INTO ChiTietHoaDon (MaHD, MaHang, SoLuong, DonGiaBan) VALUES (@hd, @hang, @sl, @gia)",
                            new SqlParameter("@hd", maHD),
                            new SqlParameter("@hang", cboHang.SelectedValue),
                            new SqlParameter("@sl", (int)numSoLuong.Value),
                            new SqlParameter("@gia", giaBan));

                        // Ton kho giam
                        ChayLenh(conn, tran,
                            "UPDATE HangHoa SET SoLuongTon = SoLuongTon - @sl WHERE MaHang = @hang",
                            new SqlParameter("@sl", (int)numSoLuong.Value),
                            new SqlParameter("@hang", cboHang.SelectedValue));
                    });

                    MessageBox.Show("Lập hóa đơn thành công!");
                    LamMoiSauThaoTac();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lập hóa đơn: " + ex.Message);
                }
            }
        }

        // =====================================================
        // 4) XOA DONG DANG CHON TRONG BANG
        //    - Xoa hoa don: cong tra lai ton kho
        //    - Xoa phieu nhap: tru lai ton kho (chan neu khong du hang de tru)
        //    - Cac bang khac: xoa truc tiep theo khoa
        // =====================================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (menuHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn một bảng ở menu bên trái trước.");
                return;
            }
            string bang = menuHienTai.Tag.ToString();

            if (bang == "vw_TongTienHoaDon")
            {
                MessageBox.Show("Đây là báo cáo tổng hợp, không thể xóa trực tiếp.");
                return;
            }
            if (!ColKhoa.ContainsKey(bang))
            {
                MessageBox.Show("Chưa cấu hình cách xóa cho bảng này.");
                return;
            }
            if (dgvDuLieu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng để xóa.");
                return;
            }

            DataRowView dong = dgvDuLieu.CurrentRow.DataBoundItem as DataRowView;
            if (dong == null) return;

            string[] cotKhoa = ColKhoa[bang];
            var giaTriKhoa = new List<object>();
            foreach (var c in cotKhoa)
            {
                if (!dong.Row.Table.Columns.Contains(c))
                {
                    MessageBox.Show("Không tìm thấy cột '" + c + "' trong dữ liệu. Hãy kiểm tra lại tên cột trong ColKhoa.");
                    return;
                }
                giaTriKhoa.Add(dong[c]);
            }

            string moTa = string.Join(" / ", cotKhoa.Select((c, i) => c + "=" + giaTriKhoa[i]));
            if (MessageBox.Show("Xóa dòng [" + moTa + "]?\nHành động này không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                switch (bang)
                {
                    case "HoaDon":
                        XoaHoaDon(giaTriKhoa[0]);
                        break;
                    case "PhieuNhap":
                        XoaPhieuNhap(giaTriKhoa[0]);
                        break;
                    default:
                        XoaTheoKhoa(bang, cotKhoa, giaTriKhoa.ToArray());
                        break;
                }

                MessageBox.Show("Đã xóa thành công!");
                LamMoiSauThaoTac();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa: " + ex.Message +
                    "\n\n(Dòng này có thể đang được dùng ở bảng khác, ví dụ hàng hóa đã có trong hóa đơn/phiếu nhập.)");
            }
        }

        // Xoa truc tiep theo (cac) cot khoa, dung cho bang don gian (1 hoac nhieu cot khoa)
        private void XoaTheoKhoa(string bang, string[] cotKhoa, object[] giaTri)
        {
            string whereClause = string.Join(" AND ", cotKhoa.Select((c, i) => c + " = @k" + i));
            var ps = new List<SqlParameter>();
            for (int i = 0; i < cotKhoa.Length; i++)
                ps.Add(new SqlParameter("@k" + i, giaTri[i] ?? DBNull.Value));

            ChayTransaction((conn, tran) =>
            {
                ChayLenh(conn, tran, "DELETE FROM " + bang + " WHERE " + whereClause, ps.ToArray());
            });
        }

        // Xoa hoa don: cong tra lai ton kho cho tung mat hang trong chi tiet hoa don, roi moi xoa
        private void XoaHoaDon(object maHD)
        {
            ChayTransaction((conn, tran) =>
            {
                using (var cmd = new SqlCommand(
                    "SELECT MaHang, SoLuong FROM ChiTietHoaDon WHERE MaHD = @hd", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@hd", maHD);
                    var ds = new List<(object maHang, int soLuong)>();
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            ds.Add((rd["MaHang"], Convert.ToInt32(rd["SoLuong"])));

                    foreach (var d in ds)
                    {
                        ChayLenh(conn, tran,
                            "UPDATE HangHoa SET SoLuongTon = SoLuongTon + @sl WHERE MaHang = @hang",
                            new SqlParameter("@sl", d.soLuong),
                            new SqlParameter("@hang", d.maHang));
                    }
                }

                ChayLenh(conn, tran, "DELETE FROM ChiTietHoaDon WHERE MaHD = @hd", new SqlParameter("@hd", maHD));
                ChayLenh(conn, tran, "DELETE FROM HoaDon WHERE MaHD = @hd", new SqlParameter("@hd", maHD));
            });
        }

        // Xoa phieu nhap: tru lai ton kho cho tung mat hang trong chi tiet nhap, roi moi xoa.
        // Neu ton kho khong du de tru (vi da ban bot) thi chan lai, bao loi.
        private void XoaPhieuNhap(object maPN)
        {
            ChayTransaction((conn, tran) =>
            {
                using (var cmd = new SqlCommand(
                    "SELECT MaHang, SoLuong FROM ChiTietNhap WHERE MaPN = @pn", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@pn", maPN);
                    var ds = new List<(object maHang, int soLuong)>();
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            ds.Add((rd["MaHang"], Convert.ToInt32(rd["SoLuong"])));

                    foreach (var d in ds)
                    {
                        using (var chk = new SqlCommand(
                            "SELECT SoLuongTon FROM HangHoa WITH (UPDLOCK) WHERE MaHang = @hang", conn, tran))
                        {
                            chk.Parameters.AddWithValue("@hang", d.maHang);
                            int ton = Convert.ToInt32(chk.ExecuteScalar());
                            if (ton < d.soLuong)
                                throw new Exception("Hàng '" + d.maHang + "' chỉ còn tồn " + ton +
                                    ", không đủ để trừ lại " + d.soLuong + " (có thể đã bán bớt).");
                        }

                        ChayLenh(conn, tran,
                            "UPDATE HangHoa SET SoLuongTon = SoLuongTon - @sl WHERE MaHang = @hang",
                            new SqlParameter("@sl", d.soLuong),
                            new SqlParameter("@hang", d.maHang));
                    }
                }

                ChayLenh(conn, tran, "DELETE FROM ChiTietNhap WHERE MaPN = @pn", new SqlParameter("@pn", maPN));
                ChayLenh(conn, tran, "DELETE FROM PhieuNhap WHERE MaPN = @pn", new SqlParameter("@pn", maPN));
            });
        }

        private object LayGiaTriThamSo(string sql, object thamSo)
        {
            using (var conn = DB.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", thamSo);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}