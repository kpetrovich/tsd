using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using DSIC.Barcode;
using System.Net.Sockets;
using System.Web;
using Aida.Service;
using Aida.Model;
using System.IO;
using System.Text;
using System.Linq;


namespace Aida
{   

    public partial class MainForm : Form
    {        
        private string barcode;

        //private string pdf417;
        private MarkRecord1c markRec = new MarkRecord1c();
        private string dataMatrix;
        //private DateTime dateRozliv;
        private Good1C tovar = new Good1C();
        private FsRarService fsRarService = new FsRarService();
        private OneCService oneCService = new OneCService();
        private int[] Month30 = new[] { 4, 6, 9, 11 };

        //private string server1C_Uri;
        //private string IMEI;
        //private Skladi[] skladi;
        //private int fsrarTimeout;
        public BaseSettings settings;
       
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                
        public MainForm()
        {
            InitializeComponent();
        }

        public string DecodeMsg(string msg)
        {
            string rus = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮёйцукенгшщзхъфывапролджэячсмитьбю";

            while (msg.IndexOf("#@") != -1)
            {
                var substr = msg.Substring(msg.IndexOf("#@"), 4);
                int ind = Convert.ToInt32(msg.Substring(msg.IndexOf("#@") + 2, 2));
                msg = msg.Replace(substr, rus.Substring(ind, 1));
            }

            return msg;
        }
              
        public void StartTimer() 
        {
            t.Tick += new EventHandler(timerTick2);
            t.Interval = 4000;
            t.Enabled = true;
        }

        private void SetCheckBoxes() 
        {
            checkBoxPDF417.Checked = !String.IsNullOrEmpty(markRec.pdf417);
            checkBoxDataMatrix.Checked = !String.IsNullOrEmpty(dataMatrix);
            checkBoxTovar.Checked = !String.IsNullOrEmpty(tovar.goodname);

            if (checkBoxPDF417.Checked && checkBoxDataMatrix.Checked && checkBoxTovar.Checked && String.IsNullOrEmpty(markRec.daterozliv))
            {
                textBoxDay.Focus();
            }
        }

        private void ClearInputVariables()
        {
            markRec.Clear();
            dataMatrix = "";
            tovar.Clear();

            textBoxYear.Text = "";
            textBoxMonth.Text = "";
            textBoxDay.Text = "";

            textBoxLog.Text = "";
            textBoxGood.Text = "";

            checkBoxOldMark.Checked = false;

            SetCheckBoxes();
        }

        private bool CheckAllInputsVariables()
        {
            if (checkBoxOldMark.Checked)
            {
                return !String.IsNullOrEmpty(markRec.pdf417) && !String.IsNullOrEmpty(dataMatrix) && !String.IsNullOrEmpty(tovar.guid) && !String.IsNullOrEmpty(markRec.daterozliv);
            }

            return !String.IsNullOrEmpty(markRec.pdf417) && !String.IsNullOrEmpty(tovar.guid) && !String.IsNullOrEmpty(markRec.daterozliv);
        }

        private bool FirstScan()
        {
            return String.IsNullOrEmpty(markRec.pdf417) && String.IsNullOrEmpty(dataMatrix) && String.IsNullOrEmpty(tovar.guid);
        }
                
        public void CallbackScanner(Wrapper.DS_BARCODE DecordingData)
        {
            if (FirstScan())
            {
                textBoxLog.Text = "";
            }

            txtBarCode.Text = DecordingData.strBarcode;
            barcode = DecordingData.strBarcode;
                 
            // pdf417
            if (barcode.Length == 68)
            {
                //pdf417 = fsRarSrvice.CheckPDF417(barcode);
                markRec.pdf417 = fsRarService.CheckFormatPDF417(barcode);
            }            

            // ean8, ean13
            if (barcode.Length > 5 && barcode.Length <= 13) // это номенклатура. получим из 1с
            {
                textBoxGood.Text = "";
                tovar.Clear();

                tovar = oneCService.GetTovar(barcode);
                if (tovar.success)
                {                    
                    textBoxGood.Text = tovar.goodname;
                }
                else
                {
                    MessageBox.Show(tovar.msg); 
                }
            }

            // data matrix
            if(barcode.IndexOf('-') > 0)
            {
                dataMatrix = barcode;
            }

            // проверка марки в ФСРАР
            if (!String.IsNullOrEmpty(markRec.pdf417) && (!String.IsNullOrEmpty(dataMatrix) || checkBoxOldMark.Checked))
            {
                // сначала поищем в базе 1С
                textBoxLog.Text += "Ищем марку в базе 1С..." + Environment.NewLine;

                //var resMarkInfo = oneCService.GetMarkInfo(markRec.pdf417, settings.server_uri);
                
                //if (resMarkInfo.success)
                //{
                //    // есть в базе 1C                    
                //    ClearInputVariables();

                //    textBoxLog.Text = "Марка уже проверялась!!!" + Environment.NewLine;
                //    textBoxLog.Text += "Товар: " + resMarkInfo.goodname + Environment.NewLine +
                //            "Серия-номер: " + resMarkInfo.serialnumber + Environment.NewLine +
                //            "Дата розлива: " + resMarkInfo.daterozliv+ Environment.NewLine; 
                //}
                //else
                //{
                    textBoxLog.Text += "Проверяем марку в ФСРАР..." + Environment.NewLine;

                    //var checkResult = fsRarService.CheckMark(settings.imei, markRec.pdf417, dataMatrix);

                    //if (checkResult.success)
                    //{
                    //    textBoxLog.Text = "Товар: " + checkResult.Good + Environment.NewLine +
                    //        "Производиитель: " + checkResult.Producer + Environment.NewLine +
                    //        "Вид: " + checkResult.MarkType + Environment.NewLine +
                    //        "Крепость: " + checkResult.Alcpercent + " Объем: " + checkResult.Capacity + Environment.NewLine +
                    //        "Серия: " + checkResult.ShSeries + " Номер: " + checkResult.ShNumber + Environment.NewLine +
                    //        "Алко.код: " + checkResult.AlcCode + Environment.NewLine;

                    //    markRec.markserial = checkResult.ShSeries;
                    //    markRec.marknumber = checkResult.ShNumber;
                    //}
                    //else
                    //{
                    //    textBoxLog.Text += checkResult.msg + Environment.NewLine;
                    //    markRec.Clear();
                    //}
                //}
            }

            SetCheckBoxes();
            txtBarCode.Text = "";

            //DoRecordIn1c();
        }   
    
