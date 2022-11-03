namespace SystemMonitoring.Models
{
	internal class Settings
	{
        /// <summary>
		/// Логин
		/// </summary>
		public string Login { get; set; }
		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Запомнить данные
		/// </summary>
		public bool Remember { get; set; } = false;
		/// <summary>
		/// Путь к файлам
		/// </summary>
		public string ReportsPath { get; set; }
	}
}