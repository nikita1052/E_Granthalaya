using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace E_Granthalaya
{
    public class Common
    {
        public static void WriteToLog(string methodName, string message)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "LogInfo\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = folderPath + "Log_" + currentDate + ".txt";
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Close();
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath, true, UnicodeEncoding.Default))
                {
                    writer.WriteLine($"{DateTime.Now} -> {methodName} -> {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}