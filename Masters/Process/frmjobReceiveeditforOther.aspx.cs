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

public partial class Masters_Process_frmjobReceiveeditforOther : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
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
                ddUnits.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDTOProcess, ds, 2, true, "---Plz Select---");

            if (variable.VarQctype == "1")
            {
                btnQcPreview.Visible = true;
                btnqcreport.Visible = true;
            }


            if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
            {
                DGDetail.Columns[16].Visible = false;
                DGDetail.Columns[17].Visible = false;
               
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
        hnprocessrecid.Value = DDissueno.SelectedValue;
        FillIssuedDetails();
    }
    protected void FillIssuedDetails()
    {

        DataSet ds;
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@Unitid", ddUnits.SelectedValue);
        param[2] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
        param[3] = new SqlParameter("@Process_rec_id", DDissueno.SelectedValue);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetNextReceiveDetailForOther]", param);

        DGDetail.DataSource = ds.Tables[0];
        DGDetail.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtTotalPcs.Text = ds.Tables[0].Compute("Count(stockno)", "").ToString();
            TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
            TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();
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

    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDTOProcess.SelectedItem.Text == "WASHING BY WEIGHT")
        {
            TRTxtWeight.Visible = true;
        }
        txtWeaverIdNo_AutoCompleteExtender.ContextKey = DDTOProcess.SelectedValue;
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        hnprocessrecid.Value = "0";
    }
    protected void FillissueNo()
    {
        string strempid = "";
        for (int i = 0; i < lstWeaverName.Items.Count; i++)
        {
            strempid = strempid == "" ? lstWeaverName.Items[i].Value : strempid + "," + lstWeaverName.Items[i].Value;
        }

        string str = @"select Distinct PIM.Process_rec_id,cast(PIM.challanNo as varchar) + ' # ' +REPLACE(convert(nvarchar(11),PIM.Receivedate,106),' ','-') From PROCESS_Receive_MASTER_" + DDTOProcess.SelectedValue + @" PIM 
                        Inner Join PROCESS_Receive_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.Process_rec_id=PID.Process_rec_id
                        inner join Employee_ProcessReceiveNo EMP on PID.Process_rec_detail_id=EMP.Process_rec_detail_id and EMP.ProcessId=" + DDTOProcess.SelectedValue + @" 
                        inner join Process_issue_Master_" + DDTOProcess.SelectedValue + @" PI on PID.issueorderid=PI.issueorderid 
                        Where PIM.CompanyId=" + DDCompanyName.SelectedValue + " and PI.Units=" + ddUnits.SelectedValue;

        if (txtissueno.Text != "")
        {
            str = str + " and PIM.challanNo='" + txtissueno.Text + "'";
        }
        if (strempid != "")
        {
            str = str + " and EMP.EMpid in(" + strempid + ")";
        }
        if (chkcomplete.Checked == true)
        {
            str = str + " and PI.status='Complete'";
        }
        else
        {
            str = str + " and PI.status='Pending'";
        }
        str = str + "  Order by PIM.Process_rec_id";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "---Plz Select---");

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
        try
        {
            string str = @"Select Ci.CompanyId, BM.BranchName CompanyName, BM.BranchAddress CompAddr1,'' CompAddr2, '' CompAddr3,BM.PhoneNo CompTel,'' CompFax, BM.GSTNo 
                    ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' as Address3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PID.issueorderid
                    ,PIM.Receivedate,PIS.ReqByDate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
                    Vf.CATEGORY_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                    PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
                    (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + DDTOProcess.SelectedValue + @",PID.issue_detail_id)) TStockNo,PID.Item_Finished_Id
                    ,case when " + Session["varcompanyId"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '''' end as FolioNo
                    ,ISNULL(PID.stockNoRemarks,'') as StockNoRemarks,ISNULL(PID.ActualWidth,'') as ActualWidth,ISNULL(PID.ActualLength,'') as ActualLength,
                    isnull(PID.Weight,0) as Weight,isnull(PIM.PartyChallanNo,'') as PartyChallanNo," + Session["varcompanyId"].ToString() + @" as MasterCompanyId  
                    ,isnull(NU.UserName,'') as UserName,(Select Distinct OM.CustomerOrderNo+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID1(NoLock) JOIN  OrderMaster OM(NoLock) ON PID1.OrderID=OM.OrderID 
                            Where PID.IssueOrderID=PID1.IssueOrderId and PID.ITEM_FINISHED_ID=PID1.Item_Finished_Id For XML PATH('')) as CustomerOrderNo,
                        (Select Distinct CustIn.CustomerCode+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID2(NoLock) JOIN  OrderMaster OM2(NoLock) ON OM2.OrderID = PID2.OrderID 
                            JOIN CustomerInfo CustIn(NoLock) ON OM2.CustomerId=CustIn.CustomerId  Where PID2.IssueOrderId=PID.IssueOrderID For XML PATH('')) as CustomerCode,pim.Remarks
                    From PROCESS_Receive_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock) 
                    Join PROCESS_Receive_DETAIL_" + DDTOProcess.SelectedValue + @" PID(NoLock) on PIM.Process_Rec_Id=PID.Process_Rec_Id
                    Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PIS(NoLock) on PIS.IssueOrderId=PID.IssueOrderId AND PIS.Issue_Detail_Id=PID.Issue_Detail_Id
                    Join BranchMaster BM(Nolock) ON BM.ID = PIM.BranchID 
                    join CompanyInfo CI(NoLock) on PIM.Companyid=CI.CompanyId
                    cross apply(select * From dbo.F_GetJobReceiveEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.Process_rec_id)) EI
                    inner join V_FinishedItemDetail vf(NoLock) on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                    JOIN NewUserDetail NU(NoLock) ON PIM.UserId=NU.UserId
                    Where PIM.Process_rec_id=" + hnprocessrecid.Value + " order by Process_rec_detail_id";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    switch (Session["varCompanyId"].ToString())
                    {
                        case "27":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReportAntique.rpt";
                            break;
                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReport_barcode.rpt";
                            break;
                        case "44":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReportagni.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReport.rpt";
                            break;
                    }
                }
                else if (ChkForActualSize.Checked == true)
                {
                    switch (Session["varCompanyId"].ToString())
                    {
                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport_barcode.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport.rpt";
                            break;
                        // Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport.rpt";
                    }
                }
                else
                {
                    switch (Session["VarCompanyId"].ToString())
                    {
                        case "27":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2Antique.rpt";
                            break;
                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2_barcode.rpt";
                            break;
                        case "44":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2_agni.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                            break;

                    }
                    //Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                }

                //Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }

    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblpack = (Label)e.Row.FindControl("lblpack");
            LinkButton LinkButton1 = (LinkButton)e.Row.FindControl("lnkdel");
            if (lblpack.Text == "1")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
                LinkButton1.Visible = false;
            }

            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (DGDetail.Columns[i].HeaderText == "Bonus")
                {
                    if (Convert.ToInt32(Session["varcompanyId"]) == 42)
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
            Label lblissuedetailid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblissuedetailid");
            Label lblprocessid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblprcessid");
            Label lblrecdetailid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblrecdetailid");
            Label lblstockno = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblstockno");
            Label lblissueorderid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblprocessrecid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblprocessrecid");
            //*****************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@processid", lblprocessid.Text);
            param[1] = new SqlParameter("@issuedetailid", lblissuedetailid.Text);
            param[2] = new SqlParameter("@recdetailid", lblrecdetailid.Text);
            param[3] = new SqlParameter("@stockno", lblstockno.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[6] = new SqlParameter("@Processrecid", lblprocessrecid.Text);
            param[7] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[8] = new SqlParameter("@userid", Session["varuserid"]);
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobReceive_DeleteForOther", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            FillIssuedDetails();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        FillissueNo();
    }

    protected void DGDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGDetail.EditIndex = e.NewEditIndex;
        FillIssuedDetails();
    }
    protected void DGDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGDetail.EditIndex = -1;
        FillIssuedDetails();
    }
    protected void DGDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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

            Label lblprocessid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblprcessid");
            Label lblrecdetailid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblrecdetailid");
            TextBox txtgridactualL = (TextBox)DGDetail.Rows[e.RowIndex].FindControl("txtgridactualL");
            TextBox txtgridactualW = (TextBox)DGDetail.Rows[e.RowIndex].FindControl("txtgridactualW");
            TextBox txtpenamount = (TextBox)DGDetail.Rows[e.RowIndex].FindControl("txtpenamount");
            TextBox txtpenremarks = (TextBox)DGDetail.Rows[e.RowIndex].FindControl("txtpenremarks");
            //*****************
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@processid", lblprocessid.Text);
            param[1] = new SqlParameter("@recdetailid", lblrecdetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@ActualLength", txtgridactualL.Text.Trim());
            param[6] = new SqlParameter("@ActualWidth", txtgridactualW.Text.Trim());
            param[7] = new SqlParameter("@Penalityamt", txtpenamount.Text == "" ? "0" : txtpenamount.Text);
            param[8] = new SqlParameter("@PenalityRemark", txtpenremarks.Text.Trim());

            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_JobReceive_Update", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            DGDetail.EditIndex = -1;
            FillIssuedDetails();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void btnqcreport_Click(object sender, EventArgs e)
    {

        try
        {
            #region
            //            string str = @"select Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
            //                                ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PID.issueorderid
            //                                ,PIM.Receivedate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
            //                                Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
            //                                PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
            //                                (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + DDTOProcess.SelectedValue + @",PID.issue_detail_id)) TStockNo,
            //                                dbo.F_GETQCValueFinishing('" + DDTOProcess.SelectedItem.Text + "'," + DDTOProcess.SelectedValue + @",PIM.Process_rec_id,PID.Process_Rec_Detail_Id) as QCVALUE,
            //                                dbo.F_GETQCValueFinishingparaname('" + DDTOProcess.SelectedItem.Text + "'," + DDTOProcess.SelectedValue + @",PIM.Process_rec_id) as QCPARAMETER 
            //                                From PROCESS_Receive_MASTER_" + DDTOProcess.SelectedValue + " PIM inner Join PROCESS_Receive_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.Process_Rec_Id=PID.Process_Rec_Id
            //                                inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
            //                                cross apply(select * From dbo.F_GetJobReceiveEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.Process_rec_id)) EI
            //                                inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
            //                                 Where PIM.Process_rec_id=" + hnprocessrecid.Value + " order by Process_rec_detail_id";

            //            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            #endregion

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            param[1] = new SqlParameter("@Refname", DDTOProcess.SelectedItem.Text);
            param[2] = new SqlParameter("@processrecid", hnprocessrecid.Value);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQCREPORTFINISHING", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2Qc.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2Qc.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            lblmsg.Visible = true;
            lblmsg.Text = ex.Message;
        }

    }
    protected void btnQcPreview_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        int Gridrows = DGDetail.Rows.Count;
        if (Gridrows > 0 && DDTOProcess.SelectedIndex > 0)
        {
            DataTable dttable = new DataTable();
            dttable.Columns.Add("TstockNo", typeof(string));
            dttable.Columns.Add("Process_Rec_Detail_id", typeof(int));
            dttable.Columns.Add("Process_Rec_id", typeof(int));
            for (int i = 0; i < Gridrows; i++)
            {
                DataRow dr = dttable.NewRow();
                Label lblstockno = (Label)DGDetail.Rows[i].FindControl("lblTStockNo");
                Label lblrecdetailid = (Label)DGDetail.Rows[i].FindControl("lblrecdetailid");
                int Process_rec_detail_id = Convert.ToInt32(lblrecdetailid.Text);
                dr["TstockNo"] = lblstockno.Text;
                dr["Process_Rec_Detail_id"] = Process_rec_detail_id;
                dr["Process_rec_id"] = hnprocessrecid.Value;
                dttable.Rows.Add(dr);
            }
            //*********************
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            param[1] = new SqlParameter("@dttable", dttable);
            //*******
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameterJobWiseOther", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Modalpopupextqc.Show();
                DataTable dt = new DataTable();
                dt.Columns.Add("SRNO", typeof(int));
                dt.Columns.Add("STOCKNO", typeof(string));
                dt.Columns.Add("Processrecid", typeof(int));
                dt.Columns.Add("Processrecdetailid", typeof(int));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt.Columns.Add(dr["Paraname"].ToString(), typeof(bool));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["SrNo"] = i + 1;
                    dr["StockNo"] = ds.Tables[1].Rows[i]["TstockNo"].ToString();
                    dr["Processrecid"] = ds.Tables[1].Rows[i]["Process_Rec_id"].ToString();
                    dr["Processrecdetailid"] = ds.Tables[1].Rows[i]["Process_Rec_Detail_Id"].ToString();
                    //**********
                    dt.Rows.Add(dr);
                }
                dt.Columns["Processrecid"].ColumnMapping = MappingType.Hidden;
                GDQC.DataSource = dt;
                GDQC.DataBind();
                //check checkboxes
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < GDQC.Rows.Count; i++)
                    {
                        int Processrecdetailid = Convert.ToInt32(GDQC.Rows[i].Cells[3].Text);
                        GridViewRow grow = GDQC.Rows[i];
                        for (int k = 4; k < grow.Cells.Count; k++)
                        {
                            string celltext = GDQC.HeaderRow.Cells[k].Text;
                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                int subprocessrecdetailid = Convert.ToInt32(ds.Tables[2].Rows[j]["RecieveDetailID"]);
                                string paramname = ds.Tables[2].Rows[j]["ParaName"].ToString();
                                if ((Processrecdetailid == subprocessrecdetailid) && (celltext == paramname))
                                {
                                    CheckBox ch = grow.Cells[k].Controls[0] as CheckBox;
                                    ch.Checked = Convert.ToBoolean(ds.Tables[2].Rows[j]["QCVALUE"]);
                                    if (grow.Cells[k].Controls.Count > 1)
                                    {
                                        TextBox txt = grow.Cells[k].Controls[1] as TextBox;
                                        if (txt != null)
                                        {
                                            txt.Text = ds.Tables[2].Rows[j]["Reason"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //
            }
            else
            {

            }
            //*************
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('Please Insert Data first to Save QC Detail')", true);
        }

    }
    protected void GDQC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            // bind checkbox control with gridview :
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                CheckBox chk = e.Row.Cells[i].Controls[0] as CheckBox;
                chk.Enabled = true;
                chk.Checked = true;

            }

        }

    }

    protected void btnqcsavenew_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        try
        {
            DataTable dtrecord = new DataTable();
            dtrecord.Columns.Add("Processrecid", typeof(int));
            dtrecord.Columns.Add("Processrecdetailid", typeof(int));
            dtrecord.Columns.Add("Parameter", typeof(string));
            dtrecord.Columns.Add("paramvalue", typeof(int));
            dtrecord.Columns.Add("Reason", typeof(string));
            //**********
            for (int i = 0; i < GDQC.Rows.Count; i++)
            {
                GridViewRow gvr = GDQC.Rows[i];
                for (int j = 4; j < gvr.Cells.Count; j++)
                {
                    DataRow dr = dtrecord.NewRow();
                    dr["Processrecid"] = GDQC.Rows[i].Cells[2].Text; //Processrecid
                    dr["Processrecdetailid"] = GDQC.Rows[i].Cells[3].Text;//Processrecdetailid                    
                    dr["Parameter"] = GDQC.HeaderRow.Cells[j].Text;
                    CheckBox chk = gvr.Cells[j].Controls[0] as CheckBox;
                    dr["paramvalue"] = chk.Checked == true ? 1 : 0;
                    if (gvr.Cells[j].Controls.Count > 1)
                    {

                        TextBox txt = gvr.Cells[j].Controls[1] as TextBox;
                        //CPH_Form_GDQC_txt4_0
                        //CPH_Form_GDQC_txt4_0
                        if (txt != null)
                        {
                            dr["Reason"] = txt.Text.Trim();
                        }

                    }
                    dtrecord.Rows.Add(dr);
                }
            }
            //*********
            if (dtrecord.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@processid", DDTOProcess.SelectedValue);
                param[3] = new SqlParameter("@UnitName", ddUnits.SelectedItem.Text);
                param[4] = new SqlParameter("@UserID", Session["varuserid"]);
                //*****
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SAVEQCJOBWISEOTHER", param);
                lblqcmsg.Text = param[1].Value.ToString();
                Modalpopupextqc.Show();
            }
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
    protected void GDQC_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                TextBox txt = new TextBox();
                txt.ID = "txt" + i;
                txt.ToolTip = "give reason for not ok";
                txt.Attributes.Add("runat", "server");
                e.Row.Cells[i].Controls.Add(txt);

            }
        }
    }
    protected void BtnUpdateWeight_Click(object sender, EventArgs e)
    {
        if (DDissueno.SelectedIndex > 0)
        {
            if (TxtWeight.Text != "")
            {
                lblqcmsg.Visible = false;
                lblqcmsg.Text = "";
                try
                {
                    SqlParameter[] param = new SqlParameter[6];
                    param[0] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
                    param[1] = new SqlParameter("@ProcessID", DDTOProcess.SelectedValue);
                    param[2] = new SqlParameter("@Process_Rec_ID", DDissueno.SelectedValue);
                    param[3] = new SqlParameter("@Weight", TxtWeight.Text);
                    param[4] = new SqlParameter("@UserID", Session["varuserid"]);
                    param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    param[5].Direction = ParameterDirection.Output;
                    
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_UPDATEPROCESSWISEWEIGHT", param);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('" + param[5].Value.ToString() + "')", true);
                    TxtWeight.Text = "";
                }
                catch (Exception ex)
                {
                    lblqcmsg.Visible = true;
                    lblqcmsg.Text = ex.Message;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('Please enter weight')", true);
                TxtWeight.Focus();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('Please select receive number')", true);
            DDissueno.Focus();
        }
    }
}