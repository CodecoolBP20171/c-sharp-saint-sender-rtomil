using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newMailForm
{
    public partial class Form1 : Form
    {
        private string text, to;
        public string Text1 { get => text; set => text = value; }
        public string To { get => to; set => to = value; }

        public Form1()
        {
            InitializeComponent();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            // TODO
            // validate and verify, save into variables, close
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            // TODO
            // SAVE into drafts
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                var answer = MessageBox.Show("Warning", "Do you want to save it into the Draft folder?", MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes)
                {
                    // TODO
                    // SAVE into drafts
                }
            }
            Close();
        }

    }
}
