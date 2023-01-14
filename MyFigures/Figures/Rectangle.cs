using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFigures
{
    public class Rectangle : Figure
    {
        public static int count = 0;
        public Rectangle() { }
        public Rectangle(int x, int y, int width, int height) : base(x, y, width, height)
        {
            if (!(x < 0 || y < 0 || x + width > pictureBox.Width || y + height > pictureBox.Height))
            {
                FiguresContainer.RectsList.Add(this);
                FiguresContainer.figureList.Add(this);
                number = count;
                count++;
            }
            else
            {
                throw new Exception("Фигура должна помещаться на холст");
            }
        }
        public Rectangle(string name, int x, int y, int width, int height) : base(name, x, y, width, height)
        {
            if (!(x < 0 || y < 0 || x + width > pictureBox.Width || y + height > pictureBox.Height))
            {
                FiguresContainer.RectsList.Add(this);
                FiguresContainer.figureList.Add(this);
                number = count;
                count++;
            }
            else
            {
                throw new Exception("Фигура должна помещаться на холст");
            }
        }
        public override void Draw()
        {
            try
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawRectangle(pen, X, Y, Width, Height);
                pictureBox.Image = bitmap;
                DrawText(Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ашипка");
            }
        }
        public void Draw_with_name()
        {
            try
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawRectangle(pen, X, Y, Width, Height);
                pictureBox.Image = bitmap;
                DrawText(Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ашипка");
            }
        }
        public void MoveTo_d(int dx, int dy)
        {
            try
            {
                if (!(X < 0 || Y < 0 || X + Width > pictureBox.Width || Y + Height > pictureBox.Height))
                {
                    X += dx; Y += dy;
                    DeleteF(this, false);
                    Draw_with_name();
                }
                else
                {
                    throw new Exception("Фигура должна помещаться на холст");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ашипка");
            }
        }
        public void ResizeRect(int width, int height)
        {
            try
            {
                if (!(X < 0 || Y < 0 || X + width > pictureBox.Width || Y + height > pictureBox.Height))
                {
                    this.Width = width; this.Height = height;
                    DeleteF(this, false);
                    Draw();
                }
                else
                {
                    throw new Exception("Фигура должна помещаться на холст");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ашипка");
            }
        }
    }
}

