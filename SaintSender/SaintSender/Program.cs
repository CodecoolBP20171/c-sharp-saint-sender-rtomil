
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SaintSender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailQuickstart
{
    class Program
    {

        static void Main(string[] args)
        {

            Connection connection = new Connection();
            var service = connection.GetService();
            MailService mailService = new MailService(service);
            var result = mailService.AllMessage();
            result[0].Serialize();
            /*foreach (var item in result)
            {
                Console.WriteLine(item.Text);
            }*/

           
            //Console.ReadKey();
            

        }
    }
}