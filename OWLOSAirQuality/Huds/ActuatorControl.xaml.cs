/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2019, 2020 by:
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
using OWLOSThingsManager.EcosystemExplorer.Huds;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OWLOSAirQuality.Huds
{
    /// <summary>
    /// Interaction logic for ValueControl.xaml
    /// </summary>
    public partial class ActuatorControl : UserControl
    {

        private Path ButtonPath;
        private Path ButtonBorderPath;

        private double ValueRadius = 0.0f;

        private SolidColorBrush OnColor = ((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha3"]);
        private SolidColorBrush OnSelectedColor = ((SolidColorBrush)App.Current.Resources["OWLOSInfo"]);

        private SolidColorBrush OffColor = ((SolidColorBrush)App.Current.Resources["OWLOSDark"]);
        private SolidColorBrush OffSelectedColor = ((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"]);


        private bool _On = false;
        public bool On
        {
            get => _On;
            set
            {                
                _On = value;
                ButtonPath_MouseLeave(null, null);
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

        public string Description
        {
            get => _Description != null ? _Description.Text : string.Empty;
            set
            {
                if (_Description != null)
                {
                    _Description.Text = value;
                }
            }
        }


        protected bool _Focused = false;
        public bool Focused
        {
            get => _Focused;

            set
            {
                _Focused = value;
                if (_Focused)
                {
                    SelSQ1.Visibility = SelSQ2.Visibility = SelSQ3.Visibility = SelSQ4.Visibility = SelSQ5.Visibility = SelSQ6.Visibility = SelSQ7.Visibility = SelSQ8.Visibility = System.Windows.Visibility.Visible;
                    OnSelect?.Invoke(this, new EventArgs());
                }
                else
                {
                    SelSQ1.Visibility = SelSQ2.Visibility = SelSQ3.Visibility = SelSQ4.Visibility = SelSQ5.Visibility = SelSQ6.Visibility = SelSQ7.Visibility = SelSQ8.Visibility = System.Windows.Visibility.Hidden;
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

                base.Dispatcher.Invoke(() =>
                {
                    try
                    {

                        switch (_Status)
                        {
                            case NetworkStatus.Online:
                                _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSuccessAlpha2"]).Color);
                                _StatusText.Text = "ONLINE";
                                BoxRectangle.Stroke = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSecondary"]).Color);
                                break;
                            case NetworkStatus.Offline:
                                _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color);
                                _StatusText.Text = "OFFLINE";
                                BoxRectangle.Stroke = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSecondaryAlpha3"]).Color);
                                break;
                            case NetworkStatus.Reconnect:
                                _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"]).Color);
                                _StatusText.Text = "RECONNECT";
                                BoxRectangle.Stroke = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSecondary"]).Color);
                                break;
                            default:
                                _StatusText.Background = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color);
                                _StatusText.Text = "ERROR";
                                BoxRectangle.Stroke = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha1"]).Color);
                                break;
                        }
                    }
                    catch { }
                });
            }
        }

        protected float? OriginalValue = float.NaN;

        public delegate void OnSelectEventHandler(object? sender, EventArgs e);
        public event OnSelectEventHandler OnSelect;

        public delegate void OnChangeEventHandler(object? sender, bool e);
        public event OnChangeEventHandler OnChange;


        public ActuatorControl()
        {
            InitializeComponent();
            Status = NetworkStatus.Offline;

            ValueRadius = ValueGrid.Height / 3.7f;

            ButtonBorderPath = new Path();
            PathGrid.Children.Add(ButtonBorderPath);
            ButtonBorderPath.Data = HudLibrary.DrawArc(ValueGrid.Width / 2.0f, ValueGrid.Height / 2.0f, ValueRadius, 0, 359);
            ButtonBorderPath.RenderTransformOrigin = new Point(0.5f, 0.5f);
            ButtonBorderPath.StrokeThickness = 2;
            ButtonBorderPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"];

            ButtonPath = new Path();
            PathGrid.Children.Add(ButtonPath);
            ButtonPath.Data = HudLibrary.DrawArc(ValueGrid.Width / 2.0f, ValueGrid.Height / 2.0f, ValueRadius - 8, 0, 359);
            ButtonPath.RenderTransformOrigin = new Point(0.5f, 0.5f);
            ButtonPath.StrokeThickness = 2;
            ButtonPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha1"];
            ButtonPath.Fill = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"];
            ButtonPath.Cursor = Cursors.Hand;


            ButtonPath.PreviewMouseDown += ButtonPath_PreviewMouseDown;
            ButtonPath.MouseEnter += ButtonPath_MouseEnter;
            ButtonPath.MouseLeave += ButtonPath_MouseLeave;

        }

        private void ButtonPath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Status = NetworkStatus.Reconnect;
            On = !_On;
            OnChange?.Invoke(this, On);
        }

        private void ButtonPath_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            base.Dispatcher.Invoke(() =>
            {
                try
                {
                    SolidColorBrush toColor = _On ? OnSelectedColor : OffSelectedColor;

                    if (App.UseAnimation)
                    {
                        ColorAnimation animation;
                        animation = new ColorAnimation
                        {
                            To = toColor.Color,
                            Duration = new Duration(TimeSpan.FromSeconds(0.7))
                        };

                        ButtonPath.Fill = new SolidColorBrush(((SolidColorBrush)ButtonPath.Fill).Color);
                        ButtonPath.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                    }
                    else
                    {
                        ButtonPath.Fill = toColor;
                    }
                }
                catch { }
            });
        }

        private void ButtonPath_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            base.Dispatcher.Invoke(() =>
            {
                try
                {

                    SolidColorBrush toColor = _On ? OnColor : OffColor;

                    if (App.UseAnimation)
                    {
                        ColorAnimation animation;
                        animation = new ColorAnimation
                        {
                            To = toColor.Color,
                            Duration = new Duration(TimeSpan.FromSeconds(0.7))
                        };

                        ButtonPath.Fill = new SolidColorBrush(((SolidColorBrush)ButtonPath.Fill).Color);
                        ButtonPath.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                    }
                    else
                    {
                        ButtonPath.Fill = toColor;
                    }
                }
                catch { }
            });

        }

        private void UserControl_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Focused = true;
        }

        private void UserControl_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Focused = false;
        }

        private void UserControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Focused = Focus();
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (App.UseAnimation)
            {
                ColorAnimation animation;
                animation = new ColorAnimation
                {
                    To = ((SolidColorBrush)App.Current.Resources["OWLOSSecondaryAlpha1"]).Color,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };

                BoxRectangle.Fill = new SolidColorBrush(((SolidColorBrush)Background).Color);
                BoxRectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                BoxRectangle.Fill = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSecondaryAlpha1"]).Color);
            }
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (App.UseAnimation)
            {
                ColorAnimation animation;
                animation = new ColorAnimation
                {
                    To = Color.FromArgb(0x01, 0x00, 0x00, 0x00),
                    Duration = new Duration(TimeSpan.FromSeconds(2))
                };
                BoxRectangle.Fill = new SolidColorBrush(((SolidColorBrush)App.Current.Resources["OWLOSSecondaryAlpha1"]).Color);
                BoxRectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                BoxRectangle.Fill = new SolidColorBrush(Color.FromArgb(0x01, 0x00, 0x00, 0x00));
            }

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try //XAML editor value change can be randomness
            {
                Canvas.SetLeft(BoxRectangle, 6.0f);
                Canvas.SetTop(BoxRectangle, 6.0f);
                Canvas.SetLeft(BoxTrapRectangle, 6.0f);
                Canvas.SetTop(BoxTrapRectangle, 6.0f);

                BoxTrapRectangle.Width = BoxRectangle.Width = e.NewSize.Width - 6 * 2;
                BoxTrapRectangle.Height = BoxRectangle.Height = e.NewSize.Height - 6 * 2;

                SmallBoxRectangle.Width = e.NewSize.Width;
                SmallBoxRectangle.Height = e.NewSize.Height;

                SelSQ3.X1 = e.NewSize.Width + 1;
                SelSQ3.X2 = e.NewSize.Width - 10;

                SelSQ4.X1 = e.NewSize.Width;
                SelSQ4.X2 = e.NewSize.Width;

                SelSQ5.Y1 = e.NewSize.Height;
                SelSQ5.Y2 = e.NewSize.Height;

                SelSQ6.Y1 = e.NewSize.Height;
                SelSQ6.Y2 = e.NewSize.Height - 10;

                SelSQ7.X1 = e.NewSize.Width + 1;
                SelSQ7.X2 = e.NewSize.Width - 10;
                SelSQ7.Y1 = e.NewSize.Height;
                SelSQ7.Y2 = e.NewSize.Height;

                SelSQ8.X1 = e.NewSize.Width;
                SelSQ8.X2 = e.NewSize.Width;
                SelSQ8.Y1 = e.NewSize.Height;
                SelSQ8.Y2 = e.NewSize.Height - 10;

                RSLine1.X1 = e.NewSize.Width - 8;
                RSLine1.X2 = e.NewSize.Width - 8;
                RSLine1.Y1 = e.NewSize.Height - 8;
                RSLine1.Y2 = e.NewSize.Height - 20;

                RSLine2.X1 = e.NewSize.Width - 6;
                RSLine2.X2 = e.NewSize.Width - 20;
                RSLine2.Y1 = e.NewSize.Height - 8;
                RSLine2.Y2 = e.NewSize.Height - 8;

            }
            catch { }
        }
    }
}
