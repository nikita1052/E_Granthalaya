using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Drawing;

namespace E_Granthalaya
{
    public partial class Adm_BooksCount : System.Web.UI.Page
    {
        string name, designation, deptCode, deptName;
        static int fid, dept_id;
        static string jname, category, period;
        DateTime dt = DateTime.Now;

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
                    GetTotalCounts();
                    GetCurrentYear();
                    GetSubscriptionReminder();
                    btn_subsr.BackColor = Color.Green;
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
                Common.WriteToLog("Adm_Dashboard.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void GetTotalCounts()
        {
            SqlParameter[] sp = null;
            DataSet ds = DBContext.GetDataSet("SELECT COUNT(*) AS TOTAL, COUNT(DISTINCT LINKEDWITH) AS UNIQUEB FROM BOOK_MASTER", sp, CommandType.Text);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lbl_countBooks.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                lbl_countUnique.Text = ds.Tables[0].Rows[0]["UNIQUEB"].ToString();
            }
            else
            {
                lbl_countBooks.Text = "0";
                lbl_countUnique.Text = "0";
            }
            SqlParameter[] sp1 = null;
            DataSet ds1 = DBContext.GetDataSet("USP_GET_TOTALISSUED", sp1, CommandType.StoredProcedure);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                string i1 = ds1.Tables[0].Rows[0]["Total"].ToString();
                string i2 = ds1.Tables[0].Rows[0]["Male"].ToString();
                string i3 = ds1.Tables[0].Rows[0]["Female"].ToString();
                string j1 = ds1.Tables[0].Rows[1]["Total"].ToString();
                string j2 = ds1.Tables[0].Rows[1]["Male"].ToString();
                string j3 = ds1.Tables[0].Rows[1]["Female"].ToString();
                lbl_i1.Text = "All : " + i1 + "| M : " + i2 + "| F : " + i3;
                lbl_i2.Text = "All : " + j1 + "| M : " + j2 + "| F : " + j3;
            }
            else
            {
                lbl_i1.Text = "All : 0 | M : 0 | F : 0";
                lbl_i2.Text = "All : 0 | M : 0 | F : 0";
            }
            SqlParameter[] sp2 = null;
            DataSet ds2 = DBContext.GetDataSet("USP_GET_TOTALPENDINGS", sp2, CommandType.StoredProcedure);
            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            {

                string i1 = ds2.Tables[0].Rows[0]["Total"].ToString();
                string i2 = ds2.Tables[0].Rows[0]["Male"].ToString();
                string i3 = ds2.Tables[0].Rows[0]["Female"].ToString();
                string j1 = ds2.Tables[0].Rows[1]["Total"].ToString();
                string j2 = ds2.Tables[0].Rows[1]["Male"].ToString();
                string j3 = ds2.Tables[0].Rows[1]["Female"].ToString();
                lbl_p1.Text = "All : " + i1 + "| M : " + i2 + "| F : " + i3;
                lbl_p2.Text = "All : " + j1 + "| M : " + j2 + "| F : " + j3;
            }
            else
            {
                lbl_p1.Text = "All : 0 | M : 0 | F : 0";
                lbl_p2.Text = "All : 0 | M : 0 | F : 0";
            }
            SqlParameter[] sp3 = new SqlParameter[]
            {
                new SqlParameter("@FLAG",0),
                new SqlParameter("@FROM",DateTime.Now),
                new SqlParameter("@TO",DateTime.Now),
            };
            DataSet ds3 = DBContext.GetDataSet("USP_GET_TOTALFINE", sp3, CommandType.StoredProcedure);
            if (ds3 != null && ds3.Tables[0].Rows.Count > 0)
            {
                string i1 = ds3.Tables[0].Rows[0]["Total"].ToString();
                string i2 = ds3.Tables[0].Rows[0]["Male"].ToString();
                string i3 = ds3.Tables[0].Rows[0]["Female"].ToString();
                lbl_f1.Text = "All : " + i1 + "| M : " + i2 + "| F : " + i3;
            }
            else
            {
                lbl_f1.Text = "All : 0 | M : 0 | F : 0";
            }
        }

        private void GetSubscriptionReminder()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("USP_GET_REMINDER_FOR_SUBSCRIPTION", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    pnl_grid.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_subs.Visible = false;
                }
                else
                {
                    pnl_subs.Visible = false;
                    pnl_grid.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridview_subscription_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EnterRecord")
            {
                string journalName = e.CommandArgument.ToString();
                Response.Redirect("Adm_SubscriptionEntry.aspx?journal_name=" + Server.UrlEncode(journalName), false);
                Context.ApplicationInstance.CompleteRequest();

            }
        }
        protected void btnview_Click(object sender, EventArgs e)
        {
            LinkButton btnView = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;
            int rowIndex = row.RowIndex;
            string journal = gridview_subscription.Rows[rowIndex].Cells[1].Text;
            string category = gridview_subscription.Rows[rowIndex].Cells[2].Text;
            string[] perioddate = (gridview_subscription.Rows[rowIndex].Cells[3].Text).Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            string period = perioddate[0];
            string datep = perioddate[1];
            string ecount = gridview_subscription.Rows[rowIndex].Cells[4].Text;
            lbl_journal.Text = journal;
            lbl_category.Text = category;
            lbl_period.Text = period;
            lbl_datep.Text = datep;
            lbl_ecount.Text = ecount;
            DataSet ds1 = DBContext.GetDataSet("SELECT PERIODICITY FROM SUBSCRIPTION_MASTER WHERE JOURNAL_NAME='" + journal + "'", null, CommandType.Text);
            string periodicity = ds1.Tables[0].Rows[0]["PERIODICITY"].ToString();
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                    {
                      new SqlParameter("@JNAME", journal),
                      new SqlParameter("@PERIOD", periodicity),
                    };
                DataSet ds = DBContext.GetDataSet("USP_GET_SUBSCRIPTIONENTRY_DETAILS", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    pnl_entries.Visible = true;
                    GridView2.DataSource = ds;
                    GridView2.DataBind();
                    pnl_noentries.Visible = false;
                }
                else
                {
                    pnl_entries.Visible = false;
                    pnl_noentries.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup();", true);
        }

        protected void btn_subsd_Click(object sender, EventArgs e)
        {
            GetSubscriptionDetails();
            btn_subsd.BackColor = Color.Green;
            btn_subsr.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }

        protected void btn_subsr_Click(object sender, EventArgs e)
        {
            GetSubscriptionReminder();
            btn_subsr.BackColor = Color.Green;
            btn_subsd.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }

        protected void Btn_Extend_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Adm_AddSubscription.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int GetCurrentYear()
        {
            int ds = 0;
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
                Common.WriteToLog("Adm_Dashboard.aspx.cs -> GetCurrentYear()", ex.Message);
            }
            return ds;
        }

        private void GetSubscriptionDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                  new SqlParameter("@year", GetCurrentYear())
                };
                DataSet ds = DBContext.GetDataSet("USP_GET_SUBSCRIPTIONSDATA", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gridview_subscription.DataSource = ds;
                    gridview_subscription.DataBind();
                    pnl_subs.Visible = true;
                    pnl_nodata.Visible = false;
                    pnl_grid.Visible = false;
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_subs.Visible = false;
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Subscriptions Found";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}