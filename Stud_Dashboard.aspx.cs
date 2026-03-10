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
    public partial class Stud_Dashboard : System.Web.UI.Page
    {
        static int id, dept_id, grno;
        string name, designation, deptCode, deptName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["loginid"] = "IF22086@UG";
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
                    GetStudentHistory();
                    GetRequestDetails();
                    btn_request.BackColor = Color.Green;
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
                Common.WriteToLog("Stud_Dashboard.aspx -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured : " + ex.Message, "Error", "error");
            }
        }
        protected void GetStudentHistory()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                  {
                      new SqlParameter("@sid", id),
                      new SqlParameter("@flag", 5),
                      new SqlParameter("@bid", 0),
                  };
                DataSet ds = DBContext.GetDataSet("USP_STUDENT_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string i1 = ds.Tables[0].Rows[0]["ISSUE"].ToString();
                    string i2 = ds.Tables[0].Rows[0]["REQUES"].ToString();
                    string i3 = ds.Tables[0].Rows[0]["CANCEL"].ToString();
                    string i4 = ds.Tables[0].Rows[0]["BOOK_RETURN"].ToString();
                    string i5 = ds.Tables[0].Rows[0]["FINE"].ToString();
                    lbl_book.Text = "Issued : " + i1 + " |  requested : " + i2;
                    lbl_history.Text = "returned : " + i4 + " | cancelled : " + i3;
                    lbl_count.Text = "paid : ₹ " + i5;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Dashboard.aspx -> GetStudentHistory() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void btn_issue_Click(object sender, EventArgs e)
        {
            btn_issue.BackColor = Color.Green;
            GetIssueDetails();
            pnl_bookhistory.Visible = false;
            pnl_bookrequest.Visible = false;
            btn_request.BackColor = ColorTranslator.FromHtml("#250063");
            btn_history.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void btn_request_Click(object sender, EventArgs e)
        {
            btn_request.BackColor = Color.Green;
            GetRequestDetails();
            pnl_bookissue.Visible = false;
            pnl_bookhistory.Visible = false;
            btn_issue.BackColor = ColorTranslator.FromHtml("#250063");
            btn_history.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void btn_history_Click(object sender, EventArgs e)
        {
            btn_history.BackColor = Color.Green;
            GetHistory();
            pnl_bookissue.Visible = false;
            pnl_bookrequest.Visible = false;
            btn_request.BackColor = ColorTranslator.FromHtml("#250063");
            btn_issue.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void GetIssueDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@sid", id),
                    new SqlParameter("@flag", 1),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_STUDENT_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView2.DataSource = ds;
                    GridView2.DataBind();
                    pnl_bookissue.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "Issue Details, No Records Found!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Dashboard.aspx ->issuebook() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured issuebook()->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void lnkdelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                int bid = Convert.ToInt32(GridView1.Rows[rowindex].Cells[1].Text);
                SqlParameter[] sp = new SqlParameter[]
                 {
                      new SqlParameter("@sid", id),
                      new SqlParameter("@flag", 4),
                      new SqlParameter("@bid", bid),
                 };
                int ds = DBContext.ExecuteNonQueryCmd("USP_STUDENT_PROFILE", sp, CommandType.StoredProcedure);
                if (ds < 1)
                {
                    ShowToastr(this.Page, "Successfully Cancelled Your Request", "Success", "Success");
                }
                else
                {
                    ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Dashboard.aspx -> lnkdelete_Click()-> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
            finally
            {
                pnl_bookrequest.Visible = false;
                GetRequestDetails(); 
                GetRequestDetails();
                GetStudentHistory();
            }
        }

        protected void GetRequestDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@sid", id),
                    new SqlParameter("@flag", 2),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_STUDENT_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    pnl_bookrequest.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "Request Details, No Records Found !";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Dashboard.aspx -> lnkdelete_Click() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured: " + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        protected void GetHistory()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@sid", id),
                    new SqlParameter("@flag", 3),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_STUDENT_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView3.DataSource = ds;
                    GridView3.DataBind();
                    pnl_bookhistory.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "User History, No Records Found!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Stud_Dashboard.aspx -> GetHistory() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}