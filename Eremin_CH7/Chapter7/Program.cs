﻿using System; using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintingButton

{
    public class PaintTheButton : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new PaintTheButton());
        }
        public PaintTheButton()
        {
            Title = "Paint the Button";
            // создали button
            Button btn = new Button();
            btn.HorizontalAlignment =  HorizontalAlignment.Center;
            btn.VerticalAlignment =  VerticalAlignment.Center;
            Content = btn;         
            // создали canvas в кнопке    
            Canvas canv = new Canvas();
            canv.Width = 144;
            canv.Height = 144;
            btn.Content = canv;
            // создали rerctangle в canvas      
            Rectangle rect = new Rectangle();
            // задаем ректанглу размеры 
            rect.Width = canv.Width;
            rect.Height = canv.Height;
            rect.RadiusX = 24;
            rect.RadiusY = 24;
            rect.Fill = Brushes.Blue;
            canv.Children.Add(rect);
            //Позиционируем канвас в левом верхнем углу
            Canvas.SetLeft(rect, 0);
            Canvas.SetRight(rect, 0);
            // Полигон как ребенок canvas
            
            Polygon poly = new Polygon();
            poly.Fill = Brushes.Yellow;
            poly.Points = new PointCollection();
            for (int i = 0; i < 5; i++)
            {
                double angle = i * 4 * Math.PI / 5;
                Point pt = new Point(48 * Math.Sin (angle),-48 * Math.Cos (angle));
                poly.Points.Add(pt);
            }
            canv.Children.Add(poly);
            //Двигаем вершины на половину высоты и ширины
            Canvas.SetLeft(poly, canv.Width / 2);  
            Canvas.SetTop(poly, canv.Height / 2);
        }
    }
}