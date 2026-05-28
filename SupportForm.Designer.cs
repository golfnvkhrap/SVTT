namespace SVTT
{
    partial class SupportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picThanks;
        private System.Windows.Forms.PictureBox picQR;
        private System.Windows.Forms.Label lblText;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            picThanks = new PictureBox();
            picQR = new PictureBox();
            lblText = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)picThanks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQR).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // picThanks
            // 
            picThanks.Dock = DockStyle.Fill;
            picThanks.Image = Properties.Resources._02_2;
            picThanks.Location = new Point(3, 5);
            picThanks.Margin = new Padding(3, 5, 3, 5);
            picThanks.Name = "picThanks";
            picThanks.Size = new Size(301, 413);
            picThanks.SizeMode = PictureBoxSizeMode.Zoom;
            picThanks.TabIndex = 0;
            picThanks.TabStop = false;
            // 
            // picQR
            // 
            picQR.Dock = DockStyle.Fill;
            picQR.Image = Properties.Resources.receipt_20260528015913;
            picQR.Location = new Point(310, 5);
            picQR.Margin = new Padding(3, 5, 3, 5);
            picQR.Name = "picQR";
            picQR.Size = new Size(277, 413);
            picQR.SizeMode = PictureBoxSizeMode.Zoom;
            picQR.TabIndex = 1;
            picQR.TabStop = false;
            // 
            // lblText
            // 
            lblText.BackColor = Color.White;
            lblText.Dock = DockStyle.Fill;
            lblText.FlatStyle = FlatStyle.Flat;
            lblText.Font = new Font("Noto Sans Thai Looped", 9F);
            lblText.Location = new Point(0, 0);
            lblText.Margin = new Padding(0);
            lblText.Name = "lblText";
            lblText.Size = new Size(590, 100);
            lblText.TabIndex = 2;
            lblText.Text = "ขอบคุณสำหรับทุกการสนับสนุนครับ เงินสนับสนุนทั้งหมดจะถูกนำไปใช้เพื่อพัฒนาต่อยอด\r\nผลงาน และพัฒนาแอปลิเคชันต่อไป";
            lblText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 283F));
            tableLayoutPanel1.Controls.Add(picThanks, 0, 0);
            tableLayoutPanel1.Controls.Add(picQR, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(590, 423);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblText, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 429);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(590, 100);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // SupportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(590, 529);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Noto Sans Thai Looped", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 5, 3, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SupportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "สนับสนุนผู้พัฒนา (SVTT)";
            Load += SupportForm_Load;
            ((System.ComponentModel.ISupportInitialize)picThanks).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQR).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);

        }

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
    }
}