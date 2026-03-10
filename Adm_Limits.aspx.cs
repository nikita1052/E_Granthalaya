using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class Book_Limit : System.Web.UI.Page
    {
        static int fid, dept_id;
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
                Common.WriteToLog("Adm_Limits.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void BindGridView()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("SELECT * FROM BOOK_LIMIT WHERE RECORD_STATUS_ID = 1", sp, CommandType.Text);
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
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
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

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                   new SqlParameter("@YEAR", GetCurrentYear()),
                   new SqlParameter("@NO_OF_DAYS", txt_Days.Text),
                   new SqlParameter("@BOOKS_LIMIT", txt_Maxbook.Text),
                   new SqlParameter("@FINE_AMOUNT ",txt_Fineamount.Text),
                   new SqlParameter("@WAITING",txt_waiting.Text),
                   new SqlParameter("@FACULTY_ID", fid),
                };

                int ds = DBContext.ExecuteNonQueryCmd("USP_INSERT_USER_LIMITS", sp, CommandType.StoredProcedure);
                if (ds == 1)
                {
                    ShowToastr(this.Page, "Successfully Saved", "Success", "Success");
                    BindGridView();
                }
                else
                {
                    ShowToastr(this.Page, "Unsuccessful", "Error", "error");
                }
                clear();
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, ex.Message, "Error", "error");
            }
        }

        private void clear()
        {
            txt_Days.Text = string.Empty;
            txt_Fineamount.Text = string.Empty;
            txt_Maxbook.Text = string.Empty;
            txt_waiting.Text = string.Empty;
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}