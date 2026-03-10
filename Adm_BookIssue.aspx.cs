using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class Adm_BookIssue : System.Web.UI.Page
    {
        static int fid, dept_id, i;
        string name, designation, deptCode, deptName;
        string deptcode, UserRole;
        DateTime dt = DateTime.Now;
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
                else if (jobTitle == "Librarian" || jobTitle == "Clerk" || jobTitle == "Peon")
                {
                    GetLoggedInUserDetails();
                    GetTimeSlot();
                    GetAllSundays();
                    StudentsIssueData();
                    TeachersIssueData();
                    AutoCancel();
                    CodeExpiry();
                    txt_date.Text = dt.ToString("dd-MM-yyy");
                    btn_student.BackColor = Color.Green;
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
                Common.WriteToLog("Adm_BookIssue.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void StudentsIssueData()
        {
            try
            {
                int slot = Convert.ToInt32(ddl_slot.SelectedValue);
                SqlParameter[] sp = new SqlParameter[]
                {
                  new SqlParameter("@slot", slot),
                };
                DataSet ds = DBContext.GetDataSet("USP_GETISSUE_DATA_STUDENTS", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    pnl_student.Visible = true;
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                    pnl_student.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Entries Found to Issue - Students";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        private void TeachersIssueData()
        {
            try
            {
                int slot = Convert.ToInt32(ddl_slot.SelectedValue);
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GETISSUE_DATA_TEACHERS", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    GridView3.DataSource = ds;
                    GridView3.DataBind();
                    pnl_teacher.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_student.Visible = false;
                }
                else
                {
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                    pnl_student.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Entries Found to Issue - Teachers";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        private void GetAllSundays()
        {
            SqlConnection con = DBContext.GetConnection();
            try
            {
                DateTime dt = DateTime.Now;
                if (dt.Month == 4 || dt.Month == 7)
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                     new SqlParameter("@year_id", 2024),
                     new SqlParameter("@holiday_from_date","2024-04-01"),
                     new SqlParameter("@holiday_to_date",  "2025-05-31"),
                     new SqlParameter("@reason","Sunday"),
                     new SqlParameter("@Faculty_Id", fid),
                     new SqlParameter("@holiday", 0),
                    };
                    DBContext.ExecuteNonQueryCmd("USP_ADDHOLIDAYRECORDS", sp, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime CalculateReturnDate()
        {
            try
            {
                int daylimit;
                string sql = "SELECT NO_OF_DAYS FROM BOOK_LIMIT WHERE YEAR_ID=@YEAR";
                SqlParameter[] sp = new SqlParameter[]
                  {
              new SqlParameter("@YEAR", GetCurrentYear()),
                  };

                daylimit = DBContext.GetCount(sql, sp, CommandType.Text);

                DateTime due_date = dt.Date.AddDays(daylimit);
                if (dt.DayOfWeek == DayOfWeek.Sunday || IsHoliday(due_date))
                {
                    due_date = FindWorkingDay(due_date);
                }
                return due_date;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> CalculateReturnDate()", ex.Message);
                throw; // Rethrow the exception to propagate it further if needed
            }
        }

        public Boolean IsHoliday(DateTime Date)
        {
            string sql = "SELECT COUNT(*) FROM HOLIDAY_MASTER WHERE H_DATE=@DATE";
            SqlParameter[] sp = { new SqlParameter("@date", Date) };
            int count = DBContext.GetCount(sql, sp, CommandType.Text);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DateTime FindWorkingDay(DateTime due_Date)
        {
            while (IsHoliday(due_Date))
            {
                due_Date = due_Date.AddDays(1);
            }
            return due_Date;
        }
        protected void ddl_slot_SelectedIndexChanged(object sender, EventArgs e)
        {
            StudentsIssueData();
        }

        //============================== DATA BIND =============================================
        private void GetTimeSlot()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GET_TIMESLOT_ISSUE", sp, CommandType.StoredProcedure);
                ddl_slot.DataSource = ds;
                ddl_slot.DataTextField = "slot_range";
                ddl_slot.DataValueField = "slot_id";
                ddl_slot.DataBind();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> GetTimeSlot()", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        public int GetCurrentYear()
        {
            int ds = 0;
            SqlConnection con = DBContext.GetConnection();
            try
            {
                string sql;
                DateTime dt = DateTime.Now;
                if (dt.Month <= 5)
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=(YEAR(GETDATE())-1)";
                }
                else
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=YEAR(GETDATE())";
                }
                SqlParameter[] sp = null;
                ds = DBContext.GetCount(sql, sp, CommandType.Text);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> GetCurrentYear()", ex.Message);
            }
            return ds;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Issue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView1.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView1.Rows[rowindex].Cells[4].Text);
                    DateTime due_date = CalculateReturnDate();
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@ret",due_date),
                      new SqlParameter("@flag",0)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_ISSUEBOOK", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Issued Successfully", "Success", "Success");
                        StudentsIssueData();
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Issue_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    StudentsIssueData();
                }
            }
            else if (e.CommandName == "CancelIssue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView1.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView1.Rows[rowindex].Cells[4].Text);
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@FLAG",0)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_CANCELISSUE", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Cancelled Successfully", "Success", "Success");
                        GridView1.EditIndex = -1;
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Cancel_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    StudentsIssueData();
                }
            }
        }

        protected void btn_code_Click(object sender, EventArgs e)
        {
            string rollno = txt_id.Text;
            try
            {
                if (!string.IsNullOrEmpty(rollno))
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                    new SqlParameter("@ROLLNO", rollno),
                    };
                    int check = DBContext.GetCount("SELECT COUNT(*) FROM STUDENTS_MASTER WHERE ROLL_NO=@ROLLNO", sp, CommandType.Text);
                    if (check > 0)
                    {
                        int count = DBContext.GetCount("SELECT COUNT(*) FROM BOOK_REQUISITION BR JOIN STUDENTS_INFO S ON S.STUDENT_ID=BR.SID WHERE ROLL_NO=@ROLLNO AND (BR.STATUS=6 OR BR.STATUS=7)", sp, CommandType.Text);
                        if (count > 0)
                        {
                            pnl_code.Visible = false;
                            ShowToastr(this.Page, "Please ask them to return their previous books", "", "info");
                        }
                        else
                        {
                            Random random = new Random();
                            string OTP = random.Next(100001, 999999).ToString();
                            SqlParameter[] sp1 = new SqlParameter[]
                            {
                            new SqlParameter("@ROLLNO", rollno),
                                new SqlParameter("@OTP", OTP),
                            new SqlParameter("@REQDATE", DateTime.Now),
                            new SqlParameter("@FID", fid),
                            };
                            DBContext.ExecuteScalarCmd("USP_GETINSTANTCODE", sp1, CommandType.StoredProcedure);
                            pnl_code.Visible = true;
                            txt_code.Text = OTP;
                            txt_id.ReadOnly = true;
                            txt_code.ReadOnly = true;
                            btn_code.Text = "Regenerate";
                            ShowToastr(this.Page, "Code Generated Successfully, Valid for 5 minutes only..", "Success", "Success");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "TimerCount", "timecount();", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalScript", "showPanel('" + pnl_code.ClientID + "');", true);
                        }
                    }
                    else
                    {
                        pnl_code.Visible = false;
                        ShowToastr(this.Page, "Entered Roll No. is Invalid", "", "Info");
                    }
                }
                else
                {
                    pnl_code.Visible = false;
                    ShowToastr(this.Page, "Kindly Fill the Roll No", "", "info");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_code_Click()", ex.Message);
                ShowToastr(this.Page, ex.Message, "Error", "error");
            }
        }
        public void CodeExpiry()
        {
            try
            {
                string sql = "UPDATE INSTANTREQ_CODES SET RECORD_STATUS_ID = 2 WHERE RECORD_STATUS_ID = 1 AND MODIFIED_ON < DATEADD(MINUTE, -5, GETDATE())";
                SqlParameter[] sp = null;
                DBContext.ExecuteNonQueryCmd(sql, sp, CommandType.Text);
                Common.WriteToLog("CodeExpiry", "Expired Codes deleted successfully.");
            }
            catch (Exception ex)
            {
                Common.WriteToLog("CodeExpiry", ex.Message);
            }
        }

        protected void btn_manual_Click(object sender, EventArgs e)
        {
            btn_stud.BackColor = Color.Green;
            btn_fac.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup1();", true);
            pnl_stud.Visible = true;
            pnl_fac.Visible = false;
        }

        protected void btn_new_Click(object sender, EventArgs e)
        {
            clear();
            txt_id.ReadOnly = false;
            btn_code.Text = "Generate Code";
            pnl_code.Visible = false;
        }

        private void clear()
        {
            txt_id.Text = string.Empty;
            txt_code.Text = string.Empty;
            txt_bid.Text = string.Empty;
            txt_bkid.Text = string.Empty;
            txt_roll.Text = string.Empty;
            ddl_fid.SelectedIndex = -1;
        }

        protected void btn_student_Click(object sender, EventArgs e)
        {
            btn_student.BackColor = Color.Green;
            StudentsIssueData();
            pnl_teacher.Visible = false;
            pnl_instant.Visible = false;
            pnl_manually.Visible = false;
            btn_teacher.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_instant.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_manually.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }
        protected void btn_manually_Click(object sender, EventArgs e)
        {
            btn_manually.BackColor = Color.Green;
            ManualIssueData();
            pnl_teacher.Visible = false;
            pnl_instant.Visible = false;
            pnl_student.Visible = false;
            btn_teacher.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_instant.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_student.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }

        protected void btn_teacher_Click(object sender, EventArgs e)
        {
            btn_teacher.BackColor = Color.Green;
            TeachersIssueData();
            pnl_student.Visible = false;
            pnl_instant.Visible = false;
            pnl_manually.Visible = false;
            btn_student.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_instant.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_manually.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }
        protected void btn_instant_Click(object sender, EventArgs e)
        {
            btn_instant.BackColor = Color.Green;
            InstantIssueData();
            pnl_student.Visible = false;
            pnl_teacher.Visible = false;
            pnl_manually.Visible = false;
            btn_student.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_teacher.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            btn_manually.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }

        protected void GridView4_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Issue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView4.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView4.Rows[rowindex].Cells[4].Text);
                    DateTime due_date = CalculateReturnDate();
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@ret",due_date),
                      new SqlParameter("@flag",0)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_ISSUEBOOK", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Issued Successfully", "Success", "Success");
                        InstantIssueData();
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Issue_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    InstantIssueData();
                }
            }
            else if (e.CommandName == "CancelIssue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView4.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView4.Rows[rowindex].Cells[4].Text);
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@FLAG",0)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_CANCELISSUE", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Cancelled Successfully", "Success", "Success");
                        GridView4.EditIndex = -1;
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Cancel_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    InstantIssueData();
                }
            }
        }

        private void InstantIssueData()
        {
            try
            {
                DataSet ds = DBContext.GetDataSet("USP_GETISSUE_DATA_INSTANT", null, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    GridView4.DataSource = ds;
                    GridView4.DataBind();
                    pnl_instant.Visible = true;
                    pnl_student.Visible = false;
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                    pnl_instant.Visible = false;
                    pnl_student.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Entries Found to Issue - Instant";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        private void ManualIssueData()
        {
            try
            {
                DataSet ds = DBContext.GetDataSet("USP_GETISSUE_DATA_MANUAL", null, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    GridView5.DataSource = ds;
                    GridView5.DataBind();
                    pnl_manually.Visible = true;
                    pnl_student.Visible = false;
                    pnl_teacher.Visible = false;
                    pnl_instant.Visible = false;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_teacher.Visible = false;
                    pnl_nodata.Visible = false;
                    pnl_instant.Visible = false;
                    pnl_manually.Visible = false;
                    pnl_student.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Manual Issues Yet";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Issue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView3.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView3.Rows[rowindex].Cells[4].Text);
                    DateTime due_date = CalculateReturnDate();
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@ret",due_date),
                      new SqlParameter("@flag",1)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_ISSUEBOOK", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Issued Successfully", "Success", "Success");
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Issue_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    TeachersIssueData();
                }
            }
            else if (e.CommandName == "CancelIssue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView3.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView3.Rows[rowindex].Cells[4].Text);
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@FLAG",1)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_CANCELISSUE", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Cancelled Successfully", "Success", "Success");
                        GridView3.EditIndex = -1;
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Cancel_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    TeachersIssueData();
                }
            }
        }

        protected void btn_FCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                string rollno = GridView3.Rows[rowindex].Cells[1].Text;
                int book = Convert.ToInt32(GridView3.Rows[rowindex].Cells[4].Text);
                SqlParameter[] sp = new SqlParameter[]
                  {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@FLAG",1)
                  };
                int ds = DBContext.ExecuteNonQueryCmd("USP_CANCELISSUE", sp, CommandType.StoredProcedure);
                if (ds < 1)
                {
                    ShowToastr(this.Page, "Book Cancelled Successfully", "Success", "Success");
                    GridView3.EditIndex = -1;
                }
                else
                {
                    ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Cancel_Click()", ex.Message);
                ShowToastr(this.Page, ex.Message, "Error", "error");
            }
            finally
            {
                TeachersIssueData();
            }
        }

        private void ManualReq(int sid, int bid, int cid, int slot, string role, long subid, int sem, int year, int facid, int flag)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[]
                {
                new SqlParameter("@SID", sid),
                new SqlParameter("@BID", bid),
                new SqlParameter("@CID", cid),
                new SqlParameter("@SLOT", slot),
                new SqlParameter("@ROLE", role),
                new SqlParameter("@SUB", subid),
                new SqlParameter("@SEM", sem),
                new SqlParameter("@FACID", facid),
                new SqlParameter("@FLAG", flag),
                new SqlParameter("@YEAR", year),
                new SqlParameter("@FID", fid),
                };
                DBContext.ExecuteNonQueryCmd("USP_INSERT_MANUALBOOK_REQUEST", sp2, CommandType.StoredProcedure);
                ShowToastr(this.Page, "Book Issued Successfully", "Success", "success");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_missue_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnl_stud.Visible == true)
                {
                    int year = GetCurrentYear();
                    string role = "Student";
                    if (string.IsNullOrEmpty(txt_roll.Text) || string.IsNullOrEmpty(Convert.ToString(txt_bid.Text)))
                    {
                        ShowToastr(this.Page, "Kindly fill all the details", "", "error");
                    }
                    else
                    {
                        string id = txt_roll.Text;
                        int bid = Convert.ToInt32(txt_bid.Text);
                        int slot = Convert.ToInt32(ddl_slot.SelectedValue);
                        SqlParameter[] sp = new SqlParameter[]
                        {
                            new SqlParameter("@ROLL",id)
                        };
                        int i = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT COUNT(*) FROM BOOK_REQUISITION WHERE SID=(SELECT STUDENT_ID FROM STUDENTS_INFO WHERE ROLL_NO=@ROLL AND STATUS='R') AND STATUS=6 AND RECORD_STATUS_ID=1", sp, CommandType.Text));
                        if (i > 2)
                        {
                            ShowToastr(this.Page, "User Request Slot is Full", "Success", "success");
                            clear();
                        }
                        else
                        {
                            long subid;
                            int cid, sid, sem;
                            SqlParameter[] sp1 = new SqlParameter[]
                            {
                            new SqlParameter("@ROLL", id),
                            new SqlParameter("@BOOK", bid),
                            new SqlParameter("@FLAG", 0),
                            };
                            DataSet ds = DBContext.GetDataSet("USP_GETDATA_FOR_MANUALREQUEST", sp1, CommandType.StoredProcedure);
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                cid = Convert.ToInt32(ds.Tables[0].Rows[0]["COURSE_ID"]);
                                subid = Convert.ToInt64(ds.Tables[0].Rows[0]["SUBJECTKEY"]);
                                sid = Convert.ToInt32(ds.Tables[0].Rows[0]["STUDENT_ID"]);
                                sem = Convert.ToInt32(ds.Tables[0].Rows[0]["SEMESTER"]);
                                DataSet c = DBContext.GetDataSet("SELECT CASE WHEN STATUS = 1 THEN 'Available' ELSE 'Not Available' END AS Status FROM BOOK_MASTER WHERE BOOK_ID =" + bid, null, CommandType.Text);
                                string status = c.Tables[0].Rows[0]["STATUS"].ToString();
                                if (status == "Available")
                                {
                                    ManualReq(sid, bid, cid, slot, role, subid, sem, year, 0, 0);
                                    ManualIssueData();
                                    btn_manually.BackColor = Color.Green;
                                    btn_teacher.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                                    btn_instant.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                                    btn_student.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                                }
                                else
                                {
                                    ShowToastr(this.Page, "Book isn't Available Now", "", "error");
                                }
                                clear();
                            }
                            else
                            {
                                ShowToastr(this.Page, "Book isn't Available Now", "", "error");
                            }
                        }
                    }
                }
                else if (pnl_fac.Visible == true)
                {
                    int year = GetCurrentYear();
                    string role = "Teacher";
                    if (ddl_fid.SelectedIndex == 0 || string.IsNullOrEmpty(Convert.ToString(txt_bkid.Text)))
                    {
                        ShowToastr(this.Page, "Kindly fill all the details", "", "error");
                    }
                    else
                    {
                        string id = ddl_fid.SelectedValue;
                        int bid = Convert.ToInt32(txt_bkid.Text);
                        long subid;
                        int cid, sem;
                        SqlParameter[] sp1 = new SqlParameter[]
                        {
                        new SqlParameter("@ROLL", Convert.ToInt32(id)),
                        new SqlParameter("@BOOK", bid),
                        new SqlParameter("@FLAG", 1),
                        };
                        DataSet ds = DBContext.GetDataSet("USP_GETDATA_FOR_MANUALREQUEST", sp1, CommandType.StoredProcedure);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            cid = Convert.ToInt32(ds.Tables[0].Rows[0]["COURSE_ID"]);
                            subid = Convert.ToInt64(ds.Tables[0].Rows[0]["SUBJECTKEY"]);
                            sem = Convert.ToInt32(ds.Tables[0].Rows[0]["SEMESTER"]);
                            int facid = Convert.ToInt32(id);
                            DataSet c = DBContext.GetDataSet("SELECT CASE WHEN STATUS = 1 THEN 'Available' ELSE 'Not Available' END AS Status FROM BOOK_MASTER WHERE BOOK_ID =" + bid, null, CommandType.Text);
                            string status = c.Tables[0].Rows[0]["STATUS"].ToString();
                            if (status == "Available")
                            {
                                ManualReq(0, bid, cid, 1, role, subid, sem, year, facid, 1);
                                ManualIssueData();
                                btn_manually.BackColor = Color.Green;
                                btn_teacher.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                                btn_instant.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                                btn_student.BackColor = ColorTranslator.FromHtml("#2e2d2d");
                            }
                            else
                            {
                                ShowToastr(this.Page, "Book isn't Available Now", "", "error");
                            }
                            clear();
                        }
                        else
                        {
                            ShowToastr(this.Page, "Book isn't Available Now", "", "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_add_Click()", ex.Message);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "closePopup1();", true);
            }
        }

        protected void GridView5_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelIssue")
            {
                try
                {
                    int rowindex = Convert.ToInt32(e.CommandArgument);
                    string rollno = GridView5.Rows[rowindex].Cells[1].Text;
                    int book = Convert.ToInt32(GridView3.Rows[rowindex].Cells[4].Text);
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@sid", rollno),
                      new SqlParameter("@book",book),
                      new SqlParameter("@FLAG",1)
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("USP_CANCELISSUE", sp, CommandType.StoredProcedure);
                    if (ds < 1)
                    {
                        ShowToastr(this.Page, "Book Cancelled Successfully", "Success", "Success");
                        GridView5.EditIndex = -1;
                    }
                    else
                    {
                        ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_Cancel_Click()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
                finally
                {
                    ManualIssueData();
                }
            }
        }

        protected void btn_instan_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup();", true);
        }

        protected void btn_stud_Click(object sender, EventArgs e)
        {
            btn_stud.BackColor = Color.Green;
            btn_fac.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            pnl_stud.Visible = true;
            pnl_fac.Visible = false;
        }

        protected void btn_fac_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("SELECT CONCAT(FIRST_NAME, ' ', LAST_NAME) AS FACNAME, FACULTY_ID FROM FACULTY_MASTER WHERE JOB_TITLE IN ('Office', 'ASSISTANT LIBRARIAN', 'ASSISTANT PROFESSOR', 'Clerk', 'Librarian', 'Peon');\r\n", sp, CommandType.Text);
                ddl_fid.DataSource = ds;
                ddl_fid.DataTextField = "FACNAME";
                ddl_fid.DataValueField = "FACULTY_ID";
                ddl_fid.DataBind();
                ddl_fid.Items.Insert(0, new ListItem("-- Select Faculty --", "0"));
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookIssue.aspx.cs -> btn_fac_Click()", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
            btn_fac.BackColor = Color.Green;
            btn_stud.BackColor = ColorTranslator.FromHtml("#2e2d2d");
            pnl_stud.Visible = false;
            pnl_fac.Visible = true;
        }

        public void AutoCancel()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            TimeSpan check = new TimeSpan(15, 15, 0, 0);
            if (time > check)
            {
                SqlParameter[] sp = null;
                DBContext.ExecuteNonQueryCmd("USP_AUTOCANCEL_ISSUES", sp, CommandType.StoredProcedure);
            }
            else
            {
                StudentsIssueData();
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}