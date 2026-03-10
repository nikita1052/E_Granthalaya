using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Reflection.Emit;
using System.Drawing;

namespace E_Granthalaya
{
    public partial class Adm_BookReturn : System.Web.UI.Page
    {
        static int fid, dept_id;
        string name, designation, deptCode, deptName;
        string deptcode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["loginid"] = "S. SAI SREE@UG";
                 Session["loginid"] = "bharathi rao@ug";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian" || jobTitle == "ASSISTANT LIBRARIAN" || jobTitle == "Peon")
                {
                    GetLoggedInUserDetails();
                    txt_date.Text = DateTime.Now.ToString("dd-MM-yyy");
                    pnl_facrtrn.Visible = false;
                    pnl_return.Visible = true;
                    ReturnDetailSGV(1);
                    btn_studreturn.BackColor = Color.Green;
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
                Common.WriteToLog("Adm_BookReturn.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }
        protected void ReturnDetailSGV(int flag)
        {
            try
            {
                SqlParameter[] sp =
                {
                new SqlParameter("@FLAG", flag)
                };
                DataSet dt = DBContext.GetDataSet("USP_GETRETURNBOOKSLIST", sp, CommandType.StoredProcedure);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    if (flag == 0) // Teacher data
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        pnl_facrtrn.Visible = true;
                        pnl_return.Visible = false;
                        pnl_nodata.Visible = false;
                    }
                    else if (flag == 1) // Student data
                    {
                        YourGridView.DataSource = dt;
                        YourGridView.DataBind();
                        pnl_facrtrn.Visible = false;
                        pnl_return.Visible = true;
                        pnl_nodata.Visible = false;
                    }
                }
                else
                {
                    if (flag == 0)
                    {
                        pnl_facrtrn.Visible = false;
                    }
                    else if (flag == 1)
                    {
                        pnl_return.Visible = false;
                    }
                    pnl_nodata.Visible = true;
                    lbl_not.Text = "No Entries Found For Return";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookReturn.aspx -> GVBind()", ex.Message);
                ShowToastr(this, "Some error occurred while fetching return books list.", "Error", "error");
            }
        }


        public Boolean IsHoliday(DateTime Date)
        {
            string sql = "SELECT COUNT(*) FROM HOLIDAY_MASTER WHERE H_DATE = @DATE";
            SqlParameter[] sp = { new SqlParameter("@date", Date) };
            int count = DBContext.GetCount(sql, sp, CommandType.Text);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long CalculateFine(DateTime current_returndate, DateTime estimated_returndate)
        {
            try
            {
                long fine = 0;
                int daysDifference = (int)(current_returndate - estimated_returndate).TotalDays;

                int workingDays = 0;
                DateTime currentDate = estimated_returndate;

                for (int i = 0; i <= daysDifference; i++)
                {
                    if (currentDate.DayOfWeek != DayOfWeek.Sunday && !IsHoliday(currentDate))
                    {
                        workingDays++;
                    }

                    currentDate = currentDate.AddDays(1);
                }

                fine = workingDays * 2;

                return fine;
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookReturn.aspx.cs -> CalculateFine()", ex.Message);
                throw;
            }
        }
        protected void btn_facreturn_Click(object sender, EventArgs e)
        {
            pnl_return.Visible = false;
            pnl_facrtrn.Visible = true;
            ReturnDetailSGV(0);
            btn_facreturn.BackColor = Color.Green;
            btn_studreturn.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }
        protected void btn_studreturn_Click(object sender, EventArgs e)
        {
            pnl_facrtrn.Visible = false;
            pnl_return.Visible = true;
            ReturnDetailSGV(1);
            btn_studreturn.BackColor = Color.Green;
            btn_facreturn.BackColor = ColorTranslator.FromHtml("#2e2d2d");
        }

        protected void btnreturn1_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnreturn1 = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnreturn1.NamingContainer;
                int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                long bookID = Convert.ToInt32(YourGridView.Rows[rowindex].Cells[4].Text) ; // Assuming "BOOK_ID" is your DataKeyName
                int brID = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Values["BR_ID"]);
                DateTime returnDate;
                if (!DateTime.TryParse(txt_date.Text, out returnDate))
                {
                    throw new ArgumentException("Invalid return date format");
                }
                {
                    SqlParameter[] param =
                    {
                         new SqlParameter("@RETURN_DATE", Convert.ToDateTime(txt_date.Text)),
                         new SqlParameter("@FACULTY_ID", fid),
                         new SqlParameter("@BOOKID", bookID),
                         new SqlParameter("@BRID", brID),
                    };
                    DataSet dr = DBContext.GetDataSet("USP_UPDATEBOOKRETURN_FAC", param, CommandType.StoredProcedure);
                    ReturnDetailSGV(0);
                    ShowToastr(this.Page, "Returned Successfully", "Success", "Success");
                    //DBContext.SendEmailNotification("vanimesh2004@gmail.com", "Book Returned Successfully", "Confirmation Mail - Book Returning");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookReturn -> btnreturn_Click", ex.Message);
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
            }
        }


