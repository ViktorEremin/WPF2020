
using System;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace Petzold.DumpControlTemplate
{
    public partial class DumpControlTemplate : Window
    {
        Control ctrl;

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new DumpControlTemplate());
        }
        public DumpControlTemplate()
        {
            InitializeComponent();
        }

        // Обрабочик Управления
        void ControlItemOnClick(object sender, RoutedEventArgs args)
        {
            // Удаляем потомка
            for (int i = 0; i < grid.Children.Count; i++)
                if (Grid.GetRow(grid.Children[i]) == 0)
                {
                    grid.Children.Remove(grid.Children[i]);
                    break;
                }

            // Очистка текста
            txtbox.Text = "";

            // Получаем класс Control 
            MenuItem item = args.Source as MenuItem;
            Type typ = (Type)item.Tag;

    
            ConstructorInfo info = typ.GetConstructor(System.Type.EmptyTypes);

            // пробуем Создать объект 
            try
            {
                ctrl = (Control)info.Invoke(null);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Title);
                return;
            }

            // пробуем создаеть объект.
            try
            {
                grid.Children.Add(ctrl);
            }
            catch
            {
                if (ctrl is Window)
                    (ctrl as Window).Show();
                else
                    return;
            }
            Title = Title.Remove(Title.IndexOf('-')) + "- " + typ.Name;
        }
        // Вкл элемеенты при открытии шаблона
        void DumpOnOpened(object sender, RoutedEventArgs args)
        {
            itemTemplate.IsEnabled = ctrl != null;
            itemItemsPanel.IsEnabled = ctrl != null && ctrl is ItemsControl;
        }
        void DumpTemplateOnClick(object sender, RoutedEventArgs args)
        {
            if (ctrl != null)
                Dump(ctrl.Template);
        }
        // Дамп объекта 
        void DumpItemsPanelOnClick(object sender, RoutedEventArgs args)
        {
            if (ctrl != null && ctrl is ItemsControl)
                Dump((ctrl as ItemsControl).ItemsPanel);
        }
        // Дамп шаблона.
        void Dump(FrameworkTemplate template)
        {
            if (template != null)
            {
                // Дамп XAML в текст
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = new string(' ', 4);
                settings.NewLineOnAttributes = true;

                StringBuilder strbuild = new StringBuilder();
                XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);

                try
                {
                    XamlWriter.Save(template, xmlwrite);
                    txtbox.Text = strbuild.ToString();
                }
                catch (Exception exc)
                {
                    txtbox.Text = exc.Message;
                }
            }
            else
                txtbox.Text = "no template";
        }
    }
}
