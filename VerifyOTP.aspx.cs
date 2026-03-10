using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class VerifyOTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //checking for logged in user
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                CountBooks();
            }
        }

        /// <summary>
        /// function to be called when button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_verify_Click(object sender, EventArgs e)
        {
            string functionName = "btn_verify_Click()";
            string OTP = string.Empty;
            int r1 = 0;
            string loginid = string.Empty;
            try
            {
                OTP = Session["otp"].ToString();
                r1 = Convert.ToInt16(Request.QueryString["r"]);
                loginid = Session["loginid"].ToString();

                if (txt_otp.Text == OTP)
                {
                    //for student
                    if (r1 == 1)
                    {
                        Response.Redirect("Stud_Dashboard.aspx");
                    }
                    //for faculty
                    else if (r1 == 2)
                    {
                        Response.Redirect("Faculty_Dashboard.aspx");
                    }
                    //for administrator
                    else if (r1 == 3)
                    {
                        Response.Redirect("Adm_Dashboard.aspx");
                    }
                    //for peon
                    else if (r1 == 4)
                    {
                        Response.Redirect("Adm_Dashboard.aspx");
                    }
                    //for clerk
                    else if (r1 == 5)
                    {
                        Response.Redirect("Adm_Dashboard.aspx");
                    }
                }
                else
                {
                    pnl_warning.Visible = true;
                    lbl_warning.Text = "Invalid OTP Entered...!!!";
                }
            }
            catch (Exception ex)
            {
                lbl_warning.Text = functionName + ": Some error occured -> " + ex.Message;
            }
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
        protected SqlConnection getConnection()
        {
            string conStr = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = conStr;
            return con;
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