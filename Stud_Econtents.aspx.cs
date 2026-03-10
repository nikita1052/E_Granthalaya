using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class Stud_Econtents : System.Web.UI.Page
    {
        static int id, dept_id, grno;
        string name, designation, deptCode, deptName, sql;

        void Page_Load(object sender, EventArgs e)
        {  
            if (!IsPostBack)
            {
                Session["loginid"] = "IF22085@UG";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian" || jobTitle == "Clerk" || jobTitle == "Peon" || jobTitle == "Assistant Professor")
                {
                    Response.Redirect("Unauthorized_Access.aspx");
                }
                else
                {
                    GetLoggedInUserDetails();
                    FillCourses();
                    FillSubjects();
                    FillContents();
                    BindGrid();
                    pnl_nodata.Visible = true;
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
                Common.WriteToLog("Stud_EContents.aspx -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error  -> " + ex.Message, "Error", "error");
            }
        }

        protected void FillCourses()
        {
            try
            {
                //DROPDOWN BINDING FOR COURSE
                SqlParameter[] sp = new SqlParameter[]
                {
                     new SqlParameter("@SID",id),
                };
                DataSet ds = DBContext.GetDataSet("USP_GETCOURSES_FOR_STUDENTCONTENTS", sp, CommandType.StoredProcedure);
                ddl_course.DataSource = ds;
                ddl_course.DataTextField = "COURSE_NAME";
                ddl_course.DataValueField = "COURSE_ID";
                ddl_course.DataBind();
                ddl_subject.SelectedIndex = 0;
                ddl_contenttype.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Contents.aspx.cs -> FillCourses() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error  : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void FillSubjects()
        {
            try
            {
                //DROPDOWN BINDING FOR SUBJECTS
                SqlParameter[] sp = new SqlParameter[]
                {
                     new SqlParameter("@SID",id),
                     new SqlParameter("@COURSE_ID",Convert.ToInt32(ddl_course.SelectedValue)),
                };
                DataSet ds = DBContext.GetDataSet("USP_GETSUBJECTS_FOR_STUDENTCONTENTS", sp, CommandType.StoredProcedure);
                ddl_subject.DataSource = ds;
                ddl_subject.DataTextField = "SUBJECTNAME";
                ddl_subject.DataValueField = "SUBJECT_KEY";
                ddl_subject.DataBind();
                ddl_contenttype.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Contents.aspx.cs -> FillSubjects() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error  : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void ddl_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void FillContents()
        {
            try
            {
                //DROPDOWN BINDING FOR CONTENT TYPE
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GETCONTENT_FOR_STUDENTCONTENTS", sp, CommandType.StoredProcedure);
                ddl_contenttype.DataSource = ds;
                ddl_contenttype.DataTextField = "CONTENT_TYPE";
                ddl_contenttype.DataValueField = "CONTENT_ID";
                ddl_contenttype.DataBind();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Contents.aspx.cs -> FillContents() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error  : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void ddl_subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddl_contenttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btn_view_Click(object sender, EventArgs e)
        {
            int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex; //taking index
            string path = GridView1.Rows[rowindex].Cells[5].Text; //copying value
            string doc = Server.MapPath("~/Uploads/") + path;
            try
            {
                Process.Start(doc); //opening
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error"); //lol
            }
        }

        protected void BindGrid()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]
                {
                       new SqlParameter("@COURSE_ID",Convert.ToInt32(ddl_course.SelectedValue)),
                       new SqlParameter("@SUBJECT_ID",ddl_subject.SelectedValue),
                       new SqlParameter("@CONTENT_ID",Convert.ToInt32(ddl_contenttype.SelectedValue)),
                };
                DataSet ds1 = DBContext.GetDataSet("USP_GETECONTENTS_FOR_STUDENT", sp1, CommandType.StoredProcedure);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds1;
                    GridView1.DataBind();
                    pnl_grid.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "Sorry, No Data Found!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Contents.aspx.cs -> BindGrid() -> ", ex.Message);
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}