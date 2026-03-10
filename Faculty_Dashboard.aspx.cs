using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;

namespace E_Granthalaya
{
    public partial class Faculty_Dashboard : System.Web.UI.Page
    {
        static int fid, dept_id, grno;
        string name, designation, deptCode, deptName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["loginid"] = "bharathi rao@ug";
                Session["loginid"] = "S. SAI SREE@UG";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Assistant Professor" || jobTitle == "Office")
                {
                    GetLoggedInUserDetails();
                    GetCount();
                    GetRequestDetails();
                    btn_request.BackColor = Color.Green;
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
                                //grno = Convert.ToInt32(ds.Tables[1].Rows[0]["GRNO"]);
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
                Common.WriteToLog("Fac_Dashboard.aspx -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured : " + ex.Message, "Error", "error");
            }
        }
        protected void GetCount()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                  {
                      new SqlParameter("@fid", fid),
                      new SqlParameter("@flag", 6),
                      new SqlParameter("@bid", 0),
                  };
                DataSet ds = DBContext.GetDataSet("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string i1 = ds.Tables[0].Rows[0]["ISSUE"].ToString();
                    string i2 = ds.Tables[0].Rows[0]["REQUES"].ToString();
                    string i3 = ds.Tables[0].Rows[0]["CANCEL"].ToString();
                    string i4 = ds.Tables[0].Rows[0]["BOOK_RETURN"].ToString();
                    string i5 = ds.Tables[0].Rows[0]["COUNT"].ToString();
                    lbl_book.Text = "Issued : " + i1 + " |  requested : " + i2;
                    lbl_history.Text = "returned : " + i4 + " | cancelled : " + i3;
                    lbl_count.Text = "Uploaded : " + i5;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx -> GetStudentHistory() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void btn_issue_Click(object sender, EventArgs e)
        {
            btn_issue.BackColor = Color.Green;
            GetIssueDetails();
            pnl_count.Visible = false;
            pnl_bookrequest.Visible = false;
            btn_request.BackColor = ColorTranslator.FromHtml("#250063");
            btn_count.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void btn_request_Click(object sender, EventArgs e)
        {
            btn_request.BackColor = Color.Green;
            GetRequestDetails();
            pnl_bookissue.Visible = false;
            pnl_count.Visible = false;
            pnl_bookhistory.Visible = false;
            btn_issue.BackColor = ColorTranslator.FromHtml("#250063");
            btn_count.BackColor = ColorTranslator.FromHtml("#250063");
            btn_history.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void btn_count_Click(object sender, EventArgs e)
        {
            btn_count.BackColor = Color.Green;
            GetContents();
            pnl_bookissue.Visible = false;
            pnl_bookrequest.Visible = false;
            pnl_bookhistory.Visible = false;
            btn_request.BackColor = ColorTranslator.FromHtml("#250063");
            btn_issue.BackColor = ColorTranslator.FromHtml("#250063");
            btn_history.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void GetIssueDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@fid", fid),
                    new SqlParameter("@flag", 1),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView2.DataSource = ds;
                    GridView2.DataBind();
                    pnl_bookissue.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_bookhistory.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_count.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "Issue Details, No Records Found!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx ->issuebook() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured issuebook()->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        protected void GetHistoryDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@fid", fid),
                    new SqlParameter("@flag", 5),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView4.DataSource = ds;
                    GridView4.DataBind();
                    pnl_bookhistory.Visible = true;
                    pnl_bookissue.Visible = false;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_count.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "History Details, No Records Found!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx ->issuebook() -> ", ex.Message);
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
                      new SqlParameter("@fid", fid),
                      new SqlParameter("@flag", 3),
                      new SqlParameter("@bid", bid),
                 };
                int ds = DBContext.ExecuteNonQueryCmd("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds < 1)
                {
                    ShowToastr(this.Page, "Successfully Cancelled Your Request", "Success", "Success");
                    GridView1.EditIndex = -1;
                }
                else
                {
                    ShowToastr(this.Page, "Something Went Wrong", "Error", "error");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx -> lnkdelete_Click()-> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
            finally
            {
                GetRequestDetails();
                GetCount();
            }
        }

        protected void btn_history_Click(object sender, EventArgs e)
        {
            btn_history.BackColor = Color.Green;
            GetHistoryDetails();
            pnl_bookissue.Visible = false;
            pnl_bookrequest.Visible = false;
            pnl_count.Visible = false;
            btn_request.BackColor = ColorTranslator.FromHtml("#250063");
            btn_issue.BackColor = ColorTranslator.FromHtml("#250063");
            btn_count.BackColor = ColorTranslator.FromHtml("#250063");
        }

        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteContent")
            {
                try
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    string doc = GridView3.Rows[RowIndex].Cells[5].Text;
                    string doc1 = Convert.ToString(Server.MapPath("~/Uploads/"));
                    string file = doc1 + doc;
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@FID", fid),
                      new SqlParameter("@DOC", file),
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("UPDATE ECONTENT_MASTER SET RECORD_STATUS_ID=0, MODIFIED_BY=@FID, MODIFIED_ON=GETDATE() WHERE DOCUMENT_PATH=@DOC", sp, CommandType.Text);
                    GetContents();
                    GetCount();
                    try
                    {
                        string doc2 = Convert.ToString(Server.MapPath("~/Uploads/") + doc);
                        File.Delete(doc2);
                    }
                    catch (Exception ex)
                    {
                        ShowToastr(this.Page, ex.Message, "Error", "error");
                    }
                    ShowToastr(this.Page, "Successfully Deleted From Folder", "Success", "Success");
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Faculty_Dashboard.aspx.cs -> GridView3_RowCommand()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
            }
        }

        protected void GetRequestDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@fid", fid),
                    new SqlParameter("@flag", 2),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    pnl_bookrequest.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_bookhistory.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_count.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "Request Details, No Records Found !";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx -> lnkdelete_Click() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured: " + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        protected string GetImageURL(object FileName)
        {
            string Folder = "~/Uploads/";
            //string Folder = "D:\\Uploads_New\\";
            string Default = "~/assets/images/defaultbook.jpg";
            if (FileName == null || string.IsNullOrEmpty(FileName.ToString()))
            {
                return Default;
            }
            string FilePath = Folder + FileName.ToString();
            string Url = Server.MapPath(FilePath);
            //string Url = FilePath;
            if (!System.IO.File.Exists(Url))
            {
                return Default;
            }
            return FilePath;
        }

        protected void GetContents()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@fid", fid),
                    new SqlParameter("@flag", 4),
                    new SqlParameter("@bid", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_FACULTY_PROFILE", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    GridView3.DataSource = ds;
                    GridView3.DataBind();
                    pnl_count.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_bookhistory.Visible = false;
                }
                else
                {
                    pnl_bookissue.Visible = false;
                    pnl_bookrequest.Visible = false;
                    pnl_bookhistory.Visible = false;
                    pnl_count.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Contents Yet Uploaded!";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Fac_Dashboard.aspx -> GetHistory() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured : " + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}