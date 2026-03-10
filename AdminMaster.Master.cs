using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace E_Granthalaya
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        static int id, dept_id;
        string name, designation, deptCode, deptName;
        string deptcode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }
                if (jobTitle == "Assistant Professor")
                {
                    dashbord_pnl.Visible = false;
                    AssistantProfessorPanel.Visible = true;
                    LibrarianHead.Visible = false;
                    AssLibrarianPnl.Visible = false;
                    PeonPanel.Visible = false;
                    OfficePnl.Visible = false;
                }
                else if (jobTitle == "Office")
                {
                    dashbord_pnl.Visible = false;
                    AssistantProfessorPanel.Visible = false;
                    LibrarianHead.Visible = false;
                    AssLibrarianPnl.Visible = false;
                    PeonPanel.Visible = false;
                    OfficePnl.Visible = true;
                }
                else if (jobTitle == "ASSISTANT LIBRARIAN")
                {
                    dashbord_pnl.Visible = false;
                    AssLibrarianPnl.Visible = true;
                    AssistantProfessorPanel.Visible = false;
                    LibrarianHead.Visible = false;
                    PeonPanel.Visible = false;
                    OfficePnl.Visible = false;
                }
                else if (jobTitle == "Peon")
                {
                    dashbord_pnl.Visible = false;
                    AssLibrarianPnl.Visible = false;
                    AssistantProfessorPanel.Visible = false;
                    LibrarianHead.Visible = false;
                    PeonPanel.Visible = true;
                    OfficePnl.Visible = false;
                }
                else if (jobTitle == "Librarian")
                {
                    dashbord_pnl.Visible = true;
                    LibrarianHead.Visible = true;
                    AssistantProfessorPanel.Visible = false;
                    AssLibrarianPnl.Visible = false;
                    PeonPanel.Visible = false;
                    OfficePnl.Visible = false;
                }
                else
                {
                    Response.Redirect("Unauthorized_Access.aspx");
                }
                GetLoggedInUserDetails();
                btnLogout.CausesValidation = false;
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
                                lbl_name.Text = name;
                                designation = Convert.ToString(ds.Tables[1].Rows[0]["JOB_TITLE"]);
                                lbl_designation.Text = designation;
                                deptCode = Convert.ToString(ds.Tables[1].Rows[0]["DEPT_CODE"]);
                                deptName = Convert.ToString(ds.Tables[1].Rows[0]["DEPT_NAME"]);
                                dept_id = Convert.ToInt32(ds.Tables[1].Rows[0]["DEPT_ID"]);
                                id = Convert.ToInt32(ds.Tables[1].Rows[0]["ID"]);
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
                Common.WriteToLog("Stud_Master.master -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
            // Server.Transfer("Login.aspx");
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}