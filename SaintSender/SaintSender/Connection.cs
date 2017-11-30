using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SaintSender
{
    public class Connection
    {
        private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        //private string[] Scopes = { GmailService.Scope.GmailReadonly };
        //private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        //private string[] Scopes2 = { GmailService.Scope.GmailSend };
        private string ApplicationName = "Gmail API .NET Quickstart";
        private string jsonName = "client_secret.json";
        private string destination = ".credentials/gmail-dotnet-quickstart.json";
        private GmailService service;
        public GmailService Service { get => service; set => service = value; }

        private MailService mailService;
        public MailService MailService { get => mailService; set => mailService = value; }

        private bool isConnected;
        public bool IsConnected { get => isConnected; set => isConnected = value; }

        private UserCredential credential;

        public Connection()
        {
            try
            {
                isConnected = true;
                Online();
                
            } catch (Exception)
            {
                isConnected = false;
                Offline();
            }
        }


        private void Online()
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
                /*Service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });*/
                service = GetService();
                UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");
                IList<Label> labels = request.Execute().Labels;
                MailService = new MailService(service, IsConnected);
            }
        }
       
        private void Offline()
        {
            MailService = new MailService();
        }

        public GmailService GetService()
        {
            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void CheckConnection()
        {
            try
            {
                isConnected = true;
                Online();
            }
            catch (Exception)
            {
                isConnected = false;
                Offline();
            }
        }
    }
}
