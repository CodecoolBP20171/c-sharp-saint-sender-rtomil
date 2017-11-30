using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaintSender
{
    public class Validator
    {
        public bool MailValidator(string email)
        {
            string starter = "^";
            string name = "[a-z0-9]+@";
            string provider = "[a-z]+.[a-z]+";
            string finisher = "$";
            string pattern = starter + name + provider + finisher;
            return Regex.IsMatch(email, pattern);
        }
    }
}
