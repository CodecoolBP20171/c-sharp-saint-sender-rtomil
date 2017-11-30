using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public partial class MainWindow
    {
        newMailForm.NewMailForm1 form;

        private void newBtn_Click(object sender, EventArgs e)
        {
            form = new newMailForm.NewMailForm1();
            form.FormClosed += newMailHelper;
            form.Show();
        }

        private void newMailHelper(object sender, FormClosedEventArgs e)
        {
            form = sender as newMailForm.NewMailForm1;
            if (form.IsSave || form.IsSend)
            {
                string text = form.Text1;
                string to = form.To;
                string subject = form.Subject;
                if (form.IsSend)
                {
                    var msg = connection.MailService.CraeteEmail(to, subject, text);
                    connection.MailService.SendMail(msg);
                } else
                {
                    // TODO SAVE THE GODDAMN EMAIL
                }
            }
        }

        private async void inboxBtn_Click(object sender, EventArgs e)
        {
            connection.MailService.ChosenLabel = SaintSender.Labels.inbox;
            FillOverview();
        }

        private void draftBtn_Click(object sender, EventArgs e)
        {
            connection.MailService.ChosenLabel = SaintSender.Labels.draft;
            FillOverview();
        }

        private void trashBtn_Click(object sender, EventArgs e)
        {
            connection.MailService.ChosenLabel = SaintSender.Labels.trash;
            FillOverview();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            connection.MailService.FromIndex = 5;
            connection.MailService.ToIndex = 5;
            RefreshPageIntervalLabel();
            FillOverview();
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            int temp = connection.MailService.FromIndex;
            connection.MailService.FromIndex = -5;
            if (temp != connection.MailService.FromIndex)
            {
                connection.MailService.ToIndex = -5;
                RefreshPageIntervalLabel();
                FillOverview();
            }
        }
        private void RefreshPageIntervalLabel()
        {
            label1.Text = connection.MailService.FromIndex + " - " + connection.MailService.ToIndex;
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (form.IsSave)
            {
                string text = form.Text1;
                string to = form.To;
                string subject = form.Subject;
                if (form.IsSend)
                {
                    var msg = connection.MailService.CraeteEmail(to, subject, text);
                    connection.MailService.SendMail(msg);
                }
                else
                {
                    // TODO SAVE THE GODDAMN EMAIL
                }
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (selectedMail != null && connection.IsConnected)
            {
                connection.MailService.DeleteMail(selectedMail.Id);
                FillOverview();
            }
        }

        private void archiveBtn_Click(object sender, EventArgs e)
        {
            if (selectedMail != null)
            {
                selectedMail.Serialize();
            }
        }

        private void spamLabel_Click(object sender, EventArgs e)
        {
            connection.MailService.ChosenLabel = SaintSender.Labels.spam;
            FillOverview();
        }
    }
}
