using System.Collections.Generic;
using Monitoring.Models.Entity;

namespace SystemMonitoringNetCore.Services;

public class RecommendationService
{
    public RecommendationService()
    {
        
    }

    public static string GetRecommendation(SensorData sensorData, CultureStatus cultureStatus)
    {
        var listUp = new List<string>();
        var listDown = new List<string>();
        
        if (sensorData.Acidity < cultureStatus.StartingValuePh)
        {
            // Кислотность меньше низкого порога
            listDown.Add("кислотность");
        }
        else if (sensorData.Acidity > cultureStatus.EndingValuePh)
        {
            // Кислотность больше максимального порога
            listUp.Add("кислотность");
        }
        
        if (sensorData.Humidity < cultureStatus.StartingValueHumidity)
        {
            // Меньше низкого порога
            listDown.Add("влажность");
        }
        else if (sensorData.Humidity > cultureStatus.EndingValueHumidity)
        {
            // Больше максимального порога
            listUp.Add("влажность");
        }
        
        if (sensorData.Nitrogen < cultureStatus.StartingValueNitrogen)
        {
            // Меньше низкого порога
            listDown.Add("азот");
        }
        else if (sensorData.Nitrogen > cultureStatus.EndingValueNitrogen)
        {
            // Больше максимального порога
            listUp.Add("азот");
        }
        
        if (sensorData.Phosphorus < cultureStatus.StartingValuePhosphor)
        {
            // Меньше низкого порога
            listDown.Add("фосфор");
        }
        else if (sensorData.Phosphorus > cultureStatus.EndingValuePhosphor)
        {
            // Больше максимального порога
            listUp.Add("фосфор");
        }
        
        if (sensorData.Potassium < cultureStatus.StartingValuePotassium)
        {
            // Меньше низкого порога
            listDown.Add("калий");
        }
        else if (sensorData.Potassium > cultureStatus.EndingValuePotassium)
        {
            // Больше максимального порога
            listUp.Add("калий");
        }
        
        if (sensorData.Salinity < cultureStatus.StartingValueSalinity)
        {
            // Меньше низкого порога
            listDown.Add("засоленность");
        }
        else if (sensorData.Potassium > cultureStatus.EndingValueSalinity)
        {
            // Больше максимального порога
            listUp.Add("засоленность");
        }
        
        if (sensorData.Temperature < cultureStatus.StartingValueTemperature)
        {
            // Меньше низкого порога
            listDown.Add("температура");
        }
        else if (sensorData.Temperature > cultureStatus.EndingValueTemperature)
        {
            // Больше максимального порога
            listUp.Add("температура");
        }

        return $"Выше нормы: {string.Join(", ", listUp)}\n" +
               $"Ниже нормы: {string.Join(", ", listDown)}";
    }
}