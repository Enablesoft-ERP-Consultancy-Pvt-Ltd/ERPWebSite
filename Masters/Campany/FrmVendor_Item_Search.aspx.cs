using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_FrmVendor_Item_Search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref ddcategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select --");
        }
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditem, "select ITEM_ID,ITEM_NAME from item_master where CATEGORY_ID=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ITEM_NAME", true, "--Select ItemName--");
    }
    private void fillgrid()
    {
        string str = "";
        try
        {
            str = @"select Item_Id,QualityName,QualityId  from Quality where Item_id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            gvitemdetail.DataSource = Ds;
            gvitemdetail.DataBind();


        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
    }

    protected void dditem_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void gvitemdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvitemdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvitemdetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string qry = @"SELECT EmpName,Address,PhoneNo,DepartmentName,QualityName
                       FROM   EmpInfo INNER JOIN  Department ON EmpInfo.Departmentid=Department.DepartmentId inner join Employee_ItemDetail 
                       on Employee_ItemDetail.EmpId = EmpInfo.EmpId inner join Quality on Quality.QualityId = Employee_ItemDetail.QualityId
                       Where EmpInfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        string str = "0";
        for (int i = 0; i < gvitemdetail.Rows.Count; i++)
        {
            if (((CheckBox)gvitemdetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                str = str == "" ? ((Label)gvitemdetail.Rows[i].FindControl("lblqualityid")).Text : str + "," + ((Label)gvitemdetail.Rows[i].FindControl("lblqualityid")).Text;

            }
        }
        qry = qry + " And  Employee_ItemDetail.QualityId in(" + str + ")";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\Vendoritemdetail.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Vendoritemdetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void txtitemsearch_TextChanged(object sender, EventArgs e)
    {
        if (txtitemsearch.Text != "")
        {
           string str = "select QualityId from Quality where item_id=" + dditem.SelectedValue + "  And  QualityName like '%" + txtitemsearch.Text + "%'";
            DataSet Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            for (int i = 0; i < gvitemdetail.Rows.Count; i++)
            {
                for (int j = 0; j < Ds1.Tables[0].Rows.Count; j++)
                {
                    if (((Label)gvitemdetail.Rows[i].FindControl("lblqualityid")).Text == Ds1.Tables[0].Rows[j]["QualityId"].ToString())
                    {
                        ((CheckBox)gvitemdetail.Rows[i].FindControl("Chkbox")).Checked = true;
                    }
                }
            }
        }
    }
}