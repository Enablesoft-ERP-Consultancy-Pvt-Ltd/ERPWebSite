using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Charge : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            txtchargename.Focus();
            Fill_Grid();
            Session["Expid"] = 0;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
           // int id = 0;
            SqlParameter[] _arrPara = new SqlParameter[6];
            _arrPara[0] = new SqlParameter("@Expid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ChargeName", SqlDbType.NVarChar);
            _arrPara[2] = new SqlParameter("@Percentage", SqlDbType.NVarChar);
            _arrPara[3] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[0].Value = Session["Expid"];
            _arrPara[1].Value = txtchargename.Text.ToUpper();
            _arrPara[2].Value = txtpercentage.Text;
            _arrPara[3].Direction = ParameterDirection.Output;
            _arrPara[4].Value = Session["varuserid"].ToString();
            _arrPara[5].Value = Session["varMasterCompanyIDForERP"].ToString();
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ExpenseName", _arrPara);
            int ID = Convert.ToInt32(_arrPara[3].Value);
            if (ID == 0)
            {
                LblError.Text = "DUPLICATE DATA EXISTS.....";
            }
            else if (ID == 1)
            {
                LblError.Text = "DATA SAVED SUCESSFULLY....";
            }
            Session["Expid"] = 0;
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Charge.aspx");
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        txtchargename.Text = "";
        txtpercentage.Text = "";
        Fill_Grid();
    }

    protected void Gvchargename_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvchargename, "Select$" + e.Row.RowIndex);
        }
    }
    private void Fill_Grid()
    {
        Gvchargename.DataSource = Fill_Grid_Data();
        Gvchargename.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select ExpID SrNo,ChargeName,Percentage  from ExpenseName Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Charge.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void Gvchargename_SelectedIndexChanged(object sender, EventArgs e)
    {

        //if (Gvchargename .Rows .Count >0)
        //{
        //    txtchargename.Text = Gvchargename.ID;
        //}
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            Session["Expid"]= Convert.ToInt32(Gvchargename.SelectedValue);
            string strsql = "select ExpID SrNo,ChargeName,Percentage  from ExpenseName where ExpID="+Gvchargename.SelectedValue;
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            txtchargename.Text = ds.Tables[0].Rows[0]["ChargeName"].ToString();
            txtpercentage.Text = ds.Tables[0].Rows[0]["Percentage"].ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Charge.aspx");
            Logs.WriteErrorLog("" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        Session["Expid"] = 0;
        LblError.Text = "";
        txtpercentage.Text = "";
        txtchargename.Text = "";
        txtchargename.Focus();
    }
    protected void Gvchargename_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}