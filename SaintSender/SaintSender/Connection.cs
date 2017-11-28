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
        private string[] Scopes = { GmailService.Scope.GmailReadonly };
        private string ApplicationName = "Gmail API .NET Quickstart";
        private string jsonName = "client_secret.json";
        private string destination = ".credentials/gmail-dotnet-quickstart.json";

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
            }
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
