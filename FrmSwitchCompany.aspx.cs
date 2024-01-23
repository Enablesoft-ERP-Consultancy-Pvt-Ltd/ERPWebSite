using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class FrmSwitchCompany : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Fill_Grid();
        }
    }
    private void Fill_Grid()
    {
        DGSwitchCompany.DataSource = fill_Data_grid();
        DGSwitchCompany.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"Select CI.CompanyId, CI.CompanyName 
            From CompanyInfo CI(Nolock) 
            JOIN Company_Authentication CA(Nolock) ON CA.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" And 
                CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserid"] + " Order By CI.CompanyId";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "FrmSwitchCompany.aspx");
            Logs.WriteErrorLog("FrmSwitchCompany|fill_Data_grid|" + ex.Message);
        }
        return ds;
    }
    protected void DGSwitchCompany_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSwitchCompany, "select$" + e.Row.RowIndex);
        }
    }

    protected void DGSwitchCompany_SelectedIndexChanged(object sender, EventArgs e)
    {        
        lblMessage.Text = "";
        string Str = "";
       
            Str = @"Select CI.CompanyId, case When "+Session["VarCompanyNo"].ToString()+@"=43 then CI.CompanyName else (Select MC.CompanyName From Master_Company MC(Nolock) Where MC.CompanyId = CA.MasterCompanyId) End + Case When IsNull(CI.ShortName, '') = '' Then '' Else '(' + IsNull(CI.ShortName, '')  + ')' End CompanyName 
            From CompanyInfo CI(Nolock) 
            JOIN Company_Authentication CA(Nolock) ON CA.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserid"] + @" 
            Where CI.CompanyId = " + DGSwitchCompany.SelectedDataKey.Value;
             

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["CurrentWorkingCompanyID"] = ds.Tables[0].Rows[0]["CompanyId"];
            Session["varCompanyName"] = ds.Tables[0].Rows[0]["CompanyName"];

            lblMessage.Text = "Welcome to" + " " + Session["varCompanyName"] + " " + "Company";
            //Response.Redirect("FrmSwitchCompany.aspx");
        }
    }
}
