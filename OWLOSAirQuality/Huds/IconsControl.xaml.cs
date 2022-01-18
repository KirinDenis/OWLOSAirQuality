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
using System.Windows.Media.Animation;

namespace OWLOSAirQuality.Huds
{
    /// <summary>
    /// Interaction logic for IconsControl.xaml
    /// </summary>
    public partial class IconsControl : UserControl
    {

        //private double _angle = 0.0f;
        //private readonly double _length = 180.0f;

        //public double angle
        //{
        //    get => _angle;
        //    set
        //    {
        //        if ((value > -1) && (value < 360))
        //        {
        //            _angle = value;
        //            // Draw();
        //        }
        //    }
        //}
        // Текущее значение иконки (Солнце/Облако/Облако с дождем/Дождь)
        private int _state = 1;
        
        public int state
        {
            get => _state;
            set
            {
                if((value>=1) && (value<=4))
                {
                    _state = value;
                    Animate();
                }
            }

        }

        public int From { get; set; }
        public int To { get; set; }

        public IconsControl()
        {
            InitializeComponent();
        }

        public void Animate()
        {
            //this.Sun.Children
            //Color currentColor = (this.Sun.Background as SolidColorBrush).Color;
            //Color currentColor = ;
            //int hideStep = 125 / 10;
            //for (int i = 0; i < 10; i++)
            //{
            //    currentColor.A -= (byte)(hideStep);
            //    this.Sun.Background = new SolidColorBrush(currentColor);
            //}


            DoubleAnimation opacityAnimationFadeIn = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(10000)),
                AutoReverse = false
            };

