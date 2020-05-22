using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public SortedSet<String> imagePathList = new SortedSet<string>();
        public int index = 0;//当前显示下标
        public int count = 0;//导入文件总数目
        Api Api;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point point = new Point(Screen.PrimaryScreen.Bounds.Left-8, Screen.PrimaryScreen.Bounds.Top);
            this.Width = Screen.PrimaryScreen.Bounds.Width/2;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 50;
            this.textBox1.Width = this.Width/2 - 20;
            this.textBox1.Height = this.Height - 50;
            this.pictureBox1.Width = this.Width / 2 - 20;
            this.pictureBox1.Height = this.Height - 100;
            this.Location = point;
            Api = new Api();
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
            image.Save("temp.jpg");
            suit();
            pictureBox1.Image = image;
            textBox1.Text = Api.GeneralBasicDemo("temp.jpg");
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
                suit();
                pictureBox1.Image = Image.FromFile(imagePathList.ElementAt(index));
                textBox1.Text = Api.GeneralBasicDemo(imagePathList.ElementAt(index));
                count = imagePathList.Count;
            }
        }

        //下一页
        private void button6_Click(object sender, EventArgs e)
        {
            if (imagePathList.Count > 1)
            {
                if(index >= imagePathList.Count - 1)
                {
                    index = -1;
                }
                suit();
                pictureBox1.Image = Image.FromFile(imagePathList.ElementAt(++index));
                textBox1.Text = Api.GeneralBasicDemo(imagePathList.ElementAt(index));
            }
        }

        //上一页
        private void button7_Click(object sender, EventArgs e)
        {
            if(imagePathList.Count > 1)
            {
                if (index == 0)
                {
                    index = imagePathList.Count;
                }
                suit();
                pictureBox1.Image = Image.FromFile(imagePathList.ElementAt(--index));
                textBox1.Text = Api.GeneralBasicDemo(imagePathList.ElementAt(index));
            }
        }

        //清空数据
        private void button3_Click(object sender, EventArgs e)
        {
            index = 0;
            pictureBox1.Image = null;
            textBox1.Text = "";
            imagePathList.Clear();
            Api.rt.words = "";
        }

        private void suit()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "文本文件(*.txt)|*.txt";
            dialog.FileName = "Text-recognition.txt";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(dialog.FileName, true);
                streamWriter.Write(this.textBox1.Text);
                streamWriter.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text.ToString());
        }
    }
}
