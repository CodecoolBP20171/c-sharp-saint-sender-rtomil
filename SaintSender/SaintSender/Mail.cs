using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender
{
    public class Mail
    {
        [NonSerialized] private string filePath;
        private string text, id, label, subject, from;
        public string Text { get => text; set => text = value; }
        public string Id { get => id; set => id = value; }
        public string Label { get => label; set => label = value; }
        public string Subject { get => subject; set => subject = value; }
        public string From { get => from; set => from = value; }


        public Mail(string text, string id, string label)
        {
            Text = text;
            Id = id;
            Label = label;
            filePath = Label + "\\" + Id + ".bin";
        }

        public void Serialize()
        {
            IFormatter formatter = new BinaryFormatter();
            using(Stream stream = new FileStream(filePath, 
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static string Deserialize(string fileName)
        {
            string filePath = fileName + ".bin";
            string result = "";

            return result;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
