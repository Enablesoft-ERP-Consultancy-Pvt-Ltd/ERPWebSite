using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.SqlTypes;
public partial class Masters_Purchase_PurchaseReceive : System.Web.UI.Page
{
    string MSg = "";
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);

        //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "showHideTransportDiv('" + chkTransportInformation.ClientID + "');", true);
        // ClientScript.RegisterClientScriptBlock(this.GetType(), "tmp", "<script type='text/javascript'>showHideTransportDiv('" + chkTransportInformation.ClientID + "');</script>", false);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["Gridstatus"] = 0;
            ViewState["PurchaseReceiveId"] = 0;
            ViewState["PurchaseReceiveDetailId"] = 0;
            hnprid.Value = "0";

            string Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by CompanyId 
            Select ID, BranchName 
            From BRANCHMASTER BM(nolock) 
            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, DSQ, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 1, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedIndexChanged();
            }

            TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtretdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyNo"].ToString());

            hncomp.Value = VarCompanyNo.ToString();
            switch (VarCompanyNo)
            {
                case 1:
                    itmcod.Visible = false;
                    TdFinish_Type.Visible = false;
                    break;
                case 2:
                    itmcod.Visible = true;
                    TdFinish_Type.Visible = true;
                    TD1.Visible = false;
                    break;
                case 3:
                    itmcod.Visible = false;
                    TdFinish_Type.Visible = false;
                    break;
                case 6:
                    TDLShort.Visible = true;
                    break;
                case 7:
                    //trvat.Visible = false;
                    tdrate.Visible = false;
                    tdamout.Visible = false;
                    tdlot.Visible = false;
                    TDtxtcst.Visible = false;
                    TDTxtnetamount.Visible = false;
                    TDtxtVat.Visible = false;
                    TxtReceiveDate.Enabled = false;
                    BtnPreview.Visible = false;
                    TDtxtremarks.Visible = false;
                    TDtxtmastremark.Visible = true;
                    TDtxtSGST.Visible = false;
                    TDtxtIGST.Visible = false;
                    TDTCS.Visible = false;
                    break;
                case 9:
                    //lblIsslotNo.Text = "Iss Lot No.";
                    //TDRecLotNo.Visible = true;
                    TDBillDate.Visible = true;
                    TDBillQty.Visible = true;
                    TRTransportBuilty.Visible = true;
                    TRParticular.Visible = true;
                    //txtBillDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    TD50MWait.Visible = true;
                    TDYarnShape.Visible = true;
                    Label31.Text = "Bag Qty";
                    txtrate.Enabled = true;

                    break;
                case 10:
                    tdlot.Visible = false;
                    itmcod.Visible = false;
                    TdFinish_Type.Visible = false;
                    break;
                case 12:
                    TDLShort.Visible = true;
                    break;
                case 16:
                    BtnComplete.Visible = true;
                    break;
                case 20:
                    DDPreviewType.SelectedValue = "1";
                    txtrate.Enabled = true;
                    break;
                case 22:
                    TDBillDate.Visible = true;
                    TDRectagNo.Visible = true;
                    Label12.Text = "Inwards No";
                    break;
                case 28:
                    BtnComplete.Visible = true;
                    break;
                case 30:
                    TDBillDate.Visible = true;
                    lblsrno.Visible = false;
                    break;
                case 14:
                    TDQtyWeight.Visible = true;                    
                    break;
            }
            ParameteLabel();
            Txtreturnqty.Text = "0";
            DDPartyName.Focus();
            if (variable.Carpetcompany == "1")
            {
                TDBaleNo.Visible = true;
                TDBellWt.Visible = true;
            }
            //show edit button
            if (Session["canedit"].ToString() == "0") //non authenticated person
            {
                ChkEditOrder.Enabled = false;
            }
            if (variable.VarPURCHASERECEIVEAUTOGENLOTNO == "1")
            {
                TDCompanyLotNo.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBINNO.Visible = true;
            }
            if (variable.VarMATERIALRECEIVEWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = true;
                FIlllegalvendor();
            }
            if (Session["UserType"].ToString() == "1")
            {
                txtrate.Enabled = true;
            }
        }
        Lblmessage.Visible = false;

    }
    protected void FIlllegalvendor()
    {
        string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
        if (Session["varCompanyno"].ToString() != "6")
        {
            Str = Str + "  AND DM.Departmentname='PURCHASE'";
        }
        Str = Str + "  Order By empname ";
        UtilityModule.ConditionalComboFill(ref DDlegalvendor, Str, true, "--Select Party--");
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        String Str = @"Select Distinct EI.EmpId,EI.EmpName 
        From PurchaseIndentIssue PII(Nolock) 
        JOIN EmpInfo EI(Nolock) ON EI.Empid = PII.Partyid And EI.MasterCompanyId=" + Session["varCompanyId"] + @" 
        Where PII.CompanyID = " + DDCompanyName.SelectedValue + " And IsNull(PII.BranchID, 0) = " + DDBranchName.SelectedValue + @" 
        order by ei.empname";

        if (Convert.ToInt32(Session["varCompanyNo"]) == 16 || Convert.ToInt32(Session["varCompanyNo"]) == 28)
        {
            Str = @"Select Distinct EI.EmpId,EI.EmpName 
                From PurchaseIndentIssue PII(Nolock) 
                JOIN EmpInfo EI(Nolock) ON EI.Empid = PII.Partyid And EI.MasterCompanyId=" + Session["varCompanyId"] + @" 
                JOIN VendorUser VU(nolock) ON VU.EmpID = EI.EmpId And VU.UserID = " + Session["varuserid"] + @" 
                Where PII.CompanyID = " + DDCompanyName.SelectedValue + " And IsNull(PII.BranchID, 0) = " + DDBranchName.SelectedValue + @" 
                order by ei.empname";
        }
        UtilityModule.ConditionalComboFill(ref DDPartyName, Str, true, "--Select Employee--");
        if (DDPartyName.Items.Count > 0)
        {
            DDPartyName.SelectedIndex = 0;
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Tdlegalvendor.Visible == true)
        {
            if (DDlegalvendor.Items.FindByValue(DDPartyName.SelectedValue) != null)
            {
                DDlegalvendor.SelectedValue = DDPartyName.SelectedValue;
            }
        }
        if (ChkEditOrder.Checked == false)
        {
            fill_order();
        }
        else
        {
            string ChallanNo = string.Empty;
            switch (Session["varcompanyid"].ToString())
            {
                case "9":
                    ChallanNo = "BillNo";
                    break;
                default:
                    ChallanNo = "receiveno+' / '+BillNo";
                    break;
            }
            UtilityModule.ConditionalComboFill(ref ddlrecchalanno, "select distinct PurchaseReceiveId," + ChallanNo + " as challanNo from PurchaseReceiveMaster  where partyid=" + DDPartyName.SelectedValue + " And CompanyId=" + DDCompanyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
            fill_order();
        }

    }
    private void fill_order()
    {
        string Postatus = string.Empty;
        //29-Dec-2015
        if (ChkEditOrder.Checked == true)
        {
            Postatus = "'Complete'" + "," + "'Pending'";
        }
        else
        {
            Postatus = "'Pending'";
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select * From PROCESS_NAME_MASTER PNM,Process_UserType PUT,UserType UT,
        NewUserDetail NUD Where PNM.PROCESS_NAME_ID=PUT.PRocessID And PUT.ID=UT.ID And ApprovalFlag=1 and UT.ID=NUD.UserType And PROCESS_NAME_ID=9 And PNM.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PII.PIndentIssueId,ChallanNo from PurchaseIndentIssue PII,Purchase_Approval PA 
            Where PII.PIndentIssueId=PA.PIndentIssueId And PII.Status in(" + Postatus + ") And PII.PartyId=" + DDPartyName.SelectedValue + @" And 
            PII.CompanyId=" + DDCompanyName.SelectedValue + "  And PII.MasterCompanyId=" + Session["varCompanyId"] + " And PII.BranchID = " + DDBranchName.SelectedValue + " Order By PII.PIndentIssueId desc", true, "--Select Order No--");
        }
        else
        {
            if (hncomp.Value == "7")
            {
                if (ChkEditOrder.Checked == true)
                {
                    UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PIndentIssueId,ChallanNo+' / '+isnull(om.localorder,' ') +' ' +isnull(om.CustomerOrderNo,' ') +'/ '+isnull(op.Purhasername,' ')
                    From PurchaseIndentIssue pii inner join ordermaster om On pii.orderid=om.orderid inner join OrderProcessPlanning OP On OP.OrderId=OM.OrderId
                    Where om.status=0 and processid=1 And PII.Status='Pending' and pii.PartyId=" + DDPartyName.SelectedValue + " And pii.CompanyId=" + DDCompanyName.SelectedValue + " And pii.MasterCompanyId=" + Session["varCompanyId"] + " order by PIndentIssueId", true, "--Select Order No--");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select Distinct pii.PIndentIssueId,ChallanNo+' / '+isnull(om.localorder,' ')  +' ' +isnull(om.CustomerOrderNo,' ') +'/ '+isnull(op.Purhasername,' ')
                    From PurchaseIndentIssue pii inner join Ordermaster om On pii.orderid=om.orderid inner join OrderProcessPlanning OP On OP.OrderId=OM.OrderId inner join 
                    V_PendingPurchaseReceive vp On vp.PIndentIssueId=pii.PIndentIssueId 
                    Where om.status=0 And  processid=1 and PII.Status='Pending' and pii.PartyId=" + DDPartyName.SelectedValue + " And pii.CompanyId=" + DDCompanyName.SelectedValue + " And pii.MasterCompanyId=" + Session["varCompanyId"] + " order by PIndentIssueId desc", true, "--Select Order No--");
                }
            }
            else if (hncomp.Value == "2")
            {

                string str = @"Select PII.PIndentIssueId, isnull(OM.LocalOrder,'') +' | ' + PII.ChallanNo from PurchaseIndentIssue PII left outer Join ordermaster OM on PII.orderid=OM.orderid
                                where PII.Status='Pending' And PII.PartyId=" + DDPartyName.SelectedValue + " And PII.CompanyId=" + DDCompanyName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"];

                //                if (TextItemCode.Text != "")
                //                {
                //                    str = @"AND PII.PIndentIssueId in (Select Distinct PIndentIssueId from PurchaseIndentIssueTran PIIT , Item_parameter_master IPM 
                //                                where  PIIT.FINISHEDID=IPM.ITEM_FINISHED_ID AND ProductCode='H-1231')";

                //                }
                str = str + "  order by isnull(OM.LocalOrder,'') +' | ' + PII.ChallanNo";

                UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select Order No--");
            }
            else
            {
                string str = @"Select Distinct PII.PIndentIssueId, Case When IsNull(OM.LocalOrder, '') = '' Then PII.ChallanNo Else OM.LocalOrder + ' | ' + PII.ChallanNo End 
                        From PurchaseIndentIssue PII(Nolock) ";
//                if (MasterCompanyId == 16)
//                {
//                    str = str + @" JOIN PurchaseIndentIssueTran PIT(Nolock) on PIT.PIndentIssueId = PII.PIndentIssueId 
//                                            And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
//                }

                str = str + @" left Join ordermaster OM(Nolock) on PII.orderid = OM.orderid 
                Where PII.Status in (" + Postatus + ") And PII.PartyId = " + DDPartyName.SelectedValue + " And PII.CompanyId = " + DDCompanyName.SelectedValue + @" 
                And PII.BranchID = " + DDBranchName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"];
                
                str = str + " Order by  PII.PindentIssueId desc";

                UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select Order No--");
            }
        }
        DDChallanNo.Focus();
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_refresh();
        string ChallanNo = string.Empty;
        switch (Session["varcompanyid"].ToString())
        {
            case "9":
                ChallanNo = "BillNo";
                break;
            default:
                ChallanNo = "receiveno+' / '+BillNo";
                break;
        }
        UtilityModule.ConditionalComboFill(ref ddlrecchalanno, "select distinct prm.PurchaseReceiveId," + ChallanNo + " as ChallanNo from PurchaseReceiveMaster prm left outer join PurchaseReceiveDetail prd  on prd.purchasereceiveid=prm.purchasereceiveid where pindentissueid=" + DDChallanNo.SelectedValue + " and prm.CompanyId=" + DDCompanyName.SelectedValue + " and partyid=" + DDPartyName.SelectedValue + " And prm.MasterCompanyId=" + Session["varCompanyId"], true, "--SELECT--");
        fill_grid();
        if (hncomp.Value == "2")
        {
            Fill_Grid_Show();
        }
        UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct ICM.Category_Id,ICM.Category_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        TextItemCode.Enabled = true;
        ddlrecchalanno.Focus();

        //FIll issue Detail to save
        Fill_porder();
        tdporder.Visible = true;
        string Str = " Select OrderID From PurchaseIndentIssueTran Where IsNull(OrderID, 0) > 0 And PindentIssueid = " + DDChallanNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TDCustomerOrderNo.Visible = true;
        }
        else
        {
            TDCustomerOrderNo.Visible = false;
        }
    }
    protected void fill_refresh()
    {
        //txtorderqty.Text = " ";
        //TxtPQty.Text = " ";
        //txtrate.Text = " ";
        //TxtAmount.Text = " ";
        //TxtPenalty.Text = " ";
        //txtvat.Text = " ";
        //txtcst.Text = " ";
        //Txtnetamount.Text = " ";
        //txtremarks.Text = " ";
        //txtmastremark.Text = " ";
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange1();
    }
    private void ddlcategorycange1()
    {
        if (DDCategory.SelectedItem.Text.ToUpper() == "RAW MATERIAL")
        {
            if (Convert.ToInt32(Session["varCompanyNo"]) == 16 || Convert.ToInt32(Session["varCompanyNo"]) == 28)
            {
                TDMoisture.Visible = true;
            }
        }
        ddlcategorycange();
        string Str = @"Select distinct IM.Item_Id, IM.Item_Name 
            From PurchaseIndentIssueTran PIT 
            inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id 
            inner join Item_Master IM on IPM.Item_Id=IM.Item_Id 
            where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " and IM.Category_Id=" + DDCategory.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        
        //if (MasterCompanyId == 16)
        //{
        //    Str = Str + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}

        UtilityModule.ConditionalComboFill(ref DDItem, Str, true, "--Select--");
    }
    private void ddlcategorycange()
    {
        TdQuality.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;
        string strsql = @"SELECT distinct IPM.[PARAMETER_ID],PARAMETER_NAME 
                        FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                        IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TdQuality.Visible = true;
                        break;
                    case "2":
                        TdDesign.Visible = true;
                        break;
                    case "3":
                        TdColor.Visible = true;
                        break;
                    case "6":
                        TdColorShade.Visible = true;
                        break;
                    case "4":
                        TdShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        break;
                    case "10":
                        TdColor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        item_indexchange();
    }
    private void item_indexchange()
    {
        ddlitem_selectedindex();
        Fill_ChallanDetail();
        int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
        if (VARQCTYPE == 1)
        {
            qulitychk.Visible = true;
            fillgrdquality();
        }
        else
        {
            qulitychk.Visible = false;
        }
    }
    private void ddlitem_selectedindex()
    {
        try
        {
            string view5 = "";
            if (Session["varcompanyno"].ToString() == "20")
            {
                view5 = "V_FinishedItemDetailNew";
            }
            else
            {
                view5 = "V_FinishedItemDetail";
            }

            string st = @"Select Distinct IPM.Quality_Id,QualityName 
                from PurchaseIndentIssueTran PIT 
                inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id 
                inner join Item_Master IM on IPM.Item_Id=IM.Item_Id 
                inner join Quality Q on Q.QualityId =IPM.Quality_Id 
                where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " and IPM.Item_Id=" + DDItem.SelectedValue + @" And IPM.MasterCompanyId=" + Session["varCompanyId"];
            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            UtilityModule.ConditionalComboFill(ref DDQuality, st, true, "--select--");
            string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
            string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
            string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
            string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
            string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
            string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
            st = null;
            st = "  Select Distinct IPM.Design_Id,DesignName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            st = st + "  Select Distinct IPM.Color_Id,ColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            st = st + " Select Distinct IPM.ShadeColor_Id,ShadeColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + color + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            st = st + " Select Distinct IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            if (ChkFt.Checked)
                st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
            else
                st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}
            st = st + "  select distinct UnitId,UnitName from Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId and IM.Item_Id=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            st = st + " Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"];
            st = st + " select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet Ds1 = null;
            Ds1 = SqlHelper.ExecuteDataset(st);
            UtilityModule.ConditionalComboFillWithDS(ref DDDesign, Ds1, 0, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDColor, Ds1, 1, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, Ds1, 2, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, Ds1, 3, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDSize, Ds1, 4, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, Ds1, 5, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, Ds1, 6, true, "--select--");
            DDUnit.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select UnitId from PurchaseIndentIssueTran PIT inner join " + view5 + " vf on vf.item_finished_id=PIT.Finishedid where Vf.Item_Id=" + DDItem.SelectedValue + " and PIT.PIndentIssueId=" + DDChallanNo.SelectedValue).ToString();

            if (DDGodown.Items.Count > 0)
            {
                if (Ds1.Tables[7].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(Ds1.Tables[7].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = Ds1.Tables[7].Rows[0]["godownid"].ToString();
                    }
                    else
                    {
                        DDGodown.SelectedIndex = 1;
                    }
                }
                else
                {
                    DDGodown.SelectedIndex = 1;
                }
                DDGodown_SelectedIndexChanged(DDGodown, new EventArgs());
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fill_size();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Report1();
    }
    private void Report1()
    {
        report();

        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@ReceiveNo", ViewState["PurchaseReceiveId"]);
        array[1] = new SqlParameter("@UserName", Session["UserName"]);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        array[3] = new SqlParameter("@UserId", Session["varuserId"]);
        array[4] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[4].Direction = ParameterDirection.Output;
        array[5] = new SqlParameter("@ReportName", Session["ReportPath"]);
        array[6] = new SqlParameter("@DSFileName", SqlDbType.VarChar, 100);
        array[6].Direction = ParameterDirection.Output;


        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1; 

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetPurchaseReceiveNEW", array);
        if (chklotsummary.Checked == true)
        {
            Session["ReportPath"] = "Reports/PurchaseReceiveNewSummary.rpt";
        }

        Session["dsFileName"] = array[6].Value;

        if (ds.Tables[0].Rows.Count > 0)
        {
            //ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=10px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    public void OpenNewWidow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
    private void fillgrdquality()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + DDCategory.SelectedValue + " and ItemID=" + DDItem.SelectedValue + " and ProcessID='9' order by SrNo");
            grdqualitychk.DataSource = ds;
            grdqualitychk.DataBind();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/purchaseReceive.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void fill_size()
    {
        string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
        string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
        string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
        string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
        string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
        string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
        string st = null;
        if (ChkFt.Checked)
            st = "Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        else
            st = "Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        UtilityModule.ConditionalComboFill(ref DDSize, st, true, "--select--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        quality_change();
    }
    public void quality_change()
    {
        string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
        string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
        string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
        string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
        string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
        string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
        string st = null;
        st = "Select Distinct IPM.Design_Id,DesignName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        st = st + "  Select Distinct IPM.Color_Id,ColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        st = st + "   Select Distinct IPM.ShadeColor_Id,ShadeColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + color + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        st = st + "   Select Distinct IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        if (ChkFt.Checked)
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        else
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        DataSet DS1 = null;
        DS1 = SqlHelper.ExecuteDataset(st);
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, DS1, 0, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColor, DS1, 1, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, DS1, 2, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShape, DS1, 3, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDSize, DS1, 4, true, "--select--");
        Fill_ChallanDetail();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_design();
    }
    private void fill_design()
    {
        string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
        string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
        string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
        string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
        string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
        string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
        string st = null;
        st = "Select Distinct IPM.Color_Id,ColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        st = st + "  Select Distinct IPM.ShadeColor_Id,ShadeColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + color + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        st = st + "  Select Distinct IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        if (ChkFt.Checked)
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        else
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

        //if (MasterCompanyId == 16)
        //{
        //    st = st + " And PIT.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
        //}
        DataSet DS1 = null;
        DS1 = SqlHelper.ExecuteDataset(st);
        UtilityModule.ConditionalComboFillWithDS(ref DDColor, DS1, 0, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, DS1, 1, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShape, DS1, 2, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDSize, DS1, 3, true, "--select--");

        Fill_ChallanDetail();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ChallanDetail();
    }
    protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDCustomerOrderNo.Visible == true)
        {
            FillCustomerOrderNo();
        }
        Fill_ChallanDetail();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ChallanDetail();
        fill_size();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ChallanDetail();
    }
    private void fill_colour()
    {
        string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
        string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
        string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
        string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
        string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
        string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
        string st = null;
        st = "Select Distinct IPM.ShadeColor_Id,ShadeColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + color + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        st = st + "  Select Distinct IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        if (ChkFt.Checked)
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        else
            st = st + "  Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet DS1 = null;
        DS1 = SqlHelper.ExecuteDataset(st);
        UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, DS1, 0, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShape, DS1, 1, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDSize, DS1, 2, true, "--select--");
        Fill_ChallanDetail();
    }
    private void fillshadecolour()
    {
        string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
        string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
        string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
        string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
        string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
        string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
        string st = null;
        st = "Select Distinct IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDShape, st, true, "--select--");
        if (ChkFt.Checked)
            st = "Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        else
            st = "Select Distinct IPM.Size_Id,case when flagsize=0 then SizeFt when flagsize=1 then SizeMtr else SizeInch end from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDSize, st, true, "--select--");
        Fill_ChallanDetail();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
    protected void FillBinno()
    {
        DDBinNo.SelectedIndex = -1;

        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
        {
            quality = 1;
        }
        if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
        {
            design = 1;
        }
        if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
        {
            color = 1;
        }
        if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
        {
            shape = 1;
        }
        if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
        {
            size = 1;
        }
        if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int finishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), finishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BinNo,BinNO From BinMaster where GODOWNID=" + DDGodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            }
        }
    }
    private void FillCustomerOrderNo()
    {
        int FinishedID = 0;
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
        {
            quality = 1;
        }
        if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
        {
            design = 1;
        }
        if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
        {
            color = 1;
        }
        if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
        {
            shape = 1;
        }
        if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
        {
            size = 1;
        }
        if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            FinishedID = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        }
        if (DDCustomerOrderNo.Visible == true)
        {
            string Str = @"Select OM.OrderID, OM.CustomerOrderNo 
                            From PurchaseIndentIssueTran PIIT(Nolock) 
                            JOIN OrderMaster OM(Nolock) ON OM.OrderID = PIIT.OrderID 
                            Where PIIT.PindentIssueid = " + DDChallanNo.SelectedValue;
            if (FinishedID > 0)
            {
                Str = Str + " AND PIIT.FINISHEDID=" + FinishedID;
            }
            Str = Str + " Order By CustomerOrderNo";

            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, Str, true, "--Select--");
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 1;
            }
        }
    }
    private void Fill_ChallanDetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            //string strsql = null;
            int color = 0;
            int quality = 0;
            int design = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
            {
                quality = 1;
            }
            if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
            {
                design = 1;
            }
            if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
            {
                color = 1;
            }
            if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
            {
                shape = 1;
            }
            if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
            {
                size = 1;
            }
            if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {

                int finishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                if (finishedid > 0)
                {
                    DDLotNo.SelectedIndex = -1;
                    string Str = "Select Distinct LotNo, LotNo From PurchaseIndentIssueTran WHERE PINDENTISSUEID=" + DDChallanNo.SelectedValue + " AND FINISHEDID=" + finishedid;

                    if (DDCustomerOrderNo.Visible == true && DDCustomerOrderNo.Items.Count > 0)
                    {
                        Str = Str + " And OrderID = " + DDCustomerOrderNo.SelectedValue;
                    }
                    UtilityModule.ConditionalComboFill(ref DDLotNo, Str, true, "--Select--");
                    if (DDLotNo.Items.Count > 0)
                    {
                        DDLotNo.SelectedIndex = 1;
                        if (TDRecLotNo.Visible == true)
                        {
                            txtLotNo.Text = DDLotNo.SelectedItem.Text;
                        }
                    }
                    //Fill Purchase Orderquantity and OrderQuantity
                    SqlParameter[] _array = new SqlParameter[6];
                    _array[0] = new SqlParameter("@Companyid", SqlDbType.Int);
                    _array[1] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                    _array[2] = new SqlParameter("@PINDENTISSUEID", SqlDbType.Int);
                    _array[3] = new SqlParameter("@lotNo", SqlDbType.NVarChar, 250);
                    _array[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                    _array[5] = new SqlParameter("@OrderID", SqlDbType.Int);

                    _array[0].Value = DDCompanyName.SelectedValue;
                    _array[1].Value = finishedid;
                    _array[2].Value = DDChallanNo.SelectedValue;
                    _array[3].Value =DDLotNo.Items.Count>0? DDLotNo.SelectedItem.Text:"";
                    _array[4].Value = Session["varcompanyId"];

                    if (DDCustomerOrderNo.Visible == true && DDCustomerOrderNo.Items.Count > 0)
                    {
                        _array[5].Value = DDCustomerOrderNo.SelectedValue;
                    }
                    else
                    {
                        _array[5].Value = 0;
                    }
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_FillPurchaseOrder_PQty", _array);


                    //strsql = @"Select ISNULL(SUM(QUANTITY-Isnull(CanQty,0)),0)-[dbo].[Get_PURCHASEPENDINGQTY](FINISHEDID,PINDENTISSUEID,LotNo) from PurchaseIndentIssueTran wHERE PINDENTISSUEID=" + DDChallanNo.SelectedValue + " AND FINISHEDID=" + finishedid + " And lotNo='" + DDLotNo.SelectedItem.Text + "' GROUP BY FINISHEDID,PINDENTISSUEID,LotNo";
                    //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select rate,quantity-isnull(CanQty,0) quantity from PurchaseIndentIssueTran where FinishedId=" + finishedid + "  and PIndentIssueId=" + DDChallanNo.SelectedValue + " And lotNo='" + DDLotNo.SelectedItem.Text + "'");
                    // Table 1 For Rate,OrderQty,vat,cst
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        txtrate.Text = ds.Tables[1].Rows[0]["rate"].ToString();
                        txtorderqty.Text = ds.Tables[1].Rows[0]["quantity"].ToString();
                        //txtvat.Text = ds.Tables[1].Rows[0]["vat"].ToString();
                        //txtcst.Text = ds.Tables[1].Rows[0]["cst"].ToString();

                        double SGST = Convert.ToDouble(ds.Tables[1].Rows[0]["SGST"]);
                        txtSGST.Text = Convert.ToString(Convert.ToDouble(SGST / 2));
                        txtCGST.Text = Convert.ToString(Convert.ToDouble(SGST / 2));
                        txtIGST.Text = ds.Tables[1].Rows[0]["IGST"].ToString();
                        txtTCS.Text = ds.Tables[1].Rows[0]["TCS"].ToString();
                    }
                    else
                    {
                        txtorderqty.Text = "0";
                    }
                    // TxtPQty.Text = SqlHelper.ExecuteScalar(con, CommandType.Text, strsql).ToString();
                    TxtPQty.Text = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
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
    protected void CheckDuplicate_GateIn_ChallanNo()
    {
        Lblmessage.Text = "";
        Lblmessage.Visible = true;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            DataSet ds;
            string str;
            str = "select BillNo From PurchaseReceiveMaster Where PartyId=" + DDPartyName.SelectedValue + "  And BillNo='" + TxtBillNo.Text + "' And PurchaseReceiveId<>" + ViewState["PurchaseReceiveId"] + " and CompanyId=" + DDCompanyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lblmessage.Text = "Challan No Already exists.....";
                return;
            }
            if (TxtGateInNo.Text != "")
            {
                str = "select GateInNo From PurchaseReceiveMaster Where  GateInNo='" + TxtGateInNo.Text + "' And PurchaseReceiveId<>" + ViewState["PurchaseReceiveId"] + " And MasterCompanyId=" + Session["varCompanyId"];
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Lblmessage.Text = "GateIn No. Already Exists.....";
                }
            }
        }
        catch (Exception ex)
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CheckDuplicate_GateIn_ChallanNo();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (Lblmessage.Text == "")
            {
                string LotNo = "",TagNo=string.Empty;
                int finishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                string ReceiveNo = (TxtReceiveNo.Text == "" ? "0" : TxtReceiveNo.Text).ToString();
                string hnpid = (hnprid.Value == "" ? "0" : hnprid.Value).ToString();
                //TxtReceiveNo.Text = ReceiveNo;
                string str;
                if (TDRecLotNo.Visible == true)
                {
                    LotNo = txtLotNo.Text;
                }
                else
                {
                    LotNo = DDLotNo.SelectedItem.Text;
                }
                if (TDRectagNo.Visible == true)
                {
                    TagNo = txttagNo.Text;
                }
                else
                {
                    TagNo = "Without Tag No";
                }

                str = "select isnull(FinishedId,0) from PurchaseReceiveDetail PRD inner join  PurchaseReceiveMaster PRM  on PRM.PurchaseReceiveId=PRD.PurchaseReceiveId where PRD.PIndentIssueId=" + DDChallanNo.SelectedValue + " and  FinishedId=" + finishedid + " and ReceiveNo=" + ReceiveNo + " and prd.lotno='" + LotNo + "' and purchasereceivedetailid <>" + hnpid + " and BaleNo='" + txtbaleno.Text + "' And prm.MasterCompanyId=" + Session["varCompanyId"] + "";
                int n = Convert.ToInt16(SqlHelper.ExecuteScalar(Tran, CommandType.Text, str));
                {
                    if (ChkEditOrder.Checked == true)
                    {
                        //Fill_ChallanDetail();
                        string qry = @"select PRD.PurchaseReceiveDetailId,QTY,ISNULL(V.ReturnQty,0) as RreturnQty
                            from  PurchaseReceiveMaster prm left outer join 
                            PurchaseReceiveDetail PRD on prd.purchasereceiveid=prm.purchasereceiveid inner join 
                            PurchaseIndentIssue PII on PII.PIndentIssueId=PRD.PIndentIssueId 
                            LEFT OUTER JOIN V_PurchaseReturnDetail V ON prd.purchasereceiveDetailid=v.purchasereceiveDetailid
                            where prd.purchasereceiveDetailid=" + hnprid.Value + " And prm.MasterCompanyId=" + Session["varCompanyId"];
                        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            // TxtQty.Text = Ds.Tables[0].Rows[0]["QTY"].ToString();
                            Txtreturnqty.Text = Ds.Tables[0].Rows[0]["RreturnQty"].ToString();
                        }
                        string b = Txtreturnqty.Text;
                        string a = TxtPQty.Text;
                        if (a == "")
                            a = "0";
                        else
                            a = TxtPQty.Text;

                        //TxtPQty.Text = Convert.ToString(Convert.ToDouble(a.ToString()) + Convert.ToDouble(TxtQty.Text) - Convert.ToDouble(Txtreturnqty.Text));
                        TxtPQty.Text = Convert.ToString(Math.Round(Convert.ToDouble(a.ToString()) + Convert.ToDouble(TxtQty.Text) + Convert.ToDouble(Txtreturnqty.Text), 3, MidpointRounding.AwayFromZero));
                        if (Convert.ToDouble(Txtreturnqty.Text) > 0)
                        {
                            if (Convert.ToDouble(TxtQty.Text) < Convert.ToDouble(Txtreturnqty.Text))
                            {
                                MessageSave("ReceiveQty can not be less than returnqty !");
                                return;
                            }

                        }
                    }
                    qtychange();
                    //if (Convert.ToDouble(TxtQty.Text) > Convert.ToDouble(TxtPQty.Text))
                    //{
                    //    MessageSave("ReceiveQty can not be greater than Pendingnqty !");
                    //    return;
                    //}
                    ViewState["PurchaseReceiveId"] = 0;
                    ViewState["PurchaseReceiveDetailId"] = 0;
                    if (n == 0)
                    {
                        SqlParameter[] _arrpara = new SqlParameter[61];
                        _arrpara[0] = new SqlParameter("@PurchaseReceiveId", SqlDbType.Int);
                        _arrpara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                        _arrpara[2] = new SqlParameter("@PartyId", SqlDbType.Int);
                        _arrpara[3] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
                        _arrpara[4] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
                        _arrpara[5] = new SqlParameter("@UserId", SqlDbType.Int);
                        _arrpara[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                        _arrpara[7] = new SqlParameter("@PurchaseReceiveDetailId", SqlDbType.Int);
                        _arrpara[8] = new SqlParameter("@FinishedId", SqlDbType.Int);
                        _arrpara[9] = new SqlParameter("@GodownId", SqlDbType.Int);
                        _arrpara[10] = new SqlParameter("@UnitId", SqlDbType.Int);
                        _arrpara[11] = new SqlParameter("@Qty", SqlDbType.Float);
                        _arrpara[12] = new SqlParameter("@PIndentIssueId", SqlDbType.Int);
                        _arrpara[13] = new SqlParameter("@GateInNo", SqlDbType.NVarChar, 50);
                        _arrpara[14] = new SqlParameter("@BillNo", SqlDbType.NVarChar, 50);
                        _arrpara[15] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
                        _arrpara[16] = new SqlParameter("@Finished_Type_Id", SqlDbType.Int);
                        _arrpara[17] = new SqlParameter("@BillNo1", SqlDbType.NVarChar, 50);
                        _arrpara[18] = new SqlParameter("@Qtyreturn", SqlDbType.Float);
                        _arrpara[19] = new SqlParameter("@rate", SqlDbType.Float);
                        _arrpara[20] = new SqlParameter("@challan_status", SqlDbType.Int);
                        _arrpara[21] = new SqlParameter("@vat", SqlDbType.Float);
                        _arrpara[22] = new SqlParameter("@Cst", SqlDbType.Float);
                        _arrpara[23] = new SqlParameter("@NetAmount", SqlDbType.Float);
                        _arrpara[24] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
                        _arrpara[25] = new SqlParameter("@Penalty", SqlDbType.Float);
                        _arrpara[26] = new SqlParameter("@MRemark", SqlDbType.NVarChar, 250);
                        _arrpara[27] = new SqlParameter("@retdate", SqlDbType.SmallDateTime);

                        _arrpara[28] = new SqlParameter("@TransportName", SqlDbType.VarChar, 250);
                        _arrpara[29] = new SqlParameter("@TransPortAdd", SqlDbType.VarChar, 300);
                        _arrpara[30] = new SqlParameter("@DriverName", SqlDbType.VarChar, 100);
                        _arrpara[31] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 15);
                        _arrpara[32] = new SqlParameter("@BiltyNo", SqlDbType.VarChar, 20);
                        _arrpara[33] = new SqlParameter("@Biltydate", SqlDbType.SmallDateTime);
                        _arrpara[34] = new SqlParameter("@IssLotNo", SqlDbType.VarChar, 50);
                        _arrpara[35] = new SqlParameter("@LShort", SqlDbType.Float);
                        _arrpara[36] = new SqlParameter("@Freight", SqlDbType.Float);
                        _arrpara[37] = new SqlParameter("@BaleNo", SqlDbType.VarChar, 50);
                        _arrpara[38] = new SqlParameter("@Bellwt", SqlDbType.Float);
                        _arrpara[39] = new SqlParameter("@SGST", SqlDbType.Float);
                        _arrpara[40] = new SqlParameter("@IGST", SqlDbType.Float);
                        _arrpara[41] = new SqlParameter("@CompanyLotno", SqlDbType.VarChar, 50);
                        _arrpara[42] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
                        _arrpara[43] = new SqlParameter("@PenalityRemarks", SqlDbType.VarChar, 250);
                        _arrpara[44] = new SqlParameter("@OldLotNoWise", SqlDbType.Int);
                        _arrpara[45] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                        _arrpara[46] = new SqlParameter("@BillDate", SqlDbType.DateTime);
                        _arrpara[47] = new SqlParameter("@BillQty", SqlDbType.Float);
                        _arrpara[48] = new SqlParameter("@TransportBuiltyAmt", SqlDbType.Float);
                        _arrpara[49] = new SqlParameter("@UnloadingExpenses", SqlDbType.Float);
                        _arrpara[50] = new SqlParameter("@Particular", SqlDbType.VarChar, 50);
                        _arrpara[51] = new SqlParameter("@FiftyMWait", SqlDbType.Float);
                        _arrpara[52] = new SqlParameter("@YarnShape", SqlDbType.VarChar, 20);
                        _arrpara[53] = new SqlParameter("@Othercharges", SqlDbType.Float);
                        _arrpara[54] = new SqlParameter("@Legalvendorid", SqlDbType.Int);
                        _arrpara[55] = new SqlParameter("@Moisture", SqlDbType.Float);
                        _arrpara[56] = new SqlParameter("@OrderID", SqlDbType.Int);
                        _arrpara[57] = new SqlParameter("@TCS", SqlDbType.Float);
                        _arrpara[58] = new SqlParameter("@BranchID", SqlDbType.Int);
                        _arrpara[59] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
                        _arrpara[60] = new SqlParameter("@QtyWeight", SqlDbType.Decimal,18-3);

                        _arrpara[0].Direction = ParameterDirection.InputOutput;
                        _arrpara[0].Value = ViewState["PurchaseReceiveId"];
                        _arrpara[1].Value = DDCompanyName.SelectedValue;
                        _arrpara[2].Value = DDPartyName.SelectedValue;
                        _arrpara[3].Direction = ParameterDirection.InputOutput;
                        if (TxtReceiveNo.Text != "")
                        {
                            _arrpara[3].Value = TxtReceiveNo.Text;
                        }
                        _arrpara[4].Value = TxtReceiveDate.Text;
                        _arrpara[5].Value = Session["varuserid"];
                        _arrpara[6].Value = Session["varCompanyId"];
                        _arrpara[7].Direction = ParameterDirection.InputOutput;
                        if (ChkEditOrder.Checked == true)
                        {
                            _arrpara[7].Value = hnprid.Value;
                        }
                        else
                        {
                            _arrpara[7].Value = ViewState["PurchaseReceiveDetailId"];
                        }
                        _arrpara[8].Value = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"]));
                        _arrpara[9].Value = DDGodown.SelectedValue;
                        _arrpara[10].Value = DDUnit.SelectedValue;
                        _arrpara[11].Value = TxtQty.Text;
                        _arrpara[12].Value = DDChallanNo.SelectedValue;
                        _arrpara[13].Direction = ParameterDirection.InputOutput;
                        _arrpara[13].Value = TxtGateInNo.Text.ToUpper();
                        _arrpara[14].Value = TxtBillNo.Text.ToUpper();
                        _arrpara[15].Value = LotNo;
                        int VarFinish_Type = TdFinish_Type.Visible == true ? Convert.ToInt32(ddFinish_Type.SelectedValue) : 0;
                        _arrpara[16].Value = VarFinish_Type;
                        _arrpara[17].Value = txtbillno1.Text;
                        _arrpara[18].Value = Txtreturnqty.Text != "" ? Convert.ToDouble(Txtreturnqty.Text) : 0;
                        _arrpara[19].Value = txtrate.Text != "" ? Convert.ToDouble(txtrate.Text) : 0;
                        _arrpara[20].Value = 0;
                        _arrpara[21].Value = txtvat.Text != "" ? Convert.ToDouble(txtvat.Text) : 0;
                        _arrpara[22].Value = txtcst.Text != "" ? Convert.ToDouble(txtcst.Text) : 0;
                        _arrpara[23].Value = Txtnetamount.Text != "" ? Convert.ToDouble(Txtnetamount.Text) : 0;
                        _arrpara[24].Value = txtremarks.Text;
                        _arrpara[25].Value = TxtPenalty.Text == "" ? "0" : TxtPenalty.Text;
                        _arrpara[26].Value = txtmastremark.Text;
                        _arrpara[27].Value = txtretdate.Text;
                        _arrpara[28].Value = txtTransportName.Text.ToUpper();
                        _arrpara[29].Value = txtTransportAddress.Text.ToUpper();
                        _arrpara[30].Value = txtDriver.Text.ToUpper();
                        _arrpara[31].Value = txtVehicleNo.Text.ToUpper();
                        _arrpara[32].Value = txtBiltyNo.Text.ToUpper();

                        System.Data.SqlTypes.SqlDateTime getDate;
                        //set DateTime null
                        getDate = SqlDateTime.Null;
                        _arrpara[33].Value = txtBiltyDate.Text == "" ? getDate : Convert.ToDateTime(txtBiltyDate.Text);
                        _arrpara[34].Value = DDLotNo.SelectedItem.Text;
                        _arrpara[35].Value = txtLshort.Text == "" ? "0" : txtLshort.Text;
                        _arrpara[36].Value = txtfreight.Text == "" ? "0" : txtfreight.Text;
                        _arrpara[37].Value = txtbaleno.Text;
                        _arrpara[38].Value = TDBellWt.Visible == false ? "0" : (txtbellwt.Text == "" ? "0" : txtbellwt.Text);
                        double GST = Convert.ToDouble(txtCGST.Text == "" ? "0" : txtCGST.Text) + Convert.ToDouble(txtSGST.Text == "" ? "0" : txtSGST.Text);
                        _arrpara[39].Value = GST;
                        //_arrpara[39].Value = txtSGST.Text != "" ? Convert.ToDouble(txtSGST.Text) : 0;
                        _arrpara[40].Value = txtIGST.Text != "" ? Convert.ToDouble(txtIGST.Text) : 0;
                        _arrpara[41].Direction = ParameterDirection.InputOutput;
                        _arrpara[41].Value = TDCompanyLotNo.Visible == true ? txtcomplotno.Text : "";
                        string BinNo = TDBINNO.Visible == true && DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
                        _arrpara[42].Value = BinNo;
                        _arrpara[43].Value = txtPenalityRemark.Text;
                        if (Session["varCompanyId"].ToString() == "44")
                        {
                            _arrpara[44].Value = 1;
                        }
                        else
                        {
                            _arrpara[44].Value = TDCompanyLotNo.Visible == true ? (chkoldlotno.Checked == true ? "1" : "0") : "0";
                        }
                        _arrpara[45].Direction = ParameterDirection.Output;

                        System.Data.SqlTypes.SqlDateTime getDate2;
                        //set DateTime null
                        getDate2 = SqlDateTime.Null;
                        _arrpara[46].Value = txtBillDate.Text == "" ? getDate2 : Convert.ToDateTime(txtBillDate.Text);
                        _arrpara[47].Value = txtBillQty.Text == "" ? "0" : txtBillQty.Text;
                        _arrpara[48].Value = txtTransportBuiltyAmt.Text == "" ? "0" : txtTransportBuiltyAmt.Text;
                        _arrpara[49].Value = txtUnloadingExpenses.Text == "" ? "0" : txtUnloadingExpenses.Text;
                        _arrpara[50].Value = txtParticular.Text;
                        _arrpara[51].Value = txt50MWait.Text == "" ? "0" : txt50MWait.Text;
                        _arrpara[52].Value = txtYarnShape.Text;
                        _arrpara[53].Value = txtothercharges.Text == "" ? "0" : txtothercharges.Text;
                        _arrpara[54].Value = Tdlegalvendor.Visible == false ? "0" : (DDlegalvendor.SelectedIndex > 0 ? DDlegalvendor.SelectedValue : "0");
                        _arrpara[55].Value = TDMoisture.Visible == true ? (TxtMoisture.Text == "" ? "0" : TxtMoisture.Text) : "0";
                        _arrpara[56].Value = TDCustomerOrderNo.Visible == true ? DDCustomerOrderNo.SelectedValue : "0";
                        _arrpara[57].Value = txtTCS.Text != "" ? Convert.ToDouble(txtTCS.Text) : 0;
                        _arrpara[58].Value = DDBranchName.SelectedValue;
                        _arrpara[59].Direction = ParameterDirection.InputOutput;
                        _arrpara[59].Value = TDRectagNo.Visible == true ? txttagNo.Text : "Without Tag No";
                        _arrpara[60].Value = TDQtyWeight.Visible == true ? (txtQtyWeight.Text == "" ? "0" : txtQtyWeight.Text) : "0";

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceive", _arrpara);

                        ViewState["PurchaseReceiveId"] = _arrpara[0].Value;
                        TxtReceiveNo.Text = _arrpara[3].Value.ToString();
                        TxtGateInNo.Text = _arrpara[13].Value.ToString();
                        txtcomplotno.Text = _arrpara[41].Value.ToString();
                        string CompanyLotno = _arrpara[41].Value.ToString();
                        string PurchaseReceiveTagNo = _arrpara[59].Value.ToString();

                        //UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[1].Value), Convert.ToString(_arrpara[15].Value), Convert.ToDouble(_arrpara[11].Value), Convert.ToString(_arrpara[4].Value), DateTime.Now.ToString("dd-MMM-yyyy"), "PurchaseReceiveDetail", Convert.ToInt32(_arrpara[7].Value), Tran, 1, true, 1, VarFinish_Type);
                        if (_arrpara[45].Value.ToString() != "")
                        {
                            Lblmessage.Visible = true;
                            Lblmessage.Text = _arrpara[45].Value.ToString();
                            Tran.Rollback();
                        }
                        else
                        {
                            //Minus Lshort in Receive Qty
                            if (txtLshort.Text != "" && txtLshort.Text != "0")
                            {
                                _arrpara[11].Value = Convert.ToDouble(_arrpara[11].Value) - (Convert.ToDouble(_arrpara[11].Value) * Convert.ToDouble(txtLshort.Text) / 100);
                            }
                            if (TDBellWt.Visible == true)
                            {
                                _arrpara[11].Value = Convert.ToDouble(_arrpara[11].Value) - Convert.ToDouble(txtbellwt.Text == "" ? "0" : txtbellwt.Text);
                            }

                           
                            if (Convert.ToString(Session["varCompanyId"]) == "43")
                            {
                                TagNo = PurchaseReceiveTagNo;
                            }

                            //string TAGNO = "Without Tag No";
                            //if (Convert.ToString(Session["varCompanyId"]) == "22")
                            //{
                            //    TAGNO = txttagNo.Text;
                            //}

                            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select StockUpdateFlag From MasterSetting(Nolock) ");
                            if (Ds.Tables[0].Rows.Count > 0)
                            {
                                if (Ds.Tables[0].Rows[0]["StockUpdateFlag"].ToString() == "1")
                                {
                                    UtilityModule.StockStockTranTableUpdateNew(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[1].Value), (variable.VarPURCHASERECEIVEAUTOGENLOTNO == "1" ? CompanyLotno : Convert.ToString(_arrpara[15].Value)), Convert.ToDouble(_arrpara[11].Value), _arrpara[4].Value.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "PurchaseReceiveDetail", Convert.ToInt32(_arrpara[7].Value), Tran, 1, true, 1, VarFinish_Type, UnitId: Convert.ToInt16(DDUnit.SelectedValue), TagNo: TagNo, BinNo: BinNo);
                                }
                            }
                            
                            report();

                            BtnPreview.Enabled = true;
                            QCSAVE(Tran, Convert.ToInt32(ViewState["PurchaseReceiveId"]), Convert.ToInt32(_arrpara[7].Value));
                            Tran.Commit();
                            lblsrno.Text = "SR No. is generated " + ViewState["PurchaseReceiveId"].ToString() + ".";
                            Lblmessage.Visible = true;
                            Lblmessage.Text = "Data Saved Successfully..............";
                            switch (Convert.ToInt16(hncomp.Value))
                            {
                                case 6:
                                case 7:
                                case 9:
                                    Fill_porder();
                                    break;
                                default:
                                    Fill_porder();
                                    break;
                            }

                            fill_grid();
                            Fill_Grid_Show();
                            RefreshControl();
                            //TxtReceiveNo.Text = "";   
                            for (int i = 0; i < grdqualitychk.Rows.Count; i++)
                            {
                                CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
                                chk.Checked = false;
                            }

                        }
                    }
                    else
                    {
                        Lblmessage.Visible = true;
                        Lblmessage.Text = "Duplicate entry.....";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
            Tran.Rollback();
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void RefreshControl()
    {
        TxtPQty.Text = null;
        txtorderqty.Text = "0";
        txtrate.Text = null;
        TxtQty.Text = null;
        txtbellwt.Text = "";
        TextItemCode.Text = "";
        TextItemCode.Focus();
        DDItem.SelectedIndex = 0;
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDColorShade.Items.Clear();
        DDLotNo.Items.Clear();
        txtLotNo.Text = "";
        txtmastremark.Text = "";
        txtvat.Text = null;
        txtcst.Text = "";
        //txtfreight.Text = "";
        TxtAmount.Text = "";
        Txtnetamount.Text = "";
        txtremarks.Text = "";
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        txtchalan_no.Text = TxtBillNo.Text;
        if (txtchalan_no.Text == "")
        {
            ddlrecchalanno.Items.Clear();
        }
        DDCompanyName.Enabled = false;
        DDPartyName.Enabled = false;
        DDChallanNo.Enabled = true;
        TxtReceiveDate.Enabled = false;
        TxtBillNo.Enabled = false;
        txtretdate.Enabled = false;
        Txtreturnqty.Text = "0";
        ViewState["PurchaseReceiveDetailId"] = 0;

        txtTransportName.Text = "";
        txtTransportAddress.Text = "";
        txtDriver.Text = "";
        txtVehicleNo.Text = "";
        txtBiltyNo.Text = "";
        txtBiltyDate.Text = "";
        txtLshort.Text = "";
        txtbaleno.Text = "";

        txtCGST.Text = "";
        txtSGST.Text = "";
        txtIGST.Text = "";
        txtPenalityRemark.Text = "";
        TxtPenalty.Text = "";
        chkoldlotno.Enabled = true;

        txtBillQty.Text = "";
        txtTransportBuiltyAmt.Text = "";
        txtUnloadingExpenses.Text = "";
        txtParticular.Text = "";
        txt50MWait.Text = "";
        txtYarnShape.Text = "";
        txtTCS.Text = "";
        txtQtyWeight.Text = "";
    }
    private void QCSAVE(SqlTransaction Tran, int ReceiveId, int ReceiveDetailId)
    {
        string checkpara = "";
        string noncheck = "";
        for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                if (checkpara == "")
                {
                    checkpara = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    checkpara = checkpara + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
            }
            else
                if (noncheck == "")
                {
                    noncheck = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    noncheck = noncheck + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
        }
        SqlParameter[] _arrpara1 = new SqlParameter[5];
        _arrpara1[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
        _arrpara1[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        _arrpara1[2] = new SqlParameter("@checkpara", SqlDbType.NVarChar, 50);
        _arrpara1[3] = new SqlParameter("@noncheck", SqlDbType.NVarChar, 50);
        _arrpara1[4] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        _arrpara1[0].Value = ReceiveId;
        _arrpara1[1].Value = ReceiveDetailId;
        _arrpara1[2].Value = checkpara;
        _arrpara1[3].Value = noncheck;
        _arrpara1[4].Value = "PurchaseReceiveDetail";
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceivequalitychk", _arrpara1);
    }
    private void fill_grid()
    {
        DGPurchaseReceiveDetail.DataSource = Fill_Grid_Data();
        DGPurchaseReceiveDetail.DataBind();
        if (Session["VarCompanyNo"].ToString() == "7")
        {
            DGPurchaseReceiveDetail.Columns[5].Visible = false;
        }
        if (DGPurchaseReceiveDetail.Rows.Count > 0)
            DGPurchaseReceiveDetail.Visible = true;
        else
            DGPurchaseReceiveDetail.Visible = false;
    }
    private DataSet Fill_Grid_Data()
    {
        string view = "";
        if (Session["varcompanyno"].ToString() == "20")
        {
            view = "V_FinishedItemDetailNew";
        }
        else
        {
            view = "V_FinishedItemDetail";
        }
        DataSet ds = null;
        try
        {
            string strsql;
            strsql = @"select PRD.PurchaseReceiveDetailId,Category_Name+'/'+Item_Name+'/'+isnull(QualityName,'')+'/'+isnull(DesignName,'')+'/'+isnull(ColorName,'')+'/'+isnull(ShadeColorName,'')+'/'+ isnull(ShapeName,'')+'/'+case when PIT.flagSize=1 then isnull(SizeMtr,'') else case When PIT.flagSize=0 Then isnull(SizeFt,'')  Else isnull(sizeinch,'') End end as ItemDescription,
                        PII.ChallanNo,GodownName,PRd.QTY,UnitName ,isnull(QualityName,'') as item,prd.lotno as lotno,prd.tagno as tagno,ISNULL(V.ReturnQty,0) qtyreturn,
                        (prd.qty-isnull(prd.bellwt,0))*prd.rate amount,prd.vat,prd.cst,prd.NetAmount,PRD.Penalty,PRD.remark,prm.MRemark,PRD.SGST,PRD.IGST,
                        isnull(Prd.Bellwt,0) as Bellwt,isnull(PRD.PenalityRemarks,'') PenalityRemarks,isnull(Prd.BillQty,0) BillQty,
                        isnull(Prd.TransportBuiltyAmt,0) TransportBuiltyAmt,isnull(Prd.UnloadingExpenses,0) UnloadingExpenses,isnull(Prd.Particular,'') Particular,
                        isnull(Prd.FiftyMWait,0) FiftyMWait,isnull(Prd.YarnShape,'') YarnShape, prd.rate,isnull(PRD.TCS,0) as TCS 
                        From  PurchaseReceiveMaster prm left outer join 
                        PurchaseReceiveDetail PRD on prd.purchasereceiveid=prm.purchasereceiveid 
                        inner join PurchaseIndentIssue PII on PII.PIndentIssueId=PRD.PIndentIssueId 
                        Inner Join PurchaseIndentIssueTran PIT on Pii.PindentIssueId=PIT.PindentIssueId And PIT.PindentIssueTranid=PRD.PindentIssueTranId 
                        Left Outer join GodownMaster GM on PRD.GodownId=GM.GodownId 
                        inner join unit on unit.UnitId=PRD.UnitId 
                        Inner join " + view + @" FID on FID.Item_Finished_Id=PRD.FinishedId 
                        LEFT OUTER JOIN V_PurchaseReturnDetail V ON prd.purchasereceiveDetailid=v.purchasereceiveDetailid 
                        Where prm.PurchaseReceiveId=" + ViewState["PurchaseReceiveId"] + " And prm.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
        }
        return ds;
    }
    protected void TxtQty_TextChanged(object sender, EventArgs e)
    {
        Txtreturnqty.Focus();
        qtychange();
    }
    private void qtychange()
    {
        Double RecQty = 0;
        if (TxtQty.Text != "" && txtrate.Text != "")
        {
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select varcompanyNo,PercentageExecssQty From Mastersetting ");
            int varcompanyno = Convert.ToInt32(ds.Tables[0].Rows[0]["varcompanyNo"]);
            Double Percentage = Convert.ToDouble(ds.Tables[0].Rows[0]["PercentageExecssQty"]);
            switch (varcompanyno)
            {
                case 6: //For ArtIndia
                    if (Convert.ToInt16(Session["varDepartment"]) == 1)
                    {
                        Percentage = 100;
                        RecQty = Math.Round(Convert.ToDouble(TxtPQty.Text) + (Convert.ToDouble(txtorderqty.Text) * Percentage / 100), 3);
                    }
                    else
                    {
                        RecQty = Math.Round(Convert.ToDouble(TxtPQty.Text) + (Convert.ToDouble(txtorderqty.Text) * Percentage / 100), 3);
                    }
                    break;
                default:
                    RecQty = Math.Round(Convert.ToDouble(TxtPQty.Text) + (Convert.ToDouble(txtorderqty.Text) * Percentage / 100), 3);
                    break;
            }

            if (ChkEditOrder.Checked == true)
            {
                hnqty.Value = "0";
            }
            if (varcompanyno == 7) // Dilli Karigari
            {
                if (Convert.ToDouble(TxtQty.Text) > RecQty)
                {

                    Lblmessage.Visible = true;
                    // Lblmessage.Text = "Qty Can not be greater than " + Math.Round(RecQty, 3) + " (Including 15% Extra Qty)";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Qty Can not be greater than " + Math.Round(RecQty, 3) + " (Including 15% Extra Qty)');", true);
                    TxtQty.Text = "";
                    return;
                }
                else
                {
                    Lblmessage.Visible = false;
                    Lblmessage.Text = "";
                }
            }
            else
            {
                if (Convert.ToDouble(TxtQty.Text) > RecQty)
                {

                    Lblmessage.Visible = true;
                    //Lblmessage.Text = "Qty Can not be greater than " + Math.Round(RecQty, 3);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Qty Can not be greater than " + Math.Round(RecQty, 3) + "');", true);
                    TxtQty.Text = "";
                    return;
                }
                else
                {
                    Lblmessage.Visible = false;
                    Lblmessage.Text = "";
                    TxtAmount.Text = Convert.ToString(Convert.ToDouble(txtrate.Text) * Convert.ToDouble(TxtQty.Text));
                    fillAmount();
                }
            }
        }
    }
    protected void TxtQty1_TextChanged(object sender, EventArgs e)
    {
        if (Txtreturnqty.Text != "")
        {

            if (ChkEditOrder.Checked == true && hnqty.Value != "0")
            {
                if (Convert.ToDouble(hnqty.Value) >= Convert.ToDouble(Txtreturnqty.Text))
                {
                    TxtQty.Text = Convert.ToString(Convert.ToDouble(hnqty.Value) - Convert.ToDouble(Txtreturnqty.Text));
                    qtychange();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Return Qty Is Greater Then Receive + Return Qty');", true);
                    Txtreturnqty.Text = Convert.ToString(Convert.ToDouble(hnqty.Value) - Convert.ToDouble(TxtQty.Text));
                    qtychange();
                    return;
                }
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select varcompanyNo,PercentageExecssQty From Mastersetting ");
            Double Percentage = Convert.ToDouble(ds.Tables[0].Rows[0]["PercentageExecssQty"]);
            Double RecQty = Math.Round(Convert.ToDouble(TxtPQty.Text) + (Convert.ToDouble(txtorderqty.Text) * Percentage / 100), 3);

            double qty = RecQty - (Convert.ToDouble(TxtQty.Text) + Convert.ToDouble(Txtreturnqty.Text));
            if (qty < 0.00)
            {
                Txtreturnqty.Text = null;
                Txtreturnqty.Focus();
                Lblmessage.Text = "Receive QTY must be less than PQty and Greater then Zero........ ";
                Lblmessage.Visible = true;
            }
            else
            {
                Lblmessage.Text = "";
            }
            BtnSave.Focus();
        }
        else
        {
            TxtQty.Text = null;
            Txtreturnqty.Focus();
            Lblmessage.Text = "Reurn QTY must be less than PQTY and Receive QTY  and Greater then Zero........ ";
        }
    }
    protected void DGPurchaseReceiveDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPurchaseReceiveDetail, "Select$" + e.Row.RowIndex);

            if (variable.Carpetcompany == "1")
            {
                for (int i = 0; i < DGPurchaseReceiveDetail.Columns.Count; i++)
                {
                    if (DGPurchaseReceiveDetail.Columns[i].HeaderText == "Bell Wt.")
                    {
                        DGPurchaseReceiveDetail.Columns[i].Visible = true;
                    }
                }
            }
           
        }
        if (hncomp.Value == "10")
        {
            DGPurchaseReceiveDetail.Columns[5].Visible = false;
        }

        if (hncomp.Value == "9")
        {
            for (int i = 0; i < DGPurchaseReceiveDetail.Columns.Count; i++)
            {
                if (DGPurchaseReceiveDetail.Columns[i].HeaderText == "BillQty" || DGPurchaseReceiveDetail.Columns[i].HeaderText == "BuiltyAmt" || DGPurchaseReceiveDetail.Columns[i].HeaderText == "UnloadExp" || DGPurchaseReceiveDetail.Columns[i].HeaderText == "Particular" || DGPurchaseReceiveDetail.Columns[i].HeaderText == "50MWait" || DGPurchaseReceiveDetail.Columns[i].HeaderText == "YarnShape")
                {
                    DGPurchaseReceiveDetail.Columns[i].Visible = true;
                }
            }
        }
        DGPurchaseReceiveDetail.Columns[15].Visible = false;
        if ((hncomp.Value == "28" && Session["varuserid"].ToString() == "1") || (hncomp.Value == "16" && Session["varuserid"].ToString() == "1"))
        {
            DGPurchaseReceiveDetail.Columns[15].Visible = true;
        }
        else
        {
            DGPurchaseReceiveDetail.Columns[15].Visible = true;
        }

        if (hncomp.Value == "43")
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < DGPurchaseReceiveDetail.Columns.Count; i++)
                {                   
                    if (DGPurchaseReceiveDetail.Columns[i].HeaderText == "TAGNO")
                    {
                        e.Row.Cells[i].Text = "UCNNo";
                    }
                }
                //e.Row.Cells[6].Text = "UCNNo";
            }

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
            {
                TextBox txtLotNo = e.Row.FindControl("txtLotNo") as TextBox;
                txtLotNo.Enabled = true;
            }
            
        }
    }
    protected void TextItemCode_TextChanged(object sender, EventArgs e)
    {
        if (DDPartyName.SelectedIndex > 0)
        {
            item_text_changed();
            DDGodown.Focus();
        }
        //Fill_ChallanDetail();
    }
    private void item_text_changed()
    {
        DataSet ds;
        string Str;
        Lblmessage.Text = "";
        if (TextItemCode.Text != "")
        {
            Str = "Select * from PurchaseIndentIssuetran PD,ITEM_PARAMETER_MASTER IPM Where PD.Finishedid=IPM.Item_Finished_id And ProductCode='" + TextItemCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count == 0)
            {
                Lblmessage.Text = "ITEM CODE DOES NOT BELONGS TO THAT PO. NO....";
                DDCategory.SelectedIndex = 0;
                ddlcategorycange();
                TextItemCode.Text = "";
                TextItemCode.Focus();
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct ICM.Category_Id,Category_Name from PurchaseIndentIssuetran PID,ITEM_CATEGORY_MASTER ICM,
                ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,UserRights_Category UC Where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.FinishedId=IPM.Item_Finished_Id And ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + " And PID.PindentIssueid=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
                Str = "Select PindentIssueid,IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate CS,PurchaseIndentIssuetran PD Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  and PD.Finishedid=IPM.Item_Finished_id And ProductCode='" + TextItemCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    ddlcategorycange();
                    UtilityModule.ConditionalComboFill(ref DDItem, "select distinct IM.Item_Id,IM.Item_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " and IM.Category_Id=" + DDCategory.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
                    DDItem.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    string st = "Select IPM.Quality_Id,QualityName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Quality Q on Q.QualityId =IPM.Quality_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    UtilityModule.ConditionalComboFill(ref DDQuality, st, true, "--select--");
                    DDQuality.SelectedValue = ds.Tables[0].Rows[0]["Quality_ID"].ToString();
                    string quality = DDQuality.SelectedIndex > 0 ? " and IPM.Quality_Id=" + DDQuality.SelectedValue + "" : "";
                    string design = DDDesign.SelectedIndex > 0 ? " and IPM.Design_Id=" + DDDesign.SelectedValue + "" : "";
                    string color = DDColor.SelectedIndex > 0 ? " and IPM.Color_Id=" + DDColor.SelectedValue + "" : "";
                    string shadecolor = DDColorShade.SelectedIndex > 0 ? " and IPM.ShadeColor_Id=" + DDColorShade.SelectedValue + "" : "";
                    string shape = DDShape.SelectedIndex > 0 ? " and IPM.Shape_Id=" + DDShape.SelectedValue + "" : "";
                    string size = DDSize.SelectedIndex > 0 ? " and IPM.Size_Id=" + DDSize.SelectedValue + "" : "";
                    st = null;
                    st = " Select IPM.Design_Id,DesignName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    st = st + " Select IPM.Color_Id,ColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    st = st + "  Select IPM.ShadeColor_Id,ShadeColorName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + color + design + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    st = st + "  Select IPM.Shape_Id,ShapeName from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    if (ChkFt.Checked)
                        st = st + "  Select IPM.Size_Id,SizeFt from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    else
                        st = st + "  Select IPM.Size_Id,SizeMtr from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id left outer join Quality Q on Q.QualityId =IPM.Quality_Id left outer join Design D on D.DesignId=IPM.Design_Id left outer join Color C on C.ColorId=IPM.Color_Id left outer join  ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id left outer join Shape SH on SH.ShapeId=IPM.Shape_Id  left outer join Size SZ on SZ.SizeId=IPM.Size_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + quality + design + color + shadecolor + shape + " and IPM.Item_Id=" + DDItem.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    st = st + " select distinct UnitId,UnitName from Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId and IM.Item_Id=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                    if (ViewState["Gridstatus"].ToString() == "1")
                    {
                        st = st + "  select Distinct G.GodownId,G.GodownName,P.remark,P.PenalityRemarks from GodownMaster G JOIN Godown_Authentication GA ON G.GodownId=GA.GodownId and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @"
                                     JOIN purchasereceivedetail P ON G.GodownId=P.GodownId Where P.PurchaseReceiveDetailId=" + DGPurchaseReceiveDetail.SelectedDataKey.Value + " And G.MasterCompanyId=" + Session["varCompanyId"];
                    }

                    //if (ViewState["Gridstatus"].ToString() == "1")
                    //{
                    //    st = st + "  select Distinct G.GodownId,G.GodownName,P.remark,P.PenalityRemarks from GodownMaster G, purchasereceivedetail P Where G.GodownId=P.GodownId AND P.PurchaseReceiveDetailId=" + DGPurchaseReceiveDetail.SelectedDataKey.Value + " And G.MasterCompanyId=" + Session["varCompanyId"];
                    //}

                    //// ViewState["Gridstatus"] = 0;
                    DataSet Ds1 = null;
                    Ds1 = SqlHelper.ExecuteDataset(st);
                    UtilityModule.ConditionalComboFillWithDS(ref DDDesign, Ds1, 0, true, "--select--");
                    UtilityModule.ConditionalComboFillWithDS(ref DDColor, Ds1, 1, true, "--select--");
                    UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, Ds1, 2, true, "--select--");
                    UtilityModule.ConditionalComboFillWithDS(ref DDShape, Ds1, 3, true, "--select--");
                    UtilityModule.ConditionalComboFillWithDS(ref DDSize, Ds1, 4, true, "--select--");
                    DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Design_ID"].ToString();
                    DDColor.SelectedValue = ds.Tables[0].Rows[0]["Color_ID"].ToString();
                    DDShape.SelectedValue = ds.Tables[0].Rows[0]["Shape_ID"].ToString();
                    DDSize.SelectedValue = ds.Tables[0].Rows[0]["Size_ID"].ToString();
                    DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_ID"].ToString();
                    CommanFunction.FillComboWithDS(DDUnit, Ds1, 5);
                    if (ViewState["Gridstatus"].ToString() == "1")
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref DDGodown, Ds1, 6, true, "-Select Godown-");
                        txtremarks.Text = Ds1.Tables[6].Rows[0]["remark"].ToString();
                        txtPenalityRemark.Text = Ds1.Tables[6].Rows[0]["PenalityRemarks"].ToString();
                        if (DDGodown.Items.Count > 0)
                        {
                            DDGodown.SelectedIndex = 1;
                        }

                    }

                    Fill_ChallanDetail();
                    UtilityModule.ConditionalComboFill(ref ddFinish_Type, @"SELECT PIT.FINISHED_TYPE_ID,FT.FINISHED_TYPE_NAME from PurchaseIndentIssueTran PIT,FINISHED_TYPE FT 
                    Where PIT.FINISHED_TYPE_ID=FT.ID AND PINDENTISSUEID=" + DDChallanNo.SelectedValue + " AND FINISHEDID=" + ds.Tables[0].Rows[0]["ITEM_FINISHED_ID"] + "", true, "Finish Type");
                    if (ddFinish_Type.Items.Count > 0)
                    {
                        ddFinish_Type.SelectedIndex = 1;
                    }
                    TxtQty.Focus();
                }
                else
                {
                    Lblmessage.Text = "ITEM CODE DOES NOT EXISTS....";
                    DDCategory.SelectedIndex = 0;
                    ddlcategorycange();
                    TextItemCode.Text = "";
                    TextItemCode.Focus();
                }
            }
        }
        else
        {
            DDCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select Distinct ProductCode from ITEM_PARAMETER_MASTER Where ProductCode Like  '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }


    protected void TxtBillNo1_TextChanged(object sender, EventArgs e)
    {
        if (txtbillno1.Text != null)
        {
            string BillNo = "select * from PurchaseReceiveMaster where BillNo1='" + txtbillno1.Text + "' and partyid=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, BillNo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lblmessage.Text = "BillNo is already Exist........";
                txtbillno1.Text = "";
                txtbillno1.Focus();
            }
            else
            {
                Lblmessage.Text = "";
            }
        }
    }
    protected void DGSHOWDATA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGSHOWDATA.PageIndex = e.NewPageIndex;
        Fill_Grid_Show();
    }
    private void Fill_Grid_Show()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT PRODUCTCODE FROM ITEM_PARAMETER_MASTER WHERE ITEM_FINISHED_ID IN (Select Finishedid From PurchaseIndentIssueTran Where Quantity-isnull(CanQty,0)-[dbo].[Get_PURCHASEPENDINGQTY](FINISHEDID,PINDENTISSUEID,LotNo, OrderID)>0 And PIndentIssueid=" + DDChallanNo.SelectedValue + ") And PRODUCTCODE<>'' And MasterCompanyId=" + Session["varCompanyId"]);
        DGSHOWDATA.DataSource = Ds;
        DGSHOWDATA.DataBind();
        if (DGSHOWDATA.Rows.Count > 0)
            DGSHOWDATA.Visible = true;
        else
            DGSHOWDATA.Visible = false;
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            tdchalan.Visible = true;
            tdchalanno.Visible = true;
            DDCompanyName.Enabled = true;
            DDPartyName.Enabled = true;
            DDChallanNo.Enabled = true;
            TxtReceiveDate.Enabled = true;
            txtretdate.Enabled = true;
            TxtBillNo.Enabled = false;
            TxtGateInNo.Enabled = false;
            TDUpdateMainRemark.Visible = false;
            if (Session["varcompanyId"].ToString() == "21")
            {
                TdlnkupdatebillNo.Visible = true;
                TxtBillNo.Enabled = true;
                //DGPurchaseReceiveDetail.ShowEditButton="True"
                TDUpdateMainRemark.Visible = true;
            }
            refresh();
            if (variable.VarMATERIALRECEIVEWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = true;
            }

            if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
            {
                BtnSave.Visible = false;
                BtnUpdateLegalVendor.Visible = false;
                DGPurchaseReceiveDetail.Columns[14].Visible = false;
                DGPurchaseReceiveDetail.Columns[15].Visible = false;
            }
        }
        else
        {
            TDUpdateMainRemark.Visible = false;
            TdlnkupdatebillNo.Visible = false;
            tdchalanno.Visible = false;
            tdchalan.Visible = false;
            DDCompanyName.Enabled = true;
            DDPartyName.Enabled = true;
            DDChallanNo.Enabled = true;
            TxtReceiveDate.Enabled = true;
            txtretdate.Enabled = true;
            TxtBillNo.Enabled = true;
            TxtGateInNo.Enabled = true;
            ViewState["PurchaseReceiveId"] = 0;
            if (variable.VarMATERIALRECEIVEWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = true;
            }
            refresh();
        }
    }
    protected void Txtchalan_no_TextChanged(object sender, EventArgs e)
    {
        TxtchalanNoTextChanged();
    }
    private void TxtchalanNoTextChanged()
    {
        DataSet ds2 = new DataSet();
        try
        {
            if (txtchalan_no.Text != "")
            {
                string str1 = @"select distinct prm.purchasereceiveid,prm.companyid,prm.partyid,prm.receiveno,Replace(convert(nvarchar(11),prm.receivedate,106),' ','-') As ReceiveDate,
                            prm.gateinno,prm.billno,prd.pindentissueid ,prd.godownid,prm.billno1,qtyreturn,isnull(Replace(convert(nvarchar(11),prm.BillDate,106),' ','-'),'') As BillDate, prm.LEGALVENDORID 
                            From PurchaseReceiveMaster prm right outer join 
                            PurchaseReceiveDetail prd on prd.purchasereceiveid=prm.purchasereceiveid
                            Where prm.companyid = " + DDCompanyName.SelectedValue + " And prm.billno='" + txtchalan_no.Text + "' And prm.MasterCompanyId=" + Session["varCompanyId"];
                if (DDPartyName.SelectedIndex > 0)
                {
                    str1 = str1 + @" And prm.PartyID = " + DDPartyName.SelectedValue;
                }
                ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    if (DDPartyName.SelectedIndex == 0)
                    {
                        DDPartyName.SelectedValue = ds2.Tables[0].Rows[0]["partyid"].ToString();
                    }
                    if (Tdlegalvendor.Visible == true)
                        DDlegalvendor.SelectedValue = ds2.Tables[0].Rows[0]["LEGALVENDORID"].ToString();

                    UtilityModule.ConditionalComboFill(ref DDChallanNo, " select PIndentIssueId,ChallanNo from PurchaseIndentIssue where PartyId=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "--Select Order No--");
                    DDChallanNo.SelectedValue = ds2.Tables[0].Rows[0]["pindentissueid"].ToString();
                    string ChallanNo = string.Empty;
                    switch (Session["varcompanyid"].ToString())
                    {
                        case "9":
                            ChallanNo = "BillNo";
                            break;
                        default:
                            ChallanNo = "receiveno+' / '+BillNo";
                            break;
                    }
                    UtilityModule.ConditionalComboFill(ref ddlrecchalanno, "select distinct prm.PurchaseReceiveId," + ChallanNo + " as ChallanNo from PurchaseReceiveMaster prm left outer join PurchaseReceiveDetail prd  on prd.purchasereceiveid=prm.purchasereceiveid where pindentissueid=" + DDChallanNo.SelectedValue + " And prm.MasterCompanyId=" + Session["varCompanyId"], true, "--SELECT--");
                    ddlrecchalanno.SelectedValue = ds2.Tables[0].Rows[0]["PurchaseReceiveId"].ToString();
                    ViewState["PurchaseReceiveId"] = ddlrecchalanno.SelectedValue;
                    TxtReceiveDate.Text = ds2.Tables[0].Rows[0]["receivedate"].ToString();
                    TxtGateInNo.Text = ds2.Tables[0].Rows[0]["gateinno"].ToString();
                    TxtBillNo.Text = ds2.Tables[0].Rows[0]["billno"].ToString();
                    DDGodown.SelectedValue = ds2.Tables[0].Rows[0]["godownid"].ToString();
                    TxtReceiveNo.Text = ds2.Tables[0].Rows[0]["receiveno"].ToString();
                    txtbillno1.Text = ds2.Tables[0].Rows[0]["billno1"].ToString();
                    txtBillDate.Text = ds2.Tables[0].Rows[0]["BillDate"].ToString();
                    UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct ICM.Category_Id,ICM.Category_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
                    Fill_Grid_Show();
                    switch (Convert.ToInt16(Session["varcompanyId"]))
                    {
                        case 6:   //ArtIndia
                        case 7:  //Dilli Karigari
                        case 9: //Hafizia
                            Fill_porder();
                            break;
                        default:
                            Fill_porder();
                            break;
                    }

                    fill_grid();
                    BtnPreview.Enabled = true;
                    report();
                    TextItemCode.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
    }
    protected void ddlrecchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["PurchaseReceiveId"] = ddlrecchalanno.SelectedValue;
            DataSet ds2 = new DataSet();
            string str1 = @"select distinct prm.purchasereceiveid,prm.companyid,prm.partyid,prm.receiveno,replace(convert(varchar(11),prm.receivedate,106), ' ','-') as receivedate,prm.gateinno,prm.billno,prd.pindentissueid,prd.godownid ,prm.billno1,prd.qtyreturn,prm.Mremark
                      ,isnull(replace(convert(varchar(11),prm.BillDate,106), ' ','-'),'') as BillDate From PurchaseReceiveMaster prm right outer join 
                      PurchaseReceiveDetail prd on prd.purchasereceiveid=prm.purchasereceiveid
                      Where prm.PurchaseReceiveId=" + ddlrecchalanno.SelectedValue + " And prm.MasterCompanyId=" + Session["varCompanyId"];
            ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                TxtReceiveDate.Text = ds2.Tables[0].Rows[0]["receivedate"].ToString();
                TxtGateInNo.Text = ds2.Tables[0].Rows[0]["gateinno"].ToString();
                TxtBillNo.Text = ds2.Tables[0].Rows[0]["billno"].ToString();
                txtchalan_no.Text = TxtBillNo.Text;
                TxtReceiveNo.Text = ds2.Tables[0].Rows[0]["receiveno"].ToString();
                txtbillno1.Text = ds2.Tables[0].Rows[0]["billno1"].ToString();
                txtmastremark.Text = ds2.Tables[0].Rows[0]["Mremark"].ToString();
                txtBillDate.Text = ds2.Tables[0].Rows[0]["BillDate"].ToString();
                //Fill_Grid_Show();
                fill_grid();
                BtnPreview.Enabled = true;
                TxtReceiveDate.Focus();
                DDCategory.SelectedIndex = -1;
                DDItem.SelectedIndex = -1;
                DDQuality.SelectedIndex = -1;
                DDDesign.SelectedIndex = -1;
                DDColor.SelectedIndex = -1;
                DDColorShade.SelectedIndex = 1;

            }
        }
        catch (Exception ex)
        {
            MSg = ex.Message.ToString();
            MessageSave(MSg);
            return;
        }
    }
    public void fillchkbox()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"Select QcmasterID from QCDETAIL where RecieveDetailID=" + DGPurchaseReceiveDetail.SelectedDataKey.Value + " and Qcvalue='1' and RefName='PurchaseReceiveDetail'";
        SqlDataAdapter sda = new SqlDataAdapter(qry, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            for (int j = 0; j < grdqualitychk.Rows.Count; j++)
            {
                if (Convert.ToString(grdqualitychk.DataKeys[j].Value) == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                {
                    CheckBox chk = (CheckBox)grdqualitychk.Rows[j].FindControl("CheckBox1");
                    chk.Checked = true;
                }
            }
        }
    }
    protected void DGPurchaseReceiveDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        try
        //        {
        //            string view2 = "";
        //            if (Session["varcompanyno"].ToString() == "20")
        //            {
        //                view2 = "V_FinishedItemDetailNew";
        //            }
        //            else
        //            {
        //                view2 = "V_FinishedItemDetail";
        //            }
        //            hnqty.Value = "0";
        //            if (ChkEditOrder.Checked == true)
        //            {
        //                ViewState["Gridstatus"] = 1;
        //                int n = DGPurchaseReceiveDetail.SelectedIndex;
        //                if (txtchalan_no.Text == "")
        //                {
        //                    string str = "SELECT receiveno,billno FROM PurchaseReceiveMaster where purchasereceiveid=" + ViewState["PurchaseReceiveId"] + " And MasterCompanyId=" + Session["varCompanyId"];
        //                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //                    txtchalan_no.Text = ds.Tables[0].Rows[0][1].ToString();
        //                    TxtchalanNoTextChanged();
        //                }
        //                UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct ICM.Category_Id,ICM.Category_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
        //                // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        //                if (hncomp.Value == "2")
        //                {
        //                    TextItemCode.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblitem")).Text;
        //                    DDChallanNo.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select pindentissueid from PurchaseIndentissue where MasterCompanyId=" + Session["varCompanyId"] + " And challanno=" + ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("LblChallanNo")).Text + "").ToString();
        //                    item_text_changed();
        //                }
        //                else
        //                {
        //                    string str = @"SELECT category_id,item_id,qualityid,colorid,designid,sizeid,shapeid,shadecolorid,prd.pindentissueid,GodownId,PRM.TransportName,
        //                                 PRM.TransportAddress,PRM.Driver,PRM.VehicleNO,PRM.BiltyNo,Replace(convert(nvarchar(11),PRM.BiltyDate,106),' ','-') As BiltyDate,LotNo,
        //                                 isnull(LshortPercentage,0) as LshortPercentage,PRM.Freight,isnull(Prd.BaleNo,'') as BaleNo,isnull(prd.BinNo,'') as BinNo,Prd.VENDORLOTNO,Prd.OldLotNowise
        //                                 FROM PurchaseReceiveDetail PRD INNER JOIN " + view2 + @" VF ON VF.ITEM_FINISHED_ID=PRD.FINISHEDID INNER JOIN PurchaseReceiveMaster PRM ON PRM.Purchasereceiveid=prd.Purchasereceiveid 
        //                                WHERE PRD.PurchaseReceiveDetailId=" + DGPurchaseReceiveDetail.SelectedDataKey.Value + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        //                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //                    if (ds2.Tables[0].Rows.Count > 0)
        //                    {
        //                        DDChallanNo.SelectedValue = ds2.Tables[0].Rows[0]["pindentissueid"].ToString();
        //                        DDCategory.SelectedValue = ds2.Tables[0].Rows[0]["category_id"].ToString();
        //                        ddlcategorycange();
        //                        UtilityModule.ConditionalComboFill(ref DDItem, "select distinct IM.Item_Id,IM.Item_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " and IM.Category_Id=" + DDCategory.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
        //                        DDItem.SelectedValue = ds2.Tables[0].Rows[0]["item_id"].ToString();
        //                        ddlitem_selectedindex();
        //                        DDQuality.SelectedValue = ds2.Tables[0].Rows[0]["qualityid"].ToString();
        //                        DDDesign.SelectedValue = ds2.Tables[0].Rows[0]["designid"].ToString();
        //                        DDColor.SelectedValue = ds2.Tables[0].Rows[0]["colorid"].ToString();
        //                        DDShape.SelectedValue = ds2.Tables[0].Rows[0]["shapeid"].ToString();
        //                        DDSize.SelectedValue = ds2.Tables[0].Rows[0]["sizeid"].ToString();
        //                        DDColorShade.SelectedValue = ds2.Tables[0].Rows[0]["shadecolorid"].ToString();
        //                        DDGodown.SelectedValue = ds2.Tables[0].Rows[0]["GodownId"].ToString();
        //                        DDGodown_SelectedIndexChanged(sender, new EventArgs());
        //                        if (TDBINNO.Visible == true)
        //                        {
        //                            if (DDBinNo.Items.FindByText(ds2.Tables[0].Rows[0]["BInNo"].ToString()) != null)
        //                            {
        //                                DDBinNo.SelectedValue = ds2.Tables[0].Rows[0]["BinNO"].ToString();
        //                            }
        //                        }
        //                        //Fill_ChallanDetail();
        //                        txtLotNo.Text = (variable.VarPURCHASERECEIVEAUTOGENLOTNO == "1" ? ds2.Tables[0].Rows[0]["VendorLotno"].ToString() : ds2.Tables[0].Rows[0]["LotNo"].ToString());
        //                        txtcomplotno.Text = (variable.VarPURCHASERECEIVEAUTOGENLOTNO == "1" ? ds2.Tables[0].Rows[0]["Lotno"].ToString() : ds2.Tables[0].Rows[0]["VendorLotno"].ToString());
        //                        if (TDCompanyLotNo.Visible == true)
        //                        {
        //                            chkoldlotno.Enabled = false;
        //                            chkoldlotno.Checked = Convert.ToBoolean(ds2.Tables[0].Rows[0]["Oldlotnowise"]);
        //                        }
        //                        //(variable.VarPURCHASERECEIVEAUTOGENLOTNO == "1" ? CompanyLotno : Convert.ToString(_arrpara[15].Value))
        //                        chkTransportInformation.Checked = true;
        //                        chkTransportInformation_CheckedChanged(sender, e);
        //                        txtTransportName.Text = ds2.Tables[0].Rows[0]["TransportName"].ToString();
        //                        txtTransportAddress.Text = ds2.Tables[0].Rows[0]["TransportAddress"].ToString();
        //                        txtDriver.Text = ds2.Tables[0].Rows[0]["Driver"].ToString();
        //                        txtVehicleNo.Text = ds2.Tables[0].Rows[0]["VehicleNo"].ToString();
        //                        txtBiltyNo.Text = ds2.Tables[0].Rows[0]["BiltyNo"].ToString();
        //                        txtBiltyDate.Text = ds2.Tables[0].Rows[0]["BiltyDate"].ToString();
        //                        txtLshort.Text = ds2.Tables[0].Rows[0]["Lshortpercentage"].ToString();
        //                        txtfreight.Text = ds2.Tables[0].Rows[0]["Freight"].ToString();
        //                        txtbaleno.Text = ds2.Tables[0].Rows[0]["BaleNo"].ToString();

        //                    }
        //                }
        //                Txtreturnqty.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblretqty")).Text;
        //                Txtnetamount.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblnetamount")).Text;
        //                TxtAmount.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblamount")).Text;
        //                txtvat.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblvat")).Text;
        //                txtcst.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblcst")).Text;
        //                TxtQty.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblqnt")).Text;
        //                txtbellwt.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblbellwt")).Text;
        //                txtSGST.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblSGST")).Text;
        //                txtIGST.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblIGST")).Text;
        //                txtremarks.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("Remark")).Text;
        //                txtmastremark.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("Mremark")).Text;
        //                string DetailID = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lbldetailid")).Text;
        //                txtPenalityRemark.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lblPenalityRemarks")).Text;

        //                ViewState["qty"] = TxtQty.Text;
        //                TxtPenalty.Text = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("LblPenality")).Text;
        //                string b = Txtreturnqty.Text;
        //                string a = TxtPQty.Text;
        //                if (a == "")
        //                    a = "0";
        //                else
        //                    a = TxtPQty.Text;

        //                //TxtPQty.Text = Convert.ToString(Convert.ToDouble(a.ToString()) + Convert.ToDouble(TxtQty.Text) - Convert.ToDouble(Txtreturnqty.Text));
        //                TxtPQty.Text = Convert.ToString(Math.Round(Convert.ToDouble(a.ToString()) + Convert.ToDouble(TxtQty.Text) - Convert.ToDouble(Txtreturnqty.Text), 3, MidpointRounding.AwayFromZero));
        //                hnprid.Value = ((Label)DGPurchaseReceiveDetail.Rows[n].FindControl("lbldetailid")).Text;
        //                fillchkbox();
        //                hnqty.Value = Convert.ToString(Convert.ToDouble(TxtQty.Text) + Convert.ToDouble(Txtreturnqty.Text));

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
        //            Lblmessage.Visible = true;
        //            Lblmessage.Text = ex.Message;
        //        }
    }
    private void save_refresh()
    {
        DDPartyName.SelectedIndex = 0;
        DDChallanNo.SelectedIndex = 0;
        TxtGateInNo.Text = "";
        TxtBillNo.Text = "";
        TextItemCode.Text = "";
        DDCategory.SelectedIndex = 0;
        DDGodown.SelectedIndex = 0;
        DDColor.SelectedIndex = 0;
        DDColorShade.SelectedIndex = 0;
        DDDesign.SelectedIndex = 0;
        ddFinish_Type.SelectedIndex = 0;
        DDItem.SelectedIndex = 0;
        TxtPQty.Text = "";
        txtrate.Text = "";
        TxtQty.Text = "";
        txtorderqty.Text = "0";
    }
    private void refresh()
    {
        ddlrecchalanno.Items.Clear();
        TxtGateInNo.Text = "";
        TxtBillNo.Text = "";
        txtchalan_no.Text = TxtBillNo.Text;
        TxtReceiveNo.Text = "";
        TextItemCode.Text = "";
        DDColor.Items.Clear();
        DDColorShade.Items.Clear();
        DDDesign.Items.Clear();
        ddFinish_Type.Items.Clear();
        DDSize.Items.Clear();
        TxtPQty.Text = "";
        txtorderqty.Text = "0";
        txtrate.Text = "";
        TxtQty.Text = "";
        fill_grid();
        DDItem.Items.Clear();
        txtmastremark.Text = "";
    }
    //protected void BtnNew_Click(object sender, EventArgs e)
    //{
    //    refresh();
    //    DDCompanyName.Enabled = true;
    //    DDPartyName.Enabled = true;
    //    DDChallanNo.Enabled = true;
    //    TxtReceiveDate.Enabled = true;
    //    TxtBillNo.Enabled = true;
    //    //DDChallanNo.Items.Clear();
    //    DGPurchaseReceiveDetail.Visible = false;
    //}
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ChallanDetail();
        if (TDBINNO.Visible == true)
        {
            FillBinno();
        }
        TxtQty.Focus();
    }
    private void fill_tax()
    {
        if (TxtAmount.Text != "")
        {
            //double vat = 0.00, cst = 0.00;
            //if (txtvat.Text != "")
            //    vat = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtvat.Text) / 100;
            //if (txtcst.Text != "")
            //    cst = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtcst.Text) / 100;
            //Txtnetamount.Text = Convert.ToString(Convert.ToDouble(TxtAmount.Text) + vat + cst);

            double SGST = 0.00, IGST = 0.00,TCS=0.00;
            if (txtSGST.Text != "")
                SGST = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtSGST.Text == "" ? "0" : txtSGST.Text) / 100;
            if (txtIGST.Text != "")
                IGST = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtIGST.Text == "" ? "0" : txtIGST.Text) / 100;
            if (txtTCS.Text != "")
                TCS = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtTCS.Text == "" ? "0" : txtTCS.Text) / 100;
            Txtnetamount.Text = Convert.ToString(Convert.ToDouble(TxtAmount.Text) + SGST + IGST+TCS);
        }
    }

    private void fillAmount()
    {
        double amount = 0;
        double Qty = 0;
        Qty = Convert.ToDouble(TxtQty.Text == "" ? "0" : TxtQty.Text);
        //Lshort
        Qty = Qty - (Qty * Convert.ToDouble((txtLshort.Text == "" ? "0" : txtLshort.Text)) / 100);
        //Bell Wt
        Qty = Qty - Convert.ToDouble(txtbellwt.Text == "" ? "0" : txtbellwt.Text);
        //
        amount = Qty * Convert.ToDouble(txtrate.Text == "" ? "0" : txtrate.Text);
        TxtAmount.Text = amount.ToString();
        ////vat
        //double vat = 0.00, cst = 0.00;//,//freight=0.00;
        //vat = amount * Convert.ToDouble(txtvat.Text == "" ? "0" : txtvat.Text) / 100;
        //cst = amount * Convert.ToDouble(txtcst.Text == "" ? "0" : txtcst.Text) / 100;
        ////freight =Convert.ToDouble(txtfreight.Text == "" ? "0" : txtfreight.Text);
        ////cst

        ////CGST SGST IGST
        double SGST = 0.00, IGST = 0.00,TCS=0.00;//,//freight=0.00;
        SGST = amount * (Convert.ToDouble(txtSGST.Text == "" ? "0" : txtSGST.Text) + Convert.ToDouble(txtCGST.Text == "" ? "0" : txtCGST.Text)) / 100;
        IGST = amount * Convert.ToDouble(txtIGST.Text == "" ? "0" : txtIGST.Text) / 100;
        TCS = amount * Convert.ToDouble(txtTCS.Text == "" ? "0" : txtTCS.Text) / 100;
        //freight =Convert.ToDouble(txtfreight.Text == "" ? "0" : txtfreight.Text);

        ////Penality
        amount = amount + SGST + IGST+TCS;
        amount = amount - Convert.ToDouble(TxtPenalty.Text == "" ? "0" : TxtPenalty.Text);
        Txtnetamount.Text = amount.ToString();
    }
    protected void txtvat_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        txtcst.Focus();
    }
    protected void txtcst_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        BtnSave.Focus();
    }
    protected void txtSGST_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        txtIGST.Focus();
    }
    protected void txtIGST_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        BtnSave.Focus();
    }
    protected void txtTCS_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        BtnSave.Focus();
    }
    protected void txtrate_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
        TxtPenalty.Focus();
    }
    protected void DGPurchaseReceiveDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (HnForDeleteCommand.Value == "true")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int VarPurchase_Rec_Detail_Id = Convert.ToInt32(DGPurchaseReceiveDetail.DataKeys[e.RowIndex].Value);
                SqlParameter[] _arrpara = new SqlParameter[3];
                _arrpara[0] = new SqlParameter("@PurchaseReceiveDetailId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@VarRowCount", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@varMsgFlag", SqlDbType.NVarChar, 250);
                _arrpara[0].Value = VarPurchase_Rec_Detail_Id;
                _arrpara[1].Value = DGPurchaseReceiveDetail.Rows.Count;
                _arrpara[2].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceiveDeleteRow", _arrpara);
                Tran.Commit();
                if (_arrpara[2].Value.ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _arrpara[2].Value + "');", true);
                }
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PurchaseReceiveDetail'," + VarPurchase_Rec_Detail_Id + ",getdate(),'Delete')");
                fill_grid();
                //Fill_Grid_Show();
                if (DGPurchaseReceiveDetail.Rows.Count == 0)
                {
                    string ChallanNo = string.Empty;
                    switch (Session["varcompanyid"].ToString())
                    {
                        case "9":
                            ChallanNo = "BillNo";
                            break;
                        default:
                            ChallanNo = "receiveno+' / '+BillNo";
                            break;
                    }
                    UtilityModule.ConditionalComboFill(ref ddlrecchalanno, "select distinct prm.PurchaseReceiveId," + ChallanNo + " as challanNo from PurchaseReceiveMaster prm left outer join PurchaseReceiveDetail prd  on prd.purchasereceiveid=prm.purchasereceiveid where pindentissueid=" + DDChallanNo.SelectedValue + " And prm.MasterCompanyId=" + Session["varCompanyId"], true, "--SELECT--");
                    UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct ICM.Category_Id,ICM.Category_Name from PurchaseIndentIssueTran PIT inner join Item_Parameter_Master IPM  on PIT.FinishedId=IPM.Item_Finished_Id inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where PIT.PIndentIssueId=" + DDChallanNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"], true, "--Select--");
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
                Tran.Rollback();
                Lblmessage.Visible = true;
                Lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    public void report()
    {
        if (hncomp.Value == "6")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_artindiaNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "20")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_MaltiRugWithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "21")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNEWKaysonsWithRate.rpt";
                //Session["ReportPath"] = "Reports/PurchaseReceiveNEWKaysons.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_KaysonsWithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "39")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNewIndusKleed.rpt";               
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "43")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNewCarpetInternational.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "44")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceivenewagni.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "14")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNEWEastern.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else if (hncomp.Value == "22")
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNEWDiamond.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        else
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseReceiveNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseReceive_WithoutRateNew.rpt";
            }
        }
        Session["CommanFormula"] = "{V_PurchaseReceiveReport.ReceiveNo} ='" + TxtReceiveNo.Text + "'";
    }
    protected void grdqualitychk_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    private void Fill_porder()
    {
        DGporder.DataSource = GetDetail1();
        DGporder.DataBind();
        if (DGporder.Rows.Count > 0)
        {
            tdporder.Visible = true;
        }
        else
        {
            tdporder.Visible = false;
        }
    }
    private DataSet GetDetail1()
    {
        string view3 = "";
        if (Session["varcompanyno"].ToString() == "20")
        {
            view3 = "V_FinishedItemDetailNew";
        }
        else
        {
            view3 = "V_FinishedItemDetail";
        }
        DataSet ds = null;
        try
        {
            string strsql = @"select pist.orderid, Case When IsNull(OM.LocalOrder, '') = '' Then '' Else IsNull(OM.LocalOrder, '') + ' / ' + OM.CustomerOrderNo + '  ' End + Category_Name+'  '+VF.ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+case when flagsize=0 then VF.SizeFt when flagsize=1 then VF.SizeMtr else VF.SizeInch end 
                            Description,sum(quantity) qty,VF.Item_Finished_Id as finishedid,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,ISNULL([dbo].F_PurchaseReceive(Finishedid,pist.Pindentissuetranid),0) AS RECQTY,pist.Lotno,isnull(Sum(pist.canqty),0) as Canqty
                            From PurchaseIndentIssue pis 
                            inner join PurchaseIndentIssueTran pist on pis.pindentissueid=pist.pindentissueid 
                            inner join " + view3 + @" VF ON pist.finishedid=vf.Item_Finished_Id 
                            Left Join OrderMaster OM ON OM.OrderID = pist.OrderID 
                            Where pis.pindentissueid=" + DDChallanNo.SelectedValue + "  And pis.MasterCompanyId=" + Session["varCompanyId"];

            //if (MasterCompanyId == 16)
            //{
            //    strsql = strsql + " And pist.UnitID in (Select UnitID From Unit(Nolock) Where UnitTypeID <> 2)";
            //}

            strsql = strsql + @" Group by OM.LocalOrder, OM.CustomerOrderNo, Category_Name,VF.ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,
                            ShapeName,vf.Item_Finished_Id,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,
                            vf.item_id,sizeid,Finishedid,pist.PindentIssueid,pist.Pindentissuetranid,pist.orderid,
                            UnitId,Sizemtr,Sizeft,Sizeinch,flagsize,pist.Lotno
                            Having isnull(sum(quantity),0)>ISNULL([dbo].F_PurchaseReceive(Finishedid,pist.Pindentissuetranid),0)";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
        return ds;
    }
    //protected void DGporder_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGporder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGporder, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGporder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int r = Convert.ToInt32(DGporder.SelectedIndex.ToString());
            string category = ((Label)DGporder.Rows[r].FindControl("lblcategoryid")).Text;
            string Item = ((Label)DGporder.Rows[r].FindControl("lblitem_id")).Text;
            string Quality = ((Label)DGporder.Rows[r].FindControl("lblQualityid")).Text;
            string Color = ((Label)DGporder.Rows[r].FindControl("lblColorid")).Text;
            string design = ((Label)DGporder.Rows[r].FindControl("lbldesignid")).Text;
            string shape = ((Label)DGporder.Rows[r].FindControl("lblshapeid")).Text;
            string shadecolor = ((Label)DGporder.Rows[r].FindControl("lblshadecolorid")).Text;
            string size = ((Label)DGporder.Rows[r].FindControl("lblsizeid")).Text;
            string Qty = ((Label)DGporder.Rows[r].FindControl("lblqty")).Text;
            string canQty = ((Label)DGporder.Rows[r].FindControl("lblcanqty")).Text;
            string recQty = ((Label)DGporder.Rows[r].FindControl("lblrecqty")).Text;
            string orderid = ((Label)DGporder.Rows[r].FindControl("lblorderid")).Text;
            string finishedid = ((Label)DGporder.Rows[r].FindControl("lblfinishedid")).Text;
            string Lotno = ((Label)DGporder.Rows[r].FindControl("lbllotno")).Text;
            //TxtReceiveNo.Text = recNo;
            DDCategory.SelectedValue = category;
            ddlcategorycange1();
            DDItem.SelectedValue = Item;
            item_indexchange();
            if (TdQuality.Visible == true)
            {
                DDQuality.SelectedValue = Quality;
                quality_change();
            }
            if (TdDesign.Visible == true)
            {
                DDDesign.SelectedValue = design;
                fill_design();
            }
            if (TdColor.Visible == true)
            {
                DDColor.SelectedValue = Color;
                fill_colour();
            }
            if (TdColorShade.Visible == true)
            {
                DDColorShade.SelectedValue = shadecolor;
                fillshadecolour();
            }
            if (TdShape.Visible == true)
            {
                DDShape.SelectedValue = shape;
                fill_size();
            }
            if (TdSize.Visible == true)
            {
                DDSize.SelectedValue = size;
                Fill_ChallanDetail();
            }
            if (TDBINNO.Visible == true)
            {
                FillBinno();
            }
            if (TDCustomerOrderNo.Visible == true)
            {
                FillCustomerOrderNo();
                DDCustomerOrderNo.SelectedValue = orderid;
                Fill_ChallanDetail();
            }

            TxtQty.Text = Convert.ToString(Math.Round(Convert.ToDouble(Qty) - Convert.ToDouble(canQty) - Convert.ToDouble(recQty), 3, MidpointRounding.AwayFromZero));
            if (Convert.ToDouble(TxtQty.Text) < 0)
                TxtQty.Text = "0";
            if (TxtQty.Text != "" && txtrate.Text != "")
            {
                TxtAmount.Text = Convert.ToString(Convert.ToDouble(txtrate.Text) * Convert.ToDouble(TxtQty.Text));
                fillAmount();
            }

            if (tdlot.Visible == true)
            {
                if (DDLotNo.Items.FindByValue(Lotno) != null)
                {
                    DDLotNo.SelectedValue = Lotno;
                    LotNoSelectedIndexChange();
                    if (TDRecLotNo.Visible == true)
                    {
                        txtLotNo.Text = DDLotNo.SelectedItem.Text;
                    }
                }
            }
            TxtGateInNo.Focus();
            if (Session["varcompanyno"].ToString() == "7")
            {
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select remark from OrderLocalConsumption where orderid=" + orderid + " and Finishedid=" + finishedid + "");
                Label3.Text = "Fabric Remark :-" + ds.Tables[0].Rows[0][0].ToString();
            }
            hnprid.Value = "0";
        }
        catch (Exception ex)
        {
            Lblmessage.Text = ex.Message;
        }
    }
    public string getgiven(string strval, string strval1, string canqty)
    {
        string val = "";
        val = Convert.ToString(Math.Round(Convert.ToDouble(strval) - Convert.ToDouble(canqty) - Convert.ToDouble(strval1), 3, MidpointRounding.AwayFromZero));
        return val;
    }
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LotNoSelectedIndexChange();
    }
    private void LotNoSelectedIndexChange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = null;
            int color = 0;
            int quality = 0;
            int design = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
            {
                quality = 1;
            }
            if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
            {
                design = 1;
            }
            if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
            {
                color = 1;
            }
            if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
            {
                shape = 1;
            }
            if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
            {
                size = 1;
            }
            if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {
                int finishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                if (finishedid > 0)
                {
                    txtorderqty.Text = "0";
                    strsql = @"Select Round(ISNULL(SUM(QUANTITY-Isnull(CanQty,0)),0)-[dbo].[Get_PURCHASEPENDINGQTY](FINISHEDID,PINDENTISSUEID,LotNo, OrderID),3) Qty 
                        From PurchaseIndentIssueTran Where PINDENTISSUEID=" + DDChallanNo.SelectedValue + " AND FINISHEDID=" + finishedid + " And lotNo='" + DDLotNo.SelectedItem.Text + @"'";
                    if (TDCustomerOrderNo.Visible == true)
                    {
                        strsql = strsql + " And OrderID = " + DDCustomerOrderNo.SelectedValue;
                    }

                    strsql = strsql + " GROUP BY FINISHEDID,PINDENTISSUEID,LotNo, OrderID";

                    strsql = strsql + @" Select rate,quantity-isnull(CanQty,0) quantity 
                    From PurchaseIndentIssueTran Where FinishedId=" + finishedid + "  and PIndentIssueId=" + DDChallanNo.SelectedValue + " And lotNo='" + DDLotNo.SelectedItem.Text + "'";
                    if (TDCustomerOrderNo.Visible == true)
                    {
                        strsql = strsql + " And OrderID = " + DDCustomerOrderNo.SelectedValue;
                    }

                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        txtrate.Text = ds.Tables[1].Rows[0]["rate"].ToString();
                        txtorderqty.Text = ds.Tables[1].Rows[0]["quantity"].ToString();
                    }
                    TxtPQty.Text = "0";
                    //DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TxtPQty.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseReceive.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
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
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "Select$" + e.Row.RowIndex);
        }

    }
    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = DGSHOWDATA.SelectedIndex;
        TextItemCode.Text = ((Label)DGSHOWDATA.Rows[n].FindControl("lblPRODUCTCODE")).Text;
        item_text_changed();
        UtilityModule.ConditionalComboFill(ref DDGodown, "Select GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select Godown");
        ////UtilityModule.ConditionalComboFill(ref DDGodown, "Select GodownId,GodownName From GodownMaster Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select Godown");
        if (DDGodown.Items.Count > 0)
        {
            DDGodown.SelectedIndex = 1;
        }
        DDGodown.Focus();
    }
    protected void btnqcchkpreview_Click(object sender, EventArgs e)
    {
        string view4 = "";
        if (Session["varcompanyno"].ToString() == "20")
        {
            view4 = "V_FinishedItemDetailNew";
        }
        else
        {
            view4 = "V_FinishedItemDetail";
        }
        string SName = "";
        string QCValue = "";
        string qry = "";
        DataSet ds = new DataSet();
        qry = @"Select CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel,CI.TinNo,Email,EmpName,Address,PhoneNo,Mobile,Fax,PRD.PurchaseReceiveId,PRM.CompanyId,
                PRM.PartyId,PRM.ReceiveNo,PRM.ReceiveDate,PRM.GateInNo,PRM.BillNo,U.UnitName,PRD.GodownId,PRD.LotNo,Sum(PRD.Qty) Qty,PRD.Rate,
                VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,GM.GodownName,Sum(Penalty) Penality,
                PRD.PurchaseReceiveDetailId,'' SName,'' QCValue
                From PurchaseReceiveMaster PRM,PurchaseReceiveDetail PRD," + view4 + @" VF,GodownMaster GM,Unit U,CompanyInfo CI,EmpInfo EI
                Where PRM.PurchaseReceiveId=PRD.PurchaseReceiveId And VF.Item_Finished_Id=PRD.FinishedId And PRD.GodownId=GM.GoDownID And 
                PRD.UnitId=U.UnitId And PRM.CompanyId=CI.CompanyId And PRM.PartyId=EI.EmpId And PRM.PurchaseReceiveId=" + ViewState["PurchaseReceiveId"] + @" 
                Group By PRD.PurchaseReceiveId,PRM.CompanyId,PRM.PartyId,PRM.ReceiveNo,PRM.ReceiveDate,PRM.GateInNo,PRM.BillNo,U.UnitName,PRD.GodownId,PRD.LotNo,PRD.Rate,
                VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,GM.GodownName,CompanyName,CompAddr1,
                CompAddr2,CompAddr3,CompFax,CompTel,CI.TinNo,Email,EmpName,Address,PhoneNo,Mobile,Fax,PRD.PurchaseReceiveDetailId";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);

        DataTable mytable = new DataTable();
        mytable.Columns.Add("PrtID", typeof(int));
        mytable.Columns.Add("SName", typeof(string));
        mytable.Columns.Add("QCValue", typeof(string));

        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        {
            string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
                         Where RefName= 'PurchaseReceiveDetail' And ProcessId=9 And QCD.RecieveID=" + ViewState["PurchaseReceiveId"] + " And QCD.RecieveDetailID=" + ds.Tables[0].Rows[j]["PurchaseReceiveDetailId"];
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (SName == "" && QCValue == "")
                {
                    SName = dt.Rows[i]["SName"].ToString();
                    QCValue = dt.Rows[i]["QCValue"].ToString();
                }
                else
                {
                    SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
                    QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
                }
            }
            mytable.Rows.Add(ds.Tables[0].Rows[j]["PurchaseReceiveDetailId"], SName, QCValue);
            SName = "";
            QCValue = "";
        }
        ds.Tables.Add(mytable);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/RptPurchaseRecQC.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseRecQC.xsd";
            Session["GetDataset"] = ds;
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



    protected void chkTransportInformation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTransportInformation.Checked == true)
        {
            DivTransPort.Visible = true;

        }
        else
        {
            DivTransPort.Visible = false;
        }
    }

    protected void txtLshort_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
    }
    protected void TxtPenalty_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
    }
    protected void txtfreight_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
    }
    protected void txtbellwt_TextChanged(object sender, EventArgs e)
    {
        fillAmount();
    }
    protected void DDPreviewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        report();
    }
    protected void chkoldlotno_CheckedChanged(object sender, EventArgs e)
    {
        txtcomplotno.Text = "";
    }
    protected void lnkupdatebillNo_Click(object sender, EventArgs e)
    {
        Lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Purchasereceiveid", ddlrecchalanno.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            param[4] = new SqlParameter("@ChallanNo", TxtBillNo.Text);
            param[5] = new SqlParameter("@BillNo", txtbillno1.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEPURCHASERECBILLNO_CHALLANNO", param);
            Tran.Commit();
            Lblmessage.Text = param[1].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altupd", "alert('" + param[1].Value.ToString() + "')", true);
            ddlrecchalanno_SelectedIndexChanged(sender, new EventArgs());
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGPurchaseReceiveDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGPurchaseReceiveDetail.EditIndex = e.NewEditIndex;
        fill_grid();
    }
    protected void DGPurchaseReceiveDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGPurchaseReceiveDetail.EditIndex = -1;
        fill_grid();
    }

    protected void DGPurchaseReceiveDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPurchaseReceiveDetailId = Convert.ToInt32(DGPurchaseReceiveDetail.DataKeys[e.RowIndex].Value);
            TextBox txtDGQty = (TextBox)DGPurchaseReceiveDetail.Rows[e.RowIndex].FindControl("TxtDGQty");
            TextBox TxtDGRate = (TextBox)DGPurchaseReceiveDetail.Rows[e.RowIndex].FindControl("TxtDGRate");

            ////////********Lotno Update Option Only For CarpetInternational Not for Other Company Possible Because TagNo Unique For CI *********
            TextBox txtLotNo = (TextBox)DGPurchaseReceiveDetail.Rows[e.RowIndex].FindControl("txtLotNo");

            SqlParameter[] _arrPara = new SqlParameter[8];
            _arrPara[0] = new SqlParameter("@PurchaseReceiveId", ViewState["PurchaseReceiveId"]);
            _arrPara[1] = new SqlParameter("@PurchaseReceiveDetailId", VarPurchaseReceiveDetailId);
            _arrPara[2] = new SqlParameter("@Userid", Session["varuserid"]);
            _arrPara[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
            _arrPara[4] = new SqlParameter("@Qty", txtDGQty.Text == "" ? "0" : txtDGQty.Text);
            _arrPara[5] = new SqlParameter("@Rate", TxtDGRate.Text == "" ? "0" : TxtDGRate.Text);
            _arrPara[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            _arrPara[6].Direction = ParameterDirection.Output;
            _arrPara[7] = new SqlParameter("@LotNo", txtLotNo.Text == "" ? "Without Lot No" : txtLotNo.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceiveUpdateQty", _arrPara);
            Tran.Commit();
            DGPurchaseReceiveDetail.EditIndex = -1;
            fill_grid();
            Lblmessage.Visible = true;
            Lblmessage.Text = _arrPara[6].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert(" + _arrPara[6].Value + ");", true);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnUpdateLegalVendor_Click(object sender, EventArgs e)
    {
        if (variable.VarMATERIALRECEIVEWITHLEGALVENDOR == "1")
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseReceiveMaster Set LEGALVENDORID = " + DDlegalvendor.SelectedValue + "  Where PurchaseReceiveId = " + ViewState["PurchaseReceiveId"] + "");
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert(Legal vendor updated);", true);
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_ChallanDetail();
    }
    protected void lnkUpdateMainRemark_Click(object sender, EventArgs e)
    {
        Lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Purchasereceiveid", ddlrecchalanno.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            param[4] = new SqlParameter("@MRemark", txtmastremark.Text);           

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEPURCHASEREC_MAINREMARK", param);
            Tran.Commit();
            Lblmessage.Text = param[1].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altupd", "alert('" + param[1].Value.ToString() + "')", true);
            ddlrecchalanno_SelectedIndexChanged(sender, new EventArgs());
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnComplete_Click(object sender, EventArgs e)
    {

    }
}