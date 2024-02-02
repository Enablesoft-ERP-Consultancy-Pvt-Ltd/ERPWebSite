using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Order_FrmOrderDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            fillgrid();
        }
    }
    private void fillgrid()
    {
        hnissueqty.Value = "0";
        hnrecqty.Value = "0";
        string sql = "";
        if (Request.QueryString["Type"] == "JW")
        {
            sql = @"select omt.CustomerOrderNo orderno, Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName As Description,isnull(Sum(Quantity),0) As Qty,
                    isnull(sum(recquantity),0) as Recqty,replace(convert(varchar(11),om.ReqDate,106),' ','-' ) as reqdate
                    From IndentMaster OM inner join IndentDetail OD On OM.IndentId=OD.IndentId inner join ordermaster omt on omt.orderid=od.orderid inner join
                    V_FinishedItemDetail V on V.Item_Finished_Id=OD.OFinishedId left outer join PP_ProcessRecTran pt on pt.indentid=OD.IndentId and pt.finishedid=OD.OFinishedId
                    Where omt.status=0 and om.PartyId=" + Request.QueryString["Vendor"] + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"  
                    Group by Item_Name,QualityName,Designname,ColorName,ShadeColorName,SizeMtr,SizeFt ,OD.OFinishedId,ReqDate,omt.CustomerOrderNo
                    Having isnull(Sum(Quantity),0)>isnull(sum(recquantity),0)";
        }
        else if (Request.QueryString["Type"] == "PO")
        {
            sql = @"select om.CustomerOrderNo orderno ,CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName As Description, isnull(sum(quantity),0) as qty,isnull(sum(QTY),0) as RecQty ,replace(convert(varchar(11),Delivery_Date,106),' ','-') Reqdate
                From PurchaseIndentIssue pii inner join 
                PurchaseIndentIssueTran pit On pii.PindentIssueid=pit.PindentIssueid inner join
                V_FinishedItemDetail V on V.Item_Finished_Id=pit.Finishedid left outer join
                Ordermaster om on om.orderid=pii.Orderid left outer join 
                PurchaseReceiveDetail prd On prd.PIndentIssueTranId=pit.PIndentIssueTranId and prd.FinishedId=pit.Finishedid
                Where om.status=0 and pii.PartyId=" + Request.QueryString["Vendor"] + "   And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                Group by CATEGORY_NAME,Item_Name,QualityName,Designname,ColorName,ShadeColorName,SizeMtr,SizeFt ,pit.Finishedid,om.CustomerOrderNo,Delivery_Date
                Having isnull(Sum(quantity),0)>isnull(sum(QTY),0)";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnissueqty.Value = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
            hnrecqty.Value = ds.Tables[0].Compute("Sum(Recqty)", "").ToString();
            DG.DataSource = ds;
            DG.DataBind();

            lblMessage1.Text = "";
            lblMessage1.Visible = false;
        }
        else
        {
            lblMessage1.Text = "No record Found";
            lblMessage1.Visible = true;
        }
    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
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
    public string getpend(string StrVAl, string Strval1)
    {
        string val = "0";
        val = Convert.ToString(Convert.ToDouble(StrVAl) - Convert.ToDouble(Strval1));
        return val;
    }
    public string gettotpend()
    {
        string val = "0";
        double count = 0;
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            count = count + Convert.ToDouble(((Label)DG.Rows[i].FindControl("lblpending")).Text);
        }
        val = Convert.ToString(count);
        return val;
    }
    public string gettotissue()
    {
        string val = "0";
        if (hnissueqty.Value != "0")
        {
            val = hnissueqty.Value;
        }
        return val;
    }
    public string gettotRec()
    {
        string val = "0";
        if (hnrecqty.Value != "0")
        {
            val = hnrecqty.Value;
        }
        return val;
    }
}