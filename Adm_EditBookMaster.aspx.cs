using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class Adm_EditBookMaster : System.Web.UI.Page
    {
        static int fid, dept_id, check;
        string name, designation, deptCode, deptName;
        string deptcode;

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
                    BindDropdownList();
                    pnl_bookedit.Visible = false;
                    pnl_grid.Visible = false;
                    pnl_nodata.Visible = true;
                    ddl_Semester.Items.Insert(0, new ListItem("-- Select --", "0"));
                    ddl_subject.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
                else
                {
                    Response.Redirect("Unauthorized_Access.aspx");
                }
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "chk_updateall")
                {
                    checkboxChanged(sender, e);
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
                Common.WriteToLog("Adm_EditBookMaster.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void BindDropdownList()
        {
            try
            {
                DataSet ds = DBContext.GetDataSet("USP_GETBOOKMASTERDATA", null, CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)
                {
                    PopulateDropDown(ddl_booktype, ds.Tables[0], "CategoryName", "Category_id");
                    PopulateDropDown(ddl_medium, ds.Tables[2], "Language", "Medium_id");
                    PopulateDropDown(ddl_course, ds.Tables[1], "Course_Name", "Course_Id");
                    PopulateDropDown(ddl_book_source, ds.Tables[3], "bs_type", "bs_id");
                    PopulateDropDown(ddl_publisher, ds.Tables[4], "PublisherName", "P_ID");
                    PopulateDropDown(ddl_author, ds.Tables[5], "AuthorName", "Author_Id");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_EditBookMaster.aspx -> BindDropdownList()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
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
                    ddl.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_EditBookMaster.aspx -> PopulateDropDown()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void GetBookDetailsGV()
        {
            try
            {
                DataSet dt;
                // string sql = "SELECT CATEGORYNAME FROM BOOK_MASTER BM JOIN CATEGORYMASTER CM ON BM.CATEGORY_ID = CM.CATEGORY_ID WHERE BM.BOOK_ID=@BOOKID";
                string type = ddl_booktype.SelectedItem.ToString();
                if (type == "Text Book" || type == "Reference")
                {
                    SqlParameter[] param = {
                        new SqlParameter("@COURSEID", Convert.ToInt32(ddl_course.SelectedValue)),
                        new SqlParameter("@SEM", Convert.ToInt32(ddl_Semester.SelectedValue)),
                        new SqlParameter("@SUBJECTID", Convert.ToInt32(ddl_subject.SelectedValue))
                    };
                    dt = DBContext.GetDataSet("USP_GETBOOKLIST_FOR_EDIT", param, CommandType.StoredProcedure);
                }
                else
                {
                    SqlParameter[] param1 = {
                        new SqlParameter("@CATEGORYID", Convert.ToInt32(ddl_booktype.SelectedValue)),
                    };
                    dt = DBContext.GetDataSet("USP_GETBOOKLIST_FOR_EDIT_NS", param1, CommandType.StoredProcedure);
                }

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    YourGridView.DataSource = dt;
                    YourGridView.DataBind();

                    pnl_grid.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_EditBookMaster.cs -> GetBookDetailsGV()", ex.Message);
                //ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        Boolean ValidateField()
        {
            if (string.IsNullOrWhiteSpace(txt_book_name.Text) ||
                ddl_author.SelectedIndex == 0 || ddl_booktype.SelectedIndex == 0 || ddl_book_source.SelectedIndex == 0 ||
                ddl_medium.SelectedIndex == 0 || ddl_publisher.SelectedIndex == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void GetSubjects()
        {
            int sem = Convert.ToInt32(ddl_Semester.SelectedValue);
            string sql = "SELECT SUBJECT_NAME, SUBJECT_ID FROM MYSQL_SM WHERE SUBJECT_ID IN (SELECT EG_SUB_ID FROM EG_SUBJECT_LINK WHERE SEMESTER = " + Convert.ToInt32(ddl_Semester.SelectedValue) + " AND COURSE_ID = " + Convert.ToInt32(ddl_course.SelectedValue) + ")";
            DataSet dt = DBContext.GetDataSet(sql, null, CommandType.Text);
            ddl_subject.DataSource = dt;
            ddl_subject.DataTextField = "Subject_Name";
            ddl_subject.DataValueField = "Subject_Id";
            ddl_subject.DataBind();
        }

        public void GetSemester()
        {
            int courseid = Convert.ToInt32(ddl_course.SelectedValue);
            string sql = "SELECT DISTINCT(SEMESTER) FROM SUBJECT_MASTER WHERE COURSE_ID=" + courseid;
            DataSet dt = DBContext.GetDataSet(sql, null, CommandType.Text);
            ddl_Semester.DataSource = dt;
            ddl_Semester.DataTextField = "SEMESTER";
            ddl_Semester.DataValueField = "SEMESTER";
            ddl_Semester.DataBind();
        }
        protected void ddl_booktype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
            {
                pnl_coursem.Visible = true;
                pnl_bookedit.Visible = false;
                pnl_grid.Visible = false;
                //clear();
                GetBookDetailsGV();
            }
            else
            {
                pnl_coursem.Visible = false;
                pnl_bookedit.Visible = false;
                pnl_grid.Visible = false;
                //clear();
                GetBookDetailsGV();
            }
        }

        protected void ddl_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSemester();
                GetSubjects();
                GetBookDetailsGV();
                pnl_bookedit.Visible = false;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_EditBookMaster.aspx -> ddl_course_SelectedIndexChanged()", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting semester.", "Error", "error");
            }
        }

        protected void ddl_Semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSubjects();
                GetBookDetailsGV();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_EditBookMaster.aspx -> ddl_Semester_SelectedIndexChanged() -> ", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting subjects.", "Error", "error");
            }
        }

        protected void ddl_subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBookDetailsGV();
        }

        void clear()
        {
            ddl_book_source.SelectedIndex = 0;
            txt_book_name.Text = string.Empty;
            txt_bookid.Text = string.Empty;
            ddl_medium.SelectedIndex = 0;
            ddl_author.SelectedIndex = 0;
            ddl_publisher.SelectedIndex = 0;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_book_name.Text) &&
                ddl_author.SelectedIndex != 0 && ddl_booktype.SelectedIndex != 0 && ddl_book_source.SelectedIndex != 0 ||
                ddl_medium.SelectedIndex != 0 && ddl_publisher.SelectedIndex != 0)
            {
                int mediumid = Convert.ToInt32(ddl_medium.SelectedValue);
                int categoryid = Convert.ToInt32(ddl_booktype.SelectedValue);
                string bookName = txt_book_name.Text;
                int authorid = Convert.ToInt32(ddl_author.SelectedValue);
                int isIndexed = check;
                int publisherid = Convert.ToInt32(ddl_publisher.SelectedValue);
                int bookid = Convert.ToInt32(txt_bookid.Text);
                // book id isko select button ke click pe se bhejna grid me se 
                //yeha kyuki grid me dikha rahe hainna bookid book id hataya kyuki mam ko nhi chahiye tha 
                SqlParameter[] param = null;
                try
                {
                    if (ddl_book_source.SelectedIndex == 0)
                    {
                        ShowToastr(this.Page, "All Fields are Required.. Please Fill Book Source!", "Error", "error");
                    }
                    else
                    {
                        if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
                        {
                            if (ddl_course.SelectedIndex != 0)
                            {
                                int subjectid = Convert.ToInt32(ddl_subject.SelectedValue);
                                string sem = (ddl_Semester.SelectedValue).ToString();
                                int courseid = Convert.ToInt32(ddl_course.SelectedValue);
                                param = new SqlParameter[]
                                {
                             new SqlParameter("@MEDIUM_ID", mediumid),new SqlParameter("@BOOKID", bookid),
                             new SqlParameter("@CATEGORY_ID", categoryid),
                             new SqlParameter("@SUBJECT_ID", subjectid),
                             new SqlParameter("@BOOKNAME", bookName),
                             new SqlParameter("@AUTHOR_ID", authorid),
                             new SqlParameter("@PUBLISHER_ID", publisherid),
                             new SqlParameter("@SEM", sem),
                             new SqlParameter("@COURSE_ID", courseid),
                             new SqlParameter("@FACULTY_ID", fid),
                             new SqlParameter("@BSOURCE", ddl_book_source.SelectedItem.Text),
                             new SqlParameter("@UPDATEALL", isIndexed)
                                };
                            }
                            else
                            {
                                ShowToastr(this.Page, "All Fields are Required.. Please Fill!", "Error", "error");
                            }
                        }
                        else
                        {
                            param = new SqlParameter[]
                            {
                             new SqlParameter("@MEDIUM_ID", mediumid),new SqlParameter("@BOOKID", bookid),
                             new SqlParameter("@CATEGORY_ID", categoryid),
                             new SqlParameter("@SUBJECT_ID", 0),
                             new SqlParameter("@BOOKNAME", bookName),
                             new SqlParameter("@AUTHOR_ID", authorid),
                             new SqlParameter("@PUBLISHER_ID", publisherid),
                             new SqlParameter("@SEM", 0),
                             new SqlParameter("@COURSE_ID", 0),
                             new SqlParameter("@FACULTY_ID", fid),
                             new SqlParameter("@BSOURCE", ddl_book_source.SelectedItem.Text),
                             new SqlParameter("@UPDATEALL", isIndexed)
                            };
                        }
                        DataSet ds = DBContext.GetDataSet("USP_UPDATEBOOKDETAILS", param, CommandType.StoredProcedure);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                                if (flag == 1)
                                {
                                    ShowToastr(this.Page, "Book Detail Updated Successfully", "Success", "success");
                                    pnl_grid.Visible = false;
                                    pnl_nodata.Visible = true;
                                    clear();
                                    pnl_bookedit.Visible = false;
                                    pnl_coursem.Visible = false;
                                }
                                else
                                {
                                    ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Error", "error");
                                    ShowToastr(this.Page, "All Fields are Required.. Please Fill!", "Error", "error");
                                }
                            }
                        }
                        if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
                        {
                            pnl_coursem.Visible = true;
                            GetBookDetailsGV();
                        }
                        else
                        {
                            pnl_coursem.Visible = false;
                            GetBookDetailsGV();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Common.WriteToLog("Adm_EditBookMaster.aspx -> Button1_Click() ->", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving the book details.", "Error", "error");
                }
            }
            else
            {
                ShowToastr(this.Page, "All Fields are Required.. Please Fill!", "Error", "error");
            }
        }

        protected void checkboxChanged(object sender, EventArgs e)
        {
            if (chk_updateall.Checked)
            {
                check = 1;
                ShowToastr(this.Page, "Okay! All the copies will be updated..", "", "Info");

            }
            else
            {
                check = 0;
                ShowToastr(this.Page, "Okay! Only the Selected book will be updated, no copies changes..", "", "Info");
            }
        }

        protected void YourGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = YourGridView.SelectedIndex;
            int BOOKID = Convert.ToInt32(YourGridView.DataKeys[index].Values[0]);
            //string sql = "SELECT BM.BOOK_ID, BM.BOOKNAME, CM.CATEGORYNAME, P.PUBLISHERNAME, A.AUTHORNAME, M.LANGUAGE, C.COURSE_NAME, BM.SEM, MSM.SUBJECT_NAME, BM.BSOURCE " +
            //             "FROM BOOK_MASTER BM " +
            //             "JOIN PUBLISHERMASTER P ON BM.PUBLISHER_ID = P.P_ID " +
            //             "JOIN MEDIUMMASTER M ON BM.MEDIUM_ID = M.MEDIUM_ID " +
            //             "JOIN AUTHORMASTER A ON BM.AUTHOR_ID = A.AUTHOR_ID " +
            //             "JOIN COURSE_MASTER C ON C.COURSE_ID = BM.COURSE_ID " +
            //             "JOIN CATEGORYMASTER CM ON CM.CATEGORY_ID = BM.CATEGORY_ID " +
            //             "JOIN MYSQL_SM MSM ON BM.SUBJECT_ID = MSM.SUBJECT_ID " +
            //             "WHERE BM.BOOK_ID = @BOOKID";

            pnl_bookedit.Visible = true;
            DataSet ds;
            // string sql = "SELECT CATEGORYNAME FROM BOOK_MASTER BM JOIN CATEGORYMASTER CM ON BM.CATEGORY_ID = CM.CATEGORY_ID WHERE BM.BOOK_ID=@BOOKID";
            string type = ddl_booktype.SelectedItem.ToString();
            string subjectName = ddl_subject.SelectedValue;
            if (type == "Text Book" || type == "Reference")
            {
                SqlParameter[] param = {
                        new SqlParameter("@COURSEID", Convert.ToInt32(ddl_course.SelectedValue)),
                        new SqlParameter("@SEM", Convert.ToInt32(ddl_Semester.SelectedValue)),
                        new SqlParameter("@SUBJECTID", Convert.ToInt32(ddl_subject.SelectedValue))
                };
                ds = DBContext.GetDataSet("USP_GETBOOKLIST_FOR_EDIT", param, CommandType.StoredProcedure);
            }
            else
            {
                SqlParameter[] param1 = {
                        new SqlParameter("@CATEGORYID", Convert.ToInt32(ddl_booktype.SelectedValue)),
                };
                ds = DBContext.GetDataSet("USP_GETBOOKLIST_FOR_EDIT_NS", param1, CommandType.StoredProcedure);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                txt_bookid.Text = ds.Tables[0].Rows[0]["BOOK_ID"].ToString();
                string categoryName = ds.Tables[0].Rows[0]["CATEGORYNAME"].ToString();
                string bookSource = ds.Tables[0].Rows[0]["BSOURCE"].ToString();
                string bookName = ds.Tables[0].Rows[0]["BOOKNAME"].ToString();
                string publisherName = ds.Tables[0].Rows[0]["PUBLISHERNAME"].ToString();
                string language = ds.Tables[0].Rows[0]["LANGUAGE"].ToString();
                string authorName = ds.Tables[0].Rows[0]["AUTHORNAME"].ToString();
                string courseName = ds.Tables[0].Rows[0]["COURSE_NAME"].ToString();
                string semester = ds.Tables[0].Rows[0]["SEM"].ToString();

                if (type == "Text Book" || type == "Reference")
                {
                    subjectName = ds.Tables[0].Rows[0]["SUBJECT_NAME"].ToString();
                }
                if (ddl_course.SelectedIndex == 0)
                {
                    BindDropdownList();
                }
                else
                {
                    DataSet dm = DBContext.GetDataSet("USP_GETBOOKMASTERDATA", null, CommandType.StoredProcedure);
                    if (dm.Tables.Count > 0)
                    {
                        PopulateDropDown(ddl_booktype, dm.Tables[0], "CategoryName", "Category_id");
                        PopulateDropDown(ddl_medium, dm.Tables[2], "Language", "Medium_id");
                        PopulateDropDown(ddl_book_source, dm.Tables[3], "bs_type", "bs_id");
                        PopulateDropDown(ddl_publisher, dm.Tables[4], "PublisherName", "P_ID");
                        PopulateDropDown(ddl_author, dm.Tables[5], "AuthorName", "Author_Id");
                    }
                }
                ddl_booktype.SelectedValue = ddl_booktype.Items.FindByText(categoryName).Value;
                //ddl_book_source.SelectedValue = ddl_book_source.Items.FindByText(bookSource).Value;
                txt_book_name.Text = bookName;
                ddl_publisher.SelectedValue = ddl_publisher.Items.FindByText(publisherName).Value;
                ddl_medium.SelectedValue = ddl_medium.Items.FindByText(language).Value;
                ddl_author.SelectedValue = ddl_author.Items.FindByText(authorName).Value;

                if (type == "Text Book" || type == "Reference")
                {
                    pnl_coursem.Visible = true;
                    if (ddl_course.SelectedIndex != 0)
                    {
                        ddl_course.SelectedValue = ddl_course.Items.FindByText(courseName).Value;
                        GetSemester();
                        ddl_Semester.SelectedValue = ddl_Semester.Items.FindByText(semester).Value;
                        GetSubjects();
                        ddl_subject.SelectedValue = ddl_subject.Items.FindByText(subjectName).Value;
                    }
                    else
                    {
                        ShowToastr(this.Page, "Please Select the Course..", "", "Info");
                    }
                }
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}