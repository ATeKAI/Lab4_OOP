using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MyFigures.Operators;
using MyFigures;
using Rectangle = MyFigures.Rectangle;

namespace Lab4_OOP
{
    public partial class Form1 : Form
    {
        private Stack<Operator> operators = new Stack<Operator>();
        private Stack<Operand> operands = new Stack<Operand>();

        public Form1()
        {
            InitializeComponent();
            Figure.bitmap = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            Figure.pictureBox = pictureBox1;
            Figure.pen = new Pen(Color.Black, 3);
        }

        private void cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                operators.Clear();
                operands.Clear();
                try
                {
                    string sourceExpression = cmd.Text.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                    cmd.Text = "";
                    for (int i = 0; i < sourceExpression.Length; i++)
                    {
                        if (IsNotOperation(sourceExpression[i]))
                        {
                            if (!Char.IsDigit(sourceExpression[i]))
                            {
                                operands.Push(new Operand(sourceExpression[i]));
                                while (i < sourceExpression.Length - 1 && IsNotOperation(sourceExpression[i + 1]))
                                {
                                    string tempStr = operands.Pop().value.ToString() + sourceExpression[i + 1].ToString();
                                    operands.Push(new Operand(tempStr));
                                    i++;
                                }
                            }
                            else if (Char.IsDigit(sourceExpression[i]))
                            {
                                operands.Push(new Operand(sourceExpression[i].ToString()));
                                while (i < sourceExpression.Length - 1 && Char.IsDigit(sourceExpression[i + 1])
                                    && IsNotOperation(sourceExpression[i + 1]))
                                {
                                    int tempNum = Convert.ToInt32(operands.Pop().value.ToString()) * 10 +
                                        (int)Char.GetNumericValue(sourceExpression[i + 1]);
                                    operands.Push(new Operand(tempNum.ToString()));
                                    i++;
                                }
                            }
                        }
                        else if (sourceExpression[i] == 'R')
                        {
                            if (operators.Count == 0)
                            {
                                operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                            }
                        }
                        else if (sourceExpression[i] == 'M')
                        {
                            if (operators.Count == 0)
                            {
                                operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                            }
                        }
                        else if (sourceExpression[i] == 'D')
                        {
                            if (operators.Count == 0)
                            {
                                operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                            }
                        }
                        else if (sourceExpression[i] == 'N')
                        {
                            if (operators.Count == 0)
                            {
                                operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                            }
                        }
                        else if (sourceExpression[i] == '[')
                        {
                            operators.Push(OperatorContainer.FindOperator(sourceExpression[i]));
                        }
                        else if (sourceExpression[i] == ']')
                        {
                            do
                            {
                                if (operators.Peek().symbolOperator == '[')
                                {
                                    operators.Pop();
                                    break;
                                }
                                if (operators.Count == 0)
                                {
                                    break;
                                }
                            }
                            while (operators.Peek().symbolOperator != '[');
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Аргументы введены некорректно.");
                    log.Items.Add("Аргументы введены некорректно.");
                }
                try
                {
                    SelectingPerformingOperation(operators.Peek());
                }
                catch
                {
                    MessageBox.Show($"Введенной операции /{cmd.Text} не существует.");
                    log.Items.Add($"Введенной операции /{cmd.Text} не существует.");
                }
            }
        }

