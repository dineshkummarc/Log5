using System.Text;

namespace Log5
{
    using System;
    using System.IO;

    public class FileLogger : Logger
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public Stream FileStream { get; private set; }
        public Encoding Encoding { get; set; }

        public FileLogger(string path)
        {
            Encoding = DefaultEncoding;
            FileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            FileStream.Seek(0, SeekOrigin.End);
        }

        protected override void LogInternal(string logLine)
        {
            if (FileStream == null)
            {
                throw new Exception("File is not ready yet");
            }

            var outputBytes = Encoding.GetBytes(logLine);
            FileStream.Write(outputBytes, 0, outputBytes.Length);
            FileStream.Flush();
        }
    }
}