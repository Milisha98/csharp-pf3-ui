namespace PF3_UI
{
    public static class Helper
    {
        public static string FormatAsCurrency(this string value)
        {
            // First, strip and non-numeric characters
            string v = System.Text.RegularExpressions.Regex.Replace(value, @"[^\d\.]", "");
            if (decimal.TryParse(v, out decimal result))
            {
                return result.ToString("c0");
            }
            return "$0";

        }
    }
}