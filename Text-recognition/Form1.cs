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
    public delegate void copyToFatherTextBox(Rectangle r);
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point point = new Point(Screen.PrimaryScreen.Bounds.Left-8, Screen.PrimaryScreen.Bounds.Top);
            this.textBox1.Width = 400;
            this.Location = point;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 新建一个和屏幕大小相同的图片
            Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            // 创建一个画板，让我们可以在画板上画图
            Graphics g = Graphics.FromImage(CatchBmp);
            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            // 创建截图窗体
            ScreenForm screenForm = new ScreenForm();
            // 指示窗体的背景图片为屏幕图片
            screenForm.BackgroundImage = CatchBmp;
            screenForm.returnFormImageString += new returnImageString(screenForm_returnFormImageString);
            screenForm.ShowDialog();
            /*//获得当前屏幕的分辨率
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            //创建一个和屏幕一样大的Bitmap
            Image myImage = new Bitmap(iWidth, iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics g = Graphics.FromImage(myImage);
            //抓屏并拷贝到myimage里
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));
            //保存为文件
            myImage.Save(@"c:/1.jpeg");
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = Image.FromFile("c:/1.jpeg");*/
        }

        public void screenForm_returnFormImageString(Bitmap bt)
        {
            Image image = new Bitmap(bt);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = image;
        }
    }
}
