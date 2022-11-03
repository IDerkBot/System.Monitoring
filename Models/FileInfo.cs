namespace SystemMonitoring.Models
{
    /// <summary>
	/// Информация о файле
	/// </summary>
	internal class FileInfo
	{
		/// <summary>
		/// Название файла
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// Путь к файлу
		/// </summary>
		public string Path { get; set; }
	}
}