using System.Text;

namespace Log5
{
    using System;
    using System.IO;


    public partial class Logger
    {

        public class File : Base.String
        {

            public static readonly Encoding DefaultEncoding = Encoding.UTF8;

            public Stream FileStream { get; private set; }
            public Encoding Encoding { get; set; }

            public File(string path, ILogFormatter<string> logFormatter = null) : base(logFormatter)
            {
                Encoding = DefaultEncoding;
                FileStream = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                FileStream.Seek(0, SeekOrigin.End);
            }

            protected override void Log(string logLine)
            {
                if (FileStream == null)
                {
                    throw new Exception("File is not ready yet");
                }

                var outputBytes = Encoding.GetBytes(logLine);
                FileStream.Write(outputBytes, 0, outputBytes.Length);
                FileStream.Flush();
            }
        }    }

}