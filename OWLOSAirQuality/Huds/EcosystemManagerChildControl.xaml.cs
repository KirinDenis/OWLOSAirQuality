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

using OWLOSThingsManager.EcosystemExplorer;
using System.Windows;
using System.Windows.Controls;

namespace OWLOSAirQuality.Huds
{
    /// <summary>
    /// Interaction logic for EcosystemManagerChildControl.xaml
    /// </summary>
    public partial class EcosystemManagerChildControl : UserControl, IEcosystemChildControl
    {
        public EcosystemControl parentControl { get; set; }
        public EcosystemManagerChildControl(Point possiton = default(Point), Point size = default(Point))
        {
            InitializeComponent();

            parentControl = new EcosystemControl(this, possiton, size);

            //Width = size.X;
            //Height = size.Y;
        }

        public void OnParentDrag()
        {
           // ThingPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSDanger"];
           // ThingShadowPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSDangerAlpha2"];
        }

        public void OnParentDrop()
        {
            //ThingPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSWarning"];
            //ThingShadowPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"];
        }

        public void OnParentGetFocus()
        {
            //ThingPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSWarning"];
            //ThingShadowPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSWarningAlpha2"];
        }

        public void OnParentLostFocus()
        {
            //ThingPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSInfo"];
            //ThingShadowPath.Stroke = (SolidColorBrush)App.Current.Resources["OWLOSInfoAlpha2"];
        }



    }
}
