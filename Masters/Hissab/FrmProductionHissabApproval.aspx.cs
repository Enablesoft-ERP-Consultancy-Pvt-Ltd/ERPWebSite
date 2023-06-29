using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class Masters_Hissab_FrmProductionHissabApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                           Select distinct Process_Name_Id,Process_Name from PROCESS_NAME_MASTER(Nolock) Where MasterCompanyId=" + Session["varCompanyId"] + @" 
                           --UNION
                           --SELECT 999 AS PROCESS_NAME_ID,'YARN OPENING' Process_name 
                            Order By Process_Name";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                //    DDCompanyName.SelectedIndex = 1;
            }
            ViewState["ID"] = 0;
            TxtAppDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            lblErr.Text = "";
            if (Session["canedit"].ToString() == "1")
            {
                ChkEdit.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDProcessName.SelectedItem.Text.ToUpper())
        {
            case "HANK MAKING":
            case "HAND SPINNING":
                ViewState["processtype"] = "999";
                break;
            case "MOTTELING":
            case "YARN OPENING+MOTTELING":
                ViewState["processtype"] = "999";
                break;
            case "YARN OPENING":
            case "TASSEL MAKING":
                ViewState["processtype"] = "999";
                break;
            case "FILLAR MOUTH CLOSING":
            case "FILLER BHARAI":
            case "FILLER CUTTING":
            case "FILLER FILLING+MOUTH CLOSING":
            case "FILLER JOB WORK":
            case "FILLER MAKING":
            case "FILLER PALTI":
            case "LABEL TAGGING":
            case "PANEL MAKING":
            case "PANEL PRESS":
                ViewState["processtype"] = "9999";
                break;
            default:
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select processtype from process_name_master where process_name_id=" + DDProcessName.SelectedValue + " ");
                ViewState["processtype"] = ds1.Tables[0].Rows[0][0].ToString();
                break;
        }

        if (DDProcessName.SelectedIndex > 0)
        {
            ProcessNameSelectedIndexChanged();
        }
        ViewState["ID"] = 0;
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string Str = "";
            if (ViewState["processtype"].ToString() == "2") // 9 For Purchase
            {
                Str = "Select Distinct E.EmpId,E.EmpName From EmpInfo E,EmpProcess EP,PurchaseHissab PH Where E.Empid=EP.Empid And E.Empid=PH.PartyId And EP.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varCompanyId"] + " Order By E.EmpName";
            }
            else if (ViewState["processtype"].ToString() == "0") // 5 For RawMaterial PreParation
            {
                Str = "Select Distinct E.EmpId,E.EmpName From EmpInfo E,EmpProcess EP,RawMaterialPreprationHissab RH where E.Empid=EP.Empid And E.Empid=RH.PartyId And CompanyId=" + DDCompanyName.SelectedValue + " And EP.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varCompanyId"] + " Order By E.EmpName";
            }
            else if (ViewState["processtype"].ToString() == "999")
            {
                Str = @"Select Distinct E.EmpId,E.EMPNAME + CASE WHEN ISNULL(E.EMPCODE,'')<> '' THEN  ' ['+E.EMPCODE+']' ELSE '' END AS EMPNAME 
                From EmpInfo E,RAWMATERIALPROCESSHISSABMASTER RH 
                Where E.Empid=RH.empid And CompanyId=" + DDCompanyName.SelectedValue + " And Rh.ProcessId=" + DDProcessName.SelectedValue + @" And 
                E.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName";

                if (DDProcessName.SelectedItem.Text.ToUpper() == "YARN OPENING")
                {
                    Str = @"Select Distinct E.EmpId,E.EMPNAME + CASE WHEN ISNULL(E.EMPCODE,'')<> '' THEN  ' ['+E.EMPCODE+']' ELSE '' END AS EMPNAME 
                From EmpInfo E,RAWMATERIALPROCESSHISSABMASTER RH 
                Where E.Empid=RH.empid And CompanyId=" + DDCompanyName.SelectedValue + @" And Rh.ProcessId = 999 And 
                E.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName";
                }
            }
            else if (ViewState["processtype"].ToString() == "9999") // Filler Type Process 
            {
                Str = @"Select Distinct E.EmpId, E.EmpName 
                    From HomeFurnishingMakingHissab RH(Nolock) 
                    JOIN EmpInfo E(Nolock) ON E.Empid = RH.EmpID 
                    JOIN EmpProcess EP(Nolock) ON EP.Empid = E.Empid And EP.ProcessId = " + DDProcessName.SelectedValue + @" 
                    Where CompanyId = " + DDCompanyName.SelectedValue + @" And 
                    RH.ProcessId = " + DDProcessName.SelectedValue + " And RH.MasterCompanyId = " + Session["varCompanyId"] + @" 
                    Order By E.EmpName ";
            }
            else
            {
                if (variable.VarFinishingNewModuleWise == "1")
                {
                    if (DDProcessName.SelectedValue == "1")
                    {
                        switch (Session["varcompanyId"].ToString())
                        {
                            case "28":
                            case "22":
                                Str = @"SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID WHERE PIM.Companyid=" + DDCompanyName.SelectedValue + @"
                                UNION
                                SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM  ON EMP.ISSUEORDERID=PIM.ISSUEORDERID AND EMP.PROCESSID=" + DDProcessName.SelectedValue + @"
                                INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID WHere PIm.companyid=" + DDCompanyName.SelectedValue + " ORDER BY EMPNAME";
                                break;
                            default:
                                Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,EMpInfo EI Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName";
                                break;
                        }
                    }
                    else
                    {
                        Str = @"select Distinct EI.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From Employee_ProcessOrderNo EMP inner join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM on EMP.IssueOrderId=PIM.IssueOrderId
                            inner join EmpInfo EI on EMP.Empid=EI.EmpId Where  EMP.ProcessId=" + DDProcessName.SelectedValue + " and PIM.Companyid= " + DDCompanyName.SelectedValue + " order by Empname";
                    }
                }
                else
                {
                    Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,EMpInfo EI Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName";
                }
            }
            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        }
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGBillDetail.DataSource = null;
        DGBillDetail.DataBind();
        PartyNameSelectedChange();
        FillGrid();
        ViewState["ID"] = 0;
    }
    protected void FillGrid()
    {
        string str, amount;
        if (ViewState["processtype"].ToString() == "2")
        {
            if (Session["varCompanyId"].ToString() == "4")
            {
                amount = "Case When Amount > BillAmt Then BillAmt ELse Amount End Amt";
            }
            else
            {
                amount = "Round(Amount, 0) Amt";
            }
            str = @"Select Distinct Phissabid Hissabid,cast(BillNo As Nvarchar)+' / '+replace(convert(varchar(11),Date,106), ' ','-') As BillNo,
                    " + amount + @", 0 Flag,0 As TDS,0 as Gst,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From PurchaseHissab where companyid=" + DDCompanyName.SelectedValue + " And PartyId=" + DDEmployerName.SelectedValue + " And  MasterCompanyId=" + Session["varCompanyId"] + @" And
                    Phissabid not in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And 
                    CompanyId=" + DDCompanyName.SelectedValue + " And HissabType=2 And ProcessId=" + DDProcessName.SelectedValue + @" And EmpId=" + DDEmployerName.SelectedValue + ")"; //2 For Purchase,0 For OtherProcess
            if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
            {
                str = str + @" Union Select Distinct Phissabid Hissabid,cast(BillNo As Nvarchar)+' / '+replace(convert(varchar(11),Date,106), ' ','-') As BillNo,
                    " + amount + @",1 Flag,0 As TDS,0 as gst,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From PurchaseHissab where companyid=" + DDCompanyName.SelectedValue + " And PartyId=" + DDEmployerName.SelectedValue + @" And 
                    Phissabid in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And PHA.ID=" + DDApprovalNo.SelectedValue + ") And MasterCompanyId=" + Session["varCompanyId"];
            }
            str = str + " Order By Phissabid";
        }
        else if (ViewState["processtype"].ToString() == "0") // 5 For RawMaterial PreParation
        {
            str = @"Select Distinct HissabId,cast(BillNo As Nvarchar)+' / '+replace(convert(varchar(11),Date,106), ' ','-') As BillNo,Round(Amount,0) Amt,0 Flag,0 As TDS,isnull(gst,0) as gst
                    ,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From RawMaterialPreprationHissab 
                    Where Companyid=" + DDCompanyName.SelectedValue + " And PartyId=" + DDEmployerName.SelectedValue + @" And 
                    HissabId not in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And 
                    CompanyId=" + DDCompanyName.SelectedValue + " And HissabType=3 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + ") And MasterCompanyId=" + Session["varCompanyId"];
            if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
            {
                str = str + @" Union Select Distinct HissabId,cast(BillNo As Nvarchar)+' / '+replace(convert(varchar(11),Date,106), ' ','-') As BillNo,Round(Amount,0) Amt,1 Flag,0 As TDS,isnull(gst,0) as gst 
                    ,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From RawMaterialPreprationHissab 
                    Where Companyid=" + DDCompanyName.SelectedValue + " And PartyId=" + DDEmployerName.SelectedValue + @" And 
                    HissabId in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And 
                    CompanyId=" + DDCompanyName.SelectedValue + " And HissabType=3 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And PHA.ID=" + DDApprovalNo.SelectedValue + ") And MasterCompanyId=" + Session["varCompanyId"];
            }
            str = str + " Order By HissabId";
        }
        else if (ViewState["processtype"].ToString() == "999") // 5 For RawMaterial PreParation
        {
            str = @"Select Distinct HissabId,BillNo+' / '+replace(convert(varchar(11),billDate,106), ' ','-') As BillNo,
                    isnull(round(Round(TotalAmount,0)-isnull(Round(DeductionAmt,0),0)+isnull(Round(AdditionAmt,0),0),0),0) as Amt,0 Flag,0 As TDS,0 as gst 
                    ,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From RAWMATERIALPROCESSHISSABMASTER 
                    Where Companyid=" + DDCompanyName.SelectedValue + " And empid=" + DDEmployerName.SelectedValue + @" And 
                    HissabId not in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And 
                    CompanyId=" + DDCompanyName.SelectedValue + " And HissabType=999 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + ") And MasterCompanyId=" + Session["varCompanyId"];
            if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
            {
                str = str + @" Union Select Distinct HissabId,BillNo+' / '+replace(convert(varchar(11),billDate,106), ' ','-') As BillNo,
                     isnull(round(Round(TotalAmount,0)-isnull(Round(DeductionAmt,0),0)+isnull(Round(AdditionAmt,0),0),0),0) as Amt,1 Flag,0 As TDS,0 as gst 
                    ,0 as AdvanceAmountFolioWise,0 as AdditionAmt,0 as DeductionAmt,0 as MaterialDeductionAmt
                    From RAWMATERIALPROCESSHISSABMASTER 
                    Where Companyid=" + DDCompanyName.SelectedValue + " And empid=" + DDEmployerName.SelectedValue + @" And 
                    HissabId in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And 
                    CompanyId=" + DDCompanyName.SelectedValue + " And HissabType=999 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And PHA.ID=" + DDApprovalNo.SelectedValue + ") And MasterCompanyId=" + Session["varCompanyId"];
            }
            str = str + " Order By HissabId";
        }
        else if (ViewState["processtype"].ToString() == "9999")
        {

            str = @"Select Distinct HissabNo HissabId,Replace(Str(HissabNo)+' / '+IsNull(replace(convert(varchar(11),Date,106), ' ','-'),''),'  ','') BillNo,
                        isnull(ROUND(ROUND(SUM(amount),2)-ROUND(SUM(Penality),0),2),0) amt, 0 Flag,
                        isnull(TdsPercetage,0) TDS, 0 gst, 0 AdvanceAmountFolioWise, 0 AdditionAmt,0 DeductionAmt,0 MaterialDeductionAmt
                        From HomeFurnishingMakingHissab PH(Nolock) 
                        where PH.companyid = " + DDCompanyName.SelectedValue + " and PH.processid=" + DDProcessName.SelectedValue + " and PH.EmpId=" + DDEmployerName.SelectedValue + @" and 
                        HissabNo not in(Select HissabId From ProcessHissabApproved PHA(Nolock),ProcessHissabApprovedDetail PHAD(Nolock) 
	                    Where PHA.ID=PHAD.ID And HissabType=0 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + @") 
                        Group By PH.HissabNo,PH.Date,PH.EmpId, PH.TdsPercetage,PH.ProcessID,PH.FROMDATE,PH.TODATE";
            if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
            {
                str = str + @" Union Select Distinct HissabNo HissabId, Replace(Str(HissabNo) + ' / ' + IsNull(replace(convert(varchar(11), Date, 106), ' ', '-'),''), '  ', '') BillNo,
                        isnull(ROUND(ROUND(SUM(amount), 0) + - ROUND(SUM(Penality), 0), 0), 0) Amt, 
                        1 Flag, TdsPercetage TDS, 0 gst, 0 AdvanceAmountFolioWise, 0 AdditionAmt, 0 DeductionAmt, 0 MaterialDeductionAmt 
                        From HomeFurnishingMakingHissab(Nolock) 
                        Where companyid = " + DDCompanyName.SelectedValue + " And processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + @" And 
                        HissabNo in(Select HissabId From ProcessHissabApproved PHA(Nolock)
                        JOIN ProcessHissabApprovedDetail PHAD(Nolock) ON PHAD.ID = PHA.ID 
                        Where PHA.HissabType = 0 And ProcessId = " + DDProcessName.SelectedValue + " And EmpId = " + DDEmployerName.SelectedValue + @" And PHA.ID = " + DDApprovalNo.SelectedValue + @") 
                        Group By HissabNo, Date, TdsPercetage ";
            }
        }
        else
        {
            if (Session["VarCompanyNo"].ToString() == "42")
            {
                str = @"Select Distinct PH.ProcessOrderNo, HissabNo HissabId,Replace(Str(ChallanNo)+' / '+IsNull(replace(convert(varchar(11),Date,106), ' ','-'),''),'  ','') BillNo,
                        case when PH.ProcessID=1 then isnull(ROUND(ROUND(SUM(amount),0)+ROUND(SUM(isnull(BonusAmt,0)),0)-ROUND(SUM(Penality),0),0),0) 
                            else isnull(ROUND(ROUND(SUM(amount),2)+ROUND(SUM(isnull(BonusAmt,0)),2)-ROUND(SUM(Penality),0),2),0) end amt,0 Flag,
                        isnull(TDS,0) As TDS,ISNULL(PH.ProcessHissabGST,0) as gst,
                        Case When PH.ProcessId=1 Then Case When PH.CommPaymentFlag=0 then isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0) else 0 End 
                            else isnull((Select sum(AA.AdvanceAmt) from AdvanceAmount AA where AA.Empid=PH.EmpId and AA.PROCESSID=PH.ProcessID and AA.Date between PH.FromDate and PH.ToDate),0) end as AdvanceAmountFolioWise,
                        ----Case When PH.ProcessID=1 then isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0)
                               ---- else isnull((Select sum(AA.AdvanceAmt) from AdvanceAmount AA where AA.Empid=PH.EmpId and AA.PROCESSID=PH.ProcessID and AA.Date between PH.FromDate and PH.ToDate),0) end as AdvanceAmountFolioWise,
                        ----isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0) as AdvanceAmountFolioWise,
                        isnull(AdditionAmt,0) as AdditionAmt,isnull(DeductionAmt,0) as DeductionAmt,ISNULL(PH.MaterialDeductionAmt,0) as MaterialDeductionAmt
                    From Process_Hissab PH
                    where PH.companyid=" + DDCompanyName.SelectedValue + " and PH.processid=" + DDProcessName.SelectedValue + " and PH.EmpId=" + DDEmployerName.SelectedValue + @" and 
                    HissabNo not in(Select HissabId From ProcessHissabApproved PHA(NoLock) JOIN ProcessHissabApprovedDetail PHAD(NoLock) ON PHA.ID=PHAD.ID Where PHA.HissabType=0 And PHA.ProcessId=" + DDProcessName.SelectedValue + " And PHA.EmpId=" + DDEmployerName.SelectedValue + @") 
                    Group By PH.HissabNo,PH.ChallanNo,PH.Date,PH.TDS,PH.AdditionAmt,PH.DeductionAmt,PH.EmpId,PH.ProcessOrderNo,PH.ProcessID,PH.MaterialDeductionAmt,PH.ProcessHissabGST,PH.FROMDATE,PH.TODATE,PH.CommPaymentFlag";

                if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
                {
                    str = str + @" Union Select Distinct PH.ProcessOrderNo, HissabNo HissabId,Replace(Str(ChallanNo)+' / '+IsNull(replace(convert(varchar(11),Date,106), ' ','-'),''),'  ','') BillNo,
                        case when PH.ProcessID=1 then isnull(ROUND(ROUND(SUM(amount),0)+ROUND(SUM(isnull(BonusAmt,0)),0)-ROUND(SUM(Penality),0),0),0) 
                            else isnull(ROUND(ROUND(SUM(amount),2)+ROUND(SUM(isnull(BonusAmt,0)),2)-ROUND(SUM(Penality),0),2),0) end amt ,1 Flag,
                        TDS ,ISNULL(PH.ProcessHissabGST,0) as gst,
                        Case When PH.ProcessId=1 Then Case When PH.CommPaymentFlag=0 then isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0) else 0 End 
                            else isnull((Select sum(AA.AdvanceAmt) from AdvanceAmount AA where AA.Empid=PH.EmpId and AA.PROCESSID=PH.ProcessID and AA.Date between PH.FromDate and PH.ToDate),0) end as AdvanceAmountFolioWise,
                        ----Case When PH.ProcessID=1 then isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0)
                                ----else isnull((Select sum(AA.AdvanceAmt) from AdvanceAmount AA where AA.Empid=PH.EmpId and AA.PROCESSID=PH.ProcessID and AA.Date between PH.FromDate and PH.ToDate),0) end as AdvanceAmountFolioWise,
                        ----isnull((Select sum(AA.AdvanceAmt) from AdvanceAmountByFolioNoWise AA where AA.PaymentType=1 and AA.Empid=PH.EmpId and AA.IssueORderId=PH.ProcessOrderNo and AA.PROCESSID=PH.ProcessID),0) as AdvanceAmountFolioWise,
                        isnull(AdditionAmt,0) as AdditionAmt,isnull(DeductionAmt,0) as DeductionAmt,ISNULL(PH.MaterialDeductionAmt,0) as MaterialDeductionAmt
                    From Process_Hissab PH
                    where PH.companyid=" + DDCompanyName.SelectedValue + " and PH.processid=" + DDProcessName.SelectedValue + " and PH.EmpId=" + DDEmployerName.SelectedValue + @" and 
                    HissabNo in(Select HissabId From ProcessHissabApproved PHA(NoLock) JOIN ProcessHissabApprovedDetail PHAD(NoLock) ON PHA.ID=PHAD.ID Where PHA.HissabType=0 And PHA.ProcessId=" + DDProcessName.SelectedValue + " And PHA.EmpId=" + DDEmployerName.SelectedValue + " And PHA.ID=" + DDApprovalNo.SelectedValue + @") 
                    Group By PH.HissabNo,PH.ChallanNo,PH.Date,PH.TDS,PH.AdditionAmt,PH.DeductionAmt,PH.EmpId,PH.ProcessOrderNo,PH.ProcessID,PH.MaterialDeductionAmt,PH.ProcessHissabGST,PH.FROMDATE,PH.TODATE,PH.CommPaymentFlag";
                }
            }
            else
            {
                str = @"Select Distinct HissabNo HissabId,Replace(Str(ChallanNo)+' / '+IsNull(replace(convert(varchar(11),Date,106), ' ','-'),''),'  ','') BillNo,isnull(ROUND(ROUND(SUM(amount),0)+ROUND(SUM(isnull(BonusAmt,0)),0)-ROUND(SUM(Penality),0),0),0) amt,0 Flag,isnull(TDS,0) As TDS,0 as gst
                    ,0 as AdvanceAmountFolioWise,isnull(AdditionAmt,0) as AdditionAmt,isnull(DeductionAmt,0) as DeductionAmt,ISNULL(MaterialDeductionAmt,0) as MaterialDeductionAmt
                    From Process_Hissab where companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " and EmpId=" + DDEmployerName.SelectedValue + @" and 
                    HissabNo not in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And HissabType=0 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + ") Group By HissabNo,ChallanNo,Date,TDS,AdditionAmt,DeductionAmt,MaterialDeductionAmt";

                if (ChkEdit.Checked == true && DDApprovalNo.SelectedIndex > 0)
                {
                    str = str + @" Union Select Distinct HissabNo HissabId,Replace(Str(ChallanNo)+' / '+IsNull(replace(convert(varchar(11),Date,106), ' ','-'),''),'  ','') BillNo,isnull(ROUND(ROUND(SUM(amount),0)+ROUND(SUM(isnull(BonusAmt,0)),0)-ROUND(SUM(Penality),0),0),0) amt ,1 Flag,TDS ,0 as gst
                    ,0 as AdvanceAmountFolioWise,isnull(AdditionAmt,0) as AdditionAmt,isnull(DeductionAmt,0) as DeductionAmt,ISNULL(PH.MaterialDeductionAmt,0) as MaterialDeductionAmt
                    From Process_Hissab where companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " and EmpId=" + DDEmployerName.SelectedValue + @" and 
                    HissabNo in(Select HissabId From ProcessHissabApproved PHA,ProcessHissabApprovedDetail PHAD Where PHA.ID=PHAD.ID And HissabType=0 And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And PHA.ID=" + DDApprovalNo.SelectedValue + ") Group By HissabNo,ChallanNo,Date,TDS,AdditionAmt,DeductionAmt,MaterialDeductionAmt";
                }
            }
        }
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(str, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        DGBillDetail.DataSource = ds;
        DGBillDetail.DataBind();
        for (int i = 0; i < DGBillDetail.Rows.Count; i++)
        {
            if (Convert.ToInt32(DGBillDetail.Rows[i].Cells[4].Text) == 1)
            {
                ((CheckBox)DGBillDetail.Rows[i].FindControl("Chkbox")).Checked = true;
            }
        }
    }
    private void PartyNameSelectedChange()
    {
        if (ChkEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDApprovalNo, @"Select Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-') As ApprovalNo   From ProcessHissabApproved 
            Where Status=0 And CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        }
    }
    protected void DDApprovalNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() == "1")
        {
            btndel.Visible = true;
        }
        ApprovalNoselectedChange();
        FillGrid();
    }
    private void ApprovalNoselectedChange()
    {
        TxtAppNo.Text = "";
        TxtAmt.Text = "";
        TxtTDS.Text = "";
        TxtNetAmt.Text = "";
        TxtRemarks.Text = "";
        ViewState["ID"] = DDApprovalNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select replace(convert(varchar(11),AppDate,106), ' ','-') AppDate,AppvNo,Amount,Tds,NetAmt,Remarks,gst From ProcessHissabApproved Where ID=" + DDApprovalNo.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtAppNo.Text = Ds.Tables[0].Rows[0]["AppvNo"].ToString();
            TxtAppDate.Text = Ds.Tables[0].Rows[0]["AppDate"].ToString();
            TxtAmt.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
            TxtTDS.Text = Ds.Tables[0].Rows[0]["Tds"].ToString();
            TxtNetAmt.Text = Ds.Tables[0].Rows[0]["NetAmt"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
            txtgst.Text = Ds.Tables[0].Rows[0]["gst"].ToString();
        }
    }
    protected void Chkchallan_CheckedChanged(object sender, EventArgs e)
    {
        fillAmt();
    }
    private void fillAmt()
    {
        TxtAmt.Text = "";
        TxtTDS.Text = "";
        TxtNetAmt.Text = "";
        double tot = 0;
        double Tds = 0;
        double gst = 0;
        double AdditionAmt = 0;
        double DeductionAmt = 0;
        double MaterialDeductionAmt = 0;
        double AdvanceAmtFolioWise = 0;
        for (int i = 0; i < DGBillDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGBillDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                tot = tot + Convert.ToDouble(DGBillDetail.Rows[i].Cells[2].Text);
                Tds = Convert.ToDouble(DGBillDetail.Rows[i].Cells[3].Text);
                Label lblgstgrid = (Label)DGBillDetail.Rows[i].FindControl("lblgstgrid");
                gst = Convert.ToDouble(lblgstgrid.Text);                

                Label lblAdditionAmt = (Label)DGBillDetail.Rows[i].FindControl("lblAdditionAmt");
                if (AdditionAmt == 0)
                {
                    AdditionAmt = Convert.ToDouble(lblAdditionAmt.Text);
                }
                
                Label lblDeductionAmt = (Label)DGBillDetail.Rows[i].FindControl("lblDeductionAmt");
                if (DeductionAmt == 0)
                {
                    DeductionAmt = Convert.ToDouble(lblDeductionAmt.Text);
                }

                Label lblMaterialDeductionAmt = (Label)DGBillDetail.Rows[i].FindControl("lblMaterialDeductionAmt");
                if (MaterialDeductionAmt == 0)
                {
                    MaterialDeductionAmt = Convert.ToDouble(lblMaterialDeductionAmt.Text);
                }

                Label lblAdvanceAmountFolioWise = (Label)DGBillDetail.Rows[i].FindControl("lblAdvanceAmountFolioWise");
                if (AdvanceAmtFolioWise == 0)
                {
                    AdvanceAmtFolioWise = Convert.ToDouble(lblAdvanceAmountFolioWise.Text);
                }
                
            }
        }
        TxtAmt.Text = tot.ToString();
        TxtTDS.Text = Tds.ToString();
        txtgst.Text = gst.ToString();
        if (gst > 0)
        {
            txtgst.Enabled = false;
           
        }
        else
        {
            txtgst.Enabled = true;
        }

        TxtAmt.Text = Math.Round((Convert.ToDouble(TxtAmt.Text) + AdditionAmt - DeductionAmt - MaterialDeductionAmt),2).ToString();

        //TxtNetAmt.Text = (Convert.ToDouble(TxtAmt.Text) * (100 - Tds) / 100).ToString();

        if (Session["VarCompanyNo"].ToString() == "42")
        {
            TxtNetAmt.Text = Math.Round(((Convert.ToDouble(TxtAmt.Text) - AdvanceAmtFolioWise) * (100 - Tds) / 100),2).ToString();
        }
        else
        {
            TxtNetAmt.Text = Math.Round((Convert.ToDouble(TxtAmt.Text) * (100 - Tds) / 100),2).ToString();
        }

        if (gst > 0)
        {
            GetNetamt();
        }
       


    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";
        if (lblErr.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[19];

                _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@EmpId", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@AppDate", SqlDbType.SmallDateTime);
                _arrpara[5] = new SqlParameter("@AppNo", SqlDbType.NVarChar, 100);
                _arrpara[6] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrpara[7] = new SqlParameter("@Tds", SqlDbType.Float);
                _arrpara[8] = new SqlParameter("@TxtNetAmt", SqlDbType.Float);
                _arrpara[9] = new SqlParameter("@Uid", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 250);
                _arrpara[12] = new SqlParameter("@HissabType", SqlDbType.Int);
                _arrpara[13] = new SqlParameter("@Status", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@TDSAMT", SqlDbType.Float);
                _arrpara[15] = new SqlParameter("@HissabId", SqlDbType.NVarChar, 100);
                _arrpara[16] = new SqlParameter("@gstpercentage", SqlDbType.Float);
                _arrpara[17] = new SqlParameter("@Tcspercentage", SqlDbType.Float);
                _arrpara[18] = new SqlParameter("@AdvanceAmtFolioWise", SqlDbType.Float);

                _arrpara[0].Value = ViewState["ID"];
                _arrpara[0].Direction = ParameterDirection.InputOutput;
                _arrpara[1].Value = DDCompanyName.SelectedValue;
                _arrpara[2].Value = DDProcessName.SelectedValue;
                _arrpara[3].Value = DDEmployerName.SelectedValue;
                _arrpara[4].Value = TxtAppDate.Text;
                _arrpara[5].Value = "";
                _arrpara[5].Direction = ParameterDirection.InputOutput;
                _arrpara[6].Value = Convert.ToDouble(TxtAmt.Text.Trim().ToString());
                _arrpara[7].Value = Convert.ToDouble(TxtTDS.Text.Trim().ToString());
                _arrpara[8].Value = Convert.ToDouble(TxtNetAmt.Text.Trim().ToString());
                _arrpara[9].Value = Session["varuserid"].ToString();
                _arrpara[10].Value = Session["varCompanyId"].ToString();
                _arrpara[11].Value = TxtRemarks.Text.ToUpper();
                if (ViewState["processtype"].ToString() == "2")
                {
                    _arrpara[12].Value = 2; //2 Purchase
                }
                else if (ViewState["processtype"].ToString() == "0")
                {
                    _arrpara[12].Value = 3;
                }
                else if (ViewState["processtype"].ToString() == "999")
                {
                    _arrpara[12].Value = ViewState["processtype"].ToString();
                }
                else
                {
                    _arrpara[12].Value = 0;
                }
                _arrpara[13].Value = 0;
                _arrpara[14].Value = Convert.ToDouble(TxtAmt.Text) - Convert.ToDouble(TxtNetAmt.Text);
                for (int i = 0; i < DGBillDetail.Rows.Count; i++)
                {
                    if (((CheckBox)DGBillDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        if (_arrpara[15].Value == null)
                        {
                            _arrpara[15].Value = ((Label)DGBillDetail.Rows[i].FindControl("lblHissabid")).Text;
                        }
                        else
                        {
                            _arrpara[15].Value = _arrpara[15].Value.ToString() + ',' + ((Label)DGBillDetail.Rows[i].FindControl("lblHissabid")).Text;
                        }

                        if (_arrpara[18].Value == null)
                        {
                            _arrpara[18].Value = ((Label)DGBillDetail.Rows[i].FindControl("lblAdvanceAmountFolioWise")).Text;
                        }
                    }
                }
                _arrpara[16].Value = txtgst.Text == "" ? "0" : txtgst.Text;
                _arrpara[17].Value = TxtTCSPercentage.Text == "" ? "0" : TxtTCSPercentage.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_HissabApproval", _arrpara);
                Tran.Commit();
                TxtAppNo.Text = _arrpara[5].Value.ToString();
                if (ChkEdit.Checked == true)
                {
                    DDApprovalNo.SelectedIndex = 0;
                    ApprovalNoselectedChange();
                }
                FillGrid();
                lblErr.Text = "Record Saved Successfully";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblErr.Text = ex.Message;
                lblErr.Visible = true;
            }
            finally
            {
                con.Close();
            }
        }
    }
    protected void Get_Tds_Amount()
    {
        if (TxtTDS.Text != "")
        {
            double amount, tds, NetAmt;
            amount = Convert.ToDouble(TxtAmt.Text.Trim());
            tds = Convert.ToDouble(TxtTDS.Text.Trim());
            NetAmt = (amount - (amount * tds / 100));
            TxtNetAmt.Text = Math.Round(NetAmt, 2).ToString();
        }
    }
    protected void GetNetamt()
    {
        decimal AdvanceAmtFolioWise = 0;
        for (int i = 0; i < DGBillDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGBillDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                Label lblAdvanceAmountFolioWise = (Label)DGBillDetail.Rows[i].FindControl("lblAdvanceAmountFolioWise");
                if (AdvanceAmtFolioWise == 0)
                {
                    AdvanceAmtFolioWise = Convert.ToDecimal(lblAdvanceAmountFolioWise.Text);
                }
            }
        }
        decimal amt = Convert.ToDecimal(TxtAmt.Text == "" ? "0" : TxtAmt.Text);
        decimal gstamt = Math.Round(amt * Convert.ToDecimal(txtgst.Text == "" ? "0" : txtgst.Text) / 100, 2);
        decimal TcsAmt = Math.Round((amt) * Convert.ToDecimal(TxtTCSPercentage.Text == "" ? "0" : TxtTCSPercentage.Text) / 100, 2);
        decimal tdsamt = Math.Round((amt) * Convert.ToDecimal(TxtTDS.Text == "" ? "0" : TxtTDS.Text) / 100, 2);

        if (Session["varCompanyNo"].ToString() == "42")
        {
            if (DDProcessName.SelectedItem.Text == "DYEING")
            {
                TxtNetAmt.Text = Math.Round((amt + gstamt + TcsAmt - tdsamt), 2).ToString();
            }
            else
            {
                tdsamt = Math.Round((amt + TcsAmt - AdvanceAmtFolioWise) * Convert.ToDecimal(TxtTDS.Text == "" ? "0" : TxtTDS.Text) / 100, 2);
                TxtNetAmt.Text = Math.Round((amt + gstamt + TcsAmt - tdsamt - AdvanceAmtFolioWise), 2).ToString();
            }
        }
        else
        {
            TxtNetAmt.Text = Math.Round((amt + gstamt + TcsAmt - tdsamt), 2).ToString();
        }
    }
    void clear()
    {
        TxtAmt.Text = "";
        TxtTDS.Text = "";
        TxtNetAmt.Text = "";
        TxtAppNo.Text = "";
        TxtRemarks.Text = "";
    }
    protected void TxtTDS_TextChanged(object sender, EventArgs e)
    {
        //Get_Tds_Amount();
        GetNetamt();
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {

        if (ChkEdit.Checked == true)
        {
            TDddApprovalNo.Visible = true;
            PartyNameSelectedChange();
        }
        else
        {
            TDddApprovalNo.Visible = false;
        }
    }
    protected void DGBillDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGBillDetail, "Select$" + e.Row.RowIndex);
    }
    //protected void DGBillDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void Preview_Click(object sender, EventArgs e)
    {
        if ((Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28" || Session["VarCompanyNo"].ToString() == "42") && (DDProcessName.SelectedItem.Text == "DYEING" || DDProcessName.SelectedItem.Text == "MOTTELING"))
        {
            DataSet ds = null;
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@AppvNo", TxtAppNo.Text);
            param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
            param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetHissabApprovalVoucherReport", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptVoucherHissabApproval.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptVoucherHissabApproval.xsd";
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
        else if ((Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28" || Session["VarCompanyNo"].ToString() == "42") && DDProcessName.SelectedItem.Text == "WEAVING")
        {
            DataSet ds = null;
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@AppvNo", TxtAppNo.Text);
            param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
            param[3] = new SqlParameter("@UserId", Session["VarUserId"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetHissabApprovalWeavingVoucherReport", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptVoucherWeavingHissabApproval.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptVoucherWeavingHissabApproval.xsd";
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
        else
        {
            DataSet DS = null;
            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from V_ProcessHissabApproved where AppvNo=" + TxtAppNo.Text + " And ProcessId=" + DDProcessName.SelectedValue);

            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptProcessHissabApproval.rpt";
                Session["GetDataset"] = DS;
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessHissabApproval.xsd";
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
    }
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        if (txtWeaverIdNoscan.Text != "")
        {
            string str = "select empid   From empinfo where empcode='" + txtWeaverIdNoscan.Text + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDEmployerName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
                {
                    DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    DDEmployerName_SelectedIndexChanged(sender, new EventArgs());

                }
                txtWeaverIdNoscan.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('No Employee found on this Employee code.')", true);
                txtWeaverIdNoscan.Focus();
            }
        }
    }
    protected void btndel_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@approvalId", DDApprovalNo.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSHISSABAPPROVAL", param);
            if (param[4].Value.ToString() != "")
            {
                lblErr.Text = param[4].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                lblErr.Text = "Approval No. Deleted Successfully.";
                Tran.Commit();
                DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
            }

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErr.Text = ex.Message;
        }
    }
    protected void txtgst_TextChanged(object sender, EventArgs e)
    {
        GetNetamt();
    }
    protected void TxtTCSPercentage_TextChanged(object sender, EventArgs e)
    {
        GetNetamt();
    }
}