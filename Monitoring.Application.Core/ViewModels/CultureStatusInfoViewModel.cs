using Monitoring.Models.Entity;
using SystemMonitoringNetCore.ViewModels.Base;

namespace SystemMonitoringNetCore.ViewModels;

public class CultureStatusInfoViewModel : BaseViewModel
{
    #region Properties

    #region Status : CultureStatus - Статус культуры

    private CultureStatus _status;

    /// <summary> Статус культуры </summary>
    public CultureStatus Status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    #endregion Status

    #region Period : string - Период
    
    /// <summary> Период </summary>
    public string Period => $"{Status.StartPeriod} - {Status.EndPeriod}";

    #endregion Period
    
    #region Ph : string - Период
    
    /// <summary> Период </summary>
    public string Ph => $"{Status.StartingValuePh} - {Status.EndingValuePh}";

    #endregion Ph
    
    #region Phosphor : string - Фосфор
    
    /// <summary> Фосфор </summary>
    public string Phosphor => $"{Status.StartingValuePhosphor} - {Status.EndingValuePhosphor}";

    #endregion Phosphor
    
    #region Potassium : string - Период
    
    /// <summary> Период </summary>
    public string Potassium => $"{Status.StartingValuePotassium} - {Status.EndingValuePotassium}";

    #endregion Potassium
    
    #region Salinity : string - Период
    
    /// <summary> Период </summary>
    public string Salinity => $"{Status.StartingValueSalinity} - {Status.EndingValueSalinity}";

    #endregion Period
    
    #region Humidity : string - Период
    
    /// <summary> Период </summary>
    public string Humidity => $"{Status.StartingValueHumidity} - {Status.EndingValueHumidity}";

    #endregion Humidity
    
    #region Nitrogen : string - Период
    
    /// <summary> Период </summary>
    public string Nitrogen => $"{Status.StartingValueNitrogen} - {Status.EndingValueNitrogen}";

    #endregion Nitrogen
    
    #region Temperature : string - Период
    
    /// <summary> Период </summary>
    public string Temperature => $"{Status.StartingValueTemperature} - {Status.EndingValueTemperature}";

    #endregion Temperature

    #endregion

    public CultureStatusInfoViewModel(CultureStatus cultureStatus)
    {
        Status = cultureStatus;
    }
}