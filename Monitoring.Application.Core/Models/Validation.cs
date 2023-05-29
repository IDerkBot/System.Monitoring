using System.Text.RegularExpressions;

namespace SystemMonitoringNetCore.Models;

public static class Validation
{
    public static bool IsEmail(string email)
    {
        var regex = new Regex("([a-z]|[0-9]){3,}@(gmail.com|yandex.ru|mail.com)");
        return regex.IsMatch(email);
    }


    public static bool IsPhone(string phone)
    {
        var regex = new Regex("^(\\+7|7|8)9[0-9][0-9]( |)(\\(|)[0-9][0-9][0-9](\\)|\\) | |)[0-9][0-9](-|)[0-9][0-9]");
        return regex.IsMatch(phone);
    }
}