using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_frmIndentRawMaterialReturn : System.Web.UI.Page
{
    string msg = "", str = "";
    static string btnclickflag = "";
    static int index = 0;
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
            str = "Select CI.CompanyId,CompanyName from CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserid"] + " And CI. MasterCompanyId=" + Session["varCompanyNo"].ToString() + @" Order By CompanyName ";
            // DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFill(ref DDCompany, str, true, "--SELECT--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            str = "  Select  DISTINCT PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master  PNM, PP_ProcessRECMaster PRM WHERE PNM.PROCESS_NAME_ID= PRM.Processid ";
            if (DDCompany.Items.Count > 0)
            {
                str = str + " AND PRM.COMPANYID=" + DDCompany.SelectedValue;
            }
            str = str + "  ORDER BY PROCESS_NAME";
            UtilityModule.ConditionalComboFill(ref ddProcessName, str, true, "--SELECT--");

            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["varCompanyNo"].ToString() == "21" && Session["usertype"].ToString() == "1")
            {
                TRGVIndentRawReturn.Visible = true;
            }
        }
    }
    protected void CHKEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKEdit.Checked == true)
        {
            ViewState["IsEdit"] = 1;
            tdGatepassDD.Visible = true;
            tdGTPassTxt.Visible = false;
        }
        else
        {
            ViewState["IsEdit"] = 0;
            tdGatepassDD.Visible = false;
            tdGTPassTxt.Visible = true;
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        string str = "";
        if (DDCompany.SelectedIndex > 0)
        {
            str = "  Select  DISTINCT PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master  PNM, PP_ProcessRECMaster PRM WHERE PNM.PROCESS_NAME_ID= PRM.Processid ";
            str = str + " AND PRM.COMPANYID=" + DDCompany.SelectedValue;

            str = str + "  ORDER BY PROCESS_NAME";
            UtilityModule.ConditionalComboFill(ref ddProcessName, str, true, "--SELECT--");
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        str = @"select Distinct E.EMpId,E.Empname +'/'+Address As EmpName from Empinfo E,PP_ProcessRECMaster PRM Where PRM.EmpId=E.EmpId 
                And E.MasterCompanyid= " + Session["varCompanyNo"] + "   AND PRM.PROCESSID=" + ddProcessName.SelectedValue + " Order By E.Empname +'/'+Address";
        UtilityModule.ConditionalComboFill(ref DDPartyName, str, true, "-Select-");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        str = @"select Distinct PM.PRMID,challanno As ChallanNo From PP_ProcessRECMaster PM,PP_ProcessRecTran PT 
        //                Where PM.prmid=Pt.Prmid And  CompanyId=" +DDCompany.SelectedValue + "  And  EmpID=" + DDPartyName.SelectedValue + @" 
        //                And PM.PrmId Not in(select Distinct ProcessRec_PrmId from RawMaterialPreprationHissab RMH,RawMaterialPreprationHissabDetail RHD 
        //                                        Where RMH.HissabId=RHD.HissabId And RMH.Hissabtype = 0 And CompanyId=" + DDCompany.SelectedValue + @" And 
        //                                        Processid=" + ddProcessName.SelectedValue + " And PartyId=" + DDPartyName.SelectedValue + @")  
        //                Order by challanno";

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            str = @"select Distinct PM.PRMID, ChallanNo + ' | ' + GatePassNo + ' | ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') ChallanNo 
                From PP_ProcessRECMaster PM JOIN PP_ProcessRecTran PT ON PM.prmid=Pt.Prmid               
                Where PM.CompanyId=" + DDCompany.SelectedValue + "  And  PM.EmpID=" + DDPartyName.SelectedValue + @" 
                And PM.PrmId Not in(select Distinct ProcessRec_PrmId from RawMaterialPreprationHissab RMH,RawMaterialPreprationHissabDetail RHD 
                                        Where RMH.HissabId=RHD.HissabId And RMH.Hissabtype = 0 And CompanyId=" + DDCompany.SelectedValue + @" And 
                                        Processid=" + ddProcessName.SelectedValue + " And PartyId=" + DDPartyName.SelectedValue + @")  
                Order by PM.PRMID Desc";
        }
        else
        {
            str = @"select Distinct PM.PRMID, GatePassNo + ' | ' + ChallanNo + ' | ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') ChallanNo 
                From PP_ProcessRECMaster PM JOIN PP_ProcessRecTran PT ON PM.prmid=Pt.Prmid               
                Where PM.CompanyId=" + DDCompany.SelectedValue + "  And  PM.EmpID=" + DDPartyName.SelectedValue + @" 
                And PM.PrmId Not in(select Distinct ProcessRec_PrmId from RawMaterialPreprationHissab RMH,RawMaterialPreprationHissabDetail RHD 
                                        Where RMH.HissabId=RHD.HissabId And RMH.Hissabtype = 0 And CompanyId=" + DDCompany.SelectedValue + @" And 
                                        Processid=" + ddProcessName.SelectedValue + " And PartyId=" + DDPartyName.SelectedValue + @")  
                Order by PM.PRMID Desc";
        }

       

        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "-Select-");
    }
    protected void Fillgrid()
    {
        string str = "";
        DGItemDetail.DataSource = null;
        DGItemDetail.DataBind();
        if (ViewState["IsEdit"].ToString() == "0")
        {
            str = @"Select Item_Name,0 As DetailID, QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
                    GodownName,LotNo,RecQuantity,RecQuantity-[dbo].[Get_RawReceiveBalQty] (PD.PRTID,PD.Finishedid,0,0) As BalQty,0 As ReturnQty, Isnull(INDENTID,0) IndentID,PM.PrmID,PrtID,Finishedid,GM.GodownId,EmpID,'' As Remark,PD.Unitid,PD.TagNo
                    from PP_ProcessRECMaster PM,PP_ProcessRECTran PD,GodownMaster GM,V_FinishedItemDetail V Where PM.PRMID=PD.PRMID And PD.Godownid=GM.GodownId 
                    And PD.FinishedId=V.ITEM_FINISHED_ID And PM.prmid=" + DDChallanNo.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + " And EmpID=" + DDPartyName.SelectedValue + " AND ProcessID=" + ddProcessName.SelectedValue + "  Order By Item_Name";
        }
        else
        {
            str = @"Select 0 as ID,0 As DetailID, Item_Name, QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
					GodownName,LotNo,RecQuantity,RecQuantity-[dbo].[Get_RawReceiveBalQty] (PD.PRTID,PD.Finishedid,0,0)  As BalQty,0 As ReturnQty, Isnull(INDENTID,0) IndentID,PM.PrmID,PrtID,Finishedid,GM.GodownId,EmpID,'' As Remark,PD.Unitid,PD.TagNo
					from PP_ProcessRECMaster PM,PP_ProcessRECTran PD,GodownMaster GM,V_FinishedItemDetail V Where PM.PRMID=PD.PRMID And PD.Godownid=GM.GodownId 
					And PD.FinishedId=V.ITEM_FINISHED_ID And PD.PRTID Not  IN (select Distinct PRTID from IndentRawReturnDetail)
					And PM.prmid= " + DDChallanNo.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + " And EmpID=" + DDPartyName.SelectedValue + " AND ProcessID=" + ddProcessName.SelectedValue + @" 
                    UNION
                    Select Distinct  1 as ID,1 As DetailID, Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
                    GodownName,PD.LotNo,PRD.RecQuantity,PRD.RecQuantity -[dbo].[Get_RawReceiveBalQty] (PD.PRTID,PD.Finishedid,0,0)  As BalQty, [dbo].[Get_IndentRawReturnQty] (PD.DetailID,PD.Finishedid,0,'" + DDGatePass.SelectedValue + @"') As ReturnQty,Isnull(PD.Indentid,0) Indentid,
                    PD.PRMID,PD.PRTID,PD.Finishedid,GM.GodownId,PM.PartyId,PD.Remarks As Remark,PD.Unitid,PD.TagNo
                    from IndentRawReturnMaster PM,IndentRawReturnDetail PD,GodownMaster GM,V_FinishedItemDetail V ,PP_ProcessRECTran PRD
                    Where PM.ID=PD.ID and PRD.PRTID=PD.PRTID
                    And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID And PD.PRMID = " + DDChallanNo.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + " And PartyId=" + DDPartyName.SelectedValue + @"
                    Order By Item_Name";
            //            str = @"Select Distinct PM.ID,PD.DetailID, Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt As ItemDescription,
            //                    GodownName,PD.LotNo,PRD.Qty,PRD.Qty - [dbo].[Get_PReceiveBalQty] (PD.PurchaseReceiveDetailId,PD.Finishedid) As BalQty,PD.QTY As ReturnQty,Isnull(PD.Rate,0) Rate,PD.PurchaseReceiveId,PD.PurchaseReceiveDetailId,PD.Finishedid,GM.GodownId,PM.PartyId,PD.Remarks As Remark 
            //                    from PurchaseReturnMaster PM,PurchaseReturnDetail PD,GodownMaster GM,V_FinishedItemDetail V ,purchasereceivedetail PRD
            //					  Where PM.ID=PD.ID and PRD.PurchaseReceiveDetailId=PD.PurchaseReceiveDetailId
            //                    And PD.Godownid=GM.GodownId And PD.FinishedId=V.ITEM_FINISHED_ID And PM.GatePassNo=" + DDGatePass.SelectedValue + " And PM.Companyid=" + DDCompany.SelectedValue + "  And PartyId=" + DDPartyName.SelectedValue;

        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGItemDetail.DataSource = ds.Tables[0];
            DGItemDetail.DataBind();
        }

    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IsEdit"].ToString() == "0")
        {
            Fillgrid();
        }
        else
        {
            string str = @"Select Distinct a.GatePassNo, a.GatePassNo GatePass 
                        From IndentRawReturnMaster a
                        JOIN IndentRawReturnDetail b ON b.ID = a.ID And b.PRMID = " + DDChallanNo.SelectedValue + " Order By GatePassNo";
            UtilityModule.ConditionalComboFill(ref DDGatePass, str, true, "-Select-");
            DGItemDetail.DataSource = null;
            DGItemDetail.DataBind();
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
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);

            if (Session["VarCompanyNo"].ToString() == "21" || Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28")
            {
                for (int i = 0; i < DGItemDetail.Columns.Count; i++)
                {
                    if (DGItemDetail.Columns[i].HeaderText == "TagNo")
                    {
                        DGItemDetail.Columns[i].Visible = true;
                    }
                }
            }
        }
    }
    protected void DGItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("save"))
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            index = gvr.RowIndex;
            SaveDetails(index);
        }
    }
    protected void SaveDetails(int index)
    {
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
            GridViewRow row = DGItemDetail.Rows[index];
            TextBox TxtReturnQty = (TextBox)row.FindControl("txtReturnQty");
            Label TxtRate = (Label)row.FindControl("LblIndentID");
            TextBox Txtremark = (TextBox)row.FindControl("Txtremark");
            Label lblFinishedid = (Label)row.FindControl("lblFinishedid");
            Label lblprmid = (Label)row.FindControl("lblPRMID");
            Label lblprtid = (Label)row.FindControl("lblPrtId");
            Label lblGodownid = (Label)row.FindControl("lblGodownid");
            Label lblPartyId = (Label)row.FindControl("lblPartyId");
            Label lblUnitid = (Label)row.FindControl("lblUnitid");
            Label lblTagNo = (Label)row.FindControl("lbltagno");

            string varlotno = Server.HtmlDecode(row.Cells[3].Text);

            if (ViewState["IsEdit"].ToString() == "1")
            {
                ViewState["DetailID"] = Convert.ToInt32(DGItemDetail.DataKeys[row.RowIndex].Values[0].ToString());
                if (string.IsNullOrEmpty( Convert.ToString(ViewState["GatePassNo"])))
                {
                    ViewState["GatePassNo"] = 0;
                }
                if (Convert.ToInt32(ViewState["DetailID"].ToString()) > 0)
                {
                    str = @"select  [dbo].[Get_RawReceiveBalQty] (" + lblprtid.Text + ", " + lblFinishedid.Text + ",1," + ViewState["GatePassNo"] + ")";
                }
                else
                {
                    str = @"select  [dbo].[Get_RawReceiveBalQty] (" + lblprtid.Text + ", " + lblFinishedid.Text + ",0," + ViewState["GatePassNo"] + ")";
                }
            }
            else if (ViewState["IsEdit"].ToString() == "0")
            {
                if (Session["varCompanyNo"].ToString() == "44")
                {
                    if (string.IsNullOrEmpty(ViewState["GatePassNo"].ToString()))
                    {
                        str = @"select  [dbo].[Get_RawReceiveBalQty] (" + lblprtid.Text + ", " + lblFinishedid.Text + ",0,0)";
                    }
                    else
                    {
                        str = @"select  [dbo].[Get_RawReceiveBalQty] (" + lblprtid.Text + ", " + lblFinishedid.Text + ",0," + ViewState["GatePassNo"] + ")";
                    }
                }
                else
                {
                    str = @"select  [dbo].[Get_RawReceiveBalQty] (" + lblprtid.Text + ", " + lblFinishedid.Text + ",0," + ViewState["GatePassNo"] + ")";
                
                }
            }
            varbal = Convert.ToDouble(SqlHelper.ExecuteScalar(tran, CommandType.Text, str));
            Double varQty = Convert.ToDouble(Server.HtmlDecode(row.Cells[5].Text));
            varbal = varQty - varbal;

            if (ViewState["IsEdit"].ToString() == "0")
            {
                if (float.Parse(TxtReturnQty.Text == " " ? "0" : TxtReturnQty.Text) == 0)
                {
                    MessageSave("Fill the return quantity!");
                    return;
                }
            }
            if (varbal >= Convert.ToDouble(TxtReturnQty.Text == " " ? "0" : TxtReturnQty.Text))
            {
                SqlCommand cmd = new SqlCommand("Pro_RawReturn_Save", con, tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;
                cmd.Parameters.AddWithValue("@PartyID", lblPartyId.Text);
                cmd.Parameters.AddWithValue("@CompanyID", DDCompany.SelectedValue);
                cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyNo"]);
                cmd.Parameters.Add("@GatePassNo", SqlDbType.NVarChar, 50);
                cmd.Parameters["@GatePassNo"].Direction = ParameterDirection.InputOutput;
                if (ViewState["IsEdit"].ToString() == "0")
                {
                    cmd.Parameters["@GatePassNo"].Value = TxtGatepassno.Text.Trim().ToUpper();
                }
                else
                {
                    cmd.Parameters["@GatePassNo"].Value = ViewState["GatePassNo"];
                }
                cmd.Parameters.AddWithValue("@ChallanNo", DDChallanNo.SelectedValue);
                cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
                cmd.Parameters.AddWithValue("@PrmID", lblprmid.Text);
                cmd.Parameters.AddWithValue("@PrtID", lblprtid.Text);
                cmd.Parameters.AddWithValue("@FinishedID", lblFinishedid.Text);
                cmd.Parameters.AddWithValue("@Mode", ViewState["IsEdit"].ToString());
                cmd.Parameters.AddWithValue("@GodownID", lblGodownid.Text);
                cmd.Parameters.AddWithValue("@LotNo", varlotno);
                cmd.Parameters.AddWithValue("@Qty", TxtReturnQty.Text);
                cmd.Parameters.AddWithValue("@IndentID", TxtRate.Text);
                cmd.Parameters.AddWithValue("@Remarks", Txtremark.Text);
                cmd.Parameters.Add("@ID", SqlDbType.Int);
                cmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@ID"].Value = ViewState["ID"];
                cmd.Parameters.Add("@DetailID", SqlDbType.Int);
                cmd.Parameters["@DetailID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@DetailID"].Value = ViewState["DetailID"];
                cmd.Parameters.AddWithValue("@ProcessID", ddProcessName.SelectedValue);
                cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 250);
                cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Unitid", lblUnitid.Text);
                cmd.Parameters.AddWithValue("@TagNo", lblTagNo.Text);

                cmd.ExecuteNonQuery();

                TxtGatepassno.Text = cmd.Parameters["@GatePassNo"].Value.ToString();
                ViewState["GatePassNo"] = cmd.Parameters["@GatePassNo"].Value.ToString();
                ViewState["ID"] = cmd.Parameters["@ID"].Value.ToString();
                ViewState["DetailID"] = cmd.Parameters["@DetailID"].Value.ToString();
                lblmsg.Text = cmd.Parameters["@Message"].Value.ToString();
                MessageSave(cmd.Parameters["@Message"].Value.ToString());
                tran.Commit();
                Fillgrid();

                if (cmd.Parameters["@GatePassNo"].Value.ToString() == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "NewForm()", true);
                }
            }
            else
            {
                MessageSave("Sorry,You can not return more than Issue balance quantity!");
                return;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/Rawmaterial/FrmPurchaseReturn");
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

    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void DDGatePass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["GatePassNo"] = DDGatePass.SelectedValue;
        Fillgrid();
        if (Session["varCompanyNo"].ToString() == "21" && Session["usertype"].ToString() == "1")
        {
            BindIndentRawReturnGridDetail();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            string Qry = "";
            Qry = @"Select Distinct PM.ID,PD.DetailID, Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt ItemDescription,
                GodownName,isnull(PD.LotNo,'') LotNo,PRD.RecQuantity,PRD.RecQuantity -  [dbo].[Get_RawReceiveBalQty] (PD.PRTID,PD.Finishedid,0,0) BalQty,
                PD.QTY ReturnQty, PD.Remarks Remark, e.EmpName, BM.BranchName CompanyName, BM.BranchAddress CompAdd, '' TINNO, '' Email, 
                BM.PhoneNo CompTel, '' CompFax, PM.GatePassNo, Pm.ChallanNo, PM.Date, PNM.PROCESS_NAME, om.LocalOrder, om.CustomerOrderNo, vi.IndentNo, 
                CI.customercode, isnull(PD.TagNo,'') TagNo
                From IndentRawReturnMAster PM 
                JOIN IndentRawReturnDetail PD ON PM.ID=PD.ID 
                JOIN GodownMaster GM ON PD.Godownid=GM.GodownId 
                JOIN V_FinishedItemDetail V ON PD.FinishedId=V.ITEM_FINISHED_ID 					
                JOIN PP_ProcessRectran PRD ON PRD.PRTID=PD.PRTID
                JOIN PP_ProcessRecMaster PRM ON PRM.PRMid=PRD.PRMid
                JOIN EmpInfo E ON E.EmpID=PM.PartyID
                --JOIN CompanyInfo C ON C.MasterCompanyid=PM.MasterCompanyid and C.Companyid=PM.Companyid
                JOIN PROCESS_NAME_MASTER PNM ON  PNM.PROCESS_NAME_ID=PRM.Processid
                JOIN V_Indent_OredrId VI ON Vi.IndentId=Pd.INDENTID
                JOIN OrderMaster OM ON OM.OrderId=VI.Orderid
                JOIN customerinfo CI ON om.customerid=CI.customerid
                JOIN BRANCHMASTER BM ON BM.ID = PM.BranchID 
                Where PM.GatePassNo='" + ViewState["GatePassNo"]+"'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "21")
                {
                    Session["rptFileName"] = "Reports/RptIndentRawReturnDuplicateKaysons.rpt";
                }
                else if (Session["VarCompanyNo"].ToString() == "43")
                {
                    Session["rptFileName"] = "Reports/RptIndentRawReturnDuplicateCI.rpt";
                }
                else
                {
                    Session["rptFileName"] = "Reports/RptIndentRawReturnDuplicate.rpt";
                }

                Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawReturn.xsd";
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
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }

    private void BindIndentRawReturnGridDetail()
    {
        SqlParameter[] _array = new SqlParameter[4];
        _array[0] = new SqlParameter("@GatePassNo", SqlDbType.VarChar,50);
        _array[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _array[3] = new SqlParameter("@UserId", SqlDbType.Int);
       

        _array[0].Value =DDGatePass.SelectedValue;
        _array[1].Value = DDCompany.SelectedValue;
        _array[2].Value = Session["VarCompanyNo"];
        _array[3].Value = Session["VarUserId"];


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentRawReturnDetail", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            GVIndentReturnDetail.DataSource = ds.Tables[0];
            GVIndentReturnDetail.DataBind();
        }
        else 
        { 
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); 
        }

    }
    protected void GVIndentReturnDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVIndentReturnDetail, "Select$" + e.Row.RowIndex);
            
        }
    }
    protected void GVIndentReturnDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int DetailId = Convert.ToInt32(GVIndentReturnDetail.DataKeys[e.RowIndex].Value);           
            SqlParameter[] arr = new SqlParameter[6];

            arr[0] = new SqlParameter("@DetailId", SqlDbType.Int);           
            arr[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[2] = new SqlParameter("@userid", Session["varuserid"]);
            arr[3] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

            arr[0].Value = DetailId;           
            arr[1].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_INDENTRAWRETURNQTY", arr);
            if (arr[1].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + arr[1].Value.ToString() + "');", true);
            }
            else
            {
                lblmsg.Text = "Row Item Deleted successfully.";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Fillgrid();
        BindIndentRawReturnGridDetail();
    }
}