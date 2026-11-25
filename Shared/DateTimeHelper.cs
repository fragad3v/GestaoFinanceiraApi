namespace GestaoFinanceiraApi.Shared
{
    public class DateTimeHelper
    {
        public static string ConvertToBrazilianFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
