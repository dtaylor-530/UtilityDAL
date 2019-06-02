using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.Contract.NonGeneric
{

    // For mulit file based storage like CSV TeaTime


    public interface IFileDbService
    {
        System.Collections.ICollection FromDb(string name);
        bool ToDb(System.Collections.ICollection lst, string name);
        List<string> SelectIds();
    }



}
