using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace Chapter13
{
    class ListColorShapes : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new ListColorShapes());
        }
        public ListColorShapes()
        {
            Title = "List Color Shapes";
            // Создаем скролл меню   
            ListBox lstbox = new ListBox();
            lstbox.Width = 400;
            lstbox.Height = 400;
            lstbox.SelectionChanged +=  ListBoxOnSelectionChanged;
            Content = lstbox;
            // Заполняем какими либо объектами, сделал прямоугольники       
            PropertyInfo[] props = typeof(Brushes).GetProperties();
            foreach (PropertyInfo prop in props)         
            {
                Rectangle rect = new Rectangle();
                rect.Width = 200;
                rect.Height = 50;
                rect.Margin = new Thickness(10, 5 , 0, 5);
                rect.Fill = prop.GetValue(null,  null) as Brush;
                lstbox.Items.Add(rect);
            }
        }
        void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListBox lstbox = sender as ListBox;
            if (lstbox.SelectedIndex != -1)
                Background = (lstbox.SelectedItem  as Shape).Fill;
        }
    }
} 