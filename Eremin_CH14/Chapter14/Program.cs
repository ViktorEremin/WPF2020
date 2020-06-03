using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chapter14
{
    public class CommandTheMenu : Window
    {
        TextBlock text;
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new CommandTheMenu());
        }
        public CommandTheMenu()
        {
            Title = "Command the Menu";
            // создаем DockPanel.       
            DockPanel dock = new DockPanel();
            Content = dock;
            // создаем меню сверху.      
            Menu menu = new Menu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);
            // Создаем объект с текстом на остальную часть экрана.    
            text = new TextBlock();
            text.Text = "Sample clipboard text";
            text.HorizontalAlignment =  HorizontalAlignment.Center;
            text.VerticalAlignment =  VerticalAlignment.Center;
            text.FontSize = 32;
            text.TextWrapping = TextWrapping.Wrap;
            dock.Children.Add(text);
            // создаем меню редактрования.       
            MenuItem itemEdit = new MenuItem();
            itemEdit.Header = "_Edit";
            menu.Items.Add(itemEdit);
            // создаем элементы этого меню.     
            MenuItem itemCut = new MenuItem();
            itemCut.Header = "Cu_t";
            itemCut.Command = ApplicationCommands.Cut;
            itemEdit.Items.Add(itemCut);
            MenuItem itemCopy = new MenuItem();
            itemCopy.Header = "_Copy";
            itemCopy.Command = ApplicationCommands .Copy;
            itemEdit.Items.Add(itemCopy);
            MenuItem itemPaste = new MenuItem();
            itemPaste.Header = "_Paste";
            itemPaste.Command =  ApplicationCommands.Paste;
            itemEdit.Items.Add(itemPaste);
            MenuItem itemDelete = new MenuItem();
            itemDelete.Header = "_Delete";
            itemDelete.Command =  ApplicationCommands.Delete;
            itemEdit.Items.Add(itemDelete);
            // Привязываем команты к коллекции окна.    
            CommandBindings.Add(new CommandBinding (ApplicationCommands.Cut,      
                CutOnExecute, CutCanExecute));
            CommandBindings.Add(new CommandBinding (ApplicationCommands.Copy,
                CopyOnExecute, CutCanExecute));
            CommandBindings.Add(new CommandBinding (ApplicationCommands.Paste,  
                PasteOnExecute, PasteCanExecute));
            CommandBindings.Add(new CommandBinding (ApplicationCommands.Delete,   
                DeleteOnExecute, CutCanExecute));
        }
        void CutCanExecute(object sender,  CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = text.Text != null &&  text.Text.Length > 0;
        }
        void PasteCanExecute(object sender,  CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = Clipboard .ContainsText();
        }
        void CutOnExecute(object sender,  ExecutedRoutedEventArgs args)
        {
            ApplicationCommands.Copy.Execute(null,  this);
            ApplicationCommands.Delete.Execute (null, this);
        }
        void CopyOnExecute(object sender,  ExecutedRoutedEventArgs args)
        {
            Clipboard.SetText(text.Text);
        }
        void PasteOnExecute(object sender,  ExecutedRoutedEventArgs args)
        {
            text.Text = Clipboard.GetText();
        }
        void DeleteOnExecute(object sender,  ExecutedRoutedEventArgs args)
        {
            text.Text = null;
        }
    } 
} 