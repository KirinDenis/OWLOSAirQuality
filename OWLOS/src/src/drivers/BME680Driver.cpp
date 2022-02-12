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

//Original BME680 used from https://github.com/adafruit/Adafruit_BME680
//Example codes used https://github.com/adafruit/Adafruit_BME680/tree/master/examples/bme680test

#include "BME680Driver.h"
#ifdef USE_BME680_DRIVER

#define DRIVER_ID "BME680"
#define BME680_LOOP_INTERVAL 2000

#define SEALEVELPRESSURE_HPA (1013.25)

bool BME680Driver::init()
{
	if (id.length() == 0)
		id = DRIVER_ID;
	BaseDriver::init(id);
	//считываем количество колонок и строк дисплея из файла или из константы (по умолчанию 20x4)
	available = false;
	//получаем I2C Slave адрес для обращения к текущему BME680 на I2C шине
	DriverPin *pinDriverInfo = getDriverPinByDriverId(id, I2CADDR_INDEX);
	if (pinDriverInfo != nullptr)
	{
		//если пользователь задал адрес, инкапсулируем класс обслуживающий BME680 и пробуем работать с через указанный порт
		BME680 = new Adafruit_BME680();

		if (BME680->begin(pinDriverInfo->driverI2CAddr))
		{

			// Set up oversampling and filter initialization
			BME680->setTemperatureOversampling(BME680_OS_8X);
			BME680->setHumidityOversampling(BME680_OS_2X);
			BME680->setPressureOversampling(BME680_OS_4X);
			BME680->setIIRFilterSize(BME680_FILTER_SIZE_3);
			BME680->setGasHeater(320, 150); // 320*C for 150 ms

#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
			debugOut("BME680", "OK", DEBUG_SUCCESS);
#endif
			available = true;
		}
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		else
		{
			debugOut("BME680", "Begin problem", DEBUG_DANGER);
		}
#endif
	}
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
	else
	{
		debugOut("BME680", "Pins problem", DEBUG_WARNING);
	}
#endif

	return available;
}

//когда сеть доступна
bool BME680Driver::begin(String _topic)
{
	BaseDriver::begin(_topic);
	setType(BME680_DRIVER_TYPE);
	setAvailable(available);
	return available;
}

bool BME680Driver::query()
{
	if (BaseDriver::query())
	{
		// Fill array of history data ---------------------------
		if (millis() >= lastHistoryMillis + historyInterval)
		{
			lastHistoryMillis = millis();
			setPressureHistoryData(atof(getPressure().c_str()));
			setAltitudeHistoryData(atof(getAltitude().c_str()));
			setTemperatureHistoryData(atof(getTemperature().c_str()));
		}
		return true;
	}
	return false;
}

//возвращает свойства драйвера BME680
String BME680Driver::getAllProperties()
{
	return BaseDriver::getAllProperties() +
		   "pressure=" + getPressure() + "//sr\n"
										 "pressurehistorydata=" +
		   getPressureHistoryData() + "//r\n"
									  "altitude=" +
		   getAltitude() + "//sr\n"
						   "altitudehistorydata=" +
		   getAltitudeHistoryData() + "//r\n"
									  "temperature=" +
		   getTemperature() + "//sr\n"
							  "temperaturehistorydata=" +
		   getTemperatureHistoryData() + "//r\n";
}
//управление свойствами BME680 драйвера
String BME680Driver::onMessage(String route, String _payload, int8_t transportMask)
{
	String result = BaseDriver::onMessage(route, _payload, transportMask);

	//обычно драйвер не управляет свойствами пинов, но в данном драйвере адрес I2C порта использован в роли Pin - для совместимости
	//с архитектурой, по этой причине необходим отдельный обработчик I2CADDR пина
	if (matchRoute(route, topic, "/setpin"))
	{
		// base is put the new address to to PinService
		result = init(); // init() get Address from PinManger
	}

	if (!result.equals(WRONG_PROPERTY_NAME))
		return result;

	if (matchRoute(route, topic, "/getpressure"))
	{
		result = onGetProperty("pressure", String(getPressure()), transportMask);
	}
	else if (matchRoute(route, topic, "/getpressurehistorydata"))
	{
		return onGetProperty("pressurehistorydata", String(getPressureHistoryData()), transportMask);
	}
	else if (matchRoute(route, topic, "/getaltitude"))
	{
		result = onGetProperty("altitude", String(getAltitude()), transportMask);
	}
	else if (matchRoute(route, topic, "/getaltitudehistorydata"))
	{
		return onGetProperty("altitudehistorydata", String(getAltitudeHistoryData()), transportMask);
	}
	else if (matchRoute(route, topic, "/gettemperature"))
	{
		result = onGetProperty("temperature", String(getTemperature()), transportMask);
	}
	else if (matchRoute(route, topic, "/gettemperaturehistorydata"))
	{
		return onGetProperty("temperaturehistorydata", String(getTemperatureHistoryData()), transportMask);
	}
	return result;
}

