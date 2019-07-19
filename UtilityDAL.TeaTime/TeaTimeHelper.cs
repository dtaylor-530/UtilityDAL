using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime;
using UtilityDAL.Common;
using UtilityInterface;

namespace UtilityDAL.TeaTime
{


    internal  class TeatimeHelper
    {

        public static void ToDb<T>(IList<T> items, string id, string dbpath) where T : struct//, IComparable
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
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing file " + ex.Message);
                    if (string.IsNullOrWhiteSpace(File.ReadAllText(connection)))
                        System.IO.File.Delete(connection);
                    else
                        return;
                }
   
                // create file and write values
                using (var tf = TeaFile<T>.Create(connection))
                {
                    foreach (var x in items)
                        tf.Write(x);

                }
        }


        public static void ToDb<T>(T item, string id, string dbpath) where T : struct//, IComparable
        {

            if (File.Exists(Path.Combine(dbpath, id + ".tea")))
                try
                {
                    using (var tf = TeaFile<T>.Append(Path.Combine(dbpath, id + ".tea")))
                    {
                            tf.Write(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing file " + ex.Message);
                }
            else
                // create file and write values
                using (var tf = TeaFile<T>.Create(Path.Combine(dbpath, id + ".tea")))
                {
                        tf.Write(item);

                }
        }



        public static List<T> FromDb<T>(string id, string dbpath) where T : struct //, IChildRow //IComparable
        {

            if (File.Exists(Path.Combine(dbpath, id + ".tea")))
                try
                {
                    using (var tf = TeaFile<T>.OpenRead(Path.Combine(dbpath, id + ".tea")))
                    {

                        return tf.Items.ToList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file " + ex.Message);
                    return null;
                }
            else
                return null;

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

        public static Dictionary<string, List<T>> FromDb<T>(string dbpath) where T : struct //, IChildRow //IComparable
        {
            return Directory.GetFiles(dbpath).Select(_ =>
            {
                try
                {
                    using (var tf = TeaFile<T>.OpenRead(_))
                    {
                        return new { k = Path.GetFileNameWithoutExtension(_), v = tf.Items.ToList() };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file " + ex.Message);
                    return null;
                }
            }).Where(_ => _ != null).ToDictionary(_ => _.k, _ => _.v);

        }


        //public static async System.Threading.Tasks.Task<IObservable<T>> FromDbAsync<T>(string id, string dbpath) where T : struct //, IChildRow//, IComparable
        //{
        //  return await DbEx.FromDbAsync(  System.Threading.Tasks.Task.Run(() =>
        //    {
        //        if (File.Exists(Path.Combine(dbpath, id + ".tea")))
        //            try
        //            {
        //                using (var tf = TeaFile<T>.OpenRead(Path.Combine(dbpath, id + ".tea")))
        //                {

        //                    return tf.Items.ToList();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error reading file " + ex.Message);
        //                return null;
        //            }
        //        else
        //            return null;
        //    }));

        //}

        
    }
}
