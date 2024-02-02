using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ProcessIssue_FrmSupplyOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            TxtAssignDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
            TxtRequiredDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CompanyName", true, "--Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            CompanySelectedChange();
            if (DDCustomerCode.Items.Count > 0 && Convert.ToInt32(Session["varcompanyno"]) == 7)
            {
                DDCustomerCode.SelectedIndex = 1;
                CustomerCodeSelectedChange();
            }

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            string str = @" select unitid,unitname from unit Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                            Select TermId,TermName from Term Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By TermName 
                            Select PaymentId,PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by PaymentName ";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 0, true, "--Select--");
            CommanFunction.FillComboWithDS(DDFreight, ds, 1);
            CommanFunction.FillComboWithDS(DDInsurance, ds, 1);
            CommanFunction.FillComboWithDS(DDPaymentAt, ds, 2);

            DDunit.SelectedIndex = 2;
            Session["IssueOrderid"] = 0;
            if (DDFreight.Items.Count > 0)
            {
                DDFreight.SelectedIndex = 0;
            }
            if (DDInsurance.Items.Count > 0)
            {
                DDInsurance.SelectedIndex = 0;
            }
            if (DDPaymentAt.Items.Count > 0)
            {
                DDPaymentAt.SelectedIndex = 0;
            }
            CheckForEditChange();
            DDCustomerCode.Focus();
            if (Convert.ToInt32(Session["VARCOMPANYNO"]) == 7)
            {
                TRDETAIL.Visible = false;
                DDcaltype.SelectedValue = "1";
                DDcaltype.Visible = false;
                TDCONSMP.Visible = false;
                calnaame.Visible = false;
                TxtAssignDate.Enabled = false;
                TxtRequiredDate.Enabled = false;
                BtnPreview.Visible = false;
                tdinst.Visible = false;
                tdtxtinst.Visible = false;
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        String Str = "Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And SupplierQty>0 And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ChkForEdit.Checked == true)
        {
            Str = Str + " And OM.Orderid in (Select OrderID From Process_Issue_Detail_9)";
        }
        Str = Str + " Order By CustomerId";
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, Str, true, "--Select--");
        if (DDCustomerCode.Items.Count > 0 && Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            DDCustomerCode.SelectedIndex = 1;
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
        DDCustomerOrderNumber.Focus();
    }
    private void CustomerCodeSelectedChange()
    {
        String Str = "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid and om.status=0 And SupplierQty>0 And OM.Customerid=" + DDCustomerCode.SelectedValue;
        if (Session["varcompanyno"].ToString() == "7")
        {
            Str = "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J,V_FinishedItemDetail v ,UserRights_Category uc Where OM.Orderid=J.Orderid and v.item_finished_id=j.Item_Finished_Id and uc.CategoryId=v.CATEGORY_ID and om.status=0 And SupplierQty>0 and uc.userid=" + Session["varuserid"] + " And OM.Customerid=" + DDCustomerCode.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        }
        if (ChkForEdit.Checked == false)
        {
            // Str = Str + " And OM.Orderid Not in (Select OrderID From Process_Issue_Detail_9)";
            Str = Str + @"  And OM.Orderid Not in (Select PID.OrderID From Process_Issue_Detail_9 PID,JobAssigns JA 
            Where PID.Orderid=JA.OrderID And JA.Item_Finished_Id=PID.Item_Finished_Id Group by PID.orderid,PID.Item_Finished_Id,JA.SupplierQty having JA.SupplierQty > Sum (PID.QTY)+ sum(isnull(PID.RejectQty,0)))";
        }
        if (ChkForEdit.Checked == true)
        {
            Str = Str + " And OM.Orderid in (Select OrderID From Process_Issue_Detail_9)";
        }
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, Str, true, "--Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerOrderNumberSelectedChange();
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select replace(convert(varchar(11),DispatchDate,106),' ','-') from ordermaster where orderid=" + DDCustomerOrderNumber.SelectedValue + "");
            if (ds.Tables[0].Rows.Count > 0)
                TxtRequiredDate.Text = ds.Tables[0].Rows[0][0].ToString();
        }
        DDProcessName.Focus();
    }
    private void CustomerOrderNumberSelectedChange()
    {
        ChkForConsumption.Checked = false;
        UtilityModule.ConditionalComboFill(ref DDProcessName, "SELECT DISTINCT PROCESS_Name_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID=9 And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        if (DDCustomerOrderNumber.SelectedIndex > 0)
        {
            DDunit.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select OrderUnitId from OrderDetail Where OrderId=" + DDCustomerOrderNumber.SelectedValue + "").ToString();
        }
        if (1 == Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ConsmpFlag From OrderMaster Where Orderid=" + DDCustomerOrderNumber.SelectedValue + "")))
        {
            ChkForConsumption.Checked = true;
        }
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            DDProcessName.SelectedValue = "9";
            ProcessName();
            Fill_ProcessNameSelectedChange();
        }
        Fill_Grid_New();
    }
    private void Fill_Grid_New()
    {
        DGDetail.DataSource = GetDetailNew();
        DGDetail.DataBind();
        Label1.Visible = true;
        if (ChkForEdit.Checked == false)
        {
            for (int i = 0; i < DGDetail.Rows.Count; i++)
            {
                GridViewRow row = DGDetail.Rows[i];
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
        }
        else
        {
            Fill_ProcessNameSelectedChange();
        }
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            fillqty();
        }
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetailNew()
    {
        DataSet DS = null;
        //Replace(Str(OM.Orderid)+'|'+str(VF.Item_Finished_Id),' ','')
        string sqlstr = "";
        {
            sqlstr = @"Select VF.Item_Finished_Id Sr_No,Item_Name Item,QualityName Quality,DesignName Design,ColorName Color,ShapeName Shape,
                         Case When " + DDunit.SelectedValue + @"=1 Then SizeMtr when " + DDunit.SelectedValue + @"=6 then SizeInch  Else SizeFt End +ShadeColorName as Size,Sum(SupplierQty) OQty,'' SQTY,
                         Sum(Case When " + DDunit.SelectedValue + @"=1 Then AreaMtr*J.SupplierQty Else AreaFt*J.SupplierQty End) Area,'' IssQty,'' Rate,
                         Case When SAO.ArticleNo IS NULL Then OD.ArticalNo Else SAO.ArticleNo End ArticalNo,SubQuantity SubQuality
                         FROM OrderMaster AS OM INNER JOIN View_OrderDetail AS OD ON OM.OrderId=OD.OrderId LEFT OUTER JOIN QualityCodeMaster AS QM ON 
                         OD.QualityCodeId=QM.QualityCodeId INNER JOIN
                         V_FinishedItemDetail AS VF ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID INNER JOIN JobAssigns AS J ON OD.Item_Finished_Id=J.ITEM_FINISHED_ID AND 
                         OD.OrderId=J.OrderId LEFT OUTER JOIN SUPPLY_ARTICLE_NO SAO ON OM.CustomerId=SAO.CustomerId AND OD.Item_Finished_Id=SAO.Finishedid
                         WHERE OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Group By VF.Item_Finished_Id,Category_Name,Item_Name,QualityName,ShadeColorName,
                         DesignName,ColorName,ShapeName,SizeMtr,SizeFt,SAO.ArticleNo,OD.ArticalNo,SubQuantity,SizeInch Having Sum(SupplierQty)>0";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            LblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmSupplyOrder.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ProcessName();
        Fill_ProcessNameSelectedChange();
        DDEmployeeName.Focus();
    }
    private void ProcessName()
    {
        if (ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select  distinct EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " pim,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " pid Where EI.Empid=EP.Empid  and pim.Empid=ei.Empid and pim.IssueOrderId=pid.IssueOrderId and pid.orderid=" + DDCustomerOrderNumber.SelectedValue + @"
            And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd Pim.CompanyId=" + DDCompanyName.SelectedValue + "  order by ei.empname", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid  And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");
        }
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeNameSelectedChange();
        DDSupplyOrderNo.Focus();
    }
    private void EmployeeNameSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDSupplyOrderNo, @"Select distinct pim.IssueOrderid,cast(pim.IssueOrderid as varchar)+'/'+cast(SupplyOrderNo as varchar) 
        From Process_Issue_Master_" + DDProcessName.SelectedValue + " pim ,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" pid
        Where pim.IssueOrderId=pid.IssueOrderId and pid.orderid=" + DDCustomerOrderNumber.SelectedValue + " and SupplyOrderNo<>0 And CompanyId=" + DDCompanyName.SelectedValue + " And EmpId=" + DDEmployeeName.SelectedValue + "", true, "--Select--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        ProcessIssue();
        BtnPreview.Focus();
        //ProcessReportPath();
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        Session["IssueOrderid"] = 0;
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara1 = new SqlParameter[18];
                int N = 0;
                int VarIssueNumber = 0;
                string strPCMDID = "";
                N = DGDetail.Rows.Count;
                for (int i = 0; i < N; i++)
                {
                    TextBox TxtIssueQTY = (TextBox)DGDetail.Rows[i].FindControl("TxtDGIssQty");
                    GridViewRow row = DGDetail.Rows[i];
                    if (TxtIssueQTY.Text == "")
                    {
                        TxtIssueQTY.Text = "0";
                    }
                    if ((TxtIssueQTY.Text != "0") && (((CheckBox)row.FindControl("Chkbox")).Checked == true))
                    {
                        if (VarIssueNumber == 0)
                        {
                            For_Master_Save(Tran);
                            VarIssueNumber = 1;
                        }
                        strPCMDID = DGDetail.DataKeys[i].Value.ToString();
                        TextBox TxtRate = (TextBox)DGDetail.Rows[i].FindControl("TxtDGRate");
                        TextBox TxtIssQty = (TextBox)DGDetail.Rows[i].FindControl("TxtDGIssQty");
                        TextBox TxtArticleNo = (TextBox)DGDetail.Rows[i].FindControl("TxtDGArticalNo");
                        TextBox TxtDesginName = (TextBox)DGDetail.Rows[i].FindControl("TxtDGDesign");
                        TextBox TxtColorName = (TextBox)DGDetail.Rows[i].FindControl("TxtDGColor");

                        _arrpara1[0] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
                        _arrpara1[1] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                        _arrpara1[2] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                        _arrpara1[3] = new SqlParameter("@Length", SqlDbType.Float);
                        _arrpara1[4] = new SqlParameter("@Width", SqlDbType.Float);
                        _arrpara1[5] = new SqlParameter("@Area", SqlDbType.Float);
                        _arrpara1[6] = new SqlParameter("@Rate", SqlDbType.Float);
                        _arrpara1[7] = new SqlParameter("@Amount", SqlDbType.Float);
                        _arrpara1[8] = new SqlParameter("@Qty", SqlDbType.Int);
                        _arrpara1[9] = new SqlParameter("@ReqByDate", SqlDbType.DateTime);
                        _arrpara1[10] = new SqlParameter("@PQty", SqlDbType.Int);
                        _arrpara1[11] = new SqlParameter("@Comm", SqlDbType.Float);
                        _arrpara1[12] = new SqlParameter("@CommAmt", SqlDbType.Float);
                        _arrpara1[13] = new SqlParameter("@Orderid", SqlDbType.Int);
                        _arrpara1[14] = new SqlParameter("@ArticleNo", SqlDbType.NVarChar, 50);
                        _arrpara1[15] = new SqlParameter("@DesginName", SqlDbType.NVarChar, 50);
                        _arrpara1[16] = new SqlParameter("@ColorName", SqlDbType.NVarChar, 50);
                        _arrpara1[17] = new SqlParameter("@QualityCodeId", SqlDbType.NVarChar, 50);
                        _arrpara1[0].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Issue_Detail_Id),0)+1 from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue);
                        _arrpara1[1].Value = Session["IssueOrderid"];
                        _arrpara1[2].Value = DGDetail.DataKeys[i].Value;
                        if (Convert.ToInt32(DGDetail.Rows[i].Cells[7].Text) == 0)
                        {
                            _arrpara1[5].Value = "0";
                        }
                        else
                        {
                            _arrpara1[5].Value = Convert.ToDouble(DGDetail.Rows[i].Cells[9].Text) / Convert.ToDouble(DGDetail.Rows[i].Cells[7].Text);
                        }
                        if (DGDetail.Rows[i].Cells[6].Text != "" && Convert.ToInt32(Session["varcompanyno"]) != 7)//&& DDcaltype.SelectedValue != "1"
                        {
                            strPCMDID = DGDetail.Rows[i].Cells[6].Text;

                            _arrpara1[3].Value = strPCMDID.Split('x')[1];
                            _arrpara1[4].Value = strPCMDID.Split('x')[0];
                        }
                        else
                        {
                            _arrpara1[3].Value = 0;
                            _arrpara1[4].Value = 0;
                            //_arrpara1[5].Value = 0;
                        }
                        _arrpara1[6].Value = TxtRate.Text == "" ? "0" : TxtRate.Text;
                        if (DDcaltype.SelectedValue == "0")
                        {
                            _arrpara1[7].Value = String.Format("{0:#0.00}", (Convert.ToDouble(_arrpara1[5].Value) * Convert.ToDouble(_arrpara1[6].Value) * Convert.ToDouble(TxtIssQty.Text)));
                        }
                        if (DDcaltype.SelectedValue == "1")
                        {
                            _arrpara1[7].Value = String.Format("{0:#0.00}", (Convert.ToDouble(_arrpara1[6].Value) * Convert.ToDouble(TxtIssQty.Text)));
                        }
                        if (DDcaltype.SelectedValue == "2")
                        {
                            _arrpara1[7].Value = String.Format("{0:#0.00}", (Convert.ToDouble(_arrpara1[6].Value) * Convert.ToDouble(TxtIssQty.Text)));
                        }
                        _arrpara1[8].Value = TxtIssQty.Text;
                        _arrpara1[9].Value = TxtRequiredDate.Text;
                        _arrpara1[10].Value = TxtIssQty.Text;
                        _arrpara1[11].Value = "0";
                        _arrpara1[12].Value = "0";
                        _arrpara1[13].Value = DDCustomerOrderNumber.SelectedValue;
                        _arrpara1[14].Value = TxtArticleNo.Text == "" ? "" : TxtArticleNo.Text.ToUpper();
                        _arrpara1[15].Value = TxtDesginName.Text == "" ? "" : TxtDesginName.Text.ToUpper();
                        _arrpara1[16].Value = TxtColorName.Text == "" ? "" : TxtColorName.Text.ToUpper();
                        _arrpara1[17].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select QualityCodeID from OrderDetail Where Orderid=" + _arrpara1[13].Value + " And Item_Finished_Id=" + _arrpara1[2].Value + "");
                        Session["processId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                        string str2 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId) values(" + _arrpara1[0].Value + "," + _arrpara1[1].Value + "," + _arrpara1[2].Value + ",'" + _arrpara1[3].Value + "','" + _arrpara1[4].Value + "'," + _arrpara1[5].Value + "," + _arrpara1[6].Value + "," + _arrpara1[7].Value + "," + _arrpara1[8].Value + ",'" + _arrpara1[9].Value + "'," + _arrpara1[10].Value + "," + _arrpara1[11].Value + "," + _arrpara1[12].Value + "," + _arrpara1[13].Value + ",'" + _arrpara1[14].Value + "'," + _arrpara1[17].Value + ")";
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str2);
                        if (TxtArticleNo.Text != "")
                        {
                            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from Supply_Article_No Where ArticleNo='" + _arrpara1[14].Value + "' And CustomerId=" + DDCustomerCode.SelectedValue + "");
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                int VarID = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(ID),0)+1 from Supply_Article_No"));
                                str2 = @"Insert Into Supply_Article_No(ID,CustomerID,Finishedid,ArticleNo) Values(" + VarID + "," + DDCustomerCode.SelectedValue + "," + _arrpara1[2].Value + ",'" + _arrpara1[14].Value + "')";
                                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str2);
                            }
                        }
                        if (ChkForConsumption.Checked == true)
                        {
                            UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara1[1].Value), Convert.ToInt32(_arrpara1[0].Value), Convert.ToInt32(_arrpara1[2].Value), 1, Convert.ToInt32(_arrpara1[13].Value), Tran);
                        }
                    }
                }
                Tran.Commit();
                MessageSave();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Successfully Saved DP No=" + Session["IssueOrderid"];
                Save_Refresh();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmSupplyOrder.aspx");
                Tran.Rollback();
                TxtIssueNo.Text = "";
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void For_Master_Save(SqlTransaction Tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[19];

        _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.DateTime);
        _arrpara[3] = new SqlParameter("@Status", SqlDbType.NVarChar);
        _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
        _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
        _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
        _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar);
        _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);
        _arrpara[9] = new SqlParameter("@CalType", SqlDbType.Int);
        _arrpara[10] = new SqlParameter("@Freight", SqlDbType.Int);
        _arrpara[11] = new SqlParameter("@Insurance", SqlDbType.Int);
        _arrpara[12] = new SqlParameter("@PaymentAt", SqlDbType.Int);
        _arrpara[13] = new SqlParameter("@Destination", SqlDbType.NVarChar, 100);
        _arrpara[14] = new SqlParameter("@Liasoning", SqlDbType.NVarChar, 50);
        _arrpara[15] = new SqlParameter("@Inspection", SqlDbType.NVarChar, 50);
        _arrpara[16] = new SqlParameter("@SampleNumber", SqlDbType.NVarChar, 100);
        _arrpara[17] = new SqlParameter("@SupplyOrderNo", SqlDbType.NVarChar, 100);
        _arrpara[18] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
        _arrpara[0].Value = (Session["IssueOrderid"]);
        _arrpara[1].Value = DDEmployeeName.SelectedValue;
        _arrpara[2].Value = TxtAssignDate.Text;
        _arrpara[3].Value = "Pending";
        _arrpara[4].Value = DDunit.SelectedValue;
        _arrpara[5].Value = Session["varuserid"];
        _arrpara[6].Value = Convert.ToString(TxtRemarks.Text).ToUpper();
        _arrpara[7].Value = TxtInstructions.Text;
        _arrpara[8].Value = DDCompanyName.SelectedValue;
        _arrpara[9].Value = DDcaltype.SelectedValue;
        _arrpara[10].Value = DDFreight.SelectedValue;
        _arrpara[11].Value = DDInsurance.SelectedValue;
        _arrpara[12].Value = DDPaymentAt.SelectedValue;
        _arrpara[13].Value = TxtDestination.Text == "" ? "" : TxtDestination.Text;
        _arrpara[14].Value = TxtLiasoning.Text == "" ? "" : TxtLiasoning.Text;
        _arrpara[15].Value = TxtInspection.Text == "" ? "" : TxtInspection.Text;
        _arrpara[16].Value = TxtSampleNumber.Text == "" ? "" : TxtSampleNumber.Text;
        _arrpara[18].Value = 0;
        if (ChkForEdit.Checked == true)
        {
            Session["IssueOrderid"] = DDSupplyOrderNo.SelectedValue;
            _arrpara[0].Value = (Session["IssueOrderid"]);
            string str1 = @"Delete SupplyOrderDesignColor Where IssueDetailId in (Select IssueDetailId from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + _arrpara[0].Value + ")";
            str1 = str1 + @"  Delete Process_Issue_Master_" + DDProcessName.SelectedValue + " Where IssueOrderid=" + _arrpara[0].Value + "";
            str1 = str1 + @"  Delete PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderid=" + _arrpara[0].Value + "";
            str1 = str1 + @"  Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderId=" + _arrpara[0].Value + "";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            _arrpara[17].Value = TxtIssueNo.Text;
            // _arrpara[18].Value = 0;
            str1 = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,Freight,Insurance,PaymentAt,Destination,Liasoning,Inspection,SampleNumber,SupplyOrderNo,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[9].Value + "," + _arrpara[10].Value + "," + _arrpara[11].Value + "," + _arrpara[12].Value + ",'" + _arrpara[13].Value + "','" + _arrpara[14].Value + "','" + _arrpara[15].Value + "','" + _arrpara[16].Value + "','" + _arrpara[17].Value + "'," + _arrpara[18].Value + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
        }
        if (Convert.ToInt32(Session["IssueOrderid"]) == 0)
        {
            int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
            Session["IssueOrderid"] = a;
            _arrpara[17].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(SupplyOrderNo ),0)+1 from Process_Issue_Master_" + DDProcessName.SelectedValue + ""));
            string str = @"Update MasterSetting Set IssueOrderid =" + Session["IssueOrderid"];
            _arrpara[0].Value = (Session["IssueOrderid"]);
            str = str + @"    Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,Freight,Insurance,PaymentAt,Destination,Liasoning,Inspection,SampleNumber,SupplyOrderNo,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[9].Value + ",'" + _arrpara[10].Value + "','" + _arrpara[11].Value + "','" + _arrpara[12].Value + "','" + _arrpara[13].Value + "','" + _arrpara[14].Value + "','" + _arrpara[15].Value + "','" + _arrpara[16].Value + "'," + _arrpara[17].Value + "," + _arrpara[18].Value + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            TxtIssueNo.Text = _arrpara[17].Value.ToString();
        }

    }
    private void MessageSave()
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('Record(s) has been saved successfully!');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    private void Fill_ProcessNameSelectedChange()
    {
        int N = 0;
        string strPCMDID = "";
        N = DGDetail.Rows.Count;
        if (DDProcessName.SelectedIndex > 0)
        {
            for (int i = 0; i < N; i++)
            {
                strPCMDID = DGDetail.DataKeys[i].Value.ToString();
                string Str = "Select Isnull(Sum(PD.Qty),0) from Process_Issue_Master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + " PD Where PM.IssueOrderId=PD.IssueOrderId And PM.SupplyOrderNo<>0 And OrderiD=" + DDCustomerOrderNumber.SelectedValue + " And Item_Finished_Id=" + strPCMDID + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
                if (ChkForEdit.Checked == true && DDSupplyOrderNo.SelectedIndex > 0)
                {
                    Str = Str + " And PM.IssueOrderid!=" + DDSupplyOrderNo.SelectedValue + "";
                }
                DGDetail.Rows[i].Cells[8].Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString();
                if (ChkForEdit.Checked == true && DDSupplyOrderNo.SelectedIndex > 0)
                {
                    Str = "Select Isnull(Sum(PD.Qty),0) Qty,PD.Rate from Process_Issue_Master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD Where PM.IssueOrderId=PD.IssueOrderId And 
                           PM.SupplyOrderNo<>0 And OrderiD=" + DDCustomerOrderNumber.SelectedValue + " And Item_Finished_Id=" + strPCMDID + " And PM.IssueOrderid=" + DDSupplyOrderNo.SelectedValue + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " Group By PD.Rate";
                    DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        GridViewRow row = DGDetail.Rows[i];
                        ((CheckBox)row.FindControl("Chkbox")).Checked = true;
                        ((TextBox)DGDetail.Rows[i].FindControl("TxtDGIssQty")).Text = Ds.Tables[0].Rows[0]["Qty"].ToString();
                        ((TextBox)DGDetail.Rows[i].FindControl("TxtDGRate")).Text = Ds.Tables[0].Rows[0]["Rate"].ToString();
                    }
                }
            }
        }
        Session["processId"] = DDProcessName.SelectedValue;
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNumber) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDEmployeeName) == false)
        {
            goto a;
        }
        if (ChkForEdit.Checked == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDSupplyOrderNo) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDunit) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    //------------------------TO Refresh the Item parameters On click Save Button----------------------------------------------------
    private void Save_Refresh()
    {
        DDEmployeeName.SelectedIndex = 0;
        DDProcessName.SelectedIndex = 0;
        DDCustomerOrderNumber.SelectedIndex = 0;
        //TxtIssueNo.Text = "";
        CustomerOrderNumberSelectedChange();
        if (ChkForEdit.Checked == true)
        {
            ChkForEdit.Checked = false;
            DDSupplyOrderNo.SelectedIndex = 0;
            CheckForEditChange();
        }
    }
    protected void TxtRequiredDate_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(TxtAssignDate.Text) > Convert.ToDateTime(TxtRequiredDate.Text))
        {
            TxtRequiredDate.Text = "";
            LblErrorMessage.Visible = true;
            TxtRequiredDate.Focus();
            LblErrorMessage.Text = "Assign Date can't be more than Required Date.......... ";
        }
        else
        {
            LblErrorMessage.Text = "";
            LblErrorMessage.Visible = false;
        }
    }
    protected void DDunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid_New();
    }
    protected void TxtsupplyOrderNoEdit_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        if (TxtsupplyOrderNoEdit.Text != "")
        {
            string Str = @"Select PM.CompanyId,CustomerId,PD.OrderId,9 ProcessId,EmpId,PM.IssueOrderId,SupplyOrderNo,Freight,Insurance,PaymentAt,Destination,
                    Liasoning,Inspection,SampleNumber,PM.Remarks,Instruction,UnitId,Replace(convert(varchar(11),isnull(PM.AssignDate,''),106), ' ','-') AssignDate,
                    Replace(convert(varchar(11),isnull(PD.ReqByDate,''),106), ' ','-') ReqByDate,CalType 
                    from Process_Issue_Master_9 PM,PROCESS_ISSUE_DETAIL_9 PD,OrderMaster OM 
                    Where PM.IssueOrderid=PD.IssueOrderid And PD.OrderId=OM.OrderId And PM.SupplyOrderNo=" + TxtsupplyOrderNoEdit.Text + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDCustomerCode.SelectedValue = Ds.Tables[0].Rows[0]["CustomerId"].ToString();
                CustomerCodeSelectedChange();
                DDCustomerOrderNumber.SelectedValue = Ds.Tables[0].Rows[0]["OrderId"].ToString();
                UtilityModule.ConditionalComboFill(ref DDProcessName, "SELECT DISTINCT PROCESS_Name_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME_ID=9 And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid  And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");
                DDEmployeeName.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                TxtAssignDate.Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
                TxtRequiredDate.Text = Ds.Tables[0].Rows[0]["ReqByDate"].ToString();
                EmployeeNameSelectedChange();
                DDSupplyOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                TxtIssueNo.Text = Ds.Tables[0].Rows[0]["SupplyOrderNo"].ToString();
                DDFreight.SelectedValue = Ds.Tables[0].Rows[0]["Freight"].ToString();
                DDInsurance.SelectedValue = Ds.Tables[0].Rows[0]["Insurance"].ToString();
                DDPaymentAt.SelectedValue = Ds.Tables[0].Rows[0]["PaymentAt"].ToString();
                TxtDestination.Text = Ds.Tables[0].Rows[0]["Destination"].ToString();
                TxtLiasoning.Text = Ds.Tables[0].Rows[0]["Liasoning"].ToString();
                TxtInspection.Text = Ds.Tables[0].Rows[0]["Inspection"].ToString();
                TxtSampleNumber.Text = Ds.Tables[0].Rows[0]["SampleNumber"].ToString();
                TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
                TxtInstructions.Text = Ds.Tables[0].Rows[0]["Instruction"].ToString();
                DDunit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
                DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["Caltype"].ToString();
                SupplyOrderNoSelectedChange();
                Fill_ProcessNameSelectedChange();
                ProcessReportPath();
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Please Enter Proper Supply Order No";
                TxtsupplyOrderNoEdit.Text = "";
                DDCustomerCode.SelectedIndex = 0;
                string sqlstr = @"Select VF.Item_Finished_Id Sr_No,Category_Name Category,Item_Name Item,QualityName Quality,
                '' Design,'' Color,ShapeName Shape,Case When " + DDunit.SelectedValue + @"=1 Then SizeMtr Else SizeFt End Size,Sum(PreProdAssignedQty) OQty,'' SQTY,
                Sum(Case When " + DDunit.SelectedValue + @"=1 Then AreaMtr*J.PreProdAssignedQty Else AreaFt*J.PreProdAssignedQty End) Area,'' IssQty,'' Rate,
                Case When SAO.ArticleNo IS NULL Then OD.ArticalNo Else SAO.ArticleNo End ArticalNo,SubQuantity SubQuality
                FROM OrderMaster AS OM INNER JOIN OrderDetail AS OD ON OM.OrderId=OD.OrderId INNER JOIN QualityCodeMaster AS QM ON 
                OD.QualityCodeId=QM.QualityCodeId INNER JOIN
                V_FinishedItemDetail AS VF ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID INNER JOIN JobAssigns AS J ON OD.Item_Finished_Id=J.ITEM_FINISHED_ID AND 
                OD.OrderId=J.OrderId LEFT OUTER JOIN SUPPLY_ARTICLE_NO SAO ON OM.CustomerId=SAO.CustomerId AND OD.Item_Finished_Id=SAO.Finishedid
                WHERE OM.OrderId=0 And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Group By VF.Item_Finished_Id,Category_Name,Item_Name,QualityName,
                DesignName,ColorName,ShapeName,SizeMtr,SizeFt,SAO.ArticleNo,OD.ArticalNo,SubQuantity";

                DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
                DGDetail.DataSource = DS;
                DGDetail.DataBind();
                TxtsupplyOrderNoEdit.Focus();
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        // ChkForEdit.TabIndex = -1;
        CheckForEditChange();
    }
    private void CheckForEditChange()
    {
        if (DDCustomerCode.Items.Count > 0)
        {
            DDCustomerCode.SelectedIndex = 0;
        }
        if (DDCustomerCode.Items.Count > 0 && Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 7)
        {
            DDCustomerCode.SelectedIndex = 1;
            CustomerCodeSelectedChange();
        }
        if (DDCustomerOrderNumber.Items.Count > 0)
        {

            DDCustomerOrderNumber.SelectedIndex = 0;
        }
        if (DDProcessName.Items.Count > 0)
        {
            DDProcessName.SelectedIndex = 0;
        }
        if (DDEmployeeName.Items.Count > 0)
        {
            DDEmployeeName.SelectedIndex = 0;
        }
        if (DDSupplyOrderNo.Items.Count > 0)
        {
            DDSupplyOrderNo.SelectedIndex = 0;
        }
        if (ChkForEdit.Checked == true)
        {
            TDSupplyOrderNoEdit.Visible = true;
            TDLblSupplyOrderNo.Visible = true;
            TDLblDDSupplyOrderNo.Visible = true;
            TDDDSupplyOrderNo.Visible = true;
            TxtsupplyOrderNoEdit.Focus();
            Btnstatus.Visible = true;
        }
        else
        {
            TDSupplyOrderNoEdit.Visible = false;
            TDLblSupplyOrderNo.Visible = false;
            TDLblDDSupplyOrderNo.Visible = false;
            TDDDSupplyOrderNo.Visible = false;
            Btnstatus.Visible = false;
        }
    }
    protected void DDSupplyOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "Select PM.CompanyId,CustomerId,PD.OrderId,1 ProcessId,EmpId,PM.IssueOrderId,SupplyOrderNo,Freight,Insurance,PaymentAt,Destination,Liasoning,Inspection,SampleNumber,PM.Remarks,Instruction,UnitId,Replace(convert(varchar(11),isnull(PM.AssignDate,''),106), ' ','-') AssignDate,Replace(convert(varchar(11),isnull(PD.ReqByDate,''),106), ' ','-') ReqByDate,Caltype from Process_Issue_Master_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.OrderId=OM.OrderId And PM.IssueOrderId=" + DDSupplyOrderNo.SelectedValue + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtAssignDate.Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
            TxtRequiredDate.Text = Ds.Tables[0].Rows[0]["ReqByDate"].ToString();
            TxtIssueNo.Text = Ds.Tables[0].Rows[0]["SupplyOrderNo"].ToString();
            DDFreight.SelectedValue = Ds.Tables[0].Rows[0]["Freight"].ToString();
            DDInsurance.SelectedValue = Ds.Tables[0].Rows[0]["Insurance"].ToString();
            DDPaymentAt.SelectedValue = Ds.Tables[0].Rows[0]["PaymentAt"].ToString();
            TxtDestination.Text = Ds.Tables[0].Rows[0]["Destination"].ToString();
            TxtLiasoning.Text = Ds.Tables[0].Rows[0]["Liasoning"].ToString();
            TxtInspection.Text = Ds.Tables[0].Rows[0]["Inspection"].ToString();
            TxtSampleNumber.Text = Ds.Tables[0].Rows[0]["SampleNumber"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
            TxtInstructions.Text = Ds.Tables[0].Rows[0]["Instruction"].ToString();
            DDunit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["Caltype"].ToString();
            SupplyOrderNoSelectedChange();
            Fill_ProcessNameSelectedChange();
            Session["IssueOrderid"] = DDSupplyOrderNo.SelectedValue;
            Session["processId"] = DDProcessName.SelectedValue;
            TxtsupplyOrderNoEdit.Text = TxtIssueNo.Text;
        }
        DDFreight.Focus();

    }
    private void SupplyOrderNoSelectedChange()
    {
        Session["IssueOrderid"] = DDSupplyOrderNo.SelectedValue;
        Fill_Grid_New_Edit();
    }
    private void Fill_Grid_New_Edit()
    {
        DGDetail.DataSource = GetDetailNew_Edit();
        DGDetail.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetailNew_Edit()
    {
        DataSet DS = null;
        string sqlstr = @"Select VF.Item_Finished_Id Sr_No,Category_Name Category,Item_Name Item,QualityName Quality,
                         [dbo].[Get_SupplyerDesignColorName] (" + DDSupplyOrderNo.SelectedValue + @",VF.Item_Finished_Id,1) Design,
                         [dbo].[Get_SupplyerDesignColorName] (" + DDSupplyOrderNo.SelectedValue + @",VF.Item_Finished_Id,2) Color,ShapeName Shape,
                         Case When " + DDunit.SelectedValue + @"=1 Then SizeMtr when " + DDunit.SelectedValue + @"=6 then sizeinch Else SizeFt End Size,Sum(J.SupplierQty) OQty,'' SQTY,
                         Sum(Case When " + DDunit.SelectedValue + @"=1 Then AreaMtr*J.SupplierQty Else AreaFt*J.SupplierQty End) Area,'' IssQty,'' Rate,
                         Case When SAO.ArticleNo IS NULL Then OD.ArticalNo Else SAO.ArticleNo End ArticalNo,SubQuantity SubQuality,0 RecQty
                         FROM OrderMaster AS OM INNER JOIN OrderDetail AS OD ON OM.OrderId=OD.OrderId LEFT OUTER JOIN QualityCodeMaster AS QM ON 
                         OD.QualityCodeId=QM.QualityCodeId INNER JOIN
                         V_FinishedItemDetail AS VF ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID INNER JOIN JobAssigns AS J ON OD.Item_Finished_Id=J.ITEM_FINISHED_ID AND 
                         OD.OrderId=J.OrderId LEFT OUTER JOIN SUPPLY_ARTICLE_NO SAO ON OM.CustomerId=SAO.CustomerId AND OD.Item_Finished_Id=SAO.Finishedid
                         WHERE J.SupplierQty>0 And OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Group By VF.Item_Finished_Id,Category_Name,Item_Name,QualityName,
                         DesignName,ColorName,ShapeName,SizeMtr,SizeFt,SAO.ArticleNo,OD.ArticalNo,SubQuantity,sizeinch";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmSupplyOrder.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return DS;
    }
    protected void refreshEmp_Click(object sender, EventArgs e)
    {
        ProcessName();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void ProcessReportPath()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Session["ReportPath"] = "Reports/ProductionOrder.rpt";
            #region Author: Rajeev, Date 28-Nov-12 ...

            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, " Delete TEMP_PROCESS_ISSUE_MASTER   Delete TEMP_PROCESS_ISSUE_DETAIL");
            string str = " Insert into TEMP_PROCESS_ISSUE_MASTER Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + Session["processId"] + " from PROCESS_ISSUE_MASTER_" + Session["processId"] + " Where IssueOrderId=" + Session["IssueOrderid"] + "";
            str = str + "  Insert into TEMP_PROCESS_ISSUE_DETAIL Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + Session["processId"] + " Where IssueOrderId=" + Session["IssueOrderid"] + "";
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, str);
            tran.Commit();
            con.Close();
            con.Dispose();

        }
        catch (Exception ex)
        {
            tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmSupplyOrder.aspx || ProcessReprortPath()");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
            #endregion
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        ProcessReportPath();
        Session["CommanFormula"] = "{View_Production_Issue_Order.IssueOrderid}=" + Session["IssueOrderid"] + "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Session["IssueOrderid"] = 0;
    }
    //protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            //e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "Select$" + e.Row.RowIndex);
        }
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[9].Visible = false;
        }
    }
    private void fillqty()
    {
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            string oqty = DGDetail.Rows[i].Cells[7].Text;
            ((TextBox)DGDetail.Rows[i].FindControl("TxtDGIssQty")).Text = oqty;
        }
    }

    protected void Btnstatus_Click(object sender, EventArgs e)
    {
        if (DDSupplyOrderNo.SelectedIndex > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update  PROCESS_ISSUE_Master_9 set status='Cancelled' where IssueOrderid=" + DDSupplyOrderNo.SelectedValue + " ");
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Order Status Is Updated.....", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please Select Supply Order No....", true);
        }
    }
}