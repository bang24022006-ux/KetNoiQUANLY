using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace KetNoiQUANLY
{
    public partial class Form1 : Form
    {
        // Chuoi ket noi SQL Server
        string chuoiketnoi = @"Server=.\SQLEXPRESS; Database=QUANLY; Integrated Security=True; TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();

            this.Text = "QUẢN LÝ HÀNG HÓA";
            Theme.StyleForm(this);
            Theme.StyleHeader(lblTieuDe, 100);
            Theme.StyleButton(btnKetNoi);
            Theme.StyleButton(btnSangForm2);
        }

        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    MessageBox.Show("DA KET NOI THANH CONG CSDL QUANLY", "THONG BAO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOI KET NOI: " + ex.Message, "THONG BAO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSangForm2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            this.Hide();

        }
    }
}