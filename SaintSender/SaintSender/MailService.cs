using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaintSender
{
    public class MailService
    {
        private Dictionary<string, string> dictionary;
        private string userId = "me";
        GmailService service;
        private List<Mail> messages;
        public List<Mail> Messages { get => messages; private set => messages = value; }


        public MailService(GmailService service)
        {
            this.service = service;
            //AllMessage();
        }
        //public Dictionary<string, string> Overview()
        public async Task<Dictionary<string, string>> OverviewAsync()
        {
            var asd = MessageInfo();
            dictionary = new Dictionary<string, string>();
            int counter = 0;
            foreach (var item in asd)
            {
                if (counter == 10) { break; }
                var id = item.Id;
                var snippet = service.Users.Messages.Get(userId, id).Execute().Snippet;
                if (string.IsNullOrWhiteSpace(snippet)) { continue; }
                dictionary[id] = snippet;
                counter++;
            }
            return dictionary;
        }
        public Dictionary<string, string> Overview(string historyId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            return dictionary;
        }

        //public List<Dictionary<string, string>> AllMessage()
        public List<Mail> AllMessage()
        {
            IList<Message> tempList = MessageInfo();
            //List<Dictionary<string, string>> messageList = new List<Dictionary<string, string>>();
            messages = new List<Mail>();
            foreach (var msg in tempList)
            {
                var temp = SingleMessage(msg);
                if (temp != null) { messages.Add(temp); }
            }

            return messages;
        }

        //private Dictionary<string, string> SingleMessage(Message message)
        public Mail SingleMessage(Message message)
        {
            //Dictionary<string, string> dictionary = new Dictionary<string, string>();
            var msgReq = service.Users.Messages.Get(userId, message.Id).Execute();
            try
            {
                if (!string.IsNullOrWhiteSpace(msgReq.Payload.Parts[0].Body.Data))
                {
                    var msgBody = msgReq.Payload.Parts[0].Body.Data.Replace('-', '+').Replace('_', '/');
                    var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(msgBody));

                    Mail mail = new Mail(decoded, msgReq.Id, "inbox");
                    foreach (var item in msgReq.Payload.Headers)
                    {
                        //Console.WriteLine(item.Name + " " + item.Value);
                        //Console.WriteLine();
                        if (item.Name.Equals("Subject")) { mail.Subject = item.Value; }
                        else if (item.Name.Equals("From")) { mail.From = item.Value; }
                    }
                    return mail;
                }
            }
            catch (Exception) { Console.WriteLine("No worries"); }
            return null;
        }
            public string SingleMessage(string messageId)
            {
                //Dictionary<string, string> dictionary = new Dictionary<string, string>();
                var msgReq = service.Users.Messages.Get(userId, messageId).Execute();
                try
                {
                    if (!string.IsNullOrWhiteSpace(msgReq.Payload.Parts[0].Body.Data))
                    {
                        var msgBody = msgReq.Payload.Parts[0].Body.Data.Replace('-', '+').Replace('_', '/');
                        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(msgBody));

                        Mail mail = new Mail(decoded, msgReq.Id, "inbox");
                        foreach (var item in msgReq.Payload.Headers)
                        {
                            //Console.WriteLine(item.Name + " " + item.Value);
                            //Console.WriteLine();
                            if (item.Name.Equals("Subject")) { mail.Subject = item.Value; }
                            else if (item.Name.Equals("From")) { mail.From = item.Value; }
                        }
                        return mail.ToString();
                    }
                }
                catch (Exception) { Console.WriteLine("No worries"); }
                return null;
                //return dictionary;
            } 

        private IList<Message> MessageInfo()
        {
            UsersResource.MessagesResource.ListRequest req = service.Users.Messages.List(userId);
            return req.Execute().Messages;
        }
        public Message CraeteEmail(string To, string subject, string text)
        {
            string snippet = "";
            if (text.Length > 30)
            {
                var tempArray = text.ToCharArray();
                for (int i = 0; i < tempArray.Length/2; i++)
                {
                    snippet += tempArray[i];
                }
                snippet += "...";
            }
            string from = "kotoszo@gmail.com";
            string decoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(text));
            return new Message()
            {
                Snippet = snippet,
                Raw = decoded
            };
        }
        public void SendMail(Message message)
        {

            string plainText = "To: kotoszo@gmail.com,kotoszo@gmail.com\r\n" +
                               "Subject: subject Test\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                               "<h1>Body Test </h1>";

            var newMsg = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Base64UrlEncode(plainText.ToString())
            };
            service.Users.Messages.Send(newMsg, userId).Execute();

            /*try
            {
                service.Users.Messages.Send(message, userId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error.";
            }
            return "Done.";*/
        }
        public string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

    }
}
