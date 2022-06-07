using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UXPolygons
{

    public class PixelCoord
    {
        public float fx = 0;
        public float fy = 0;
        public int x = 0;
        public int y = 0;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly float step = 1;
        private readonly Brush baseColor = Brushes.Blue;

        private void drawPixel(float x, float y, Brush color)
        {
            int _x = (int)Math.Truncate(x);
            int _y = (int)Math.Truncate(y);


            double intpart;

            float fX = modf(x, out intpart);
            float fY = modf(y, out intpart);

            if (fX > 0.5)
            {
                fX = 1.0f - fX;
            }
            if (fY > 0.5)
            {
                fY = 1.0f - fY;
            }

            byte alphaChannel = (byte)((fX * 127 + fY * 127) * 2);


            Rectangle rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness(_x * step, _y * step, _x * step, _y * step),
                //Fill = alphaBlend(alphaChannel, color, null)
                Fill = color
            };

            rect.Tag = new PixelCoord()
            {
                fx = x,
                fy = y,
                x = _x,
                y = _y
            };
            rect.MouseEnter += Rect_MouseEnter;

            canvas.Children.Add(rect);


            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x - 1) * step, _y * step, (_x - 1) * step, _y * step),
                Fill = alphaBlend(alphaChannel, color, null)                
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x + 1) * step, _y * step, (_x + 1) * step, _y * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x) * step, (_y - 1) * step, (_x) * step, (_y - 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x) * step, (_y + 1) * step, (_x) * step, (_y + 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x-1) * step, (_y + 1) * step, (_x -1 ) * step, (_y + 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);


            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x + 1) * step, (_y + 1) * step, (_x  - 1) * step, (_y + 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x + 1) * step, (_y + 1) * step, (_x  + 1) * step, (_y + 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);


            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x + 1) * step, (_y - 1) * step, (_x - 1) * step, (_y - 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x + 1) * step, (_y + 1) * step, (_x + 1) * step, (_y - 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x - 1) * step, (_y - 1) * step, (_x - 1) * step, (_y - 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };
            canvas.Children.Add(rect);

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x - 1) * step, (_y + 1) * step, (_x + 1) * step, (_y - 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };

            rect = new Rectangle()
            {
                Width = step,
                Height = step,
                Margin = new Thickness((_x - 1) * step, (_y + 1) * step, (_x + 1) * step, (_y + 1) * step),
                Fill = alphaBlend(alphaChannel, color, null)
            };


            /*
                        rect = new Rectangle()
                        {
                            Width = step,
                            Height = step,
                            Margin = new Thickness(Math.Truncate(x) * step, Math.Truncate(y) * step, Math.Truncate(x) * step, Math.Truncate(y) * step),
                            Fill = Brushes.Green
                        };
                        canvas.Children.Add(rect);
            */

            rect = new Rectangle()
            {
                Width = 0.5f,
                Height = 0.5f,
                Margin = new Thickness(x * step, y * step, x * step, y * step),
                Fill = Brushes.Red
            };
            canvas.Children.Add(rect);

        }

        private void Rect_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PixelCoord pixelCoord = (sender as Rectangle).Tag as PixelCoord;
            coord.Text = string.Format("xf:{0} yf:{1} x:{2} y:{3}", pixelCoord.fx, pixelCoord.fy, pixelCoord.x, pixelCoord.y);
        }

        /// <summary>
        /// backColour not using for fast coding now, only alpha channel added
        /// </summary>
        /// <param name="alphaChannel"></param>
        /// <param name="frontColour"></param>
        /// <param name="backColour"></param>
        /// <returns></returns>
        private Brush alphaBlend(byte alphaChannel, Brush frontColour, Brush backColour)
        {
            
            return new SolidColorBrush(Color.FromArgb(alphaChannel, (frontColour  as SolidColorBrush).Color.R , (frontColour as SolidColorBrush).Color.G, (frontColour as SolidColorBrush).Color.B));
        }

        private float modf(double source, out double _decimal)
        {
            _decimal = (int)Math.Truncate(source);
            return (float)(source - _decimal);
        }

        private const double DEG2RAD = 0.0174532925;
        void fillArc(int x, int y, int start_angle, int seg_count, int rx, int ry, int w, Brush colour)
        {

            byte seg = 1; // Segments are 3 degrees wide = 120 segments for 360 degrees
            byte inc = 1; // Draw segments every 3 degrees, increase to 6 for segmented ring

            // Calculate first pair of coordinates for segment start
            float sx = (float)Math.Cos((start_angle - 90) * DEG2RAD);
            float sy = (float)Math.Sin((start_angle - 90) * DEG2RAD);
            int x0 = (int) sx * (rx - w) + x;
            int y0 = (int)sy * (ry - w) + y;
            int x1 = (int)sx * rx + x;
            int y1 = (int)sy * ry + y;

            // Draw colour blocks every inc degrees
            for (int i = start_angle; i < start_angle + seg * seg_count; i += inc)
            {

                // Calculate pair of coordinates for segment end
                float sx2 = (float)Math.Cos((i + seg - 90) * DEG2RAD);
                float sy2 = (float)Math.Sin((i + seg - 90) * DEG2RAD);
                int x2 = (int) sx2 * (rx - w) + x;
                int y2 = (int)sy2 * (ry - w) + y;
                int x3 = (int)sx2 * rx + x;
                int y3 = (int)sy2 * ry + y;

                //tft.fillTriangle(x0, y0, x1, y1, x2, y2, colour);
                //tft.fillTriangle(x1, y1, x2, y2, x3, y3, colour);

                // Copy segment end to sgement start for next segment
                x0 = x2;
                y0 = y2;
                x1 = x3;
                y1 = y3;
            }

            // SMOOTH  
            for (int i = start_angle; i < start_angle + seg * seg_count; i += inc)
            {
                // Calculate pair of coordinates for segment end
                sx = (float)Math.Cos((i + seg - 90) * DEG2RAD);
                sy = (float)Math.Sin((i + seg - 90) * DEG2RAD);

                float smoothX = sx * (rx - w) + x;
                float smoothY = sy * (ry - w) + y;

                drawPixel(smoothX, smoothY, colour);
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            drawPixel(10, 10, baseColor);

            fillArc(200, 200, 150, 270, 100, 100, 4, baseColor);

        }
    }
}
