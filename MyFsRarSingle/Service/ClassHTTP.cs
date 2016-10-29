//using System;
//using System.Text;
//using System.Net;
//using System.IO;
//using System.Security.Cryptography;
//using System.Security.Cryptography.X509Certificates;
//using System.Web;


//namespace Mark
//{
//    public class Response 
//    {
//        public bool Success { get; set; }
//        public string Msg { get; set; }
//    }

//    public class MarkStatus
//    {
//        public int Key { get; set; }
//        public string Value { get; set; }
//    }

//    public class GoodResult
//    {
//        public int Id { get; set; }
//        public string Production { get; set; }
//    }

//    public class Mark
//    {
//        public int Id { get; set; }
//        public string Ser { get; set; }
//        public MarkStatus Id_Markstatus { get; set; }
//        public string Alcperc { get; set; }
//        public string Capacity { get; set; }
//        public string Country { get; set; }
//        public string Mtype { get; set; }
//        public string Organization { get; set; }
//        public string Prodkind { get; set; }
//        public GoodResult Result { get; set; }
//        public string ShSeries { get; set; }
//        public string ShNumber { get; set; }
//    }

    
//    public class MarksArray
//    {
//        public bool Success { get; set; }        
//        public Mark[] Marks { get; set; }
//    }

//    public class ResponceResult
//    {
//        public bool Success { get; set; }
//        public string Status { get; set; }
//        public string Headers { get; set; }
//        public string ResultText { get; set; }
//    }

//    public class MarksFsrar : ResponceResult
//    {
//        public MarksArray Marks { get; set; }
//    }

//    public class MarkId : ResponceResult
//    {
//        public int Id { get; set; }
//    }

//    public class FsRarRoom
//    {
//        private string FsRarServiceCoockie { get; set; }
//        private string Csrf { get; set; }

//        private class TrustAllCertificatePolicy : ICertificatePolicy
//        {
//            public TrustAllCertificatePolicy() { }

//            public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
//            {
//                return true;
//            }
//        }

//        private Response GetRequest()
//        {
//            FsRarServiceCoockie = "";

//            try
//            {
//                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

//                Uri uri = new Uri("https://service.fsrar.ru/auth/login");
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
//                request.KeepAlive = true;
//                request.PreAuthenticate = true;
//                request.AllowWriteStreamBuffering = false;
//                request.Referer = "https://service.fsrar.ru/";
//                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
//                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
//                request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
//                request.Headers.Add("CacheControl", "max-age=0");
//                request.Headers.Add("Cookie", "f5_cspm=1234;");
//                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.94 Safari/537.36";
//                request.Headers.Add("Upgrade-Insecure-Requests", "1");

//                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//                using (Stream stream = response.GetResponseStream())
//                    if (stream != null)
//                        using (StreamReader reader = new StreamReader(stream))
//                        {
//                            string html = reader.ReadToEnd();

//                            var setCoockieStr = response.Headers["Set-Cookie"];
                            
//                            var lastInd = setCoockieStr.LastIndexOf("SERVICE_FSRAR=");

//                            lastInd = lastInd + "SERVICE_FSRAR=".Length;
//                            FsRarServiceCoockie = "SERVICE_FSRAR=" + setCoockieStr.Substring(lastInd, setCoockieStr.IndexOf(";", lastInd) - lastInd);

//                            if (response.StatusCode != HttpStatusCode.OK)
//                            {
//                                return new Response { Success = false };
//                            }

//                            int indStart;
//                            indStart = html.IndexOf("name: 'csrf'");

//                            if (indStart > -1)
//                            {
//                                string val = "value: '";
//                                int startValInd = html.IndexOf(val, indStart);
//                                int endValInd = html.IndexOf("'", startValInd + val.Length);
//                                startValInd = startValInd + val.Length;
//                                string csrf = html.Substring(startValInd, endValInd - startValInd);

//                                Csrf = csrf;
//                            }
//                        }

//                return new Response { Success = true };
//            }
//            catch (Exception ex)
//            {                
//                return new Response { Success = false, Msg = ex.Message };
//            }

