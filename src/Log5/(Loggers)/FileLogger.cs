using System.Text;

namespace Log5
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;


    public partial class Logger
    {

        public class File : Base.String, IDisposable
        {

            private static readonly Regex ReFormatParameter = new Regex(@"\{0\}", RegexOptions.Compiled);

            public static readonly Encoding DefaultEncoding = Encoding.UTF8;



            public string Path { get; private set; }
            public string Directory { get; private set; }
            public string Name { get; private set; }

            public bool Rotating { get; private set; }
            public long MaximumLength { get; private set; }
            public bool Running { get; private set; }
            public Encoding Encoding { get; set; }


            #region Private Fields

            private Stream _fileStream;
            private long _currentFileLength;
            private int _rotationNumber;

            #endregion


            #region Locking Fields

            private readonly object _runLock;
            private bool _runStarting;
            private bool _runStopping;
            private int _runningLogs;

            #endregion


            public File(string path, ILogFormatter<string> logFormatter = null, long maximumSize = 0) : base(logFormatter)
            {
                var rotating = maximumSize != 0;

                if (rotating && !ReFormatParameter.IsMatch(path))
                {
                    throw new ArgumentException(
                        "The rotating option was specified, but the Path " +
                        "parameter does not have a format parameter", "path"
                    );
                }

                Encoding = DefaultEncoding;
                Path = path;
                Rotating = rotating;
                MaximumLength = maximumSize;
                Running = false;
                _runLock = new object();
                _runningLogs = 0;

                // Break the path up into directory and file name
                Directory = System.IO.Path.GetDirectoryName(Path);
                Name = System.IO.Path.GetFileName(Path);

                if (Directory == null || Name == null)
                {
                    throw new ArgumentException(
                        "Path could not be broken up into " +
                        "directory and file", "path"
                    );
                }

                if (rotating && !ReFormatParameter.IsMatch(Name))
                {
                    throw new ArgumentException(
                        "The format parameter must be in the filename " +
                        "portion of the path", "path"
                    );
                }
            }


            public void Start()
            {
                lock (_runLock)
                {
                    if (Running || _runStarting)
                    {
                        return;
                    }

                    _runStarting = true;
                }

                string path = null;

                if (Rotating)
                {
                    var re = new Regex("^" + Regex.Escape(Name).Replace("\\{0\\}", "\\d+") + "$", RegexOptions.Compiled);

                    var files = System.IO.Directory.GetFiles(Directory);
                    var log = new bool[files.Length];

                    for (var i = 0; i < files.Length; i++)
                    {
                        var m = re.Match(files[i]);

                        if (m.Success)
                        {
                            log[i] = true;
                        }
                    }

                    for (var i = files.Length; i >= 0; i--)
                    {
                        if (log[i])
                        {
                            var fileInfo = new FileInfo(files[i]);

                            if (fileInfo.Length >= MaximumLength)
                            {
                                i += 1;
                            }

                            _rotationNumber = i;
                            path = Directory + String.Format(Name, i.ToString("2N"));
                        }
                    }

                    // It should be impossible for path to be null at this point
                }
                else
                {
                    path = Path;
                }

                _fileStream = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                _fileStream.Seek(0, SeekOrigin.End);
                _currentFileLength = _fileStream.Length;

                lock (_runLock)
                {
                    Running = true;
                    _runStarting = false;
                }
            }


            public void Stop()
            {
                lock (_runLock)
                {
                    if (!Running || _runStopping)
                    {
                        return;
                    }

                    _runStopping = true;

                    // Anything related to n is for testing
                    var n = 0;

                    while (Interlocked.CompareExchange(ref _runningLogs, 0, 0) != 0)
                    {
                        Thread.Sleep(20);
                        n++;

                        if (n > 20)
                        {
                            // Uh-oh
                            System.Console.WriteLine("FileLogger.Stop: Slept more than 20 times waiting for running logs to finish");
                            break;
                        }
                    }
                }

                _fileStream.Close();
                _fileStream = null;

                lock (_runLock)
                {
                    Running = false;
                    _runStopping = false;
                }
            }


            protected override void Log(string logLine)
            {
                lock (_runLock)
                {
                    if (!Running)
                    {
                        throw new InvalidOperationException("Logger is not started yet");
                    }

                    if (_runStopping)
                    {
                        return;
                    }

                    Interlocked.Increment(ref _runningLogs);
                }
                try
                {
                    var outputBytes = Encoding.GetBytes(logLine);
                    _fileStream.Write(outputBytes, 0, outputBytes.Length);
                    _currentFileLength += outputBytes.Length;
                    _fileStream.Flush();

                    if (Rotating && _currentFileLength > MaximumLength)
                    {
                        // The new file
                        _rotationNumber += 1;
                        var path = Directory + String.Format(Name, _rotationNumber.ToString("2N"));

                        _fileStream.Close();
                        _fileStream = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        _fileStream.Seek(0, SeekOrigin.End);
                        _currentFileLength = _fileStream.Length;
                    }
                }
                finally
                {
                    Interlocked.Decrement(ref _runningLogs);
                }
            }


            #region Implementation of IDisposable

            /// <summary>
            /// Performs application-defined tasks associated with freeing,
            /// releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                Stop();
            }

            #endregion
        }   
    }
}
