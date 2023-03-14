using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class HRUserControls_Leavetype : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            fillgrid();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@Leaveid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnleaveid.Value;
            param[1] = new SqlParameter("@name", txtname.Text.Trim());
            param[2] = new SqlParameter("@Code", txtcode.Text);
            param[3] = new SqlParameter("@Type", DDtype.SelectedValue);
            param[4] = new SqlParameter("@unit", RDdays.Checked == true ? "1" : (RDhours.Checked == true ? "2" : "0"));
            param[5] = new SqlParameter("@Description", txtdesc.Text.Trim());
            param[6] = new SqlParameter("@Userid", Session["varuserid"]);
            param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVELEAVETYPE", param);
            Tran.Commit();
            if (param[7].Value.ToString() != "")
            {
                lblmsg.Text = param[7].Value.ToString();
            }
            else
            {
                lblmsg.Text = "Data Saved successfully !!!.";
                hnleaveid.Value = "0";
                Refreshcontrol();
            }
            fillgrid();
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
    protected void Refreshcontrol()
    {
        txtname.Text = "";
        txtcode.Text = "";
        DDtype.SelectedValue = "1";
        RDdays.Checked = true;
        txtdesc.Text = "";
    }
    protected void fillgrid()
    {
        string str = @"SELECT LeaveId,NAME,CODE,TYPE=CASE WHEN TYPE=1 THEN 'PAID' WHEN TYPE=2 THEN 'UNPAID' WHEN TYPE=3 THEN 'ONDUTY' WHEN TYPE=4 THEN 'RESTRICTED HOLIDAY' ELSE'' END,
                    UNIT=CASE WHEN UNIT=1 THEN 'DAYS' WHEN UNIT=2 THEN 'HOURS' ELSE '' END,DESCRIPTION,TYPE AS TYPEID,UNIT AS UNITID
                    FROM HR_LEAVETYPE";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        Dgdetail.DataSource = ds.Tables[0];
        Dgdetail.DataBind();
    }
    protected void lbEdit_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow grv = (GridViewRow)lb.NamingContainer;
        hnleaveid.Value = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblleaveid")).Text;
        txtname.Text = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblname")).Text;
        txtcode.Text = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblcode")).Text;
        Label lbltypeid = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lbltypeid"));
        Label lblunitid = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblunitid"));
        txtdesc.Text = ((Label)Dgdetail.Rows[grv.RowIndex].FindControl("lbldesc")).Text;
        if (DDtype.Items.FindByValue(lbltypeid.Text) != null)
        {
            DDtype.SelectedValue = lbltypeid.Text;
        }
        RDdays.Checked = false;
        RDhours.Checked = false;
        switch (lblunitid.Text)
        {
            case "1":
                RDdays.Checked = true;
                break;
            case "2":
                RDhours.Checked = true;
                break;
            default:
                break;
        }
    }
    protected void lbldel_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            Label lblleaveid = (Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblleaveid");
            Label lblname = (Label)Dgdetail.Rows[grv.RowIndex].FindControl("lblname");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@leaveid", lblleaveid.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@Name", lblname.Text);
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_DELETELEAVETYPE", param);
            Tran.Commit();
            lblmsg.Text = param[1].Value.ToString();
            fillgrid();
        }
        catch (SqlException exs)
        {
            if (exs.Number == 547)
            {
                lblmsg.Text = "This Leave type is already used in Other Process.";
            }
            else
            {
                lblmsg.Text = exs.Message;
            }
            Tran.Rollback();
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
}