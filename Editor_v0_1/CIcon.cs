using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Editor_v0_1
{
    public class CIcon
    {
        public string name = "";

        int iconWidth = 100;
        int iconHeight = 100;

        Point position;

        Rectangle icon = null;

        public string Name
        {
            get { return name; }
        }

        public double X
        {
            get { return position.X; }
        }

        public double Y
        {
            get { return position.Y; }
        }

        public int IconWidth
        {
            get { return iconWidth; }
        }

        public int IconHeight
        {
            get { return iconHeight; }
        }

        public CIcon(int iconWidth, int iconHeight, string imagePath)
        {
            this.iconWidth = iconWidth;
            this.iconHeight = iconHeight;
            position = new Point(0, 0);

            name = System.IO.Path.GetFileNameWithoutExtension(imagePath);

            icon = new Rectangle();
            //установка цвета линии обводки и цвета заливки при помощи коллекции кистей
            icon.Stroke = Brushes.Black;
            ImageBrush ib = new ImageBrush();
            //позиция изображения будет указана как координаты левого верхнего угла
            //изображение будет растянуто по размерам прямоугольника, описанного вокруг фигуры
            ib.AlignmentX = AlignmentX.Left;
            ib.AlignmentY = AlignmentY.Top;

            //загрузка изображения и назначение кисти
            ib.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

            icon.RenderTransform = new TranslateTransform(position.X, position.Y);

            icon.Fill = ib;
            //параметры выравнивания
            icon.HorizontalAlignment = HorizontalAlignment.Left;
            icon.VerticalAlignment = VerticalAlignment.Center;
            //размеры прямоугольника
            icon.Height = iconHeight;
            icon.Width = iconWidth;
        }

        public Rectangle getIcon()
        {
            return icon; 
        }

        public void setPosition(Point newPosition)
        {
            //можно добавить проверку полученого значения

            position = newPosition;
            icon.RenderTransform = new TranslateTransform(position.X, position.Y);
        }

        public bool isMouseOver(Point mousePosition)
        {
            double lb = position.X;
            double rb = position.X + iconWidth;
            double ub = position.Y;
            double db = position.Y + iconHeight;

            if (mousePosition.X > lb && mousePosition.X < rb && mousePosition.Y > ub && mousePosition.Y < db)
                return true;

            return false;
        }

        public Rectangle CloneIcon()
        {
            Rectangle iconCopy = new Rectangle();

            //установка цвета линии обводки и цвета заливки при помощи коллекции кистей
            iconCopy.Stroke = Brushes.Black;

            iconCopy.Fill = icon.Fill.Clone();
            
            //параметры выравнивания
            iconCopy.HorizontalAlignment = HorizontalAlignment.Left;
            iconCopy.VerticalAlignment = VerticalAlignment.Center;
            
            //размеры прямоугольника
            iconCopy.Height = iconHeight;
            iconCopy.Width = iconWidth;

            iconCopy.RenderTransform = new TranslateTransform(0, 0);

            return iconCopy;
        }
    }
}