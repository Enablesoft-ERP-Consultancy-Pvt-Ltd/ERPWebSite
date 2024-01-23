using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_ProcessIssue_ProcessIssue : System.Web.UI.Page
{
    // int PROCESS_ISSUE_ID = 0;
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CompanyName", true, "--Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select Distinct CI.Customerid,CI.Customercode + SPACE(5)+CI.CompanyName from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDunit, "select unitid,unitname from unit where unitid in (1,2,4)", true, "--Select--");
            DDunit.SelectedIndex = 3;
            DDcaltype.SelectedIndex = 1;
            Session["IssueOrderid"] = 0;
            lablechange();
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 1:
                    TxtProductCode.Visible = false;
                    LblProdCode.Visible = false;
                    break;
                case 2:
                    TxtProductCode.Visible = true;
                    LblProdCode.Visible = true;
                    break;
            }
            DDCustomerCode.Focus();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select Distinct CI.Customerid,CI.Customercode + SPACE(5)+CI.CompanyName from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.Customerid=" + DDCustomerCode.SelectedValue, true, "--Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDProcessName, "Select PROCESS_Name_ID,PROCESS_NAME from ProcessOrderWise Where Orderid=" + DDCustomerOrderNumber.SelectedValue + " Order By PCMID", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDCategoryName, @"SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        TxtAssignDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
        TxtRequiredDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
    }

    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct IPM.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN Item_Parameter_Master IPM ON IPM.Item_Finished_id=OD.Item_Finished_id INNER JOIN ITEM_MASTER IM ON IPM.ITEM_ID=IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select---");
    }

   
    //-------------------------------------------------
    private void fill_QuantityRequired()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            TxtTotalQty.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "select  PreProdAssignedQty from JobAssigns where OrderId=" + DDCustomerOrderNumber.SelectedValue + "and Item_Finished_ID=" + DDDescription.SelectedValue));
            TxtPreQuantity.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0) PQty from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue)));
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    //-------------------------------------------------------------------------------------------------
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Description();
    }
    private void Fill_Description()
    {
        string STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(SizeFt,'') Else Case When 1=" + DDunit.SelectedValue + " Then Isnull(SizeMtr,'') Else '' End End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and IPM.Item_Id=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
    }
    private void Area()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int SizeId = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select size_Id from Item_Parameter_Master where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
            if (SizeId != 0)
            {
                LblArea.Visible = true;
                TdArea.Visible = true;

                SqlParameter[] _arrpara = new SqlParameter[6];

                _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@ShapeId", SqlDbType.Int);

                _arrpara[0].Value = SizeId;
                _arrpara[1].Value = DDunit.SelectedValue;
                _arrpara[2].Direction = ParameterDirection.Output;
                _arrpara[3].Direction = ParameterDirection.Output;
                _arrpara[4].Direction = ParameterDirection.Output;
                _arrpara[5].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Area", _arrpara);
                TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                TxtArea.Text = string.Format("{0:#0.00}", _arrpara[4].Value);
            }
            else
            {
                LblArea.Visible = false;
                TdArea.Visible = false;
                TxtArea.Text = "0";

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        ProcessIssue();
        Fill_Grid();
        ProcessReportPath();
    }
    private void ProcessReportPath()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, " Delete TEMP_PROCESS_ISSUE_MASTER");
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, " Delete TEMP_PROCESS_ISSUE_DETAIL");
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, " Insert into TEMP_PROCESS_ISSUE_MASTER Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + Session["processId"] + " from PROCESS_ISSUE_MASTER_" + Session["processId"] + " Where IssueOrderId=" + Session["IssueOrderid"] + "");
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, " Insert into TEMP_PROCESS_ISSUE_DETAIL Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + Session["processId"] + " Where IssueOrderId=" + Session["IssueOrderid"] + "");
        Session["ReportPath"] = "Reports/ProductionOrder.rpt";
        Session["CommanFormula"] = "{View_Production_Issue_Order.IssueOrderid}=" + Session["IssueOrderid"] + "";
        con.Close();
        con.Dispose();
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[23];

            _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.DateTime);
            _arrpara[3] = new SqlParameter("@Status", SqlDbType.NVarChar);
            _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
            _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

            _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[11] = new SqlParameter("@Length", SqlDbType.Float);
            _arrpara[12] = new SqlParameter("@Width", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[15] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.DateTime);
            _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Int);

            _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.Float);
            _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);
            int num;
            if (Convert.ToUInt32(Session["IssueOrderid"]) == 0 || DDProcessName.SelectedValue != Convert.ToString(Session["processId"]))
            {
                // Session["IssueOrderid"]
                int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                Session["IssueOrderid"] = a;
                num = 1;
            }
            else
            {
                num = 0;
            }
            _arrpara[0].Value = (Session["IssueOrderid"]);
            _arrpara[1].Value = DDEmployeeName.SelectedValue;
            _arrpara[2].Value = TxtAssignDate.Text;
            _arrpara[3].Value = "Pending";
            _arrpara[4].Value = DDunit.SelectedValue;
            _arrpara[5].Value = Session["varuserid"];
            _arrpara[6].Value = Convert.ToString(TxtRemarks.Text).ToUpper();
            _arrpara[7].Value = TxtInstructions.Text;
            _arrpara[8].Value = DDCompanyName.SelectedValue;

            _arrpara[9].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Issue_Detail_Id),0)+1 from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue);
            _arrpara[10].Value = DDDescription.SelectedValue;
            _arrpara[11].Value = TxtLength.Text;
            _arrpara[12].Value = TxtWidth.Text;
            _arrpara[13].Value = TxtArea.Text;
            _arrpara[14].Value = TxtRate.Text;
            if (DDcaltype.SelectedValue == "0")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
            }
            if (DDcaltype.SelectedValue == "1")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
            }
            _arrpara[16].Value = TxtQtyRequired.Text;
            _arrpara[17].Value = TxtRequiredDate.Text;
            _arrpara[18].Value = TxtQtyRequired.Text;
            _arrpara[19].Value = "0";
            _arrpara[20].Value = "0";
            _arrpara[21].Value = DDCustomerOrderNumber.SelectedValue;
            _arrpara[22].Value = DDcaltype.SelectedValue;

            Session["processId"] = Convert.ToInt32(DDProcessName.SelectedValue);

            if (num == 1)
            {
                
                string str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                str = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Process_Issue_Master_" + DDProcessName.SelectedValue + "'," + _arrpara[0].Value + ",getdate(),'Insert')");
            }
            string str1 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            DataSet dt1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt1.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Process_Issue_Detail_" + DDProcessName.SelectedValue + "'," + _arrpara[9].Value + ",getdate(),'Insert')");
            
            UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[21].Value), Tran);
           
            Tran.Commit();
            
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Data Successfully Saved.......";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Save_Refresh();
    }
    //**********************************************************************************************************
    private void Fill_Grid()
    {
        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as " + lblcategoryname.Text + @",IM.Item_Name as " + lblitemname.Text + @",IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Length,Width,
                        Length + 'x' + Width Size,Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + Session["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return DS;
    }

    //------------------------TO Refresh the Item parameters On click Save Button----------------------------------------------------
    private void Save_Refresh()
    {
        DDCategoryName.SelectedIndex = 0;
        DDItemName.SelectedIndex = 0;
        DDDescription.SelectedIndex = 0;
        TxtPreQuantity.Text = "";
        TxtTotalQty.Text = "";
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        TxtRate.Text = "";
        TxtQtyRequired.Text = "";
    }

    private void FillDetailBack()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"select IM.CATEGORY_ID,IPM.ITEM_ID,PID.ORDER_QTY,PID.REQUIRED_DATE,IPM.Item_Finished_Id,PID.Area,PID.RATE,PID.REMARKS,PID.Instructions
                          from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " AS PID Inner Join ITEM_PARAMETER_MASTER as IPM ON PID.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID Inner Join ITEM_MASTER AS IM ON IPM.ITEM_ID = IM.ITEM_ID where Issue_Detail_Id=" + DGOrderdetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);


            UtilityModule.ConditionalComboFill(ref DDCategoryName, "SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId where OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select---");
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDescription, " select JA.Item_Finished_Id,Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join Item_Parameter_Master IPM On IPM.Item_Finished_Id=JA.Item_Finished_Id where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
            DDDescription.SelectedValue = ds.Tables[0].Rows[0]["Item_Finished_Id"].ToString();

            TxtQtyRequired.Text = ds.Tables[0].Rows[0]["ORDER_QTY"].ToString();
            TxtArea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
            if (Convert.ToDouble(TxtArea.Text) > 0.0)
            {
                TdArea.Visible = true;
                LblErrorMessage.Visible = true;
            }
            else
            {
                TdArea.Visible = false;
                LblErrorMessage.Visible = false;
            }
            TxtRequiredDate.Text = ds.Tables[0].Rows[0]["REQUIRED_DATE"].ToString();
            fill_QuantityRequired();
            TxtPreQuantity.Text = Convert.ToString(Convert.ToInt32(TxtPreQuantity.Text) - Convert.ToInt32(TxtQtyRequired.Text));
            TxtRemarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
            TxtInstructions.Text = ds.Tables[0].Rows[0]["Instructions"].ToString();
            TxtRate.Text = ds.Tables[0].Rows[0]["RATE"].ToString();
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }

    protected void DGOrderdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDetailBack();
        BtnUpdate.Visible = true;
        BtnSave.Visible = false;
    }

    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        Update_Process_Issue_Detail();
        Fill_Grid();
        BtnSave.Visible = true;
        BtnUpdate.Visible = false;

    }
    private void Update_Process_Issue_Detail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[6];

            _arrpara[0] = new SqlParameter("@processId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ORDER_QTY", SqlDbType.Float);
            _arrpara[3] = new SqlParameter("@REQUIRED_DATE", SqlDbType.DateTime);
            _arrpara[4] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[5] = new SqlParameter("@Area", SqlDbType.Float);

            _arrpara[0].Value = DDProcessName.SelectedValue;
            _arrpara[1].Value = DGOrderdetail.SelectedValue;
            _arrpara[2].Value = TxtQtyRequired.Text;
            _arrpara[3].Value = TxtRequiredDate.Text;
            _arrpara[4].Value = TxtRate.Text;
            _arrpara[5].Value = TxtArea.Text;


            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Update_Process_Issue_Detail", _arrpara);
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Save_Refresh();
    }
    //*************************Validate Assign Date & Required Date************************

    private void validateRequiredDate()
    {
        if (Convert.ToDateTime(TxtAssignDate.Text) < Convert.ToDateTime(TxtRequiredDate.Text))
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Required Date must be greater then Assign......";
        }
        else
        {
            LblErrorMessage.Visible = false;
        }
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_After_DDDescription();
    }
    private void Fill_After_DDDescription()
    {
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        fill_QuantityRequired();
        Area();
        MASTER_RATE();
        TxtRemarks.Text = (SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Isnull(WeavingInstruction,'') from OrderDetail Where Orderid=" + DDCustomerOrderNumber.SelectedValue + " And Item_Finished_Id=" + DDDescription.SelectedValue + "")).ToString();
    }

    private void MASTER_RATE()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        TxtRate.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "SELECT IsNull(Sum(RATE),0) RATE FROM PROCESS_MASTER Where PROCESS_NAME_ID=" + DDProcessName.SelectedValue + " AND ITEM_FINISHED_ID=" + DDDescription.SelectedValue + ""));
    }

    protected void TxtQtyRequired_TextChanged(object sender, EventArgs e)
    {
        if (TxtQtyRequired.Text != "")
        {
            if (Convert.ToDouble(TxtQtyRequired.Text) > 0 && Convert.ToDouble(TxtQtyRequired.Text) <= Convert.ToDouble(TxtPreQuantity.Text))
            {
                LblErrorMessage.Visible = false;
            }
            else
            {
                TxtQtyRequired.Text = "";
                TxtQtyRequired.Focus();
                LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater Then Zero and less then PQty.........................";
                LblErrorMessage.Visible = true;
            }

        }
        else
        {
            LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater Then Zero.........................";
            LblErrorMessage.Visible = true;

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

    protected void TxtLength_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }

    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 0, 0));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 0, 0));
            }
        }
    }
   
    protected void TxtProductCode_TextChanged(object sender, EventArgs e)
    {
        Out_ProdCode_TextChanges();
    }
    private void Out_ProdCode_TextChanges()
    {
        DataSet ds;
        string Str;
        LblErrorMessage.Text = "";
        try
        {
            if (TxtProductCode.Text != "")
            {
                UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_Id", true, "--SELECT--");
                Str = "Select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,OrderDetail OD where IPM.ITEM_ID=IM.ITEM_ID and OD.Item_Finished_id=IPM.Item_Finished_id and OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and ProductCode='" + TxtProductCode.Text + "' And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    OUT_CATEGORY_DEPENDS_CONTROLS();
                    DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    Fill_Description();
                    DDDescription.SelectedValue = ds.Tables[0].Rows[0]["ITEM_FINISHED_ID"].ToString();
                    Fill_After_DDDescription();
                }
                else
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                    DDCategoryName.SelectedIndex = 0;
                    OUT_CATEGORY_DEPENDS_CONTROLS();
                    TxtProductCode.Text = "";
                    TxtProductCode.Focus();
                }
            }
            else
            {
                DDCategoryName.SelectedIndex = 0;
                OUT_CATEGORY_DEPENDS_CONTROLS();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueDestini.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Some Importent fields Missing...........";
        }


    }
    private void OUT_CATEGORY_DEPENDS_CONTROLS()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + DDCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where ProductCode Like  '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId;
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
    public static int FTest(DropDownList DDCustomerOrderNumber)
    {
        int Varorderid = Convert.ToInt32(DDCustomerOrderNumber.SelectedValue);
        return Varorderid;
    }
}