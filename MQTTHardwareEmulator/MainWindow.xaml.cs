using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WpfApp1
{
    public class Lamp
    {
        public int lamp { get; set; }
        public int on { get; set; }
    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected List<Lamp> Lamps = new List<Lamp>();

        private IManagedMqttClient _mqttClient;

        private string Topic = "v1_nohardware/devices/me/telemetry/lamps/";
        public MainWindow()
        {
            InitializeComponent();

            Lamps.Add(new Lamp()
            {
                lamp = 0,
                on = 0
            });

            Lamps.Add(new Lamp()
            {
                lamp = 1,
                on = 0
            });

            Lamps.Add(new Lamp()
            {
                lamp = 2,
                on = 0
            });

            Lamps.Add(new Lamp()
            {
                lamp = 3,
                on = 0
            });


            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                    .WithClientId("OWLOSUX")
                                                    .WithTcpServer("owlos.org", 1883);


            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                    .WithClientOptions(builder.Build())
                                    .Build();


            _mqttClient = new MqttFactory().CreateManagedMqttClient();


            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
            
            _mqttClient.SubscribeAsync(Topic + "status");
            _mqttClient.SubscribeAsync(Topic + "set");
            _mqttClient.SubscribeAsync(Topic + "get");

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        base.Dispatcher.Invoke(() =>
                        {
                            text.Text += "Topic: " + topic + ". Message Received: " + payload + "\n";

                            if (topic.Equals(Topic + "status"))
                            {
                                _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
                            }
                            if (topic.Equals(Topic + "set"))
                            {
                                Lamps = JsonConvert.DeserializeObject<List<Lamp>>(payload);
                                _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
                            }
                            if (topic.Equals(Topic + "get"))
                            {
                                Lamps = JsonConvert.DeserializeObject<List<Lamp>>(payload);

                                Lamp1Text.Text = Lamps[0].on.ToString();
                                Lamp2Text.Text = Lamps[1].on.ToString();

                                GLamp1Text.Text = Lamps[0].on.ToString();
                                GLamp2Text.Text = Lamps[1].on.ToString();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    base.Dispatcher.Invoke(() =>
                    {
                        text.Text += ex.Message + "\n";
                    });
                }
            });

            _mqttClient.StartAsync(options).GetAwaiter().GetResult();
        }


        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            base.Dispatcher.Invoke(() =>
            {
                text.Text += "Successfully connected.\n";                
            });
            
        }

        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            base.Dispatcher.Invoke(() =>
            {
                text.Text += "Couldn't connect to broker.\n";
            });
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            base.Dispatcher.Invoke(() =>
            {
                text.Text += "Successfully disconnected.\n";
            });
        }
//SET
        private void bl1on_Click(object sender, RoutedEventArgs e)
        {
            Lamps[0].on = 1;
            _mqttClient.PublishAsync(Topic + "set", JsonConvert.SerializeObject(Lamps));
            Lamp1Text.Text = Lamps[0].on.ToString();
        }

        private void bl1off_Click(object sender, RoutedEventArgs e)
        {
            Lamps[0].on = 0;
            _mqttClient.PublishAsync(Topic + "set", JsonConvert.SerializeObject(Lamps));
            Lamp1Text.Text = Lamps[0].on.ToString();
        }

        private void bl2on_Click(object sender, RoutedEventArgs e)
        {
            Lamps[1].on = 1;
            _mqttClient.PublishAsync(Topic + "set", JsonConvert.SerializeObject(Lamps));
            Lamp2Text.Text = Lamps[1].on.ToString();
        }

        private void bl2off_Click(object sender, RoutedEventArgs e)
        {
            Lamps[1].on = 0;
            _mqttClient.PublishAsync(Topic + "set", JsonConvert.SerializeObject(Lamps));
            Lamp2Text.Text = Lamps[1].on.ToString();
        }
//GET
        private void Gbl1on_Click(object sender, RoutedEventArgs e)
        {            
            _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
        }

        private void Gbl1off_Click(object sender, RoutedEventArgs e)
        {         
            _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
        }

        private void Gbl2on_Click(object sender, RoutedEventArgs e)
        {         
            _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
        }

        private void Gbl2off_Click(object sender, RoutedEventArgs e)
        {            
            _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
        }

        private void bStatus_Click(object sender, RoutedEventArgs e)
        {
            _mqttClient.PublishAsync(Topic + "status", DateTime.Now.ToString());
        }
    }
}
