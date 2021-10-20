using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb2CirclePaint
{
    static class Algoritm
    {
        //Рисование круга по методу брезенхема
        public static void BrCircle(Graphics g, Brush clr, int _x, int _y, int radius)
        {
            int x = 0, y = radius, gap = 0, delta = (2 - 2 * radius);
            while (y >= 0)
            {
                PutPixel(g, clr, _x + x, _y + y);
                PutPixel(g, clr, _x + x, _y - y);
                PutPixel(g, clr, _x - x, _y - y);
                PutPixel(g, clr, _x - x, _y + y);
                gap = 2 * (delta + y) - 1;
                if (delta < 0 && gap <= 0)
                {
                    x++;
                    delta += 2 * x + 1;
                    continue;
                }
                if (delta > 0 && gap > 0)
                {
                    y--;
                    delta -= 2 * y + 1;
                    continue;
                }
                x++;
                delta += 2 * (x - y);
                y--;
            }
        }
        //Метод, устанавливающий пиксел на форме с заданными цветом и прозрачностью
        private static void PutPixel(Graphics g, Brush brush, int x, int y)
        {
            g.FillRectangle(brush, x, y, 1, 1);
        }
    }
}
