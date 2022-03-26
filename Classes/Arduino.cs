using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitoring.Classes
{
	internal static class Arduino
	{
		// Search Port
		internal static bool HavePorts()
		{
			return _ports.Length != 0;
		}

		private static string[] _ports => GetPorts();

		internal static string[] Ports => _ports;
		internal static string[] GetPorts()
		{
			return SerialPort.GetPortNames();
		}

		// Open Arduino Port
		internal static void OpenPort(SerialPort serialPort)
		{
			serialPort.Open();
		}
		// Read Data

	}
}
