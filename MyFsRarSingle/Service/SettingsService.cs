using System;
using System.IO;
using Aida.Model;
using CodeBetter.Json;
using Microsoft.VisualBasic;

namespace Aida.Service
{
    public class SettingsService
    {
        public static string GetAppPath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        public static FileSettings GetSettings()
        {
            var appPath = GetAppPath();

            if (!File.Exists(appPath + "\\settings.txt"))
            {
                return new FileSettings { success = false, msg = "File not found settings.txt" };
            }

            try
            {
                FileSettings settings;
                using (var streamReader = new StreamReader(appPath + "\\settings.txt", System.Text.Encoding.UTF8))
                {
                    var settingsText = streamReader.ReadToEnd();
                    settings = Converter.Deserialize<FileSettings>(settingsText);
                }

                //return new Settings { success = true, server_uri = settings.server_uri , IMEI = settings.IMEI };
                settings.success = true;

                return settings;
            }
            catch (Exception ex)
            {
                return new FileSettings { success = false, msg = ex.Message };
            }
            //return "http://192.168.1.254/HttpService1c/hs/";
        }

        public static Result SaveSettings(BaseSettings settings) 
        {
            var fileSettings = new FileSettings();

            fileSettings.setting_code = settings.code;
            fileSettings.server_uri = settings.server_uri;

            var strJson = Converter.Serialize(fileSettings);

            var appPath = GetAppPath();
            try
            {
                using (var streamWriter = new StreamWriter(appPath + "\\settings.txt", false))
                {
                    streamWriter.Write(strJson);
                }

                return new Result { success = true };
            }
            catch (Exception ex)
            {
                return new Result { success = false, msg = ex.Message };
            }            
        }

        

        //public static byte[] ReadFile(string filePath)
        //{
        //    byte[] buffer;
        //    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    try
        //    {
        //        int length = (int)fileStream.Length;  // get file length
        //        buffer = new byte[length];            // create buffer
        //        int count;                            // actual number of bytes read
        //        int sum = 0;                          // total number of bytes read

        //        // read until Read method returns 0 (end of the stream has been reached)
        //        while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
        //            sum += count;  // sum is a buffer offset for next reading
        //    }
        //    finally
        //    {
        //        fileStream.Close();
        //    }
        //    return buffer;
        //}
    }
}
