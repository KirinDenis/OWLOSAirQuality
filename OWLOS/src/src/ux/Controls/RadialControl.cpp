/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2022 by:
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

#include "RadialControl.h"

extern TFT_eSPI tft;

extern uint16_t touchX, touchY;
extern bool touch;

RadialControlClass::RadialControlClass()
{
}

float RadialControlClass::getCurrentStep(float _value)
{
    // value to 50 to oneHungred
    //-20 ... 0%     0
    //-10 ... 20%    10
    // 0   ... 40%    20
    // 10  ... 60%    30
    // 20  ... 80%    40
    // 30  ... 100%   50
    int currentAValue = abs(low) + _value; // 50
    float percent = currentAValue * (oneHungred / 100.0f);
    // 0%    step = 0
    // 100%  step = (360 - angleFrom) + angleTo;
    return (fullStep / 100.0f) * percent; // 2.4 * 50 = 120
}

void RadialControlClass::drawIndicator()
{
    float currentStep = getCurrentStep(value);

    u_int value_color = OWLOSDangerColor;
    if ((value > lowDanger) && (value <= lowWarning))
    {
        value_color = OWLOSWarningColor;
    }
    else if ((value > lowWarning) && (value <= highWarning))
    {
        value_color = OWLOSPrimaryColor;
    }
    else if ((value > highWarning) && (value <= highDanger))
    {
        value_color = OWLOSWarningColor;
    }

    fillArc(x, y, angleFrom, currentStep, value_size, value_size, width, value_color);

    // plotQuadBezierSegAA(x,y, 199, 100, 250, 200);

    if (angleFrom + currentStep <= 360)
    {
        fillArc(x, y, angleFrom + currentStep, fullStep - currentStep, value_size, value_size, width, OWLOSSecondaryColor);
    }
    else
    {
        fillArc(x, y, 360 - (360 - (angleFrom + currentStep)), fullStep - currentStep, value_size, value_size, width, OWLOSSecondaryColor);
    }
}

void RadialControlClass::drawIndicatorInfo()
{
    fillArc(x, y, angleFrom, getCurrentStep(lowDanger), size, size, 2, OWLOSDangerColor);
    fillArc(x, y, angleFrom + getCurrentStep(lowDanger), getCurrentStep(lowWarning) - getCurrentStep(lowDanger), size, size, 2, OWLOSWarningColor);
    fillArc(x, y, angleFrom + getCurrentStep(lowWarning), getCurrentStep(highWarning) - getCurrentStep(lowWarning), size, size, 2, OWLOSInfoColor);
    fillArc(x, y, angleFrom + getCurrentStep(highWarning), getCurrentStep(highDanger) - getCurrentStep(highWarning), size, size, 2, OWLOSWarningColor);
    fillArc(x, y, angleFrom + getCurrentStep(highDanger), fullStep - getCurrentStep(highDanger), size, size, 2, OWLOSDangerColor);

    //--- text
    tft.loadFont(AA_FONT_BIG);

    // unit of mesure
    String valuStr = "C";
    int16_t tW = tft.textWidth(valuStr);
    int16_t tH = tft.fontHeight(0);
    int16_t tX = x - (tW / 2);
    int16_t tY = y - (tH / 2);
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(tX, tY);

    tft.print(valuStr);

    tft.unloadFont();
}

TextRect RadialControlClass::getTextRect(String text)
{
    TextRect result;
    result.width = tft.textWidth(text);
    result.height = tft.fontHeight(0);

    result.x = x - (result.width / 2);
    result.y = y + size - result.height;
    return result;
}

void RadialControlClass::drawValue()
{
    if (value != prevValue)
    {
        String valueStr;
        // hide previos
        tft.loadFont(AA_FONT_BIG);

        if (printFloatValue)
        {
            valueStr = (String)prevValue;
        }
        else
        {
            valueStr = (String)(int)prevValue;
        }

        TextRect textRect = getTextRect(valueStr);

        tft.fillRect(textRect.x, textRect.y, textRect.width, textRect.height, OWLOSDarkColor);

        prevValue = value;

        if (printFloatValue)
        {
            valueStr = (String)prevValue;
        }
        else
        {
            valueStr = (String)(int)prevValue;
        }

        // print new
        textRect = getTextRect(valueStr);

        if (printFloatValue)
        {
            valueStr = (String)prevValue;
        }
        else
        {
            valueStr = (String)(int)prevValue;
        }

        tft.setTextColor(OWLOSPrimaryColor, OWLOSDarkColor);
        tft.setCursor(textRect.x, textRect.y);
        tft.print(valueStr);
        tft.unloadFont();
    }
}

void RadialControlClass::setSize(u_short _size)
{
    size = _size;
    width = _size / 5;
    value_size = _size - width / 2;
}

void RadialControlClass::refresh()
{
    drawIndicatorInfo();
    draw(low);
}

void RadialControlClass::draw(float _value)
{
    if ((_value >= low) && (_value <= high))
    {
        value = _value;
        drawIndicator();
        drawValue();
    }
}
