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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace OWLOSThingsManager.EcosystemExplorer.Huds
{
    public static class HudLibrary
    {
        public static Point PolarToCartesian(double centerX, double centerY, double radius, double angleInDegrees)
        {
            double angleInRadians = (angleInDegrees - 90) * Math.PI / 180.0;

            Point point = new Point(centerX + (radius * Math.Cos(angleInRadians)), centerY + (radius * Math.Sin(angleInRadians)));

            return point;
        }

        public static Geometry DrawArc(double x, double y, double radius, double startAngle, double endAngle)
        {

            Point start = PolarToCartesian(x, y, radius, endAngle);
            Point end = PolarToCartesian(x, y, radius, startAngle);

            int largeArcFlag = endAngle - startAngle <= 180 ? 0 : 1;

            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            try
            {
                return Geometry.Parse(string.Join(" ", new string[11] { "M", start.X.ToString(nfi), start.Y.ToString(nfi), "A", radius.ToString(nfi), radius.ToString(nfi), "0", largeArcFlag.ToString(nfi), "0", end.X.ToString(nfi), end.Y.ToString(nfi) }));
            }
            catch
            {
                return null;
            }
        }

    }
}
