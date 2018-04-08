using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRM.Services
{
    public class ConsolePrinterService<T> : IPrinterService<T>
    {
        public string Print(IEnumerable<T> results)
        {
            var output = new StringBuilder();
            var resultType = results.GetType().GetGenericArguments().FirstOrDefault();
            if (resultType != null) output.AppendLine(string.Join("|", resultType.GetProperties().Select(p => p.Name)));
            foreach (var result in results)
            {
                var values = new List<object>();
                foreach (var propertyInfo in result.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType.GetGenericArguments().Any(ga => ga.IsEnum))
                    {
                        var enumValues = propertyInfo.GetValue(result) as IEnumerable;
                        var enumStrings = new List<string>();
                        if (enumValues != null)
                            enumStrings.AddRange(from object enumValue in enumValues select enumValue.ToString());

                        values.Add(string.Join(",", enumStrings));
                    }
                    else
                    {
                        values.Add(propertyInfo.GetValue(result));
                    }
                }
                output.AppendLine(string.Join("|", values));
            }
            return output.ToString();
        }
    }
}