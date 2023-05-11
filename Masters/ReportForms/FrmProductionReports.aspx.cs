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
public partial class Masters_ReportForms_FrmProductionReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompany, "Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + " Order by Companyname");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            if (Session["varcompanyId"].ToString() == "16")
            {
                ChkForFt.Checked = true;
                RDProductionDeliveryStatusReport.Visible = false;
                RDMaterialBalReport.Visible = false;
                RDProdDeliveryStatus.Visible = false;
                RDProcessHissabAgainstOrderNo.Visible = false;
                RDBlanceQtyToPay.Visible = false;
                RDProcessHissabFromDateToDate.Visible = false;
                RDFinishingConsumptionReport.Visible = false;
                RDProductionBalToIssueReport.Visible = false;
                RDProcessIssRecWthConsumption.Checked = true;
            }
            ////if (Session["varcompanyId"].ToString() == "38")
            ////{
            ////    RDMaterialBalReport.Visible = false;
            ////    RDProdDeliveryStatus.Visible = false;
            ////    RDProcessHissabAgainstOrderNo.Visible = false;
            ////    RDProcessHissabFromDateToDate.Visible = false;
            ////    RDProcessDetailWithConsumption.Visible = false;
            ////}           

            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");


            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRCustomerCode.Visible = false;
            }
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
        if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        {
            string str = "";
            if (Session["VarCompanyNo"].ToString() == "43")
            {
                str = "Select OrderId,CustomerOrderNo+ ' / ' +LocalOrder From OrderMaster Order By CustomerOrderNo";
            }
            else
            {
                str = "Select OrderId,LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster Order By CustomerOrderNo";
            }
            

            if (Session["varCompanyId"].ToString() == "16")
            {
                str = "Select OrderId, CustomerOrderNo From OrderMaster Order By CustomerOrderNo";
            }
            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        }

        UtilityModule.ConditionalComboFill(ref DDProcess, "Select Process_Name_ID,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name", true, "--Select--");

    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillorderno();
    }
    protected void fillorderno()
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "43")
        {
            str = "Select OrderId,CustomerOrderNo+ ' / ' +LocalOrder From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " ";
        }
        else
        {
            str = "Select OrderId,LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " ";
        }
       
        if (DDorderstatus.SelectedValue != "-1")
        {
            str = str + " and Status=" + DDorderstatus.SelectedValue;
        }
        str = str + " Order By CustomerOrderNo";
        if (variable.VarORDERNODROPDOWNWITHLOCALORDER == "0")
        {
            str = "Select OrderId,CustomerOrderNo From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " ";
            if (DDorderstatus.SelectedValue != "-1")
            {
                str = str + " and Status=" + DDorderstatus.SelectedValue;
            }
            str = str + " Order By CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, @"Select Distinct VF.Item_Id,VF.Item_Name From ORDER_CONSUMPTION_DETAIL OCD,V_FinishedItemDetail VF 
        Where VF.Item_Finished_id=OCD.IFinishedid And Orderid=" + DDOrderNo.SelectedValue + " And ProcessId=" + DDProcess.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By VF.Item_Name", true, "--Select--");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                if (RDProductionDeliveryStatusReport.Checked == true)
                {
                    ProductionDeliveryStatusReport(tran);
                    Session["ReportPath"] = "Reports/RptOrderProductionStatus.rpt";
                    Session["CommanFormula"] = "{View_Order_Detail.Orderid}=" + DDOrderNo.SelectedValue;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                }
                if (RDProductionBalToIssueReport.Checked == true)
                {
                    ProductionBalToIssueReport(tran);
                    Session["ReportPath"] = "Reports/RptProductionBalReport.rpt";
                    Session["CommanFormula"] = "{View_Order_Production_Bal.Orderid}=" + DDOrderNo.SelectedValue + "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                }
                if (RDMaterialBalReport.Checked == true)
                {
                    //MaterialBalReport(tran);
                    Session["ReportPath"] = "Reports/RptMatBalAtContractorAgainstAllOrders.rpt";
                    Session["CommanFormula"] = "{OrderMaster.CompanyId}=" + DDCompany.SelectedValue;
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        Session["CommanFormula"] = Session["CommanFormula"] + " And {OrderMaster.CustomerId}=" + DDCustCode.SelectedValue;
                    }
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        Session["CommanFormula"] = Session["CommanFormula"] + " And {OrderMaster.OrderId}=" + DDOrderNo.SelectedValue;
                    }
                    if (DDProcess.SelectedIndex > 0)
                    {
                        Session["CommanFormula"] = Session["CommanFormula"] + " And {View_Order_Material_Issue_Rec.ProcessId}=" + DDProcess.SelectedValue;
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                }
                if (RDProcessIssRecWthConsumption.Checked == true)
                {
                    if (ChkWithOrderWiseSummary.Checked == true)
                    {
                        if (Session["VarCompanyNo"].ToString() == "39")
                        {
                            OrderConsumptionDetail_WithIssRecSummary();
                        }
                    }
                    else
                    {

                        ProcessIssRecWthConsumption(tran);
                        if (Session["VarCompanyNo"].ToString() == "39")
                        {
                            Session["ReportPath"] = "Reports/RptProcessIssRecWthConsumptionAndLossReport.rpt";
                        }
                        else
                        {
                            Session["ReportPath"] = "Reports/RptProcessIssRecWthConsumptionReport.rpt";
                        }
                        Session["CommanFormula"] = "{TEMP_PROCESS_ISS_REC_DETAIL.Orderid}=" + DDOrderNo.SelectedValue + "";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                        tran.Commit();
                    }        
                    
                   
                }
               
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProductionReports.aspx");
                tran.Rollback();
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (RDProdDeliveryStatus.Checked == true)
            {
                ProductionDeliveryStatusReport();
            }
            if (RDProcessHissabAgainstOrderNo.Checked == true)
            {
                ProcessHissabAgainstOrderNo();
            }
            if (RDProductionDeliveryStatus.Checked == true)
            {
                ProductionDeliveryStatus();
            }
            if (RDBlanceQtyToPay.Checked == true)
            {
                BlanceQtyToPay();
            }
            if (RDProcessHissabFromDateToDate.Checked == true)
            {
                ProcessHissabFromDateToDate();
            }
            if (RDFinishingConsumptionReport.Checked == true)
            {
                FinishingProcessConsumptionReport();
            }
            if (RDProcessDetailWithConsumption.Checked == true)
            {
                ProcessDetailWithConsumption();
            }
            if (RDProcessIssRecWthConsumption.Checked == true && ChkWithOrderWiseSummary.Checked == true)
            {
                OrderConsumptionDetail_WithIssRecSummary();
            }
        }
    }
    private void ProcessDetailWithConsumption()
    {
        SqlParameter[] param = new SqlParameter[4];

        param[0] = new SqlParameter("@COMPANYID", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@PROCESSID", DDProcess.SelectedValue);
        param[2] = new SqlParameter("@ISSUEORDERID", 0);
        param[3] = new SqlParameter("@DATE", TxtToDate.Text);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_ProcessDetailWithConsumption", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptProcessDetailWithConsumption.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptProcessDetailWithConsumption.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    private void ProcessHissabFromDateToDate()
    {
        string Str2 = "";
        string Str = @"Select PM.IssueOrderId,EI.EmpName,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,
        ProdSizeMtr,ProdSizeFt,Sum(Qty) Qty,isnull(sum(CancelQty),0) CancelQty,Sum(PQty) PQty,Sum((Qty-isnull(cancelQty,0))*ProdAreaFt) IssAreaInGajGir,Sum((Qty-isnull(cancelQty,0))*ProdAreaMtr) IssAreaInMtr,Sum(case When Caltype=0 Then (Qty-isnull(cancelQty,0))*Rate*Area Else (Qty-isnull(cancelQty,0))*Rate End) IssAmt,0 RecQty,Avg(Rate) Rate,0 Amount,
        Avg(Comm) Comm,0 CommAmt,0 Penality,0 RecAreaInGajGir,0 RecAreaInMtr,'" + TxtToDate.Text + @"' SelectDate From PROCESS_ISSUE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcess.SelectedValue + @" PD,
        V_FinishedItemDetail VF,EmpInfo EI Where PM.IssueOrderId=PD.IssueOrderId And PD.Item_Finished_Id=VF.Item_Finished_Id And PM.EmpId=EI.EmpId And PM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        string Str1 = " And PM.IssueOrderId in (Select PD.IssueOrderId From PROCESS_RECEIVE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcess.SelectedValue + @" PD 
        Where PM.Process_Rec_Id=PD.Process_Rec_Id And PM.CompanyId=" + DDCompany.SelectedValue + " And ReceiveDate<='" + TxtToDate.Text + "'";
        if (DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + " And PD.Orderid=" + DDOrderNo.SelectedValue;
            Str1 = Str1 + " And PD.Orderid=" + DDOrderNo.SelectedValue;
            Str2 = Str2 + " And PD.Orderid=" + DDOrderNo.SelectedValue;
        }
        Str1 = Str1 + " )";
        Str = Str + Str1 + @" Group BY PM.IssueOrderId,EI.EmpName,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt
        Union 
        Select PD.IssueOrderId,EI.EmpName,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,ProdSizeMtr,
        ProdSizeFt,0 Qty,0 CancelQty,0 PQty,0 IssAreaInGajGir,0 IssAreaInMtr,0 IssAmt,Sum(Qty) RecQty,Avg(Rate) Rate,Sum(Amount) Amount,Avg(Comm) Comm,Sum(CommAmt) CommAmt,
        Sum(Penality) Penality,Sum(Qty*ProdAreaFt) RecAreaInGajGir,Sum(Qty*ProdAreaMtr) RecAreaInMtr,'" + TxtToDate.Text + @"' SelectDate From PROCESS_RECEIVE_MASTER_" + DDProcess.SelectedValue + @" PM,
        PROCESS_RECEIVE_DETAIL_" + DDProcess.SelectedValue + @" PD,V_FinishedItemDetail VF,EmpInfo EI Where PM.Process_Rec_Id=PD.Process_Rec_Id And 
        PD.Item_Finished_Id=VF.Item_Finished_Id And PM.EmpId=EI.EmpId And PM.CompanyId=" + DDCompany.SelectedValue + " And PM.ReceiveDate<='" + TxtToDate.Text + @"' And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        Str = Str + Str2 + @" Group BY PD.IssueOrderId,EI.EmpName,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptProcessHissabFromToDate.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessHissabFromToDate.xsd";
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
    private void BlanceQtyToPay()
    {
        string Str = @"Select CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,OM.OrderID,J.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,
        ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,Sum(QtyRequired) OQty,Sum(TagStock) SQty,0 Qty ,0 PQty,0 PaidQty,Sum(PreProdAssignedQty) JobAssignQty,OD.OrderUnitId as UnitId 
        From OrderMaster OM,OrderDetail OD,Jobassigns J,V_FinishedItemDetail VF,CustomerInfo CI 
        Where OM.OrderId=OD.OrderId And OM.OrderId=J.OrderId And OD.Item_Finished_Id=J.Item_Finished_Id And OD.Item_Finished_Id=VF.Item_Finished_Id And OM.CustomerId=CI.CustomerID And 
        OM.Companyid=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And OM.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + " And OM.Orderid=" + DDOrderNo.SelectedValue;
        }
        Str = Str + @" Group BY CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,OM.OrderID,J.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,
        ProdSizeMtr,ProdSizeFt,OD.OrderUnitId";

        Str = Str + @" Union Select CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,
        ProdSizeMtr,ProdSizeFt,0 OQty,0 SQty,Sum(Qty-isnull(cancelQty,0)) Qty,Sum(PQty-isnull(cancelQty,0)) PQty,0 PaidQty,0 JobAssignQty, PM.UNITID as UnitId From PROCESS_ISSUE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcess.SelectedValue + @" PD,
        V_FinishedItemDetail VF,OrderMaster OM,CustomerInfo CI Where PM.IssueOrderId=PD.IssueOrderId And PD.Item_Finished_Id=VF.Item_Finished_Id And PD.OrderId=OM.OrderId And 
        OM.CustomerId=CI.CustomerID And OM.Companyid=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And OM.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + " And OM.Orderid=" + DDOrderNo.SelectedValue;
        }
        Str = Str + @" Group BY CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,
        ProdSizeMtr,ProdSizeFt,PM.UNITID Union Select CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,
        ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,0 OQty,0 SQty,0 Qty,0 PQty,Count(PD.Item_Finished_Id) PaidQty,0 JobAssignQty,0 as UnitId From PROCESS_RECEIVE_DETAIL_" + DDProcess.SelectedValue + @" PD,
        PROCESS_STOCK_DETAIL PSD,V_FinishedItemDetail VF,OrderMaster OM,CustomerInfo CI Where PD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And PD.Item_Finished_Id=VF.Item_Finished_Id And 
        PD.OrderId=OM.OrderId And OM.CustomerId=CI.CustomerID And HissabFlag<>0 And ToProcessId=" + DDProcess.SelectedValue + " And OM.Companyid=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And OM.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + " And OM.Orderid=" + DDOrderNo.SelectedValue;
        }
        Str = Str + @" And PSD.StockNo in (Select StockNo From PROCESS_HISSAB Where HissabNo in (Select HissabNo From ProcessHissabApproved Where HissabType=0)) 
        Group BY CI.CustomerCode,OM.CustomerOrderNo,OM.LocalOrder,PD.OrderID,PD.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,
        ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBalanceProductionToPay.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBalanceProductionToPay.xsd";
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
    private void ProductionDeliveryStatus()
    {
        SqlParameter[] _arrpara = new SqlParameter[5];
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@Sizeflag", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@ORDERSTATUS", DDorderstatus.SelectedValue);
        _arrpara[4] = new SqlParameter("@CompanyName", DDCompany.SelectedItem.Text);

        _arrpara[0].Value = DDOrderNo.SelectedValue;
        _arrpara[1].Value = DDProcess.SelectedValue;
        _arrpara[2].Value = ChkForFt.Checked == true ? "1" : "2";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderProductionDeliveryStatus", _arrpara);
        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "37")
            {
                Session["rptFileName"] = "~\\Reports\\RptProductionDeliveryStatusSundeepExport.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptProductionDeliveryStatus.rpt";
            }
           
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionDeliveryStatus.xsd";
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
    private void ProcessHissabAgainstOrderNo()
    {
        //SqlParameter[] _arrpara = new SqlParameter[2];
        //_arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        //_arrpara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);

        //_arrpara[0].Value = DDOrderNo.SelectedValue;
        //_arrpara[1].Value = DDProcess.SelectedValue;

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderProcessDetail", _arrpara);



        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_OrderProcessDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@OrderId", DDOrderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", DDProcess.SelectedValue); 

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();


        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptProcessHissabAgainstOrder.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessHissabAgainstOrder.xsd";
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
    private void ProductionDeliveryStatusReport()
    {
        string Str = @"Select QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,Sizeinch,CompanyId,OrderId,CustomerCode,CustomerOrderNo,Sum(QtyRequired) QtyRequired,
                        Sum(TArea) TArea,Sum(QtyIss) QtyIss,Sum(QtyRec) QtyRec,View_Order_Process_Detail.MasterCompanyId,U.Unitid,U.UnitName From View_Order_Process_Detail,Unit U 
                        Where View_Order_Process_Detail.OrderUnitId=U.UnitId And  View_Order_Process_Detail.MasterCompanyId=" + Session["varCompanyId"] + " And  Orderid=" + DDOrderNo.SelectedValue + " And Processid in (0," + DDProcess.SelectedValue + @")
                        Group By QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,Sizeinch,CompanyId,OrderId,CustomerCode,CustomerOrderNo,U.UnitName,U.UnitId,View_Order_Process_Detail.MasterCompanyId

                        Select OM.OrderId,E.Empname,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,VF.SizeFt,Vf.Sizeinch,UnitId,Isnull(Sum(Qty),0) As IssQty,Isnull(Sum(Qty-Pqty),0) As RecQty,isnull(Sum(CancelQty),0) As CancelQty,ReqByDate," + DDProcess.SelectedValue + @" As ProcessId,CI.MasterCompanyId
                        From PROCESS_ISSUE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_ISSUE_Detail_" + DDProcess.SelectedValue + " PD,OrderMaster OM,CustomerInfo CI,V_FinishedItemDetail Vf,EmpInfo E where PM.IssueOrderId=PD.IssueOrderId And VF.ITEM_FINISHED_ID=PD.Item_Finished_Id And OM.OrderId=" + DDOrderNo.SelectedValue + @" And 
                        PD.OrderId=OM.OrderId And OM.Customerid=CI.CustomerId And E.EmpId=PM.Empid And VF.MasterCompanyId=" + Session["varCompanyId"] + @" 
                        Group by OM.OrderId,E.Empname,Customercode,CustomerOrderNo,OM.CompanyId,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,VF.SizeFt,vf.Sizeinch,UnitId,ReqByDate,CI.MasterCompanyId 

                        Select PROCESS_NAME_ID,PROCESS_NAME,ShortName From Process_Name_Master where PROCESS_NAME_ID=" + DDProcess.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        Session["ReportPath"] = "Reports/RptProd_DeliveryStatus_FilterAgainstOrder.rpt";
        Session["dsFileName"] = "~\\ReportSchema\\RptProd_DeliveryStatus_FilterAgainstOrder.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    private void DetailDeliveryStatusMaterialBal(SqlTransaction tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[1];
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[0].Value = DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_ForDetailDeliveryStatusMaterialBal", _arrpara);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptDetailDeliveryStatusMaterialBalAgainstOrder.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptDetailDeliveryStatusMaterialBalAgainstOrder.xsd";
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
    private void ProductionDeliveryStatusReport(SqlTransaction tran)
    {
        string Str = "";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table TEMP_ORDER_CONSMP_AREA");
        Str = @"Insert into TEMP_ORDER_CONSMP_AREA Select Distinct Orderid,OrderDetailId,VF.Item_Id ItemId,VF.Item_Name,ProcessId,Sum(IQty) Qty,[dbo].[Get_OrderDetail_Area] (OrderDetailId) Area,
                        Sum(IQty)*[dbo].[Get_OrderDetail_Area] (OrderDetailId) TConsmp," + Session["varuserid"] + "," + Session["varCompanyId"] + @" 
                        From ORDER_CONSUMPTION_DETAIL OCD,V_FinishedItemDetail VF Where VF.Item_Finished_id=OCD.IFinishedid And Orderid=" + DDOrderNo.SelectedValue + @" And 
                        ProcessId=" + DDProcess.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        if (DDItemName.SelectedIndex > 0)
        {
            Str = Str + " And VF.Item_Id=" + DDItemName.SelectedValue;
        }
        Str = Str + " Group By Orderid,OrderDetailId,VF.Item_Id,ProcessId,VF.Item_Name";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);

        ProcessIssRecWthConsumption(tran);
    }
    private void ProductionBalToIssueReport(SqlTransaction tran)
    {
        String Str = "";
        DataSet Ds = null;
        Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * from SysObjects Where Xtype='FN' And Name='Get_ProcessQty'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Str = "ALTER";
        }
        else
        {
            Str = "CREATE";
        }
        Str = Str + @" FUNCTION [dbo].[Get_ProcessQty] (@OrderID int,@Finishedid int)
                                RETURNS float AS
                                BEGIN
                                    DECLARE @Qty float
	                                Begin
	                                    SELECT @Qty=IsNull(Sum(Qty),0) From PROCESS_ISSUE_DETAIL_" + DDProcess.SelectedValue + @" Where OrderId=@OrderID And Item_Finished_id=@Finishedid
	                                end 
	                                Return(@Qty)
                                END";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * from SysObjects Where Xtype='V' And Name='View_Order_Production_Bal'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Str = "ALTER";
        }
        else
        {
            Str = "CREATE";
        }
        int VarSizeFlag = 1;
        if (ChkForFt.Checked == true)
        {
            VarSizeFlag = 2;
        }
        Str = Str + @" View View_Order_Production_Bal As Select OrderId,Item_Finished_id,Sum(QtyRequired) OQty,
                            Case When Tag_Flag=1 Then [dbo].[Get_PreProdAssigned_Qty](OrderId,Item_Finished_id) Else 0 End TQty,
                            [dbo].[Get_ProcessQty](OrderId,Item_Finished_id) IssQty," + VarSizeFlag + " UnitId From OrderDetail Where OrderId=" + DDOrderNo.SelectedValue + " Group By OrderId,Item_Finished_id,Tag_Flag";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
    }
    private void MaterialBalReport(SqlTransaction tran)
    {
        String Str = "";
        DataSet Ds = null;
        Str = "Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
        if (DDProcess.SelectedIndex > 0)
        {
            Str = Str + "  And Process_Name_Id=" + DDProcess.SelectedValue + "";
        }
        Str = Str + " Order BY Process_Name_Id";
        Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, Str);
        Str = "";
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    DataSet Ds1 = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * from SysObjects Where Xtype='V' And Name='View_Order_Process_Iss'");
                    if (Ds1.Tables[0].Rows.Count > 0)
                    {
                        Str = "ALTER";
                    }
                    else
                    {
                        Str = "CREATE";
                    }
                    Str = Str + @" View View_Order_Process_Iss As ";

                    Str = Str + " Select  DISTINCT PD.IssueOrderId,PD.OrderID from PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,ORDERMASTER OM Where PD.ORDERID=OM.ORDERID And OM.COMPANYID=" + DDCompany.SelectedValue + " And OM.CUSTOMERID=" + DDCustCode.SelectedValue + "";
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + "  And OM.OrderId=" + DDOrderNo.SelectedValue + "";
                    }
                }
                if (i > 0)
                {
                    Str = Str + " UNION Select  DISTINCT PD.IssueOrderId,PD.OrderID from PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,ORDERMASTER OM Where PD.ORDERID=OM.ORDERID And OM.COMPANYID=" + DDCompany.SelectedValue + " And OM.CUSTOMERID=" + DDCustCode.SelectedValue + "";
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " And OM.OrderId=" + DDOrderNo.SelectedValue + "";
                    }
                }
            }
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);

            Ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, "Select * from SysObjects Where Xtype='V' And Name='View_Order_Material_Issue_Rec'");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Str = "ALTER";
            }
            else
            {
                Str = "CREATE";
            }
            Str = Str + @" View View_Order_Material_Issue_Rec As Select ProcessId,PrOrderID,Finishedid,OrderID,IsNull(Case When TranType=0 Then Sum(IssueQuantity) End,0) IssQty,
                           IsNull(Case When TranType=1 Then Sum(IssueQuantity) End,0) RecQty,0 RecConsmp
                           From ProcessRawMaster PM,ProcessRawTran PT,View_Order_Process_Iss VOM 
                           Where PM.TypeFlag = 0 And PM.PRMId=PT.PRMId And VOM.IssueOrderID=PM.PrOrderID And PM.MasterCompanyId=" + Session["varCompanyId"] + @"
                           Group By CompanyId,ProcessId,PrOrderID,Finishedid,TranType,OrderID UNION 
                           Select PRC.ProcessID,PRC.IssueOrderId,PRC.IFinishedid,VOM.OrderId,0,0,Sum(TConsmp+TLoss) RecConsmp From PROCESS_RECEIVE_CONSUMPTION PRC,
                           View_Order_Process_Iss VOM Where PRC.IssueOrderId=VOM.IssueOrderId 
                           Group By PRC.ProcessID,PRC.IssueOrderId,PRC.IFinishedid,VOM.OrderId";
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        }
    }
    private void ProcessIssRecWthConsumption(SqlTransaction tran)
    {
        string Str = "";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table TEMP_PROCESS_CONSMP_ISS_DETAIL");
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Truncate Table TEMP_PROCESS_ISS_REC_DETAIL");

        Str = @"Insert into TEMP_PROCESS_ISS_REC_DETAIL SELECT PM.IssueOrderId,PM.Empid,PM.UnitId,PM.CompanyId,PM.AssignDate,PD.Item_Finished_id,Sum(PD.Qty) Qty,
               Sum((PD.Qty-isnull(CancelQty,0))*PD.Area) Area,Sum(PD.Qty-PD.PQty) RQty,Sum(PD.PQty-isnull(cancelQty,0)) PQty,
               PD.Orderid,PD.ReqByDate,DateDiff(Day,PD.ReqByDate,PM.AssignDate) LateDays," + Session["varuserid"] + "," + Session["varCompanyId"] + @",'',Sum(isnull(CancelQty,0)) As CancelQty
               FROM PROCESS_ISSUE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcess.SelectedValue + @" PD 
               Where PM.IssueOrderId=PD.IssueOrderId And PD.Orderid=" + DDOrderNo.SelectedValue + @"
               Group By PM.IssueOrderId,PM.Empid,PM.UnitId,PM.CompanyId,PM.AssignDate,PD.Item_Finished_id,PD.Orderid,PD.ReqByDate";
        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);

        if (Session["VarCompanyNo"].ToString() == "16" && DDProcess.SelectedItem.Text == "WEAVING")
        {
            Str = @"Insert into TEMP_PROCESS_ISS_REC_DETAIL SELECT PM.IssueOrderId,(Select distinct EmpID from Employee_HomeFurnishingOrderMaster Where IssueOrderId=PM.IssueOrderId and ProcessID=1) as EmpId,
                PM.UnitId,PM.CompanyId,PM.AssignDate,PD.OrderDetailDetail_FinishedID as Item_Finished_id,Sum(PD.Qty) Qty,
               Sum((PD.Qty-isnull(CancelQty,0))*PD.Area) Area,Sum(PD.Qty-PD.PQty) RQty,Sum(PD.PQty-isnull(cancelQty,0)) PQty,
               PD.Orderid,PD.ReqByDate,DateDiff(Day,PD.ReqByDate,PM.AssignDate) LateDays," + Session["varuserid"] + "," + Session["varCompanyId"] + @",'',Sum(isnull(CancelQty,0)) As CancelQty
               FROM HomeFurnishingOrderMaster PM JOIN HomeFurnishingOrderDetail PD ON PM.IssueOrderId=PD.IssueOrderId 
               Where PD.Orderid=" + DDOrderNo.SelectedValue + @"
               Group By PM.IssueOrderId,PM.UnitId,PM.CompanyId,PM.AssignDate,PD.OrderDetailDetail_FinishedID,PD.Orderid,PD.ReqByDate";
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        }
      

        Str = @"Insert into TEMP_PROCESS_CONSMP_ISS_DETAIL SELECT PM.Issueorderid,PD.Orderid,OCD.IFINISHEDID,Isnull(Round(Sum(CASE WHEN PM.CalType=0 THEN CASE WHEN PM.UnitId=1 Then Case When OCD.MasterCompanyId<>9 Then  (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY End else Case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY/10.76391  End END ELSE 
               CASE WHEN PM.UnitId=1 Then case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*OCD.IQTY  End else case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.IQTY Else (PD.Qty-isnull(CancelQty,0))*OCD.IQTY/10.76391  End END END),3),0) ConsmpQTY,[dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid) IssQty,
               [dbo].[Get_Process_Rec_Consmp_Qty](PM.Issueorderid,OCD.IFINISHEDID) RecConsmp,Round([dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid)-[dbo].[Get_Process_Rec_Consmp_Qty](PM.Issueorderid,OCD.IFINISHEDID),3) PendQty," + Session["varuserid"] + "," + Session["varCompanyId"] + @",
                Isnull(Round(Sum(CASE WHEN PM.CalType=0 THEN CASE WHEN PM.UnitId=1 Then Case When OCD.MasterCompanyId<>9 Then  (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS End 
                else Case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS/10.76391  End END 
                ELSE CASE WHEN PM.UnitId=1 Then case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS*1.196 Else (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS  End 
                else case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS Else (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS/10.76391  End END END),3),0) ConsmpLossQTY
               FROM PROCESS_CONSUMPTION_DETAIL OCD,PROCESS_ISSUE_MASTER_" + DDProcess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcess.SelectedValue + @" PD ,V_finisheditemdetail vf
               Where PM.IssueOrderid=PD.IssueOrderid And OCD.Issue_Detail_Id=PD.Issue_Detail_Id and PD.item_finished_id=vf.item_finished_id And PD.Orderid=" + DDOrderNo.SelectedValue + "  And OCD.PROCESSID=" + DDProcess.SelectedValue + @" 
               Group By OCD.IFINISHEDID,PM.Issueorderid,PD.Orderid,OCD.MasterCompanyId";
         SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);

        if (Session["VarCompanyNo"].ToString()=="16" && DDProcess.SelectedItem.Text == "WEAVING")
        {
            Str = @"Insert into TEMP_PROCESS_CONSMP_ISS_DETAIL SELECT PM.Issueorderid,PD.Orderid,OCD.IFINISHEDID,Isnull(Round(Sum(CASE WHEN PM.CalType=0 THEN CASE WHEN PM.UnitId=1 Then Case When OCD.MasterCompanyId<>9 Then  (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY End else Case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.IQTY/10.76391  End END ELSE 
               CASE WHEN PM.UnitId=1 Then case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*OCD.IQTY  End else case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.IQTY Else (PD.Qty-isnull(CancelQty,0))*OCD.IQTY/10.76391  End END END),3),0) ConsmpQTY,
			   [dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid) IssQty,
               [dbo].[Get_HomeFurnishing_Rec_Consmp_Qty](PM.Issueorderid,OCD.IFINISHEDID) RecConsmp,
			   Round([dbo].[Get_ProcessIssueQty] (OCD.IFINISHEDID,PM.Issueorderid)-[dbo].[Get_HomeFurnishing_Rec_Consmp_Qty](PM.Issueorderid,OCD.IFINISHEDID),3) PendQty," + Session["varuserid"] + "," + Session["varCompanyId"] + @",
                Isnull(Round(Sum(CASE WHEN PM.CalType=0 THEN CASE WHEN PM.UnitId=1 Then Case When OCD.MasterCompanyId<>9 Then  (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS End 
                else Case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS Else (PD.Qty-isnull(CancelQty,0))*PD.Area*OCD.ILOSS/10.76391  End END 
                ELSE CASE WHEN PM.UnitId=1 Then case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS*1.196 Else (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS  End 
                else case When OCD.MasterCompanyId<>9 Then (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS Else (PD.Qty-isnull(CancelQty,0))*OCD.ILOSS/10.76391  End END END),3),0) ConsmpLossQTY
               FROM HomeFurnishingConsumptionDetail OCD 
			   JOIN HomeFurnishingOrderDetail PD ON OCD.ISSUE_DETAIL_ID=PD.IssueDetailId and OCD.PROCESSID=1 
			   JOIN HomeFurnishingOrderMaster PM ON PM.IssueOrderid=PD.IssueOrderid
			   JOIN V_finisheditemdetail vf ON PD.OrderDetailDetail_FinishedID=Vf.ITEM_FINISHED_ID
               Where PD.Orderid=" + DDOrderNo.SelectedValue + "  And OCD.PROCESSID=" + DDProcess.SelectedValue + @" 
               Group By OCD.IFINISHEDID,PM.Issueorderid,PD.Orderid,OCD.MasterCompanyId";
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);
        }
        
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (RDProcessDetailWithConsumption.Checked == false)
        {
            if (RDMaterialBalReport.Checked == false)
            {
                if (RDBlanceQtyToPay.Checked == false)
                {
                    if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "0")
                    {
                        if (UtilityModule.VALIDDROPDOWNLIST(DDCustCode) == false)
                        {
                            goto a;
                        }
                    }
                }
                if (RDBlanceQtyToPay.Checked == false)
                {
                    if (UtilityModule.VALIDDROPDOWNLIST(DDOrderNo) == false)
                    {
                        goto a;
                    }
                }
                if (UtilityModule.VALIDDROPDOWNLIST(DDProcess) == false)
                {
                    goto a;
                }
                else
                {
                    goto B;
                }
            a:
                UtilityModule.SHOWMSG(lblMessage);
            B: ;
            }
        }
        else
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDProcess) == false)
            {
                goto a;
            }
            else
            {
                goto B;
            }
        a:
            UtilityModule.SHOWMSG(lblMessage);
        B: ;
        }
    }
    private static int GetDelayDays(DateTime ReqDate, DateTime RecDate)
    {
        TimeSpan ts = RecDate.Subtract(ReqDate);
        int DelayDays = ts.Days;
        if (DelayDays < 0)
        {
            DelayDays = 0;
        }
        return DelayDays;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void FinishingProcessConsumptionReport()
    {
        SqlParameter[] _arrpara = new SqlParameter[5];
        _arrpara[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _arrpara[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);


        _arrpara[0].Value = DDCustCode.SelectedValue;
        _arrpara[1].Value = DDOrderNo.SelectedValue;
        _arrpara[2].Value = DDProcess.SelectedValue;
        _arrpara[3].Value = Session["varcompanyId"];
        _arrpara[4].Direction = ParameterDirection.Output;


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForFinishingProcessConsumptionOrderReport", _arrpara);
        if (_arrpara[4].Value.ToString() == "")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptFinishingProcessConsumptionOrderWise.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptFinishingProcessConsumptionOrderWise.xsd";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Consumption not defined!');", true);
        }


    }
    //protected void RDProcessHissabFromDateToDate_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (RDProcessHissabFromDateToDate.Checked == true)
    //    {
    //        TRToDate.Visible = true;
    //        TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
    //    }
    //    else
    //    {
    //        TRToDate.Visible = false;
    //    }
    //}

    protected void DDorderstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillorderno();
    }

    private void OrderConsumptionDetail_WithIssRecSummary()
    {
        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        //param[1] = new SqlParameter("@MasterCompanyId", Session["VarcompanyNo"]);
        //param[2] = new SqlParameter("@UserID", Session["VarUserId"]);
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETORDERWEAVINGCONSUMPTION_WITHISSRECSUMMARY", param);


        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETORDERWEAVINGCONSUMPTION_WITHISSRECSUMMARY", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@orderid", DDOrderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarcompanyNo"]);
        cmd.Parameters.AddWithValue("@UserID", Session["VarUserId"]);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptOrderWeavingConsumptionWithIssRecSummary.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderWeavingConsumptionWithIssRecSummary.xsd";
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