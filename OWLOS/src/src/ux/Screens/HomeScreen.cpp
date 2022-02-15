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

ButtonControlClass homeButton("home", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 1, 16);
ButtonControlClass sensorsButton("sensors", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 2, 16);
ButtonControlClass transportButton("transport", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 3, 16);
ButtonControlClass logButton("log", OWLOSLightColor, OWLOSInfoColor, OWLOSWarningColor, 4, 16);

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

TextControlClass dhtHomeHeaderItem(0, 0);
TextControlClass dhtHomeTempItem(10, 20, 4);
TextControlClass dhtHomeTempValueItem(1, 1);
TextControlClass dhtHomeHumItem(0, 2);
TextControlClass dhtHomeHumValueItem(1, 2);
TextControlClass dhtHomeHeatItem(0, 3);
TextControlClass dhtHomeHeatValueItem(1, 3);

int scount = 0;
int size = 1;

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


void HomeScreenInit()
{
    homeButton.OnTouchEvent = HomeButtonTouch;
    sensorsButton.OnTouchEvent = SensorButtonTouch;
    transportButton.OnTouchEvent = TransportButtonTouch;
    logButton.OnTouchEvent = LogButtonTouch;
    sensorsButton.selected = true;
}

void HomeScreenRefresh()
{
    tft.fillScreen(OWLOSDarkColor);
    homeButton.refresh();
    sensorsButton.refresh();
    transportButton.refresh();
    logButton.refresh();
}

