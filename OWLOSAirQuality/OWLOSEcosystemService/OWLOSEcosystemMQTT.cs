using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using OWLOSAirQuality.Huds;
using OWLOSEcosystemService.DTO.Things;
using OWLOSThingsManager.Ecosystem.OWLOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace OWLOSAirQuality.OWLOSEcosystemService
{
    /// <summary>
    /// Temporary for testing 
    /// </summary>
    public class Lamp
    {
        public int lamp { get; set; }
        public int on { get; set; }
    }

    public class OWLOSEcosystemMQTT
    {
        /// <summary>
        /// Temporary
        /// </summary>
        public List<Lamp> GetLamps = new List<Lamp>();

        private IManagedMqttClient _mqttClient;

        private string Topic = "v1_nohardware/devices/me/telemetry/lamps/";

        protected NetworkStatus _Status = NetworkStatus.Offline;

        public delegate void OWLOSLogEventHandler(object? sender, OWLOSLogEventArgs e);
        public event OWLOSLogEventHandler OnLog;

        public delegate void OnPropertyValueChangeEventHandler(object? sender, ValueEventArgs e);
        public event OnPropertyValueChangeEventHandler OnPropertyValueChangeChanged;

        public OWLOSEcosystemMQTT()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                    .WithClientId("OWLOSUX" + Environment.TickCount.ToString())
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

                        //text.Text += "Topic: " + topic + ". Message Received: " + payload + "\n";

                        //Status 
                        //if (topic.Equals(Topic + "status"))
                       // {
                        //    _mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
                       // }
                        //Get 
                        if (topic.Equals(Topic + "set"))
                        {
                            //Lamps = JsonConvert.DeserializeObject<List<Lamp>>(payload);
                            //_mqttClient.PublishAsync(Topic + "get", JsonConvert.SerializeObject(Lamps));
                        }
                        //Set
                        if (topic.Equals(Topic + "get"))
                        {
                            GetLamps = JsonConvert.DeserializeObject<List<Lamp>>(payload);

                            foreach (Lamp lamp in GetLamps)
                            {
                                OnPropertyValueChangeChanged?.Invoke(lamp, new ValueEventArgs(lamp.on));
                                string lampStatus = (lamp.on == 1) ? "ON" : "OFF";
                                Log(new OWLOSLogEventArgs("Lamp " + lamp.lamp.ToString() + " " + lampStatus, ConsoleMessageCode.Info));
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    _Status = NetworkStatus.Erorr;
                }
            });

            _mqttClient.StartAsync(options).GetAwaiter().GetResult();
        }

        public NetworkStatus SetLamp(int lampId, bool value)
        {
            try
            {
                if (_mqttClient.IsConnected)
                {
                    if ((GetLamps != null) && (GetLamps.Count > lampId))
                    {
                        GetLamps[lampId].on = value ? 1 : 0;
                        _mqttClient.PublishAsync(Topic + "set", JsonConvert.SerializeObject(GetLamps));
                    }
                    else
                    {
                        return NetworkStatus.Erorr;
                    }
                 
                }
                else
                {
                    return NetworkStatus.Offline;
                }
            }
            catch
            {
                return NetworkStatus.Erorr;
            }
            return NetworkStatus.Reconnect;
        }

        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _Status = NetworkStatus.Online;
            _mqttClient.PublishAsync(Topic + "status", DateTime.Now.ToString());
            Log(new OWLOSLogEventArgs("MQTT connected", ConsoleMessageCode.Success));
        }


        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            _Status = NetworkStatus.Erorr;
            Log(new OWLOSLogEventArgs("MQTT failed", ConsoleMessageCode.Danger));
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            _Status = NetworkStatus.Offline;
            Log(new OWLOSLogEventArgs("MQTT disconnect", ConsoleMessageCode.Warning));
        }

        protected virtual void Log(OWLOSLogEventArgs e)
        {
            OnLog?.Invoke(this, e);
        }

    }
}
