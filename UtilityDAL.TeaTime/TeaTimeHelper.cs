using Optional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeaTime;

namespace UtilityDAL.Teatime
{
    internal class TeatimeHelper
    {
        /// <summary>
        /// Write entries to db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="id"></param>
        /// <param name="dbpath"></param>  
        /// /// <param name="recreateIfNullOrWhiteSpace">Recreates Database if null or whitespace</param>
        /// <returns>True if database appended to and not created </returns>
        public static bool ToDb<T>(IList<T> items, string id, string dbpath, bool recreateIfNullOrWhiteSpace = true) where T : struct//, IComparable
        {
            string connection = Path.Combine(dbpath, id + ".tea");
            if (File.Exists(connection))
                try
                {
                    using (var tf = TeaFile<T>.Append(connection))
                    {
                        foreach (var x in items)
                            tf.Write(x);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    if (recreateIfNullOrWhiteSpace && string.IsNullOrWhiteSpace(File.ReadAllText(connection)))
                        File.Delete(connection);
                    else
                        throw;
                }

            // create file and write values
            using (var tf = TeaFile<T>.Create(connection))
            {
                foreach (var x in items)
                    tf.Write(x);
            }
            return false;
        }

        public static void ToDb<T>(T item, string id, string dbpath) where T : struct//, IComparable
        {
            if (File.Exists(Path.Combine(dbpath, id + ".tea")))

                using (var tf = TeaFile<T>.Append(Path.Combine(dbpath, id + ".tea")))
                {
                    tf.Write(item);
                }

            else
                // create file and write values
                using (var tf = TeaFile<T>.Create(Path.Combine(dbpath, id + ".tea")))
                {
                    tf.Write(item);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="dbpath"></param>
        /// <returns>null if file/entry does not exist</returns>
        public static Option<List<T>> FromDb<T>(string id, string dbpath) where T : struct //, IChildRow //IComparable
        {
            if (File.Exists(Path.Combine(dbpath, id + ".tea")))

                using (var tf = TeaFile<T>.OpenRead(Path.Combine(dbpath, id + ".tea")))
                {
                    return Option.Some(tf.Items.ToList());
                }

            else
                return Option.None<List<T>>();
        }

        public static bool Clear(string id, string dbpath)
        {
            string path = Path.Combine(dbpath, id + ".tea");
            if (File.Exists(path))
                File.Delete(path);
            else
                return false;
            return true;
        }

        /// <summary>
        /// Returns the whole database, with exceptions from retrieving entries.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbpath"></param>
        /// <returns></returns>
        public static Dictionary<string, (Option<List<T>>, Option<Exception>)> FromDb<T>(string dbpath) where T : struct //, IChildRow //IComparable
        {
            return Directory.GetFiles(dbpath).Select(_ =>
            {
                var key = Path.GetFileNameWithoutExtension(_);
                try
                {
                    using (var tf = TeaFile<T>.OpenRead(_))
                    {
                        return (key, v: Option.Some(tf.Items.ToList()), ex: Option.None<Exception>());
                    }
                }
                catch (Exception ex)
                {
                    return (key, v: Option.None<List<T>>(), ex: Option.Some(ex));
                }
            }).ToDictionary(_ => _.key, _ => (_.v, _.ex));
        }
    }
}