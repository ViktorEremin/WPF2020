using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Petzold.GetMedieval
{
    public class MedievalButton : Control
    {
        // 2 приват поля   
        FormattedText formtxt;
        bool isMouseReallyOver;
        // Статические поля для чтения.    
        public static readonly DependencyProperty  TextProperty;
        public static readonly RoutedEvent KnockEvent;
        public static readonly RoutedEvent  PreviewKnockEvent;
        // Статический конструктор.      
        static MedievalButton()  
        {
            // регистрация зависимого свойства.    
            TextProperty = DependencyProperty.Register("Text",  typeof(string), typeof (MedievalButton), 
                new FrameworkPropertyMetadata(" ",FrameworkPropertyMetadataOptions.AffectsMeasure));
            // регистрация событий.    
            KnockEvent = EventManager.RegisterRoutedEvent ("Knock", RoutingStrategy.Bubble,     
                typeof(RoutedEventHandler),  typeof(MedievalButton));

            PreviewKnockEvent =  EventManager.RegisterRoutedEvent ("PreviewKnock",  RoutingStrategy.Tunnel,          
                typeof(RoutedEventHandler),  typeof(MedievalButton));
        }
        // открытый интерфейс 1.   
        public string Text
        {
            set
            {
                SetValue(TextProperty, value == null  ? " " : value);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }
        // Открытый интерфейс 2.   
        public event RoutedEventHandler Knock
        {
            add
            {
                AddHandler(KnockEvent, value);
            }
            remove
            {
                RemoveHandler(KnockEvent, value);
            }
        }
        public event RoutedEventHandler PreviewKnock
        {
            add
            {
                AddHandler(PreviewKnockEvent, value);
            }
            remove
            {
                RemoveHandler(PreviewKnockEvent,  value);
            }
        }
        // Вызывается если размер кнопки меняется.
        protected override Size MeasureOverride(Size  sizeAvailable)
        {
            formtxt = new FormattedText(Text, CultureInfo.CurrentCulture,  FlowDirection, new Typeface(FontFamily, FontStyle , FontWeight, FontStretch),  FontSize, Foreground);
            // учитываем внутр. отступы для размеров.     
            Size sizeDesired = new Size(Math.Max(48,  formtxt.Width) + 4,
                formtxt.Height + 4);
            sizeDesired.Width += Padding.Left +  Padding.Right;
            sizeDesired.Height += Padding.Top +  Padding.Bottom;

            return sizeDesired;
        }
        // вызывается для перерисовкки кнопки
        protected override void OnRender (DrawingContext dc)
        {
            // Цвет фона.      
            Brush brushBackground = SystemColors .ControlBrush;
            if (isMouseReallyOver && IsMouseCaptured)
                brushBackground = SystemColors .ControlDarkBrush;
            // Ширина пера.  
            Pen pen = new Pen(Foreground,  IsMouseOver ? 2 : 1);
            //Закругляем углы квадрата.     
            dc.DrawRoundedRectangle (brushBackground, pen,  new Rect(new  Point(0, 0), RenderSize), 4, 4);
            // Основной цвет.  
            formtxt.SetForegroundBrush(IsEnabled ? Foreground :  SystemColors.ControlDarkBrush);
            //Ставим начальную точку текста    
            Point ptText = new Point(2, 2);
            switch (HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:ptText.X += Padding.Left;

                    break;
                case HorizontalAlignment.Right: ptText.X += RenderSize.Width -  formtxt.Width - Padding.Right;

                    break;
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch: ptText.X += (RenderSize.Width  - formtxt.Width - Padding.Left - Padding .Right) / 2;

                    break;
            }
            switch (VerticalContentAlignment)
            {
                case VerticalAlignment.Top: ptText.Y += Padding.Top;

                    break;
                case VerticalAlignment.Bottom: ptText.Y +=   RenderSize.Height -  formtxt.Height - Padding.Bottom;

                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch: ptText.Y += (RenderSize.Height  - formtxt.Height -  Padding.Top - Padding .Bottom) / 2;

                    break;
            }
            // Пишем текст  
            dc.DrawText(formtxt, ptText);
        }
        // События мыши влияющие на кнопку
        protected override void OnMouseEnter (MouseEventArgs args)
        {
            base.OnMouseEnter(args);
            InvalidateVisual();
        }
        protected override void OnMouseLeave (MouseEventArgs args)
        {
            base.OnMouseLeave(args);
            InvalidateVisual();
        }
        protected override void OnMouseMove (MouseEventArgs args)
        {
            base.OnMouseMove(args);
            // Проверка направления 
            Point pt = args.GetPosition(this);
            bool isReallyOverNow = (pt.X >= 0 &&  pt.X < ActualWidth &&  pt.Y >= 0 &&  pt.Y < ActualHeight);
            if (isReallyOverNow != isMouseReallyOver)
            {
                isMouseReallyOver = isReallyOverNow;
                InvalidateVisual();
            }
        } 
        protected override void  OnMouseLeftButtonDown(MouseButtonEventArgs args)
        {
            base.OnMouseLeftButtonDown(args);
            CaptureMouse();
            InvalidateVisual();
            args.Handled = true;
        }
        // Триггер кнока  
        protected override void  OnMouseLeftButtonUp(MouseButtonEventArgs args)
        {
            base.OnMouseLeftButtonUp(args);
            if (IsMouseCaptured)
            {
                if (isMouseReallyOver)
                {
                    OnPreviewKnock();
                    OnKnock();
                }
                args.Handled = true;
                Mouse.Capture(null);
            }
        }
        // При потери захвата мыши, кнопка перерисовывается.   
        protected override void OnLostMouseCapture (MouseEventArgs args)
        {
            base.OnLostMouseCapture(args);
            InvalidateVisual();
        }
        // пробел и энтер вызывают нажатие   
        protected override void OnKeyDown (KeyEventArgs args)
        {
            base.OnKeyDown(args);
            if (args.Key == Key.Space || args.Key  == Key.Enter)
                args.Handled = true;
        }
        protected override void OnKeyUp (KeyEventArgs args)
        {
            base.OnKeyUp(args);
            if (args.Key == Key.Space || args.Key  == Key.Enter)
            {
                OnPreviewKnock();
                OnKnock();
                args.Handled = true;
            }
        }
        // делает клик.     
        protected virtual void OnKnock()
        {
            RoutedEventArgs argsEvent = new  RoutedEventArgs();
            argsEvent.RoutedEvent = MedievalButton .PreviewKnockEvent;
            argsEvent.Source = this;
            RaiseEvent(argsEvent);
        }
        // инициирует 'PreviewKnock' .    
        protected virtual void OnPreviewKnock()
        {
            RoutedEventArgs argsEvent = new  RoutedEventArgs();
            argsEvent.RoutedEvent = MedievalButton .KnockEvent;
            argsEvent.Source = this;
            RaiseEvent(argsEvent);
        }
    }
    public class GetMedieval : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new GetMedieval());
        }
        public GetMedieval()
        {
            Title = "Get Medieval";
            MedievalButton btn = new MedievalButton();
            btn.Text = "I am only button. Dont klick";
            btn.FontSize = 24;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Padding = new Thickness(5, 20, 5, 20);
            btn.Knock += ButtonOnKnock;
            Content = btn;
        }
        void ButtonOnKnock(object sender, RoutedEventArgs args)
        {
            MedievalButton btn = args.Source as MedievalButton;
            MessageBox.Show("The button labeled  \"" + btn.Text + "\" has been knocked. Dont click again!", Title);
        }
    }
}  