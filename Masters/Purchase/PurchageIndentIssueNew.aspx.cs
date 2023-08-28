using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

public partial class Masters_Purchase_PurchageIndentIssueNew : System.Web.UI.Page
{
    static int MasterCompanyId;
    string msg = "";
    //static string Purchasecode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        //  DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        //Purchasecode = dr["purchasecode"].ToString();
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            hnqty.Value = "0";
            hncunsp.Value = "0";
            ViewState["PIndentIssueId"] = 0;
            ViewState["PIndentIssueTranId"] = 0;
            txtpindentissueid.Text = "0";
            txtpindentissuedetailid.Text = "0";
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyno"].ToString());
            hncompid.Value = VarCompanyNo.ToString();
            string Qry = @"Select Distinct CI.CompanyId,CompanyName from Companyinfo CI(nolock),Company_Authentication CA(nolock) 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by CompanyName 
            Select PaymentId,PaymentName from Payment(nolock) Where MasterCompanyId=" + Session["varCompanyId"] + @" order by PaymentName
            select TermId,TermName from Term(nolock) Where MasterCompanyId=" + Session["varCompanyId"] + @" order by TermName
            select TransModeid,TransModeName from Transmode(nolock) Where MasterCompanyId=" + Session["varCompanyId"] + @" order by TransModename
            select purchasecode,smssetting,flagforsampleorder,varprodcode from Mastersetting(nolock)
            Select ID, BranchName 
            From BRANCHMASTER BM(nolock) 
            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @";select 1 as id,isnull(CompAddr1,'') as compaddr from CompanyInfo
			union all
			select 2 as id,isnull(CompAddr2,'') as compaddr from CompanyInfo
			union all
			select 3 as id,isnull(CompAddr3,'') as compaddr from CompanyInfo";

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddpayement, DSQ, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddelivery, DSQ, 2, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddtransprt, DSQ, 3, true, "Select Mode");
            UtilityModule.ConditionalComboFill(ref DDCurrency, "select CurrencyId,CurrencyName from currencyinfo order by CurrencyName", true, "--Plz Select Currency--");
            if (Session["varcompanyid"].ToString() == "44")
            {
                UtilityModule.ConditionalComboFillWithDS(ref ddlDeliveryAddress, DSQ, 6, true, "Select Address");
            }
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Currencyinfo, DDCurrency, pWhere: "MasterCompanyId=" + Session["varcompanyId"] + "", pID: "CurrencyId", pName: "CurrencyName", pFillBlank: true, Selecttext: "--Plz Select Currency--");
            ViewState["purchasecode"] = DSQ.Tables[4].Rows[0]["purchasecode"].ToString();

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 5, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            CompanySelectedIndexChanged();
            if (Convert.ToInt16(DSQ.Tables[4].Rows[0]["smssetting"]) == 1)
            {
                chkforSms.Visible = true;
            }
            else
            {
                chkforSms.Visible = false;
            }
            if (Convert.ToInt16(DSQ.Tables[4].Rows[0]["flagforsampleorder"]) == 1)
            {
                chkforsample.Visible = true;
            }
            else
            {
                chkforsample.Visible = false;
            }
            //Prod code
            if (DSQ.Tables[4].Rows[0]["varprodcode"].ToString() == "1")
            {
                tdProCode.Visible = true;
            }
            else
            {
                tdProCode.Visible = false;
            }

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtcomp_date.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtNextdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            lablechange();
            //Size Type
            UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
            //
            txtDeliveryAddress.Visible = true;
            switch (Convert.ToInt32(hncompid.Value))
            {
                case 1:
                    tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    ddempname.Focus();
                    break;
                case 2:
                    tdProCode.Visible = true;
                    TdFinish_Type.Visible = true;
                    chkcustomervise.Checked = true;
                    ddcustomercode.Focus();
                    chkcustomervise.Enabled = false;
                    chkindentvise.Enabled = false;
                    TdWithoutOrder.Visible = true;
                    break;
                case 3:
                    tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    chkcustomervise.Checked = true;
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = true;
                    //LblorderNo.Text = "Buyer Order No.";
                    ddcustomercode.Focus();
                    btnpriview.Visible = true;
                    break;
                case 4:
                    tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    //chkcustomervise.Checked = true;
                    ddcustomercode.Focus();
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = true;
                    //ddempname.Focus();
                    break;
                case 5:
                    tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    //chkcustomervise.Checked = true;
                    ddcustomercode.Focus();
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = true;
                    //ddempname.Focus();
                    break;
                case 6:
                    tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    //chkcustomervise.Checked = true;
                    ddcustomercode.Focus();
                    TxtProdCode.Visible = true;
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = true;
                    DDsizetype.Items.Clear();
                    DDsizetype.Items.Add(new ListItem("FT", "0"));
                    DDsizetype.Items.Add(new ListItem("CM", "1"));
                    DDsizetype.Items.Add(new ListItem("INCH", "2"));
                    trrevisedremark.Visible = true;
                    revisedremark.Visible = false;
                    txtchalanno.Enabled = false;
                    DDPreviewType.SelectedValue = "2";
                    //ddempname.Focus();
                    break;
                case 7:
                    //tdProCode.Visible = true;
                    TdFinish_Type.Visible = true;
                    chkcustomervise.Checked = true;
                    ddcustomercode.Focus();
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = false;
                    TdFinish_Type.Visible = false;
                    Tr2.Visible = false;
                    TxtRate.Visible = false;
                    tdrate.Visible = false;
                    tdlotno.Visible = false;
                    tdamt.Visible = false;
                    Tdweig.Visible = false;
                    tragent.Visible = false;
                    trfright.Visible = false;
                    trtransort.Visible = false;
                    trpament1.Visible = false;
                    tddes.Visible = false;
                    tddes1.Visible = false;
                    tdinsu1.Visible = false;
                    tdinsu.Visible = false;
                    txtdate.Enabled = false;
                    DDPreviewType.Visible = false;
                    Btnorder.Visible = false;
                    btnpriview.Visible = false;
                    Tdnextdate.Visible = true;
                    trrevisedremark.Visible = true;
                    revisedremark.Visible = true;
                    tdthanlength.Visible = true;
                    chkindent.Visible = false;
                    break;
                case 9:  //Hafizia
                    //tdlotno.Visible = false;
                    chkindentvise.Enabled = true;
                    TdlblAgentName.Visible = false;
                    TdtxtAgentName.Visible = false;
                    TdlblCurrency.Visible = true;
                    TdDDCurrency.Visible = true;
                    tddes.Visible = false;
                    tddes1.Visible = false;
                    TdlblFrieghtRate.Visible = false;
                    TdtxtfrieghtRate.Visible = false;
                    break;
                case 10:
                    chkcustomervise.Enabled = true;
                    chkindentvise.Enabled = true;
                    tdlotno.Visible = false;
                    //tdProCode.Visible = false;
                    TdFinish_Type.Visible = false;
                    chkcustomervise.Checked = true;
                    chkcustomervise.Enabled = true;
                    //LblorderNo.Text = "Buyer Order No.";
                    ddcustomercode.Focus();
                    btnpriview.Visible = true;
                    break;
                case 12:  // Choudhary Export
                    chkindentvise.Enabled = true;
                    break;
                case 14:
                    chkindentvise.Enabled = true;
                    DDPreviewType.SelectedValue = "2";
                    break;
                case 16:
                    TDChkForSampleFlag.Visible = true;
                    chkindentvise.Enabled = true;
                    break;
                case 28:
                    chkindentvise.Enabled = true;
                    break;
                case 20:
                    txtchalanno.Enabled = false;
                    TDManualOrderNo.Visible = true;
                    // DDPreviewType.Enabled = false;
                    DDPreviewType.SelectedIndex = 1;
                    break;
                case 22:
                    tdreqby.Visible = true;
                    tdreqfor.Visible = true;
                    DDPreviewType.Items.Add(new ListItem("For Work Order","3"));
                    DDPreviewType.Items.Add(new ListItem("For Sale Order","4"));
                    break;
                case 27:
                    DDPreviewType.SelectedIndex = 0;
                    DDPreviewType.Enabled = false;
                    break;
                case 30:  // SAMARA Export
                    chkindentvise.Enabled = true;
                    txtchalanno.Enabled = false;
                    break;
                case 44:
                    DivOtherInformation.Visible = true;
                    trrevisedremark.Visible = true;
                    ddlDeliveryAddress.Visible = true;
                    txtDeliveryAddress.Visible = false;
                    break;
            }
            OnCheckedChange();
            if (ddcustomercode.Items.Count > 0)
            {
                if (Convert.ToInt32(hncompid.Value) != 2)
                {
                    ddcustomercode.SelectedIndex = 1;
                    ddcustomercodechanged();
                }
            }
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select * From PROCESS_NAME_MASTER PNM,Process_UserType PUT,UserType UT,
            NewUserDetail NUD Where PNM.PROCESS_NAME_ID=PUT.PRocessID And PUT.ID=UT.ID And ApprovalFlag=1 and UT.ID=NUD.UserType And PROCESS_NAME_ID=10 And PNM.MasterCompanyId=" + Session["varCompanyId"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                btnpriview.Visible = false;
            }
        }

        //show edit button
        if (Session["canedit"].ToString() == "0") //non authenticated person
        {
            ChkEditOrder.Enabled = false;
        }
        //
