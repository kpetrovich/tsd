using System.Windows.Forms;
using System;

namespace Aida
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBarCode = new System.Windows.Forms.TextBox();
            this.checkBoxPDF417 = new System.Windows.Forms.CheckBox();
            this.checkBoxDataMatrix = new System.Windows.Forms.CheckBox();
            this.checkBoxTovar = new System.Windows.Forms.CheckBox();
            this.checkBoxOldMark = new System.Windows.Forms.CheckBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.textBoxGood = new System.Windows.Forms.TextBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemMain = new System.Windows.Forms.MenuItem();
            this.menuItemCloseApp = new System.Windows.Forms.MenuItem();
            this.menuItemGetCurrentIP = new System.Windows.Forms.MenuItem();
            this.menuItemCheckConnections = new System.Windows.Forms.MenuItem();
            this.menuItemClearInputs = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMonth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.buttonOkData = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBoxSklad = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtBarCode
            // 
            this.txtBarCode.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtBarCode.Location = new System.Drawing.Point(3, 3);
            this.txtBarCode.Multiline = true;
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.ReadOnly = true;
            this.txtBarCode.Size = new System.Drawing.Size(474, 66);
            this.txtBarCode.TabIndex = 15;
            // 
            // checkBoxPDF417
            // 
            this.checkBoxPDF417.Enabled = false;
            this.checkBoxPDF417.Location = new System.Drawing.Point(316, 75);
            this.checkBoxPDF417.Name = "checkBoxPDF417";
            this.checkBoxPDF417.Size = new System.Drawing.Size(144, 36);
            this.checkBoxPDF417.TabIndex = 28;
            this.checkBoxPDF417.TabStop = false;
            this.checkBoxPDF417.Text = "PDF417";
            // 
            // checkBoxDataMatrix
            // 
            this.checkBoxDataMatrix.Enabled = false;
            this.checkBoxDataMatrix.Location = new System.Drawing.Point(130, 75);
            this.checkBoxDataMatrix.Name = "checkBoxDataMatrix";
            this.checkBoxDataMatrix.Size = new System.Drawing.Size(180, 36);
            this.checkBoxDataMatrix.TabIndex = 27;
            this.checkBoxDataMatrix.TabStop = false;
            this.checkBoxDataMatrix.Text = "DataMatrix";
            // 
            // checkBoxTovar
            // 
            this.checkBoxTovar.Enabled = false;
            this.checkBoxTovar.Location = new System.Drawing.Point(3, 75);
            this.checkBoxTovar.Name = "checkBoxTovar";
            this.checkBoxTovar.Size = new System.Drawing.Size(123, 36);
            this.checkBoxTovar.TabIndex = 26;
            this.checkBoxTovar.TabStop = false;
            this.checkBoxTovar.Text = "Товар";
            // 
            // checkBoxOldMark
            // 
            this.checkBoxOldMark.Location = new System.Drawing.Point(264, 417);
            this.checkBoxOldMark.Name = "checkBoxOldMark";
            this.checkBoxOldMark.Size = new System.Drawing.Size(210, 39);
            this.checkBoxOldMark.TabIndex = 29;
            this.checkBoxOldMark.Text = "Старая марка";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.textBoxLog.Location = new System.Drawing.Point(3, 165);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(472, 246);
            this.textBoxLog.TabIndex = 34;
            // 
            // textBoxGood
            // 
            this.textBoxGood.Location = new System.Drawing.Point(3, 119);
            this.textBoxGood.Name = "textBoxGood";
            this.textBoxGood.Size = new System.Drawing.Size(471, 41);
            this.textBoxGood.TabIndex = 35;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemMain);
            // 
            // menuItemMain
            // 
            this.menuItemMain.MenuItems.Add(this.menuItemCloseApp);
            this.menuItemMain.MenuItems.Add(this.menuItemGetCurrentIP);
            this.menuItemMain.MenuItems.Add(this.menuItemCheckConnections);
            this.menuItemMain.MenuItems.Add(this.menuItemClearInputs);
            this.menuItemMain.Text = "Меню";
            // 
            // menuItemCloseApp
            // 
            this.menuItemCloseApp.Text = "Выход";
            this.menuItemCloseApp.Click += new System.EventHandler(this.menuItemCloseApp_Click);
            // 
            // menuItemGetCurrentIP
            // 
            this.menuItemGetCurrentIP.Text = "Текущий IP";
            this.menuItemGetCurrentIP.Click += new System.EventHandler(this.menuItemGetCurrentIP_Click);
            // 
            // menuItemCheckConnections
            // 
            this.menuItemCheckConnections.Text = "Проверка связи";
            this.menuItemCheckConnections.Click += new System.EventHandler(this.menuItemCheckConnections_Click);
            // 
            // menuItemClearInputs
            // 
            this.menuItemClearInputs.Text = "Очистить поля ввода";
            this.menuItemClearInputs.Click += new System.EventHandler(this.menuItemClearInputs_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 29);
            this.label1.Text = "Дата розлива";
            // 
            // textBoxDay
            // 
            this.textBoxDay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxDay.Location = new System.Drawing.Point(8, 464);
            this.textBoxDay.MaxLength = 2;
            this.textBoxDay.Name = "textBoxDay";
            this.textBoxDay.Size = new System.Drawing.Size(41, 51);
            this.textBoxDay.TabIndex = 38;
            this.textBoxDay.TextChanged += new System.EventHandler(this.textBoxDay_TextChanged);
            this.textBoxDay.GotFocus += new System.EventHandler(this.textBoxDay_GotFocus);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(45, 480);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 29);
            this.label2.Text = ".";
            // 
            // textBoxMonth
            // 
            this.textBoxMonth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMonth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxMonth.Location = new System.Drawing.Point(60, 464);
            this.textBoxMonth.MaxLength = 2;
            this.textBoxMonth.Name = "textBoxMonth";
            this.textBoxMonth.Size = new System.Drawing.Size(43, 51);
            this.textBoxMonth.TabIndex = 40;
            this.textBoxMonth.TextChanged += new System.EventHandler(this.textBoxMonth_TextChanged);
            this.textBoxMonth.GotFocus += new System.EventHandler(this.textBoxMonth_GotFocus);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(99, 480);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 31);
            this.label3.Text = ".";
            // 
            // textBoxYear
            // 
            this.textBoxYear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxYear.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxYear.Location = new System.Drawing.Point(114, 464);
            this.textBoxYear.MaxLength = 2;
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(44, 51);
            this.textBoxYear.TabIndex = 43;
            this.textBoxYear.TextChanged += new System.EventHandler(this.textBoxYear_TextChanged);
            this.textBoxYear.GotFocus += new System.EventHandler(this.textBoxYear_GotFocus);
            // 
            // buttonOkData
            // 
            this.buttonOkData.Location = new System.Drawing.Point(178, 462);
            this.buttonOkData.Name = "buttonOkData";
            this.buttonOkData.Size = new System.Drawing.Size(109, 53);
            this.buttonOkData.TabIndex = 44;
            this.buttonOkData.Text = "OK";
            this.buttonOkData.Click += new System.EventHandler(this.buttonOkData_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(5, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 54);
            this.button1.TabIndex = 48;
            // 
            // comboBoxSklad
            // 
            this.comboBoxSklad.Location = new System.Drawing.Point(293, 462);
            this.comboBoxSklad.Name = "comboBoxSklad";
            this.comboBoxSklad.Size = new System.Drawing.Size(181, 41);
            this.comboBoxSklad.TabIndex = 49;
            this.comboBoxSklad.SelectedValueChanged += new System.EventHandler(this.comboBoxSklad_SelectedValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.comboBoxSklad);
            this.Controls.Add(this.buttonOkData);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.textBoxMonth);
            this.Controls.Add(this.textBoxDay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGood);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.checkBoxOldMark);
            this.Controls.Add(this.checkBoxPDF417);
            this.Controls.Add(this.checkBoxDataMatrix);
            this.Controls.Add(this.checkBoxTovar);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Fs rar";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

       
        #endregion

        private System.Windows.Forms.TextBox txtBarCode;
        private System.Windows.Forms.CheckBox checkBoxPDF417;
        private System.Windows.Forms.CheckBox checkBoxDataMatrix;
        private System.Windows.Forms.CheckBox checkBoxTovar;
        private System.Windows.Forms.CheckBox checkBoxOldMark;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.TextBox textBoxGood;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemMain;
        private System.Windows.Forms.MenuItem menuItemCloseApp;
        private System.Windows.Forms.MenuItem menuItemGetCurrentIP;
        private System.Windows.Forms.MenuItem menuItemCheckConnections;
        private System.Windows.Forms.MenuItem menuItemClearInputs;
        private Label label1;
        private TextBox textBoxDay;
        private Label label2;
        private TextBox textBoxMonth;
        private Label label3;
        private TextBox textBoxYear;
        private Button buttonOkData;
        private Button button1;
        private ComboBox comboBoxSklad;

    }
}

