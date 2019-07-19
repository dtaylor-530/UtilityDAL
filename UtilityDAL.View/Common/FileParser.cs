
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL.Contract;

namespace UtilityDAL.View
{
    [Description("file")]
    public abstract class FileParser : IFileParser
    {
        public virtual string Filter()
        {
            string extension = this.GetType().GetDescription();
            return "*." + extension;
        }

        public abstract ICollection Parse(string path);


        public abstract string Map(string value);

    }


    public static class AttributeHelper
    {
        public static string GetDescription(this Type value)
        {
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(value, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return  attribute.Description;
        }
    }
}
