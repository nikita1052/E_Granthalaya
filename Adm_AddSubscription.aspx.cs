using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Policy;
using System.Collections;
using System.Drawing.Imaging;
using System.Diagnostics.Contracts;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Xml.Linq;

namespace E_Granthalaya
{
    public partial class Adm_AddSubscription : System.Web.UI.Page
    {
        static int fid, dept_id, year_id, j_entry;
        string name, designation, deptCode, deptName, sql;
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
                    fil_all_ddl1();
                    GetPeriodicity();
                    YourGridView.AutoGenerateColumns = false;
                    GetSubscriptionDetailsGV();
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
                Common.WriteToLog("Adm_AddSubscription.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        protected void fil_all_ddl1()
        {
            //SqlConnection con = DBContext.GetConnection();
            try
            {
                DataSet ds = DBContext.GetDataSet("USP_GET_JOURNAL_CATEGORY_DATA", null, CommandType.StoredProcedure);

                if (ds.Tables.Count >= 2)
                {
                    PopulateDropDown(ddl_journal_name, ds.Tables[0], "journal_name", "journal_id");
                    PopulateDropDown(ddl_category_name, ds.Tables[1], "CategoryName", "Category_id");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddSubscription.cs -> fil_all_ddl1()", ex.Message);
            }
        }

        private void GetPeriodicity()
        {
            ddl_periodicity.Items.Clear();
            ddl_periodicity.Items.Insert(0, new ListItem("--Select--", "-1"));
            string[] periodicities = { "Daily", "Weekly", "Fortnightly", "Monthly", "Quarterly", "Half Yearly", "Yearly" };
            for (int i = 0; i < periodicities.Length; i++)
            {
                ddl_periodicity.Items.Insert(i + 1, new ListItem(periodicities[i], i.ToString()));
            }
        }

        protected void PopulateDropDown(DropDownList ddl, DataTable dt, string textField, string valueField)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = textField;
                    ddl.DataValueField = valueField;
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddSubscription.cs -> PopulateDropDown()", ex.Message);
            }
        }
        protected void GetSubscriptionDetailsGV()
        {
            try
            {
                SqlParameter[] param = { new SqlParameter("@CATEGORYCODE", ddl_category_name.SelectedValue) };
                DataSet dt = DBContext.GetDataSet("USP_GET_SUBSCRIPTIONS", param, CommandType.StoredProcedure);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    YourGridView.DataSource = dt;
                    YourGridView.DataBind();
                    PNL_GRID.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    PNL_GRID.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddSubscription.cs -> GetSubscriptionDetailsGV()", ex.Message);
                return;
            }
        }

        public int BindYear()
        {
            int ds = 0;
            try
            {
                string sql;
                DateTime dt = DateTime.Now;
                if (dt.Month <= 5)
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID = (YEAR(GETDATE())-1)";
                }
                else
                {
                    sql = "SELECT YEAR_ID FROM ACADEMIC_YEAR_MASTER WHERE YEAR_ID = YEAR(GETDATE())";
                }

                ds = DBContext.GetCount(sql, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddSubcription.aspx.cs -> BindYear()", ex.Message);
            }
            return ds;
        }

        public void clear()
        {
            ddl_journal_name.SelectedIndex = -1;
            //txt_sub_id.Text = "";
            txt_sub_no.Text = "";
            txt_address.InnerText = "";
            txt_tel_no.Text = "";
            txt_mob_no.Text = "";
            txt_email.Text = "";
            txt_sub_from.Text = "";
            txt_sub_to.Text = "";
            txt_cont_per.Text = "";
            ddl_category_name.SelectedIndex = -1;
            ddl_periodicity.SelectedIndex = -1;
            txt_amount.Text = "";
            txt_trans_id.Text = "";
            txt_trans_date.Text = "";
        }

