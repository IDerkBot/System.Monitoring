namespace Monitoring.Models.Entity;

public class Sensor
{
    /// <summary>
    /// Идентификатор датчика
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Влажность
    /// </summary>
    public decimal Humidity { get; set; }

    /// <summary>
    /// Температура
    /// </summary>
    public decimal Temperature { get; set; }

    /// <summary>
    /// Кислотность
    /// </summary>
    public decimal Acidity { get; set; }

    /// <summary>
    /// Фосфор
    /// </summary>
    public decimal Phosphorus { get; set; }

    /// <summary>
    /// Кальций
    /// </summary>
    public decimal Calcium { get; set; }

    /// <summary>
    /// Магнезий
    /// </summary>
    public decimal Magnesium { get; set; }

    /// <summary>
    /// Калий
    /// </summary>
    public decimal Calium { get; set; }

    /// <summary>
    /// Азот
    /// </summary>
    public decimal Asot { get; set; }

    /// <summary>
    /// Рекомендации
    /// </summary>
    public string Recommendation { get; set; }
}