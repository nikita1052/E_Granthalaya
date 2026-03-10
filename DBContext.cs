using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace E_Granthalaya
{
    public class DBContext
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString);
            return con;
        }

        public static void SendEmailNotification(string email, string emailBody, string Subject)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                string senderEmail = "esanchalan@thesiacollege.com";
                string senderDisplayName = "SIA E-Granthalaya";
                MailAddress fromAddress = new MailAddress(senderEmail, senderDisplayName);
                MailMessage msg = new MailMessage();
                msg.From = fromAddress;
                msg.To.Add(email);
                client.Credentials = new System.Net.NetworkCredential("esanchalan@thesiacollege.com", "$ia@Es#123");
                msg.Subject = Subject;
                msg.Body = emailBody;
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> SendEmailNotification()", ex.Message);
            }
        }


        public static DataSet GetDataSet(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            DataSet ds = new DataSet();
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> GetDataSet()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return ds;
        }

        public static SqlDataReader GetDataReader(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            SqlDataReader dr = null;
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> GetDataSet()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return dr;
        }

        public static string ExecuteScalarCmd(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            string value = string.Empty;
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }
                value = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> ExecuteScalarCmd()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return value;
        }

        public static int ExecuteNonQueryCmd(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            int affectedRow = 0;
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }

                affectedRow = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> ExecuteNonQueryCmd()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return affectedRow;
        }

        public static int GetCount(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            int existCount = 0;
            //  checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }

                existCount = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> GetDataSet()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return existCount;
        }

        public static DateTime GetDate(string query, SqlParameter[] param, CommandType cmdType)
        {
            SqlConnection c = GetConnection();
            DateTime existCount = DateTime.Now;
            //  checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);
            try
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(query, c);
                cmd.CommandType = cmdType;
                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value == null ? 0 : p.Value);
                    }
                }

                existCount = Convert.ToDateTime(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                Common.WriteToLog("DBContext.cs -> GetDataSet()", ex.Message);
            }
            finally
            {
                c.Close();
            }
            return existCount;
        }
    }
}