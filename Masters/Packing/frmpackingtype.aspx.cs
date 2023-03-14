using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Packing_frmpackingtype : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            FillGrid();
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
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnid.Value;
            param[1] = new SqlParameter("@PackingType", txtpacktype.Text);
            param[2] = new SqlParameter("@Remarks", txtremarks.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savepackingtype", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
            FillGrid();
            txtpacktype.Text = "";
            txtremarks.Text = "";
            hnid.Value = "0";
            //
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
    }
    protected void FillGrid()
    {
        string str = "select ID,PackingType,Remarks From PackingType order by PackingType";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGPacktype.DataSource = ds.Tables[0];
        DGPacktype.DataBind();
    }
    protected void DGPacktype_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPacktype, "Edit$" + e.Row.RowIndex);
        }
    }
    protected void DGPacktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = DGPacktype.SelectedRow;
        Label lblid = (Label)gvr.FindControl("lblid");
        Label lblpacktype = (Label)gvr.FindControl("lblpacktype");
        Label lblremarks = (Label)gvr.FindControl("lblremarks");
        txtpacktype.Text = lblpacktype.Text;
        txtremarks.Text = lblremarks.Text;
        hnid.Value = lblid.Text;
    }
}