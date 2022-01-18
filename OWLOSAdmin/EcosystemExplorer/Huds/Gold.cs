/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2019, 2020, 2021 by:
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

namespace OWLOSAdmin.EcosystemExplorer.Huds
{
    public static class Gold
    {
        public const double goldenRation = 1.61803398875;
        public const double baseSize = 1280.0f;
        public const double cellSize = baseSize * goldenRation * goldenRation * goldenRation * goldenRation * goldenRation;
        public const double stepSize = cellSize / goldenRation / goldenRation / goldenRation / goldenRation / goldenRation / goldenRation / goldenRation / goldenRation / goldenRation;

        public static readonly double size = 700;
        public static readonly double center = size / 2;
        public static readonly double radius = size / 2.618;
        public static readonly double radius1 = radius / goldenRation;
        public static readonly double radius2 = radius1 / goldenRation;
        public static readonly double radius3 = radius2 / goldenRation;
        public static readonly double radius4 = radius3 / goldenRation;
        public static readonly double radius5 = radius4 / goldenRation;
        public static readonly double radius6 = radius5 / goldenRation;
        public static readonly double radius7 = radius6 / goldenRation;
        public static readonly double radius8 = radius7 / goldenRation;
    }
}
