namespace Log5
{

    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Security;
    using System.Text;


    public partial class Logger
    {

        public class Mail : Base.BulkString
        {

            public class Params
            {
                public string From { get; set; }
                public string To { get; set; }
                public string Subject { get; set; }
                public List<string> Servers { get; set; }
                public int Port { get; set; }
                public string UserName { get; set; }
                public SecureString Password { get; set; }
            }


            public Params MailParams;
            private SmtpClient _smtpClient;

            public Mail(Params mailParams, ILogFormatter<string> logFormatter = null) : base(logFormatter)
            {
                MailParams = mailParams;
            }


            protected override void Log(string logLine)
            {
                var message = new MailMessage(
                    MailParams.From,
                    MailParams.To,
                    MailParams.Subject,
                    body: logLine
                );

                Send(message);
            }

            protected override void Log(IList<string> logLines)
            {
                var body = new StringBuilder();
                foreach (var logLine in logLines)
                {
                    body.AppendLine(logLine);
                }

                var message = new MailMessage(
                    MailParams.From,
                    MailParams.To,
                    MailParams.Subject,
                    body: body.ToString()
                );

                Send(message);
            }


            protected void Send(MailMessage message)
            {
                if (_smtpClient == null)
                {
                    _smtpClient = new SmtpClient(MailParams.Servers[0], MailParams.Port)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(MailParams.UserName, MailParams.Password)
                    };
                }

                _smtpClient.Send(message);
            }

        }
    }

}
