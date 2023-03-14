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
using ClosedXML.Excel;
using System.IO;
public partial class Masters_ReportForms_FrmIndentRecDetailWithMultipleOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
                        Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and " + (variable.Carpetcompany == "1" ? " Process_Name='DYEING'" : "1=1") + @" Order By PROCESS_NAME
                        select EmpId,ltrim(Empname)+'/'+Address As Empname  from Empinfo  Where MasterCompanyId=" + Session["varCompanyId"] + @"  Order by EmpName                  
                        select distinct CI.CustomerId,CI.CustomerCode+'/'+CompanyName as customercode from OrderMaster OM inner join V_Indent_OredrId VO on Om.OrderId=VO.Orderid inner join customerinfo CI on CI.CustomerId=OM.CustomerId order by CustomerCode
                        select IM.Item_Id,Im.Item_Name From Item_Master Im inner join CategorySeparate cs on IM.CATEGORY_ID=Cs.Categoryid and cs.id=1 order by IM.ITEM_NAME";
            if (variable.JoborderNewModule == "1")
            {
                str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
            }
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                DDCompany_SelectedIndexChanged(sender, new EventArgs());
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmpName, ds, 2, true, "--Select--");
            //UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddItemName, ds, 4, true, "--ALL--");
            if (ddItemName.Items.Count > 0)
            {
                ddItemName.SelectedIndex = 0;
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCompany.Items.Count > 0)
            {
                //    DDCompany.SelectedIndex = 1;
            }

            //
           
            //if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            //{
            //    TRCustomerCode.Visible = false;
            //}
            //if (Session["varcompanyId"].ToString() == "16")
            //{
            //    RDPONot.Visible = false;
            //    RDdyerledger.Visible = false;
            //}
        }
    }

    protected void DDIndentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDIndentNo.SelectedIndex > 0)
        {
            //TRPartyChallanNo.Visible = false;
            //txtPartyChallanNo.Text = "";
        }
        else
        {
           // TRPartyChallanNo.Visible = true;
        }
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = string.Empty;
        if (chksample.Checked == true)
        {
            //TRPartyChallanNo.Visible = false;
            //txtPartyChallanNo.Text = "";

            str = @" Select ID as Indentid,indentNo From SampleDyeingmaster SM Where 1=1";
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " and companyid=" + DDCompany.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " and Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and empid=" + DDEmpName.SelectedValue;
            }
            str = str + " order by Indentid";
        }
        else
        {
            str = @"Select Distinct IM.IndentId,IM.IndentNo from PP_ProcessRecMaster PRM JOIN PP_ProcessRecTran PRT ON PRM.PRMID=PRT.PRMID
                    JOIN IndentMaster IM ON PRT.IndentID=IM.IndentId
                  Where IM.MasterCompanyid= " + Session["varcompanyid"];
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " and PRM.companyid=" + DDCompany.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and PRT.CustomerOrderId=" + DDOrderNo.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " and PRM.Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and PRM.EmpId=" + DDEmpName.SelectedValue;
            }

            //if (variable.JoborderNewModule == "1")
            //{
            //    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
            //}
            str = str + "  order by IndentId";
        }

        UtilityModule.ConditionalComboFill(ref DDIndentNo, str, true, "--Select--");
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str, strsample = string.Empty;
        str = @" select Distinct EI.EmpId,ltrim(Empname)+'/'+Address as EmpName from PP_ProcessRecMaster PRM JOIN PP_ProcessRecTran PRT ON PRM.PRMID=PRT.PRMID
                 inner join empinfo EI on EI.EmpId=PRM.EmpId  Where EI.MasterCompanyid= " + Session["varcompanyid"];

        strsample = "select Distinct ei.EmpId,ei.EmpName+'/'+Address as Empname From SampleDyeingmaster SM inner join EmpInfo ei on SM.empid=ei.EmpId Where 1=1";

        if (DDCompany.SelectedIndex > 0)
        {
            str = str + " and PRM.companyid=" + DDCompany.SelectedValue;
            strsample = strsample + " and SM.companyid=" + DDCompany.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and PRT.CustomerOrderid=" + DDOrderNo.SelectedValue;
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + " and PRM.Processid=" + DDProcessName.SelectedValue;
            strsample = strsample + " and SM.Processid=" + DDProcessName.SelectedValue;
        }
        strsample = strsample + "  order by Empname";
        //if (variable.JoborderNewModule == "1")
        //{
        //    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
        //}
        str = str + " UNION " + strsample;
        UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Select--");

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
        if (RDProcessRecDetail.Checked == true)
        {
            IndentRecDetail();
        }
       
