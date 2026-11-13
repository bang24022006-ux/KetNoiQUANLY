using System;
using System.Drawing;
using System.Windows.Forms;

namespace KetNoiQUANLY
{
    // Chua mau sac va ham dinh dang chung cho Form1, Form2
    public static class Theme
    {
        public static readonly Color Primary = ColorTranslator.FromHtml("#2563EB"); // xanh duong
        public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#1E40AF");
        public static readonly Color Secondary = ColorTranslator.FromHtml("#6B7280"); // xam
        public static readonly Color SecondaryDark = ColorTranslator.FromHtml("#4B5563");
        public static readonly Color Background = ColorTranslator.FromHtml("#F3F4F6");
        public static readonly Color RowAlt = ColorTranslator.FromHtml("#EFF6FF");
        public static readonly Color RowSelect = ColorTranslator.FromHtml("#BFDBFE");
        public static readonly Color GridLine = ColorTranslator.FromHtml("#E5E7EB");

        public static void StyleForm(Form f)
        {
            f.BackColor = Background;
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        // Bien Label thanh thanh tieu de mau xanh o tren cung
        public static void StyleHeader(Label l, int height)
        {
            l.AutoSize = false;
            l.Dock = DockStyle.Top;
            l.Height = height;
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.BackColor = Primary;
            l.ForeColor = Color.White;
            l.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        }

        // primary = true: nut xanh; false: nut xam
        public static void StyleButton(Button b, bool primary = true)
        {
            Color normal = primary ? Primary : Secondary;
            Color hover = primary ? PrimaryDark : SecondaryDark;

            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hover;
            b.FlatAppearance.MouseDownBackColor = hover;
            b.BackColor = normal;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }

        public static void StyleGrid(DataGridView g)
        {
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Color.White;
            g.GridColor = GridLine;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 38;
            g.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primary;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            g.RowHeadersVisible = false;
            g.RowTemplate.Height = 30;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            g.DefaultCellStyle.SelectionBackColor = RowSelect;
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;

            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Cot tien: them dau phan cach hang nghin, can phai
            g.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewColumn c in g.Columns)
                {
                    if (c.Name.StartsWith("Gia") || c.Name.StartsWith("DonGia") || c.Name == "TongTien")
                    {
                        c.DefaultCellStyle.Format = "N0";
                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            };
        }
    }
}
