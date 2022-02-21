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

#include "MediumTextControl.h"

extern TFT_eSPI tft;

extern uint16_t touchX, touchY;
extern bool touch;

MediumTextControlClass::MediumTextControlClass(int _col, int row, int _captionColor, int _valueColor)
{
    col = _col;

    if (row == 1)
    {
        y = HEIGHT / 2 + GOLD_8*2;
    }

    captionColor = _captionColor;
    valueColor = _valueColor;
}

void MediumTextControlClass::refresh()
{
    String _captionText = captionText;
    String _valueText = valueText;
    captionText = valueText = "";
    draw(_captionText, _valueText);
}

void MediumTextControlClass::draw(String caption, String value, int captionColor, int valueColor)
{
    if (OnTouchEvent != nullptr)
    {
        if (touch)
        {
            if (!inTouch)
            {
                inTouch = true;
                bgColor = OWLOSDangerColor;
            }

            if ((touchY > y) && (touchY < HEIGHT / 2))
            {
                (*OnTouchEvent)();
                return;
            }
        }
        else
        {
            if (inTouch)
            {
                inTouch = false;
                bgColor = OWLOSDarkColor;
            }
        }
    }

    //если текст не совпадает - закрываем предидущий текст цветом предидущего фона
    if ((!caption.equals(captionText)) || (!value.equals(valueText)) || inTouch)
    {
        int yStep = y;
        tft.setTextColor(captionColor, bgColor);

        tft.loadFont(AA_FONT_SMALL);
        if (col == 0)
        {
            tft.setCursor(LEFT_PADDING, yStep);
            tft.fillRect(LEFT_PADDING, yStep, tft.textWidth(captionText), tft.fontHeight(0), bgColor);
        }
        else
        {
            tft.setCursor(WIDTH - tft.textWidth(caption) - LEFT_PADDING, yStep);
            tft.fillRect(WIDTH - tft.textWidth(captionText) - LEFT_PADDING, yStep, tft.textWidth(captionText), tft.fontHeight(0), bgColor);
        }

        captionText = caption;
        tft.print(captionText);

        yStep += tft.fontHeight(0) + GOLD_8;
        tft.unloadFont();

        tft.setTextColor(valueColor, bgColor);
        tft.loadFont(AA_FONT_BIG);
        if (col == 0)
        {
            tft.setCursor(LEFT_PADDING, yStep);
            tft.fillRect(LEFT_PADDING, yStep, tft.textWidth(valueText), tft.fontHeight(0), bgColor);
        }
        else
        {
            tft.setCursor(WIDTH - tft.textWidth(value) - LEFT_PADDING, yStep);
            tft.fillRect(WIDTH - tft.textWidth(valueText) - LEFT_PADDING, yStep, tft.textWidth(valueText), tft.fontHeight(0), bgColor);
        }

        valueText = value;

        tft.print(valueText);

        tft.unloadFont();
    }
}