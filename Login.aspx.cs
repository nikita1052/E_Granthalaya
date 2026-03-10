using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Drawing;
using System.Globalization;

namespace E_Granthalaya
{
    public partial class Login : System.Web.UI.Page
    {
        #region Global variable declaration
        static string OTP;
        int r;
        int y = 0;
        int dept_id = 0;
        string designation;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CountBooks();
        }

        /// <summary>
        /// Function to connect to Database
        /// </summary>
        /// <returns></returns>
        protected SqlConnection getConnection()
        {
            string conStr = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = conStr;
            return con;
        }

        /// <summary>
        /// function to send OTP notification on registered email address
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="name">name of the user</param>
        void sendOTP(string email, string name)
        {
            string functionName = "sendOTP()";
            try
            {
                Random random = new Random();
                //generating random otp number
                string capitalizedName = CapitalizeName(name);
                OTP = random.Next(1001, 9999).ToString();
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
                msg.Subject = "OTP to Login - SIA EGranthalaya v.1.0";
                string msg1 = "<b>Hello " + capitalizedName + "</b><br>";
                msg1 += "<b>Your Login-OTP is \"" + OTP + "\"</b><br><br>";
                msg1 += "<b>Thanks and Regards,<br>The SIA College of Higher Education</b>";
                msg.Body = msg1;
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                lbl_warning.Text = functionName + ": Some error occured -> " + ex.Message;
            }
        }

