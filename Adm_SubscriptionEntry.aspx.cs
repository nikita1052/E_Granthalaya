using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Services.Description;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;

namespace E_Granthalaya
{
    public partial class Adm_SubscriptionEntry : System.Web.UI.Page
    {
        static int fid, dept_id;
        string name, designation, deptCode, deptName, journal, journal_id;
        string deptcode;
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
                    GetJournals();
                    pnl_nodata.Visible = true;
                    if (Request.QueryString["journal_name"] != null)
                    {
                        string journalName = Server.UrlDecode(Request.QueryString["journal_name"]);
                        DataSet ds = DBContext.GetDataSet("SELECT CONCAT(JOURNAL_NAME,' : ',PERIODICITY) AS JOURNAL,JOURNAL_ID FROM SUBSCRIPTION_MASTER WHERE JOURNAL_NAME='" + journalName + "'", null, CommandType.Text);
                        journal = ds.Tables[0].Rows[0]["JOURNAL"].ToString();
                        journal_id = ds.Tables[0].Rows[0]["JOURNAL_ID"].ToString();
                        ddl_jname.SelectedValue = journal_id;
                        if (ddl_jname.SelectedIndex == 0)
                        {
                            pnl_books.Visible = false;
                            pnl_bookdetails.Visible = false;
                            ShowToastr(this.Page, "Select valid journal.", "Error", "error");
                        }
                        else
                        {
                            SubscriptionDetails();
                            GetSubscriptionDetails();
                        }
                    }
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
                Common.WriteToLog("Adm_SubscriptionEntry.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void GetJournals()
        {
            DataSet ds = DBContext.GetDataSet("USP_GET_JOURNALS_FOR_ENTRY", null, CommandType.StoredProcedure);
            ddl_jname.DataSource = ds;
            ddl_jname.DataTextField = "JOURNALNAME";
            ddl_jname.DataValueField = "JOURNAL_ID";
            ddl_jname.DataBind();
            ListItem li = new ListItem("--Select Journal--", "0");
            ddl_jname.Items.Insert(0, li);
        }

        protected void ddl_jname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_jname.SelectedIndex == 0)
            {
                pnl_books.Visible = false;
                pnl_bookdetails.Visible = false;
                ShowToastr(this.Page, "Select valid journal.", "Error", "error");
            }
            else
            {
                SubscriptionDetails();
                GetSubscriptionDetails();
            }
        }
        private int GetSubscriptionDetailsCountForCurrentYear(string JR_ID)
        {
            int count = 0;
            string query = " SELECT COUNT(*) FROM SUBSCRIPTION_ENTRY WHERE YEAR(SUBS_DATE) = @CurrentYear AND JOURNAL_ID=@JR_ID ";
            SqlParameter[] param = new SqlParameter[]
            {
         new SqlParameter("@CurrentYear", DateTime.Now.Year),new SqlParameter("@JR_ID", Convert.ToInt32(JR_ID))
            };

            DataSet ds = DBContext.GetDataSet(query, param, CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return count;
        }

        protected void UploadButton_Click(string JR_ID)
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
                            string subno = $"{DateTime.Now.Year}{JR_ID}{GetSubscriptionDetailsCountForCurrentYear(JR_ID) + 1}";
                            string basePath = ConfigurationManager.AppSettings["ImageUrl1"];
                            // string filePath = Server.MapPath($"C:/Amit_project/SIA_Granthalaya/SIA_Granthalaya/GranthalayaBookImages/GRANTHALAYA/main/{bookId}.jpg");
                            string filePath = Path.Combine(basePath, subno + ".jpg");
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
        protected void btnview_Click(object sender, EventArgs e)
        {
            // Get the LinkButton that raised the event
            LinkButton btn = (LinkButton)sender;
            // Get the GridViewRow containing the LinkButton
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            // Get the row index
            int rowindex = row.RowIndex;
            // Get the BOOK_ID from the DataKeys collection using the row index
            int bookID = Convert.ToInt32(gridview_subscription_details.DataKeys[rowindex]["SUBS_IMAGE"]);
            // string linkedwith = Convert.ToString(DBContext.ExecuteScalarCmd("SELECT LINKEDWITH FROM BOOK_MASTER WHERE BOOK_ID=" + bookID, null, CommandType.Text));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "Showmodal(" + bookID + ");", true);
        }
        protected void gridview_subscription_details_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridview_subscription_details.EditIndex = -1;
            SubscriptionDetails();
            GetSubscriptionDetails();
        }

