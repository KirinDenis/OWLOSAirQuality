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

#ifndef RADIALCONTROL_H
#define RADIALCONTROL_H

#include "../UXColors.h"
#include "../UXUtils.h"

struct TextRect 
{
    uint16_t x;
    uint16_t y;
    uint16_t width;
    uint16_t height;
};

class RadialControlClass
{
protected:
    u_short size = 100;
    u_short width = size / 6;
    u_short value_size = size - width / 2;

    int angleFrom = 270 - 30;
    int angleTo = 90 + 30;
    int fullStep = (360 - angleFrom) + angleTo; // 120 + 120 = 240



    float value = low;
    float prevValue = high;

    float getCurrentStep(float _value);
    void drawIndicator();
    void drawIndicatorInfo();
    
    TextRect getTextRect(String text);
    void drawValue();

public:
    //void (*OnTouchEvent)() = nullptr;

    u_int x = 240;
    u_int y = 150;

    float low = -50;
    float high = 50;
    // value to percent
    // 100 percent = high - low
    float oneHungred = abs(high + (low * -1));


    float lowDanger = -35;
    float lowWarning = -25;
    float highWarning = 20;
    float highDanger = 30;
    
    bool printFloatValue = false;
    
    RadialControlClass();

    void setSize(u_short _size);

    void refresh();
    void draw(float _value);
};

#endif