//        //*********************
//        string str, strsample = "";
//        DataSet ds;
//        string VarDateflag;
//        if (ChkForDate.Checked == true)
//        {
//            VarDateflag = "1";
//        }
//        else
//        {
//            VarDateflag = "0";
//        }
//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//        if (con.State == ConnectionState.Closed)
//        {
//            con.Open();
//        }
//        SqlTransaction Tran = con.BeginTransaction();
//        try
//        {            
                       
//            if (RDProcessRecDetail.Checked == true)
//            {
//                if (variable.JoborderNewModule == "1")
//                {
//                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End as CompanyName,Empname,IndentNo,
//                            '' as IssueChallanNo,PM.ChallanNo As RecChallanNo,PM.Date,Finishedid,PT.LotNo,
//                            CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
//                            case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft  End As Description,Sum(RecQuantity) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,
//                            '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid
//                            ,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process ,PT.TagNo
//                            From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
//                            inner join IndentMaster Im on PT.IndentId=IM.IndentID
//                            inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
//                            inner join EmpInfo E on PM.Empid=E.EmpId
//                            inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
//                            inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
//                            inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID                           
//                            left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
//                            left join OrderMaster OM on OD.OrderId=OM.OrderId
//                            left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
//                            INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
//                            Where IM.MasterCompanyId=" + Session["varCompanyId"];

//                }
//                else
//                {
//                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
//                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,PM.Date,Finishedid,PT.LotNo,
//                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
//                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
//                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
//                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
//                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
//                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo
//                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
//                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
//                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
//                        inner join EmpInfo E on PM.Empid=E.EmpId
//                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
//                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
//                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
//                        left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
//                        left join OrderMaster OM on OD.OrderId=OM.OrderId
//                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
//                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
//                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];
//                    switch (Session["varcompanyNo"].ToString())
//                    {
//                        case "16":
//                            str = str + " and PT.Rec_Iss_ItemFlag=0";
//                            break;
//                        default:
//                            break;
//                    }
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = @"select pnm.process_name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,ei.EmpName,case When CHARINDEX('s',sm.indentNo)=0 then 'S-'+sm.indentNo else Sm.indentNo End as Indentno
//                                ,Sm.indentno as Issuechallanno,srm.ChallanNo as RecChallanNo,Srm.ReceiveDate as Date,Srd.Rfinishedid as Finishedid,srd.LotNo,
//                                Vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName+' '+Vf.ShapeName+' '+
//                                case When vf.SizeId>0 then vf.SizeFt else '' End As Description,Sum(Srd.recqty) as Recqty,sum(srd.lossqty) as Lossqty,0 as retqty,
//                                '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,'' as Localorder,'' as Customerorderno,0 as Lshort,0 as shrinkage,sm.id as prmid,pnm.process_name as Re_process,
//                                SUM(SRD.UNDYEDQTY) AS UNDYEDQTY,Srd.TagNo
//                                 From SampleDyeingReceivemaster Srm inner join SampleDyeingReceiveDetail srd on srm.ID=srd.Masterid
//                                inner join SampleDyeingmaster sm on srd.issueid=sm.ID
//                                inner join companyinfo ci on srm.companyid=ci.companyid
//                                inner join EmpInfo ei on srm.empid=ei.EmpId
//                                inner join PROCESS_NAME_MASTER pnm on sm.processid=pnm.PROCESS_NAME_ID
//                                inner join V_FinishedItemDetail vf on srd.Rfinishedid=vf.ITEM_FINISHED_ID Where 1=1 and (Recqty+lossqty+undyedqty)>0";
//                    }

