using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityHelper;
using UtilityInterface;
using UtilityInterface.Generic;

namespace UtilityDAL
{

    public class TextSaveLoadService : IPermanent<string>
    {
        private readonly string _directory;

        public TextSaveLoadService(string directory)
        {
            _directory = directory;
        }

        public string Load()
        {
            return System.IO.File.ReadAllText(_directory);

        }

        public bool Save(string text)
        {
            System.IO.File.WriteAllText(_directory, (string)text);
            return true;
        }
    }

    public class TextDictionarySaveLoadService : IPermanent<IDictionary<string, string>>
    {
        private readonly string _directory;

        public TextDictionarySaveLoadService(string directory)
        {
            _directory = directory;
        }

        public IDictionary<string, string> Load()
        {
            return System.IO.Directory.GetFiles(_directory).ToDictionary(_ => System.IO.Path.GetFileNameWithoutExtension(_), _ => System.IO.File.ReadAllText(_));
        }

        public bool Save(IDictionary<string, string>  @object)
        {
            IDictionary<string, string> xx = @object as IDictionary<string, string>;
            foreach (var x in xx)
                System.IO.File.WriteAllText(System.IO.Path.Combine(_directory, x.Key + ".txt"), (string)x.Value);
            return true;
        }
    }




}
