using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace E_Granthalaya
{
    public partial class Adm_Report : System.Web.UI.Page
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
                    btn_open.CausesValidation = false;
                    GetLoggedInUserDetails();
                    GetReportList();
                    pnl_nodata.Visible = true;
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

        protected void GetLoggedInUserDetails()
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
                Common.WriteToLog("Adm_Report.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }


        private void BindGridView(Panel pn, GridView gd, string procedure, SqlParameter[] sp)
        {
            try
            {   //BindGridView(pnl_bookscost, gridview7, "USP_REPORT_GETTOTALBOOKCOST", GetDateParameters());
                DataSet ds = DBContext.GetDataSet(procedure, sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    visibility();
                    pn.Visible = true;
                    gd.DataSource = ds;
                    gd.DataBind();
                }
                else
                {
                    pn.Visible = false;
                    pnl_nodata.Visible = true;
                    ShowToastr(this.Page, "No Records availiable ", "Error", "error");
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }

        }

        protected void ddl_report_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string from = txt_fromdate.Text;
                string to = txt_todate.Text;
                lbl_ddl.Text = ddl_report.SelectedItem.ToString();
                if ((!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to)) || ddl_report.SelectedIndex == 1)
                {
                    if (ddl_report.SelectedIndex == 1)
                    {
                        SqlParameter[] sp = new SqlParameter[]
                        {
                             new SqlParameter("@DEPT_ID", dept_id)
                        };
                        visibility();
                        BindGridView(pnl_books, gridview, "USP_REPORT_COURSEWISEDETAILS", sp);
                    }
                    else if (ddl_report.SelectedIndex == 2)
                    {
                        visibility();
                        BindGridView(pnl_bookscost, gridview7, "USP_REPORT_GETTOTALBOOKCOST", GetDateParametersWithDeptId(dept_id));
                    }
                    //else if (ddl_report.SelectedIndex == 3)
                    //{
                    //    visibility();
                    //    BindGridView(pnl_issue, gridview1, "USP_REPORT_GETTOPISSUINGSTUDENTS", GetDateParameters());
                    //}
                    else if (ddl_report.SelectedIndex == 3)
                    {
                        visibility();
                        BindGridView(pnl_return, gridview4, "USP_REPORT_GETRETURNEDSTUDENTS", GetDateParameters());
                    }
                    else if (ddl_report.SelectedIndex == 4)
                    {
                        visibility();
                        BindGridView(pnl_cancel, gridview5, "USP_REPORT_GETCANCELEDSTUDENTS", GetDateParameters());
                    }
                    else if (ddl_report.SelectedIndex == 5)
                    {
                        visibility();
                        BindGridView(pnl_fine, gridview6, "USP_REPORT_GETMAXFINESTUDENTS", GetDateParameters());
                    }
                    else if (ddl_report.SelectedIndex == 6)
                    {
                        visibility();
                        BindGridView(pnl_subs, gridview10, "USP_REPORT_SUBSDETAILS", GetDateParameters());
                    }
                    else if (ddl_report.SelectedIndex == 7)
                    {
                        visibility();
                        GetBooksByFlag(1);  // Flag 1 for issued books
                    }
                    else if (ddl_report.SelectedIndex == 8)
                    {
                        visibility();
                        GetBooksByFlag(2);  // Flag 2 for requested books
                    }
                    else if (ddl_report.SelectedIndex == 9)
                    {
                        visibility();
                        GetBooksByFlag(3);  // Flag 3 for returned books
                    }
                    else if (ddl_report.SelectedIndex == 10)
                    {
                        visibility();
                        GetBooksByFlag(4);  // Flag 4 for canceled books
                    }
                    else if (ddl_report.SelectedIndex == 11)
                    {
                        visibility();
                        GetBooksByFlag(5);  // Flag 5 for discarded books
                    }
                    else if (ddl_report.SelectedIndex == 12)
                    {
                        visibility();
                        BindGridView(pnl_finepaid, finepaid, "USP_REPORT_GETFINEPAIDSTUDENTS", GetDateParameters());
                        DataSet ds3 = DBContext.GetDataSet("select sum(fine)  as TotalFine from BOOK_REQUISITION where STATUS=8 and RETURN_DATE BETWEEN @FROM_DATE AND @TO_DATE;", GetDateParameters(), CommandType.Text);
                        if (ds3 != null && ds3.Tables[0].Rows.Count > 0)
                        {
                            string i1 = ds3.Tables[0].Rows[0]["TotalFine"].ToString();
                            lbl_f1.Text = "Total Fine Collected : " + i1 + "/-";
                        }
                        else
                        {
                            lbl_f1.Text = "Total Fine Collected : 0/- ";
                        }
                    }
                    else if (ddl_report.SelectedIndex == 13)
                    {
                        visibility();
                        BindGridView(pnl_coursecount, gridview13, "USP_REPORT_COURSEWISECOUNT", GetDateParametersWithDeptId(dept_id));
                    }
                    else if (ddl_report.SelectedIndex == 14)
                    {
                        visibility();
                        BindGridView(pnl_purchase, GridView14, "USP_REPORT_GETBOOKPURCHASEDETAILS", GetDateParametersWithBillId());
                    }
                }
                else
                {
                    visibility();
                    pnl_nodata.Visible = true;
                    ShowToastr(this.Page, "Please select From Date and To Date", "", "INFO");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_Report.aspx.cs -> ddl_report_SelectedIndexChanged()", ex.Message);
            }
        }

        void visibility()
        {
            pnl_entries.Visible = false;
            pnl_fine.Visible = false;
            //   pnl_issue.Visible = false;
            pnl_nodata.Visible = false;
            pnl_return.Visible = false;
            pnl_stream.Visible = false;
            pnl_subs.Visible = false;
            pnl_subsdate.Visible = false;
            pnl_bookscost.Visible = false;
            pnl_bookdetails.Visible = false;
            pnl_bookdetails2.Visible = false;
            pnl_books.Visible = false;
            pnl_cancel.Visible = false;
            pnl_shrink.Visible = false;
            pnl_shrink2.Visible = false;
            pnl_finepaid.Visible = false;
            pnl_details3.Visible = false;
            pnl_shrink3.Visible = false;
            pnl_discarded.Visible = false;
            pnl_coursecount.Visible = false;
            pnl_purchase.Visible = false;
        }

        void GetReportList()
        {
            try
            {
                string ReportFor = ConfigurationManager.AppSettings["ForReport"];
                string[] options = ReportFor.Split(',');
                ddl_report.Items.Insert(0, new ListItem("--Report For--", "0"));
                for (int i = 0; i < options.Length; i++)
                {
                    ddl_report.Items.Add(new ListItem(options[i], options[i]));
                }
                ddl_report.DataBind();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_Report.aspx.cs -> GetReportList()", ex.Message);
            }
        }
        private SqlParameter[] GetDateParametersWithDeptId(int deptId)
        {
            DateTime fromDate = Convert.ToDateTime(txt_fromdate.Text);
            DateTime toDate = Convert.ToDateTime(txt_todate.Text);
            return new SqlParameter[]
            {
                new SqlParameter("@FROM_DATE", fromDate),
                new SqlParameter("@TO_DATE", toDate),
                new SqlParameter("@DEPT_ID", deptId)
            };
        }

        private SqlParameter[] GetDateParameters()
        {
            DateTime fromDate = Convert.ToDateTime(txt_fromdate.Text);
            DateTime toDate = Convert.ToDateTime(txt_todate.Text);
            return new SqlParameter[]
            {
            new SqlParameter("@FROM_DATE", fromDate),
            new SqlParameter("@TO_DATE", toDate)
            };
        }
        protected void GetBooksByFlag(int flag)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FROM_DATE", DateTime.Parse(txt_fromdate.Text)),
                    new SqlParameter("@TO_DATE", DateTime.Parse(txt_todate.Text)),
                    new SqlParameter("@FLAG", flag)
                };
                BindGridView(pnl_stream, gridview11, "USP_REPORT_GETBOOKS", parameters);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_Report.aspx.cs -> GetBooksByFlag()", ex.Message);
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
            }
        }
        private SqlParameter[] GetDateParametersWithBillId()
        {
            DateTime fromDate = Convert.ToDateTime(txt_fromdate.Text);
            DateTime toDate = Convert.ToDateTime(txt_todate.Text);
            return new SqlParameter[]
            {
               new SqlParameter("@FROM_DATE", fromDate),
               new SqlParameter("@TO_DATE", toDate),
            };
        }


        protected void gridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridview, "Select$" + e.Row.RowIndex);
                e.Row.CssClass = "gridview-row";
            }
        }
        protected void gridview_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow selectedRow = gridview.SelectedRow;

            // Get the data from the selected row
            string courseName = selectedRow.Cells[1].Text;
            string totalBooks = selectedRow.Cells[2].Text;
            string issuedBooks = selectedRow.Cells[3].Text;
            string availableBooks = selectedRow.Cells[4].Text;
            string discardedBooks = selectedRow.Cells[5].Text;
            SqlParameter[] sp =
            {
                new SqlParameter("@COURSENAME",courseName)
            };
            BindGridView(pnl_bookdetails, gridview8, "USP_REPORT_GETPUBLISHERBOOKCOUNTBYSUB", sp);
            pnl_bookdetails.Visible = true;
            pnl_shrink.Visible = true;
            pnl_books.Visible = false;
        }

        protected void btn_open_Click(object sender, EventArgs e)
        {
            pnl_books.Visible = true;
            pnl_shrink.Visible = false;
            pnl_bookdetails.Visible = false;
        }

        protected void btn_open2_Click(object sender, EventArgs e)
        {

            pnl_bookscost.Visible = true;
            pnl_shrink2.Visible = false;
            pnl_bookdetails2.Visible = false;
        }

        protected void btn_open3_Click(object sender, EventArgs e)
        {
            pnl_subs.Visible = true;
            pnl_shrink3.Visible = false;
            pnl_details3.Visible = false;
        }

        protected void gridview10_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridview10, "Select$" + e.Row.RowIndex);
                e.Row.CssClass = "gridview-row";
            }
        }


        protected void gridview10_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow selectedRow = gridview10.SelectedRow;
            //int journalId = Convert.ToInt32(selectedRow.Cells[1].Text);
            string journalname = selectedRow.Cells[1].Text;
            //string periodicity = selectedRow.Cells[2].Text;
            //string category = selectedRow.Cells[3].Text;
            //string period = selectedRow.Cells[4].Text;
            //  string subdate = selectedRow.Cells[5].Text;
            SqlParameter[] sp = {
                new SqlParameter("@JOURNAL_NAME", journalname)
            };
            BindGridView(pnl_details3, gridview1, "USP_REPORT_GETSUBSCRIPTIONSENTRIES", sp);
            pnl_details3.Visible = true;
            pnl_shrink3.Visible = true;
            pnl_books.Visible = false;
        }

        protected void gridview7_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridview7, "Select$" + e.Row.RowIndex);
                e.Row.CssClass = "gridview-row";
            }
        }

        protected void gridview7_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow selectedRow = gridview7.SelectedRow;
            string courseName = selectedRow.Cells[1].Text;
            string activeCost = selectedRow.Cells[2].Text;
            string discardCost = selectedRow.Cells[3].Text;
            SqlParameter[] sp =
             {
                new SqlParameter("@COURSENAME",courseName),
                new SqlParameter("@FROM_DATE", DateTime.Parse(txt_fromdate.Text)),
                new SqlParameter("@TO_DATE", DateTime.Parse(txt_todate.Text))
            };
            BindGridView(pnl_bookdetails2, gridview9, "USP_REPORT_GETCOSTOFBOOKS", sp);
            pnl_bookscost.Visible = false;
            pnl_shrink2.Visible = true;
            pnl_bookdetails2.Visible = true;
        }
        protected void GetdiscardedBooks(int flag)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FROM_DATE", DateTime.Parse(txt_fromdate.Text)),
                    new SqlParameter("@TO_DATE", DateTime.Parse(txt_todate.Text)),
                    new SqlParameter("@FLAG", flag)
                };
                BindGridView(pnl_discarded, gridview12, "USP_REPORT_GETBOOKS", parameters);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_Report.aspx.cs -> GetBooksByFlag()", ex.Message);
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
            }
        }



        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}