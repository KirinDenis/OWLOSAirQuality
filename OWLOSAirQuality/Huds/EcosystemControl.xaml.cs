﻿/* ----------------------------------------------------------------------------
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

using OWLOSAirQuality;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OWLOSThingsManager.EcosystemExplorer
{
    /// <summary>
    /// Interaction logic for EcosystemControl.xaml
    /// </summary>

    public partial class EcosystemControl : UserControl
    {

        private SingleAirQualityMainWindow _window = null;
        //когда панель принадлежит отдельному окну
        public SingleAirQualityMainWindow window
        {
            get => _window;
            set
            {
                _window = value;
                if (_window == null)
                {
                    OnEcosystem?.Invoke(this, new EventArgs());
                }
                else
                {
                    OnWindow?.Invoke(this, new EventArgs());
                }
            }

        }

        //когда панель принадлежит окну экосистемы (сюда сохраняестя Parent для экосистемы)
        public Grid parentGrid = null;

        public Transform storedRenderTransform = null;

        private readonly IEcosystemChildControl childControl = null;

        private readonly DependencyPropertyDescriptor renderTransform = DependencyPropertyDescriptor.FromProperty(RenderTransformProperty, typeof(UserControl));

        private bool isInDrag = false;

        private Point currentPoint;

        private Point anchorPoint;

        public double resizeArea = 20.0f;
        public TranslateTransform transform { get; } = new TranslateTransform();


        private static EcosystemControl CurrentFocused;

        private bool _isFocused;

        private bool _isVisible = false;
        public bool isVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                if (value)
                {
                    OnShow?.Invoke(this, new EventArgs());
                }
                else
                {
                    OnHiden?.Invoke(this, new EventArgs());
                }
            }
        }

        private readonly bool lockMove = false;

        private Point ResizePoint;

        public event EventHandler OnPositionChanged;
        public event EventHandler OnShow;
        public event EventHandler OnHiden;
        public event EventHandler OnWindow;
        public event EventHandler OnEcosystem;

        public bool IsFocused
        {
            get => _isFocused;
            private set
            {
                _isFocused = value;
                if (_isFocused)
                {
                    childControl?.OnParentGetFocus();
                    Dispatcher.Invoke(() =>
                    {
                        {
                            FrameworkElement frameworkElement = Mouse.DirectlyOver as FrameworkElement;
                            Type directlyOverType = frameworkElement?.GetType();
                            if (directlyOverType == null || (directlyOverType.Name != "TextBoxView" &&
                                                             directlyOverType != typeof(Slider) &&
                                                             directlyOverType != typeof(Button) &&
                                                             directlyOverType != typeof(CheckBox) &&
                                                             directlyOverType != typeof(ComboBox) &&
                                                             directlyOverType != typeof(Slider) &&
                                                             directlyOverType != typeof(ScrollViewer) &&
                                                             directlyOverType != typeof(ScrollBar) &&
                                                             directlyOverType != typeof(TextBox) &&
                                                             directlyOverType != typeof(Border)))
                            {

                            }
                        }

                        //Перемещаем текущий object вниз списка, тем самым делая его главнее.
                        if (Parent is Panel panel)
                        {
                            //    Unloaded -= UserControl_Unloaded;
                            panel.Children.Remove(this);
                            panel.Children.Add(this);
                            //   Unloaded += UserControl_Unloaded;
                        }
                    }
                    );
                }
                else
                {
                    childControl?.OnParentLostFocus();
                    Dispatcher.Invoke(() =>
                    {
                        //  if ((treeViewItem != null) && treeViewItem.IsSelected)
                        //  {
                        //      treeViewItem.IsSelected = false;
                        //  }
                    }
                    );
                }

                if (CurrentFocused != null
                    && CurrentFocused != this
                    && CurrentFocused.IsFocused
                    && IsFocused)
                {
                    CurrentFocused.IsFocused = false;
                    CurrentFocused = this;
                }
                else
                {
                    CurrentFocused = this;
                }
            }
        }

        public EcosystemControl(IEcosystemChildControl childControl, Point possition = default(Point), Point size = default(Point))
        {
            InitializeComponent();

            if (childControl != null)
            {
                this.childControl = childControl;
                childHolderGrid.Children.Add(childControl as UserControl);
            }

            MoveTransform(possition.X, possition.Y);

            if (size != default(Point))
            {
                Width = size.X;
                Height = size.Y;
            }

            childControl?.OnParentLostFocus();
        }

        public void Show()
        {
            isVisible = true;
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = (new BrushConverter().ConvertFromString("#FFFFFFFF") as SolidColorBrush).Color,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            OpacityMask = new BrushConverter().ConvertFromString("#00FFFFFF") as SolidColorBrush;
            OpacityMask.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        public void Hide()
        {
            isVisible = false;
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = (new BrushConverter().ConvertFromString("#00FFFFFF") as SolidColorBrush).Color,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            OpacityMask = new BrushConverter().ConvertFromString("#FFFFFFFF") as SolidColorBrush;
            OpacityMask.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }


        public void MoveTransform(double x, double y)
        {
            transform.X = x;
            transform.Y = y;
            renderTransform.AddValueChanged(this, EcosystemControlPositionChanged);
            RenderTransform = transform;
        }
        public void EcosystemControlPositionChanged(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(sender, e);
        }

        private void EcosystemControlPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (window == null)
            {
                if (isInDrag)
                {
                    //this.Margin = new Thickness(e.GetPosition(Parent as Grid).X - clickLocalPosition.X, e.GetPosition(Parent as Grid).Y - clickLocalPosition.Y, 0, 0);
                    currentPoint = e.GetPosition(Parent as Grid);
                    Point localPoint = e.GetPosition(this);

                    if (double.IsNaN(Width))
                    {
                        Width = ActualWidth;
                    }

                    if (double.IsNaN(Height))
                    {
                        Height = ActualHeight;
                    }


                    //drag or resize 
                    if ((localPoint.X > resizeArea) && (localPoint.Y > resizeArea) && (Width - localPoint.X > resizeArea) && (Height - localPoint.Y > resizeArea))
                    {
                        //OWLOSWindow has drag itself
                        if (window == null)
                        {
                            transform.X += (currentPoint.X - anchorPoint.X);
                            transform.Y += (currentPoint.Y - anchorPoint.Y);
                            RenderTransform = transform;
                        }
                    }
                    else
                    {
                        if (window == null)
                        {
                            try
                            {
                                Width = ActualWidth + (currentPoint.X - anchorPoint.X);
                                Height = ActualHeight + (currentPoint.Y - anchorPoint.Y);
                            }
                            catch
                            { }
                        }
                    }

                    anchorPoint = currentPoint;

                }
            }
        }

        private void EcosystemControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (window == null)
                {

                    FrameworkElement frameworkElement = Mouse.DirectlyOver as FrameworkElement;
                    Type directlyOverType = frameworkElement?.GetType();
                    if (directlyOverType == null || (directlyOverType.Name != "TextBoxView" &&
                                                     directlyOverType != typeof(Slider) &&
                                                     directlyOverType != typeof(Button) &&
                                                     directlyOverType != typeof(CheckBox) &&
                                                     directlyOverType != typeof(ComboBox) &&
                                                     directlyOverType != typeof(Slider) &&
                                                     //directlyOverType != typeof(ScrollViewer) &&
                                                     directlyOverType != typeof(ScrollBar) &&
                                                     directlyOverType != typeof(TextBox)))
                    {
                        anchorPoint = e.GetPosition(Parent as Grid);


                        CaptureMouse();

                        isInDrag = true;
                    }
                }
                else
                {
                    window.DragMove();
                }
            }

        }

        private void EcosystemControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (isInDrag)
                {
                    ReleaseMouseCapture();
                    isInDrag = false;
                }
            }
        }

        private void EcosystemControlGotFocus(object sender, RoutedEventArgs e)
        {
            IsFocused = true;
        }

        private void EcosystemControlLostFocus(object sender, RoutedEventArgs e)
        {
            IsFocused = false;
        }

        private void EcosystemControlPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ResizePoint = e.GetPosition(Parent as Grid);
                EcosystemControlGotFocus(this, null);

                // EcosystemControlMouseDown(this, e);


            }
        }

        private void SwitchFromPanelToWindow()
        {

            if (window == null)
            {
                window = new SingleAirQualityMainWindow
                {
                    Width = ActualWidth,
                    Height = ActualHeight
                };
                storedRenderTransform = RenderTransform;
                //сразу после этого Parent уходит в null
                parentGrid = (Parent as Grid);
                (Parent as Grid).Children.Remove(this);
                childHolderGrid.Children.Remove(childControl as UserControl);
                //window.MainGrid.Children.Add(this);
                window.MainGrid.Children.Add(childControl as UserControl);

                window.CloseTextBlock.MouseDown += SwitchWindowButton_MouseDown;

                RenderTransform = null;
                HorizontalAlignment = HorizontalAlignment.Stretch;
                VerticalAlignment = VerticalAlignment.Stretch;

                Show();
                window.Show();
            }

        }

        private void SwitchFromWindowToPanel()
        {
            window.MainGrid.Children.Remove(childControl as UserControl);
            parentGrid.Children.Add(this);
            childHolderGrid.Children.Add(childControl as UserControl);
            RenderTransform = storedRenderTransform;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            Show();
            window.Hide();
            window = null;

        }

        private void SwitchWindowButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (window == null)
            {
                SwitchFromPanelToWindow();
            }
            else
            {
                SwitchFromWindowToPanel();
            }

        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (window == null)
            {
                Hide();
            }
            else
            {
                SwitchFromWindowToPanel();
                Hide();
            }
        }

        private void HideTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = ((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha3"]).Color,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            (sender as TextBlock).Background = new SolidColorBrush(((SolidColorBrush)(sender as TextBlock).Background).Color);
            (sender as TextBlock).Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

        }

        private void HideTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = ((SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"]).Color,
                Duration = new Duration(TimeSpan.FromSeconds(1.3))
            };

            (sender as TextBlock).Background = new SolidColorBrush(((SolidColorBrush)(sender as TextBlock).Background).Color);
            (sender as TextBlock).Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        private void CloseTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = ((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha3"]).Color,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            (sender as TextBlock).Background = new SolidColorBrush(((SolidColorBrush)(sender as TextBlock).Background).Color);
            (sender as TextBlock).Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

        }

        private void CloseTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ColorAnimation animation;
            animation = new ColorAnimation
            {
                To = ((SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"]).Color,
                Duration = new Duration(TimeSpan.FromSeconds(1.3))
            };

            (sender as TextBlock).Background = new SolidColorBrush(((SolidColorBrush)(sender as TextBlock).Background).Color);
            (sender as TextBlock).Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

    }
}
