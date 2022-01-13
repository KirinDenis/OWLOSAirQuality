using OWLOSAirQuality.Huds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OWLOSAirQuality.Frames
{
    /// <summary>
    /// Interaction logic for StationEmulatorControl.xaml
    /// </summary>
    public partial class StationEmulatorFrame : Window
    {
        private readonly ConsoleControl logConsole;

        private bool timerBusy = false; 
        public StationEmulatorFrame()
        {
            InitializeComponent();

            logConsole = new ConsoleControl();
            LogGrid.Children.Add(logConsole);

            Timer lifeCycleTimer = new Timer(1000)
            {
                AutoReset = true
            };
            lifeCycleTimer.Elapsed += new ElapsedEventHandler(OnLifeCycleTimer);
            lifeCycleTimer.Start();
            OnLifeCycleTimer(null, null);
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
                HttpClientHandler handler = new HttpClientHandler();

                // Set custom server validation callback
                handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

                HttpClient client = new HttpClient(handler);                

                HttpResponseMessage response = client.PostAsync(OWLOSEcosystemHost, new StringContent(airQualityData)).GetAwaiter().GetResult(); 

                response.EnsureSuccessStatusCode();

                string  responseBody = await response.Content.ReadAsStringAsync();

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

               string airQualityDate =  "topic:owlos/owlthingb0c40a24/AirQuality" +
"token:ed3f0fc7-d7ec-42dc-a3fe-a64131d0c788" +
"airqualityonly:1" +
"TickCount:267993692" +
"DHT22:yes" +
"DHT22temp:nan" +
"DHT22tempHD:30;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;27.80;" +
"DHT22hum:nan" +
"DHT22humHD:30;24.50;24.50;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.40;24.50;24.50;24.40;24.50;24.40;24.50;24.40;24.40;24.40;24.40;24.40;24.30;24.30;24.40;24.50;24.40;24.30;" +
"DHT22heat:nan" +
"DHT22heatHD:30;26.74;26.74;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.74;26.74;26.73;26.74;26.73;26.74;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.73;26.74;26.73;26.73;" +
"DHT22c:1" +
"BMP280:yes" +
"BMP280pressure:100165.72" +
"BMP280pressureHD:30;100171.65;100172.31;100173.58;100174.45;100176.05;100173.52;100174.58;100175.63;100171.72;100173.81;100176.37;100174.12;100171.69;100171.14;100170.83;100170.27;100165.87;100169.10;100169.84;100169.00;100169.02;100170.72;100171.36;100171.12;100169.63;100168.89;100166.50;100168.64;100168.87;100165.72;" +
"BMP280altitude:96.97" +
"BMP280altitudeHD:30;96.47;96.41;96.31;96.23;96.10;96.31;96.22;96.14;96.46;96.29;96.07;96.26;96.47;96.51;96.54;96.59;96.96;96.68;96.62;96.69;96.69;96.55;96.49;96.51;96.64;96.70;96.90;96.72;96.70;96.97;" +
"BMP280temperature:29.26" +
"BMP280temperatureHD:30;29.14;29.14;29.14;29.15;29.17;29.18;29.14;29.12;29.14;29.14;29.12;29.11;29.15;29.17;29.19;29.20;29.17;29.16;29.18;29.19;29.21;29.22;29.23;29.23;29.24;29.27;29.27;29.28;29.28;29.26;" +
"ADS1X15:yes" +
"ADS1X15MQ135:1" +
"ADS1X15MQ135HD:30;0.00;0.00;0.00;0.00;0.00;1.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;1.00;0.00;1.00;0.00;0.00;0.00;0.00;1.00;0.00;0.00;0.00;1.00;" +
"ADS1X15MQ7:2712" +
"ADS1X15MQ7HD:30;2718.00;2718.00;2717.00;2719.00;2720.00;2717.00;2721.00;2718.00;2719.00;2716.00;2718.00;2719.00;2719.00;2717.00;2715.00;2715.00;2714.00;2718.00;2717.00;2716.00;2715.00;2713.00;2717.00;2715.00;2715.00;2717.00;2712.00;2712.00;2712.00;2712.00;" +
"ADS1X15Light:-1" +
"ADS1X15LightHD:30;-1.00;-1.00;-1.00;-1.00;-1.00;-2.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-2.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;-1.00;" +
"CCS811:yes" +
"CCS811CO2:2132" +
"CCS811CO2HD:30;2473.00;2467.00;2467.00;2467.00;2487.00;2488.00;2488.00;2495.00;2495.00;2495.00;2495.00;2495.00;2488.00;2481.00;2461.00;2436.00;2400.00;2380.00;2349.00;2330.00;2311.00;2294.00;2280.00;2257.00;2245.00;2174.00;2166.00;2162.00;2150.00;2138.00;" +
"CCS811TVOC:990" +
"CCS811TVOCHD:30;1718.00;1708.00;1708.00;1708.00;1748.00;1758.00;1758.00;1773.00;1773.00;1773.00;1773.00;1773.00;1758.00;1738.00;1688.00;1643.00;1572.00;1523.00;1462.00;1419.00;1385.00;1347.00;1310.00;1262.00;1234.00;1084.00;1067.00;1049.00;1027.00;999.00;" +
"CCS811resistence:0.00" +
"CCS811resistenceHD:30;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;0.00;" +
"CCS811temp:-273.15" +
"CCS811tempHD:30;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;-273.15;";

                string response = PostAirQualityData("https://192.168.1.100:5004/", airQualityDate).Result;



            base.Dispatcher.Invoke(() =>
            {
                logConsole.AddToconsole(response, ConsoleMessageCode.Success);
            });

            timerBusy = false;
        }
    }
}
