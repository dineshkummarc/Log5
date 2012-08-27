namespace Log5
{

    public partial class Logger
    {
        public class Null : Logger
        {

            // This class only ever has one instance, which is retrieved through the static
            // Instance property. All of its members are effectively static and pure - they
            // do not use any class properties, and are deterministic and thread-safe

            #region Pseudo-Static Implemenetation

            /// <summary>
            /// This variable holds the one instance of FormConverter
            /// </summary>
            public readonly static Null Instance = new Null();


            /// <summary>
            /// This constructor is private so that the only way to access
            /// it is through the Instance property
            /// </summary>
            private Null() { }

            #endregion


            public override void Log(LogEntry logEntry)
            {
                // Do nothing
            }
        }
    }
}
