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

using OWLOSEcosystemService.DTO.Things;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OWLOSAirQuality.Huds
{
    public partial class ModeControl : UserControl
    {
        protected float? OriginalValue = float.NaN;

        protected ColorAnimation animation;

        private Random random = new Random();
        public string Value
        {
            get => _Value != null ? _Value.Text : string.Empty;
            set
            {
                if (_Value != null)
                {                   
                    if (!string.IsNullOrEmpty(value))
                    {
                        _Value.Text = value;

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


                        bool setTrap = false;


                        if (!float.IsNaN((float)OriginalValue))
                        {

                            if (!float.IsNaN(LowDangerTrap))
                            {
                                if (OriginalValue <= LowDangerTrap)
                                {
                                    animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color;
                                    animation.Duration = new Duration(TimeSpan.FromSeconds(0.2 + random.NextDouble()));

                                    Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                                    Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                    setTrap = true;
                                }
                            }

                            if (!float.IsNaN(LowWarningTrap))
                            {
                                if ((OriginalValue <= LowWarningTrap) && (!setTrap))
                                {
                                    animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color;
                                    animation.Duration = new Duration(TimeSpan.FromSeconds(0.7 + +random.NextDouble()));

                                    Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                                    Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                    setTrap = true;
                                }
                            }

                            if (!float.IsNaN(HighDangerTrap))
                            {
                                if ((OriginalValue >= HighDangerTrap) && (!setTrap))
                                {
                                    animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color;
                                    animation.Duration = new Duration(TimeSpan.FromSeconds(0.2 + random.NextDouble()));

                                    Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                                    Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                    setTrap = true;
                                }
                            }

                            if (!float.IsNaN(HighWarningTrap))
                            {
                                if ((OriginalValue >= HighWarningTrap) && (!setTrap))
                                {
                                    animation.To = ((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color;
                                    animation.Duration = new Duration(TimeSpan.FromSeconds(0.5 + random.NextDouble()));

                                    Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"]).Color);
                                    Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                    setTrap = true;
                                }
                            }

                            if (!setTrap)
                            {
                                Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]).Color);
                                Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                            }
                        }
                    }
                    return;
                }
                _Value.Text = "--";
            }
        }

        protected float _LowWarningTrap = float.NaN;
        public float LowWarningTrap
        {
            get => _LowWarningTrap;
            set
            {
                _LowWarningTrap = value;
                Value = Value;
            }
        }

        protected float _HighWarningTrap = float.NaN;
        public float HighWarningTrap
        {
            get => _HighWarningTrap;
            set
            {
                _HighWarningTrap = value;
                Value = Value;
            }
        }

        protected float _LowDangerTrap = float.NaN;
        public float LowDangerTrap
        {
            get => _LowDangerTrap;
            set
            {
                _LowDangerTrap = value;
                Value = Value;
            }
        }

        protected float _HighDangerTrap = float.NaN;
        public float HighDangerTrap
        {
            get => _HighDangerTrap;
            set
            {
                _HighDangerTrap = value;
                Value = Value;
            }
        }


        public string TopDescription
        {
            get => _TopDescription != null ? _TopDescription.Text : string.Empty;
            set
            {
                if (_TopDescription != null)
                {
                    _TopDescription.Text = value;
                }
            }
        }

        public string Caption
        {
            get => _Caption != null ? _Caption.Text : string.Empty;
            set
            {
                if (_Caption != null)
                {
                    _Caption.Text = value;
                }
            }
        }

        public ModeControl()
        {
            InitializeComponent();
        }

        protected void SetValue(float? _OriginalValue)
        {
            if ((_OriginalValue != null) && (!float.IsNaN((float)_OriginalValue)))
            {

                    Value = (OriginalValue = _OriginalValue).ToString();                    
            }
            else
            {
                Value = "NaN";
            }
        }

        public void InjectValue(float _OriginalValue)
        {
            OriginalValue = _OriginalValue;
            Value = OriginalValue.ToString();
        }
        public void OnValueChanged(object sender, ValueEventArgs e)
        {
            base.Dispatcher.Invoke(() =>
            {
                SetValue(e.value);
            });
        }

    }
}
