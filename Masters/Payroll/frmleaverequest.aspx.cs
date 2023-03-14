using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Payroll_frmleaverequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

        }
    }
    protected void chkfilterbydate_CheckedChanged(object sender, EventArgs e)
    {
        Trfromto.Visible = false;

        if (chkfilterbydate.Checked == true)
        {
            Trfromto.Visible = true;
        }
    }
    protected void FillGrid()
    {

        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Findby", ddfindby.SelectedItem.Text);
            param[1] = new SqlParameter("@Datefilter", chkfilterbydate.Checked == true ? 1 : 0);
            param[2] = new SqlParameter("@From", txtfrom.Text == "" ? DBNull.Value : (object)txtfrom.Text);
            param[3] = new SqlParameter("@To", txtto.Text == "" ? DBNull.Value : (object)txtto.Text);
            param[4] = new SqlParameter("@empcode", txtempcode.Text);
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                param[5] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
            }
            else
            {
                param[5] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETLEAVEREQUESTS", param);

            Dgdetail.DataSource = ds.Tables[0];
            Dgdetail.DataBind();
            TBsave.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                TBsave.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnfinddata_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //***********
        DataTable dt = new DataTable();
        dt.Columns.Add("APplicationid", typeof(int));
        dt.Columns.Add("Actionstatus", typeof(string));
        dt.Columns.Add("comments", typeof(string));
        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            CheckBox chkitem = (CheckBox)Dgdetail.Rows[i].FindControl("chkitem");
            if (chkitem.Checked == true)
            {
                Label lblapplicationid = (Label)Dgdetail.Rows[i].FindControl("lblapplicationid");
                TextBox txtcomments = (TextBox)Dgdetail.Rows[i].FindControl("txtcomments");
                DataRow dr = dt.NewRow();

                dr["Applicationid"] = lblapplicationid.Text;
                dr["Actionstatus"] = DDaction.SelectedItem.Text;
                dr["comments"] = txtcomments.Text;
                dt.Rows.Add(dr);
            }
        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altdt", "alert('Please select atleast one Checkbox to save Data ?')", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@dt", dt);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_UPDATELEAVEREQUESTS", param);
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[2].Value.ToString() + "')", true);
            lblmsg.Text = param[2].Value.ToString();
            FillGrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Dgdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblstatus = (Label)e.Row.FindControl("lblstatus");
            CheckBox chkitem = (CheckBox)e.Row.FindControl("chkitem");
            //chkitem.Visible = false;
            switch (lblstatus.Text.ToUpper())
            {
                //case "PENDING":
                //    chkitem.Visible = true;
                //    break;
                case "APPROVED":
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case "REJECTED":
                    e.Row.BackColor = System.Drawing.Color.Red;
                    break;
                default:                    
                    break;
            }
        }
    }
}