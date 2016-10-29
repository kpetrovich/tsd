
using System.Collections.Generic;
using System;
namespace Aida.Model
{
    
    public class Bottle 
    {
        public string pdf417;
        public DateTime dateRozliv;
        public int serial;
        public int number;
    }

    public class SupplyMarkArray 
    {
        public int table_id;
        public string good_guid;
        public string good_name;
        public string barcodeEAN;
        public double totalCount;
        public double currentCount;
        public List<Bottle> bottles;
    }

    public class CurrentMark
    {
        public string good_name;
        public string barcode;
        public string dataMatrix;
        public string pdf417;
        public int serial;
        public int number;
        public DateTime dateRozliv;

        public void Clear()
        {
            good_name = "";
            barcode = "";
            dataMatrix = "";
            pdf417 = "";
            dateRozliv = DateTime.MinValue;
            serial = 0;
            number = 0;
        }
    }

    public class MarInfokRequest 
    {
        public string pdf417;
        public string datamatrix;
        public string imei;
        public string key;
    }

    public class DataMatrix
    {
        public string serial;
        public string number;
    }
}