        Boolean Isvalid()
        {
            string Sub_Date_from = txt_sub_from.Text;
            string Sub_Date_To = txt_sub_to.Text;
            if (string.IsNullOrEmpty(Sub_Date_To) || string.IsNullOrEmpty(Sub_Date_from) || ddl_journal_name.SelectedIndex == 0 || ddl_periodicity.SelectedIndex == 0 || ddl_category_name.SelectedIndex == 0)
            {
                return false;
            }
            else
            {
                return true;

            }
        }

        void count(int flag)
        {
            if (!string.IsNullOrEmpty(txt_sub_from.Text) && !string.IsNullOrEmpty(txt_sub_to.Text))
            {
                DateTime from = Convert.ToDateTime(txt_sub_from.Text);
                DateTime to = Convert.ToDateTime(txt_sub_to.Text);
                SqlParameter[] sp = {
                new SqlParameter("@FLAG", flag),
                new SqlParameter("@FROM", from),
                new SqlParameter("@TO", to),
                };
                DataSet ds = DBContext.GetDataSet("USP_PERIODICITY_COUNT", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables.Count > 0)
                {
                    j_entry = Convert.ToInt32(ds.Tables[0].Rows[0]["ENTRYCOUNT"]);
                    ShowToastr(this.Page, j_entry + " entries would be made accross the date selected", "Note :", "info");
                }
            }
            else
            {
                ShowToastr(this.Page, "Please select the from date and to date", "Error", "error");
            }
        }

