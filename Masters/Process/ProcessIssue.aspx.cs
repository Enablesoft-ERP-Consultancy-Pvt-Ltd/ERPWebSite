using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Process_ProcessIssue : System.Web.UI.Page
{
    private DataSet DS;
    static int MasterCompanyId;
    static string hnempid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            try
            {
                DataSet ds = new DataSet();
                DDCustomerCode.Focus();
                string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                
                select unitid,unitname from unit where unitid in (1,2,4,6,7)";


                //Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varCompanyId"] + @" order by customercode
                ds = SqlHelper.ExecuteDataset(str);
                UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");

                if (DDCompanyName.Items.Count > 0)
                {
                    DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                    DDCompanyName.Enabled = false;
                }

                if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
                {
                    Tdlabelcustcode.Visible = false;
                    Tddropdowncustcode.Visible = false;
                }
                DDCompanyName_SelectedIndexChanged(sender, new EventArgs());
                //UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select--");
                UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 1, true, "--Select--");

                if (hncomp.Value == "6")
                {
                    DDunit.SelectedValue = "1";
                    BtnPreviewConsumption.Visible = false;
                }
                else if (Session["varcompanyId"].ToString() == "27")
                {
                    DDunit.SelectedValue = "1";
                    BtnPreviewConsumption.Visible = true;
                }
                else
                {
                    DDunit.SelectedIndex = 2;
                    BtnPreviewConsumption.Visible = false;
                }

                if (Session["VarCompanyId"].ToString() == "37")
                {
                    DDunit.SelectedValue = "2";
                    ChkForExport.Visible = true;
                }
                else
                {
                    ChkForExport.Visible = false;
                }

                ViewState["IssueOrderid"] = 0;
                lablechange();
                hnCustomerId.Value = "0";

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
            }

            #region Author: Rajeev, Date: 30-Nov-12 .......
            #endregion
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select smssetting,VarProdCode from Mastersetting");
            //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            if (Convert.ToInt16(ds1.Tables[0].Rows[0]["smssetting"]) == 1)
            {
                chkforsms.Visible = true;
            }
            else
            {
                chkforsms.Visible = false;
            }

            if (Session["varcompanyId"].ToString() == "20")
            {
                btnaddEmployee.Visible = true;
            }
            else
            {
                btnaddEmployee.Visible = false;
            }
            //int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode, From MasterSetting"));
            switch (Convert.ToInt16(ds1.Tables[0].Rows[0]["VarProdCode"]))
            {
                case 0:
                    TDProductCode.Visible = false;
                    TDLblProdCode.Visible = false;
                    break;
                case 1:
                    TDProductCode.Visible = true;
                    TDLblProdCode.Visible = true;
                    break;
            }
            //companyno
            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 9:
                    chkforsummary.Visible = true;
                    lblLocalOrderNo.Visible = true;
                    txtlocalorerNo.Visible = true;
                    tdLQualityGrmPerMeterMinus.Visible = true;
                    tdLQualityGrmPerMeterPlus.Visible = true;
                    tdtxtQualityGrmPerMeterMinus.Visible = true;
                    tdtxtQualityGrmPerMeterPlus.Visible = true;
                    chkforProductionOrder.Visible = true;
                    chkforWeaverRawMaterial.Visible = true;
                    Label5.Text = "Folio No";
                    //Label18.Text = "Value";
                    break;
                case 30:
                    Label5.Text = "Folio No";
                    break;
            }



        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varCompanyId"] + " order by CustomerId", true, "--Select--");
        if (Tddropdowncustcode.Visible == true)
        {
            FillCustomercode();
        }
        else
        {
            FillOrderNO();
        }

    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        // UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.Customerid=" + DDCustomerCode.SelectedValue + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " order by OM.OrderId", true, "--Select--");

        FillOrderNO();
    }
    protected void FillOrderNO()
    {
        hnempid = "";
        ViewState["IssueOrderid"] = 0;
        string view = "";
        string orderNo = "OM.CustomerOrderNo";
        if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        {
            orderNo = "Om.Localorder + ' # '+om.customerorderNo ";
        }
        if (Session["varcompanyid"].ToString() == "9")
        {
            view = "V_ProductionIssueBal_OrderNo_Hafizia";
        }
        else
        {
            view = "V_ProductionIssueBal_OrderNo";
        }
        string str = "";
        if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
        {
            str = @"select Distinct OM.OrderId," + orderNo + @" as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    and Om.ORDERFROMSAMPLE=0
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join V_PRODUCTIONISSUEBAL_ORDERNO_TAGGINGWITHINTERNALPROD VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    Where OM.CompanyId=" + DDCompanyName.SelectedValue;
            if (DDCustomerCode.SelectedIndex > 0)
            {
                str = str + " and OM.CustomerId=" + DDCustomerCode.SelectedValue;
            }
            str = str + " order by OM.OrderId";
        }
        else
        {
            str = @"select Distinct OM.OrderId," + orderNo + @" as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    and oM.ORDERFROMSAMPLE=0
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join " + view + @" VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    Where OM.CompanyId=" + DDCompanyName.SelectedValue;
            if (DDCustomerCode.SelectedIndex > 0)
            {
                str = str + " and OM.CustomerId=" + DDCustomerCode.SelectedValue;
            }
            if (Session["VarCompanyNo"].ToString() == "38")
            {
                str = str + " and OM.Status=0";
            }
            str = str + " order by OM.OrderId";
        }
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, str, true, "--Plz Select--");
    }
    protected void FillCustomercode()
    {
        string view = "";
        if (Session["varcompanyid"].ToString() == "9")
        {
            view = "V_ProductionIssueBal_OrderNo_Hafizia";
        }
        else
        {
            view = "V_ProductionIssueBal_OrderNo";
        }

        string str = @"select Distinct CU.Customerid,Cu.customercode from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join " + view + @" VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    inner Join Customerinfo cu on om.customerid=cu.customerid
                    Where OM.CompanyId=" + DDCompanyName.SelectedValue + " order by customercode";

        UtilityModule.ConditionalComboFill(ref DDCustomerCode, str, true, "--Plz Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
        //{
        //    LblErrorMessage.Text = "";
        //    int checkcustomerid = 0;  
        //    string str2 = "";
        //    str2 = "select CustomerId from OrderMaster Where orderid=" + DDCustomerOrderNumber.SelectedValue;
        //    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        //    if (ds2.Tables[0].Rows.Count > 0)
        //    {
        //        checkcustomerid= Convert.ToInt32( ds2.Tables[0].Rows[0]["CustomerId"].ToString());
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

        string Str = @"SELECT PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
        // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        hncomp.Value = Convert.ToString(Session["varCompanyId"]);
        switch (hncomp.Value)
        {
            case "6":
            case "10":
                break;
            case "9":
                Str = Str + " And PROCESS_NAME_ID in(1,16,35)";
                break;
            default:
                Str = Str + " And PROCESS_NAME_ID=1";
                break;
        }

        Str = Str + " select  Top(1) orderunitid from orderdetail Where orderid=" + DDCustomerOrderNumber.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Select--");

        if (Session["varcompanyId"].ToString() == "20")
        {
            DDProcessName.Enabled = false;
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                DDProcessNameSelectedIndex();
            }
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() != "37")
            {
                DDunit.SelectedValue = ds.Tables[1].Rows[0]["OrderUnitid"].ToString();
            }
        }

        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
        {
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                DDProcessNameSelectedIndex();
            }
        }

    }

    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DDProcessNameSelectedIndex();
        if (DDProcessName.SelectedIndex > 0)
        {
            tdlblLastProductionOrder.Visible = true;
            tdLastProductionOrder.Visible = true;
            lblLastProductionOrder.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(Max(IssueOrderid ),0) from process_issue_master_" + DDProcessName.SelectedValue + ""));
        }
        else
        {
            lblLastProductionOrder.Text = "";
            tdlblLastProductionOrder.Visible = false;
            tdLastProductionOrder.Visible = false;
        }

    }
    protected void DDProcessNameSelectedIndex()
    {
        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "0")
        {
            ViewState["IssueOrderid"] = 0;
        }

        string viewname = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            viewname = "V_FinishedItemDetailNew";
        }
        if (DDCustomerOrderNumber.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0)
        {
            DataSet ds = new DataSet();
            string str;
            switch (Session["varcompanyid"].ToString())
            {
                case "9":

                    str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA," + viewname + @" VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";

                    break;
                default:
                    if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA," + viewname + @" VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     V_PROCESSORDERQTYOTHER_TAGGINGWITHINTERNALPRODUCTION PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0))";

                    }
                    else
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA," + viewname + @" VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQtyother PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0))";
                    }
                    break;
            }
            str = str + " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid  And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " and isnull(Ei.blacklist,0)=0 order by EI.Empname";
            str = str + " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP, Department DP Where EI.Empid=EP.Empid and EI.DepartmentId=DP.DepartmentId And DP.DepartmentName='Production' And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " and isnull(Ei.blacklist,0)=0 order by EI.Empname";
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 0, true, "--Select--");
            if (DDCategoryName.Items.Count > 0)
            {
                DDCategoryName.SelectedIndex = 1;
                FillCategorySelectedChange();
            }
            if (hncomp.Value == "6" || hncomp.Value == "10")
                UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 1, true, "--Select--");
            else
                UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Select--");

            if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1" && hnempid.ToString() != "")
            {
                DDEmployeeName.SelectedValue = hnempid;
            }


        }
        TxtAssignDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
        TxtRequiredDate.Text = (DateTime.Now).ToString("dd-MMM-yyyy");
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategorySelectedChange();
    }
    private void FillCategorySelectedChange()
    {
        string STR;
        string ViewName = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            ViewName = "V_FinishedItemDetailNew";

        }
        if (Session["varcompanyId"].ToString() == "9")
        {

            STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA," + ViewName + @" VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";

        }
        else
        {
            if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA," + ViewName + @" VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     V_PROCESSORDERQTYOTHER_TAGGINGWITHINTERNALPRODUCTION PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0))";
            }
            else
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA," + ViewName + @" VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQtyother PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0))";
            }
        }
        UtilityModule.ConditionalComboFill(ref DDItemName, STR, true, "---Select---");
        if (DDItemName.Items.Count > 0)
        {
            DDItemName.SelectedIndex = 1;
            Fill_Description();
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Description();
    }
    private void Fill_Description()
    {
        string STR;
        string size = "";
        if (chkexportsize.Checked == true || (variable.VarWEAVERPURCHASEORDER_FULLAREA == "1" && chkpurchasefolio.Checked == true))
        {
            switch (DDunit.SelectedValue.ToString())
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
            switch (DDunit.SelectedValue.ToString())
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
        //////***************************************

        if (variable.VarProductionSizeItemWise == "1")
        {
            if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
            {
                STR = @"Select distinct JA.Item_Finished_Id,
                        IPM.QualityName+Space(2)+IPM.DesignName+Space(2)+IPM.ColorName+Space(2)+IPM.ShapeName+Space(2)+IPM.ShadeColorName+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + @",'') Else '' End End Description 
                     From JobAssigns JA(NoLock) 
                     inner join OrderDetail Od(NoLock) ON JA.Item_Finished_Id=Od.Item_Finished_Id 
                     inner join V_FinishedItemDetail IPM(NoLock) On IPM.ITEM_FINISHED_ID=JA.Item_Finished_Id  
                     inner join Item_Master IM(NoLock) ON IM.Item_Id=IPM.Item_Id 
                     JOIN SizeAttachedWithItem SA(NoLock) ON IPM.SizeId=SA.SizeId and SA.ItemId=IM.ITEM_ID
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And IPM.Item_Id=" + DDItemName.SelectedValue + @" 
                            AND JA.Item_Finished_Id in  (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                            V_PROCESSORDERQTYOTHER_TAGGINGWITHINTERNALPRODUCTION PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                            Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0))
                order by Description";
            }
            else
            {
                STR = @"Select distinct JA.Item_Finished_Id,
                        IPM.QualityName+Space(2)+IPM.DesignName+Space(2)+IPM.ColorName+Space(2)+IPM.ShapeName+Space(2)+IPM.ShadeColorName+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + @",'') Else '' End End Description 
                    From JobAssigns JA(NoLock) 
                    inner join OrderDetail Od(NoLock) ON JA.Item_Finished_Id=Od.Item_Finished_Id 
                    inner join V_FinishedItemDetail IPM(NoLock) On IPM.ITEM_FINISHED_ID=JA.Item_Finished_Id  
                    inner join Item_Master IM(NoLock) ON IM.Item_Id=IPM.Item_Id 
                    JOIN SizeAttachedWithItem SA(NoLock) ON IPM.SizeId=SA.SizeId and SA.ItemId=IM.ITEM_ID
                    Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And IPM.Item_Id=" + DDItemName.SelectedValue + @" 
                        AND JA.Item_Finished_Id in  (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                        v_ProcessOrderQtyother PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                        Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0)) 
                    order by Description";
               
            }
        }
        else
        {
            if (Session["varcompanyId"].ToString() == "9") //Hafizia
            {

                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + ",'') Else Isnull(" + size + @",'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";

            }
            else
            {
                if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                {
                    STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + @",'') Else '' End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in  (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     V_PROCESSORDERQTYOTHER_TAGGINGWITHINTERNALPRODUCTION PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0)) order by Description";
                }
                else
                {
                    STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(" + size + ",'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(" + size + @",'') Else '' End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in  (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQtyother PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(PD.Qty),0)) order by Description";
                }
            }
        }


        
        UtilityModule.ConditionalComboFill(ref DDDescription, STR, true, "--Select--");
    }
    private void Area()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {

            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select size_Id,SHAPE_ID from Item_Parameter_Master where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                int SizeId = Convert.ToInt32(Ds.Tables[0].Rows[0]["size_Id"]);
                if (SizeId != 0 && hncomp.Value != "6")
                {
                    LblArea.Visible = true;
                    TdArea.Visible = true;
                    SqlParameter[] _arrpara = new SqlParameter[10];
                    _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                    _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@Shapeid", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@ExportSizeflag", SqlDbType.TinyInt);
                    _arrpara[7] = new SqlParameter("@PuchaseFolio", SqlDbType.TinyInt);
                    _arrpara[8] = new SqlParameter("@ShapeName", SqlDbType.VarChar, 10);
                    _arrpara[9] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);

                    _arrpara[0].Value = SizeId;
                    _arrpara[1].Value = DDunit.SelectedValue;
                    _arrpara[2].Direction = ParameterDirection.Output;
                    _arrpara[3].Direction = ParameterDirection.Output;
                    _arrpara[4].Direction = ParameterDirection.Output;
                    _arrpara[5].Direction = ParameterDirection.Output;
                    _arrpara[6].Value = chkexportsize.Checked == true ? "1" : "0";
                    _arrpara[7].Value = chkpurchasefolio.Checked == true ? "1" : "0";
                    _arrpara[8].Direction = ParameterDirection.Output;
                    _arrpara[9].Value = DDDescription.SelectedValue;

                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Area", _arrpara);

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
                    else if (Session["varCompanyNo"].ToString() == "38")
                    {
                        TxtArea.Text = string.Format("{0:#0.00}", _arrpara[4].Value);
                        TxtArea.Text = UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(TxtArea.Text), variable.VarRoundFtFlag).ToString();
                    }
                    else
                    {
                        TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
                    }

                   
                    if (variable.VarWEAVERPURCHASEORDER_FULLAREA == "1" && chkpurchasefolio.Checked == true)
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
                            if (Session["varCompanyId"].ToString() == "33" && Convert.ToInt32(DDunit.SelectedValue) == 2)
                            {
                                TxtArea.Text = string.Format("{0:#0.00}", _arrpara[4].Value);
                            }
                            else
                            {

                                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"]), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
                            }
                          

                            //TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"]), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
                        }
                    }
                }
                else if (SizeId != 0 && hncomp.Value == "6")
                {
                    //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                    string str = "";
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = "select ExpWidthMS_Ft as WidthFt,ExpLengthMS_Ft as LengthFt, 0 as HeightFt,LEFT(MtrSize, CHARINDEX('x', MtrSize) - 1) AS WidthMtr,REPLACE(SUBSTRING(MtrSize, CHARINDEX('x', MtrSize), LEN(MtrSize)), 'x', '') as LengthMtr,0 as HeightMtr,Export_Area as AreaFt,MtrArea as AreaMtr from QualitySizeNew where sizeid=" + SizeId + " and updatedate is null";
                    }                   
                    else
                    {
                        str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr from size where sizeid=" + SizeId + " And MasterCompanyId=" + Session["varCompanyId"];
                    }
                    DS = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
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
                    LblArea.Visible = false;
                    TdArea.Visible = false;
                    TxtArea.Text = "0";
                }
                //datatset dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCompanyName, "", true, "--Select--");
        ViewState["IssueOrderid"] = 0;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        ProcessIssue();
    }
    private void ProcessReportPath()
    {
        #region Author: Rajeev, Date: 30-Nov-12...
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string str = "";
        str = "Delete TEMP_PROCESS_ISSUE_MASTER ";
        str = str + " Delete TEMP_PROCESS_ISSUE_DETAIL  ";
        str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["ProcessId"] + " from PROCESS_ISSUE_MASTER_" + ViewState["ProcessId"] + " Where IssueOrderId=" + ViewState["IssueOrderid"] + "";
        str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight) Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + ViewState["ProcessId"] + " Where IssueOrderId=" + ViewState["IssueOrderid"] + "";
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);
        con.Close();
        con.Dispose();
        #endregion
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        CHECKVALIDCONTROL();
        ChkDuplicateData();
        if (LblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[37];
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
                _arrpara[23] = new SqlParameter("@Freight", SqlDbType.Int);
                _arrpara[24] = new SqlParameter("@Insurance", SqlDbType.Int);
                _arrpara[25] = new SqlParameter("@PaymentAt", SqlDbType.Int);
                _arrpara[26] = new SqlParameter("@Destination", SqlDbType.NVarChar, 100);
                _arrpara[27] = new SqlParameter("@Liasoning", SqlDbType.NVarChar, 50);
                _arrpara[28] = new SqlParameter("@Inspection", SqlDbType.NVarChar, 50);
                _arrpara[29] = new SqlParameter("@SampleNumber", SqlDbType.NVarChar, 100);
                _arrpara[30] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
                _arrpara[31] = new SqlParameter("@Approvalflag", SqlDbType.Bit);
                _arrpara[32] = new SqlParameter("@estimatedWeight", SqlDbType.Float);
                _arrpara[33] = new SqlParameter("@Purchasefolio", SqlDbType.TinyInt);
                _arrpara[34] = new SqlParameter("@ExportSizeflag", SqlDbType.TinyInt);
                _arrpara[35] = new SqlParameter("@QualityGrmPerMeterMinus", SqlDbType.Float);
                _arrpara[36] = new SqlParameter("@QualityGrmPerMeterPlus", SqlDbType.Float);

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
                if (Session["varcompanyid"].ToString() == "9")
                {
                    _arrpara[11].Value = Convert.ToDouble(TxtLength.Text);
                    _arrpara[12].Value = Convert.ToDouble(TxtWidth.Text);
                }
                else
                {
                    //_arrpara[11].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text == "" ? "0" : TxtLength.Text));
                    //_arrpara[12].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text == "" ? "0" : TxtWidth.Text));

                    _arrpara[11].Value = string.Format("{0:#0.00}", TxtLength.Text == "" ? "0" : TxtLength.Text);
                    _arrpara[12].Value = string.Format("{0:#0.00}", TxtWidth.Text == "" ? "0" : TxtWidth.Text);
                }
                _arrpara[13].Value = TxtArea.Text == "" ? "0" : TxtArea.Text;
                _arrpara[14].Value = TxtRate.Text == "" ? "0" : TxtRate.Text;
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
                _arrpara[21].Value = DDCustomerOrderNumber.SelectedValue;
                _arrpara[22].Value = DDcaltype.SelectedValue;
                _arrpara[23].Value = 0;
                _arrpara[24].Value = 0;
                _arrpara[25].Value = 0;
                _arrpara[26].Value = "";
                _arrpara[27].Value = "";
                _arrpara[28].Value = "";
                _arrpara[29].Value = "";
                _arrpara[30].Value = ChkForFix.Checked == true ? 0 : 1;
                _arrpara[31].Value = ChkPPApproval.Checked == true ? 1 : 0;
                _arrpara[32].Value = txtWeight.Text == "" ? "0" : txtWeight.Text;
                _arrpara[33].Value = chkpurchasefolio.Checked == true ? "1" : "0";
                _arrpara[34].Value = chkexportsize.Checked == true ? "1" : "0";
                _arrpara[35].Value = TxtQualityGrmPerMeterMinus.Text == "" ? "0" : TxtQualityGrmPerMeterMinus.Text;
                _arrpara[36].Value = TxtQualityGrmPerMeterPlus.Text == "" ? "0" : TxtQualityGrmPerMeterPlus.Text;

                //Session["processId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || DDProcessName.SelectedValue != Convert.ToString(ViewState["ProcessId"]))
                {
                    int a = 0;
                    string str = "";
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
                            _arrpara[0].Value = (ViewState["IssueOrderid"]);
                            break;
                        default:
                            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                            ViewState["IssueOrderid"] = a;
                            _arrpara[0].Value = (ViewState["IssueOrderid"]);
                            str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                            break;
                    }
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
                    //    _arrpara[0].Value = (ViewState["IssueOrderid"]);
                    //}
                    //else
                    //{
                    //    a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                    //    ViewState["IssueOrderid"] = a;
                    //    _arrpara[0].Value = (ViewState["IssueOrderid"]);
                    //    str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    //}
                    #endregion

                    string ChallanNo = "";
                    if (variable.VarCompanyWiseChallanNoGenerated == "1")
                    {
                        SqlConnection con2 = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                        if (con2.State == ConnectionState.Closed)
                        {
                            con2.Open();
                        }
                        SqlTransaction tran2 = con2.BeginTransaction();
                        try
                        {
                            SqlParameter[] param = new SqlParameter[6];
                            param[0] = new SqlParameter("@TranType", SqlDbType.Int);
                            param[1] = new SqlParameter("@IssueRecNo", SqlDbType.VarChar, 100);
                            param[2] = new SqlParameter("@TableName", SqlDbType.VarChar, 300);
                            param[3] = new SqlParameter("@TableId", SqlDbType.Int);
                            param[4] = new SqlParameter("@CompanyId", SqlDbType.Int);
                            param[5] = new SqlParameter("@VarCompanyId", SqlDbType.Int);

                            param[0].Value = 0;
                            param[1].Direction = ParameterDirection.Output;
                            param[2].Value = "Process_Issue_Master_" + DDProcessName.SelectedValue;
                            param[3].Value = ViewState["IssueOrderid"];
                            param[4].Value = DDCompanyName.SelectedValue;
                            param[5].Value = Session["varCompanyNo"];

                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GatePassNew", param);

                            ChallanNo = param[1].Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            tran2.Rollback();
                            LblErrorMessage.Text = ex.Message;
                            LblErrorMessage.Visible = true;
                        }
                        finally
                        {
                            con2.Close();
                            con2.Dispose();
                        }

                    }
                    else
                    {
                        ChallanNo = ViewState["IssueOrderid"].ToString();
                    }

                    str = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,Freight,Insurance,PaymentAt,Destination,Liasoning,Inspection,SampleNumber,FlagFixOrWeight,Purchaseflag,FolioStatus,ChallanNo) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "',N'" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + "," + _arrpara[24].Value + "," + _arrpara[25].Value + ",'" + _arrpara[26].Value + "','" + _arrpara[27].Value + "','" + _arrpara[28].Value + "','" + _arrpara[29].Value + "'," + _arrpara[30].Value + ",'" + _arrpara[33].Value + "',0,'" + ChallanNo + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    //TxtChallanNo.Text = ViewState["IssueOrderid"].ToString();
                    TxtChallanNo.Text = ChallanNo;
                }
                string str1 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,RejectQty,CancelQty,Approvalflag,estimatedweight,ExportSizeflag,QualityGrmPerMeterMinus,QualityGrmPerMeterPlus) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ",0,0," + _arrpara[31].Value + "," + _arrpara[32].Value + "," + _arrpara[34].Value + "," + _arrpara[35].Value + "," + _arrpara[36].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[21].Value), Tran, effectivedate: TxtAssignDate.Text);
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update  JobAssigns set prod_status=1 where PreProdAssignedQty<>0 and orderid=" + _arrpara[21].Value + "");
                Tran.Commit();
                hnIssueOrderId.Value = ViewState["IssueOrderid"].ToString();
                btnLoomDetail.Visible = true;
                int VarFinishedidNew = Convert.ToInt32(DDDescription.SelectedValue);
                Fill_Description();
                if (DDDescription.Items.Count == 0)
                {
                    FillCategorySelectedChange();
                }
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Successfully Saved.......";
                Fill_Grid();
                //ProcessReportPath();
                Save_Refresh();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
                Tran.Rollback();
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
                ViewState["IssueOrderid"] = 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    //**********************************************************************************************************
    private void ChkDuplicateData()
    {
        DataSet Ds;
        if (Session["VarCompanyNo"].ToString() == "40" || Session["VarCompanyNo"].ToString() == "30" || Session["VarCompanyNo"].ToString() == "38")
        {
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select issueorderid from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " And Item_Finished_id=" + DDDescription.SelectedValue + " and OrderId=" + DDCustomerOrderNumber.SelectedValue + " ");
        }
        else
        {
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select issueorderid from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " And Item_Finished_id=" + DDDescription.SelectedValue + "");
        }


        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtQtyRequired.Text = "";
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Duplicate Data Exists.....";
            TxtQtyRequired.Focus();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (Tddropdowncustcode.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
            {
                goto a;
            }
        }
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
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) +  Pd.Width+'x'+PD.Length Description,Length,Width,
                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + Pd.Width+'x'+PD.Length Description,Length,Width,
                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
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
        txtWeight.Text = "";
        TxtQtyRequired.Text = "";
        TxtCommission.Text = "";
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
            string sql = @"select IM.CATEGORY_ID,IPM.ITEM_ID,PID.ORDER_QTY,PID.REQUIRED_DATE,IPM.Item_Finished_Id,PID.Area,PID.RATE,PID.REMARKS,PID.Instructions
                          from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " AS PID Inner Join ITEM_PARAMETER_MASTER as IPM ON PID.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID Inner Join ITEM_MASTER AS IM ON IPM.ITEM_ID = IM.ITEM_ID where Issue_Detail_Id=" + DGOrderdetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            UtilityModule.ConditionalComboFill(ref DDCategoryName, "SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId where OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select---");
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDescription, " select JA.Item_Finished_Id,Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join Item_Parameter_Master IPM On IPM.Item_Finished_Id=JA.Item_Finished_Id where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void fill_QuantityRequired()
    {
        int VarTotalQty = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            TxtTotalQty.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select  SUm(PreProdAssignedQty) as PreProdAssignedQty from JobAssigns Where OrderId=" + DDCustomerOrderNumber.SelectedValue + " And Item_Finished_ID=" + DDDescription.SelectedValue));
            if (TxtTotalQty.Text != "")
            {
                VarTotalQty = Convert.ToInt32(TxtTotalQty.Text);
            }
            if (Session["varcompanyid"].ToString() == "9")
            {
                TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-IsNull(Sum(PID.RejectQty),0) PQty From v_ProcessOrderQty PID  Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue)));
            }
            else
            {
                if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                {
                    if (Session["varcompanyid"].ToString() == "27")
                    {
                        TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PIM.EMPID<>0 and PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                        TxtQtyRequired.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PIM.EMPID<>0 and PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                    }
                    else
                    {
                        TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PIM.EMPID<>0 and PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                    }

                }
                else
                {
                    if (Session["varcompanyid"].ToString() == "27")
                    {
                        TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                        TxtQtyRequired.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                    }
                    else
                    {

                        TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status<>'Canceled'")));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
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
        TxtRemarks.Text = "";
        TxtLength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        TxtTotalQty.Text = "";
        TxtPreQuantity.Text = "";
        TxtInstructions.Text = "";
        fill_QuantityRequired();
        Area();
        MASTER_RATE();
        //on 11-04-2015 
        //if (DDDescription.SelectedIndex > 0)
        //{
        //    TxtRemarks.Text = (SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Isnull(WeavingInstruction,'') from OrderDetail Where Orderid=" + DDCustomerOrderNumber.SelectedValue + " And Item_Finished_Id=" + DDDescription.SelectedValue + "")).ToString();
        //}
        //TxtCommission.Text = UtilityModule.Fill_Comm(Convert.ToInt32(DDDescription.SelectedValue)).ToString();
        //End
        Fill_Remark_Commission_Instruction();
    }
    private void MASTER_RATE()
    {
        TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1, orderunitid: Convert.ToInt16(DDunit.SelectedValue)).ToString();
    }
    protected void TxtQtyRequired_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        switch (Session["varcompanyNo"].ToString())
        {
            case "5":

                break;
            default:
                if (TxtQtyRequired.Text != "")
                {
                    if (Convert.ToDouble(TxtQtyRequired.Text) > 0 && Convert.ToDouble(TxtQtyRequired.Text) <= Convert.ToDouble(TxtPreQuantity.Text))
                    {
                        LblErrorMessage.Visible = false;
                        BtnSave.Focus();
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
        TxtQtyRequired.Focus();
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
            if (variable.VarWEAVERPURCHASEORDER_FULLAREA == "1" && chkpurchasefolio.Checked == true)
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
                    //if (variable.VarNewQualitySize == "1")
                    //{
                    //    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetailNew Where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                    //}
                    //else
                    //{
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                    //}
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
        //ProcessReportPath();
        if (Convert.ToInt32(Session["varCompanyId"]) == 4)
        {
            ProcessReportPath();
            Session["ReportPath"] = "Reports/ProductionOrderForDeepak.rpt";
            Session["CommanFormula"] = "{View_Production_Issue_Order.IssueOrderid}=" + ViewState["IssueOrderid"];
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            if (Session["varcompanyid"].ToString() == "16")
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
        // string qry = "";
        // string str = "";

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
            //array[1].Value = ViewState["ProcessId"];
            //array[2].Value = Session["varcompanyId"];
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
                else if (Convert.ToInt32(Session["VarcompanyId"]) == 27)//For Antique Panipat
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
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
                UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_Id", true, "--SELECT--");
                Str = "Select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,OrderDetail OD where IPM.ITEM_ID=IM.ITEM_ID and OD.Item_Finished_id=IPM.Item_Finished_id and OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and ProductCode='" + TxtProductCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Some Importent fields Missing...........";
        }
    }
    private void OUT_CATEGORY_DEPENDS_CONTROLS()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + DDCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
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
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "1")
        {
            hnempid = DDEmployeeName.SelectedValue;
        }

        if (variable.VarMANYCUSTOMERORDERISSUE_SINGLEPRODUCTIONORDERNO == "0")
        {
            ViewState["IssueOrderid"] = 0;
        }
        //ViewState["IssueOrderid"] = 0;
        Fill_Grid();
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
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
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "");
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "");
            //                Tran.Commit();
            //                Fill_Grid();
            //                //ProcessReportPath();
            //                LblErrorMessage.Visible = true;
            //                LblErrorMessage.Text = "Successfully Deleted...";
            //                ViewState["IssueOrderid"] = "0";
            //            }
            #endregion
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssue.aspx");
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
    protected void DDunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDescription.SelectedIndex > 0)
        {
            Fill_After_DDDescription();
        }
    }
    protected void BtnSendSms_Click(object sender, EventArgs e)
    {
        try
        {
            BtnSendSms.Enabled = false;

            if (hnIssueOrderId.Value != "" || hnIssueOrderId.Value != "0")
            {
                UtilityModule.SendmessageToWeaver_Vendor_Finisher(MasterTableName: "Process_Issue_Master_" + DDProcessName.SelectedValue + "", DetailTable: "Process_issue_Detail_" + DDProcessName.SelectedValue + "", UniqueColName: "IssueOrderId", EmpIdColName: "Empid", OrderId: Convert.ToInt64(hnIssueOrderId.Value), OrderNo: "IssueOrderId", MasterCompanyId: Convert.ToInt16(Session["varcompanyId"]), FinishedidColName: "Item_Finished_id", QtyCOlName: "Qty", ReqByDate: "Reqbydate", JobName: "" + DDProcessName.SelectedItem.Text + "", UnitName: "Pcs");
            }
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Message Sent Successfully";
            BtnSendSms.Enabled = true;
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }

    }


    protected void chkforsms_CheckedChanged(object sender, EventArgs e)
    {
        if (chkforsms.Checked == true)
        {
            BtnSendSms.Visible = true;

        }
        else
        {
            BtnSendSms.Visible = false;
        }

    }
    protected string Getinstruction(int item_Finished_id)
    {
        string instructions = "";
        string str = @"select Q.Instruction from ITEM_PARAMETER_MASTER IM inner join Quality Q on Im.QUALITY_ID=Q.QualityId 
                    where im.ITEM_FINISHED_ID=" + item_Finished_id + "";
        instructions = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str));
        return instructions;
    }
    protected void Fill_Remark_Commission_Instruction()
    {
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@orderid", DDCustomerOrderNumber.SelectedValue);
        param[1] = new SqlParameter("@itemfinishedid", DDDescription.SelectedValue);
        param[2] = new SqlParameter("@remark", SqlDbType.NVarChar, 1000);
        param[3] = new SqlParameter("@instruction", SqlDbType.NVarChar, 8000);
        param[4] = new SqlParameter("@Commission", SqlDbType.Float);
        param[5] = new SqlParameter("@effectivedate", TxtAssignDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtAssignDate.Text);
        param[6] = new SqlParameter("@TypeId", 1);
        //output parameter
        param[2].Direction = ParameterDirection.Output;
        param[3].Direction = ParameterDirection.Output;
        param[4].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_FillRemark_Commission_Instruction", param);

        TxtRemarks.Text = param[2].Value.ToString();
        TxtInstructions.Text = param[3].Value.ToString();
        TxtCommission.Text = param[4].Value.ToString();
    }
    protected void txtlocalorerNo_TextChanged(object sender, EventArgs e)
    {
        if (txtlocalorerNo.Text != "")
        {
            string str = "select CustomerId,OrderId from ordermaster Where CompanyID = " + DDCompanyName.SelectedValue + " And localorder='" + txtlocalorerNo.Text + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                DDCustomerCode_SelectedIndexChanged(sender, e);
                DDCustomerOrderNumber.SelectedValue = ds.Tables[0].Rows[0]["orderid"].ToString();
                DDCustomerOrderNumber_SelectedIndexChanged(sender, e);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please enter correct Sr No..');", true);
                txtlocalorerNo.Focus();
            }
        }
    }

    protected void refreshEmployee_Click(object sender, EventArgs e)
    {
        DDProcessNameSelectedIndex();
    }
    protected void chkexportsize_CheckedChanged(object sender, EventArgs e)
    {
        Fill_Description();
    }
    protected void chkpurchasefolio_CheckedChanged(object sender, EventArgs e)
    {
        Fill_Description();
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
            sht.Cell("A1").Value = "Production Order (" + ds.Tables[0].Rows[0]["IssueOrderId"] + ")";
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

            sht.Cell("A3").Value = "Address. " + ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"] + "";
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

            sht.Cell("A6").Value = "Phone No:" + " " + ds.Tables[0].Rows[0]["CompTel"];
            sht.Range("A6:C6").Style.Font.FontName = "Times New Roman";
            sht.Range("A6:C6").Style.Font.FontSize = 11;
            sht.Range("A6:C6").Style.Font.Bold = true;
            sht.Range("A6:C6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A6:C6").Merge();

            using (var a = sht.Range("A6:C6"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Cell("A7").Value = "GSTIN No:" + " " + ds.Tables[0].Rows[0]["GSTNo"];
            sht.Range("A7:C7").Style.Font.FontName = "Times New Roman";
            sht.Range("A7:C7").Style.Font.FontSize = 11;
            sht.Range("A7:C7").Style.Font.Bold = true;
            sht.Range("A7:C7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A7:C7").Merge();

            using (var a = sht.Range("A7:C7"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }


            sht.Cell("D2").Value = "Emp Name: " + " " + ds.Tables[0].Rows[0]["EmpName"];
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


            sht.Cell("D5").Value = "Phone No" + " " + ds.Tables[0].Rows[0]["PhoneNo"];
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

            sht.Cell("D9").Value = "PO No" + " " + ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
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

            sht.Cell("G10").Value = "Customer Code:" + " " + ds.Tables[0].Rows[0]["CustomerCode"].ToString();
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
            sht.Range("A" + row).Style.Font.FontSize = 12;
            sht.Range("A" + row).Style.Font.SetBold();
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
                DataTable dtdistinctFinishedId = ds.Tables[1].DefaultView.ToTable(true, "Item_Name", "Quality", "Design", "Color", "ShadeColor", "FinishedId");
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


            sht.Range("A" + row + ":I" + (row + 1)).Style.Font.FontName = "Times New Roman";
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
            sht.Range("E" + row + ":F" + row).Style.Font.FontName = "Times New Roman";
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