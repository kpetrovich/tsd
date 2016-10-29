using System;
using System.Net;
using System.IO;
using CodeBetter.Json;
using Aida.Model;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Aida.Service
{

    public class OneCService
    {
        public string serverUri;
        public bool enableTimeoutCheck;
        private int keyCounter;

        #region Keys
        private string[] keyArrayChehov = {
                                    "1v8lbuXuXmE/mEEqvNsH/z08RwxQB2MFYBfx3kUmdyA=",
                                    "kimE0t+EcdSj1WgBHbkbIJD30AyrJ+bMwwepvxxPTng=",
                                    "UKs+OXd9WbpmNJ6T5aq+NTIfOhLPyIh39pTWe/QaI9o=",
                                    "BPubaJ8UrgaqCszn0SidRnQsRZ/sdsnNRXWhXdQlweE=",
                                    "UNSNpZd8Vj0zo2oPhRo33Qh7E8cNs1rmhDtRXzlH7fI=",
                                    "UKs+OXd9WbpmNJ6T5aq+NTIfOhLPyIh39pTWe/QaI9o=",
                                    "zK0JCs32oLHN6AumyGLqDsPWlJP+BBlUJNc/UbJeDW0=",
                                    "QMHD0wpjNYlNx9rcI4XKCBa9OAfDKXhrMHQ+dvU/iNI=",
                                    "LGnSZm/qT9w0vHG60MMxSbg2hdcYG9lAbcT32CL7ruY=",
                                    "QIRNKzJqH8Y/yynQMV31tuQ8V+On/igfcN4dNvsGfcc=",
                                    "fxGNAbmtCvZBY8nlLurO+slMBr0ShPP1iZYmwDWxpf4=",
                                    "M3kaR0nQbDLWiovdYuiG6izhMDBRJL31rae0Uu0w7Zc=",
                                    "UBrbRRHn7UF2exYBQuPft24vBiceKB/wxbZoIylCJ9I=",
                                    "yHUg3Z1Zy0pYB6/P3z5Aj07U00mgpwgS25BcwBkuKR4=",
                                    "REyvTTP/0hW0wLlfG6eHG3fXpTn1CSRDxll/9LLFZVk=",
                                    "XBMPpqcbXh3TWzMg34W4kjomJCnI0YDTuiK2FV2MWhQ=",
                                    "orpIm5e4S2Ll4XxWA14+PEJh0HLtzv1K0frZDaJXr64=",
                                    "OBhfhyjZ+m+kj1R5Ljqd11b5SyN5VLz2HowI7v+kc8o=",
                                    "S0VdgXjPTl+kw9121Dx/ImjH++FUrO/VMVsw8FI8Eg0=",
                                    "hSMy/bDDXQz+lTaVl/R517cM7hSSdhMIujhRiOghJqA=",
                                    "etPU8RE8K2vkoUUSALplTNseLyoBQsgbXrNrIcAlVjE=",
                                    "mlk9jF+VhsCPAttBBTfqjVy8XCsRSXDfLudqOkbUBYw=",
                                    "4pmGzqMBQu5ZSxm2PD/XGmmWh3Ir4R2pV2y3722WWkA=",
                                    "FoGjVXlxCGZ3rMM1DvBwTlhkLVAyRwscBDXN4+vLw9U=",
                                    "EvCMydfLKMFl5nRrhPHS0bzTVnObfO3QEFUaO2cnxmI=",
                                    "eXs1fCem6GHrAmgDTQdhU5gRH/99GyCBWnF1Y0Nfb3s=",
                                    "eJrJ3oGFDHfNV37Cv0DqmYk0xWqVLacfDaIBUl+SiJk="
        };

        private string[] keyArrayAlex = {
                                    "iG/W2J+GYelAELNq1wplElcpCSXnT/UzR46hPsjqS9M=",
                                    "JoY0zU4OADLjtnesGS7BrRKS2QHJLuzMS5waWfQyNDg=",
                                    "HW+nD2mPqrXeeDEMfsB8GF++RYEiQQTwBLJMIfyTSnQ=",
                                    "78l31clAVaE90pUyl8S/WnO2xIftLWPoCJt9NCYOy8A=",
                                    "CnX5GFKPEmVrzn25Y79yWdRafHrKB+Lysqu0smHrpBI=",
                                    "vEha/LucnNKPiCpvR51BCX+B5pDl7gSgsRloqNqMZnA=",
                                    "fZBnlGeNMpVQX90m89lgrKLRAhZmLuaFGWNHoRCjud0=",
                                    "QBhv4JBQ0+maCvfY7iy3nTcOgnxBb0LgvesYAoR+PAE=",
                                    "Kxhmdjw6OAnJUmZKlQfDVpay1Bv3oow59l3mqkwIpto=",
                                    "MX4q97zj/seAnYFRlhsOSYrb5kZgj5H1E0bzs99Bcws=",
                                    "odUc90K844ebD48Kk9x5t4S+/EAfpChNQCN6L+btBe8=",
                                    "JbeS7RwL8ZaDq6Gnp21uUxAcyGaTcNBP8fOlyAGfMJU=",
                                    "SWXxJeGp+Ok7OGFXpGohbhvXlcrYVIa0ANLDZjlXyYY=",
                                    "V/5jhHigZz4lOHGGXW3RfpXm6o+FJmW6eYzAXAzj1EY=",
                                    "sKRQpj9cgm4fkupfEvO3jjZ1fzAa/2kKGILfWOY3myg=",
                                    "4n617PdFDuP6SrVFcJb+JqzQFHTE+y2wp+iQIC2KqTE=",
                                    "uPv5RYmLAnQHoerekZ2FY736ZjhmxBGExCoV3HDd/dE=",
                                    "YGPdYVt79BYoxw36pgpP7PAE78I6Z9kmx4DwvACJyxg=",
                                    "N21j/jZuCS+/p0pJYJYAu5HHvwHnxGeV2NV7cCYqn6c=",
                                    "Oqi8NeJpglf2YpRGvuzFhta8KZQVPOj7zuFDHvKd/eA=",
                                    "kHPEeZpSgeWRmdQvG6gsnJRQvOU2dipCtMlSQsKao/I=",
                                    "cWf+99NTmlS3UxQ3PE+CXe2MUqsyhKFLYf/0CYEB0Yo="
        };
        #endregion

        private string GetKey(string IMEI)
        {
            if (IMEI == "359628043907471")
            {
                if (keyCounter >= keyArrayChehov.Length - 1)
                {
                    keyCounter = 0;
                }
                var key = keyArrayChehov[keyCounter];
                keyCounter++;

                return key;
            }

            if (IMEI == "359628044048382")
            {
                if (keyCounter >= keyArrayAlex.Length - 1)
                {
                    keyCounter = 0;
                }

                var key = keyArrayAlex[keyCounter];
                keyCounter++;

                return key;
            }

            return "";
        }

        private class TestResult
        {
            public string testResult = "";
        }

        private class PriceRequest 
        {
            public string barcode;
            public string skladcode;
        }

        private class AppListRequset 
        {
            public string code;
        }

        private class AppRowsRequset
        {
            public string doc_guid;
        }

        public class CheckSupplyUpload 
        {
            public string app_doc_guid;
        }
        
        public string CheckConnection() 
        {
            string body = "";
            // {"testResult":"OK"}
            try
            {
                Uri uri = new Uri("http://" + serverUri + "hs/tsd/test");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = true;
                
                if (enableTimeoutCheck)
                {
                    request.Timeout = 3000;
                }

                request.AllowWriteStreamBuffering = false;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            body = reader.ReadToEnd();

                            var test = Converter.Deserialize<TestResult>(body);
                            return test.testResult;
                        }
                    }
                }

                return body;
            }
            catch (Exception ex)
            {
                return "Ошибка соединения!!!" + Environment.NewLine + ex.Message;
            }
        }
        
        public Good1C GetTovar(string barcode)
        {
            var good = new Good1C();           
                        
            try
            {
                Uri uri = new Uri("http://" + serverUri + "hs/tsd/goods/" + barcode);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowWriteStreamBuffering = false;

                if (enableTimeoutCheck)
                {
                    request.Timeout = 3000;
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();
                            // {"success": true, "msg": "", "guid":"be53759c-0319-11e3-8c7a-00224d4fec88", "goodname":"Водка"}
                            good = Converter.Deserialize<Good1C>(body);
                            //good.msg = body;
                            //good.success = false;
                            return good;
                        }
                    }                    
                }

                good.success = false;
                good.msg = "Ошибка соединения!!!";

                return good;
            }
            catch (Exception ex)
            {
                good.success = false;
                good.msg = "Ошибка соединения!!!" + Environment.NewLine + ex.Message;

                return good;
            }            
        }

        public BaseSettingsArray GetBaseSettings()
        {
            var baseSettings = new BaseSettingsArray();

            try
            {                
                Uri uri = new Uri("http://" + serverUri + "hs/tsd/settings");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowWriteStreamBuffering = false;

                if (enableTimeoutCheck)
                {
                    request.Timeout = 10000;
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();
                            // {"success": true, "msg": "", "guid":"be53759c-0319-11e3-8c7a-00224d4fec88", "goodname":"Водка"}
                            baseSettings = Converter.Deserialize<BaseSettingsArray>(body);
                            baseSettings.success = true;

                            return baseSettings;
                        }
                    }
                }

                baseSettings.success = false;
                baseSettings.msg = "Ошибка получения списка баз!!!";

                return baseSettings;
            }
            catch (Exception ex)
            {
                baseSettings.success = false;
                baseSettings.msg = "Ошибка соединения!!!" + Environment.NewLine + ex.Message;

                return baseSettings;
            }
        }

        // ?????
        //public Result PostMarkTo1cBase(string server1CUri, MarkRecord1c markRec)
        //{
        //    try
        //    {
        //        var bodyJson = Converter.Serialize(markRec);
        //        byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

        //        Uri uri = new Uri("http://" + server1CUri + "hs/tsd/addmark");
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //        request.Method = "POST";
        //        request.KeepAlive = false;
        //        request.ContentLength = buffer.Length;
        //        request.ContentType = "text/json";
        //        request.Timeout = 3000;

        //        // post data
        //        using (Stream postData = request.GetRequestStream())
        //        {
        //            postData.Write(buffer, 0, buffer.Length);
        //            postData.Close();
        //        }

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        using (Stream stream = response.GetResponseStream())
        //        {
        //            if (stream != null)
        //            {
        //                using (StreamReader reader = new StreamReader(stream))
        //                {
        //                    var body = reader.ReadToEnd();

        //                    var res = Converter.Deserialize<Result>(body);
        //                    return new Result { success = res.success, msg = res.msg };
        //                }
        //            }
        //        }

        //        return new Result { success = false, msg = "Ошибка отправки в 1С !!!" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
        //    }
        //}
        
        public Result PostLostPricesTo1cBase(DataTable dataTable)
        {            

            var list = new string[dataTable.Rows.Count];
            
            for (int count =0; count < dataTable.Rows.Count; count++)
            {
                list[count] = dataTable.Rows[count].Field<string>("goodguid");
            }

            try
            {
                var bodyJson = Converter.Serialize(new LostPriceList { list = list });
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/uploadlostprice");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 3000;
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<Result>(body);
                            return new Result { success = res.success, msg = res.msg };
                        }
                    }
                }

                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }
        }

        // ?????
        
        //public MarkInfo GetMarkInfo(string barcode, string serverUri) 
        //{
        //    try
        //    {
        //        Uri uri = new Uri("http://" + serverUri + "hs/tsd/getmark/" + barcode);
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //        request.Method = "GET";
        //        request.KeepAlive = true;
        //        request.AllowWriteStreamBuffering = false;
        //        request.Timeout = 3000;

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        using (Stream stream = response.GetResponseStream())
        //        {
        //            if (stream != null)
        //            {
        //                using (StreamReader reader = new StreamReader(stream))
        //                {
        //                    var body = reader.ReadToEnd();

        //                    // {"success": true, "msg": "", "guid":"a3b2730b-802c-11e4-98e4-5474b0227b0c", "goodname":"...","serialnumber":"103-554319934","daterozliv":"14.11.2014"}
        //                    var markInfo = Converter.Deserialize<MarkInfo>(body);
        //                    return markInfo;
        //                }
        //            }
        //        }

        //        return new MarkInfo { success = false, msg = "Error!"};
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MarkInfo { success = false, msg = ex.Message };
        //    }    
        //}
    
        public Price GetPrice(string barcode, string skladcode)
        {
            try
            {
                // {"barcode":"123123123", "skladcode":"------"}

                var bodyJson = Converter.Serialize(new PriceRequest { barcode = barcode, skladcode = skladcode});
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/goodprice/");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 3000;
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<Price>(body);

                            return res;
                        }
                    }
                }

                return new Price { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new Price { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }
        }

        public ApplicationList GetApplicationList(string settingsCode)
        {
            try
            {
                // {
                //  "applist": [
                //      {
                //    "doc_guid": "95f767d7-68fb-11e6-9a8b-00224d4fec88",
                //    "doc_number": "ЦЛ000015929",
                //    "doc_date": "23.08.16",
                //    "contragent": "XXI ВЕК колбаса",
                //    "comment": "----"
                //    },
                //    { ... }
                //    ]
                //}    

                var bodyJson = Converter.Serialize(new AppListRequset { code = settingsCode });
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/getapplist");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 10000;
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<ApplicationList>(body);
                            
                            return res;
                        }
                    }
                }

                return new ApplicationList { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new ApplicationList { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }            
        }

        public Result UploadSupplyTo1cBase(string appdocguid, DataRowCollection rows, string sklad_code, List<SupplyMarkArray> supplyMarkArray)
        {
            try
            {
                // {
                //		"app_doc_guid" : "95f767d7-68fb-11e6-9a8b-00224d4fec88",
                //		"sklad_code" : "qwe",
                //		"supply_list": [
                //			{
                //              "supply_detail_id": 123,
                //				"good_guid": "95f767d7-68fb-11e6-9a8b-00224d4fec88",
                //				"good_count_fact": 3,
                //              "order": 3
                //			},
                //			{
                //				...
                //			}
                //			],
                //      "marks_list":[
                //          {
                //              "supply_detail_id": 123,
                //              "mark" : "22N0000152419RB6Y9F37TX60607003125768Q62WONFLURRG9XMXD3G29PAG6JIWZ76",
                //              "mark_serial" : 102,
                //              "mark_number" : 169305768,
                //              "date_rozliv" : "16.06.10"
                //          }
                //      ]
                // }

                var uploadSupply = new UploadSupplyDoc();
                uploadSupply.app_doc_guid = appdocguid;
                uploadSupply.sklad_code = sklad_code;
                uploadSupply.supply_list = new UploadSupplyRow[rows.Count];

                // заполним массив товаров
                for (int i = 0; i < rows.Count; i++)
                {
                    var uploadSupplyRow = new UploadSupplyRow {
                        supply_detail_id = rows[i].Field<int>(SUPPLY_DETAIL_TABLE.id),
                        good_guid = rows[i].Field<string>(SUPPLY_DETAIL_TABLE.good_guid),
                        good_count_fact = Convert.ToDouble(rows[i].Field<string>(SUPPLY_DETAIL_TABLE.good_count_fact)),
                        order = Convert.ToInt16(rows[i].Field<int>(SUPPLY_DETAIL_TABLE.roworder))
                    };

                    uploadSupply.supply_list[i] = uploadSupplyRow;
                }

                // заполним массив марок
                if (supplyMarkArray != null && supplyMarkArray.Count > 0)
                {
                    int totalMarksCount = 0;
                    for (int i = 0; i < supplyMarkArray.Count; i++) 
                    {                        
                        totalMarksCount += supplyMarkArray[i].bottles.Count;                        
                    }

                    uploadSupply.mark_list = new UploadSupplyMarkRow[totalMarksCount];

                    int currentMarksCount = 0;
                    // цикл по таблице акцизных товаров
                    for (int i = 0; i < supplyMarkArray.Count; i++) 
                    {
                        var supply_detail_id = supplyMarkArray[i].table_id;

                        // цикл по акцизным маркам 
                        for (int j = 0; j < supplyMarkArray[i].bottles.Count; j++)
                        {
                            var mark_row = supplyMarkArray[i].bottles[j];

                            var uploadMarkRow = new UploadSupplyMarkRow
                            {
                                supply_detail_id = supply_detail_id,
                                mark = mark_row.pdf417,
                                mark_serial = mark_row.serial,
                                mark_number = mark_row.number,
                                date_rozliv = mark_row.dateRozliv.ToString("dd.MM.yy")
                            };

                            uploadSupply.mark_list[currentMarksCount] = uploadMarkRow;
                            currentMarksCount++;
                        }
                    }
                }

                var bodyJson = Converter.Serialize(uploadSupply);
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/postsupply");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 30000; // 30 sec
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<Result>(body);
                            
                            return res;
                        }
                    }
                }

                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }            
        }

        public Result CheckSupplyUploaded(string appdocguid)
        {
            try
            {
                // {"app_doc_guid" : "95f767d7-68fb-11e6-9a8b-00224d4fec88" }
                var check = new CheckSupplyUpload { app_doc_guid = appdocguid };
                var bodyJson = Converter.Serialize(check);
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/checksupplyupload");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 20000; // 20 sec
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<Result>(body);

                            return res;
                        }
                    }
                }

                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new Result { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }
        }

        public ApplicationDocument GetApplicationDocumentRows(string app_doc_guid)
        {
            try
            {
                // *result*
                // {
                //  "rows": [
                //      {
                //    "good_guid": "95f767d7-68fb-11e6-9a8b-00224d4fec88",
                //    "good_name": "Конфеты, КГ",
                //    "barcode": "1234567890123",
                //    "good_count_doc": "5",
                //    "akcis": false,
                //    "dopis": false
                //    },
                //    { ... }
                //    ]
                //}    

                var bodyJson = Converter.Serialize(new AppRowsRequset { doc_guid = app_doc_guid });
                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/getapprows");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 10000;
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var res = Converter.Deserialize<ApplicationDocument>(body);
                            
                            return res;
                        }
                    }
                }

                return new ApplicationDocument { success = false, msg = "Ошибка запроса товаров из 1С !!!" };
            }
            catch (Exception ex)
            {
                return new ApplicationDocument { success = false, msg = "Ошибка запроса товаров из 1С !!!" + Environment.NewLine + ex.Message };
            }
        }

        public CheckMarkResult CheckMarkFsRar(string imei, CurrentMark markInfo) 
        {
            try
            {
                var bodyJson = Converter.Serialize(new MarInfokRequest { 
                    datamatrix = markInfo.dataMatrix, 
                    imei = imei, 
                    pdf417 = markInfo.pdf417, 
                    key = GetKey(imei) 
                } );

                byte[] buffer = Encoding.UTF8.GetBytes(bodyJson);

                Uri uri = new Uri("http://" + serverUri + "hs/tsd/checkmark");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentLength = buffer.Length;
                request.ContentType = "text/json";

                if (enableTimeoutCheck)
                {
                    request.Timeout = 3000;
                }

                // post data
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                    postData.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var body = reader.ReadToEnd();

                            var result = Converter.Deserialize<CheckMarkResult>(body);

                            return result;
                        }
                    }
                }

                return new CheckMarkResult { success = false, msg = "Ошибка отправки в 1С !!!" };
            }
            catch (Exception ex)
            {
                return new CheckMarkResult { success = false, msg = "Ошибка отправки в 1С !!!" + Environment.NewLine + ex.Message };
            }
        }
    }
}
