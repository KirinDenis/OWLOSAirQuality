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

#include "../Controls/RadialControl.h"

extern TFT_eSPI tft;

extern int currentMode;

extern bool SetupComplete;

RadialControlClass radial1;
RadialControlClass radial2;

void testScreenInit()
{
}

void testScreenRefresh()
{
  tft.fillScreen(OWLOSDarkColor);

    radial1.x = 100;
    radial1.refresh();

    radial2.x = 350;
    radial2.setSize(70);
    radial2.refresh();
}

int inc = 0;

void testScreenDraw()
{
  if (SetupComplete)
  {

    radial1.draw(inc);

    radial2.draw(100 - inc);

    inc += 2;
    if (inc > 100)
    {
      inc = 0;
    }
  }
}
