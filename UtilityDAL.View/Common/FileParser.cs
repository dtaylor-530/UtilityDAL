using System;
using System.Collections;
using System.ComponentModel;
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

        public virtual string Map(string value) => System.IO.Path.GetFileNameWithoutExtension(value);
    }

    public static class AttributeHelper
    {
        public static string GetDescription(this Type value)
        {
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(value, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute.Description;
        }
    }
}