// Pressure ------------------------------------------------
String BME680Driver::getPressure()
{
	//если нет обслуживающего класса
	if (BME680 == nullptr)
	{
		setAvailable(false);
		pressure = "nan";

#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 object not ready", DEBUG_WARNING);
#endif
		return pressure;
	}
	//пробуем получить значение от сенсора
	float _pressure = BME680->readPressure();
	if (_pressure != _pressure) // float NAN check
	{
		//если сенсор не доступем
		setAvailable(false);
		pressure = "nan";

#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 going to NOT available now, check sensor", DEBUG_WARNING);
#endif
	}
	else
	{
		pressure = String(_pressure);
	}
#ifdef DETAILED_DEBUG
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
	debugOut(id, "BME680 pressure=" + pressure);
#endif
#endif
	return pressure;
}

// Altitude ------------------------------------------------
String BME680Driver::getAltitude()
{
	//если нет обслуживающего класса
	if (BME680 == nullptr)
	{
		setAvailable(false);
		altitude = "nan";
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 object not ready", DEBUG_WARNING);
#endif
		return altitude;
	}
	//пробуем получить значение от сенсора
	float _altitude = BME680->readAltitude(SEALEVELPRESSURE_HPA);
	if (_altitude != _altitude) // float NAN check
	{
		//если сенсор не доступем
		setAvailable(false);
		altitude = "nan";

#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 going to NOT available now, check sensor", DEBUG_WARNING);
#endif
	}
	else
	{
		altitude = String(_altitude);
	}
#ifdef DETAILED_DEBUG
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
	debugOut(id, "BME680 altitude=" + altitude);
#endif
#endif
	return altitude;
}

// Temperature ------------------------------------------------
String BME680Driver::getTemperature()
{
	//если нет обслуживающего класса
	if (BME680 == nullptr)
	{
		setAvailable(false);
		temperature = "nan";
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 object not ready", DEBUG_WARNING);
#endif
		return temperature;
	}
	//пробуем получить значение от сенсора
	float _temperature = BME680->readTemperature();
	if (_temperature != _temperature) // float NAN check
	{
		//если сенсор не доступем
		setAvailable(false);
		temperature = "nan";

#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
		debugOut(id, "BME680 going to NOT available now, check sensor", DEBUG_WARNING);
#endif
	}
	else
	{
		temperature = String(_temperature);
	}
#ifdef DETAILED_DEBUG
#if defined(DEBUG) || defined(LOGO_SCREEN_UX)
	debugOut(id, "BME680 temperature=" + temperature);
#endif
#endif
	return temperature;
}

// History data ------------------------------------------------
//получение накопленных данных о показаниях сенсора давления
String BME680Driver::getPressureHistoryData()
{
	String dataHistory = String(pressureHistoryCount) + ";";

	for (int k = 0; k < pressureHistoryCount; k++)
	{
		dataHistory += String(pressureHistoryData[k]) + ";";
	}

	return dataHistory;
}
//добавление очередного значения давления в историю
bool BME680Driver::setPressureHistoryData(float _historydata)
{
	if (isnan(_historydata))
		return false;

	if (pressureHistoryCount < historySize)
	{
		pressureHistoryData[pressureHistoryCount] = _historydata;
		pressureHistoryCount++;
	}
	else
	{
		for (int k = 1; k < historySize; k++)
		{
			pressureHistoryData[k - 1] = pressureHistoryData[k];
		}

		pressureHistoryData[historySize - 1] = _historydata;
	}
	return true;
}

// Altitude -----------------------------------------------------------------
String BME680Driver::getAltitudeHistoryData()
{
	String dataHistory = String(altitudeHistoryCount) + ";";

	for (int k = 0; k < altitudeHistoryCount; k++)
	{
		dataHistory += String(altitudeHistoryData[k]) + ";";
	}

	return dataHistory;
}

bool BME680Driver::setAltitudeHistoryData(float _historydata)
{
	if (isnan(_historydata))
		return false;

	if (altitudeHistoryCount < historySize)
	{
		altitudeHistoryData[altitudeHistoryCount] = _historydata;
		altitudeHistoryCount++;
	}
	else
	{
		for (int k = 1; k < historySize; k++)
		{
			altitudeHistoryData[k - 1] = altitudeHistoryData[k];
		}

		altitudeHistoryData[historySize - 1] = _historydata;
	}
	return true;
}

// Temperature -----------------------------------------------------------------
String BME680Driver::getTemperatureHistoryData()
{
	String dataHistory = String(temperatureHistoryCount) + ";";

	for (int k = 0; k < temperatureHistoryCount; k++)
	{
		dataHistory += String(temperatureHistoryData[k]) + ";";
	}

	return dataHistory;
}

bool BME680Driver::setTemperatureHistoryData(float _historydata)
{
	if (isnan(_historydata))
		return false;

	if (temperatureHistoryCount < historySize)
	{
		temperatureHistoryData[temperatureHistoryCount] = _historydata;
		temperatureHistoryCount++;
	}
	else
	{
		for (int k = 1; k < historySize; k++)
		{
			temperatureHistoryData[k - 1] = temperatureHistoryData[k];
		}

		temperatureHistoryData[historySize - 1] = _historydata;
	}
	return true;
}

#endif
