using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aida.Model
{
    public class LOSTPRICE_TABLE
    {
        public const string table_name = "lostprice";
        // fields
        public const string good_guid = "good_guid";
        public const string good_name = "good_name";
    }

    public class APPLICATIONS_TABLE
    {
        public const string table_name = "applications";
        // fields
        public const string id = "id";
        public const string contragent = "contragent";
        public const string doc_guid = "doc_guid";
        public const string doc_number = "doc_number";
        public const string doc_date = "doc_date";
        public const string uploaded = "uploaded";
    }

    public class SUPPLY_DETAIL_TABLE
    {
        public const string table_name = "supply_detail";
        // fields
        public const string id = "id";
        public const string app_doc_id = "app_doc_id";
        public const string good_guid = "good_guid";
        public const string good_name = "good_name";
        public const string barcode = "barcode";
        public const string good_count_doc = "good_count_doc";
        public const string good_count_fact = "good_count_fact";
        public const string akcis = "akcis";
        public const string dopis = "dopis";
        public const string roworder = "roworder";
    }

    public class APP_ALKO_DETAIL_TABLE
    {
        public const string table_name = "app_alko_detail";
        // fields
        public const string id = "id";
        public const string supplydetail_id = "supplydetail_id";
        public const string mark = "mark";
        public const string mark_serial = "mark_serial";
        public const string mark_number = "mark_number";
        public const string date_rozliv = "date_rozliv";
    }

    
    public class SqlFieldModel 
    {
        public string fieldName;
        public object fieldValue;
    }

    public class InsertSqlRecordModel 
    {
        public string tableName;
        public SqlFieldModel[] Fields;

        public string CreateSqlInsertCmd()
        {
            
            string fildNames = "";
            string fildValues = "";

            foreach (var field in Fields) 
            {
                if (String.IsNullOrEmpty(fildNames))
                {
                    fildNames = fildNames + field.fieldName;
                }
                else
                {
                    fildNames = fildNames + "," +field.fieldName;
                }

                var fieldValue = field.fieldValue;

                if (fieldValue.GetType() == typeof(double))
                {
                    fieldValue = fieldValue.ToString().Replace(',', '.');
                }

                if (String.IsNullOrEmpty(fildValues))
                {
                    fildValues = fildValues + fieldValue;
                }
                else
                {   
                    fildValues = fildValues + "," + fieldValue;
                }                
            }

            return "INSERT INTO " + tableName + " (" + fildNames + ") VALUES (" + fildValues + ")";

        }
    }

    public class UpdateSqlRecordModel
    {
        public string expression;
        public string tableName;
        public SqlFieldModel[] Fields;

        public string CreateSqlUpdateCmd()
        {            
            string setText = "";

            foreach (var field in Fields)
            {

                var fieldValue = field.fieldValue;

                if (fieldValue.GetType() == typeof(double)) 
                {
                    fieldValue = fieldValue.ToString().Replace(',', '.');
                }

                if (String.IsNullOrEmpty(setText))
                {
                    setText = setText + field.fieldName + " = " + fieldValue;                
                }
                else
                {
                    setText = setText + ", " + field.fieldName + " = " + fieldValue;
                }
            }

            return "UPDATE " + tableName + " SET " + setText + " WHERE " + expression;
        }
    }

    public class LostPrice 
    {
        public string GoodGuid;
        public string GoodName;
    }

    public class SupplyDetailDB
    {
        public List<SupplyDetailRow> rows;
    }

    public class SupplyDetailRow
    {
        public int id;
        public int app_doc_id;
        public string good_guid;
        public string good_name;
        public string barcode;
        public double good_count_doc;
        public double good_count_fact;
        public bool akcis;
        public bool dopis;
        public int roworder;
    }

    public class ApplicationDB 
    {
        public int id;
        public string contragent;
        public string doc_number;
        public string doc_guid;
        public string doc_date;
        public bool uploaded;
    }
}
