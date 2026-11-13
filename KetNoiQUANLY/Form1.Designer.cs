namespace KetNoiQUANLY
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnKetNoi = new Button();
            btnSangForm2 = new Button();
            lblTieuDe = new Label();
            SuspendLayout();
            // 
            // btnKetNoi
            // 
            btnKetNoi.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnKetNoi.Location = new Point(310, 160);
            btnKetNoi.Name = "btnKetNoi";
            btnKetNoi.Size = new Size(180, 50);
            btnKetNoi.TabIndex = 0;
            btnKetNoi.Text = "KẾT NỐI CSDL";
            btnKetNoi.UseVisualStyleBackColor = true;
            btnKetNoi.Click += btnKetNoi_Click;
            // 
            // btnSangForm2
            // 
            btnSangForm2.Location = new Point(310, 240);
            btnSangForm2.Name = "btnSangForm2";
            btnSangForm2.Size = new Size(180, 50);
            btnSangForm2.TabIndex = 1;
            btnSangForm2.Text = "SANG FORM2";
            btnSangForm2.UseVisualStyleBackColor = true;
            btnSangForm2.Click += btnSangForm2_Click;
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDe.Location = new Point(0, 60);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(800, 50);
            lblTieuDe.TabIndex = 2;
            lblTieuDe.Text = "QUẢN LÝ HÀNG HÓA, MUA VÀ BÁN HÀNG";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 403);
            Controls.Add(lblTieuDe);
            Controls.Add(btnSangForm2);
            Controls.Add(btnKetNoi);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "QUAN LY HANG HOA";
            ResumeLayout(false);
        }

        #endregion

        private Button btnKetNoi;
        private Button btnSangForm2;
        private Label lblTieuDe;
    }
}
