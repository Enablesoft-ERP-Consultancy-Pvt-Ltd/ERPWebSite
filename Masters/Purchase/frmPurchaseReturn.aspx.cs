using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Purchase_frmPurchaseReturn : System.Web.UI.Page
{
    //string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            ViewState["ID"] = 0;
            ViewState["DetailID"] = 0;
            ViewState["IsEdit"] = 0;
            ViewState["GatePassNo"] = 0;
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyno"].ToString());
            hncomp.Value = VarCompanyNo.ToString();
            //hncomp.Value = VarCompanyNo.ToString();
            string Str = "Select CI.CompanyId,CompanyName from CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserid"] + " And CI. MasterCompanyId=" + Session["varMasterCompanyIDForERP"].ToString() + @" Order By CompanyName";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, Ds, 0, true, "--SELECT--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            BindEmp();
           
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        //show edit button
        if (Session["canedit"].ToString() == "0") //non authenticated person
        {
            CHKEdit.Enabled = false;
        }
    }
    protected void BindEmp()
    {
        string Str = @"select Distinct E.EMpId,E.Empname +'/'+Address 
        From Empinfo E(nolock)
        JOIN PurchaseReceiveMaster PM(nolock) ON PM.PartyId = E.EmpId And Challan_Status = 0 
        Where E.MasterCompanyid=" + Session["varMasterCompanyIDForERP"];

        Str = Str + " Order By E.Empname +'/'+Address";

        if (Convert.ToInt32(Session["varCompanyno"]) == 16 || Convert.ToInt32(Session["varCompanyno"]) == 28)
        {
            Str = @"Select Distinct E.EMpId, E.Empname + '/' + Address 
                From Empinfo E(nolock)
                JOIN VendorUser VU(nolock) ON VU.EmpID = E.EmpId And VU.UserID = " + Session["varuserid"] + @" 
                JOIN PurchaseReceiveMaster PM(nolock) ON PM.PartyId = E.EmpId And Challan_Status = 0 
                Where E.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + " Order By E.Empname +'/'+Address "; 
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDPartyName, Ds, 0, true, "--SELECT--");
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        BindEmp();
        DDChallanNo.Items.Clear();
        //if (CHKEdit.Checked == true)
        //{
        //    if (chkcomplete.Checked == false)
        //    {
        //        DDGatePass.Items.Clear();
        //    }
        //}        
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ViewState["IsEdit"].ToString() == "0")
        {
            str = @"select PurchaseReceiveId,BillNo+'|'+Replace(Convert(nvarchar(11),ReceiveDate,106),' ','-') As ChallanNo 
            From PurchaseReceiveMaster(Nolock) 
            Where CompanyId=" + DDCompany.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" And PartyId=" + DDPartyName.SelectedValue + " ";
            str = str + " AND Challan_Status= 0";
            //if (chkcomplete.Checked == true)
            //{
            //    str = str + " AND Challan_Status= 1";
            //}
            //else
            //{
            //    str = str + " AND Challan_Status= 0";
            //}
            str = str + " Order by ReceiveDate";
        }
        else
        {
            str = @"select PurchaseReceiveId,BillNo+'|'+Replace(Convert(nvarchar(11),ReceiveDate,106),' ','-') As ChallanNo 
            From PurchaseReceiveMaster(Nolock)  
            Where CompanyId=" + DDCompany.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" And 
            PartyId=" + DDPartyName.SelectedValue + @" AND BillNo in (Select distinct ChallanNO from PurchaseReturnMaster Where PartyID=" + DDPartyName.SelectedValue + @") ";
            str = str + " AND Challan_Status= 0";
            //if (chkcomplete.Checked == true)
            //{
            //    str = str + " AND Challan_Status= 1";
            //}
            //else
            //{
            //    str = str + " AND Challan_Status= 0";
            //}
            str = str + " Order by ReceiveDate";
        }
        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");
    }

    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IsEdit"].ToString() == "0")
        {
            Fillgrid();
        }
        else
        {
            //string str = "select distinct GatePassNo,GatePassNo as GatePass from purchasereturnmaster Where challanNo= left('" + DDChallanNo.SelectedItem.Text.Trim() + "', charindex('/','" + DDChallanNo.SelectedItem.Text.Trim() + "')-1)  Order by GatePassNo";
            string str = @"Select Distinct a.GatePassNo, a.GatePassNo GatePass 
                From PurchaseReturnMaster a(Nolock) 
                JOIN PurchaseReturnDetail b(Nolock) ON b.ID = a.ID 
                Where b.PurchaseReceiveID = " + DDChallanNo.SelectedValue + @" 
                    And challanNo= left('" + DDChallanNo.SelectedItem.Text.Trim() + "', charindex('|','" + DDChallanNo.SelectedItem.Text.Trim() + @"') - 1) 
                Order by a.GatePassNo ";
            UtilityModule.ConditionalComboFill(ref DDGatePass, str, true, "-Select-");
        }
    }
    protected void Fillgrid()
    {
        string str = "";
        DGItemDetail.DataSource = null;
        DGItemDetail.DataBind();
        if (ViewState["IsEdit"].ToString() == "0")
        {
            str = @"Select Item_Name,0 As DetailID, QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
                     GodownName,LotNo,ROUND(Qty-isnull(Pd.Bellwt,0),3) as Qty,ROUND((Qty-isnull(Pd.Bellwt,0)) - [dbo].[Get_PReceiveBalQty] (PD.PurchaseReceiveDetailId,PD.Finishedid,0,0),3) As BalQty,0 As ReturnQty,Isnull(Rate,0) Rate,PM.PurchaseReceiveId,PurchaseReceiveDetailId,Finishedid,GM.GodownId,PartyId,'' As Remark,PD.unitid 
                     from PurchaseReceiveMaster PM,PurchaseReceiveDetail PD,GodownMaster GM,V_FinishedItemDetail V 
                     Where PM.PurchaseReceiveId=PD.PurchaseReceiveId And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID And 
                     PD.StockUpdateFlag = 1 And PM.PurchaseReceiveId=" + DDChallanNo.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + @" And 
                     PartyId=" + DDPartyName.SelectedValue;
        }
        else
        {
            str = @"Select 0 as ID,0 As DetailID,Item_Name, QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
                    GodownName,LotNo,ROUND(Qty-isnull(pd.bellwt,0),3) as qty,ROUND((PD.Qty-isnull(pd.bellwt,0)) - [dbo].[Get_PReceiveBalQty] (PD.PurchaseReceiveDetailId,PD.Finishedid,0,0),3) As BalQty,0 As ReturnQty,Isnull(Rate,0) Rate,
                    PM.PurchaseReceiveId,PurchaseReceiveDetailId,Finishedid,GM.GodownId,PartyId,'' As Remark,PD.Unitid 
                    from PurchaseReceiveMaster PM,PurchaseReceiveDetail PD,GodownMaster GM,V_FinishedItemDetail V 
                    Where PM.PurchaseReceiveId=PD.PurchaseReceiveId And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID  And 
                    PD.PurchaseReceiveDetailId Not IN (select Distinct PurchaseReceiveDetailID from PurchaseReturnDetail) And 
                    PD.StockUpdateFlag = 1 And PM.PurchaseReceiveId=" + DDChallanNo.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + "   And PartyId=" + DDPartyName.SelectedValue + @"
                    Union
                    Select Distinct  1 as ID,1 As DetailID, Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
                    GodownName,PD.LotNo,ROUND(PRD.Qty-isnull(Prd.bellwt,0),3) as Qty,ROUND((PRD.Qty-isnull(Prd.bellwt,0)) - [dbo].[Get_PReceiveBalQty] (PD.PurchaseReceiveDetailId,PD.Finishedid,0,0),3) As BalQty,[dbo].[Get_PurReturnQty] (PD.PurchaseReceiveDetailId,PD.Finishedid,0," + DDGatePass.SelectedValue + @") As ReturnQty,Isnull(PD.Rate,0) Rate,
                    PD.PurchaseReceiveId,PD.PurchaseReceiveDetailId,PD.Finishedid,GM.GodownId,PM.PartyId,PD.Remarks As Remark,PD.unitid 
                    from PurchaseReturnMaster PM,PurchaseReturnDetail PD,GodownMaster GM,V_FinishedItemDetail V ,purchasereceivedetail PRD
                    Where PM.ID=PD.ID and PRD.PurchaseReceiveDetailId=PD.PurchaseReceiveDetailId
                    And PRD.StockUpdateFlag = 1 And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID And 
                    PM.ChallanNo= left('" + DDChallanNo.SelectedItem.Text.Trim() + "', charindex('|','" + DDChallanNo.SelectedItem.Text.Trim() + @"')-1) And 
                    PM.Companyid=" + DDCompany.SelectedValue + "  And PartyId=" + DDPartyName.SelectedValue + @" 
                    Order By Item_Name";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGItemDetail.DataSource = ds.Tables[0];
            DGItemDetail.DataBind();
        }
    }
    protected void DGItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string Str;
        Double varbal = 0;
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            if (e.CommandName.Equals("save"))
            {
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                int index = gvr.RowIndex;
                GridViewRow row = DGItemDetail.Rows[index];
                TextBox TxtReturnQty = (TextBox)row.FindControl("txtReturnQty");
                TextBox TxtRate = (TextBox)row.FindControl("txtRate");
                TextBox Txtremark = (TextBox)row.FindControl("Txtremark");
                Label lblFinishedid = (Label)row.FindControl("lblFinishedid");
                Label lblPurchaseReceiveId = (Label)row.FindControl("lblPurchaseReceiveId");
                Label lblPurchaseReceiveDetailId = (Label)row.FindControl("lblPurchaseReceiveDetailId");
                Label lblGodownid = (Label)row.FindControl("lblGodownid");
                Label lblPartyId = (Label)row.FindControl("lblPartyId");
                Label lblunitid = (Label)row.FindControl("lblunitid");

                string varlotno = Server.HtmlDecode(row.Cells[3].Text);

                if (ViewState["IsEdit"].ToString() == "1")
                {
                    ViewState["DetailID"] = Convert.ToInt32(DGItemDetail.DataKeys[row.RowIndex].Values[0].ToString());
                    if (Convert.ToInt32(ViewState["DetailID"].ToString()) > 0)
                    {
                        Str = @"select  [dbo].[Get_PReceiveBalQty] (" + lblPurchaseReceiveDetailId.Text + ", " + lblFinishedid.Text + ",1,'" + ViewState["GatePassNo"] + "')";
                    }
                    else
                    {
                        Str = @"select  [dbo].[Get_PReceiveBalQty] (" + lblPurchaseReceiveDetailId.Text + ", " + lblFinishedid.Text + ",0,'" + ViewState["GatePassNo"] + "')";
                    }
                }
                else
                {
                    Str = @"select  [dbo].[Get_PReceiveBalQty] (" + lblPurchaseReceiveDetailId.Text + ", " + lblFinishedid.Text + ",0,'" + ViewState["GatePassNo"] + "')";
                }
                varbal = Convert.ToDouble(SqlHelper.ExecuteScalar(tran, CommandType.Text, Str));
                Double varQty = Convert.ToDouble(Server.HtmlDecode(row.Cells[4].Text));
                varbal = varQty - varbal;
                // float varBalQty = float.Parse (Server.HtmlDecode(row.Cells[5].Text));
                if (float.Parse(TxtReturnQty.Text == " " ? "0" : TxtReturnQty.Text) == 0)
                {
                    MessageSave("Fill the return quantity!");
                    return;
                }
                if (varbal >= Convert.ToDouble(TxtReturnQty.Text == " " ? "0" : TxtReturnQty.Text))
                {
                    SqlParameter[] _param = new SqlParameter[20];
                    _param[0] = new SqlParameter("@PartyID", lblPartyId.Text);
                    _param[1] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
                    _param[2] = new SqlParameter("@UserID", Session["varuserid"]);
                    _param[3] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
                    _param[4] = new SqlParameter("@GatePassNo", SqlDbType.NVarChar, 50);
                    _param[4].Direction = ParameterDirection.InputOutput;
                    if (ViewState["IsEdit"].ToString() == "0")
                    {
                        _param[4].Value = TxtGatepassno.Text.Trim().ToUpper();
                    }
                    else
                    {
                        _param[4].Value = ViewState["GatePassNo"];
                    }
                    _param[5] = new SqlParameter("@ChallanNo", DDChallanNo.SelectedValue);
                    _param[6] = new SqlParameter("@Date", TxtDate.Text);
                    _param[7] = new SqlParameter("@PurchaseReceiveID", lblPurchaseReceiveId.Text);
                    _param[8] = new SqlParameter("@PurchaseReceiveDetailID", lblPurchaseReceiveDetailId.Text);
                    _param[9] = new SqlParameter("@FinishedID", lblFinishedid.Text);
                    _param[10] = new SqlParameter("@Mode", ViewState["IsEdit"].ToString());
                    _param[11] = new SqlParameter("@GodownID", lblGodownid.Text);
                    _param[12] = new SqlParameter("@LotNo", varlotno);
                    _param[13] = new SqlParameter("@Qty", TxtReturnQty.Text);
                    _param[14] = new SqlParameter("@Rate", TxtRate.Text);
                    _param[15] = new SqlParameter("@Remarks", Txtremark.Text);
                    _param[16] = new SqlParameter("@ID", ViewState["ID"]);
                    _param[16].Direction = ParameterDirection.InputOutput;
                    _param[17] = new SqlParameter("@DetailID", ViewState["DetailID"]);
                    _param[17].Direction = ParameterDirection.InputOutput;
                    _param[18] = new SqlParameter("@Message", SqlDbType.NVarChar, 250);
                    _param[18].Direction = ParameterDirection.InputOutput;
                    _param[19] = new SqlParameter("@Unitid",lblunitid.Text);

                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_PurchaseReturn_Save", _param);

                    TxtGatepassno.Text = _param[4].Value.ToString();
                    ViewState["GatePassNo"] = _param[4].Value;
                    ViewState["ID"] = _param[16].Value;
                    ViewState["DetailID"] = _param[17].Value;                    
                    MessageSave(_param[18].Value.ToString());
                    lblmsg.Text = _param[18].Value.ToString();
                    tran.Commit();
                    Fillgrid();
                }
                else
                {
                    MessageSave("Sorry,You can not return more than purchase balance quantity!");
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Purchase/FrmPurchaseReturn");
            MessageSave(ex.Message);
            lblmsg.Text = ex.Message;
            tran.Rollback();
            return;
        }
        finally
        {
            tran.Dispose();
            con.Close();
            con.Dispose();
        }

    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);
        }
        if (hncomp.Value == "10")
        {
            DGItemDetail.Columns[3].Visible = false;
        }


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

    protected void CHKEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKEdit.Checked == true)
        {
            ViewState["IsEdit"] = 1;
            tdGatepassDD.Visible = true;
        }
        else
        {
            ViewState["IsEdit"] = 0;
            tdGatepassDD.Visible = false;
        }
    }

    protected void DDGatePass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["GatePassNo"] = DDGatePass.SelectedValue;
        Fillgrid();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            string Qry = "";
            Qry = @"Select Distinct PM.ID,PD.DetailID, Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
            GodownName,PD.LotNo,PRD.Qty,PRD.Qty -  [dbo].[Get_PReceiveBalQty] (PD.PurchaseReceiveDetailId,PD.Finishedid,0,0) As BalQty,PD.QTY As ReturnQty,Isnull(PD.Rate,0) Rate,
            PD.Remarks As Remark ,e.EmpName,BM.BranchName CompanyName,BM.BranchAddress CompAdd, C.TINNO,C.Email,BM.PhoneNo CompTel,C.CompFax,PM.GatePassNo,Pm.ChallanNo,
            PM.Date,isnull(PM.MasterCompanyId,0) as MasterCompanyId, PRD.SGST/2 SGST, PRD.SGST/2 CGST, PRD.IGST, 
            PD.QTY * Isnull(PD.Rate,0) * 0.01 * PRD.SGST/2 SGSTAmt, PD.QTY * Isnull(PD.Rate,0) * 0.01 * PRD.SGST/2 CGSTAmt, PD.QTY * Isnull(PD.Rate,0) * 0.01 * PRD.IGST IGSTAmt 
            from PurchaseReturnMaster PM(Nolock),PurchaseReturnDetail PD(Nolock),GodownMaster GM(Nolock),V_FinishedItemDetail V(Nolock),purchasereceivedetail PRD(Nolock),
            EmpInfo E(Nolock), CompanyInfo C(Nolock), BRANCHMASTER BM(Nolock) 
            Where PM.ID=PD.ID and PRD.PurchaseReceiveDetailId=PD.PurchaseReceiveDetailId And E.EmpID=PM.PartyID AND C.Companyid=PM.Companyid
            And C.MasterCompanyid=PM.MasterCompanyid
            And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID 
            And BM.ID= PM.BranchID And PM.GatePassNo=" + ViewState["GatePassNo"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptPurchaseReturn.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseReturn.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                MessageSave("No Record(s) Found!");
                return;
            }
        }
        catch (Exception ex)
        {
            MessageSave(ex.Message);
            return;

        }
    }
    //protected void DGItemDetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
}