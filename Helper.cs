using System.Collections.Generic;
using System.Linq;
using PF3.Models;

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

        // Pivot from Columns > Rows to Rows > Columns
        public static List<List<PublishMessage>> Pivot(this List<IEnumerable<PublishMessage>> raw)
        {
            return raw.SelectMany((c, i) => c.Select((r, j) => new { col = i, row = j, item = r }))
                      .GroupBy(x => x.row)
                      .Select(r => r.Select(c => c.item).ToList())
                      .ToList();
            
        }        
    }


}