using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace E_Granthalaya
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string bookId = context.Request.QueryString["linkedwith"];
            // string imagePath = "D:/SIA_Granthalaya/GranthalayaBookImages/GRANTHALAYA/main/" + bookId + ".jpg";
            string basePath = ConfigurationManager.AppSettings["ImageUrl"];
            if (string.IsNullOrEmpty(basePath))
            {
                context.Response.StatusCode = 500; // Internal Server Error
                context.Response.Write("Configuration Error: ExternalDataFilePath is not set.");
                return;
            }

            string imagePath = Path.Combine(basePath, bookId + ".jpg");

            if (File.Exists(imagePath))
            {
                context.Response.ContentType = "image/jpeg"; // Set the appropriate content type
                context.Response.WriteFile(imagePath); // Write the image content to the response
            }
            else
            {
                context.Response.ContentType = "image/jpeg"; // Set the appropriate content type
                context.Response.WriteFile("D:/E-Granthalaya Work/E_Granthalaya/E_Granthalaya/assets/images/defaultbook.jpg");

                //    // Start searching from the previous bookId
                //    for (int prevBookId = currentBookId - 1; prevBookId > 0; prevBookId--)
                //    {
                //        previousImagePath = "D:/SIA_Granthalaya/GranthalayaBookImages/GRANTHALAYA/main/" + prevBookId + ".jpg";

                //        if (File.Exists(previousImagePath))
                //        {
                //            // If image for the previous bookId exists, return it
                //            context.Response.ContentType = "image/jpeg";
                //            context.Response.WriteFile(previousImagePath);
                //            return; // Exit the loop and method once an existing image is found
                //        }
                //    }
            }

            context.Response.StatusCode = 404; // Return a 404 status code if the image doesn't exist
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