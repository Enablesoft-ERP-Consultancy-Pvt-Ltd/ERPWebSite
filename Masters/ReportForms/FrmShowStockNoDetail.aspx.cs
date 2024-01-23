using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
public partial class Masters_Campany_FrmShowStockNoDetail : CustomPage
{

    private const string SCRIPT_DOFOCUS =
  @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Campany_FrmShowStockNoDetail),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
            {
                DGStock.Columns[11].Visible = true;
                DGStock.Columns[12].Visible = true;
                trStockRemark.Visible = true;
            }

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                btnpack.Visible = true;
                trStockRemark.Visible = true;
                BtnSaveRemark.Visible = false;
            }

            txtStockNo.Text = "";
            txtStockNo.Focus();
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarNum = 0;
            if (txtStockNo.Text != "")
            {
                string StrNew = "";
                string[] Str = txtStockNo.Text.Split(',');

                foreach (string arrStr in Str)
                {
                    if (VarNum == 0)
                    {
                        StrNew = "'" + arrStr + "'";
                        VarNum = 1;
                    }
                    else
                    {
                        StrNew = StrNew + "," + "'" + arrStr + "'";
                    }
                }

                DataSet Ds;

                int ChkTrue = 0;
                if (chkForBazarSize.Checked == true)
                {
                    ChkTrue = 1;
                }

                string Query = @"select '0|0|0|0|0' Stockno,CN.Tstockno,'-' as Empname,P.TInvoiceNo as IssueChallanNo,Replace(convert(nvarchar(11),P.PackingDate,106),' ','-') as orderDate,
                                '-' as RecchallanNo,'-' as Receivedate,CN.CompanyId,0,0,0,0,CN.Pack,'STOCK OUT' as Process_name,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeFt  Description,
                                isnull(CC.customercode,'') as customercode,isnull(OM.customerorderNO,'') as customerorderNO,999999999999999999999999 as ProcessDetailId,isnull(CN.PackingId,0) as PackingId ,0 as ToProcessid,
                                CN.Item_Finished_Id,isnull(OM.LocalOrder,'') as LocalOrder, 0 Penality, '' PenalityRemark ,0 as OnlyIssueOrderId
                                From carpetnumber CN(Nolock) 
                                inner Join packing P(Nolock) on CN.PackingID=P.PackingId
                                inner join V_FinishedItemDetail vf(Nolock) on CN.Item_Finished_Id=vf.ITEM_FINISHED_ID
                                left join OrderMaster Om(Nolock) on CN.OrderId=OM.OrderId
                                left join customerinfo CC(Nolock) on OM.CustomerId=CC.customerid 
                                Where CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And TSTOCKNO in (" + StrNew + @")
                                UNION ALL
                                Select Replace(cast(CN.StockNo as nvarchar)+'|'+Str(IssueDetailId)+'|'+Str(ReceiveDetailId)+'|'+Str(ToProcessId)+'|0',' ','')  StockNo,CN.TStockNo,
                                '' EmpName,'' IssueChallanNo,replace(convert(varchar(11),PSd.OrderDate,106), ' ','-')OrderDate,'' RecChallanNo,
                                Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,CN.CompanyId,FromProcessId,ToProcessId,ReceiveDetailId,IssueDetailId,Pack,
                                PM.PROCESS_NAME,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeFt + 
                                Case When PSD.ToProcessId = 1 Then 
                                        Case When " + ChkTrue + @" = 1 Then 
                                            Case When VF.MasterCompanyID in (16, 28) Then + ' (' + PRD.ActualWidth + 'X' + PRD.ActualLength + ') ' 
                                            Else + ' (' + PRD.Width + 'X' + PRD.Length + ') ' End 
                                        Else '' End Else '' End Description,
                                CC.customercode,OM.customerorderNO,psd.ProcessDetailId,isnull(CN.PackingId,0) as PackingId,PSd.Toprocessid,CN.Item_Finished_Id,
                                isnull(OM.LocalOrder,'') as LocalOrder, 
                                Case When PSD.ToProcessId = 1 Then PRD.Penality Else 0 End Penality, Case When PSD.ToProcessId = 1 Then PRD.PRemarks Else '' End PenalityRemark,0 as OnlyIssueOrderId 
                                From CarpetNumber CN(Nolock) 
                                inner join V_FinishedItemDetail VF(Nolock) on cn.Item_Finished_Id=vf.ITEM_FINISHED_ID 
                                left outer join Process_Stock_Detail PSD(Nolock) on PSD.StockNo=CN.StockNo  
                                left join Process_Name_Master PM(Nolock) on PSD.ToProcessId=PM.PROCESS_NAME_ID 
                                Left Join ordermaster OM(Nolock) on CN.orderid=OM.orderid 
                                left join Customerinfo CC(Nolock) on OM.customerid=CC.customerid 
                                left join PROCESS_RECEIVE_DETAIL_1 PRD(Nolock) ON PRD.Process_Rec_Detail_Id = CN.Process_Rec_Detail_Id And PRD.Process_Rec_Id = CN.Process_Rec_id 
                                Where CN.Item_Finished_id=VF.Item_Finished_id And CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And TSTOCKNO in (" + StrNew + ") And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                                UNION ALL
                                select Replace(cast(ls.StockNo as nvarchar)+'|'+Str(ls.IssueDetailId)+'|'+Str(ls.Process_rec_detail_id)+'|'+Str(ls.ProcessId)+'|0',' ','')  StockNo,ls.TStockNo,
                                '' EmpName,'' IssueChallanNo,replace(convert(varchar(11),pim.AssignDate,106), ' ','-')OrderDate,'' RecChallanNo,
                                '' ReceiveDate,PIM.CompanyId,0 as FromProcessId,ls.ProcessId ToProcessId,ls.Process_rec_detail_id as  ReceiveDetailId,IssueDetailId,0 as Pack,
                                PNM.Process_Name,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeFt  Description,
                                CC.customercode,OM.customerorderNO,0 as ProcessDetailId,0 as PackingId,ls.ProcessId Toprocessid,ls.Item_Finished_Id, 
                                isnull(OM.LocalOrder,'') LocalOrder, 0 Penality, '' PenalityRemark ,0 as OnlyIssueOrderId
                                From LoomStockNo ls(Nolock) 
                                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = ls.ProcessId 
                                inner join V_FinishedItemDetail vf(Nolock) on ls.Item_Finished_id=vf.ITEM_FINISHED_ID
                                inner join OrderMaster om(Nolock) on ls.Orderid=om.OrderId 
                                inner join customerinfo cc(Nolock) on om.Customerid=cc.CustomerId
                                Left join PROCESS_ISSUE_MASTER_1 pim(Nolock) on ls.Issueorderid=pim.IssueOrderId
                                Left join PROCESS_ISSUE_DETAIL_1 PID(Nolock) on ls.IssueDetailid=pid.Issue_Detail_Id and pim.issueorderid=pid.IssueOrderId
                                Where Ls.bazarstatus=0 And --ls.ProcessID = 1 And 
                                ls.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And Ls.TSTOCKNO in (" + StrNew + @") 
                                UNION ALL
                                Select Replace(cast(ls.StockNo as nvarchar) + '|' + Str(HSD.IssueDetailId) + '|' + Str(HSD.Process_rec_detail_id) + '|' + Str(HSD.ToProcessId)+'|1', ' ', '')  StockNo, 
                                ls.TStockNo, 
                                (Select EI.EmpName + ', ' 
	                                From Employee_HomeFurnishingOrderMaster EHDOM(Nolock)
	                                JOIN Empinfo EI ON EI.EmpID = EHDOM.EmpID
	                                Where EHDOM.ProcessID = pim.PROCESSID And EHDOM.IssueOrderID = HSD.IssueorderID And EHDOM.IssueDetailID = HSD.IssueDetailID For xml path('')) EmpName, 
	                                PIM.CHALLANNO IssueChallanNo, replace(convert(varchar(11), pim.AssignDate, 106), ' ', '-') OrderDate, prm.ChallanNo RecChallanNo, 
                                replace(convert(varchar(11), prm.ReceiveDate, 106), ' ', '-') ReceiveDate, PIM.CompanyId, HSD.FromProcessId, HSD.ToProcessId, HSD.Process_rec_detail_id ReceiveDetailId, 
                                HSD.IssueDetailId, 0 Pack, PNM.PROCESS_NAME, VF.Item_Name + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt Description, 
                                CC.customercode, OM.customerorderNO, 0 ProcessDetailId, 0 PackingId, HSD.Toprocessid, ls.OrderDetailDetail_FinishedID Item_Finished_Id, 
                                isnull(OM.LocalOrder, '') LocalOrder, 0 Penality, '' PenalityRemark ,0 as OnlyIssueOrderId
                                From HomeFurnishingStockNo ls(Nolock) 
                                JOIN HomeFurnishing_Stock_Detail HSD(Nolock) ON HSD.StockNo = ls.StockNo 
                                inner join V_FinishedItemDetail vf(Nolock) on ls.OrderDetailDetail_FinishedID = vf.ITEM_FINISHED_ID
                                inner join OrderMaster om(Nolock) on ls.Orderid = om.OrderId 
                                inner join customerinfo cc(Nolock) on om.Customerid = cc.CustomerId
                                join HomeFurnishingOrderMaster pim(Nolock) on HSD.Issueorderid = pim.IssueOrderId --And pim.ProcessID = 1 
                                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = pim.PROCESSID 
                                Left join HomeFurnishingReceiveMaster prm(Nolock) on HSD.Process_Rec_ID = prm.ProcessRecId --And prm.ProcessID = 1 
                                Where ls.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And Ls.TSTOCKNO in (" + StrNew + @") 
                                UNION ALL
                                select '0|0|0|0|0' as Stockno,CN.Tstockno,'-' as Empname, SOWC.IssueNo IssueChallanNo, Replace(convert(nvarchar(11), SOWC.IssueDate, 106), ' ', '-') orderDate,
                                '-' as RecchallanNo,'-' as Receivedate,CN.CompanyId,0,0,0,0,CN.Pack,'STOCK OUT' as Process_name,VF.Item_Name+' '+VF.QualityName+' '+VF.DesignName+' '+VF.ColorName+' '+VF.ShapeName+' '+VF.SizeFt  Description,
                                isnull(CC.customercode,'') as customercode,isnull(OM.customerorderNO,'') as customerorderNO,999999999999999999999999 as ProcessDetailId,isnull(CN.PackingId,0) as PackingId ,0 as ToProcessid,
                                CN.Item_Finished_Id,isnull(OM.LocalOrder,'') as LocalOrder, 0 Penality, '' PenalityRemark ,0 as OnlyIssueOrderId
                                From carpetnumber CN(Nolock) 
                                JOIN StockNoOutWardChallan SOWC(Nolock) ON SOWC.StockNo = CN.StockNo 
                                inner join V_FinishedItemDetail vf(Nolock) on CN.Item_Finished_Id=vf.ITEM_FINISHED_ID
                                left join OrderMaster Om(Nolock) on CN.OrderId=OM.OrderId
                                left join customerinfo CC(Nolock) on OM.CustomerId=CC.customerid 
                                Where CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And CN.TSTOCKNO in (" + StrNew + @")
                                UNION ALL 
                                Select '0|0|0|0|0' Stockno,CN.Tstockno, '-' Empname, Cast(MD.ID as Nvarchar) IssueChallanNo, Replace(convert(nvarchar(11), MD.SCANDATE, 106), ' ', '-') orderDate, 
                                '-' RecchallanNo, '-' Receivedate, CN.CompanyId, 0, 0, 0, 0, CN.Pack,'METAL DETECTION' Process_name, 
                                VF.Item_Name + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt  Description, 
                                IsNull(CC.customercode, '') customercode, IsNull(OM.customerorderNO, '') customerorderNO, 999999999999999999999999 ProcessDetailId, 
                                IsNull(CN.PackingId, 0) PackingId, 0 ToProcessid, CN.Item_Finished_Id, isnull(OM.LocalOrder, '') LocalOrder, 0 Penality, '' PenalityRemark,0 as OnlyIssueOrderId 
                                From carpetnumber CN(Nolock) 
                                JOIN METALDETECTION MD(Nolock) ON MD.StockNo = CN.StockNo 
                                inner join V_FinishedItemDetail vf(Nolock) on CN.Item_Finished_Id = vf.ITEM_FINISHED_ID
                                left join OrderMaster Om(Nolock) on CN.OrderId=OM.OrderId
                                left join customerinfo CC(Nolock) on OM.CustomerId=CC.customerid 
                                Where CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And CN.TSTOCKNO in (" + StrNew + @") 
                                Order by ProcessDetailId";

                                //Select Replace(cast(ls.StockNo as nvarchar) + '|' + Str(0) + '|' + Str(0) + '|' + Str(1), ' ', '')  StockNo, 
                                //ls.TStockNo, 
                                //(Select EI.EmpName + ', ' 
                                //    From Employee_HomeFurnishingOrderMaster EHDOM(Nolock)
                                //    JOIN Empinfo EI ON EI.EmpID = EHDOM.EmpID
                                //    Where EHDOM.ProcessID = pim.PROCESSID And EHDOM.IssueOrderID = ls.IssueorderID And EHDOM.IssueDetailID = ls.IssueDetailID For xml path('')) EmpName, 
                                //    PIM.CHALLANNO IssueChallanNo, replace(convert(varchar(11), pim.AssignDate, 106), ' ', '-') OrderDate, prm.ChallanNo RecChallanNo, 
                                //replace(convert(varchar(11), prm.ReceiveDate, 106), ' ', '-') ReceiveDate, PIM.CompanyId, 0 as FromProcessId, 1 as ToProcessId, ls.Process_rec_detail_id ReceiveDetailId, ls.IssueDetailId, 
                                //0 Pack, 'WEAVING' Process_Name, VF.Item_Name + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt Description, 
                                //CC.customercode, OM.customerorderNO, 0 ProcessDetailId, 0 PackingId, 1 Toprocessid, ls.OrderDetailDetail_FinishedID Item_Finished_Id, 
                                //isnull(OM.LocalOrder, '') LocalOrder, 0 Penality, '' PenalityRemark 
                                //From HomeFurnishingStockNo ls(Nolock) 
                                //inner join V_FinishedItemDetail vf on ls.OrderDetailDetail_FinishedID = vf.ITEM_FINISHED_ID
                                //inner join OrderMaster om on ls.Orderid = om.OrderId 
                                //inner join customerinfo cc on om.Customerid = cc.CustomerId
                                //join HomeFurnishingOrderMaster pim on ls.Issueorderid = pim.IssueOrderId And pim.ProcessID = 1 
                                //Left join HomeFurnishingReceiveMaster prm on ls.Process_Rec_ID = prm.ProcessRecId And prm.ProcessID = 1 
                                //Where ls.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And Ls.TSTOCKNO in (" + StrNew + @") 
                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Query);

                //**********Confirm
                lblmsg.Text = "";
                btnconfirm.Visible = false;
                btnPreview.Visible = false;
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    btnPreview.Visible = true;
                    if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Packingid"]) != 0)
                    {
                        if (Session["usertype"].ToString() == "1")
                        {
                            btnconfirm.Visible = true;
                        }
                    }
                    if (Session["varcompanyNo"].ToString() == "28" && Session["usertype"].ToString() == "1")
                    {
                        btnconfirm.Visible = true;
                    }
                }
                //***********

                DGStock.DataSource = Ds;
                DGStock.DataBind();

                VarNum = 0;
                for (int i = 0; i < DGStock.Rows.Count; i++)
                {
                    if (Convert.ToString(DGStock.Rows[i].Cells[1].Text) != "&nbsp;" && Convert.ToString(DGStock.Rows[i].Cells[1].Text) != "STOCK OUT")
                    {
                        StrNew = DGStock.DataKeys[i].Value.ToString();
                        int VarIssueDetailId = Convert.ToInt32(StrNew.Split('|')[1]);
                        int VarReceiveDetailId = Convert.ToInt32(StrNew.Split('|')[2]);
                        int VarProcessId = Convert.ToInt32(StrNew.Split('|')[3]);
                        int VarTypeFlag = Convert.ToInt32(StrNew.Split('|')[4]);

                        if (VarIssueDetailId > 0 && VarTypeFlag == 0)
                        {
                            if (variable.VarFinishingNewModuleWise == "1")
                            {
                                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text,
                                            @"select Distinct replace(convert(varchar(11),PIM.AssignDate,106), ' ','-') AssignDate,isnull(PIM.ChallanNo,PIM.IssueOrderId) as IssueOrderId,
                                            Case when EI.EMPID is null Then dbo.F_getFolioEmployeeNew_IssueDetailIdWise(PIM.IssueOrderId," + VarProcessId + @",PID.Issue_Detail_id) Else EI.EMpName End as Empname,
                                            PIM.IssueOrderId as OnlyIssueOrderId 
                                            From process_issue_Master_" + VarProcessId + @" PIM 
                                            inner Join PROCESS_ISSUE_DETAIL_" + VarProcessId + @" PID on PIM.IssueOrderId=PID.IssueOrderId
                                            left join EmpInfo ei on Pim.Empid=ei.EmpId 
                                            Where PID.Issue_Detail_id=" + VarIssueDetailId + "");

                            }
                            else
                            {
                                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select replace(convert(varchar(11),PM.AssignDate,106), ' ','-') AssignDate,
                                isnull(PM.ChallanNo,PM.IssueOrderId) as IssueOrderId,EI.EmpName,PM.IssueOrderId as OnlyIssueOrderId 
                                From PROCESS_ISSUE_MASTER_" + VarProcessId + " PM,PROCESS_ISSUE_DETAIL_" + VarProcessId + @" PD,EmpInfo EI 
                                Where PM.IssueOrderId=PD.IssueOrderId And PM.EmpID=EI.EmpID And Issue_Detail_Id=" + VarIssueDetailId + @" And 
                                EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
                            }
                            if (Ds.Tables[0].Rows.Count > 0)
                            {

                                //if (VarProcessId == 1)
                                //{
                                //    DGStock.Rows[i].Cells[2].Text = "INTERNAL ORDER";
                                //}
                                //else
                                //{
                                    DGStock.Rows[i].Cells[2].Text = (Ds.Tables[0].Rows[0]["EmpName"]).ToString();
                                //}
                                DGStock.Rows[i].Cells[3].Text = (Ds.Tables[0].Rows[0]["IssueOrderId"]).ToString();
                                if (DGStock.Rows[i].Cells[4].Text != "")
                                {
                                    DGStock.Rows[i].Cells[4].Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
                                }

                                DGStock.Rows[i].Cells[16].Text = (Ds.Tables[0].Rows[0]["OnlyIssueOrderId"]).ToString();
                            }
                        }
                        if (VarReceiveDetailId > 0 && VarTypeFlag == 0)
                        {
                            //DGStock.Rows[i].Cells[5].Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Process_Rec_Id From PROCESS_Receive_DETAIL_" + VarProcessId + " Where Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();

                            DGStock.Rows[i].Cells[5].Text = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Distinct PRM.CHALLANNO From Process_Receive_Master_" + VarProcessId + " PRM(NoLock) JOIN PROCESS_RECEIVE_DETAIL_" + VarProcessId + " PRD(NoLock) ON PRM.PROCESS_REC_ID=PRD.PROCESS_REC_ID Where PRD.Process_Rec_Detail_Id=" + VarReceiveDetailId + "").ToString();
                        }
                    }
                }
            }
            if (DGStock.Rows.Count == 0)
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "No Records Found or Stock No. is not available";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmShowStockNoDetail.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int VarRawDetailShowOrNot = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
            LinkButton linkforseeDetail = e.Row.FindControl("linkforseeDetail") as LinkButton;
            Label lblprocessId = e.Row.FindControl("lblProcessId") as Label;

            if (lblprocessId.Text == "1")
            {
                VarRawDetailShowOrNot = 1;
            }
            if (lblprocessId.Text != "1")
            {
                linkforseeDetail.Visible = false;
            }
            if (lblprocessId.Text == "117" && VarRawDetailShowOrNot == 0)
            {
                linkforseeDetail.Visible = true;
            }
        }
    }
    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        BtnShow_Click(sender, e);
    }
    protected void lnkbtnName_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        LinkButton lnk = sender as LinkButton;

        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            hngridrowindex.Value = grv.RowIndex.ToString();            

            int IssueOrderId = Convert.ToInt32(DGStock.Rows[grv.RowIndex].Cells[16].Text);
            //int IssueOrderId = Convert.ToInt32(DGStock.Rows[grv.RowIndex].Cells[3].Text);
            int ProcessId = Convert.ToInt32(((Label)DGStock.Rows[grv.RowIndex].FindControl("lblProcessId")).Text);
            int Item_Finished_Id = Convert.ToInt32(((Label)DGStock.Rows[grv.RowIndex].FindControl("lblFinishedid")).Text);
            string VarStockNo = Convert.ToString(DGStock.Rows[grv.RowIndex].Cells[0].Text);

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@processid", ProcessId);
            param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
            param[2] = new SqlParameter("@issueorderid", IssueOrderId);
            param[3] = new SqlParameter("@TStockNo", VarStockNo);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawDetailForStockNo", param);

