using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;

namespace E_Granthalaya
{
    public partial class TIME_SLOT : System.Web.UI.Page
    {
        string name, designation, deptCode, deptName;
        static int fid, dept_id;

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
                    BindGridview();
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
                Common.WriteToLog("Adm_TimeSlot.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void BindGridview()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GETTIMESLOT_ALLRECORDS", sp, CommandType.StoredProcedure);
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

            catch (Exception ex)
            {
                Common.WriteToLog("Adm_TimeSlot.aspx.cs ->BindGridview() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void ClearEntries()
        {
            txt_from_time.Text = txt_to_time.Text = txt_limit.Text = string.Empty;
            txt_from_time.Focus();
        }

        protected void btn_save_Click1(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                 {
                      new SqlParameter("@FROM_TIME", txt_from_time.Text),
                      new SqlParameter("@TO_TIME", txt_to_time.Text),
                      new SqlParameter("@LIMIT", txt_limit.Text),
                      new SqlParameter("@FACULTY_ID", fid),

                 };
                int ds = DBContext.ExecuteNonQueryCmd("USP_ADDTIMESLOT", sp, CommandType.StoredProcedure);

                if (ds == 1)
                {
                    ShowToastr(this.Page, "Added Successfully...!", "Success", "success");
                    ClearEntries();
                    BindGridview();
                }
                else
                {
                    ShowToastr(this.Page, "Time Slot Overlap...!", "Error", "error");
                    ClearEntries();
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_TimeSlot.aspx.cs -> btn_save_Click1() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                string slot = GridView1.Rows[rowIndex].Cells[1].Text;
                SqlParameter[] sp = new SqlParameter[]
                 {
                      new SqlParameter("@TIME_DESC", slot),
                      new SqlParameter("@FACULTY_ID", fid),
                 };
                int ds = DBContext.ExecuteNonQueryCmd("USP_DELETE_TIMESLOT_RECORDS", sp, CommandType.StoredProcedure);
                if (ds < 1)
                {
                    ShowToastr(this.Page, "Time-slot deleted successfully...!", "Success", "success");
                    GridView1.EditIndex = -1;
                    BindGridview();
                }
                else
                {
                    ShowToastr(this.Page, "Something went wrong! Try after sometime.", "Error", "error");

                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_TimeSlot.aspx.cs -> GridView1_RowDeleting() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}