void drawHomeDHTStatus()
{
    int statusColor = OWLOSDangerColor;
    int textColor = OWLOSPrimaryColor;

#define AA_FONT_SMALL "omegaflight20"
#define AA_FONT_BIG "omegaflight54"
#define AA_FONT_LARGE "omegaflight108"


  //tft.fillScreen(TFT_BLACK);

  tft.setTextColor(OWLOSInfoColor, TFT_BLACK);                                        
  tft.setCursor(0, 0); // Set cursor at top left of screen
  tft.loadFont(AA_FONT_SMALL); // M
  tft.println(); 
  tft.print("temperature"); // print leaves cursor at end of line
  tft.println(); 
  tft.println(); 
  tft.unloadFont();

  tft.loadFont(AA_FONT_LARGE); // M
  tft.setTextColor(OWLOSLightColor, TFT_BLACK);                                        
  tft.println(_DHTDriver->temperature + "C");
  tft.unloadFont();

  tft.setTextColor(OWLOSInfoColor, TFT_BLACK);                                        
  //tft.setCursor(0, 0); // Set cursor at top left of screen
  tft.loadFont(AA_FONT_SMALL); // M
  tft.println(); 
  tft.print("humidity"); // print leaves cursor at end of line
  tft.println(); 
  tft.println(); 
  tft.unloadFont();

  tft.loadFont(AA_FONT_BIG); // M
  tft.setTextColor(OWLOSPrimaryColor, TFT_BLACK);                                        
  tft.println(_DHTDriver->humidity + "%");
  tft.unloadFont();


/*
  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
  // Small font
  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

  tft.loadFont(AA_FONT_SMALL); // Must load the font first

  tft.println("Small 15pt font"); // println moves cursor down for a new line

  tft.println(); // New line

  tft.print("ABC"); // print leaves cursor at end of line

  tft.setTextColor(TFT_CYAN, TFT_BLACK);
  tft.println("1234"); // Added to line after ABC

  tft.setTextColor(TFT_YELLOW, TFT_BLACK);
  // print stream formatting can be used,see:
  // https://www.arduino.cc/en/Serial/Print
  int ivalue = 1234;
  tft.println(ivalue);       // print as an ASCII-encoded decimal
  tft.println(ivalue, DEC);  // print as an ASCII-encoded decimal
  tft.println(ivalue, HEX);  // print as an ASCII-encoded hexadecimal
  tft.println(ivalue, OCT);  // print as an ASCII-encoded octal
  tft.println(ivalue, BIN);  // print as an ASCII-encoded binary

  tft.println(); // New line
  tft.setTextColor(TFT_MAGENTA, TFT_BLACK);
  float fvalue = 1.23456;
  tft.println(fvalue, 0);  // no decimal places
  tft.println(fvalue, 1);  // 1 decimal place
  tft.println(fvalue, 2);  // 2 decimal places
  tft.println(fvalue, 5);  // 5 decimal places

  delay(5000);

  // Get ready for the next demo while we have this font loaded
  tft.fillScreen(TFT_BLACK);
  tft.setCursor(0, 0); // Set cursor at top left of screen
  tft.setTextColor(TFT_WHITE, TFT_BLACK);
  tft.println("Wrong and right ways to");
  tft.println("print changing values...");

  tft.unloadFont(); // Remove the font to recover memory used


  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
  // Large font
  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

  tft.loadFont(AA_FONT_LARGE); // Load another different font

  //tft.fillScreen(TFT_BLACK);
  
  // Draw changing numbers - does not work unless a filled rectangle is drawn over the old text
  int pI = 0;
  for (int i = 0; i <= 20; i++)
  {
    tft.setCursor(50, 50);
    //tft.print("      "); // Overprinting old number with spaces DOES NOT WORK!
    tft.setTextColor(TFT_BLACK, TFT_BLACK);
    tft.print(pI / 10.0, 1);
    pI = i;

    tft.setTextColor(TFT_GREEN, TFT_BLACK);
    tft.setCursor(50, 50);
    tft.print(i / 10.0, 1);

    tft.fillRect (50, 90, 60, 40, TFT_BLACK); // Overprint with a filled rectangle
    tft.setTextColor(TFT_GREEN, TFT_BLACK);
    tft.setCursor(50, 90);
    tft.print(i / 10.0, 1);
    
    delay (200);
  }

  delay(5000);

  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
  // Large font text wrapping
  // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

  tft.fillScreen(TFT_BLACK);
  
  tft.setTextColor(TFT_YELLOW, TFT_BLACK); // Change the font colour and the background colour

  tft.setCursor(0, 0); // Set cursor at top left of screen

  tft.println("Large font!");

  tft.setTextWrap(true); // Wrap on width
  tft.setTextColor(TFT_CYAN, TFT_BLACK);
  tft.println("Long lines wrap to the next line");

  tft.setTextWrap(false, false); // Wrap on width and height switched off
  tft.setTextColor(TFT_MAGENTA, TFT_BLACK);
  tft.println("Unless text wrap is switched off");

  tft.unloadFont(); // Remove the font to recover memory used

  delay(8000);
*/



//tft.setTextColor(statusColor, OWLOSSecondaryColor);
//tft.drawString(String(size), 10, 10, size);

//dhtHomeHeaderItem.draw("DHT 22", statusColor, OWLOSSecondaryColor, size);

    /*
    if ((_DHTDriver != nullptr) && (_DHTDriver->available == 1))
    {
        statusColor = OWLOSLightColor;
        textColor = OWLOSLightColor;

        if (_DHTDriver->celsius == 1)
        {
            dhtHomeTempValueItem.draw(_DHTDriver->temperature + "C", textColor, OWLOSDarkColor, 7);
        }
        else
        {
            dhtHomeTempValueItem.draw(_DHTDriver->temperature + "F", textColor, OWLOSDarkColor, 8);
        }
        dhtHomeHumValueItem.draw(_DHTDriver->humidity + "%", textColor, OWLOSDarkColor, 9);
        dhtHomeHeatValueItem.draw(_DHTDriver->heatIndex, textColor, OWLOSDarkColor, 10);
    }
    else
    {
        dhtHomeTempValueItem.draw("--", textColor, OWLOSDarkColor, 2);
        dhtHomeHumValueItem.draw("--", textColor, OWLOSDarkColor, 2);
        dhtHomeHeatValueItem.draw("--", textColor, OWLOSDarkColor, 2);
    }

    dhtHomeHeaderItem.draw("DHT 22", statusColor, OWLOSSecondaryColor, 1);
    textColor = OWLOSPrimaryColor;
    dhtHomeTempItem.draw("temperature", textColor, OWLOSDarkColor, 4);
    dhtHomeHumItem.draw("humidity", textColor, OWLOSDarkColor, 5);
    dhtHomeHeatItem.draw("heat index", textColor, OWLOSDarkColor, 6);
    */
}


void HomeScreenDraw()
{
    homeButton.draw();
    sensorsButton.draw();
    transportButton.draw();
    logButton.draw();

    drawHomeDHTStatus();
}
