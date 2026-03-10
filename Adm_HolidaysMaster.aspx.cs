using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Mail;
using System.Globalization;

namespace E_Granthalaya
{
    public partial class Adm_HolidaysMaster : System.Web.UI.Page
    {
        static int fid, dept_id;
        string name, designation, deptCode, deptName;
        DateTime UpDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               Session["loginid"] = "bharathi rao@ug";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian")
                {
                    GetLoggedInUserDetails();
                    Allsunday();
                    BindYear();
                    pnl_nodata.Visible = true;
                }
                else
                {
                    Response.Redirect("Unauthorized_Access.aspx");
                }
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


        void GetLoggedInUserDetails()
        {
            string loginid = Session["loginid"].ToString();
            try
            {
                if (loginid != "Administrator")
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@LOGINID", loginid)
                    };
                    DataSet ds = DBContext.GetDataSet("USP_GET_LOGGEDIN_USER_DETAILS", param, CommandType.StoredProcedure);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);
                            if (flag == 0)
                            {
                                Response.Redirect("Login.aspx");
                            }
                            else
                            {
                                name = Convert.ToString(ds.Tables[1].Rows[0]["NAME"]);
                                designation = Convert.ToString(ds.Tables[1].Rows[0]["JOB_TITLE"]);
                                deptCode = Convert.ToString(ds.Tables[1].Rows[0]["DEPT_CODE"]);
                                deptName = Convert.ToString(ds.Tables[1].Rows[0]["DEPT_NAME"]);
                                dept_id = Convert.ToInt32(ds.Tables[1].Rows[0]["DEPT_ID"]);
                                fid = Convert.ToInt32(ds.Tables[1].Rows[0]["ID"]);
                            }
                        }
                    }
                }
                else
                {
                    name = "Administrator";
                    designation = "Administrator";
                    fid = 0;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void Allsunday()
        {
            try
            {
                DateTime dt = DateTime.Now;
                if (dt.Month == 6 || dt.Month == 7)
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                      new SqlParameter("@YEAR_ID", GetCurrentYear()),
                      new SqlParameter("@HOLIDAY_FROM_DATE",GetCurrentYear().ToString() + "-06-01"),
                      new SqlParameter("@HOLIDAY_TO_DATE",  (GetCurrentYear()+1).ToString() + "-05-31"),
                      new SqlParameter("@REASON","Sunday"),
                      new SqlParameter("@FACULTY_Id", fid),
                      new SqlParameter("@HOLIDAY", 0),
                    };

                    DBContext.ExecuteNonQueryCmd("USP_ADDHOLIDAYRECORDS", sp, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> Allsunday() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        public int GetCurrentYear()
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            try
            {
                if (now.Month <= 5)
                {
                    year -= 1;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_Limit.aspx.cs -> GetCurrentYear()", ex.Message);
            }
            return year;
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                DateTime dateValue = Convert.ToDateTime(GridView1.Rows[e.RowIndex].Cells[1].Text);
                string query = "SELECT DISTINCT ISSUE_DATE FROM BOOK_REQUISITION WHERE STATUS = 6 AND RECORD_STATUS_ID = 1 AND ISSUE_DATE>@DATE";

                SqlParameter[] parameters =
                {
                   new SqlParameter("@DATE", dateValue),

                };

                DataSet dt = DBContext.GetDataSet(query, parameters, CommandType.Text);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    UpDate = Convert.ToDateTime(dt.Tables[0].Rows[0]["ISSUE_DATE"]);

                }
                List<string> issueEmails = GetEmailAddresses(UpDate, "issue");
                List<string> returnEmails = GetEmailAddresses(UpDate, "return");

                SqlParameter[] sp = new SqlParameter[]
                {
                   new SqlParameter("@DELETE_HOLIDAY", dateValue),
                   new SqlParameter("@FACULTY_ID", fid),
                };

                int ds = DBContext.ExecuteNonQueryCmd("USP_DELETE_HOLIDAY_RECORDS", sp, CommandType.StoredProcedure);

                if (ds < 1)
                {
                    ShowToastr(this.Page, "Holiday deleted successfully...!", "Success", "success");
                    SendEmailForDateUpdate(Convert.ToDateTime(dateValue), issueEmails, "issue", false);
                    SendEmailForDateUpdate(Convert.ToDateTime(dateValue), returnEmails, "return", false);
                    GridView1.EditIndex = -1;
                    BindGridview();
                }
                else
                {
                    ShowToastr(this.Page, "Something went wrong! Try after sometime.", "Error", "error");
                    BindGridview();
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> GridView1_RowDeleting() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridview();
        }

        private void BindYear()
        {
            try
            {
                string sql;
                DateTime dt = DateTime.Now;
                if (dt.Month <= 5)
                {
                    sql = "SELECT * FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=(YEAR(GETDATE())-1)";
                }
                else
                {
                    sql = "SELECT * FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=YEAR(GETDATE())";
                }

                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);
                ddl_year.DataSource = ds;
                ddl_year.DataTextField = "YEAR_DESC";
                ddl_year.DataValueField = "YEAR_ID";
                ddl_year.DataBind();
                ListItem liItem = new ListItem("--Select Year--", "0");
                ddl_year.Items.Insert(0, liItem);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> BindYear() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridview();
        }

        protected void BindGridview()
        {
            try
            {
                int selectedYearId = Convert.ToInt32(ddl_year.SelectedItem.Value);
                if (selectedYearId == 0)
                {
                    GridView1.DataSource = null;
                    panel_content.Visible = false;
                    ShowToastr(this.Page, "Please select valid year...!", "Error", "error");
                    pnl_nodata.Visible = true;
                }
                else
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                      new SqlParameter("@YEAR_ID", selectedYearId),
                    };

                    DataSet ds = DBContext.GetDataSet("USP_GETHOLIDAY_ALLRECORDS", sp, CommandType.StoredProcedure);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        panel_content.Visible = true;
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                        pnl_nodata.Visible = false;
                    }
                    else
                    {
                        panel_content.Visible = false;
                        pnl_nodata.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> BindGridview() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured -> " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> issueEmails = GetEmailAddresses(Convert.ToDateTime(txt_fromDate.Text), "issue");
                List<string> returnEmails = GetEmailAddresses(Convert.ToDateTime(txt_fromDate.Text), "return");

                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@YEAR_ID", ddl_year.SelectedItem.Value),
                    new SqlParameter("@HOLIDAY_FROM_DATE", Convert.ToDateTime(txt_fromDate.Text)),
                    new SqlParameter("@HOLIDAY_TO_DATE",  Convert.ToDateTime(txt_toDate.Text)),
                    new SqlParameter("@REASON", txt_reason.Text),
                    new SqlParameter("@FACULTY_ID", fid),
                    new SqlParameter("@HOLIDAY", 1),
                };

                int ds = DBContext.ExecuteNonQueryCmd("USP_ADDHOLIDAYRECORDS", sp, CommandType.StoredProcedure);
                if (ds >= 1)
                {
                    ShowToastr(this.Page, "Details Added Successfully...!", "Success", "success");
                    BindGridview();

                    SendEmailForDateUpdate(Convert.ToDateTime(txt_fromDate.Text), issueEmails, "issue", true);
                    SendEmailForDateUpdate(Convert.ToDateTime(txt_fromDate.Text), returnEmails, "return", true);
                    cleartxtcontrol();

                }
                else
                {
                    ShowToastr(this.Page, "Holiday Already Declared...!", "Error", "error");
                    cleartxtcontrol();

                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> btn_save_Click() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void cleartxtcontrol()
        {
            txt_fromDate.Text = txt_reason.Text = txt_toDate.Text = string.Empty;
            ddl_year.Focus();
        }

        public static List<string> GetEmailAddresses(DateTime date, string dateType)
        {
            List<string> emailAddresses = new List<string>();

            try
            {
                string dateColumn;
                if (dateType == "issue")
                {
                    dateColumn = "ISSUE_DATE";
                }
                else if (dateType == "return")
                {
                    dateColumn = "DUE_DATE";
                }
                else
                {
                    throw new ArgumentException("Invalid date type specified");
                }

                string query = $"SELECT DISTINCT SI.EMAIL FROM [DBO].[STUDENTS_INFO] SI " +
                               "JOIN BOOK_REQUISITION BR ON SI.STUDENT_ID = BR.SID " +
                               $"WHERE  BR.{dateColumn} =@Date";

                SqlParameter[] parameters = new SqlParameter[]
                {
                  new SqlParameter("@Date", date)
                };

                DataSet ds = DBContext.GetDataSet(query, parameters, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["EMAIL"] != DBNull.Value)
                        {
                            emailAddresses.Add(row["EMAIL"].ToString());
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs ->GetEmailAddresses -> SQL Exception", sqlEx.Message);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_HolidaysMaster.aspx.cs ->GetEmailAddresses -> Exception", ex.Message);
            }

            return emailAddresses;
        }


        public static void SendEmailForDateUpdate(DateTime datevalue, List<string> emailAddresses, string dateType, bool isHolidayDeclared)
        {
            foreach (string email in emailAddresses)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    continue;
                }

                try
                {
                    string dateColumn;
                    string subject;
                    int status;
                    if (dateType == "issue")
                    {
                        dateColumn = "ISSUE_DATE";
                        status = 6;
                        subject = "Updated Issue Date";
                    }
                    else if (dateType == "return")
                    {
                        dateColumn = "DUE_DATE";
                        status = 7;
                        subject = "Updated Return Date";
                    }
                    else
                    {
                        throw new ArgumentException("Invalid date type specified");
                    }

                    string query = $"SELECT DISTINCT BR.{dateColumn}, SI.FIRSTNAME " +
                                   "FROM [DBO].[STUDENTS_INFO] SI " +
                                   "JOIN BOOK_REQUISITION BR ON SI.STUDENT_ID = BR.SID " +
                                   "WHERE SI.EMAIL = @EMAIL " +
                                   "AND BR.STATUS=@STATUS AND BR.MODIFIED_BY = @FID";

                    SqlParameter[] parameters =
                    {
                           new SqlParameter("@EMAIL", email),
                           new SqlParameter("@STATUS", status),
                           new SqlParameter("@FID", fid),
                    };

                    DataSet ds = DBContext.GetDataSet(query, parameters, CommandType.Text);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DateTime date = Convert.ToDateTime(ds.Tables[0].Rows[0][dateColumn]);
                        string firstName = CapitalizeName(Convert.ToString(ds.Tables[0].Rows[0]["FIRSTNAME"]));

                        string emailBody = $"<b>Hello {firstName},</b><br>" +
                                           $"<b>{datevalue.ToString("dd-MM-yyy")} has been {(isHolidayDeclared ? "declared as a holiday" : "cancelled as a holiday")} and your {dateType} date is updated to {date.ToString("dd-MM-yyy")}.<br><br></b>" +
                                           "<b>Thanks,<br>The SIA College of Higher Education</b>";

                        DBContext.SendEmailNotification(email, emailBody, subject);
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_HolidaysMaster.aspx.cs -> SendEmailForDateUpdate", ex.Message);
                }
            }
        }

        private static string CapitalizeName(string name)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.ToLower());
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}