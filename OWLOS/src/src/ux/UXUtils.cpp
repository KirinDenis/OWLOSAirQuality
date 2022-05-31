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

#include "UXUtils.h"
#include "../config.h"

TFT_eSPI tft = TFT_eSPI(); // Invoke custom library with default width and height

int currentMode = TRANSPORT_MODE;

//--------------------------------------------------------------------------------------------------
void drawArc(int x, int y, int radiusFrom, int radiusTo, double angleFrom, double angleTo, int color)
{
    double xp1, yp1, xp2, yp2;

    for (double radius = radiusFrom; radius < radiusTo; radius += 0.5)
    {
        xp2 = -1;
        yp2 = -1;
        for (double angle = angleFrom; angle < angleTo; angle += 0.05)
        {
            xp1 = radius * sin(angle) + x;
            yp1 = radius * cos(angle) + y;

            if (xp2 != -1)
            {
                tft.drawLine(xp1, yp1, xp2, yp2, color);
                tft.drawPixel(xp1, yp1 - 1, 0xAAAA);
                tft.drawPixel(xp1, yp1 + 1, 0xAAAA);
                tft.drawPixel(xp1, yp1, color);
            }

            xp2 = xp1;
            yp2 = yp1;
        }
    }
}

#define DEG2RAD 0.0174532925
void fillArc(int x, int y, int start_angle, int seg_count, int rx, int ry, int w, unsigned int colour)
{

  byte seg = 1; // Segments are 3 degrees wide = 120 segments for 360 degrees
  byte inc = 1; // Draw segments every 3 degrees, increase to 6 for segmented ring

  // Calculate first pair of coordinates for segment start
  float sx = cos((start_angle - 90) * DEG2RAD);
  float sy = sin((start_angle - 90) * DEG2RAD);
  uint16_t x0 = sx * (rx - w) + x;
  uint16_t y0 = sy * (ry - w) + y;
  uint16_t x1 = sx * rx + x;
  uint16_t y1 = sy * ry + y;

  // Draw colour blocks every inc degrees
  for (int i = start_angle; i < start_angle + seg * seg_count; i += inc)
  {

    // Calculate pair of coordinates for segment end
    float sx2 = cos((i + seg - 90) * DEG2RAD);
    float sy2 = sin((i + seg - 90) * DEG2RAD);
    int x2 = sx2 * (rx - w) + x;
    int y2 = sy2 * (ry - w) + y;
    int x3 = sx2 * rx + x;
    int y3 = sy2 * ry + y;

    tft.fillTriangle(x0, y0, x1, y1, x2, y2, colour);
    tft.fillTriangle(x1, y1, x2, y2, x3, y3, colour);

    // Copy segment end to sgement start for next segment
    x0 = x2;
    y0 = y2;
    x1 = x3;
    y1 = y3;
  }

  // SMOOTH  
  for (int i = start_angle; i < start_angle + seg * seg_count; i += inc)
  {
    // Calculate pair of coordinates for segment end
    sx = cos((i + seg - 90) * DEG2RAD);
    sy = sin((i + seg - 90) * DEG2RAD);

    float smoothX = sx * (rx - w) + x;
    float smoothY = sy * (ry - w) + y;
    double intpart;

    float fX = modf(smoothX, &intpart);
    float fY = modf(smoothY, &intpart);

    if (fX > 0.5)
    {
      fX = 1.0 - fX;
    }
    if (fY > 0.5)
    {
      fY = 1.0 - fY;
    }

    byte alphaChannel = (fX * 127 + fY * 127) * 2;

    tft.drawPixel(smoothX, smoothY, tft.alphaBlend(alphaChannel, colour, OWLOSDarkColor));
  }
}


void drawWifiIcon(int x, int y, int dBm)
{
    double angleFrom = 3.14 - 0.7;
    double angleTo = 3.14 + 0.7;

    int color;

    int radius = GOLD_8;

    if (dBm > -50)
    {
        color = OWLOSLightColor;
    }
    else
    {
        color = 0xBBBB;
    }
    drawArc(x, y, radius - 1, radius - 0, angleFrom, angleTo, color);

    if (dBm > -67)
    {
        color = OWLOSLightColor;
    }
    else
    {
        color = 0xBBBB;
    }
    drawArc(x, y, radius - 8, radius - 7, angleFrom, angleTo, color);

    if (dBm > -70)
    {
        color = OWLOSLightColor;
    }
    else
    {
        color = 0xBBBB;
    }
    drawArc(x, y, radius - 15, radius - 14, angleFrom, angleTo, color);

    if (dBm > -80)
    {
        color = OWLOSLightColor;
    }
    else
    {
        color = 0xBBBB;
    }
    drawArc(x, y, radius - 22, radius - 21, angleFrom, angleTo, color);

    //tft.setTextColor(OWLOSLightColor, OWLOSDarkColor);
    //tft.drawString(String(dBm) + " dBn", x + GOLD_9, y + GOLD_11, 1);
}

String millisToDate(unsigned long _millis)
{
    _millis = _millis / 1000; //to seconds
    int days = (_millis / (24 * 60 * 60));
    _millis = _millis - days * (24 * 60 * 60);
    byte hours = _millis / (60 * 60);
    _millis = _millis - hours * (60 * 60);
    byte minutes = _millis / 60;
    byte seconds = _millis - minutes * 60;

    String _hours = String(hours);
    if (hours < 10)
    {
        _hours = "0" + _hours;
    }

    String _minutes = String(minutes);
    if (minutes < 10)
    {
        _minutes = "0" + _minutes;
    }

    String _seconds = String(seconds);
    if (seconds < 10)
    {
        _seconds = "0" + _seconds;
    }

    return String(days) + "d." + _hours + ":" + _minutes + ":" + _seconds;
}

//-----------------------------------
String bytesToString(int bytesCount)
{
    if (bytesCount > 1024 * 1024 * 1024)
    {
        return String(bytesCount / (1024 * 1024 * 1024)) + " Gb";
    }
    else if (bytesCount > 1024 * 1024)
    {
        return String(bytesCount / (1024 * 1024)) + " Mb";
    }
    else if (bytesCount > 1024)
    {
        return String(bytesCount / (1024)) + " Kb";
    }
    else
    {
        return String(bytesCount) + " B";
    }
}
