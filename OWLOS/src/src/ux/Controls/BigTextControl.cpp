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

#include "BigTextControl.h"

extern TFT_eSPI tft;

extern uint16_t touchX, touchY;
extern bool touch;

BigTextControlClass::BigTextControlClass(int row, int _captionColor, int _valueColor, int _valueTypeColor)
{
    if (row == 1)
    {
        y = HEIGHT / 2;
    }

    captionColor = _captionColor;
    valueColor = _valueColor;
    valueTypeColor = _valueTypeColor;
}

void BigTextControlClass::refresh()
{
    String _captionText = captionText;
    String _valueText = valueText;
    String _valueTypeText = valueTypeText;
    captionText = valueText = valueTypeText = "";
    draw(_captionText, _valueText, _valueTypeText);
}

void BigTextControlClass::draw(String caption, String value, String valueType, int captionColor, int valueColor, int valueTypeColor)
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
    if ((!caption.equals(captionText)) || (!value.equals(valueText)) || (!valueType.equals(valueTypeText)) || inTouch)
    {
        int yStep = y;

        tft.loadFont(AA_FONT_SMALL);

        if (caption.compareTo(captionText) != 0)
        {
            tft.fillRect(LEFT_PADDING, yStep, tft.textWidth(captionText), tft.fontHeight(0), bgColor);
            tft.setTextColor(captionColor, bgColor);
            tft.setCursor(LEFT_PADDING, yStep);
            captionText = caption;
            tft.print(caption);
        }

        yStep += tft.fontHeight(1) + GOLD_8;
        tft.unloadFont();

        if (value.compareTo(valueText) != 0)
        {
            tft.loadFont(AA_FONT_BIG);
            int valueTypeWidth = tft.textWidth(valueType) + GOLD_8;
            tft.unloadFont();

            tft.loadFont(AA_FONT_LARGE);

            tft.fillRect(0, yStep, WIDTH, tft.fontHeight(0), bgColor);

            tft.setTextColor(valueColor, bgColor);
            valueText = value;
            int valueLeft = WIDTH / 2 - (tft.textWidth(valueText) / 2 + valueTypeWidth / 2); // recalculate left
            int valueHeight = tft.fontHeight(0);
            tft.setCursor(valueLeft, yStep);
            tft.print(valueText);
            valueLeft += tft.textWidth(valueText) + GOLD_8;
            tft.unloadFont();

            tft.loadFont(AA_FONT_BIG);
            tft.setTextColor(valueTypeColor, bgColor);
            tft.setCursor(valueLeft, yStep + valueHeight - tft.fontHeight(0) - GOLD_10);
            valueTypeText = valueType;
            tft.print(valueTypeText);
            tft.unloadFont();
        }
    }
}