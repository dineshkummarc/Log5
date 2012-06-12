using System.Text;

namespace Log5
{
    using System;
    using System.IO;

    public class FileLogger : Logger
    {
        public Stream FileStream { get; private set; }

        public FileLogger(string path)
        {
            FileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            FileStream.Seek(0, SeekOrigin.End);
        }

        public override void Log(LogLevel logLevel, string msg)
        {
            if (FileStream == null)
            {
                throw new Exception("File is not ready yet");
            }

            var dateString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff");

            var outputString = String.Format("[{0,-5} : {1}] {2}\n", dateString, logLevel, msg);
            var outputBytes = Encoding.UTF8.GetBytes(outputString);
            FileStream.Write(outputBytes, 0, outputBytes.Length);
            FileStream.Flush();
        }
    }
}