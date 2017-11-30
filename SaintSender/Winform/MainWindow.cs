using SaintSender;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public partial class MainWindow : Form
    {
        Dictionary<string, RichTextBox> labels;
        private int posX;
        Connection connection;
        Label selectedLabel;
        Mail selectedMail;
        private int tickInterval = 15;

        public MainWindow()
        {
            InitializeComponent();
            labels = new Dictionary<string, RichTextBox>();
            posX = 0;
            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Rows.Add();
            }
            timer1.Start();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var mailId = dataGridView1.Rows[e.RowIndex].Cells[0].Value as string;
                LinkedList<Label> pairs = new LinkedList<Label>();
                if (selectedLabel != null) { selectedLabel.BackColor = Color.White; }
                Label temp = NewLabel(mailId);
                temp.MouseClick += Temp_MouseClick;
                panel3.Controls.Add(temp);

                selectedMail = connection.MailService.SingleMessage(mailId);
                RichTextBox temp2 = NewRichTextBox(selectedMail.ToString());
                panel3.Controls.Add(temp2);
                temp2.BringToFront();

                labels[mailId] = temp2;
                posX += mailId.Length*6;
                selectedLabel = temp;
            }
        }
        private Label NewLabel(string mailId)
        {
            return new Label
            {
                Text = mailId,
                Location = new Point(posX, 0),
                BackColor = Color.Green
            };
        }
        private RichTextBox NewRichTextBox(string text)
        {
            return new RichTextBox
            {
                Multiline = true,
                Width = panel3.Bounds.Width,
                Height = 250,
                Location = new Point(0, 24),
                AutoSize = true,
                AcceptsTab = true,
                Text = text,
                ReadOnly = true
            };
        }

        private void Temp_MouseClick(object sender, MouseEventArgs e)
        {
            selectedLabel.BackColor = Color.White;
            selectedLabel = sender as Label;
            selectedLabel.BackColor = Color.Green;
            string id = selectedLabel.Text;
            foreach (var item in labels)
            {
                if (item.Key.Equals(id))
                {
                    item.Value.BringToFront();
                    var temp = connection.MailService.Mails.Where(x => x.Id.Equals(id));
                    selectedMail = temp.First();
                    break;
                }
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            connection = new Connection();
            connectionStatus.BackColor = connection.IsConnected ? Color.Green : Color.Red;
            RefreshPageIntervalLabel();
            FillOverview();
        }

        private async void FillOverview()
        {
            var tempDict = await Task.Run(connection.MailService.OverviewAsync);
            int round;
            if (tempDict.Count > 0)
            {
                for (int i = 0; i < tempDict.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = tempDict.ElementAt(i).Key;
                    dataGridView1.Rows[i].Cells[1].Value = tempDict.ElementAt(i).Value;
                }
                if (tempDict.Count < 5)
                {
                    round = 5 - tempDict.Count+1;
                    EmptyHandler(round);
                }
            } else
            {
                round = 0;
                EmptyHandler(round);
            }
        }
        private void EmptyHandler(int round)
        {
            for (int i = round; i < 5; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = "";
                dataGridView1.Rows[i].Cells[1].Value = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tickInterval == 0)
            {
                connection.CheckConnection();
                connectionStatus.BackColor = connection.IsConnected ? Color.Green : Color.Red;
                FillOverview();
            }
            refLabel.Text = "Refresh in: " + tickInterval;
            tickInterval -= 1;
            if (tickInterval < 0) { tickInterval = 15; }
        }
    }
}
