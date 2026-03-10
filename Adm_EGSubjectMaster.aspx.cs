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
    public partial class Adm_EGSubjectMaster : System.Web.UI.Page
    {
        static int fid, dept_id;
        string name, designation, deptCode, deptName;
        string deptcode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["loginid"] = "bharathi rao@Ug";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian")
                {
                    GetLoggedInUserDetails();
                    BindGridView();
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
                Common.WriteToLog("Adm_EGSubjectMaster.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void BindGridView()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@YEAR",GetCurrentYear()),
                };
                DataSet ds = DBContext.GetDataSet("USP_GETSUBJECTS_NOTIN_EG", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    pnl_grid.Visible = true;
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
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
                Common.WriteToLog("Adm_EGSubjectMaster.aspx.cs -> BindGridView()", ex.Message);
                ShowToastr(this.Page, ex.Message, "Error", "error");
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

        protected void btn_addsubject_Click(object sender, EventArgs e)
        {
            string subject = txt_addsubject.Text;
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@SUBJECT",subject),
            };
            DBContext.ExecuteScalarCmd("USP_SAVE_NEWSUBJECTIN_MYSQLSM", sp, CommandType.StoredProcedure);
            ShowToastr(this.Page, "Successfully Added!", "Success", "Success");
            BindGridView();
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (((DropDownList)row.FindControl("ddl_select")).SelectedIndex != 0)
                {
                    total++;
                    string course = row.Cells[1].Text;
                    int sem = Convert.ToInt32(row.Cells[2].Text);
                    string subname = row.Cells[3].Text;
                    string subid = ((DropDownList)row.FindControl("ddl_select")).SelectedValue;
                    SqlParameter[] sp = new SqlParameter[]
                    {
                    new SqlParameter("@COURSE",course),
                    new SqlParameter("@SUBNAME",subname),
                    new SqlParameter("@SEM",sem),
                    new SqlParameter("@SUBID",subid),
                    new SqlParameter("@FID",fid),
                    };
                    DBContext.ExecuteScalarCmd("USP_SAVE_NEWSUBJECTIN_EG", sp, CommandType.StoredProcedure);
                }
                //    else
                //    {
                //        ShowToastr(this.Page, "Please Select a Subject to Link With" + row, "Error", "error");
                //    }
            }
            ShowToastr(this.Page, total + " Subjects added successfully to EGranthalaya!", "Success", "Success");
            BindGridView();
        }

        //protected void btn_save_Click1(object sender, EventArgs e)
        //{
        //    int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
        //    string course = GridView1.Rows[rowindex].Cells[1].Text;
        //    int sem = 2;
        //    string subname = GridView1.Rows[rowindex].Cells[3].Text;
        //    string subid = ((DropDownList)GridView1.Rows[rowindex].Cells[1].FindControl("ddl_select")).SelectedValue;
        //    SqlParameter[] sp = new SqlParameter[]
        //    {
        //        new SqlParameter("@COURSE",course),
        //        new SqlParameter("@SUBNAME",subname),
        //        new SqlParameter("@SEM",sem),
        //        new SqlParameter("@SUBID",subid),
        //        new SqlParameter("@FID",fid),
        //    };
        //    DBContext.ExecuteScalarCmd("USP_SAVE_NEWSUBJECTIN_EG", sp, CommandType.StoredProcedure);
        //    ShowToastr(this.Page, "Successfully Added The Subject !", "Success", "Success");
        //    BindGridView();
        //}

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlsubject = e.Row.FindControl("ddl_select") as DropDownList;
                if (ddlsubject != null)
                {
                    SqlParameter[] sp = null;
                    DataSet ds = DBContext.GetDataSet("SELECT * FROM MYSQL_SM WHERE SUBJECT_NAME!='' ORDER BY SUBJECT_NAME", sp, CommandType.Text);
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "Subject_Name";
                    ddlsubject.DataValueField = "Subject_Id";
                    ddlsubject.DataBind();
                    ddlsubject.Items.Insert(0, new ListItem("--Select Subject--", "0"));
                }
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }

}