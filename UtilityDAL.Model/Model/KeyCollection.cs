using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Model
{
    public class KeyCollection
    {
        public KeyCollection(string key, ICollection list)
        {
            Key = key;
            Collection = list;
        }

        public string Key { get; }

        public ICollection Collection { get; }
    }
}
