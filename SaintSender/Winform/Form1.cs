using SaintSender;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public partial class Form1 : Form
    {
        Dictionary<string, RichTextBox> labels;
        private int posX;
        Connection connection;
        Label selected;

        public Form1()
        {
            InitializeComponent();
            labels = new Dictionary<string, RichTextBox>();
            posX = 0;
            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var mailId = dataGridView1.Rows[e.RowIndex].Cells[0].Value as string;
            LinkedList<Label> pairs = new LinkedList<Label>();
            if (selected != null) { selected.BackColor = Color.White; }
            Label temp = NewLabel(mailId);
            temp.MouseClick += Temp_MouseClick;
            panel3.Controls.Add(temp);

            var text = connection.MailService.SingleMessage(mailId);
            RichTextBox temp2 = NewRichTextBox(text);
            panel3.Controls.Add(temp2);
            temp2.BringToFront();

            labels[mailId] = temp2;
            posX += mailId.Length*6;
            selected = temp;
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
                //Anchor = AnchorStyles.Right,
                ReadOnly = true
            };
        }

        private void Temp_MouseClick(object sender, MouseEventArgs e)
        {
            selected.BackColor = Color.White;
            selected = sender as Label;
            selected.BackColor = Color.Green;
            string id = selected.Text;
            foreach (var item in labels)
            {
                if (item.Key.Equals(id))
                {
                    item.Value.BringToFront();
                    break;
                }
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            connection = new Connection();
            var asd = await Task.Run(connection.MailService.OverviewAsync);
            for (int i = 0; i < 5; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = asd.ElementAt(i).Key;
                dataGridView1.Rows[i].Cells[1].Value = asd.ElementAt(i).Value;
            }
        }

    }
}
