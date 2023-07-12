namespace Application.Helper;

public static class DateHelper
{
    public static string GetFormattedExpirationDate(int daysToExpire)
    {
        var now = DateTime.Now;
        var expirationDate = now.AddDays(daysToExpire);
        var formattedExpirationDate = expirationDate.ToString("dd-MM-yyyy");
        return formattedExpirationDate;
    }
}