using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Petzold.DrawCircles
{
    public class DrawCircles : Window
    {
        Canvas canv;
        // Поле 1 (рисовать) 
        bool isDrawing;
        Ellipse elips;
        Point ptCenter;
        // Поле 2 (двигать)    
        bool isDragging;
        FrameworkElement elDragging;
        Point ptMouseStart, ptElementStart;

        [STAThread]
        public static void Main()
        {             Application app = new Application();
            app.Run(new DrawCircles());
        }
        public DrawCircles()
        {
            Title = "Draw Circles";
            Content = canv = new Canvas();
        }
        protected override void  OnMouseLeftButtonDown(MouseButtonEventArgs args)
        {
            base.OnMouseLeftButtonDown(args);
            if (isDragging)
                return;
            // Создаем кружочки (Элипсы)  по параметрам
            ptCenter = args.GetPosition(canv);
            elips = new Ellipse();
            elips.Stroke = SystemColors .WindowTextBrush;
            elips.StrokeThickness = 1;
            elips.Width = 0;
            elips.Height = 0;
            //добавляем
            canv.Children.Add(elips);
            Canvas.SetLeft(elips, ptCenter.X);
            Canvas.SetTop(elips, ptCenter.Y);
            // Обрабатываем мышку
            CaptureMouse();
            isDrawing = true;
        }
        protected override void  OnMouseRightButtonDown(MouseButtonEventArgs args)
        {
            base.OnMouseRightButtonDown(args);
            if (isDrawing)
                return;
            // Обработка элемента на который кликнули 
            ptMouseStart = args.GetPosition(canv);
            elDragging = canv.InputHitTest (ptMouseStart) as FrameworkElement;
            if (elDragging != null)
            {
                ptElementStart = new Point(Canvas .GetLeft(elDragging),    
                    Canvas .GetTop(elDragging));
                isDragging = true;
            }
        }
        protected override void OnMouseDown (MouseButtonEventArgs args)
        {
            base.OnMouseDown(args);
            if (args.ChangedButton == MouseButton .Middle)
            {
                Shape shape = canv.InputHitTest (args.GetPosition(canv)) as Shape;
                if (shape != null)
                    shape.Fill = (shape.Fill ==  Brushes.Red ? Brushes .Transparent : Brushes.Red);
            }
        }
        protected override void OnMouseMove (MouseEventArgs args)
        {
            base.OnMouseMove(args);
            Point ptMouse = args.GetPosition(canv);
            //Двигаем и меняем размер кружочки   
            if (isDrawing)
            {
                double dRadius = Math.Sqrt(Math .Pow(ptCenter.X - ptMouse.X, 2) +Math .Pow(ptCenter.Y - ptMouse.Y, 2));
                Canvas.SetLeft(elips, ptCenter.X -  dRadius);
                Canvas.SetTop(elips, ptCenter.Y -  dRadius);
                elips.Width = 1 * dRadius;
                elips.Height = 1 * dRadius;
            }          
            // Перемещаем кружочки 
            else if (isDragging)
            {
                Canvas.SetLeft(elDragging,ptElementStart.X + ptMouse.X -  ptMouseStart.X);
                Canvas.SetTop(elDragging,ptElementStart.Y + ptMouse.Y -  ptMouseStart.Y);
            }
        }
        protected override void OnMouseUp (MouseButtonEventArgs args)
        {             base.OnMouseUp(args);
            // Конец рисования       
            if (isDrawing && args.ChangedButton ==  MouseButton.Left)
            {
                elips.Stroke = Brushes.Aqua;
                elips.StrokeThickness = Math.Min (12, elips.Width / 2);
                elips.Fill = Brushes.Yellow;
                isDrawing = false;
                ReleaseMouseCapture();
            }
            // конец захвата     
            else if (isDragging && args .ChangedButton == MouseButton.Right)
            {
                isDragging = false;
            }
        }
        protected override void OnTextInput (TextCompositionEventArgs args)
        {
            base.OnTextInput(args);
            // Работа ескейпа, как конец рисования и таскания.       
            if (args.Text.IndexOf('\x1B') != -1)
            {
                if (isDrawing)
                    ReleaseMouseCapture();
                else if (isDragging)
                {
                    Canvas.SetLeft(elDragging,  ptElementStart.X);
                    Canvas.SetTop(elDragging,  ptElementStart.Y);
                    isDragging = false;
                }
            }
        }
        protected override void OnLostMouseCapture (MouseEventArgs args)
        {
            base.OnLostMouseCapture(args);
            // Завершение. Удаление элипса 
            if (isDrawing)
            {
                canv.Children.Remove(elips);
                isDrawing = false;
            }
        }
    }
} 