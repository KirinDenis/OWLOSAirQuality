# OWLOS Air Quality

![OWLOS Air Quality PCB](https://github.com/KirinDenis/OWLOSAirQuality/raw/main/OWLOSResource/images/OWLOSPCBImage.jpg)

### Open Source IoT Solution for monitoring air quality based on ![OWLOS](https://github.com/KirinDenis/owlos)
 
[We on Facebook](https://www.facebook.com/groups/OWLOS)

### Used hardware: 
- ESP32 microcontroller
- ILI9486 3.5" TFT LCD touch screen
- TP4056
 - linear charger for single cell
- CCS811 
  -- eCO2, TVOC, Resistance levels sensor
- SI7021 
  -- temperature and humidity sensor 
- BMP280 (or BME680)
-- absolute barometric pressure sensor
- ADS1X15
-- 12 and 16-bit ADC for photoresistor, MQ7, MQ135
- DHT22 (or DHT11 or AM2301)
-- temperature, humidity and heat index sensor
- Photoresistor
-- light level sensor 
- MQ7 
-- different gases contains CO sensor
- MQ135
-- gas sensor (ammonia gas), sulfide, benzene series steam, smoke and other toxic gases sensor

### SCH+PCB+3D+BOM
We have published the PCB of OWLOS Air Quality based on ESP32 ([Click here to view online](https://365.altium.com/files/A92F63A8-C7F4-40DA-98BB-F1BCB85EE9DF)).

We discuss the results of the assembly and use of this PCB at our [Facebook](https://www.facebook.com/groups/OWLOS) community. 

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


