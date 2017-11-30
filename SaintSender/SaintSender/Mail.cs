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
    [Serializable]
    public class Mail
    {
        [NonSerialized] private string filePath;
        private string text, id, label, subject, from, snippet;
        public string Text { get => text; set => text = value; }
        public string Id { get => id; set => id = value; }
        public string Label { get => label; set => label = value; }
        public string Subject { get => subject; set => subject = value; }
        public string From { get => from; set => from = value; }
        public string Snippet { get => snippet; set => snippet = value; }


        /*public string Text { get => text; set => text = value; }
public string Id { get => id; set => id = value; }
public string Label { get => label; set => label = value; }
public string Subject { get => subject; set => subject = value; }
public string From { get => from; set => from = value; }*/

        private Mail() { }
        public Mail(string text, string id, string label)
        {
            Text = text;
            Id = id;
            Label = label;
            filePath = "backup\\ " + Id + ".bin";
        }

        public void Serialize()
        {
            IFormatter formatter = new BinaryFormatter();
            var a = this;
            using(Stream stream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static Mail Deserialize(string fileName)
        {
            Mail obj;
            //string name = fileName.Replace(" ", "").Replace(".bin", "");
            string filePath = "backup\\" + fileName;
            IFormatter formatter = new BinaryFormatter();
            using(Stream stream = new FileStream(filePath, 
                FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                obj = (Mail)formatter.Deserialize(stream);
            }
            //string result = "";

            return obj;
        }

        public override string ToString()
        {
            return From + "\n\n\n\n" + Text;
        }
    }
}
