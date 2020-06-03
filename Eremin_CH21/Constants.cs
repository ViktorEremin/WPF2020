
using System;
using System.Windows;
using System.Windows.Media;

namespace Petzold.AccessStaticFields
{
    public static class Constants
    {
        // Открытые статические члены
        public static readonly FontFamily fntfam =
            new FontFamily("colibri");

        public static double FontSize
        {
            get { return 80 / 0.75; }
        }
        //фон
        public static readonly LinearGradientBrush brush =
            new LinearGradientBrush(Colors.Red, Colors.Yellow,
                                    new Point(0, 0), new Point(1, 1));
    }
}