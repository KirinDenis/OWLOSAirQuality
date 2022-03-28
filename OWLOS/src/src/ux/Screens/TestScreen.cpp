/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2021 by:
- Denis Kirin (deniskirinacs@gmail.com)
- Serhii Lehkii (sergey@light.kiev.ua)

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

#include "TestScreen.h"

#include "../../utils/Utils.h"

extern TFT_eSPI tft;

extern int currentMode;

extern bool SetupComplete;

int count = 0;

#define LOG_HEIGHT 14 * GOLD_8 

void testScreenInit()
{
}

void testScreenRefresh()
{
  tft.fillScreen(OWLOSDarkColor);

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
  for (int i = start_angle; i < start_angle + seg * seg_count; i += inc) {

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
}

byte inc = 0;
unsigned int col = 0;

void testScreenDraw()
{
  if (SetupComplete)
  {
    count++;
    tft.setCursor(0, 0);
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.print("[" + millisToDate(millis()) + "] ");
    tft.setTextColor(OWLOSLightColor, OWLOSDarkColor);
    tft.print(count);

  

    //tft.drawCircle(150, 150, 100, OWLOSLightColor);
    //tft.drawCircleHelper(150, 150, 80, 90, OWLOSLightColor);

  //fillArc(160, 120, inc * 6, 1, 140, 140, 140, OWLOSInfoColor);

  // Continuous segmented (inc*2) elliptical arc drawing
  //fillArc(160, 120, ((inc * 2) % 60) * 6, 1, 120, 120, 120, OWLOSWarningColor);

  // Circle drawing using arc with arc width = radius
  //fillArc(160, 120, inc * 6, 1, 42, 42, 42, OWLOSSuccessColor);

  fillArc(150, 150, 200, inc, 100, 100, 10, OWLOSSuccessColor);
  inc++;
  col += 1;
  if (col > 191) col = 0;
  if (inc > 360 / 3) inc = 0;
    

  }
}


