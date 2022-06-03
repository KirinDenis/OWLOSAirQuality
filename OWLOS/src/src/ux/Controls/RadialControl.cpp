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

    fillArc(x + (size / 2), y + (size / 2), angleFrom, currentStep, indicatorRadius, indicatorRadius, indicatorWidth, value_color);

    if (angleFrom + currentStep <= 360)
    {
        fillArc(x + (size / 2), y + (size / 2), angleFrom + currentStep, fullStep - currentStep, indicatorRadius, indicatorRadius, indicatorWidth, OWLOSSecondaryColor);
    }
    else
    {
        fillArc(x + (size / 2), y + (size / 2), 360 - (360 - (angleFrom + currentStep)), fullStep - currentStep, indicatorRadius, indicatorRadius, indicatorWidth, OWLOSSecondaryColor);
    }
}

void RadialControlClass::drawIndicatorInfo()
{
    fillArc(x + (size / 2), y + (size / 2), angleFrom, getCurrentStep(lowDanger), indicatorRadius + 4, indicatorRadius + 4, 1, OWLOSDangerColor);
    fillArc(x + (size / 2), y + (size / 2), angleFrom + getCurrentStep(lowDanger), getCurrentStep(lowWarning) - getCurrentStep(lowDanger), indicatorRadius + 4, indicatorRadius + 4, 1, OWLOSWarningColor);
    fillArc(x + (size / 2), y + (size / 2), angleFrom + getCurrentStep(lowWarning), getCurrentStep(highWarning) - getCurrentStep(lowWarning), indicatorRadius + 4, indicatorRadius + 4, 1, OWLOSInfoColor);
    fillArc(x + (size / 2), y + (size / 2), angleFrom + getCurrentStep(highWarning), getCurrentStep(highDanger) - getCurrentStep(highWarning), indicatorRadius + 4, indicatorRadius + 4, 1, OWLOSWarningColor);
    fillArc(x + (size / 2), y + (size / 2), angleFrom + getCurrentStep(highDanger), fullStep - getCurrentStep(highDanger), indicatorRadius + 4, indicatorRadius + 4, 1, OWLOSDangerColor);

    //--- text
    tft.loadFont(AA_FONT_BIG);

    // unit of mesure    
    TextRect textRect = getTextRect(unitOfMesure);
    int16_t tX = x + (size / 2) - (textRect.width / 2);
    int16_t tY = y + (size / 2) - (textRect.height / 2);
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(tX, tY);
    tft.print(unitOfMesure);
    tft.unloadFont();

    // title
    tft.loadFont(AA_FONT_SMALL);
    tft.setTextColor(OWLOSInfoColor, OWLOSDarkColor);
    tft.setCursor(x + size / 20, y);
    tft.print(title);

    // value type
    textRect = getTextRect(valueType);
    tft.setCursor((x + size / 2) - textRect.width / 2, y + size - size / 11);
    tft.print(valueType);

    tft.unloadFont();

    // String title = "";
    // String unitOfMesure = "";
    // String valueType = "";
}

TextRect RadialControlClass::getTextRect(String text)
{
    TextRect result;
    result.width = tft.textWidth(text);
    result.height = tft.fontHeight(0);

    result.x = x + (size / 2) - (result.width / 2);
    result.y = y + size - result.height - size / 6;
    return result;
}

void RadialControlClass::drawValue()
{
        String valueStr;
        
        if (printFloatValue)
        {
            valueStr = (String)value;
        }
        else
        {
            valueStr = (String)(int)value;
        }

        if (!prevValue.equals(valueStr))
        {

            prevValue = valueStr;

            tft.fillRect(prevTextRect.x, prevTextRect.y, prevTextRect.width, prevTextRect.height, OWLOSDarkColor);

            tft.loadFont(AA_FONT_BIG);
            // print new
            TextRect textRect = getTextRect(valueStr);
            if (textRect.width > size / 2)
            {
                tft.unloadFont();
                tft.loadFont(AA_FONT_SMALL);
                textRect = getTextRect(valueStr);
            }

            prevTextRect = textRect;

            tft.setTextColor(OWLOSPrimaryColor, OWLOSDarkColor);
            tft.setCursor(textRect.x, textRect.y);
            tft.print(valueStr);
            tft.unloadFont();
        }
}


void RadialControlClass::setSize(u_short _size)
{
    size = _size;
    indicatorRadius = size / 2 - size / 10;
    indicatorWidth = indicatorRadius / 10;
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
