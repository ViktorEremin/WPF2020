using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ConsoleApp1
{
    class Program
    {
        const double G = 9.8;
        static void Main(string[] args)
        {
         int alphaGrad = Convert.ToInt32(Console.ReadLine());
         int v0 = Convert.ToInt32(Console.ReadLine());
            Coord(alphaGrad, v0);
            Console.ReadKey();
        }
        static void Coord(int alphaGrad, int v0)
        {
         
            double alphaRad = alphaGrad * Math.PI / 180, xCoord, yCoord;
            double t = (2 * v0 * Math.Sin(alphaRad) / G) + 1;

           for (double i = 0; i < t; i+=0.3)
            {
                xCoord = v0 * Math.Cos(alphaRad) * i;
                yCoord = v0 * Math.Sin(alphaRad) * i - G * i * i * 0.5;
                if (yCoord < 0) break;
                Console.WriteLine($"{xCoord} {yCoord}");
            }
        }

    }
}
