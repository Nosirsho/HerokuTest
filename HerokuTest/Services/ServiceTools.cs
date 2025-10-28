namespace HerokuTest.Services;

public static class ServiceTools
{
    public static DateTime PasrseDate(string dateStr)
    {
        //правильный формат [23.09.2025]
        if (dateStr.Length != 12)
        {
            throw new FormatException($"Неправильный формат даты: П-р: [dd.mm.yyyy]");
        }
        string dateString = dateStr.Replace("[", "").Replace("]", "").Replace(" ", "");
        string format = "dd.MM.yyyy";

        DateTime date = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
        return date;
    }
}