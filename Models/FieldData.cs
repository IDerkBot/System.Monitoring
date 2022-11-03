namespace SystemMonitoring.Models
{
    /// <summary>
    /// Данные поля
    /// </summary>
    public class FieldData
    {
        /// <summary>
        /// Район поля
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Статус посадки
        /// </summary>
        public string Status { get; set; } = null;
        /// <summary>
        /// Посаженная культура
        /// </summary>
        public string Culture { get; set; } = null;
        /// <summary>
        /// Дата посадки
        /// </summary>
        public string Date { get; set; } = null;
    }
}