using System;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;

namespace Eremin_CH1
{
    class ThrowWindowParty : Application
    {[STAThread]
        static void Main(string[] args)
        {
            ThrowWindowParty app = new ThrowWindowParty();
            app.Run();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Window winMain = new Window();
            winMain.Title = "Main WINDOW";
            winMain.MouseDown += WindowOnMouseDown;
            winMain.Show();

            for(int i = 0; i < 2; i++)
            {
                Window win = new Window();
                win.Title = "Window #" + (i + 1);
                win.Show();
            }
        }
        void WindowOnMouseDown(object sender, MouseButtonEventArgs args)
        {
            Window win = new Window();
            win.Title = "Modal DIALOG box";
            win.ShowDialog();
        }
    }
}
