![OWLOS UX](https://github.com/KirinDenis/owlos/raw/master/OWLOSResource/images/OWLOSAirQualityLogo.jpg)

# OWLOS Air Quality
### DIY Open Source OS for building IoT ecosystems
[ESP32]
- built-in sensors, actuators, LCDs, DHTs, Steppers and other drivers
- built-in script language interpreter 
- built-in HTTP(S) RESTful server, RESTful client, MQTT client
- WiFi access point/station modes
- OTA supported 
- UART AT+ interface supported
- flexible assembly configuration (config.h)
- Web and desktop UX
- doesn't require programming skills
- doesn't require internet and additional servers
- Open Source under GPL-3.0 License 

![OWLOS SCHEME](https://github.com/KirinDenis/owlos/raw/master/OWLOSResource/images/owlos-scheme.png)

### Source Code map

- **/OWLOS/** C/C++ firmware source code (ESP32, ESP8266)
- **/OWLOS/data/** JavaScript  stand-alone and embedded UX 
- **/OWLOSAdmin/** .Net Core C# WPF UX
- **/OWLOSEcosystem/** (FFR) .Net Core C# WPF + OpenGL UX
- **/OWLOSResource/** Blendar 3D models, schematics and images resources
- **/OWLOSStarter/** C/C++ for uploading OWLOS firmware bin from GitHub (HTTP Updater)

### How to build

1. install [PlatformIO IDE](https://platformio.org/)
2. install COM port drivers for your board
3. in PlatformIO open OWLOS workspace 
4. setup your build configuration in config.h file
5. build and upload OWLOS firmware to your board

[**More detailed**](https://github.com/KirinDenis/owlos/wiki/How-to-install-EN) instruction here


### Special thanks to

- [PlatformIO IDE](https://platformio.org/)
- [ESP32 Arduino Core](https://github.com/espressif/arduino-esp32)
- [me-no-dev/AsyncTCP (ESP32)](https://github.com/me-no-dev/AsyncTCP)
- [ESP32 HTTPS Server](https://github.com/fhessel/esp32_https_server)
- [Async MQTT Client](http://platformio.org/lib/show/346/AsyncMqttClient)
- [DS3231 Real-Time Clock](http://www.jarzebski.pl/arduino/komponenty/zegar-czasu-rzeczywistego-rtc-ds3231.html)
- [Adafruit Unified Sensor Library](https://github.com/adafruit/Adafruit_Sensor)
- [DHT-sensor-library](https://github.com/adafruit/DHT-sensor-library)
- [LiquidCrystal_I2C](https://gitlab.com/tandembyte/liquidcrystal_i2c)	

### Copyright 2019, 2020, 2021 by

- Mónica (rovt@ua.fm)
- Yan Sokolov (Dadras279@gmail.com)
- Ddone Deedone (https://techadv.xyz/)
- Serhii Demyanov (demianfog@gmail.com)
- Serhii Lehkii (sergey@light.kiev.ua)
- Konstantin Brul (konstabrul@gmail.com)
- Vitalii Glushchenko (cehoweek@gmail.com)
- Stanislav Kvashchuk (skat@ukr.net)
- Vladimir Kovalevich (covalevich@gmail.com)
- Boris Pavlov (hiroyashy@gmail.com)
- Denys Melnychuk (meldenvar@gmail.com)
- Denis Kirin (deniskirinacs@gmail.com)

[We on Facebook](https://www.facebook.com/groups/OWLOS)
