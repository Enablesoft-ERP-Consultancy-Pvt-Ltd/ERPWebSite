using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_EditProcessIssueNew : System.Web.UI.Page
{
    private DataSet DS;
    int PROCESS_ISSUE_ID = 0;
    static int hnOrderId = 0;
    static int hnOldQty = 0;
    static string btnclickflag = "";
    static int VarProcess_Issue_Detail_Id = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            DDCompanyName.Focus();
            ChkEditOrder.Checked = true;
            DDPOrderNo.Visible = true;

            DataSet ds = new DataSet();
            string str = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName
                           SELECT Distinct CI.CustomerId,CI.CustomerCode CustomerCode From CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid Order By CI.CustomerCode
                           select unitid,unitname from unit where unitid in (1,2,4,6,7)";
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, true, "--Select--");

            DDunit.SelectedIndex = 2;
            ViewState["IssueOrderid"] = PROCESS_ISSUE_ID;
            ParameteLabel();
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "2":
                    TDItemCode1.Visible = false;
                    TDItemCode2.Visible = false;
                    break;
                case "9":
                    btnLoomDetail.Visible = true;
                    chkforsummary.Visible = true;
                    BtnSave.Visible = false;
                    break;
            }
            hncomp.Value = Session["varMasterCompanyIDForERP"].ToString();
            Fill_Temp_OrderNo();
            DDCustomerCode.Focus();
            BindProcess();
        }
    }
    private void Fill_Temp_OrderNo()
    {
        string strinsert = "", str = "";
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_ISSUE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_name_id from PROCESS_NAME_MASTER ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                str = "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW select Pm.Companyid,Om.CustomerId,Pd.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + ",PM.Empid,Pm.issueorderid,PM.ChallanNO from PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM inner join PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD on PM.IssueOrderId=PD.IssueOrderId  inner join OrderMaster OM on Om.OrderId=Pd.Orderid where PM.Status!='Canceled' and PM.SampleNumber=''";
                strinsert = strinsert + "  " + str;
            }
            if (strinsert != "")
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strinsert);
            }
        }
        Ds.Dispose();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT distinct CI.CustomerId,CI.CustomerCode From CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid order by CI.CustomerCode", true, "--Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    private void CustomerCodeSelectedChange()
    {
        //SqlConnection con = new SqlConnection();
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, "Select  Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.Customerid=" + DDCustomerCode.SelectedValue + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " order by OM.Orderid", true, "--Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerOrderNumberSelectedChange();
    }
    private void CustomerOrderNumberSelectedChange()
    {
        string str;
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            str = @"SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        }
        else
        {
            str = @"SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        }
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 0, true, "--Select--");

        if (DDCategoryName.Items.Count > 0)
        {
            DDCategoryName.SelectedIndex = 1;
            BindCategory();
        }


        //        string str;
        //        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        //        {
        //            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER  where process_name_id in(1,16)
        //                    SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        //        }
        //        else
        //        {
        //            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where Process_Name_id=1
        //                       SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        //        }
        //        DataSet ds = new DataSet();
        //        ds = SqlHelper.ExecuteDataset(str);
        //        UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Select--");
        //        if (Session["varMasterCompanyIDForERP"].ToString() == "20")
        //        {
        //            DDProcessName.Enabled = false;
        //            if (DDProcessName.Items.Count > 0)
        //            {
        //                DDProcessName.SelectedIndex = 1;
        //                ProcessSelectedChange();
        //            }
        //        }
        //        UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 1, true, "--Select--");
    }
    protected void BindProcess()
    {
        string str;
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER  where process_name_id in(1,16)";
        }
        else
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where Process_Name_id=1";
        }
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Select--");
        if (Session["varMasterCompanyIDForERP"].ToString() == "20")
        {
            DDProcessName.Enabled = false;
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                ProcessSelectedChange();
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,PROCESS_ISSUE_Master_" + DDProcessName.SelectedValue + " PM Where PM.EmpId=Ei.EmpId Order By EI.EmpName", true, "--Select--");
        if (DDEmployeeName.Items.Count > 0)
        {
            DDEmployeeName.SelectedIndex = 1;
            EmployeeSelectedChange();
        }
        TxtAssignDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
    }
    private void EmployeeSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDPOrderNo, "select Distinct PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PD where PM.Empid=" + DDEmployeeName.SelectedValue + " and PM.CompanyId=" + DDCompanyName.SelectedValue + " and PM.IssueOrderId=PD.IssueOrderId and PM.Status!='Canceled' and PM.SampleNumber='' order by PM.IssueOrderId", true, "--Select--");
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindCategory();
    }
    protected void BindCategory()
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "6")
        {
            string STR;
            if (variable.VarNewQualitySize == "1")
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id and pd.issueorderid=" + DDPOrderNo.SelectedValue + @"
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty)";
            }
            else
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id and pd.issueorderid=" + DDPOrderNo.SelectedValue + @"
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty)";
            }
            UtilityModule.ConditionalComboFill(ref DDItemName, STR, true, "---Select---");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");
        }

        if (DDItemName.Items.Count > 0)
        {
            DDItemName.SelectedIndex = 1;
            dditemname_chage();

        }
    }
    //-------------------------------------------------
    private void fill_QuantityRequired()
    {
        TxtTotalQty.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select  Sum(PreProdAssignedQty) as PreProdAssignedQty  from JobAssigns where OrderId=" + DDCustomerOrderNumber.SelectedValue + "and Item_Finished_ID=" + DDDescription.SelectedValue));
        String Str = "Select IsNull(Sum(PID.Qty),0)-isnull(Sum(PID.CancelQty),0) PQty from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status!='Canceled'";
        if (BtnUpdate.Visible == true)
        {
            Str = Str + " And PID.Issue_Detail_Id!=" + DGOrderdetail.SelectedValue;
        }
        TxtPreQuantity.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str)));

        TxtQtyRequired.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str)));
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        dditemname_chage();
    }
    private void dditemname_chage()
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "6")
        {
            string STR = "";
            if (variable.VarNewQualitySize == "1")
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(Sizeinch,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else Isnull(ProdSizeFt,'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  and pd.issueorderid=" + DDPOrderNo.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty) order by Description";
            }
            else
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(Sizeinch,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else Isnull(ProdSizeFt,'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  and pd.issueorderid=" + DDPOrderNo.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty) order by Description";
            }
            UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
        }
        else
        {
            string STR = "";
            if (variable.VarNewQualitySize == "1")
            {
                STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 1=" + DDunit.SelectedValue + "  Then ProdSizeMtr Else case When 6=" + DDunit.SelectedValue + " Then SizeInch Else ProdSizeFt End End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On IPM.FinishedId=JA.Item_Finished_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " order by Description";
            }
            else
            {
                STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 1=" + DDunit.SelectedValue + "  Then ProdSizeMtr Else case When 6=" + DDunit.SelectedValue + " Then SizeInch Else ProdSizeFt End End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " order by Description";
            }
            UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
        }
    }
    private void Area()
    {
        try
        {
            //DataSet dt;
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select size_Id,SHAPE_ID from Item_Parameter_Master where Item_Finished_Id=" + DDDescription.SelectedValue);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                int SizeId = Convert.ToInt32(Ds.Tables[0].Rows[0]["size_Id"]);
                if (SizeId != 0 && Session["varMasterCompanyIDForERP"].ToString() != "6")
                {
                    //LblArea.Visible = true;
                    TdArea.Visible = true;
                    SqlParameter[] _arrpara = new SqlParameter[9];
                    _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                    _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@ShapeId", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@Khap", SqlDbType.VarChar, (100));
                    _arrpara[7] = new SqlParameter("@KhapLength", SqlDbType.Float);
                    _arrpara[8] = new SqlParameter("@Khapwidth", SqlDbType.Float);

                    _arrpara[0].Value = SizeId;
                    _arrpara[1].Value = DDunit.SelectedValue;
                    _arrpara[2].Direction = ParameterDirection.Output;
                    _arrpara[3].Direction = ParameterDirection.Output;
                    _arrpara[4].Direction = ParameterDirection.Output;
                    _arrpara[5].Direction = ParameterDirection.Output;
                    _arrpara[6].Direction = ParameterDirection.Output;
                    _arrpara[7].Direction = ParameterDirection.Output;
                    _arrpara[8].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_AreaNew", _arrpara);

                    if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                    {
                        switch (DDunit.SelectedValue)
                        {
                            case "2": //ft
                                TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                                TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                                TxtKhap.Text = string.Format("{0:#0.00}", _arrpara[6].Value);
                                txtAfterKhapSizeOrder.Text = string.Format("{0:#0.00}", _arrpara[8].Value) + 'x' + string.Format("{0:#0.00}", _arrpara[7].Value);
                                break;
                            default:
                                TxtLength.Text = _arrpara[2].Value.ToString();
                                TxtWidth.Text = _arrpara[3].Value.ToString();
                                TxtKhap.Text = _arrpara[6].Value.ToString();
                                txtAfterKhapSizeOrder.Text = _arrpara[8].Value.ToString() + 'x' + _arrpara[7].Value.ToString();
                                break;
                        }
                    }
                    else
                    {
                        TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                        TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                        TxtKhap.Text = string.Format("{0:#0.00}", _arrpara[6].Value);
                        txtAfterKhapSizeOrder.Text = string.Format("{0:#0.00}", _arrpara[8].Value) + 'x' + string.Format("{0:#0.00}", _arrpara[7].Value);
                    }
                    //TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
                    if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                    {
                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"])));
                    }

                    if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                    {
                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(_arrpara[7].Value.ToString()), Convert.ToDouble(_arrpara[8].Value.ToString()), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"]), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
                    }

                }
                else if (SizeId != 0 && Session["varMasterCompanyIDForERP"].ToString() == "6")
                {
                    //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                    string str = "";
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = "select ExpWidthMS_Ft as WidthFt,ExpLengthMS_Ft as LengthFt, 0 as HeightFt,LEFT(MtrSize, CHARINDEX('x', MtrSize) - 1) AS WidthMtr,REPLACE(SUBSTRING(MtrSize, CHARINDEX('x', MtrSize), LEN(MtrSize)), 'x', '') as LengthMtr,0 as HeightMtr,Export_Area as AreaFt,MtrArea as AreaMtr,KhapWidth+'x'+KhapLength as Khap from QualitySizeNew where sizeid=" + SizeId + "";
                    }
                    else
                    {
                        str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr,0 as Khap from size where sizeid=" + SizeId + "";
                    }
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (DDunit.SelectedValue == "2")
                    {
                        TxtLength.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["LengthFt"].ToString());
                        TxtWidth.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["WidthFt"].ToString());
                        TxtArea.Text = string.Format("{0:#0.0000}", DS.Tables[0].Rows[0]["AreaFt"].ToString());
                        TxtKhap.Text = string.Format("{0:#0.0000}", DS.Tables[0].Rows[0]["Khap"].ToString());
                    }
                    else
                    {
                        TxtLength.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["LengthMtr"].ToString());
                        TxtWidth.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["Widthmtr"].ToString());
                        decimal area;
                        area = Convert.ToDecimal((Convert.ToDecimal(TxtLength.Text) * Convert.ToDecimal(TxtWidth.Text) * Convert.ToDecimal(10.764)) / 10000);
                        TxtArea.Text = string.Format("{0:#0.0000}", area);
                        TxtKhap.Text = string.Format("{0:#0.0000}", DS.Tables[0].Rows[0]["Khap"].ToString());
                    }
                }
                else
                {
                    // LblArea.Visible = false;
                    TdArea.Visible = false;
                    TxtArea.Text = "0";
                }
                //datatset dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (CKHUPDATEWEAVERNAME.Checked == true)
        {
            LblErrorMessage.Text = "";
            btnclickflag = "";
            btnclickflag = "BtnUpdateWeaverName";
            txtpwd.Focus();
            Popup(true);

        }
        else
        {
            ProcessIssue();
        }
    }
    private void UpdateWeaverName()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrpara = new SqlParameter[4];

                _arrpara[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@Empid", SqlDbType.Int);

                _arrpara[0].Value = (DDProcessName.SelectedValue);
                _arrpara[1].Value = (ViewState["IssueOrderid"]);
                _arrpara[2].Value = DDEmployeeName.SelectedValue;
                _arrpara[3] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
                _arrpara[3].Direction = ParameterDirection.Output;

                //**********

                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_UpdateWeaverName", _arrpara);

                Tran.Commit();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = _arrpara[3].Value.ToString();
                //LblErrorMessage.Text = "Data Successfully Saved.......";
                Fill_Grid();
                CKHUPDATEWEAVERNAME.Checked = false;

            }
            else
            {
                Tran.Commit();
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Check_Length_Width_Format();
            if (CKHCURRENTCONSUMPTION.Checked == false)
            {
                CHECKVALIDCONTROL();

                ChkDuplicateData();
                if (Session["varMasterCompanyIDForERP"].ToString() == "20")
                {
                    ChkItemFinishedId();
                }
                if (LblErrorMessage.Text == "")
                {
                    SqlParameter[] _arrpara = new SqlParameter[29];

                    _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.NVarChar);
                    _arrpara[3] = new SqlParameter("@Status", SqlDbType.DateTime);
                    _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
                    _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 8000);
                    _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar, 8000);
                    _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

                    _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Float);
                    _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.DateTime);
                    _arrpara[11] = new SqlParameter("@Length", SqlDbType.Int);
                    _arrpara[12] = new SqlParameter("@Width", SqlDbType.Int);
                    _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[14] = new SqlParameter("@Rate", SqlDbType.NVarChar);
                    _arrpara[15] = new SqlParameter("@Amount", SqlDbType.NVarChar);
                    _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Float);
                    _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.Int);
                    _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Float);

                    _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
                    _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.DateTime);
                    _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
                    _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);
                    _arrpara[23] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
                    _arrpara[24] = new SqlParameter("@CancelQty", SqlDbType.Int);

                    _arrpara[25] = new SqlParameter("@Khap", SqlDbType.VarChar, 100);
                    _arrpara[26] = new SqlParameter("@Consump", SqlDbType.Float);
                    _arrpara[27] = new SqlParameter("@HSCode", SqlDbType.VarChar, 100);
                    _arrpara[28] = new SqlParameter("@AfterKhapSizeOrder", SqlDbType.VarChar, 50);

                    int num;
                    //if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || DDProcessName.SelectedValue != Convert.ToString(ViewState["ProcessId"]))
                    if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0)
                    {
                        // ViewState["IssueOrderid"]
                        int a;
                        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                        {
                            switch (DDProcessName.SelectedValue)
                            {
                                case "1":
                                case "16":
                                    a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from v_ProcessMaxId"));
                                    break;
                                default:
                                    a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from process_issue_master_" + DDProcessName.SelectedValue + ""));
                                    break;
                            }
                            ViewState["IssueOrderid"] = a;
                            num = 1;
                        }
                        else
                        {
                            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                            ViewState["IssueOrderid"] = a;
                            num = 1;
                        }
                    }
                    else
                    {
                        num = 0;
                    }
                    _arrpara[0].Value = (ViewState["IssueOrderid"]);
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
                    _arrpara[11].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                    _arrpara[12].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                    _arrpara[13].Value = TxtArea.Text;
                    _arrpara[14].Value = TxtRate.Text;
                    if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                    {
                        _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                        _arrpara[20].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                    }
                    if (DDcaltype.SelectedValue == "1")
                    {
                        _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                        _arrpara[20].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtQtyRequired.Text)));
                    }
                    _arrpara[16].Value = TxtQtyRequired.Text;
                    _arrpara[17].Value = TxtRequiredDate.Text;
                    _arrpara[18].Value = TxtQtyRequired.Text;
                    _arrpara[19].Value = TxtCommission.Text;
                    //_arrpara[20].Value = "0";
                    _arrpara[21].Value = DDCustomerOrderNumber.SelectedValue;
                    _arrpara[22].Value = DDcaltype.SelectedValue;
                    _arrpara[23].Value = ChkForFix.Checked == true ? 0 : 1;
                    _arrpara[24].Value = TxtCancelQty.Text == "" ? "0" : TxtCancelQty.Text;

                    _arrpara[25].Value = TxtKhap.Text == "" ? "0" : TxtKhap.Text;
                    _arrpara[26].Value = TxtConsump.Text == "" ? "0" : TxtConsump.Text;
                    _arrpara[27].Value = TxtHSCode.Text;
                    _arrpara[28].Value = txtAfterKhapSizeOrder.Text;

                    ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);

                    if (num == 1)
                    {
                        #region Author :Rajeev ...
                        //Select IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid from PROCESS_ISSUE_MASTER_1
                        //Select Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid From PROCESS_ISSUE_DETAIL_1
                        string str;
                        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                        {
                            str = @"  Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ")";
                        }
                        else
                        {
                            str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value +
                           @"  Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight,HSCode) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ",'" + _arrpara[27].Value + "')";
                        }

                        //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                        //str = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ")";

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                        #endregion
                    }
                    string str1 = "";
                    str1 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,CancelQty,Khap,Consump,AfterKhapSizeOrder) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[24].Value + ",'" + _arrpara[25].Value + "'," + _arrpara[26].Value + ",'" + _arrpara[28].Value + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                    //UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[21].Value), Tran);
                    UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[21].Value), Tran, effectivedate: TxtAssignDate.Text);
                    str1 = @"Update Process_Issue_Master_" + DDProcessName.SelectedValue + " set Status='Pending' from Process_issue_master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD
                           where PM.IssueOrderId=PD.IssueOrderId And Pqty<>0  And PM.IssueOrderid=" + _arrpara[0].Value + "";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

                    Tran.Commit();
                    DetailNo.Visible = true;
                    lblIssuedetailNo.Text = _arrpara[9].Value.ToString();
                    //SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_ProcessIssue", _arrpara);
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Data Successfully Saved.......";
                    Fill_Grid();
                    Save_Refresh();
                }
                else
                {
                    Tran.Commit();
                }
            }
            else
            {
                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * From Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where IssueOrderid=" + DDPOrderNo.SelectedValue);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(Ds.Tables[0].Rows[i]["IssueOrderid"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["Issue_Detail_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["Item_Finished_id"]), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[i]["Orderid"]), Tran, effectivedate: TxtAssignDate.Text);
                    }
                }
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Consumption Successfully Changed.......";
                Tran.Commit();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            DetailNo.Visible = false;
            lblIssuedetailNo.Text = "";
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void ChkDuplicateData()
    {
        // DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select issueorderid from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " And Item_Finished_id=" + DDDescription.SelectedValue + "");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select issueorderid from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " And Item_Finished_id=" + DDDescription.SelectedValue + " and OrderId=" + DDCustomerOrderNumber.SelectedValue + " ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtQtyRequired.Text = "";
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Duplicate Data Exists.....";
            TxtQtyRequired.Focus();
        }
    }
    private void ChkItemFinishedId()
    {
        int count = DGOrderdetail.Rows.Count;
        string Qualityid = "";
        string Designid = "";

        if (DGOrderdetail.Rows.Count > 0)
        {
            for (int k = 0; k < count; k++)
            {
                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Qualityid,DesignId from ViewFindFinishedidItemidQDCSSNew Where FinishedId=" + DDDescription.SelectedValue + " ");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Qualityid = Ds.Tables[0].Rows[0]["QualityId"].ToString();
                    Designid = Ds.Tables[0].Rows[0]["DesignId"].ToString();
                }

                string GLQualityId = ((Label)(DGOrderdetail.Rows[k].FindControl("lblQualityid"))).Text;
                string GLDesignId = ((Label)(DGOrderdetail.Rows[k].FindControl("lblDesignid"))).Text;
                if (GLQualityId != Qualityid || GLDesignId != Designid)
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Please add same quality type and design item.....";
                }
            }

        }


    }
    //**********************************************************************************************************
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDPOrderNo) == false)
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

        if (UtilityModule.VALIDDROPDOWNLIST(DDunit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDescription) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtQtyRequired) == false)
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
    private void Fill_Grid()
    {

        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();

        if (DGOrderdetail.Rows.Count > 0)
        {
            BtnConsump.Visible = true;
            BtnUpdateRate.Visible = true;
            BtnCancelOrder.Visible = true;
        }
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;

        string sqlstr = "";
        if (variable.VarNewQualitySize == "1")
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch else SizeFt ENd End Description,Length,Width,
                         Width + 'x' + Length Size,Area,Qty*Area as TArea,Rate,Qty,Amount,Isnull(CancelQty,0) CancelQty,PD.OrderId,Itemremark,PD.Item_Finished_Id,ISNULL(IPM.Quality, '') as Quality,ISNULL(IPM.Qualityid, '') as Qualityid, 
                        ISNULL(IPM.Designid, '') as Designid, ISNULL(IPM.Design, '') as Design,Comm,CommAmt,Khap,Consump,ISNULL((Amount+CommAmt),0) as GTotal,IPM.Color,IPM.Shape,Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Size From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " Order By Issue_Detail_Id asc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch else SizeFt ENd End Description,Length,Width,
                        Width + 'x' + Length Size,Qty*Area as Area,Rate,Qty,Amount,Isnull(CancelQty,0) CancelQty,PD.OrderId,Itemremark From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " Order By Issue_Detail_Id desc";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            //LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
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
        if (DDDescription.Items.Count > 0)
        {
            DDDescription.SelectedIndex = 0;
        }
        TxtPreQuantity.Text = "";
        TxtTotalQty.Text = "";
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        TxtRate.Text = "";
        TxtQtyRequired.Text = "";
        TxtCommission.Text = "";
        TxtKhap.Text = "";
        TxtConsump.Text = "";
        txtAfterKhapSizeOrder.Text = "";
    }

    private void FillDetailBack()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"SELECT distinct IM.CATEGORY_ID, IM.ITEM_ID, PID.Item_Finished_Id,PID.Length,PID.Width, PID.Area,PID.Rate, PID.Qty,replace(convert(varchar(11),PID.ReqByDate,106), ' ','-') as ReqByDate,PID.Issue_Detail_Id,JA.PreProdAssignedQty TQty,PID.PQty,Comm,Isnull(CancelQty,0) CancelQty,PID.Orderid,
                        om.CustomerId,pid.IssueOrderId, PID.Khap,PID.Consump,PIM.UnitId,PID.AfterKhapSizeOrder
                          FROM PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID INNER JOIN PROCESS_ISSUE_MASTER_1 PIM ON PIM.IssueOrderId=PID.IssueOrderId INNER JOIN ITEM_PARAMETER_MASTER IPM ON PID.Item_Finished_Id = IPM.ITEM_FINISHED_ID INNER JOIN ITEM_MASTER IM ON IPM.ITEM_ID = IM.ITEM_ID                           
                          inner Join JobAssigns JA ON JA.OrderId=PID.OrderId And PID.Item_Finished_Id=JA.ITEM_FINISHED_ID 
                          inner join ordermaster om on pid.orderid=om.orderid Where Issue_Detail_Id=" + DGOrderdetail.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //DDPOrderNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderid"].ToString();
                //POrderNoSelectedChange();

                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["Customerid"].ToString();
                CustomerCodeSelectedChange();
                DDCustomerOrderNumber.SelectedValue = ds.Tables[0].Rows[0]["Orderid"].ToString();
                CustomerOrderNumberSelectedChange();

                DDunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();

                //UtilityModule.ConditionalComboFill(ref DDCategoryName, "SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId where OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "--Select--");
                DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                //UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");


                //DDEmployeeName.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                //EmployeeSelectedChange();


                //                if (Session["varMasterCompanyIDForERP"].ToString() == "6")
                //                {
                //                    string STR1 = "";
                //                    if (variable.VarNewQualitySize == "1")
                //                    {
                //                        STR1 = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF Where JA.OrderId=OD.OrderId AND 
                //                    OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + @" AND 
                //                    OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                //                    PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id and pd.issueorderid=" + DDPOrderNo.SelectedValue + @"
                //                    Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty )";
                //                    }
                //                    else
                //                    {
                //                        STR1 = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF Where JA.OrderId=OD.OrderId AND 
                //                    OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + @" AND 
                //                    OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                //                    PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id and pd.issueorderid=" + DDPOrderNo.SelectedValue + @"
                //                    Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty )";
                //                    }

                //                    UtilityModule.ConditionalComboFill(ref DDItemName, STR1, true, "---Select---");
                //                }
                //                else
                //                {
                //                    UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");
                //                }
                //BindCategory();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                //string STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When Unitid=" + DDunit.SelectedValue + "  Then SizeMtr Else SizeFt End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue;
                //UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
                dditemname_chage();
                DDDescription.SelectedValue = ds.Tables[0].Rows[0]["Item_Finished_Id"].ToString();
                fill_QuantityRequired();
                //TxtTotalQty.Text = ds.Tables[0].Rows[0]["TQty"].ToString();
                TxtLength.Text = ds.Tables[0].Rows[0]["Length"].ToString();
                TxtQtyRequired.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                TxtCancelQty.Text = ds.Tables[0].Rows[0]["CancelQty"].ToString();
                //TxtPreQuantity.Text = ds.Tables[0].Rows[0]["TQty"].ToString();
                TxtReceived.Text = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "select  isnull(sum(Qty)-sum(PQty+CancelQty),0) from Process_Issue_Detail_" + DDProcessName.SelectedValue + " where Item_Finished_id=" + DDDescription.SelectedValue + " and IssueOrderId=" + DDPOrderNo.SelectedValue).ToString()).ToString();
                TxtWidth.Text = ds.Tables[0].Rows[0]["Width"].ToString();
                TxtArea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                TxtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                TxtCommission.Text = ds.Tables[0].Rows[0]["Comm"].ToString();
                TxtRequiredDate.Text = ds.Tables[0].Rows[0]["ReqByDate"].ToString();
                TxtRate.Text = ds.Tables[0].Rows[0]["RATE"].ToString();

                TxtKhap.Text = ds.Tables[0].Rows[0]["Khap"].ToString();
                TxtConsump.Text = ds.Tables[0].Rows[0]["Consump"].ToString();
                txtAfterKhapSizeOrder.Text = ds.Tables[0].Rows[0]["AfterKhapSizeOrder"].ToString();

                LblErrorMessage.Visible = false;
                hnOrderId = Convert.ToInt16(ds.Tables[0].Rows[0]["OrderId"]);
                hnOldQty = Convert.ToInt16(ds.Tables[0].Rows[0]["Qty"]);

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            LblErrorMessage.Text = ex.Message;
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
        //int rowindex = DGOrderdetail.SelectedRow.RowIndex;
        // TextBox txtItemrem = ((TextBox)DGOrderdetail.Rows[rowindex].FindControl("txtItemRemark"));
        BtnUpdate.Visible = true;
        BtnSave.Visible = false;
        FillDetailBack();
    }
    decimal TotalQty = 0;
    decimal TotalArea = 0;
    decimal TotalWAmount = 0;
    decimal TotalComm = 0;
    decimal GTotal = 0;
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            TotalQty += Convert.ToDecimal(lblQty.Text);
            Label lblTArea = (Label)e.Row.FindControl("lblTArea");
            TotalArea += Convert.ToDecimal(lblTArea.Text);
            Label lblWAmount = (Label)e.Row.FindControl("lblWAmount");
            TotalWAmount += Convert.ToDecimal(lblWAmount.Text);
            Label lblCommAmt = (Label)e.Row.FindControl("lblCommAmt");
            TotalComm += Convert.ToDecimal(lblCommAmt.Text);
            Label lblGTotal = (Label)e.Row.FindControl("lblGTotal");
            GTotal += Convert.ToDecimal(lblGTotal.Text);

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
            ((TextBox)e.Row.FindControl("txtItemRemark")).Attributes.Add("onfocus", "onTextFocus();");
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblGrandTQty = (Label)e.Row.FindControl("lblGrandTQty");
            lblGrandTQty.Text = TotalQty.ToString();
            Label lblGrandTArea = (Label)e.Row.FindControl("lblGrandTArea");
            lblGrandTArea.Text = TotalArea.ToString();
            Label lblGrandWAmount = (Label)e.Row.FindControl("lblGrandWAmount");
            lblGrandWAmount.Text = TotalWAmount.ToString();
            Label lblGrandCommAmt = (Label)e.Row.FindControl("lblGrandCommAmt");
            lblGrandCommAmt.Text = TotalComm.ToString();
            Label lblGrandGTotal = (Label)e.Row.FindControl("lblGrandGTotal");
            lblGrandGTotal.Text = GTotal.ToString();

        }
    }
    protected void UpdateDetails()
    {
        LblErrorMessage.Text = "";
        Update_Process_Issue_Detail();
        Fill_Grid();
        BtnSave.Visible = true;
        BtnUpdate.Visible = false;
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "9":
            case "20":
                btnclickflag = "";
                btnclickflag = "BtnUpdate";
                txtpwd.Focus();
                Popup(true);
                break;
            default:
                UpdateDetails();
                break;
        }


        //if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        //{
        //    txtpwd.Focus();
        //    Popup(true);
        //}
        //else
        //{
        //    UpdateDetails();
        //}
        #region
        //LblErrorMessage.Text = "";
        //Update_Process_Issue_Detail();
        //Fill_Grid();
        //BtnSave.Visible = true;
        //BtnUpdate.Visible = false;
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        con.Open();
        //        try
        //        {
        //            #region Author: Rajeev...
        //            string str = @"Delete TEMP_PROCESS_ISSUE_MASTER
        //                           Delete TEMP_PROCESS_ISSUE_DETAIL
        //                           Insert into TEMP_PROCESS_ISSUE_MASTER Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + Session["processId"] + " from PROCESS_ISSUE_MASTER_" + Session["processId"] + " Where IssueOrderId=" + ViewState["IssueOrderid"] + @"
        //                           Insert into TEMP_PROCESS_ISSUE_DETAIL Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + Session["processId"] + " Where IssueOrderId=" + ViewState["IssueOrderid"];
        //            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);

        //            #endregion
        //            Session["ReportPath"] = "Reports/ProductionOrder.rpt";
        //            Session["CommanFormula"] = "{View_Production_Issue_Order.IssueOrderid}=" + ViewState["IssueOrderid"] + "";
        //        }
        //        catch (Exception ex)
        //        {
        //            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //Popup(true);
        #endregion

    }
    private void Update_Process_Issue_Detail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Check_Length_Width_Format();
            LblErrorMessage.Visible = true;
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrpara = new SqlParameter[23];

                _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Float);
                _arrpara[2] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@Length", SqlDbType.Decimal);
                _arrpara[4] = new SqlParameter("@Width", SqlDbType.Decimal);
                _arrpara[5] = new SqlParameter("@Area", SqlDbType.Decimal);
                _arrpara[6] = new SqlParameter("@Rate", SqlDbType.Float);
                _arrpara[7] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrpara[8] = new SqlParameter("@Qty", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@ReqByDate", SqlDbType.DateTime);
                _arrpara[10] = new SqlParameter("@PQty", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@Comm", SqlDbType.Float);
                _arrpara[12] = new SqlParameter("@CommAmount", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@CancelQty", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@Processid", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@remarks", SqlDbType.NVarChar, 500);
                _arrpara[16] = new SqlParameter("@instruction", SqlDbType.NVarChar, 4000);
                _arrpara[17] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrpara[18] = new SqlParameter("@oldQty", SqlDbType.Int);
                _arrpara[19] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                _arrpara[20] = new SqlParameter("@HSCode", SqlDbType.VarChar, 100);
                _arrpara[21] = new SqlParameter("@unitid", SqlDbType.Int);
                _arrpara[22] = new SqlParameter("@orderid", DDCustomerOrderNumber.SelectedValue);
                //_arrpara[22] = new SqlParameter("@AfterKhapSizeOrder", SqlDbType.VarChar, 50);


                _arrpara[0].Value = (ViewState["IssueOrderid"]);
                _arrpara[1].Value = DGOrderdetail.SelectedValue;
                _arrpara[2].Value = DDDescription.SelectedValue;
                if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                {
                    _arrpara[3].Value = Convert.ToDouble(TxtLength.Text);
                    _arrpara[4].Value = Convert.ToDouble(TxtWidth.Text);
                }
                else
                {
                    _arrpara[3].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                    _arrpara[4].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                }
                _arrpara[5].Value = TxtArea.Text;
                _arrpara[6].Value = TxtRate.Text;

                Double TQty;
                TQty = Convert.ToDouble(TxtQtyRequired.Text) - Convert.ToDouble(TxtCancelQty.Text == "" ? "0" : TxtCancelQty.Text);

                if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                {
                    _arrpara[7].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * TQty));
                    _arrpara[12].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text) * TQty));
                }
                if (DDcaltype.SelectedValue == "1")
                {
                    _arrpara[7].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * TQty));
                    _arrpara[12].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text) * TQty));
                }
                _arrpara[8].Value = TxtQtyRequired.Text;
                _arrpara[9].Value = TxtRequiredDate.Text;
                _arrpara[10].Value = Convert.ToInt32(TxtQtyRequired.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Qty-PQty from Process_Issue_Detail_" + DDProcessName.SelectedValue + " where Issue_Detail_Id=" + DGOrderdetail.SelectedValue).ToString());
                _arrpara[11].Value = TxtCommission.Text;
                _arrpara[13].Value = TxtCancelQty.Text;
                _arrpara[14].Value = DDProcessName.SelectedValue;
                _arrpara[15].Value = TxtRemarks.Text;
                _arrpara[16].Value = TxtInstructions.Text;
                _arrpara[17].Value = Session["varMasterCompanyIDForERP"];
                _arrpara[18].Value = hnOldQty;
                _arrpara[19].Direction = ParameterDirection.Output;
                _arrpara[20].Value = TxtHSCode.Text;
                _arrpara[21].Value = DDunit.SelectedValue;
                // _arrpara[22].Value = txtAfterKhapSizeOrder.Text;
                ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateProductionorder", _arrpara);

                //string str1 = @"Update Process_Issue_Detail_" + DDProcessName.SelectedValue + " set Item_Finished_id=" + _arrpara[2].Value + ",Length=" + _arrpara[3].Value + ",Width=" + _arrpara[4].Value + ",Area=" + _arrpara[5].Value + ",Rate=" + _arrpara[6].Value + ",Amount=" + _arrpara[7].Value + ",Qty=" + _arrpara[8].Value + ",ReqByDate='" + _arrpara[9].Value + "',PQty=" + _arrpara[10].Value + ",Comm=" + _arrpara[11].Value + ",CommAmt=" + _arrpara[12].Value + ",CancelQty=" + _arrpara[13].Value + " where Issue_Detail_Id=" + DGOrderdetail.SelectedValue + " ";
                ////SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                //str1 = str1 + @" Update Process_Issue_Master_" + DDProcessName.SelectedValue + " Set Remarks=N'" + TxtRemarks.Text + "',Instruction=N'" + TxtInstructions.Text + "' Where ISSUEORDERID=" + ViewState["IssueOrderid"];
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

                if (_arrpara[19].Value.ToString() == "")
                {
                    if (CKHCURRENTCONSUMPTION.Checked == true)
                    {
                        UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[1].Value), Convert.ToInt32(_arrpara[2].Value), Convert.ToInt32(DDProcessName.SelectedValue), hnOrderId, Tran, effectivedate: TxtAssignDate.Text);
                    }
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Data Successfully Update.......";
                    Tran.Commit();
                    BtnSave.Visible = true;
                    BtnUpdate.Visible = false;
                    Save_Refresh();
                }
                else
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[19].Value.ToString();
                    Tran.Commit();
                }
                //                str1 = @"Update Process_Issue_Master_" + DDProcessName.SelectedValue + " set Status='Pending' from Process_issue_master_" + DDProcessName.SelectedValue + " PM,Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PD
                //                           where PM.IssueOrderId=PD.IssueOrderId And Pqty<>0  And PM.IssueOrderid=" + _arrpara[0].Value + "";
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

                //SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_ProcessIssue", _arrpara);

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
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
    protected void Fill_Remark_Commission_Instruction()
    {
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@orderid", DDCustomerOrderNumber.SelectedValue);
        param[1] = new SqlParameter("@itemfinishedid", DDDescription.SelectedValue);
        param[2] = new SqlParameter("@remark", SqlDbType.NVarChar, 1000);
        param[3] = new SqlParameter("@instruction", SqlDbType.NVarChar, 8000);
        param[4] = new SqlParameter("@Commission", SqlDbType.Float);
        param[5] = new SqlParameter("@effectivedate", TxtAssignDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtAssignDate.Text);
        param[6] = new SqlParameter("@TypeId", 1);
        param[7] = new SqlParameter("@HSCode", SqlDbType.VarChar, 100);
        //output parameter
        param[2].Direction = ParameterDirection.Output;
        param[3].Direction = ParameterDirection.Output;
        param[4].Direction = ParameterDirection.Output;
        param[7].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_FillRemark_Commission_Instruction", param);

        TxtRemarks.Text = param[2].Value.ToString();
        TxtInstructions.Text = param[3].Value.ToString();
        TxtCommission.Text = param[4].Value.ToString();

        TxtHSCode.Text = param[7].Value.ToString();
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        TxtTotalQty.Text = "";
        TxtPreQuantity.Text = "";
        fill_QuantityRequired();
        Area();
        MASTER_RATE();
        //TxtCommission.Text = UtilityModule.Fill_Comm(Convert.ToInt32(DDDescription.SelectedValue)).ToString();
        Fill_Remark_Commission_Instruction();
        MASTER_CONSUMPTION();
    }
    private void MASTER_RATE()
    {
        //TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue)).ToString();
        TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1).ToString();
    }
    private void MASTER_CONSUMPTION()
    {
        TxtConsump.Text = UtilityModule.PROCESS_CONSUMPTION(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDunit.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1).ToString();

    }
    protected void TxtQtyRequired_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        TxtCancelQty.Text = "0";
        switch (Session["varcompanyNo"].ToString())
        {
            case "5":
            case "16":
                break;
            default:
                if (TxtQtyRequired.Text != "")
                {
                    if (BtnUpdate.Visible == true && Convert.ToInt32(TxtQtyRequired.Text) > 0 && Convert.ToInt32(TxtQtyRequired.Text) <= Convert.ToInt32(TxtPreQuantity.Text))
                    {
                        if (Convert.ToInt32(TxtQtyRequired.Text) >= Convert.ToInt32(TxtReceived.Text))
                        {
                            LblErrorMessage.Visible = false;
                        }
                        else
                        {
                            LblErrorMessage.Visible = true;
                            LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater than equals to  " + TxtReceived.Text + "  and less than PQty.........................";
                        }
                    }
                    else if (Convert.ToInt32(TxtQtyRequired.Text) > 0 && Convert.ToInt32(TxtQtyRequired.Text) <= Convert.ToInt32(TxtPreQuantity.Text))
                    {
                        LblErrorMessage.Visible = false;
                    }

                    else
                    {
                        TxtQtyRequired.Text = "";
                        TxtQtyRequired.Focus();
                        LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater than Zero and less than PQty.........................";
                        LblErrorMessage.Visible = true;
                    }

                }
                else
                {
                    LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater than Zero.........................";
                    LblErrorMessage.Visible = true;

                }
                break;
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

    protected void TxtRate_TextChanged(object sender, EventArgs e)
    {

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
            int Shape = 0;
            if (DDDescription.SelectedIndex > 0)
            {
                if (variable.VarNewQualitySize == "1")
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetailNew Where Item_Finished_Id=" + DDDescription.SelectedValue));
                }
                else
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + DDDescription.SelectedValue));
                }
            }
            Area();
            //if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            //{
            //    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            //}

            //if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            //{
            //        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
            //}

        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

        // ProcessReportPath();
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 4)
        {
            ProcessReportPath();
            Session["ReportPath"] = "Reports/ProductionOrderForDeepak.rpt";
            Session["CommanFormula"] = "{View_Production_Issue_Order.IssueOrderid}=" + ViewState["IssueOrderid"];
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            Report();
        }
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        string qry = "";
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);


            array[0].Value = ViewState["IssueOrderid"];
            array[1].Value = DDProcessName.SelectedValue;
            array[2].Value = Session["varMasterCompanyIDForERP"];

            //if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            //{
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrderReport", array);

            //        qry = @"SELECT VPI.Item_Name,VPI.Description,VPI.AssignDate,VPI.Remarks,VPI.Instruction,VPI.IssueOrderid,Round(VPI.Area,4) Area,VPI.Qty,VPI.ReqByDate,CI.CompanyName, CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.TinNo,EI.EmpName,EI.Address,EI.PhoneNo,OM.CustomerOrderNo,OM.LocalOrder,Unit.UnitName,
            //                             PNM.ShortName,VPI.Rate,VPI.Amount,VPI.UnitId,OM.CUSTOMERORDERNO,CIC.CUSTOMERCODE ,u.unitname,VPI.Comm,CancelQty
            //                             FROM View_Production_Issue_Order VPI INNER JOIN CompanyInfo CI ON VPI.Companyid=CI.CompanyId INNER JOIN EmpInfo EI ON VPI.Empid=EI.EmpId INNER JOIN 
            //                             OrderMaster OM ON VPI.Orderid=OM.OrderId INNER JOIN Unit ON VPI.UnitId=Unit.UnitId INNER JOIN PROCESS_NAME_MASTER PNM ON VPI.PROCESSID=PNM.PROCESS_NAME_ID INNER JOIN
            //                             CUSTOMERINFO CIC ON CIC.CUSTOMERID=OM.CUSTOMERID inner join unit u on vpi.unitid=u.unitid Where VPI.IssueOrderid=" + ViewState["IssueOrderid"] + "";
            //        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            //        str = @"SELECT VPIC.QTY,IM.ITEM_ID,IM.ITEM_NAME,VF.Finishedid,VF.Quality,vf.Design,vf.Color,vf.Shape,VF.ShadeColor,VPIC.Issueorderid,vpic.unitname FROM VIEW_PROCESS_ISSUE_CONSUMPTION VPIC INNER JOIN 
            //                               ViewFindFinishedId2 VF ON VPIC.FINISHEDID=VF.Finishedid INNER JOIN ITEM_MASTER IM ON VF.ITEM_ID=IM.ITEM_ID ORDER BY IM.ITEM_ID,VF.Quality,VF.Finishedid";
            //        SqlDataAdapter sda = new SqlDataAdapter(str, con);
            //        DataTable dt = new DataTable();
            //        sda.Fill(dt);
            //        ds.Tables.Add(dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 5)
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderPoshNew.rpt";
                }
                else if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 9)//For Hafizia
                {
                    if (chkforsummary.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForHafiziaSummary.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForHafizia.rpt";
                    }
                }
                else
                {
                    if (variable.VarNewConsumptionWise == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForMaltiRug.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNew.rpt";
                    }
                }
                // Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\ProductionOrderNew.xsd";
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
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            DDPOrderNo.Visible = true;
        }
        else
        {
            DDPOrderNo.Visible = false;
        }
    }
    protected void DDPOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        POrderNoSelectedChange();
    }
    private void POrderNoSelectedChange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string sqlstr = "Select replace(convert(varchar(11),AssignDate,106), ' ','-') as AssignDate,UnitId,Remarks,Instruction,CalType,Item_Finished_Id,Rate,Qty, replace(convert(varchar(11),ReqByDate,106), ' ','-') as ReqByDate,FlagFixOrWeight,HSCode From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where PM.IssueOrderId=PD.IssueOrderId And PM.IssueOrderId=" + DDPOrderNo.SelectedValue + "";
            con.Open();
            DataSet DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count > 0)
            {
                TxtAssignDate.Text = DS.Tables[0].Rows[0]["AssignDate"].ToString();
                DDunit.SelectedValue = DS.Tables[0].Rows[0]["UnitId"].ToString();
                DDcaltype.SelectedValue = DS.Tables[0].Rows[0]["CalType"].ToString();
                TxtInstructions.Text = DS.Tables[0].Rows[0]["Instruction"].ToString();
                TxtRemarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                TxtHSCode.Text = DS.Tables[0].Rows[0]["HSCode"].ToString();
                ViewState["IssueOrderid"] = DDPOrderNo.SelectedValue;
                hnIssueOrderId.Value = DDPOrderNo.SelectedValue;
                if (DS.Tables[0].Rows[0]["FlagFixOrWeight"] == "0")
                {
                    ChkForFix.Checked = true;
                }
                Fill_Grid();

            }
            else
            {
                ViewState["IssueOrderid"] = "0";
                DGOrderdetail.DataSource = null;
                DGOrderdetail.DataBind();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
        }
    }
    private void ProcessReportPath()
    {
        string str = @"Delete TEMP_PROCESS_ISSUE_MASTER
                       Delete TEMP_PROCESS_ISSUE_DETAIL ";
        str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + DDProcessName.SelectedValue + " from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + "";
        str = str + "  Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight) Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + "";
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    }
    protected void DDunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DDItemName.SelectedIndex = '0';
        if (DDDescription.SelectedIndex > 0)
        {
            DDDescription_SelectedIndexChanged(sender, e);
        }
    }
    protected void DeleteRow(int VarProcess_Issue_Detail_Id)
    {

        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            ViewState["IssueOrderid"] = DDPOrderNo.SelectedValue;
            if (0 != Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Qty-PQty from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "")))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "You Have Received....";
            }
            else
            {
                string sqlstr = "Select * from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where IssueOrderID=" + ViewState["IssueOrderid"] + " ";

                DataSet DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, sqlstr);
                if (DS.Tables[0].Rows.Count == 1)
                {
                    //if (DGOrderdetail.Rows.Count == 1)
                    //{
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Issue_Master_" + DDProcessName.SelectedValue + @" 
                Where IssueOrderID in (Select IssueOrderID from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")");
                }
                #region Author: Rajeev, Date:3-dec-12....
                string str = "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "";
                str = str + "  Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "");
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + Session["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "");
                #endregion
                Tran.Commit();
                if (DGOrderdetail.Rows.Count == 1)
                {
                    Fill_Temp_OrderNo();
                }
                Fill_Grid();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Successfully Deleted...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
        btnclickflag = "";

        btnclickflag = "BtnDeleteRow";
        txtpwd.Focus();
        Popup(true);

    }
    //protected void DGOrderdetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void TxtOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (TxtOrderNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
            @"Select * from TEMP_PROCESS_ISSUE_MASTER_NEW Where Companyid = " + DDCompanyName.SelectedValue + " And IssueOrderid='" + TxtOrderNo.Text + "'");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ProcessSelectedChange();
                DDEmployeeName.SelectedValue = Ds.Tables[0].Rows[0]["Empid"].ToString();
                EmployeeSelectedChange();
                DDPOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderid"].ToString();
                POrderNoSelectedChange();
            }
            else
            {
                TxtOrderNo.Text = "";
                TxtOrderNo.Focus();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Order No";
                DGOrderdetail.DataSource = null;
                DGOrderdetail.DataBind();
                TxtRemarks.Text = "";
                TxtHSCode.Text = "";
                TxtInstructions.Text = "";
            }
        }
    }
    protected void TxtCancelQty_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (TxtCancelQty.Text != "")
        {
            if (Convert.ToInt16(TxtCancelQty.Text) > 0 && Convert.ToInt16(TxtCancelQty.Text) > Convert.ToInt16(TxtQtyRequired.Text))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Cancel Qty Must be Less than or equal to Issue Qty....";
                TxtCancelQty.Text = "0";
            }
        }
        else
        {
            LblErrorMessage.Text = "Cancel Qty Must Be Integer and Greater Then Zero.........................";
            LblErrorMessage.Visible = true;

        }
    }

    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;
        Fill_Grid();
    }

    protected void DGOrderdetail_RowUpdating1(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string str;
            string itemreamrk = ((TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtItemRemark")).Text;
            str = "Update Process_Issue_Detail_" + DDProcessName.SelectedValue + " set ItemRemark=N'" + itemreamrk + "' where Issue_Detail_Id=" + DGOrderdetail.DataKeys[e.RowIndex].Value + "";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DGOrderdetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;

        }
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            // UpdateDetails();
            //Popup(false);

            if (btnclickflag == "BtnUpdate")
            {
                UpdateDetails();
            }
            else if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(VarProcess_Issue_Detail_Id);
            }
            else if (btnclickflag == "BtnUpdateWeaverName")
            {
                UpdateWeaverName();
            }
            else if (btnclickflag == "BtnUpdateConsumption")
            {
                UpdateConsumption();
            }
            else if (btnclickflag == "BtnUpdateRate")
            {
                UpdateRate();
            }
            else if (btnclickflag == "BtnCancelOrder")
            {
                CancelWeaverOrder();
            }
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    private void UpdateConsumption()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("PROCESS_ISSUE_ID", typeof(int));
        dtrecords.Columns.Add("PROCESS_ISSUE_DETAIL_ID", typeof(int));
        dtrecords.Columns.Add("ITEM_FINISHED_ID", typeof(int));
        dtrecords.Columns.Add("ORDER_ID", typeof(int));
        dtrecords.Columns.Add("Process_ID", typeof(int));
        dtrecords.Columns.Add("effectivedate", typeof(DateTime));
        dtrecords.Columns.Add("mastercompanyid", typeof(int));

        for (int i = 0; i < DGOrderdetail.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)DGOrderdetail.Rows[i].FindControl("Chkboxitem"));
            //TextBox txtqty = ((TextBox)DGIndent.Rows[i].FindControl("txtQty"));
            //if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            if (chkoutItem.Checked == true)
            {
                DataRow dr = dtrecords.NewRow();
                //***********
                Label lblItemFinishedId = ((Label)DGOrderdetail.Rows[i].FindControl("lblItemFinishedId"));
                Label lblorderdid = ((Label)DGOrderdetail.Rows[i].FindControl("lblorderdid"));
                Label lblIssueDetailId = ((Label)DGOrderdetail.Rows[i].FindControl("lblIssueDetailId"));

                //**************
                dr["PROCESS_ISSUE_ID"] = (ViewState["IssueOrderid"]);
                dr["PROCESS_ISSUE_DETAIL_ID"] = Convert.ToInt32(lblIssueDetailId.Text);
                dr["ITEM_FINISHED_ID"] = Convert.ToInt32(lblItemFinishedId.Text);
                dr["ORDER_ID"] = Convert.ToInt32(lblorderdid.Text);
                dr["Process_ID"] = Convert.ToInt32(DDProcessName.SelectedValue);
                dr["effectivedate"] = TxtAssignDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtAssignDate.Text;
                dr["mastercompanyid"] = HttpContext.Current.Session["varMasterCompanyIDForERP"];

                dtrecords.Rows.Add(dr);
            }
        }
        //********************
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                param[0].Direction = ParameterDirection.Output;
                param[1] = new SqlParameter("@dtrecords", dtrecords);

                //**********
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew2", param);
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveProcessIssueConsump", param);
                Tran.Commit();
                //if (rowscount == -1)
                //{
                //    lblMessage.Visible = true;
                //    lblMessage.Text = "";
                //}

                LblErrorMessage.Text = param[0].Value.ToString();
                LblErrorMessage.Visible = true;
                Fill_Grid();

            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }

        //if (CKHCURRENTCONSUMPTION.Checked == true)
        //{
        //    UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[1].Value), Convert.ToInt32(_arrpara[2].Value), Convert.ToInt32(DDProcessName.SelectedValue), hnOrderId, Tran, effectivedate: TxtAssignDate.Text);
        //}
        //LblErrorMessage.Visible = true;
        //LblErrorMessage.Text = "Data Successfully Update.......";
    }
    protected void BtnConsump_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnUpdateConsumption";
        txtpwd.Focus();
        Popup(true);
    }
    private void UpdateRate()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("PROCESS_ISSUE_ID", typeof(int));
        dtrecords.Columns.Add("PROCESS_ISSUE_DETAIL_ID", typeof(int));
        dtrecords.Columns.Add("ITEM_FINISHED_ID", typeof(int));
        dtrecords.Columns.Add("ORDER_ID", typeof(int));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("TypeId", typeof(int));
        dtrecords.Columns.Add("UnitId", typeof(int));
        dtrecords.Columns.Add("Process_ID", typeof(int));
        dtrecords.Columns.Add("effectivedate", typeof(DateTime));
        dtrecords.Columns.Add("mastercompanyid", typeof(int));

        for (int i = 0; i < DGOrderdetail.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)DGOrderdetail.Rows[i].FindControl("Chkboxitem"));
            //TextBox txtqty = ((TextBox)DGIndent.Rows[i].FindControl("txtQty"));
            //if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            if (chkoutItem.Checked == true)
            {
                DataRow dr = dtrecords.NewRow();
                //***********
                Label lblItemFinishedId = ((Label)DGOrderdetail.Rows[i].FindControl("lblItemFinishedId"));
                Label lblorderdid = ((Label)DGOrderdetail.Rows[i].FindControl("lblorderdid"));
                Label lblIssueDetailId = ((Label)DGOrderdetail.Rows[i].FindControl("lblIssueDetailId"));
                Label lblArea = ((Label)DGOrderdetail.Rows[i].FindControl("lblArea"));
                Label lblQty = ((Label)DGOrderdetail.Rows[i].FindControl("lblQty"));

                //**************
                dr["PROCESS_ISSUE_ID"] = (ViewState["IssueOrderid"]);
                dr["PROCESS_ISSUE_DETAIL_ID"] = Convert.ToInt32(lblIssueDetailId.Text);
                dr["ITEM_FINISHED_ID"] = Convert.ToInt32(lblItemFinishedId.Text);
                dr["ORDER_ID"] = Convert.ToInt32(lblorderdid.Text);
                dr["Area"] = lblArea.Text;
                dr["Qty"] = Convert.ToInt32(lblQty.Text);
                dr["TypeId"] = 1;
                dr["UnitId"] = Convert.ToInt32(DDunit.SelectedValue);
                dr["Process_ID"] = Convert.ToInt32(DDProcessName.SelectedValue);
                dr["effectivedate"] = TxtAssignDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtAssignDate.Text;
                dr["mastercompanyid"] = HttpContext.Current.Session["varMasterCompanyIDForERP"];

                dtrecords.Rows.Add(dr);
            }
        }
        //********************
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                param[0].Direction = ParameterDirection.Output;
                param[1] = new SqlParameter("@dtrecords", dtrecords);

                //**********               
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_UPDATE_CONSUMPTION_RATE_COMMISSION_KHAP_QUALITYDETAILWISE", param);
                Tran.Commit();
                //if (rowscount == -1)
                //{
                //    lblMessage.Visible = true;
                //    lblMessage.Text = "";
                //}

                LblErrorMessage.Text = param[0].Value.ToString();
                LblErrorMessage.Visible = true;
                Fill_Grid();

            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void BtnUpdateRate_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnUpdateRate";
        txtpwd.Focus();
        Popup(true);
    }
    private void CancelWeaverOrder()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (TxtOrderNo.Text != "")
            {
                if (LblErrorMessage.Text == "")
                {
                    SqlParameter[] _arrpara = new SqlParameter[3];

                    _arrpara[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@IssueOrderid", SqlDbType.Int);

                    _arrpara[0].Value = (DDProcessName.SelectedValue);
                    _arrpara[1].Value = (ViewState["IssueOrderid"]);
                    _arrpara[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
                    _arrpara[2].Direction = ParameterDirection.Output;

                    //**********

                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_CancelWeaverOrder", _arrpara);

                    Tran.Commit();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[2].Value.ToString();
                    //LblErrorMessage.Text = "Data Successfully Saved.......";
                    if (LblErrorMessage.Text == "Data updated successfully...")
                    {
                        DGOrderdetail.DataSource = null;
                        DGOrderdetail.DataBind();
                    }
                }
                else
                {
                    Tran.Commit();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill order number.');", true);
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssueNew.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void BtnCancelOrder_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnCancelOrder";
        txtpwd.Focus();
        Popup(true);

    }


}