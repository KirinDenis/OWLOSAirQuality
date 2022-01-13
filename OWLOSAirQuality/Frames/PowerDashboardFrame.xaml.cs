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
using System.Windows.Media;
using System.Windows.Shapes;

namespace OWLOSAirQuality.Frames
{
    public partial class PowerDashboardFrame : Window
    {
        private readonly OWLOSEcosystemServiceClient EcosystemServiceClient;
        private bool SensorsJoined = false;

        private Random random = new Random();
        private double Direction = 10.0f;
        private double Speed = 4.0f;

        private readonly List<SearchIndex> SearchIndices = new List<SearchIndex>();

        private bool timerBusy = false;

        private OWLOSEcosystemMQTT _OWLOSEcosystemMQTT;

        private readonly ConsoleControl logConsole;
        public PowerDashboardFrame(OWLOSEcosystemServiceClient EcosystemServiceClient)
        {
            InitializeComponent();

            this.EcosystemServiceClient = EcosystemServiceClient;
            EcosystemServiceClient.OnACDataReady += Ecosystem_OnACDataReady;

            // ValueGraph.GraphPath.Fill = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha3"];
            ValueGraph1.GraphPath.Fill = new LinearGradientBrush((Color)App.Current.Resources["OWLOSInfoColorAlpha3"], (Color)App.Current.Resources["OWLOSInfoColorAlpha4"], new Point(0.5, 0), new Point(0.5, 1));
            ValueGraph2.GraphPath.Fill = new LinearGradientBrush((Color)App.Current.Resources["OWLOSInfoColorAlpha3"], (Color)App.Current.Resources["OWLOSInfoColorAlpha4"], new Point(0.5, 0), new Point(0.5, 1));
            ValueGraph3.GraphPath.Fill = new LinearGradientBrush((Color)App.Current.Resources["OWLOSWarningColorAlpha3"], (Color)App.Current.Resources["OWLOSWarningColorAlpha4"], new Point(0.5, 0), new Point(0.5, 1));
            ValueGraph4.GraphPath.Fill = new LinearGradientBrush((Color)App.Current.Resources["OWLOSDangerColorAlpha3"], (Color)App.Current.Resources["OWLOSDangerColorAlpha4"], new Point(0.5, 0), new Point(0.5, 1));

            Timer lifeCycleTimer = new Timer(10000)
            {
                AutoReset = true
            };
            lifeCycleTimer.Elapsed += new ElapsedEventHandler(OnLifeCycleTimer);
            lifeCycleTimer.Start();
            OnLifeCycleTimer(null, null);


            Timer lifeCycleTimer2 = new Timer(10000)
            {
                AutoReset = true
            };
            lifeCycleTimer2.Elapsed += LifeCycleTimer2_Elapsed;
            lifeCycleTimer2.Start();

            _OWLOSEcosystemMQTT = new OWLOSEcosystemMQTT();
            _OWLOSEcosystemMQTT.OnPropertyValueChangeChanged += _OWLOSEcosystemMQTT_OnPropertyValueChangeChanged;

            Lamp1ActuatorControl.OnChange += Lamp1ActuatorControl_OnChange;
            Lamp2ActuatorControl.OnChange += Lamp2ActuatorControl_OnChange;
            Lamp3ActuatorControl.OnChange += Lamp3ActuatorControl_OnChange;

            this.EcosystemServiceClient = EcosystemServiceClient;
            logConsole = new ConsoleControl();
            LogGrid.Children.Add(logConsole);
            EcosystemServiceClient.OnLog += EcosystemServiceClient_OnLog;
            _OWLOSEcosystemMQTT.OnLog += EcosystemServiceClient_OnLog;
        }

        private void EcosystemServiceClient_OnLog(object sender, OWLOSLogEventArgs e)
        {
            logConsole.AddToconsole(e.Message, e.EventType);
        }

        private void Lamp1ActuatorControl_OnChange(object sender, bool e)
        {
            Lamp1ActuatorControl.Status = _OWLOSEcosystemMQTT.SetLamp(0, e);
        }

        private void Lamp2ActuatorControl_OnChange(object sender, bool e)
        {
            Lamp1ActuatorControl.Status = _OWLOSEcosystemMQTT.SetLamp(1, e);
        }