//                }
//                if (DDCompany.SelectedIndex > 0)
//                {
//                    if (variable.JoborderNewModule == "1")
//                    {
//                        str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
//                    }
//                    else
//                    {
//                        str = str + " And PRM.CompanyId=" + DDCompany.SelectedValue;
//                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                        {
//                            strsample = strsample + " And SRM.CompanyId=" + DDCompany.SelectedValue;
//                        }
//                    }

//                }
//                //if (DDCustCode.SelectedIndex > 0)
//                //{
//                //    str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
//                //}
//                if (DDOrderNo.SelectedIndex > 0)
//                {
//                    str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
//                }
//                if (DDProcessName.SelectedIndex > 0)
//                {
//                    str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = strsample + " And Sm.Processid=" + DDProcessName.SelectedValue;
//                    }
//                }
//                if (DDEmpName.SelectedIndex > 0)
//                {
//                    str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = strsample + " And Srm.EmpId=" + DDEmpName.SelectedValue;
//                    }

//                }
//                if (DDIndentNo.SelectedIndex > 0)
//                {
//                    if (chksample.Checked == true)
//                    {
//                        strsample = strsample + "  And sm.id=" + DDIndentNo.SelectedValue;
//                        str = str + "  And IM.IndentId=0";
//                    }
//                    else
//                    {
//                        str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
//                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                        {
//                            strsample = strsample + "  And sm.id=0";
//                        }
//                    }

//                }
//                else
//                {
//                    if (chksample.Checked == true)
//                    {
//                        str = str + "  And IM.IndentId=0";
//                    }
//                }
//                //if (TRPartyChallanNo.Visible == true)
//                //{
//                //    if (txtPartyChallanNo.Text != "")
//                //    {
//                //        str = str + "  And PM.ChallanNo='" + txtPartyChallanNo.Text + "'";
//                //        strsample = strsample + "  And SRM.ChallanNo='" + txtPartyChallanNo.Text + "'";
//                //    }
//                //}
//                if (ddItemName.SelectedIndex > 0)
//                {
//                    str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
//                    }
//                }
//                if (DDQuality.SelectedIndex > 0)
//                {
//                    str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
//                    }
//                }
//                if (DDShadeColor.SelectedIndex > 0)
//                {
//                    str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
//                    }
//                }
//                if (ChkForDate.Checked == true)
//                {
//                    str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
//                    strsample = strsample + "  And Srm.Receivedate>='" + TxtFromDate.Text + "' And srm.Receivedate<='" + TxtToDate.Text + "'";
//                }
//                if (variable.JoborderNewModule == "1")
//                {
//                    str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process ";
//                }
//                else
//                {
//                    str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PRM.ChallanNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process ";
//                    strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Srd.TagNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID";

//                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
//                    {
//                        str = str + " UNION ALL " + strsample;
//                    }
//                }

//                if (Session["WithoutBOM"].ToString() == "1")
//                {
//                    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
//                }

//                switch (Session["varcompanyid"].ToString())
//                {
//                    case "6":
//                    case "12":
//                        Session["rptFilename"] = "Reports/RptIndentRawRecDetailIndentWiseArtindia.rpt";
//                        break;
//                    default:
//                        Session["rptFilename"] = "Reports/RptIndentRawRecDetailIndentWise.rpt";
//                        break;

//                }
//                Session["dsFilename"] = "~\\ReportSchema\\RptIndentRawRecDetailIndentWise.xsd";


//                ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
//                Session["GetDataset"] = ds;
//            }           
           
