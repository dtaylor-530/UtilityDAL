using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityInterface;


namespace UtilityDAL
{

    public interface IBSONRow
    {
        int Id { get; set; }
        string Name { get; set; }
    }

}