//        }

//        private string PasswordMD5(string input)
//        {
//            byte[] hash = Encoding.ASCII.GetBytes(input);
//            MD5 md5 = new MD5CryptoServiceProvider();
//            byte[] hashenc = md5.ComputeHash(hash);
//            string result = "";
//            foreach (var b in hashenc)
//            {
//                result += b.ToString("x2");
//            }
//            return result;
//        }

//        private Response PostRequest()
//        {
//            string strReturn = null;
//            try
//            {
//                string Inn = "6154103293";
//                string Password = "PJSjKH";
//                //Encoding the post vars
//                var data = "inn=" + Inn + "&csrf=" + Uri.EscapeDataString(Csrf) + "&password=" + PasswordMD5(Password) + "&user_type=user";

//                byte[] buffer = Encoding.ASCII.GetBytes(data);
//                //Initialisation with provided url
//                Uri uri = new Uri("https://service.fsrar.ru/auth/login");
//                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(uri);

//                //Set method to post, otherwise postvars will not be used
//                WebReq.AllowWriteStreamBuffering = true;
//                WebReq.Method = "POST";
//                WebReq.KeepAlive = true;
//                WebReq.AllowAutoRedirect = true;
//                WebReq.ContentType = "application/x-www-form-urlencoded";
//                WebReq.ContentLength = buffer.Length;
//                WebReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
//                WebReq.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
//                WebReq.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
//                WebReq.Headers.Add("CacheControl", "max-age=0");
//                WebReq.Headers.Add("Cookie", "f5_cspm=1234; " + FsRarServiceCoockie + ";");
                
//                WebReq.Headers.Add("Origin", "https://service.fsrar.ru");
//                WebReq.Referer = "https://service.fsrar.ru/auth/login";


//                Stream PostData = WebReq.GetRequestStream();
//                PostData.Write(buffer, 0, buffer.Length);
//                PostData.Close();

//                //Get the response handle, we have no true response yet
//                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

//                //information about the response
//                var status = WebResp.StatusCode;

//                if (status != HttpStatusCode.OK)
//                {
//                    return new Response { Success = false }; 
//                    //labelLocation.Text = WebResp.ResponseUri.AbsoluteUri;
//                }

//                //if (WebResp.StatusCode == HttpStatusCode.Redirect)
//                //{
//                //    string location = WebResp.Headers["Location"];
//                //    Location.Content = location;

//                //    //return true;
//                //}

//                //StatusBox.Text = status.ToString();

//                //ListBoxHeaders.Items.Clear();

//                //for (int i = 0; i < WebResp.Headers.Count - 1; i++)
//                //{
//                //    ListBoxHeaders.Items.Add(WebResp.Headers.Keys[i] + " : " + WebResp.Headers[i]);
//                //}

//                //read the response
//                Stream WebResponse = WebResp.GetResponseStream();
//                StreamReader _response = new StreamReader(WebResponse);
//                strReturn = _response.ReadToEnd();

//                //textBox1.Text = strReturn;
//                return new Response { Success = strReturn.Contains("Новости") && strReturn.Contains("Сообщения") };

//            }
//            catch (Exception ex)
//            {
//                return new Response { Success = false, Msg = ex.Message };
//            }
//        }

//        public Response ConnectToMyRoom() 
//        {
//            var resGet = GetRequest();
//            if (resGet.Success)
//            {
//                var resPost = PostRequest();
//                if(resPost.Success)
//                {
//                    return new Response{ Success = true, Msg = "Соединились" };
//                }
//            }
//            else 
//            {
//                return new Response { Success = resGet.Success, Msg = resGet.Msg };
//            };

//            return new Response{ Success = false, Msg = "Не соединились. Неизвестная ошибка"};
//        }

//        private static string GetUnixTime_ms()
//        {
//            long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
//            return unixTime.ToString();
//        }

//        public MarksFsrar GetMarks()
//        {
//            var resultMarks = new MarksFsrar();
//            // first post. get marks list
                        