        protected void btnview_Click(object sender, EventArgs e)
        {
            LinkButton btnView = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;

            int rowIndex = row.RowIndex;
            int BR_ID = Convert.ToInt32(YourGridView.DataKeys[rowIndex].Value);

            string rollname = YourGridView.Rows[rowIndex].Cells[2].Text;
            string bookname = YourGridView.Rows[rowIndex].Cells[3].Text;
            int bookid = Convert.ToInt32(YourGridView.Rows[rowIndex].Cells[4].Text);

            string sql = "SELECT (PUBLISHERNAME) FROM BOOK_MASTER B INNER JOIN PUBLISHERMASTER P ON B.PUBLISHER_ID=P.P_ID  WHERE BOOK_ID=@BOOKID GROUP BY PUBLISHERNAME";
            SqlParameter[] sp = { new SqlParameter("@BOOKID", bookid) };
            object o = DBContext.ExecuteScalarCmd(sql, sp, CommandType.Text);

            string sql1 = "SELECT DUE_DATE FROM BOOK_REQUISITION WHERE BR_ID=@BR_ID";
            SqlParameter[] sp1 = { new SqlParameter("@BR_ID", BR_ID) };

            DateTime Duedate = Convert.ToDateTime(DBContext.ExecuteScalarCmd(sql1, sp1, CommandType.Text));
            string publisher = Convert.ToString(o);

            lbl_publisher.Text = publisher;
            lbl_brid.Text = BR_ID.ToString();
            lblbookname.Text = bookname.ToString();
            lblbookid.Text = bookid.ToString();
            lblbookid.Visible = false;
            Label2.Visible = false;
            lbl_duedate.Text = Duedate.ToString("d MMMM yyyy");
            lblrollno.Text = rollname;

            sql = "SELECT DUE_DATE FROM BOOK_REQUISITION WHERE BOOK_ID=" + Convert.ToInt32(lblbookid.Text) + " AND STATUS=7";
            DateTime es_returndate = DBContext.GetDate(sql, null, CommandType.Text);
            lblfine.Text = CalculateFine(Convert.ToDateTime(txt_date.Text), es_returndate).ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "ShowPopup();", true);
        }
        protected void btnreturn_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
               // int bid = Convert.ToInt32(GridView1.Rows[rowindex].Cells[1].Text);
                long bookID = Convert.ToInt32(YourGridView.Rows[rowindex].Cells[4].Text); //Convert.ToInt64(lblbookid.Text);
                long fine = Convert.ToInt32(YourGridView.Rows[rowindex].Cells[5].Text);
                int brid = Convert.ToInt32(YourGridView.Rows[rowindex].Cells[1].Text);
                {
                    SqlParameter[] param =
                    {
                         new SqlParameter("@RETURN_DATE", Convert.ToDateTime(txt_date.Text)),
                         new SqlParameter("@FACULTY_ID", fid),
                         new SqlParameter("@BOOKID", bookID),
                         new SqlParameter("@BRID", brid),
                         new SqlParameter("@FINE",fine)
                    };
                    DataSet dr = DBContext.GetDataSet("USP_UPDATEBOOKRETURN", param, CommandType.StoredProcedure);
                    ReturnDetailSGV(1);
                    ShowToastr(this.Page, "Returned Successfully", "Success", "Success");
                    //DBContext.SendEmailNotification("vanimesh2004@gmail.com", "Book Returned Successfully", "Confirmation Mail - Book Returning");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "closeModal();", true);
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_BookReturn -> btnreturn_Click", ex.Message);
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
            }
        }

        protected void YourGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = YourGridView.SelectedIndex;
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }

    }
}