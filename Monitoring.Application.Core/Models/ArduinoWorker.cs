using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Monitoring.Models.Entity;
using Newtonsoft.Json;

//using Task = Microsoft.Office.Interop.Word.Task;

namespace SystemMonitoringNetCore.Models;

public class ArduinoWorker : INotifyPropertyChanged
{
    private readonly SerialPort _serialPort;
    private string[] _sensorsId;
    private readonly Timer _tm = new(10000);
    public List<SensorData> SensorData = new();
    public event EventHandler Load;
    public event EventHandler Complete;

    #region Index : int - Description

    private int _index;

    /// <summary> Description </summary>
    public int Index
    {
        get => _index;
        set => SetField(ref _index, value);
    }

    #endregion Index
    
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
        try
        {
            if(!_serialPort.IsOpen)
                _serialPort.Open();
        }
        catch { /*ignored*/ }
    }

    public void Disconnect()
    {
        _serialPort.Close();
    }

    public void ReadSensors(string[] sensorsId)
    {
        _sensorsId = sensorsId;
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
        Load?.Invoke(this, EventArgs.Empty);
        if (_sensorsId.Length == _index)
        {
            Complete?.Invoke(this, EventArgs.Empty);
            Disconnect();
        }
    }

    private void ReadData()
    {
        try
        {
            var inData = _serialPort.ReadLine();
            var sensor = JsonConvert.DeserializeObject<SensorDataJson>(inData);

            var sensorDatabase = Db.DbContext.Sensors.First(x => x.Uid == sensor.uid);
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}