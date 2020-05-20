using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_recognition
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        DateTime dateTime;
        Graphics graphics;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Width = 400;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphics = this.CreateGraphics();
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            Bitmap bmp = new Bitmap(ScreenArea.Width, ScreenArea.Height);
            graphics.CopyFromScreen(0, 0, 0, 0, new Size(ScreenArea.Width, ScreenArea.Height));
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBox1.Image = bmp;
            bmp.Dispose();
        }
    }
}