        protected void gridview_subscription_details_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                DateTime dateValue = Convert.ToDateTime(gridview_subscription_details.Rows[e.RowIndex].Cells[1].Text);
                SqlParameter[] sp = new SqlParameter[]
                {
                   new SqlParameter("@JOURNAL_ID", ddl_jname.SelectedValue ),
                   new SqlParameter("@SUBSCRIPTION_DATE", dateValue),
                   new SqlParameter("@USERID", fid),
                   new SqlParameter("@STATUS",0),
                   new SqlParameter("@IMAGE",""),

                };
                int ds = DBContext.ExecuteNonQueryCmd("USP_SAVE_SUBSCRIPTION_ENTRY", sp, CommandType.StoredProcedure);
                if (ds < 1)
                {
                    ShowToastr(this.Page, "Subscription Entry Deleted Successfully...!", "Success", "success");
                    GetSubscriptionDetails();
                }
                else
                {
                    ShowToastr(this.Page, "Something went wrong! Try after sometime.", "Error", "error");
                    GetSubscriptionDetails();
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_SubscriptionEntry.aspx.cs -> gridview_subscription_details_RowDeleting() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void btn_subscription_entry_click(object sender, EventArgs e)
        {
            try
            {
                string JR_ID = ddl_jname.SelectedValue;
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@JOURNAL_ID", ddl_jname.SelectedValue ),
                    new SqlParameter("@SUBSCRIPTION_DATE", txt_subscription_date.Text),
                    new SqlParameter("@USERID", fid),
                    new SqlParameter("@STATUS",1),
                    new SqlParameter("@IMAGE",$"{DateTime.Now.Year}{JR_ID}{GetSubscriptionDetailsCountForCurrentYear(JR_ID)+1}"),

                };
                DataSet ds = DBContext.GetDataSet("USP_SAVE_SUBSCRIPTION_ENTRY", sp, CommandType.StoredProcedure);
                UploadButton_Click(ddl_jname.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["FLAG"]);
                        if (flag == 0)
                        {
                            ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Error", "error");
                        }
                        else
                        {
                            ShowToastr(this.Page, ds.Tables[0].Rows[0]["MSG"].ToString(), "Success", "success");
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "swal('Success','Successfully Added','success')', { closeButton: true });", true);
                        }
                    }
                }
                SubscriptionDetails();
                GetSubscriptionDetails();
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_SubscriptionEntry.aspx.cs -> btn_subscription_entry_click()", ex.Message);
                ShowToastr(this.Page, Convert.ToString(ex.Message), "Error", "error");
            }
            finally
            {
                txt_subscription_date.Text = string.Empty;
            }

        }

        protected void SubscriptionDetails()
        {
            SqlParameter[] sp = new SqlParameter[]
                {
                   new SqlParameter("@JOURNAL_ID", ddl_jname.SelectedValue),
                   new SqlParameter("@FLAG", 0),
                };
            DataSet ds = DBContext.GetDataSet("USP_GET_SUBSCRIPTIONS_ENTRIES", sp, CommandType.StoredProcedure);
            lbl_jname.Text = ds.Tables[0].Rows[0]["JOURNAL_NAME"].ToString();
            lbl_Category.Text = ds.Tables[0].Rows[0]["CATEGORYNAME"].ToString();
            lbl_Periodicity.Text = ds.Tables[0].Rows[0]["PERIODICITY"].ToString();
            lbl_Period.Text = ds.Tables[0].Rows[0]["PERIOD"].ToString();
            pnl_bookdetails.Visible = true;
        }

        private void GetSubscriptionDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                   new SqlParameter("@JOURNAL_ID", Convert.ToInt32( ddl_jname.SelectedValue)),
                   new SqlParameter("@FLAG", 1),
                };

                DataSet ds = DBContext.GetDataSet("USP_GET_SUBSCRIPTIONS_ENTRIES", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gridview_subscription_details.DataSource = ds;
                    gridview_subscription_details.DataBind();
                    pnl_nodata.Visible = false;
                    pnl_books.Visible = true;
                }
                else
                {
                    pnl_books.Visible = false;
                    pnl_nodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_SubscriptionEntry.aspx.cs -> GetSubscriptionDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured while getting subscription details.", "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}