using System.Windows.Forms;
using System.Drawing;
using System;


namespace MyFigures
{
     public abstract class Figure
     {
         
        public int X { get; protected set; }
        public int Y{ get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public string Name { get; set; }
        
        protected int number;
        public static Bitmap bitmap;
        public static PictureBox pictureBox;
        public static Pen pen;
        
        public Figure() { }
        public Figure(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
        public Figure(string name, int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Name = name;
        }
        abstract public void Draw();
        
        public virtual void MoveTo(int x, int y)
        {
            try
            {
                if (!(x < 0 || y < 0 || x + Width > pictureBox.Width || y + Height > pictureBox.Height))
                {
                    this.X = x; this.Y = y;
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
        public void ClearMap()
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.WhiteSmoke);
            pictureBox.Image = bitmap;
        }
        public void DeleteF(Figure figure, bool flag)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, X, Y, Width, Height);
            FiguresContainer.figureList.Remove(figure);
            ClearMap();
            foreach (Figure f in FiguresContainer.figureList)
            {
                f.Draw();
            }
            if (flag != true)
            {
                FiguresContainer.figureList.Add(figure);
            }
            pictureBox.Image = bitmap;
        }
        public void DrawText(string type,int number)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString($"{type} {number}", new Font("Times New Roman", 10), Brushes.Gray, new RectangleF(X,Y,Width,Height), sf);
        }
        public void DrawText(string type, int number, int x, int y, int width, int height)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString($"{type} {number}", new Font("Times New Roman", 10), Brushes.Gray, new RectangleF(x, y, width, height), sf);
        }
        public void DrawText(string name)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString($"{name}", new Font("Times New Roman", 10), Brushes.Gray, new RectangleF(X, Y, Width, Height), sf);
        }
    }

}