        private static string CapitalizeName(string name)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.ToLower());
        }

        public int getAcademicYear()
        {
            SqlCommand cmd = null;
            SqlConnection c = getConnection();
            DateTime fdt = DateTime.Today;
            int mn = fdt.Month;
            int yy = fdt.Year;
            c.Open();

            int l = txt_user.Text.Length;

            cmd = new SqlCommand("SELECT CHARINDEX('@', '" + txt_user.Text + "') AS MatchPosition", c);
            int p = Convert.ToInt32(cmd.ExecuteScalar());

            string dept = txt_user.Text.Substring(p);

            cmd = new SqlCommand("Select Dept_Id from Department_Master where Dept_Code='" + dept + "'", c);
            dept_id = Convert.ToInt32(cmd.ExecuteScalar());

            //string ay = "";
            if (dept_id == 1 || dept_id == 2)
            {
                if (mn <= 5)
                {
                    yy = yy - 1;

                    //ay = Convert.ToString(yy) + "-" + Convert.ToString(yy1);
                }
            }
            if (dept_id == 3)
            {

                if (mn < 9)
                {
                    yy = yy - 1;

                    //ay = Convert.ToString(yy) + "-" + Convert.ToString(yy1);
                }
            }
            return yy;
        }

        private string GetCurrentUserJobTitle()
        {
            string loginid = Session["loginid"]?.ToString();
            string designation = "Unknown";
            try
            {
                if (loginid != "Administrator")
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@LOGINID", loginid)
                    };
                    DataSet ds = DBContext.GetDataSet("USP_GET_LOGGEDIN_USER_DETAILS", param, CommandType.StoredProcedure);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);
                        if (flag == 0)
                        {
                            Response.Redirect("Login.aspx");
                        }
                        else if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            designation = Convert.ToString(ds.Tables[1].Rows[0]["JOB_TITLE"]);
                        }
                    }
                }
                else
                {
                    designation = "Administrator";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("AdminMaster.master -> GetCurrentUserJobTitle()", ex.Message);
                ShowToastr(this.Page, "Some error occurred -> " + ex.Message, "Error", "error");
            }
            return designation;
        }
        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
        /// <summary>
        /// function to be called on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_send_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                r = 0;
                y = getAcademicYear();
                string loginId = string.Empty;
                string dob = string.Empty;
                SqlConnection c = getConnection();
                //try
                //{
                loginId = txt_user.Text.ToString();
                //dob = txt_dob.Text;
                int number;
                bool result = Int32.TryParse(txt_user.Text, out number);
                result = true;
                c.Open();
                SqlCommand cmd;
                //if login using formno
                if (r == 0 && result == true)
                {
                    cmd = new SqlCommand("select a.Roll_No,a.FirstName,a.LastName,a.GRNO,c.Dept_ID,a.Roll_No+'@'+c.Dept_Code as login_id ,a.email from students_info a join Course_Master b on a.Course_Id=b.Course_Id join Department_Master c on b.Dept_id=c.Dept_ID where a.Roll_No is not null and a.Roll_No!='' and a.Roll_No+'@'+c.Dept_Code='" + loginId + "'and a.year_id=" + y, c);
                    SqlDataReader dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                    {
                        r = 1;
                        string email = "", name = "";
                        name = dr2["FirstName"].ToString() + " " + dr2["LastName"].ToString();
                        string roll = dr2["Roll_No"].ToString();
                        int grno = Convert.ToInt16(dr2["GRNO"]);
                        int dept_id = Convert.ToInt16(dr2["Dept_Id"]);
                        email = dr2["Email"].ToString();

                        sendOTP(email, name);
                        Session["loginid"] = loginId;
                        Session["otp"] = OTP;
                        Response.Redirect("VerifyOTP.aspx?r=" + r); dr2.Close();
                    }
                    else
                    {
                        dr2.Close();
                        //checking user in Faculty Master
                        int fid = 0;
                        //cmd = new SqlCommand("select faculty_id from Faculty_Master where Faculty_Id in (select Faculty_ID from Faculty_Appointment where Year_ID="+y+") and UserName ='" + loginId + "'", c);
                        cmd = new SqlCommand("select faculty_id from Faculty_Master where UserName ='" + loginId + "'", c);
                        fid = Convert.ToInt32(cmd.ExecuteScalar());
                        //getting faculty details using username if found in Faculty_Master
                        if (fid > 0)
                        {
                            cmd = new SqlCommand("select * from Faculty_Master where UserName ='" + loginId + "'", c);
                            SqlDataReader dr4 = cmd.ExecuteReader();
                            if (dr4.Read())
                            {
                                string email = "", name = "";
                                name = dr4["First_Name"].ToString() + " " + dr4["Last_Name"].ToString();
                                email = dr4["Email"].ToString();
                                designation = Convert.ToString(dr4["JOB_TITLE"]);
                                if (designation == "Librarian" )
                                {
                                    r = 3;
                                }
                                else if (designation == "Assistant Professor" || designation == "Office")
                                {
                                    r = 2;
                                }
                                else if (designation == "Clerk") 
                                {
                                    r = 5;
                                }
                                else if (designation == "Peon") 
                                {
                                    r = 4;
                                }
                                sendOTP(email, name);
                                Session["loginid"] = loginId;
                                Session["otp"] = OTP;
                                Response.Redirect("VerifyOTP.aspx?r=" + r);
                            }
                            else
                            {
                                panel_warning.Visible = true;
                                lbl_warning.Text = "Invalid Login ID...!!!";
                            }

                            dr4.Close();
                        }
                    }
                }
            }
            else
            {
                panel_warning.Visible = true;
                lbl_warning.Text = "All Fields are Required...!!!";
            }
        }

        protected void CountBooks()
        {
            SqlConnection con = getConnection();
            SqlCommand sql = new SqlCommand("SELECT COUNT(*) AS TOTAL, COUNT(DISTINCT LINKEDWITH) AS UNIQUEB FROM BOOK_MASTER", con);
            sql.CommandType = CommandType.Text;
            DataSet ds1 = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(sql);
            sda.Fill(ds1);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                lbl_books.Text = ds1.Tables[0].Rows[0]["Total"].ToString();
                lbl_unique.Text = ds1.Tables[0].Rows[0]["UNIQUEB"].ToString();
            }
            else
            {
                lbl_books.Text = "0";
                lbl_unique.Text = "0";
            }
        }
    }
}