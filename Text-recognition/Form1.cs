using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_recognition
{
    public delegate void copyToFatherTextBox(Rectangle r);
    public partial class Form1 : Form
    {
        public List<String> imagePathList = new List<string>();
        public int index = 0;
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

        //截图
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
        }

        //接收截图返回数据
        public void screenForm_returnFormImageString(Bitmap bt)
        {
            Image image = new Bitmap(bt);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = image;
        }

        //本地加载图片
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹"; //窗体标题
            dialog.Filter = "图片文件(*.jpg,*.png)|*.jpg;*.png"; //文件筛选
            //默认路径设置为我的电脑文件夹
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(String file in dialog.FileNames)
                {
                    imagePathList.Add(file);
                }
                pictureBox1.Image = Image.FromFile(imagePathList[index]);
            }
        }

        //下一页
        private void button6_Click(object sender, EventArgs e)
        {
            if (imagePathList.Count != 0)
            {
                if(index == imagePathList.Count - 1)
                {
                    index = -1;
                }
                pictureBox1.Image = Image.FromFile(imagePathList[++index]);
            }
        }

        //上一页
        private void button7_Click(object sender, EventArgs e)
        {
            if(imagePathList.Count!= 0)
            {
                if (index == 0)
                {
                    index = imagePathList.Count;
                }
                pictureBox1.Image = Image.FromFile(imagePathList[--index]);
            }
        }

        //清空数据
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = "";
            imagePathList.Clear();
        }
    }
}
