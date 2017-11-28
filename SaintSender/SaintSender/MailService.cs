using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender
{
    class MailService
    {

        private string userId = "me";
        GmailService service;
        private List<Mail> messages;
        public List<Mail> Messages { get => messages; private set => messages = value; }


        public MailService(GmailService service)
        {
            this.service = service;
            AllMessage();
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
        private Mail SingleMessage(Message message)
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
            //return dictionary;
        } 

        private IList<Message> MessageInfo()
        {
            UsersResource.MessagesResource.ListRequest req = service.Users.Messages.List(userId);
            return req.Execute().Messages;
        }

    }
}