//        if (Convert.ToInt32(Session["varCompanyno"]) == 16 || Convert.ToInt32(Session["varCompanyno"]) == 28)
//        {
//            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select IsNull(ShortName, '') ShortName From BranchMaster(Nolock) 
//                Where CompanyID = " + ddCompName.SelectedValue + " And ID = " + DDBranchName.SelectedValue + " And MasterCompanyId = " + Session["varCompanyId"] + "");
//            if (Ds.Tables[0].Rows.Count > 0)
//            {
//                ViewState["purchasecode"] = Ds.Tables[0].Rows[0]["ShortName"].ToString();
//            }
//        }
      
    }
    public void lablechange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            String[] ParameterList = new String[8];
            ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
            lblqualityname.Text = ParameterList[0];
            lbldesignname.Text = ParameterList[1];
            lblcolorname.Text = ParameterList[2];
            lblshapename.Text = ParameterList[3];
            lblsizename.Text = ParameterList[4];
            lblcategoryname.Text = ParameterList[5];
            lblitemname.Text = ParameterList[6];
            lblshadecolor.Text = ParameterList[7];
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
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
    //protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddcustomercodechanged();
    //    ddorderno.Focus();
    //}
    private void GetDeliveryAddress()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select BranchAddress CompanyAddress 
            From BRANCHMASTER(Nolock) Where CompanyId = " + ddCompName.SelectedValue + " ANd ID = " + DDBranchName.SelectedValue);
        if (Session["varcompanyid"].ToString() != "44")
        {
            txtDeliveryAddress.Text = ds.Tables[0].Rows[0]["CompanyAddress"].ToString();
        }
    }
    private void ddcustomercodechanged()
    {
        string Str = "";
        int VarApprovalFlag = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ApprovalFlag from Process_Name_Master Where Process_Name_Id=10 And MasterCompanyId=" + Session["varCompanyId"] + ""));
        if (ChkEditOrder.Checked == true)
        {
//            if (Session["varcompanyno"].ToString() == "7")
//            {
//                Str = @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo+'/ '+isnull(op.Purhasername,' ') from OrderMaster OM,Jobassigns JA,PurchaseIndentIssue PII,OrderProcessPlanning OP
//                      Where OM.Orderid=JA.Orderid and OP.OrderId=OM.OrderId and om.status=0 and op.processid=1 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.Orderid=PII.Orderid And PII.Companyid=" + ddCompName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"] + "";
//            }
//            else if (Session["varcompanyno"].ToString() == "16" || Session["varcompanyno"].ToString() == "28")
//            {
//                Str = "Select Distinct OM.OrderId, CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and Om.status=0 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.CompanyId=" + ddCompName.SelectedValue;
//                if (VarApprovalFlag == 1)
//                {
//                    Str = Str + " And OM.Orderid in (Select Orderid From Order_Approval)";
//                }
//            }
//            else
//            {
//                Str = @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,PurchaseIndentIssue PII 
//                      Where OM.Orderid=JA.Orderid and om.status=0 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.Orderid=PII.Orderid And PII.CompanyId=" + ddCompName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"] + "";

            Str = @"Select Distinct OM.OrderId,CustomerOrderNo+ ' / ' +LocalOrder from OrderMaster OM,Jobassigns JA,PurchaseIndentIssue PII 
                      Where OM.Orderid=JA.Orderid and om.status=0  And OM.Orderid=PII.Orderid And PII.CompanyId=" + ddCompName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"] + "";
          //  }
            if (VarApprovalFlag == 1)
            {
                Str = Str + " And PII.PindentIssueid in(Select PindentIssueId From Purchase_Approval)";
                btnpriview.Visible = true;
            }
//            if (Session["varcompanyno"].ToString() == "2")
//            {
//                if (TxtProdCode.Text != "")
//                {
//                    Str = Str + @"  AND OM.OrderId IN ( Select Distinct OrderID from PurchaseIndentIssue PII,PurchaseIndentIssueTran PIIT , Item_parameter_master IPM 
//                                where PII.PindentIssueid=PIIT.PindentIssueid AND PIIT.FINISHEDID=IPM.ITEM_FINISHED_ID AND ProductCode='" + TxtProdCode.Text.Trim() + "') ";
//                }
//            }
//            if (Session["varcompanyno"].ToString() == "7")
//            {
//                Str = Str + " Order By OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo+'/ '+isnull(op.Purhasername,' ')";
//            }
//            else if (Session["varcompanyno"].ToString() == "16" || Session["varcompanyno"].ToString() == "28")
//            {
//                Str = Str + " Order By CustomerOrderNo";
//            }
//            else
//            {
            Str = Str + " Order By CustomerOrderNo+ ' / ' +LocalOrder ";
            //}
            UtilityModule.ConditionalComboFill(ref ddorderno, Str, true, "Select OrderNo.");
        }
        else
        {
//            if (hncompid.Value == "7")
//            {
//                UtilityModule.ConditionalComboFill(ref ddorderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo+'/'+isnull(ocp.Purhasername,' ') from Jobassigns JA,orderprocessplanning ocp,OrderMaster OM LEFT OUTER JOIN PurchaseIndentIssue PII oN OM.ORDERID=PII.ORDERID AND PII.Status='Pending'
//                Where om.status=0  and ocp.processid=1 and OM.Orderid=JA.Orderid and ocp.orderid=om.orderid And CustomerId=" + ddcustomercode.SelectedValue + " And Om.Companyid=" + ddCompName.SelectedValue + @" AND ISNULL(OCP.FinalStatus,0)=1 And
//                Om.orderid in(select Distinct orderid from V_OrderConsumptionAndPurchaseQty group by orderid,finishedid Having sum(consumption)>sum(issue))
//                Order By LocalOrder+ ' / ' +CustomerOrderNo+'/'+isnull(ocp.Purhasername,' ') ", true, "Select OrderNo.");
//            }
//            else if (Session["varcompanyno"].ToString() == "2")
//            {
//                Str = "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and Om.status=0 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.CompanyId=" + ddCompName.SelectedValue;
//                if (TxtProdCode.Text != "")
//                {
//                    Str = Str + "  AND OM.OrderId in (Select Distinct OrderID from order_consumption_Detail OCD, Item_parameter_master IPM where OCD.IFINISHEDID=IPM.ITEM_FINISHED_ID AND ProductCode='" + TxtProdCode.Text.Trim() + "') ";
//                }
//                if (VarApprovalFlag == 1)
//                {
//                    Str = Str + " And OM.Orderid in (Select Orderid From Order_Approval)";
//                }
//                Str = Str + " Order By LocalOrder+ ' / ' +CustomerOrderNo";
//                UtilityModule.ConditionalComboFill(ref ddorderno, Str, true, "Select OrderNo.");
//            }
//            else if (Session["varcompanyno"].ToString() == "16" || Session["varcompanyno"].ToString() == "28")
//            {
//                Str = "Select Distinct OM.OrderId, CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and Om.status=0 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.CompanyId=" + ddCompName.SelectedValue;
//                if (VarApprovalFlag == 1)
//                {
//                    Str = Str + " And OM.Orderid in (Select Orderid From Order_Approval)";
//                }
//                Str = Str + " Order By CustomerOrderNo";
//                UtilityModule.ConditionalComboFill(ref ddorderno, Str, true, "Select OrderNo.");
//            }
//            else
//            {
                //Str = "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and Om.status=0 And CustomerId=" + ddcustomercode.SelectedValue + " And OM.CompanyId=" + ddCompName.SelectedValue;

            Str = "Select Distinct OM.OrderId,CustomerOrderNo+ ' / ' +LocalOrder from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and Om.status=0 And OM.CompanyId=" + ddCompName.SelectedValue;
                if (VarApprovalFlag == 1)
                {
                    Str = Str + " And OM.Orderid in (Select Orderid From Order_Approval)";
                }
                Str = Str + " Order By CustomerOrderNo+ ' / ' +LocalOrder";
                UtilityModule.ConditionalComboFill(ref ddorderno, Str, true, "Select OrderNo.");
          //  }
        }
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hncunsp.Value = "";
        DataSet ds7 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Consmp_type,customerid  from ordermaster where orderid=" + ddorderno.SelectedValue + "");
        if (ds7.Tables[0].Rows.Count > 0)
        {
            hncunsp.Value = ds7.Tables[0].Rows[0][0].ToString();
        }
        if(ds7 !=null && ds7.Tables.Count>0 && ds7.Tables[0].Rows.Count>0)
        if (ddcustomercode.Items.FindByValue(ds7.Tables[0].Rows[0]["customerid"].ToString()) != null)
        {
            ddcustomercode.SelectedValue = ds7.Tables[0].Rows[0]["customerid"].ToString();
        }
        //if (ChkEditOrder.Checked == true)
        //{
        //    UtilityModule.ConditionalComboFill(ref ddempname, "select empid,empname from empinfo where MasterCompanyId=" + Session["varCompanyId"] + " And empid in(select distinct partyid from PurchaseIndentIssue where orderid=" + ddorderno.SelectedValue + ") Order By empname", true, "--Select--");
        //    Fill_Grid();
        //}
        if (hncunsp.Value == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME 
            from ITEM_CATEGORY_MASTER icm 
            inner join ITEM_MASTER im on icm.CATEGORY_ID=im.CATEGORY_ID 
            inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id  
            inner join OrderLocalConsumption  OCD on OCD.FINISHEDID=ipm.ITEM_FINISHED_ID 
            inner join UserRights_Category UC on icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @" 
            where OCD.orderid=" + ddorderno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME ", true, "Select Category");

            TDGridShow.Visible = true;

            Fill_GridForLocalConsump();
        }
        else
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
            string str = string.Empty;
            if (chkcustomervise.Checked == true)
            {
                switch (Session["varcompanyid"].ToString())
                {
                    case "6":
                        str = @"SELECT distinct CATEGORY_ID,Category_Name   
                                from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue + " order by Category_Name";
                        break;
                    case "28":
                    case "16":
                        str = @"SELECT distinct CATEGORY_ID, Category_Name   
                                from V_ConsumptionAndPurchaseQtyForChampoPNM vc 
                                inner join V_FinishedItemDetail vf on vc.finishedid=vf.ITEM_FINISHED_ID 
                                Where vc.orderid=" + ddorderno.SelectedValue + " order by Category_Name";
                        break;
                    case "42":
                        str = @"SELECT distinct CATEGORY_ID,Category_Name
                                from V_ConsumptionQtyAndPurchaseQtyNew vc inner join " + view3 + @" vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue + " order by Category_Name";
                        break;
                    case "44":
                        str = @"SELECT distinct CATEGORY_ID,Category_Name
                                from V_ConsumptionQtyAndPurchaseQtyAgni vc inner join " + view3 + @" vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue + " order by Category_Name";
                        break;
                    default:
                        if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                        {
                            str = "select distinct category_id,category_name from ITEM_CATEGORY_MASTER icm inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") And icm.MasterCompanyId=" + Session["varCompanyId"] + " Order By category_name ";
                        }
                        else
                        {
                            str = @"SELECT distinct CATEGORY_ID,Category_Name
                                from V_ConsumptionQtyAndPurchaseQty vc inner join " + view3 + @" vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue + " order by Category_Name";
                        }
                        break;
                }

                UtilityModule.ConditionalComboFill(ref ddCatagory, str, true, "Select Category");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm inner join ITEM_MASTER im on 
            icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id  inner join ORDER_CONSUMPTION_DETAIL OCD on 
            OCD.IFINISHEDID=ipm.ITEM_FINISHED_ID inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where OCD.orderid=" + ddorderno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME ", true, "Select Category");
            }
            Fill_GridForLocalConsump();
            TDGridShow.Visible = true;
        }
        //if (hncompid.Value == "2")
        //{
        //    Fill_Grid_Show();
        //}
        if (hncunsp.Value == "1")
        {
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select replace(convert(varchar(11),FinalDate,106), ' ','-') as date from OrderProcessPlanning where  processid=1 and orderid=" + ddorderno.SelectedValue + "");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                hnreqdate.Value = ds2.Tables[0].Rows[0][0].ToString();
                lblreqdate.Text = "Purchase SRC date -" + ds2.Tables[0].Rows[0][0].ToString();
                txtcomp_date.Text = ds2.Tables[0].Rows[0][0].ToString();
                txtduedate.Text = ds2.Tables[0].Rows[0][0].ToString();
                txtNextdate.Text = ds2.Tables[0].Rows[0][0].ToString();
            }
            TdrequirDate.Visible = true;
        }
        else
        {
            lblreqdate.Text = "";
            TdrequirDate.Visible = true;
        }
        TxtOrderNo.Text = ddorderno.SelectedItem.Text;
        ddempname.Focus();
    }
    protected void chkcustomervise_CheckedChanged(object sender, EventArgs e)
    {
        ddcustomercodechanged();
        ddorderno.Focus();
        chkcustomervise_Checked();
    }
    private void chkcustomervise_Checked()
    {
        if (chkcustomervise.Checked)
        {
            TxtIndentNo.Text = "";
            chkindentvise.Checked = false;
            ddcustomercode.Focus();
        }
        OnCheckedChange();
    }
    protected void chkindentvise_CheckedChanged(object sender, EventArgs e)
    {
        chkindentvise_CheckedChanged();
    }
    private void chkindentvise_CheckedChanged()
    {
        if (chkindentvise.Checked)
        {
            chkcustomervise.Checked = false;
            TxtIndentNo.Focus();
        }
        else
        {
            tdIndenttext.Visible = false;
        }
        OnCheckedChange();
    }
    private void OnCheckedChange()
    {
        
        if (chkcustomervise.Checked)
        {
            if (Session["varcompanyid"].ToString() != "44")
            {
                ddCatagory.Enabled = false;
                dditemname.Enabled = false;
                dquality.Enabled = false;
                dddesign.Enabled = false;
                ddcolor.Enabled = false;
                ddshape.Enabled = false;
                ddsize.Enabled = false;
                ddlshadeNew.Enabled = false;
            }
        }
        else
        {
            ddCatagory.Enabled = true;
            dditemname.Enabled = true;
            dquality.Enabled = true;
            dddesign.Enabled = true;
            ddcolor.Enabled = true;
            ddshape.Enabled = true;
            ddsize.Enabled = true;
            ddlshade.Enabled = true;
        }
        if (chkcustomervise.Checked)
        {
            tdIndenttext.Visible = false;
            tdindentno.Visible = false;
            tdcustomer.Visible = true;
            tdorderno.Visible = true;
            chkindentvise.Checked = false;
            btnopen.Visible = false;

            if (hncompid.Value == "7" || hncompid.Value == "3" || hncompid.Value == "10")
            {
                UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName from customerinfo ci 
                inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join OrderLocalConsumption  ocd on ocd.orderid=om.orderid
                inner join Jobassigns JA ON OM.Orderid=JA.Orderid ", true, "Select CustomerCode");
            }
            else if (hncompid.Value == "6")
            {
                UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid,ci.Customercode from customerinfo ci 
                inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
                inner join Jobassigns JA ON OM.Orderid=JA.Orderid  ", true, "Select CustomerCode");
            }
            else
            {
                if (Session["withoutBOM"].ToString() == "1")
                {
                    UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName from customerinfo ci 
                inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join OrderLocalConsumption  ocd on ocd.orderid=om.orderid
                inner join Jobassigns JA ON OM.Orderid=JA.Orderid ", true, "Select CustomerCode");
                }
                else
                {
                    if (Session["varCompanyId"].ToString() == "44")
                    {
                        UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid, ci.Customercode as Code
                            From customerinfo ci 
                            join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" And OM.CompanyID = " + ddCompName.SelectedValue + @" 
                            join Jobassigns JA ON OM.Orderid=JA.Orderid 
                            Order By ci.Customercode  ", true, "Select CustomerCode");
                    }
                    else {
                        UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid, ci.Customercode + SPACE(5) + CI.CompanyName Code
                            From customerinfo ci 
                            join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" And OM.CompanyID = " + ddCompName.SelectedValue + @" 
                            join Jobassigns JA ON OM.Orderid=JA.Orderid 
                            Order By ci.Customercode + SPACE(5) + CI.CompanyName  ", true, "Select CustomerCode");
                    
                    }
                }

            }


            //string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
            //if (Session["varCompanyno"].ToString() != "6")
            //{
            //    Str = Str + "  AND DM.Departmentname='PURCHASE'";
            //}
            //Str = Str + "  Order By empname ";


            string Str = "select EI.empid,EI.empname+(case when EI.Empcode<>'' Then  '('+Ei.empcode+')' Else '' End) as empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
            if (Session["varCompanyno"].ToString() != "6")
            {
                Str = Str + "  AND DM.Departmentname='PURCHASE'";
            }
            Str = Str + "  Group by EI.empid,EI.empname,EI.EmpCode ";
            Str = Str + "  Order By empname ";

            UtilityModule.ConditionalComboFill(ref ddempname, Str, true, "--Select Party--");
        }
        else if (chkindentvise.Checked)
        {
            tdIndenttext.Visible = false;
            tdindentno.Visible = true;
            tdcustomer.Visible = false;
            tdorderno.Visible = false;
            chkcustomervise.Checked = false;
            AQty.Visible = true;
            PQty.Visible = true;
            btnopen.Visible = false;
            string str = "";
            if (variable.VarPURCHASEORDER_INDENTOTHERVENDOR == "1")
            {
                //str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where  EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0 and EI.Partytype=0";
                //str = str + "  AND DM.Departmentname='PURCHASE'";
                //str = str + "  Order By empname ";

                str = "select EI.empid,EI.empname+(case when EI.Empcode<>'' Then  '('+Ei.empcode+')' Else '' End) as empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where  EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0 and EI.Partytype=0";
                str = str + "  AND DM.Departmentname='PURCHASE'";
                str = str + "  Group by EI.empid,EI.empname,EI.EmpCode";
                str = str + "  Order By empname ";
            }
            else
            {
                //str = "select distinct ei.empid ,empname from empinfo ei inner join  PurchaseIndentMaster pim on ei.empid=pim.partyid And ei.MasterCompanyId=" + Session["varCompanyId"] + " Order By empname ";

                str = "select EI.empid,EI.empname+(case when EI.Empcode<>'' Then  '('+Ei.empcode+')' Else '' End) as empname from empinfo ei inner join  PurchaseIndentMaster pim on ei.empid=pim.partyid And ei.MasterCompanyId=" + Session["varCompanyId"] + "";
                str = str + "  Group by EI.empid,EI.empname,EI.EmpCode";
                str = str + "  Order By empname ";
            }
            UtilityModule.ConditionalComboFill(ref ddempname, str, true, "Select Party");
        }
        else if (chkforsample.Checked)
        {
            if (chkforsample.Checked == true)
            {
                tdIndenttext.Visible = false;
                tdindentno.Visible = false;
                tdcustomer.Visible = true;
                tdorderno.Visible = true;
                chkindentvise.Checked = false;
                btnopen.Visible = false;
                UtilityModule.ConditionalComboFill(ref ddcustomercode, @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName from customerinfo ci 
                inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join OrderLocalConsumption  ocd on ocd.orderid=om.orderid
                inner join Jobassigns JA ON OM.Orderid=JA.Orderid ", true, "Select CustomerCode");
            }
        }
        else
        {
            tdindentno.Visible = false;
            tdcustomer.Visible = false;
            tdorderno.Visible = false;
            chkindentvise.Checked = false;
            chkcustomervise.Checked = false;
            AQty.Visible = false;
            PQty.Visible = false;
            btnopen.Visible = true;
            
            //string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid  Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
            //switch (Session["varcompanyNo"].ToString())
            //{
            //    case "6":
            //        break;
            //    default:
            //        Str = Str + "  AND DM.Departmentname='PURCHASE'";
            //        break;
            //}
            //Str = Str + "  Order By EI.empname ";
            //UtilityModule.ConditionalComboFill(ref ddempname, Str, true, "--Select Party--");

            string Str = "select EI.empid,EI.empname+(case when EI.Empcode<>'' Then  '('+Ei.empcode+')' Else '' End) as empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid  Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
            switch (Session["varcompanyNo"].ToString())
            {
                case "6":
                    break;
                default:
                    Str = Str + "  AND DM.Departmentname='PURCHASE'";
                    break;
            }
            Str = Str + "  Group by EI.empid,EI.empname,EI.EmpCode ";
            Str = Str + "  Order By EI.empname ";
            UtilityModule.ConditionalComboFill(ref ddempname, Str, true, "--Select Party--");

            UtilityModule.ConditionalComboFill(ref ddCatagory, "select distinct category_id,category_name from ITEM_CATEGORY_MASTER icm inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") And icm.MasterCompanyId=" + Session["varCompanyId"] + " Order By category_name ", true, "Select Category");

            if (Session["varcompanyno"].ToString() == "20")
            {
                if (ddCatagory.Items.Count > 0)
                {
                    ddCatagory.SelectedIndex = 2;
                    ddlcategorycange();
                    ddlcategorychange1();
                }
            }
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PIndentIssueId"] = "0";
        EmpNameSelectedChange(sender);
        if (variable.VarCompanyWiseChallanNoGenerated == "0")
        {
            getOrderNo();
        }
    }
    private void EmpNameSelectedChange(object sender = null)
    {
        int VarApprovalFlag = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ApprovalFlag from Process_Name_Master Where Process_Name_Id=10 And MasterCompanyId=" + Session["varCompanyId"] + ""));
        string Str = "";
        if (chkindentvise.Checked == true)
        {
            if (variable.VarPURCHASEORDER_INDENTOTHERVENDOR == "1")
            {
                if (TxtIndentNo.Text == "")
                {
                    if (ChkEditOrder.Checked == true)
                    {
                        Str = "select distinct PM.PIndentId,PM.pindentno from V_PURCHASEINDENTPENDINGINDENTNO PM   where   PM.Companyid=" + ddCompName.SelectedValue + " AND PM.FlagApproval <> 0  Order By pindentno ";
                    }
                    else
                    {
                        Str = "select distinct PM.PIndentId,PM.pindentno from V_PURCHASEINDENTPENDINGINDENTNO PM   where   PM.Companyid=" + ddCompName.SelectedValue + " AND PM.FlagApproval <> 0  and Round((Indentqty-orderedqty),0)>0 Order By pindentno ";
                    }
                    UtilityModule.ConditionalComboFill(ref ddindentno, Str, true, "Select IndentNo");
                }
                else
                {
                    if (ddindentno.Items.Count > 0)
                    {
                        if (sender != null)
                        {
                            ddindentno_SelectedIndexChanged(sender, new EventArgs());
                        }
                    }
                }

            }
            else
            {
                Str = "select distinct PIndentId,pindentno from PurchaseIndentMaster where partyid=" + ddempname.SelectedValue + "And Companyid=" + ddCompName.SelectedValue + " AND FlagApproval <> 0 And MasterCompanyId=" + Session["varCompanyId"] + "  Order By pindentno ";
                UtilityModule.ConditionalComboFill(ref ddindentno, Str, true, "Select IndentNo");
            }


        }
        txtdate.Focus();
        if (ChkEditOrder.Checked == true)
        {
            if (chkcustomervise.Checked == true)
            {
                //Fill Complete ChallanNo
                if (ChkForComplete.Checked == true)
                {
                    Str = @"Select pindentissueid,challanno from PurchaseIndentIssue 
                        Where Status='Complete' And orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + @" And 
                        CompanyId=" + ddCompName.SelectedValue + " And IsNull(BranchID, 0) = " + DDBranchName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
                }
                else
                {
                    Str = @"Select pindentissueid,challanno 
                    from PurchaseIndentIssue 
                    where Status='Pending' And orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + @" And 
                    CompanyId=" + ddCompName.SelectedValue + " And IsNull(BranchID, 0) = " + DDBranchName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
                }
                if (VarApprovalFlag == 1)
                {
                    Str = Str + " And PIndentIssueID in (Select PindentIssueId From Purchase_Approval)";
                }
                Str = Str + " Order By pindentissueid desc";
                UtilityModule.ConditionalComboFill(ref ddlchalanno, Str, true, "--Select--.");
                ddlchalanno.Focus();
            }
            else if (chkcustomervise.Checked == false && chkindentvise.Checked == false)
            {
                if (ChkForComplete.Checked == true)
                {
                    UtilityModule.ConditionalComboFill(ref ddlchalanno, @"select pindentissueid,challanno 
                    From PurchaseIndentIssue 
                    where Status='Complete' And partyid=" + ddempname.SelectedValue + " ANd MasterCompanyId=" + Session["varCompanyId"] + @" And 
                    CompanyId=" + ddCompName.SelectedValue + " And IsNull(BranchID, 0) = " + DDBranchName.SelectedValue + " Order By pindentissueid desc", true, "--Select--.");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddlchalanno, @"select pindentissueid,challanno 
                    from PurchaseIndentIssue 
                    where Status='Pending' And partyid=" + ddempname.SelectedValue + " ANd MasterCompanyId=" + Session["varCompanyId"] + @" And 
                    CompanyId=" + ddCompName.SelectedValue + " And IsNull(BranchID, 0) = " + DDBranchName.SelectedValue + " Order By pindentissueid desc", true, "--Select--.");
                }
            }
        }
        if (hncompid.Value == "7")
        {
            Btnorder.Visible = true;
        }
        if (TDPoNoNew.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDPoNoNew, "select pindentissueid,challanno from PurchaseIndentIssue where Status='Complete' And partyid=" + ddempname.SelectedValue + " And CompanyId=" + ddCompName.SelectedValue + " ANd MasterCompanyId=" + Session["varCompanyId"] + " Order By challanno ", true, "--Select--.");
        }
    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ViewState["PIndentIssueId"] = "0";
        if (ChkEditOrder.Checked == true)
        {
            ////Fill Complete Challan
            string str = @"SELECT Distinct PII.PINDENTISSUEID,PII.CHALLANNO FROM PURCHASEINDENTISSUE PII INNER JOIN PURCHASEINDENTISSUETRAN PIT ON PII.PINDENTISSUEID=PIT.PINDENTISSUEID
                         WHERE PII.COMPANYID=" + ddCompName.SelectedValue + " AND PII.PARTYID=" + ddempname.SelectedValue + " AND PIT.PINDENTID=" + ddindentno.SelectedValue;
            if (ChkForComplete.Checked == true)
            {
                str = str + " and pii.status='Complete'";
            }
            else
            {
                str = str + " and pii.status='Pending'";
            }
            str = str + "   ORDER BY CHALLANNO";
            UtilityModule.ConditionalComboFill(ref ddlchalanno, str, true, "--Select--.");
            //if (ChkForComplete.Checked == true)
            //{
            //    UtilityModule.ConditionalComboFill(ref ddlchalanno, "select pindentissueid,challanno from PurchaseIndentIssue where Status='Complete' And  partyid=" + ddempname.SelectedValue + " and indentid=" + ddindentno.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by challanno ", true, "--Select--.");
            //}
            //else
            //{
            //    UtilityModule.ConditionalComboFill(ref ddlchalanno, "select pindentissueid,challanno from PurchaseIndentIssue where Status='Pending' And  partyid=" + ddempname.SelectedValue + " and indentid=" + ddindentno.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by challanno ", true, "--Select--.");
            //}
        }
        UtilityModule.ConditionalComboFill(ref ddCatagory, @" select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm 
        inner join ITEM_MASTER im on icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id
        inner join PurchaseIndentDetail pim on pim.finishedid=ipm.ITEM_FINISHED_ID inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where pim.pindentid=" + ddindentno.SelectedValue + " And icm.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME ", true, "Select Category");
        //if (hncompid.Value == "6")
        //{
        //    TDGridShow.Visible = true;
        //    Fill_GridForLocalConsump();
        //}
        TDGridShow.Visible = true;
        Fill_GridForLocalConsump();
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyId"].ToString() == "44")
        {
            if (ddCatagory.SelectedValue == "10")
            {
                tdextra.Visible = true;

            }
            else
            {

                tdextra.Visible = false;
            }
        }

        ddlcategorycange();
        ddlcategorychange1();
    }
    private void ddlcategorychange1()
    {
        if (chkindentvise.Checked)
        {
            UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm on im.item_id=ipm.item_id
            inner join PurchaseIndentDetail pid on ipm.ITEM_FINISHED_ID=pid.FinishedId where pid.PIndentId=" + ddindentno.SelectedValue + " and im.category_id=" + ddCatagory.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ", true, "Select Item");
        }
        else if (chkcustomervise.Checked)
        {
            if (hncunsp.Value == "1")
            {
                UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm  on im.item_id=ipm.item_id
                inner join OrderLocalConsumption  ocm on ipm.ITEM_FINISHED_ID=OCM.FinishedId where im.CATEGORY_ID=" + ddCatagory.SelectedValue + " and ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ", true, "Select Item");
            }
            else
            {
                string str = string.Empty;
                switch (Session["varcompanyid"].ToString())
                {
                    case "6":
                        str = @"SELECT distinct ITEM_ID,ITEM_NAME   
                                from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue + " and category_id=" + ddCatagory.SelectedValue + " order by Item_name";
                        break;
                    default:
                        if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                        {
                            str = "select distinct item_id,item_name  from ITEM_MASTER where category_id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name ";
                        }
                        else
                        {
                            str = @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm  on im.item_id=ipm.item_id
                                inner join ORDER_CONSUMPTION_DETAIL ocm on ipm.ITEM_FINISHED_ID=OCM.IFinishedId where im.CATEGORY_ID=" + ddCatagory.SelectedValue + " and ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ";
                        }
                        break;
                }
                UtilityModule.ConditionalComboFill(ref dditemname, str, true, "Select Item");
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref dditemname, "select distinct item_id,item_name  from ITEM_MASTER where category_id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name ", true, "Select Item");
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillitemchange();
    }
    private void fillitemchange()
    {
        try
        {
            if (chkindentvise.Checked)
            {
                UtilityModule.ConditionalComboFill(ref dquality, @"select distinct qualityid, qualityname from quality q inner join ITEM_PARAMETER_MASTER ipm on q.qualityid=ipm.quality_id
                inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID where pid.PIndentId=" + ddindentno.SelectedValue + " and q.item_id=" + dditemname.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "Select Item");
            }
            else if (chkcustomervise.Checked)
            {
                if (hncunsp.Value == "1")
                {
                    UtilityModule.ConditionalComboFill(ref dquality, @" select distinct qualityid, qualityname from quality q inner join ITEM_PARAMETER_MASTER ipm on q.qualityid=ipm.quality_id
                    inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID where ocm.orderid=" + ddorderno.SelectedValue + " and q.item_id=" + dditemname.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "Select Item");
                }
                else
                {
                    if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                    {
                        UtilityModule.ConditionalComboFill(ref dquality, "select distinct qualityid, qualityname from quality where item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "Select Item");
                    }
                    else
                    {
                        UtilityModule.ConditionalComboFill(ref dquality, @" select distinct qualityid, qualityname from quality q inner join ITEM_PARAMETER_MASTER ipm on q.qualityid=ipm.quality_id
                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID where ocm.orderid=" + ddorderno.SelectedValue + " and q.item_id=" + dditemname.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "Select Item");
                    }
                }
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref dquality, "select distinct qualityid, qualityname from quality where item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "Select Item");
            }
            //if (Convert.ToInt32(Session["varCompanyno"]) == 14)
            //{
            //    UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT distinct  u.UnitTypeID as UnitId,u.UnitType as UnitName FROM ITEM_MASTER i INNER JOIN  UNIT_TYPE_MASTER u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"] + " Order By u.UnitType", true, "Select Unit");
            //}
            //else 
            //{
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT distinct u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"] + " Order By u.UnitName ", true, "Select Unit");
            //}
            if (hncompid.Value == "7")
            {
                ddlunit.SelectedValue = "1";
            }
            else if (hncompid.Value == "2")
            {
                ddlunit.SelectedIndex = 1;
            }
            else
            {
                if (ddlunit.Items.Count > 0)
                {
                    ddlunit.SelectedIndex = 1;
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
    }
    private void ddlcategorycange()
    {
        try
        {
            ql.Visible = false;
            clr.Visible = false;
            dsn.Visible = false;
            shp.Visible = false;
            sz.Visible = false;
            shd.Visible = false;
            string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                          " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                          " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["PARAMETER_ID"].ToString())
                    {
                        case "1":
                            ql.Visible = true;
                            break;
                        case "2":
                            dsn.Visible = true;
                            if (chkindentvise.Checked)
                            {
                                UtilityModule.ConditionalComboFill(ref dddesign, @"select distinct designId, designName from Design d
                                inner join ITEM_PARAMETER_MASTER ipm on d.designId=ipm.design_Id inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
                                Where pid.PIndentId=" + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ", true, "--Select Design--");
                            }
                            else if (chkcustomervise.Checked)
                            {
                                if (hncunsp.Value == "1")
                                {
                                    UtilityModule.ConditionalComboFill(ref dddesign, @"select distinct designId, designName from Design d  inner join ITEM_PARAMETER_MASTER ipm on d.designId=ipm.design_Id
                                    inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ", true, "--Select Design--");
                                }
                                else
                                {
                                    string str = string.Empty;
                                    switch (Session["varcompanyid"].ToString())
                                    {
                                        case "6":
                                            str = @"SELECT distinct designId,designName   
                                                    from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                                    on vc.finishedid=vf.ITEM_FINISHED_ID
                                                    Where vc.orderid=" + ddorderno.SelectedValue + "  order by designname";
                                            break;
                                        default:
                                            if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                                            {
                                                str = "select distinct designId, designName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ";
                                            }
                                            else
                                            {
                                                str = @"select distinct designId, designName from Design d inner join ITEM_PARAMETER_MASTER ipm on d.designId=ipm.design_Id
                                                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ";
                                            }
                                            break;
                                    }

                                    UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select Design--");
                                }
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref dddesign, "select distinct designId, designName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ", true, "Select Design");
                            }
                            break;
                        case "3":
                            clr.Visible = true;
                            if (chkindentvise.Checked)
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, @"select distinct colorid, colorname from color c
                                inner join ITEM_PARAMETER_MASTER ipm on c.colorId=ipm.color_Id inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
                                where pid.PIndentId=" + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "--Select Color--");
                            }
                            else if (chkcustomervise.Checked)
                            {
                                if (hncunsp.Value == "1")
                                {
                                    UtilityModule.ConditionalComboFill(ref ddcolor, @" select distinct colorid, colorname from color c inner join ITEM_PARAMETER_MASTER ipm on c.colorid=ipm.color_Id
                                    inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "--Select Color--");
                                }
                                else
                                {
                                    string str = string.Empty;
                                    switch (Session["varcompanyid"].ToString())
                                    {
                                        case "6":
                                            str = @"SELECT distinct ColorId,ColorName
                                                    from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                                    on vc.finishedid=vf.ITEM_FINISHED_ID
                                                    Where vc.orderid=" + ddorderno.SelectedValue + "  order by colorname";
                                            break;
                                        default:
                                            if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                                            {
                                                str = "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ";
                                            }
                                            else
                                            {
                                                str = @"select distinct colorid, colorname from color c inner join ITEM_PARAMETER_MASTER ipm on c.colorid=ipm.color_Id
                                                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ";
                                            }
                                            break;
                                    }

                                    UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select Color--");
                                }
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "Select Color");
                            }
                            break;
                        case "4":
                            shp.Visible = true;
                            if (chkindentvise.Checked)
                            {
                                UtilityModule.ConditionalComboFill(ref ddshape, @"select distinct ShapeId, ShapeName from shape s inner join ITEM_PARAMETER_MASTER ipm on s.ShapeId=ipm.shape_Id
                                inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
                                Where pid.PIndentId= " + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeName ", true, "--Select Shape--");
                            }
                            else if (chkcustomervise.Checked)
                            {
                                if (hncunsp.Value == "1")
                                {
                                    UtilityModule.ConditionalComboFill(ref ddshape, @"select distinct ShapeId, ShapeName from shape s  inner join ITEM_PARAMETER_MASTER ipm on s.shapeid=ipm.shape_Id
                                    inner join OrderLocalConsumption ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                                    Where ocm.orderid= " + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeName ", true, "--Select Shape--");
                                }
                                else
                                {
                                    string str = string.Empty;
                                    switch (Session["varcompanyid"].ToString())
                                    {
                                        case "6":
                                            str = @"SELECT distinct ShapeId,ShapeName
                                                    from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                                    on vc.finishedid=vf.ITEM_FINISHED_ID
                                                    Where vc.orderid=" + ddorderno.SelectedValue + "  order by ShapeName";
                                            break;
                                        default:
                                            if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                                            {
                                                str = "select distinct ShapeId, ShapeName from shape Where MasterCompanyId=" + Session["varCompanyId"] + "  Order By ShapeName ";
                                            }
                                            else
                                            {
                                                str = @"select distinct ShapeId, ShapeName from shape s  inner join ITEM_PARAMETER_MASTER ipm on s.shapeid=ipm.shape_Id
                                                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                                    Where ocm.orderid= " + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShapeName ";
                                            }
                                            break;
                                    }
                                    UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select Shape--");
                                }
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddshape, "select distinct ShapeId, ShapeName from shape Where MasterCompanyId=" + Session["varCompanyId"] + "  Order By ShapeName ", true, "Select Shape");
                            }
                            break;
                        case "5":
                            sz.Visible = true;
                            //ChkForMtr.Checked = false;
                            break;
                        case "6":
                            shd.Visible = true;
                            if (chkindentvise.Checked)
                            {
                                UtilityModule.ConditionalComboFill(ref ddlshade, @"select distinct ShadecolorId, ShadeColorName from ShadeColor s inner join ITEM_PARAMETER_MASTER ipm on s.ShadecolorId=ipm.Shadecolor_Id
                                inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
                                Where pid.PIndentId=" + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ", true, "Select ShadeColor");
                            }
                            else if (chkcustomervise.Checked)
                            {
                                TDshdNew.Visible = true;
                                if (hncunsp.Value == "1")
                                {
                                    UtilityModule.ConditionalComboFill(ref ddlshade, @"select distinct ShadecolorId, ShadeColorName from ShadeColor s   inner join ITEM_PARAMETER_MASTER ipm on s.ShadecolorId=ipm.Shadecolor_Id
                                    inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                                    where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ", true, "Select ShadeColor");
                                }
                                else
                                {
                                    string str = string.Empty;
                                    switch (Session["varcompanyid"].ToString())
                                    {
                                        case "6":
                                            str = @"SELECT distinct ShadecolorId,ShadeColorName
                                                    from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                                    on vc.finishedid=vf.ITEM_FINISHED_ID
                                                    Where vc.orderid=" + ddorderno.SelectedValue + "  order by ShadeColorName";
                                            break;
                                        default:
                                            if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                                            {
                                                str = "select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ";
                                            }
                                            else
                                            {
                                                str = @"select distinct ShadecolorId, ShadeColorName from ShadeColor s   inner join ITEM_PARAMETER_MASTER ipm on s.ShadecolorId=ipm.Shadecolor_Id
                                                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                                    where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ";
                                            }
                                            break;
                                    }
                                    UtilityModule.ConditionalComboFill(ref ddlshadeNew, str, true, "Select ShadeColor");
                                }
                                UtilityModule.ConditionalComboFill(ref ddlshade, "select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ", true, "Select ShadeColor");
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddlshade, "select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName ", true, "Select ShadeColor");
                            }
                            break;
                        case "10":
                            clr.Visible = true;
                            if (chkindentvise.Checked)
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, @"select distinct colorid, colorname from color c
                                inner join ITEM_PARAMETER_MASTER ipm on c.colorId=ipm.color_Id inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
                                where pid.PIndentId=" + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "--Select Color--");
                            }
                            else if (chkcustomervise.Checked)
                            {
                                if (hncunsp.Value == "1")
                                {
                                    UtilityModule.ConditionalComboFill(ref ddcolor, @" select distinct colorid, colorname from color c inner join ITEM_PARAMETER_MASTER ipm on c.colorid=ipm.color_Id
                                    inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "--Select Color--");
                                }
                                else
                                {
                                    if (variable.VarCUSTOMERWISEPURCHASEWITHOUTBOM == "1")
                                    {
                                        UtilityModule.ConditionalComboFill(ref ddcolor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "Select Color");
                                    }
                                    else
                                    {
                                        UtilityModule.ConditionalComboFill(ref ddcolor, @" select distinct colorid, colorname from color c inner join ITEM_PARAMETER_MASTER ipm on c.colorid=ipm.color_Id
                                    inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                    Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "--Select Color--");
                                    }
                                }
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddcolor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "Select Color");
                            }
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Label3.Visible = false;
        Label3.Text = "";
        if ((ChkEditOrder.Checked == true && txtqty.Text != "") || (ChkEditOrder.Checked == false && txtqty.Text != ""))
        {
            CheckGSTType();
            if (Label1.Text != "")
            {
                return;
            }

            CheckTCSType();

            if (variable.VarGetPurchaseRateFromMaster == "1")
            {
                if (Label65.Text != "")
                {
                    return;
                }
            }

        }
        if (MasterCompanyId == 22)
        {
            if (string.IsNullOrEmpty(txtreqfor.Text) || string.IsNullOrEmpty(txtReqBy.Text))
            {
                lblerrormessage.Visible = true;
                lblerrormessage.Text = "Please enter value in mandatory fields!!";
              
                return;
            
            }

        }
        if (MasterCompanyId == 44)
        {
            if (ChkEditOrder.Checked == false)
            {
                if (ddpayement.SelectedIndex == 0 || dddelivery.SelectedIndex == 0)
                {
                    lblerrormessage.Visible = true;
                    lblerrormessage.Text = "Payment Term & Term of Delivery are  mandatory fields!!";

                    return;

                }
                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select IsNull(Hscode, '') hsn From Quality(Nolock) 
                Where item_id = " + dditemname.SelectedValue + " And qualityid = " + dquality.SelectedValue + " And MasterCompanyId = " + Session["varCompanyId"] + "");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["hsn"].ToString()))
                    {
                        lblerrormessage.Visible = true;
                        lblerrormessage.Text = "Please Add HSN Code  First";

                        return;

                    }
                }
            }

        }
        Save_detail();
        Fill_GridForLocalConsump();
        //Fill_Grid_Show();
        report();
        btnpriview.Visible = true;
        lblerrormessage.Text = string.Empty;
    }
    private void Save_Refresh()
    {
        TxtProdCode.Text = "";
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        ddlshadeNew.SelectedValue = null;
        txtqty.Text = "";
        TxtAmount.Text = "";
        TxtApprovedQty.Text = "";
        TxtRate.Text = "";
        TxtPreIssueQty.Text = "";
        TxtRate.Text = "";
        //ddlunit.SelectedValue = null;
        btnsave.Text = "Save";
        TxtProdCode.Focus();
        txtchalan_no.Text = "";
        //ddCompName.SelectedIndex = 1;
        txtweig.Text = "";
        TxtItemRemark.Text = "";
        if (hncompid.Value != "7")
        {

            txtcomp_date.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        }

        if (hncompid.Value != "2")
        {
            txtcomp_date.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        }
        TxtEduCess.Text = "";
        TxtCst.Text = "";
        TxtExceisDuty.Text = "";
        TxtNetAmount.Text = "";
        TxtTotalAmount.Text = "";
        txtremarks.Text = "";
        TxtLotNo.Text = "";
        if (Session["varcompanyid"].ToString() != "44")
        {
            txtDeliveryAddress.Text = "";
        }
        dddelivery.SelectedIndex = 0;
        txtSGST.Text = "";
        txtIGST.Text = "";
        txtCGST.Text = "";
        txtTCS.Text = "";
        if (ChkEditOrder.Checked == true)
        {
            txtchalan_no.Text = txtchalanno.Text;
        }
    }
    //public void Save_Image(int Pindentissuetranid)
    //{
    //    SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    myConnection.Open();
    //    if (PhotoImage.FileName != "")
    //    {
    //        string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
    //        string targetPath = Server.MapPath("/../.PurchaseImage" + Pindentissuetranid + "PindentIssueImage.gif");
    //        string img = "~\\PurchaseImage\\d" + Pindentissuetranid + " PindentIssueImage.gif";
    //        //string img = "PurchaseImage/d"+OrderDetailId+"" + filename;
    //        Stream strm = PhotoImage.PostedFile.InputStream;
    //        var targetFile = targetPath;

    //        FileInfo TheFile = new FileInfo(Server.MapPath("~\\PindentIssueImage.gif\\d") + Pindentissuetranid + "PindentIssueImage.gif");
    //        if (TheFile.Exists)
    //        {
    //            File.Delete(MapPath("~\\PurchaseImage\\d") + Pindentissuetranid + "PindentIssueImage.gif");
    //        }
    //        //if (PhotoImage.FileName != null && PhotoImage.FileName != "")
    //        //{
    //        //    GenerateThumbnails(0.3, strm, targetFile);
    //        //}
    //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssueTran Set ImagePath='" + img + "' Where Pindentissuetranid=" + Pindentissuetranid + "");
    //    }
    //    else
    //    {
    //        if (Session["ProdCodeValidation"].ToString() == "1" && TxtProdCode.Text != "")
    //        {
    //            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Top(1)PIT.ImagePath From Item_Parameter_Master IM, PurchaseIndentIssueTran PIT where Productcode='" + TxtProdCode.Text + "' And IM.Item_Finished_id=PIT.Finishedid And IM.MasterCompanyId=" + Session["varCompanyId"] + " and od.Item_Finished_Id=im.Item_Finished_id and Pindentissuetranid=" + Pindentissuetranid + "");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                string targetPath = Server.MapPath("../../PurchaseImage/d" + Pindentissuetranid + "PindentIssueImage.gif");
    //                string img = "~\\PurchaseImage\\d" + Pindentissuetranid + "PindentIssueImage.gif";

    //                // FileInfo TheFile = new FileInfo(Server.MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif");
    //                FileInfo TheFile = new FileInfo(Server.MapPath(ds.Tables[0].Rows[0]["photo"].ToString()));
    //                if (TheFile.Exists)
    //                {
    //                    //  File.Copy(MapPath("~/Item_Image/") + ItemFinishedId + "_Item.gif", MapPath("~\\PurchaseImage\\d") + OrderDetailId + "PindentIssueImage.gif");
    //                    File.Copy(MapPath(ds.Tables[0].Rows[0]["photo"].ToString()), MapPath("~\\PurchaseImage\\d") + Pindentissuetranid + "PindentIssueImage.gif");
    //                }
    //                //else
    //                //{
    //                //    Stream strm = PhotoImage.PostedFile.InputStream;
    //                //    var targetFile = targetPath;
    //                //    GenerateThumbnails(0.3, strm, targetFile);
    //                //}
    //                SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE PurchaseIndentIssueTran SET ImagePath='" + img + "' where  Pindentissuetranid=" + Pindentissuetranid + " ");
    //            }
    //        }
    //        else
    //        {
    //            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select PIT.ImagePath from PurchaseIndentIssueTran PIT,Item_Parameter_master IPM where IPM.ITEM_FINISHED_ID= PIT.FINISHEDID AND PIT.Pindentissuetranid=" + Pindentissuetranid + " And .MasterCompanyId=" + Session["varCompanyId"] + "");
    //            //SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "UPDATE DRAFT_ORDER_DETAIL SET PHOTO=MII.PHOTO FROM MAIN_ITEM_IMAGE MII WHERE DRAFT_ORDER_DETAIL.ITEM_FINISHED_ID=MII.FINISHEDID AND ORDERDETAILID=" + OrderDetailId + " And MII.MasterCompanyId=" + Session["varCompanyId"] + "");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                string targetPath = Server.MapPath("../../PurchaseImage/d" + Pindentissuetranid + "PindentIssueImage.gif");
    //                string img = "~\\PurchaseImage\\d" + Pindentissuetranid + "PindentIssueImage.gif";
    //                FileInfo TheFile = new FileInfo(Server.MapPath("~/PurchaseImage//") + ItemFinishedId + "_Item.gif");
    //                if (TheFile.Exists) 
    //                {
    //                    File.Delete(MapPath("~\\PurchaseImage\\d") + Pindentissuetranid + "PindentIssueImage.gif");
    //                    File.Copy(MapPath("~/PurchaseImage/") + ItemFinishedId + "_Item.gif", MapPath("~\\PurchaseImage\\d") + purchasedetailid + "PindentIssueImage.gif");
    //                }
    //                SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE PurchaseIndentIssueTran SET ImagePath='" + img + "' where  purchasedetailid=" + purchasedetailid + " ");
    //            }

    //        }
    //    }

    //    myConnection.Close();
    //    myConnection.Dispose();
    //}
    private void CheckGSTType()
    {
        Label1.Visible = false;
        Label1.Text = "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Pindentissuetranid,GSTType from PurchaseIndentIssueTran where PindentIssueid=" + ViewState["PIndentIssueId"] + "  order by Pindentissuetranid ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (DDGSType.SelectedValue != Ds.Tables[0].Rows[0]["GSTType"].ToString())
            {
                Label1.Visible = true;
                Label1.Text = "Please select same GST Type";
                return;
            }
        }
        FillGSTIGST();
    }
    private void Save_detail()
    {
        if (Label3.Visible == false)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            int varfinishedIDNew = varfinishedid;
            if (chkcustomervise.Checked == true && TDshdNew.Visible == true)
            {
                varfinishedIDNew = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshadeNew, "", Convert.ToInt32(Session["varCompanyId"]));
            }
            if (Convert.ToInt32(ViewState["PIndentIssueId"]) > 0)
            {
                string LOT = "";
                if (TxtLotNo.Text == "" || TxtLotNo.Visible == false)
                {
                    LOT = "Without Lot No";
                }
                else
                {
                    LOT = TxtLotNo.Text;
                }
                string str = "select Pindentissuetranid from PurchaseIndentIssueTran where PindentIssueid=" + ViewState["PIndentIssueId"] + " and Finishedid=" + varfinishedid.ToString() + " and LotNo='" + LOT + "'";
                if (chkindentvise.Checked == true)
                {
                    if (ddindentno.SelectedIndex > 0)
                    {
                        str = str + " and Pindentid=" + ddindentno.SelectedValue;
                    }
                }
                if (chkcustomervise.Checked == true)
                {
                    if (ddorderno.SelectedIndex > 0)
                    {
                        str = str + " And OrderID = " + ddorderno.SelectedValue;
                    }
                }
                DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Duplicate Enrty');", true);
                    tran.Commit();
                    return;
                }
            }
            try
            {
                if (Label1.Visible == false && Label1.Text == "")
                {
                    Label1.Text = "";
                    Label1.Visible = false;


                    //ViewState["PIndentIssueId"] = 0;
                    ViewState["PIndentIssueTranId"] = 0;
                    SqlParameter[] arr = new SqlParameter[60];
                    arr[0] = new SqlParameter("@PindentIssueid", SqlDbType.Int);
                    arr[1] = new SqlParameter("@Companyid", SqlDbType.Int);
                    arr[2] = new SqlParameter("@Partyid", SqlDbType.Int);
                    arr[3] = new SqlParameter("@Indentid", SqlDbType.Int);
                    arr[4] = new SqlParameter("@Orderid", SqlDbType.Int);
                    arr[5] = new SqlParameter("@Challanno", SqlDbType.NVarChar, 50);
                    arr[6] = new SqlParameter("@Date", SqlDbType.DateTime);
                    arr[7] = new SqlParameter("@Userid", SqlDbType.Int);
                    arr[8] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                    arr[9] = new SqlParameter("@Pindentissuetranid", SqlDbType.Int);
                    arr[10] = new SqlParameter("@Finishedid", SqlDbType.Int);
                    arr[11] = new SqlParameter("@Unitid", SqlDbType.Int);
                    arr[12] = new SqlParameter("@Quantity", SqlDbType.Float);
                    arr[13] = new SqlParameter("@DueDate", SqlDbType.DateTime);
                    arr[14] = new SqlParameter("@Destination", SqlDbType.NVarChar, 50);
                    arr[15] = new SqlParameter("@PayementTermId", SqlDbType.Int);
                    arr[16] = new SqlParameter("@Insurence", SqlDbType.NVarChar, 50);
                    arr[17] = new SqlParameter("@Freight", SqlDbType.Float);
                    arr[18] = new SqlParameter("@FeightRate", SqlDbType.Float, 50);
                    arr[19] = new SqlParameter("@TranportModeId", SqlDbType.Int);
                    arr[20] = new SqlParameter("@DeliveryTermid", SqlDbType.Int);
                    arr[21] = new SqlParameter("@Formno", SqlDbType.NVarChar, 50);
                    arr[22] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
                    arr[23] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 4000);
                    arr[24] = new SqlParameter("@Amount", SqlDbType.Float);
                    arr[25] = new SqlParameter("@Rate", SqlDbType.Float);
                    arr[26] = new SqlParameter("@ExciseDuty", SqlDbType.Float);
                    arr[27] = new SqlParameter("@EduCess", SqlDbType.Float);
                    arr[28] = new SqlParameter("@CST", SqlDbType.Float);
                    arr[29] = new SqlParameter("@NetAmount", SqlDbType.Float);
                    arr[30] = new SqlParameter("@AgentName", SqlDbType.NVarChar, 200);
                    arr[31] = new SqlParameter("@PackingCharge", SqlDbType.Int);
                    arr[32] = new SqlParameter("@Finish_Type", SqlDbType.Int);
                    arr[33] = new SqlParameter("@weight", SqlDbType.Float);
                    arr[34] = new SqlParameter("@delivery_date", SqlDbType.DateTime);
                    arr[35] = new SqlParameter("@flagsize", SqlDbType.Int);
                    arr[36] = new SqlParameter("@ItemRemark", SqlDbType.VarChar, 4000);

                    arr[37] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                    arr[38] = new SqlParameter("@SupplierRef", SqlDbType.VarChar, 250);
                    arr[39] = new SqlParameter("@SupplierRefDate", SqlDbType.SmallDateTime);
                    arr[40] = new SqlParameter("@VendorRef", SqlDbType.VarChar, 250);
                    arr[41] = new SqlParameter("@VendorRefDate", SqlDbType.SmallDateTime);
                    arr[42] = new SqlParameter("@TypeOfForm", SqlDbType.VarChar, 100);
                    arr[43] = new SqlParameter("@Mill", SqlDbType.VarChar, 500);
                    arr[44] = new SqlParameter("@DeliveryAddress", SqlDbType.VarChar, 500);
                    arr[45] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
                    //arr[46] = new SqlParameter("@imgPath", SqlDbType.VarChar);
                    arr[46] = new SqlParameter("@PSGST", SqlDbType.Float);
                    arr[47] = new SqlParameter("@PIGST", SqlDbType.Float);
                    arr[48] = new SqlParameter("@ManualChallanNo", SqlDbType.VarChar, 50);
                    arr[49] = new SqlParameter("@Pindentid", SqlDbType.Int);
                    arr[50] = new SqlParameter("@GSTType", SqlDbType.Int);
                    arr[51] = new SqlParameter("@FinishedIDNew", SqlDbType.Int);
                    arr[52] = new SqlParameter("@OrderNo", SqlDbType.VarChar, 100);
                    arr[53] = new SqlParameter("@TCSType", SqlDbType.Int);
                    arr[54] = new SqlParameter("@TCS", SqlDbType.Float);
                    arr[55] = new SqlParameter("@BranchID", SqlDbType.Int);
                    arr[56] = new SqlParameter("@CustomerID", SqlDbType.Int);
                    arr[57] = new SqlParameter("@ReqBy", SqlDbType.VarChar, 100);
                    arr[58] = new SqlParameter("@ReqFor", SqlDbType.VarChar, 100);
                    arr[59] = new SqlParameter("@EXTRAQUANTITY", SqlDbType.Float);

                    arr[0].Direction = ParameterDirection.InputOutput;
                    arr[0].Value = ViewState["PIndentIssueId"];
                    arr[1].Value = ddCompName.SelectedValue;
                    arr[2].Value = ddempname.SelectedValue;
                    if (ddindentno.Visible == true)
                    {
                        arr[3].Value = ddindentno.SelectedValue;
                    }
                    else
                    {
                        arr[3].Value = 0;
                    }
                    if (ddorderno.Visible == true)
                    {
                        arr[4].Value = ddorderno.SelectedValue;
                    }
                    else
                    {
                        arr[4].Value = 0;
                    }
                    arr[5].Direction = ParameterDirection.InputOutput;
                    if (ChkWithoutOrder.Checked == true)
                    {
                        arr[5].Value = 0;
                    }
                    else
                    {
                        arr[5].Value = txtchalanno.Text;
                    }
                    string LOT = "";
                    if (TxtLotNo.Text == "" || TxtLotNo.Visible == false)
                    {
                        LOT = "Without Lot No";
                    }
                    else
                    {
                        LOT = TxtLotNo.Text.ToUpper();
                    }
                    arr[6].Value = txtdate.Text;
                    arr[7].Value = Session["varuserid"].ToString();
                    arr[8].Value = Session["varCompanyId"].ToString();
                    arr[9].Direction = ParameterDirection.InputOutput;
                    arr[9].Value = ViewState["PIndentIssueTranId"];
                    arr[10].Value = varfinishedid;
                    arr[11].Value = ddlunit.SelectedValue;
                    arr[12].Value = Convert.ToDouble(txtqty.Text != "" ? txtqty.Text : "0");
                    arr[13].Value = txtduedate.Text;
                    arr[14].Value = txtdestination.Text.ToUpper();
                    arr[15].Value = ddpayement.SelectedIndex > 0 ? ddpayement.SelectedValue : "0";
                    arr[16].Value = txtinsurence.Text;
                    arr[17].Value = txtfrieght.Text == "" ? "0" : txtfrieght.Text;
                    arr[18].Value = Convert.ToDouble(txtfrieghtrate.Text != "" ? txtfrieghtrate.Text : "0.00");
                    arr[19].Value = ddtransprt.SelectedIndex > 0 ? ddtransprt.SelectedValue : "0";
                    arr[20].Value = dddelivery.SelectedIndex > 0 ? dddelivery.SelectedValue : "0";
                    arr[21].Value = txtform.Text.ToUpper();
                    //arr[22].Value = TxtLotNo.Text != "" ? TxtLotNo.Text.ToUpper() : "Without Lot No";
                    arr[22].Value = LOT;
                    arr[23].Value = txtremarks.Text;
                    arr[24].Value = Convert.ToDouble(TxtAmount.Text != "" ? TxtAmount.Text : "0");
                    arr[25].Value = Convert.ToDouble(TxtRate.Text != "" ? TxtRate.Text : "0");
                    arr[26].Value = Convert.ToDouble(TxtExceisDuty.Text != "" ? TxtExceisDuty.Text : "0");
                    arr[27].Value = Convert.ToDouble(TxtEduCess.Text != "" ? TxtEduCess.Text : "0");
                    arr[28].Value = Convert.ToDouble(TxtCst.Text != "" ? TxtCst.Text : "0");
                    arr[29].Value = Convert.ToDouble(TxtNetAmount.Text != "" ? TxtNetAmount.Text : "0");
                    arr[30].Value = TxtAgentName.Text.ToUpper();
                    arr[31].Value = DDPackingCharges.SelectedValue;
                    arr[32].Value = TdFinish_Type.Visible == true ? ddFinish_Type.SelectedValue : "0";
                    arr[33].Value = Convert.ToDouble(txtweig.Text != "" ? txtweig.Text : "0");
                    arr[34].Value = Convert.ToDateTime(txtcomp_date.Text);
                    arr[35].Value = DDsizetype.SelectedValue;
                    arr[36].Value = TxtItemRemark.Text;
                    arr[37].Value = DDCurrency.SelectedIndex <= 0 ? "0" : DDCurrency.SelectedValue;
                    arr[38].Value = txtSupplierRef.Text;
                    arr[39].Value = txtSupplierRefDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : txtSupplierRefDate.Text;
                    arr[40].Value = txtvendorRef.Text;
                    arr[41].Value = txtVendorRefDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : txtVendorRefDate.Text;
                    arr[42].Value = txtTypeofForm.Text;
                    arr[43].Value = txtMill.Text;
                    if (Session["varcompanyid"].ToString() == "44")
                    {
                        arr[44].Value = ddlDeliveryAddress.SelectedItem.Text;
                    }
                    else
                    {
                        arr[44].Value = txtDeliveryAddress.Text;
                    
                    }
                    arr[45].Direction = ParameterDirection.Output;
                    //arr[46].Value = hfImgURL.Value;
                    double GST = Convert.ToDouble(txtCGST.Text == "" ? "0" : txtCGST.Text) + Convert.ToDouble(txtSGST.Text == "" ? "0" : txtSGST.Text);
                    arr[46].Value = GST;
                    arr[47].Value = Convert.ToDouble(txtIGST.Text != "" ? txtIGST.Text : "0");
                    arr[48].Value = txtManualChallanNo.Text;
                    arr[49].Value = chkindentvise.Checked == true ? (ddindentno.SelectedValue) : "0";
                    arr[50].Value = DDGSType.SelectedValue;
                    arr[51].Value = varfinishedIDNew;
                    arr[52].Value = TxtOrderNo.Text;
                    arr[53].Value = DDTCSType.SelectedValue;
                    arr[54].Value = Convert.ToDouble(txtTCS.Text != "" ? txtTCS.Text : "0");
                    arr[55].Value = DDBranchName.SelectedValue;
                    arr[56].Value = 0;
                    arr[57].Value = txtReqBy.Text;
                    arr[58].Value = txtreqfor.Text;
                    arr[59].Value = Convert.ToDouble(txtextraqty.Text != "" ? txtextraqty.Text : "0");
                    if (tdcustomer.Visible == true && ddcustomercode.SelectedIndex > 0)
                    {
                        arr[56].Value = ddcustomercode.SelectedValue;
                    }
                    if (txtqty.Text != "")
                    {
                        if (hncompid.Value == "2")
                        {
                            if (ddCatagory.SelectedIndex > 0 && dditemname.SelectedIndex > 0 && TxtProdCode.Text != "" && ddempname.SelectedIndex > 0)
                            {
                                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_PurchaseIndentIssue]", arr);
                                ViewState["PIndentIssueId"] = arr[0].Value;
                                tran.Commit();
                                msg = arr[45].Value.ToString();
                                MessageSave(msg);
                            }

                        }
                        else
                        {
                            if (ddCatagory.SelectedIndex > 0 && dditemname.SelectedIndex > 0 && ddempname.SelectedIndex > 0)
                            {
                                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_PurchaseIndentIssuenew]", arr);
                                ViewState["PIndentIssueId"] = arr[0].Value;
                                ViewState["PIndentIssueTranId"] = arr[9].Value;
                                //string A = arr[35].Value;
                                tran.Commit();

                                msg = arr[45].Value.ToString();
                                MessageSave(msg);
                            }
                        }
                    }
                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select MII.PHOTO from MAIN_ITEM_IMAGE mii,purchaseindentissuetran PIT where 
                        PIT.FINISHEDID=MII.FINISHEDID AND PIT.Pindentissuetranid=" + ViewState["PIndentIssueTranId"] + @" 
                        And MII.MasterCompanyId=" + Session["varCompanyId"] + "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        string targetPath = Server.MapPath("../../PurchaseImage/" + ViewState["PIndentIssueTranId"] + "_PindentIssueImage.gif");
                        string img = "~/PurchaseImage/" + ViewState["PIndentIssueTranId"] + "_PindentIssueImage.gif";
                        FileInfo TheFile = new FileInfo(Server.MapPath("~/Item_Image/") + varfinishedid + "_Item.gif");
                        if (TheFile.Exists)
                        {
                            File.Delete(MapPath("~\\PurchaseImage\\") + ViewState["PIndentIssueTranId"] + "_PindentIssueImage.gif");
                            File.Copy(MapPath("~/Item_Image/") + varfinishedid + "_Item.gif", MapPath("~\\PurchaseImage\\") + ViewState["PIndentIssueTranId"] + "_PindentIssueImage.gif");
                        }
                        SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "UPDATE purchaseindentissuetran SET ImagePath='" + img + "' where  Pindentissuetranid=" + ViewState["PIndentIssueTranId"] + " ");
                    }

                    if (ChkEditOrder.Checked == true)
                    {
                        grid_data();
                        pnl1.Enabled = true;
                    }
                    else
                    {
                        pnl1.Enabled = false;
                    }
                    if (ChkEditOrder.Checked == false)
                    {
                        if (hncompid.Value == "2")
                        {
                            if (ddempname.SelectedIndex == 0 || ddCatagory.SelectedIndex == 0 || dditemname.SelectedIndex == 0 || TxtProdCode.Text == "")
                            {
                                Label2.Text = "1 or More than One Mendatray Fields are not filled";
                                Label2.Visible = true;
                                pnl1.Enabled = true;
                            }
                            else
                            {
                                Label2.Visible = false;
                            }
                        }
                        else
                        {
                            if (ddempname.SelectedIndex == 0 || ddCatagory.SelectedIndex == 0 || dditemname.SelectedIndex == 0)
                            {
                                Label2.Text = "1 or More than One Mendatray Fields are not filled";
                                Label2.Visible = true;
                                pnl1.Enabled = true;
                            }
                            else
                            {
                                Label2.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (ddorderno.SelectedIndex == 0)
                        {
                            Label2.Text = "1 or More than One Manadatory Fields are not filled";
                            Label2.Visible = true;
                        }
                        else
                        {
                            Label2.Visible = false;
                        }
                    }
                    txtchalanno.Text = arr[5].Value.ToString();
                    Save_Refresh();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
                Label1.Text = ex.Message;
                Label1.Visible = true;
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            Fill_Grid();
        }
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = @"select *  from PurchaseIndentIssue pis  inner join PurchaseIndentIssuetran psit on pis.pindentissueid=psit.pindentissueid
                where pis.MasterCompanyId=" + Session["varCompanyId"] + " And  psit.finishedid=" + Varfinishedid + "and pis.challanno=" + txtchalanno.Text + "and pindentissuetranid !=" + Convert.ToInt32(txtpindentissuedetailid.Text);
            }
            else
            {
                strsql = @"select *  from PurchaseIndentIssue pis  inner join PurchaseIndentIssuetran psit on pis.pindentissueid=psit.pindentissueid
                where psit.finishedid=" + Varfinishedid + "and pis.challanno=" + txtchalanno.Text + " And pis.MasterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Lblfinished.Visible = true;
            }
            else
            {
                Lblfinished.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
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
    private void Fill_Grid()
    {
        DataSet ds = new DataSet();
        ds = fill_Data_grid();
       
        if (ds.Tables[0].Rows.Count > 0)
        {
            gddetail.DataSource = ds;
            gddetail.DataBind();
            if (gddetail.Rows.Count > 0)
                gddetail.Visible = true;
            else
                gddetail.Visible = false;
            if (hncompid.Value == "2")
            {
                visible();
            }
            if (dddelivery.Items.FindByValue(ds.Tables[0].Rows[0]["deliverytermid"].ToString()) != null)
            {
                dddelivery.SelectedValue = ds.Tables[0].Rows[0]["deliverytermid"].ToString();
            }
        }

    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        string strsql;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string view2 = "";
            if (Session["varcompanyno"].ToString() == "20")
            {
                view2 = "ViewFindFinishedidItemidQDCSSNew";
            }
            else
            {
                view2 = "ViewFindFinishedidItemidQDCSS";
            }

            if (ChkEditOrder.Checked == true)
            {
                strsql = @"SELECT distinct Lotno,Rate,Amount,pist.Pindentissuetranid, icm.CATEGORY_NAME, im.ITEM_NAME, pist.quantity,isnull(pist.weight,0) weight,
                         IPM1.QDCS + Space(2)+ case when flagsize=0 then case when Sizeft='' then '' else cast(sizeft as varchar) +' Ft' end  
                         when flagsize=1 then case when sizemtr='' then '' else cast(sizemtr as varchar)+' Mtr' end else case when sizeinch='' then sizeinch 
                         else cast(sizeinch as varchar)+' Inch' end end DESCRIPTION,CASE WHEN pist.delivery_date IS NULL THEN replace(convert(varchar(11),GETDATE(),106), ' ','-') 
                         ELSE replace(convert(varchar(11),pist.delivery_date,106), ' ','-') END as ddate ,CASE WHEN Pii.duedate IS NULL THEN 
                         replace(convert(varchar(11),GETDATE(),106), ' ','-') ELSE replace(convert(varchar(11),pii.duedate,106), ' ','-') END as duedate,
                          pist.vat,pist.Cst,pist.SGST/2 as SGST,pist.SGST/2 as CGST,pist.IGST, pist.NetAmount,isnull(pist.canqty,0) as cancel,pist.remark as itemremark, pist.Finishedid,
                           isnull(pist.TCS,0) as TCS,pii.deliverytermid,pist.extraqty
                         FROM ITEM_MASTER im INNER JOIN ITEM_CATEGORY_MASTER icm ON im.CATEGORY_ID = icm.CATEGORY_ID INNER JOIN ITEM_PARAMETER_MASTER IPM ON im.ITEM_ID = IPM.ITEM_ID 
                         INNER JOIN PurchaseIndentIssueTran pist ON IPM.ITEM_FINISHED_ID = pist.Finishedid inner join " + view2 + @" IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid 
                         inner join PurchaseIndentIssue pii on pii.pindentissueid=pist.pindentissueid
                         Where pii.pindentissueid=" + ViewState["PIndentIssueId"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                if (chkindentvise.Checked == true)
                {
                    strsql = strsql + " and Pist.Pindentid=" + ddindentno.SelectedValue;
                }
            }
            else
            {
                strsql = @"SELECT distinct Lotno,Rate,Amount,pist.Pindentissuetranid, icm.CATEGORY_NAME, im.ITEM_NAME, pist.quantity,isnull(pist.weight,0) weight,IPM1.QDCS + Space(2)+  
                         case when flagsize=0 then case when Sizeft='' then '' else cast(sizeft as varchar) +' Ft' end  when flagsize=1 then case when sizemtr='' then '' 
                        else cast(sizemtr as varchar)+' Mtr' end else case when sizeinch='' then sizeinch else cast(sizeinch as varchar)+' Inch' end end  DESCRIPTION,
                        replace(convert(varchar(11),pist.delivery_date,106), ' ','-') as ddate, replace(convert(varchar(11),pii.duedate,106), ' ','-') as duedate,
                        pist.vat,pist.Cst,pist.SGST/2 as SGST,pist.SGST/2 as CGST,pist.IGST,pist.NetAmount,pist.canqty as cancel,pist.remark as itemremark, pist.Finishedid
                        ,  isnull(pist.TCS,0) as TCS,pii.deliverytermid,pist.extraqty
                        FROM ITEM_MASTER im INNER JOIN ITEM_CATEGORY_MASTER icm  ON im.CATEGORY_ID = icm.CATEGORY_ID 
                        INNER JOIN ITEM_PARAMETER_MASTER IPM ON im.ITEM_ID = IPM.ITEM_ID INNER JOIN PurchaseIndentIssueTran pist ON IPM.ITEM_FINISHED_ID = pist.Finishedid
                        Inner Join  " + view2 + @" IPM1 on IPM.Item_Finished_Id=IPM1.Finishedid inner join PurchaseIndentIssue pii on 
                        pii.pindentissueid=pist.pindentissueid where pist.pindentissueid=" + ViewState["PIndentIssueId"] + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
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
    protected void gddetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gddetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gddetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"SELECT distinct  PurchaseIndentIssue.Companyid, PurchaseIndentIssue.Partyid, PurchaseIndentIssue.Challanno, PurchaseIndentIssue.Date, 
            ITEM_CATEGORY_MASTER.CATEGORY_ID, ITEM_PARAMETER_MASTER.QUALITY_ID, ITEM_PARAMETER_MASTER.DESIGN_ID, 
            ITEM_PARAMETER_MASTER.COLOR_ID, ITEM_PARAMETER_MASTER.SHAPE_ID, ITEM_PARAMETER_MASTER.SIZE_ID, 
            ITEM_PARAMETER_MASTER.ITEM_ID, ITEM_PARAMETER_MASTER.SHADECOLOR_ID, PurchaseIndentIssueTran.quantity, 
            PurchaseIndentIssueTran.Unitid, PurchaseIndentIssueTran.PindentIssueid, PurchaseIndentIssueTran.Pindentissuetranid, 
            PurchaseIndentIssue.DueDate, PurchaseIndentIssue.Destination, PurchaseIndentIssue.PayementTermId, 
            PurchaseIndentIssue.Insurence, PurchaseIndentIssue.Freight, PurchaseIndentIssue.FeightRate, 
            PurchaseIndentIssue.TranportModeId, PurchaseIndentIssue.DeliveryTermid, PurchaseIndentIssue.Formno, 
            PurchaseIndentIssue.Remarks,,case when PurchaseIndentIssue.MasterCompanyId=20 then isnull(PurchaseIndentIssue.ManualChallanNo,'') else '' end as ManualChallanNo
            FROM  PurchaseIndentIssueTran INNER JOIN
            PurchaseIndentIssue ON PurchaseIndentIssueTran.PindentIssueid = PurchaseIndentIssue.PindentIssueid INNER JOIN
            ITEM_CATEGORY_MASTER INNER JOIN  ITEM_MASTER ON ITEM_CATEGORY_MASTER.CATEGORY_ID = ITEM_MASTER.CATEGORY_ID INNER JOIN
            ITEM_PARAMETER_MASTER ON ITEM_MASTER.ITEM_ID = ITEM_PARAMETER_MASTER.ITEM_ID ON 
            PurchaseIndentIssueTran.Finishedid = ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID 
            Where PurchaseIndentIssueTran.Pindentissuetranid=" + gddetail.SelectedValue + " And PurchaseIndentIssue.Companyid = " + ddCompName.SelectedValue + " And ITEM_MASTER.MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            txtpindentissueid.Text = ds.Tables[0].Rows[0]["pindentissueid"].ToString();
            txtpindentissuedetailid.Text = ds.Tables[0].Rows[0]["Pindentissuetranid"].ToString();
            ddempname.SelectedValue = ds.Tables[0].Rows[0]["partyid"].ToString();
            txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"].ToString()).ToString("dd-MMM-yyyy");
            string Qry = @"select distinct category_id,category_name from ITEM_CATEGORY_MASTER icm inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @") And icm.MasterCompanyId=" + Session["varCompanyId"] + @" Order By category_name 
            select item_id,item_name  from ITEM_MASTER where category_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By item_name 
            select distinct qualityid, qualityname from quality where item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By qualityname
            SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["item_id"].ToString()) + " And i.MasterCompanyId=" + Session["varCompanyId"] + @" Order By u.UnitName            
            select distinct designId, designName from Design Where MasterCompanyId=" + Session["varCompanyId"] + @"  Order By designName
            select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By colorname
            select distinct ShapeId, ShapeName from shape Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By ShapeName
            select distinct SizeId, SizeFt from Size Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By SizeFt
            select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + "  Order By ShadeColorName";
            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "Select Category");
            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref dditemname, DSQ, 1, true, "Select Item");
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Item");
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 3, true, "Select Unit");
            dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
            ddlunit.SelectedValue = ds.Tables[0].Rows[0]["Unitid"].ToString();
            UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 4, true, "Slect Design");
            UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 5, true, "Select Color");
            UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 6, true, "Select Shape");
            UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 7, true, "Select Size");
            UtilityModule.ConditionalComboFillWithDS(ref ddlshade, DSQ, 9, true, "Select ShadeColor");
            dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
            ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
            ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
            ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
            ddlshade.SelectedValue = ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
            Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                  Select PaymentId,PaymentName from Payment Where MasterCompanyId=" + Session["varCompanyId"] + @" order by PaymentName
                  select TermId,TermName from Term Where MasterCompanyId=" + Session["varCompanyId"] + @" order by TermName
                  select TransModeid,TransModeName from Transmode Where MasterCOmpanyId=" + Session["varCompanyId"] + " order by TransModename";
            DSQ = SqlHelper.ExecuteDataset(Qry);

            //UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "Select Comp Name");

            UtilityModule.ConditionalComboFillWithDS(ref ddpayement, DSQ, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddelivery, DSQ, 2, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddtransprt, DSQ, 3, true, "Select Mode");
            txtduedate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["duedate"].ToString()).ToString("dd-MMM-yyyy");
            txtdestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
            ddpayement.SelectedValue = ds.Tables[0].Rows[0]["PayementTermId"].ToString();
            txtinsurence.Text = ds.Tables[0].Rows[0]["Insurence"].ToString();
            txtfrieght.Text = ds.Tables[0].Rows[0]["Freight"].ToString();
            txtfrieghtrate.Text = ds.Tables[0].Rows[0]["FeightRate"].ToString();
            ddtransprt.SelectedValue = ds.Tables[0].Rows[0]["TranportModeId"].ToString();
            dddelivery.SelectedValue = ds.Tables[0].Rows[0]["DeliveryTermid"].ToString();
            txtform.Text = ds.Tables[0].Rows[0]["Formno"].ToString();
            txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            txtchalanno.Text = ds.Tables[0].Rows[0]["Challanno"].ToString();
            txtqty.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
            txtManualChallanNo.Text = ds.Tables[0].Rows[0]["ManualChallanNo"].ToString();
            int q = (dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0);
            if (q > 0)
            {
                ql.Visible = true;
            }
            else
            {
                ql.Visible = false;
            }
            int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0);
            if (d > 0)
            {
                dsn.Visible = true;
            }
            else
            {
                dsn.Visible = false;
            }
            int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
            if (c > 0)
            {
                clr.Visible = true;
            }
            else
            {
                clr.Visible = false;
            }
            int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
            if (s > 0)
            {
                shp.Visible = true;
            }
            else
            {
                shp.Visible = false;
            }
            int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
            if (si > 0)
            {
                sz.Visible = true;
            }
            else
            {
                sz.Visible = false;
            }
            int sd = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
            if (sd > 0)
            {
                shd.Visible = true;
            }
            else
            {
                shd.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
        pnl1.Enabled = false;
    }
    private void refresh_form()
    {
        txtpindentissueid.Text = "0";
        txtpindentissuedetailid.Text = "0";
        if (Convert.ToInt32(Session["varcompanyno"]) != 7)
        {
            ddcustomercode.SelectedValue = null;
            ddorderno.SelectedValue = null;
            txtduedate.Text = "";
            Btnorder.Visible = false;
        }
        ddempname.SelectedValue = null;
        ddindentno.SelectedValue = null;
        txtchalanno.Text = "";
        ddCatagory.SelectedValue = null;
        dditemname.SelectedValue = null;
        dquality.SelectedValue = null;
        dddesign.SelectedValue = null;
        ddcolor.SelectedValue = null;
        ddshape.SelectedValue = null;
        ddsize.SelectedValue = null;
        ddlshade.SelectedValue = null;
        txtqty.Text = "";
        ddlunit.SelectedValue = null;
        txtdestination.Text = "";
        ddpayement.SelectedValue = null;
        txtfrieght.Text = "";
        txtfrieghtrate.Text = "";
        txtinsurence.Text = "";
        ddtransprt.SelectedValue = null;
        dddelivery.SelectedValue = null;
        txtform.Text = "";
        txtremarks.Text = "";
        pnl1.Enabled = true;
        ddlchalanno.SelectedValue = null;
        txtchalan_no.Text = "";
        Lblfinished.Visible = false;
        Session.Remove("orderid");
        Session.Remove("indentid");
        ViewState["PIndentIssueId"] = 0;
        ViewState["PIndentIssueTranId"] = 0;
        TxtTotalAmount.Text = "";
        TxtCst.Text = "";
        TxtExceisDuty.Text = "";
        TxtNetAmount.Text = "";
        txtSGST.Text = "";
        txtIGST.Text = "";
        Fill_Grid();
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        refresh_form();
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
        if (variable.VarGetPurchaseRateFromMaster == "1")
        {
            FillPurchaseRate();
        }

        FillGSTIGST();
        fill_text();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
        if (Session["varcompanyNo"].ToString() == "20")
        {
            FillRate();
            fill_text();
        }

    }
    private void FillRate()
    {
        if (Session["varcompanyNo"].ToString() == "20")
        {
            double rate = 0;
            rate = UtilityModule.getshaderate(Convert.ToInt32(dquality.SelectedValue), Convert.ToInt32(ddlshade.SelectedValue));
            TxtRate.Text = Convert.ToString(rate);
        }
    }
    private void Fill_Quantity()
    {
        if (chkindentvise.Checked == true)
        {
            int color = 0;
            int quality = 0;
            int design = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
            {
                quality = 1;
            }
            if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
            {
                design = 1;
            }
            if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
            {
                color = 1;
            }
            if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
            {
                shape = 1;
            }
            if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
            {
                size = 1;
            }
            if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                con.Open();
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"])));
                    if (finishedid > 0)
                    {

                        string Qry = @"select isnull(sum(Qty),0) Qty,isnull(sum(rate),0) as rate From PurchaseIndentMaster PIM inner join PurchaseIndentDetail PID on PIM.PIndentId=PID.PIndentId inner join PurchaseIndentApproval PIA on PIA.PIndentNo=PIM.PIndentNo where PIM.PIndentid=" + ddindentno.SelectedValue + " and FinishedId=" + finishedid + " And PIM.MasterCompanyId=" + Session["varCompanyId"] + @"
                                       select isnull(sum(Quantity),0) Qty From PurchaseIndentIssue PII inner join PurchaseIndentIssueTran PIT on PIT.PIndentIssueId=PII.PIndentIssueId where IndentId=" + ddindentno.SelectedValue + " and FinishedId=" + finishedid + " And PII.MasterCompanyId=" + Session["varCompanyId"] + @"
                                       select isnull(UnitId,0),ImageName from PurchaseIndentDetail where PIndentId=" + ddindentno.SelectedValue + " and FinishedId=" + finishedid;
                        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                        TxtApprovedQty.Text = DSQ.Tables[0].Rows[0][0].ToString();
                        TxtRate.Text = DSQ.Tables[0].Rows[0][1].ToString();
                        TxtPreIssueQty.Text = DSQ.Tables[1].Rows[0][0].ToString();
                        ddlunit.SelectedValue = DSQ.Tables[2].Rows[0][0].ToString();
                        trimage.Visible = true;
                        lblimage.ImageUrl = DSQ.Tables[2].Rows[0][1].ToString();
                        Tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                TxtApprovedQty.Text = "";
                TxtPreIssueQty.Text = "";
                TxtRate.Text = "";
            }
        }
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
        Fill_Quantity();
    }
    private void FillShapeSelectedChange()
    {
        string Str = "";
        string strsql = "";
        if (DDsizetype.SelectedValue == "0")
        {
            Str = "Sizeft";
        }
        else if (DDsizetype.SelectedValue == "1")
        {
            Str = "SizeMTR";
        }
        else
            Str = "SizeInch";
        if (chkindentvise.Checked)
        {
            strsql = @"select distinct SizeId, " + Str + @" from Size s inner join ITEM_PARAMETER_MASTER ipm on s.SizeId=ipm.size_Id
            inner join PurchaseIndentDetail pid on pid.finishedid=ipm.ITEM_FINISHED_ID
            where pid.PIndentId=" + ddindentno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"];
            if (ddshape.SelectedIndex > 0)
            {
                strsql = strsql + " And s.shapeId=" + ddshape.SelectedValue;
            }
            strsql = strsql + " Order by " + Str;

            UtilityModule.ConditionalComboFill(ref ddsize, strsql, true, "--Select Size--");
        }
        else if (chkcustomervise.Checked)
        {
            if (hncunsp.Value == "1")
            {
                strsql = @"select distinct SizeId, " + Str + @" from Size s inner join ITEM_PARAMETER_MASTER ipm on s.SizeId=ipm.size_Id
                inner join OrderLocalConsumption  ocm on ocm.finishedid=ipm.ITEM_FINISHED_ID
                Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"];
                if (ddshape.SelectedIndex > 0)
                {
                    strsql = strsql + " And s.shapeId=" + ddshape.SelectedValue;
                }
                strsql = strsql + " Order by " + Str;

                UtilityModule.ConditionalComboFill(ref ddsize, strsql, true, "--Select Size--");
            }
            else
            {
                string str = string.Empty;
                switch (Session["varcompanyid"].ToString())
                {
                    case "6":
                        str = @"SELECT distinct SizeId," + Str + @" 
                                from V_ConsumptionQtyAndPurchaseQtyForArtIndia vc inner join V_FinishedItemDetail vf
                                on vc.finishedid=vf.ITEM_FINISHED_ID
                                Where vc.orderid=" + ddorderno.SelectedValue;
                        if (ddshape.SelectedIndex > 0)
                        {
                            str = str + " And shapeId=" + ddshape.SelectedValue;
                        }
                        str = str + " Order by " + Str;
                        break;
                    default:
                        str = @"select distinct SizeId, " + Str + @" from Size s inner join ITEM_PARAMETER_MASTER ipm on s.SizeId=ipm.size_Id
                                 inner join ORDER_CONSUMPTION_DETAIL ocm on ocm.ifinishedid=ipm.ITEM_FINISHED_ID
                                 Where ocm.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"];
                        if (ddshape.SelectedIndex > 0)
                        {
                            str = str + " And s.shapeId=" + ddshape.SelectedValue;
                        }
                        str = str + " Order by " + Str;
                        break;
                }

                UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select Size--");
            }
        }
        else
        {
            strsql = "select distinct SizeId, " + Str + @" from Size Where MasterCompanyId=" + Session["varCompanyId"];
            if (ddshape.SelectedIndex > 0)
            {
                strsql = strsql + " And shapeId=" + ddshape.SelectedValue;
            }
            strsql = strsql + " Order by " + Str;

            UtilityModule.ConditionalComboFill(ref ddsize, strsql, true, "--Select Size--");
        }
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Quantity();
    }
    protected void txtqty_TextChanged(object sender, EventArgs e)
    {
        fill_text();
        //if (Session["varcompanyNo"].ToString() == "12")
        //{
        //  int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //  TxtRate.Text = UtilityModule.getItemRate(Convert.ToInt16(ddempname.SelectedValue), varfinishedid, "PURCHASE").ToString();
        //}
        TxtRate.Focus();
    }
    protected void txtextraqty_TextChanged(object sender, EventArgs e)
    {
        fill_text();
        //if (Session["varcompanyNo"].ToString() == "12")
        //{
        //  int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //  TxtRate.Text = UtilityModule.getItemRate(Convert.ToInt16(ddempname.SelectedValue), varfinishedid, "PURCHASE").ToString();
        //}
        TxtRate.Focus();
    }
    private void fill_text()
    {
       double TotalAmt = 0.00;
        //double NetAmt = 0.00;
        double cst = 0.00;
        double IGST = 0.00;
        double TCS = 0.00;
        if (hncompid.Value == "7")
        {
            if (Convert.ToDouble(txtqty.Text) > Convert.ToDouble(hnqty.Value))
            {
                lblqty.Text = "Qty Is Greater Then Required Qty";
                lblqty.Visible = true;
            }
            else
            {
                lblqty.Text = "";
                lblqty.Visible = false;
            }
        }
        if (hncompid.Value == "6" && chkindentvise.Checked == true)
        {
            lblqty.Text = "";
            lblqty.Visible = false;
            if (Convert.ToDouble(txtqty.Text) > (Convert.ToDouble(TxtApprovedQty.Text == "" ? "0" : TxtApprovedQty.Text) - Convert.ToDouble(TxtPreIssueQty.Text)))
            {

                lblqty.Text = "Remaining qty for Order only " + (Convert.ToDouble(TxtApprovedQty.Text) - Convert.ToDouble(TxtPreIssueQty.Text)) + "";
                txtqty.Text = "0";
                lblqty.Visible = true;
                return;
            }
        }
        double orderQty = 0,Qty = 0,extraqty=0,finalqty=0;
        
        if (txtqty.Text != "")
        {
            DataSet ds = new DataSet();
           if (Session["varCompanyId"].ToString() == "44")
            {
                extraqty = Convert.ToDouble(string.IsNullOrEmpty(txtextraqty.Text) ? "0" : txtextraqty.Text);
                //if (ddCatagory.SelectedValue == "10")
                //{
                //    tdextra.Visible = true;
                //}
                //else
                //{
                //    tdextra.Visible = false;
                
                //}
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select varcompanyNo,PercentageExecssQtyForProduction From Mastersetting ");
                int varcompanyno = Convert.ToInt32(ds.Tables[0].Rows[0]["varcompanyNo"]);
                Double Percentage = Convert.ToDouble(ds.Tables[0].Rows[0]["PercentageExecssQtyForProduction"]);

                orderQty = Math.Round((Convert.ToDouble(txtqty.Text ) * Percentage / 100), 3);
               
               // Qty = orderQty;
                finalqty = Convert.ToDouble(txtqty.Text) + extraqty ;
            //    if (Convert.ToDouble(finalqty) > (Convert.ToDouble(hnqty.Value)+orderQty))
            //{
            //    lblqty.Text = "Qty Is Greater Then Required Qty";
            //    lblqty.Visible = true;
            //    return;
            //}
            }
            else
            {
                Qty = Convert.ToDouble(txtqty.Text);
            }
            double Rate = Convert.ToDouble(string.IsNullOrEmpty(TxtRate.Text) ? "0" : TxtRate.Text);

            TxtAmount.Text = (Rate * (finalqty)).ToString();
        }
        if (TxtAmount.Text != "")
        {
            //if (TxtExceisDuty.Text != "")
            //    TotalAmt = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(TxtExceisDuty.Text) / 100;
            //if (TxtCst.Text != "")
            //    cst = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(TxtCst.Text) / 100;

            if (txtSGST.Text != "")
                TotalAmt = Convert.ToDouble(TxtAmount.Text) * (Convert.ToDouble(txtSGST.Text) + Convert.ToDouble(txtCGST.Text)) / 100;
            if (txtIGST.Text != "")
                IGST = Convert.ToDouble(TxtAmount.Text) * Convert.ToDouble(txtIGST.Text) / 100;
            if (txtTCS.Text != "")
                TCS = Convert.ToDouble(TxtAmount.Text == "" ? "0" : TxtAmount.Text) * Convert.ToDouble(txtTCS.Text == "" ? "0" : txtTCS.Text) / 100;
            TxtNetAmount.Text = Convert.ToString(Convert.ToDouble(TxtAmount.Text) + TotalAmt + IGST + TCS);
        }
    }
    protected void TxtRate_TextChanged(object sender, EventArgs e)
    {
        fill_text();
        //if (Session["varcompanyNo"].ToString() == "12")
        //{
        //    Double rate;
        //    int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //    rate = UtilityModule.getItemRate(Convert.ToInt16(ddempname.SelectedValue), varfinishedid, "PURCHASE");
        //    lblerrormessage.Visible = false;
        //    lblerrormessage.Text = "";
        //    if (rate < Convert.ToDouble(TxtRate.Text))
        //    {
        //        lblerrormessage.Visible = true;
        //        lblerrormessage.Text = "Rate can not be greater than " + rate;
        //        TxtRate.Text = rate.ToString();
        //        TxtAmount.Text = "0";
        //    }
        //}
        TxtLotNo.Focus();
    }
    protected void txtSGST_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    protected void txtIGST_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    protected void TxtExceisDuty_TextChanged(object sender, EventArgs e)
    {
        fill_text();
        TxtEduCess.Focus();
    }
    protected void TxtEduCess_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    protected void TxtCst_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    protected void txtfrieghtrate_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select DISTINCT PRODUCTCODE from ITEM_PARAMETER_MASTER IPM inner join ORDER_CONSUMPTION_DETAIL OCD on OCD.IFINISHEDID=IPM.ITEM_FINISHED_ID Where ProductCode Like '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyId;
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
    protected void TxtIndentNo_TextChanged(object sender, EventArgs e)
    {
        indentchage();
        TDGridShow.Visible = true;
        Fill_GridForLocalConsump();
    }
    private void indentchage()
    {
        DataSet ds = null;
        try
        {
            ds = null;
            string sql = "";

            sql = "select PIndentId,PIndentNo,PartyId,DepartmentId,CompanyId from PurchaseIndentMaster where FlagApproval=1 And CompanyId = " + ddCompName.SelectedValue + " and PIndentNo='" + TxtIndentNo.Text + "' And MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["PartyId"].ToString()) != null)
                {
                    ddempname.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
                }
                if (variable.VarPURCHASEORDER_INDENTOTHERVENDOR == "1")
                {
                    if (ChkEditOrder.Checked == true)
                    {
                        sql = "select distinct PIndentId,pindentno from V_PURCHASEINDENTPENDINGINDENTNO where companyid=" + ddCompName.SelectedValue + "   Order By pindentno ";
                    }
                    else
                    {
                        sql = "select distinct PIndentId,pindentno from V_PURCHASEINDENTPENDINGINDENTNO where companyid=" + ddCompName.SelectedValue + " and Round((Indentqty-orderedqty),0)>0  Order By pindentno ";
                    }
                }
                else
                {
                    sql = "select distinct PIndentId,pindentno from PurchaseIndentMaster where companyid=" + ddCompName.SelectedValue + " and partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By pindentno ";
                }

                UtilityModule.ConditionalComboFill(ref ddindentno, sql, true, "Select IndentNo");

                ddindentno.SelectedValue = ds.Tables[0].Rows[0]["PIndentId"].ToString();

                UtilityModule.ConditionalComboFill(ref ddCatagory, @" select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm 
                inner join ITEM_MASTER im on icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id
                inner join PurchaseIndentDetail pim on pim.finishedid=ipm.ITEM_FINISHED_ID inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where pim.pindentid=" + ddindentno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME ", true, "Select Category");
                ddCatagory.SelectedIndex = 1;
                ddlcategorycange();
                if (chkindentvise.Checked)
                {
                    UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm on im.item_id=ipm.item_id
                    inner join PurchaseIndentDetail pid on ipm.ITEM_FINISHED_ID=pid.FinishedId where pid.PIndentId=" + ddindentno.SelectedValue + " and im.category_id=" + ddCatagory.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ", true, "Select Item");
                }
                else if (chkcustomervise.Checked)
                {
                    if (hncunsp.Value == "1")
                    {
                        UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm  on im.item_id=ipm.item_id
                        inner join ORDER_CONSUMPTION_DETAIL ocm on ipm.ITEM_FINISHED_ID=ocm.FinishedId where im.CATEGORY_ID=" + ddCatagory.SelectedValue + " and ocm.orderid=" + ddorderno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ", true, "Select Item");
                    }
                    else
                    {
                        UtilityModule.ConditionalComboFill(ref dditemname, @"select distinct im.item_id,im.item_name from ITEM_MASTER im  inner join ITEM_PARAMETER_MASTER ipm  on im.item_id=ipm.item_id
                        inner join OrderLocalConsumption  ocm on ipm.ITEM_FINISHED_ID=ocm.FinishedId where im.CATEGORY_ID=" + ddCatagory.SelectedValue + " and ocm.orderid=" + ddorderno.SelectedValue + " And im.MasterCompanyId=" + Session["varCompanyId"] + " Order By im.item_name ", true, "Select Item");
                    }
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref dditemname, "select distinct item_id,item_name  from ITEM_MASTER where category_id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By item_name ", true, "Select Item");
                }
                Label1.Text = "";
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Indent No Does not exist.............";
                TxtIndentNo.Text = "";
                TxtIndentNo.Focus();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        //if (ddorderno.SelectedIndex > 0)
        {
            DataSet ds1;
            string Str;
            if (TxtProdCode.Text != "")
            {
                ddCatagory.SelectedIndex = 1;
                //                Str = @"select IPM.*,IM.CATEGORY_ID,cs.id from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate cs where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid
                //                  and ProductCode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];
                Str = @"select IPM.*,IM.CATEGORY_ID,cs.id, IsNull(PHOTO,'') PHOTO from ITEM_MASTER IM,CategorySeparate cs, ITEM_PARAMETER_MASTER IPM LEFT OUTER JOIN MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID  where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid  and ProductCode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];

                ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                    dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID inner join UserRights_Category UC 
                    on(ITEM_CATEGORY_MASTER.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + @") And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varCompanyId"] + @" Order By ITEM_CATEGORY_MASTER.CATEGORY_NAME 
                    Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By Item_Name 
                    select qualityid,qualityname from quality where item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By qualityname 
                    select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By DesignName
                    SELECT  ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By ColorName
                    select  Shapeid,ShapeName from Shape Where MasterCompanyid=" + Session["varCompanyId"] + @" Order by ShapeName
                    SELECT  SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString()) + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By SIZEFT
                    ";
                    if (ChkWithoutOrder.Checked == true)
                    {
                        Qry = Qry + @" SELECT ID, FINISHED_TYPE_NAME from FINISHED_TYPE ORDER BY FINISHED_TYPE_NAME";
                    }
                    else
                    {
                        Qry = Qry + @" SELECT OCD.I_FINISHED_TYPE_ID,FT.FINISHED_TYPE_NAME from ORDER_CONSUMPTION_DETAIL OCD,FINISHED_TYPE FT 
                    Where OCD.I_FINISHED_TYPE_ID=FT.ID AND ORDERID=" + ddorderno.SelectedValue + " AND IFINISHEDID=" + ds1.Tables[0].Rows[0]["Item_Finished_id"] + " ORDER BY OCD.PCMDID";
                    }
                    Qry = Qry + @"
                    SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " And i.MasterCompanyId=" + Session["varCompanyId"] + " Order By u.UnitName ";
                    DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                    UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "Select Catagory");
                    ddCatagory.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref dditemname, DSQ, 1, true, "--Select Item--");
                    dditemname.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                    dquality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "Select Design");
                    dddesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                    ddcolor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                    ddshape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                    ddsize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();
                    lblimage.ImageUrl = ds1.Tables[0].Rows[0]["PHOTO"].ToString();
                    //hfImgURL.Value = ds1.Tables[0].Rows[0]["ImagePath"].ToString();
                    UtilityModule.ConditionalComboFillWithDS(ref ddFinish_Type, DSQ, 7, true, "Finish Type");
                    if (ddFinish_Type.Items.Count > 0)
                    {
                        ddFinish_Type.SelectedIndex = 1;
                    }
                    UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 8, true, "Select Unit");
                    if (ddlunit.Items.Count > 0)
                    {
                        ddlunit.SelectedIndex = 1;
                    }
                    Session["finishedid"] = ds1.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                    if (ChkWithoutOrder.Checked == false)
                    {
                        Qry = @"SELECT ISNULL(SUM(CASE WHEN ORDERCALTYPE=1 THEN OCD.IQTY*QTYREQUIRED ELSE OCD.IQTY*QTYREQUIRED*TOTALAREA END),0) QTY FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID AND OM.ORDERID=OD.ORDERID AND IRATE<>0 AND OCD.ORDERID=" + ddorderno.SelectedValue + " AND IFINISHEDID=" + ds1.Tables[0].Rows[0]["Item_Finished_id"] + @"
                                SELECT ISNULL(OCD.IRATE,0) FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID AND OM.ORDERID=OD.ORDERID AND OCD.IRATE<>0 AND OCD.ORDERID=" + ddorderno.SelectedValue + " AND IFINISHEDID=" + ds1.Tables[0].Rows[0]["Item_Finished_id"];
                        DSQ = SqlHelper.ExecuteDataset(Qry);
                        if (DSQ.Tables[0].Rows.Count > 0)
                        {
                            txtqty.Text = DSQ.Tables[0].Rows[0][0].ToString();
                            if (DSQ.Tables[1].Rows.Count > 0)
                            {
                                TxtRate.Text = DSQ.Tables[1].Rows[0][0].ToString();
                            }
                            DataSet dsqty = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(sum(quantity) ,0)from PurchaseIndentIssueTran pt inner join PurchaseIndentIssue pi On   pi.PindentIssueid=pt.PindentIssueid where pi.orderid=" + ddorderno.SelectedValue + " and Finishedid=" + ds1.Tables[0].Rows[0]["Item_Finished_id"] + " And pi.MasterCompanyId=" + Session["varCompanyId"] + "");
                            txtqty.Text = Convert.ToString(Convert.ToDouble(DSQ.Tables[0].Rows[0][0].ToString()) - Convert.ToDouble(dsqty.Tables[0].Rows[0][0].ToString()));
                        }
                        fill_text();
                    }
                    TxtLotNo.Focus();
                    if (Convert.ToInt32(dquality.SelectedValue) > 0)
                    {
                        ql.Visible = true;
                    }
                    else
                    {
                        ql.Visible = false;
                    }
                    int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0);
                    if (d > 0)
                    {
                        dsn.Visible = true;
                    }
                    else
                    {
                        dsn.Visible = false;
                    }
                    int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                    if (c > 0)
                    {
                        clr.Visible = true;
                    }
                    else
                    {
                        clr.Visible = false;
                    }
                    int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                    if (s > 0)
                    {
                        shp.Visible = true;
                    }
                    else
                    {
                        shp.Visible = false;
                    }
                    int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                    if (si > 0)
                    {
                        sz.Visible = true;
                    }
                    else
                    {
                        sz.Visible = false;
                    }
                    Label2.Visible = false;
                }
                else
                {
                    Label2.Visible = true;
                    TxtProdCode.Text = "";
                    TxtProdCode.Focus();
                }
                txtcomp_date.Focus();
            }
            else
            {
                ddCatagory.SelectedValue = null;
                TxtProdCode.Focus();
            }
        }
    }
    protected void BTNCLOSE_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyId"].ToString() == "7")
        {
            chkcustomervise.Checked = true;
            ddcustomercode.SelectedIndex = 1;
        }
        if (ChkEditOrder.Checked == true)
        {
            if (variable.VarPURCHASEORDERWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = true;
                FIlllegalvendor();
                BtnChangeVendorName.Visible = true;
            }

            if (variable.VarUpdatePurchaseOrderGSTType == "1" && Session["usertype"].ToString() == "1")
            {
                BtnUpdateGSTType.Visible = true;
            }

            if (Session["varCompanyId"].ToString() != "7")
            {
                chkcustomervise.Checked = false;
            }
            if (Session["varCompanyId"].ToString() == "2")
            {
                chkcustomervise.Checked = true;
            }
            if (hncompid.Value == "3")
            {
                chkcustomervise.Checked = true;
            }
            if (hncompid.Value == "5")
            {
                TDPoNoNew.Visible = true;
            }
            if (hncompid.Value == "10")
            {
                chkcustomervise.Checked = true;
            }
            if (hncompid.Value == "20")
            {
                TDManualOrderNo.Visible = true;
                txtchalan_no.Enabled = true;
            }
            ddcustomercodechanged();
            tdchalanno.Visible = true;
            tdchalan.Visible = true;
            pnl1.Enabled = true;
            ddempname.SelectedIndex = -1;
            txtchalanno.Text = "";
            txtchalan_no.Text = txtchalanno.Text;
            gddetail.Visible = false;
            if (ddcustomercode.Items.Count > 0)
            {
                if (hncompid.Value != "2")
                {
                    ddcustomercode.SelectedIndex = 1;
                    ddcustomercodechanged();
                }
            }
            BtnOrderComplete.Visible = true;
            if (Session["varcompanyid"].ToString() == "6")
            {
                switch (Session["varuserid"].ToString())
                {
                    case "1":
                    case "18":
                    case "38":
                        TDCheckForComplete.Visible = true;
                        break;
                }
            }
            else
            {
                if (Session["varuserid"].ToString() == "1")
                {
                    TDCheckForComplete.Visible = true;
                }
            }
            if (Session["usertype"].ToString() == "1")
            {
                TDPoNoNew.Visible = true;
            }

        }
        else
        {
            TDPoNoNew.Visible = false;
            chkindentvise.Checked = false;
            ddempname.SelectedIndex = -1;
            ddcustomercodechanged();
            tdchalan.Visible = false;
            tdchalanno.Visible = false;
            pnl1.Enabled = true;
            txtchalanno.Text = "";
            txtchalan_no.Text = txtchalanno.Text;
            gddetail.Visible = false;
            if (ddcustomercode.Items.Count > 0)
            {
                ddcustomercode.SelectedIndex = 1;
                ddcustomercodechanged();
            }
            TDCheckForComplete.Visible = false;
            BtnOrderComplete.Visible = false;
            txtManualChallanNo.Text = "";
            dquality.SelectedIndex = -1;
            dditemname.SelectedIndex = -1;
            ddlshade.SelectedIndex = -1;

            if (variable.VarPURCHASEORDERWITHLEGALVENDOR == "1")
            {
                Tdlegalvendor.Visible = false;
                //FIlllegalvendor();
                BtnChangeVendorName.Visible = false;
                DDlegalvendor.Items.Clear();
            }

            if (variable.VarUpdatePurchaseOrderGSTType == "1")
            {
                BtnUpdateGSTType.Visible = false;
            }
        }

    }
    protected void Txtchalan_no_TextChanged(object sender, EventArgs e)
    {
        if (txtchalan_no.Text != "")
        {
            DataSet dt2 = new DataSet();
            string str2 = @"Select companyid,partyid,orderid,indentid,challanno,date,duedate,pindentissueid,remarks,PayeMentTermId,TranportModeId,DeliveryTermId,isnull(ManualChallanNo,'') as ManualChallanNo, CustomerID,requestby,requestfor 
            From PurchaseIndentIssue(Nolock) 
            Where companyid = " + ddCompName.SelectedValue + " And IsNull(BranchID, 0) = " + DDBranchName.SelectedValue + @" And 
            Challanno='" + txtchalan_no.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            if (ChkForComplete.Checked == true)
            {
                str2 = str2 + " And Status = 'complete' ";
            }
            else
            {
                str2 = str2 + " And Status = 'Pending' ";
            }

            dt2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            if (dt2.Tables[0].Rows.Count > 0)
            {
                DataSet dt3 = new DataSet();
                if (dt2.Tables[0].Rows[0]["orderid"].ToString() != "0")
                {
                    chkindentvise.Checked = false;
                    chkcustomervise.Checked = true;
                    chkcustomervise_Checked();
                }
                else if (dt2.Tables[0].Rows[0]["indentid"].ToString() != "0")
                {
                    chkcustomervise.Checked = false;
                    chkindentvise.Checked = true;
                    chkindentvise_CheckedChanged();
                    TxtIndentNo.Text = dt2.Tables[0].Rows[0]["indentid"].ToString();
                    indentchage();
                }
                else
                {
                    chkindentvise.Checked = false;
                    chkcustomervise.Checked = false;
                    OnCheckedChange();
                }
                string st3 = "select customerid from ordermaster where orderid in(select orderid from PurchaseIndentIssue where MasterCompanyId=" + Session["varCompanyId"] + " And pindentissueid=" + dt2.Tables[0].Rows[0]["pindentissueid"].ToString() + ")";
                dt3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, st3);
                if (dt3.Tables[0].Rows.Count > 0)
                {
                    ddcustomercode.SelectedValue = dt3.Tables[0].Rows[0][0].ToString();
                    ddcustomercodechanged();
                }
                if (Convert.ToInt32(dt2.Tables[0].Rows[0]["CustomerID"]) > 0)
                {
                    ChkForForSampleFlag.Checked = true;
                    ChkForForSampleFlag_CheckedChanged(sender, new EventArgs());
                    ddcustomercode.SelectedValue = dt2.Tables[0].Rows[0]["CustomerID"].ToString();
                }
                ddorderno.SelectedValue = dt2.Tables[0].Rows[0]["orderid"].ToString();
                ddempname.SelectedValue = dt2.Tables[0].Rows[0]["partyid"].ToString();
                if (ChkForComplete.Checked == true)
                {
                    UtilityModule.ConditionalComboFill(ref ddlchalanno, "Select PindentIssueid,challanno from PurchaseIndentIssue Where Status='Complete' And Companyid=" + ddCompName.SelectedValue + " And Partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Challanno", true, "--Select--.");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddlchalanno, "Select PindentIssueid,challanno from PurchaseIndentIssue Where Status='Pending' And Companyid=" + ddCompName.SelectedValue + " And Partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Challanno", true, "--Select--.");
                }

                ddlchalanno.SelectedValue = dt2.Tables[0].Rows[0]["pindentissueid"].ToString();
                txtdate.Text = Convert.ToDateTime(dt2.Tables[0].Rows[0]["date"].ToString()).ToString("dd-MMM-yyyy");
                txtchalanno.Text = dt2.Tables[0].Rows[0]["challanno"].ToString();
                txtduedate.Text = Convert.ToDateTime(dt2.Tables[0].Rows[0]["duedate"].ToString()).ToString("dd-MMM-yyyy");
                ddpayement.SelectedValue = dt2.Tables[0].Rows[0]["PayeMentTermId"].ToString();
                ddtransprt.SelectedValue = dt2.Tables[0].Rows[0]["TranportModeId"].ToString();
                dddelivery.SelectedValue = dt2.Tables[0].Rows[0]["DeliveryTermId"].ToString();
                txtremarks.Text = dt2.Tables[0].Rows[0]["remarks"].ToString();
                txtManualChallanNo.Text = dt2.Tables[0].Rows[0]["ManualChallanNo"].ToString();
                txtManualChallanNo.Text = dt2.Tables[0].Rows[0]["ManualChallanNo"].ToString();
                txtReqBy.Text = dt2.Tables[0].Rows[0]["requestby"].ToString();
                txtreqfor.Text = dt2.Tables[0].Rows[0]["requestfor"].ToString();
                if (hncunsp.Value == "1")
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm inner join ITEM_MASTER im on 
                    icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id  inner join OrderLocalConsumption  OCD on 
                    OCD.IFINISHEDID=ipm.ITEM_FINISHED_ID inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where OCD.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME", true, "Select Category");
                }
                else if (ddorderno.Items.Count > 0)
                {
                    UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm inner join ITEM_MASTER im on 
                    icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id  inner join ORDER_CONSUMPTION_DETAIL OCD on 
                    OCD.IFINISHEDID=ipm.ITEM_FINISHED_ID inner join UserRights_Category UC on(icm.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where OCD.orderid=" + ddorderno.SelectedValue + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + " Order By icm.CATEGORY_NAME", true, "Select Category");
                }
                // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
                ViewState["PIndentIssueId"] = dt2.Tables[0].Rows[0]["pindentissueid"].ToString();
                Fill_Grid();
                report();
                TxtProdCode.Focus();
                Fill_GridForLocalConsump();
            }
            else
            {
                refresh_form();
                btnsave.Enabled = true;
                txtchalan_no.Focus();
            }
        }
    }
    protected void ddlchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalan_no.Text = ddlchalanno.SelectedItem.Text;
        txtchalanno.Text = ddlchalanno.SelectedItem.Text;
        if (Session["varCompanyId"].ToString() == "7")
        {
            DataSet dt9 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select replace(convert(varchar(11),date,106),' ','-'),Remark from PurchaseTracking where tablename='PurchaseIndentIssue' and ptrackid=" + ddlchalanno.SelectedValue + " order by date desc ");
            if (dt9.Tables[0].Rows.Count > 0)
            {
                txtNextdate.Text = dt9.Tables[0].Rows[0][0].ToString();
                TXTreviseRemark.Text = dt9.Tables[0].Rows[0]["Remark"].ToString();
            }
        }
        DataSet dt8 = new DataSet();
        string str2 = "select pindentissueid,orderid,indentid,convert(varchar(11),Date,106) as Date,convert(varchar(11),duedate,106) as duedate,remarks,PayeMentTermId,TranportModeId,DeliveryTermId,isnull(ManualChallanNo,'') as ManualChallanNo,DeliveryAddress,requestby,requestfor from PurchaseIndentIssue where pindentissueid=" + ddlchalanno.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
        dt8 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        if (dt8.Tables[0].Rows.Count > 0)
        {
            ViewState["PIndentIssueId"] = dt8.Tables[0].Rows[0]["pindentissueid"].ToString();
            txtduedate.Text = Convert.ToDateTime(dt8.Tables[0].Rows[0]["duedate"].ToString()).ToString("dd-MMM-yyyy");
            txtremarks.Text = dt8.Tables[0].Rows[0]["remarks"].ToString();
            txtdate.Text = Convert.ToDateTime(dt8.Tables[0].Rows[0]["Date"].ToString()).ToString("dd-MMM-yyyy");
            ddpayement.SelectedValue = dt8.Tables[0].Rows[0]["PayeMentTermId"].ToString();
            ddtransprt.SelectedValue = dt8.Tables[0].Rows[0]["TranportModeId"].ToString();
            dddelivery.SelectedValue = dt8.Tables[0].Rows[0]["DeliveryTermId"].ToString();
            txtManualChallanNo.Text = dt8.Tables[0].Rows[0]["ManualChallanNo"].ToString();
            if (Session["varcompanyid"].ToString() != "44")
            {
                txtDeliveryAddress.Text = dt8.Tables[0].Rows[0]["DeliveryAddress"].ToString();
            }
            txtReqBy.Text = dt8.Tables[0].Rows[0]["requestby"].ToString();
            txtreqfor.Text = dt8.Tables[0].Rows[0]["requestfor"].ToString();
            report();
            Fill_Grid();

        }
    }
    protected void txtrate1_changed(object sender, EventArgs e)
    {
        for (int i = 0; i < gddetail.Rows.Count; i++)
        {
            string rate = ((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Text;
            string qty = ((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Text;
            string amount;
            //if (Session["varcompanyNo"].ToString() == "12")
            //{
            //    string varfinishedid = ((Label)gddetail.Rows[i].FindControl("lblFinishedid")).Text;
            //    string Hrate = ((Label)gddetail.Rows[i].FindControl("lblhrate")).Text;
            //    Double Itemrate = UtilityModule.getItemRate(Convert.ToInt16(ddempname.SelectedValue), Convert.ToInt16(varfinishedid), "PURCHASE");
            //    if (Convert.ToDouble(rate) > Itemrate)
            //    {
            //        TextBox txtrate = ((TextBox)gddetail.Rows[i].FindControl("TXTRate1"));
            //        txtrate.Text = Hrate;
            //        rate = Hrate;
            //    }
            //}
            amount = Convert.ToString(Convert.ToDouble(rate) * Convert.ToDouble(qty));
            gddetail.Rows[i].Cells[7].Text = amount;
            FILL_TEXT1();
        }
    }
    protected void txtqnt1_changed(object sender, EventArgs e)
    {
        int Rowindex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        string rate = ((TextBox)gddetail.Rows[Rowindex].FindControl("TXTRate1")).Text;
        string qty = ((TextBox)gddetail.Rows[Rowindex].FindControl("TXTQTY1")).Text;
        string CanQty = ((TextBox)gddetail.Rows[Rowindex].FindControl("Txtcancel")).Text == "" ? "0" : ((TextBox)gddetail.Rows[Rowindex].FindControl("Txtcancel")).Text;
        string PIndentIssueTranId = ((Label)gddetail.Rows[Rowindex].FindControl("lbldetailId")).Text;
        string amount;

        string str = @"select PT.PindentIssueId,PT.PIndentIssueTranid,Quantity As issueQty,Isnull(CanQty,0) As CanQty,Isnull(Sum(Qty-isnull(ReturnQty,0)),0) As RecQty,PT.extraqty  from PurchaseIndentIssueTran PT Left Outer Join PurchaseReceivedetail PR 
                     On PT.Pindentissuetranid=PR.PIndentIssueTranId left outer join V_PurchaseReturnDetail V on V.PurchaseReceiveDetailId=PR.PurchaseReceiveDetailId
                     Where PT.PindentIssueTranid=" + PIndentIssueTranId + " group  by PT.PindentIssueId,PT.PIndentIssueTranid,Quantity,CanQty,PT.extraqty";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Double tot = Convert.ToDouble(qty) - Convert.ToDouble(ds.Tables[0].Rows[0]["RecQty"]) + Convert.ToDouble(CanQty);
            if (Convert.ToDouble(qty) < tot || tot < 0)
            {
                Label3.Text = "Plz check issue Qty...";
                Label3.Visible = true;
                ((TextBox)gddetail.Rows[Rowindex].FindControl("TXTQTY1")).Text = ds.Tables[0].Rows[0]["IssueQty"].ToString();
                return;
            }
            else
            {
                Label3.Visible = false;
                Label3.Text = "";
            }
        }
        amount = Convert.ToString(Convert.ToDouble(rate) * Convert.ToDouble(qty));
        gddetail.Rows[Rowindex].Cells[7].Text = amount;
        FILL_TEXT1();
        if (hncunsp.Value == "1")
        {
            string date = ((TextBox)gddetail.Rows[Rowindex].FindControl("txtdivery_date")).Text;
            if (Convert.ToDateTime(hnreqdate.Value) >= Convert.ToDateTime(date))
            {
                Label3.Text = "";
                Label3.Visible = false;
                txtweig.Focus();
            }
            else
            {
                Label3.Text = "Delivery Date is Less Than Required Date";
                Label3.Visible = true;
                txtcomp_date.Focus();
            }

        }
    }
    private void visible()
    {
        if (ChkEditOrder.Checked == true)
        {
            for (int i = 0; i < gddetail.Rows.Count; i++)
            {
                DataSet ds7 = new DataSet();
                string str7 = "select pindentissuetranid from PurchaseReceiveDetail where pindentissuetranid=" + ((Label)gddetail.Rows[i].FindControl("lbldetailid")).Text + "";
                ds7 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str7);
                if (ds7.Tables[0].Rows.Count > 0)
                {
                    ((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Enabled = false;
                    ((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Enabled = false;
                }
                else
                {
                    ((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Enabled = true;
                    ((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Enabled = true;
                    ((TextBox)gddetail.Rows[i].FindControl("TXTextraQTY")).Enabled = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < gddetail.Rows.Count; i++)
            {
                ((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Enabled = true;
                ((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Enabled = true;
            }
        }
    }
    private void grid_data()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran1 = con.BeginTransaction();
        try
        {
            SqlParameter[] arr1 = new SqlParameter[29];
            arr1[0] = new SqlParameter("@PindentIssueid", SqlDbType.Int);
            arr1[1] = new SqlParameter("@Pindentissuetranid", SqlDbType.Int);
            arr1[2] = new SqlParameter("@QTY", SqlDbType.Float);
            arr1[3] = new SqlParameter("@RATE", SqlDbType.Float);
            arr1[4] = new SqlParameter("@amount", SqlDbType.Float);
            arr1[5] = new SqlParameter("@weight", SqlDbType.Float);
            arr1[6] = new SqlParameter("@deliverydate", SqlDbType.DateTime);
            arr1[7] = new SqlParameter("@duedate", SqlDbType.DateTime);
            arr1[8] = new SqlParameter("@Vat", SqlDbType.Float);
            arr1[9] = new SqlParameter("@NetAmount", SqlDbType.Float);
            arr1[10] = new SqlParameter("@Cst", SqlDbType.Float);
            arr1[11] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 250);
            arr1[12] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr1[13] = new SqlParameter("@CompanyId", SqlDbType.Int);
            arr1[14] = new SqlParameter("@canqty", SqlDbType.Float);
            arr1[15] = new SqlParameter("@Tdate", SqlDbType.DateTime);
            arr1[16] = new SqlParameter("@companyno", SqlDbType.Int);
            arr1[17] = new SqlParameter("@RRemarks", SqlDbType.NVarChar, 250);
            arr1[18] = new SqlParameter("@ItemRemark", SqlDbType.NVarChar, 4000);
            arr1[19] = new SqlParameter("@Date", SqlDbType.DateTime);
            arr1[20] = new SqlParameter("@freight", SqlDbType.Float);
            arr1[21] = new SqlParameter("@freightRate", SqlDbType.Float);
            arr1[22] = new SqlParameter("@PayeMentTermId", SqlDbType.Int);
            arr1[23] = new SqlParameter("@TranportModeId", SqlDbType.Int);
            arr1[24] = new SqlParameter("@DeliveryTermId", SqlDbType.Int);
            arr1[25] = new SqlParameter("@SGST", SqlDbType.Float);
            arr1[26] = new SqlParameter("@IGST", SqlDbType.Float);
            arr1[27] = new SqlParameter("@ManualChallanNo", SqlDbType.VarChar, 50);
            arr1[28] = new SqlParameter("@msg", SqlDbType.VarChar, 300);
            for (int i = 0; i < gddetail.Rows.Count; i++)
            {
                arr1[0].Direction = ParameterDirection.InputOutput;
                arr1[0].Value = ViewState["PIndentIssueId"];
                arr1[1].Value = ((Label)gddetail.Rows[i].FindControl("lbldetailid")).Text;
                arr1[2].Value = ((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Text;
                arr1[3].Value = ((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Text;
                //arr1[4].Value = ((Label)gddetail.Rows[i].FindControl("lblAmount")).Text=="" ? "0" : ((Label)gddetail.Rows[i].FindControl("lblAmount")).Text;
                arr1[4].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Text) * Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Text);
                //arr1[4].Value = gddetail.Rows[i].Cells[7].Text != "" ? gddetail.Rows[i].Cells[7].Text : "0";
                if (((TextBox)gddetail.Rows[i].FindControl("TXTweig1")).Text != "")
                    arr1[5].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTweig1")).Text);
                else
                    arr1[5].Value = 0;
                arr1[6].Value = ((TextBox)gddetail.Rows[i].FindControl("txtdivery_date")).Text;
                arr1[7].Value = txtduedate.Text;
                arr1[8].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTvat")).Text);
                arr1[9].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTnetamount")).Text);
                arr1[10].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTcst")).Text);
                arr1[11].Value = txtremarks.Text;
                arr1[12].Value = Session["varuserid"].ToString();
                arr1[13].Value = Session["varCompanyId"].ToString();
                arr1[14].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("Txtcancel")).Text);
                if (txtNextdate.Visible == false || Tdnextdate.Visible == false)
                {
                    arr1[15].Value = DateTime.Now.Date.ToString();
                }
                else
                {
                    arr1[15].Value = txtNextdate.Text;
                }
                arr1[16].Value = Session["varcompanyno"].ToString();
                if (TXTreviseRemark.Visible == false || trrevisedremark.Visible == false)
                {
                    arr1[17].Value = "";
                }
                else
                {
                    arr1[17].Value = TXTreviseRemark.Text;
                }
                arr1[18].Value = ((TextBox)gddetail.Rows[i].FindControl("Txtitemremark")).Text;
                arr1[19].Value = txtdate.Text;
                arr1[20].Value = txtfrieght.Text == "" ? "0" : txtfrieght.Text;
                arr1[21].Value = txtfrieghtrate.Text == "" ? "0" : txtfrieghtrate.Text;
                arr1[22].Value = ddpayement.SelectedIndex <= 0 ? "0" : ddpayement.SelectedValue;
                arr1[23].Value = ddtransprt.SelectedIndex <= 0 ? "0" : ddtransprt.SelectedValue;
                arr1[24].Value = dddelivery.SelectedIndex <= 0 ? "0" : dddelivery.SelectedValue;

                arr1[25].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTSGST")).Text) + Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTCGST")).Text);
                arr1[26].Value = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTIGST")).Text);

                arr1[27].Value = txtManualChallanNo.Text;
                arr1[28].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(tran1, CommandType.StoredProcedure, "[Pro_PUrchaseIndentIssue_1]", arr1);
                ViewState["PIndentIssueId"] = arr1[0].Value;
                if (arr1[28].Value != "")
                {
                    i = gddetail.Rows.Count + 1;
                }

            }
            if (arr1[28].Value != "")
            {
                msg = Convert.ToString(arr1[28].Value);
                tran1.Rollback();
            }
            else
            {
                tran1.Commit();
                msg = "Record(s)  has been saved successfully !";
            }
            MessageSave(msg);
        }
        catch (Exception ex)
        {
            MessageSave(ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
            tran1.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void report()
    {
        if (Convert.ToInt32(Session["varCompanyId"]) == 2)
        {
            Session["ReportPath"] = "Reports/PurchaseIndentIssDestini_1NEW.rpt";
            Session["CommanFormula"] = "{V_PIndentIssueReport_1.PIndentIssueId}=" + ViewState["PIndentIssueId"] + "";
        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 6)//For ArtIndia
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssArtNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIss_withoutratNEW.rpt";
            }
            else
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssArtWithoutimgNEW.rpt";
            }
        }
        else if (Convert.ToInt16(Session["varcompanyId"]) == 9) //Hafizia
        {
            Session["ReportPath"] = "Reports/PurchaseIndentIssNEWForHafizia.rpt";

        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 20)//For MaltiRugs
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssMaltiRugWithoutRateNEW.rpt";
            }
            else
            {
                //PurchaseIndentIssWithoutimgNEW
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";
            }
        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 27)//For AntiquePanipat
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssNewAntique.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssMaltiRugWithoutRateNEW.rpt";
            }
            else
            {
                //PurchaseIndentIssWithoutimgNEW
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";
            }
        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 22)//For DiamondExport
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssNewDiamond.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIss_withoutratNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "3")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEWWO.rpt";
            }
            else if (DDPreviewType.SelectedValue == "4")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEWSALE.rpt";
            }
            else
            {
               // //PurchaseIndentIssWithoutimgNEW
               // Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";

                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";
            }
        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 21)//For Kaysons Agra
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssNewDiamond.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIss_withoutratNEW.rpt";
            }
            else
            {
                //PurchaseIndentIssWithoutimgNEW
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEWKaysons.rpt";
            }
        }
        else if (Convert.ToInt32(Session["varCompanyId"]) == 44)//For Agni
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssAgni.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIss_withoutratNEW.rpt";
            }
            else
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";
            }
        }   
        else
        {
            if (DDPreviewType.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIssNEW.rpt";
            }
            else if (DDPreviewType.SelectedValue == "1")
            {
                Session["ReportPath"] = "Reports/PurchaseIndentIss_withoutratNEW.rpt";
            }
            else
            {
                //PurchaseIndentIssWithoutimgNEW
                Session["ReportPath"] = "Reports/PurchaseIndentIssWithoutimgNEW.rpt";
            }
            Session["CommanFormula"] = "{V_PIndentIssueReport.PIndentIssueId}=" + ViewState["PIndentIssueId"] + "";
        }
    }
    private void FILL_TEXT1()
    {
        // double tot = 0;
        double qty,extraqty, rate, VAT, CST, amt, SGST, IGST, Weight;
        double TotalQty = 0,totalextraqty=0, TotalAmount = 0, TotalWeight = 0;
        for (int i = 0; i < gddetail.Rows.Count; i++)
        {

            qty = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTQTY1")).Text);
           // extraqty = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTextraQTY")).Text);
          //  extraqty = Convert.ToDouble(val); //Convert.ToDouble(((Label)gddetail.Rows[i].FindControl("lblextraqty")).Text);
            rate = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTRate1")).Text);
            Weight = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTweig1")).Text);
            gddetail.Rows[i].Cells[7].Text = Convert.ToString(qty * rate);
            amt = Convert.ToDouble(gddetail.Rows[i].Cells[7].Text);
            //VAT = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTvat")).Text);
            //CST = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTcst")).Text);
            SGST = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTSGST")).Text) + Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTCGST")).Text);
            IGST = Convert.ToDouble(((TextBox)gddetail.Rows[i].FindControl("TXTIGST")).Text);
            ((TextBox)gddetail.Rows[i].FindControl("TXTnetamount")).Text = Convert.ToString(amt + (amt * SGST) / 100 + (amt * IGST) / 100);

            TotalQty += qty;
           // totalextraqty += extraqty;
            TotalAmount += amt;
            TotalWeight += Weight;

        }
        Label lblTotalQty = (Label)gddetail.FooterRow.FindControl("lblTotalQty");
        lblTotalQty.Text = Convert.ToString(TotalQty);
        Label lblTotalextraqty = (Label)gddetail.FooterRow.FindControl("lblextraTotalQty");
        lblTotalextraqty.Text = Convert.ToString(totalextraqty);
        Label lblTotalAmount = (Label)gddetail.FooterRow.FindControl("lblTotalAmount");
        lblTotalAmount.Text = Convert.ToString(TotalAmount);
        Label lblTotalWeight = (Label)gddetail.FooterRow.FindControl("lblTotalWeight");
        lblTotalWeight.Text = Convert.ToString(TotalWeight);
    }
    protected void DDPreviewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        report();
    }
    protected void gddetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label1.Text = "";
        Label1.Visible = false;
        string pid = ((Label)gddetail.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PurchaseReceiveDetailId from PurchaseReceiveDetail where PIndentIssueTranId=" + pid + " ");
        if (ds.Tables[0].Rows.Count == 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (gddetail.Rows.Count == 1)
                {
                    string Qry = @"delete from PurchaseIndentIssue where PindentIssueid=" + ViewState["PIndentIssueId"] + @"
                    delete from PurchaseIndentIssueTran where PIndentIssueTranId=" + pid;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Qry);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "delete from PurchaseIndentIssueTran where PIndentIssueTranId=" + pid + " ");
                }

                Tran.Commit();

                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'PurchaseIndentIssueTran'," + pid + ",getdate(),'Delete')");

                Fill_Grid();

                if (ChkEditOrder.Checked == true)
                {
                    if (gddetail.Rows.Count == 0)
                    {
                        string str = "select pindentissueid,challanno from PurchaseIndentIssue where  orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"];
                        if (ChkForComplete.Checked == true)
                        {
                            str = str + " and Status='Complete'";
                        }
                        else
                        {
                            str = str + " and Status='Pending'";
                        }
                        str = str + " Order By challanno ";
                        UtilityModule.ConditionalComboFill(ref ddlchalanno, str, true, "--Select--.");
                        //if (ChkForComplete.Checked == true)
                        //{
                        //    UtilityModule.ConditionalComboFill(ref ddlchalanno, "select pindentissueid,challanno from PurchaseIndentIssue where Status='Complete' And orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " Order By challanno ", true, "--Select--.");
                        //}
                        //else
                        //{
                        //    UtilityModule.ConditionalComboFill(ref ddlchalanno, "select pindentissueid,challanno from PurchaseIndentIssue where Status='Pending' And orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " Order By challanno ", true, "--Select--.");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
                Tran.Rollback();
                Label1.Visible = true;
                Label1.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        msg = "Record(s)  has been deleted successfully !";
        MessageSave(msg);
    }
    protected void txtduedate_TextChanged(object sender, EventArgs e)
    {
        if (hncunsp.Value == "1")
        {
            if (Convert.ToDateTime(hnreqdate.Value) >= Convert.ToDateTime(txtduedate.Text))
            {
                Label3.Text = "";
                Label3.Visible = false;
                txtremarks.Focus();
            }
            else
            {
                Label3.Text = "Due Date is Less Than Required Date";
                Label3.Visible = true;
                txtdate.Focus();
            }
        }
        else
        {
            txtremarks.Focus();
        }
    }
    protected void txtcomp_date_TextChanged(object sender, EventArgs e)
    {
        if (hncunsp.Value == "1")
        {
            if (Convert.ToDateTime(hnreqdate.Value) >= Convert.ToDateTime(txtcomp_date.Text))
            {
                Label3.Text = "";
                Label3.Visible = false;
                txtweig.Focus();
            }
            else
            {
                Label3.Text = "Delivery Date is Less Than Required Date";
                Label3.Visible = true;
                txtcomp_date.Focus();
            }
        }
        else
        {
            txtweig.Focus();
        }
    }
    private void Fill_GridForLocalConsump()
    {
        DGShowConsumption.DataSource = GetDetail();
        DGShowConsumption.DataBind();
        if (DGShowConsumption.Rows.Count > 0)
        {
            TDGridShow.Visible = true;
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
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
            
            string strsql = "";
            con.Open();
            //hncunsp=1 means OrderWise Consumption
            if (hncunsp.Value == "0")
            {
                if (Session["varcompanyno"].ToString() == "44" && chkcustomervise.Checked == true)
                {
//                    strsql = @"SELECT VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + case when isnull(vc.oshadeid,0)>0 then (select top 1 ShadeColorName from V_FinishedItemDetail where ShadecolorId=vc.oshadeid) else  VF.ShadeColorName end  + '  ' + VF.ShapeName Description,
//                    VF.ITEM_FINISHED_ID, Sum(VC.ConsumptionQty) Qty, Sum(VC.PurchaseQty) PurchaseQty, VC.UnitID, VF.Qualityid, VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid , '0' thanlength, max(VC.Isizeflag) flagsize, VC.finished_type_id I_FINISHED_Type_ID,
//                    '' Remark, '' ItemRemark, 0 IRate, 0 Iweight, VF.ITEM_FINISHED_ID FinishedID  
//                    FROM V_ConsumptionQtyAndPurchaseQtyAgni VC
//                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = vc.finishedid 
//                    --LEFT JOIN DefinePurchaseItemUserWise b(Nolock) ON b.ITEM_FINISHED_ID = vc.finishedid And b.OrderID = VC.OrderID 
//                    Where VC.ORDERID = " + ddorderno.SelectedValue + @" And VF.MasterCompanyId = " + Session["varCompanyId"] + @" --And b.UserID = " + Session["varuserid"] + @"
//                    Group by VF.ITEM_FINISHED_ID, VF.Category_Name, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShadeColorName, VF.ShapeName,VF.Item_Finished_Id, VF.Qualityid,
//                    VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid, VC.finished_type_id, VC.Unitid ,oshadeid ,vf.MasterCompanyId
//                    Having  isnull(sum(VC.consumptionqty),0) > isnull(sum(VC.purchaseqty),0) 
//                    Order By VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName";

                    strsql = @"SELECT VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + case when isnull(min(vc.oshadeid),0)>0 then (select top 1 ShadeColorName from V_FinishedItemDetail where ShadecolorId=min(vc.oshadeid)) else  min(VF.ShadeColorName) end  + '  ' + VF.ShapeName Description,
                     Sum(VC.ConsumptionQty) Qty, Sum(VC.PurchaseQty) PurchaseQty, VC.UnitID, VF.Qualityid, VF.Colorid, VF.designid, VF.shapeid, min(VF.shadecolorid) as shadecolorid,VF.category_id, VF.item_id, VF.sizeid , '0' thanlength, max(VC.Isizeflag) flagsize,
                    '' Remark, '' ItemRemark, 0 IRate, 0 Iweight,max(VF.ITEM_FINISHED_ID) FinishedID , VC.finished_type_id I_FINISHED_Type_ID,vc.finishedid
                    FROM V_ConsumptionQtyAndPurchaseQtyAgni VC
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = vc.finishedid 
                    --LEFT JOIN DefinePurchaseItemUserWise b(Nolock) ON b.ITEM_FINISHED_ID = vc.finishedid And b.OrderID = VC.OrderID 
                    Where VC.ORDERID = " + ddorderno.SelectedValue + @" And VF.MasterCompanyId = " + Session["varCompanyId"] + @" --And b.UserID = " + Session["varuserid"] + @"
                    Group by  VF.Category_Name, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName,VF.ShapeName, VF.Qualityid,
                    VF.Colorid, VF.designid, VF.shapeid,  VF.category_id, VF.item_id, VF.sizeid, VC.Unitid ,vf.MasterCompanyId, VC.finished_type_id,vc.finishedid
                    Having  isnull(sum(VC.consumptionqty),0) > isnull(sum(VC.purchaseqty),0) 
                    ";
                }
                else if (Session["varcompanyno"].ToString() == "16" && chkcustomervise.Checked == true)
                {
                    strsql = @"SELECT VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName Description,
                    VF.ITEM_FINISHED_ID, Sum(VC.ConsumptionQty) Qty, Sum(VC.PurchaseQty) PurchaseQty, VC.UnitID, VF.Qualityid, VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid , '0' thanlength, max(VC.Isizeflag) flagsize, VC.finished_type_id I_FINISHED_Type_ID,
                    '' Remark, '' ItemRemark, 0 IRate, 0 Iweight, VF.ITEM_FINISHED_ID FinishedID  
                    FROM V_ConsumptionAndPurchaseQtyForChampoPNM VC
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = vc.finishedid 
                    --LEFT JOIN DefinePurchaseItemUserWise b(Nolock) ON b.ITEM_FINISHED_ID = vc.finishedid And b.OrderID = VC.OrderID 
                    Where VC.ORDERID = " + ddorderno.SelectedValue + @" And VF.MasterCompanyId = " + Session["varCompanyId"] + @" --And b.UserID = " + Session["varuserid"] + @"
                    Group by VF.ITEM_FINISHED_ID, VF.Category_Name, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShadeColorName, VF.ShapeName,VF.Item_Finished_Id, VF.Qualityid,
                    VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid, VC.finished_type_id, VC.Unitid 
                    Having  isnull(sum(VC.consumptionqty),0) > isnull(sum(VC.purchaseqty),0) 
                    Order By VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName";
                }
                else if (Session["varcompanyno"].ToString() == "28" && chkcustomervise.Checked == true)
                {
                    strsql = @"SELECT VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName Description,
                    VF.ITEM_FINISHED_ID, Sum(VC.ConsumptionQty) Qty, Sum(VC.PurchaseQty) PurchaseQty, VC.UnitID, VF.Qualityid, VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid , '0' thanlength, max(VC.Isizeflag) flagsize, VC.finished_type_id I_FINISHED_Type_ID,
                    '' Remark, '' ItemRemark, 0 IRate, 0 Iweight, VF.ITEM_FINISHED_ID FinishedID  
                    FROM V_ConsumptionAndPurchaseQtyForChampoPNM VC
                    JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = vc.finishedid 
                    Where VC.ORDERID = " + ddorderno.SelectedValue + @" And VF.MasterCompanyId = " + Session["varCompanyId"] + @"  
                    Group by VF.ITEM_FINISHED_ID, VF.Category_Name, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShadeColorName, VF.ShapeName,VF.Item_Finished_Id, VF.Qualityid,
                    VF.Colorid, VF.designid, VF.shapeid, VF.shadecolorid, VF.category_id, VF.item_id, VF.sizeid, VC.finished_type_id, VC.Unitid 
                    Having  isnull(sum(VC.consumptionqty),0) > isnull(sum(VC.purchaseqty),0) 
                    Order By VF.Category_Name + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName";
                }
                else if (Session["varcompanyno"].ToString() == "6" && chkindentvise.Checked == true)
                {
                    strsql = @"SELECT  Category_Name+'  '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName  Description,sum(Qty) as qty,VF.Item_Finished_Id as finishedid,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,OD.UnitId ,'0' as thanlength,flagsize,Isnull(Rate,0) As IRate,0 As I_FINISHED_Type_ID ,PIM.Remark AS Remark,0 as IWeight,itemremark as itemremark
                FROM PurchaseIndentMaster pim inner join  PurchaseIndentDetail OD on pim.pindentid=od.pindentid Inner JOIN V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id 
                INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id 
                Where pim.pindentid=" + ddindentno.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                Group by Category_Name,VF.ITEM_NAME ,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,VF.Item_Finished_Id ,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,OD.UnitId,flagsize,pim.remark,Rate,itemremark,IPM.DESCRIPTION  ";
                }
                else if (chkcustomervise.Checked == true && Session["varcompanyId"].ToString() == "6")
                {
                    strsql = @" SELECT Category_Name+'  '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName  Description,
                    vd.ITEM_FINISHED_ID,sum(consumptionqty) as qty,sum(purchaseqty),vd.Item_Finished_Id as finishedid,Unitid as UnitId,
                    Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid ,'0' as thanlength,'0' as flagsize,finished_type_id as I_FINISHED_Type_ID ,'' AS Remark,'' as itemremark,0 AS IRate , 0  Iweight
                    FROM V_FinishedItemDetail vd ,V_ConsumptionQtyAndPurchaseQtyForArtIndia vc
                    WHERE  vc.finishedid=vd.ITEM_FINISHED_ID AND vc.ORDERID=" + ddorderno.SelectedValue + @"  And vd.MasterCompanyId=" + Session["varcompanyno"] + @"
                    Group by vd.ITEM_FINISHED_ID,Category_Name,ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,vd.Item_Finished_Id,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid,finished_type_id,Unitid
                    Having  isnull(sum(consumptionqty),0)>isnull(sum(purchaseqty),0) Order By vd.ITEM_FINISHED_ID";
                }
                else if (chkcustomervise.Checked == true && Session["varcompanyId"].ToString() == "42")
                {
                    strsql = @" SELECT Category_Name+'  '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName  Description,
                    vd.ITEM_FINISHED_ID,sum(consumptionqty) as qty,sum(purchaseqty),vd.Item_Finished_Id as finishedid,Unitid as UnitId,
                    Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid ,'0' as thanlength,max(Isizeflag) as flagsize,finished_type_id as I_FINISHED_Type_ID ,'' AS Remark,'' as itemremark,0 AS IRate , 0  Iweight
                    FROM " + view + @" vd ,V_ConsumptionQtyAndPurchaseQtyNew vc
                    WHERE  vc.finishedid=vd.ITEM_FINISHED_ID AND vc.ORDERID=" + ddorderno.SelectedValue + @"  And vd.MasterCompanyId=" + Session["varcompanyno"] + @"
                    Group by vd.ITEM_FINISHED_ID,Category_Name,ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,vd.Item_Finished_Id,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid,finished_type_id,Unitid
                    Having  isnull(sum(consumptionqty),0)>isnull(sum(purchaseqty),0) Order By vd.ITEM_FINISHED_ID";
                }
                else if (Session["varcompanyId"].ToString() != "6" && chkindentvise.Checked == false)
                {
                    strsql = @" SELECT Category_Name+'  '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName  Description,
                    vd.ITEM_FINISHED_ID,sum(consumptionqty) as qty,sum(purchaseqty),vd.Item_Finished_Id as finishedid,Unitid as UnitId,
                    Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid ,'0' as thanlength,max(Isizeflag) as flagsize,finished_type_id as I_FINISHED_Type_ID ,'' AS Remark,'' as itemremark,0 AS IRate , 0  Iweight
                    FROM " + view + @" vd ,V_ConsumptionQtyAndPurchaseQty vc
                    WHERE  vc.finishedid=vd.ITEM_FINISHED_ID AND vc.ORDERID=" + ddorderno.SelectedValue + @"  And vd.MasterCompanyId=" + Session["varcompanyno"] + @"
                    Group by vd.ITEM_FINISHED_ID,Category_Name,ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,vd.Item_Finished_Id,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vd.item_id,sizeid,finished_type_id,Unitid
                    Having  isnull(sum(consumptionqty),0)>isnull(sum(purchaseqty),0) Order By vd.ITEM_FINISHED_ID";

                }
                else if (Session["varcompanyno"].ToString() != "6" && chkindentvise.Checked == true)
                {
                    strsql = @"SELECT  Category_Name+'  '+ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName  Description,sum(Qty) as qty,VF.Item_Finished_Id as finishedid,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,OD.UnitId ,'0' as thanlength,flagsize,Isnull(Rate,0) As IRate,0 As I_FINISHED_Type_ID ,PIM.Remark AS Remark,0 as IWeight,itemremark as itemremark
                FROM PurchaseIndentMaster pim inner join  PurchaseIndentDetail OD on pim.pindentid=od.pindentid Inner JOIN " + view + @" VF ON OD.FinishedId=VF.Item_Finished_Id 
                INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id 
                Where pim.pindentid=" + ddindentno.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                Group by Category_Name,VF.ITEM_NAME ,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,VF.Item_Finished_Id ,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,OD.UnitId,flagsize,pim.remark,Rate,itemremark,IPM.DESCRIPTION  ";
                }

            }
            else if (hncunsp.Value == "1")
            {
                strsql = @"SELECT Category_Name+'  '+VF.ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
                CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End Description,sum(Qty) As Qty,VF.Item_Finished_Id as finishedid,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,sizeid,OD.UNitId ,od.thanlength,0 as flagsize,0 As IRate,0 As I_FINISHED_Type_ID,od.Remark as Remark,0 as IWeight,'' as itemremark, 0  Iweight
                FROM OrderLocalConsumption OD Inner JOIN " + view + @" VF ON OD.FinishedId=VF.Item_Finished_Id 
                INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id 
                Where OrderId=" + ddorderno.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                Group by Category_Name,ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,SizeUnit,SizeMtr,SizeFt,shapeid,shadecolorid,category_id,vf.item_id,sizeid,VF.Item_Finished_Id,Qualityid,designid,VF.COlorid,OD.UNitId,thanlength,od.remark ";
            }
            if (strsql != "")
            {
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
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
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        report();
        ////ViewState["VarCompanyNo"] = Session["varCompanyId"];
        //Report1();
        ReportData();
    }
    private void ReportData()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@PIndentIssueId", ViewState["PIndentIssueId"]);
        array[1] = new SqlParameter("@UserName", Session["UserName"]);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        //array[2] = new SqlParameter("@MasterCompanyId", 2);
        array[3] = new SqlParameter("@UserId", Session["varuserId"]);
        array[4] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[4].Direction = ParameterDirection.Output;
        array[5] = new SqlParameter("@ReportName", Session["ReportPath"]);
        array[6] = new SqlParameter("@DSFileName", SqlDbType.VarChar, 100);
        array[6].Direction = ParameterDirection.Output;

        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1; 

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetPurchaseIndentIssNEW", array);

        Session["dsFileName"] = array[6].Value;
        //switch (Convert.ToInt16(Session["varcompanyId"]))
        //{
        //    case 9:

        //        Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEWForHafizia.xsd";
        //        break;
        //    case 2:               
        //        Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssDestini_1New.xsd";
        //        break;
        //    default:
        //        if (Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtWithoutimgNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssWithoutimgNEW.rpt")
        //        {

        //            Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEW.xsd";
        //        }
        //        else
        //        {                   
        //            Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIss_withoutratNew.xsd";
        //        }
        //        break;
        //}
        if (DDPreviewType.SelectedIndex != 1)
        {
            ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["ImageName"] = img_Byte;
                    }
                }
            }
        }

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
        //Update ReportCount value in MasterTable
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue set ReportCount=isnull(ReportCount,0)+1 where PIndentIssueId=" + ViewState["PIndentIssueId"] + " And MasterCompanyId=" + Session["varcompanyId"] + "");
        //


        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    Session["rptFileName"] = "~\\Reports\\RptForm12EmpSalaryReport.rpt";

        //    Session["GetDataset"] = ds;
        //    Session["dsFileName"] = "~\\ReportSchema\\RptForm12EmpSalaryReport.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //}
    }
    //    private void Report1()
    //    {
    //        string qry = "";
    //        switch (Convert.ToInt16(Session["varcompanyId"]))
    //        {
    //            case 9:
    //                qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo, VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VEI.EmpMobile,
    //                  VEI.EmpFax,VPIR.Challanno,VPIR.Date,VPIR.Destination,VF.ITEM_NAME  AS ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,
    //                  case when VPIR.flagsize=0 then VF.SizeFt when VPIR.flagsize=1 then VF.SizeMtr else VF.SizeInch end as SizeMtr,VPIR.freight,VPIR.FeightRate,VPIR.Remarks,
    //                  VPIR.Quantity,VPIR.LotNo,VPIR.AgentName,VPIR.PIndentIssueTranId,VPIR.PindentIssueid,FT.FINISHED_TYPE_NAME,VPIR.Amount,VPIR.ExciseDuty,
    //                  VPIR.EduCess,VPIR.CST,VPIR.delivery_date,VPIR.PSGST,VPIR.PIGST,VPIR.SGST,VPIR.IGST, cm.customercode,em.empname,VPIR.rate,cmm.customercode as custcode,omm.customerorderno as custorderno,
    //                  omm.localorder as loc ,u.UnitName as UnitId,case When isnull(piid.ImageName,'')='' Then VPIR.ImagePath Else piid.ImageName End As  photo,
    //                  case when VF.Sizeid>0 Then (case when VPIR.flagsize=0 then 'Ft' when VPIR.flagsize=1 then 'Mtr' else 'Inch' end) Else '' ENd  flagsize,T.TermName,P.PaymentName,
    //                  TM.transmodeName,VPIR.Formno,VPIR.vat,vpir.remark,vpir.NetAmount,VPIR.CanQty,
    //                  ALI.Legalname,ALI.RegisterAddress,ALI.TINNo as LITinNo,ALI.CSTNo As LICstNo,isnull(VPIR.CurrencyName,'') as CurrencyName,VPIR.SupplierRef,VPIR.VendorRef,VPIR.TypeofForm,VPIR.Mill
    //                  ," + Session["varcompanyId"] + @" as MastercompanyId,VEI.EmpTiNo,VCI.GSTIN,VEI.EMPGSTIN  FROM V_PIndentIssueReport VPIR INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId Left outer Join AddLegalInformation ALI on ALI.COmpanyId=VCI.CompanyId INNER JOIN 
    //                  V_EmployeeInfo VEI ON VPIR.Partyid=VEI.EmpId INNER JOIN 
    //                  V_FinishedItemDetail VF ON VPIR.FinishedId=VF.ITEM_FINISHED_ID LEFT OUTER JOIN FINISHED_TYPE FT ON VPIR.Finished_type_id=FT.ID left outer join 
    //                  Ordermaster om on om.orderid=VPIR.orderid left outer join Customerinfo cm on cm.customerid=om.customerid left outer join 
    //                  PurchaseIndentMaster pid on pid.pindentid=VPIR.indentid left outer join Empinfo em on em.empid=pid.empid left outer join 
    //                  Customerinfo cmm on pid.customercode=cmm.customerid left outer join Ordermaster omm on pid.orderid=omm.orderid inner join 
    //                  unit u On u.unitid=VPIR.unitid left outer join v_purchaseIndent piid On piid.PIndentId=pid.PIndentId and VPIR.FinishedId=piid.FinishedId Left Outer Join 
    //                  Term T ON VPIR.DeliveryTermid=T.TermId Left Outer Join Payment P ON VPIR.PayementTermId=P.PaymentId Left Outer Join 
    //                  TransMode TM ON VPIR.TranportModeId=TM.transmodeId Where VPIR.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And VPIR.MasterCompanyId=" + Session["varcompanyId"];
    //                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEWForHafizia.xsd";
    //                break;
    //            case 2:
    //                qry = @"SELECT V_PIndentIssueReport_1.customerOrderNo,V_PIndentIssueReport_1.Challanno,V_PIndentIssueReport_1.Date,V_PIndentIssueReport_1.DueDate,V_PIndentIssueReport_1.Remarks,V_PIndentIssueReport_1.Rate,V_PIndentIssueReport_1.Quantity,V_PIndentIssueReport_1.Amount,V_PIndentIssueReport_1.unitid,V_PIndentIssueReport_1.InDescription,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.TinNo,CompanyInfo.Email,EmpInfo.EmpName,EmpInfo.Mobile,V_PIndentIssueReport_1.weight,V_PIndentIssueReport_1.Delivery_Date,V_PIndentIssueReport_1.LocalOrder,OrderMaster.CustomerOrderNo,V_PIndentIssueReport_1.itemremark
    //            ," + Session["varcompanyId"] + @" as MastercompanyId,'' As Photo FROM   V_PIndentIssueReport_1 INNER JOIN CompanyInfo ON V_PIndentIssueReport_1.Companyid=CompanyInfo.CompanyId
    //            INNER JOIN EmpInfo ON V_PIndentIssueReport_1.Partyid=EmpInfo.EmpId
    //            INNER JOIN OrderMaster ON V_PIndentIssueReport_1.Orderid=OrderMaster.OrderId
    //            Where V_PIndentIssueReport_1.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport_1.MasterCompanyId=" + Session["varCompanyId"];
    //                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssDestini_1New.xsd";
    //                break;
    //            default:
    //                if (Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtWithoutimgNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssWithoutimgNEW.rpt")
    //                {
    //                    qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo, VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VEI.EmpMobile,
    //                VEI.EmpFax,VPIR.Challanno,VPIR.Date,VPIR.Destination, VF.CATEGORY_NAME +'   ' + VF.ITEM_NAME  AS ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,
    //                case when VPIR.flagsize=0 then VF.SizeFt when VPIR.flagsize=1 then VF.SizeMtr else VF.SizeInch end as SizeMtr,VPIR.freight,VPIR.FeightRate,VPIR.Remarks,
    //                VPIR.Quantity,VPIR.LotNo,VPIR.AgentName,VPIR.PIndentIssueTranId,VPIR.PindentIssueid,FT.FINISHED_TYPE_NAME,VPIR.Amount,VPIR.ExciseDuty,
    //                VPIR.EduCess,VPIR.CST,VPIR.delivery_date,VPIR.PSGST,VPIR.PIGST,VPIR.SGST,VPIR.IGST,cm.customercode,em.empname,VPIR.rate,cmm.customercode as custcode,omm.customerorderno as custorderno,
    //                omm.localorder as loc ,u.UnitName as UnitId,case When isnull(piid.ImageName,'')='' Then VPIR.ImagePath Else piid.ImageName End As  photo ,
    //                case when Vf.sizeid>0 Then (case when VPIR.flagsize=0 then 'Ft' when VPIR.flagsize=1 then 'Mtr' else 'Inch' end) Else '' End  flagsize,T.TermName,P.PaymentName,
    //                TM.transmodeName,VPIR.Formno,VPIR.vat,vpir.remark,vpir.NetAmount,VPIR.CanQty,VPIR.ReportCount,VPIR.RevisedStatus
    //                ," + Session["varcompanyId"] + @" as MastercompanyId,'" + Session["UserName"] + @"' As Username,OM.CustomerOrderNo As DirectCustOrder,Om.LocalOrder As DirectCustLocalOrder,VF.ProductCode as ProductCode,VEI.EmpAddress2,VEI.EmpAddress3,VCI.CompAddr2,VCI.CompAddr3,VCI.GSTIN,VEI.EMPGSTIN
    //                FROM V_PIndentIssueReport VPIR INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId INNER JOIN 
    //                V_EmployeeInfo VEI ON VPIR.Partyid=VEI.EmpId INNER JOIN 
    //                V_FinishedItemDetail VF ON VPIR.FinishedId=VF.ITEM_FINISHED_ID LEFT OUTER JOIN FINISHED_TYPE FT ON VPIR.Finished_type_id=FT.ID left outer join 
    //                Ordermaster om on om.orderid=VPIR.orderid left outer join Customerinfo cm on cm.customerid=om.customerid left outer join 
    //                PurchaseIndentMaster pid on pid.pindentid=VPIR.indentid left outer join Empinfo em on em.empid=pid.empid left outer join 
    //                Customerinfo cmm on pid.customercode=cmm.customerid left outer join Ordermaster omm on pid.orderid=omm.orderid inner join 
    //                unit u On u.unitid=VPIR.unitid left outer join v_purchaseIndent piid On piid.PIndentId=pid.PIndentId and VPIR.FinishedId=piid.FinishedId Left Outer Join 
    //                Term T ON VPIR.DeliveryTermid=T.TermId Left Outer Join Payment P ON VPIR.PayementTermId=P.PaymentId Left Outer Join 
    //                TransMode TM ON VPIR.TranportModeId=TM.transmodeId Where VPIR.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And VPIR.MasterCompanyId=" + Session["varCompanyId"];
    //                    Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEW.xsd";
    //                }
    //                else
    //                {
    //                    qry = @" SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,
    //                             V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,V_EmployeeInfo.EmpFax,V_PIndentIssueReport.Challanno,V_PIndentIssueReport.Date,V_PIndentIssueReport.Destination,V_FinishedItemDetail.CATEGORY_NAME + '/' +V_FinishedItemDetail.ITEM_NAME As ITEM_NAME,V_FinishedItemDetail.QualityName,V_FinishedItemDetail.designName,V_FinishedItemDetail.ColorName,V_FinishedItemDetail.ShadeColorName,V_FinishedItemDetail.ShapeName,V_FinishedItemDetail.SizeMtr,V_PIndentIssueReport.FeightRate,V_PIndentIssueReport.Remarks,V_PIndentIssueReport.Quantity,V_PIndentIssueReport.LotNo,Payment.PaymentName,V_PIndentIssueReport.AgentName,Term.TermName,V_PIndentIssueReport.PIndentIssueTranId,V_PIndentIssueReport.PindentIssueid,FINISHED_TYPE.FINISHED_TYPE_NAME,
    //                            V_PIndentIssueReport.Amount,V_PIndentIssueReport.ExciseDuty,V_PIndentIssueReport.EduCess,V_PIndentIssueReport.CST,V_PIndentIssueReport.PSGST,V_PIndentIssueReport.PIGST,V_PIndentIssueReport.SGST,V_PIndentIssueReport.IGST,
    //                            CanQty,ReportCount,RevisedStatus
    //                             ," + Session["varcompanyId"] + @" as MastercompanyId,V_FinishedItemDetail.ProductCode as ProductCode,V_PIndentIssueReport.ImagePath As  photo,V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN FROM   V_PIndentIssueReport INNER JOIN V_Companyinfo ON V_PIndentIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN V_EmployeeInfo ON V_PIndentIssueReport.Partyid=V_EmployeeInfo.EmpId INNER JOIN V_FinishedItemDetail ON V_PIndentIssueReport.FinishedId=V_FinishedItemDetail.ITEM_FINISHED_ID LEFT OUTER JOIN Payment ON V_PIndentIssueReport.PayementTermId=Payment.PaymentId LEFT OUTER JOIN Term ON V_PIndentIssueReport.DeliveryTermid=Term.TermId
    //                             LEFT OUTER JOIN FINISHED_TYPE ON V_PIndentIssueReport.Finished_type_id=FINISHED_TYPE.ID
    //                             Where V_PIndentIssueReport.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport.MasterCompanyId=" + Session["varCompanyId"];
    //                    Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIss_withoutratNew.xsd";
    //                }
    //                break;
    //        }

    //        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
    //        SqlDataAdapter sda = new SqlDataAdapter(qry, ErpGlobal.DBCONNECTIONSTRING);
    //        DataSet ds = new DataSet();
    //        sda.Fill(ds);
    //        if (DDPreviewType.SelectedIndex != 1)
    //        {
    //            ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
    //            foreach (DataRow dr in ds.Tables[0].Rows)
    //            {
    //                if (Convert.ToString(dr["Photo"]) != "")
    //                {
    //                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
    //                    if (TheFile.Exists)
    //                    {
    //                        string img = dr["Photo"].ToString();
    //                        img = Server.MapPath(img);
    //                        Byte[] img_Byte = File.ReadAllBytes(img);
    //                        dr["ImageName"] = img_Byte;
    //                    }
    //                }
    //            }
    //        }
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            //ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
    //            Session["rptFileName"] = Session["ReportPath"];
    //            Session["GetDataset"] = ds;
    //            StringBuilder stb = new StringBuilder();
    //            stb.Append("<script>");
    //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=10px, left=0px, scrollbars=1, resizable = yes');</script>");
    //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //        }
    //        //Update ReportCount value in MasterTable
    //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue set ReportCount=isnull(ReportCount,0)+1 where PIndentIssueId=" + ViewState["PIndentIssueId"] + " And MasterCompanyId=" + Session["varcompanyId"] + "");
    //        //
    //    }
    private void Report1Sendpdf()
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
        string qry = "";
        switch (Convert.ToInt16(Session["varcompanyId"]))
        {
            case 9:
                qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo, VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VEI.EmpMobile,
                  VEI.EmpFax,VPIR.Challanno,VPIR.Date,VPIR.Destination,VF.ITEM_NAME  AS ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,
                  case when VPIR.flagsize=0 then VF.SizeFt when VPIR.flagsize=1 then VF.SizeMtr else VF.SizeInch end as SizeMtr,VPIR.freight,VPIR.FeightRate,VPIR.Remarks,
                  VPIR.Quantity,VPIR.LotNo,VPIR.AgentName,VPIR.PIndentIssueTranId,VPIR.PindentIssueid,FT.FINISHED_TYPE_NAME,VPIR.Amount,VPIR.ExciseDuty,
                  VPIR.EduCess,VPIR.CST,VPIR.delivery_date,VPIR.PSGST,VPIR.PIGST,VPIR.SGST,VPIR.IGST,cm.customercode,em.empname,VPIR.rate,cmm.customercode as custcode,omm.customerorderno as custorderno,
                  omm.localorder as loc ,u.UnitName as UnitId,piid.ImageName photo ,
                  case when VPIR.flagsize=0 then 'Ft' when VPIR.flagsize=1 then 'Mtr' else 'Inch' end  flagsize,T.TermName,P.PaymentName,
                  TM.transmodeName,VPIR.Formno,VPIR.vat,vpir.remark,vpir.NetAmount,VPIR.CanQty,
                  ALI.Legalname,ALI.RegisterAddress,ALI.TINNo as LITinNo,ALI.CSTNo As LICstNo,VPIR.CurrencyName,VPIR.SupplierRef,VPIR.VendorRef,VPIR.TypeofForm,VPIR.Mill
                  ," + Session["varcompanyId"] + @" as MastercompanyId   FROM V_PIndentIssueReport VPIR INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId Left outer Join AddLegalInformation ALI on ALI.COmpanyId=VCI.CompanyId INNER JOIN 
                  V_EmployeeInfo VEI ON VPIR.Partyid=VEI.EmpId INNER JOIN 
                  V_FinishedItemDetail VF ON VPIR.FinishedId=VF.ITEM_FINISHED_ID LEFT OUTER JOIN FINISHED_TYPE FT ON VPIR.Finished_type_id=FT.ID left outer join 
                  Ordermaster om on om.orderid=VPIR.orderid left outer join Customerinfo cm on cm.customerid=om.customerid left outer join 
                  PurchaseIndentMaster pid on pid.pindentid=VPIR.indentid left outer join Empinfo em on em.empid=pid.empid left outer join 
                  Customerinfo cmm on pid.customercode=cmm.customerid left outer join Ordermaster omm on pid.orderid=omm.orderid inner join 
                  unit u On u.unitid=VPIR.unitid left outer join v_purchaseIndent piid On piid.PIndentId=pid.PIndentId and VPIR.FinishedId=piid.FinishedId Left Outer Join 
                  Term T ON VPIR.DeliveryTermid=T.TermId Left Outer Join Payment P ON VPIR.PayementTermId=P.PaymentId Left Outer Join 
                  TransMode TM ON VPIR.TranportModeId=TM.transmodeId Where VPIR.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And VPIR.MasterCompanyId=" + Session["varcompanyId"];
                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEWForHafizia.xsd";
                break;
            case 2:
                qry = @"SELECT V_PIndentIssueReport_1.customerOrderNo,V_PIndentIssueReport_1.Challanno,V_PIndentIssueReport_1.Date,V_PIndentIssueReport_1.DueDate,V_PIndentIssueReport_1.Remarks,V_PIndentIssueReport_1.Rate,V_PIndentIssueReport_1.Quantity,V_PIndentIssueReport_1.Amount,V_PIndentIssueReport_1.unitid,V_PIndentIssueReport_1.InDescription,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.TinNo,CompanyInfo.Email,EmpInfo.EmpName,EmpInfo.Mobile,V_PIndentIssueReport_1.weight,V_PIndentIssueReport_1.Delivery_Date,V_PIndentIssueReport_1.LocalOrder,OrderMaster.CustomerOrderNo,V_PIndentIssueReport_1.itemremark
            ," + Session["varcompanyId"] + @" as MastercompanyId FROM   V_PIndentIssueReport_1 INNER JOIN CompanyInfo ON V_PIndentIssueReport_1.Companyid=CompanyInfo.CompanyId
            INNER JOIN EmpInfo ON V_PIndentIssueReport_1.Partyid=EmpInfo.EmpId
            INNER JOIN OrderMaster ON V_PIndentIssueReport_1.Orderid=OrderMaster.OrderId
            Where V_PIndentIssueReport_1.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport_1.MasterCompanyId=" + Session["varCompanyId"];
                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssDestini_1New.xsd";
                break;
            default:
                if (Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtWithoutimgNEW.rpt")
                {
                    qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo, VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VEI.EmpMobile,
                VEI.EmpFax,VPIR.Challanno,VPIR.Date,VPIR.Destination, VF.CATEGORY_NAME +'/' + VF.ITEM_NAME  AS ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,
                case when VPIR.flagsize=0 then VF.SizeFt when VPIR.flagsize=1 then VF.SizeMtr else VF.SizeInch end as SizeMtr,VPIR.freight,VPIR.FeightRate,VPIR.Remarks,
                VPIR.Quantity,VPIR.LotNo,VPIR.AgentName,VPIR.PIndentIssueTranId,VPIR.PindentIssueid,FT.FINISHED_TYPE_NAME,VPIR.Amount,VPIR.ExciseDuty,
                VPIR.EduCess,VPIR.CST,VPIR.delivery_date,VPIR.PSGST,VPIR.PIGST,VPIR.SGST,VPIR.IGST,cm.customercode,em.empname,VPIR.rate,cmm.customercode as custcode,omm.customerorderno as custorderno,
                omm.localorder as loc ,u.UnitName as UnitId,piid.ImageName photo ,
                case when VPIR.flagsize=0 then 'Ft' when VPIR.flagsize=1 then 'Mtr' else 'Inch' end  flagsize,T.TermName,P.PaymentName,
                TM.transmodeName,VPIR.Formno,VPIR.vat,vpir.remark,vpir.NetAmount,VPIR.CanQty,VPIR.ReportCount,VPIR.RevisedStatus
                ," + Session["varcompanyId"] + @" as MastercompanyId,'" + Session["UserName"] + @"' As Username,OM.CustomerOrderNo As DirectCustOrder,Om.LocalOrder As DirectCustLocalOrder FROM V_PIndentIssueReport VPIR INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId INNER JOIN 
                V_EmployeeInfo VEI ON VPIR.Partyid=VEI.EmpId INNER JOIN 
                " + view4 + @" VF ON VPIR.FinishedId=VF.ITEM_FINISHED_ID LEFT OUTER JOIN FINISHED_TYPE FT ON VPIR.Finished_type_id=FT.ID left outer join 
                Ordermaster om on om.orderid=VPIR.orderid left outer join Customerinfo cm on cm.customerid=om.customerid left outer join 
                PurchaseIndentMaster pid on pid.pindentid=VPIR.indentid left outer join Empinfo em on em.empid=pid.empid left outer join 
                Customerinfo cmm on pid.customercode=cmm.customerid left outer join Ordermaster omm on pid.orderid=omm.orderid inner join 
                unit u On u.unitid=VPIR.unitid left outer join v_purchaseIndent piid On piid.PIndentId=pid.PIndentId and VPIR.FinishedId=piid.FinishedId Left Outer Join 
                Term T ON VPIR.DeliveryTermid=T.TermId Left Outer Join Payment P ON VPIR.PayementTermId=P.PaymentId Left Outer Join 
                TransMode TM ON VPIR.TranportModeId=TM.transmodeId Where VPIR.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And VPIR.MasterCompanyId=" + Session["varCompanyId"];
                    Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEW.xsd";
                }
                else
                {
                    qry = @" SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,
                             V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,V_EmployeeInfo.EmpFax,V_PIndentIssueReport.Challanno,V_PIndentIssueReport.Date,V_PIndentIssueReport.Destination,VF.CATEGORY_NAME + '/' +VF.ITEM_NAME As ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,
                            V_PIndentIssueReport.FeightRate,V_PIndentIssueReport.Remarks,V_PIndentIssueReport.Quantity,V_PIndentIssueReport.LotNo,Payment.PaymentName,V_PIndentIssueReport.AgentName,Term.TermName,V_PIndentIssueReport.PIndentIssueTranId,V_PIndentIssueReport.PindentIssueid,FINISHED_TYPE.FINISHED_TYPE_NAME,V_PIndentIssueReport.Amount,V_PIndentIssueReport.ExciseDuty,V_PIndentIssueReport.EduCess,V_PIndentIssueReport.CST,V_PIndentIssueReport.PSGST,V_PIndentIssueReport.PIGST,V_PIndentIssueReport.SGST,V_PIndentIssueReport.IGST,
                            CanQty,ReportCount,RevisedStatus
                              ," + Session["varcompanyId"] + @" as MastercompanyId FROM   V_PIndentIssueReport INNER JOIN V_Companyinfo ON V_PIndentIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN V_EmployeeInfo ON V_PIndentIssueReport.Partyid=V_EmployeeInfo.EmpId INNER JOIN " + view4 + @" VF ON V_PIndentIssueReport.FinishedId=VF.ITEM_FINISHED_ID LEFT OUTER JOIN Payment ON V_PIndentIssueReport.PayementTermId=Payment.PaymentId LEFT OUTER JOIN Term ON V_PIndentIssueReport.DeliveryTermid=Term.TermId
                               LEFT OUTER JOIN FINISHED_TYPE ON V_PIndentIssueReport.Finished_type_id=FINISHED_TYPE.ID
                Where V_PIndentIssueReport.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport.MasterCompanyId=" + Session["varCompanyId"];
                    Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIss_withoutratNew.xsd";
                }
                break;
        }


        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        SqlDataAdapter sda = new SqlDataAdapter(qry, ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        if (Session["varCompanyId"].ToString() == "6" && DDPreviewType.SelectedIndex != 1)
        {
            ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["ImageName"] = img_Byte;
                    }
                }
            }
        }
        if (ds.Tables[0].Rows.Count > 0)
        {

            SendpdfViaMail.sendPdf(ds, rptFilename: "", frommail: "mkmanojkumar70@gmail.com", Tomail: "mkmanojkumar70@gmail.com", OrderId: Convert.ToInt16(ViewState["PIndentIssueId"]));

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        //Update ReportCount value in MasterTable
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue set ReportCount=isnull(ReportCount,0)+1 where PIndentIssueId=" + ViewState["PIndentIssueId"] + " And MasterCompanyId=" + Session["varcompanyId"] + "");
        //
    }
    //protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void gddetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    decimal TotalQty = 0;
    decimal TotalextraQty = 0;
    decimal TotalAmt = 0;
    decimal TotalWeight = 0;
    protected void gddetail_RowDataBound2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtQty = (TextBox)e.Row.FindControl("TXTQTY1");
            TotalQty += Convert.ToDecimal(txtQty.Text);
            TextBox txtextraQty = (TextBox)e.Row.FindControl("TXTextraQTY");
            TotalextraQty += Convert.ToDecimal(string.IsNullOrEmpty(txtextraQty.Text) ? "0" : txtextraQty.Text);
            Label lblTotalAmt = (Label)e.Row.FindControl("lblAmount");
            TotalAmt += Convert.ToDecimal(lblTotalAmt.Text);
            TextBox txtWeight = (TextBox)e.Row.FindControl("TXTweig1");
            TotalWeight += Convert.ToDecimal(txtWeight.Text);

            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            //e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
            lblTotalQty.Text = TotalQty.ToString();
            Label lblextraTotalQty = (Label)e.Row.FindControl("lblextraTotalQty");
            lblextraTotalQty.Text = TotalextraQty.ToString();
            
            Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
            lblTotalAmount.Text = TotalAmt.ToString();
            Label lblTotalWeight = (Label)e.Row.FindControl("lblTotalWeight");
            lblTotalWeight.Text = TotalWeight.ToString();
        }


        if (hncompid.Value == "7")
        {
            gddetail.Columns[4].Visible = false;
            gddetail.Columns[5].Visible = false;
            gddetail.Columns[7].Visible = false;
            gddetail.Columns[8].Visible = false;
            gddetail.Columns[10].Visible = false;
            gddetail.Columns[11].Visible = false;
            gddetail.Columns[12].Visible = false;
        }
        if (hncompid.Value == "10")
        {
            gddetail.Columns[4].Visible = false;

        }
        if (chkindentvise.Checked == true)
        {
            gddetail.Columns[15].Visible = false;
        }

    }
    protected void DGShowConsumption_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(DGShowConsumption.SelectedIndex.ToString());
        string finishedid = ((Label)DGShowConsumption.Rows[r].FindControl("lblfinishedid")).Text;
        string category = ((Label)DGShowConsumption.Rows[r].FindControl("lblcategoryid")).Text;
        string Item = ((Label)DGShowConsumption.Rows[r].FindControl("lblitem_id")).Text;
        string Quality = ((Label)DGShowConsumption.Rows[r].FindControl("lblQualityid")).Text;
        string Color = ((Label)DGShowConsumption.Rows[r].FindControl("lblColorid")).Text;
        string design = ((Label)DGShowConsumption.Rows[r].FindControl("lbldesignid")).Text;
        string shape = ((Label)DGShowConsumption.Rows[r].FindControl("lblshapeid")).Text;
        string shadecolor = ((Label)DGShowConsumption.Rows[r].FindControl("lblshadecolorid")).Text;
        string size = ((Label)DGShowConsumption.Rows[r].FindControl("lblsizeid")).Text;
        string Qty = ((Label)DGShowConsumption.Rows[r].FindControl("lblqty")).Text;
        string lblorderedqty = ((Label)DGShowConsumption.Rows[r].FindControl("lblorderedqty")).Text;
        string thanlength = ((Label)DGShowConsumption.Rows[r].FindControl("thanlength")).Text;
        string flagsize = ((Label)DGShowConsumption.Rows[r].FindControl("flagsize")).Text;
        string remark = ((Label)DGShowConsumption.Rows[r].FindControl("lblmastremark")).Text;
        string Rate = ((Label)DGShowConsumption.Rows[r].FindControl("lblRate")).Text;
        string FinishType = ((Label)DGShowConsumption.Rows[r].FindControl("lblFinishType")).Text;
        string UnitId = ((Label)DGShowConsumption.Rows[r].FindControl("lblUnitId")).Text;

        //string IWeight = ((Label)DGShowConsumption.Rows[r].FindControl("LblIWeight")).Text;
        TxtPreIssueQty.Text = lblorderedqty;
        if (ddCatagory.Visible == true)
        {
            ddCatagory.SelectedValue = category;
            ddlcategorycange();
            ddlcategorychange1();
            if (Session["varCompanyId"].ToString() == "44")
            {
                if (category == "10")
                {
                    tdextra.Visible = true;

                }
                else
                {

                    tdextra.Visible = false;
                }
            }

        }
        if (dditemname.Visible == true)
        {
            dditemname.SelectedValue = Item;
            fillitemchange();
        }
        if (dquality.Visible == true)
        {
            dquality.SelectedValue = Quality;
            TxtProdCode.Text = dquality.SelectedItem.Text;
            Fill_Quantity();
        }
        UtilityModule.ConditionalComboFill(ref ddFinish_Type, "select Distinct Id,FINISHED_TYPE_NAME from FINISHED_Type Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        if (dddesign.Visible == true)
        {
            dddesign.SelectedValue = design;
        }
        if (ddcolor.Visible == true)
        {
            ddcolor.SelectedValue = Color;
        }
        if (ddlshade.Visible == true)
        {
            ddlshade.SelectedValue = shadecolor;
        }
        if (chkcustomervise.Checked == true)
        {
            if (ddlshadeNew.Visible == true)
            {
                ddlshadeNew.SelectedValue = shadecolor;
            }
        }
        if (ddshape.Visible == true)
        {
            ddshape.SelectedValue = shape;
            DDsizetype.SelectedValue = flagsize;
            FillShapeSelectedChange();
        }
        if (ddsize.Visible == true)
        {
            ddsize.SelectedValue = size;
        }
        if (ddFinish_Type.Visible == true)
        {
            ddFinish_Type.SelectedValue = FinishType;
        }
        if (ddlunit.Visible == true)
        {
            ddlunit.SelectedValue = UnitId;
        }
        TxtItemRemark.Text = ((Label)DGShowConsumption.Rows[r].FindControl("lblitemremark")).Text;
        if (chkcustomervise.Checked == true)
        {
            //DataSet ds11 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(min(IRATE),0) as rate,isnull(min(IWEIGHT),0) as iweight  from ORDER_CONSUMPTION_DETAIL where orderid=" + ddorderno.SelectedValue + " and IFINISHEDID=" + finishedid + " ");
            DataSet ds11 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(IRATE),0) as rate,isnull(max(IWEIGHT),0) as iweight  from ORDER_CONSUMPTION_DETAIL where orderid=" + ddorderno.SelectedValue + " and IFINISHEDID=" + finishedid + " ");
            TxtRate.Text = ds11.Tables[0].Rows[0]["rate"].ToString();
            txtweig.Text = ds11.Tables[0].Rows[0]["iweight"].ToString();
        }
        else
        {
            TxtRate.Text = Rate;
        }
        txtremarks.Text = remark;
        if (TxtRate.Text == "")
        {
            TxtRate.Text = "0";
        }
        TxtAmount.Text = (Convert.ToDecimal(Qty) * Convert.ToDecimal(TxtRate.Text)).ToString();

        double qn = Convert.ToDouble(Convert.ToDouble(Qty) - Convert.ToDouble(lblorderedqty));
        if (qn > 0)
        {
            txtqty.Text = Math.Round(qn, 3).ToString();
        }
        else
        {
            txtqty.Text = "0";
        }
        hnqty.Value = txtqty.Text;
        Txtthanlength.Text = thanlength;

        if (chkindentvise.Checked == true)
        {
            TxtApprovedQty.Text = Qty;
            if (TxtPreIssueQty.Text == "")
                TxtPreIssueQty.Text = lblorderedqty;
            txtqty.Text = Convert.ToString(Convert.ToDouble(TxtApprovedQty.Text) - Convert.ToDouble(TxtPreIssueQty.Text));
        }
        if (Session["varcompanyno"].ToString() == "7")
        {
            Label4.Text = "Fabric Remark : " + remark;
        }
    }
    protected void DGShowConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowConsumption, "Select$" + e.Row.RowIndex);
        }
        e.Row.Cells[2].Visible = true;
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
    }
    protected void txtcanqnt_changed(object sender, EventArgs e)
    {
        int RowIndex = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;
        string ppid = ((Label)gddetail.Rows[RowIndex].FindControl("lbldetailid")).Text;
        string str = @"select PT.PindentIssueId,PT.PIndentIssueTranid,ROUND(Sum(Quantity),3) As issueQty,ROUND(Isnull(Sum(CanQty),0),3) As CanQty,ROUND(Isnull(Sum(Qty-isnull(ReturnQty,0)),0),3) As RecQty  from PurchaseIndentIssueTran PT Left Outer Join PurchaseReceivedetail PR 
                         On PT.Pindentissuetranid=PR.PIndentIssueTranId left outer join V_PurchaseReturnDetail V on V.PurchaseReceiveDetailId=PR.PurchaseReceiveDetailId
                         Where PT.PindentIssueTranId=" + ppid + " group  by PT.PindentIssueId,PT.PIndentIssueTranid";
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(qty),0) from PurchaseReceiveDetail where pindentissuetranid=" + ppid + "");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string qty = ((TextBox)gddetail.Rows[RowIndex].FindControl("TXTQTY1")).Text;
            string canqty = ((TextBox)gddetail.Rows[RowIndex].FindControl("Txtcancel")).Text == "" ? "0" : ((TextBox)gddetail.Rows[RowIndex].FindControl("Txtcancel")).Text;

            //int tot = Convert.ToInt32(qty) - (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) + Convert.ToInt32(canqty));
            Double tot = Convert.ToDouble(qty) - Convert.ToDouble(ds.Tables[0].Rows[0]["RecQty"]);
            //if (tot < 0)
            //{
            //    ((TextBox)gddetail.Rows[i].FindControl("Txtcancel")).Text = "0";
            //    Label3.Visible = true;
            //    Label3.Text = "PLZ fill Correct Cancel Qty";
            //}
            //else
            //{
            //    Label3.Visible = false;
            //    Label3.Text = "";
            //}
            if (Convert.ToDouble(canqty) > tot || Convert.ToDouble(canqty) < 0)
            {
                ((TextBox)gddetail.Rows[RowIndex].FindControl("Txtcancel")).Text = ds.Tables[0].Rows[0]["CanQty"].ToString();
                Label3.Visible = true;
                Label3.Text = "Plz fill Correct Qty.Qty can not be greater than " + tot + ".";
            }
            else
            {
                Label3.Visible = false;
                Label3.Text = "";

            }
        }
    }
    public string getgiven(string strval, string strval1)
    {
        string val = "0";
        DataSet ds;
        if (chkcustomervise.Checked == true)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(quantity-IsNull(piit.CanQty, 0)),0) from PurchaseIndentIssueTran piit,PurchaseIndentIssue pii where pii.PindentIssueid=piit.PindentIssueid and  finishedid=" + strval + " and Finished_Type_Id=" + strval1 + " and piit.orderid=" + ddorderno.SelectedValue + " And pii.MasterCompanyid=" + Session["varCompanyId"] + "");
            val = ds.Tables[0].Rows[0][0].ToString();
        }
        if (chkindentvise.Checked == true)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(quantity-IsNull(piit.CanQty, 0)),0) from PurchaseIndentIssueTran piit,PurchaseIndentIssue pii where pii.PindentIssueid=piit.PindentIssueid and  finishedid=" + strval + "  and  piit.pindentId=" + ddindentno.SelectedValue + " And pii.MasterCompanyid=" + Session["varCompanyId"] + "");
            val = ds.Tables[0].Rows[0][0].ToString();
        }
        return val;
    }
    protected void refreshEmp_Click(object sender, EventArgs e)
    {
        //string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.DepartmentId Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";

        string Str = "select EI.empid,EI.empname+(case when EI.Empcode<>'' Then  '('+Ei.empcode+')' Else '' End) as empname from empinfo EI inner join Department DM on EI.Departmentid=DM.DepartmentId Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
        if (Session["varCompanyno"].ToString() != "6")
        {
            Str = Str + "  AND DM.DepartmentName='PURCHASE'";
        }
        Str = Str + "  Group By EI.empid,EI.empname,EI.Empcode";
        Str = Str + "  Order By EI.empname ";
        UtilityModule.ConditionalComboFill(ref ddempname, Str, true, "--Select Party--");
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from purchaseindentissue where  pindentissueid=" + Convert.ToInt32(txtpindentissueid.Text));
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from purchaseindentissuetran where pindentissuetranid=" + Convert.ToInt32(txtpindentissuedetailid.Text));
        Fill_Grid();
    }
    protected void BtnOrderComplete_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue Set Status='Complete' Where PIndentIssueID=" + ddlchalanno.SelectedValue);
        refresh_form();
    }
    private void fill_grid_show()
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
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,(isnull(Sum(consumptionqty),0)-isnull(sum(purchaseqty),0)) as qty
        from V_ConsumptionQtyAndPurchaseIndentQty vc inner join " + view5 + @" v On vc.finishedid=v.ITEM_FINISHED_ID
        where orderid=" + ddorderno.SelectedValue + @"
        group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId
        having  round(isnull(Sum(consumptionqty),0),2)>round(isnull(sum(purchaseqty),0),2)");
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
    protected void BtnOrderUnComplete_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PurchaseIndentIssue Set Status='Pending' Where PIndentIssueID=" + DDPoNoNew.SelectedValue);
        EmpNameSelectedChange();
    }
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        if (ddempname.Items.Count > 0)
        {
            ddempname.SelectedIndex = 0;
        }
        if (ddcustomercode.Items.Count > 0)
        {
            ddcustomercode.SelectedIndex = 0;
        }
        GetDeliveryAddress();
    }
    protected void chkotherinformation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkotherinformation.Checked == true)
        {
            DivOtherInformation.Visible = true;
        }
        else
        {
            DivOtherInformation.Visible = false;
        }
    }
    protected void BtnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            report();
            // ViewState["VarCompanyNo"] = Session["varCompanyId"];
            Report1Sendpdf();
        }
        catch (Exception ex)
        {
            Label1.Visible = true;
            Label1.Text = ex.Message;
        }
    }
    protected void gddetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddImage")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../Carpet/AddPhotoRefImage1.aspx?SrNo=" + gddetail.DataKeys[rowindex].Value + "&img=pp&PPI=yes', 'nwwin', 'toolbar=0, titlebar=1,  top=200px, left=100px, scrollbars=1, resizable = yes,width=550px,Height=200px');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    protected void chkforSms_CheckedChanged(object sender, EventArgs e)
    {
        if (chkforSms.Checked == true)
        {
            Btnforsms.Visible = true;
        }
        else
        {
            Btnforsms.Visible = false;
        }
    }
    protected void Btnforsms_Click(object sender, EventArgs e)
    {
        try
        {
            Btnforsms.Enabled = false;
            if (Convert.ToString(ViewState["PIndentIssueId"]) != "" || Convert.ToString(ViewState["PIndentIssueId"]) != "0")
            {
                UtilityModule.SendmessageToWeaver_Vendor_Finisher(MasterTableName: "PurchaseIndentIssue", DetailTable: "PurchaseIndentIssueTran", UniqueColName: "PIndentIssueId", EmpIdColName: "Partyid", OrderId: Convert.ToInt64(ViewState["PIndentIssueId"]), OrderNo: "PIndentIssueId", MasterCompanyId: Convert.ToInt16(Session["varcompanyId"]), FinishedidColName: "Finishedid", QtyCOlName: "quantity", ReqByDate: "Delivery_Date", JobName: "Purchase");
            }
            Btnforsms.Enabled = true;
            lblerrormessage.Visible = true;
            lblerrormessage.Text = "Message Sent Successfully";
        }
        catch (Exception ex)
        {
            lblerrormessage.Visible = true;
            lblerrormessage.Text = ex.Message;
        }
    }
    protected void chkforsample_CheckedChanged(object sender, EventArgs e)
    {
        if (chkforsample.Checked)
        {
            TxtIndentNo.Text = "";
            chkindentvise.Checked = false;
            ddcustomercode.Focus();

        }
        OnCheckedChange();
    }
    protected void getOrderNo()
    {
        int maxid = 0;
        string str = "";
        string maxchallanid = "";
        string Date = "";
        DataSet ds;
        if (variable.VarGeneratePurchaseOrderChallanNoCompWise == "1")
        {
            str = "select  isnull(ChallanNo,0) as Issueid,getdate() as date from purchaseindentissue where Companyid=" + ddCompName.SelectedValue + " order by PindentIssueid desc";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                maxchallanid = ds.Tables[0].Rows[0]["Issueid"].ToString();
                string result = maxchallanid.Substring(0, maxchallanid.IndexOf("/"));
                var output = Regex.Replace(result, "[^0-9]+", string.Empty);
                maxid = Convert.ToInt32(output) + 1;
                Date = ds.Tables[0].Rows[0]["date"].ToString();
            }
            else
            {
                maxid = 1;
                Date = DateTime.Now.ToString();
            }

        }
        else
        {
            str = "select isnull(max(pindentissueid),0)+1 as Issueid,getdate() as date from purchaseindentissue";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            maxid = Convert.ToInt32(ds.Tables[0].Rows[0]["Issueid"]);
            Date = ds.Tables[0].Rows[0]["date"].ToString();

        }

    Line:
        string code = "";
        switch (Session["varcompanyId"].ToString())
        {
            case "20":
                code = ViewState["purchasecode"].ToString() + maxid.ToString();
                break;
            case "30":
                code = ViewState["purchasecode"].ToString() + maxid.ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(Date), separator: "-", fullyear: true);
                break;
            default:
                code = ViewState["purchasecode"].ToString() + maxid.ToString() + "/" + UtilityModule.GetFinancialYear(Convert.ToDateTime(Date));
                break;
        }

        str = "select Challanno From purchaseindentissue Where Challanno='" + code + "' and CompanyId=" + ddCompName.SelectedValue + "";
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            maxid = maxid + 1;
            goto Line;
        }
        txtchalanno.Text = code;
        ds.Dispose();
    }
    private void FillGSTIGST()
    {
        Label1.Visible = false;
        Label1.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ProcessId", "9");
            param[1] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            param[2] = new SqlParameter("@ItemId", dditemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[4] = new SqlParameter("@EffectiveDate", txtdate.Text);
            param[5] = new SqlParameter("@GSTType", DDGSType.SelectedValue);
            param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@CompanyID", ddCompName.SelectedValue);
            param[10] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

            if (DDGSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
                {
                    txtCGST.Text = param[6].Value.ToString();
                    txtSGST.Text = param[7].Value.ToString();
                    txtIGST.Text = param[8].Value.ToString();
                    fill_text();
                }
                else
                {
                    txtCGST.Text = "0";
                    txtSGST.Text = "0";
                    txtIGST.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add GST/IGST regarding selected item";
                    return;
                }
            }
            else
            {
                txtCGST.Text = "0";
                txtSGST.Text = "0";
                txtIGST.Text = "0";
                fill_text();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrormessage.Text = ex.Message;
            con.Close();
        }
    }
    protected void DDGSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDGSType.SelectedValue == "1")
        {
            TDCGST.Visible = true;
            TDSGST.Visible = true;
            TDIGST.Visible = false;
            FillGSTIGST();
        }
        else if (DDGSType.SelectedValue == "2")
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = true;
            FillGSTIGST();
        }
        else
        {
            TDCGST.Visible = false;
            TDSGST.Visible = false;
            TDIGST.Visible = false;
            txtCGST.Text = "0";
            txtSGST.Text = "0";
            txtIGST.Text = "0";
            fill_text();
        }
    }
    protected void BtnChangeVendorName_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label1.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //ViewState["order_id"]
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@PIndentIssueId", ddlchalanno.SelectedValue);
            param[1] = new SqlParameter("@OldEmpId", ddempname.SelectedValue);
            param[2] = new SqlParameter("@EmpId", DDlegalvendor.SelectedValue);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEPURCHASEVENDOR", param);
            Label1.Text = param[5].Value.ToString();
            Label1.Visible = true;
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void BtnUpdateGSTType_Click(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label1.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //ViewState["order_id"]
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@PIndentIssueId", ddlchalanno.SelectedValue);
            param[1] = new SqlParameter("@GSTType", DDGSType.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEPURCHASEORDER_GSTTYPE", param);
            Label1.Text = param[4].Value.ToString();
            Label1.Visible = true;
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
            Label1.Visible = true;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        fillitemchange();
    }
    protected void txtTCS_TextChanged(object sender, EventArgs e)
    {
        fill_text();
    }
    private void CheckTCSType()
    {
        Label1.Visible = false;
        Label1.Text = "";
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Pindentissuetranid,TCSType from PurchaseIndentIssueTran where PindentIssueid=" + ViewState["PIndentIssueId"] + "  order by Pindentissuetranid ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (DDTCSType.SelectedValue != Ds.Tables[0].Rows[0]["TCSType"].ToString())
            {
                Label1.Visible = true;
                Label1.Text = "Please select same TCS Type";
                return;
            }
        }
        FillTCS();
    }
    private void FillTCS()
    {
        Label1.Visible = false;
        Label1.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@ProcessId", "9");
            param[1] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            param[2] = new SqlParameter("@ItemId", dditemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[4] = new SqlParameter("@EffectiveDate", txtdate.Text);
            param[5] = new SqlParameter("@TCSType", DDTCSType.SelectedValue);
            param[6] = new SqlParameter("@TCSRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Get_TCS_Rate", param);

            if (DDTCSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "")
                {
                    txtTCS.Text = param[6].Value.ToString();
                    fill_text();
                }
                else
                {
                    txtTCS.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add TCS regarding selected item";
                    return;
                }
            }
            else
            {
                txtTCS.Text = "0";
                fill_text();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrormessage.Text = ex.Message;
            con.Close();
        }
    }
    protected void DDTCSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDTCSType.SelectedValue == "1")
        {
            TDTCS.Visible = true;
            FillTCS();
        }
        else
        {
            //DDTCSType.SelectedIndex = 0;
            TDTCS.Visible = false;
            txtTCS.Text = "0";
            fill_text();
        }
    }
    protected void ChkForForSampleFlag_CheckedChanged(object sender, EventArgs e)
    {
        tdcustomer.Visible = false;
        if (ChkForForSampleFlag.Checked)
        {
            tdcustomer.Visible = true;
            TxtIndentNo.Text = "";
            UtilityModule.ConditionalComboFill(ref ddcustomercode, @"Select Distinct OM.CustomerID, CI.CustomerCode + SPACE(5) + CI.CompanyName 
                From OrderMaster OM(Nolock)
                JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId And CI.MasterCompanyId = " + Session["varCompanyId"] + @" 
                Where OM.CompanyId = " + ddCompName.SelectedValue + @" 
                Order By CI.CustomerCode + SPACE(5) + CI.CompanyName ", true, "Select CustomerCode");

            ddcustomercode.Focus();
        }
    }

    private void FillPurchaseRate()
    {
        Label65.Visible = false;
        Label65.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@ProcessId", "9");
            param[1] = new SqlParameter("@CategoryId", ddCatagory.SelectedValue);
            param[2] = new SqlParameter("@ItemId", dditemname.SelectedValue);
            param[3] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[4] = new SqlParameter("@EffectiveDate", txtdate.Text);
            param[5] = new SqlParameter("@EmpId", ddempname.SelectedValue);
            param[6] = new SqlParameter("@PurchaseRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;           
            param[7] = new SqlParameter("@CompanyID", ddCompName.SelectedValue);
            param[8] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetPurchaseRateFromMaster", param);

           if(variable.VarGetPurchaseRateFromMaster=="1")
           {
                if (param[6].Value.ToString() != "")
                {
                    TxtRate.Text = param[6].Value.ToString();
                    TxtRate.Enabled = false; 
                    //fill_text();
                }
                else
                {
                    TxtRate.Text = "0";
                    TxtRate.Enabled = false; 
                    Label65.Visible = true;
                    Label65.Text = "Please Add Purchase Rate in Master Regarding Selected Item";
                    return;
                }
            }
            else
            {
                TxtRate.Text = "0";                
                //fill_text();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrormessage.Text = ex.Message;
            con.Close();
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@indentissueid", ddlchalanno.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@Userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CANCELPRODUCTIONORDER", param);
            //******
            if (param[1].Value.ToString() != "")
            {
                lblerrormessage.Text = param[1].Value.ToString();
            }
            else
            {
                lblerrormessage.Text = "PO canceled successfully.";
                gddetail.DataSource = null;
                gddetail.DataBind();
                //*****************************
                DGShowConsumption.DataSource = null;
                DGShowConsumption.DataBind();
                ddlchalanno.SelectedIndex = -1;
             //   ddlchalanno.Items.Clear();
            }
            lblerrormessage.Visible = true;
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerrormessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
            EmpNameSelectedChange(sender);
        }



    }
}