using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
//using System.Windows.Forms;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;

public partial class Masters_ProcessIssue_ProcessIssueNew : System.Web.UI.Page
{
    private DataSet DS;
    static int MasterCompanyId;
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
                Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varCompanyId"] + @" order by customercode
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
                if (hncomp.Value == "6")
                    DDunit.SelectedValue = "1";
                else
                    DDunit.SelectedIndex = 2;
                ViewState["IssueOrderid"] = 0;
                lablechange();

                BindProcess();


            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid And CI.MasterCompanyId=" + Session["varCompanyId"] + " order by CustomerId", true, "--Select--");
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        // UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, "Select Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.Customerid=" + DDCustomerCode.SelectedValue + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " order by OM.OrderId", true, "--Select--");

        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@Customerid", DDCustomerCode.SelectedValue);
        //param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //param[2].Direction = ParameterDirection.Output;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_BindDropDownCustomerOrderNo", param);
        //if (param[2].Value.ToString() != "")
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "checkecisno", "alert('" + param[2].Value.ToString() + "')", true);
        //    return;
        //}
        //fill        
        if (ds.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerOrderNumber, ds, 0, true, "--Select--");
        }

    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();

        string Str = " select  Top(1) orderunitid from orderdetail Where orderid=" + DDCustomerOrderNumber.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DDunit.SelectedValue = ds.Tables[0].Rows[0]["OrderUnitid"].ToString();
        }
    }
    protected void BindProcess()
    {
        string Str = @"SELECT PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
        // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        hncomp.Value = Convert.ToString(Session["varCompanyId"]);
        switch (hncomp.Value)
        {
            case "6":
            case "10":
                break;
            case "9":
                Str = Str + " And PROCESS_NAME_ID in(1,16)";
                break;
            default:
                Str = Str + " And PROCESS_NAME_ID=1";
                break;
        }

        //Str = Str + " select  Top(1) orderunitid from orderdetail Where orderid=" + DDCustomerOrderNumber.SelectedValue;
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

        //if (ds.Tables[1].Rows.Count > 0)
        //{
        //    DDunit.SelectedValue = ds.Tables[1].Rows[0]["OrderUnitid"].ToString();
        //}

    }
    protected void BindCategory()
    {
        if (DDCustomerOrderNumber.SelectedIndex > 0)
        {
            DataSet ds = new DataSet();
            string str;
            switch (Session["varcompanyid"].ToString())
            {
                case "9":
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
                    }
                    else
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
                    }
                    break;
                default:
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0)-Isnull(Sum(CancelQty),0))";
                    }
                    else
                    {
                        str = @"SELECT DISTINCT VF.CATEGORY_ID,VF.CATEGORY_NAME  FROM OrderMaster OM,OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF 
                     Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And  JA.OrderId=OD.OrderId AND OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                     JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + "  Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0)-Isnull(Sum(CancelQty),0))";
                    }
                    break;
            }
            //str = str + " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid  And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by EI.Empname";
            //str = str + " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP, Department DP Where EI.Empid=EP.Empid and EI.DepartmentId=DP.DepartmentId And DP.DepartmentName='Production' And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + "order by EI.Empname";
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryName, ds, 0, true, "--Select--");

            if (DDCategoryName.Items.Count > 0)
            {
                DDCategoryName.SelectedIndex = 1;
                FillCategorySelectedChange();
            }
            else
            {
                DDCategoryName.Items.Clear();
                DDItemName.Items.Clear();
                DDDescription.Items.Clear();
            }
            //if (hncomp.Value == "6" || hncomp.Value == "10")
            //    UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 1, true, "--Select--");
            //else
            //    UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Select--");


        }
    }


    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DDProcessNameSelectedIndex();
    }
    protected void DDProcessNameSelectedIndex()
    {
        //ViewState["IssueOrderid"] = 0;
        //if (DDCustomerOrderNumber.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0)
        if (DDProcessName.SelectedIndex > 0)
        {
            DataSet ds = new DataSet();
            string str;

            str = " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid  And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by EI.Empname";
            str = str + " Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP, Department DP Where EI.Empid=EP.Empid and EI.DepartmentId=DP.DepartmentId And DP.DepartmentName='Production' And EP.Processid=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by EI.Empname";
            ds = SqlHelper.ExecuteDataset(str);

            if (hncomp.Value == "6" || hncomp.Value == "10")
                UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "--Select--");
            else
                UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 1, true, "--Select--");
            if (hncomp.Value != "20")
            {
                if (DDEmployeeName.Items.Count > 0)
                {
                    DDEmployeeName.SelectedIndex = 1;
                }
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
        if (Session["varcompanyId"].ToString() == "9")
        {
            if (variable.VarNewQualitySize == "1")
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
            }
            else
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetailNew VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0)-Isnull(Sum(CancelQty),0))";
            }
            else
            {
                STR = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM OrderDetail OD,JobAssigns JA,V_FinishedItemDetail VF Where JA.OrderId=OD.OrderId AND 
                      OD.Item_Finished_Id=JA.Item_Finished_Id AND OD.Item_Finished_Id=VF.Item_Finished_Id AND VF.CATEGORY_ID=" + DDCategoryName.SelectedValue + " AND VF.MasterCompanyId=" + Session["varCompanyId"] + @" AND 
                      OD.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0)-Isnull(Sum(CancelQty),0))";
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
        if (Session["varcompanyId"].ToString() == "9") //Hafizia
        {
            if (variable.VarNewQualitySize == "1")
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(Sizeinch,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else Isnull(ProdSizeFt,'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
            }
            else
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 6=" + DDunit.SelectedValue + " Then Isnull(Sizeinch,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else Isnull(ProdSizeFt,'') End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     v_ProcessOrderQty PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0))";
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(ProdSizeFt,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else '' End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSSNew IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>(Select IsNull(Sum(PID.Qty),0)-isnull(Sum(PID.CancelQty),0) PQty from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid=" + DDCustomerOrderNumber.SelectedValue + " and PID.Item_Finished_Id=JA.ITEM_FINISHED_ID and PIM.Status!='Canceled') ) order by Description";
            }
            else
            {
                STR = "Select distinct JA.Item_Finished_Id,QDCS+Space(5)+ Case When 2=" + DDunit.SelectedValue + " Then Isnull(ProdSizeFt,'') Else Case When 1=" + DDunit.SelectedValue + @" Then Isnull(ProdSizeMtr,'') Else '' End End Description 
                     From JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join ViewFindFinishedidItemidQDCSS IPM On 
                     IPM.FinishedId=JA.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=IPM.Item_Id Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + @" And 
                     IPM.Item_Id=" + DDItemName.SelectedValue + @" AND JA.Item_Finished_Id in (Select JA.Item_Finished_Id from JobAssigns JA LEFT OUTER JOIN 
                     PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.OrderId=JA.OrderId And PD.Item_Finished_Id=JA.Item_Finished_Id 
                     Where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " Group By JA.Item_Finished_Id,PreProdAssignedQty Having PreProdAssignedQty>IsNull(Sum(Qty),0)-Isnull(Sum(CancelQty),0)) order by Description";
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
                    SqlParameter[] _arrpara = new SqlParameter[9];
                    _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
                    _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@Shapeid", SqlDbType.Int);
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

                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_AreaNew", _arrpara);
                    if (Session["varcompanyid"].ToString() == "9")
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
                    // TxtArea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);

                    if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                    {
                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"])));
                    }
                    if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                    {
                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(_arrpara[7].Value.ToString()), Convert.ToDouble(_arrpara[8].Value.ToString()), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[0]["SHAPE_ID"]), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
                    }


                }
                else if (SizeId != 0 && hncomp.Value == "6")
                {
                    //datatset dt1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "");
                    string str = "";
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = "select ExpWidthMS_Ft as WidthFt,ExpLengthMS_Ft as LengthFt, 0 as HeightFt,LEFT(MtrSize, CHARINDEX('x', MtrSize) - 1) AS WidthMtr,REPLACE(SUBSTRING(MtrSize, CHARINDEX('x', MtrSize), LEN(MtrSize)), 'x', '') as LengthMtr,0 as HeightMtr,Export_Area as AreaFt,MtrArea as AreaMtr,cast(KhapWidth as varchar) +'x'+cast (KhapLength as varchar) as Khap from QualitySizeNew where sizeid=" + SizeId + "";
                    }
                    else
                    {
                        str = "select WidthFt,LengthFt,HeightFt,WidthMtr,LengthMtr,HeightMtr,AreaFt,AreaMtr,0 as Khap from size where sizeid=" + SizeId + " And MasterCompanyId=" + Session["varCompanyId"];
                    }
                    DS = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
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
                    LblArea.Visible = false;
                    TdArea.Visible = false;
                    TxtArea.Text = "0";
                }
                //datatset dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
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
        if (Session["varcompanyId"].ToString() == "20")
        {
            ChkItemFinishedId();
        }
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

                _arrpara[33] = new SqlParameter("@Khap", SqlDbType.VarChar, 100);
                _arrpara[34] = new SqlParameter("@Consump", SqlDbType.Float);
                _arrpara[35] = new SqlParameter("@HSCode", SqlDbType.VarChar, 100);
                _arrpara[36] = new SqlParameter("@AfterKhapSizeOrder", SqlDbType.VarChar, 50);

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
                    _arrpara[11].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text == "" ? "0" : TxtLength.Text));
                    _arrpara[12].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text == "" ? "0" : TxtWidth.Text));
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

                _arrpara[33].Value = TxtKhap.Text == "" ? "0" : TxtKhap.Text;
                _arrpara[34].Value = TxtConsump.Text == "" ? "0" : TxtConsump.Text;
                _arrpara[35].Value = TxtHSCode.Text;
                _arrpara[36].Value = txtAfterKhapSizeOrder.Text;

                //Session["processId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || DDProcessName.SelectedValue != Convert.ToString(ViewState["ProcessId"]))
                {
                    int a = 0;
                    string str = "";
                    if (Session["varcompanyId"].ToString() == "9")
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
                        _arrpara[0].Value = (ViewState["IssueOrderid"]);
                    }
                    else
                    {
                        //a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                        a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from process_issue_master_" + DDProcessName.SelectedValue + ""));
                        ViewState["IssueOrderid"] = a;
                        _arrpara[0].Value = (ViewState["IssueOrderid"]);
                        str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    }
                    str = @"Insert Into Process_Issue_Master_" + DDProcessName.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,Freight,Insurance,PaymentAt,Destination,Liasoning,Inspection,SampleNumber,FlagFixOrWeight,HSCode) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "',N'" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + "," + _arrpara[24].Value + "," + _arrpara[25].Value + ",'" + _arrpara[26].Value + "','" + _arrpara[27].Value + "','" + _arrpara[28].Value + "','" + _arrpara[29].Value + "'," + _arrpara[30].Value + ",'" + _arrpara[35].Value + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    TxtChallanNo.Text = ViewState["IssueOrderid"].ToString();
                }
                string str1 = @"Insert Into Process_Issue_Detail_" + DDProcessName.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,RejectQty,CancelQty,Approvalflag,estimatedweight,Khap,Consump,AfterKhapSizeOrder) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ",0,0," + _arrpara[31].Value + "," + _arrpara[32].Value + ",'" + _arrpara[33].Value + "'," + _arrpara[34].Value + ",'" + _arrpara[36].Value + "')";
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
                UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item, IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Description,Length,Width,
                        Width + 'x' + Length Size,Area,Qty*Area as TArea,Rate,Qty,Amount,PD.Item_Finished_Id,ISNULL(IPM.Quality, '') as Quality,ISNULL(IPM.Qualityid, '') as Qualityid, 
                        ISNULL(IPM.Designid, '') as Designid, ISNULL(IPM.Design, '') as Design,Comm,CommAmt,Khap,Consump,ISNULL((Amount+CommAmt),0) as GTotal,IPM.Color,IPM.Shape,
                        Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Size,isnull(CancelQty,0) as CancelQty 
                        From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id asc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Description,Length,Width,
                        Width + 'x' + Length Size,Area, Qty*Area as TArea,Rate,Qty,Amount,PD.Item_Finished_Id From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
            string sql = @"select IM.CATEGORY_ID,IPM.ITEM_ID,PID.ORDER_QTY,PID.REQUIRED_DATE,IPM.Item_Finished_Id,PID.Area,PID.RATE,PID.REMARKS,PID.Instructions,PID.HSCode
                          from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " AS PID Inner Join ITEM_PARAMETER_MASTER as IPM ON PID.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID Inner Join ITEM_MASTER AS IM ON IPM.ITEM_ID = IM.ITEM_ID where Issue_Detail_Id=" + DGOrderdetail.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            UtilityModule.ConditionalComboFill(ref DDCategoryName, "SELECT Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_MASTER IM INNER JOIN ITEM_CATEGORY_MASTER ICM ON IM.CATEGORY_ID = ICM.CATEGORY_ID INNER JOIN OrderDetail OD ON IM.ITEM_ID = OD.ITEM_ID INNER JOIN OrderMaster OM ON OD.OrderId = OM.OrderId where OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDItemName, "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select---");
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDescription, "select JA.Item_Finished_Id,Description from JobAssigns JA inner join OrderDetail Od ON JA.Item_Finished_Id=Od.Item_Finished_Id inner join Item_Master IM ON IM.Item_Id=OD.Item_Id inner join Item_Parameter_Master IPM On IPM.Item_Finished_Id=JA.Item_Finished_Id where JA.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and OD.Item_Id=" + DDItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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

            TxtQtyRequired.Text = Convert.ToString(Convert.ToInt32(TxtPreQuantity.Text) - Convert.ToInt32(TxtQtyRequired.Text));

            TxtRemarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
            TxtInstructions.Text = ds.Tables[0].Rows[0]["Instructions"].ToString();
            TxtRate.Text = ds.Tables[0].Rows[0]["RATE"].ToString();
            TxtHSCode.Text = ds.Tables[0].Rows[0]["HSCode"].ToString();
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
            else if (Session["varcompanyid"].ToString() == "20")
            {
                TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status!='Canceled'")));
                TxtQtyRequired.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue + " and PIM.Status!='Canceled'")));
            }
            else
            {
                TxtPreQuantity.Text = Convert.ToString(VarTotalQty - Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select IsNull(Sum(PID.Qty),0)-Isnull(Sum(PID.CancelQty),0)-IsNull(Sum(PID.RejectQty),0) PQty From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " as PID Inner Join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " as PIM ON PID.IssueOrderid=PIM.IssueOrderid Where PID.Orderid='" + DDCustomerOrderNumber.SelectedValue + "' and PID.Item_Finished_Id=" + DDDescription.SelectedValue)));

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //protected void DGOrderdetail_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    FillDetailBack();
    //}

    decimal TotalQty = 0;
    decimal TotalArea = 0;
    decimal TotalWAmount = 0;
    decimal TotalComm = 0;
    decimal GTotal = 0;
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQty = (Label)e.Row.FindControl("hnlblQty");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
        TxtKhap.Text = "";
        TxtConsump.Text = "";
        TxtHSCode.Text = "";
        fill_QuantityRequired();
        Area();
        MASTER_RATE();
        MASTER_CONSUMPTION();
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
        TxtRate.Text = UtilityModule.PROCESS_RATE(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDCustomerOrderNumber.SelectedValue), Convert.ToInt32(DDProcessName.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1).ToString();
    }
    private void MASTER_CONSUMPTION()
    {
        TxtConsump.Text = UtilityModule.PROCESS_CONSUMPTION(Convert.ToInt32(DDDescription.SelectedValue), Convert.ToInt32(DDunit.SelectedValue), effectivedate: TxtAssignDate.Text, TypeId: 1).ToString();

    }
    protected void TxtQtyRequired_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        switch (Session["varcompanyNo"].ToString())
        {
            case "5":
            case "16":
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
                        LblErrorMessage.Text = "Quantity Required Must Be Integer and Greater than Zero and less then PQty.........................";
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
            if (DDDescription.SelectedIndex > 0)
            {
                if (variable.VarNewQualitySize == "1")
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetailNew Where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
                else
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
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
            Report();
        }
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[0].Value = ViewState["IssueOrderid"];
        array[1].Value = ViewState["ProcessId"];
        array[2].Value = Session["varcompanyId"];
        //if (Session["varcompanyId"].ToString() == "9")
        //{
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrderReport", array);


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
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\ProductionOrderNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
        ViewState["IssueOrderid"] = 0;

        Fill_Grid();
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            if (0 != Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Qty-PQty from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "")))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "You Have Received....";
            }
            else
            {
                if (DGOrderdetail.Rows.Count == 1)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Issue_Master_" + DDProcessName.SelectedValue + @" 
                Where IssueOrderID in (Select IssueOrderID from Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")");
                }
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete Process_Issue_Detail_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "");
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_CONSUMPTION_DETAIL Where IssueOrderID=" + ViewState["IssueOrderid"] + " And Issue_Detail_ID=" + VarProcess_Issue_Detail_Id + "");
                Tran.Commit();
                Fill_Grid();
                Fill_Description();
                //ProcessReportPath();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Successfully Deleted...";
                if (DGOrderdetail.Rows.Count == 0)
                {
                    ViewState["IssueOrderid"] = "0";
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
        Fill_Description();
    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;
        Fill_Grid();
        Fill_Description();
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        LblErrorMessage.Text = "";
        string lblIssueDetailId = ((Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblIssueDetailId")).Text;

        TextBox txtReqQtyEdit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtReqQtyEdit");
        Label lblComm = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblComm");
        Label lblArea = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblArea");
        Label lblRate = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblRate");
        Label lblCancelQty = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblCancelQty");
        Label lblItemFinishedId = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

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
                SqlParameter[] _arrpara = new SqlParameter[23];

                _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Float);
                _arrpara[2] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrpara[3] = new SqlParameter("@Qty", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@PQty", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@CommAmount", SqlDbType.Float);
                _arrpara[6] = new SqlParameter("@Processid", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                _arrpara[9] = new SqlParameter("@orderid", DDCustomerOrderNumber.SelectedValue);
                _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);



                _arrpara[0].Value = (ViewState["IssueOrderid"]);
                _arrpara[1].Value = lblIssueDetailId;

                Double TQty;
                TQty = Convert.ToDouble(txtReqQtyEdit.Text) - Convert.ToDouble(lblCancelQty.Text == "" ? "0" : lblCancelQty.Text);

                if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                {
                    _arrpara[2].Value = String.Format("{0:#0.00}", (Convert.ToDouble(lblArea.Text) * Convert.ToDouble(lblRate.Text) * TQty));
                    _arrpara[5].Value = String.Format("{0:#0.00}", (Convert.ToDouble(lblArea.Text) * Convert.ToDouble(lblComm.Text) * TQty));
                }
                if (DDcaltype.SelectedValue == "1")
                {
                    _arrpara[2].Value = String.Format("{0:#0.00}", (Convert.ToDouble(lblRate.Text) * TQty));
                    _arrpara[5].Value = String.Format("{0:#0.00}", (Convert.ToDouble(lblComm.Text) * TQty));
                }
                _arrpara[3].Value = txtReqQtyEdit.Text;
                _arrpara[4].Value = Convert.ToInt32(txtReqQtyEdit.Text) - Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Qty-PQty from Process_Issue_Detail_" + DDProcessName.SelectedValue + " where Issue_Detail_Id=" + lblIssueDetailId).ToString());
                _arrpara[6].Value = DDProcessName.SelectedValue;
                _arrpara[7].Value = Session["varcompanyid"];
                _arrpara[8].Direction = ParameterDirection.Output;
                _arrpara[10].Value = lblItemFinishedId.Text;
                ViewState["ProcessId"] = Convert.ToInt32(DDProcessName.SelectedValue);
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateProductionorderQty", _arrpara);

                if (_arrpara[8].Value.ToString() == "")
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Data Successfully Update.......";
                    Tran.Commit();
                    DGOrderdetail.EditIndex = -1;
                    Fill_Grid();
                    //BtnSave.Visible = true;
                    //BtnUpdate.Visible = false;
                    //Save_Refresh();
                }
                else
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[8].Value.ToString();
                    Tran.Commit();
                }

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessIssueNew.aspx");
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
    protected void txtlocalorerNo_TextChanged(object sender, EventArgs e)
    {
        if (txtlocalorerNo.Text != "")
        {
            string str = "select CustomerId,OrderId from ordermaster Where CompanyID = " + Session["CurrentWorkingCompanyID"] + " And localorder='" + txtlocalorerNo.Text + "'";
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
}