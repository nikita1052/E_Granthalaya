using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace E_Granthalaya
{
    public partial class OPAC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCategory();
                GetCourse();
                GetSubjects();
                GetAuthor();
                GetPublisher();
                bindgridview();
            }
        }

        protected void btn_filters_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "openModal();", true);
        }

        private void bindgridview()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GET_BOOKS_FOR_OPAC", sp, CommandType.StoredProcedure);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gridview_books.DataSource = ds;
                    gridview_books.DataBind();

                }
                else
                {

                    lbl_not.Text = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("OPAC.aspx -> bindgridview()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void GetCourse()
        {
            try
            {

                string sql = "SELECT DISTINCT(COURSE_NAME) FROM COURSE_MASTER WHERE RECORD_STATUS_ID=1";
                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        cbl_course.DataSource = ds;
                        cbl_course.DataTextField = "COURSE_NAME";
                        cbl_course.DataValueField = "COURSE_NAME";
                        cbl_course.DataBind();
                        //ListItem li = new ListItem("--Select--", "0");
                        //cbl_course.Items.Insert(0, li);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetCourse()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void GetCategory()
        {
            try
            {
                string sql = "SELECT DISTINCT(CATEGORYNAME) FROM CATEGORYMASTER WHERE RECORD_STATUS_ID=1";
                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        cbl_category.DataSource = ds;
                        cbl_category.DataTextField = "CATEGORYNAME";
                        cbl_category.DataValueField = "CATEGORYNAME";
                        cbl_category.DataBind();
                        //ListItem li = new ListItem("--Select--", "0");
                        //cbl_category.Items.Insert(0, li);

                        //ListItem li = new ListItem("All","0");
                        //ddl_publisher.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetPublisher()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void GetSubjects()
        {
            try
            {

                string sql = "SELECT DISTINCT(SUBJECT_NAME) FROM SUBJECT_MASTER WHERE RECORD_STATUS_ID=1";
                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);
                cbl_subject.DataSource = ds;
                cbl_subject.DataTextField = "SUBJECT_NAME";
                cbl_subject.DataValueField = "SUBJECT_NAME";
                cbl_subject.DataBind();
                //ListItem li = new ListItem("--Select--", "0");
                //cbl_subject.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.ASPX -> GetSubjects()", ex.Message);
                ShowToastr(this.Page, "Error while getting subjects.", "Error", "error");
            }
        }

        public void GetAuthor()
        {
            try
            {
                string sql = "SELECT DISTINCT(AUTHORNAME) FROM AUTHORMASTER WHERE RECORD_STATUS_ID=1";
                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        cbl_author.DataSource = ds;
                        cbl_author.DataTextField = "AUTHORNAME";
                        cbl_author.DataValueField = "AUTHORNAME";
                        cbl_author.DataBind();
                        //ListItem li = new ListItem("--Select--", "0");
                        //cbl_author.Items.Insert(0, li);

                        //ListItem li = new ListItem("All","0");
                        //ddl_publisher.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetPublisher()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void GetPublisher()
        {
            try
            {
                string sql = "SELECT DISTINCT(PUBLISHERNAME) FROM PUBLISHERMASTER WHERE RECORD_STATUS_ID=1";
                DataSet ds = DBContext.GetDataSet(sql, null, CommandType.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        cbl_publisher.DataSource = ds;
                        cbl_publisher.DataTextField = "PUBLISHERNAME";
                        cbl_publisher.DataValueField = "PUBLISHERNAME";
                        cbl_publisher.DataBind();
                        //ListItem li = new ListItem("--Select--", "0");
                        //ddl_publisher.Items.Insert(0, li);

                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetPublisher()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }

        protected void btn_view_Click(object sender, EventArgs e)
        {
            LinkButton btn_view = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn_view.NamingContainer;
            int rowindex = row.RowIndex;

            string bookname = gridview_books.Rows[rowindex].Cells[1].Text;
            string publishername = gridview_books.Rows[rowindex].Cells[2].Text;
            string author = gridview_books.Rows[rowindex].Cells[3].Text;
            string course = gridview_books.Rows[rowindex].Cells[4].Text;
            //string sql = "SELECT (PUBLISHERNAME) FROM BOOK_MASTER B INNER JOIN PUBLISHERMASTER P ON B.PUBLISHER_ID=P.P_ID  WHERE BOOK_ID=@BOOKID GROUP BY PUBLISHERNAME";
            //SqlParameter[] sp = { new SqlParameter("@BOOKID", Convert.ToInt32(bookid)) };
            //object o = DBContext.ExecuteScalarCmd(sql, sp, CommandType.Text);
            //string publisher = Convert.ToString(o);
            lbl_publisher.Text = publishername;
            lbl_author.Text = author;
            lbl_bookname.Text = bookname;
            lbl_course.Text = course;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup();", true);
        }


        protected void btn_savefilters_Click(object sender, EventArgs e)
        {
            List<string> selectedCategory = GetSelectedValues(cbl_category);
            List<string> selectedAuthors = GetSelectedValues(cbl_author);
            List<string> selectedPublishers = GetSelectedValues(cbl_publisher);
            List<string> selectedSubjects = GetSelectedValues(cbl_subject);
            List<string> selectedCourse = GetSelectedValues(cbl_course);
            try
            {

                string query = BuildSqlQuery(selectedCategory, selectedAuthors, selectedPublishers, selectedSubjects, selectedCourse);
                DataSet ds = DBContext.GetDataSet(query, null, CommandType.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        gridview_books.DataSource = ds;
                        gridview_books.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_BookRequest.aspx -> GetCourse()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private List<string> GetSelectedValues(CheckBoxList checkBoxList)
        {
            return checkBoxList.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Value).ToList();
        }

        private string BuildSqlQuery(List<string> category, List<string> authors, List<string> publishers, List<string> subjects, List<string> course)
        {
            string baseQuery = "SELECT DISTINCT TOP 100 A.BOOKNAME AS BOOKNAME,A.LINKEDWITH,B.PUBLISHERNAME AS PUBLISHERNAME,E.AUTHORNAME AS AUTHOR,C.COURSE_NAME AS COURSENAME,SM.SUBJECT_NAME  FROM BOOK_MASTER A INNER JOIN PUBLISHERMASTER B ON A.PUBLISHER_ID=B.P_ID INNER JOIN COURSE_MASTER C ON A.COURSE_ID=C.COURSE_ID INNER JOIN AUTHORMASTER E ON A.AUTHOR_ID=E.AUTHOR_ID INNER JOIN EG_SUBJECT_LINK EG ON A.SUBJECT_ID=EG.EG_SUB_ID INNER JOIN SUBJECT_MASTER SM ON EG.SUBJECTKEY=SM.SUBJECT_KEY INNER JOIN CATEGORYMASTER CG ON CG.CATEGORY_ID=A.CATEGORY_ID";
            List<string> filters = new List<string>();

            if (category.Count > 0 && !category.Contains("ALL"))
            {
                string categoryFilter = string.Join(",", category.Select(s => $"'{s}'"));
                filters.Add($"CG.CATEGORYNAME IN ({categoryFilter})");
            }

            if (authors.Count > 0)
            {
                string authorFilter = string.Join(",", authors.Select(a => $"'{a}'"));
                filters.Add($"E.AUTHORNAME IN ({authorFilter})");
            }

            if (publishers.Count > 0)
            {
                string publisherFilter = string.Join(",", publishers.Select(p => $"'{p}'"));
                filters.Add($"B.PUBLISHERNAME IN ({publisherFilter})");
            }

            if (subjects.Count > 0)
            {
                string subjectFilter = string.Join(",", subjects.Select(p => $"'{p}'"));
                filters.Add($"SM.SUBJECT_NAME ({subjectFilter})");
            }

            if (course.Count > 0)
            {
                string courseFilter = string.Join(",", course.Select(p => $"'{p}'"));
                filters.Add($"C.COURSE_NAME IN ({courseFilter})");
            }

            if (filters.Count > 0)
            {
                baseQuery += " WHERE " + string.Join(" AND ", filters);
            }

            return baseQuery;
        }

        void visibility()
        {
            pnl_category.Visible = false;
            pnl_course.Visible = false;
            pnl_publisher.Visible = false;
            pnl_subject.Visible = false;
            pnl_author.Visible = false;
            btn_category.BackColor = Color.White;
            btn_course.BackColor = Color.White;
            btn_publisher.BackColor = Color.White;
            btn_author.BackColor = Color.White;
            btn_subject.BackColor = Color.White;
            btn_category.ForeColor = ColorTranslator.FromHtml("#00235e");
            btn_course.ForeColor = ColorTranslator.FromHtml("#00235e");
            btn_publisher.ForeColor = ColorTranslator.FromHtml("#00235e");
            btn_author.ForeColor = ColorTranslator.FromHtml("#00235e");
            btn_subject.ForeColor = ColorTranslator.FromHtml("#00235e");

        }
        protected void btn_category_Click(object sender, EventArgs e)
        {
            visibility();
            pnl_category.Visible = true;
            btn_category.BackColor = ColorTranslator.FromHtml("#00235e");
            btn_category.ForeColor = Color.White;
        }

        protected void btn_course_Click(object sender, EventArgs e)
        {
            visibility();
            btn_course.BackColor = ColorTranslator.FromHtml("#00235e");
            btn_course.ForeColor = Color.White;
            pnl_course.Visible = true;
        }

        protected void btn_subject_Click(object sender, EventArgs e)
        {
            visibility();
            btn_subject.BackColor = ColorTranslator.FromHtml("#00235e");
            btn_subject.ForeColor = Color.White;
            pnl_subject.Visible = true;
        }

        protected void btn_publisher_Click(object sender, EventArgs e)
        {
            visibility();
            btn_publisher.BackColor = ColorTranslator.FromHtml("#00235e");
            btn_publisher.ForeColor = Color.White;
            pnl_publisher.Visible = true;
        }

        protected void btn_author_Click(object sender, EventArgs e)
        {
            visibility();
            btn_author.BackColor = ColorTranslator.FromHtml("#00235e");
            btn_author.ForeColor = Color.White;
            pnl_author.Visible = true;
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
