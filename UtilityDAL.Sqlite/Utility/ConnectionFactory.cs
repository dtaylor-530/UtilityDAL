﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL.Sqlite
{
    public class ConnectionFactory
    {
        public static SQLiteConnection Create<T>(string path = null, Func<Type, bool> func = null)
        {
            return Create(string.IsNullOrEmpty(path) ? $"../../../Data/{typeof(T).Name}.{Constants.Extension}" : path);

            Type[] GetTypes() =>
                UtilityHelper.TypeHelper
                    .GetTypesByAssembly<T>()
                    .Where(func ?? new Func<Type, bool>(type => type.GetMethods().Any() == false))
                    .ToArray();
        }



        public static SQLiteConnection Create(string path, Type[] types, Func<Type, bool> func = null)
        {
            return Create(path, GetTypes());

            Type[] GetTypes() =>
                types.SelectMany(type =>
                UtilityHelper.TypeHelper
                    .GetTypesByAssembly(type)
                    .Where(func ?? new Func<Type, bool>(t => t.GetMethods().Any() == false)))
                    .ToArray();
        }

        public static SQLiteConnection CreateTemp(params Type[] types)
        {
            return Create(System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "sqlite"), types);
        }

        public static SQLiteConnection Create(string path, params Type[] types)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            SQLiteConnection conn = new SQLiteConnection(path);

            foreach (var type in types)
            {
                conn.CreateTable(type, CreateFlags.AutoIncPK);
            }

            return conn;
        }
    }
}
