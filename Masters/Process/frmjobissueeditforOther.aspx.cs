using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Process_frmjobissueeditforOther : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                         select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                         select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                        WHere PNM.ProcessType=1 and PNM.PROCESS_NAME_ID<>1 order by PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 1, true, "---Plz Select---");
            if (ddUnits.Items.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    if (Session["CurrentWorkingCompanyID"].ToString() == "2")
                    {
                        ddUnits.SelectedIndex = 2;
                    }
                    else
                    {
                        ddUnits.SelectedIndex = 1;
                    }
                }
                else
                {
                    ddUnits.SelectedIndex = 1;
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDTOProcess, ds, 2, true, "---Plz Select---");
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 16 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 44 || Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 43)
            {
                TDupdateemp.Visible = true;
                TDactiveemployee.Visible = true;
            }

            switch (Session["varcompanyNo"].ToString())
            {               
                case "38":
                    ChkForWithoutRate.Visible = true;
                    break;                
                default:
                    ChkForWithoutRate.Visible = false;
                    break;
            }
        }
    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        string str = "select Empid,Empname from empinfo Where EMpid='" + txtgetvalue.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
            {

                lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
            }
            txtWeaverIdNo.Text = "";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDissueno.SelectedValue;
        FillIssuedDetails();
        BtnStockNoToPNM.Visible = false;
        BtnPanipatPNM1.Visible = false;
        BtnPanipatPNM2.Visible = false;
        BtnChampoHome.Visible = false;

        if (DDissueno.SelectedIndex > 0)
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                case "28":
                     BtnStockNoToPNM.Visible = true;
                     BtnPanipatPNM1.Visible = true;
                     BtnPanipatPNM2.Visible = true;
                     BtnChampoHome.Visible = true;
                    break;
                case "44":
                    BtnStockNoToPNM.Visible = false;
                     BtnPanipatPNM1.Visible = false;
                     BtnPanipatPNM2.Visible = false;
                     BtnChampoHome.Visible = false;
                    break;  
                default:
                     BtnStockNoToPNM.Visible = false;
                     BtnPanipatPNM1.Visible = false;
                     BtnPanipatPNM2.Visible = false;
                     BtnChampoHome.Visible = false;
                    break;
            }
            
           
        }
    }
    protected void FillIssuedDetails()
    {
        DataSet ds;
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@Unitid", ddUnits.SelectedValue);
        param[2] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
        param[3] = new SqlParameter("@Issueorderid", DDissueno.SelectedValue);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetNextissueDetailForOther]", param);

        DGDetail.DataSource = ds;
        DGDetail.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtTotalPcs.Text = ds.Tables[0].Compute("Count(stockno)", "").ToString();

            if (Session["VarCompanyNo"].ToString() == "43")
            {
                TxtArea.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("Sum(Area)", "")), 2).ToString();
                TxtAmount.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("Sum(Amount)", "")),2).ToString();
            }
            else if (Session["VarCompanyNo"].ToString() == "44")
            {
                txtremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            
            }
            else
            {
                TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
                TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();
            }
          
        }
        else
        {
            TxtTotalPcs.Text = "";
            TxtArea.Text = "";
            TxtAmount.Text = "";
        }
        //********EmpDetail
        lstWeaverName.Items.Clear();
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    lstWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[i]["Empname"].ToString(), ds.Tables[1].Rows[i]["Empid"].ToString()));
                }
            }
        }
    }
    protected void lnkgetissueno_Click(object sender, EventArgs e)
    {
        FillissueNo();
    }
    protected void btnDeleteName_Click(object sender, EventArgs e)
    {
        lstWeaverName.Items.Remove(lstWeaverName.SelectedItem);
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            string lblissueorderid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueorderId")).Text;
            string lblissueDetailid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueDetailid")).Text;
            string lblProcessid = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblProcessid")).Text;
            string lblStockNo = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblStockNo")).Text;


            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@IssueOrderid", lblissueorderid);
            param[1] = new SqlParameter("@IssueDetailid", lblissueDetailid);
            param[2] = new SqlParameter("@Processid", lblProcessid);
            param[3] = new SqlParameter("@StockNo", lblStockNo);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextIssueForother", param);

            if (param[4].Value.ToString() != "")
            {

                lblmsg.Text = param[4].Value.ToString();
            }
            Tran.Commit();
            FillIssuedDetails();
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
    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDTOProcess.SelectedItem.Text.ToUpper())
        {
            case "THIRD BACKING":
            case "LATEXING":
                Trconsmpflag.Visible = true;
                break;
            default:
                Trconsmpflag.Visible = false;
                break;
        }
        txtWeaverIdNo_AutoCompleteExtender.ContextKey = DDTOProcess.SelectedValue;
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        lstWeaverName.Items.Clear();
        DDissueno.Items.Clear();
        hnissueorderid.Value = "0";
    }
    protected void FillissueNo()
    {
        string strempid = "";
        for (int i = 0; i < lstWeaverName.Items.Count; i++)
        {
            strempid = strempid == "" ? lstWeaverName.Items[i].Value : strempid + "," + lstWeaverName.Items[i].Value;
        }

        string str = @"select Distinct PIM.IssueOrderId,cast(PIM.ChallanNo as varchar) + ' # ' +REPLACE(convert(nvarchar(11),PIM.AssignDate,106),' ','-') 
                    From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock)  
                    Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID(NoLock) on PIM.IssueOrderId=PID.IssueOrderId";
        
        if (strempid != "")
        {
            str = str + " join Employee_ProcessOrderNo EMP(NoLock) ON PID.Issue_Detail_Id=EMP.IssueDetailId And PID.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId=" + DDTOProcess.SelectedValue;
            str = str + " And EMP.EMpid in (" + strempid + ")";
        }

        str = str + " Where PIM.CompanyId=" + DDCompanyName.SelectedValue + " and PIM.Units=" + ddUnits.SelectedValue;

        if (txtissueno.Text != "")
        {
            ////str = str + " and PIM.issueorderid='" + txtissueno.Text + "'";

            str = str + " and PIM.ChallanNo='" + txtissueno.Text + "'";
        }
        //if (strempid != "")
        //{
        //    str = str + " and EMP.EMpid in(" + strempid + ")";
        //}
        if (chkcomplete.Checked == true)
        {
            str = str + " and PIM.status='Complete'";
        }
        else
        {
            str = str + " and PIM.status='Pending'";
        }
        str = str + "  Order by PIM.IssueOrderId";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(str, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();        

        UtilityModule.ConditionalComboFillWithDS(ref DDissueno, ds, 0, true, "---Plz Select---");
    }
    protected void txtissueno_TextChanged(object sender, EventArgs e)
    {
        FillissueNo();
        if (DDissueno.Items.Count > 0)
        {
            DDissueno.SelectedIndex = 1;
            DDissueno_SelectedIndexChanged(sender, new EventArgs());
        }
        else
        {
            lblmsg.Text = "Please enter correct issue No.";
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //string str = "";
        //DataSet ds;
        try
        {
            //            str = @"select PIM.ChallanNo, Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
            //                        ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PIM.issueorderid
            //                        ,PIM.assigndate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
            //                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
            //                        PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
            //                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction,PID.Item_Finished_Id,
            //                        case when " + Session["varMasterCompanyIDForERP"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '''' end as FolioNo,
            //                        isnull(PID.JobIssueWeight,0) as JobIssueWeight
            //                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " PIM inner Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId
            //                        inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
            //                        inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
            //                        cross apply(select * From dbo.F_GetJobIssueEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.issueorderid)) EI
            //                        Where PIM.issueorderid=" + hnissueorderid.Value + " order by Issue_Detail_id";

//            str = @"Select isnull(PIM.ChallanNo,'') as ChallanNo, Ci.CompanyId,BM.BranchName CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, '' CompAddr3,BM.PhoneNo CompTel,CI.CompFax,CI.GSTNo
//                        ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PIM.issueorderid
//                        ,PIM.assigndate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
//                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
//                        PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
//                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction,PID.Item_Finished_Id,
//                        case when " + Session["varMasterCompanyIDForERP"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '' end as FolioNo,
//                        isnull(PID.JobIssueWeight,0) as JobIssueWeight, PID.Amount,PID.GSTTYPE,
//                        Case When PID.GSTTYPE=1 Then isnull(round(PID.AMOUNT*PID.CGST/100+  PID.AMOUNT*PID.SGST/100,3),0) else 0 end SGST,
//                        Case When PID.GSTTYPE=2 Then isnull(round(PID.AMOUNT*PID.IGST/100,2),0) else 0 end IGST,
//                        isnull(VF.HSNCode,'') as HSNCode,isnull(PIM.EWayBillNo,'') as EWayBillNo,
//                        (Select Distinct OM.CustomerOrderNo+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID1(NoLock) JOIN  OrderMaster OM(NoLock) ON PID1.OrderID=OM.OrderID 
//                            Where PIM.IssueOrderID=PID1.IssueOrderId and PID.ITEM_FINISHED_ID=PID1.Item_Finished_Id For XML PATH('')) as CustomerOrderNo,
//                        (Select Distinct CustIn.CustomerCode+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID2(NoLock) JOIN  OrderMaster OM2(NoLock) ON OM2.OrderID = PID2.OrderID 
//                            JOIN CustomerInfo CustIn(NoLock) ON OM2.CustomerId=CustIn.CustomerId  Where PID2.IssueOrderId=PIM.IssueOrderID For XML PATH('')) as CustomerCode 
//                        ,isnull(NU.UserName,'') as UserName
//                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock) 
//                        Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID(NoLock) on PIM.IssueOrderId=PID.IssueOrderId
//                        JOIN BranchMaster BM(NoLock) ON BM.ID = PIM.BranchID 
//                        inner join CompanyInfo CI(NoLock) on BM.Companyid=CI.CompanyId
//                        inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
//                        cross apply(select * From dbo.F_GetJobIssueEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.issueorderid)) EI
//                        JOIN NewUserDetail NU(NoLock) ON PIM.UserId=NU.UserId
//                        Where PIM.issueorderid=" + hnissueorderid.Value + " order by Issue_Detail_id";


            
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_Get_Process_Issue_DetailForReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@IssueOrderID", hnissueorderid.Value);
        cmd.Parameters.AddWithValue("@ProcessID", DDTOProcess.SelectedValue);
        cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    if (ChkForWithoutRate.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryWithoutRate.rpt";
                    }
                    else
                    {
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "27":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryForAntique.rpt";
                                break;
                            case "30":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummarySamara.rpt";
                                break;
                            case "16":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary_barcode.rpt";
                                break;
                            case "28":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryWithHSN.rpt";
                                break;
                            case "44":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary_agni.rpt";
                                break;
                            default:
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary.rpt";
                                break;
                        }
                        // Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary.rpt";
                    }
                }
                else
                {

                    if (ChkForWithoutRate.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptNextissueNew2WithoutRate.rpt";
                    }
                    else
                    {
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "16":
                            case "28":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2_barcode.rpt";
                                break;
                            case "42":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2_VikramMirzapur.rpt";
                                break;
                            case "44":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2_agni.rpt";
                                break;
                            default:
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2.rpt";
                                break;

                        }
                    }

                    //Session["rptFileName"] = "~\\Reports\\RptNextissueNew2.rpt";
                }


                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextissueNew2.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {

            lblmsg.Text = ex.Message;
        }
        finally
        {

        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisreceived = (Label)e.Row.FindControl("lblisreceived");
            Label lblconsmpflag = (Label)e.Row.FindControl("lblconsmpflag");
            if (lblisreceived.Text == "Y")
            {
                LinkButton lnkdel = (LinkButton)e.Row.FindControl("lnkdel");
                e.Row.BackColor = System.Drawing.Color.Green;
                lnkdel.Visible = false;
            }

            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 43)
            {
                LinkButton lnkdel2 = (LinkButton)e.Row.FindControl("lnkdel");
                if (Session["usertype"].ToString() != "1")
                {
                    lnkdel2.Visible = false;
                }
                else
                {
                    if (lblisreceived.Text == "Y")
                    {
                        //LinkButton lnkdel = (LinkButton)e.Row.FindControl("lnkdel");
                        e.Row.BackColor = System.Drawing.Color.Green;
                        lnkdel2.Visible = false;
                    }
                }
            }
            

            switch (DDTOProcess.SelectedItem.Text.ToUpper())
            {
                case "THIRD BACKING":
                case "LATEXING":
                    if (lblconsmpflag.Text == "0")
                    {
                        e.Row.BackColor = System.Drawing.Color.Red;
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (DGDetail.Columns[i].HeaderText == "Bonus")
                {
                    if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 42)
                    {
                        DGDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGDetail.Columns[i].Visible = false;
                    }
                }
            }

        }
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        FillissueNo();
        DGDetail.DataSource = null;
        DGDetail.DataBind();

    }
    protected void btnupdateconsmp_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@issueorderid", DDissueno.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_NEXTISSUECONSUMPTIONUPDATE", param);
            lblmsg.Text = param[2].Value.ToString();
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

    protected void btnupdateemp_Click(object sender, EventArgs e)
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
            //Employeedetail
            DataTable dtrecord = new DataTable();
            dtrecord.Columns.Add("processid", typeof(int));
            dtrecord.Columns.Add("issueorderid", typeof(int));
            dtrecord.Columns.Add("issuedetailid", typeof(int));
            dtrecord.Columns.Add("empid", typeof(int));
            for (int i = 0; i < lstWeaverName.Items.Count; i++)
            {
                for (int j = 0; j < DGDetail.Rows.Count; j++)
                {
                    Label lblissueorderid = ((Label)DGDetail.Rows[j].FindControl("lblIssueorderId"));
                    Label lblissuedetailid = ((Label)DGDetail.Rows[j].FindControl("lblIssueDetailid"));

                    DataRow dr = dtrecord.NewRow();
                    dr["processid"] = DDTOProcess.SelectedValue;
                    dr["issueorderid"] = lblissueorderid.Text;
                    dr["issuedetailid"] = lblissuedetailid.Text;
                    dr["empid"] = lstWeaverName.Items[i].Value;
                    dtrecord.Rows.Add(dr);
                }
            }
            
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", hnissueorderid.Value);
            param[1] = new SqlParameter("@processid", DDTOProcess.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@dtrecord", dtrecord);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFolioEmployee", param);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[3].Value.ToString() + "')", true);

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
    protected void btnactiveemployee_Click(object sender, EventArgs e)
    {
        lblpopupmsg.Text = "";
        FillEmployeeForDeactive();
        ModalpopupextDeactivefolio.Show();
    }

    protected void FillEmployeeForDeactive()
    {
        string str = @"select Distinct EI.EmpName+'('+EI.EmpCode+')' as Employee,EMP.IssueOrderId,Emp.ActiveStatus,Ei.Empid 
                From Employee_ProcessOrderNo EMP(Nolock) 
                inner Join EmpInfo EI(Nolock) on Emp.Empid=Ei.EmpId and EMP.ProcessId=" + DDTOProcess.SelectedValue + " and EMP.IssueOrderId=" + DDissueno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
    }
    protected void btnemployeesave_Click(object sender, EventArgs e)
    {
        lblpopupmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            DataTable dtrecord = new DataTable();
            dtrecord.Columns.Add("empid", typeof(int));
            dtrecord.Columns.Add("activestatus", typeof(int));
            dtrecord.Columns.Add("processid", typeof(int));
            dtrecord.Columns.Add("issueorderid", typeof(int));

            for (int i = 0; i < GVDetail.Rows.Count; i++)
            {
                Label lblempid = ((Label)GVDetail.Rows[i].FindControl("lblempid"));
                Label lblactivestatus = ((Label)GVDetail.Rows[i].FindControl("lblactivestatus"));
                CheckBox Chkboxitem = ((CheckBox)GVDetail.Rows[i].FindControl("Chkboxitem"));
                DataRow dr = dtrecord.NewRow();
                dr["empid"] = lblempid.Text;
                dr["activestatus"] = Chkboxitem.Checked == true ? 0 : 1;
                dr["Processid"] = DDTOProcess.SelectedValue;
                dr["issueorderid"] = DDissueno.SelectedValue;
                dtrecord.Rows.Add(dr);
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", DDissueno.SelectedValue);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@dtrecord", dtrecord);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFolioActiveStatus", param);
            Tran.Commit();
            lblpopupmsg.Text = param[3].Value.ToString();
            FillEmployeeForDeactive();
            ModalpopupextDeactivefolio.Show();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblpopupmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void GVDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox Chkboxitem = (CheckBox)e.Row.FindControl("Chkboxitem");
            Label lblactivestatus = (Label)e.Row.FindControl("lblactivestatus");
            if (lblactivestatus.Text == "1")
            {
                Chkboxitem.Checked = false;
            }
            else
            {
                Chkboxitem.Checked = true;
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void BtnStockNoToPNM_Click(object sender, EventArgs e)
    {
        TrasferStockNoToPNM(1);
    }
    protected void BtnStockNoToChampoPanipatPNM1_Click(object sender, EventArgs e)
    {
        TrasferStockNoToPNM(2);
    }
    protected void BtnStockNoToChampoPanipatPNM2_Click(object sender, EventArgs e)
    {
        TrasferStockNoToPNM(3);
    }
    protected void BtnStockNoToChampoPanipatPNM3_Click(object sender, EventArgs e)
    {
        TrasferStockNoToPNM(4);
    }
    private void TrasferStockNoToPNM(int TypeFlag)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@ISSUEORDERID", DDissueno.SelectedValue);
            param[1] = new SqlParameter("@ProcessID", DDTOProcess.SelectedValue);
            param[2] = new SqlParameter("@USERID", 1);
            param[3] = new SqlParameter("@MASTERCOMPANYID", 28);
            param[4] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@TYPEFLAG", TypeFlag);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Save_StockNo_InPNMERP", param);
            if (param[4].Value.ToString() == "Successfully Inserted")
            {
                Tran.Commit();
            }
            else
            {
                Tran.Rollback();
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[4].Value + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + ex.Message + "')", true);
            Tran.Rollback();
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
}