//            DataSet DS = (DataSet)(Session["GetDataset"]);
//            if (DS.Tables[0].Rows.Count > 0)
//            {
//                Session["GetDataset"] = DS;
//                StringBuilder stb = new StringBuilder();
//                stb.Append("<script>");
//                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//            }
//            else
//            {
//                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
//            }
//            Tran.Commit();
//        }
//        catch (Exception ex)
//        {
//            Tran.Rollback();
//            MessageSave(ex.Message);
//        }
//        finally
//        {
//            con.Close();
//            con.Dispose();
//        }

    }
    protected void IndentRecDetail()
    {
        try
        {            
            string Where = "";
            string WhereSample = "";           

            if (DDCompany.SelectedIndex > 0)
            {                
                    Where = Where + " And PRM.CompanyId=" + DDCompany.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        WhereSample = WhereSample + " And SRM.CompanyId=" + DDCompany.SelectedValue;
                    }               

            }           
            if (DDOrderNo.SelectedIndex > 0)
            {
                Where = Where + " And PT.CustomerOrderId=" + DDOrderNo.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                Where = Where + " And PM.Processid=" + DDProcessName.SelectedValue;
                if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                {
                    WhereSample = WhereSample + " And Sm.Processid=" + DDProcessName.SelectedValue;
                }
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                Where = Where + " And PM.EmpId=" + DDEmpName.SelectedValue;
                if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                {
                    WhereSample = WhereSample + " And Srm.EmpId=" + DDEmpName.SelectedValue;
                }

            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    WhereSample = WhereSample + "  And sm.id=" + DDIndentNo.SelectedValue;
                    Where = Where + "  And IM.IndentId=0";
                }
                else
                {
                    Where = Where + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        WhereSample = WhereSample + "  And sm.id=0";
                    }
                }

            }
            else
            {
                if (chksample.Checked == true)
                {
                    Where = Where + "  And IM.IndentId=0";
                }
            }
            //if (TRPartyChallanNo.Visible == true)
            //{
            //    if (txtPartyChallanNo.Text != "")
            //    {
            //        str = str + "  And PM.ChallanNo='" + txtPartyChallanNo.Text + "'";
            //        strsample = strsample + "  And SRM.ChallanNo='" + txtPartyChallanNo.Text + "'";
            //    }
            //}
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + "  and VF.Item_id=" + ddItemName.SelectedValue;
                if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                {
                    WhereSample = WhereSample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                }
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                {
                    WhereSample = WhereSample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                }
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                Where = Where + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                {
                    WhereSample = WhereSample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                }
            }
            if (ChkForDate.Checked == true)
            {
                Where = Where + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                WhereSample = WhereSample + "  And Srm.Receivedate>='" + TxtFromDate.Text + "' And srm.Receivedate<='" + TxtToDate.Text + "'";
            }
            //if (variable.JoborderNewModule == "1")
            //{
            //    str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process ";
            //}
            //else
            //{
            //    str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PRM.ChallanNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process ";
            //    strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Srd.TagNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID";

            //    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
            //    {
            //        str = str + " UNION ALL " + strsample;
            //    }
            //}

            //if (Session["WithoutBOM"].ToString() == "1")
            //{
            //    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
            //}


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_IndentRecDetailWithOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@WhereSample", WhereSample);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@ChkForSample", chksample.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {               
                Session["rptFilename"] = "Reports/RptIndentRawRecDetailOrderWiseSamara.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawRecDetailOrderWiseSamara.xsd";                
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
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
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCustomerOrder();
        //if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        //{
        //    DDCustCode_SelectedIndexChanged(sender, new EventArgs());
        //}
        //UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void BindCustomerOrder()
    {
        string str = "";
        str = @"Select distinct PPC.OrderId,LocalOrder+' / '+customerorderno as OrderNo From PP_Consumption PPC 
                JOIN PP_ProcessRecTran PRT ON PPC.OrderID=PRT.CustomerOrderId
                JOIN OrderMaster OM ON OM.OrderId=PPC.OrderId 
                Where  OM.CompanyId=" +DDCompany.SelectedValue+ " and OM.Status=0 and PPC.MasterCompanyId=" + Session["varCompanyId"] + "";

        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
    }

//    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string str = "", str1 = "";
//        if (Session["varcompanyid"].ToString() == "16")
//        {
//            str = @"Select distinct OM.OrderId, CustomerOrderNo as CustomerOrderNo 
//                From OrderMaster OM 
//                Where OM.Status=0";
//        }
//        else
//        {
//            str = @"Select distinct OM.OrderId, LocalOrder+ ' / ' +CustomerOrderNo as CustomerOrderNo 
//                From OrderMaster OM 
//                join V_Indent_OredrId VO ON Om.OrderId=VO.Orderid Where OM.Status=0";
//        }
//        str1 = @" Select Distinct PP.PPID, PP.CHALLANNO  
//                From OrderMaster OM 
//                JOIN ProcessProgram PP ON PP.Order_ID = OM.OrderID 
//                Where OM.Status = 0";

//        if (DDCompany.SelectedIndex > 0)
//        {
//            str = str + " And OM.companyid=" + DDCompany.SelectedValue;
//            str1= str1 + " And OM.CompanyId = " + DDCompany.SelectedValue;
//        }
//        if (DDCustCode.SelectedIndex > 0)
//        {
//            str = str + " And Om.customerid=" + DDCustCode.SelectedValue;
//            str = str + " And OM.CustomerId = " + DDCustCode.SelectedValue;
//        }
//        str = str + " Order By CustomerOrderNo";
//        str1 = str1 + " Order BY PP.PPID Desc";

//        if (variable.JoborderNewModule == "1")
//        {
//            str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
//        }
//        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
//        UtilityModule.ConditionalComboFill(ref DDProcessProgram, str1, true, "--Select--");

//    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
//        string str = string.Empty;
//        str = @"select Distinct PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME from IndentMaster IM inner join V_Indent_OredrId VO on Im.IndentID=VO.IndentId
//                inner join PROCESS_NAME_MASTER PNM on PNM.PROCESS_NAME_ID=IM.ProcessID Where PNM.mastercompanyId=" + Session["varcompanyid"];
//        if (DDCompany.SelectedIndex > 0)
//        {
//            str = str + "  and IM.Companyid=" + DDCompany.SelectedValue;
//        }
//        if (DDOrderNo.SelectedIndex > 0)
//        {
//            str = str + "  and VO.orderid=" + DDOrderNo.SelectedValue;
//        }
//        str = str + "  order by PNM.PROCESS_NAME";

//        if (variable.JoborderNewModule == "1")
//        {
//            str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
//        }

        string str = string.Empty;
        str = @"Select Distinct PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PP_ProcessRecMaster PRM JOIN PP_ProcessRecTran PRT ON PRM.PRMID=PRT.PRMID
                JOIN PROCESS_NAME_MASTER PNM ON PNM.PROCESS_NAME_ID=PRM.ProcessID 
                Where PNM.mastercompanyId=" + Session["varcompanyid"];
        if (DDCompany.SelectedIndex > 0)
        {
            str = str + "  and  PRM.CompanyID=" + DDCompany.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + "  and PRT.CustomerOrderId=" + DDOrderNo.SelectedValue;
        }
        str = str + "  order by PNM.PROCESS_NAME";

        //if (variable.JoborderNewModule == "1")
        //{
        //    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
        //}
        UtilityModule.ConditionalComboFill(ref DDProcessName, str, true, "--Select--");
    }

    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Q.QualityId,Q.QualityName From Quality Q inner Join ITEM_MASTER IM on Q.Item_Id=IM.ITEM_ID
                    inner join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid	 and cs.id=1 ";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and Im.Item_id=" + ddItemName.SelectedValue;
        }
        str = str + " order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---ALL---");
        if (DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedIndex = 0;
            DDQuality_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName From V_finisheditemdetail VF Where Vf.ShadeColorName<>''";

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.QualityId=" + DDQuality.SelectedValue;
        }
        str = str + " order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDShadeColor, str, true, "---Select---");
    }
    protected void chksample_CheckedChanged(object sender, EventArgs e)
    {
        FillIndentNo();
    }
    
    //protected void ChkForExportExcel_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (ChkForDate.Checked == true)
    //    {
    //        trDates.Visible = true;
    //    }
    //    else
    //    {
    //        trDates.Visible = false;
    //    }
    //}
    
   
   
    
}