using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Contract
{
    public interface IFileParser
    {

        string Filter();

        System.Collections.ICollection Parse(string path);

        string Map(string value);


    }
}
