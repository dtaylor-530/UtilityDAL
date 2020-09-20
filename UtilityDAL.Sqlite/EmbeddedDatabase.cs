using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UtilityDAL.Sqlite
{
    public class EmbeddedDatabase
    {

        public static IDisposable Get(Stream resourceStream, out SQLiteConnection conn, bool disposeResourceStream = true)
        {
            var dir = Directory.CreateDirectory(Path.GetTempPath() + "/EmbeddedDatabase").FullName;
            string path = Path.GetFileNameWithoutExtension(dir) + ".sqlite";

            using (var fileStream = File.OpenWrite(path))
            {
                CopyStream(resourceStream, fileStream);
            }
            if (disposeResourceStream)
                resourceStream.Dispose();

            conn = new SQLiteConnection(path);

            void CopyStream(Stream inputStream, Stream outputStream, int bufferLength = 4096)
            {
                var buffer = new byte[bufferLength];
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, bufferLength)) > 0)
                {
                    outputStream.Write(buffer, 0, bytesRead);
                }
            }

            return new Disposable(conn, path);
        }

        public static Stream FindEmbeddedResourceStream(string fileName, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            var stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }

        class Disposable : IDisposable
        {
            private readonly SQLiteConnection conn;
            private readonly string path;

            public Disposable(SQLiteConnection conn, string path)
            {
                this.conn = conn;
                this.path = path;
            }

            public void Dispose()
            {
                conn.Close();
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

    }
}