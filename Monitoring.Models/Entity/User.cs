namespace Monitoring.Models.Entity;

/// <summary>
/// Таблица пользователей
/// </summary>
public class User
{
    public int Id { get; set; }
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// Уровень доступа // ToDo: поменять с int на byte (меньше занимает памяти)
    /// </summary>
    public int Access { get; set; }
}