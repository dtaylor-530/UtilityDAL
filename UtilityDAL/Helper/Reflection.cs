using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL
{
    public static class Reflection
    {
        public static Type[] ImplementsInterface(Type myType, Type intface)
        {

            // this conditional is necessary if myType can be an interface,
            // because an interface doesn't implement itself: for example,
            // typeof (IList<int>).GetInterfaces () does not contain IList<int>!
            if (myType.IsInterface && myType.IsGenericType &&
                myType.GetGenericTypeDefinition() == intface)
                return myType.GetGenericArguments();

            foreach (var i in myType.GetInterfaces())
                if (i.IsGenericType && i.GetGenericTypeDefinition() == intface)
                    return i.GetGenericArguments();

            return null;
        }

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
