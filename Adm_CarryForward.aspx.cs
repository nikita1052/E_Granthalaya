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
    public partial class Adm_CarryForward : System.Web.UI.Page
    {
        string name, designation, deptCode, deptName;
        static int fid, dept_id, yid, cid, eid, sem;
        private string eGrathalayaConnectionString;
        private string eSanchalanConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session//["loginid"] = "S. SAI SREE@UG"; 
                Session["loginid"] = "bharathi rao@pg";
                if (Session["loginid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                GetLoggedInUserDetails();
                BindGridview();
            }
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

                    if (ds != null && ds.Tables.Count > 0)
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
                else
                {
                    name = "Administrator";
                    designation = "Administrator";
                    fid = 0;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_CarryForward.aspx.cs -> GetLoggedInUserDetails()", ex.Message);
                ShowToastr(this.Page, "Some error occurred -> " + ex.Message, "Error", "error");
            }
        }

       

        protected void BindGridview()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@FID", fid),
                new SqlParameter("@FLAG", 0),
                };
                DataSet ds = DBContext.GetDataSet("USP_CARRYFORWARD", sp, CommandType.StoredProcedure);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    panel_content.Visible = true;
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                }
                else
                {
                    panel_content.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("Adm_CarryForward.aspx.cs ->BindGridview() -> ", ex.Message);
                ShowToastr(this.Page, "Some Error Occurred ->BindGridview()" + Convert.ToString(ex.Message), "Error", "error");
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            try
            {

                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@FID", fid), 
                };
                int ds = DBContext.ExecuteNonQueryCmd("USP_CARRYFORWARD_DATA", sp, CommandType.StoredProcedure);
                if (ds > 0)
                {
                    ShowToastr(this.Page, "New Data Added Successfully.", "Success", "success");
                    BindGridview();
                }
            }
            catch (Exception ex)
            {
                Common.WriteToLog("btn_update_Click", ex.Message);
                ShowToastr(this.Page, "ALREADY ADDED" + ex.Message, "Error", "error");
            }
        }

        public void ShowToastr(Page page, string message, string title, string type = "info")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "toastr_message", "toastr." + type.ToLower() + "('" + message + "', '" + title + "', { closeButton: true });", true);
        }
    }
}
