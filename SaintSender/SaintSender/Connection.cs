using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace SaintSender
{
    public class Connection
    {
        private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        //private string[] Scopes = { GmailService.Scope.GmailReadonly };
        //private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private string[] Scopes2 = { GmailService.Scope.GmailSend };
        private string ApplicationName = "Gmail API .NET Quickstart";
        private string jsonName = "client_secret.json";
        private string destination = ".credentials/gmail-dotnet-quickstart.json";
        private GmailService service;
        public GmailService Service { get => service; set => service = value; }

        private MailService mailService;
        public MailService MailService { get => mailService; set => mailService = value; }

        private UserCredential credential;

        public Connection()
        {
            using (var stream =
                new FileStream(jsonName, FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, destination);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
                Service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                service = GetService();
                MailService = new MailService(service);
            }
        }
        public GmailService Sender()
        {
            GmailService ervice;
            using (var stream =
                new FileStream(jsonName, FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, destination);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes2,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
                ervice = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            return ervice;
        }


        public GmailService GetService()
        {
            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

    }
}
