/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2021 by:
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

#include "HomeScreen.h"

#include "../Controls/ButtonControl.h"
#include "../Controls/TextControl.h"
#include "../Controls/ButtonControl.h"

#include "LogScreen.h"
#include "SensorsScreen.h"
#include "TransportScreen.h"

#include "../../services/DriverService.h"
#include "../../services/AirQualityService.h"

extern TFT_eSPI tft;

ButtonControlClass homeButton("home", OWLOSLightColor, OWLOSSecondaryColor, OWLOSWarningColor, 1, 16);
ButtonControlClass sensorsButton("sensors", OWLOSLightColor, OWLOSSecondaryColor, OWLOSWarningColor, 2, 16);
ButtonControlClass transportButton("transport", OWLOSLightColor, OWLOSSecondaryColor, OWLOSWarningColor, 3, 16);
ButtonControlClass logButton("log", OWLOSLightColor, OWLOSSecondaryColor, OWLOSWarningColor, 4, 16);

ButtonControlClass DHTButton("DHT", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 1, 1);
ButtonControlClass pressureButton("Pressure", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 2, 1);
ButtonControlClass CO2Button("CO2::TVOC", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 3, 1);
ButtonControlClass gasButton("Gas::Smoke", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 4, 1);

extern int currentMode;

extern DHTDriver *_DHTDriver;

#ifdef USE_BMP280_DRIVER
extern BMP280Driver *_BMP280Driver;
#endif

#ifdef USE_BME680_DRIVER
extern BME680Driver *_BME680Driver;
#endif

extern ADS1X15Driver *_ADS1X15Driver;
extern CCS811Driver *_CCS811Driver;

#define AA_FONT_SMALL "omegaflight20"
#define AA_FONT_BIG "omegaflight54"
#define AA_FONT_LARGE "omegaflight108"

TextControlClass dhtHomeHeaderItem(0, 0);
TextControlClass dhtHomeTempItem(10, 20, 4);
TextControlClass dhtHomeTempValueItem(1, 1);
TextControlClass dhtHomeHumItem(0, 2);
TextControlClass dhtHomeHumValueItem(1, 2);
TextControlClass dhtHomeHeatItem(0, 3);
TextControlClass dhtHomeHeatValueItem(1, 3);

#define HOME_SENSOR_DHT 0
#define HOME_SENSOR_PRESSURE 1
#define HOME_SENSOR_CO2 2
#define HOME_SENSOR_GAS 3

int currentHomeSensor = HOME_SENSOR_DHT;

String prevTemp = "";
String prevHum = "";
String prevHeat = "";

String prevkPa = "";
String prevmmHg = "";
String prevAltitude = "";
String prevBMxTemp = "";

String bigTextCaption = "";
String bigTextValue = "";
String bigTextValueType = "";

int valueLeftPadding = GOLD_9;

void RefreshHomeScreenButtons()
{

    prevTemp = "";
    prevHum = "";
    prevHeat = "";
    prevkPa = "";
    prevmmHg = "";
    prevAltitude = "";
    prevBMxTemp = "";

    bigTextCaption = "";
    bigTextValue = "";
    bigTextValueType = "";


    homeButton.refresh();
    sensorsButton.refresh();
    transportButton.refresh();
    logButton.refresh();

    switch (currentHomeSensor)
    {
    case HOME_SENSOR_DHT:
        DHTButton.selected = true;
        pressureButton.selected = CO2Button.selected = gasButton.selected = false;
        break;
    case HOME_SENSOR_PRESSURE:
        pressureButton.selected = true;
        DHTButton.selected = CO2Button.selected = gasButton.selected = false;
        break;
    case HOME_SENSOR_CO2:
        CO2Button.selected = true;
        DHTButton.selected = pressureButton.selected = gasButton.selected = false;
        break;
    case HOME_SENSOR_GAS:
        gasButton.selected = true;
        DHTButton.selected = pressureButton.selected = CO2Button.selected = false;
        break;
    }

    DHTButton.refresh();
    pressureButton.refresh();
    CO2Button.refresh();
    gasButton.refresh();
}
// Controls --------------------------------------------------------------------------------
// horisontalPosition - two positions, indexes 0 and 1 - 0 up, 1 down
void BigText(int horisontalPosition, String caption, String value, String valueType, int captionColor = OWLOSInfoColor, int valueColor = OWLOSLightColor, int valueTypeColor = OWLOSInfoColor)
{
    int yStep = GOLD_6 + GOLD_8;
    if (horisontalPosition == 1)
    {
        yStep = HEIGHT / 2;
    }

    tft.loadFont(AA_FONT_SMALL);

    if (caption.compareTo(bigTextCaption) != 0)
    {
        tft.fillRect(valueLeftPadding, yStep, tft.textWidth(bigTextCaption), tft.fontHeight(0), OWLOSDarkColor);
        tft.setTextColor(captionColor, OWLOSDarkColor);
        tft.setCursor(valueLeftPadding, yStep);
        bigTextCaption = caption;
        tft.print(caption);
    }

    yStep += tft.fontHeight(1) + GOLD_8;
    tft.unloadFont();

    if (value.compareTo(bigTextValue) != 0)
    {
        tft.loadFont(AA_FONT_BIG);
        int valueTypeWidth = tft.textWidth(valueType) + GOLD_8;
        tft.unloadFont();

        tft.loadFont(AA_FONT_LARGE);

        tft.fillRect(0, yStep, WIDTH, tft.fontHeight(0), OWLOSDarkColor);

        tft.setTextColor(OWLOSLightColor, OWLOSDarkColor);
        bigTextValue = value;
        int valueLeft = WIDTH / 2 - (tft.textWidth(bigTextValue) / 2 + valueTypeWidth / 2); // recalculate left
        int valueHeight = tft.fontHeight(0);
        tft.setCursor(valueLeft, yStep);
        tft.print(bigTextValue);
        valueLeft += tft.textWidth(bigTextValue) + GOLD_8;
        tft.unloadFont();

        tft.loadFont(AA_FONT_BIG);
        tft.setCursor(valueLeft, yStep + valueHeight - tft.fontHeight(0) - GOLD_10);
        bigTextValueType = valueType;
        tft.print(bigTextValueType);
        tft.unloadFont();
    }
}

