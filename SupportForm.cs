using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SVTT
{
    public partial class SupportForm : Form
    {
        public SupportForm()
        {
            InitializeComponent();

            // ✅ ผูก Event และเปลี่ยนเมาส์รูปมือให้กับ picQR (ภาพ QR Code สำหรับสแกน) เท่านั้น
            picQR.MouseClick += PictureBox_MouseClick;
            picQR.Cursor = Cursors.Hand;
        }

        private void SupportForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ แก้ไขให้โหลดไฟล์ตรงตามตัวแปรที่ควรจะเป็นแล้วครับ
                if (File.Exists("qrcode.png"))
                {
                    picQR.Image = Image.FromFile("qrcode.png");
                }

                if (File.Exists("thankyou.png"))
                {
                    picThanks.Image = Image.FromFile("thankyou.png");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถโหลดภาพประกอบได้: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // เมธอดจัดการการคลิกซ้าย
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender is PictureBox clickedPictureBox && clickedPictureBox.Image != null)
                {
                    ShowExpandedImage(clickedPictureBox.Image);
                }
            }
        }

        // ฟังก์ชันสร้างหน้าต่างขยายภาพชั่วคราว
        private void ShowExpandedImage(Image image)
        {
            Form viewerForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.Black,
                ShowInTaskbar = false
            };

            int maxWidth = (int)(Screen.PrimaryScreen.Bounds.Width * 0.8);
            int maxHeight = (int)(Screen.PrimaryScreen.Bounds.Height * 0.8);

            int newWidth = image.Width;
            int newHeight = image.Height;

            if (newWidth > maxWidth || newHeight > maxHeight)
            {
                double ratioX = (double)maxWidth / image.Width;
                double ratioY = (double)maxHeight / image.Height;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);
            }

            viewerForm.Size = new Size(newWidth, newHeight);

            PictureBox pb = new PictureBox
            {
                Image = image,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand
            };

            pb.Click += (s, a) => viewerForm.Close();
            viewerForm.KeyDown += (s, a) => viewerForm.Close();

            viewerForm.Controls.Add(pb);
            viewerForm.ShowDialog(this);
        }
    }
}