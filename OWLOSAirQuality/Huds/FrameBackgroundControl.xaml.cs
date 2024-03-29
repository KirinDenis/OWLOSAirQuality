﻿/* ----------------------------------------------------------------------------
OWLOS DIY Open Source OS for building IoT ecosystems
Copyright 2019, 2020 by:
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

using System.Windows.Controls;

namespace OWLOSAirQuality.Huds
{
    /// <summary>
    /// Interaction logic for FrameBackgroundControl.xaml
    /// </summary>
    public partial class FrameBackgroundControl : UserControl
    {
        public string TitleFrame
        {
            get => _TitleFrame != null ? _TitleFrame.Text : string.Empty;
            set
            {
                if (_TitleFrame != null)
                {
                    _TitleFrame.Text = value;
                }
            }
        }

        public string DescriptionFrame
        {
            get => _DescriptionFrame != null ? _DescriptionFrame.Text : string.Empty;
            set
            {
                if (_DescriptionFrame != null)
                {
                    _DescriptionFrame.Text = value;
                }
            }
        }

        public string QueryInterval
        {
            get => _QueryInterval != null ? _QueryInterval.Text : string.Empty;
            set
            {
                if (_QueryInterval != null)
                {
                    _QueryInterval.Text = value;
                }
            }
        }

        public string Status
        {
            get => _Status != null ? _Status.Text : string.Empty;
            set
            {
                if (_Status != null)
                {
                    _Status.Text = value;
                }
            }
        }

        public string URL
        {
            get => _URL != null ? _URL.Text : string.Empty;
            set
            {
                if (_URL != null)
                {
                    _URL.Text = value;
                }
            }
        }





        public FrameBackgroundControl()
        {
            InitializeComponent();
        }
    }
}