        private void Lamp3ActuatorControl_OnChange(object sender, bool e)
        {
            Lamp1ActuatorControl.Status = _OWLOSEcosystemMQTT.SetLamp(2, e);
        }

        private void _OWLOSEcosystemMQTT_OnPropertyValueChangeChanged(object sender, ValueEventArgs e)
        {
            bool value = (sender as Lamp).on == 1 ? true : false;

            switch ((sender as Lamp).lamp)
            {
                case 0:
                    Lamp1ActuatorControl.Status = OWLOSThingsManager.Ecosystem.OWLOS.NetworkStatus.Online;
                    Lamp1ActuatorControl.On = (sender as Lamp).on == 1 ? true : false;
                    break;
                case 1:
                    Lamp2ActuatorControl.Status = OWLOSThingsManager.Ecosystem.OWLOS.NetworkStatus.Online;
                    Lamp2ActuatorControl.On = (sender as Lamp).on == 1 ? true : false;
                    break;
                case 2:
                    Lamp3ActuatorControl.Status = OWLOSThingsManager.Ecosystem.OWLOS.NetworkStatus.Online;
                    Lamp3ActuatorControl.On = (sender as Lamp).on == 1 ? true : false;
                    break;
            }
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
                            acData.OnDHT22tempChanged += DHT22TemperatureValueControl.OnValueChanged;

                            acData.OnBMP280temperatureChanged += BMP280TemperatureControl.OnValueChanged;
                            acData.OnBMP280temperatureChanged += BMP280TemperatureValueControl.OnValueChanged;

                            acData.OnCCS811tempChanged += CCS811TemperatureControl.OnValueChanged;
                            acData.OnCCS811tempChanged += CCS811TemperatureValueControl.OnValueChanged;


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

            base.Dispatcher.Invoke(() =>
            {

                if (float.IsNaN((float)KW1Control.OriginalValue))
                {
                    KW1Control.OriginalValue = 0;
                    KW1ValueControl.OnValueChanged(null, new ValueEventArgs((float)0));
                }
                //--- TEMP
                if (Speed > 0)
                {
                    if (Direction > (float)KW1Control.OriginalValue)
                    {
                        KW1Control.OnValueChanged(null, new ValueEventArgs((float)(KW1Control.OriginalValue + Speed) + 150));
                        KW1ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW1Control.OriginalValue + Speed) + 150));
                    }
                    else
                    {
                        KW1Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                        KW1ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                        float _direction = random.Next(100);
                        if (_direction > Direction)
                        {
                            Speed = random.Next(100) / 20.0f + 1; //0..1
                            Direction = _direction;
                        }
                        else
                        {
                            Speed = -(random.Next(100) / 20.0f) - 1; //0..1
                            Direction = _direction;
                        }

                    }
                }
                else
                {
                    if (Direction > (float)KW1Control.OriginalValue)
                    {
                        KW1Control.OnValueChanged(null, new ValueEventArgs((float)(KW1Control.OriginalValue + Speed) + 150));
                        KW1ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW1Control.OriginalValue + Speed) + 150));
                    }
                    else
                    {
                        KW1Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                        KW1ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                        float _direction = random.Next(100);
                        if (_direction > Direction)
                        {
                            Speed = random.Next(100) / 20.0f + 1; //0..1
                            Direction = _direction;
                        }
                        else
                        {
                            Speed = -(random.Next(200) / 10.0f) - 1; //0..1
                            Direction = _direction;
                        }
                    }
                }
            });
            //Second
            if (float.IsNaN((float)KW2Control.OriginalValue))
            {
                KW2Control.OriginalValue = 0;
                KW2ValueControl.OnValueChanged(null, new ValueEventArgs((float)0));
            }
            //--- TEMP
            if (Speed > 0)
            {
                if (Direction > (float)KW2Control.OriginalValue)
                {
                    KW2Control.OnValueChanged(null, new ValueEventArgs((float)(KW2Control.OriginalValue + Speed) + 150));
                    KW2ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW2Control.OriginalValue + Speed) + 150));
                }
                else
                {
                    KW2Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    KW2ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    float _direction = random.Next(100);
                    if (_direction > Direction)
                    {
                        Speed = random.Next(100) / 20.0f + 1; //0..1
                        Direction = _direction;
                    }
                    else
                    {
                        Speed = -(random.Next(100) / 20.0f) - 1; //0..1
                        Direction = _direction;
                    }

                }
            }
            else
            {
                if (Direction > (float)KW2Control.OriginalValue)
                {
                    KW2Control.OnValueChanged(null, new ValueEventArgs((float)(KW2Control.OriginalValue + Speed) + 150));
                    KW2ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW2Control.OriginalValue + Speed) + 150));
                }
                else
                {
                    KW2Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    KW2ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    float _direction = random.Next(100);
                    if (_direction > Direction)
                    {
                        Speed = random.Next(100) / 20.0f + 1; //0..1
                        Direction = _direction;
                    }
                    else
                    {
                        Speed = -(random.Next(200) / 10.0f) - 1; //0..1
                        Direction = _direction;
                    }
                }
            }
            //Thirty 
            if (float.IsNaN((float)KW3Control.OriginalValue))
            {
                KW3Control.OriginalValue = 0;
                KW3ValueControl.OnValueChanged(null, new ValueEventArgs((float)0));
            }
            //--- TEMP
            if (Speed > 0)
            {
                if (Direction > (float)KW3Control.OriginalValue)
                {
                    KW3Control.OnValueChanged(null, new ValueEventArgs((float)(KW3Control.OriginalValue + Speed) + 150));
                    KW3ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW3Control.OriginalValue + Speed) + 150));
                }
                else
                {
                    KW3Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    KW3ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    float _direction = random.Next(100);
                    if (_direction > Direction)
                    {
                        Speed = random.Next(100) / 20.0f + 1; //0..1
                        Direction = _direction;
                    }
                    else
                    {
                        Speed = -(random.Next(100) / 20.0f) - 1; //0..1
                        Direction = _direction;
                    }

                }
            }
            else
            {
                if (Direction > (float)KW3Control.OriginalValue)
                {
                    KW3Control.OnValueChanged(null, new ValueEventArgs((float)(KW3Control.OriginalValue + Speed) + 150));
                    KW3ValueControl.OnValueChanged(null, new ValueEventArgs((float)(KW3Control.OriginalValue + Speed) + 150));
                }
                else
                {
                    KW3Control.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    KW3ValueControl.OnValueChanged(null, new ValueEventArgs((float)Direction + 150));
                    float _direction = random.Next(100);
                    if (_direction > Direction)
                    {
                        Speed = random.Next(100) / 20.0f + 1; //0..1
                        Direction = _direction;
                    }
                    else
                    {
                        Speed = -(random.Next(200) / 10.0f) - 1; //0..1
                        Direction = _direction;
                    }
                }
            }


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

        private async void OnLifeCycleTimer(object source, ElapsedEventArgs e)
        {
            if (timerBusy)
            {
                return;
            }
            timerBusy = true;

            if (EcosystemServiceClient == null)
            {
                timerBusy = false;
                return;
            }

            ThingAirQualityHistoryData thingAirQualities;
            thingAirQualities = EcosystemServiceClient.GetOneHourData(0);

            base.Dispatcher.Invoke(() =>
            {
                ValueGraph1.Update(thingAirQualities.DHT22temp, thingAirQualities.QueryTime, thingAirQualities.Statuses);
                ValueGraph2.Update(thingAirQualities.DHT22hum, thingAirQualities.QueryTime, thingAirQualities.Statuses);
                ValueGraph3.Update(thingAirQualities.BMP280pressure, thingAirQualities.QueryTime, thingAirQualities.Statuses);
                ValueGraph4.Update(thingAirQualities.CCS811CO2, thingAirQualities.QueryTime, thingAirQualities.Statuses);
                ValueGraph5.Update(thingAirQualities.BMP280temperature, thingAirQualities.QueryTime, thingAirQualities.Statuses);
                ValueGraph6.Update(thingAirQualities.ADS1X15MQ135, thingAirQualities.QueryTime, thingAirQualities.Statuses);

            });
            timerBusy = false;
        }
    }
}
