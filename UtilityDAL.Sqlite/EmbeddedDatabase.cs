using SQLite;
using System;
using System.IO;

namespace UtilityDAL.Sqlite
{
    public class EmbeddedDatabase
    {

        public static IDisposable Get(Stream resourceStream, out SQLiteConnection conn)
        {
            string path = Path.Combine(Path.GetTempPath(), "EmbeddedDatabase.sqlite");

            using (var fileStream = File.OpenWrite(path))
            {
                CopyStream(resourceStream, fileStream);
            }


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