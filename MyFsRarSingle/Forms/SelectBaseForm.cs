using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aida.Service;
using Aida.Model;

namespace Aida.Forms
{
    public partial class SelectBaseForm : Form
    {
        public string serverAddress;
        public BaseSettings selectedItemBase;

        public class ListItem
        {            
            public string name { get; set; }
            public BaseSettings itemList { get; set; }
        }

        public SelectBaseForm()
        {
            InitializeComponent();
        }

        private void SelectBaseForm_Load(object sender, EventArgs e)
        {
            // загрузим список баз
            var oneCService = new OneCService();
            oneCService.serverUri = serverAddress;
            
            var baseSettings = oneCService.GetBaseSettings();

            if (baseSettings.success)
            {
                listBoxBase.DisplayMember = "name";
                listBoxBase.ValueMember = "itemList";

                listBoxBase.Items.Clear();

                foreach (var item in baseSettings.settings)
                {
                    listBoxBase.Items.Add(new ListItem { name = item.name, itemList = item });
                }
            }
            else
            {                
                MessageBox.Show(baseSettings.msg);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (listBoxBase.SelectedIndex > -1)
            {
                var selItem = (ListItem)listBoxBase.SelectedItem;

                selectedItemBase = selItem.itemList;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Нужно выбрать что нибудь");
            }
        }

        
    }
}