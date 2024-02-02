using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_FrmDebitnote : System.Web.UI.Page
{
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string qry = "";
        if (!IsPostBack)
        {
            if (Session["varMasterCompanyIDForERP"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            ViewState["ID"] = 0;
            ChkPurchase.Checked = true;
            UtilityModule.ConditionalComboFill(ref ddCompName, "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Companyname", true, "Select Comp Name");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomer, "Select Customerid,customercode from customerinfo Where MasterCompanyID=" + Session["varMasterCompanyIDForERP"] + " order by customercode", true, "Select Comp Name");
            if (ChkPurchase.Checked == true)
            {
                qry = @"Select Distinct ITEM_FINISHED_ID,ProductCode from Item_Parameter_Master IPM, PurchaseIndentIssuetran PIT
                        Where IPM.ITEM_FINISHED_ID=PIT.FinishedID AND  MasterCompanyID=" + ddCompName.SelectedValue + "  AND ProductCode <> '' ";
            }
            else
            {
                qry = @"Select Distinct ITEM_FINISHED_ID,ProductCode from Item_Parameter_Master IPM, PP_ProcessRecTran PRT
                        Where IPM.ITEM_FINISHED_ID=PRT.FinishedID AND  MasterCompanyID=" + ddCompName.SelectedValue + " AND ProductCode <> ''";
            }
            qry = qry + "Order By ProductCode";
            UtilityModule.ConditionalComboFill(ref DDItemCode, qry, true, "--Select--");

        }

    }
    protected void DDItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkPurchase.Checked == true)
        {
            str = @"select Distinct PIT.Finished_Type_Id, FINISHED_TYPE_NAME from PurchaseIndentIssuetran PIT, Finished_Type FT where PIT.Finished_Type_Id= FT.id AND 
                    finishedid=" + DDItemCode.SelectedValue + "  ";
        }
        else
        {
            str = @"select Distinct ID.O_Finished_Type_Id, FINISHED_TYPE_NAME from IndentDetail ID, Finished_Type FT where ID.O_Finished_Type_Id= FT.id AND 
                    Ifinishedid=" + DDItemCode.SelectedValue + " AND ID.O_Finished_Type_Id in(select Distinct Finish_Type from PP_ProcessRecTran where FinishedID=" + DDItemCode.SelectedValue + ") ";
        }
        str = str + "  ORder BY FINISHED_TYPE_NAME";
        UtilityModule.ConditionalComboFill(ref DDFinish, str, true, "--Select--");

    }
    protected void DDFinish_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkPurchase.Checked == true)
        {
            str = @"Select Distinct OM.OrderID,OM.CustomerOrderNo+ ' /' +OM.LocalOrder as CustomerOrder from PurchaseIndentIssue PII, OrderMaster OM ,
                    PurchaseIndentIssuetran PIT
                    Where OM.OrderID=PII.OrderID AND PII.PIndentIssueID=PIT.PIndentIssueID AND PIT.FinishedID=" + DDItemCode.SelectedValue + @"
                    AND PIT.Finished_Type_Id=" + DDFinish.SelectedValue;
        }
        else
        {
            str = @"Select Distinct OM.OrderID,OM.CustomerOrderNo+ ' /' +OM.LocalOrder as CustomerOrder from IndentMaster IM, OrderMaster OM , IndentDetail ID
                    Where OM.OrderID=ID.OrderID AND IM.IndentID=ID.IndentID AND ID.IFinishedID=" + DDItemCode.SelectedValue + "  AND ID.O_Finished_Type_Id=" + DDFinish.SelectedValue;
        }
        str = str + "ORDER by OM.CustomerOrderNo+ ' /' +OM.LocalOrder";
        UtilityModule.ConditionalComboFill(ref ddOrder, str, true, "--Select--");
    }
    protected void ddOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkPurchase.Checked == true)
        {
            str = @"select Distinct PII.PindentIssueid, PII.ChallanNo from PurchaseIndentIssue PII, PurchaseIndentIssuetran PIT where PII.PindentIssueid=PIT.PindentIssueid 
                AND Orderid=" + ddOrder.SelectedValue + " AND PIT.FinishedID=" + DDItemCode.SelectedValue + " and PIT.FINISHED_TYPE_ID=" + DDFinish.SelectedValue + @"
                ORDER by PII.ChallanNo  ";
        }
        else
        {
            str = @"select Distinct IM.IndentID, IM.IndentNo from IndentMaster IM, IndentDetail ID where IM.IndentID=ID.IndentID
                AND Orderid=" + ddOrder.SelectedValue + " AND ID.IFinishedID=" + DDItemCode.SelectedValue + " and ID.O_Finished_Type_Id=" + DDFinish.SelectedValue + @"
                ORDER by IM.IndentNo";
        }
        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkPurchase.Checked == true)
        {
            str = @"select e.EmpID,e.EmpName from PurchaseIndentIssue PII, EMPINFO E where PII.PartyID=E.EmpID AND PII.PindentIssueid=" + DDChallanNo.SelectedValue;
        }
        else
        {
            str = "select e.EmpID,e.EmpName from IndentMaster IM, EMPINFO E where IM.PartyID=E.EmpID AND IM.IndentID=" + DDChallanNo.SelectedValue;
        }
        str = str + "  Order BY e.EmpName";
        UtilityModule.ConditionalComboFill(ref DDParty, str, true, "--Select--");
        if (ChkEdit.Checked == true)
        {
            str = "Select Distinct ID, ID DNNO  From debitnote Where Challanno=" + DDChallanNo.SelectedValue + " Order By DNNO";
            UtilityModule.ConditionalComboFill(ref DDDNno, str, true, "--Select--");
        }
    }
    protected void DDParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkPurchase.Checked == true)
        {
            str = @"select Max(PIT.Rate) RATE,SUM(PIT.QUANTITY) QUANTITY, SUM(AMOUNT) AMOUNT from PurchaseIndentIssue PII, PurchaseIndentIssuetran PIT where PII.PindentIssueid=PIT.PindentIssueid 
            AND PIT.FinishedID=" + DDItemCode.SelectedValue + " and PIT.FINISHED_TYPE_ID=" + DDFinish.SelectedValue + " AND PII.PartyID=" + DDParty.SelectedValue + " AND PII.PindentIssueid=" + DDChallanNo.SelectedValue + @"
            GROUP BY PIT.FinishedID,PIT.FINISHED_TYPE_ID,PII.PartyID,PII.Challanno";
        }
        else
        {
            str = @"select Max(ID.Rate) RATE,sum(PRT.RecQUANTITY) QUANTITY, sum(PRT.RecQUANTITY) *Max(ID.Rate) As AMOUNT from IndentMaster IM, IndentDetail ID, PP_ProcessRecTran PRT
                 where PRT.FinishedID=ID.IFinishedID AND ID.O_Finished_Type_Id=PRT.Finish_Type AND IM.IndentID=PRT.IndentID AND IM.PartyID=" + DDParty.SelectedValue + " AND IM.IndentID=" + DDChallanNo.SelectedValue + @"
                AND PRT.FinishedID=" + DDItemCode.SelectedValue + " and ID.O_FINISHED_TYPE_ID=" + DDFinish.SelectedValue + " AND ID.OrderID=" + ddOrder.SelectedValue + @"
                GROUP BY ID.IFinishedID,PRT.FINISH_TYPE,IM.PartyID,IM.IndentID";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtRate.Text = ds.Tables[0].Rows[0]["RATE"].ToString();
            txtQty.Text = ds.Tables[0].Rows[0]["QUANTITY"].ToString();
            TxtAmount.Text = ds.Tables[0].Rows[0]["AMOUNT"].ToString();
        }
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        TxtAmount.Text = (Convert.ToInt32(txtQty.Text == "" ? "0" : txtQty.Text) * Convert.ToInt32(TxtRate.Text == "" ? "0" : TxtRate.Text)).ToString();
    }
    protected void TxtRate_TextChanged(object sender, EventArgs e)
    {
        TxtAmount.Text = (Convert.ToInt32(txtQty.Text == "" ? "0" : txtQty.Text) * Convert.ToInt32(TxtRate.Text == "" ? "0" : TxtRate.Text)).ToString();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlParameter[] _param = new SqlParameter[13];
        _param[0] = new SqlParameter("@ID", ViewState["ID"]);
        _param[0].Direction = ParameterDirection.InputOutput;
        _param[1] = new SqlParameter("@CompanyID", Session["varMasterCompanyIDForERP"]);
        _param[2] = new SqlParameter("@FinishedID", DDItemCode.SelectedValue);
        _param[3] = new SqlParameter("@Finished_Type", DDFinish.SelectedValue);
        _param[4] = new SqlParameter("@OrderID", ddOrder.SelectedValue);
        _param[5] = new SqlParameter("@ChallanNO", DDChallanNo.SelectedValue);
        _param[6] = new SqlParameter("@PartyID", DDParty.SelectedValue);
        _param[7] = new SqlParameter("@Qty", txtQty.Text == "" ? "0" : txtQty.Text);
        _param[8] = new SqlParameter("@Rate", TxtRate.Text == "" ? "0" : TxtRate.Text);
        _param[9] = new SqlParameter("@Amount", TxtAmount.Text == "" ? "0" : TxtAmount.Text);
        _param[10] = new SqlParameter("@Date", txtdate.Text);
        _param[11] = new SqlParameter("@Remarks", Txtremarks.Text);
        _param[12] = new SqlParameter("@DebitType", ChkPurchase.Checked == true ? 0 : 1);
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Save_DebitNote", _param);
        // ViewState["ID"] = _param[0].Value;
        MessageSave("Record(s) has been saved successfully! Serial No is: " + _param[0].Value.ToString());
        ViewState["ID"] = 0;
        FillGrid(Convert.ToInt32(_param[0].Value));
    }
    private void FillGrid(int id)
    {
        string Qry = @"Select DN.ID,IPM.PRODUCTCODE,FINISHED_TYPE_NAME,OM.localorder+'/'+OM.CustomerOrderNo ORDERNO,CHALLANNO,E.EMPNAME,QTY,RATE,AMOUNT,
                        REPLACE (CONVERT(NVARCHAR(20), DN.Date,106),' ','-') AS DATE,DN.REMARKS,case when DebitType=0 then 'PURCHASE' Else 'PRODUCTION' End AS TYPE
                        From debitnote DN, OrderMaster OM, Item_Parameter_Master IPM, Finished_Type FT, EMPiNFO E
                        where DN.OrderID=OM.OrderID AND DN.FINISHEDID=IPM.ITEM_FINISHED_ID AND FT.ID=DN.FINISHED_TYPE AND E.EMPID=DN.PARTYID AND DN.id=" + id;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
        DGDebitNote.DataSource = ds;
        DGDebitNote.DataBind();
       // DGDebitNote.Columns[0].Visible = false;
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEdit.Checked == true)
        {
            TDDNno.Visible = true;
        }
        else
        {
            TDDNno.Visible = false;
        }
    }
    protected void DDDNno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid(Convert.ToInt32(DDDNno.SelectedValue));
    }
    protected void DGDebitNote_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ID"] = DGDebitNote.DataKeyNames;
        int i = Convert.ToInt32(DGDebitNote.SelectedIndex.ToString());
        txtQty.Text = DGDebitNote.Rows[i].Cells[6].Text;
        TxtRate.Text = DGDebitNote.Rows[i].Cells[7].Text;
        TxtAmount.Text= DGDebitNote.Rows[i].Cells[8].Text;
        txtdate.Text = DGDebitNote.Rows[i].Cells[9].Text;
        Txtremarks.Text = DGDebitNote.Rows[i].Cells[10].Text;


    }
    protected void DGDebitNote_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGDebitNote_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDebitNote, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGDebitNote_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //ViewState["Del"] = 1;
        int i = Convert.ToInt32(DGDebitNote.DataKeys[e.RowIndex].Value);
        SqlParameter[] _param = new SqlParameter[3];
        string str = "Delete DebitNote Where ID=" + i;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        str = @"Select DN.ID,IPM.PRODUCTCODE,FINISHED_TYPE_NAME,OM.localorder+'/'+OM.CustomerOrderNo ORDERNO,CHALLANNO,E.EMPNAME,QTY,RATE,AMOUNT,
                        REPLACE (CONVERT(NVARCHAR(20), DN.Date,106),' ','-') AS DATE,DN.REMARKS,case when DebitType=0 then 'PURCHASE' Else 'PRODUCTION' End AS TYPE
                        From debitnote DN, OrderMaster OM, Item_Parameter_Master IPM, Finished_Type FT, EMPiNFO E
                        where DN.OrderID=OM.OrderID AND DN.FINISHEDID=IPM.ITEM_FINISHED_ID AND FT.ID=DN.FINISHED_TYPE AND E.EMPID=DN.PARTYID AND CHALLANNO= " + DDChallanNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGDebitNote.DataSource = ds;
        DGDebitNote.DataBind();
       
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
//        protected void DeleteCustomer(object sender, EventArgs e)
//{
//    LinkButton lnkRemove = (LinkButton)sender;
//    SqlConnection con = new SqlConnection(strConnString);
//    SqlCommand cmd = new SqlCommand();
//    cmd.CommandType = CommandType.Text;
//    cmd.CommandText = "delete from  customers where " +
//    "CustomerID=@CustomerID;" +
//     "select CustomerID,ContactName,CompanyName from customers";
//    cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar).Value
//        = lnkRemove.CommandArgument;
//    GridView1.DataSource = GetData(cmd);
//    GridView1.DataBind();
//}
        LinkButton lnkRemove = (LinkButton)sender;
        int i = Convert.ToInt32(DGDebitNote.SelectedIndex.ToString());
        string str = "Delete DebitNote Where ID=" + lnkRemove.CommandArgument;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        str = @"Select DN.ID,IPM.PRODUCTCODE,FINISHED_TYPE_NAME,OM.localorder+'/'+OM.CustomerOrderNo ORDERNO,CHALLANNO,E.EMPNAME,QTY,RATE,AMOUNT,
                        REPLACE (CONVERT(NVARCHAR(20), DN.Date,106),' ','-') AS DATE,DN.REMARKS,case when DebitType=0 then 'PURCHASE' Else 'PRODUCTION' End AS TYPE
                        From debitnote DN, OrderMaster OM, Item_Parameter_Master IPM, Finished_Type FT, EMPiNFO E
                        where DN.OrderID=OM.OrderID AND DN.FINISHEDID=IPM.ITEM_FINISHED_ID AND FT.ID=DN.FINISHED_TYPE AND E.EMPID=DN.PARTYID AND CHALLANNO= " + DDChallanNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGDebitNote.DataSource = ds;
        DGDebitNote.DataBind();
    }
    protected void DGDebitNote_RowCommand(object sender, GridViewCommandEventArgs e)
    {
//        if (ViewState["Del"] == "1")
//        {
//            int i = Convert.ToInt32(DGDebitNote.DataKeys[e.CommandName].Value);
//            SqlParameter[] _param = new SqlParameter[3];
//            string str = "Delete DebitNote Where ID=" + i;
//            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            str = @"Select DN.ID,IPM.PRODUCTCODE,FINISHED_TYPE_NAME,OM.localorder+'/'+OM.CustomerOrderNo ORDERNO,CHALLANNO,E.EMPNAME,QTY,RATE,AMOUNT,
//                                    REPLACE (CONVERT(NVARCHAR(20), DN.Date,106),' ','-') AS DATE,DN.REMARKS,case when DebitType=0 then 'PURCHASE' Else 'PRODUCTION' End AS TYPE
//                                    From debitnote DN, OrderMaster OM, Item_Parameter_Master IPM, Finished_Type FT, EMPiNFO E
//                                    where DN.OrderID=OM.OrderID AND DN.FINISHEDID=IPM.ITEM_FINISHED_ID AND FT.ID=DN.FINISHED_TYPE AND E.EMPID=DN.PARTYID AND CHALLANNO= " + DDChallanNo.SelectedValue;
//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            DGDebitNote.DataSource = ds;
//            DGDebitNote.DataBind();
//        }
    }
}