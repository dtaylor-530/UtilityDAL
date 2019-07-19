using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace UtilityDAL.Common
{
    public static class DbEx
    {

        public static string GetDirectory(string directory = null, string extension = null, Type type = null)
        {
            if (directory == null)
                directory = Constants.DefaultDbDirectory;

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string name = type?.Name ?? Constants.DefaultDbName;
            //int i=0;
            //while (System.IO.File.Exists("../../Data/" + name + i + "." + extension))
            //    i++;

            return System.IO.Path.Combine(directory, name + "." + extension ?? Constants.DefaultDbExtension);
        }


        public static string GetDirectory<T>(string directory = null, string extension = null) => GetDirectory(directory, extension, typeof(T));


        public static string GetConnectionString(string providerName, bool thrw = false)
        {
            foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
                if (c.ProviderName == providerName)
                    return c.ConnectionString;

            // defaults to bin directory
            if (!thrw)
                return String.Empty;
            else
                throw new Exception($"add connection-string with a provider-name of '{providerName}' to the app config file or amend provider-name");

        }


        //public static async System.Threading.Tasks.Task<IObservable<T>> FromDbAsync<T>(System.Threading.Tasks.Task<List<T>> t)
        //{

        //    var x = await t;
        //    return x.ToObservable();


        //}

    }
}
