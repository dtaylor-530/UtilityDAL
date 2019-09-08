using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UtilityDAL.CSV
{

    public class Dictionary<T>
    {
        const string database = @"database.csv";
        string databaseFullName => System.IO.Path.Combine(directoryName, database);
        string directoryName;
        private string key;
        private IDictionary<string, string> predicates;
        private PropertyCache<T> yu;

        public Dictionary(string key, string directoryName, IDictionary<string, string> predicates)
        {
            this.directoryName = directoryName;
            this.key = key;
            this.predicates = predicates;
            yu = new PropertyCache<T>();

            var dir = new DirectoryInfo(directoryName);
            if (!dir.Exists)
            {
                dir.Create();
            }

            if (!System.IO.File.Exists(System.IO.Path.Combine(dir.FullName, database)))
            {
                var writer = new StreamWriter(System.IO.Path.Combine(dir.FullName, database));
                Csv.CsvWriter.Write(writer, yu.PropertyNames, new string[][] { }, ',');
                writer.Close();
            }
        }
        public Dictionary(string key, IDictionary<string, string> predicates) : this(key, "../../../Data", predicates)
        {
        }
        public Dictionary(string key) : this(key, "../../../Data", null)
        {
        }
        public Dictionary(string key, string directoryName) : this(key, directoryName, null)
        {
        }

        public void Clear() => File.WriteAllText(databaseFullName, String.Empty);

        public void Insert(IEnumerable<T> data)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

            foreach (Csv.ICsvLine line in GetLines)
            {
                dict[line[key]] = line.Values;
            }

            foreach (var competition in data.GroupBy(t => yu.GetPropertyValue<string>(t, key)))
            {
                dict[competition.Key] = yu.GetValues<T>(competition.First()).ToArray();
            }

            using (var f = File.Open(databaseFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var writer = new StreamWriter(f))
            {
                Csv.CsvWriter.Write(writer, yu.PropertyNames, dict.Select(_ => _.Value), ',');
                writer.Flush();
            }
        }

        public IEnumerable<T> Retrieve() => predicates != null ?
                                                       MapByPredicates<T>(GetLines, predicates) :
                                                       GetLines.Select(Map);

        private IEnumerable<Csv.ICsvLine> GetLines => Csv.CsvReader.ReadFromText(File.ReadAllText(databaseFullName));

        private static IEnumerable<T> MapByPredicates<T>(IEnumerable<Csv.ICsvLine> lines, IDictionary<string, string> predicates)
        {
            foreach (var line in from line in lines
                                 join p in predicates
                                 on true equals true into ab
                                 from a in ab
                                 join p in predicates
                                 on line[a.Key] equals p.Value
                                 select line)
            {
                yield return Helper.Map<T>(line.Headers.Zip(line.Values, (a, b) => (a, b)).ToDictionary(c => c.a, c => (object)c.b));
            }
        }

        private static T Map(Csv.ICsvLine line) => Helper.Map<T>(line.Headers
                                                                    .Zip(line.Values, (a, b) => (a, b))
                                                                    .ToDictionary(c => c.a, c => (object)c.b));
    }

    static class Helper
    {
        public static T Map<T>(this Dictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict.Where(a => a.Value != null))
            {
                UtilityHelper.PropertyHelper.SetValue(obj, kv.Key, kv.Value);
            }
            return (T)obj;
        }
    }

    public class PropertyCache<R>
    {
        Type type;
        Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>();
        public PropertyCache()
        {
            type = typeof(R);
            dictionary = typeof(R).GetProperties().ToDictionary(a => a.Name, a => a);
        }

        public T GetPropertyValue<T>(R obj, string name) => UtilityHelper.PropertyHelper.GetPropertyValue<T>(obj, dictionary[name]);
        public IEnumerable<string> GetValues<T>(R obj) => dictionary.Select(a => a.Value.GetValue(obj).ToString());

        public IEnumerable<T> GetPropertyValues<T>(IEnumerable<R> obj, string name) => obj.Select(r => GetPropertyValue<T>(r, name));

        public string[] PropertyNames => dictionary.Keys.Cast<string>().ToArray();
    }
}
