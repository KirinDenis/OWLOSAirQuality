![OWLOS Air Quality Logo](https://github.com/KirinDenis/OWLOSAirQuality/raw/main/OWLOSResource/images/OWLOSAirQualityLogo.jpg)

# OWLOS Air Quality
### Open Source IoT Solution for monitoring air quality based on ![OWLOS](https://github.com/KirinDenis/owlos)
 
[We on Facebook](https://www.facebook.com/groups/OWLOS)

### Used hardware: 
- ESP32 microcontroller
- CCS811 + SI7021 + BMP280 (or BME680)
- ADS1115
- 3.5" TFT LCD with touch
- DHT22
- Photoresistor 
- MQ7
- MQ135

### OWLOS Air Quality SCH+PCB+3D+BOM
https://365.altium.com/files/A92F63A8-C7F4-40DA-98BB-F1BCB85EE9DF


### Youtube video describing the architecture (click the image)
[![OWLOS Air Quality](https://img.youtube.com/vi/HRcJmzvD9GQ/0.jpg)](https://www.youtube.com/watch?v=HRcJmzvD9GQ)

### Source Code map

- **/OWLOS/** C/C++ firmware source code (ESP32, ESP8266)
- **/OWLOS/data/** JavaScript  stand-alone and embedded UX 
- **/OWLOSEcosystem/** (FFR) .Net Core C# WPF + OpenGL UX
- **/OWLOSResource/** Blendar 3D models, schematics and images resources

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

### Copyright 2019, 2020, 2021, 2022 by

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