//-----------------------------------------------------------------------------------------

void HomeButtonTouch()
{
    if (currentMode != HOME_MODE)
    {
        homeButton.selected = true;
        HomeScreenRefresh();
        sensorsButton.selected = transportButton.selected = logButton.selected = false;
        sensorsButton.refresh();
        transportButton.refresh();
        logButton.refresh();

        RefreshHomeScreenButtons();
        currentMode = HOME_MODE;
    }
}

void SensorButtonTouch()
{
    if (currentMode != SENSORS_MODE)
    {
        sensorsButton.selected = true;
        sensorsScreenRefresh();
        homeButton.selected = transportButton.selected = logButton.selected = false;
        homeButton.refresh();
        transportButton.refresh();
        logButton.refresh();

        currentMode = SENSORS_MODE;
    }
}

void TransportButtonTouch()
{
    if (currentMode != TRANSPORT_MODE)
    {
        transportButton.selected = true;
        transportScreenRefresh();
        homeButton.selected = sensorsButton.selected = logButton.selected = false;
        homeButton.refresh();
        sensorsButton.refresh();
        logButton.refresh();

        currentMode = TRANSPORT_MODE;
    }
}

void LogButtonTouch()
{
    if (currentMode != LOG_MODE)
    {
        logButton.selected = true;
        logScreenRefresh();
        homeButton.selected = sensorsButton.selected = transportButton.selected = false;
        homeButton.refresh();
        sensorsButton.refresh();
        transportButton.refresh();

        currentMode = LOG_MODE;
    }
}

//--------------------------------------------------------------------------------
// Home Screen
void HomeDHTButtonTouch()
{
    if (currentHomeSensor != HOME_SENSOR_DHT)
    {
        tft.fillScreen(OWLOSDarkColor);
        currentHomeSensor = HOME_SENSOR_DHT;
        RefreshHomeScreenButtons();
    }
}

void HomePressureButtonTouch()
{
    if (currentHomeSensor != HOME_SENSOR_PRESSURE)
    {
        tft.fillScreen(OWLOSDarkColor);
        currentHomeSensor = HOME_SENSOR_PRESSURE;
        RefreshHomeScreenButtons();
    }
}

void HomeCO2ButtonTouch()
{
    if (currentHomeSensor != HOME_SENSOR_CO2)
    {
        tft.fillScreen(OWLOSDarkColor);
        currentHomeSensor = HOME_SENSOR_CO2;
        RefreshHomeScreenButtons();
    }
}

void HomeGasButtonTouch()
{
    if (currentHomeSensor != HOME_SENSOR_GAS)
    {
        tft.fillScreen(OWLOSDarkColor);
        currentHomeSensor = HOME_SENSOR_GAS;
        RefreshHomeScreenButtons();
    }
}

void HomeScreenInit()
{
    homeButton.OnTouchEvent = HomeButtonTouch;
    sensorsButton.OnTouchEvent = SensorButtonTouch;
    transportButton.OnTouchEvent = TransportButtonTouch;
    logButton.OnTouchEvent = LogButtonTouch;

    DHTButton.selectColor = OWLOSSecondaryColor;
    DHTButton.OnTouchEvent = HomeDHTButtonTouch;

    pressureButton.selectColor = OWLOSSecondaryColor;
    pressureButton.OnTouchEvent = HomePressureButtonTouch;

    CO2Button.selectColor = OWLOSSecondaryColor;
    CO2Button.OnTouchEvent = HomeCO2ButtonTouch;

    gasButton.selectColor = OWLOSSecondaryColor;
    gasButton.OnTouchEvent = HomeGasButtonTouch;

    homeButton.selected = true;
}

void HomeScreenRefresh()
{
    tft.fillScreen(OWLOSDarkColor);

    RefreshHomeScreenButtons();
}

