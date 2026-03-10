using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web.Services.Description;

namespace E_Granthalaya
{
    public partial class Stud_BookRequest : System.Web.UI.Page
    {
        static int id, dept_id, grno;
        string name, designation, deptCode, deptName;
        DateTime issueDate;

        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               Session["loginid"] = "if23102@UG";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }
                else if (jobTitle == "Librarian" || jobTitle == "Clerk" || jobTitle == "Peon" || jobTitle == "Assistant Professor")
                {
                    Response.Redirect("Unauthorized_Access.aspx");
                }
                else
                {
                    GetLoggedInUserDetails();
                    GetCourse();
                    GetSubjects();
                    GetPublisher();
                    issueDate = Convert.ToDateTime(CalculateIssueDate());
                   // DateTime nextIssueDate;
                    GetTimeSlots();
                    //issueDate = nextIssueDate;
                    GetBookList();
                    lblstudent.Visible = false;
                    lblbookid.Visible = false;
                }
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "chk_viewMode")
                {
                    chk_viewMode_CheckedChanged(sender, e);
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
                                id = Convert.ToInt32(ds.Tables[1].Rows[0]["ID"]);
                                grno = Convert.ToInt32(ds.Tables[1].Rows[0]["GRNO"]);
                            }
                        }
                    }
                }
                else
                {
                    name = "Administrator";
                    designation = "Administrator";
                    id = 0;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void GetCourse()
        {
            try
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@GRNO", grno),
                    new SqlParameter("@DEPTID", dept_id)
                };

                DataSet ds = DBContext.GetDataSet("USP_GET_COURSE_FOR_STUDENT", param, CommandType.StoredProcedure);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddl_course.DataSource = ds.Tables[0];
                        ddl_course.DataTextField = "COURSE_NAME";
                        ddl_course.DataValueField = "COURSE_ID";
                        ddl_course.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetCourse()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }
        protected void GetCategory()
        {
            try
            {
                SqlParameter[] param = null;


                DataSet ds = DBContext.GetDataSet("USP_GET_CATEGORY_FOR_STUDENT", param, CommandType.StoredProcedure);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddl_category.DataSource = ds.Tables[0];
                        ddl_category.DataTextField = "CATEGORYNAME";
                        ddl_category.DataValueField = "CATEGORY_ID";
                        ddl_category.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetCourse()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void GetSubjects()
        {
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@COURSEID", ddl_course.SelectedValue),
                   new SqlParameter("@YEARID", GetAcademicYear())
                };

                DataSet DS = DBContext.GetDataSet("USP_GET_SUBJECTS_FOR_BOOKREQUEST", sp, CommandType.StoredProcedure);
                ddl_subject.DataSource = DS;
                ddl_subject.DataTextField = "SUBJECT_NAME";
                ddl_subject.DataValueField = "SUBJECT_KEY";
                ddl_subject.DataBind();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.ASPX -> GetSubjects()", ex.Message);
                ShowToastr(this.Page, "Error while getting subjects.", "Error", "error");
            }
        }

        public void GetPublisher()
        {
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@COURSEID", Convert.ToInt32(ddl_course.SelectedValue)),
                   new SqlParameter("@SUBJECTKEY", ddl_subject.SelectedValue)
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_PUBLISHER_FOR_BOOKREQUEST", sp, CommandType.StoredProcedure);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddl_publisher.DataSource = ds.Tables[0];
                        ddl_publisher.DataTextField = "PUBLISHERNAME";
                        ddl_publisher.DataValueField = "P_ID";
                        ddl_publisher.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetPublisher()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }
        public void GetPublisherForCategory()
        {
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@CATEGORYID", ddl_category.SelectedValue),
                   //new SqlParameter("@SUBJECTKEY", ddl_subject.SelectedValue)
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_PUBLISHERCATE_FOR_BOOKREQUEST", sp, CommandType.StoredProcedure);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddl_publisher.DataSource = ds.Tables[0];
                        ddl_publisher.DataTextField = "PUBLISHERNAME";
                        ddl_publisher.DataValueField = "P_ID";
                        ddl_publisher.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetPublisher()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void GetTimeSlots()
        {
            
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@ISSUEDATE", CalculateIssueDate().Date)
                };

                DataSet DS = DBContext.GetDataSet("USP_GET_TIMESLOT_FOR_BOOKREQUEST", sp, CommandType.StoredProcedure);
                issueDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["AVAILABLE_ISSUE_DATE"]);
                ddl_timeslot.DataSource = DS;
                ddl_timeslot.DataTextField = "TIME_DESC";
                ddl_timeslot.DataValueField = "SLOT_ID";
                ddl_timeslot.DataBind();

                ListItem li = new ListItem("--Select--", "0");
                ddl_timeslot.Items.Insert(0, li);

            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> GetTimeSlots()", ex.Message);
                ShowToastr(this.Page, "Error while getting timslots.", "Error", "error");
            }
        }

        public void GetBookList()
        {
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@COURSEID", ddl_course.SelectedValue),
                   new SqlParameter("@SUBJECTKEY", ddl_subject.SelectedValue),
                   new SqlParameter("@PUBLISHERID", ddl_publisher.SelectedValue)
                };

                DataSet DS = DBContext.GetDataSet("USP_GET_BOOKS_FOR_BOOKREQUEST", sp, CommandType.StoredProcedure);
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    if (chk_viewMode.Checked)
                    {
                        pnl_grid.Visible = false;
                        pnl_card.Visible = true;
                        pnl_nodata.Visible = false;
                        Repeater1.DataSource = DS;
                        Repeater1.DataBind();
                    }
                    else
                    {
                        pnl_grid.Visible = false;
                        pnl_card.Visible = false;
                        pnl_nodata.Visible = false;
                        GridView1.DataSource = DS;
                        GridView1.DataBind();
                    }
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_card.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.ASPX -> GetSubjects()", ex.Message);
                ShowToastr(this.Page, "Error while getting Books details.", "Error", "error");
            }
        }

        public void GetBookListForCategory()
        {
            try
            {
                SqlParameter[] sp = {
                   new SqlParameter("@CATEGORYID", ddl_category.SelectedValue),
                  // new SqlParameter("@SUBJECTKEY", ddl_subject.SelectedValue),
                   new SqlParameter("@PUBLISHERID", ddl_publisher.SelectedValue)
                };

                DataSet DS = DBContext.GetDataSet("USP_GET_BOOKS_FOR_BOOKREQUEST_CATE", sp, CommandType.StoredProcedure);
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    if (chk_viewMode.Checked)
                    {
                        pnl_grid.Visible = false;
                        pnl_card.Visible = true;
                        pnl_nodata.Visible = false;
                        Repeater1.DataSource = DS;
                        Repeater1.DataBind();
                    }
                    else
                    {
                        pnl_grid.Visible = false;
                        pnl_card.Visible = false;
                        pnl_nodata.Visible = false;
                        GridView1.DataSource = DS;
                        GridView1.DataBind();
                    }
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_card.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.ASPX -> GetSubjects()", ex.Message);
                ShowToastr(this.Page, "Error while getting Books details.", "Error", "error");
            }
        }
        protected void ddl_courseSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_course.SelectedItem.Text != "ALL")
            {
                GetSubjects();
                GetPublisher();
                GetBookList();
                lable1.Visible = false; ddl_category.Visible = false;
                lable.Visible = true; ddl_subject.Visible = true;
            }
            else
            {
                lable.Visible = false; ddl_subject.Visible = false;

                GetCategory();
                GetPublisherForCategory();
                GetBookListForCategory();
                lable1.Visible = true; ddl_category.Visible = true;

            }
        }

        protected void ddl_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPublisherForCategory();
            GetBookListForCategory();
        }

        protected void ddl_subjectSelectedIndexChanged(object sender, EventArgs e)
        {
            GetPublisher();
            GetBookList();
        }

        protected void ddl_publisherSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_course.SelectedItem.Text != "ALL")
            {
                GetBookList();
            }
            else
            {
                GetBookListForCategory();
            }
        }
        protected void ddl_timeslotSelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_information.Visible = true;
            lbl_information.Text = "Book issue date will be: " + issueDate.ToString("d MMMM yyyy") + " between " + ddl_timeslot.SelectedItem.Text + " timeslot.";
        }

        protected void chk_viewMode_CheckedChanged(object sender, EventArgs e)
        {
            //ShowToastr(this.Page, "Successful", "Success", "success");
            if (chk_viewMode.Checked)
            {
                pnl_card.Visible = true;
                pnl_grid.Visible = false;
            }
            else
            {
                pnl_grid.Visible = true;
                pnl_card.Visible = false;
            }
            GetBookList();
        }

        protected string GetImageURL(object bookPhoto)
        {
            string imageUrl = bookPhoto.ToString();

            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = "~/assets/images/defaultbook.jpg";
            }
            else
            {
                if (!System.IO.File.Exists(Server.MapPath(imageUrl)))
                {
                    imageUrl = "~/assets/images/defaultbook.jpg";
                }
            }

            return imageUrl;
        }

        public int GetAcademicYear()
        {
            int ds = 0;
            try
            {
                string sql;
                DateTime dt = DateTime.Now;

                if (dt.Month <= 6)
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=(YEAR(GETDATE())-1)";
                }
                else
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID=YEAR(GETDATE())";
                }

                ds = Convert.ToInt32(DBContext.ExecuteScalarCmd(sql, null, CommandType.Text));
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> GetAcademicYear()", ex.Message);
            }
            return ds;
        }

        protected void btn_closeClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closemodal();", true);
        }

        protected void btn_instantClick(object sender, EventArgs e)
        {
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
            Label5.Visible = true;
            txt_code.Visible = true;
            lbl_information.Text = "Book issue date will be Today !! You Can take Your Book Now";
            string status = lbl_status.Text;
            if (status == "Book is Available!")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "showmodal();", true);
            }
            else
            {
                ShowToastr(this.Page, "Book is Not Availiable Yet !!", "Error", "error");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
            }
        }
        protected void btn_instantsaveClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_code.Text))
            {
                string rollno = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT ROLL_NO FROM STUDENTS_INFO WHERE STUDENT_ID=" + id, null, CommandType.Text));
                int otp = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT CODE FROM INSTANTREQ_CODES WHERE RECORD_STATUS_ID=1 and ROLLNO='" + rollno + "'", null, CommandType.Text));
                //int otp = 1111;
                if (Convert.ToInt32(txt_code.Text) == otp)
                {
                    issueDate = Convert.ToDateTime(CalculateIssueDate());
                    try
                    {
                        lbl_information.Text = "Book issue date will be Today !! You can take Your Book Now";
                        int bookID = Convert.ToInt32(lblbookid.Text);
                        int courseId = Convert.ToInt32(ddl_course.SelectedValue);
                        string subjectKey = Convert.ToString(ddl_subject.SelectedValue);
                        int publisherID = Convert.ToInt32(ddl_publisher.SelectedValue);
                        // int slotID = Convert.ToInt32(ddl_timeslot.SelectedValue);
                        string status = lbl_status.Text;
                        if (status == "Book is Available!")
                        {
                            SqlParameter[] param = {
                                new SqlParameter("@YEARID", GetAcademicYear()),
                                new SqlParameter("@BOOKID", bookID),
                                new SqlParameter("@STUDID", id),
                                new SqlParameter("@COURSEID", courseId),
                                new SqlParameter("@PUBLISHERID", publisherID),
                                new SqlParameter("@SLOTID", 0),
                                new SqlParameter("@SUBJECTKEY", subjectKey),
                                new SqlParameter("@ISSUEDATE", DateTime.Now),new SqlParameter("@REQ_TYPE", "Instant"),
                                };
                            DataSet ds = DBContext.GetDataSet("USP_SAVE_BOOKREQUEST_DETAILS", param, CommandType.StoredProcedure);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int flag = Convert.ToInt16(ds.Tables[0].Rows[0]["FLAG"]);
                                    if (flag == 0)
                                    {
                                        ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Error", "error");
                                    }
                                    else
                                    {
                                        ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Success", "success");
                                        if (ddl_course.SelectedItem.Text != "ALL")
                                        {
                                            GetBookList();
                                        }
                                        else
                                        {
                                            GetBookListForCategory();
                                        }
                                        ShowToastr(this.Page, "Book Request Placed Successfully...!", "Success", "Success");
                                    }
                                }
                            }
                        }
                        else
                        {
                            ShowToastr(this.Page, "Book is Not Availiable Yet !!", "Error", "error");
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
                    }
                    catch (Exception ex)
                    {
                        Common.WriteToLog("Stud_BookRequest.cs -> btn_save_Click()", ex.Message);
                    }
                }
                else
                {
                    ShowToastr(this.Page, "Invalid OTP !!", "Error", "error");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
                ShowToastr(this.Page, "Kindly Fill the code and Verify", "Error", "error");
            }
        }

        protected void btn_saveClick(object sender, EventArgs e)
        {
            try
            {
                issueDate = Convert.ToDateTime(CalculateIssueDate());
                int bookID = Convert.ToInt32(lblbookid.Text);
                int courseId = Convert.ToInt32(ddl_course.SelectedValue);
                string subjectKey = Convert.ToString(ddl_subject.SelectedValue);
                int publisherID = Convert.ToInt32(ddl_publisher.SelectedValue);
                int slotID = Convert.ToInt32(ddl_timeslot.SelectedValue);
                string status = lbl_status.Text;
                if (status == "Book is Available!")
                {
                    SqlParameter[] param = {
                    new SqlParameter("@YEARID", GetAcademicYear()),
                    new SqlParameter("@BOOKID", bookID),
                    new SqlParameter("@STUDID", id),
                    new SqlParameter("@COURSEID", courseId),
                    new SqlParameter("@PUBLISHERID", publisherID),
                    new SqlParameter("@SLOTID", slotID),
                    new SqlParameter("@SUBJECTKEY", subjectKey),
                    new SqlParameter("@ISSUEDATE", issueDate),new SqlParameter("@REQ_TYPE", "Normal"),
                    };

                    DataSet ds = DBContext.GetDataSet("USP_SAVE_BOOKREQUEST_DETAILS", param, CommandType.StoredProcedure);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt16(ds.Tables[0].Rows[0]["FLAG"]);
                            if (flag == 0)
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Error", "error");
                            }
                            else
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Success", "success");
                                if (ddl_course.SelectedItem.Text != "ALL")
                                {
                                    GetBookList();
                                }
                                else
                                {
                                    GetBookListForCategory();
                                }
                            }
                        }
                    }
                }
                else
                {
                    ShowToastr(this.Page, "Book is Not Availiable Yet !!", "Error", "error");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> btn_save_Click()", ex.Message);
            }

        }

        protected void PopulateDropDown(DropDownList ddl, DataTable dt, string textField, string valueField)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = textField;
                    ddl.DataValueField = valueField;
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("-- Select -- ", "-1"));
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> PopulateDropDown()", ex.Message);
            }
        }

        protected void btn_viewInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddl_timeslot.SelectedValue) == 0)
                {
                    ShowToastr(this.Page, "Please select valid time slot.", "Error", "error");
                    return;
                }
                else
                {
                    string[] details = (sender as Button).CommandArgument.Split('|');

                    int bookId = Convert.ToInt32(details[0]);
                    string linkedwith = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT LINKEDWITH FROM BOOK_MASTER WHERE BOOK_ID=" + bookId, null, CommandType.Text));
                    string bookName = details[1].ToString();
                    string publisherName = details[2].ToString();
                    string status = details[3].ToString();
                    string course = ddl_course.SelectedItem.Text;
                    string timeSlot = ddl_timeslot.SelectedItem.Text;
                    string semDetails = ddl_subject.SelectedItem.Text;
                    string[] arr = semDetails.Split('-');
                    string sem = arr[0];
                    string subject = arr[1];

                    DateTime issueDate = CalculateIssueDate();
                    DateTime returndate = issueDate.AddDays(2);
                    lbl_status.Text = status;
                    lblbookid.Text = bookId.ToString();
                    lblbookname.Text = bookName;
                    lbl_publisher.Text = publisherName;
                    lbl_course.Text = course;
                    lbl_sem.Text = sem;
                    if (ddl_course.SelectedItem.Text != "ALL")
                    {
                        lbl_subject.Text = subject;
                    }
                    else
                    {
                        Label3.Visible = false;
                        lbl_subject.Visible = false;
                    }
                    lbl_timeslot.Text = timeSlot;

                    lbl_information.Text = "Book issue date will be: " + issueDate.ToString("d MMMM yyyy") + " between " + ddl_timeslot.SelectedItem.Text + " timeslot.";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup(" + linkedwith + ");", true);
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> btn_viewInfo_Click()", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting the book details.", "Error", "error");
            }
        }

        protected void btn_view1Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddl_timeslot.SelectedValue) == 0)
                {
                    ShowToastr(this.Page, "Please select valid time slot.", "Error", "error");
                    return;
                }
                else
                {
                    int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;

                    int bookID = Convert.ToInt32(GridView1.DataKeys[rowindex].Value);
                    string linkedwith = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT LINKEDWITH FROM BOOK_MASTER WHERE BOOK_ID=" + bookID, null, CommandType.Text));
                    string bookName = GridView1.Rows[rowindex].Cells[2].Text;
                    string publisherName = GridView1.Rows[rowindex].Cells[3].Text;
                    string status = GridView1.Rows[rowindex].Cells[4].Text;
                    string course = ddl_course.SelectedItem.Text;
                    string timeSlot = ddl_timeslot.SelectedItem.Text;
                    string details = ddl_subject.SelectedItem.Text;
                    string[] arr = details.Split('-');
                    string sem = arr[0];
                    string subject = arr[1];

                    DateTime issueDate = CalculateIssueDate();
                    DateTime returndate = issueDate.AddDays(2);

                    //int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                    //GridViewRow row = GridView1.Rows[rowIndex];
                    lbl_status.Text = status;
                    lblbookid.Text = bookID.ToString();
                    lblbookname.Text = bookName;
                    lbl_publisher.Text = publisherName;
                    lbl_course.Text = course;
                    lbl_sem.Text = sem;
                    lbl_subject.Text = subject;
                    lbl_timeslot.Text = timeSlot;

                    lbl_information.Text = "Book issue date will be: " + issueDate.ToString("d MMMM yyyy") + " between " + ddl_timeslot.SelectedItem.Text + " timeslot.";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup(" + linkedwith + ");", true);
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> btn_view1Click()", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting the book details.", "Error", "error");
            }
        }
        protected string GetBackgroundColor1(string status)
        {
            switch (status)
            {
                case "Book is Discarded!":
                    return "border-left: .40rem solid #680000 !important;";
                case "Book is Available!":
                    return "border-left: .40rem solid #006317 !important;";
                case "Book is Issued!":
                    return "border-left: .40rem solid #7C4202 !important;";
                case "Book is Requested!":
                    return "border-left: .40rem solid #033763 !important;";
                default:
                    return "";
            }
        }
        protected string GetBackgroundColor3(string status)
        {
            switch (status)
            {
                case "Book is Discarded!":
                    return "background-color: #680000; box-shadow: 0 4px 8px rgba(104, 0, 0, 0.7);";
                case "Book is Available!":
                    return "background-color: #006317; box-shadow: 0 4px 8px rgba(0, 99, 23, 0.7);";
                case "Book is Issued!":
                    return "background-color: #7C4202; box-shadow: 0 4px 8px rgba(124, 66, 2, 0.7);";
                case "Book is Requested!":
                    return "background-color: #033763; box-shadow: 0 4px 8px rgba(3, 55, 99, 0.7);";
                default:
                    return "";
            }
        }
        protected string GetBackgroundColor2(string status)
        {
            switch (status)
            {
                case "Book is Discarded!":
                    return "background-color: #FFECEC; box-shadow: 0 4px 8px rgba(255, 204, 204, 0.7);";
                case "Book is Available!":
                    return "background-color: #F1FFF1; box-shadow: 0 4px 8px rgba(204, 255, 204, 0.7);";
                case "Book is Issued!":
                    return "background-color: #FFFFF0; box-shadow: 0 4px 8px rgba(255, 255, 204, 0.7);";
                case "Book is Requested!":
                    return "background-color: #F4FCFF; box-shadow: 0 4px 8px rgba(204, 230, 255, 0.7);";
                default:
                    return "";
            }
        }
        public DateTime CalculateIssueDate()
        {
            try
            {
                DateTime current_time = DateTime.Now;
                DateTime issue_date = current_time.Hour < 9 ? current_time : current_time.Date.AddDays(1);

                // checking sunday and holidays in table
                if (current_time.DayOfWeek == DayOfWeek.Sunday || IsHoliday(issue_date))
                {
                    issue_date = FindWorkingDay(issue_date);
                }

                return issue_date;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.cs -> CalculateIssueDate()", ex.Message);
                return DateTime.Now.AddDays(1);
            }
        }

        public Boolean IsHoliday(DateTime Date)
        {
            string sql = "SELECT COUNT(*) FROM HOLIDAY_MASTER WHERE H_DATE = CAST(@date AS DATE)";
            SqlParameter[] sp = { new SqlParameter("@date", Date) };
            int count = Convert.ToInt32(DBContext.ExecuteScalarCmd(sql, sp, CommandType.Text));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DateTime FindWorkingDay(DateTime current_Date)
        {
            while (IsHoliday(current_Date))
            {
                current_Date = current_Date.AddDays(1);
            }
            return current_Date;
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}