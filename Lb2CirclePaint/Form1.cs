using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lb2CirclePaint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Graphics g;

        private Bitmap bpm;

        //Создаём ручку по умолчанию
        private Pen pen = new Pen(color: Color.Aqua);
        private Brush brush = new SolidBrush(Color.Black);
        private int bitmapState = 0;

        private int state = 0;

        //Круг, эллипс
        private Point CirclePoint = new Point();
        private bool isMouseDown = false;

        private Point newPoint = new Point();
        private int oldradius;
        private Pen oldPen = new Pen(Color.White);
        private Brush oldbrush = new SolidBrush(Color.White);
        private Point pointDelete = new Point();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (bitmapState == 0)
            {
                //Создаём Image внутри pictureBox
                bpm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                ////Создаём объект graphics для рисования на bitmap
                g = Graphics.FromImage(bpm);

                g.Clear(Color.White);
                pictureBox1.Image = bpm;
                bitmapState++;
            }
            
        }

        /// <summary>
        /// Выбор цвета контура
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                label3.ForeColor = color;
                pen.Color = color;
            }
        }


        /// <summary>
        /// Выбор цвета заливки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog2.Color;
                brush = new SolidBrush(color);
            }
        }
        /// <summary>
        /// Первичная отрисовка окружности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (state == 0)
            {
                isMouseDown = true;
                int radius = Convert.ToInt32(textBox3.Text);
                oldradius = radius;
                Size size = new Size(radius, radius);

                CirclePoint = new Point(e.X, e.Y);

                pointDelete = new Point(e.X,e.Y);//для возможности удаления
                pointDelete.X -= radius / 2;//для возможности удаления
                pointDelete.Y -= radius / 2;//для возможности удаления

                CirclePoint.X -= radius / 2;
                CirclePoint.Y -= radius / 2;

                Rectangle rectangle = new Rectangle(CirclePoint, size);
                pen.Width = Convert.ToInt32(textBox2.Text);
                pen.Width = oldPen.Width;
                //брезенхем
                brush = new SolidBrush(pen.Color);
                Algoritm.BrCircle(g,brush,e.X ,e.Y,radius/2);
                //брезенхем
                //g.DrawEllipse(pen, rectangle);
                
                state = 1;
                pictureBox1.Refresh();
                
                //Circlepoint && deletepoint хранят одно значение
            }
            else if (state == 3)//перенос центра и отрисовка круга
            {
                //Удаление старого круга
                Size oldSize = new Size(oldradius, oldradius);
                Rectangle oldRectangle = new Rectangle(CirclePoint, oldSize);
                oldPen.Width = pen.Width+2;
                g.DrawEllipse(oldPen, oldRectangle);
                

                //Присваивание актуальных значений
                int radius = Convert.ToInt32(textBox3.Text);
                Size size = new Size(radius, radius);

                //Передача новых значений в глобальные переменные old
                oldradius = radius;
                oldSize = new Size(oldradius, oldradius);
                CirclePoint = new Point(e.X, e.Y);
                CirclePoint.X -= radius / 2;
                CirclePoint.Y -= radius / 2;
                //Отрисовка нового круга
                Rectangle rectangle = new Rectangle(CirclePoint, size);
                pen.Width = Convert.ToInt32(textBox2.Text);
                //брезенхем
                brush = new SolidBrush(pen.Color);
                Algoritm.BrCircle(g, brush, e.X, e.Y, radius / 2);
                //брезенхем
                // g.DrawEllipse(pen, rectangle);
                
                pictureBox1.Refresh();
                pointDelete = CirclePoint;
                state = 1;
            }
        }


        /// <summary>
        /// Изменение толщины, цвета и размера окружности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            
            //Удаление старого круга
            Size oldSize = new Size(oldradius, oldradius);
            Rectangle oldRectangle = new Rectangle(pointDelete, oldSize);
            oldPen.Width = pen.Width+2;
            g.DrawEllipse(oldPen, oldRectangle);
            
            //Присваивание актуальных значений
            int radius = Convert.ToInt32(textBox3.Text);
            Size size = new Size(radius, radius);

            //Передача новых значений в глобальные переменные old
            oldradius = radius;
            oldSize = new Size(oldradius, oldradius);

            //Отрисовка нового круга
            Rectangle rectangle = new Rectangle(CirclePoint, size);
            pen.Width = Convert.ToInt32(textBox2.Text);
            //брезенхем
            brush = new SolidBrush(pen.Color);
            Algoritm.BrCircle(g, brush, CirclePoint.X, CirclePoint.Y, radius / 2);
            //брезенхем
            //g.DrawEllipse(pen, rectangle);
            pictureBox1.Refresh();
            state = 1;
            
        }
        
        /// <summary>
        /// Удаление окружности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //Удаление старого круга
            Size oldSize = new Size(oldradius, oldradius);
            Rectangle oldRectangle = new Rectangle(pointDelete, oldSize);
            oldPen.Width = pen.Width+2;
            g.DrawEllipse(oldPen, oldRectangle);
            g.FillEllipse(oldbrush, oldRectangle);
           
            pictureBox1.Refresh();
            state = 0;
        }

        /// <summary>
        /// Сохранение рисунка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //Переносим рисунок в созданный Image
            pictureBox1.DrawToBitmap(bpm, pictureBox1.ClientRectangle);

            g.Save();
            bpm.Save("image.png", ImageFormat.Png);

            //pictureBox1.Image = Image.FromFile("image.png");
            pictureBox1.Refresh();
            state = 0;
        }
        /// <summary>
        /// Перемещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            state = 3;
        }
    }
}
