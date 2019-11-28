namespace PF3_UI
{
    public static class Helper
    {
        public static decimal ToDecimal(this string value)
        {
            string v = System.Text.RegularExpressions.Regex.Replace(value, @"[^\d\.]", "");
            if (!decimal.TryParse(v, out decimal result)) return 0m;
            return result;
        }
    }
}