using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_PunchCardIndent_FrmPunchCardIndentReceive_OnProductionOrder : System.Web.UI.Page
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
                           SELECT Distinct CI.CustomerId,CI.CustomerCode CustomerCode From CustomerInfo CI,JobAssigns J,OrderMaster OM Where CI.Customerid=OM.Customerid And OM.Orderid=J.Orderid Order By CI.CustomerCode
                           select unitid,unitname from unit where unitid in (1,2,4,6,7)";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, true, "--Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }


            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");


            switch (Session["varcompanyId"].ToString())
            {
                case "30":

                    //FillFolioEmployee(sender);
                    break;
                default:
                    ;
                    break;
            }
            Fill_Temp_OrderNo();
            DDCustomerCode.Focus();

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }

            hnReceiveid.Value = "0";
            hnissueorderid.Value = "0";
            HnMissingID.Value = "0";
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
        //lblcategoryname.Text = ParameterList[5];
        //lblitemname.Text = ParameterList[6];
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

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();
    }
    private void CustomerCodeSelectedChange()
    {
        //SqlConnection con = new SqlConnection();
        string str = @"Select  Distinct OM.OrderId,OM.LocalOrder+' / '+OM.CustomerOrderNo from OrderMaster OM,JobAssigns J Where OM.Orderid=J.Orderid And OM.CompanyId=" + DDcompany.SelectedValue + " and Om.ORDERFROMSAMPLE=0 ";
        if (Tddropdowncustcode.Visible == true)
        {
            str = str + "  and OM.Customerid=" + DDCustomerCode.SelectedValue;
        }
        str = str + " order by OM.Orderid ";
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, str, true, "--Select--");
    }

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

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();
    }
    protected void DDProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ProcessSelectedChange();

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();
    }
    private void ProcessSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, "Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,PROCESS_ISSUE_Master_" + DDProcessName.SelectedValue + " PM Where PM.EmpId=Ei.EmpId and PM.Status<>'Canceled' Order By EI.EmpName", true, "--Select--");
        txtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        //TxtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();
    }
    private void EmployeeSelectedChange()
    {
        string str = "";
        if (variable.VarLoomNoGenerated == "1")
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM 
                    inner join PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD on Pm.issueorderid=Pd.issueorderid
                    Left join LoomstockNo ls on pm.issueorderid=Ls.issueorderid And ls.ProcessID = " + DDProcessName.SelectedValue + @"
                    where ls.issueorderid is null and  PM.Empid=" + DDEmployeeName.SelectedValue + " and PM.CompanyId=" + DDcompany.SelectedValue + @" 
                    and PD.OrderId=" + DDCustomerOrderNumber.SelectedValue + " and PM.Status<>'canceled' order by PM.Issueorderid";
        }
        else
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD 
                where PM.Empid=" + DDEmployeeName.SelectedValue + " and PM.CompanyId=" + DDcompany.SelectedValue + @" and PM.IssueOrderId=PD.IssueOrderId 
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
            string str = "SELECT Distinct OD.ITEM_ID,IM.ITEM_NAME FROM OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId = OD.OrderId INNER JOIN ITEM_MASTER IM ON OD.ITEM_ID = IM.ITEM_ID where IM.CATEGORY_ID=" + DDCategoryName.SelectedValue + " and OM.OrderId=" + DDCustomerOrderNumber.SelectedValue + "";
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
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        dditemname_chage();
    }
    private void dditemname_chage()
    {
        string size = "";
        if ((variable.VarWEAVERPURCHASEORDER_FULLAREA == "1"))
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
    protected void DDPOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        POrderNoSelectedChange();

        BindIssueNo();

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();

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
                //TxtRequiredDate.Text = DS.Tables[0].Rows[0]["ReqByDate"].ToString();
                //TxtAssignDate.Text = DS.Tables[0].Rows[0]["AssignDate"].ToString();
                //DDunit.SelectedValue = DS.Tables[0].Rows[0]["UnitId"].ToString();
                //DDcaltype.SelectedValue = DS.Tables[0].Rows[0]["CalType"].ToString();
                //TxtInstructions.Text = DS.Tables[0].Rows[0]["Instruction"].ToString();
                //TxtRemarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                //ViewState["IssueOrderid"] = DDPOrderNo.SelectedValue;
                //ViewState["ProcessId"] = DDProcessName.SelectedValue;
                //hnIssueOrderId.Value = DDPOrderNo.SelectedValue;
                //hnpurchasefolio.Value = DS.Tables[0].Rows[0]["Purchaseflag"].ToString();
                //if (DS.Tables[0].Rows[0]["FlagFixOrWeight"] == "0")
                //{
                //    ChkForFix.Checked = true;
                //}

            }
            else
            {
                ViewState["IssueOrderid"] = "0";
            }
            //Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/PunchCardIndent/FrmPunchCardIndentReceive_OnProductionOrder.aspx");
            lblmessage.Text = ex.Message;
            lblmessage.Visible = true;
        }
        finally
        {
            con.Close();
        }
    }
    protected void DDunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DDItemName.SelectedIndex = '0';
        if (DDDescription.SelectedIndex > 0)
        {
            DDDescription_SelectedIndexChanged(sender, e);
        }
    }
    private void BindIssueGridOnFolio()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@CompanyId", DDCategoryName.SelectedValue);
        param[1] = new SqlParameter("@PCIIssueId", DDIssueNo.SelectedValue);
        param[2] = new SqlParameter("@MSG", SqlDbType.VarChar, 200);
        param[2].Direction = ParameterDirection.Output;

        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIndentIssueOnFolioDetailForReceive", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }
    }
    private void BindIssueNo()
    {
        string str = @"select PCIIssueId,ChallanNo from PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER MIM
                       Where MIM.CompanyId=" + DDcompany.SelectedValue + " and MIM.folioIssueOrderId=" + DDPOrderNo.SelectedValue + @" 
                        and  MIM.ProcessID=" + DDProcessName.SelectedValue + @" ";

        UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "--Plz Select--");
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIssueGridOnFolio();

        if (chkEdit.Checked == true)
        {
            FillReceiveNo();
        }
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindIssueGridOnFolio();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        //gvdetail.DataSource = null;
        //gvdetail.DataBind();

        if (txtfolionoedit.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
            @"Select * from TEMP_PROCESS_ISSUE_MASTER_NEW Where Companyid = " + DDcompany.SelectedValue + " And ChallanNo = '" + txtfolionoedit.Text + "'");
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

                BindIssueNo();

                if (chkEdit.Checked == true)
                {
                    FillReceiveNo();
                }
            }
            else
            {
                txtfolionoedit.Text = "";
                txtfolionoedit.Focus();
                lblmessage.Visible = true;
                lblmessage.Text = "Pls Enter Correct Order No";
            }
        }
    }

    protected void FillReceiveNo()
    {
        string str = @"select PCIReceiveId,RecChallanNo from PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERMASTER MIM
                       Where MIM.CompanyId=" + DDcompany.SelectedValue + " and MIM.folioIssueOrderId=" + DDPOrderNo.SelectedValue + @" 
                        and  MIM.ProcessID=" + DDProcessName.SelectedValue + @" ";

        UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");

    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnReceiveid.Value = DDReceiveNo.SelectedValue;
        FillReceiveGrid();
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            TDReceiveNo.Visible = true;
            FillReceiveNo();
        }
        else
        {
            TDReceiveNo.Visible = false;
            DDReceiveNo.Items.Clear();
        }

        txtReceiveNo.Text = "";
        hnReceiveid.Value = "0";

        DG.DataSource = null;
        DG.DataBind();

        gvdetail.DataSource = null;
        gvdetail.DataBind();

    }
    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        //{
        //    goto a;
        //}   

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
        if (UtilityModule.VALIDDROPDOWNLIST(DDPOrderNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDEmployeeName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDIssueNo) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblmessage.Visible = true;
        UtilityModule.SHOWMSG(lblmessage);
    B: ;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        if (lblmessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

                Label lblNoOfSet = ((Label)DG.Rows[i].FindControl("lblNoOfSet"));
                Label lblPerSetQty = ((Label)DG.Rows[i].FindControl("lblPerSetQty"));
                Label lblStockNoSeries = ((Label)DG.Rows[i].FindControl("lblStockNoSeries"));

                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                Label lblPCIIssueId = ((Label)DG.Rows[i].FindControl("lblPCIIssueId"));
                Label lblPCIIssueDetailId = ((Label)DG.Rows[i].FindControl("lblPCIIssueDetailId"));
                Label lblPunchCardIndentType = ((Label)DG.Rows[i].FindControl("lblPunchCardIndentType"));
                Label lblSNSID = ((Label)DG.Rows[i].FindControl("lblSNSID"));

                //if (Chkboxitem.Checked == false)   // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
                //    return;
                //}

                if ((lblNoOfSet.Text == "") && Chkboxitem.Checked == true)   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('NoOf Set Qty can not be blank');", true);
                    //txtReceiveNoOfSet.Focus();
                    return;
                }
                if (Chkboxitem.Checked == true && (Convert.ToInt32(lblNoOfSet.Text) <= 0))   // Change when Updated Completed
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('NoOf Set Qty always greater then zero');", true);
                    //txtReceiveNoOfSet.Focus();
                    return;
                }
                //if (Convert.ToDecimal(txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text) > Convert.ToDecimal(lblBalToRecQty.Text) && Chkboxitem.Checked == true)   // Change when Updated Completed
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be greater than balance qty');", true);
                //    txtReceiveQty.Text = "";
                //    txtReceiveQty.Focus();
                //    return;
                //}
            }

            string Strdetail = "";
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

                Label lblNoOfSet = ((Label)DG.Rows[i].FindControl("lblNoOfSet"));
                Label lblPerSetQty = ((Label)DG.Rows[i].FindControl("lblPerSetQty"));
                Label lblStockNoSeries = ((Label)DG.Rows[i].FindControl("lblStockNoSeries"));

                Label lblItemFinishedId = ((Label)DG.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)DG.Rows[i].FindControl("lblOrderId"));
                Label lblPCIIssueId = ((Label)DG.Rows[i].FindControl("lblPCIIssueId"));
                Label lblPCIIssueDetailId = ((Label)DG.Rows[i].FindControl("lblPCIIssueDetailId"));
                Label lblPunchCardIndentType = ((Label)DG.Rows[i].FindControl("lblPunchCardIndentType"));
                Label lblSNSID = ((Label)DG.Rows[i].FindControl("lblSNSID"));


                if (Chkboxitem.Checked == true && DDCategoryName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && DDPOrderNo.SelectedIndex > 0 && DDIssueNo.SelectedIndex > 0)
                {
                    Strdetail = Strdetail + lblNoOfSet.Text + '|' + lblPerSetQty.Text + '|' + lblStockNoSeries.Text + '|' + lblItemFinishedId.Text + '|' + lblOrderId.Text + '|' + lblPCIIssueId.Text + '|' + lblPCIIssueDetailId.Text + '|' + lblPunchCardIndentType.Text + '|' + lblSNSID.Text + '~';
                }
            }


            if (Strdetail != "")
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    //        //******
                    SqlCommand cmd = new SqlCommand("Pro_SavePunchCardIndent_ReceiveFromProductionOrder", con, Tran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3000;
                    cmd.Parameters.Add("@PCIReceiveId", SqlDbType.Int);
                    cmd.Parameters["@PCIReceiveId"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@PCIReceiveId"].Value = hnReceiveid.Value;
                    cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
                    cmd.Parameters.AddWithValue("@FolioIssueOrderId", DDPOrderNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@EmpId", DDEmployeeName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Receivedate", txtReceiveDate.Text);
                    cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                    cmd.Parameters.AddWithValue("@StringDetail", Strdetail);
                    cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@MSStockno", txtMapStencilstockno.Text);
                    cmd.Parameters.Add("@RecChallanNo", SqlDbType.VarChar, 30);
                    cmd.Parameters["@RecChallanNo"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@RecChallanNo"].Value = "";
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                    {
                        lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                        Tran.Rollback();
                    }
                    else
                    {
                        lblmessage.Text = "Data Saved Successfully.";
                        Tran.Commit();
                        //txtfoliono.Text = cmd.Parameters["@FolioNo"].Value.ToString(); //param[5].Value.ToString();
                        hnReceiveid.Value = cmd.Parameters["@PCIReceiveid"].Value.ToString();// param[0].Value.ToString();
                        txtReceiveNo.Text = cmd.Parameters["@RecChallanNo"].Value.ToString();
                        BindIssueGridOnFolio();
                        FillReceiveGrid();
                    }
                    //******                    

                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    lblmessage.Text = ex.Message;
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
    }

    protected void FillReceiveGrid()
    {
        string str = @"select MIM.PCIReceiveId, MID.PCIReceiveDetailID,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                        CASE WHEN OD.OrderUnitId = 6 THEN VF.SizeInch else Case When OD.OrderUnitId = 1 THEN VF.SizeMtr ELSE VF.SizeFt END End As Size,
                        --CASE WHEN PCIGSNS.UnitID = 2 THEN VF.SIZEFT ELSE VF.SIZEMTR END As Size,
                        MID.PCIIssueDetailId,MIM.FolioIssueOrderId,MID.ItemFinishedID,MIM.CompanyId,MIM.RecChallanNo,
                        Replace(CONVERT(nvarchar(11),MIM.ReceiveDate,106),' ','-') as ReceiveDate,MID.RecNoOfSet,MID.RecPerSetQty,MId.TotalReceiveQty,
                        PCIGSNS.StockNoSeries,MID.SNSID,MID.PCIIssueId,MID.PCIIssueDetailId,isnull(MID.Rate,0) as Rate,
                        isnull(MID.Amount,0) as Amount,isnull(MID.NoOfMissingQty,0) as NoOfMissingQty
                        from PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERMASTER MIM(NoLock) 
                        JOIN PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERDETAIL MID(NoLock) ON MIM.PCIReceiveid=MID.PCIReceiveid 
                        JOIN V_FinishedItemDetail VF(NoLock) ON MID.ItemFinishedId=VF.Item_Finished_Id
                        JOIN OrderDetail OD(NoLock) ON MID.OrderId=OD.OrderID and MID.ItemFinishedId=OD.Item_Finished_id
                        JOIN PunchCardIndent_GenerateStockNoSeries PCIGSNS(NoLock) ON PCIGSNS.SNSID=MID.SNSID
                        where MIM.CompanyId=" + DDcompany.SelectedValue + " ";
        ////where MIM.ID=" + hnReceiveid.Value;
        //if (txtEditIssueNo.Text != "")
        //{
        //    str = str + " and MIM.ChallanNo='" + txtEditIssueNo.Text + "'";
        //}
        //else
        //{
        str = str + " and MIM.PCIReceiveId=" + hnReceiveid.Value + "";
        //}

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
            Label lblPCIReceiveId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblPCIReceiveId");
            Label lblPCIReceiveDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblPCIReceiveDetailId");
            Label lblSNSID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblSNSID");

            // Label lblQty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblQty");
            Label lblItemFinishedId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblItemFinishedId");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@PCIReceiveId", lblPCIReceiveId.Text);
            param[1] = new SqlParameter("@PCIReceiveDetailid", lblPCIReceiveDetailId.Text);
            param[2] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            param[3] = new SqlParameter("@SNSID", lblSNSID.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDER", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillReceiveGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@PCIReceiveID", hnReceiveid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIndentReceiveProductionOrderReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptPunchCardIndentReceiveProductionOrderReport.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPunchCardIndentReceiveProductionOrderReport.xsd";
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
    protected void lnkPCStockNo_Click(object sender, EventArgs e)
    {
        Modalpopupext.Show();
        ViewState["qcdetail"] = null;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblSNSID = (Label)gvdetail.Rows[grv.RowIndex].FindControl("lblSNSID");
            Label lblItemFinishedId = (Label)gvdetail.Rows[grv.RowIndex].FindControl("lblItemFinishedId");
            Label lblPCIReceiveId = (Label)gvdetail.Rows[grv.RowIndex].FindControl("lblPCIReceiveId");
            Label lblPCIReceiveDetailId = (Label)gvdetail.Rows[grv.RowIndex].FindControl("lblPCIReceiveDetailId");

            lblMissingPCIReceiveID.Text = lblPCIReceiveId.Text;
            lblMissingPCIReceiveDetailId.Text = lblPCIReceiveDetailId.Text;

            //**********get QC parameter
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@SNSID", lblSNSID.Text);
            param[1] = new SqlParameter("@ItemFinishedID", lblItemFinishedId.Text);
            param[2] = new SqlParameter("@PCIReceiveId", lblPCIReceiveId.Text);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPUNCHCARDGENERATEDSTOCKNO", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GVPunchCardStockNo.DataSource = ds;
                GVPunchCardStockNo.DataBind();
                GVPunchCardStockNo.Visible = true;

                string str2 = "";
                str2 = @"select SNID,1 as flag from PUNCHCARDINDENT_MISSINGSTOCKNO where PCIReceiveID=" + lblPCIReceiveId.Text + " and PCIReceiveDetailId=" + lblPCIReceiveDetailId.Text + "";
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                    {
                        int PId = Convert.ToInt32(ds2.Tables[0].Rows[j]["SNID"]);
                        foreach (GridViewRow row in GVPunchCardStockNo.Rows)
                        {
                            CheckBox Chkboxitem = row.FindControl("Chkboxitem") as CheckBox;
                            Label lblSNID = row.FindControl("lblSNID") as Label;
                            if (PId == Convert.ToInt32(lblSNID.Text))
                            {
                                Chkboxitem.Checked = true;
                            }
                        }
                    }
                }
            }
            else
            {
                GVPunchCardStockNo.DataSource = null;
                GVPunchCardStockNo.DataBind();

                GVPunchCardStockNo.Visible = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
            //*************



        }
    }
    protected void GVPunchCardStockNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVPunchCardStockNo, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnMissingPCStockNo_Click(object sender, EventArgs e)
    {
        string MissingStockNoDetailData = "";

        for (int i = 0; i < GVPunchCardStockNo.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GVPunchCardStockNo.Rows[i].FindControl("Chkboxitem"));

            Label lblSNID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblSNID"));
            Label lblSNSID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblSNSID"));
            Label lblPCStockNo = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCStockNo"));
            Label lblPCIReceiveID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCIReceiveID"));
            Label lblPCIReceiveDetailId = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCIReceiveDetailId"));
            TextBox txtRate = ((TextBox)GVPunchCardStockNo.Rows[i].FindControl("txtRate"));

            if (Chkboxitem.Checked == true && (txtRate.Text == "" || txtRate.Text == "0"))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Missing Rate can not be blank');", true);
                return;
            }


        }

        for (int i = 0; i < GVPunchCardStockNo.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GVPunchCardStockNo.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label lblSNID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblSNID"));
                Label lblSNSID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblSNSID"));
                Label lblPCStockNo = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCStockNo"));
                Label lblPCIReceiveID = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCIReceiveID"));
                Label lblPCIReceiveDetailId = ((Label)GVPunchCardStockNo.Rows[i].FindControl("lblPCIReceiveDetailId"));
                TextBox txtRate = ((TextBox)GVPunchCardStockNo.Rows[i].FindControl("txtRate"));

                if (MissingStockNoDetailData == "")
                {
                    MissingStockNoDetailData = lblSNID.Text + "|" + lblSNSID.Text + "|" + lblPCStockNo.Text + "|" + txtRate.Text + "~";
                }
                else
                {
                    MissingStockNoDetailData = MissingStockNoDetailData + lblSNID.Text + "|" + lblSNSID.Text + "|" + lblPCStockNo.Text + "|" + txtRate.Text + "~";
                }
            }
        }

        if (MissingStockNoDetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[7];
            arr[0] = new SqlParameter("@PCIMissingId", SqlDbType.Int);
            arr[1] = new SqlParameter("@PCIReceiveId", SqlDbType.Int);
            arr[2] = new SqlParameter("@PCIReceiveDetailId", SqlDbType.Int);
            arr[3] = new SqlParameter("@MissingStockNoDetailData", SqlDbType.NVarChar);
            arr[4] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);


            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnMissingID.Value;
            arr[1].Value = Convert.ToInt32(lblMissingPCIReceiveID.Text);
            arr[2].Value = Convert.ToInt32(lblMissingPCIReceiveDetailId.Text);
            arr[3].Value = MissingStockNoDetailData;
            arr[4].Value = Session["varuserid"];
            arr[5].Value = Session["varCompanyId"];
            arr[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SavePunchCardMissing_StockNo]", arr);

            if (arr[6].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[6].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnMissingID.Value = arr[0].Value.ToString();
                tran.Commit();
            }

            //BindIssueGridOnFolio();
            FillReceiveGrid();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

}