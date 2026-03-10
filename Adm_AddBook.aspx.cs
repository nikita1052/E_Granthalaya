using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Lifetime;
using System.Web.Services.Description;
using System.Net.NetworkInformation;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;

namespace E_Granthalaya
{
    public partial class Adm_AddBook : System.Web.UI.Page
    {
        static int fid, dept_id, pd_id;
        string name, designation, deptCode, deptName;
        string deptcode;
        string copy;
        int n_copy = 0;
        static int C = 0;
        static double cst = 0, d_cst = 0, n_cst = 0, a_cst = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               Session["loginid"] = "S. SAI SREE@UG";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian" || jobTitle == "Assistant Professor")
                {
                    GetLoggedInUserDetails();
                    BindDropdownList();
                }
                else
                {
                    Response.Redirect("Unauthorized_Access.aspx");

                }
                // ScriptManager.RegisterStartupScript(this, GetType(), "showLoadingScreen", "showLoadingScreen();", true);
            }
        }
        //protected void Page_LoadComplete(object sender, EventArgs e)
        //{
        //    // Hide the loading screen
        //    ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingScreen", "hideLoadingScreen();", true);
        //}
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
                Common.WriteToLog("Adm_AddBook.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "", "warning");
            }
        }

        protected void BindDropdownList()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@DEPT_ID", dept_id),
                };
                DataSet ds = DBContext.GetDataSet("USP_GETBOOKMASTERDATA", sp, CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)
                {
                    PopulateDropDown(ddl_booktype, ds.Tables[0], "CategoryName", "Category_id");
                    PopulateDropDown(ddl_course, ds.Tables[1], "Course_Name", "Course_Id");
                    PopulateDropDown(ddl_medium, ds.Tables[2], "Language", "Medium_id");
                    PopulateDropDown(ddl_book_source, ds.Tables[3], "bs_type", "bs_id");
                    PopulateDropDown(ddl_publisher, ds.Tables[4], "PublisherName", "P_ID");
                    PopulateDropDown(ddl_author, ds.Tables[5], "AuthorName", "Author_Id");
                    PopulateDropDown(ddl_vendorname, ds.Tables[6], "VENDORNAME", "VENDOR_ID");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddBook.aspx -> BindDropdownList()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "", "warning");
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
                    ddl.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddBook.aspx -> PopulateDropDown()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "", "warning");
            }
        }

        protected void GetBookDetailsGV()
        {
            try
            {
                SqlParameter[] param = null;
                DataSet dt = DBContext.GetDataSet("USP_GETBOOKLIST", param, CommandType.StoredProcedure);
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
                Common.WriteToLog("Adm_AddBook.cs -> GetBookDetailsGV()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "", "warning");
            }
        }
        protected void GetLatestBookDetailsGV(SqlParameter [] sp)
        {
            try
            {
                DataSet dt = DBContext.GetDataSet("USP_REPORT_GETTODAYBOOK", sp, CommandType.StoredProcedure);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    Panel1.Visible = true;
                    pnl_nodata.Visible = false;
                }
                else
                {
                    Panel1.Visible = true;
                    PNL_GRID.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddBook.cs -> GetBookDetailsGV()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "", "warning");
            }
        }

        Boolean ValidateField()
        {
            if (string.IsNullOrWhiteSpace(txt_book_name.Text) || string.IsNullOrWhiteSpace(txt_Ttl_cost.Text) || string.IsNullOrWhiteSpace(txt_no_cpy.Text) ||
                ddl_author.SelectedIndex == 0 || ddl_booktype.SelectedIndex == 0 || ddl_book_source.SelectedIndex == 0 ||
                ddl_medium.SelectedIndex == 0 || ddl_publisher.SelectedIndex == 0 || !fileupload.HasFile)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void GetSubjects()
        {
            int sem = Convert.ToInt32(ddl_Semester.SelectedValue);
            string sql = "SELECT SUBJECT_NAME, SUBJECT_ID FROM MYSQL_SM WHERE SUBJECT_ID IN (SELECT EG_SUB_ID FROM EG_SUBJECT_LINK WHERE SEMESTER = " + Convert.ToInt32(ddl_Semester.SelectedValue) + " AND COURSE_ID = " + Convert.ToInt32(ddl_course.SelectedValue) + ")";
            DataSet dt = DBContext.GetDataSet(sql, null, CommandType.Text);
            ddl_subject.DataSource = dt;
            ddl_subject.DataTextField = "Subject_Name";
            ddl_subject.DataValueField = "Subject_Id";
            ddl_subject.DataBind();

            ListItem li = new ListItem("--Select --", "-1");
            ddl_subject.Items.Insert(0, li);
        }

        public void GetSemester()
        {
            int courseid = Convert.ToInt32(ddl_course.SelectedValue);
            ddl_Semester.Enabled = true;
            string sql = "SELECT DISTINCT(SEMESTER) FROM SUBJECT_MASTER WHERE COURSE_ID=" + courseid;
            DataSet dt = DBContext.GetDataSet(sql, null, CommandType.Text);
            ddl_Semester.DataSource = dt;
            ddl_Semester.DataTextField = "SEMESTER";
            ddl_Semester.DataValueField = "SEMESTER";
            ddl_Semester.DataBind();

            ListItem li = new ListItem("--Select--", "-1");
            ddl_Semester.Items.Insert(0, li);
        }

        protected void btn_add_publisher_Click(object sender, EventArgs e)
        {

            string add_publisher = txt_add_publisher.Text.Trim();
            if (!string.IsNullOrEmpty(add_publisher))
            {
                try
                {
                    SqlParameter[] param = { new SqlParameter("@PublisherName", add_publisher), new SqlParameter("@FACULTY_ID", fid) };
                    DataSet ds = DBContext.GetDataSet("USP_SAVEPUBLISHER", param, CommandType.StoredProcedure);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                            if (flag == 0)
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "", "warning");
                            }
                            else
                            {
                                ShowToastr(this.Page, "New Publisher Added Successfully.", "Success", "success");
                                txt_add_publisher.Text = "";
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('publisher');", true);

                            }
                        }
                    }

                    BindDropdownList();
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> btn_add_publisher_Click() -> ", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving publisher.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "Please Enter the Publisher Name and submit", "", "warning");
            }
        }
        protected void SelectButton_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row1 = (GridViewRow)btn.NamingContainer;
            int rowIndex = row1.RowIndex;
            string billId = (YourGridView.DataKeys[rowIndex].Values["LINKEDWITH"]).ToString();
            int SUB = Convert.ToInt32(YourGridView.DataKeys[rowIndex].Values["SUBJECT_ID"]);
            string bookId = $"{billId}{SUB}{rowIndex + 1}";
            SqlParameter[] sp =
            {
                new SqlParameter("@BILLID",billId)
            };
            DataSet dsCostDetails = DBContext.GetDataSet("SELECT COSTRS, NO_OF_COPY, DISCOUNT FROM TEMP_BOOK_MASTER WHERE LINKEDWITH=@BILLID", sp, CommandType.Text);

            if (dsCostDetails != null && dsCostDetails.Tables.Count > 0 && dsCostDetails.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsCostDetails.Tables[0].Rows[0];
                double cost = Convert.ToDouble(row["COSTRS"]);
                int noCopy = Convert.ToInt32(row["NO_OF_COPY"]);
                double discount = Convert.ToDouble(row["DISCOUNT"]);

                // Update total amounts
                double cst = cost * noCopy; // Total cost for the copies being deleted
                double d_cst = noCopy * (cost - (cost * (discount / 100.0))); // Total discounted cost for the copies being deleted

                a_cst -= cst;
                n_cst -= d_cst;

                txt_act_cst.Text = a_cst.ToString();
                txt_act_disc.Text = (a_cst - n_cst).ToString();
                txt_Ttl_amount.Text = n_cst.ToString();
            }

            DataSet ds = DBContext.GetDataSet("DELETE FROM TEMP_BOOK_MASTER WHERE LINKEDWITH=@BILLID", sp, CommandType.Text);
            int c = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT COUNT(*) FROM TEMP_BOOK_MASTER", null, CommandType.Text));
            if (c == 0)
            {
                SqlParameter[] sp1 = {
                new SqlParameter("@BILLID",YourGridView.DataKeys[rowIndex].Values["BILL_ID"])
                };
                ds = DBContext.GetDataSet("DELETE FROM PURCHASE_DETAILS WHERE BILL_ID=@BILLID", sp1, CommandType.Text);
            }

            string basePath = ConfigurationManager.AppSettings["ImageUrl"];
            // string filePath = Server.MapPath($"C:/Amit_project/SIA_Granthalaya/SIA_Granthalaya/GranthalayaBookImages/GRANTHALAYA/main/{bookId}.jpg");
            string filePath = Path.Combine(basePath, billId + ".jpg");
            if (File.Exists(filePath)) // this is for same file it delete that and store new one
            {
                File.Delete(filePath);
            }
            ShowToastr(this.Page, " Deleted Successfully.", "Success", "success");
            GetBookDetailsGV();
        }

        protected void ddl_Semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSubjects();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddBook.aspx -> ddl_Semester_SelectedIndexChanged() -> ", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting subjects.", "", "warning");
            }
        }

        protected void btn_add_author_Click(object sender, EventArgs e)
        {
            string add_author = txt_add_author.Text.Trim();
            if (!string.IsNullOrEmpty(add_author))
            {
                try
                {
                    SqlParameter[] sp1 = { new SqlParameter("@AuthorName", add_author), new SqlParameter("@Faculty_Id", fid) };
                    DataSet ds = DBContext.GetDataSet("USP_SAVEAUTHOR", sp1, CommandType.StoredProcedure);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                            if (flag == 0)
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "", "warning");
                            }
                            else
                            {
                                ShowToastr(this.Page, "New Author Added Successfully.", "Success", "success");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('exampleModal1');", true);
                                txt_add_author.Text = "";
                            }
                        }
                    }
                    txt_add_author.Text = "";
                    BindDropdownList();
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> btn_add_author_Click() -> ", ex.Message);
                }
            }
            else
            {
                ShowToastr(this.Page, "Please Enter the Author Name and submit", "", "warning");
            }
        }

        private int GetPurchaseDetailsCountForCurrentYear()
        {
            int count = 0;
            string query = " SELECT COUNT(*) FROM PURCHASE_DETAILS WHERE YEAR(DOP) = @CurrentYear ";
            SqlParameter[] param = new SqlParameter[]
            {
         new SqlParameter("@CurrentYear", DateTime.Now.Year)
            };

            DataSet ds = DBContext.GetDataSet(query, param, CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return count;
        }


        protected void btn_add_Click(object sender, EventArgs e)
        {
            C = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT COUNT(*) FROM TEMP_BOOK_MASTER", null, CommandType.Text)) + 1;
            // HERE IT IS COUNT FOR INDEXING 
            if (ValidateField())
            {
                int mediumid = Convert.ToInt32(ddl_medium.SelectedValue);
                int categoryid = Convert.ToInt32(ddl_booktype.SelectedValue);
                string bookName = txt_book_name.Text;
                int authorid = Convert.ToInt32(ddl_author.SelectedValue);
                int publisherid = Convert.ToInt32(ddl_publisher.SelectedValue);
                int no_copy = Convert.ToInt32(txt_no_cpy.Text);
                double cost = Convert.ToInt32(txt_Ttl_cost.Text);
                double discount = Convert.ToInt32(txt_Ttl_discount.Text);
                int currentYear = DateTime.Now.Year;// FOR CURR_YEAR
                int purchaseDetailsCount = GetPurchaseDetailsCountForCurrentYear(); // FOR TAKING COUNT OF CURRENT YEAR
                string pd_id = currentYear.ToString() + (purchaseDetailsCount + 1).ToString(); // MAKING BILL_ID BY CONCATING 
                SqlParameter[] param = null;

                try
                {
                    if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
                    {
                        int subjectid = Convert.ToInt32(ddl_subject.SelectedValue);
                        string sem = (ddl_Semester.SelectedValue).ToString();
                        int courseid = Convert.ToInt32(ddl_course.SelectedValue);

                        param = new SqlParameter[]
                        {
                             new SqlParameter("@MEDIUM_ID", mediumid),
                             new SqlParameter("@CATEGORY_ID", categoryid),
                             new SqlParameter("@SUBJECT_ID", subjectid),
                             new SqlParameter("@BOOKNAME", bookName),
                             new SqlParameter("@AUTHOR_ID", authorid),
                             new SqlParameter("@PUBLISHER_ID", publisherid),
                             new SqlParameter("@SEM", sem),
                             new SqlParameter("@COURSE_ID", courseid),
                             new SqlParameter("@FACULTY_ID", fid),
                             new SqlParameter("@NO_COPY", no_copy),
                             new SqlParameter("@COSTRS", cost),
                             new SqlParameter("@DISCOUNT",Convert.ToInt32( txt_Ttl_discount.Text)),
                             new SqlParameter("@INCREMENTING_VALUE",C),
                             new SqlParameter("@LINKEDWITH", Convert.ToInt64(pd_id)),
                             new SqlParameter("@BILL_ID", (pd_id).ToString()),
                             new SqlParameter("@BSOURCE", ddl_book_source.SelectedItem.Text)
                        };
                        UploadButton_Click((pd_id).ToString(), subjectid.ToString(), C.ToString());
                    }
                    else
                    {
                        param = new SqlParameter[]
                        {
                             new SqlParameter("@MEDIUM_ID", mediumid),
                             new SqlParameter("@CATEGORY_ID", categoryid),
                             new SqlParameter("@SUBJECT_ID", 0),
                             new SqlParameter("@BOOKNAME", bookName),
                             new SqlParameter("@AUTHOR_ID", authorid),
                             new SqlParameter("@PUBLISHER_ID", publisherid),
                             new SqlParameter("@SEM", 0),
                             new SqlParameter("@COURSE_ID", 0),
                             new SqlParameter("@FACULTY_ID", fid),
                             new SqlParameter("@NO_COPY", no_copy),
                             new SqlParameter("@COSTRS", cost),
                             new SqlParameter("@DISCOUNT",Convert.ToInt32( txt_Ttl_discount.Text)),
                             new SqlParameter("@INCREMENTING_VALUE",C),
                             new SqlParameter("@LINKEDWITH", Convert.ToInt64(pd_id)),
                             new SqlParameter("@BILL_ID", (pd_id).ToString()),
                             new SqlParameter("@BSOURCE", ddl_book_source.SelectedItem.Text)
                        };
                        UploadButton_Click((pd_id).ToString(), "0", C.ToString());
                    }

                    DataSet ds = DBContext.GetDataSet("USP_TEMP_SAVEBOOKDETAILS1", param, CommandType.StoredProcedure);
                    SqlParameter[] sp =
                    {
                       new SqlParameter("@BILL_ID", (pd_id).ToString()),
                    };
                    int c = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT COUNT(*) FROM TEMP_BOOK_MASTER", null, CommandType.Text));
                    if (c == 1)
                    {
                        DataSet ds1 = DBContext.GetDataSet("INSERT INTO PURCHASE_DETAILS(BILL_ID) VALUES(@BILL_ID) ", sp, CommandType.Text);
                    }
                    cst = 0;
                    d_cst = 0;
                    for (int i = 0; i < no_copy; i++)
                    {
                        cst = cst + cost; // amount without discount for 1 book
                        double d = cost - (cost * (discount / 100.0));
                        d_cst = (d_cst + d); // amount with discount for 1 book
                    }
                    n_copy += no_copy;
                    a_cst = cst + a_cst; // atual total amount without disc.
                    n_cst = d_cst + n_cst; // net total amount with disc.
                    txt_act_cst.Text = a_cst.ToString();
                    txt_act_disc.Text = (a_cst - n_cst).ToString();
                    txt_Ttl_amount.Text = n_cst.ToString();

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                            if (flag == 1)
                            {
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", "swal('Sucess','New Book Added Successfully','success')", true);
                                ShowToastr(this.Page, "Book details saved successfully.", "Success", "success");
                                // GetBookDetailsGV();

                            }
                            else
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "", "warning");
                            }
                        }
                    }
                    copy = txt_no_cpy.Text;
                    // clear();
                    GetBookDetailsGV();
                    pnl_invoice.Visible = true;
                }
                catch (SqlException ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> Button1_Click() ->", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving the book details.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "please fill all required fields.", "", "warning");
            }
        }
        //protected void SelectButton_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;
        //    string bookName = YourGridView.DataKeys[row.RowIndex].Values["BOOKNAME"].ToString();
        //    string cost = row.Cells[5].Text; 
        //    string copy = row.Cells[7].Text;
        //    txt_mdl_cost.Text = cost;
        //    txt_mdl_nocpy.Text = copy; 
        //}

        protected void ddl_booktype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
            {
                ListItem li = new ListItem("--Select--", "0");
                ddl_Semester.Items.Add(li);
                ddl_subject.Items.Add(li);
                coursesem.Visible = true;
            }
            else
            {
                coursesem.Visible = false;
            }
        }

        void clear()
        {
            //txt_date_bill.Text = "";
            txt_book_name.Text = "";
            ddl_medium.SelectedIndex = 0;
            ddl_author.SelectedIndex = 0;
            ddl_publisher.SelectedIndex = 0;
            if (ddl_booktype.SelectedItem.Text == "Text Book" || ddl_booktype.SelectedItem.Text == "Reference")
            {
                ddl_Semester.SelectedIndex = 0;
                ddl_course.SelectedIndex = 0;
                ddl_subject.SelectedIndex = 0;
            }
            ddl_booktype.SelectedIndex = 0;
            ddl_book_source.SelectedIndex = 0;
            //txt_date_bill.Text = "";
            txt_invoice_no.Text = "";
            txt_Ttl_cost.Text = "";
            txt_no_cpy.Text = "";
            txt_Ttl_amount.Text = "";
            txt_date.Text = "";
            txt_act_cst.Text = "";
            txt_act_disc.Text = "";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Ttl_cost.Text) && !string.IsNullOrWhiteSpace(txt_Ttl_discount.Text) && !string.IsNullOrWhiteSpace(txt_Ttl_amount.Text) && !string.IsNullOrWhiteSpace(txt_no_cpy.Text) && !string.IsNullOrWhiteSpace(txt_invoice_no.Text) && !string.IsNullOrWhiteSpace(txt_date.Text))
            {
                DateTime date = Convert.ToDateTime(txt_date.Text);
                string invoice = (txt_invoice_no.Text);
                decimal costRs = Convert.ToDecimal(txt_act_cst.Text);
                decimal discount = Convert.ToDecimal(txt_act_disc.Text);
                decimal amount = Convert.ToDecimal(txt_Ttl_amount.Text);

                int purchaseDetailsCount = GetPurchaseDetailsCountForCurrentYear();
                string pd_id = (DateTime.Now.Year).ToString() + (purchaseDetailsCount + 1).ToString();

                //UploadButton_Click();
                DataSet DTT = DBContext.GetDataSet("SELECT * FROM TEMP_BOOK_MASTER", null, CommandType.Text);
                n_copy = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT SUM(NO_OF_COPY) FROM TEMP_BOOK_MASTER", null, CommandType.Text));
                try
                {
                    SqlParameter[] sp =
                                {
                                    new SqlParameter("@DOP", date),
                                    new SqlParameter("@COST", costRs),
                                    new SqlParameter("@DISC_AMOUNT", discount),
                                    new SqlParameter("@RECIPT_NO", invoice.ToString()),
                                    new SqlParameter("@NO_OF_COPIES", n_copy),
                                    new SqlParameter("@ACTUAL_AMOUNT", amount),
                                    new SqlParameter("@FACULTY_ID", fid),
                                    new SqlParameter("@BILL_ID", pd_id),
                                    new SqlParameter("@VENDORID", Convert.ToInt32(ddl_vendorname.SelectedValue)),
                                };

                    DataSet ds = DBContext.GetDataSet("USP_SAVEBOOKPURCHASEDETAILS", sp, CommandType.StoredProcedure);

                    if (DTT != null)
                    {
                        if (DTT.Tables.Count != 0 || DTT.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow row in DTT.Tables[0].Rows)
                            {
                                n_copy += 1;
                                SqlParameter[] param =
                                {
                                    new SqlParameter("@MEDIUM_ID", Convert.ToInt32(row["MEDIUM_ID"])),
                                    new SqlParameter("@CATEGORY_ID", Convert.ToInt32(row["CATEGORY_ID"])),
                                    new SqlParameter("@SUBJECT_ID", Convert.ToInt64(row["SUBJECT_ID"])),
                                    new SqlParameter("@BOOKNAME", row["BOOKNAME"].ToString()),
                                    new SqlParameter("@AUTHOR_ID", Convert.ToInt64(row["AUTHOR_ID"])),
                                    new SqlParameter("@PUBLISHER_ID", Convert.ToInt64(row["PUBLISHER_ID"])),
                                    new SqlParameter("@SEM", Convert.ToInt64(row["SEM"])),
                                    new SqlParameter("@COURSE_ID", Convert.ToInt64(row["COURSE_ID"])),
                                    new SqlParameter("@FACULTY_ID", fid),
                                    new SqlParameter("@NO_COPY", Convert.ToInt64(row["NO_OF_COPY"])),
                                    new SqlParameter("@COSTRS", Convert.ToInt32(row["COSTRS"])),
                                    new SqlParameter("@BILL_ID", (row["BILL_ID"])),
                                    new SqlParameter("@BSOURCE", row["BSOURCE"]),
                                    new SqlParameter("@DEPT_ID",dept_id),
                                    new SqlParameter("@BOOKPHOTO", row["BOOK_PHOTO"] ),
                                    new SqlParameter("@LINKEDWITH", Convert.ToInt64(row["LINKEDWITH"]) )
                                };
                                DataSet ds1 = DBContext.GetDataSet("USP_SAVEBOOKDETAILS", param, CommandType.StoredProcedure);

                            }

                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                                    if (flag == 1)
                                    {
                                        ShowToastr(this.Page, "Book Details and Invoice saved successfully.", "Success", "success");
                                        txt_act_cst.Text = "0";
                                        txt_act_disc.Text = "0"; 
                                        txt_Ttl_amount.Text = "0"; 

                                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('bookmoneydtl');", true);
                                        // clear();
                                    }
                                    else
                                    {
                                        ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "", "warning");
                                    }
                                }
                            }
                            DataSet ds2 = DBContext.GetDataSet("DELETE FROM TEMP_BOOK_MASTER", null, CommandType.Text);

                        }
                        else
                        {
                            ShowToastr(this.Page, "No data available to save.", "", "warning");
                            return;
                        }
                    }
                    else
                    {
                        ShowToastr(this.Page, "No data available to save.", "", "warning");
                        return;
                    }

                    clear();
                    GetBookDetailsGV();
                    int pdid = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT MAX(PD_ID) FROM PURCHASE_DETAILS",null,CommandType.Text));
                    string invoice_no = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT VOUCHER_NO FROM PURCHASE_DETAILS WHERE PD_ID="+ pdid, null, CommandType.Text));
                    DateTime invoice_date = Convert.ToDateTime(DBContext.ExecuteScalarCmd("SELECT VOUCHER_DATE FROM PURCHASE_DETAILS WHERE PD_ID="+ pdid, null, CommandType.Text));
                    SqlParameter[] sp1 =
                                {
                                    new SqlParameter("@INVOICE", invoice_no),
                                    new SqlParameter("@INVOICE_DATE", invoice_date),
                                };
                    GetLatestBookDetailsGV(sp1);

                }
                catch (SqlException ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> Button1_Click() ->", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving the book details.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "please fill all required fields.", "", "warning");
            }
        }

        protected void btn_add_booktype_Click(object sender, EventArgs e)
        {
            string add_booktype = txt_add_booktype.Text.Trim();
            if (!string.IsNullOrEmpty(add_booktype))
            {
                try
                {
                    SqlParameter[] sp1 = { new SqlParameter("@CATEGORYNAME", add_booktype), new SqlParameter("@FACULTY_ID", fid) };

                    DataSet ds = DBContext.GetDataSet("USP_SAVECATEGORY", sp1, CommandType.StoredProcedure);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                            if (flag == 1)
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSg"].ToString(), "Success", "success");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('booktype');", true);
                                txt_add_booktype.Text = "";
                            }
                            else
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSg"].ToString(), "", "warning");
                            }
                        }
                    }

                    BindDropdownList();
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> btn_add_booktype_Click()", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving category.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "Book Type/Category name cannot be empty", "", "warning");
            }
        }

        protected void btn_add_vendor_Click(object sender, EventArgs e)
        {
            string vendorName = txt_VendorName.Text.Trim();
            string nameOfPerson = txt_NameOfPerson.Text.Trim();
            string contactNo = txt_ContactNo.Text.Trim();
            string mailAddress = txt_MailAddress.Text.Trim();

            if (!string.IsNullOrEmpty(vendorName) && !string.IsNullOrEmpty(nameOfPerson) &&
                !string.IsNullOrEmpty(contactNo) && !string.IsNullOrEmpty(mailAddress))
            {
                try
                {
                    SqlParameter[] param = {
                new SqlParameter("@VENDORNAME", vendorName),
                new SqlParameter("@NAME_OF_PERSON", nameOfPerson),
                new SqlParameter("@CONTACT_NO", contactNo),
                new SqlParameter("@MAIL_ADDRESS", mailAddress),new SqlParameter("@FACULTY_ID", fid)
            };

                    DataSet ds = DBContext.GetDataSet("USP_SAVEVENDOR", param, CommandType.StoredProcedure);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                        if (flag == 1)
                        {
                            ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Success", "success");
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('mdl_vendor');", true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('mdl_vendor');", true);
                            txt_VendorName.Text = "";
                            txt_NameOfPerson.Text = "";
                            txt_ContactNo.Text = "";
                            txt_MailAddress.Text = "";
                        }
                        else
                        {
                            ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "", "warning");
                        }
                    }

                    // Optionally, refresh any dropdown lists or other data on the page
                    BindDropdownList();
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("YourPage.aspx -> btn_add_vendor_Click()", ex.Message);
                    ShowToastr(this.Page, "Some error occurred while saving vendor details.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "Please fill all required fields and submit", "", "warning");
            }
        }

        protected void btn_showdetails_Click(object sender, EventArgs e)
        {
            txt_act_cst.Text = a_cst.ToString();
            txt_act_disc.Text = txt_Ttl_discount.Text;
            txt_Ttl_amount.Text = n_cst.ToString();
        }

        protected void btn_add_medium_Click(object sender, EventArgs e)
        {
            string add_medium = txt_add_medium.Text.Trim();
            if (!string.IsNullOrEmpty(add_medium))
            {
                try
                {
                    SqlParameter[] param = { new SqlParameter("@LANGUAGE", add_medium), new SqlParameter("@FACULTY_ID", fid) };

                    DataSet ds = DBContext.GetDataSet("USP_SAVEMEDIUM", param, CommandType.StoredProcedure);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);

                            if (flag == 1)
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSg"].ToString(), "Success", "success");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "closeModal('exmodal');", true);
                                txt_add_medium.Text = "";
                            }
                            else
                            {
                                ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSg"].ToString(), "", "warning");
                            }
                        }
                    }

                    BindDropdownList();
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_AddBook.aspx -> btn_add_medium_Click()", ex.Message);
                    ShowToastr(this.Page, "Some error occured while saving medium details.", "", "warning");
                }
            }
            else
            {
                ShowToastr(this.Page, "Please Enter the Medium and submit", "", "warning");
            }
        }

        protected void ddl_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSemester();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_AddBook.aspx -> ddl_course_SelectedIndexChanged()", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting semester.", "", "warning");
            }
        }

        protected void UploadButton_Click(string BILL_ID, string SUB, string INDEX)
        {
            if (fileupload.HasFile)
            {
                if (Path.GetExtension(fileupload.FileName).ToLower() == ".jpg" || Path.GetExtension(fileupload.FileName).ToLower() == ".png")
                {
                    // Check the file size (file size is in bytes, so 1 MB = 1 * 1024 * 1024 bytes)
                    if (fileupload.PostedFile.ContentLength < 1 * 1024 * 1024)
                    {
                        try
                        {
                            // Compress the image and get the byte array
                            byte[] imageBytes = CompressImage(fileupload.PostedFile.InputStream);
                            string bookId = $"{BILL_ID}{SUB}{INDEX}";
                            string basePath = ConfigurationManager.AppSettings["ImageUrl"];
                            // string filePath = Server.MapPath($"C:/Amit_project/SIA_Granthalaya/SIA_Granthalaya/GranthalayaBookImages/GRANTHALAYA/main/{bookId}.jpg");
                            string filePath = Path.Combine(basePath, bookId + ".jpg");
                            if (File.Exists(filePath)) // this is for same file it delete that and store new one
                            {
                                File.Delete(filePath);
                            }
                            // Save new  compressed image
                            File.WriteAllBytes(filePath, imageBytes);
                        }
                        catch (Exception ex)
                        {
                            ShowToastr(this.Page, "Some Error Occured -> Upload status: The file could not be uploaded. The following error occured: " + ex.Message, "", "warning");
                        }
                    }
                    else
                    {
                        ShowToastr(this.Page, "Upload status: The file size exceeds the 1 MB limit!", "", "warning");
                    }
                }
                else
                {
                    ShowToastr(this.Page, "Upload status: Only JPG images are allowed!", "", "warning");
                }
            }
        }

        private byte[] CompressImage(Stream stream)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(stream))
            {

                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 50L);
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, jpgEncoder, encoderParameters);
                    return ms.ToArray();
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        protected void btn_view_Click(object sender, EventArgs e)
        {
            // Get the LinkButton that raised the event
            LinkButton btn = (LinkButton)sender;
            // Get the GridViewRow containing the LinkButton
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            // Get the row index
            int rowindex = row.RowIndex;
            // Get the BOOK_ID from the DataKeys collection using the row index
            int bookID = Convert.ToInt32(YourGridView.DataKeys[rowindex]["BOOK_ID"]);
            string linkedwith = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT LINKEDWITH FROM BOOK_MASTER WHERE BOOK_ID=" + bookID, null, CommandType.Text));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "Showmodal(" + linkedwith + ");", true);
        }
        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}