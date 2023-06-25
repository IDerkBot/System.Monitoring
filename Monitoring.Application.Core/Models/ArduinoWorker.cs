using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Timers;
using Monitoring.Models.Entity;
using Newtonsoft.Json;

//using Task = Microsoft.Office.Interop.Word.Task;

namespace SystemMonitoringNetCore.Models;

public class ArduinoWorker
{
    private readonly SerialPort _serialPort;
    private string[] _sensorsId;
    private int _index;
    private readonly Timer _tm = new(10000);
    public List<SensorData> SensorData = new();
    public event EventHandler Load;
    public event EventHandler Complete;
    
    public ArduinoWorker(string port)
    {
        _serialPort = new SerialPort(port);
        _serialPort.BaudRate = 9600;
        _serialPort.Parity = Parity.None;
        _serialPort.ReadTimeout = 100;
        _serialPort.DataReceived += SerialPortOnDataReceived;
        _tm.Elapsed += TmOnElapsed;
    }

    private void TmOnElapsed(object? sender, ElapsedEventArgs e)
    {
        SerialPortOnDataReceived(null, null);
    }

    public void Connect()
    {
        _serialPort.Open();
    }

    public void Disconnect()
    {
        _serialPort.Close();
    }

    public void ReadSensors(string[] sensorsId)
    {
        _sensorsId = sensorsId;
        _index++;
        Send();
    }

    private void Send()
    {
        if(_index > _sensorsId.Length - 1) return;
        var id = _sensorsId[_index];
        _serialPort.Write(id);
        _tm.Start();
    } 
    
    private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        _index++;
        _tm.Stop();
        ReadData();
        
        Send();
        if (_sensorsId.Length == _index)
        {
            Complete?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ReadData()
    {
        try
        {
            var inData = _serialPort.ReadLine();
            var sensor = JsonConvert.DeserializeObject<SensorDataJson>(inData);
            Debug.WriteLine($"GET {sensor.uid}");
            if (Db.DbContext.Sensors.Any(x => x.Uid == sensor.uid))
            {
                // Если есть такой датчик
            }
            else
            {
                // Если нет
                Db.DbContext.Sensors.Add(new Sensor
                {
                    Uid = sensor.uid,
                    PositionX = 13,
                    PositionY = 13
                });
            }
            var sensorDatabase = new Sensor();
            SensorData.Add(new SensorData
            {
                Sensor = sensorDatabase,
                Humidity = sensor.ms / 10.0,
                Temperature = sensor.tm / 10.0,
                Acidity = sensor.ph / 10.0,
                Nitrogen = sensor.nc,
                Phosphorus = sensor.nc,
                Potassium = sensor.poc,
                Salinity = sensor.sal,
            });
        }
        catch
        {
            Debug.WriteLine("Time out");
        }
    }
    

    private static void ConvertSensor(string line)
    {
        // Sensors.Add(new Sensor(line));
    }
}