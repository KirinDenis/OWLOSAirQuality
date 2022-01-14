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

using OWLOSAirQuality.Huds;
using OWLOSAirQuality.OWLOSEcosystemService;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace OWLOSAirQuality.Frames
{
    /// <summary>
    /// Interaction logic for StationEmulatorControl.xaml
    /// </summary>
    public partial class StationEmulatorFrame : Window
    {
        protected OWLOSEcosystemManager EcosystemManager;

        private readonly ConsoleControl logConsole;

        private bool timerBusy = false;

        protected string connectionURL = string.Empty;

        protected string userToken = string.Empty;

        protected int queryInterval = 10;
        public StationEmulatorFrame()
        {
            InitializeComponent();

            this.EcosystemManager = App.EcosystemManager;

            logConsole = new ConsoleControl();
            LogGrid.Children.Add(logConsole);

            ApplyConnectionSettings();

            Timer lifeCycleTimer = new Timer(queryInterval * 1000)
            {
                AutoReset = true
            };
            lifeCycleTimer.Elapsed += new ElapsedEventHandler(OnLifeCycleTimer);
            lifeCycleTimer.Start();
            OnLifeCycleTimer(null, null);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ApplyConnectionSettings()
        {
            connectionURL = EcosystemURLTextBox.Text;

            userToken = UserTokenTextBox.Text;

            if (!int.TryParse(QueryIntervalTextBox.Text, out queryInterval))
            {
                queryInterval = 10;
                QueryIntervalTextBox.Text = queryInterval.ToString();
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyConnectionSettings();
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
        {
            // It is possible inpect the certificate provided by server
            //   Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            //   Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
            //   Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
            //    Console.WriteLine($"Issuer: {certificate.Issuer}");
            //
            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
            //     Console.WriteLine($"Errors: {sslErrors}");
            return true; // sslErrors == SslPolicyErrors.None;
        }

        protected async Task<string> PostAirQualityData(string OWLOSEcosystemHost, string airQualityData)
        {
            try
            {

                // Create an HttpClientHandler object and set to use default credentials
                HttpClientHandler handler = new HttpClientHandler
                {

                    // Set custom server validation callback
                    ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation
                };

                HttpClient client = new HttpClient(handler);

                HttpResponseMessage response = client.PostAsync(OWLOSEcosystemHost, new StringContent(airQualityData)).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return OWLOSEcosystemHost + " " + response.StatusCode.ToString();
            }
            catch (Exception exception)
            {
                return OWLOSEcosystemHost + " " + exception.Message;
            }
        }


        private async void OnLifeCycleTimer(object source, ElapsedEventArgs e)
        {
            if (timerBusy)
            {
                return;
            }
            timerBusy = true;

            string airQualityDate = "topic:owlos/owlthingb0c40a24/AirQuality\n" +
"token:ed3f0fc7-d7ec-42dc-a3fe-a64131d0c788\n" +
"airqualityonly:1\n" +
"TickCount:267993692\n" +
"DHT22:yes\n" +
"DHT22temp:nan\n" +
"DHT22tempHD:30;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;\n" +
"DHT22hum:nan\n" +
"DHT22humHD:30;24.50;24.50;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.50;24.50;24.40;24.50;24.40;24.50;24.40;24.40;24.40;24.40;24.40;24.30;24.30;24.40;24.50;24.40;24.30;\n" +
"DHT22heat:nan\n" +
"DHT22heatHD:30;26.74;26.74;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.74;26.74;26.73;26.74;26.73;26.74;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.74;26.73;26.73;\n" +
"DHT22c:1\n" +
"BMP280:yes\n" +
"BMP280pressure:100165.72\n" +
"BMP280pressureHD:30;100171.65;100172.31;100173.58;100174.45;100176.05;100173.52;100174.58;100175.63;100171.72;100173.81;100176.37;100174.12;100171.69;100171.14;100170.83;100170.27;100165.87;100169.10;100169.84;100169.00;100169.02;100170.72;100171.36;100171.12;100169.63;100168.89;100166.50;100168.64;100168.87;100165.72;\n" +
"BMP280altitude:96.97\n" +
"BMP280altitudeHD:30;96.47;96.41;96.31;96.23;96.10;96.31;96.22;96.14;96.46;96.29;96.07;96.26;96.47;96.51;96.54;96.59;96.96;96.68;96.62;96.69;96.69;96.55;96.49;96.51;96.64;96.70;96.90;96.72;96.70;96.97;\n" +
"BMP280temperature:29.26\n" +
"BMP280temperatureHD:30;29.14;29.14;29.14;29.15;29.17;29.18;29.14;29.12;29.14;29.14;29.12;29.11;29.15;29.17;29.19;29.20;29.17;29.16;29.18;29.19;29.21;29.22;29.23;29.23;29.24;29.27;29.27;29.28;29.28;29.26;\n" +
"ADS1X15:yes\n" +
"ADS1X15MQ135:1\n" +
"ADS1X15MQ135HD:30;0.00;0.00;0.00;0.00;0.00;1.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;1.00;0.00;1.00;0.00;0.00;0.00;0.00;1.00;0.00;0.00;0.00;1.00;\n" +
"ADS1X15MQ7:2712\n" +
"ADS1X15MQ7HD:30;2718.00;2718.00;2717.00;2719.00;2720.00;2717.00;2721.00;2718.00;2719.00;2716.00;2718.00;2719.00;2719.00;2717.00;2715.00;2715.00;2714.00;2718.00;2717.00;2716.00;2715.00;2713.00;2717.00;2715.00;2715.00;2717.00;2712.00;2712.00;2712.00;2712.00;\n" +
"ADS1X15Light:-1\n" +
"ADS1X15LightHD:30;-1.00;-1.00;-1.00;-1.00;-1.00;-2.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-2.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;\n" +
"CCS811:yes\n" +
"CCS811CO2:2132\n" +
"CCS811CO2HD:30;2473.00;2467.00;2467.00;2467.00;2487.00;2488.00;2488.00;2495.00;2495.00;2495.00;2495.00;2495.00;2488.00;2481.00;2461.00;2436.00;2400.00;2380.00;2349.00;2330.00;2311.00;2294.00;2280.00;2257.00;2245.00;2174.00;2166.00;2162.00;2150.00;2138.00;\n" +
"CCS811TVOC:990\n" +
"CCS811TVOCHD:30;1718.00;1708.00;1708.00;1708.00;1748.00;1758.00;1758.00;1773.00;1773.00;1773.00;1773.00;1773.00;1758.00;1738.00;1688.00;1643.00;1572.00;1523.00;1462.00;1419.00;1385.00;1347.00;1310.00;1262.00;1234.00;1084.00;1067.00;1049.00;1027.00;999.00;\n" +
"CCS811resistence:0.00\n" +
"CCS811resistenceHD:30;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;\n" +
"CCS811temp:-273.15\n" +
"CCS811tempHD:30;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;\n";

            string response = PostAirQualityData(connectionURL + "/Things/AirQuality", airQualityDate).Result;

            base.Dispatcher.Invoke(() =>
            {
                logConsole.AddToconsole(response, ConsoleMessageCode.Success);
            });

            timerBusy = false;
        }

    }
}