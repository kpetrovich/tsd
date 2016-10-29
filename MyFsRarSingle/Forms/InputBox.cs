using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aida.Forms
{
    public partial class InputBox : Form
    {
        public string serverAddress;

        public InputBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverAddress = textBoxServerAddress.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            textBoxServerAddress.Text = "192.168.1.254/test/";
        }

    }
}