//            string str = "";

//            str = @"select Distinct ITEM_NAME,QualityName,ShadeColorName,Lotno 
//                From ProcessRawMaster PM 
//                inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
//                inner join V_FinishedItemDetail v on PT.Finishedid=v.ITEM_FINISHED_ID
//                inner join Process_issue_detail_" + ProcessId + @" PID on PM.Prorderid=PID.IssueOrderId 
//                inner join PROCESS_CONSUMPTION_DETAIL PCD on PM.Prorderid=PCD.ISSUEORDERID and PT.Finishedid=PCd.IFINISHEDID and 
//                        PCd.PROCESSID=" + ProcessId + @" And PID.Issue_Detail_Id=PCd.ISSUE_DETAIL_ID
//                Where PM.TypeFlag = 0 And PM.Processid=" + ProcessId + " and PM.Prorderid=" + IssueOrderId + " and PM.trantype=0 and PID.Item_Finished_Id=" + Item_Finished_Id;

//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            GDLinkedtoCustomer.DataSource = ds;
            GDLinkedtoCustomer.DataBind();
        }
    }
    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        #region
        //string str = "", msg = "";
        //DataSet ds = null;
        //string Tstockno = txtStockNo.Text;
        //string[] split = Tstockno.Split(',');
        //foreach (string item in split)
        //{
        //    str = @"select isnull(PackingId,0) as PackingId From carpetNumber Where Tstockno='" + item + "' and PackingId=999999999";
        //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update carpetnumber set Pack=0,Packingid=0,PackingDetailID=0 Where Tstockno='" + item + "'");
        //        msg = msg + " StockNo - " + item + " confirmed sucessfully";
        //    }
        //    else
        //    {
        //        msg = msg + " StockNo -" + item + " can not confirmed";
        //    }
        //}
        //lblmsg.Text = msg;
        #endregion
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //*******
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKCONFIRMED", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
        }

        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnprintstockrawdetail_Click(object sender, EventArgs e)
    {
        GetStockRawdetail();
        ModalPopupExtender1.Show();

    }
    protected void GetStockRawdetail()
    {
        
        int rowindex = Convert.ToInt16(hngridrowindex.Value);
        //int IssueOrderId = Convert.ToInt32(DGStock.Rows[rowindex].Cells[3].Text);
        int IssueOrderId = Convert.ToInt32(DGStock.Rows[rowindex].Cells[16].Text);
        int ProcessId = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblProcessId")).Text);
        int Item_Finished_Id = Convert.ToInt32(((Label)DGStock.Rows[rowindex].FindControl("lblFinishedid")).Text);
        string ReceiveDate = DGStock.Rows[rowindex].Cells[6].Text;
        //*****************
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@processid", ProcessId);
        param[1] = new SqlParameter("@Finishedid", Item_Finished_Id);
        param[2] = new SqlParameter("@issueorderid", IssueOrderId);
        param[3] = new SqlParameter("@Receivedate", ReceiveDate);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetStockRawdetailForOthers", param);

        //Export to excel
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;

        GridView1.DataSource = ds;
        GridView1.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=STOCKRAWDETAIL" + DateTime.Now + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            //Apply text style to each Row
            GridView1.Rows[i].Attributes.Add("class", "textmode");
        }
        GridView1.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (ChkForStockRawIssueDetail.Checked == true)
        {
            StockWiseRawMasterialIssueDetail();
        }
        else
        {
            Report();
        }
    }
    protected void StockWiseRawMasterialIssueDetail()
    {
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@processid", 1);
        param[1] = new SqlParameter("@Prmid", 0);
        param[2] = new SqlParameter("@TStockNo", txtStockNo.Text);
        param[3] = new SqlParameter("@Type", 1);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKNOWISEMATERIALISSUE", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\Rptstocknowisematerialissue.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rptstocknowisematerialissue.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void Report()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "STOCKNOSTATUSREORT" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        DGStock.GridLines = GridLines.Both;
        DGStock.HeaderStyle.Font.Bold = true;
        DGStock.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();
    }
    protected void BtnSaveRemark_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Remark", TxtStockNoRemark.Text);

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SAVE_UPDATE_STOCKNOREMARK", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
            txtStockNo.Text = "";
            TxtStockNoRemark.Text = "";
            txtStockNo.Focus();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void btnpack_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@TstockNo", txtStockNo.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Remark", TxtStockNoRemark.Text.Trim());
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DIRECTSTOCKPACK", param);
            if (param[3].Value.ToString() != "")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[3].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Stock No. packed successfully.";
                Tran.Commit();

            }


        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}