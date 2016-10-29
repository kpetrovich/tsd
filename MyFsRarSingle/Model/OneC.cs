using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CodeBetter.Json;

namespace Aida.Model
{
    [SerializeIncludingBaseAttribute]
    public class Good1C : Result
    {      
        public string goodname;
        public string guid;

        public Good1C()
        {
            goodname = "";
            guid = "";
            msg = "";
            success = false;
        }

        public void Clear()
        {
            goodname = "";
            guid = "";
            msg = "";
            success = false;
        }
    }

    public class MarkRecord1c
    {
        public string guid;
        public string markserial;
        public string marknumber;
        public string pdf417;
        public string daterozliv;

        public void Clear()
        {
            guid = "";
            markserial = "";
            marknumber = "";
            pdf417 = "";
            daterozliv = "";
        }
    }

    //[SerializeIncludingBaseAttribute]
    //public class MarkInfo : Result
    //{
    //    public string guid;
    //    public string goodname;
    //    public string serialnumber;
    //    public string daterozliv;
    //}

    [SerializeIncludingBaseAttribute]
    public class BaseSettingsArray : Result
    {
        public BaseSettings[] settings;
    }

    public class BaseSettings : Result
    {
        public string code;
        public string server_uri;
        public string name;
        public string imei;
        public Sklad[] sklads;
    }

    [SerializeIncludingBaseAttribute]
    public class Price : Result 
    {
        public string pricetype;
        public string good;
        public string price;
    }

    public class LostPriceList 
    {
        public string[] list;
    }

    [SerializeIncludingBaseAttribute]
    public class ApplicationList : Result
    {
        public ApplicationDoc[] applist;
    }

    public class ApplicationDoc 
    {
        public string doc_guid;
        public string doc_number;
        public string doc_date;
        public string contragent;
        public bool uploaded;
        public string comment;
    }

    [SerializeIncludingBaseAttribute]
    public class ApplicationDocument : Result
    {
        public ApplicationDocumentRow[] rows;
    }

    public class ApplicationDocumentRow 
    {
        public string good_guid;
        public string good_name;
        public string barcode;
        public double good_count_doc;
        public double good_count_fact;
        public bool akcis;
        public bool dopis;
        //public int order;
    }

    public class UploadSupplyRow 
    {
        public int supply_detail_id;
        public string good_guid;
        public double good_count_fact;
        public int order;
    }

    public class UploadSupplyMarkRow 
    {
        public int supply_detail_id;
        public string mark;
        public int mark_serial;
        public int mark_number;
        public string date_rozliv;
    }

    public class UploadSupplyDoc 
    {
        public string app_doc_guid;
        public string sklad_code;
        public UploadSupplyRow[] supply_list;
        public UploadSupplyMarkRow[] mark_list;
    }

    [SerializeIncludingBaseAttribute]
    public class CheckMarkResult : Result 
    {
        public string good_name;
        public string producer;
        public string type;
        public string alcperc;
		public string volume;
		public string alcode;
		public string serial;
		public string number;
    }
}
