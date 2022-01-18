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

using OWLOSAdmin.EcosystemExplorer.Huds;
using OWLOSThingsManager.EcosystemExplorer.Huds;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OWLOSAirQuality.Huds
{
    /// <summary>
    /// Interaction logic for PercentageControl.xaml
    /// </summary>
    public partial class PercentageControl : UserControl
    {
        private double[] _data = null;
        public double[] data
        {
            get => _data;
            set
            {
                if ((value != null) && (value.Length > 0))
                {
                    _data = value;
                    Draw();
                }
            }
        }

        private double _angle = 0.0f;

        public double angle
        {
            get => _angle;
            set
            {
                if ((value > -1) && (value < 360))
                {
                    _angle = value;
                    // Draw();
                }
            }
        }

        protected double radius = 0;
        public PercentageControl()
        {
            InitializeComponent();
        }

        public PercentageControl(double radius)
        {
            InitializeComponent();
            this.radius = radius;
        }

        private void Draw()
        {
            double startAngle = angle;
            double endAngle;
            int hideStep = 125 / data.Length;
            PetalControl[] percentPetals = new PetalControl[data.Length];
            Color currentColor = (App.Current.Resources["OWLOSPrimary"] as SolidColorBrush).Color;
            for (int i = 0; i < data.Length; i++) 
            {
                endAngle = startAngle + data[i] / 100.0f * 360.0f;
                Path p = new Path
                {
                    StrokeThickness = 40,
                    Data = HudLibrary.DrawArc(Gold.center, Gold.center, radius, startAngle, endAngle),
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 700,
                    Height = 700
                };
                //p.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"];
                currentColor.A -= (byte)(hideStep);
                p.Stroke = new SolidColorBrush(currentColor);
                //var pt = new PathText();
                percentangeMainGrid.Children.Add(p);

                //percentPetals[i] = new PetalControl(radius, 22 + i * 14, 4 * 2, 15, data[i].ToString());
                percentPetals[i] = new PetalControl(radius, (endAngle - startAngle)/2 + startAngle - 12, 10 * 2, 15, data[i].ToString("G4"));
                percentPetals[i].petalBackground.Stroke = null;
                percentPetals[i].petalBorder1.Stroke = null;
                percentPetals[i].petalBorder2.Stroke = null;
                percentPetals[i].petalNameText.Foreground = (SolidColorBrush)App.Current.Resources["OWLOSWarning"];
                percentangeMainGrid.Children.Add(percentPetals[i]);

                startAngle = endAngle;
            }
        }

    }
}