void drawHomeDHTStatus()
{
    if (_DHTDriver->celsius)
    {        
      BigText(0, "temperature", String(atoi(_DHTDriver->temperature.c_str())), "C");
    }
    else 
    {
      BigText(0, "temperature", String(atoi(_DHTDriver->temperature.c_str())), "F");
    }

    int yStep = HEIGHT / 2 + GOLD_8;
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(valueLeftPadding, yStep);

    tft.loadFont(AA_FONT_SMALL);
    tft.print("humidity");

    tft.setCursor(WIDTH - tft.textWidth("heat index") - valueLeftPadding, yStep);
    tft.print("heat index");

    yStep += tft.fontHeight(0) + GOLD_8;
    tft.unloadFont();

    tft.setTextColor(OWLOSPrimaryColor, OWLOSDarkColor);
    tft.setCursor(valueLeftPadding, yStep);

    tft.loadFont(AA_FONT_BIG);

    String cHum = String(atoi(_DHTDriver->humidity.c_str())) + "%";
    if (prevHum.compareTo(cHum) != 0)
    {
        tft.fillRect(valueLeftPadding, yStep, tft.textWidth(prevHum), tft.fontHeight(0), OWLOSDarkColor);        
        prevHum = cHum;        
        tft.print(prevHum);
    }

    String cHeat = String(atoi(_DHTDriver->heatIndex.c_str()));
    if (prevHeat.compareTo(cHeat) != 0)
    {
        tft.fillRect(WIDTH - tft.textWidth(prevHeat) - valueLeftPadding, yStep, tft.textWidth(prevHeat), tft.fontHeight(0), OWLOSDarkColor);        
        prevHeat = cHeat;
        tft.setCursor(WIDTH - tft.textWidth(prevHeat) - valueLeftPadding, yStep);
        tft.print(prevHeat);
    }

    tft.unloadFont();
}

void drawHomePressureStatus()
{
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(valueLeftPadding, GOLD_6 + GOLD_8);

    String kPaStr = "--";
    String mmHgStr = "--";
    String altitudeStr = "--";
    String temperatureStr = "--";

// NOTE: Use BME680 if BMP280 is not pressent on the PCB
#ifdef USE_BMP280_DRIVER
    if ((_BMP280Driver != nullptr) && (_BMP280Driver->available == 1))
    {
        float kPa = atof(_BMP280Driver->pressure.c_str()) / 1000.0f;
        int mmHg = kPa * 7.5006375541921;

        kPaStr = String(kPa);
        mmHgStr = String(mmHg);
        altitudeStr = String(atoi(_BMP280Driver->altitude.c_str()));
        temperatureStr = String(atoi(_BMP280Driver->temperature.c_str()));
    }
#endif

#ifdef USE_BME680_DRIVER
    if ((_BME680Driver != nullptr) && (_BME680Driver->available == 1))
    {
        float kPa = atof(_BME680Driver->pressure.c_str()) / 1000.0f;
        int mmHg = kPa * 7.5006375541921;

        kPaStr = String(kPa);
        mmHgStr = String(mmHg);
        altitudeStr = String(atoi(_BME680Driver->altitude.c_str()));
        temperatureStr = String(atoi(_BME680Driver->temperature.c_str()));
    }
#endif

    BigText(0, "pressure", mmHgStr, "mmHg");

    int yStep = HEIGHT / 2  + GOLD_8;

    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(valueLeftPadding, yStep);

    tft.loadFont(AA_FONT_SMALL);
    tft.print("altitude");

    tft.setCursor(WIDTH - tft.textWidth("temperature") - valueLeftPadding, yStep);
    tft.print("temperature");

    yStep += tft.fontHeight(0) + GOLD_8;
    tft.unloadFont();

    tft.setTextColor(OWLOSPrimaryColor, OWLOSDarkColor);
    tft.setCursor(valueLeftPadding, yStep);

    tft.loadFont(AA_FONT_BIG);

    if (prevAltitude.compareTo(altitudeStr + "m") != 0)
    {
        tft.fillRect(valueLeftPadding, yStep, tft.textWidth(prevAltitude), tft.fontHeight(0), OWLOSDarkColor);
        prevAltitude = altitudeStr + "m";
        tft.print(prevAltitude);
    }

    if (prevBMxTemp.compareTo(temperatureStr + "C") != 0)
    {
        tft.fillRect(WIDTH - tft.textWidth(prevBMxTemp) - valueLeftPadding, yStep, tft.textWidth(prevBMxTemp), tft.fontHeight(0), OWLOSDarkColor);
        prevBMxTemp = temperatureStr + "C";
        tft.setCursor(WIDTH - tft.textWidth(prevBMxTemp) - valueLeftPadding, yStep);
        tft.print(prevBMxTemp);
    }

    tft.unloadFont();
}

void HomeScreenDraw()
{
    homeButton.draw();
    sensorsButton.draw();
    transportButton.draw();
    logButton.draw();

    DHTButton.draw();
    pressureButton.draw();
    CO2Button.draw();
    gasButton.draw();

    if (currentMode == HOME_MODE)
    {
        switch (currentHomeSensor)
        {
        case HOME_SENSOR_DHT:
            drawHomeDHTStatus();
            break;
        case HOME_SENSOR_PRESSURE:
            drawHomePressureStatus();
            break;
        }
    }
}
