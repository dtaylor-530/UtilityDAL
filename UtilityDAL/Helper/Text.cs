using System.IO;

namespace UtilityDAL.Common
{
    //http://www.csharp-examples.net
    public static class Text
    {
        public static void ReadWriteFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.Open))
            {
                // read from file or write to file
            }
        }

        //Open existing file for reading
        public static void ReadFromFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.Open, FileAccess.Read))
            {
                // read from file
            }
        }

        //Open existing file for writing
        public static void WriteToFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.Open, FileAccess.Write))
            {
                // write to file
            }
        }

        //Open file for writing(with seek to end), if the file doesn't exist create it
        public static void OpenWriteFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.Append))
            {
                // append to file
            }
        }

        //Create new file and open it for read and write, if the file exists overwrite it

        public static void CreateNewFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.Create))
            {
                // write to just created file
            }
        }

        //Create new file and open it for read and write, if the file exists throw exception
        public static void CreateNewReadWriteFile()
        {
            using (var fileStream = new FileStream(@"c:\file.txt", FileMode.CreateNew))
            {
                // write to just created file
            }
        }

        public static void DeleteFiles()
        {
            string[] filePaths = Directory.GetFiles(@"c:\MyDir\");
            foreach (string filePath in filePaths)
                File.Delete(filePath);
        }

        // Read file using FileStream

        //First create FileStream to open a file for reading.Then call FileStream.Read in a loop until the whole file is read.Finally close the stream.

        public static byte[] ReadFileToBytes(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static void Ex()
        {
            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.
            string[] lines = { "First line", "Second line", "Third line" };
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(@"C:\Users\Public\TestFolder\WriteLines.txt", lines);

            // Example #2: Write one string to a text file.
            string text = "A class is the most powerful data type in C#. Like a structure, " +
                           "a class defines the data and behavior of the data type. ";
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);

            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls
            // IDisposable.Dispose on the stream object.
            // NOTE: do not use FileStream for text files because it writes bytes, but StreamWriter
            // encodes the output as text.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }

            // Example #4: Append new text to an existing file.
            // The using statement automatically flushes AND CLOSES the stream and calls
            // IDisposable.Dispose on the stream object.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
            {
                file.WriteLine("Fourth line");
            }
        }
    }
}