using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Monitoring.Models.Entity;

//using Task = Microsoft.Office.Interop.Word.Task;

namespace SystemMonitoringNetCore.Models;

public static class ArduinoWorker
{
    private static readonly SerialPort SerialPort = new SerialPort("COM8", 9600);
    private static readonly List<Sensor> Sensors = new List<Sensor>();
    private static readonly char[] Separators = { ';', '\r' };

    public static List<Sensor> GetSensors()
    {
        return Sensors;
    }

    public static void FindSensors()
    {
        if (!SerialPort.IsOpen) SerialPort.Open();
        if (SerialPort.BytesToRead == 0) return;
        for (int i = 0; i < 10; i++)
        {
            var data = SerialPort.ReadLine().Trim();
            if (!string.IsNullOrWhiteSpace(data))
                if (data.Split(Separators).Length == 5)
                {
                    if (Sensors.All(x => x.Id != int.Parse(data.Substring(0, 1))))
                    {
                        ConvertSensor(data);
                    }
                }
                else
                    i--;
            else
                i--;
        }
    }

    //public async static Task ReadAsync(this SerialPort serialPort, byte[] buffer, int offset, int count)
    //{
    //    var bytesToRead = count;
    //    var temp = new byte[count];

    //    while (bytesToRead > 0)
    //    {
    //        var readBytes = await serialPort.BaseStream.ReadAsync(temp, 0, bytesToRead);
    //        Array.Copy(temp, 0, buffer, offset + count - bytesToRead, readBytes);
    //        bytesToRead -= readBytes;
    //    }
    //}

    //public async static Task<byte[]> ReadAsync(this SerialPort serialPort, int count)
    //{
    //    var buffer = new byte[count];
    //    await serialPort.ReadAsync(buffer, 0, count);
    //    return buffer;
    //}

    private static void ConvertSensor(string line)
    {
        // Sensors.Add(new Sensor(line));
    }
}