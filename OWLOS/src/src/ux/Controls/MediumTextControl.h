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

#ifndef MEDIUMTEXTCONTROL_H
#define MEDIUMTEXTCONTROL_H

//#include <Arduino.h>
#include "../UXColors.h"
#include "../UXUtils.h"

/* ------------------------------------------------------
Класс рисует большой текст во всю длинну экрана
- две строки на весь экран
*/
class MediumTextControlClass
{
protected:
    bool inTouch = false;
    int size = 1;

    int bgColor = OWLOSDarkColor;
    int captionColor = OWLOSInfoColor;
    int valueColor = OWLOSLightColor;

    String captionText = "";
    String valueText = "";

    int col = 0;
    int y = GOLD_6 + GOLD_8;

    void draw();

public:
    void (*OnTouchEvent)() = nullptr;
    // col = 0 left, col = 1 right
    // row = 0 up, row = 1 down
    MediumTextControlClass(int _col, int row, int _captionColor = OWLOSInfoColor, int _valueColor = OWLOSInfoColor);
    void refresh();
    void draw(String caption, String value, int _captionColor = OWLOSInfoColor, int _valueColor = OWLOSInfoColor);
};

#endif