        // записываем в базу 1с
        //private void DoRecordIn1c()
        //{
        //    if (checkBoxPDF417.Checked && checkBoxTovar.Checked && !String.IsNullOrEmpty(markRec.daterozliv))
        //    {
        //        textBoxLog.Text = "Записываем в базу 1С..." + Environment.NewLine;
                                
        //        markRec.guid = tovar.guid;
        //        var recResult = oneCService.PostMarkTo1cBase(settings.server_uri, markRec);

        //        MessageBox.Show(recResult.msg);

        //        if (recResult.success)
        //        {
        //            ClearInputVariables();
        //            txtBarCode.Focus();
        //        }
        //        else 
        //        {
        //            textBoxLog.Text += "Повторите отправку в 1С..." + Environment.NewLine;
        //            buttonOkData.Focus();
        //        }
        //    }           
        //}
        
        private void timerTick2(object sender, EventArgs e) 
        {
            textBoxLog.Text = "Время ожидания ответа вышло. Попробуйте еще раз";
            txtBarCode.Text = "";
            
            menuItemCheckConnections.Enabled = true;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            //settings = SettingsService.GetSettings();

            if (!settings.success) 
            {
                MessageBox.Show(settings.msg);
                Close();
            }

            foreach (var item in settings.sklads)
            {
                comboBoxSklad.Items.Add(item.name);
            }

            
            //server1C_Uri = result.server_uri;
            //IMEI = result.IMEI;

            if (String.IsNullOrEmpty(settings.imei))
            {
                MessageBox.Show("IMEI не прописан!!!");
                Close();
            }

            if (Wrapper.DSBarcodeOpen() == Wrapper.MB_RESULT.MBRES_SUCCESS)
            {
                // SCAN Key Enable setting. #1
                Wrapper.DS3_DS5_SCANKEY stScanKey = new Wrapper.DS3_DS5_SCANKEY();
                stScanKey.bLeft = true;
                stScanKey.bMiddle = true;
                stScanKey.bRight = true;
                stScanKey.bTrigger = true;

                // SCAN Key Enable setting. #2
                Wrapper.DSBarcodeSetDS3ScanKey(stScanKey);
                // Barcode Result Data Call back function connection
                Wrapper.DSBarcodeSetCallback(CallbackScanner);
            }

            TestConnections();
        }
        

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Wrapper.DSBarcodeClose();
        }

        private void TestConnections()
        {
            menuItemCheckConnections.Enabled = false;

            textBoxLog.Text = "";

            //FsRarService fsrarService = new FsRarService();
            textBoxLog.Text += fsRarService.CheckConnection() + Environment.NewLine;
            
            Refresh();

            textBoxLog.Text += oneCService.CheckConnection() + Environment.NewLine;
            
            menuItemCheckConnections.Enabled = true;
        }              
                
        //private void buttonShowCalendar_Click(object sender, EventArgs e)
        //{
           
        //    using (FormSelectDate formC = new FormSelectDate())
        //    {
        //        if (formC.ShowDialog() == DialogResult.OK)
        //        {
        //            textBoxDateRozliv.Text = formC.date.ToShortDateString();
        //            if (formC.date == DateTime.MinValue)
        //            {
        //                markRec.daterozliv = "";
        //            }
        //            else
        //            {
        //                markRec.daterozliv = formC.date.ToString("yyyyMMdd");
        //            }

        //            Refresh();

        //            DoRecordIn1c();
        //        }
        //        else
        //        {
        //            textBoxDateRozliv.Text = "";
        //        }
        //    }
        //}
      
