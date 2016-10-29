using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Aida.Model
{
    public enum Layouts 
    {
        MAIN_MENU = 0,
        CHECK_PRICE,
        LOST_PRICE,
        SUPPLY,
        INVENTORY,
        CHECK_MARK,
        CONTRAGENT_SELECT,
        APPLICATION_SELECT,
        SUPPLY_SELECT,
        SKLAD_SELECT,
        CHECK_MARK_SUPPLY
    }
    
    public class Layout 
    {
        private static void SetUnvisible(Control ctrl)
        {
            var list = ctrl.Controls.Cast<Control>();
            foreach (var item in list)
            {
                item.Visible = false;

                SetUnvisible(item);
            }
        }

        private static void SetVisibleControls(Control control, string[] strNameControls)
        {
            var list = control.Controls.Cast<Control>();
            foreach (var item in list)
            {
                if(strNameControls.Any<string>(str => str == item.Name))
                {
                    item.Visible = true;
                    // установим видимость для подчиненных элементов управления
                    foreach (var ctrl in item.Controls.Cast<Control>()) 
                    {
                        ctrl.Visible = true;
                    }
                }

                if (strNameControls.First<string>() == item.Name)
                {
                    item.Focus();
                }

                SetVisibleControls(item, strNameControls);
            }
        }

        public static Layouts SetLayout(MainMenuForm form, Control mainButton, Layouts layout)
        {
            SetUnvisible(form);

            var ver = " " + form.version;

            switch (layout)
            {
                case Layouts.MAIN_MENU:
                    {
                        form.Text = "АИДА" + ver;
                        mainButton.Text = "МЕНЮ";

                        string[] controlsList = 
                        { 
                            "buttonCheckPrice", "labelMain",  "buttonCenniki", "buttonPrihod", "buttonInventarka", "buttonCheckMark", "buttonMain" 
                        };

                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.CHECK_PRICE:
                    {                        
                        form.Text = "ЦЕНЫ";
                        mainButton.Text = "НАЗАД";

                        string[] controlsList = 
                        { 
                            "listBoxSklads", "textBoxResult" , "buttonMain" 
                        };

                        SetVisibleControls(form, controlsList);
                        
                        break;
                    }
                case Layouts.LOST_PRICE:
                    {
                        form.Text = "Печать цен";
                        mainButton.Text = "МЕНЮ";

                        string[] controlsList = 
                        { 
                            "dataGridGoods", "buttonMain" 
                        };

                        SetVisibleControls(form, controlsList);
                        
                        break;
                    }
                case Layouts.CONTRAGENT_SELECT: 
                    {
                        form.Text = "Поставщики";

                        string[] controlsList = 
                        { 
                            "listBoxSupplyLayer", "buttonCancel", "buttonОК", "labelGettingGoodsList"
                        };

                        SetVisibleControls(form, controlsList);
                        

                        break;
                    }
                case Layouts.APPLICATION_SELECT:
                    {
                        form.Text = "Заявки";

                        string[] controlsList = 
                        { 
                            "listBoxSupplyLayer", "buttonCancel", "buttonОК", "labelGettingGoodsList"
                        };

                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.SUPPLY:
                    {
                        form.Text = "Приход";
                        mainButton.Text = "МЕНЮ";

                        string[] controlsList = 
                        { 
                            "dataGridSupplyList", "buttonMain", "labelContragent", "labelGoodCount", "textBoxGoodCount"
                        };

                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.SUPPLY_SELECT:
                    {
                        form.Text = "Приходы";

                        string[] controlsList = 
                        { 
                            "dataGridSupplyList", "buttonCancel", "buttonОК", "buttonDeleteApplication"
                        };

                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.SKLAD_SELECT:
                    {
                        form.Text = "Склады";

                        string[] controlsList = 
                        { 
                            "listBoxSklads", "buttonCancel", "buttonОК"
                        };

                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.CHECK_MARK:
                    {
                        form.Text = "Проверка марки";

                        string[] controlsList = 
                        { 
                            "txtBarCode", "checkBoxDataMatrix", "checkBoxPDF417", "textBoxGood", 
                            "textBoxLog", "checkBoxOldMark", "buttonMain", "checkBoxCheckOnline"
                            
                        };
                        
                        SetVisibleControls(form, controlsList);

                        break;
                    }
                case Layouts.CHECK_MARK_SUPPLY:
                    {
                        form.Text = "Проверка марки";
                        mainButton.Text = "НАЗАД";

                        string[] controlsList = 
                        { 
                            "txtBarCode", "checkBoxTovar", "checkBoxDataMatrix", "checkBoxPDF417", "textBoxGood", 
                            "textBoxLog", "checkBoxOldMark", "buttonMain", "buttonOkData", "label1", "button1", 
                            "textBoxDay", "textBoxMonth", "textBoxYear", "label2", "label3", "checkBoxCheckOnline"                            
                        };
                        
                        SetVisibleControls(form, controlsList);

                        break;
                    }
            }

            return layout;
        }
    }
}
