using System.Collections.Generic;
using System.Linq;
using PF3.Models;
using PF3_UI.Mortgage;

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

        public static List<PIItem> ToPiItems(this List<List<PublishMessage>> raw)
        {
            var results = new List<PIItem>();            
            foreach (var row in raw)
            {
                var principle = row.First();
                var interest = row.Last();

                var item = new PIItem
                { When = principle.When,
                  Principle = principle.Balance,
                  Interest = interest.Balance
                };
                results.Add(item);

            }
            return results;
        }
    }


}