            DoubleAnimation opacityAnimationFadeOut = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(10000)),
                AutoReverse = false
            };


            TransformGroup transformGroup = new TransformGroup();

            ScaleTransform scaleTransform = new ScaleTransform
            {
                ScaleX = 0.3,
                ScaleY = 0.3,
                //scaleTransform.CenterX = 350;
                //scaleTransform.CenterY = 350;
                CenterX = 525,
                CenterY = 525
            };
            transformGroup.Children.Add(scaleTransform);

            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,                
                Duration = new Duration(TimeSpan.FromMilliseconds(10000))
            };

            RotateTransform rotateTransform = new RotateTransform
            {
                Angle = 180,
                CenterX = 350,
                CenterY = 350
            };
            transformGroup.Children.Add(rotateTransform);

            //Sun.RenderTransform = rotateTransform;
            Sun.RenderTransform = transformGroup;
            //Sun.Visibility = Visibility.Hidden;

            Storyboard story = new Storyboard();
            story.Children.Add(rotateAnimation);
            Storyboard.SetTarget(rotateAnimation, Sun);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            if(state==1)
            {
                story.Children.Add(opacityAnimationFadeOut);
                Storyboard.SetTarget(opacityAnimationFadeOut, Sun);
                Storyboard.SetTargetProperty(opacityAnimationFadeOut, new PropertyPath("Opacity"));
            }
            else
            {
                story.Children.Add(opacityAnimationFadeIn);
                Storyboard.SetTarget(opacityAnimationFadeIn, Sun);
                Storyboard.SetTargetProperty(opacityAnimationFadeIn, new PropertyPath("Opacity"));
            }
            


            story.Begin();

            TransformGroup transformGroupClouds = new TransformGroup();
            transformGroupClouds.Children.Add(scaleTransform);

            DoubleAnimation rotateAnimationClouds = new DoubleAnimation()
            {
                From = 90,
                To = 450,
                Duration = new Duration(TimeSpan.FromMilliseconds(10000))
            };

            RotateTransform rotateTransformClouds = new RotateTransform
            {
                Angle = 45,
                CenterX = 350,
                CenterY = 350
            };

            transformGroupClouds.Children.Add(rotateTransformClouds);
            Clouds.RenderTransform = transformGroupClouds;

            Storyboard storyClouds = new Storyboard();
            storyClouds.Children.Add(rotateAnimationClouds);
            Storyboard.SetTarget(rotateAnimationClouds, Clouds);
            Storyboard.SetTargetProperty(rotateAnimationClouds, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            if(state==2)
            {
                storyClouds.Children.Add(opacityAnimationFadeOut);
                Storyboard.SetTarget(opacityAnimationFadeOut, Clouds);
                Storyboard.SetTargetProperty(opacityAnimationFadeOut, new PropertyPath("Opacity"));
            }
            else
            {
                storyClouds.Children.Add(opacityAnimationFadeIn);
                Storyboard.SetTarget(opacityAnimationFadeIn, Clouds);
                Storyboard.SetTargetProperty(opacityAnimationFadeIn, new PropertyPath("Opacity"));
            }

            storyClouds.Begin();

            //Sun.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            // Cloudy
            TransformGroup transformGroupCloudy = new TransformGroup();
            transformGroupCloudy.Children.Add(scaleTransform);

            DoubleAnimation rotateAnimationCloudy = new DoubleAnimation()
            {
                From = 180,
                To = 540,
                Duration = new Duration(TimeSpan.FromMilliseconds(10000))
            };

            RotateTransform rotateTransformCloudy = new RotateTransform
            {
                Angle = 90,
                CenterX = 350,
                CenterY = 350
            };

            transformGroupCloudy.Children.Add(rotateTransformCloudy);
            Cloudy.RenderTransform = transformGroupCloudy;

            Storyboard storyCloudy = new Storyboard();
            storyCloudy.Children.Add(rotateAnimationCloudy);
            Storyboard.SetTarget(rotateAnimationCloudy, Cloudy);
            Storyboard.SetTargetProperty(rotateAnimationCloudy, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));
           
            if(state==3)
            {
                storyCloudy.Children.Add(opacityAnimationFadeOut);
                Storyboard.SetTarget(opacityAnimationFadeOut, Cloudy);
                Storyboard.SetTargetProperty(opacityAnimationFadeOut, new PropertyPath("Opacity"));
            }
            else
            {
                storyCloudy.Children.Add(opacityAnimationFadeIn);
                Storyboard.SetTarget(opacityAnimationFadeIn, Cloudy);
                Storyboard.SetTargetProperty(opacityAnimationFadeIn, new PropertyPath("Opacity"));
            }

            storyCloudy.Begin();

            // END Cloudy

            /// Stormy begin
            /// 
            TransformGroup transformGroupStormy = new TransformGroup();
            transformGroupStormy.Children.Add(scaleTransform);

            DoubleAnimation rotateAnimationStormy = new DoubleAnimation()
            {
                From = 270,
                To = 630,
                Duration = new Duration(TimeSpan.FromMilliseconds(10000))
            };

            RotateTransform rotateTransformStormy = new RotateTransform
            {
                Angle = 90,
                CenterX = 350,
                CenterY = 350
            };

            transformGroupStormy.Children.Add(rotateTransformStormy);
            Stormy.RenderTransform = transformGroupStormy;

            Storyboard storyStormy = new Storyboard();
            storyStormy.Children.Add(rotateAnimationStormy);
            Storyboard.SetTarget(rotateAnimationStormy, Stormy);
            Storyboard.SetTargetProperty(rotateAnimationStormy, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            if(state==4)
            {
                storyStormy.Children.Add(opacityAnimationFadeOut);
                Storyboard.SetTarget(opacityAnimationFadeOut, Stormy);
                Storyboard.SetTargetProperty(opacityAnimationFadeOut, new PropertyPath("Opacity"));
            }
            else
            {
                storyStormy.Children.Add(opacityAnimationFadeIn);
                Storyboard.SetTarget(opacityAnimationFadeIn, Stormy);
                Storyboard.SetTargetProperty(opacityAnimationFadeIn, new PropertyPath("Opacity"));
            }


            storyStormy.Begin();




            // END Stormy
        }
    }
}