        private bool IsNotOperation(char item)
        {
            if (!(item == 'M' || item == 'R' || item == 'D' || item == 'N' || item == ';' || item == '[' 
            || item == ']'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool IsInBounds(int x, int y, int w, int h)
        {
            if ((y < 0) || (y + h > Figure.pictureBox.Height) || (x < 0) || (x + w > Figure.pictureBox.Width))
            {
                throw new Exception("Нарушены границы или что-то не так");
            }
            else return true;
        }
        
        private void SelectingPerformingOperation(Operator op)
        {
            try
            {
                if (op.symbolOperator == 'R')
                {
                    AddRectangle(ref op);
                }
                else if (op.symbolOperator == 'M')
                {
                    MoveRectangles();
                }
                else if (op.symbolOperator == 'D')
                {
                    DeleteFigureByName();
                }
                else if (op.symbolOperator == 'N')
                {
                    RenameFigure();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void AddRectangle(ref Operator op)
        {
            if (operands.Count == 5)
            {
                int w = Convert.ToInt32(operands.Pop().value.ToString());
                int h = Convert.ToInt32(operands.Pop().value.ToString());
                int y = Convert.ToInt32(operands.Pop().value.ToString());
                int x = Convert.ToInt32(operands.Pop().value.ToString());
                string name = operands.Pop().value.ToString();
                if (IsInBounds(x, y, w, h))
                {
                    Rectangle figure = new Rectangle(name, x, y, w, h);
                    op = new Operator(figure.Draw_with_name, 'R');
                    log.Items.Add($"{figure.Name} отрисован");
                    op.operatorMethod();
                }
                else
                {
                    MessageBox.Show($"Фигура {name} вышла за границы.");
                    log.Items.Add($"Фигура {name} вышла за границы.");
                }
            }
            else
            {
                MessageBox.Show("Опертор R принимает 5 параматетров.");
                log.Items.Add("Неверное число параметров для оператора R.");
            }
        }

        private void MoveRectangles()
        {
            if (operands.Count == 3)
            {
                Rectangle figure = null;
                int dy = Convert.ToInt32(operands.Pop().value.ToString());
                int dx = Convert.ToInt32(operands.Pop().value.ToString());
                string name = operands.Pop().value.ToString();
                foreach (Figure f in FiguresContainer.figureList)
                {
                    if (f.Name == name)
                    {
                        figure = (Rectangle)f;
                    }
                }
                if (figure != null)
                {
                    if (IsInBounds(figure.X + dx, figure.Y + dy, figure.Width, figure.Height))
                    {
                        figure.MoveTo_d(dx, dy);
                        log.Items.Add($"Фигура {figure.Name} успешно перемещена");
                    }
                    else
                    {
                        MessageBox.Show($"Фигура {name} вышла за границы.");
                        log.Items.Add($"Фигура {name} вышла за границы.");
                    }
                }
                else
                {
                    MessageBox.Show($"Фигуры {name} не существует");
                    log.Items.Add($"Фигуры {name} не существует");
                }
            }
            else
            {
                MessageBox.Show("Опертор M принимает 3 параматетра.");
                log.Items.Add("Неверное число параметров для оператора M.");
            }
        }

        private void DeleteFigureByName()
        {
            if (operands.Count == 1)
            {
                Rectangle figure = null;
                string name = operands.Pop().value.ToString();
                foreach (Figure f in FiguresContainer.figureList)
                {
                    if (f.Name == name)
                    {
                        figure = (Rectangle)f;
                    }
                }
                if (figure != null)
                {
                    figure.DeleteF(figure, true);
                    log.Items.Add($"Фигура {figure.Name} успешно удалена");
                }
                else
                {
                    MessageBox.Show($"Фигуры {name} не существует");
                    log.Items.Add($"Фигуры {name} не существует");
                }
            }
            else
            {
                MessageBox.Show("Опертор D принимает 1 параматетр.");
                log.Items.Add("Неверное число параметров для оператора D.");
            }
        }

        private void RenameFigure()
        {
            if (operands.Count == 2)
            {
                Rectangle figure = null;
                string newName = operands.Pop().value.ToString();
                string oldName = operands.Pop().value.ToString();
                foreach (Figure f in FiguresContainer.figureList)
                {
                    if (f.Name == oldName)
                    {
                        figure = (Rectangle)f;
                    }
                }

                if (figure != null)
                {
                    figure.Name = newName;
                    figure.ClearMap();
                    figure.Draw_with_name();
                    log.Items.Add($"Фигура {oldName} успешно переименована в " +
                                  $"{figure.Name}");
                }
                else
                {
                    MessageBox.Show($"Фигуры {oldName} не существует");
                    log.Items.Add($"Фигуры {oldName} не существует");
                }
            }
        }
    }
}
