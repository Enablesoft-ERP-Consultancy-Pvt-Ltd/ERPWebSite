using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Order_OrderDeptPlanning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str="";
            if (Session["varcompanyno"].ToString() != "7")
            {
                str = @"SELECT Distinct OrderId,LocalOrder+ ' / ' +CustomerOrderNo FROM OrderMaster where status=0 ";
            }
            else
            {
                str = @" select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo from OrderMaster om inner join orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  left outer join OrderProcessPlanning pm On om.orderid=pm.orderid Where om.status=0 and om.orderid  in (select distinct orderid from JobAssigns where supplierqty=0) and isnull(finalstatus,0)<>1  and uc.userid=" + Session["varuserid"] + " And V.MasterCompanyId=" + Session["varCompanyId"] + "";
            }
            UtilityModule.ConditionalComboFill(ref DDOrderNo,str, true, "--Select--");
            if (Session["varuserid"].ToString() == "12")
            {
                tdname.Visible = true;
            }
            else
            {
                tdname.Visible = false;
            }
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql;
        if (DDOrderNo.SelectedIndex > 0)
        {
            lblMessage.Visible = false;
            Lblsave.Visible = false;
            lblMessage.Text = "";
            Lblsave.Text = "";
            sql = @"select Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName+'  '+Case When OrderUnitId=4 Then SizeMtr Else SizeMtr End As Description,Sum(QtyRequired) As Qty
                     From OrderMaster OM,OrderDetail OD,V_FinishedItemDetail V  where OM.OrderId=OD.OrderId 
                     And V.Item_Finished_Id=OD.Item_Finished_Id And OM.OrderId=" + DDOrderNo.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"] + " group by Item_Name,QualityName,Designname,ColorName,ShadeColorName,OrderUnitId,SizeMtr,SizeFt ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DG.DataSource = ds;
                DG.DataBind();
            }
            else
            {
                DG.DataSource = ds;
                DG.DataBind();
            }
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Distinct processid from UserRightsProcess where userid="+Session["Varuserid"]+"");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                sql = @"SELECT ID,LocalOrder+ ' / ' +CustomerOrderNo OrderNo,PROCESS_NAME ProcessName,OP.Processid,
                       Replace(Convert(varchar(11),Date,106), ' ','-') AS Date,Qty, 
                       case when FinalDate is NULL then  Replace(Convert(varchar(11),Date,106), ' ','-') else Replace(Convert(varchar(11),FinalDate,106), ' ','-') end  FinalDate,op.planremark as remark,op.depremark
                       From OrderProcessPlanning OP,OrderMaster OM,Process_Name_Master PNM Where OP.OrderId=OM.OrderId And OP.PROCESSID=PNM.PROCESS_NAME_ID and op.processid in(select Distinct processid from UserRightsProcess where userid=" + Session["Varuserid"] + ") And OP.OrderId=" + DDOrderNo.SelectedValue + " And PNM.MasterCompanyId=" + Session["varCompanyId"] + "";
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DGDeptPlan.DataSource = ds1;
                    DGDeptPlan.DataBind();
                    FindProcessId();
                    BtnSave.Visible = true;

                }
                else
                {
                    DGDeptPlan.DataSource = ds1;
                    DGDeptPlan.DataBind();
                    BtnSave.Visible = false;
                }
            }
            DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Distinct processid from UserRightsProcess where userid=" + Session["Varuserid"] + " and ProcessId=1");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                sql = @"Select Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName  As Description,Sum(Qty) As Qty
                     From OrderLocalConsumption OM,V_FinishedItemDetail V  
                     Where V.Item_Finished_Id=OM.Finishedid And OM.OrderId=" + DDOrderNo.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"] + "  group by Item_Name,QualityName,Designname,ColorName,ShadeColorName ";
                DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    DGPurchase.DataSource = ds4;
                    DGPurchase.DataBind();
                    tdpurchase.Visible = true;
                }
            }
            else
            {
                tdpurchase.Visible = false;
            }
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection Con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        Con.Open();
        SqlTransaction Tran = Con.BeginTransaction();
        try
        {
            lblMessage.Text = "";
            for (int j = 0; j < DGDeptPlan.Rows.Count; j++)
            {
                try
                {
                    if (((TextBox)DGDeptPlan.Rows[j].FindControl("TxtProcessReqDate")).Enabled == true && ((TextBox)DGDeptPlan.Rows[j].FindControl("TxtProcessReqDate")).Text == "")
                    {                       
                        lblMessage.Visible = true;
                        lblMessage.Text = "Date can not be blank....";
                    }
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Order/OrderDeptPlanning.aspx");
                }

            }

            for (int i = 0; i < DGDeptPlan.Rows.Count; i++)
            {
                if (((TextBox)DGDeptPlan.Rows[i].FindControl("TxtProcessReqDate")).Enabled == true && ((TextBox)DGDeptPlan.Rows[i].FindControl("TxtProcessReqDate")).Text != "" && lblMessage.Text == "")
                {
                    if(tdname.Visible==true)
                    {
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update OrderProcessPlanning set FinalDate='" + ((TextBox)DGDeptPlan.Rows[i].FindControl("TxtProcessReqDate")).Text + "',depremark='" + ((TextBox)DGDeptPlan.Rows[i].FindControl("Txtremark")).Text + "',Purhasername='"+DDNAME.SelectedItem.Text+"' where  ID=" + DGDeptPlan.DataKeys[i].Value + "");
                    }
                    else
                    {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update OrderProcessPlanning set FinalDate='" + ((TextBox)DGDeptPlan.Rows[i].FindControl("TxtProcessReqDate")).Text + "',depremark='"+((TextBox)DGDeptPlan.Rows[i].FindControl("Txtremark")).Text + "' where  ID=" + DGDeptPlan.DataKeys[i].Value + "");
                    }
                    lblMessage.Visible = false;
                    Lblsave.Visible = true;
                    Lblsave.Text = "Data Saved Successfully...";
                }

            }
            Tran.Commit();
            
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/OrderDeptPlanning.aspx");
            Tran.Rollback();
        }
        finally
        {
            Con.Close();
            Con.Dispose();
        }
    }
    protected void FindProcessId()
    {
        string str = "select ProcessId From UserRightsProcess where UserId=" + Session["varuserid"] + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < DGDeptPlan.Rows.Count; j++)
                {
                    if (ds.Tables[0].Rows[i]["ProcessId"].ToString() == ((Label)DGDeptPlan.Rows[j].FindControl("Lblprocessid")).Text)
                    {
                        ((TextBox)DGDeptPlan.Rows[j].FindControl("TxtProcessReqDate")).Enabled = true;
                    }
                }
            }
        }

    }
    protected void DGDeptPlan_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGPurchase_RowCreated(object sender, GridViewRowEventArgs e)
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