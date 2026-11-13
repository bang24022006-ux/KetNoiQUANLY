namespace KetNoiQUANLY
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnVeForm1 = new Button();
            lblThongBao = new Label();
            cboBang = new ComboBox();
            btnTai = new Button();
            txtTimKiem = new TextBox();
            btnTim = new Button();
            dgvDuLieu = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvDuLieu).BeginInit();
            SuspendLayout();
            // 
            // btnVeForm1
            // 
            btnVeForm1.Location = new Point(20, 415);
            btnVeForm1.Name = "btnVeForm1";
            btnVeForm1.Size = new Size(180, 40);
            btnVeForm1.TabIndex = 1;
            btnVeForm1.Text = "QUAY VỀ FORM1";
            btnVeForm1.UseVisualStyleBackColor = true;
            btnVeForm1.Click += btnVeForm1_Click;
            // 
            // lblThongBao
            // 
            lblThongBao.AutoSize = true;
            lblThongBao.Location = new Point(20, 15);
            lblThongBao.Name = "lblThongBao";
            lblThongBao.Size = new Size(110, 20);
            lblThongBao.TabIndex = 2;
            lblThongBao.Text = "ĐÂY LÀ FORM2";
            // 
            // cboBang
            // 
            cboBang.FormattingEnabled = true;
            cboBang.Items.AddRange(new object[] { "LoaiHang", "NhaCungCap", "KhachHang", "HangHoa", "PhieuNhap", "ChiTietNhap", "HoaDon", "ChiTietHoaDon", "vw_TongTienHoaDon" });
            cboBang.Location = new Point(20, 50);
            cboBang.Name = "cboBang";
            cboBang.Size = new Size(200, 28);
            cboBang.TabIndex = 3;
            // 
            // btnTai
            // 
            btnTai.Location = new Point(240, 45);
            btnTai.Name = "btnTai";
            btnTai.Size = new Size(140, 35);
            btnTai.TabIndex = 4;
            btnTai.Text = "TẢI DỮ LIỆU";
            btnTai.UseVisualStyleBackColor = true;
            btnTai.Click += btnTai_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(420, 50);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(160, 27);
            txtTimKiem.TabIndex = 5;
            // 
            // btnTim
            // 
            btnTim.Location = new Point(600, 45);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(150, 35);
            btnTim.TabIndex = 6;
            btnTim.Text = "TÌM HÀNG HÓA";
            btnTim.UseVisualStyleBackColor = true;
            btnTim.Click += btnTim_Click;
            // 
            // dgvDuLieu
            // 
            dgvDuLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDuLieu.Location = new Point(20, 100);
            dgvDuLieu.Name = "dgvDuLieu";
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.RowHeadersWidth = 51;
            dgvDuLieu.Size = new Size(750, 300);
            dgvDuLieu.TabIndex = 7;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1227, 493);
            Controls.Add(dgvDuLieu);
            Controls.Add(btnTim);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTai);
            Controls.Add(cboBang);
            Controls.Add(lblThongBao);
            Controls.Add(btnVeForm1);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)dgvDuLieu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnVeForm1;
        private Label lblThongBao;
        private ComboBox cboBang;
       
        private TextBox txtTimKiem;
        private Button btnTim;
        private DataGridView dgvDuLieu;
        private Button btnTai;
    }
}