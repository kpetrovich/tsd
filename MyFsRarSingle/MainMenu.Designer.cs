namespace Aida
{
    partial class MainMenuForm
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
            this.buttonCheckPrice = new System.Windows.Forms.Button();
            this.buttonCenniki = new System.Windows.Forms.Button();
            this.buttonPrihod = new System.Windows.Forms.Button();
            this.buttonInventarka = new System.Windows.Forms.Button();
            this.buttonCheckMark = new System.Windows.Forms.Button();
            this.labelMain = new System.Windows.Forms.Label();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.listBoxSklads = new System.Windows.Forms.ListBox();
            this.contextMenuMain = new System.Windows.Forms.ContextMenu();
            this.menuItemCheckConnection1C = new System.Windows.Forms.MenuItem();
            this.menuItemGetIP = new System.Windows.Forms.MenuItem();
            this.menuItemChangeServer = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.buttonMain = new System.Windows.Forms.Button();
            this.dataGridGoods = new System.Windows.Forms.DataGrid();
            this.contextMenuLostPriceTable = new System.Windows.Forms.ContextMenu();
            this.menuItemDeleteRowLostPriceTable = new System.Windows.Forms.MenuItem();
            this.dataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.contextMenuLostPrice = new System.Windows.Forms.ContextMenu();
            this.menuItemClearLostPriceDataGrid = new System.Windows.Forms.MenuItem();
            this.menuItemUploadLostPriceTo1C = new System.Windows.Forms.MenuItem();
            this.menuItemBackToMainMenu = new System.Windows.Forms.MenuItem();
            this.listBoxSupplyLayer = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonОК = new System.Windows.Forms.Button();
            this.contextMenuSupply = new System.Windows.Forms.ContextMenu();
            this.menuItemSupplyUpload1C = new System.Windows.Forms.MenuItem();
            this.menuItemSetGoodCountToZero = new System.Windows.Forms.MenuItem();
            this.menuItemSupplyExit = new System.Windows.Forms.MenuItem();
            this.dataGridSupplyList = new System.Windows.Forms.DataGrid();
            this.contextMenuSupplyOperations = new System.Windows.Forms.ContextMenu();
            this.menuItemNewSupply = new System.Windows.Forms.MenuItem();
            this.menuItemEditSupply = new System.Windows.Forms.MenuItem();
            this.labelGoodCount = new System.Windows.Forms.Label();
            this.textBoxGoodCount = new System.Windows.Forms.TextBox();
            this.labelGettingGoodsList = new System.Windows.Forms.Label();
            this.buttonDeleteApplication = new System.Windows.Forms.Button();
            this.buttonOkData = new System.Windows.Forms.Button();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.textBoxMonth = new System.Windows.Forms.TextBox();
            this.textBoxDay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGood = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.checkBoxOldMark = new System.Windows.Forms.CheckBox();
            this.checkBoxPDF417 = new System.Windows.Forms.CheckBox();
            this.checkBoxDataMatrix = new System.Windows.Forms.CheckBox();
            this.checkBoxTovar = new System.Windows.Forms.CheckBox();
            this.txtBarCode = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuCheckMark = new System.Windows.Forms.ContextMenu();
            this.menuItemCheckFsrar = new System.Windows.Forms.MenuItem();
            this.menuItemExitMark = new System.Windows.Forms.MenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxCheckOnline = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonCheckPrice
            // 
            this.buttonCheckPrice.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.buttonCheckPrice.Location = new System.Drawing.Point(24, 70);
            this.buttonCheckPrice.Name = "buttonCheckPrice";
            this.buttonCheckPrice.Size = new System.Drawing.Size(432, 66);
            this.buttonCheckPrice.TabIndex = 0;
            this.buttonCheckPrice.Text = "Проверить цену";
            this.buttonCheckPrice.Click += new System.EventHandler(this.buttonCheckPrice_Click);
            // 
            // buttonCenniki
            // 
            this.buttonCenniki.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.buttonCenniki.Location = new System.Drawing.Point(24, 160);
            this.buttonCenniki.Name = "buttonCenniki";
            this.buttonCenniki.Size = new System.Drawing.Size(432, 66);
            this.buttonCenniki.TabIndex = 1;
            this.buttonCenniki.Text = "Ценники";
            this.buttonCenniki.Click += new System.EventHandler(this.buttonCenniki_Click);
            // 
            // buttonPrihod
            // 
            this.buttonPrihod.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.buttonPrihod.Location = new System.Drawing.Point(24, 250);
            this.buttonPrihod.Name = "buttonPrihod";
            this.buttonPrihod.Size = new System.Drawing.Size(432, 66);
            this.buttonPrihod.TabIndex = 2;
            this.buttonPrihod.Text = "Приход";
            this.buttonPrihod.Click += new System.EventHandler(this.buttonPrihod_Click);
            // 
            // buttonInventarka
            // 
            this.buttonInventarka.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.buttonInventarka.Location = new System.Drawing.Point(24, 341);
            this.buttonInventarka.Name = "buttonInventarka";
            this.buttonInventarka.Size = new System.Drawing.Size(432, 66);
            this.buttonInventarka.TabIndex = 3;
            this.buttonInventarka.Text = "Инвентаризация";
            this.buttonInventarka.Click += new System.EventHandler(this.buttonInventarka_Click);
            // 
            // buttonCheckMark
            // 
            this.buttonCheckMark.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.buttonCheckMark.Location = new System.Drawing.Point(24, 430);
            this.buttonCheckMark.Name = "buttonCheckMark";
            this.buttonCheckMark.Size = new System.Drawing.Size(432, 66);
            this.buttonCheckMark.TabIndex = 4;
            this.buttonCheckMark.Text = "Проверка марки";
            this.buttonCheckMark.Click += new System.EventHandler(this.buttonCheckMark_Click);
            // 
            // labelMain
            // 
            this.labelMain.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.labelMain.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelMain.Location = new System.Drawing.Point(24, 11);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(432, 46);
            this.labelMain.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.textBoxResult.Location = new System.Drawing.Point(2, 266);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(474, 249);
            this.textBoxResult.TabIndex = 7;
            // 
            // listBoxSklads
            // 
            this.listBoxSklads.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
            this.listBoxSklads.Location = new System.Drawing.Point(3, 2);
            this.listBoxSklads.Name = "listBoxSklads";
            this.listBoxSklads.Size = new System.Drawing.Size(473, 258);
            this.listBoxSklads.TabIndex = 5;
            // 
            // contextMenuMain
            // 
            this.contextMenuMain.MenuItems.Add(this.menuItemCheckConnection1C);
            this.contextMenuMain.MenuItems.Add(this.menuItemGetIP);
            this.contextMenuMain.MenuItems.Add(this.menuItemChangeServer);
            this.contextMenuMain.MenuItems.Add(this.menuItemExit);
            // 
            // menuItemCheckConnection1C
            // 
            this.menuItemCheckConnection1C.Text = "Проверить связь с 1С";
            this.menuItemCheckConnection1C.Click += new System.EventHandler(this.menuItemCheckConnection1C_Click);
            // 
            // menuItemGetIP
            // 
            this.menuItemGetIP.Text = "Показать IP адрес";
            this.menuItemGetIP.Click += new System.EventHandler(this.menuItemGetIP_Click);
            // 
            // menuItemChangeServer
            // 
            this.menuItemChangeServer.Text = "Сменить сервер";
            this.menuItemChangeServer.Click += new System.EventHandler(this.menuItemChangeServer_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Text = "Выход";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // buttonMain
            // 
            this.buttonMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonMain.Location = new System.Drawing.Point(48, 522);
            this.buttonMain.Name = "buttonMain";
            this.buttonMain.Size = new System.Drawing.Size(385, 62);
            this.buttonMain.TabIndex = 8;
            this.buttonMain.Text = "Меню";
            this.buttonMain.Click += new System.EventHandler(this.buttonMain_Click);
            // 
            // dataGridGoods
            // 
            this.dataGridGoods.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridGoods.ContextMenu = this.contextMenuLostPriceTable;
            this.dataGridGoods.Location = new System.Drawing.Point(3, 3);
            this.dataGridGoods.Name = "dataGridGoods";
            this.dataGridGoods.Size = new System.Drawing.Size(474, 514);
            this.dataGridGoods.TabIndex = 10;
            this.dataGridGoods.TableStyles.Add(this.dataGridTableStyle);
            // 
            // contextMenuLostPriceTable
            // 
            this.contextMenuLostPriceTable.MenuItems.Add(this.menuItemDeleteRowLostPriceTable);
            // 
            // menuItemDeleteRowLostPriceTable
            // 
            this.menuItemDeleteRowLostPriceTable.Text = "Удалить строку";
            this.menuItemDeleteRowLostPriceTable.Click += new System.EventHandler(this.menuItemDeleteRowLostPriceTable_Click);
            // 
            // contextMenuLostPrice
            // 
            this.contextMenuLostPrice.MenuItems.Add(this.menuItemClearLostPriceDataGrid);
            this.contextMenuLostPrice.MenuItems.Add(this.menuItemUploadLostPriceTo1C);
            this.contextMenuLostPrice.MenuItems.Add(this.menuItemBackToMainMenu);
            // 
            // menuItemClearLostPriceDataGrid
            // 
            this.menuItemClearLostPriceDataGrid.Text = "Очистить";
            this.menuItemClearLostPriceDataGrid.Click += new System.EventHandler(this.menuItemClearLostPriceDataGrid_Click);
            // 
            // menuItemUploadLostPriceTo1C
            // 
            this.menuItemUploadLostPriceTo1C.Text = "Выгрузить в 1С";
            this.menuItemUploadLostPriceTo1C.Click += new System.EventHandler(this.menuItemUploadLostPriceTo1C_Click);
            // 
            // menuItemBackToMainMenu
            // 
            this.menuItemBackToMainMenu.Text = "Назад в меню";
            this.menuItemBackToMainMenu.Click += new System.EventHandler(this.menuItemBackToMainMenu_Click);
            // 
            // listBoxSupplyLayer
            // 
            this.listBoxSupplyLayer.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
            this.listBoxSupplyLayer.Location = new System.Drawing.Point(6, 5);
            this.listBoxSupplyLayer.Name = "listBoxSupplyLayer";
            this.listBoxSupplyLayer.Size = new System.Drawing.Size(469, 466);
            this.listBoxSupplyLayer.TabIndex = 12;
            this.listBoxSupplyLayer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxSupplyLayer_KeyUp);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(24, 521);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(178, 62);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonОК
            // 
            this.buttonОК.Location = new System.Drawing.Point(278, 521);
            this.buttonОК.Name = "buttonОК";
            this.buttonОК.Size = new System.Drawing.Size(178, 62);
            this.buttonОК.TabIndex = 14;
            this.buttonОК.Text = "Выбрать";
            this.buttonОК.Click += new System.EventHandler(this.buttonОК_Click);
            // 
            // contextMenuSupply
            // 
            this.contextMenuSupply.MenuItems.Add(this.menuItemSupplyUpload1C);
            this.contextMenuSupply.MenuItems.Add(this.menuItemSetGoodCountToZero);
            this.contextMenuSupply.MenuItems.Add(this.menuItemSupplyExit);
            // 
            // menuItemSupplyUpload1C
            // 
            this.menuItemSupplyUpload1C.Text = "Выгрузить в 1С";
            this.menuItemSupplyUpload1C.Click += new System.EventHandler(this.menuItemSupplyUpload1C_Click);
            // 
            // menuItemSetGoodCountToZero
            // 
            this.menuItemSetGoodCountToZero.Text = "Обнулить строку";
            this.menuItemSetGoodCountToZero.Click += new System.EventHandler(this.menuItemSetGoodCountToZero_Click);
            // 
            // menuItemSupplyExit
            // 
            this.menuItemSupplyExit.Text = "Назад в меню";
            this.menuItemSupplyExit.Click += new System.EventHandler(this.menuItemSupplyExit_Click);
            // 
            // dataGridSupplyList
            // 
            this.dataGridSupplyList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridSupplyList.Location = new System.Drawing.Point(3, 5);
            this.dataGridSupplyList.Name = "dataGridSupplyList";
            this.dataGridSupplyList.PreferredRowHeight = 50;
            this.dataGridSupplyList.Size = new System.Drawing.Size(472, 447);
            this.dataGridSupplyList.TabIndex = 16;
            this.dataGridSupplyList.Click += new System.EventHandler(this.dataGridSupplyList_Click);
            // 
            // contextMenuSupplyOperations
            // 
            this.contextMenuSupplyOperations.MenuItems.Add(this.menuItemNewSupply);
            this.contextMenuSupplyOperations.MenuItems.Add(this.menuItemEditSupply);
            // 
            // menuItemNewSupply
            // 
            this.menuItemNewSupply.Text = "Новый документ";
            this.menuItemNewSupply.Click += new System.EventHandler(this.menuItemNewSupply_Click);
            // 
            // menuItemEditSupply
            // 
            this.menuItemEditSupply.Text = "Редактировать";
            this.menuItemEditSupply.Click += new System.EventHandler(this.menuItemEditSupply_Click);
            // 
            // labelGoodCount
            // 
            this.labelGoodCount.Location = new System.Drawing.Point(8, 466);
            this.labelGoodCount.Name = "labelGoodCount";
            this.labelGoodCount.Size = new System.Drawing.Size(95, 29);
            this.labelGoodCount.Text = "Кол-во:";
            // 
            // textBoxGoodCount
            // 
            this.textBoxGoodCount.Location = new System.Drawing.Point(106, 462);
            this.textBoxGoodCount.Name = "textBoxGoodCount";
            this.textBoxGoodCount.Size = new System.Drawing.Size(68, 41);
            this.textBoxGoodCount.TabIndex = 18;
            this.textBoxGoodCount.GotFocus += new System.EventHandler(this.textBoxGoodCount_GotFocus);
            this.textBoxGoodCount.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxGoodCount_KeyUp);
            // 
            // labelGettingGoodsList
            // 
            this.labelGettingGoodsList.Location = new System.Drawing.Point(24, 482);
            this.labelGettingGoodsList.Name = "labelGettingGoodsList";
            this.labelGettingGoodsList.Size = new System.Drawing.Size(431, 33);
            // 
            // buttonDeleteApplication
            // 
            this.buttonDeleteApplication.Location = new System.Drawing.Point(163, 467);
            this.buttonDeleteApplication.Name = "buttonDeleteApplication";
            this.buttonDeleteApplication.Size = new System.Drawing.Size(155, 36);
            this.buttonDeleteApplication.TabIndex = 22;
            this.buttonDeleteApplication.Text = "Удалить";
            this.buttonDeleteApplication.Click += new System.EventHandler(this.buttonDeleteApplication_Click);
            // 
            // buttonOkData
            // 
            this.buttonOkData.Location = new System.Drawing.Point(168, 452);
            this.buttonOkData.Name = "buttonOkData";
            this.buttonOkData.Size = new System.Drawing.Size(94, 54);
            this.buttonOkData.TabIndex = 59;
            this.buttonOkData.Text = "OK";
            this.buttonOkData.Click += new System.EventHandler(this.buttonOkData_Click);
            // 
            // textBoxYear
            // 
            this.textBoxYear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxYear.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxYear.Location = new System.Drawing.Point(114, 454);
            this.textBoxYear.MaxLength = 2;
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(44, 51);
            this.textBoxYear.TabIndex = 58;
            this.textBoxYear.TextChanged += new System.EventHandler(this.textBoxYear_TextChanged);
            this.textBoxYear.GotFocus += new System.EventHandler(this.textBoxYear_GotFocus);
            // 
            // textBoxMonth
            // 
            this.textBoxMonth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMonth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxMonth.Location = new System.Drawing.Point(60, 454);
            this.textBoxMonth.MaxLength = 2;
            this.textBoxMonth.Name = "textBoxMonth";
            this.textBoxMonth.Size = new System.Drawing.Size(43, 51);
            this.textBoxMonth.TabIndex = 57;
            this.textBoxMonth.TextChanged += new System.EventHandler(this.textBoxMonth_TextChanged);
            this.textBoxMonth.GotFocus += new System.EventHandler(this.textBoxMonth_GotFocus);
            // 
            // textBoxDay
            // 
            this.textBoxDay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.textBoxDay.Location = new System.Drawing.Point(8, 454);
            this.textBoxDay.MaxLength = 2;
            this.textBoxDay.Name = "textBoxDay";
            this.textBoxDay.Size = new System.Drawing.Size(41, 51);
            this.textBoxDay.TabIndex = 56;
            this.textBoxDay.TextChanged += new System.EventHandler(this.textBoxDay_TextChanged);
            this.textBoxDay.GotFocus += new System.EventHandler(this.textBoxDay_GotFocus);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 420);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 29);
            this.label1.Text = "Дата розлива";
            // 
            // textBoxGood
            // 
            this.textBoxGood.Location = new System.Drawing.Point(3, 121);
            this.textBoxGood.Name = "textBoxGood";
            this.textBoxGood.Size = new System.Drawing.Size(471, 41);
            this.textBoxGood.TabIndex = 55;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.textBoxLog.Location = new System.Drawing.Point(3, 167);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(472, 246);
            this.textBoxLog.TabIndex = 54;
            // 
            // checkBoxOldMark
            // 
            this.checkBoxOldMark.Location = new System.Drawing.Point(268, 419);
            this.checkBoxOldMark.Name = "checkBoxOldMark";
            this.checkBoxOldMark.Size = new System.Drawing.Size(206, 39);
            this.checkBoxOldMark.TabIndex = 53;
            this.checkBoxOldMark.Text = "старая марка";
            // 
            // checkBoxPDF417
            // 
            this.checkBoxPDF417.Enabled = false;
            this.checkBoxPDF417.Location = new System.Drawing.Point(6, 77);
            this.checkBoxPDF417.Name = "checkBoxPDF417";
            this.checkBoxPDF417.Size = new System.Drawing.Size(138, 36);
            this.checkBoxPDF417.TabIndex = 52;
            this.checkBoxPDF417.TabStop = false;
            this.checkBoxPDF417.Text = "PDF417";
            // 
            // checkBoxDataMatrix
            // 
            this.checkBoxDataMatrix.Enabled = false;
            this.checkBoxDataMatrix.Location = new System.Drawing.Point(150, 77);
            this.checkBoxDataMatrix.Name = "checkBoxDataMatrix";
            this.checkBoxDataMatrix.Size = new System.Drawing.Size(174, 36);
            this.checkBoxDataMatrix.TabIndex = 51;
            this.checkBoxDataMatrix.TabStop = false;
            this.checkBoxDataMatrix.Text = "DataMatrix";
            // 
            // checkBoxTovar
            // 
            this.checkBoxTovar.Enabled = false;
            this.checkBoxTovar.Location = new System.Drawing.Point(333, 77);
            this.checkBoxTovar.Name = "checkBoxTovar";
            this.checkBoxTovar.Size = new System.Drawing.Size(123, 36);
            this.checkBoxTovar.TabIndex = 50;
            this.checkBoxTovar.TabStop = false;
            this.checkBoxTovar.Text = "Товар";
            // 
            // txtBarCode
            // 
            this.txtBarCode.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.txtBarCode.Location = new System.Drawing.Point(3, 5);
            this.txtBarCode.Multiline = true;
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.ReadOnly = true;
            this.txtBarCode.Size = new System.Drawing.Size(474, 66);
            this.txtBarCode.TabIndex = 49;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(5, 452);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 54);
            this.button1.TabIndex = 61;
            // 
            // contextMenuCheckMark
            // 
            this.contextMenuCheckMark.MenuItems.Add(this.menuItemCheckFsrar);
            this.contextMenuCheckMark.MenuItems.Add(this.menuItemExitMark);
            // 
            // menuItemCheckFsrar
            // 
            this.menuItemCheckFsrar.Text = "Проверка связи с ФСРАР";
            this.menuItemCheckFsrar.Click += new System.EventHandler(this.menuItemCheckFsrar_Click);
            // 
            // menuItemExitMark
            // 
            this.menuItemExitMark.Text = "Назад в меню";
            this.menuItemExitMark.Click += new System.EventHandler(this.menuItemExitMark_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(43, 472);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 28);
            this.label2.Text = ".";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(97, 472);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 31);
            this.label3.Text = ".";
            // 
            // checkBoxCheckOnline
            // 
            this.checkBoxCheckOnline.Location = new System.Drawing.Point(268, 468);
            this.checkBoxCheckOnline.Name = "checkBoxCheckOnline";
            this.checkBoxCheckOnline.Size = new System.Drawing.Size(179, 31);
            this.checkBoxCheckOnline.TabIndex = 66;
            this.checkBoxCheckOnline.Text = "проверять";
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 588);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.textBoxMonth);
            this.Controls.Add(this.textBoxDay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxCheckOnline);
            this.Controls.Add(this.buttonMain);
            this.Controls.Add(this.buttonОК);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.buttonCheckPrice);
            this.Controls.Add(this.buttonCenniki);
            this.Controls.Add(this.buttonPrihod);
            this.Controls.Add(this.buttonInventarka);
            this.Controls.Add(this.checkBoxPDF417);
            this.Controls.Add(this.checkBoxDataMatrix);
            this.Controls.Add(this.checkBoxTovar);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.buttonOkData);
            this.Controls.Add(this.checkBoxOldMark);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDeleteApplication);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.listBoxSupplyLayer);
            this.Controls.Add(this.listBoxSklads);
            this.Controls.Add(this.textBoxGood);
            this.Controls.Add(this.dataGridSupplyList);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.dataGridGoods);
            this.Controls.Add(this.buttonCheckMark);
            this.Controls.Add(this.labelGettingGoodsList);
            this.Controls.Add(this.labelGoodCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxGoodCount);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 52);
            this.Name = "MainMenuForm";
            this.Text = "АИДА v1.1";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCheckPrice;
        private System.Windows.Forms.Button buttonCenniki;
        private System.Windows.Forms.Button buttonPrihod;
        private System.Windows.Forms.Button buttonInventarka;
        private System.Windows.Forms.Button buttonCheckMark;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.ListBox listBoxSklads;
        private System.Windows.Forms.ContextMenu contextMenuMain;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.Button buttonMain;
        private System.Windows.Forms.MenuItem menuItemChangeServer;
        private System.Windows.Forms.DataGrid dataGridGoods;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle;
        private System.Windows.Forms.ContextMenu contextMenuLostPrice;
        private System.Windows.Forms.MenuItem menuItemClearLostPriceDataGrid;
        private System.Windows.Forms.MenuItem menuItemUploadLostPriceTo1C;
        private System.Windows.Forms.MenuItem menuItemBackToMainMenu;
        private System.Windows.Forms.ContextMenu contextMenuLostPriceTable;
        private System.Windows.Forms.MenuItem menuItemDeleteRowLostPriceTable;
        private System.Windows.Forms.ListBox listBoxSupplyLayer;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonОК;
        private System.Windows.Forms.ContextMenu contextMenuSupply;
        private System.Windows.Forms.MenuItem menuItemSupplyUpload1C;
        private System.Windows.Forms.MenuItem menuItemSupplyExit;
        private System.Windows.Forms.DataGrid dataGridSupplyList;
        private System.Windows.Forms.ContextMenu contextMenuSupplyOperations;
        private System.Windows.Forms.MenuItem menuItemNewSupply;
        private System.Windows.Forms.MenuItem menuItemEditSupply;
        private System.Windows.Forms.Label labelGoodCount;
        private System.Windows.Forms.TextBox textBoxGoodCount;
        private System.Windows.Forms.Label labelGettingGoodsList;
        private System.Windows.Forms.Button buttonDeleteApplication;
        private System.Windows.Forms.MenuItem menuItemSetGoodCountToZero;
        private System.Windows.Forms.Button buttonOkData;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.TextBox textBoxMonth;
        private System.Windows.Forms.TextBox textBoxDay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGood;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.CheckBox checkBoxOldMark;
        private System.Windows.Forms.CheckBox checkBoxPDF417;
        private System.Windows.Forms.CheckBox checkBoxDataMatrix;
        private System.Windows.Forms.CheckBox checkBoxTovar;
        private System.Windows.Forms.TextBox txtBarCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenu contextMenuCheckMark;
        private System.Windows.Forms.MenuItem menuItemCheckFsrar;
        private System.Windows.Forms.MenuItem menuItemExitMark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuItem menuItemCheckConnection1C;
        private System.Windows.Forms.MenuItem menuItemGetIP;
        private System.Windows.Forms.CheckBox checkBoxCheckOnline;
    }
}