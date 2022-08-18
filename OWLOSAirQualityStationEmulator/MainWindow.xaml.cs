using OWLOSAirQaulityUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OWLOSAirQualityStationEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
          
         
         
         */
        private async void sendData_Click(object sender, RoutedEventArgs e)
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://localhost:43043/Data/AirQuality");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));

                    CSVConvert csv = new CSVConvert();



                    string data = csv.Deserialise(csv.Serialise(""));

                    HttpResponseMessage responseClient = await client.PostAsync("http://localhost:43043/Data/AirQuality", new StringContent("Hello", Encoding.UTF8, "text/csv"));

                    string response = await responseClient.Content.ReadAsStringAsync();

                }
                catch
                {
                }
            }

        }

    }
}