        private void menuItemCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemGetCurrentIP_Click(object sender, EventArgs e)
        {
            string myHost = System.Net.Dns.GetHostName();
            string myIP = null;

            for (int i = 0; i < Dns.GetHostEntry(myHost).AddressList.Length; i++)
            {
                if (Dns.GetHostEntry(myHost).AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    myIP = Dns.GetHostEntry(myHost).AddressList[i].ToString();
                }
            }
            MessageBox.Show(myIP);
        }

        private void menuItemCheckConnections_Click(object sender, EventArgs e)
        {
            TestConnections();
        }

        private void menuItemClearInputs_Click(object sender, EventArgs e)
        {
            ClearInputVariables();
            
        }

        //private bool allowKeyEntered = false;

        //private void textBoxDateRozliv_KeyDown(object sender, KeyEventArgs e)
        //{
        //    allowKeyEntered = false;

        //    // Determine whether the keystroke is a number from the top of the keyboard.
        //    if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
        //        e.KeyCode == Keys.Back || e.KeyCode == Keys.Return || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
        //    {
        //       allowKeyEntered = true;
        //    }

        //    //textBoxLog.Text += e.KeyCode.ToString()+Environment.NewLine;            
        //}

        //private void textBoxDateRozliv_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!allowKeyEntered)
        //    {
        //        // Stop the character from being entered into the control since it is non-numerical.
        //        e.Handled = true;
        //    }
        //}

        //private void textBoxDateRozliv_GotFocus(object sender, EventArgs e)
        //{
        //    if (textBoxDateRozliv.Text.Length == 0)
        //        textBoxDateRozliv.Text = DateInput.Clear();

        //    var inpt = (TextBox)sender;
        //    inpt.Select(0, 0);
        //}

        
        private void textBoxDay_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDay.Text.Length == 0)
            {
                return;
            }

            if (textBoxDay.Text.Length == 2)
            {
                if (Convert.ToInt32(textBoxDay.Text) > 31)
                {
                    textBoxDay.Text = "";
                    return;
                }

                textBoxMonth.Focus();
            }
                
        }

        private void textBoxDay_GotFocus(object sender, EventArgs e)
        {
            textBoxDay.Text = "";
        }

        private void textBoxMonth_GotFocus(object sender, EventArgs e)
        {
            textBoxMonth.Text = "";
        }        

        private void textBoxMonth_TextChanged(object sender, EventArgs e)
        {
            if (textBoxMonth.Text.Length == 0) 
            {
                return;
            }

            if (textBoxMonth.Text.Length == 2)
            {
                var cday = Convert.ToInt32(textBoxDay.Text);
                var cmonth = Convert.ToInt32(textBoxMonth.Text);

                if ((cmonth > 12) || (cday == 31 && Month30.Contains(cmonth)) || (cday == 30 && cmonth == 2)) 
                {
                    textBoxMonth.Text = "";
                    return;
                }
                                
                textBoxYear.Focus();
            }
        }

        private void textBoxYear_GotFocus(object sender, EventArgs e)
        {
            textBoxYear.Text = "";
        }

        private void textBoxYear_TextChanged(object sender, EventArgs e)
        {
            if (textBoxYear.Text.Length == 0)
            {
                return;
            }           
            
            if (textBoxYear.Text.Length == 2) 
            {
                var cday = Convert.ToInt32(textBoxDay.Text);
                var cmonth = Convert.ToInt32(textBoxMonth.Text);
                var cyear = Convert.ToInt32(textBoxYear.Text);

                if (cyear > 50 && cyear < 100) cyear = 1900 + cyear;
                if (cyear == 0) cyear = 2000;
                if (cyear > 0 && cyear <= 50) cyear = 2000 + cyear;

                if (cday == 29 && cmonth == 2 && !DateTime.IsLeapYear(cyear))
                {
                    textBoxYear.Text = "";
                    return;
                }

                buttonOkData.Focus();
            }
        }

        private void buttonOkData_Click(object sender, EventArgs e)
        {
            if (textBoxDay.Text.Length == 0)
            {
                textBoxDay.Focus();
                return;
            }

            if (textBoxMonth.Text.Length == 0)
            {
                textBoxMonth.Focus();
                return;
            }

            if (textBoxYear.Text.Length == 0)
            {
                textBoxYear.Focus();
                return;
            }

            var cday = Convert.ToInt32(textBoxDay.Text);
            var cmonth = Convert.ToInt32(textBoxMonth.Text);
            var cyear = Convert.ToInt32(textBoxYear.Text);

            if (cyear > 50 && cyear < 100) cyear = 1900 + cyear;
            if (cyear == 0) cyear = 2000;
            if (cyear > 0 && cyear <= 50) cyear = 2000 + cyear;

            markRec.daterozliv = cyear.ToString() + cmonth.ToString("D2") + cday.ToString("D2");

            //DoRecordIn1c();
        }

        private void comboBoxSklad_SelectedValueChanged(object sender, EventArgs e)
        {
            var i = settings.sklads.First(s => s.name == comboBoxSklad.SelectedItem.ToString());

            textBoxLog.Text = i.code;
        }
                           
                
    }
}