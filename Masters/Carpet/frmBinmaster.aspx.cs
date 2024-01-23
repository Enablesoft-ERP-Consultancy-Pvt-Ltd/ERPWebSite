using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Carpet_frmBinmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            UtilityModule.ConditionalComboFill(ref DDgodown, "select GoDownID,GodownName From GodownMaster order by GodownName", true, "--Plz Select--");
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
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Binid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnbinid.Value;
            param[1] = new SqlParameter("@BinNo", txtbinno.Text.Trim());
            param[2] = new SqlParameter("@Capacity", txtcapacity.Text == "" ? "0" : txtcapacity.Text);
            param[3] = new SqlParameter("@GodownId", DDgodown.SelectedValue);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEBINMASTER", param);
            lblmsg.Text = param[6].Value.ToString();
            Tran.Commit();
            txtbinno.Focus();
            refresh();
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
    protected void refresh()
    {

        txtbinno.Text = "";
        txtcapacity.Text = "";
        hnbinid.Value = "0";
        btnsave.Text = "Save";
    }
    protected void fillgrid()
    {
        string str = @"select BINID,BINNO,gm.GodownName,gm.godownid,BM.capacity,case when isnumeric(BM.BInNo)=1 Then cast(Bm.binno as int) else 999999 end as BinNo1
                      From BINMASTER  BM inner join GodownMaster Gm on BM.GODOWNID=gm.GoDownID Where 1=1";
        if (DDgodown.SelectedIndex > 0)
        {
            str = str + " and BM.godownid=" + DDgodown.SelectedValue;
        }
        str = str + " order by BinNo1";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGDetail.DataSource = ds.Tables[0];
        DGDetail.DataBind();
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1")
        {
            DDgodown.Enabled = false;
            txtbinno.Enabled = false;
            txtcapacity.Enabled = false;
        }
        hnbinid.Value = ((Label)DGDetail.Rows[DGDetail.SelectedIndex].FindControl("lblbinid")).Text;
        DDgodown.SelectedValue = ((Label)DGDetail.Rows[DGDetail.SelectedIndex].FindControl("lblgodownid")).Text;
        txtbinno.Text = ((Label)DGDetail.Rows[DGDetail.SelectedIndex].FindControl("lblbinno")).Text;
        txtcapacity.Text = ((Label)DGDetail.Rows[DGDetail.SelectedIndex].FindControl("lblcapacity")).Text;
        btnsave.Text = "Update";
    }
}