using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityHelper;
using UtilityInterface;

namespace UtilityDAL
{

    public class XmlSaveLoadService<T> : IPermanent<List<T>>
    {
        private readonly string _directory;
        private XMLDb xmldb;
        private string _key;

        public XmlSaveLoadService(string key, string directory)
        {
            _key = key;
            xmldb = new XMLDb(directory);
            _directory = directory;
        }

        public List<T> Load()
        {
            return System.IO.Directory.GetFiles(_directory).Where(_ => _.EndsWith("xml")).Select(_ => xmldb.Read<T>(System.IO.Path.GetFileNameWithoutExtension(_))).ToList();
        }

        public bool Save(object @object)
        {
            List<T> xx = @object as List<T>;
            foreach (var x in xx)
            {
                xmldb.Write<T>(x, FileNameCleaner.MakeValid(x.GetPropValue<string>(_key)));
            }
            return true;
        }

        public T Find(string x)
        {
            var clean = FileNameCleaner.MakeValid(x);
            if (System.IO.File.Exists(System.IO.Path.Combine(_directory, clean + ".xml")))
                return xmldb.Read<T>(System.IO.Path.GetFileNameWithoutExtension(clean));
            else
                return default(T);
        }

    }

}
