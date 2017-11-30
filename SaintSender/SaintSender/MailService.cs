using System;
using System.Text;
using Google.Apis.Gmail.v1;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace SaintSender
{
    public enum Labels { inbox, spam, trash, draft }
    public class MailService
    {
        GmailService service;
        private string userId = "me";
        private Dictionary<string, string> dictionary;
        private List<Mail> messages;
        public List<Mail> Messages { get => messages; private set => messages = value; }

        private Labels chosenLabel;
        public Labels ChosenLabel { get => chosenLabel; set => chosenLabel = value; }

        private static List<Mail> mails;
        public List<Mail> Mails { get { return mails; } private set { } }

        private bool isConnected;

        private int fromIndex, toIndex;
        public int FromIndex
        {
            get { return fromIndex; }
            set
            {
                fromIndex += value;
                if (fromIndex < 0) { fromIndex = 0; }
            }
        }
        public int ToIndex
        {
            get { return toIndex; }
            set
            {  toIndex += value;
                if (toIndex < 5) { toIndex = 5; }
            }
        }

        private List<string> labelIds;
        public MailService()
        {
            mails = new List<Mail>();
            if (!Directory.Exists("backup"))
            {
                Directory.CreateDirectory("backup");
            }
        }
        public MailService(GmailService service, bool isConnected) : this()
        {
            this.isConnected = isConnected;
            if (isConnected)
            {
                this.service = service;
                ChosenLabel = Labels.inbox;
            }
            FromIndex = 0;
            ToIndex = 5;
        }
        public async Task<Dictionary<string, string>> OverviewAsync()
        {
            dictionary = new Dictionary<string, string>();
            if (isConnected)
            {
                var messages = MsgFromGmail();
                if (messages != null)
                {
                    for (int i = FromIndex; i < toIndex; i++)
                    {
                        var id = messages[i].Id;
                        var snippet = service.Users.Messages.Get(userId, id).Execute().Snippet;

                        if (string.IsNullOrWhiteSpace(snippet)) { snippet = "Mime"; }
                        dictionary[id] = snippet;
                    }
                }
            }
            else
            {
                var messages = MsgFromBackup();
                foreach (var item in messages)
                {
                    dictionary[item.Id] = item.Snippet;
                }
            }
            return dictionary;
        }

        public void DeleteMail(string mailId)
        {
            service.Users.Messages.Delete(userId, mailId).Execute();
        }

        public Mail SingleMessage(string messageId)
        {
            if (isConnected)
            {
                var msgReq = service.Users.Messages.Get(userId, messageId).Execute();
                string decoded;
                try
                {
                    string msgBody = msgReq.Payload.Parts[0].Body.Data.Replace('-', '+').Replace('_', '/');
                    decoded = Encoding.UTF8.GetString(Convert.FromBase64String(msgBody));
                }
                catch (Exception) { decoded = "Mime"; }

                Mail mail = new Mail(decoded, msgReq.Id, "inbox");
                foreach (var item in msgReq.Payload.Headers)
                {
                    if (item.Name.Equals("Subject")) { mail.Subject = item.Value; }
                    else if (item.Name.Equals("From")) { mail.From = item.Value; }
                }
                mail.Snippet = service.Users.Messages.Get(userId, messageId).Execute().Snippet;
                mails.Add(mail);
                return mail;
            } else
            {
                return mails.Find(x => x.Id.Equals(messageId));
            }
        } 

        private IList<Message> MsgFromGmail()
        {
            UsersResource.MessagesResource.ListRequest req = service.Users.Messages.List(userId);
            string query;
            switch (ChosenLabel)
            {
                case Labels.spam:
                    query = "label:spam";
                    break;
                case Labels.trash:
                    query = "label:trash";
                    break;
                case Labels.draft:
                    query = "label:draft";
                    break;
                default:
                case Labels.inbox:
                    query = "label:inbox";
                    break;
            }
            req.Q = query;
            return req.Execute().Messages;
        }
        public Message CraeteEmail(string To, string subject, string text)
        {
            string plainText = string.Format("To: {0}\r\n" + 
                                    "Subject: {1}\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" + 
                               "{2}", To, subject, text);
            return new Message
            {
                Raw = Base64UrlEncode(plainText.ToString())
            };
        }
        public void SendMail(Message message)
        {
            service.Users.Messages.Send(message, userId).Execute();
        }
        public string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public List<Mail> MsgFromBackup()
        {
            mails = new List<Mail>();
            var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());
            DirectoryInfo dir = new DirectoryInfo(dirs[0]);
            FileInfo[] files = dir.GetFiles();
            foreach (var item in files)
            {
                mails.Add(Mail.Deserialize(item.Name));
            }
            return mails;
        }
    }
}
