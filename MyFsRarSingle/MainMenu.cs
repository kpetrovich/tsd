using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Aida.Model;
using Aida.Service;
using Aida.Forms;
using DSIC.Barcode;
using System.Threading;
using System.Reflection;
using System.Collections;
using CodeBetter.Json;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


namespace Aida
{
    public partial class MainMenuForm : Form
    {
        public string version = "v1.2";

        Wrapper.pCallBack myCallback;
        private int[] Month30 = new[] { 4, 6, 9, 11 };

        private Layouts currentLayout;
        private FileSettings fileSetting;
        private BaseSettings appSettings;
        private FsRarService fsRarService;
        private DataBaseService dbService;
        private DataTable dataTableLostPrice;
        private DataTable dataTableSupply;
        private DataTable dataTableSupplyDetail;
        private OneCService oneCService;
        private ApplicationList applicationsList;
        private string app_doc_guid;
        private int maxOrderSupplyDetail;

        private List<SupplyMarkArray> supplyMarksArray;

        //private MarkRecord1c markRec = new MarkRecord1c();
        private CurrentMark currentMark = new CurrentMark();
        private Good1C currentGood = new Good1C();

        Thread thread1;
        Thread thread2;
        private string timerText = "";
        private bool timerEnabled;
       

        public MainMenuForm()
        {
            InitializeComponent();
        }
        
        private void MainMenu_Load(object sender, EventArgs e)
        {
            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.MAIN_MENU);

            appSettings  = GetSettings();

            if (!appSettings.success)
            {
                MessageBox.Show(appSettings.msg);
                CloseProgarm();
                return;
            }
            else 
            {
                labelMain.Text = appSettings.name;
            }

            InitDataBase();
            InitScanner();

            oneCService = new OneCService();
            oneCService.serverUri = appSettings.server_uri;
            oneCService.enableTimeoutCheck = true;

            dbService = new DataBaseService();
            fsRarService = new FsRarService();

            // признак проверки онлайн марки в ФСРАР
            checkBoxCheckOnline.Checked = true;
           
        }
               

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///       МЕТОДЫ
        ///

