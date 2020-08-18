using System;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.IO;

namespace zbxinstall
{
    class Log
    {
        internal static async void WriteAsync()
        {
            LocalLog();
            Log send = new Log();
            await send.SendEmailAsync();
        }
        private static void LocalLog()
        {
            if (!string.IsNullOrEmpty(Logs.Log))
            {
                using (FileStream fstream = new FileStream($"{IOs.RootDir}{IOs.ZabbixAgent}\\{IOs.LogPath}", FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fstream, System.Text.Encoding.Default))
                    {
                        sw.Write(Logs.Log);
                    }
                }
            }
        }
        async Task SendEmailAsync()
        {
            Message();
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Logs.Head, Logs.SendEmail));
            emailMessage.To.Add(new MailboxAddress("", Logs.Email));
            emailMessage.Subject = Logs.Topic;
            try
            {
                emailMessage.Body = new TextPart("Plain")
                {
                    Text = Logs.Log
                };
            }
            catch
            {
                //
            }
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(Logs.SendEmail, Logs.SendEmailPassword);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
        private static void Message()
        {
            Logs.Topic = $"{Logs.HostName} установка/обновление zabbix";
            Logs.Head = $"Zabbix Install";
        }
    }
    struct Logs
    {
        internal static string Log { get; set; }
        internal static string HostName { get; } = Environment.MachineName;
        internal static string Head { get; set; }
        internal static string Topic { get; set; }
        internal static string Body { get; set; }
        internal static string SendEmail { get; } = "prtestalert@gmail.com";
        internal static string SendEmailPassword { get; } = "EFIAmors123";
        internal static string Email { get; } = "tera@1eska.ru";
    }
}
