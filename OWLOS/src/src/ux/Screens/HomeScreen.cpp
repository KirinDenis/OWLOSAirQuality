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
#include "../Controls/BigTextControl.h"
#include "../Controls/MediumTextControl.h"

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

BigTextControlClass bigText(0);
MediumTextControlClass leftDownMediumText(0, 1);
MediumTextControlClass rightDownMediumText(1, 1);

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

#define HOME_SENSOR_DHT 0
#define HOME_SENSOR_PRESSURE 1
#define HOME_SENSOR_CO2 2
#define HOME_SENSOR_GAS 3

int currentHomeSensor = HOME_SENSOR_DHT;

void RefreshHomeScreenButtons()
{

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

    bigText.refresh();
    leftDownMediumText.refresh();
    rightDownMediumText.refresh();
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
        bigText.draw("temperature", String(atoi(_DHTDriver->temperature.c_str())), "C");
    }
    else
    {
        bigText.draw("temperature", String(atoi(_DHTDriver->temperature.c_str())), "F");
    }

    leftDownMediumText.draw("humidity", String(atoi(_DHTDriver->humidity.c_str())) + "%");
    rightDownMediumText.draw("heat index", String(atoi(_DHTDriver->heatIndex.c_str())));
}

void drawHomePressureStatus()
{
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

    bigText.draw("pressure", mmHgStr, "mmHg");

    leftDownMediumText.draw("altitude", altitudeStr + "m");
    rightDownMediumText.draw("temperature", temperatureStr + "C");
}

void drawHomeCO2Status()
{
    bigText.draw("eCO2", _CCS811Driver->CO2, "PPM");
    leftDownMediumText.draw("TVOC", _CCS811Driver->TVOC + "PPB");
    rightDownMediumText.draw("Resistance", _CCS811Driver->resistence);
}

void drawHomeGasStatus()
{
    bigText.draw("CO2 CO Smoke Dust [MQ135]", _ADS1X15Driver->chanel_1, "");
    leftDownMediumText.draw("CO [MQ7]", _ADS1X15Driver->chanel_2);
    rightDownMediumText.draw("eCO2", _CCS811Driver->CO2 + "PPM");
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
        case HOME_SENSOR_CO2:
            drawHomeCO2Status();
            break;
        case HOME_SENSOR_GAS:
            drawHomeGasStatus();
            break;
        }
    }
}