//            if (String.IsNullOrEmpty(FsRarServiceCoockie))
//            {
//                return new MarksFsrar { Success = false, ResultText = "no coockie"};
//            }

//            try
//            {
//                //Encoding the post vars
//                var data = "page=1&start=0&limit=1000&sort=[{\"property\":\"cd\",\"direction\":\"DESC\"}]";

//                byte[] buffer = Encoding.ASCII.GetBytes(data);
//                //Initialisation with provided url
//                Uri uri = new Uri("https://service.fsrar.ru/checkmarks/getmarks?_dc=" + GetUnixTime_ms());
//                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(uri);

//                //Set method to post, otherwise postvars will not be used
//                webReq.Method = "POST";
//                webReq.AllowAutoRedirect = true;

//                // init HEADERS
//                // client
//                webReq.Accept = "*/*";
//                webReq.Headers.Add("Accept-Encoding", "gzip, deflate");
//                webReq.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
//                webReq.Headers.Add("X-Requested-With", "XMLHttpRequest");
//                // coockie
//                webReq.Headers.Add("Cookie", "f5_cspm=1234; " + FsRarServiceCoockie + ";");
//                // entity
//                webReq.ContentLength = buffer.Length;
//                webReq.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
//                // miscellaneous
//                webReq.Referer = "https://service.fsrar.ru/cabinet/home";
//                // security
//                webReq.Headers.Add("Origin", "https://service.fsrar.ru");

//                //string sdf="";

//                //for (int i=0;i < webReq.Headers.Count;i++)
//                //{
//                //    sdf += webReq.Headers.AllKeys[i]+":"+webReq.Headers[i]+Environment.NewLine;
//                //}

//                //return new MarksFsrar { Success = true, ResultText = sdf };

//                // post data
//                Stream postData = webReq.GetRequestStream();
//                postData.Write(buffer, 0, buffer.Length);
//                postData.Close();

                

//                //Get the response handle, we have no true response yet
//                HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();

//                //information about the response
//                resultMarks.Status = webResp.StatusCode.ToString();

                

//                //ListBoxHeaders.Items.Clear();
//                for (int i = 0; i < webResp.Headers.Count - 1; i++)
//                {
//                    resultMarks.Headers += webResp.Headers.Keys[i] + " : " + webResp.Headers[i] + Environment.NewLine;
//                }

//                //read the response
//                Stream webResponse = webResp.GetResponseStream();
//                if (webResponse == null)
//                {
//                    resultMarks.ResultText = "error!";
//                    return resultMarks;
//                }

//                StreamReader response = new StreamReader(webResponse);
//                var strReturn = response.ReadToEnd();

//                resultMarks.ResultText = strReturn;
                
//                //strReturn = strReturn.Replace(".Descr", "Descr");
                
//                //MarksArray marksResponse = JsonConvert.DeserializeObject<MarksArray>(strReturn);
//                //MarksArray marksResponse = Converter.Deserialize<MarksArray>(strReturn);
//                //MarksArray marksResponse = null;
//                //var marksResponse = new MarksArray { Success = true}; 
//                //return new MarksFsrar { Success = true, ResultText = marksResponse.success.ToString()};

//                //if (marksResponse.success)
//                //{                    
//                //    resultMarks.Marks = marksResponse;

//                //    return resultMarks;
//                //}

//                return null;
//            }
//            catch (Exception ex)
//            {                
//                return new MarksFsrar { Success = false, ResultText = ex.Message };
//            }
//        } // GetMarks()

//        public MarkId IsMarkExists(MarksArray allMarks, string ser)
//        {
//            if (allMarks.Marks.Length == 0)
//            {
//                return new MarkId { Success = false, Id = 0 };
//            }

//            Mark markFirst = null;

//            foreach (var mark in allMarks.Marks)
//            {
//                if(mark.Ser == ser)
//                {
//                    markFirst = mark;
//                    break;
//                }
//            }
            

//            if (markFirst != null)
//            {
//                return new MarkId
//                {
//                    Success = true,
//                    Id = markFirst.Id
//                };
//            }

//            return new MarkId { Success = false };
//        }// IsMarkExists
//    }


//}
