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
    public delegate void returnImageString(Bitmap bt);
    public partial class ScreenForm : Form
    {
        public event returnImageString returnFormImageString;
        public ScreenForm()
        {
            InitializeComponent();
        }
        #region 定义程序变量
        // 用来记录鼠标按下的坐标，用来确定绘图起点
        private Point DownPoint;
        // 用来表示是否截图完成
        private bool CatchFinished = false;
        // 用来表示截图开始
        private bool CatchStart = false;
        // 用来保存原始图像
        private Bitmap originBmp;
        // 用来保存截图的矩形
        private Rectangle CatchRectangle;
        #endregion

        private void ScreenForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            // 改变鼠标样式
            this.Cursor = Cursors.Cross;
            // 保存全屏图片
            originBmp = new Bitmap(this.BackgroundImage);
        }

        /*鼠标按下时开始截图*/
        private void ScreenForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CatchStart)
            {
                CatchStart = true;
                DownPoint = new Point(e.X, e.Y);
            }
        }
        /*鼠标移动时显示截取区域的边框*/
        private void ScreenForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (CatchStart)
            {
                // 新建一个图片对象，让它与屏幕图片相同
                Bitmap copyBmp = (Bitmap)originBmp.Clone();

                // 获取鼠标按下的坐标
                Point newPoint = new Point(DownPoint.X, DownPoint.Y);

                // 新建画板和画笔
                Graphics g = Graphics.FromImage(copyBmp);
                Pen p = new Pen(Color.Red, 1);

                // 获取矩形的长宽
                int width = Math.Abs(e.X - DownPoint.X);
                int height = Math.Abs(e.Y - DownPoint.Y);
                if (e.X < DownPoint.X)
                {
                    newPoint.X = e.X;
                }
                if (e.Y < DownPoint.Y)
                {
                    newPoint.Y = e.Y;
                }

                CatchRectangle = new Rectangle(newPoint, new Size(width, height));

                // 将矩形画在画板上
                g.DrawRectangle(p, CatchRectangle);

                // 释放目前的画板
                g.Dispose();
                p.Dispose();
                // 从当前窗体创建新的画板
                Graphics g1 = this.CreateGraphics();

                // 将刚才所画的图片画到截图窗体上
                // 为什么不直接在当前窗体画图呢？
                // 如果自己解决将矩形画在窗体上，会造成图片抖动并且有无数个矩形
                // 这样实现也属于二次缓冲技术
                g1.DrawImage(copyBmp, new Point(0, 0));
                g1.Dispose();
                // 释放拷贝图片，防止内存被大量消耗
                copyBmp.Dispose();
            }
        }

        /*鼠标放开时截图操作完成*/
        private void ScreenForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (CatchStart)
            {
                CatchStart = false;
                CatchFinished = true;
            }
        }

        private void ScreenForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (CatchFinished)
            {
                // 新建一个与矩形一样大小的空白图片
                Bitmap CatchedBmp = new Bitmap(CatchRectangle.Width, CatchRectangle.Height);

                Graphics g = Graphics.FromImage(CatchedBmp);

                // 把originBmp中指定部分按照指定大小画到空白图片上
                // CatchRectangle指定originBmp中指定部分
                // 第二个参数指定绘制到空白图片的位置和大小
                // 画完后CatchedBmp不再是空白图片了，而是具有与截取的图片一样的内容
                g.DrawImage(originBmp, new Rectangle(0, 0, CatchRectangle.Width, CatchRectangle.Height), CatchRectangle, GraphicsUnit.Pixel);
                // 将图片保存到剪切板中
                returnFormImageString(CatchedBmp);
                g.Dispose();
                CatchFinished = false;
                this.BackgroundImage = originBmp;
                CatchedBmp.Dispose();
                this.Close();
            }
        }

        private void ScreenForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
