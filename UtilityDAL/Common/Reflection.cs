using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL
{
    public static class Reflection
    {


        public static Type[] GetTypesByAssembly<T>()
        {
            System.Reflection.Assembly asm = typeof(T).Assembly;
            return asm.GetTypes();


        }

        public static Type[] GetTypesByAssembly(Type t)
        {
            System.Reflection.Assembly asm = t.Assembly;
            return asm.GetTypes();


        }
    }


}
