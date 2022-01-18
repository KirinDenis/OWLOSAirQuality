/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2019, 2020, 2021 by:
- Denis Kirin (deniskirinacs@gmail.com)

This file is part of OWLOS DIY Open Source OS for building IoT ecosystems

OWLOS is free software : you can redistribute it and/or modify it under the
terms of the GNU General Public License as published by the Free Software
Foundation, either version 3 of the License, or (at your option) any later
version.

OWLOS is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with OWLOS. If not, see < https://www.gnu.org/licenses/>.

GitHub: https://github.com/KirinDenis/owlos

(Этот файл — часть OWLOS DIY Open Source OS for building IoT ecosystems.

OWLOS - свободная программа: вы можете перераспространять ее и/или изменять
ее на условиях Стандартной общественной лицензии GNU в том виде, в каком она
была опубликована Фондом свободного программного обеспечения; версии 3
лицензии, любой более поздней версии.

OWLOS распространяется в надежде, что она будет полезной, но БЕЗО ВСЯКИХ
ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА или ПРИГОДНОСТИ ДЛЯ
ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ.
Подробнее см.в Стандартной общественной лицензии GNU.

Вы должны были получить копию Стандартной общественной лицензии GNU вместе с
этой программой. Если это не так, см. <https://www.gnu.org/licenses/>.)
--------------------------------------------------------------------------------------*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PathText
{
    /// <summary>
    /// Interaction logic for PathText.xaml
    /// </summary>
    public class PathTextControl
    {
        private Grid grid = null;
        private readonly TextBlock sourceTextBlock = null;
        private TextBlock[] textBlock;

        private readonly double x;
        private readonly double y;
        private readonly double radius;
        private readonly double fromAngel;
        private readonly double toAngel;

        public SolidColorBrush Foreground
        {
            get
            {
                return sourceTextBlock.Foreground as SolidColorBrush;
            }
            set
            {
                if (textBlock != null)
                {
                    for (int i = 0; i < textBlock.Length; i++)
                    {
                        textBlock[i].Foreground = value;
                    }
                }
            }
        }

        public PathTextControl(double x, double y, double radius, double fromAngel, double toAngel, TextBlock sourceTextBlock)
        {

            this.x = x;
            this.y = y;
            this.radius = radius;
            this.fromAngel = fromAngel;
            this.toAngel = toAngel;

            this.sourceTextBlock = sourceTextBlock;

            Update();

        }

        private void Update()
        {

            if (sourceTextBlock.Parent == null)
            {
                return;
            }

            if (grid == null)
            {
                grid = sourceTextBlock.Parent as Grid;
            }

            

            textBlock = new TextBlock[sourceTextBlock.Text.Length];

            double textBlocksAngel = 0;

            for (int i = 0; i < sourceTextBlock.Text.Length; i++)
            {
                textBlock[i] = new TextBlock
                {
                    Text = sourceTextBlock.Text[i].ToString(),
                    FontSize = sourceTextBlock.FontSize,
                    FontFamily = sourceTextBlock.FontFamily,
                    FontStretch = sourceTextBlock.FontStretch,
                    FontWeight = sourceTextBlock.FontWeight,
                    FontStyle = sourceTextBlock.FontStyle,
                    IsHitTestVisible = false,
                    Background = sourceTextBlock.Background,
                    Foreground = sourceTextBlock.Foreground
                };

                grid.Children.Add(textBlock[i]);

                textBlock[i].Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                textBlocksAngel += textBlock[i].DesiredSize.Width / 5;

            }

            double angel = fromAngel;

            angel += 25 / 2 - textBlocksAngel / 2;

            Rotate(angel);

            /*
            for (int i = 0; i < sourceTextBlock.Text.Length; i++)
            {
                //if (angel > this.toAngel - 90) break;

                double xP = radius * Math.Cos(angel / ((180 / Math.PI))) + x;
                double yP = radius * Math.Sin(angel / ((180 / Math.PI))) + y;

                TransformGroup group = new TransformGroup();

                RotateTransform rt = new RotateTransform();

                TranslateTransform tt = new TranslateTransform
                {
                    X = xP,
                    Y = yP
                };

                group.Children.Add(rt);
                group.Children.Add(tt);

                textBlock[i].RenderTransform = group;


                rt.Angle = angel + 97.0f - (textBlock[i].DesiredSize.Width);
                angel += textBlock[i].DesiredSize.Width / (radius / 60.0f);

            }
            */
            grid.Children.Remove(sourceTextBlock);

        }

        public void Rotate(double angel)
        {
            angel -= 90;

            for (int i = 0; i < textBlock.Length; i++)
            {
                double xP = radius * Math.Cos(angel / ((180 / Math.PI))) + x;
                double yP = radius * Math.Sin(angel / ((180 / Math.PI))) + y;

                TransformGroup group = new TransformGroup();

                RotateTransform rt = new RotateTransform();

                TranslateTransform tt = new TranslateTransform
                {
                    X = xP,
                    Y = yP
                };

                group.Children.Add(rt);
                group.Children.Add(tt);

                textBlock[i].RenderTransform = group;

                double charWidth = textBlock[i].DesiredSize.Width;
                if (textBlock[i].Text.ToLower().Equals("m"))
                {
                    charWidth *= 0.5f;
                }

                rt.Angle = angel + 97.0f - charWidth;
                angel += textBlock[i].DesiredSize.Width / (radius / 60.0f);
            }
        }
    }
}
