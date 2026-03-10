using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace E_Granthalaya
{
    /// <summary>
    /// Summary description for ImageHandler1
    /// </summary>
    public class ImageHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string subNo = context.Request.QueryString["SUBS_IMAGE"];

            string basePath = ConfigurationManager.AppSettings["ImageUrl1"];
            if (string.IsNullOrEmpty(basePath))
            {
                context.Response.StatusCode = 500;
                context.Response.Write("Configuration Error: ExternalDataFilePath is not set.");
                return;
            }

            string imagePath = Path.Combine(basePath, subNo + ".jpg");

            if (File.Exists(imagePath))
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.WriteFile(imagePath);
            }
            else
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.WriteFile("D:/E-Granthalaya Work/E_Granthalaya/E_Granthalaya/assets/images/defaultbook.jpg");
            }
            context.Response.StatusCode = 404;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}