        protected void ddl_periodicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_sub_from.Text) && !string.IsNullOrEmpty(txt_sub_to.Text))
            {
                if (ddl_periodicity.SelectedIndex != 0)
                {
                    if (ddl_periodicity.SelectedItem.Text == "Daily")
                    {
                        count(1);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Weekly")
                    {
                        count(2);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Fortnightly")
                    {
                        count(3);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Monthly")
                    {
                        count(4);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Quarterly")
                    {
                        count(5);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Half Yearly")
                    {
                        count(6);
                    }
                    else if (ddl_periodicity.SelectedItem.Text == "Yearly")
                    {
                        count(7);
                    }
                }
                else
                {
                    ShowToastr(this.Page, "Please select the periodicity", "Error", "error");
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            int JournalID = Convert.ToInt32(ddl_journal_name.SelectedValue);
            string JournalName = ddl_journal_name.SelectedItem.Text;
            //string Sub_Id = txt_sub_id.Text;
            string Sub_no = txt_sub_no.Text;
            string Address = txt_address.InnerText;
            string Tel_No = txt_tel_no.Text;
            string Mob_No = txt_mob_no.Text;
            string Email = txt_email.Text;
            string Sub_Date_from = txt_sub_from.Text;
            string Sub_Date_To = txt_sub_to.Text;
            string Cont_Person = txt_cont_per.Text;
            string Periodicity = ddl_periodicity.SelectedItem.Text;
            string Amount = txt_amount.Text;
            string Transaction_Id = txt_trans_id.Text;
            string Transaction_date = txt_trans_date.Text;
            year_id = BindYear();

            if (!string.IsNullOrEmpty(Sub_Date_To) || !string.IsNullOrEmpty(Sub_Date_from) || ddl_journal_name.SelectedIndex != 0 || ddl_periodicity.SelectedIndex != 0 || ddl_category_name.SelectedIndex != 0)
            {
                if (Convert.ToDateTime(Sub_Date_To) <= Convert.ToDateTime(Sub_Date_from))
                {
                    ShowToastr(this.Page, "Subscription To-Date must be greater than From-Date.", "Error", "error");
                    return;
                }
            }
            else
            {
                ShowToastr(this.Page, "Please enter both Subscription From-Date and To-Date.", "Error", "error");
                return;
            }
            if (!Regex.IsMatch(Mob_No, "^[1-9][0-9]{9}$"))
            {
                ShowToastr(this.Page, "Invalid Mobile number", "Error", "error");
                return;
            }
            else
            {
                try
                {
                    sql = "SELECT COUNT(*) FROM SUBSCRIPTION_MASTER WHERE JOURNAL_NAME = @JOURNALNAME AND PERIODICITY = @PERIODICITY";
                    SqlParameter[] checkParams = { new SqlParameter("@JOURNALNAME", JournalName), new SqlParameter("@PERIODICITY", Periodicity) };
                    int count = DBContext.GetCount(sql, checkParams, CommandType.Text);

                    if (count > 0)
                    {
                        ShowToastr(this.Page, "A subscription with the same Journal Name and Periodicity already exists", "Error", "error");
                        return;
                    }
                    else
                       if (Isvalid())
                    {
                        SqlParameter[] sp =
                        {
                              new SqlParameter("@JournalID", JournalID),
                              new SqlParameter("@JournalName", JournalName),
                              new SqlParameter("@CategoryCode", ddl_category_name.SelectedValue),
                             // new SqlParameter("@Sub_Id", Convert.ToInt32(Sub_Id)),
                              new SqlParameter("@Sub_no", (Sub_no)),
                              new SqlParameter("@Address", Address),
                              new SqlParameter("@Tel_No", Tel_No),
                              new SqlParameter("@Mob_No", Mob_No),
                              new SqlParameter("@Email", Email),
                              new SqlParameter("@Sub_Date_from", Convert.ToDateTime(Sub_Date_from)),
                              new SqlParameter("@Sub_Date_To", Convert.ToDateTime(Sub_Date_To)),
                              new SqlParameter("@Cont_Person", Cont_Person),
                              new SqlParameter("@Periodicity", Periodicity),
                              new SqlParameter("@Amount", Convert.ToDecimal(Amount)),
                              new SqlParameter("@Transaction_Id", Transaction_Id),
                              new SqlParameter("@Transaction_date", Transaction_date),
                              new SqlParameter("@Faculty_Id", fid),
                              new SqlParameter("@Entry", j_entry),
                              new SqlParameter("@year_id",year_id)
                            };

                        IDataReader dr1 = DBContext.GetDataReader("USP_INSERTSUBSCRIPTION", sp, CommandType.StoredProcedure);
                        ShowToastr(this.Page, "Added successfully", "Success", "success");
                        GetSubscriptionDetailsGV();
                        clear();
                    }
                    else
                    {
                        ShowToastr(this.Page, "Kindly Fill all the Boxes", "Error", "error");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddSubscription.cs -> btn_save_Click()", ex.Message);
                    return;
                }
            }
        }

        protected void btn_journal_name_Click(object sender, EventArgs e)
        {
            string journalName = txt_add_journal.Text.Trim();
            if (!string.IsNullOrEmpty(journalName))
            {
                try
                {
                    sql = "SELECT COUNT(*) FROM JOURNAL_MASTER WHERE JOURNAL_NAME = @JOURNALNAME";
                    SqlParameter[] param = { new SqlParameter("@JOURNALNAME", journalName) };
                    int count = (int)DBContext.GetCount(sql, param, CommandType.Text);

                    if (count == 0)
                    {
                        SqlParameter[] sp = { new SqlParameter("@JOURNALNAME", journalName), new SqlParameter("@FACULTY_ID", 153) };
                        IDataReader dr1 = DBContext.GetDataReader("USP_INSERTJOURNALNAME", sp, CommandType.StoredProcedure);
                        ShowToastr(this.Page, "Journal Name Added Successfully.", "Success", "Success");
                        fil_all_ddl1();
                        txt_add_journal.Text = "";
                    }
                    else
                    {
                        ShowToastr(this.Page, "Some Error Occured -> Journal name already exists.", "Error", "error");
                    }

                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddSubscription.cs -> btn_journal_name_Click()", ex.Message);
                }
            }
            else
            {
                ShowToastr(this.Page, "Please Enter the Journal Name and submit", "Error", "error");
                return;
            }
        }

        protected void ddl_category_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSubscriptionDetailsGV();
        }

        protected void btn_category_name_Click(object sender, EventArgs e)
        {
            string CategoryName = txt_category_name.Text.Trim();
            if (!string.IsNullOrEmpty(CategoryName))
            {
                try
                {
                    sql = "SELECT COUNT(*) FROM CATEGORYMASTER WHERE CATEGORYNAME = @CATEGORYNAME";
                    SqlParameter[] param = { new SqlParameter("@CategoryName", CategoryName) };
                    int count = DBContext.GetCount(sql, param, CommandType.Text);
                    if (count == 0)
                    {
                        SqlParameter[] sp = { new SqlParameter("@CategoryName", CategoryName), new SqlParameter("@FACULTY_ID", fid) };
                        IDataReader dr1 = DBContext.GetDataReader("USP_SAVECATEGORY", sp, CommandType.StoredProcedure);

                        ShowToastr(this.Page, "Category Name Added Successfully.", "Success", "Success");
                        fil_all_ddl1();
                        txt_category_name.Text = "";
                    }
                    else
                    {
                        ShowToastr(this.Page, "Category name already exists.", "Error", "error");
                    }

                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddSubscription.cs -> btn_category_name_Click()", ex.Message);
                }
            }
            else
            {
                ShowToastr(this.Page, "Please Enter the Category Name and submit", "Error", "error");
            }
        }

        protected void YourGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = YourGridView.SelectedIndex;
            int subscriptionId = Convert.ToInt32(YourGridView.DataKeys[index].Values[0]);
            string sql = "SELECT * FROM SUBSCRIPTION_MASTER A JOIN JOURNAL_MASTER B ON A.JOURNAL_ID=B.JOURNAL_ID \nJOIN CATEGORYMASTER C ON A.CATEGORYCODE=C.CATEGORY_ID WHERE SUBSCRIPTION_ID=@SubscriptionId"; ;

            SqlParameter[] sp = { new SqlParameter("@SubscriptionId", subscriptionId) };
            DataSet ds = DBContext.GetDataSet(sql, sp, CommandType.Text);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    string journalname = ds.Tables[0].Rows[0]["JOURNAL_NAME"].ToString();
                    string CATEGORYNAME = ds.Tables[0].Rows[0]["CATEGORYNAME"].ToString();
                    //txt_sub_id.Text = ds.Tables[0].Rows[0]["subscription_id"].ToString();
                    txt_sub_no.Text = ds.Tables[0].Rows[0]["subscription_no"].ToString();
                    txt_address.InnerText = ds.Tables[0].Rows[0]["address"].ToString();
                    txt_tel_no.Text = ds.Tables[0].Rows[0]["tel_no"].ToString();
                    txt_mob_no.Text = ds.Tables[0].Rows[0]["mob_no"].ToString();
                    txt_email.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txt_sub_from.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["subscription_from"]).ToString("yyyy-MM-dd");
                    txt_sub_to.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["subscription_to"]).ToString("yyyy-MM-dd");
                    txt_cont_per.Text = ds.Tables[0].Rows[0]["contact_person"].ToString();
                    ddl_periodicity.SelectedItem.Text = ds.Tables[0].Rows[0]["periodicity"].ToString();
                    txt_amount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                    txt_trans_id.Text = ds.Tables[0].Rows[0]["transaction_id"].ToString();
                    txt_trans_date.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["transaction_date"]).ToString("yyyy-MM-dd");
                    fil_all_ddl1();
                    //  ddl_publisher.SelectedValue = ddl_publisher.Items.FindByText(publisherName).Value;
                    ddl_journal_name.SelectedValue = ddl_journal_name.Items.FindByText(journalname).Value;
                    ddl_category_name.SelectedValue = ddl_category_name.Items.FindByText(CATEGORYNAME).Value;
                }
                ShowToastr(this.Page, "You can now update the changes for the selected Subscription", "", "info");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}