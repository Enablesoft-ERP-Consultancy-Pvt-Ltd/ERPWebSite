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
using ClosedXML.Excel;

public partial class Masters_ProcessIssue_ProcessIssue : System.Web.UI.Page
{
    private DataSet DS;
    int PROCESS_ISSUE_ID = 0;
    static int hnOrderId = 0;
    static int hnOldQty = 0;
    static string btnclickflag = "";
    static int VarProcess_Issue_Detail_Id = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {


            DDCompanyName.Focus();
            ChkEditOrder.Checked = true;
            DDPOrderNo.Visible = true;
            
            DataSet ds = new DataSet();
            string str = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
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
            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                Tdlabelcustcode.Visible = false;
                Tddropdowncustcode.Visible = false;
                DDCompanyName_SelectedIndexChanged(sender, new EventArgs());
            }

            if (Session["VarCompanyId"].ToString() == "37")
            {
                ChkForExport.Visible = true;
            }
            else
            {
                ChkForExport.Visible = false;
            }


            if (Session["varcompanyId"].ToString() == "27")
            {
                DDunit.SelectedValue = "1";
                BtnPreviewConsumption.Visible = true;
            }
            else
            {
                DDunit.SelectedIndex = 2;
                BtnPreviewConsumption.Visible = false;
            }
            ViewState["IssueOrderid"] = PROCESS_ISSUE_ID;
            ParameteLabel();
            switch (Session["varcompanyId"].ToString())
            {
                case "2":
                    TDItemCode1.Visible = false;
                    TDItemCode2.Visible = false;
                    break;
                case "9":
                    btnLoomDetail.Visible = true;
                    chkforsummary.Visible = true;
                    BtnSave.Visible = false;
                    tdtxtQualityGrmPerMeterMinus.Visible = true;
                    tdtxtQualityGrmPerMeterPlus.Visible = true;
                    chkforProductionOrder.Visible = true;
                    chkforWeaverRawMaterial.Visible = true;
                    lblPONo.Text = "Folio No";
                    //lblComm.Text = "Value";
                    break;
                case "30":
                case "32":
                case "31":
                case "4":
                    BtnUpdateEmpName.Visible = true;
                    break;
               
            }
            hncomp.Value = Session["varCompanyId"].ToString();
            Fill_Temp_OrderNo();
            DDCustomerCode.Focus();


        }
    }
    private void Fill_Temp_OrderNo()
    {
        string strinsert = "", str = "";
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_ISSUE_MASTER_NEW");
        if (Session["varcompanyid"].ToString() == "9")
        {
            str = "Select Process_name_id from PROCESS_NAME_MASTER Where Process_name_id in(1,16,35)";
        }
        else
        {
            str = "Select Process_name_id from PROCESS_NAME_MASTER Where Process_name_id=1";
        }

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                str = "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW select Pm.Companyid,Om.CustomerId,Pd.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + ",PM.Empid,Pm.issueorderid,PM.ChallanNO from PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM inner join PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD on PM.IssueOrderId=PD.IssueOrderId  inner join OrderMaster OM on Om.OrderId=Pd.Orderid Where PM.Status<>'canceled'";
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
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Tddropdowncustcode.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT distinct CI.CustomerId,CI.CustomerCode From CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid order by CI.CustomerCode", true, "--Select--");
        }
        else
        {
            CustomerCodeSelectedChange();

        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    private void CustomerCodeSelectedChange()
    {
        //SqlConnection con = new SqlConnection();
        string str = @"Select  Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.CompanyId=" + DDCompanyName.SelectedValue + " and Om.ORDERFROMSAMPLE=0 ";
        if (Tddropdowncustcode.Visible == true)
        {
            str = str + "  and OM.Customerid=" + DDCustomerCode.SelectedValue;
        }
        str = str + " order by OM.Orderid ";
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, str, true, "--Select--");
    }
    //private void CheckDuplicateCustomerId()
    //{
    //    LblErrorMessage.Text = "";
    //    int checkcustomerid = 0;
    //    string str2 = "";
    //    str2 = "select CustomerId from OrderMaster Where orderid=" + DDCustomerOrderNumber.SelectedValue;
    //    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
    //    if (ds2.Tables[0].Rows.Count > 0)
    //    {
    //        checkcustomerid = Convert.ToInt32(ds2.Tables[0].Rows[0]["CustomerId"].ToString());
    //        if (hnCustomerId.Value == "0")
    //        {
    //            hnCustomerId.Value = ds2.Tables[0].Rows[0]["CustomerId"].ToString();
    //        }

    //        if (Convert.ToInt32(hnCustomerId.Value) != checkcustomerid)
    //        {
    //            DDCustomerOrderNumber.SelectedIndex = 0;
    //            LblErrorMessage.Text = "Please Select Same Customer OrderNo";
    //            LblErrorMessage.Visible = true;
    //            return;
    //        }
    //    }
    //}

    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "0")
        {
            CustomerOrderNumberSelectedChange();
        }
        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
        {
            //CheckDuplicateCustomerId();
            CategoryselectedindexChanged();
        }
    }
    private void CustomerOrderNumberSelectedChange()
    {
        string str;
        if (Session["varcompanyId"].ToString() == "9")
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER  where process_name_id in(1,16,35)
                    SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        }
        else
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where Process_Name_id=1
                       SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId inner Join JobAssigns JA ON JA.OrderId=OD.OrderId where  OD.Item_Finished_Id=JA.Item_Finished_Id  and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue;
        }
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Select--");
        if (Session["varcompanyId"].ToString() == "20")
        {
            DDProcessName.Enabled = false;
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                ProcessSelectedChange();
            }
        }
        UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 1, true, "--Select--");
        if (DDCategoryName.Items.Count > 0)
        {
            DDCategoryName.SelectedIndex = 1;
            CategoryselectedindexChanged();

        }

    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,PROCESS_ISSUE_Master_" + DDProcessName.SelectedValue + " PM Where PM.EmpId=Ei.EmpId and PM.Status<>'Canceled' Order By EI.EmpName", true, "--Select--");
        TxtAssignDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        TxtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
    }
    private void EmployeeSelectedChange()
    {
        string str = "";
        if (variable.VarLoomNoGenerated == "1")
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM 
                    inner join PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD on Pm.issueorderid=Pd.issueorderid
                    Left join LoomstockNo ls on pm.issueorderid=Ls.issueorderid And ls.ProcessID = " + DDProcessName.SelectedValue + @"
                    where ls.issueorderid is null and  PM.Empid=" + DDEmployeeName.SelectedValue + " and PM.CompanyId=" + DDCompanyName.SelectedValue + @" 
                    and PD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and PM.Status<>'canceled' order by PM.Issueorderid";
        }
        else
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD 
                where PM.Empid=" + DDEmployeeName.SelectedValue + " and PM.CompanyId=" + DDCompanyName.SelectedValue + @" and PM.IssueOrderId=PD.IssueOrderId 
                and PD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and PM.Status<>'canceled' order by PM.Issueorderid";
        }
        UtilityModule.ConditionalComboFill(ref DDPOrderNo, str, true, "--Select--");
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryselectedindexChanged();
    }
    protected void CategoryselectedindexChanged()
    {
        if (Session["varCompanyId"].ToString() == "6")
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
            string str = "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue+"";
            if (DDItemName.SelectedIndex > 0)
            {
                str = str + " and IM.Item_Id= " + DDItemName.SelectedValue + "";
            }

            UtilityModule.ConditionalComboFill(ref DDItemName, str, true, "---Select---");

            //UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");
            
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
        TxtTotalQty.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select  Sum(PreProdAssignedQty) as PreProdAssignedQty  from JobAssigns where OrderId=" + DDCustomerOrderNumber.SelectedValue + " and Item_Finished_ID=" + DDDescription.SelectedValue));
        String Str = "";
        if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
        {
            Str = "Select IsNull(Sum(PID.Qty),0)-isnull(Sum(PID.CancelQty),0) PQty from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where Pim.empid<>0 and PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PIM.Status<>'Canceled' and PID.Item_Finished_Id=" + DDDescription.SelectedValue;
        }
        else
        {
            Str = "Select IsNull(Sum(PID.Qty),0)-isnull(Sum(PID.CancelQty),0) PQty from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PIM.Status<>'Canceled' and PID.Item_Finished_Id=" + DDDescription.SelectedValue;
        }

        if (BtnUpdate.Visible == true)
        {
            Str = Str + " And PID.Issue_Detail_Id!=" + DGOrderdetail.SelectedValue;
        }
        if (Session["varcompanyid"].ToString() == "27")
        {
            TxtPreQuantity.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text == "" ? "0" : TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str)));
            TxtQtyRequired.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text == "" ? "0" : TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str)));
        }
        else
        {
            TxtPreQuantity.Text = Convert.ToString(Convert.ToInt32(TxtTotalQty.Text == "" ? "0" : TxtTotalQty.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str)));
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        dditemname_chage();
    }
    private void dditemname_chage()
    {
        string size = "";
        if (chkexportsize.Checked == true || (hnpurchasefolio.Value == "1" && variable.VarWEAVERPURCHASEORDER_FULLAREA == "1"))
        {
            switch (DDunit.SelectedValue)
            {
                case "6":
                    size = "Sizeinch";
                    break;
                case "1":
                    size = "Sizemtr";
                    break;
                default:
                    size = "Sizeft";
                    break;
            }
        }
        else if (variable.VarProductionSizeItemWise == "1")
        {
            switch (DDunit.SelectedValue.ToString())
            {
                case "6":
                    size = "Sizeinch";
                    break;
                case "1":
                    size = "ItemProdSizeMtr";
                    break;
                default:
                    size = "ItemProdSizeFt";
                    break;
            }

        }
        else
        {
            switch (DDunit.SelectedValue)
            {
                case "6":
                    size = "Sizeinch";
                    break;
                case "1":
                    size = "ProdSizeMtr";
                    break;
                default:
                    size = "ProdSizeFt";
                    break;
            }
        }
        string STR = "";
        if (variable.VarProductionSizeItemWise == "1")
        {

            STR = @"Select Distinct JA.Item_Finished_Id,IPM.QualityName+Space(2)+IPM.DesignName+Space(2)+IPM.ColorName+Space(2)+IPM.ShapeName+Space(2)+IPM.ShadeColorName+Space(5)+ " + size + @" Description 
                        from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id 
                        inner join Item_Master IM ON IM.Item_Id=OD.Item_Id 
                        inner join V_FinishedItemDetail IPM On IPM.ITEM_FINISHED_ID=JA.Item_Finished_Id 
                        JOIN SizeAttachedWithItem SA ON IPM.SizeId=SA.SizeId and SA.ItemId=IM.ITEM_ID
                        LEFT JOIN  View_Process_Issue_Detail VPD ON JA.Item_Finished_Id=VPD.Item_Finished_Id and VPD.PROCESS_NAME_ID=" + DDProcessName.SelectedValue + @" 
                        Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " ";

            UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
        }
        else
        {
            if (Session["varCompanyId"].ToString() == "6")
            {

                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + ",'') Else Isnull(" + size + @",'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  and pd.issueorderid=" + DDPOrderNo.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty)";

                UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
            }
            else
            {
                if (variable.VarNewQualitySize == "1")
                {
                    ////STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ " + size + " End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On IPM.FinishedId=JA.Item_Finished_Id left JOIN PROCESS_ISSUE_DETAIL_16 PD ON JA.Item_Finished_Id=PD.Item_Finished_Id Where   JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " and PD.Item_Finished_Id is  null";

                    STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ " + size + " End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On IPM.FinishedId=JA.Item_Finished_Id LEFT JOIN  View_Process_Issue_Detail VPD ON JA.Item_Finished_Id=VPD.Item_Finished_Id and VPD.PROCESS_NAME_ID=" + DDProcessName.SelectedValue + " Where   JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " and PD.Item_Finished_Id is  null";
                }
                else
                {
                    //////STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ " + size + " Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id left JOIN PROCESS_ISSUE_DETAIL_16 PD ON JA.Item_Finished_Id=PD.Item_Finished_Id Where   JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + "and PD.Item_Finished_Id is  null";
                    //STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ " + size + " Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id left JOIN PROCESS_ISSUE_DETAIL_16 PD ON JA.Item_Finished_Id=PD.Item_Finished_Id Where   JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " ";

                    STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ " + size + " Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id LEFT JOIN  View_Process_Issue_Detail VPD ON JA.Item_Finished_Id=VPD.Item_Finished_Id and VPD.PROCESS_NAME_ID=" + DDProcessName.SelectedValue + " Where   JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " ";
                }
                UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
            }
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
                if (SizeId != 0 && Session["varCompanyId"].ToString() != "6")
                {
                    //LblArea.Visible = true;
                    TdArea.Visible = true;
                    SqlParameter[] _arrpara = new SqlParameter[10];
                    _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                    _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@ShapeId", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@ExportSizeflag", chkexportsize.Checked == true ? "1" : "0");
                    _arrpara[7] = new SqlParameter("@PuchaseFolio", SqlDbType.TinyInt);
                    _arrpara[8] = new SqlParameter("@ShapeName", SqlDbType.VarChar, 10);
                    _arrpara[9] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);


                    _arrpara[0].Value = SizeId;
                    _arrpara[1].Value = DDunit.SelectedValue;
                    _arrpara[2].Direction = ParameterDirection.Output;
                    _arrpara[3].Direction = ParameterDirection.Output;
                    _arrpara[4].Direction = ParameterDirection.Output;
                    _arrpara[5].Direction = ParameterDirection.Output;
                    _arrpara[7].Value = hnpurchasefolio.Value;
                    _arrpara[8].Direction = ParameterDirection.Output;
                    _arrpara[9].Value = DDDescription.SelectedValue;


                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Area", _arrpara);


                    switch (DDunit.SelectedValue)
                    {
                        case "2": //ft
                            TxtLength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                            TxtWidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                            break;
                        default:
                            TxtLength.Text = _arrpara[2].Value.ToString();
                            TxtWidth.Text = _arrpara[3].Value.ToString();
                            break;
                    }

                    if (Session["varCompanyNo"].ToString() == "42")
                    { 
                        TxtArea.Text = string.Format("{0:#0.00}", _arrpara[4].Value);
                        TxtArea.Text = UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(TxtArea.Text), variable.VarRoundFtFlag).ToString();
                    }
                    else
                    {
                        TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
                    }

                    if (variable.VarWEAVERPURCHASEORDER_FULLAREA == "1" && hnpurchasefolio.Value == "1")
                    {
                        if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                        {
                            UtilityModule.FtAreaCalculate_WeaverOrder(TxtLength, TxtWidth, TxtArea, 1, Convert.ToInt16(Session["varcompanyId"]));
                        }
                        if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                        {
                            UtilityModule.AreaMtSq_Weaverorder(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), TxtArea, _arrpara[8].Value.ToString(), Convert.ToInt16(Session["varcompanyId"]));
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                        {
                            TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"])));
                        }
                        if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                        {
                              TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"]), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
                                                       
                        }
                    }
                }
                else if (SizeId != 0 && Session["varCompanyId"].ToString() == "6")
                {
                    //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                    string str = "";
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = "select ExpWidthMS_Ft as WidthFt,ExpLengthMS_Ft as LengthFt, 0 as HeightFt,LEFT(MtrSize, CHARINDEX('x', MtrSize) - 1) AS WidthMtr,REPLACE(SUBSTRING(MtrSize, CHARINDEX('x', MtrSize), LEN(MtrSize)), 'x', '') as LengthMtr,0 as HeightMtr,Export_Area as AreaFt,MtrArea as AreaMtr from QualitySizeNew where sizeid=" + SizeId + "";
                    }
                    else
                    {
                        str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr from size where sizeid=" + SizeId + "";
                    }
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (DDunit.SelectedValue == "2")
                    {
                        TxtLength.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["LengthFt"].ToString());
                        TxtWidth.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["WidthFt"].ToString());
                        TxtArea.Text = string.Format("{0:#0.0000}", DS.Tables[0].Rows[0]["AreaFt"].ToString());
                    }
                    else
                    {
                        TxtLength.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["LengthMtr"].ToString());
                        TxtWidth.Text = string.Format("{0:#0.00}", DS.Tables[0].Rows[0]["Widthmtr"].ToString());
                        decimal area;
                        area = Convert.ToDecimal((Convert.ToDecimal(TxtLength.Text) * Convert.ToDecimal(TxtWidth.Text) * Convert.ToDecimal(10.764)) / 10000);
                        TxtArea.Text = string.Format("{0:#0.0000}", area);
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        ProcessIssue();
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
                if (LblErrorMessage.Text == "")
                {
                    SqlParameter[] _arrpara = new SqlParameter[28];

                    _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.DateTime);
                    _arrpara[3] = new SqlParameter("@Status", SqlDbType.VarChar,30);
                    _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
                    _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 8000);
                    _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar, 8000);
                    _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

                    _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
                    _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                    _arrpara[11] = new SqlParameter("@Length", SqlDbType.VarChar, 50);
                    _arrpara[12] = new SqlParameter("@Width", SqlDbType.VarChar, 50);
                    _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[14] = new SqlParameter("@Rate", SqlDbType.NVarChar);
                    _arrpara[15] = new SqlParameter("@Amount", SqlDbType.NVarChar);
                    _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Float);
                    _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.DateTime);
                    _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Float);

                    _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
                    _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.Float);
                    _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
                    _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);
                    _arrpara[23] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
                    _arrpara[24] = new SqlParameter("@CancelQty", SqlDbType.Int);
                    _arrpara[25] = new SqlParameter("@ExportSizeflag", chkexportsize.Checked == true ? "1" : "0");
                    _arrpara[26] = new SqlParameter("@QualityGrmPerMeterMinus", SqlDbType.Float);
                    _arrpara[27] = new SqlParameter("@QualityGrmPerMeterPlus", SqlDbType.Float);

                    int num;
                    if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || DDProcessName.SelectedValue != Convert.ToString(ViewState["ProcessId"]))
                    {
                        // ViewState["IssueOrderid"]
                        int a;
                        //***********************
                        switch (variable.VarMaintainProcessissueSeq)
                        {
                            case "0":  //Not Maintain Seq....
                                switch (Session["varcompanyId"].ToString())
                                {
                                    case "9":  //Hafizia
                                        switch (DDProcessName.SelectedValue)
                                        {
                                            case "1":
                                            case "16":
                                            case "35":
                                                a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from v_ProcessMaxId"));
                                                break;

                                            default:
                                                a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from process_issue_master_" + DDProcessName.SelectedValue + ""));
                                                break;
                                        }
                                        break;
                                    default:
                                        a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from process_issue_master_" + DDProcessName.SelectedValue + ""));
                                        break;
                                }
                                ViewState["IssueOrderid"] = a;
                                num = 1;
                                break;
                            default:
                                a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                                ViewState["IssueOrderid"] = a;
                                num = 1;
                                break;
                        }
                        //*********************
                        #region
                        //if (Session["varcompanyId"].ToString() == "9")
                        //{
                        //    switch (DDProcessName.SelectedValue)
                        //    {
                        //        case "1":
                        //        case "16":
                        //            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from v_ProcessMaxId"));
                        //            break;
                        //        default:
                        //            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from process_issue_master_" + DDProcessName.SelectedValue + ""));
                        //            break;
                        //    }
                        //    ViewState["IssueOrderid"] = a;
                        //    num = 1;
                        //}
                        //else
                        //{
                        //    a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                        //    ViewState["IssueOrderid"] = a;
                        //    num = 1;
                        //}
                        #endregion
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
                    _arrpara[11].Value = string.Format("{0:#0.00}", TxtLength.Text);
                    _arrpara[12].Value = string.Format("{0:#0.00}", TxtWidth.Text);
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

                    _arrpara[26].Value = TxtQualityGrmPerMeterMinus.Text == "" ? "0" : TxtQualityGrmPerMeterMinus.Text;
                    _arrpara[27].Value = TxtQualityGrmPerMeterPlus.Text == "" ? "0" : TxtQualityGrmPerMeterPlus.Text;
                    ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);

                    if (num == 1)
                    {
                        #region Author :Rajeev ...
                        //Select IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid from PROCESS_ISSUE_MASTER_1
                        //Select Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid From PROCESS_ISSUE_DETAIL_1
                        string str;
                        if (Session["varcompanyId"].ToString() == "9")
                        {
                            str = @"  Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ")";
                        }
                        else
                        {
                            str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value +
                           @"  Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ")";
                        }

                        //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                        //str = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,FlagFixOrWeight) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + ")";

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                        #endregion
                    }
                    string str1 = "";
                    str1 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,CancelQty,ExportSizeflag,QualityGrmPerMeterMinus,QualityGrmPerMeterPlus) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[24].Value + "," + _arrpara[25].Value + ",'" + _arrpara[26].Value + "','" + _arrpara[27].Value + "')";
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
    //**********************************************************************************************************
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (tdtxtQualityGrmPerMeterMinus.Visible == true)
        {
            if (UtilityModule.VALIDTEXTBOX(TxtQualityGrmPerMeterMinus) == false)
            {
                goto a;
            }
        }
        if (tdtxtQualityGrmPerMeterPlus.Visible == true)
        {
            if (UtilityModule.VALIDTEXTBOX(TxtQualityGrmPerMeterPlus) == false)
            {
                goto a;
            }
        }
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
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;

        string sqlstr = "";
        if (variable.VarNewQualitySize == "1")
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name Item,IPM.QDCS + Space(5) +PD.Width+' x '+PD.Length  Description,Length,Width,
                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount,Isnull(CancelQty,0) CancelQty,PD.OrderId,Itemremark,PD.Item_Finished_id From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " Order By Issue_Detail_Id Desc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name Item,IPM.QDCS + Space(5) +  PD.Width+' x '+PD.Length  Description,Length,Width,
                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount,Isnull(CancelQty,0) CancelQty,PD.OrderId,Itemremark,PD.Item_Finished_id From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " Order By Issue_Detail_Id Desc";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            //LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
        TxtQualityGrmPerMeterMinus.Text = "";
        TxtQualityGrmPerMeterPlus.Text = "";
    }

    private void FillDetailBack()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"SELECT IM.CATEGORY_ID, IM.ITEM_ID, PID.Item_Finished_Id,PID.Length,PID.Width, PID.Area,PID.Rate, PID.Qty,replace(convert(varchar(11),PID.ReqByDate,106), ' ','-') as ReqByDate,
                           PID.Issue_Detail_Id,JA.PreProdAssignedQty TQty,PID.PQty,Comm,Isnull(CancelQty,0) CancelQty,PID.Orderid,isnull(PID.ExportSizeflag,0) as ExportSizeflag,
                          ISNULL(PID.QualityGrmPerMeterMinus,0) as QualityGrmPerMeterMinus,ISNULL(PID.QualityGrmPerMeterPlus,0) as QualityGrmPerMeterPlus
                          FROM PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID INNER JOIN ITEM_PARAMETER_MASTER IPM ON PID.Item_Finished_Id = IPM.ITEM_FINISHED_ID INNER JOIN ITEM_MASTER IM ON IPM.ITEM_ID = IM.ITEM_ID 
                          inner Join JobAssigns JA ON JA.OrderId=PID.OrderId And PID.Item_Finished_Id=JA.ITEM_FINISHED_ID Where Issue_Detail_Id=" + DGOrderdetail.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDCategoryName, "SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId where OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "--Select--");
                DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                //UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");
                if (Session["varCompanyId"].ToString() == "6")
                {
                    string STR1;

                    STR1 = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF Where JA.OrderId=OD.OrderId AND 
                    OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + @" AND 
                    OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                    PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id and pd.issueorderid=" + DDPOrderNo.SelectedValue + @"
                    Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty )";

                    UtilityModule.ConditionalComboFill(ref DDItemName, STR1, true, "---Select---");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue, true, "---Select---");
                }
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                //string STR = "Select Distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When Unitid=" + DDunit.SelectedValue + "  Then SizeMtr Else SizeFt End Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join ViewFindFinishedidItemidQDCSS IPM On IPM.FinishedId=JA.Item_Finished_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue;
                //UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
                chkexportsize.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ExportSizeflag"]);
                dditemname_chage();

                if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
                {
                    DDCustomerOrderNumber.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
                                  
                    CustomerOrderNumberSelectedChange();
                   
                    if (DDProcessName.Items.Count > 0)
                    {
                        DDProcessName.SelectedIndex = 1;                        
                    }
                }

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
                LblErrorMessage.Visible = false;
                hnOrderId = Convert.ToInt16(ds.Tables[0].Rows[0]["OrderId"]);
                hnOldQty = Convert.ToInt16(ds.Tables[0].Rows[0]["Qty"]);
                TxtQualityGrmPerMeterMinus.Text = ds.Tables[0].Rows[0]["QualityGrmPerMeterMinus"].ToString();
                TxtQualityGrmPerMeterPlus.Text = ds.Tables[0].Rows[0]["QualityGrmPerMeterPlus"].ToString();

                

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
            ((TextBox)e.Row.FindControl("txtItemRemark")).Attributes.Add("onfocus", "onTextFocus();");
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
        if (Session["varcompanyid"].ToString() == "9")
        {
            btnclickflag = "";
            btnclickflag = "BtnUpdate";
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            UpdateDetails();

        }
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
                SqlParameter[] _arrpara = new SqlParameter[25];

                _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Float);
                _arrpara[2] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@Length", SqlDbType.VarChar, 50);
                _arrpara[4] = new SqlParameter("@Width", SqlDbType.VarChar, 50);
                _arrpara[5] = new SqlParameter("@Area", SqlDbType.Float);
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
                _arrpara[20] = new SqlParameter("@Unitid", DDunit.SelectedValue);
                _arrpara[21] = new SqlParameter("@ExportSizeflag", chkexportsize.Checked == true ? "1" : "0");
                _arrpara[22] = new SqlParameter("@Orderid", DDCustomerOrderNumber.SelectedValue);
                _arrpara[23] = new SqlParameter("@QualityGrmPerMeterMinus", SqlDbType.Float);
                _arrpara[24] = new SqlParameter("@QualityGrmPerMeterPlus", SqlDbType.Float);

                _arrpara[0].Value = (ViewState["IssueOrderid"]);
                _arrpara[1].Value = DGOrderdetail.SelectedValue;
                _arrpara[2].Value = DDDescription.SelectedValue;
                if (Session["varcompanyid"].ToString() == "9")
                {
                    _arrpara[3].Value = TxtLength.Text;
                    _arrpara[4].Value = TxtWidth.Text;
                }
                else
                {
                    _arrpara[3].Value = string.Format("{0:#0.00}", TxtLength.Text);
                    _arrpara[4].Value = string.Format("{0:#0.00}", TxtWidth.Text);
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
                _arrpara[17].Value = Session["varcompanyid"];
                _arrpara[18].Value = hnOldQty;
                _arrpara[19].Direction = ParameterDirection.Output;

                _arrpara[23].Value = TxtQualityGrmPerMeterMinus.Text == "" ? "0" : TxtQualityGrmPerMeterMinus.Text;
                _arrpara[24].Value = TxtQualityGrmPerMeterPlus.Text == "" ? "0" : TxtQualityGrmPerMeterPlus.Text;
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
                    chkexportsize.Checked = false;
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
        TxtCommission.Text = UtilityModule.Fill_Comm(Convert.ToInt32(DDDescription.SelectedValue)).ToString();
    }
    private void MASTER_RATE()
    {
        //TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue)).ToString();
        TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1, orderunitid: Convert.ToInt16(DDunit.SelectedValue)).ToString();
    }
    protected void TxtQtyRequired_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        TxtCancelQty.Text = "0";
        switch (Session["varcompanyNo"].ToString())
        {
            case "5":
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
            string shapename = "";
            if (variable.VarWEAVERPURCHASEORDER_FULLAREA == "1" && hnpurchasefolio.Value == "1")
            {
                if (DDDescription.SelectedIndex > 0)
                {
                    shapename = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select shapeName From V_FinishedItemDetail Where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                {
                    UtilityModule.FtAreaCalculate_WeaverOrder(TxtLength, TxtWidth, TxtArea, 1, Convert.ToInt16(Session["varcompanyId"]));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    UtilityModule.AreaMtSq_Weaverorder(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), TxtArea, "", Convert.ToInt16(Session["varcompanyId"]));
                }
            }
            else
            {

                if (DDDescription.SelectedIndex > 0)
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + DDDescription.SelectedValue));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
                }
            }
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
            if (Session["VarcompanyNo"].ToString() == "16")
            {
                string str = "select * From V_Weaverorder Where issueorderid=" + ViewState["IssueOrderid"];
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = "~\\Reports\\rptWeaverorder.rpt";
                    Session["Getdataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptWeaverorder.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
                }
                return;

            }
            else
            {
                Report();
            }

        }
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        string qry = "";
        string str = "";

        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //SqlParameter[] array = new SqlParameter[3];
            //array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            //array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            //array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);


            //array[0].Value = ViewState["IssueOrderid"];
            //array[1].Value = DDProcessName.SelectedValue;
            //array[2].Value = Session["VarcompanyNo"];

            ////if (Session["varcompanyId"].ToString() == "9")
            ////{
            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrderReport", array);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("Pro_ForProductionOrderReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            cmd.Parameters.AddWithValue("@IssueOrderId", ViewState["IssueOrderid"]);
            cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Session["VarcompanyId"]) == 5)
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderPoshNew.rpt";
                }
                else if (Convert.ToInt32(Session["VarcompanyId"]) == 9)//For Hafizia
                {
                    if (chkforsummary.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForHafiziaSummary.rpt";
                    }
                    else if (chkforProductionOrder.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForHafiziaHindi.rpt";
                    }
                    else if (chkforWeaverRawMaterial.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptWeaverRawMaterialForHafizia.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForHafizia.rpt";
                    }
                }
                else if (Convert.ToInt32(Session["VarcompanyNo"]) == 27)//For Antique Panipat
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderNewAntique.rpt";
                }
                else if (Convert.ToInt32(Session["VarcompanyNo"]) == 19)//For JRExport
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderNewJRExport.rpt";
                }
                else if (Convert.ToInt32(Session["VarcompanyNo"]) == 32)//For HomeTexIndia
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderNewHomeTexIndia.rpt";
                }
                else if (Convert.ToInt32(Session["VarcompanyNo"]) == 40)//For Kirpa Rugs
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderNewKirpaRugs.rpt";
                }
                else if (Convert.ToInt32(Session["VarcompanyNo"]) == 37)//For SundeepExports
                {
                    if (ChkForExport.Checked == true)
                    {
                        ExportExcelReport(ds);
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNew.rpt";
                    }
                }
                else
                {
                    if (variable.VarNewConsumptionWise == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewForMaltiRug.rpt";
                    }
                    else if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\ProductionOrderNewManyCustomerOrderOnSingleProOrder.rpt";
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
            //UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
            //Tran.Rollback();
            //LblErrorMessage.Text = ex.Message;
            //LblErrorMessage.Visible = true;
        }
        finally
        {
            //con.Close();
            //con.Dispose();
        }
    }
    protected void BtnPreviewConsumption_Click(object sender, EventArgs e)
    {
        ReportConsumption();
    }
    private void ReportConsumption()
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
            array[2].Value = Session["varcompanyId"];

            //if (Session["varcompanyId"].ToString() == "9")
            //{
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionConsumptionOrderReport", array);


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Session["VarcompanyId"]) == 27)//For Antique Panipat
                {
                    Session["rptFileName"] = "~\\Reports\\ProductionOrderConsumptionForAntiquePanipat.rpt";
                }

                // Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\ProductionConsumptionOrderNew.xsd";
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
            string sqlstr = "Select replace(convert(varchar(11),AssignDate,106), ' ','-') as AssignDate,UnitId,Remarks,Instruction,CalType,Item_Finished_Id,Rate,Qty, replace(convert(varchar(11),ReqByDate,106), ' ','-') as ReqByDate,FlagFixOrWeight,isnull(PURCHASEFLAG,0) as Purchaseflag From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where PM.IssueOrderId=PD.IssueOrderId And PM.IssueOrderId=" + DDPOrderNo.SelectedValue + " and PM.Status<>'Canceled'";
            con.Open();
            DataSet DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count > 0)
            {
                TxtRequiredDate.Text = DS.Tables[0].Rows[0]["ReqByDate"].ToString();
                TxtAssignDate.Text = DS.Tables[0].Rows[0]["AssignDate"].ToString();
                DDunit.SelectedValue = DS.Tables[0].Rows[0]["UnitId"].ToString();
                DDcaltype.SelectedValue = DS.Tables[0].Rows[0]["CalType"].ToString();
                TxtInstructions.Text = DS.Tables[0].Rows[0]["Instruction"].ToString();
                TxtRemarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                ViewState["IssueOrderid"] = DDPOrderNo.SelectedValue;
                ViewState["ProcessId"] = DDProcessName.SelectedValue;
                hnIssueOrderId.Value = DDPOrderNo.SelectedValue;
                hnpurchasefolio.Value = DS.Tables[0].Rows[0]["Purchaseflag"].ToString();
                if (DS.Tables[0].Rows[0]["FlagFixOrWeight"] == "0")
                {
                    ChkForFix.Checked = true;
                }

            }
            else
            {
                ViewState["IssueOrderid"] = "0";
            }
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            ViewState["IssueOrderid"] = DDPOrderNo.SelectedValue;
            SqlParameter[] param = new SqlParameter[6];

            param[0] = new SqlParameter("@issueorderid", ViewState["IssueOrderid"]);
            param[1] = new SqlParameter("@ISSUEDETAILID", VarProcess_Issue_Detail_Id);
            param[2] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "DELETEFIRSTPROCESSORDER", param);
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = param[3].Value.ToString();
            Fill_Grid();
            //int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);

            #region
            //            if (0 != Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Qty-PQty from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "")))
            //            {
            //                LblErrorMessage.Visible = true;
            //                LblErrorMessage.Text = "You Have Received....";
            //            }
            //            else
            //            {
            //                if (DGOrderdetail.Rows.Count == 1)
            //                {
            //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Issue_Master_" + DDProcessName.SelectedValue + @" 
            //                Where IssueOrderID in (Select IssueOrderID from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")");
            //                }
            //                #region Author: Rajeev, Date:3-dec-12....
            //                string str = "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "";
            //                str = str + "  Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id;
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

            //                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "");
            //                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + Session["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "");
            //                #endregion
            //                Tran.Commit();
            //                if (DGOrderdetail.Rows.Count == 1)
            //                {
            //                    Fill_Temp_OrderNo();
            //                }
            //                Fill_Grid();
            //                LblErrorMessage.Visible = true;
            //                LblErrorMessage.Text = "Successfully Deleted...";
            //            }
            #endregion
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
        //DeleteRow(e);
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
        if (TxtOrderNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
            @"Select * from TEMP_PROCESS_ISSUE_MASTER_NEW Where Companyid = " + DDCompanyName.SelectedValue + " And ChallanNo = '" + TxtOrderNo.Text + "'");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDCustomerCode.SelectedValue = Ds.Tables[0].Rows[0]["Customerid"].ToString();
                //hnCustomerId.Value = Ds.Tables[0].Rows[0]["Customerid"].ToString();
                CustomerCodeSelectedChange();
                DDCustomerOrderNumber.SelectedValue = Ds.Tables[0].Rows[0]["Orderid"].ToString();
                CustomerOrderNumberSelectedChange();
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ProcessSelectedChange();
                DDEmployeeName.SelectedValue = Ds.Tables[0].Rows[0]["Empid"].ToString();
                EmployeeSelectedChange();
                if (DDPOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["IssueOrderid"].ToString()) != null)
                {
                    DDPOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderid"].ToString();
                    POrderNoSelectedChange();
                }

            }
            else
            {
                TxtOrderNo.Text = "";
                TxtOrderNo.Focus();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Order No";
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
            if (btnclickflag == "BtnUpdate")
            {
                UpdateDetails();
            }
            else if (btnclickflag == "BtnCancelPO")
            {
                CancelPO(sender);
            }
            else if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(VarProcess_Issue_Detail_Id);
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

    protected void chkexportsize_CheckedChanged(object sender, EventArgs e)
    {
        dditemname_chage();
    }
    protected void btnupdateconsumption_Click(object sender, EventArgs e)
    {
        int rowcount = DGOrderdetail.Rows.Count;
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            for (int i = 0; i < rowcount; i++)
            {
                Label lblitemfinishedid = (Label)DGOrderdetail.Rows[i].FindControl("lblitemfinishedid");
                Label lblorderdid = (Label)DGOrderdetail.Rows[i].FindControl("lblorderdid");
                int issue_Detail_id = Convert.ToInt32(DGOrderdetail.DataKeys[i].Value);

                UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(DDPOrderNo.SelectedValue), issue_Detail_id, Convert.ToInt32(lblitemfinishedid.Text), Convert.ToInt16(DDProcessName.SelectedValue), Convert.ToInt32(lblorderdid.Text), Tran, TxtAssignDate.Text);

                ScriptManager.RegisterStartupScript(Page, GetType(), "consmp", "alert('Consumption updated successfully....')", true);
            }
            Tran.Commit();
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
    protected void CancelPO(object sender)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrpara = new SqlParameter[5];

            _arrpara[0] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            _arrpara[1] = new SqlParameter("@IssueOrderid", ViewState["IssueOrderid"]);
            _arrpara[2] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
            _arrpara[2].Direction = ParameterDirection.Output;
            _arrpara[3] = new SqlParameter("@userid", Session["varuserid"]);
            _arrpara[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);

            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CancelWeaverOrderNew", _arrpara);
            //*********

            Tran.Commit();
            LblErrorMessage.Visible = true;
            if (_arrpara[2].Value.ToString() != "")
            {
                LblErrorMessage.Text = _arrpara[2].Value.ToString();
            }
            else
            {
                LblErrorMessage.Text = "Po No. canceled successfully...";
                DDPOrderNo.SelectedIndex = 0;
                DDPOrderNo_SelectedIndexChanged(sender, new EventArgs());
                EmployeeSelectedChange();
            }
        }
        catch (Exception ex)
        {
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnCancelPO";
        txtpwd.Focus();
        Popup(true);

    }
    private void UpdateEmpName(object sender)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LblErrorMessage.Text = "";        
            LblErrorMessage.Visible = true;
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrpara = new SqlParameter[7];

                _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@EmpId", SqlDbType.Int);                
                _arrpara[2] = new SqlParameter("@Processid", SqlDbType.Int);               
                _arrpara[3] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                _arrpara[6] = new SqlParameter("@ReqByDate", SqlDbType.DateTime);
              
                _arrpara[0].Value = (ViewState["IssueOrderid"]);
                _arrpara[1].Value = DDEmployeeName.SelectedValue;                
                _arrpara[2].Value = DDProcessName.SelectedValue;              
                _arrpara[3].Value = Session["varcompanyid"];
                _arrpara[4].Value = Session["varuserid"];              
                _arrpara[5].Direction = ParameterDirection.Output;
                _arrpara[6].Value = TxtRequiredDate.Text;
                
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateProductionOrderEmpName", _arrpara);
                              

                if (_arrpara[5].Value.ToString() == "")
                {                   
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "EMPLOYEE NAME/REQ DATE UPDATED SUCCESSFULLY."; 
                    Tran.Commit();
                    Fill_Temp_OrderNo();
                }
                else
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[5].Value.ToString();
                    Tran.Commit();
                    
                }               

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
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
    protected void BtnUpdateEmpName_Click(object sender, EventArgs e)
    {
        UpdateEmpName(sender);      

    }
    protected void ExportExcelReport(DataSet ds)
    {
        if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
        }
        string Path = "";

        ////string str = @"select * From V_PACKINGLIST_EXCEL Where  invoiceid='" + ViewState["InvoiceId"] + "' order by Id";
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        //string str = @"select * From V_PACKINGLIST_CHAMPO Where  invoiceid='" + ViewState["InvoiceId"] + "' order by Id";
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlCommand cmd = new SqlCommand(str, con);
        ////cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandTimeout = 300;

        //DataSet ds = new DataSet();
        //SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //cmd.ExecuteNonQuery();
        //ad.Fill(ds);
        ////*************

        //con.Close();
        //con.Dispose();


        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ProductionOrderDetail");

            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            sht.PageSetup.AdjustTo(77);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            sht.PageSetup.VerticalDpi = 300;
            sht.PageSetup.HorizontalDpi = 300;

            //
            sht.PageSetup.Margins.Top = 0.2;
            sht.PageSetup.Margins.Left = 0.5;
            sht.PageSetup.Margins.Right = 0.3;
            sht.PageSetup.Margins.Bottom = 0.2;
            sht.PageSetup.FitToPages(1, 1);
            //sht.PageSetup.SetScaleHFWithDocument();


            sht.Column("A").Width = 20.67;
            sht.Column("B").Width = 20.89;
            sht.Column("C").Width = 20.89;
            sht.Column("D").Width = 23.23;
            sht.Column("E").Width = 11.56;
            sht.Column("F").Width = 11.67;
            sht.Column("G").Width = 10.78;
            sht.Column("H").Width = 10.67;
            sht.Column("I").Width = 13.44;            


            sht.Row(1).Height = 26.4;
            

            //************
            //*****Header                
            sht.Cell("A1").Value ="Production Order ("+ ds.Tables[0].Rows[0]["IssueOrderId"]+")";
            sht.Range("A1:I1").Style.Font.FontName = "Times New Roman";
            sht.Range("A1:I1").Style.Font.FontSize = 13;
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:I1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:I1").Merge();
            //***************           

                using (var a = sht.Range("A1:I1"))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A1:A13"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("I1:I13"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }             


                sht.Cell("A2").Value = "Company Name: " + ds.Tables[0].Rows[0]["CompanyName"];
                sht.Range("A2:C2").Style.Font.FontName = "Times New Roman";
                sht.Range("A2:C2").Style.Font.FontSize = 11;
                sht.Range("A2:C2").Style.Font.Bold = true;
                sht.Range("A2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A2:C2").Merge();

                using (var a = sht.Range("A2:C2"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("C2:C10"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("A3").Value = "Address. " + ds.Tables[0].Rows[0]["CompAddr1"]+" "+ds.Tables[0].Rows[0]["CompAddr2"]+" "+ds.Tables[0].Rows[0]["CompAddr3"]+"";
                sht.Range("A3:C5").Style.Font.FontName = "Times New Roman";
                sht.Range("A3:C5").Style.Font.FontSize = 11;
                sht.Range("A3:C5").Style.Font.Bold = true;
                sht.Range("A3:C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A3:C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:C5").Merge();
                sht.Range("A3:C5").Style.Alignment.SetWrapText();

                using (var a = sht.Range("A5:C5"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("A6").Value = "Phone No:"+" "+ ds.Tables[0].Rows[0]["CompTel"];
                sht.Range("A6:C6").Style.Font.FontName = "Times New Roman";
                sht.Range("A6:C6").Style.Font.FontSize = 11;
                sht.Range("A6:C6").Style.Font.Bold = true;
                sht.Range("A6:C6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A6:C6").Merge();

                using (var a = sht.Range("A6:C6"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("A7").Value = "GSTIN No:"+" "+ ds.Tables[0].Rows[0]["GSTNo"];
                sht.Range("A7:C7").Style.Font.FontName = "Times New Roman";
                sht.Range("A7:C7").Style.Font.FontSize = 11;
                sht.Range("A7:C7").Style.Font.Bold = true;
                sht.Range("A7:C7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A7:C7").Merge();

                using (var a = sht.Range("A7:C7"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }            


                sht.Cell("D2").Value = "Emp Name: "+" " + ds.Tables[0].Rows[0]["EmpName"];
                sht.Range("D2:I2").Style.Font.FontName = "Times New Roman";
                sht.Range("D2:I2").Style.Font.FontSize = 11;
                sht.Range("D2:I2").Style.Font.Bold = true;
                sht.Range("D2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D2:I2").Merge();

                using (var a = sht.Range("D2:I2"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D3").Value = "Address:" + " " + ds.Tables[0].Rows[0]["Address"];
                sht.Range("D3:I4").Style.Font.FontName = "Times New Roman";
                sht.Range("D3:I4").Style.Font.FontSize = 11;
                sht.Range("D3:I4").Style.Font.Bold = true;
                sht.Range("D3:I4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D3:I4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("D3:I4").Merge();
                sht.Range("D3:I4").Style.Alignment.SetWrapText();

                using (var a = sht.Range("D4:I4"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                sht.Cell("D5").Value ="Phone No"+" "+ ds.Tables[0].Rows[0]["PhoneNo"];
                sht.Range("D5:I5").Style.Font.FontName = "Times New Roman";
                sht.Range("D5:I5").Style.Font.FontSize = 11;
                sht.Range("D5:I5").Style.Font.Bold = true;
                sht.Range("D5:I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D5:I5").Merge();

                using (var a = sht.Range("D5:I5"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D6").Value = "GSTIN No:" + " " + ds.Tables[0].Rows[0]["EmpGstNo"].ToString();
                sht.Range("D6:I6").Style.Font.FontName = "Times New Roman";
                sht.Range("D6:I6").Style.Font.FontSize = 11;
                sht.Range("D6:I6").Style.Font.Bold = true;
                sht.Range("D6:I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D6:I6").Merge();

                using (var a = sht.Range("D6:I6"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D7").Value = "POrder Date:" + " " + Convert.ToDateTime(ds.Tables[0].Rows[0]["AssignDate"]).ToString("dd/MM/yyyy");
                sht.Range("D7:F7").Style.Font.FontName = "Times New Roman";
                sht.Range("D7:F7").Style.Font.FontSize = 11;
                sht.Range("D7:F7").Style.Font.Bold = true;
                sht.Range("D7:F7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D7:F7").Merge();

                using (var a = sht.Range("D7:F7"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }               

         
                sht.Cell("G7").Value = "Delivery Date:" + " " + Convert.ToDateTime(ds.Tables[0].Rows[0]["ReqByDate"]).ToString("dd/MM/yyyy");
                sht.Range("G7:I7").Style.Font.FontName = "Times New Roman";
                sht.Range("G7:I7").Style.Font.FontSize = 11;
                sht.Range("G7:I7").Style.Font.Bold = true;
                sht.Range("G7:I7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G7:I7").Merge();

                using (var a = sht.Range("G8:I8"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D8").Value = "Unit Name" + " " + ds.Tables[0].Rows[0]["UnitName"].ToString();
                sht.Range("D8:I8").Style.Font.FontName = "Times New Roman";
                sht.Range("D8:I8").Style.Font.FontSize = 11;
                sht.Range("D8:I8").Style.Font.Bold = true;
                sht.Range("D8:I8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D8:I8").Merge();

                using (var a = sht.Range("D8:I8"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D9").Value ="PO No"+ " "+ ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                sht.Range("D9:I9").Style.Font.FontName = "Times New Roman";
                sht.Range("D9:I9").Style.Font.FontSize = 11;
                sht.Range("D9:I9").Style.Font.Bold = true;
                sht.Range("D9:I9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D9:I9").Merge();

                using (var a = sht.Range("D9:I9"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("D10").Value = "Buyer OrderNo:" + " " + ds.Tables[0].Rows[0]["CustomerOrderNo"].ToString();
                sht.Range("D10:F10").Style.Font.FontName = "Times New Roman";
                sht.Range("D10:F10").Style.Font.FontSize = 11;
                sht.Range("D10:F10").Style.Font.Bold = true;
                sht.Range("D10:F10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D10:F10").Merge();

                using (var a = sht.Range("D10:F10"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Cell("G10").Value = "Customer Code:"+" " + ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                sht.Range("G10:I10").Style.Font.FontName = "Times New Roman";
                sht.Range("G10:I10").Style.Font.FontSize = 11;
                sht.Range("G10:I10").Style.Font.Bold = true;
                sht.Range("G10:I10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G10:I10").Merge();

                using (var a = sht.Range("D10:I10"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A10:I10"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A11").Value = "Item Name";
                sht.Range("A11:A12").Style.Font.FontName = "Times New Roman";
                sht.Range("A11:A12").Style.Font.FontSize = 12;
                sht.Range("A11:A12").Style.Font.Bold = true;
                sht.Range("A11:A12").Merge();
                sht.Range("A11:A12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A11:A12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("B11").Value = "Quality";
                sht.Range("B11:B12").Style.Font.FontName = "Times New Roman";
                sht.Range("B11:B12").Style.Font.FontSize = 12;
                sht.Range("B11:B12").Style.Font.Bold = true;
                sht.Range("B11:B12").Merge();
                sht.Range("B11:B12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B11:B12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("C11").Value = "Design";
                sht.Range("C11:C12").Style.Font.FontName = "Times New Roman";
                sht.Range("C11:C12").Style.Font.FontSize = 12;
                sht.Range("C11:C12").Style.Font.Bold = true;
                sht.Range("C11:C12").Merge();
                sht.Range("C11:C12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C11:C12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("D11").Value = "Color";
                sht.Range("D11:D12").Style.Font.FontName = "Times New Roman";
                sht.Range("D11:D12").Style.Font.FontSize = 12;
                sht.Range("D11:D12").Style.Font.Bold = true;
                sht.Range("D11:D12").Merge();
                sht.Range("D11:D12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D11:D12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E11").Value = "Size";
                sht.Range("E11:E12").Style.Font.FontName = "Times New Roman";
                sht.Range("E11:E12").Style.Font.FontSize = 12;
                sht.Range("E11:E12").Style.Font.Bold = true;
                sht.Range("E11:E12").Merge();
                sht.Range("E11:E12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E11:E12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("F11").Value = "Qty";
                sht.Range("F11:F12").Style.Font.FontName = "Times New Roman";
                sht.Range("F11:F12").Style.Font.FontSize = 12;
                sht.Range("F11:F12").Style.Font.Bold = true;
                sht.Range("F11:F12").Merge();
                sht.Range("F11:F12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F11:F12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("G11").Value = "Area in Sq." + ds.Tables[0].Rows[0]["UnitName"] + "";
                sht.Range("G11:G12").Style.Font.FontName = "Times New Roman";
                sht.Range("G11:G12").Style.Font.FontSize = 12;
                sht.Range("G11:G12").Style.Font.Bold = true;
                sht.Range("G11:G12").Merge();
                sht.Range("G11:G12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G11:G12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G11:G12").Style.Alignment.SetWrapText();

                sht.Range("H11").Value = "Rate";
                sht.Range("H11:H12").Style.Font.FontName = "Times New Roman";
                sht.Range("H11:H12").Style.Font.FontSize = 12;
                sht.Range("H11:H12").Style.Font.Bold = true;
                sht.Range("H11:H12").Merge();
                sht.Range("H11:H12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H11:H12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H11:H12").Style.Alignment.SetWrapText();

                sht.Range("I11").Value = "Amount";
                sht.Range("I11:I12").Style.Font.FontName = "Times New Roman";
                sht.Range("I11:I12").Style.Font.FontSize = 12;
                sht.Range("I11:I12").Style.Font.Bold = true;
                sht.Range("I11:I12").Merge();
                sht.Range("I11:I12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I11:I12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I11:I12").Style.Alignment.SetWrapText();




                using (var a = sht.Range("A11:I12"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //

                decimal TotalArea = 0, TotalTaxableValue = 0, TotalIGSTAmount = 0, TotalAmountInINR = 0, TotalAmountBeforeTax = 0;

                int row = 13;
                int totalrowcount = 55;
                int rowcount = ds.Tables[0].Rows.Count;
                totalrowcount = totalrowcount - rowcount;
                int SrNo = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Times New Roman";
                    sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 11;
                    //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    //sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();
                    sht.Range("A" + row + ":I" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    SrNo = SrNo + 1;

                    //sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"].ToString());
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();                  
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Size2"] + " " + ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("E" + row).Style.Alignment.SetWrapText();
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Area"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);                  


                    using (var a = sht.Range("A" + row + ":I" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                //row = row + 1;
                sht.Range("E" + row + ":E" + row).Merge();
                sht.Range("E" + row).SetValue("TOTAL");              
                sht.Range("F" + row).SetValue(ds.Tables[0].Compute("sum(Qty)", ""));
                sht.Range("G" + row).SetValue(string.Format("{0:0.0000}", ds.Tables[0].Compute("sum(Area)", "")));
                sht.Range("I" + row).SetValue(string.Format("{0:0.00}", ds.Tables[0].Compute("sum(Amount)", "")));
               

                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var a = sht.Range("A" + row + ":I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                ////Blank Row
                row = row + 1;
                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 12;
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;
               
                sht.Range("A" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row ).Style.Font.FontSize = 12;
                sht.Range("A" + row ).Style.Font.SetBold();
                sht.Range("A" + row).Value = "Item Name";
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                sht.Range("B" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("B" + row).Style.Font.FontSize = 12;
                sht.Range("B" + row).Style.Font.SetBold();
                sht.Range("B" + row).Value = "Quality";
                sht.Range("B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("C" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("C" + row).Style.Font.FontSize = 12;
                sht.Range("C" + row).Style.Font.SetBold();
                sht.Range("C" + row).Value = "Design/Color";
                sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("D" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("D" + row).Style.Font.FontSize = 12;
                sht.Range("D" + row).Style.Font.SetBold();
                sht.Range("D" + row).Value = "Shade Color";
                sht.Range("D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("E" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("E" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row).Style.Font.SetBold();
                sht.Range("E" + row).Value = "Qty";
                sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                using (var a = sht.Range("A" + row + ":E" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                row = row + 1;

                if (ds.Tables[1].Rows.Count > 0)
                {                    
                    int noofrows2 = 0;
                    int i2 = 0;
                    DataTable dtdistinctFinishedId = ds.Tables[1].DefaultView.ToTable(true,"Item_Name", "Quality","Design","Color", "ShadeColor", "FinishedId");
                    noofrows2 = dtdistinctFinishedId.Rows.Count;

                    for (i2 = 0; i2 < noofrows2; i2++)
                    {

                        sht.Range("A" + row + ":E" + row).Style.Font.FontName = "Times New Roman";
                        sht.Range("A" + row + ":E" + row).Style.Font.FontSize = 11;
                        //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                        sht.Range("A" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        //sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();
                        sht.Range("A" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        SrNo = SrNo + 1;

                        //sht.Range("B" + row + ":B" + row).Merge();
                        sht.Range("A" + row).SetValue(dtdistinctFinishedId.Rows[i2]["Item_Name"].ToString());
                        sht.Range("A" + row).Style.Alignment.SetWrapText();
                        sht.Range("B" + row).SetValue(dtdistinctFinishedId.Rows[i2]["Quality"].ToString());
                        sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(dtdistinctFinishedId.Rows[i2]["Design"].ToString() + " " + dtdistinctFinishedId.Rows[i2]["Color"].ToString());
                        sht.Range("C" + row).Style.Alignment.SetWrapText();
                        sht.Range("D" + row).SetValue(dtdistinctFinishedId.Rows[i2]["ShadeColor"].ToString());
                        sht.Range("D" + row).Style.Alignment.SetWrapText();
                        sht.Range("E" + row).SetValue(ds.Tables[1].Compute("sum(Qty)", "Quality='" + dtdistinctFinishedId.Rows[i2]["Quality"].ToString() + "' and FinishedId='" + dtdistinctFinishedId.Rows[i2]["FinishedId"].ToString() + "' "));                       


                        using (var a = sht.Range("A" + row + ":E" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;


                    }
                }

                row = row + 1;              

               
                sht.Range("A" + row + ":I" + (row+1)).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + (row + 1)).Style.Font.FontSize = 12;
                sht.Range("A" + row + ":I" + (row + 1)).Style.Font.Bold = true;
                sht.Range("A" + row + ":I" + (row + 1)).Merge();
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetWrapText();
                sht.Range("A" + row).Value = "Instruction :-" + " " + ds.Tables[0].Rows[0]["Instruction"];

                row = row + 2;
               
                sht.Range("A" + row + ":I" + (row + 1)).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + (row + 1)).Style.Font.FontSize = 12;
                sht.Range("A" + row + ":I" + (row + 1)).Style.Font.Bold = true;
                sht.Range("A" + row + ":I" + (row + 1)).Merge();
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A" + row + ":I" + (row + 1)).Style.Alignment.SetWrapText();
                sht.Range("A" + row).Value = "Remarks :-" + " " + ds.Tables[0].Rows[0]["Remarks"];

                ////Blank Row
                row = row + 2;
                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 12;
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //using (var a = sht.Range("A" + row + ":I" + row))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}

                ////Blank Row
                row = row + 1;
                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 12;
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //using (var a = sht.Range("A" + row + ":I" + row))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}

                row = row + 1;

                sht.Range("A" + row).Value = "Order Issue Signature";
                sht.Range("A" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + row).Style.Font.FontSize = 12;
                sht.Range("A" + row).Style.Font.Bold = true;                
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A" + row).Style.Alignment.SetWrapText();

                sht.Range("C" + row).Value = "Prepared By";
                sht.Range("C" + row).Style.Font.FontName = "Times New Roman";
                sht.Range("C" + row).Style.Font.FontSize = 12;
                sht.Range("C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("C" + row).Style.Alignment.SetWrapText();                

                sht.Range("E" + row).Value = "Receiver Signature";
                sht.Range("E" + row + ":F" +row).Style.Font.FontName = "Times New Roman";
                sht.Range("E" + row + ":F" + row).Style.Font.FontSize = 12;
                sht.Range("E" + row + ":F" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":F" + row).Merge();
                sht.Range("E" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E" + row + ":F" + row).Style.Alignment.SetWrapText();

                string Fileextension = "xlsx";
                string filename = "ProductionOrderExcelReport_" + UtilityModule.validateFilename(ds.Tables[0].Rows[0]["IssueOrderId"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Tempexcel/" + filename);
                //string Fileextension = "xlsx";
                //string filename = "Packing-" + UtilityModule.validateFilename("invoice") + "." + Fileextension + "";
                //Path = Server.MapPath("~/PackingExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
        }
    }

}