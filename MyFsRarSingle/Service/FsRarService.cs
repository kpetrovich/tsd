using System;
using System.Net;
using System.IO;
using CodeBetter.Json;
using Aida.Model;
using System.Text;

namespace Aida.Service
{  

    public class FsRarService
    {        
        private class TestResult
        {
            public string speedResult = "";
        }

        private int keyCounter;        

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

        private string GetKey(string IMEI) 
        {   
            if (IMEI == "359628043907471") 
            {
                if (keyCounter == keyArrayChehov.Length - 1)
                {
                    keyCounter = 0;
                }

                return keyArrayChehov[keyCounter];
            }

            if (IMEI == "359628044048382")
            {
                if (keyCounter == keyArrayAlex.Length - 1)
                {
                    keyCounter = 0;
                }

                return keyArrayAlex[keyCounter];
            }

            return "";
        }

        //public Mark CheckMark(string IMEI, string pdf417, string dataMatrix) 
        //{
        //    try
        //    {
        //        //Encoding the post vars
        //        var body = "{\"IMEI\":\""+IMEI+"\",\"Key\":\""+GetKey(IMEI)+"\"}";

        //        byte[] buffer = Encoding.ASCII.GetBytes(body);

        //        Uri uri = new Uri("http://146.120.90.77:7076/mark/" + pdf417 + "/" + dataMatrix);

        //        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);

        //        //Set method to post, otherwise postvars will not be used
        //        webReq.Method = "POST";

        //        // init HEADERS
        //        // entity
        //        webReq.ContentLength = buffer.Length;
        //        webReq.ContentType = "text/json";
        //        webReq.KeepAlive = false;
        //        webReq.Timeout = 6000;

        //        //return new MarksFsrar { Success = true, ResultText = sdf };

        //        // post data
        //        using (Stream postData = webReq.GetRequestStream())
        //        {
        //            postData.Write(buffer, 0, buffer.Length);
        //            postData.Close();
        //        }

        //        //Get the response handle, we have no true response yet
        //        string bodyResponse = "";
        //        using (HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse())
        //        using (Stream webResponse = webResp.GetResponseStream())
        //        {
        //            if (webResponse == null)
        //            {
        //                return new Mark { success = false, msg = "Error" };
        //            }
                    
        //            using (StreamReader response = new StreamReader(webResponse))
        //            {
        //                bodyResponse = response.ReadToEnd();
        //            }
        //        }
                
        //        //return new Mark { success = true, msg = bodyResponse };

        //        MarkResponse markResponse = Converter.Deserialize<MarkResponse>(bodyResponse);

        //        if (markResponse.GetMarkInfoResult.Success)
        //        {
        //            var arrayKeyValue = markResponse.GetMarkInfoResult.MarkData.MarkInfoResult;
        //            var mark = new Mark { success = true };

        //            if (arrayKeyValue.Length > 0)
        //            {
        //                foreach (var pair in arrayKeyValue)
        //                {
        //                    if (pair.Key == "Тип марки") mark.MarkType = pair.Value;
        //                    if (pair.Key == "Серия") mark.ShSeries = pair.Value;
        //                    if (pair.Key == "Номер") mark.ShNumber = pair.Value;
        //                    if (pair.Key == "Продукция") mark.Good = pair.Value;
        //                    if (pair.Key == "крепость") mark.Alcpercent = pair.Value;
        //                    if (pair.Key == "Емкость") mark.Capacity = pair.Value;
        //                    if (pair.Key == "Производитель") mark.Producer = pair.Value;
        //                    if (pair.Key == "alc_code") mark.AlcCode = pair.Value;
        //                }
        //            }

        //            return mark;
        //        }
        //        else 
        //        {
        //            return new Mark { success = markResponse.GetMarkInfoResult.Success, msg = markResponse.GetMarkInfoResult.Error };
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Mark { success = false, msg = ex.Message };
        //    }            
        //}

        public string CheckConnection()
        {
            string body = "";
            // {"speedResult":"OK"}
            try
            {
                Uri uri = new Uri("http://146.120.90.77:7076/speedtest");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 5000;
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
                            return test.speedResult == "OK" ? "Соединение с ФСРАР - есть" : "";
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

        public string CheckFormatPDF417(string barcode) 
        {
            var ind = barcode.IndexOf("N000");
            
	        if(ind == 2)
            {
		        return barcode;
	        }else
            {
                return barcode.Substring(ind-2) + barcode.Substring(0, ind-2);
            }
        }    
    }
}