        private void InitScanner() 
        {
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
                myCallback = new Wrapper.pCallBack(CallbackScanner);
                Wrapper.DSBarcodeSetCallback(myCallback);
                Wrapper.DSBarcodeSetEnable(false);
            }
            else
            {
                MessageBox.Show("Ошибка активации сканера!");
                CloseProgarm();
            }
        }

        private void InitDataBase() 
        {
            var dbService = new DataBaseService();
            var result = dbService.InitBase();

            if (!result.success)
            {
                MessageBox.Show(result.msg);
                CloseProgarm();
            }
        }

        private BaseSettings GetSettings()
        {
            fileSetting = SettingsService.GetSettings();
            
            if (!fileSetting.success)
            {
                return SelectAndSaveSettings();
            }
            else 
            {                
                // запросим настройки у сервера
                var _oneCService = new OneCService();
                _oneCService.serverUri = fileSetting.server_uri;

                var baseSettings = _oneCService.GetBaseSettings();

                if (!baseSettings.success) 
                {
                    //MessageBox.Show(baseSettings.msg + Environment.NewLine + "Ошибка получения параметров из базы: " + fileSetting.server_uri);
                    //CloseProgarm();

                    return new BaseSettings { success = false, msg = baseSettings.msg + Environment.NewLine + "Ошибка получения параметров из базы: " + fileSetting.server_uri };
                }

                // выберем настройку базы согласно записи из файла настроек                
                var appSettingsResult = baseSettings.settings.FirstOrDefault(i => i.code == fileSetting.setting_code);

                if (appSettingsResult == null)
                {
                    // заполним настройки в памяти
                    return SelectAndSaveSettings();
                }
                else
                {
                    appSettingsResult.success = true;
                    return appSettingsResult;
                }
            }            
        }

        public void CallbackScanner(Wrapper.DS_BARCODE DecordingData)
        {
            switch (currentLayout)
            {
                case Layouts.CHECK_PRICE: 
                    {
                        CheckPriceScan(DecordingData.strBarcode);
                        break;
                    }
                case Layouts.LOST_PRICE: 
                    {
                        LostPriceScan(DecordingData.strBarcode);
                        break;
                    }
                case Layouts.SUPPLY:
                    {
                        SupplyGoodScan(DecordingData.strBarcode);
                        break;
                    }
                case Layouts.INVENTORY:
                    {
                        break;
                    }
                case Layouts.CHECK_MARK:
                    {
                        CheckMarkScan(DecordingData.strBarcode);
                        break;
                    }
                case Layouts.CHECK_MARK_SUPPLY:
                    {
                        CheckMarkSupplyScan(DecordingData.strBarcode);
                        break;
                    }
            }
        }

        private void SupplyGoodScan(string barcode)
        {
            bool finded = false;
            
            for (var i = 0; i < dataTableSupplyDetail.Rows.Count; i++) 
            {
                var barcodeCell = dataTableSupplyDetail.Rows[i].Field<string>(SUPPLY_DETAIL_TABLE.barcode);

                if (barcodeCell.IndexOf(barcode) > -1)
                {
                    finded = true;
                    dataGridSupplyList.CurrentRowIndex = i;
                    dataGridSupplyList.Select(dataGridSupplyList.CurrentRowIndex);
                    break;
                }
            }

            if (!finded)
            {
                MessageBox.Show("Товар не найден");
            }
            else 
            {
                if (dataTableSupplyDetail.Rows[dataGridSupplyList.CurrentRowIndex].Field<bool>(SUPPLY_DETAIL_TABLE.akcis))
                {
                    // это акцизный товар, надо переключиться в режим проверки марок
                    FollowToCheckMarkLayer();
                }
                else
                {
                    textBoxGoodCount.Focus();
                }
            }
        }

        private void LostPriceScan(string barcode) 
        {            
            var result = oneCService.GetTovar(barcode);
            
            if (result.success)
            {                
                var resFind = dataTableLostPrice.Select("goodguid='" + result.guid + "'");

                if (resFind.Length == 0)
                {
                    dataTableLostPrice.Rows.Add(new object[] { result.guid, result.goodname });
                    dataGridGoods.CurrentRowIndex = dataTableLostPrice.Rows.Count - 1;

                    var recInit = new InsertSqlRecordModel
                    {
                        tableName = "lostprice",
                        Fields = new SqlFieldModel[] {
                            new SqlFieldModel {
                                fieldName = "good_guid", 
                                fieldValue = "'"+result.guid+"'"},
                            new SqlFieldModel {
                                fieldName = "good_name", 
                                fieldValue = "'"+result.goodname+"'"}
                        }
                    };
                    
                    var insertResult = dbService.InsertRecordToDb(recInit);
                    if (!insertResult.success)
                    {
                        MessageBox.Show(insertResult.msg);
                    }
                }
                else
                {
                    dataGridGoods.CurrentRowIndex = dataTableLostPrice.Rows.IndexOf(resFind[0]);
                }
            }
            else
            {
                MessageBox.Show(result.msg);
            }
        }

        private void CheckPriceScan(string barcode)
        {
            textBoxResult.Text = "Отправлен запрос по штрихкоду: " + barcode;

            if (!String.IsNullOrEmpty(barcode))
            {
                var skladcode = (ListItem)listBoxSklads.SelectedItem;

                if (barcode.Length >= 8 && barcode.Length <= 13)
                {                    
                    var result = oneCService.GetPrice(barcode, skladcode.itemList.code);

                    if (result.success)
                    {
                        textBoxResult.Text = "Товар: " + result.good + Environment.NewLine +
                            "Тип цены: " + result.pricetype + Environment.NewLine +
                            "Цена: " + result.price;
                    }
                    else
                    {
                        textBoxResult.Text = result.msg;
                    }
                }
                else
                {
                    textBoxResult.Text = "Неверный штрихкод!";
                }
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private BaseSettings SelectAndSaveSettings()
        {
            InputBox inputBox = new InputBox();
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return ChangeBase(inputBox.serverAddress);    
            }

            return new BaseSettings { success = false};
        }

        private BaseSettings ChangeBase(string serverName)
        {
            if (!String.IsNullOrEmpty(serverName))
            {
                // получим список настроек баз из 1С
                SelectBaseForm formSelectBase = new SelectBaseForm();
                formSelectBase.serverAddress = serverName;
                // сделаем выбор базы
                if (formSelectBase.ShowDialog() == DialogResult.OK)
                {
                    var selSettings = formSelectBase.selectedItemBase;

                    // запишем настройки в файл                        
                    var saveResult = SettingsService.SaveSettings(selSettings);

                    if (saveResult.success)
                    {
                        MessageBox.Show("Настройки сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка сохранения настроек." + Environment.NewLine + saveResult.msg);
                    }

                    selSettings.success = true;
                    return selSettings;
                }
                else
                {
                    return new BaseSettings { success = false, msg = "Ошибка сохранения настроек" };
                }
            }

            return new BaseSettings { success = false, msg = "Ошибка сохранения настроек" };
        }
                
        private void CheckPrice()
        {
            textBoxResult.Text = "";

            FillSkaldListFromSettings();

            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.CHECK_PRICE);

            Wrapper.DSBarcodeSetEnable(true);
        }

        private void FillSkaldListFromSettings() 
        {
            listBoxSklads.Items.Clear();
            listBoxSklads.ValueMember = "code";
            listBoxSklads.DisplayMember = "name";

            for (int i = 0; i < appSettings.sklads.Length; i++)
            {
                listBoxSklads.Items.Add(new ListItem { itemList = appSettings.sklads[i], name = appSettings.sklads[i].name });
            }

            if (appSettings.sklads.Length > 0)
            {
                listBoxSklads.SelectedIndex = 0;
            }
        }
        
        private void GetLostPrice() 
        {
            InitDataGridLostPrice();
            ReadLostPriceTableFromDB();

            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.LOST_PRICE);

            Wrapper.DSBarcodeSetEnable(true);
        }

        private void InitDataGridLostPrice()
        {
            dataTableLostPrice = new DataTable();

            DataColumn dcGuid = new DataColumn { ColumnName = "goodguid", DataType = Type.GetType("System.String") };
            dataTableLostPrice.Columns.Add(dcGuid);
            DataColumn dc = new DataColumn { ColumnName = "googname", Caption = "Товар", DataType = Type.GetType("System.String"), MaxLength = 400 };
            dataTableLostPrice.Columns.Add(dc);
            
            dataTableLostPrice.TableName = "lostprice";

            dataGridGoods.TableStyles.Clear();
            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = dataTableLostPrice.TableName;
            foreach (DataColumn item in dataTableLostPrice.Columns)
            {
                if (item.MaxLength > 0)
                {
                    DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();
                    tbcName.Width = item.MaxLength;
                    tbcName.MappingName = item.ColumnName;
                    tbcName.HeaderText = item.Caption;
                    tableStyle.GridColumnStyles.Add(tbcName);
                }
            }
            dataGridGoods.TableStyles.Add(tableStyle);
            dataGridGoods.DataSource = dataTableLostPrice;
            dataGridGoods.Refresh();
        }

        private void InitSupplyListDataTable()
        {
            dataTableSupply = new DataTable();

            DataColumn dc;
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.id, DataType = Type.GetType("System.Int32") };
            dataTableSupply.Columns.Add(dc);
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.doc_guid, DataType = Type.GetType("System.String") };
            dataTableSupply.Columns.Add(dc);
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.contragent, Caption = "Поставщик", DataType = Type.GetType("System.String"), Namespace = "250" };
            dataTableSupply.Columns.Add(dc);
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.doc_number, Caption = "Номер", DataType = Type.GetType("System.String"), Namespace = "150" };
            dataTableSupply.Columns.Add(dc);
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.doc_date, Caption = "Дата", DataType = Type.GetType("System.String"), Namespace = "100" };
            dataTableSupply.Columns.Add(dc);
            dc = new DataColumn { ColumnName = APPLICATIONS_TABLE.uploaded, Caption = "1C", DataType = Type.GetType("System.String"), Namespace = "100" };
            dataTableSupply.Columns.Add(dc);

            dataTableSupply.TableName = APPLICATIONS_TABLE.table_name;

            dataGridSupplyList.TableStyles.Clear();
            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = dataTableSupply.TableName;
            foreach (DataColumn item in dataTableSupply.Columns)
            {
                if (!String.IsNullOrEmpty(item.Namespace))
                {
                    DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();
                    tbcName.Width = Convert.ToInt32(item.Namespace);
                    tbcName.MappingName = item.ColumnName;
                    tbcName.HeaderText = item.Caption;
                    tableStyle.GridColumnStyles.Add(tbcName);
                }
            }

            dataGridSupplyList.TableStyles.Add(tableStyle);
            dataGridSupplyList.DataSource = dataTableSupply;
            dataGridSupplyList.Refresh();
        }

        private void InitSupplyDetailListDataTable()
        {
            dataTableSupplyDetail = new DataTable();

            DataColumn dc;
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.id, DataType = Type.GetType("System.Int32") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.app_doc_id, DataType = Type.GetType("System.Int32") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.good_guid, DataType = Type.GetType("System.String") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.good_name, Caption = "Товар", DataType = Type.GetType("System.String"), Namespace = "240" };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.barcode, DataType = Type.GetType("System.String") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.good_count_doc, Caption = "Кол-во", DataType = Type.GetType("System.String"), Namespace = "85" };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.good_count_fact, Caption = "Факт", DataType = Type.GetType("System.String"), Namespace = "80" };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.akcis, DataType = Type.GetType("System.Boolean") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.dopis, DataType = Type.GetType("System.Boolean") };
            dataTableSupplyDetail.Columns.Add(dc);
            dc = new DataColumn { ColumnName = SUPPLY_DETAIL_TABLE.roworder, DataType = Type.GetType("System.Int32") };
            dataTableSupplyDetail.Columns.Add(dc);
            
            dataTableSupplyDetail.TableName = SUPPLY_DETAIL_TABLE.table_name;

            dataGridSupplyList.TableStyles.Clear();
            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = dataTableSupplyDetail.TableName;
                        
            foreach (DataColumn item in dataTableSupplyDetail.Columns)
            {
                if (!String.IsNullOrEmpty(item.Namespace))
                {                    
                    NewDataGridTextBoxColumn tbcName = new NewDataGridTextBoxColumn();
                    
                    tbcName.dt = dataTableSupplyDetail;
                    tbcName.dgFont = dataGridSupplyList.Font;
                    //tbcName.dg = dataGridSupplyList;

                    tbcName.Width = Convert.ToInt32(item.Namespace);
                    tbcName.MappingName = item.ColumnName;
                    tbcName.HeaderText = item.Caption;
                    tableStyle.GridColumnStyles.Add(tbcName);
                }
            }

            dataGridSupplyList.TableStyles.Add(tableStyle);
            dataGridSupplyList.DataSource = dataTableSupplyDetail;
        }

        private void ReadLostPriceTableFromDB()
        {
            var list = dbService.ReadLostPriceTable();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    dataTableLostPrice.Rows.Add(new object[] { item.GoodGuid, item.GoodName });
                }
                dataGridGoods.CurrentRowIndex = dataTableLostPrice.Rows.Count - 1;
            }
        }
        
        private void BackToMainMenu() 
        {
            Wrapper.DSBarcodeSetEnable(false);
            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.MAIN_MENU);
        }
        
        private void GetApplications()
        {
            timerText = "";

            timerEnabled = true;
            // поток 1 блинкует название кнопки Приход
            thread1 = new Thread(new ThreadStart(ThreadMethod_BlinkSupplyButton));
            thread1.Start();
            // поток 2 получает из базы 1С список заявок
            thread2 = new Thread(new ThreadStart(ThreadMethod_GetApplicationsList));
            thread2.Start();            
        }

        private int DoSupply(ApplicationDoc app1cDoc) 
        {
            return DoSupply(app1cDoc, 0);
        }

        private int DoSupply(DataRow dataRow)
        {            
            return DoSupply(new ApplicationDoc { 
                doc_guid = dataRow.Field<string>(APPLICATIONS_TABLE.doc_guid), 
                contragent = dataRow.Field<string>(APPLICATIONS_TABLE.contragent),
                uploaded = dataRow.Field<string>(APPLICATIONS_TABLE.uploaded) == "Да" ? true : false
            }, dataRow.Field<int>(APPLICATIONS_TABLE.id));
        }

        private int DoSupply(ApplicationDoc app1cDoc, int appdoc_id) 
        {
            if (appdoc_id == 0)
            {
                // поищем в базе, может уже начали приход по этой заявке
                appdoc_id = dbService.GetApplicationTableId(app1cDoc.doc_guid);

                if (appdoc_id == 0)
                {
                    appdoc_id = dbService.WriteNewApplicationToDB(app1cDoc);

                    if (appdoc_id == 0)
                    {
                        return 0; // ошибка - возвращаемся в главное меню
                    }
                }
            }

            menuItemSupplyUpload1C.Enabled = !app1cDoc.uploaded;
            
            app_doc_guid = app1cDoc.doc_guid;
            this.Text = app1cDoc.contragent;

            // попробуем прочесть строки заявки из базы терминала
            var result = dbService.ReadSupplyDetailTable(appdoc_id);

            InitSupplyDetailListDataTable();

            Wrapper.DSBarcodeSetEnable(true);

            if (result.Count > 0)
            {                
                // в базе уже есть эта заявка, заполним DataTable и продолжим редактировать
                maxOrderSupplyDetail = 0;

                foreach (var item in result)
                {
                    if (maxOrderSupplyDetail < item.roworder) 
                    { 
                        maxOrderSupplyDetail = item.roworder; 
                    }

                    dataTableSupplyDetail.Rows.Add(new object[] { item.id, item.app_doc_id, item.good_guid , item.good_name, item.barcode, 
                        item.good_count_doc.ToString().Replace('.',','), item.good_count_fact.ToString().Replace('.',','), item.akcis, item.dopis, item.roworder 
                    });                   
                }

                InitSupplyMarkArray(result);

                return 1; // сразу можно приступить к приходу
            }
            else 
            {
                if (!String.IsNullOrEmpty(app1cDoc.doc_guid))
                {
                    // заявки нет в базе, запросим ее из 1с
                    GetGoodsListFrom1Cbase(app1cDoc.doc_guid, appdoc_id);
                }
                else 
                {
                    return 0;
                }
            }
                       
            return 2; // ждем получения списка товаров
        }

        private void GetGoodsListFrom1Cbase(string doc_guid, int appdoc_id)
        {
            timerText = "";

            timerEnabled = true;
            // поток 1 блинкует текст надписи
            thread1 = new Thread(new ThreadStart(ThreadMethod_BlinkLabel));
            thread1.Start();
            // поток 2 получает из базы 1С список товаров
            //thread2 = new Thread(new ThreadStart(ThreadMethod_GetGoodListForSupply));
            thread2 = new Thread(() => ThreadMethod_GetGoodListForSupply(doc_guid, appdoc_id));
            thread2.Start();    
        }

        private void ThreadMethod_BlinkLabel()
        {
            // задержка 400 мс
            Thread.Sleep(400);

            labelGettingGoodsList.Invoke((ThreadStart)delegate
            {
                timerText += ".";
                if (currentLayout == Layouts.SKLAD_SELECT)
                {
                    labelGettingGoodsList.Text = "Отправка поступления в 1С " + timerText;
                }
                else 
                { 
                    labelGettingGoodsList.Text = "Получение списка товаров " + timerText;
                }

                if (timerText.Length == 4)
                {
                    timerText = "";
                }
            });

            if (timerEnabled)
            {
                thread1 = new Thread(new ThreadStart(ThreadMethod_BlinkLabel));
                thread1.Start();
            }
            else
            {
                labelGettingGoodsList.Invoke((ThreadStart)delegate
                {
                    labelGettingGoodsList.Text = "";
                });
            }
        }

        private void ThreadMethod_BlinkSupplyButton()
        {
            // задержка 400 мс
            Thread.Sleep(400); 
            
            buttonPrihod.Invoke((ThreadStart)delegate
            {
                timerText += ".";
                buttonPrihod.Text = "Поиск заявок " + timerText;

                if (timerText.Length == 4)
                {
                    timerText = "";
                }
            });

            if (timerEnabled)
            {
                thread1 = new Thread(new ThreadStart(ThreadMethod_BlinkSupplyButton));
                thread1.Start();
            }
            else
            {
                buttonPrihod.Invoke((ThreadStart)delegate
                {
                    buttonPrihod.Text = "Приход";
                });
            }
        }

        private void ThreadMethod_GetApplicationsList() 
        {
            applicationsList = oneCService.GetApplicationList(appSettings.code);
            
            if (!applicationsList.success)
            {
                timerEnabled = false;
                
                buttonPrihod.Invoke((ThreadStart)delegate
                {
                    buttonPrihod.Text = "Приход";
                });

                MessageBox.Show(applicationsList.msg);
                return;
            }

            listBoxSupplyLayer.Invoke((ThreadStart)delegate 
            {
                listBoxSupplyLayer.Items.Clear();
                listBoxSupplyLayer.Font = new Font("Tahoma", 18, FontStyle.Regular);
                listBoxSupplyLayer.Height = 500;

                foreach (var item in applicationsList.applist)
                {
                    if (!listBoxSupplyLayer.Items.Contains(item.contragent))
                    {
                        listBoxSupplyLayer.Items.Add(item.contragent);
                    }
                }
            });

            timerEnabled = false;
            
            this.Invoke((ThreadStart)delegate 
            {
                currentLayout = Layout.SetLayout(this, buttonMain, Layouts.CONTRAGENT_SELECT);
            });
        }

        private void ThreadMethod_GetGoodListForSupply(string doc_guid, int appdoc_id)
        {
            
            //// в базе еще нет этой заявки, загрузим строки заявки из 1С
            var appRowsList = oneCService.GetApplicationDocumentRows(doc_guid);

            //////////////////////// ****************************************************************************************************
            // заглущка для алкоголя
            //foreach (var r in appRowsList.rows) 
            //{
            //    r.akcis = false;
            //}
            //////////////////////// ****************************************************************************************************

            if (!appRowsList.success)
            {
                timerEnabled = false;
                MessageBox.Show(appRowsList.msg);

                this.Invoke((ThreadStart)delegate 
                {
                    BackToMainMenu();
                });

                return;
            }
            else 
            {
                if (appRowsList.rows.Length == 0)
                {
                    timerEnabled = false;
                    labelGettingGoodsList.Invoke((ThreadStart)delegate
                    {
                        labelGettingGoodsList.Text = "";
                    });

                    MessageBox.Show("Список товаров в заявке пуст");
                    this.Invoke((ThreadStart)delegate
                        {
                            BackToMainMenu();
                        });
                    
                    return;
                }
            }

            // добавим строки заявки в базу терминала
            var insResult = dbService.InsertSupplyRowsToDb(appdoc_id, appRowsList.rows);
            if (insResult.success)
            {
                // перечитаем эти записи и добавим их в DataTable
                var readResult = dbService.ReadSupplyDetailTable(appdoc_id);

                if (readResult.Count > 0)
                {
                    dataGridSupplyList.Invoke((ThreadStart)delegate
                    {
                        foreach (var item in readResult)
                        {
                            // добавим данные в DataTable
                            dataTableSupplyDetail.Rows.Add(new object[] { item.id, appdoc_id, item.good_guid, item.good_name,
                                item.barcode, item.good_count_doc, 0, item.akcis, item.dopis, item.roworder });                            
                        }

                        InitSupplyMarkArray(readResult);
                    });
                }
                else
                {
                    timerEnabled = false;
                    MessageBox.Show("Ошибка чтения новой заявки из базы терминала");

                    this.Invoke((ThreadStart)delegate
                    {
                        BackToMainMenu();
                    });

                    return;
                }
            }
            else
            {
                timerEnabled = false;
                MessageBox.Show(insResult.msg);

                this.Invoke((ThreadStart)delegate
                {
                    BackToMainMenu();
                });

                return;
            }            

            timerEnabled = false;

            this.Invoke((ThreadStart)delegate
            {
                currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY);
            });
        }

        private void InitSupplyMarkArray(List<SupplyDetailRow> supplyDetailRowList)
        {
            // если есть акцизный товар в таблице
            bool akcis = false;
            foreach (var item in supplyDetailRowList)
            {
                if (item.akcis && supplyMarksArray == null)
                {
                    akcis = true;
                    break;
                }
            }

            if (akcis)
            {
                // прочтем из базы раннее сканированные акцизные марки, если есть
                supplyMarksArray = new List<SupplyMarkArray>();

                foreach (var item in supplyDetailRowList)
                {
                    if (item.akcis)
                    {
                        var result = dbService.ReadAlkoDetailTable(item.id);

                        // заполним массив товаров и акцизных марок
                        supplyMarksArray.Add(new SupplyMarkArray {
                            table_id = item.id,
                            barcodeEAN = item.barcode,
                            currentCount = item.good_count_fact,
                            good_guid = item.good_guid,
                            good_name = item.good_name,
                            totalCount = item.good_count_doc,
                            bottles = result                            
                        });
                    }
                }
            }
        }

        private void GetSupplyListFromDB()
        {
            var result = dbService.GetSupplyListTable();

            if (result.Count == 0)
            {
                if (currentLayout == Layouts.MAIN_MENU)
                {
                    MessageBox.Show("Список пуст." + Environment.NewLine + "Нечего редактировать.");
                }
                else
                {
                    BackToMainMenu();
                }

                return;
            }

            //InitSupplyListDataTable();
            dataTableSupply.Rows.Clear();

            foreach (var item in result)
            {
                dataTableSupply.Rows.Add(new object[] { item.id, item.doc_guid, item.contragent, item.doc_number, item.doc_date, item.uploaded ? "Да" : "Нет" });
            }

            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY_SELECT);
        }

        private void CloseProgarm() 
        {
            if (timerEnabled)
            {
                thread1.Abort();
                thread2.Abort();
            }

            Close();
        }

        private void UploadSupplyTo1C() 
        {
            timerText = "";

            timerEnabled = true;
            labelGettingGoodsList.Visible = true;
            // поток 1 блинкует название кнопки Приход
            thread1 = new Thread(new ThreadStart(ThreadMethod_BlinkLabel));
            thread1.Start();
            // поток 2 получает из базы 1С список заявокS
            thread2 = new Thread(new ThreadStart(ThreadMethod_UploadSupply));
            thread2.Start();
        }

        private void ThreadMethod_UploadSupply() 
        {
            ListItem skladItem = null;

            listBoxSklads.Invoke((ThreadStart)delegate
            {
                skladItem = (ListItem)listBoxSklads.SelectedItem;
            });

            // выгрузим приход в 1С
            var result = oneCService.UploadSupplyTo1cBase(app_doc_guid, dataTableSupplyDetail.Rows, skladItem.itemList.code, supplyMarksArray);
            timerEnabled = false;

            if (result.success)
            {
                // установим статус удачной выгрузки накладной в 1С
                var app_id = dataTableSupplyDetail.Rows[0].Field<int>(SUPPLY_DETAIL_TABLE.app_doc_id);
                dbService.SetUploadStatus(app_id);                
            }

            labelGettingGoodsList.Invoke((ThreadStart)delegate {
                labelGettingGoodsList.Visible = false;
            });
            
            MessageBox.Show(result.msg);

            this.Invoke((ThreadStart)delegate
            {
                BackToMainMenu();
            });
        }

        private void CheckMark() 
        {
            Wrapper.DSBarcodeSetEnable(true);
            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.CHECK_MARK);
        }

        private Result CheckAkcisCount() 
        {
            if (supplyMarksArray == null)
            {
                return new Result { success = true };
            }

            for (int i = 0; i < dataTableSupplyDetail.Rows.Count; i++) 
            {
                // поолучим строку в таблице просмотра
                var row = dataTableSupplyDetail.Rows[i];
                // найдем строку с товаром в таблице марок
                var bottle_row = supplyMarksArray.FirstOrDefault(bottle => bottle.table_id == row.Field<int>(SUPPLY_DETAIL_TABLE.id));

                if (bottle_row != null)
                { // если по этому товару есть акцизные марки

                    // сравним кол-во в таблице просмотра с кол-вом акцизных марок
                    if (Convert.ToInt32(row.Field<string>(SUPPLY_DETAIL_TABLE.good_count_fact)) != bottle_row.bottles.Count)
                    {
                        // если кол-во не совпадает, то выдадим ошибку
                        return new Result { success = false, 
                            msg = "Для товара: " + bottle_row.good_name + Environment.NewLine + 
                            "несовпадает кол-во марок с фактом: " + Environment.NewLine +
                            "Факт=" + row.Field<string>(SUPPLY_DETAIL_TABLE.good_count_fact) + " Марок=" + bottle_row.bottles.Count.ToString()
                        };
                    }
                }
            }

            return new Result { success = true };
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///       КНОПКИ / ОБРАБОТЧИКИ
        /// 

        private void buttonCenniki_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonCheckPrice_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonMain_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonPrihod_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonОК_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonInventarka_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        private void buttonCheckMark_Click(object sender, EventArgs e)
        {
            ButtonClickHandler(sender);
        }

        //***********
        private void ButtonClickHandler(object sender)
        {
            var button = (Button)sender;

            /////////// кнопка Отмена
            #region buttonCancel
            if (button.Name == buttonCancel.Name)
            {
                currentLayout = Layout.SetLayout(this, buttonMain, Layouts.MAIN_MENU);
            }
            #endregion

            /////////// кнопка ОК (Выбрать)
            #region buttonОК
            if (button.Name == buttonОК.Name)
            {
                if (currentLayout == Layouts.CONTRAGENT_SELECT)
                {
                    // выбор контрагента
                    #region CONTRAGENT_SELECT

                    var contragentSelected = (string)listBoxSupplyLayer.SelectedItem;

                    if (String.IsNullOrEmpty(contragentSelected))
                    {
                        return;
                    }

                    // проверим, если по одному Поставщику есть несколько заявок, то предложим выбрать одну                
                    var appListForContragent = applicationsList.applist.Where<ApplicationDoc>(i => i.contragent == contragentSelected).Select(r => new ApplicationDoc
                    {
                        contragent = r.contragent,
                        doc_date = r.doc_date,
                        doc_guid = r.doc_guid,
                        doc_number = r.doc_number,
                        uploaded = r.uploaded,
                        comment = r.comment
                    }).ToList();

                    if (appListForContragent.Count == 1)
                    {
                        // сразу начнем приход товаров
                        var res = DoSupply(appListForContragent[0]);
                        if (res == 1)
                        {
                            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY);
                            return;
                        }

                        if (res == 0)
                        {
                            BackToMainMenu();
                            return;
                        }
                    }
                    else
                    {
                        // предложим выбрать Заявку
                        listBoxSupplyLayer.Items.Clear();
                        listBoxSupplyLayer.DisplayMember = "name";                        
                        listBoxSupplyLayer.Font = new Font("Tahoma", 12, FontStyle.Regular);
                        listBoxSupplyLayer.Height = 500;

                        foreach (var item in appListForContragent)
                        {
                            listBoxSupplyLayer.Items.Add(new ListItemApp { itemList = item, name = item.doc_number + "/" + item.doc_date + "/"+item.comment });
                        }

                        currentLayout = Layout.SetLayout(this, buttonMain, Layouts.APPLICATION_SELECT);
                        return;
                    }
                    #endregion
                }

                if (currentLayout == Layouts.APPLICATION_SELECT)
                {
                    // выбор заявки, если она у контрагента не одна
                    #region APPLICATION_SELECT
                    
                    var applicationSelected = (ListItemApp)listBoxSupplyLayer.SelectedItem;

                    if (listBoxSupplyLayer.SelectedIndex == -1)
                    {
                        return;
                    }

                    var res2 = DoSupply(applicationSelected.itemList);
                    if (res2 == 1)
                    {
                        currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY);
                        return;
                    }

                    if (res2 == 0)
                    {
                        BackToMainMenu();
                        return;
                    }
                    #endregion
                }

                if (currentLayout == Layouts.SUPPLY_SELECT)
                {
                    // редактирование существующей заявки
                    #region SUPPLY_SELECT

                    if (dataGridSupplyList.CurrentRowIndex > -1)
                    {
                        //var res3 = DoSupply(app_id, app_guid, app_contragent);
                        var res3 = DoSupply(dataTableSupply.Rows[dataGridSupplyList.CurrentRowIndex]);
                        
                        if (res3 == 1)
                        {
                            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY);
                            return;
                        }
                    }
                    #endregion
                }

                if (currentLayout == Layouts.SKLAD_SELECT)
                {
                    // выгружаем приход на выбранный склад
                    #region SKLAD_SELECT

                    UploadSupplyTo1C();

                    #endregion
                }
            }
            #endregion

            /////////// кнопка (главное меню) Приход
            #region buttonPrihod
            if (button.Name == buttonPrihod.Name)
            {
                contextMenuSupplyOperations.Show(buttonPrihod, new Point { X = 0, Y = -20 - contextMenuSupplyOperations.MenuItems.Count * 80 });      
            }
            #endregion

            /////////// кнопка МЕНЮ
            #region buttonMain
            if (button.Name == buttonMain.Name)
            {
                switch (currentLayout)
                {
                    case Layouts.MAIN_MENU:
                        {
                            contextMenuMain.Show(buttonMain, new Point { X = 0, Y = -20 - contextMenuMain.MenuItems.Count * 80 });
                            break;
                        }
                    case Layouts.LOST_PRICE:
                        {
                            contextMenuLostPrice.Show(buttonMain, new Point { X = 0, Y = -20 - contextMenuLostPrice.MenuItems.Count * 80 });
                            break;
                        }
                    case Layouts.CHECK_PRICE:
                        {
                            BackToMainMenu();
                            break;
                        }
                    case Layouts.SUPPLY:
                        {
                            contextMenuSupply.Show(buttonMain, new Point { X = 0, Y = -20 - contextMenuSupply.MenuItems.Count * 80 });
                            break;
                        }
                    case Layouts.CHECK_MARK:
                        {
                            contextMenuCheckMark.Show(buttonMain, new Point { X = 0, Y = -20 - contextMenuCheckMark.MenuItems.Count * 80 });
                            break;
                        }
                    case Layouts.CHECK_MARK_SUPPLY:
                        {
                            currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SUPPLY);
                            break;
                        }
                }
            }
            #endregion

            /////////// кнопка (главное меню) Ценники
            #region buttonCenniki
            if (button.Name == buttonCenniki.Name)
            {
                GetLostPrice();
            }
            #endregion

            /////////// кнопка (главное меню) Проверить цену 
            #region buttonCheckPrice
            if (button.Name == buttonCheckPrice.Name)
            {
                CheckPrice();
            }
            #endregion

            /////////// кнопка (главное меню) Проверка марки
            if (button.Name == buttonCheckMark.Name)
            {
                CheckMark();    
            }
        }
        //***********
        private void textBoxGoodCount_KeyUp(object sender, KeyEventArgs e)
        {
            if (FollowToCheckMarkLayer()) 
            {
                textBoxGoodCount.Text = "";
                return;
            }

            if (e.KeyCode == Keys.Enter && !String.IsNullOrEmpty(textBoxGoodCount.Text))
            {
                maxOrderSupplyDetail++;

                var row = dataTableSupplyDetail.Rows[dataGridSupplyList.CurrentRowIndex];
                var field = Convert.ToDouble(row.Field<string>(SUPPLY_DETAIL_TABLE.good_count_fact));
                
                // установим новое значение в ячейке Факт
                var newValue = field + Convert.ToDouble(textBoxGoodCount.Text.Replace('.',','));
                row.SetField<string>(SUPPLY_DETAIL_TABLE.good_count_fact, newValue.ToString().Replace('.', ','));
                row.SetField<int>(SUPPLY_DETAIL_TABLE.roworder, maxOrderSupplyDetail);

                textBoxGoodCount.Text = "";

                // запишем изменение в базу терминала
                var updRec = new UpdateSqlRecordModel();
                updRec.tableName = SUPPLY_DETAIL_TABLE.table_name;
                updRec.expression = SUPPLY_DETAIL_TABLE.id + " = " + row.Field<int>(SUPPLY_DETAIL_TABLE.id);

                updRec.Fields = new SqlFieldModel[] 
                {
                    new SqlFieldModel
                    {
                        fieldName = SUPPLY_DETAIL_TABLE.good_count_fact,
                        fieldValue = newValue
                    },
                    new SqlFieldModel
                    {
                        fieldName = SUPPLY_DETAIL_TABLE.roworder,
                        fieldValue = maxOrderSupplyDetail
                    }
                };
                
                var result = dbService.UpdateTableRecord(updRec);

                if (!result.success)
                {
                    MessageBox.Show(result.msg);
                }
            }
        }

        private void buttonDeleteApplication_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить?", "Удаление заявки", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (dataGridSupplyList.CurrentRowIndex > -1)
                {
                    // удалим заявку из таблицы
                    var doc_guid = dataTableSupply.Rows[dataGridSupplyList.CurrentRowIndex].Field<string>(APPLICATIONS_TABLE.doc_guid);
                    var delResult = dbService.DeleteRecordFromTable(APPLICATIONS_TABLE.table_name, APPLICATIONS_TABLE.doc_guid + " = '" + doc_guid + "'");
                    if (!delResult.success)
                    {
                        MessageBox.Show(delResult.msg);
                    }
                    else
                    {
                        // перечитаем заявки
                        GetSupplyListFromDB();
                    }
                }
            }
        }
        
        private bool FollowToCheckMarkLayer() 
        {
            // при вводе поля Кол-во проверяем, акцизный товар
            var row = dataTableSupplyDetail.Rows[dataGridSupplyList.CurrentRowIndex];
            var akcis = row.Field<bool>(SUPPLY_DETAIL_TABLE.akcis);

            if (akcis)
            {
                // это акцизный товар, переходим на слой проверки акцизной марки
                currentGood.goodname = row.Field<string>(SUPPLY_DETAIL_TABLE.good_name);
                currentGood.guid = row.Field<string>(SUPPLY_DETAIL_TABLE.good_guid);

                textBoxGood.Text = row.Field<string>(SUPPLY_DETAIL_TABLE.good_name);
                checkBoxTovar.Checked = true;
                
                txtBarCode.Text = "";
                currentLayout = Layout.SetLayout(this, buttonMain, Layouts.CHECK_MARK_SUPPLY);
            }

            return akcis;
        }

        private void textBoxGoodCount_GotFocus(object sender, EventArgs e)
        {
            //FollowToCheckMarkLayer();
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///       КОНТЕКСТНОЕ МЕНЮ
        ///       

        private void menuItemDeleteRowLostPriceTable_Click(object sender, EventArgs e)
        {
            if (dataGridGoods.CurrentRowIndex > -1)
            {
                var good_guid = dataTableLostPrice.Rows[dataGridGoods.CurrentRowIndex].Field<string>("goodguid");

                dataTableLostPrice.Rows.RemoveAt(dataGridGoods.CurrentRowIndex);

                dbService.DeleteRecordFromTable("lostprice", "good_guid = '" + good_guid + "'");
            }
        }

        private void menuItemClearLostPriceDataGrid_Click(object sender, EventArgs e)
        {
            dataTableLostPrice.Rows.Clear();

            var result = dbService.DeleteAllRecordsFromTable("lostprice");
            if (!result.success)
            {
                MessageBox.Show(result.msg);
            }
        }

        private void menuItemBackToMainMenu_Click(object sender, EventArgs e)
        {
            BackToMainMenu();
        }

        private void menuItemUploadLostPriceTo1C_Click(object sender, EventArgs e)
        {
            var result = oneCService.PostLostPricesTo1cBase(dataTableLostPrice);
            MessageBox.Show(result.msg);
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {            
            CloseProgarm();
        }

        private void menuItemChangeServer_Click(object sender, EventArgs e)
        {
            appSettings = ChangeBase(appSettings.server_uri);

            if (!appSettings.success)
            {
                CloseProgarm();
            }
            else
            {
                oneCService.serverUri = appSettings.server_uri;
                labelMain.Text = appSettings.name;
            }
        }

        private void menuItemSupplyExit_Click(object sender, EventArgs e)
        {
            BackToMainMenu();
        }

        private void menuItemSupplyUpload1C_Click(object sender, EventArgs e)
        {
            // проверим не выгружалась ли эта накладная
            //textBoxGoodCount.Enabled = false;

            var result = oneCService.CheckSupplyUploaded(app_doc_guid);

            if (result.success)
            {
                if (String.IsNullOrEmpty(result.msg))
                {
                    // проверим накладную на совподение акцизных марок с кол-вом товара
                    result = CheckAkcisCount();

                    if (result.success)
                    {
                        // выбор склада для поступления
                        FillSkaldListFromSettings();

                        currentLayout = Layout.SetLayout(this, buttonMain, Layouts.SKLAD_SELECT);
                    }
                    else 
                    {
                        MessageBox.Show(result.msg);
                    }
                }
                else
                {
                    MessageBox.Show(result.msg);
                    BackToMainMenu();
                }
            }
            else
            {
                MessageBox.Show(result.msg);
                BackToMainMenu();
            }

            //textBoxGoodCount.Enabled = true;
        }

        private void menuItemNewSupply_Click(object sender, EventArgs e)
        {
            GetApplications();
        }

        private void menuItemEditSupply_Click(object sender, EventArgs e)
        {
            InitSupplyListDataTable();
            GetSupplyListFromDB();
        }

        private void menuItemSetGoodCountToZero_Click(object sender, EventArgs e)
        {
            // обнуление фактического количества

            if (dataGridSupplyList.CurrentRowIndex > -1) 
            {
                // обнулим факт кол-во в таблице просмотров
                var currentRow = dataTableSupplyDetail.Rows[dataGridSupplyList.CurrentRowIndex];
                currentRow.SetField<string>(SUPPLY_DETAIL_TABLE.good_count_fact, "0");

                var supply_detail_id = currentRow.Field<int>(SUPPLY_DETAIL_TABLE.id);

                // обнулим значение факт кол-ва в БД
                var updRec = new UpdateSqlRecordModel();
                updRec.tableName = SUPPLY_DETAIL_TABLE.table_name;
                updRec.expression = SUPPLY_DETAIL_TABLE.id + " = " + supply_detail_id;

                updRec.Fields = new SqlFieldModel[] 
                {
                    new SqlFieldModel
                    {
                        fieldName = SUPPLY_DETAIL_TABLE.good_count_fact,
                        fieldValue = 0
                    }
                };

                var result = dbService.UpdateTableRecord(updRec);
                if (!result.success)
                {
                    MessageBox.Show(result.msg);
                }
                
                // удалим марки из таблицы
                if (supplyMarksArray != null)
                {
                    var mark_row = supplyMarksArray.FirstOrDefault(r => r.table_id == supply_detail_id);
                    if (mark_row != null)
                    {
                        mark_row.currentCount = 0;
                        mark_row.bottles = new List<Bottle>();
                    }
                }

                //удалим записи о марках из БД
                result = dbService.DeleteRecordFromTable(APP_ALKO_DETAIL_TABLE.table_name, 
                    APP_ALKO_DETAIL_TABLE.supplydetail_id + " = " + supply_detail_id.ToString());

                if (!result.success) 
                {
                    MessageBox.Show(result.msg);
                }
            }
        }

        private void menuItemExitMark_Click(object sender, EventArgs e)
        {
            BackToMainMenu();
        }

        private void menuItemCheckFsrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(fsRarService.CheckConnection());
        }

        private void menuItemCheckConnection1C_Click(object sender, EventArgs e)
        {
            MessageBox.Show(oneCService.CheckConnection());
        }

        private void menuItemGetIP_Click(object sender, EventArgs e)
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

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///       КЛАВИАТУРА
        ///
      
        private void listBoxSupplyLayer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               ButtonClickHandler(buttonОК); 
            }

            if (e.KeyCode == Keys.Escape)
            {
                BackToMainMenu();
            }
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///       ПРОВЕРКА МАРКИ
        ///

        private bool FirstScan()
        {
            return String.IsNullOrEmpty(currentMark.pdf417) && String.IsNullOrEmpty(currentMark.dataMatrix);
        }

        private void CheckMarkSupplyScan(string barcode)
        {
            if (FirstScan())
            {
                textBoxLog.Text = "";
            }

            txtBarCode.Text = barcode;

            // pdf417
            if (barcode.Length == 68)
            {
                currentMark.pdf417 = fsRarService.CheckFormatPDF417(barcode);

                // проверка на дубликат
                // найдем товар в таблице
                var supplyItem = supplyMarksArray.FirstOrDefault(elem => elem.good_guid == currentGood.guid);

                // проверим не пробивалась ли бутылка ранее
                var result = supplyItem.bottles.FirstOrDefault(b => b.pdf417 == currentMark.pdf417);

                if (result != null)
                {
                    MessageBox.Show("Эта бутылка уже пробивалась");
                    ClearInputs();
                }
            }

            // ean8, ean13
            if (barcode.Length > 5 && barcode.Length <= 13)
            {
                textBoxGood.Text = "";
                currentGood.Clear();
                currentMark.Clear();

                //проверим на совпадение товара. поищем товар в таблице товаров
                int findedind = -1;

                for (var i = 0; i < dataTableSupplyDetail.Rows.Count; i++)
                {
                    var barcodeCell = dataTableSupplyDetail.Rows[i].Field<string>(SUPPLY_DETAIL_TABLE.barcode);

                    if (barcodeCell.IndexOf(barcode) > -1)
                    {
                        findedind = i;
                        dataGridSupplyList.CurrentRowIndex = i;
                        break;
                    }
                }

                if (findedind > -1)
                {                    
                    currentGood.goodname = dataTableSupplyDetail.Rows[findedind].Field<string>(SUPPLY_DETAIL_TABLE.good_name);
                    currentGood.guid = dataTableSupplyDetail.Rows[findedind].Field<string>(SUPPLY_DETAIL_TABLE.good_guid);

                    textBoxGood.Text = currentGood.goodname;
                }
                else 
                {
                    MessageBox.Show("Этот товар не найден в накладной");
                    currentGood.Clear();
                }
            }

            // data matrix
            if (barcode.IndexOf('-') > 0)
            {
                currentMark.dataMatrix = barcode;
            }

            // проверка марки в ФСРАР
            if (!String.IsNullOrEmpty(currentMark.pdf417) && 
                !String.IsNullOrEmpty(currentMark.dataMatrix) || checkBoxOldMark.Checked && 
                !String.IsNullOrEmpty(currentGood.guid))
            {
                if (checkBoxCheckOnline.Checked)
                { // проверяем онлайн
                    textBoxLog.Text += "Проверяем марку в ФСРАР..." + Environment.NewLine;

                    var checkResult = oneCService.CheckMarkFsRar(appSettings.imei, currentMark);

                    if (checkResult.success)
                    {
                        // проверка марки прошла успешно

                        if (String.IsNullOrEmpty(checkResult.good_name))
                        {
                            textBoxLog.Text = "Серия: " + checkResult.serial + " Номер: " + checkResult.number + Environment.NewLine;
                        }
                        else
                        {
                            textBoxLog.Text = "Товар: " + checkResult.good_name + Environment.NewLine +
                            "Производиитель: " + checkResult.producer + Environment.NewLine +
                            "Вид: " + checkResult.type + Environment.NewLine +
                            "Крепость: " + checkResult.alcperc + " Объем: " + checkResult.volume + Environment.NewLine +
                            "Серия: " + checkResult.serial + " Номер: " + checkResult.number + Environment.NewLine +
                            "Алко.код: " + checkResult.alcode + Environment.NewLine;
                        }

                        currentMark.serial = Convert.ToInt32(checkResult.serial);
                        currentMark.number = Convert.ToInt32(checkResult.number);

                        textBoxGood.Text = "";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(checkResult.serial))
                        {
                            textBoxLog.Text += checkResult.msg + Environment.NewLine;

                            // ошибка при проверке марки, запрос на повторение проверки

                            var answer = MessageBox.Show("Повторить проверку?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if (answer == DialogResult.No)
                            {
                                textBoxGood.Text = "";
                                currentMark.Clear();
                                currentGood.Clear();
                            }
                            else
                            {
                                //повторим запрос
                                textBoxLog.Text = "";
                                CheckMarkSupplyScan(barcode);
                            }
                        }
                        else
                        {
                            textBoxLog.Text = checkResult.msg + Environment.NewLine +
                                "Товар: " + checkResult.good_name + Environment.NewLine +
                                "Серия: " + checkResult.serial + " Номер: " + checkResult.number + Environment.NewLine;

                            textBoxGood.Text = "";

                            currentMark.Clear();
                            currentGood.Clear();
                        }
                    }
                }
                else 
                {
                    // онлайн не проверяем
                    var parseResult = ParseDataMatrix(currentMark.dataMatrix);

                    currentMark.serial = Convert.ToInt32(parseResult.serial);
                    currentMark.number = Convert.ToInt32(parseResult.number);

                    textBoxLog.Text = "Серия: " + parseResult.serial + " Номер: " + parseResult.number;
                }                
            }

            SetCheckBoxes();
            txtBarCode.Text = "";

        }

        private void CheckMarkScan(string barcode)
        {
            if (FirstScan())
            {
                textBoxLog.Text = "";
            }

            txtBarCode.Text = barcode;            

            // pdf417
            if (barcode.Length == 68)
            {                
                currentMark.pdf417 = fsRarService.CheckFormatPDF417(barcode);
            }
      
            // data matrix
            if (barcode.IndexOf('-') > 0)
            {
                currentMark.dataMatrix = barcode;

                var parseResult = ParseDataMatrix(barcode);
                textBoxLog.Text += parseResult.serial + "-" + parseResult.number;
            }

            // проверка марки в ФСРАР
            if (!String.IsNullOrEmpty(currentMark.pdf417) && (!String.IsNullOrEmpty(currentMark.dataMatrix) || checkBoxOldMark.Checked))
            {
                SetCheckBoxes();

                if (checkBoxCheckOnline.Checked)
                {// если нужно проверять онлайн
                    txtBarCode.Text = "";
                    textBoxLog.Text += "Проверяем марку в ФСРАР..." + Environment.NewLine;

                    var checkResult = oneCService.CheckMarkFsRar(appSettings.imei, currentMark);

                    if (checkResult.success)
                    {
                        if (String.IsNullOrEmpty(checkResult.good_name))
                        {
                            textBoxLog.Text = "Серия: " + checkResult.serial + " Номер: " + checkResult.number + Environment.NewLine;
                        }
                        else
                        {
                            textBoxLog.Text = "Товар: " + checkResult.good_name + Environment.NewLine +
                            "Производиитель: " + checkResult.producer + Environment.NewLine +
                            "Вид: " + checkResult.type + Environment.NewLine +
                            "Крепость: " + checkResult.alcperc + " Объем: " + checkResult.volume + Environment.NewLine +
                            "Серия: " + checkResult.serial + " Номер: " + checkResult.number + Environment.NewLine +
                            "Алко.код: " + checkResult.alcode + Environment.NewLine;
                        }

                        currentMark.serial = Convert.ToInt32(checkResult.serial);
                        currentMark.number = Convert.ToInt32(checkResult.number);

                        if (currentLayout == Layouts.CHECK_MARK)
                        {
                            currentMark.Clear();
                        }
                    }
                    else
                    {
                        textBoxLog.Text += checkResult.msg + Environment.NewLine;
                        currentMark.Clear();
                    }
                }
                else 
                { // онлайн не проверяем
                    var parseResult = ParseDataMatrix(txtBarCode.Text);

                    currentMark.serial = Convert.ToInt32(parseResult.serial);
                    currentMark.number = Convert.ToInt32(parseResult.number);  
                }
            }

            SetCheckBoxes();
            //txtBarCode.Text = "";

        }

        
        private DataMatrix ParseDataMatrix(string dataMatrix) 
        {
            var strInd = dataMatrix.IndexOf('-')+1;

            return new DataMatrix { serial = dataMatrix.Substring(strInd, 3), number = dataMatrix.Substring(strInd+3, 9) };
        }

        private void SetCheckBoxes()
        {
            checkBoxPDF417.Checked = !String.IsNullOrEmpty(currentMark.pdf417);
            checkBoxDataMatrix.Checked = !String.IsNullOrEmpty(currentMark.dataMatrix);

            if (currentLayout == Layouts.CHECK_MARK_SUPPLY)
            {
                checkBoxTovar.Checked = !String.IsNullOrEmpty(currentGood.goodname);

                if (checkBoxPDF417.Checked && checkBoxDataMatrix.Checked && checkBoxTovar.Checked)
                {
                    textBoxDay.Focus();
                }
            }
        }

        private void buttonOkData_Click(object sender, EventArgs e)
        {
            // проверялось ли в ФСРАР
            if (currentMark.serial == 0 || currentMark.number == 0) 
            {
                MessageBox.Show("Бутылка не проверена в ФСРАР");
                return;
            }

            // проверка корректности даты розлива
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

            var daterozliv = cyear.ToString() + "."+cmonth.ToString("D2") +"."+ cday.ToString("D2");

            DateTime dRozliv;

            try
            {
                dRozliv = DateTime.Parse(daterozliv);
            }
            catch(Exception)
            {
                MessageBox.Show("Неверный формат даты розлива");
                return;
            }

            // + запишем результат в таблицу марок и в базу данных терминала

            // найдем товар в таблице
            var supplyItem = supplyMarksArray.FirstOrDefault(elem => elem.good_guid == currentGood.guid);

            // проверим не пробивалась ли бутылка ранее
            var result = supplyItem.bottles.FirstOrDefault(b => b.pdf417 == currentMark.pdf417);

            if (result != null)
            {
                MessageBox.Show("Эта бутылка уже пробивалась");
            }
            else
            {
                // запишем новую бутылку в таблицу марок
                Bottle newBottle = new Bottle
                {
                    pdf417 = currentMark.pdf417,
                    serial = currentMark.serial,
                    number = currentMark.number,
                    dateRozliv = dRozliv
                };

                supplyItem.bottles.Add(newBottle);

                supplyItem.currentCount++;

                // нарастим значение в таблице просмотра
                int supply_detail_id = 0;
                DataRow currentRow;

                for (var i = 0; i < dataTableSupplyDetail.Rows.Count; i++)
                {
                    currentRow = dataTableSupplyDetail.Rows[i];
                    // найдем строку с нужным товаром
                    if (currentRow.Field<string>(SUPPLY_DETAIL_TABLE.good_guid) == currentGood.guid)
                    {
                        // обновим фактичесткое кол-во в таблице просмотра
                        currentRow.SetField<string>(SUPPLY_DETAIL_TABLE.good_count_fact, supplyItem.currentCount.ToString());
                        supply_detail_id = currentRow.Field<int>(SUPPLY_DETAIL_TABLE.id);

                        // запишем значение в базу из таблицы просмотра                        
                        var updRec = new UpdateSqlRecordModel();
                        updRec.tableName = SUPPLY_DETAIL_TABLE.table_name;
                        updRec.expression = SUPPLY_DETAIL_TABLE.id + " = " + supply_detail_id;

                        updRec.Fields = new SqlFieldModel[] 
                        {
                            new SqlFieldModel
                            {
                                fieldName = SUPPLY_DETAIL_TABLE.good_count_fact,
                                fieldValue = supplyItem.currentCount
                            }
                        };

                        var updateResult = dbService.UpdateTableRecord(updRec);

                        if (!updateResult.success)
                        {
                            MessageBox.Show(updateResult.msg);
                        }

                        break;
                    }
                }                

                // запишем новую бутылку в базу данных терминала
                var insertResult = dbService.WriteBottleToDB(supply_detail_id, newBottle);

                if (!insertResult.success)
                {
                    MessageBox.Show("Ошибка добавления записи в базу!" + Environment.NewLine + insertResult.msg);
                }

                txtBarCode.Text = "Проверено " + supplyItem.currentCount.ToString() + " из " + supplyItem.totalCount.ToString();
                textBoxLog.Text = "";
                
            }

            // - запишем результат в таблицу марок и в базу данных терминала

            ClearInputs();
        }

        private void ClearInputs()
        {
            currentMark.Clear();
            currentGood.Clear();

            textBoxYear.Text = "";
            textBoxMonth.Text = "";
            textBoxDay.Text = "";

            textBoxLog.Text = "";
            textBoxGood.Text = "";

            checkBoxOldMark.Checked = false;

            SetCheckBoxes();
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

        private void textBoxYear_GotFocus(object sender, EventArgs e)
        {
            textBoxYear.Text = "";
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

        private void textBoxMonth_GotFocus(object sender, EventArgs e)
        {
            textBoxMonth.Text = "";
        }

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

        private void dataGridSupplyList_Click(object sender, EventArgs e)
        {
            dataGridSupplyList.Select(dataGridSupplyList.CurrentRowIndex);
        }
               
    }
}