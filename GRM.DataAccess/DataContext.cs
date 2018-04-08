using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GRM.Interfaces;

namespace GRM.DataAccess
{
    public class DataContext<T> : IDataContext<T>
    {
        private readonly IDictionary<string,int> _distributions = new Dictionary<string, int>{{ "digital download", 0},{"streaming",1}};
        
        // Synchronization object
        private readonly object _lock = new object();

        public IList<T> Read(string path)
        {
            var objects = new List<T>();
            lock (_lock)
            {
                using (var streamReader = new StreamReader(path))
                {
                    var propetyIndexer = GetPropertyIndex(streamReader.ReadLine());
                    while (!streamReader.EndOfStream)
                    {
                        var propertyValues = streamReader.ReadLine()?.Split('|');
                        if (propertyValues == null) continue;
                        
                        var instance = Activator.CreateInstance<T>();
                        var properties = instance.GetType().GetProperties();
                        foreach (var propertyInfo in properties)
                        {
                            var propertyIndex = propetyIndexer.FirstOrDefault(kv => kv.Key.Equals(propertyInfo.Name))
                                .Value;
                            if (propertyInfo.PropertyType.GetGenericArguments().Any(ga=>ga.IsEnum))
                            {
                                var listType = typeof(List<>);
                                var genericArgs = propertyInfo.PropertyType.GetGenericArguments();
                                var concreteType = listType.MakeGenericType(genericArgs);
                                var newList = Activator.CreateInstance(concreteType);
                                {
                                    var listValues = propertyValues[propertyIndex].Split(',');
                                    foreach (var listValue in listValues)
                                    {
                                        var enuValue = Enum.Parse(genericArgs.FirstOrDefault(), _distributions[listValue.Trim()].ToString());
                                        ((IList) newList).Add(enuValue);
                                    }
                                }

                                propertyInfo.SetValue(instance,newList);

                            }
                            else if (propertyInfo.PropertyType == typeof(DateTime?))
                            {
                                var provider = CultureInfo.InvariantCulture;
                                var format = "d MMM yyyy";
                                if (string.IsNullOrWhiteSpace(propertyValues[propertyIndex]))
                                    propertyInfo.SetValue(instance, null);
                                else
                                {
                                    if (propertyValues[propertyIndex].Split(' ')[1].Length == 4)
                                        format = "d MMMM yyyy";
                                    propertyInfo.SetValue(instance,
                                        DateTime.ParseExact(
                                            propertyValues[propertyIndex].Replace("st", string.Empty),
                                            format, provider));
                                }
                            }
                            else if (propertyInfo.PropertyType.IsEnum)
                            {
                                var enuValue = Enum.Parse(propertyInfo.PropertyType, _distributions[propertyValues[propertyIndex]].ToString());
                                propertyInfo.SetValue(instance,enuValue);
                            }
                            else
                            {
                                propertyInfo.SetValue(instance, propertyValues[propertyIndex]);
                            }
                        }

                        objects.Add(instance);
                    }
                }
            }

            return objects;

        }

        private IEnumerable<KeyValuePair<string,int>> GetPropertyIndex(string header)
        {
            var keyValues = new List<KeyValuePair<string, int>>();
            var index = 0;
            foreach (var propertyName in header.Split('|'))
            {
                keyValues.Add(new KeyValuePair<string, int>(propertyName, index));
                index++;
            }

            return keyValues;
        }
    }
}
