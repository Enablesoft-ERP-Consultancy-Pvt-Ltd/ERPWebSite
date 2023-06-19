
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
public partial class Masters_ReportForms_frmGatePass_InDetail_ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = string.Empty;
            if (Session["VarCompanyNo"].ToString() == "16")
            {
                str = @"select CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["Varuserid"] + "  And CI.MasterCompanyId=" + Session["VarcompanyNo"] + @" Order by CompanyId
                    Select EmpId,EmpName From Empinfo  Where MasterCompanyId=" + Session["varcompanyNo"] + @" Order by Empname
                    Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CATEGORY_NAME select GM.GODOWNID,GM.GODOWNNAME from GODOWNMASTER GM(NoLock) JOIN  Godown_Authentication GA(NoLock) ON GM.GoDownID=GA.GodownID 
                             Where GM.MasterCompanyId=" + Session["varCompanyId"] + @" and GA.UserId=" + Session["VarUserId"] + " ORDER BY GM.GODOWNNAME";
            }
            else
            {
                str = @"select CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["Varuserid"] + "  And CI.MasterCompanyId=" + Session["VarcompanyNo"] + @" Order by CompanyId
                    Select EmpId,EmpName From Empinfo  Where MasterCompanyId=" + Session["varcompanyNo"] + @" Order by Empname
                    Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CATEGORY_NAME
                    Select Distinct GM.GoDownID, GM.GodownName  
                    From GateInMaster a(Nolock) 
                    JOIN GateInDetail b(Nolock) ON b.GateInID = a.GateInID 
                    JOIN GodownMaster GM ON GM.GoDownID = b.GODOWNID 
                    Where a.CompanyId = " + Session["CurrentWorkingCompanyID"] + " And a.MASTERCOMPANYID = " + Session["varcompanyNo"] + @" 
                    Order By GM.GodownName ";
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "Select CompanyName");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDEmpName, ds, 1, true, "Select Employee");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "ALL");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodownName, ds, 3, true, "Select Godown No");
            //UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by ITEM_NAME", true, "ALL");
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            RDGatePassDetail.Checked = true;
        }
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        DataSet ds = new DataSet(); 
        if (RDGatePassDetail.Checked == true)
        {
            Str = @"select GateoutId,cast(IssueNo As Nvarchar)+'/'+Replace(Convert(nvarchar(11),IssueDate,106),' ','-') As GateNo from GateOutMaster(Nolock) Where PartyId=" + DDEmpName.SelectedValue + " And  CompanyId=" + DDCompany.SelectedValue;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGatePass_In, ds, 0, true, "Select Gate No");
        }
        if (RDGateInDetail.Checked == true)
        {
            Str = @"Select GateInId,cast(GateInNo As Nvarchar)+'/'+Replace(Convert(nvarchar(11),GateinDate,106),' ','-') As GateNo from GateInMaster(Nolock) Where PartyId=" + DDEmpName.SelectedValue + " And  CompanyId=" + DDCompany.SelectedValue;
            Str = Str + @" Select Distinct GM.GoDownID, GM.GodownName  
                From GateInMaster a(Nolock) 
                JOIN GateInDetail b(Nolock) ON b.GateInID = a.GateInID 
                JOIN GodownMaster GM ON GM.GoDownID = b.GODOWNID 
                Where a.CompanyId = " + DDCompany.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varcompanyNo"];

            if (DDEmpName.SelectedIndex > 0)
            {
                Str = Str + @" And a.PARTYID = " + DDEmpName.SelectedValue;
            }
            Str = Str + @" Order By GM.GodownName ";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGatePass_In, ds, 0, true, "Select Gate No");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodownName, ds, 1, true, "Select Godown No");
        }

        DDGatePass_In.Items.Clear();
    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
        }
        else
        {
            trDates.Visible = false;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string str;
        DataSet ds;
        string VarDateflag;
        string Filterby = "";
        if (ChkForDate.Checked == true)
        {
            VarDateflag = "1";
        }
        else
        {
            VarDateflag = "0";
        }

        try
        {
            if (RDGatePassDetail.Checked == true)
            {
                if (Session["VarCompanyNo"].ToString() == "14")
                {
                    str = @"select IssueNo As GateNo,case When " + DDCompany.SelectedIndex + ">0 then CI.CompanyName Else 'ALL COMPANY' End CompanyName,EmpName+' '+'/'+Address As Employee,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName As Description,Lotno,ISSUEDATE Date,Sum(ISSUEQTY) As Qty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" Dateflag,GD.Remark ,
                      case when " + DDCompany.SelectedIndex + ">0 then CI.compaddr1 else '' END as Address,case when " + DDCompany.SelectedIndex + @">0 then CI.GstNo else '' END as Gstno,GM.MasterCompanyid,GD.FINISHEDID,U.UnitName,
                        ----isnull((select top (1) isnull(PRD.rate,0) from PurchaseReceiveMaster PRM(NoLock) JOIN PurchaseReceiveDetail PRD(NoLock) ON PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                        ----where PRD.FinishedId=GD.FINISHEDID order by PRM.ReceiveDate desc),0) as LastPurchaseRate,
                        --Case When isnull((select top (1) isnull(PRD.rate,0) from PurchaseReceiveMaster PRM(NoLock) JOIN PurchaseReceiveDetail PRD(NoLock) ON PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                        -- where PRD.FinishedId=GD.FINISHEDID order by PRM.ReceiveDate desc),0)=0 
                         --then  isnull((select top (1) isnull(PRD2.rate,0) from PurchaseReceiveMaster PRM2(NoLock) JOIN PurchaseReceiveDetail PRD2(NoLock) ON PRM2.PurchaseReceiveId=PRD2.PurchaseReceiveId
                         --where PRD2.LotNo=GD.LotNo order by PRM2.ReceiveDate desc),0) 
                         --Else isnull((select top (1) isnull(PRD.rate,0) from PurchaseReceiveMaster PRM(NoLock) JOIN PurchaseReceiveDetail PRD(NoLock) ON PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                         --where PRD.FinishedId=GD.FINISHEDID order by PRM.ReceiveDate desc),0) End as LastPurchaseRate,
                        Case When isnull((select top (1) isnull(PRD.rate,0) from PurchaseReceiveMaster PRM(NoLock) JOIN PurchaseReceiveDetail PRD(NoLock) ON PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                              where PRD.FinishedId=GD.FINISHEDID and PRD.LotNo=GD.LotNo order by PRM.ReceiveDate desc),0)=0
	                          Then Case when isnull((select top (1) isnull(PRD2.rate,0) from PurchaseReceiveMaster PRM2(NoLock) JOIN PurchaseReceiveDetail PRD2(NoLock) ON PRM2.PurchaseReceiveId=PRD2.PurchaseReceiveId
                                where PRD2.FinishedId=GD.FINISHEDID order by PRM2.ReceiveDate desc),0)=0
	                          Then Case when isnull((select top (1) isnull(PRD3.rate,0) from PurchaseReceiveMaster PRM3(NoLock) JOIN PurchaseReceiveDetail PRD3(NoLock) ON PRM3.PurchaseReceiveId=PRD3.PurchaseReceiveId
		                        JOIN V_FinishedItemDetail VF3(NoLock) ON PRD3.FinishedId=VF3.Item_Finished_Id
	                            where VF3.CATEGORY_ID=Vf.CATEGORY_ID and VF3.ITEM_ID=Vf.ITEM_ID and VF3.QualityId=vf.QualityId and PRD3.LotNo=GD.LotNo  order by PRM3.ReceiveDate desc),0)=0
	                          Then Case when isnull((select top (1) isnull(PRD4.rate,0) from PurchaseReceiveMaster PRM4(NoLock) JOIN PurchaseReceiveDetail PRD4(NoLock) ON PRM4.PurchaseReceiveId=PRD4.PurchaseReceiveId
                                  where PRD4.LotNo=GD.LotNo  order by PRM4.ReceiveDate desc),0)=0
	                          Then 	isnull((select top (1) isnull(PRD3.rate,0) from PurchaseReceiveMaster PRM3(NoLock) JOIN PurchaseReceiveDetail PRD3(NoLock) ON PRM3.PurchaseReceiveId=PRD3.PurchaseReceiveId
		                        JOIN V_FinishedItemDetail VF3(NoLock) ON PRD3.FinishedId=VF3.Item_Finished_Id
	                            where VF3.CATEGORY_ID=Vf.CATEGORY_ID and VF3.ITEM_ID=Vf.ITEM_ID and VF3.QualityId=vf.QualityId  order by PRM3.ReceiveDate desc),0)
	                        Else isnull((select top (1) isnull(PRD4.rate,0) from PurchaseReceiveMaster PRM4(NoLock) JOIN PurchaseReceiveDetail PRD4(NoLock) ON PRM4.PurchaseReceiveId=PRD4.PurchaseReceiveId
                                  where PRD4.LotNo=GD.LotNo  order by PRM4.ReceiveDate desc),0) End
	                        Else isnull((select top (1) isnull(PRD3.rate,0) from PurchaseReceiveMaster PRM3(NoLock) JOIN PurchaseReceiveDetail PRD3(NoLock) ON PRM3.PurchaseReceiveId=PRD3.PurchaseReceiveId
		                        JOIN V_FinishedItemDetail VF3(NoLock) ON PRD3.FinishedId=VF3.Item_Finished_Id
	                            where VF3.CATEGORY_ID=Vf.CATEGORY_ID and VF3.ITEM_ID=Vf.ITEM_ID and VF3.QualityId=vf.QualityId and PRD3.LotNo=GD.LotNo  order by PRM3.ReceiveDate desc),0) End
	                        Else isnull((select top (1) isnull(PRD2.rate,0) from PurchaseReceiveMaster PRM2(NoLock) JOIN PurchaseReceiveDetail PRD2(NoLock) ON PRM2.PurchaseReceiveId=PRD2.PurchaseReceiveId
                                where PRD2.FinishedId=GD.FINISHEDID order by PRM2.ReceiveDate desc),0) End
	                        Else isnull((select top (1) isnull(PRD.rate,0) from PurchaseReceiveMaster PRM(NoLock) JOIN PurchaseReceiveDetail PRD(NoLock) ON PRM.PurchaseReceiveId=PRD.PurchaseReceiveId
                              where PRD.FinishedId=GD.FINISHEDID and PRD.LotNo=GD.LotNo order by PRM.ReceiveDate desc),0) End as LastPurchaseRate,
                        Case When vf.MasterCompanyId=14 Then isnull((select Top (1) isnull(PPRT.Rate,0) as DyeingRate from PP_ProcessRecMaster PPRM JOIN PP_ProcessRecTran PPRT ON PPRM.PRMid=PPRT.PRMID
                            Where PPRT.Finishedid=GD.FINISHEDID and PPRT.Lotno=GD.LotNo  Order by PPRM.Date desc),0)
	                        Else isnull((select Top (1) isnull(PPRT.Rate,0) as DyeingRate from PP_ProcessRecMaster PPRM JOIN PP_ProcessRecTran PPRT ON PPRM.PRMid=PPRT.PRMID
                            Where PPRT.Finishedid=GD.FINISHEDID Order by PPRM.Date desc),0) End as LastDyeingRate
                        ----,isnull((select Top (1) isnull(PPRT.Rate,0) from PP_ProcessRecMaster PPRM(NoLock) JOIN PP_ProcessRecTran PPRT(NoLock) ON PPRM.PRMid=PPRT.PRMID
                        ----Where PPRT.Finishedid=GD.FINISHEDID Order by PPRM.Date desc),0) as LastDyeingRate  
                     From GateOutMaster GM(NoLock) JOIN GateOutDetail GD(NoLock) ON GM.GateOutId=GD.GateOutId
                     JOIN Companyinfo CI(NoLock) ON GM.CompanyId=CI.CompanyId
                     JOIN V_FinishedItemDetail vf(NoLock) ON GD.FINISHEDID=vf.Item_Finished_id 
                     JOIN Empinfo EI(NoLock) ON GM.PartyId=EI.EmpId
                     JOIN Unit U ON GD.Unitid=U.UnitId
                     Where CI.MasterCompanyId=" + Session["varCompanyId"];
                    if (DDCompany.SelectedIndex > 0)
                    {
                        str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                        Filterby = Filterby + " Employee-" + DDEmpName.SelectedItem.Text + ",";
                    }
                    if (DDGatePass_In.SelectedIndex > 0)
                    {
                        str = str + "  And GM.GateoutId=" + DDGatePass_In.SelectedValue;
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And GM.IssueDate>='" + TxtFromDate.Text + "' And GM.IssueDate<='" + TxtToDate.Text + "'";
                        Filterby = Filterby + " IssueDate>=" + TxtFromDate.Text + " and IssueDate<=" + TxtToDate.Text + ",";
                    }
                    if (DDCategory.SelectedIndex > 0)
                    {
                        str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;
                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                        Filterby = Filterby + " ItemName-" + ddItemName.SelectedItem.Text + ",";
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                        Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text + ",";
                    }
                    if (DDDesign.SelectedIndex > 0)
                    {
                        str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
                    }
                    if (DDColor.SelectedIndex > 0)
                    {
                        str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
                    }
                    if (TxtGateInOutPassNo.Text != "")
                    {
                        str = str + " AND GM.IssueNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
                    }
                    str = str + " group by IssueNo,CompanyName,CI.compaddr1,ci.gstno,EmpName,Address,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,Lotno,ISSUEDATE,GD.Remark,GM.MasterCompanyid,GD.FINISHEDID,U.UnitName,Vf.CATEGORY_ID ,Vf.ITEM_ID ,vf.QualityId,vf.MasterCompanyId";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (chkexcelexport.Checked == true)
                    {
                        GatePassDetailExcelExportEastern(ds, Filterby);
                        return;
                    }
                    Session["dsFilename"] = "~\\ReportSchema\\RptGeneralGatePassDetail.xsd";
                    Session["rptFilename"] = "Reports/RptGeneralGatePassDetail.rpt";
                    Session["GetDataset"] = ds;
                }
                else
                {

                    str = @"select IssueNo As GateNo,case When " + DDCompany.SelectedIndex + ">0 then CI.CompanyName Else 'ALL COMPANY' End CompanyName,EmpName+' '+'/'+Address As Employee,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName As Description,Lotno,ISSUEDATE Date,Sum(ISSUEQTY) As Qty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" Dateflag,GD.Remark ,
                      case when " + DDCompany.SelectedIndex + ">0 then CI.compaddr1 else '' END as Address,case when " + DDCompany.SelectedIndex + @">0 then CI.GstNo else '' END as Gstno,GM.MasterCompanyid
                      from GateOutMaster GM,GateOutDetail GD,Companyinfo CI,Empinfo EI,V_FinishedItemDetail vf
                      Where GM.GateOutId=GD.GateOutId And  GD.FINISHEDID=vf.Item_Finished_id And GM.PartyId=EI.EmpId And GM.CompanyId=CI.CompanyId
                      And CI.MasterCompanyId=" + Session["varCompanyId"];
                    if (DDCompany.SelectedIndex > 0)
                    {
                        str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                        Filterby = Filterby + " Employee-" + DDEmpName.SelectedItem.Text + ",";
                    }
                    if (DDGodownName.Items.Count > 0 && DDGodownName.SelectedIndex > 0)
                    {
                        str = str + " And GD.GodownID = " + DDGodownName.SelectedValue;
                    }
                    if (DDGatePass_In.SelectedIndex > 0)
                    {
                        str = str + "  And GM.GateoutId=" + DDGatePass_In.SelectedValue;
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And GM.IssueDate>='" + TxtFromDate.Text + "' And GM.IssueDate<='" + TxtToDate.Text + "'";
                        Filterby = Filterby + " IssueDate>=" + TxtFromDate.Text + " and IssueDate<=" + TxtToDate.Text + ",";
                    }
                    if (DDCategory.SelectedIndex > 0)
                    {
                        str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                        Filterby = Filterby + " ItemName-" + ddItemName.SelectedItem.Text + ",";
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                        Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text + ",";

                    }
                    if (DDDesign.SelectedIndex > 0)
                    {
                        str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

                    }
                    if (DDColor.SelectedIndex > 0)
                    {
                        str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
                    }
                    if (TxtGateInOutPassNo.Text != "")
                    {
                        str = str + " AND GM.IssueNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
                    }
                    str = str + " group by IssueNo,CompanyName,CI.compaddr1,ci.gstno,EmpName,Address,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,Lotno,ISSUEDATE,GD.Remark,GM.MasterCompanyid,GD.GodownID";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (chkexcelexport.Checked == true)
                    {
                        GatepassDetailExcelexport(ds, Filterby);
                        return;
                    }
                    Session["dsFilename"] = "~\\ReportSchema\\RptGeneralGatePassDetail.xsd";
                    Session["rptFilename"] = "Reports/RptGeneralGatePassDetail.rpt";
                    Session["GetDataset"] = ds;
                }
               
            }
            if (RDGateInDetail.Checked == true)
            {
                str = @"select GateInNo As GateNo,case When " + DDCompany.SelectedIndex + ">0 then CI.CompanyName Else 'ALL COMPANY' End CompanyName,EmpName+' '+'/'+Address As Employee,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName As Description,Lotno,GateInDate Date,Sum(Qty) As Qty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" Dateflag,
                        case when " + DDCompany.SelectedIndex + ">0 then CI.compaddr1 else '' END as Address,case when " + DDCompany.SelectedIndex + @">0 then CI.GstNo else '' END as Gstno,GM.MasterCompanyId, 
                        GD.Remark, GD.IssQtyFromOther, GD.ShadeStatus, GD.FolioNo, GM.ChallanNo ,isnull(NUD.UserName,'') as UserName 
                        from GateInMaster GM(nolock)
                        JOIN GateInDetail GD(nolock) ON GD.GateInId = GM.GateInId 
                        JOIN Companyinfo CI(nolock) ON CI.CompanyId = GM.CompanyId 
                        JOIN Empinfo EI(nolock) ON EI.EmpId = GM.PartyId 
                        JOIN V_FinishedItemDetail VF(nolock) ON VF.Item_Finished_id = GD.FINISHEDID
                        JOIN NewUserDetail NUD(NoLock) ON GM.UserID=NUD.UserId
                        Where GM.MasterCompanyID = " + Session["varCompanyId"];
                if (DDCompany.SelectedIndex > 0)
                {
                    str = str + " And GM.CompanyId=" + DDCompany.SelectedValue;
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    str = str + " And GM.PartyId =" + DDEmpName.SelectedValue;
                    Filterby = Filterby + " Employee-" + DDEmpName.SelectedItem.Text + ",";
                }
                if (DDGodownName.Items.Count > 0 && DDGodownName.SelectedIndex > 0)
                {
                    str = str + " And GD.GodownID = " + DDGodownName.SelectedValue;
                }
                if (DDGatePass_In.SelectedIndex > 0)
                {
                    str = str + "  And GM.GateinId = " + DDGatePass_In.SelectedValue;
                }
                if (ChkForDate.Checked == true)
                {
                    str = str + "  And GM.GateInDate>='" + TxtFromDate.Text + "' And GM.GateInDate<='" + TxtToDate.Text + "'";
                    Filterby = Filterby + " GateInDate>=" + TxtFromDate.Text + " and GateInDate<=" + TxtToDate.Text + ",";
                }
                if (TxtGateInOutPassNo.Text != "")
                {
                    str = str + " AND GM.GateInNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
                }
                if (DDCategory.SelectedIndex > 0)
                {
                    str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;
                }
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
                }
                str = str + @" group by GateInNo,CompanyName,CI.compaddr1,ci.gstno,EmpName,Address,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,
                    Lotno,GateInDate,GM.MasterCompanyId, GD.Remark, GD.IssQtyFromOther, GD.ShadeStatus, GD.FolioNo, GM.ChallanNo,NUD.UserName ";

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (chkexcelexport.Checked == true)
                {
                    if (Session["VarCompanyNo"].ToString() == "45")
                    {
                        GateINDetailExcelexportMWS(ds, Filterby);
                        return;
                    }
                    else
                    {
                        GateINDetailExcelexport(ds, Filterby);
                        return;
                    }
                    
                }
                Session["dsFilename"] = "~\\ReportSchema\\RptGeneralGateInDetail.xsd";
                Session["rptFilename"] = "Reports/RptGeneralGateInDetail.rpt";
                Session["GetDataset"] = ds;
            }
            if (RDGatePass_inDetail.Checked == true)
            {
                if (Session["varCompanyId"].ToString() == "14" && chkexcelexport.Checked == true)
                {
                    GatePass_INWithTagNoExcelReport();
                    return;
                }
                else
                {
                    str = @"select IssueNo As GateNo,case When " + DDCompany.SelectedIndex + ">0 then CI.CompanyName Else 'ALL COMPANY' End CompanyName,EmpName+' '+'/'+Address As Employee,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName As Description,Lotno,ISSUEDATE Date,Sum(ISSUEQTY) As IssQty,0 As RecQty ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" Dateflag  
                      ,case when " + DDCompany.SelectedIndex + ">0 then CI.compaddr1 else '' END as Address,case when " + DDCompany.SelectedIndex + @">0 then CI.GstNo else '' END as Gstno,GM.MasterCompanyId
                      from GateOutMaster GM,GateOutDetail GD,Companyinfo CI,Empinfo EI,V_FinishedItemDetail vf
                      Where GM.GateOutId=GD.GateOutId And  GD.FINISHEDID=vf.Item_Finished_id And GM.PartyId=EI.EmpId And GM.CompanyId=CI.CompanyId
                      And CI.MasterCompanyId=" + Session["varCompanyId"];
                    if (DDCompany.SelectedIndex > 0)
                    {
                        str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                        Filterby = Filterby + " Employee-" + DDEmpName.SelectedItem.Text + ",";
                    }
                    if (DDGodownName.Items.Count > 0 && DDGodownName.SelectedIndex > 0)
                    {
                        str = str + " And GD.GodownID = " + DDGodownName.SelectedValue;
                    }
                    if (DDGatePass_In.SelectedIndex > 0)
                    {
                        str = str + "  And GM.GateoutId=" + DDGatePass_In.SelectedValue;
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And GM.IssueDate>='" + TxtFromDate.Text + "' And GM.IssueDate<='" + TxtToDate.Text + "'";
                        Filterby = Filterby + " Date>=" + TxtFromDate.Text + "  and Date<=" + TxtToDate.Text;
                    }
                    if (DDCategory.SelectedIndex > 0)
                    {
                        str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                        Filterby = Filterby + " ItemName-" + ddItemName.SelectedItem.Text + ",";
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                        Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text + ",";

                    }
                    if (DDDesign.SelectedIndex > 0)
                    {
                        str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

                    }
                    if (DDColor.SelectedIndex > 0)
                    {
                        str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
                    }
                    if (TxtGateInOutPassNo.Text != "")
                    {
                        str = str + " AND GM.IssueNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
                    }
                    str = str + " group by IssueNo,CompanyName,CI.compaddr1,ci.gstno,EmpName,Address,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,Lotno,ISSUEDATE,GM.MasterCompanyId,GD.GodownID  Union ALL";
                    str = str + @" select GateInNo As GateNo,case When " + DDCompany.SelectedIndex + ">0 then CI.CompanyName Else 'ALL' End ComPanyName,EmpName+' '+'/'+Address As Employee,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName As Description,Lotno,GateInDate Date,0 As IssQty,Sum(Qty) As RecQty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" Dateflag
                        ,case when " + DDCompany.SelectedIndex + ">0 then CI.compaddr1 else '' END as Address,case when " + DDCompany.SelectedIndex + @">0 then CI.GstNo else '' END as Gstno,GM.MasterCompanyId
                        from GateInMaster GM,GateInDetail GD,Companyinfo CI,Empinfo EI,V_FinishedItemDetail vf
                        Where GM.GateInId=GD.GateInId And  GD.FINISHEDID=vf.Item_Finished_id And GM.PartyId=EI.EmpId And GM.CompanyId=CI.CompanyId
                        And CI.MasterCompanyId=" + Session["varCompanyId"];
                    if (DDCompany.SelectedIndex > 0)
                    {
                        str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                    }
                    if (DDGodownName.Items.Count > 0 && DDGodownName.SelectedIndex > 0)
                    {
                        str = str + " And GD.GodownID = " + DDGodownName.SelectedValue;
                    }
                    if (DDGatePass_In.SelectedIndex > 0)
                    {
                        str = str + "  And GM.GateinId=" + DDGatePass_In.SelectedValue;
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And GM.GateInDate>='" + TxtFromDate.Text + "' And GM.GateInDate<='" + TxtToDate.Text + "'";
                    }
                    if (DDCategory.SelectedIndex > 0)
                    {
                        str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                        Filterby = Filterby + " ItemName-" + ddItemName.SelectedItem.Text + ",";
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                        Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text + ",";

                    }
                    if (DDDesign.SelectedIndex > 0)
                    {
                        str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

                    }
                    if (DDColor.SelectedIndex > 0)
                    {
                        str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
                    }
                    if (TxtGateInOutPassNo.Text != "")
                    {
                        str = str + " AND GM.GateInNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
                    }
                    str = str + "  group by GateInNo,CompanyName,CI.Compaddr1,ci.gstno,EmpName,Address,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,Lotno,GateInDate,GM.MasterCompanyId,GD.GodownID";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (chkexcelexport.Checked == true)
                    {
                        GATE_IN_PASSDETAILEXCEL(ds, Filterby);
                        return;
                    }
                    Session["dsFilename"] = "~\\ReportSchema\\RptGeneralGatePass_In_Detail.xsd";
                    Session["rptFilename"] = "Reports/RptGeneralGatePass_In_Detail.rpt";
                    Session["GetDataset"] = ds;
                }

               
            }
            DataSet DS = (DataSet)(Session["GetDataset"]);
            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["GetDataset"] = DS;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }

        }
        catch (Exception ex)
        {

            MessageSave(ex.Message);
        }


    }
    protected void GatepassDetailExcelexport(DataSet ds, string filterby)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("General Gate Pass Detail");

            sht.Range("A1:G1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " General Gate Pass Detail";
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:G1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 20.00;
            //
            sht.Range("A2:G2").Merge();
            sht.Range("A2").Value = "Filter By - " + filterby;
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetWrapText();
            sht.Range("A2:G2").Style.Font.FontSize = 10;
            sht.Row(1).Height = 20.00;

            sht.Range("A3").Value = "EMP NAME";
            sht.Range("B3").Value = "GATE PASS NO.";
            sht.Range("C3").Value = "DATE";
            sht.Range("D3").Value = "ITEM DESCRIPTION";
            sht.Range("E3").Value = "LOT NO.";
            sht.Range("F3").Value = "REMARK";
            sht.Range("G3").Value = "QTY";
            sht.Range("A3:G3").Style.Font.Bold = true;
            sht.Range("A3:G3").Style.Font.FontSize = 10;
            sht.Range("G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 25.50;
            //*****************************
            int row = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Employee,Date";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                row = row + 1;
            }
            //************
            sht.Range("G" + row).FormulaA1 = "SUM(G4:G" + (row - 1) + ")";
            //********************************
            sht.Columns(1, 10).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GeneralGatePassDetail" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnexcel1", "alert('No Record Found!');", true);
        }
    }

    protected void GateINDetailExcelexport(DataSet ds, string filterby)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("General Gate In Detail");

            sht.Range("A1:H1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " General Gate In Detail";
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:H1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 20.00;
            //
            sht.Range("A2:H2").Merge();
            sht.Range("A2").Value = "Filter By - " + filterby;
            sht.Range("A2:H2").Style.Font.Bold = true;
            sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:H2").Style.Alignment.SetWrapText();
            sht.Range("A2:H2").Style.Font.FontSize = 10;
            sht.Row(1).Height = 20.00;

            sht.Range("A3").Value = "EMP NAME";           
            sht.Range("B3").Value = "GATE IN NO.";
            sht.Range("C3").Value = "DATE";
            sht.Range("D3").Value = "ITEM DESCRIPTION";
            sht.Range("E3").Value = "LOT NO.";              
            sht.Range("F3").Value = "QTY";
            sht.Range("G3").Value = "CHALLAN NO";
            sht.Range("H3").Value = "REMARK";        

            sht.Range("A3:H3").Style.Font.Bold = true;
            sht.Range("A3:H3").Style.Font.FontSize = 10;
            sht.Range("F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 25.50;
            //*****************************
            int row = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Employee,Date";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);               
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);  
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);

                row = row + 1;
            }
            //************
           // sht.Range("H" + row).FormulaA1 = "SUM(H4:H" + (row - 1) + ")"; 
            sht.Range("I" + row).FormulaA1 = "SUM(F4:F" + (row - 1) + ")";
            //********************************
            sht.Columns(1, 11).AdjustToContents();
            using (var a = sht.Range("A1" + ":H" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GeneralGateInDetail" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnexcel1", "alert('No Record Found!');", true);
        }
    }
    //GATE_IN_PASSDETAILEXCEL
    protected void GATE_IN_PASSDETAILEXCEL(DataSet ds, string filterby)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("General Gate Pass_In Detail");

            sht.Range("A1:G1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " General Gate Pass/In Detail";
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:G1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 20.00;
            //
            sht.Range("A2:G2").Merge();
            sht.Range("A2").Value = "Filter By - " + filterby;
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetWrapText();
            sht.Range("A2:G2").Style.Font.FontSize = 10;
            sht.Row(1).Height = 20.00;

            sht.Range("A3").Value = "EMP NAME";
            sht.Range("B3").Value = "GATE PASS/IN NO.";
            sht.Range("C3").Value = "DATE";
            sht.Range("D3").Value = "ITEM DESCRIPTION";
            sht.Range("E3").Value = "LOT NO.";
            sht.Range("F3").Value = "ISS QTY.";
            sht.Range("G3").Value = "REC QTY";
            sht.Range("A3:G3").Style.Font.Bold = true;
            sht.Range("A3:G3").Style.Font.FontSize = 10;
            sht.Range("G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 25.50;
            //*****************************
            int row = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Employee,Date";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                row = row + 1;
            }
            //************
            sht.Range("F" + row).FormulaA1 = "SUM(F4:F" + (row - 1) + ")";
            sht.Range("G" + row).FormulaA1 = "SUM(G4:G" + (row - 1) + ")";
            //********************************
            sht.Columns(1, 10).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GeneralGatePass_InDetail" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnexcel1", "alert('No Record Found!');", true);
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

    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategory.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by ITEM_NAME", true, "ALL");
        }
        ddlcategorycange();
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDQuality.Visible = true;
        if (ddItemName.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by QualityName", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varCompanyid"] + "  order by QualityName", true, "ALL");
        }
    }
    private void ddlcategorycange()
    {
        try
        {
            TRDDQuality.Visible = false;
            TRDDColor.Visible = false;
            TRDDDesign.Visible = false;
            string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                          " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                          " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["PARAMETER_ID"].ToString())
                    {
                        case "1":
                            TRDDQuality.Visible = true;
                            break;
                        case "2":
                            TRDDDesign.Visible = true;
                            UtilityModule.ConditionalComboFill(ref DDDesign, "select distinct designId, designName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order By designName ", true, "ALL");
                            break;
                        case "3":
                            TRDDColor.Visible = true;
                            UtilityModule.ConditionalComboFill(ref DDColor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By colorname ", true, "ALL");
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

    protected void GatePass_INWithTagNoExcelReport()
    {
        ////lblErrorMessage.Text = "";
        try
        {

            string str = "";
            string Filterby = "";

            string str2 = "";
            string Filterby2 = "";

            ////GatePass Condition
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " And CI.CompanyId=" + DDCompany.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                Filterby = Filterby + " Employee-" + DDEmpName.SelectedItem.Text + ",";
            }
            if (DDGatePass_In.SelectedIndex > 0)
            {
                str = str + "  And GM.GateoutId=" + DDGatePass_In.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                str = str + "  And GM.IssueDate>='" + TxtFromDate.Text + "' And GM.IssueDate<='" + TxtToDate.Text + "'";
                Filterby = Filterby + " Date>=" + TxtFromDate.Text + "  and Date<=" + TxtToDate.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                Filterby = Filterby + " ItemName-" + ddItemName.SelectedItem.Text + ",";
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text + ",";

            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " AND vf.COLORID = " + DDColor.SelectedValue;
            }
            if (TxtGateInOutPassNo.Text != "")
            {
                str = str + " AND GM.IssueNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
            }


            ////GateIn Condition

            if (DDCompany.SelectedIndex > 0)
            {
                str2 = str2 + " And CI.CompanyId=" + DDCompany.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str2 = str2 + " And EI.EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDGatePass_In.SelectedIndex > 0)
            {
                str2 = str2 + "  And GM.GateinId=" + DDGatePass_In.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                str2 = str2 + "  And GM.GateInDate>='" + TxtFromDate.Text + "' And GM.GateInDate<='" + TxtToDate.Text + "'";
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str2 = str2 + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

            }
            if (ddItemName.SelectedIndex > 0)
            {
                str2 = str2 + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
                Filterby2 = Filterby2 + " ItemName-" + ddItemName.SelectedItem.Text + ",";
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str2 = str2 + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                Filterby2 = Filterby2 + " Quality-" + DDQuality.SelectedItem.Text + ",";

            }
            if (DDDesign.SelectedIndex > 0)
            {
                str2 = str2 + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str2 = str2 + " AND vf.COLORID = " + DDColor.SelectedValue;
            }
            if (TxtGateInOutPassNo.Text != "")
            {
                str2 = str2 + " AND GM.GateInNo='" + TxtGateInOutPassNo.Text.Trim() + "'";
            }


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETGATEPASS_IN_WITHTAGNO_EXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);   
            cmd.Parameters.AddWithValue("@Fromdate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@Todate",TxtToDate.Text);        
            cmd.Parameters.AddWithValue("@WhereGatePass", str);
            cmd.Parameters.AddWithValue("@WhereGateIn", str2);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("General Gate Pass_In Detail");

                sht.Range("A1:H1").Merge();
                sht.Range("A1").Value = "General Gate Pass/In Detail";
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:H1").Style.Font.FontSize = 13;
                sht.Row(1).Height = 20.00;
                //
                sht.Range("A2:H2").Merge();
                sht.Range("A2").Value = "Filter By - " + Filterby;
                sht.Range("A2:H2").Style.Font.Bold = true;
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetWrapText();
                sht.Range("A2:H2").Style.Font.FontSize = 10;
                sht.Row(1).Height = 20.00;

                sht.Range("A3").Value = "EMP NAME";
                sht.Range("B3").Value = "GATE PASS/IN NO.";
                sht.Range("C3").Value = "DATE";
                sht.Range("D3").Value = "ITEM DESCRIPTION";
                sht.Range("E3").Value = "LOT NO.";
                sht.Range("F3").Value = "TAG NO.";

                sht.Range("G3").Value = "ISS QTY.";
                sht.Range("H3").Value = "REC QTY";
                sht.Range("A3:H3").Style.Font.Bold = true;
                sht.Range("A3:H3").Style.Font.FontSize = 10;
                sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Row(1).Height = 25.50;
                //*****************************
                int row = 4;
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "Employee,Date";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Tagno"]);

                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Issqty"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                    row = row + 1;
                }
                //************
                sht.Range("G" + row).FormulaA1 = "SUM(G4:G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "SUM(H4:H" + (row - 1) + ")";
                //********************************
                sht.Columns(1, 10).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("GeneralGatePass_InDetail" + DateTime.Now + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                // Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Visible = true;
            //lblErrorMessage.Text = ex.Message;
        }
    }

    protected void GatePassDetailExcelExportEastern(DataSet ds, string filterby)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("General Gate Pass Detail");

            sht.Range("A1:M1").Merge();
            sht.Range("A1").Value = "General Gate Pass Detail";
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:M1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 20.00;
            //
            sht.Range("A2:M2").Merge();
            sht.Range("A2").Value = "Filter By - " + filterby;
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetWrapText();
            sht.Range("A2:M2").Style.Font.FontSize = 10;
            sht.Row(1).Height = 20.00;

            sht.Range("A3").Value = "EMP NAME";
            sht.Range("B3").Value = "GATE PASS NO.";
            sht.Range("C3").Value = "DATE";
            sht.Range("D3").Value = "ITEM DESCRIPTION";
            sht.Range("E3").Value = "LOT NO.";
            sht.Range("F3").Value = "REMARK";
            sht.Range("G3").Value = "QTY";
            sht.Range("H3").Value = "UNIT";
            sht.Range("I3").Value = "PURCHASE RATE";
            sht.Range("J3").Value = "AMOUNT";
            sht.Range("K3").Value = "DYEINGRATE";
            sht.Range("L3").Value = "DYEING AMOUNT";
            sht.Range("M3").Value = "TOTAL AMOUNT";


            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("A3:M3").Style.Font.FontSize = 10;
            sht.Range("G3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 25.50;
            //*****************************
            int row = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Employee,Date";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);

                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["UnitName"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["LastPurchaseRate"]);
                decimal PurchaseRateAmt = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["LastPurchaseRate"]);
                sht.Range("J" + row).SetValue(Math.Round(PurchaseRateAmt,2));
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["LastDyeingRate"]);
                decimal DyeingRateAmt = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["LastDyeingRate"]);
                sht.Range("L" + row).SetValue(Math.Round(DyeingRateAmt,2));
                sht.Range("M" + row).SetValue(Math.Round(PurchaseRateAmt + DyeingRateAmt,2));


                row = row + 1;
            }
            //************
            sht.Range("G" + row).FormulaA1 = "SUM(G4:G" + (row - 1) + ")";
            sht.Range("J" + row).FormulaA1 = "SUM(J4:J" + (row - 1) + ")";
            sht.Range("L" + row).FormulaA1 = "SUM(L4:L" + (row - 1) + ")";
            //********************************
            sht.Columns(1, 15).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GeneralGatePassDetailNew" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnexcel1", "alert('No Record Found!');", true);
        }
    }

    protected void GateINDetailExcelexportMWS(DataSet ds, string filterby)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("General Gate In Detail");

            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " General Gate In Detail";
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:L1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 20.00;
            //
            sht.Range("A2:L2").Merge();
            sht.Range("A2").Value = "Filter By - " + filterby;
            sht.Range("A2:L2").Style.Font.Bold = true;
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetWrapText();
            sht.Range("A2:L2").Style.Font.FontSize = 10;
            sht.Row(1).Height = 20.00;

            sht.Range("A3").Value = "EMP NAME";
            sht.Range("B3").Value = "FOLIO NO.";
            sht.Range("C3").Value = "GATE IN NO.";
            sht.Range("D3").Value = "DATE";
            sht.Range("E3").Value = "ITEM DESCRIPTION";
            sht.Range("F3").Value = "LOT NO.";           
            sht.Range("G3").Value = "ISS QTY FROM MWS";
            sht.Range("H3").Value = "RECQTY";
            sht.Range("I3").Value = "CHALLAN NO";
            sht.Range("J3").Value = "SHADE / RM STATUS";
            sht.Range("K3").Value = "REMARK";
            sht.Range("L3").Value = "USER NAME";

            sht.Range("A3:L3").Style.Font.Bold = true;
            sht.Range("A3:L3").Style.Font.FontSize = 10;
            sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Row(1).Height = 25.50;
            //*****************************
            int row = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Employee,Date";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["FOLIONO"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["GateNo"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);                
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["IssQtyFromOther"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["ShadeStatus"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Remark"]);
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }
            //************
            sht.Range("H" + row).FormulaA1 = "SUM(H4:H" + (row - 1) + ")";
            sht.Range("G" + row).FormulaA1 = "SUM(G4:G" + (row - 1) + ")";
            //********************************
            sht.Columns(1, 13).AdjustToContents();
            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GeneralGateInDetail" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opnexcel1", "alert('No Record Found!');", true);
        }
    }
}