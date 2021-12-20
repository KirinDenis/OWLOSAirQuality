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

using OWLOSAirQuality.Huds;
using OWLOSAirQuality.OWLOSEcosystemService;
using OWLOSEcosystemService.DTO.Things;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace OWLOSAirQuality.Frames
{
    public partial class PowerDashboardFrame : Window
    {
        private readonly OWLOSEcosystemServiceClient EcosystemServiceClient;
        private bool SensorsJoined = false;

        //private Random random = new Random();
        //private double Direction = 10.0f;
        //private double Speed = 4.0f;

        private readonly List<SearchIndex> SearchIndices = new List<SearchIndex>();
        public PowerDashboardFrame(OWLOSEcosystemServiceClient EcosystemServiceClient)
        {
            InitializeComponent();

            this.EcosystemServiceClient = EcosystemServiceClient;

            EcosystemServiceClient.OnACDataReady += Ecosystem_OnACDataReady;

            Timer lifeCycleTimer2 = new Timer(1000)
            {
                AutoReset = true
            };
            lifeCycleTimer2.Elapsed += LifeCycleTimer2_Elapsed;
            lifeCycleTimer2.Start();

        }

        private void Ecosystem_OnACDataReady(object sender, EventArgs e)
        {
            base.Dispatcher.Invoke(() =>
            {

                try
                {
                    _BackgroundControl.URL = EcosystemServiceClient.URL;
                    ThingAirQuality acData = EcosystemServiceClient.dailyAirQulity[OWLOSEcosystemServiceClient.dailyAirQulitySize - 1];
                    if (acData != null)
                    {
                        switch (acData.Status)
                        {
                            case ThingAirQualityStatus.Online:
                                _BackgroundControl.Status = "[online] " + DateTime.Now;
                                break;
                            case ThingAirQualityStatus.OnlineWithError:
                                _BackgroundControl.Status = "[online with error] " + DateTime.Now;
                                break;
                            case ThingAirQualityStatus.Offline:
                                _BackgroundControl.Status = "[offline] " + DateTime.Now;
                                break;
                            default:
                                _BackgroundControl.Status = "[error] " + DateTime.Now;
                                break;
                        }
                        if (!SensorsJoined)
                        {
                            acData.OnDHT22tempChanged += DHT22TemperatureControl.OnValueChanged;

                            acData.OnBMP280temperatureChanged += BMP280TemperatureControl.OnValueChanged;

                            acData.OnCCS811tempChanged += CCS811TemperatureControl.OnValueChanged;


                            SensorsJoined = true;
                        }
                    }
                    else
                    {
                        _BackgroundControl.Status = "[data is null] " + DateTime.Now;
                    }
                }
                catch
                {
                    _BackgroundControl.Status = "[error] " + DateTime.Now;
                }
            });
        }



        private void LifeCycleTimer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
         base.Dispatcher.Invoke(() =>
         {

             if (float.IsNaN((float)TestRadialValueControl1.OriginalValue))
             {
                 TestRadialValueControl1.OriginalValue = 0;
             }
             //--- TEMP
             if (Speed > 0)
             {
                 if (Direction > (float)TestRadialValueControl1.OriginalValue)
                 {
                     TestRadialValueControl1.OnValueChanged(null, new ValueEventArgs((float)(TestRadialValueControl1.OriginalValue + Speed)));
                 }
                 else
                 {
                     TestRadialValueControl1.OnValueChanged(null, new ValueEventArgs((float)Direction));
                     float _direction = random.Next(100);
                     if (_direction > Direction)
                     {
                         Speed = random.Next(100) / 10.0f + 1; //0..1
                         Direction = _direction;
                     }
                     else
                     {
                         Speed = -(random.Next(100) / 10.0f) - 1; //0..1
                         Direction = _direction;
                     }

                 }
             }
             else
             {
                 if (Direction > (float)TestRadialValueControl1.OriginalValue)
                 {
                     TestRadialValueControl1.OnValueChanged(null, new ValueEventArgs((float)(TestRadialValueControl1.OriginalValue + Speed)));
                 }
                 else
                 {
                     TestRadialValueControl1.OnValueChanged(null, new ValueEventArgs((float)Direction));
                     float _direction = random.Next(100);
                     if (_direction > Direction)
                     {
                         Speed = random.Next(100) / 10.0f + 1; //0..1
                         Direction = _direction;
                     }
                     else
                     {
                         Speed = -(random.Next(100) / 10.0f) - 1; //0..1
                         Direction = _direction;
                     }
                 }
             }
         });
             */
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_BackgroundControl.SearchTextBox.Text))
            {
                _BackgroundControl.SearchTextBox.Text = "search";
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _BackgroundControl.SearchTextBox.Text = string.Empty;
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string serchedText = (sender as TextBox).Text.ToLower();

            foreach (SearchIndex searchIndex in SearchIndices)
            {
                if (searchIndex.Index.ToLower().IndexOf(serchedText) != -1)
                {
                    if (searchIndex.RelatedValueControl.GetType() == typeof(ValueControl))
                    {
                        (searchIndex.RelatedValueControl as ValueControl).Focused = true;
                    }
                    else
                    if (searchIndex.RelatedValueControl.GetType() == typeof(PresureValueControl))
                    {
                        (searchIndex.RelatedValueControl as PresureValueControl).Focused = true;
                    }
                    else
                    if (searchIndex.RelatedValueControl.GetType() == typeof(TemperatureValueControl))
                    {
                        (searchIndex.RelatedValueControl as TemperatureValueControl).Focused = true;
                    }
                    break;
                }
            }
        }

    }
}
