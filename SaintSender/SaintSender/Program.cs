
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

            //var asd = connection.MailService.OverviewAsync();
            /*var mail = connection.MailService.CraeteEmail("kotoszo@gmail.com", "anyád", "LOLHABIBLAFI");
            connection.MailService.SendMail(mail);*/
            /*bool a;
            a = connection.MailService.MailValidator("kotoszo@hotmail.com");
            Console.WriteLine(a);
            a = connection.MailService.MailValidator("kot@ma.h");
            Console.WriteLine(a);
            a = connection.MailService.MailValidator("32a1sd.dwa3d1w@.h0");
            Console.WriteLine(a);
            a = connection.MailService.MailValidator("@5151.asdasd");
            Console.WriteLine(a);
            Console.WriteLine();*/

            //var service = connection.GetService();
            //MailService mailService = new MailService(service);
            //mailService.Overview();
            Console.ReadKey();
            

        }
        
    }
}