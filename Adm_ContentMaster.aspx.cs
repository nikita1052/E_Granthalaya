using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Granthalaya
{
    public partial class Adm_ContentMaster : System.Web.UI.Page
    {
        string name, designation, deptCode, deptName;
        static int fid, dept_id;
        DateTime dt = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["loginid"] = "s. sai sree@Ug";
                Session["loginid"] = "bharathi rao@Ug";
                string jobTitle = GetCurrentUserJobTitle();
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else if (jobTitle == "Librarian")
                {
                    GetLoggedInUserDetails();
                    BindYearDDL();
                    ddl_course.Items.Insert(0, "-- Select Course --");
                    ddl_semsub.Items.Insert(0, "-- Select Semester : Subject --");
                    ddl_con.Items.Insert(0, "-- Content Type --");
                    pnl_nodata.Visible = true;
                }
                else if (jobTitle == "Assistant Professor")
                {
                    GetLoggedInUserDetails();
                    BindYearDDL();
                    btn_addcon.Visible = false;
                    ddl_course.Items.Insert(0, "-- Select Course --");
                    ddl_semsub.Items.Insert(0, "-- Select Semester : Subject --");
                    ddl_con.Items.Insert(0, "-- Content Type --");
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
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occured -> " + ex.Message, "Error", "error");
            }
        }

        private void GetEContentsData(int f)
        {
            try
            {
                int year = Convert.ToInt32(ddl_year.SelectedValue);
                int course = Convert.ToInt32(ddl_course.SelectedValue);
                string[] semsub = (ddl_semsub.SelectedItem.Text).Split(new string[] { "sem", ":" }, StringSplitOptions.RemoveEmptyEntries);
                string sem = semsub[0];
                string sub = ddl_semsub.SelectedValue;
                int cont = Convert.ToInt32(ddl_con.SelectedValue);
                if (f == 0)
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                        new SqlParameter("@YEAR", year),
                        new SqlParameter("@COURSE", course),
                        new SqlParameter("@SEM", sem),
                        new SqlParameter("@SUB", sub),
                        new SqlParameter("@CONT", cont),
                        new SqlParameter("@CHECK", f),
                    };
                    DataSet ds = DBContext.GetDataSet("USP_GET_ECONTENTS", sp, CommandType.StoredProcedure);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        pnl_grid.Visible = true;
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                        pnl_nodata.Visible = false;
                    }
                    else
                    {
                        pnl_grid.Visible = false;
                        pnl_nodata.Visible = true;
                    }
                }
                else if (f == 1)
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                        new SqlParameter("@YEAR", year),
                        new SqlParameter("@COURSE", course),
                        new SqlParameter("@SEM", sem),
                        new SqlParameter("@SUB", sub),
                        new SqlParameter("@CONT", cont),
                        new SqlParameter("@CHECK", f),
                    };
                    DataSet ds = DBContext.GetDataSet("USP_GET_ECONTENTS", sp, CommandType.StoredProcedure);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        pnl_grid.Visible = true;
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                        pnl_nodata.Visible = false;
                    }
                    else
                    {
                        pnl_grid.Visible = false;
                        pnl_nodata.Visible = true;
                    }
                }
                else
                {
                    pnl_grid.Visible = false;
                    pnl_nodata.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ShowToastr(this.Page, "An error occurred: " + ex.Message, "Error", "error");
                return;
            }
        }

        private void BindYearDDL()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("SELECT * FROM ACADEMIC_YEAR_MASTER ORDER BY YEAR_ID DESC", sp, CommandType.Text);
                ddl_year.DataSource = ds;
                ddl_year.DataTextField = "YEAR_DESC";
                ddl_year.DataValueField = "YEAR_ID";
                ddl_year.DataBind();
                ddl_year.Items.Insert(0, new ListItem("--Select Year--", "0"));
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> BindYearDDL()", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }
        protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetCurrentUserJobTitle() == "Librarian")
            {
                string year = ddl_year.SelectedValue;
                string course = ddl_course.SelectedValue;
                string sem = "";
                int check = 3;
                SqlParameter[] sp = new SqlParameter[]
                {
                  new SqlParameter("@YEAR", year),
                  new SqlParameter("@COURSE", course),
                  new SqlParameter("@SEM", sem),
                  new SqlParameter("@FID", fid),
                  new SqlParameter("@CHECK", check),
                  new SqlParameter("@FLAG", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_BIND_FOR_ECONTENTS", sp, CommandType.StoredProcedure);
                ddl_course.DataSource = ds;
                ddl_course.DataTextField = "COURSE_NAME";
                ddl_course.DataValueField = "COURSE_ID";
                ddl_course.DataBind();
                ddl_course.Items.Insert(0, new ListItem("--Select Course--", "-1"));
                ddl_semsub.SelectedIndex = 0;
                pnl_grid.Visible = false;
            }
            else
            {
                try
                {
                    string year = ddl_year.SelectedValue;
                    string course = ddl_course.SelectedValue;
                    string sem = "";
                    int check = 1;
                    SqlParameter[] sp = new SqlParameter[]
                    {
                  new SqlParameter("@YEAR", year),
                  new SqlParameter("@COURSE", course),
                  new SqlParameter("@SEM", sem),
                  new SqlParameter("@FID", fid),
                  new SqlParameter("@CHECK", check),
                  new SqlParameter("@FLAG", 0),
                    };
                    DataSet ds = DBContext.GetDataSet("USP_BIND_FOR_ECONTENTS", sp, CommandType.StoredProcedure);
                    ddl_course.DataSource = ds;
                    ddl_course.DataTextField = "COURSE_NAME";
                    ddl_course.DataValueField = "COURSE_ID";
                    ddl_course.DataBind();
                    ddl_course.Items.Insert(0, new ListItem("--Select Course--", "-1"));
                    ddl_semsub.SelectedIndex = 0;
                    pnl_grid.Visible = false;
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_ContentMaster.aspx.cs -> ddl_year_SelectedIndexChanged() -> ", ex.Message);
                    ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
                }
            }
        }

        protected void ddl_course_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetCurrentUserJobTitle() == "Librarian")
            {
                try
                {
                    string year = ddl_year.SelectedValue;
                    string course = ddl_course.SelectedValue;
                    string sem = "";
                    int check = 3;
                    SqlParameter[] sp = new SqlParameter[]
                    {
                        new SqlParameter("@YEAR", year),
                        new SqlParameter("@COURSE", course),
                        new SqlParameter("@SEM", sem),
                        new SqlParameter("@FID", fid),
                        new SqlParameter("@CHECK", check),
                        new SqlParameter("@FLAG", 1),
                    };
                    DataSet ds = DBContext.GetDataSet("USP_BIND_FOR_ECONTENTS", sp, CommandType.StoredProcedure);
                    ddl_semsub.DataSource = ds;
                    ddl_semsub.DataTextField = "SUBJECTNAME";
                    ddl_semsub.DataValueField = "SUBJECT_KEY";
                    ddl_semsub.DataBind();
                    ddl_semsub.Items.Insert(0, "--Select Semester : Subject--");
                    pnl_grid.Visible = false;
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_ContentMaster.aspx.cs -> ddl_sem_SelectedIndexChanged() -> ", ex.Message);
                    ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
                }
            }
            else
            {
                try
                {
                    string year = ddl_year.SelectedValue;
                    string course = ddl_course.SelectedValue;
                    string sem = "";
                    int check = 2;
                    SqlParameter[] sp = new SqlParameter[]
                    {
                  new SqlParameter("@YEAR", year),
                  new SqlParameter("@COURSE", course),
                  new SqlParameter("@SEM", sem),
                  new SqlParameter("@FID", fid),
                  new SqlParameter("@CHECK", check),
                  new SqlParameter("@FLAG", 0),
                    };
                    DataSet ds = DBContext.GetDataSet("USP_BIND_FOR_ECONTENTS", sp, CommandType.StoredProcedure);
                    ddl_semsub.DataSource = ds;
                    ddl_semsub.DataTextField = "SUBJECTNAME";
                    ddl_semsub.DataValueField = "SUBJECT_KEY";
                    ddl_semsub.DataBind();
                    ddl_semsub.Items.Insert(0, "--Select Semester : Subject--");
                    pnl_grid.Visible = false;
                }
                catch (Exception ex)
                {
                    Common.WriteToLog("Adm_ContentMaster.aspx.cs -> ddl_sem_SelectedIndexChanged() -> ", ex.Message);
                    ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
                }
            }
        }

        protected void ddl_sub_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindContentDDL();
                GetEContentsData(0);
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> ddl_sub_SelectedIndexChanged() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        private void BindContentDDL()
        {
            try
            {
                SqlParameter[] sp = null;
                DataSet ds = DBContext.GetDataSet("SELECT * FROM CONTENT_MASTER WHERE RECORD_STATUS_ID=1", sp, CommandType.Text);
                ddl_con.DataSource = ds;
                ddl_con.DataTextField = "CONTENT_TYPE";
                ddl_con.DataValueField = "CONTENT_ID";
                ddl_con.DataBind();
                ddl_con.Items.Insert(0, new ListItem("--Select Content Type--", "0"));
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> BindContentDDL() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occured ->" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        //============================== MODAL POPUP ONCLICK =============================================
        protected void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                string txtadd = txt_addcontent.Text;
                if (string.IsNullOrEmpty(txtadd))
                {
                    ShowToastr(this.Page, "Kindly fill all the details", "Error", "error");
                }
                else
                {
                    SqlParameter[] sp = new SqlParameter[]
                    {
                        new SqlParameter("@TEXT",txtadd)
                    };
                    int i = Convert.ToInt32(DBContext.ExecuteScalarCmd("SELECT COUNT(*) FROM CONTENT_MASTER WHERE CONTENT_TYPE = @TEXT AND RECORD_STATUS_ID=1", sp, CommandType.Text));
                    if (i > 0)
                    {
                        ShowToastr(this.Page, "Entered Type Already Exists", "Error", "error");
                        txt_addcontent.Text = "";
                    }
                    else
                    {
                        SqlParameter[] sp1 = new SqlParameter[]
                        {
                        new SqlParameter("@TEXT", txtadd),
                        new SqlParameter("@FACULTY_ID", fid),
                        };
                        DBContext.ExecuteNonQueryCmd("USP_INSERT_CONTENTTYPE", sp1, CommandType.StoredProcedure);
                        txt_addcontent.Text = "";
                        BindContentDDL();
                        ShowToastr(this.Page, "Content Type Added Successfully", "Success", "Success");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> btn_add_Click()", ex.Message);
                ShowToastr(this.Page, ex.Message, "Error", "error");
            }
            finally
            {
                BindContentDDL();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteContent")
            {
                try
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    string doc = GridView1.Rows[RowIndex].Cells[2].Text;
                    string doc1 = Convert.ToString(Server.MapPath("~/Uploads/"));
                    string file = doc1 + doc;
                    SqlParameter[] sp = new SqlParameter[]
                      {
                      new SqlParameter("@FID", fid),
                      new SqlParameter("@DOC", file),
                      };
                    int ds = DBContext.ExecuteNonQueryCmd("UPDATE ECONTENT_MASTER SET RECORD_STATUS_ID=0, MODIFIED_BY=@FID, MODIFIED_ON=GETDATE() WHERE DOCUMENT_PATH=@DOC", sp, CommandType.Text);
                    GetEContentsData(1);
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
                    Common.WriteToLog("Adm_ContentMaster.aspx.cs -> GridView1_RowDeleting()", ex.Message);
                    ShowToastr(this.Page, ex.Message, "Error", "error");
                }
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

        //============================== SQL DATA INSERTION, RESIZE IMAGE, FILE UPLOAD =============================================
        protected void ddl_con_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEContentsData(1);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl_year.SelectedIndex != 0 && ddl_course.SelectedIndex != 0 &&
                    ddl_semsub.SelectedIndex != 0 && ddl_con.SelectedIndex != 0)
                {
                    int year = Convert.ToInt32(ddl_year.SelectedValue);
                    int course = Convert.ToInt32(ddl_course.SelectedValue);
                    string subb = ddl_semsub.SelectedValue;
                    string[] semsub = (ddl_semsub.SelectedItem.Text).Split(new string[] { "sem", ":" }, StringSplitOptions.RemoveEmptyEntries);
                    string sem = semsub[0];
                    int cont = Convert.ToInt32(ddl_con.SelectedValue);
                    string filename = txtfile.Text;

                    DataSet ecount = DBContext.GetDataSet("SELECT COALESCE(MAX(ec_id) + 1, 1) AS ID FROM ECONTENT_MASTER;", null, CommandType.Text);
                    string ec_count = Convert.ToString(ecount.Tables[0].Rows[0]["ID"]);

                    if (string.IsNullOrEmpty(filename))
                    {
                        ShowToastr(this.Page, "Please Enter file name for your content", "Warning", "warning");
                    }
                    else
                    {
                        if (fileupload.HasFile)
                        {
                            if (fileupload.FileBytes.Length > 4194304)
                            {
                                ShowToastr(this.Page, "File Size exceeds 4 MB - Please Upload File Size Maximum 4 MB", "Error", "error");
                                return;
                            }
                            HttpPostedFile postedFile = fileupload.PostedFile;
                            string fileExtension = Path.GetExtension(postedFile.FileName).ToLower();
                            string extension = ec_count + "_" + filename + fileExtension;
                            string fileuploadPath = Server.MapPath("~/Uploads/") + extension;

                            string[] ForPDFPPTs = { ".pdf", ".pptx", ".ppt" };
                            string[] ForImagesDocs = { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".xls", ".xlsx" };

                            if (ForPDFPPTs.Contains(fileExtension))
                            {
                                SaveFile(postedFile, fileuploadPath, extension, year, course, sem, subb, cont);
                            }
                            else if (ForImagesDocs.Contains(fileExtension))
                            {
                                if (fileupload.FileBytes.Length > 512000)
                                {
                                    ShowToastr(this.Page, "File Size exceeds 500 KB - Please Upload File Size Maximum 500 KB", "Error", "error");
                                }
                                else
                                {
                                    SaveFile(postedFile, fileuploadPath, extension, year, course, sem, subb, cont);
                                }
                            }
                            else
                            {
                                ShowToastr(this.Page, "Only (Doc/PDF/PPT/Image/Excel) File Acceptable - Please Upload Your File Again", "Error", "error");
                            }
                        }
                        else
                        {
                            ShowToastr(this.Page, "You have not selected any file - Browse and Select File First", "Error", "error");
                        }
                    }
                }
                else
                {
                    ShowToastr(this.Page, "Kindly fill all the details", "Error", "error");
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_ContentMaster.aspx.cs -> btn_submit_click()", ex.Message);
                ShowToastr(this.Page, ex.Message, "Error", "error");
            }
        }

        private void SaveFile(HttpPostedFile postedFile, string fileuploadPath, string extension, int year, int course, string sem, string subb, int cont)
        {
            if (File.Exists(fileuploadPath))
            {
                ShowToastr(this.Page, "This File Already Exists, Try uploading a different file.", "Error", "error");
            }
            else
            {
                postedFile.SaveAs(fileuploadPath);
                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@YEAR_ID", year),
                new SqlParameter("@COURSE_ID", course),
                new SqlParameter("@SEM", sem),
                new SqlParameter("@SUB", subb),
                new SqlParameter("@CONTENT", cont),
                new SqlParameter("@DOC", fileuploadPath),
                new SqlParameter("@UID", fid),
                };
                DBContext.ExecuteScalarCmd("USP_SAVE_UPLOADED_FILE_DATA", sp, CommandType.StoredProcedure);
                ShowToastr(this.Page, "File added successfully", "Success", "success");
                txtfile.Text = string.Empty;
                GetEContentsData(1);
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}