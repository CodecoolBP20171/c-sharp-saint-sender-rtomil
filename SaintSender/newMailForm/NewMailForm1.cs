using System;
using SaintSender;
using System.Drawing;
using System.Windows.Forms;

namespace newMailForm
{
    public partial class NewMailForm1 : Form
    {
        Validator validator;

        private string text, to, subject;
        public string To { get => to; set => to = value; }
        public string Text1 { get => text; set => text = value; }
        public string Subject { get => subject; set => subject = value; }

        private bool isSave, isSend, isValid;
        public bool IsSave { get => isSave; set => isSave = value; }
        public bool IsSend { get => isSend; set => isSend = value; }
        public bool IsValid { get => isValid; set => isValid = value; }
        
        public NewMailForm1()
        {
            InitializeComponent();
            validator = new Validator();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            IsSave = false;
            IsSend = true;
            Memorizer(isSend);
            // TODO
            // validate and verify, save into variables, close
            Exit();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            IsSave = true;
            IsSend = false;
            Memorizer(isSend);
            // TODO
            // SAVE into drafts
            Exit();
        }
        private void Memorizer(bool isSend)
        {
            Text1 = msgBox.Text;
            Subject = subjBox.Text;
            if (isSend)
            {
                if (validator.MailValidator(toBox.Text))
                {
                    To = toBox.Text;
                    toBox.BackColor = Color.White;
                    isValid = true;
                } else
                {
                    toBox.BackColor = Color.Red;
                    isValid = false;
                }
            }
            if (IsSave) { isValid = true; }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(msgBox.Text))
            {
                var answer = MessageBox.Show("Warning", "Do you want to save it into the Draft folder?", MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes)
                {
                    IsSave = true;
                    IsSend = false;
                    Memorizer(isSend);
                }
            } else
            {
                isValid = true;
            }
            Exit();
        }
        private void Exit()
        {
            if (IsValid)
            {
                Close();
            }
        }
    }
}
