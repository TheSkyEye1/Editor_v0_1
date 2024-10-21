using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Numerics;
using System.Xml.Linq;
using System.Windows.Controls;

namespace Editor_v0_1
{
    public class CIconList
    {
        List<CIcon> icons = new List<CIcon>();

        int border = 5;  //расстояние от иконки до края экрана или другой иконки

        int x;  //позиция иконки на экране
        int y;

        int x_sh;  //сдвиг иконок по верктикали и горизонтали
        int y_sh;

        int imageWidth = 100;   //ширина и высота иконки
        int imageHeight = 100;

        int canvasW = 220;  //ширина и высота канваса
        int canvasH = 400;

        public CIconList(int icon_width, int icon_height, int canvas_width, int canvas_height)
        {
            this.imageWidth = icon_width;
            this.imageHeight = icon_height;

            canvasW = canvas_width;
            canvasH = canvas_height;

            x = border;
            y = border;

            x_sh = icon_width + border;
            y_sh = icon_height + border;
        }

        public void Load(string path)
        {
            if (y > border || x > border)
            {
                x = border;
                y += y_sh;
            }

            string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + path;//@"\Resources\Monsters\";
            string filter = "*.png";
            string[] files = Directory.GetFiles(folder, filter);

            foreach (string file in files)
            {
                CIcon icon = new CIcon(imageWidth, imageHeight, file);

                icon.setPosition(new Point(x, y));

                icons.Add(icon);

                x += x_sh;
                if (x >= canvasW-x_sh)
                {
                    x = border;
                    y += y_sh;
                }
            }
        }

        public int getDeltaY()  //насколько сумма высот картинок больше высоты канваса
        {
            return (y - canvasH) + y_sh;
        }

        public void scroll(double delta)
        {
            foreach (CIcon icon in icons)
            {
                Point newPosition = new Point(icon.X, icon.Y + delta);
                icon.setPosition(newPosition);
            }
        }

        public List<CIcon> getIcons()
        {
            return icons; 
        }

        public CIcon findByName(string name)
        {
            foreach(CIcon icon in icons)
                if (icon.name == name)
                    return icon;

            return null;
        }

        public CIcon isMouuseOver(Point mousePosition)
        {
            foreach (CIcon icon in icons)
                if (icon.isMouseOver(mousePosition))
                    return icon;

            return null;
        }
    }
}
