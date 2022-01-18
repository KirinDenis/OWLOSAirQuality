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

using OWLOSThingsManager.Ecosystem.OWLOS;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OWLOSAirQuality.Huds
{
    public partial class ThingConnectionControl : UserControl
    {

        protected ColorAnimation animation;

        private readonly Random random = new Random();
        public string Name
        {
            get => _Name != null ? _Name.Text : string.Empty;
            set
            {
                if (_Name != null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        _Name.Text = value;
                    }
                }
            }
        }


        protected NetworkStatus _Status = NetworkStatus.Offline;
        public NetworkStatus Status
        {
            get => _Status;

            set
            {
                _Status = value;

                if (animation == null)
                {
                    animation = new ColorAnimation
                    {
                        To = ((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color,
                        Duration = new Duration(TimeSpan.FromSeconds(0.7 + random.NextDouble())),
                        RepeatBehavior = RepeatBehavior.Forever
                    };
                    Background = new SolidColorBrush(((SolidColorBrush)Background).Color);
                }

                switch (_Status)
                {
                    case NetworkStatus.Online:
                        _Name.Foreground = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSLight"]).Color);
                        _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSuccessAlpha2"]).Color);
                        _StatusText.Text = "ONLINE";

                        Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                        Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                        break;
                    case NetworkStatus.Offline:
                        _Name.Foreground = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfo"]).Color);
                        _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color);
                        _StatusText.Text = "OFFLINE";

                        animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color;
                        animation.Duration = new Duration(TimeSpan.FromSeconds(0.2 + random.NextDouble()));

                        Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                        Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);


                        break;
                    case NetworkStatus.Reconnect:
                        _Name.Foreground = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSPrimary"]).Color);
                        _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"]).Color);
                        _StatusText.Text = "RECONNECT";


                        animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"]).Color;
                        animation.Duration = new Duration(TimeSpan.FromSeconds(0.2 + random.NextDouble()));

                        Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                        Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                        break;
                    default:
                        _Name.Foreground = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSDanger"]).Color);
                        _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color);
                        _StatusText.Text = "ERROR";

                        animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color;
                        animation.Duration = new Duration(TimeSpan.FromSeconds(0.2 + random.NextDouble()));

                        Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                        Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                        break;
                }
            }
        }

        public ThingConnectionControl()
        {
            InitializeComponent();
        }
    }
}
