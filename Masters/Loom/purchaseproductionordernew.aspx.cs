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

public partial class Masters_Loom_frmproductionorderonLoom : System.Web.UI.Page
{
    static string ChkUpdateRateFlag = "";
    static int hnEmpId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + gride.ClientID + "');", true);     
        if (!IsPostBack)
        {
            hnEmployeeType.Value = "0";
            switch (variable.VarPRODUCTIONINTERNAL_EXTERNAL)
            {
                case "1":
                    hnordercaltype.Value = "";
                    break;
                default:
                    hnordercaltype.Value = "1";
                    DDCalType.SelectedValue = hnordercaltype.Value;
                    break;
            }

            hnEmpWagescalculation.Value = "";
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                Select CI.CustomerId, CI.CustomerCode 
                From Customerinfo CI(Nolock)";
                if (Convert.ToInt32(Session["varcompanyNo"]) == 42)
                {
                    str = str + @" JOIN CompanyWiseCustomerDetail CCD(Nolock) ON CCD.CustomerID = CI.CustomerID And CCD.CompanyID = " + Session["CurrentWorkingCompanyID"];
                }

                str = str + @" Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" order by CI.Customercode 

                select UnitsId,UnitName from Units order by UnitName
                select UnitId,UnitName From Unit Where Unitid in(1,2,6)
                Select ID, BranchName 
                From BRANCHMASTER BM(nolock) 
                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @" 
                Select Distinct a.DepartmentID, D.DepartmentName 
                From ProcessIssueToDepartmentMaster a(Nolock)
                JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentID 
                JOIN BranchUser BU(nolock) ON BU.BranchID = a.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                Where a.Status = 'Pending' And a.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And a.MasterCompanyID = " + Session["varCompanyId"] + @" And a.ProcessID = 1 
                Order By D.DepartmentName
select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds, 3, true, "--Plz Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddlshipto, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            if (Session["usertype"].ToString() == "1")
            {
                TDComplete.Visible = true;
            }
            if (Session["varCompanyNo"].ToString() == "16" || Session["varCompanyNo"].ToString() == "28" || Session["varCompanyNo"].ToString() == "39")
            {
                UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 5, true, "--Plz Select--");
                TDDepartmentName.Visible = true;
                //TDDepartmentIssueNo.Visible = true;
            }
            if (Session["varCompanyNo"].ToString() == "44")
            {
                UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 6, true, "--Plz Select--");
            }
            if (Session["varCompanyNo"].ToString() == "27")
            {
                TDComplete.Visible = true;
            }
            hnissueorderid.Value = "0";
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    TDTanaCottonLotNo.Visible = false;
                    txtWeaverIdNoscan.Visible = true;
                    txtWeaverIdNo.Visible = false;
                    //TDcheckpurchasefolio.Visible = true;
                    ChkForWithoutRate.Visible = true;
                    TDTanaLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    ChkForSlipPrint.Visible = true;
                    BtnOrderProcessToPNM.Visible = true;
                    //BtnChampoPanipat.Visible = true;
                    BtnChampoPanipat.Visible = false;
                    BtnPanipatPNM1.Visible = true;
                    BtnPanipatPNM2.Visible = true;
                    BtnChampoHome.Visible = true;
                    BtnStockNoStatus.Visible = true;
                    if (Convert.ToInt16(Request.QueryString["ForEdit"]) == 1)
                    {
                        TDEdit.Visible = true;
                        chkEdit.Checked = true;
                        chkEdit.Enabled = false;
                        EditSelectedChange();
                        TDCustomerCode.Visible = true;
                        TDCustomerOrderNo.Visible = true;
                    }
                    else
                    {
                        TDChkForStockNoAttach.Visible = true;
                        TDEdit.Visible = false;
                        chkEdit.Visible = false;
                        ChkForWithoutRate.Visible = false;
                        ChkForSlipPrint.Visible = false;
                        btnPreview.Visible = false;
                        TDCustomerCode.Visible = false;
                        TDCustomerOrderNo.Visible = false;
                    }
                    TDChkForStockNoAttachWithoutMaterialIssue.Visible = true;

                    break;
                case "14":
                    TDTanaCottonLotNo.Visible = false;
                    chkforRateUpdate.Visible = false;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    TDTanaLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    break;
                case "27":
                case "34":
                    TDTanaCottonLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    TDTanaLotNo.Visible = false;
                    BtnPreviewConsumption.Visible = true;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    TDLastFolioNo.Visible = true;
                    lblLastFolioNo.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(Max(IssueOrderid ),0) from process_issue_master_1"));
                    break;
                case "28":
                    //txtWeaverIdNoscan.Visible = true;
                    //txtWeaverIdNo.Visible = false;
                    //TDTanaLotNo.Visible = false;
                    //BtnUpdateTanaLotNo.Visible = false;
                    //break;
                    TDTanaCottonLotNo.Visible = false;
                    txtWeaverIdNoscan.Visible = true;
                    txtWeaverIdNo.Visible = false;
                    //TDcheckpurchasefolio.Visible = true;
                    ChkForWithoutRate.Visible = true;
                    TDTanaLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    ChkForSlipPrint.Visible = true;
                    if (Convert.ToInt16(Request.QueryString["ForEdit"]) == 1)
                    {
                        TDEdit.Visible = true;
                        chkEdit.Checked = true;
                        chkEdit.Enabled = false;
                        EditSelectedChange();
                        TDCustomerCode.Visible = true;
                        TDCustomerOrderNo.Visible = true;
                    }
                    else
                    {
                        TDChkForStockNoAttach.Visible = true;
                        TDEdit.Visible = false;
                        chkEdit.Visible = false;
                        ChkForWithoutRate.Visible = false;
                        ChkForSlipPrint.Visible = false;
                        btnPreview.Visible = false;
                        TDCustomerCode.Visible = false;
                        TDCustomerOrderNo.Visible = false;
                    }
                    TDChkForStockNoAttachWithoutMaterialIssue.Visible = true;
                    break;
                case "22":
                    TDTanaCottonLotNo.Visible = true;
                    TDTanaLotNo.Visible = true;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    BtnPreviewConsumption.Visible = false;
                    ChkForWithoutRate.Visible = false;
                    TDLastFolioNo.Visible = false;
                    tdweaverpedningstock.Visible = false;
                    break;
                case "39":
                    TDTanaCottonLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    TDTanaLotNo.Visible = false;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    BtnPreviewConsumption.Visible = false;
                    ChkForWithoutRate.Visible = false;
                    TDLastFolioNo.Visible = false;
                    ChkForSlipPrint.Visible = false;
                    TDChkForStockNoAttach.Visible = true;
                    break;
                case "44":
                    TDTanaCottonLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    TDTanaLotNo.Visible = false;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    BtnPreviewConsumption.Visible = false;
                    ChkForWithoutRate.Visible = false;
                    TDLastFolioNo.Visible = false;
                    ChkForSlipPrint.Visible = false;
                    ddunit.Enabled = false;
                    tdprocess.Visible = true;
                    tdwithoutorder.Visible = true;
                     DDorderNo.Enabled = true;
            DDcustcode.Enabled = true;
            ddCatagory.Enabled = false;
            txtprodqty.Enabled = false;
            txtrateo.Enabled = false;
            txtamount.Enabled = false;
           ddltypegst.Enabled = false;
           TXTIGST.Enabled = false;
           TXTSGST.Enabled = false;
           TXTCGST.Enabled = false;

                    break;
                default:
                    TDTanaCottonLotNo.Visible = false;
                    BtnUpdateTanaLotNo.Visible = false;
                    TDTanaLotNo.Visible = false;
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    BtnPreviewConsumption.Visible = false;
                    ChkForWithoutRate.Visible = false;
                    TDLastFolioNo.Visible = false;
                    ChkForSlipPrint.Visible = false;
                    break;
            }
            //if (ddunit.Items.FindByValue(variable.VarDefaultProductionunit) != null)
            //{
            //    ddunit.SelectedValue = variable.VarDefaultProductionunit;
            //}

            if (variable.VarProductionOrderPcsWise == "1")
            {
                TDPcsWise.Visible = true;
            }
            else
            {
                TDPcsWise.Visible = false;
                ChkForPcsWise.Checked = false;
            }

            if (Session["varCompanyId"].ToString() == "22")
            {
                fillCottonLotNoGrid();
            }
            if (Session["varSubcompanyID"].ToString() == "282")
            {
                DDCalType.SelectedValue = "1";
            }
            if (Session["varCompanyId"].ToString() == "42")
            {
                DDCalType.SelectedValue = "0";
                hnordercaltype.Value = "0";
                TDChkForMaterialRate.Visible = true;
            }
            if (Session["varCompanyId"].ToString() == "247")
            {                
                hnordercaltype.Value = "0";
            }
            if (Session["varCompanyId"].ToString() == "36")
            {
                DDCalType.SelectedValue = "0";
                hnordercaltype.Value = "0";
            }
            if (Session["varCompanyId"].ToString() == "44")
            {
                DDCalType.SelectedValue = "1";
              //  hnordercaltype.Value = "0";
            }
            //if (Session["varCompanyId"].ToString() == "43")
            //{
            //    DDCalType.SelectedValue = "0";
            //    hnordercaltype.Value = "0";
            //}
        }
        if (string.IsNullOrEmpty(ddempname.SelectedValue))
        {
            if (Convert.ToString(ddempname.SelectedValue) == "")
            {
                //                strf = @"select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION'
                //        and EI.Status='P' and EI.Blacklist=0 order by Empname";

                string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.blacklist=0";
                if (Session["varCompanyno"].ToString() != "6")
                {
                    Str = Str + "  AND DM.Departmentname='PURCHASE'";
                }
                Str = Str + "  Order By empname ";
                UtilityModule.ConditionalComboFill(ref ddempname, Str, true, "--Select Party--");
            }
        }
    }
    protected void fillCottonLotNoGrid()
    {
        string str = @"SELECT s.StockId,s.lotno  from Stock S JOIN companyinfo CI ON s.companyid=ci.companyid
                        JOIN v_finishedItemDetail V ON s.Item_Finished_ID=v.Item_Finished_ID
                        Where S.CompanyId=" + DDcompany.SelectedValue + @" and Category_Name='Cotton'
                        group by s.StockId,s.lotno
                        having Round(Sum(s.qtyinhand),3)>0
                        Order by s.lotno";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DGTanaCottonLotNo.DataSource = ds.Tables[0];
            DGTanaCottonLotNo.DataBind();
        }

    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //*********************
        txtloomid.Text = "0";
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        DGConsumption.DataSource = null;
        DGConsumption.DataBind();
        listWeaverName.Items.Clear();
        hnissueorderid.Value = "0";
        enablecontrols();
        //*********************
        AutoCompleteExtenderloomno.ContextKey = "0#" + DDcompany.SelectedValue + "#" + DDProdunit.SelectedValue;

        if (TDLoomNoDropdown.Visible == true)
        {
            if (chkEdit.Checked == true)
            {
                string str = @"select Distinct PLM.UID,PLM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom 
                            from Process_issue_master_1 PIM inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                            Left join ITEM_MASTER Im on PLm.Itemid=IM.ITEM_ID 
                            left join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1
                            Where Plm.CompanyId=" + DDcompany.SelectedValue + " and PLm.UnitId=" + DDProdunit.SelectedValue + "";

                string str1 = @"select Top 1 PIM.IssueOrderID, PIM.LoomId 
                            from Process_issue_master_1 PIM inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                            join Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1
                            JOIN Empinfo EI ON EI.EmpID = EMP.EMPID 
                            Where Plm.CompanyId=" + DDcompany.SelectedValue + " and PLm.UnitId=" + DDProdunit.SelectedValue + "";

                if (chkcomplete.Checked == true)
                {
                    str = str + " and PIM.status='Complete'";
                    str1 = str1 + " and PIM.status='Complete'";
                }
                else
                {
                    str = str + " and PIM.status='Pending'";
                    str1 = str1 + " and PIM.status='Pending'";
                }
                if (txteditempid.Text != "")
                {
                    str = str + " and EMP.EMPID=" + txteditempid.Text + "";
                    str1 = str1 + " and EMP.EMPID=" + txteditempid.Text + "";
                }
                if (txteditempcode.Text != "")
                {
                    str1 = str1 + " and EI.EMPCode = '" + txteditempcode.Text + "'";
                }
                if (txtfolionoedit.Text != "")
                {
                    str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
                    str1 = str1 + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

                    ///str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
                }
                str = str + " order by Loom,loomno ";
                str1 = str1 + " order by PIM.IssueOrderID Desc ";
                str = str + str1;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                UtilityModule.ConditionalComboFillWithDS(ref DDLoomNo, ds, 0, true, "--Plz Select--");

                if (DDLoomNo.Items.Count > 0)
                {
                    if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
                    {
                        DDLoomNo.SelectedValue = ds.Tables[1].Rows[0]["LoomId"].ToString();
                    }
                    else
                    {
                        DDLoomNo.SelectedIndex = 1;
                    }

                    DDLoomNo_SelectedIndexChanged(sender, new EventArgs());
                }
                if (TDTanaLotNo.Visible == true)
                {
                    string str2 = @"select IsNull(PIM.TanaLotNo,'') as TanaLotNo From Process_issue_master_1 PIM  Where 1=1";
                    if (txtfolionoedit.Text != "")
                    {
                        str2 = str2 + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

                        ///str2 = str2 + " and PIM.Issueorderid=" + txtfolionoedit.Text;
                    }

                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        txtTanaLotNo.Text = Convert.ToString(ds2.Tables[0].Rows[0]["TanaLotNo"]);
                    }
                }
            }
            
        }
        else
        {
            string strf = string.Empty;
            //                UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo from ProductionLoomMaster PM 
            //                                            Left join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
            //                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
            if (chkEdit.Checked == true)
            {
                strf = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo 
                From Process_issue_master_1 PIM
                LEFT JOIN Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1   
                Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + @"
                And IsNull(PIM.BranchID, 0) = " + DDBranchName.SelectedValue;

                if (chkcomplete.Checked == true)
                {
                    strf = strf + " and PIM.Status='Complete'";
                }
                else
                {
                    strf = strf + " and PIM.Status='Pending'";
                }
                if (txteditempid.Text != "")
                {
                    strf = strf + " and EMP.EMPID=" + txteditempid.Text + "";
                }
                if (txtfolionoedit.Text != "")
                {
                    strf = strf + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

                    ////str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
                }
                strf = strf + " order by PIM.IssueOrderId desc";
                UtilityModule.ConditionalComboFill(ref DDFolioNo, strf, true, "--Plz Select--");
                if (DDFolioNo.Items.Count > 0)
                {
                    DDFolioNo.SelectedIndex = 1;
                    DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            //employee
           


        }

    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (TDDepartmentName.Visible == true && DDDepartmentName.SelectedIndex > 0)
        {
            FillDepartmentIssueNo();
        }
        else
        {
            if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
            {
                switch (variable.VarPRODUCTIONINTERNAL_EXTERNAL)
                {
                    case "1":
                        string view = "V_ORDERBALITEMTOBEORDERED_TAGGINGWITHINTERALPROD";
                        if (hnordercaltype.Value == "0")
                        {
                            view = "V_ORDERBALITEMTOBEORDERED_PREPRODASSIGNEDQTY";
                        }
                        if (Session["varcompanyNo"].ToString() == "16")
                        {
                            view = "V_ORDERBALITEMTOBEORDERED_TAGGINGWITHINTERALPROD";
                            if (hnEmployeeType.Value == "1")
                            {
                                view = "V_ORDERBALITEMTOBEORDERED_PREPRODASSIGNEDQTY";
                            }
                        }
                        if (Session["varcompanyNo"].ToString() == "28")
                        {
                            view = "V_OrderBalItemtobeordered";
                        }

                        str = @"select Distinct OM.OrderId,OM.CustomerOrderNo as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    and OM.ORDERFROMSAMPLE=0
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join  " + view + @" VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    Where OM.CompanyId=" + DDcompany.SelectedValue + " and OM.status='0' and OM.CustomerId=" + DDcustcode.SelectedValue + " order by OM.OrderId";
                        break;
                    default:
                        str = @"select Distinct OM.OrderId,OM.CustomerOrderNo as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    and OM.ORDERFROMSAMPLE=0
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join V_ORDERBALITEMTOBEORDERED_TAGGINGWITHINTERALPROD VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    Where OM.CompanyId=" + DDcompany.SelectedValue + " and OM.status='0' and OM.CustomerId=" + DDcustcode.SelectedValue + " order by OM.OrderId";
                        break;
                }

            }
            else
            {
                str = @"select Distinct OM.OrderId,OM.CustomerOrderNo as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    And Om.ORDERFROMSAMPLE=0
                    inner join JobAssigns JB on OD.Orderid=JB.OrderId
                    and OD.Item_Finished_Id=JB.ITEM_FINISHED_ID
                    inner join V_OrderBalItemtobeordered VB on OD.OrderId=VB.OrderId 
                    and OD.Item_Finished_Id=VB.ITEM_FINISHED_ID
                    Where OM.CompanyId=" + DDcompany.SelectedValue + " and OM.status='0' and OM.CustomerId=" + DDcustcode.SelectedValue + " order by OM.OrderId";
            }
            UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
        }
    }
    protected void FillissueDetails()
    {
        string str = "";
        if (chkwithoutorder.Checked)
        {
            //DG.DataSource = ds.Tables[0].DefaultView;
            //DG.DataBind();
        
        
        
        
        
        }
        else
        {
            if (hnordercaltype.Value != "")
            {
                string length = "", Width = "", Area = "", ColumnName = "";
                if (Session["varCompanyNo"].ToString() == "27" || Session["varCompanyNo"].ToString() == "34")
                {
                    if (variable.VarProductionSizeItemWise == "1")
                    {
                        ColumnName = "ItemPRODAREAMTR";
                    }
                    else
                    {
                        ColumnName = "PRODAREAMTR";
                    }
                }
                else
                {
                    if (variable.VarProductionSizeItemWise == "1")
                    {
                        ColumnName = "ItemProdAreamtr";
                    }
                    else
                    {
                        ColumnName = "Areamtr";
                    }
                }
                switch (hnordercaltype.Value)
                {
                    case "1": //Pcs Wise
                        switch (ddunit.SelectedValue)
                        {
                            case "1":
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemProdLengthMtr";
                                    Width = "ItemProdWidthmtr";
                                    Area = ColumnName;
                                }
                                else
                                {
                                    length = "LengthMtr";
                                    Width = "Widthmtr";
                                    Area = ColumnName;
                                }

                                break;
                            case "2":
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemProdLengthft";
                                    Width = "ItemProdWidthft";

                                    if (Session["VarCompanyNo"].ToString() == "43")
                                    {
                                        Area = "ItemProdAreaFt";
                                    }
                                    else
                                    {
                                        Area = "Actualfullareasqyd";
                                    }

                                }
                                else
                                {
                                    length = "Lengthft";
                                    Width = "Widthft";
                                    Area = "Actualfullareasqyd";
                                }

                                break;
                            default:
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemProdLengthft";
                                    Width = "ItemProdWidthft";
                                    Area = "Actualfullareasqyd";
                                }
                                else
                                {
                                    length = "Lengthft";
                                    Width = "Widthft";
                                    Area = "Actualfullareasqyd";
                                }
                                break;
                        }
                        break;
                    default:
                        switch (ddunit.SelectedValue)
                        {
                            case "1":
                                if (chkexportsize.Checked == true)
                                {
                                    length = "LengthMtr";
                                    Width = "Widthmtr";
                                    Area = "Areamtr";
                                }
                                else
                                {
                                    if (variable.VarProductionSizeItemWise == "1")
                                    {
                                        length = "ItemPRODLENGTHMTR";
                                        Width = "ItemPRODWIDTHMTR";
                                        Area = "ItemPRODAREAMTR";
                                    }
                                    else
                                    {
                                        length = "PRODLENGTHMTR";
                                        Width = "PRODWIDTHMTR";
                                        Area = "PRODAREAMTR";
                                    }
                                }
                                break;
                            case "2":
                                if (chkexportsize.Checked == true)
                                {
                                    length = "Lengthft";
                                    Width = "Widthft";
                                    Area = "Actualfullareasqyd";
                                }
                                else
                                {
                                    if (variable.VarProductionSizeItemWise == "1")
                                    {
                                        length = "ItemPRODLENGTHFT";
                                        Width = "ItemPRODWIDTHFT";
                                        Area = "ItemPRODAREAFT";
                                    }
                                    else
                                    {
                                        length = "PRODLENGTHFT";
                                        Width = "PRODWIDTHFT";
                                        Area = "PRODAREAFT";
                                    }
                                }
                                break;
                            default:
                                if (chkexportsize.Checked == true)
                                {
                                    length = "Lengthft";
                                    Width = "Widthft";
                                    Area = "Actualfullareasqyd";
                                }
                                else
                                {
                                    if (variable.VarProductionSizeItemWise == "1")
                                    {
                                        length = "ItemPRODLENGTHFT";
                                        Width = "ItemPRODWIDTHFT";
                                        Area = "ItemPRODAREAFT";
                                    }
                                    else
                                    {
                                        length = "PRODLENGTHFT";
                                        Width = "PRODWIDTHFT";
                                        Area = "PRODAREAFT";
                                    }

                                }
                                break;
                        }
                        break;
                }

                if (variable.VarProductionSizeItemWise == "1")
                {

                    str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                        case when " + Session["varcompanyid"] + "=43 then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+Case when " + ddunit.SelectedValue + @"=1  Then VF.ProdSizeMtr ELse VF.Prodsizeft ENd +' ('+Case when " + ddunit.SelectedValue + @"=1  Then CS.MtSizeAToC ELse  CS.SizeNameAToC +')' end 
                        Else  dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) end as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                        VJ.PREPRODASSIGNEDQTY as QtyRequired,
                        dbo.F_getProductionOrderQty(OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                        LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                        Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                        Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN SA." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN SA." + Area + " ELSE SA." + Area + @" END),
                        vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                        from OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                        inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                        inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                        inner join unit u on OD.OrderUnitId=U.UnitId 
                        JOIN SizeAttachedWithItem SA(NoLock) ON vf.SizeId=SA.SizeId and SA.ItemId=vf.ITEM_ID 
                        CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                        JOIN CustomerSize CS ON OM.CustomerId=CS.CustomerId and VF.SizeId=CS.Sizeid
                        Where Om.orderid=" + DDorderNo.SelectedValue + "  order by OD.orderdetailid";

                }
                else
                {
                    if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                    {
                        switch (variable.VarPRODUCTIONINTERNAL_EXTERNAL)
                        {
                            case "1":
                                string Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                string Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                if (hnordercaltype.Value == "0")
                                {
                                    Qtyrequired = "vj.preprodassignedqty";
                                    Function = "[F_GETPRODUCTIONORDERQTY_ExterNal]";
                                }

                                if (Session["varcompanyNo"].ToString() == "16")
                                {
                                    Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                    Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                    if (hnEmployeeType.Value == "1")
                                    {
                                        Qtyrequired = "vj.preprodassignedqty";
                                        Function = "[F_GETPRODUCTIONORDERQTY_ExterNal]";
                                    }
                                }

                                if (Session["varcompanyNo"].ToString() == "28" && Session["varSubcompanyID"].ToString() == "281")
                                {
                                    Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                    Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                    if (Convert.ToInt32(DDorderNo.SelectedValue) < 730)
                                    {
                                        Qtyrequired = "vj.preprodassignedqty";
                                        Function = "[F_GETPRODUCTIONORDERQTY_EXTERNALFORPNM]";
                                    }
                                }
                                if (Session["varcompanyNo"].ToString() == "28" && Session["varSubcompanyID"].ToString() != "281")
                                {
                                    Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                    Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                }

                                if (DDDepartmentIssueNo.SelectedIndex > 0)
                                {
                                    str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+case when " + ddunit.SelectedValue + @"=1 Then Vf.Sizemtr Else vf.sizeft end ELse
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            PIDD.Qty QtyRequired,(SELECT ISNULL(SUM(PID.QTY),0)-ISNULL(SUM(PID.CANCELQTY),0) 
	                                FROM PROCESS_ISSUE_MASTER_1 PIM(Nolock) 
	                                INNER JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PIM.ISSUEORDERID=PID.ISSUEORDERID AND PID.ORDERID=OM.OrderId AND PID.ITEM_FINISHED_ID=OD.Item_Finished_Id 
	                                WHERE PIM.STATUS<>'CANCELED' AND PIM.EMPID=0 AND IsNull(PIM.DEPARTMENTTYPE, 0) = 1 And IsNull(PIM.DepartmentIssueOrderID, 0) = " + DDDepartmentIssueNo.SelectedValue + @") OrderedQty,JOBRATE.RATE,
                            LENGTH=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END,
                            Width=case when " + hnordercaltype.Value + @"=1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
                            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate, vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId 
                            From OrderMaster OM 
                            JOIN OrderDetail OD on OM.OrderId=OD.OrderId
                            JOIN V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            JOIN ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id And PIDD.IssueOrderID = " + DDDepartmentIssueNo.SelectedValue + @" 
                            JOIN Unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " order by OD.orderdetailid";
                                }
                                else if (hnEmployeeType.Value == "1")
                                {
                                    str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id,od.OrderUnitId  as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+Case When OD.OrderUnitId = 1 Then vf.LWHMtr  When OD.OrderUnitId = 6 Then VF.LWHInch Else VF.LWHFt End ELse  dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,Case When OD.OrderUnitId = 1 Then 'Mtr'  When OD.OrderUnitId = 6 THEN 'INCH' ELSE 'Ft' END as UnitName,                            " + Qtyrequired + @" QtyRequired, dbo." + Function + @"(OM.OrderId,OD.Item_Finished_Id) OrderedQty,JOBRATE.RATE,
                            LENGTH=Case When OD.OrderUnitId = 1 Then LengthMtr  When OD.OrderUnitId = 6 THEN LengthINCH ELSE LengthFt END,
                            Width=Case When OD.OrderUnitId = 1 Then WidthMtr  When OD.OrderUnitId = 6 THEN WidthINCH ELSE WidthFt END,
                            Area=Case When OD.OrderUnitId = 1 Then AreaMtr  When OD.OrderUnitId = 6 THEN AreaInch ELSE AreaFt END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId
                            From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                            LEFT JOIN V_ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id 
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + " > 0  order by OD.orderdetailid";




                                }
                                else
                                {
                                    str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id,od.OrderUnitId as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+case when " + ddunit.SelectedValue + @"=1 Then Vf.Sizemtr Else vf.sizeft end ELse
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            " + Qtyrequired + @" - IsNull(PIDD.Qty, 0) QtyRequired, dbo." + Function + @"(OM.OrderId,OD.Item_Finished_Id) OrderedQty,JOBRATE.RATE,
                            LENGTH=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END,
                            Width=case when " + hnordercaltype.Value + @"=1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
                            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId
                            From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                            LEFT JOIN V_ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id 
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + " - IsNull(PIDD.Qty, 0) > 0  order by OD.orderdetailid";
                                }
                                break;
                            default:
                                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            Vj.INTERNALPRODASSIGNEDQTY as  QtyRequired,dbo.[F_GETPRODUCTIONORDERQTY_INTERNAL](OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                            LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                            Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                            Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END),
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                            from OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and vj.INTERNALPRODASSIGNEDQTY>0  order by OD.orderdetailid";
                                break;
                        }

                    }
                    else
                    {
                        str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                        dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                        OD.QtyRequired,dbo.F_getProductionOrderQty(OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                        LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                        Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                        Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END),
                        vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                        from OrderMaster OM 
                        inner join OrderDetail OD on OM.OrderId=OD.OrderId
                        inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                        inner join unit u on OD.OrderUnitId=U.UnitId 
                        CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                        Where Om.orderid=" + DDorderNo.SelectedValue + "  order by OD.orderdetailid";
                    }
                }


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {


                            if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["orderunitid"].ToString()) != null)
                            {
                                ddunit.SelectedValue = ds.Tables[0].Rows[0]["orderunitid"].ToString();
                            }
                        }
                    }
                }
                DG.DataSource = ds.Tables[0].DefaultView;
                DG.DataBind();
            }
        }
    }
    protected void FillissueDetailscustom()
    {
        string str = "";

        if (hnordercaltype.Value != "")
        {
            string length = "", Width = "", Area = "", ColumnName = "";
            if (Session["varCompanyNo"].ToString() == "27" || Session["varCompanyNo"].ToString() == "34")
            {
                if (variable.VarProductionSizeItemWise == "1")
                {
                    ColumnName = "ItemPRODAREAMTR";
                }
                else
                {
                    ColumnName = "PRODAREAMTR";
                }
            }
            else
            {
                if (variable.VarProductionSizeItemWise == "1")
                {
                    ColumnName = "ItemProdAreamtr";
                }
                else
                {
                    ColumnName = "Areamtr";
                }
            }
            switch (hnordercaltype.Value)
            {
                case "1": //Pcs Wise
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            if (variable.VarProductionSizeItemWise == "1")
                            {
                                length = "ItemProdLengthMtr";
                                Width = "ItemProdWidthmtr";
                                Area = ColumnName;
                            }
                            else
                            {
                                length = "LengthMtr";
                                Width = "Widthmtr";
                                Area = ColumnName;
                            }

                            break;
                        case "2":
                            if (variable.VarProductionSizeItemWise == "1")
                            {
                                length = "ItemProdLengthft";
                                Width = "ItemProdWidthft";

                                if (Session["VarCompanyNo"].ToString() == "43")
                                {
                                    Area = "ItemProdAreaFt";
                                }
                                else
                                {
                                    Area = "Actualfullareasqyd";
                                }

                            }
                            else
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                Area = "Actualfullareasqyd";
                            }

                            break;
                        default:
                            if (variable.VarProductionSizeItemWise == "1")
                            {
                                length = "ItemProdLengthft";
                                Width = "ItemProdWidthft";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                Area = "Actualfullareasqyd";
                            }
                            break;
                    }
                    break;
                default:
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            if (chkexportsize.Checked == true)
                            {
                                length = "LengthMtr";
                                Width = "Widthmtr";
                                Area = "Areamtr";
                            }
                            else
                            {
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemPRODLENGTHMTR";
                                    Width = "ItemPRODWIDTHMTR";
                                    Area = "ItemPRODAREAMTR";
                                }
                                else
                                {
                                    length = "PRODLENGTHMTR";
                                    Width = "PRODWIDTHMTR";
                                    Area = "PRODAREAMTR";
                                }
                            }
                            break;
                        case "2":
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemPRODLENGTHFT";
                                    Width = "ItemPRODWIDTHFT";
                                    Area = "ItemPRODAREAFT";
                                }
                                else
                                {
                                    length = "PRODLENGTHFT";
                                    Width = "PRODWIDTHFT";
                                    Area = "PRODAREAFT";
                                }
                            }
                            break;
                        default:
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                if (variable.VarProductionSizeItemWise == "1")
                                {
                                    length = "ItemPRODLENGTHFT";
                                    Width = "ItemPRODWIDTHFT";
                                    Area = "ItemPRODAREAFT";
                                }
                                else
                                {
                                    length = "PRODLENGTHFT";
                                    Width = "PRODWIDTHFT";
                                    Area = "PRODAREAFT";
                                }

                            }
                            break;
                    }
                    break;
            }

            if (variable.VarProductionSizeItemWise == "1")
            {

                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                        case when " + Session["varcompanyid"] + "=43 then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+Case when " + ddunit.SelectedValue + @"=1  Then VF.ProdSizeMtr ELse VF.Prodsizeft ENd +' ('+Case when " + ddunit.SelectedValue + @"=1  Then CS.MtSizeAToC ELse  CS.SizeNameAToC +')' end 
                        Else  dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) end as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                        VJ.PREPRODASSIGNEDQTY as QtyRequired,
                        dbo.F_getProductionOrderQty(OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                        LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                        Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                        Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN SA." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN SA." + Area + " ELSE SA." + Area + @" END),
                        vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                        from OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                        inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                        inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                        inner join unit u on OD.OrderUnitId=U.UnitId 
                        JOIN SizeAttachedWithItem SA(NoLock) ON vf.SizeId=SA.SizeId and SA.ItemId=vf.ITEM_ID 
                        CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                        JOIN CustomerSize CS ON OM.CustomerId=CS.CustomerId and VF.SizeId=CS.Sizeid
                        Where Om.orderid=" + DDorderNo.SelectedValue + "  order by OD.orderdetailid";

            }
            else
            {
                if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                {
                    switch (variable.VarPRODUCTIONINTERNAL_EXTERNAL)
                    {
                        case "1":
                            string Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                            string Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                            if (hnordercaltype.Value == "0")
                            {
                                Qtyrequired = "vj.preprodassignedqty";
                                Function = "[F_GETPRODUCTIONORDERQTY_ExterNal]";
                            }

                            if (Session["varcompanyNo"].ToString() == "16")
                            {
                                Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                if (hnEmployeeType.Value == "1")
                                {
                                    Qtyrequired = "vj.preprodassignedqty";
                                    Function = "[F_GETPRODUCTIONORDERQTY_ExterNal]";
                                }
                            }

                            if (Session["varcompanyNo"].ToString() == "28" && Session["varSubcompanyID"].ToString() == "281")
                            {
                                Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                                if (Convert.ToInt32(DDorderNo.SelectedValue) < 730)
                                {
                                    Qtyrequired = "vj.preprodassignedqty";
                                    Function = "[F_GETPRODUCTIONORDERQTY_EXTERNALFORPNM]";
                                }
                            }
                            if (Session["varcompanyNo"].ToString() == "28" && Session["varSubcompanyID"].ToString() != "281")
                            {
                                Qtyrequired = "Vj.INTERNALPRODASSIGNEDQTY";
                                Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL]";
                            }

                            if (DDDepartmentIssueNo.SelectedIndex > 0)
                            {
                                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+case when " + ddunit.SelectedValue + @"=1 Then Vf.Sizemtr Else vf.sizeft end ELse
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            PIDD.Qty QtyRequired,(SELECT ISNULL(SUM(PID.QTY),0)-ISNULL(SUM(PID.CANCELQTY),0) 
	                                FROM PROCESS_ISSUE_MASTER_1 PIM(Nolock) 
	                                INNER JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PIM.ISSUEORDERID=PID.ISSUEORDERID AND PID.ORDERID=OM.OrderId AND PID.ITEM_FINISHED_ID=OD.Item_Finished_Id 
	                                WHERE PIM.STATUS<>'CANCELED' AND PIM.EMPID=0 AND IsNull(PIM.DEPARTMENTTYPE, 0) = 1 And IsNull(PIM.DepartmentIssueOrderID, 0) = " + DDDepartmentIssueNo.SelectedValue + @") OrderedQty,JOBRATE.RATE,
                            LENGTH=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END,
                            Width=case when " + hnordercaltype.Value + @"=1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
                            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate, vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId 
                            From OrderMaster OM 
                            JOIN OrderDetail OD on OM.OrderId=OD.OrderId
                            JOIN V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            JOIN ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id And PIDD.IssueOrderID = " + DDDepartmentIssueNo.SelectedValue + @" 
                            JOIN Unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " order by OD.orderdetailid";
                            }
                            else if (hnEmployeeType.Value == "1")
                            {
                                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id,od.OrderUnitId  as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+Case When OD.OrderUnitId = 1 Then vf.LWHMtr  When OD.OrderUnitId = 6 Then VF.LWHInch Else VF.LWHFt End ELse  dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,Case When OD.OrderUnitId = 1 Then 'Mtr'  When OD.OrderUnitId = 6 THEN 'INCH' ELSE 'Ft' END as UnitName,                            " + Qtyrequired + @" QtyRequired, dbo." + Function + @"(OM.OrderId,OD.Item_Finished_Id) OrderedQty,JOBRATE.RATE,
                            LENGTH=Case When OD.OrderUnitId = 1 Then LengthMtr  When OD.OrderUnitId = 6 THEN LengthINCH ELSE LengthFt END,
                            Width=Case When OD.OrderUnitId = 1 Then WidthMtr  When OD.OrderUnitId = 6 THEN WidthINCH ELSE WidthFt END,
                            Area=Case When OD.OrderUnitId = 1 Then AreaMtr  When OD.OrderUnitId = 6 THEN AreaInch ELSE AreaFt END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId
                            From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                            LEFT JOIN V_ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id 
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + " > 0  order by OD.orderdetailid";

                                //                                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                                //                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+case when " + ddunit.SelectedValue + @"=1 Then Vf.Sizemtr Else vf.sizeft end ELse
                                //                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                                //                            " + Qtyrequired + @" QtyRequired, dbo." + Function + @"(OM.OrderId,OD.Item_Finished_Id) OrderedQty,JOBRATE.RATE,
                                //                            LENGTH=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
                                //                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END,
                                //                            Width=case when " + hnordercaltype.Value + @"=1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
                                //                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
                                //                            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
                                //                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
                                //                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId
                                //                            From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                                //                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                                //                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                                //                            LEFT JOIN V_ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id 
                                //                            inner join unit u on OD.OrderUnitId=U.UnitId 
                                //                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                                //                            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + " > 0  order by OD.orderdetailid";

                            }
                            else
                            {
                                str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                            case When " + hnordercaltype.Value + "=1 Then VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+' '+case when " + ddunit.SelectedValue + @"=1 Then Vf.Sizemtr Else vf.sizeft end ELse
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) END as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            " + Qtyrequired + @" - IsNull(PIDD.Qty, 0) QtyRequired, dbo." + Function + @"(OM.OrderId,OD.Item_Finished_Id) OrderedQty,JOBRATE.RATE,
                            LENGTH=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END,
                            Width=case when " + hnordercaltype.Value + @"=1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
                            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
                            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate,vf.CATEGORY_ID,vf.ITEM_ID,vf.QualityId
                            From OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id 
                            LEFT JOIN V_ProcessIssueToDepartmentDetail PIDD ON PIDD.orderid=OM.orderid and PIDD.item_finished_id=OD.Item_finished_id 
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + " - IsNull(PIDD.Qty, 0) > 0  order by OD.orderdetailid";
                            }
                            break;
                        default:
                            str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                            dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                            Vj.INTERNALPRODASSIGNEDQTY as  QtyRequired,dbo.[F_GETPRODUCTIONORDERQTY_INTERNAL](OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                            LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                            Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                            Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END),
                            vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                            from OrderMaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId
                            inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                            inner join V_JOBASSIGNSQTY VJ on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id
                            inner join unit u on OD.OrderUnitId=U.UnitId 
                            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                            Where Om.orderid=" + DDorderNo.SelectedValue + " and vj.INTERNALPRODASSIGNEDQTY>0  order by OD.orderdetailid";
                            break;
                    }

                }
                else
                {
                    str = @"select Om.OrderId,OD.OrderDetailId,OD.Item_Finished_Id," + ddunit.SelectedValue + @" as OrderUnitId,OD.flagsize,
                        dbo.F_getItemDescription(OD.Item_Finished_Id,Case when " + ddunit.SelectedValue + "=1  Then 1 ELse case when " + ddunit.SelectedValue + "=2 Then 0 Else   Od.flagsize ENd ENd) as ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
                        OD.QtyRequired,dbo.F_getProductionOrderQty(OM.OrderId,OD.Item_Finished_Id) as OrderedQty,JOBRATE.RATE,
                        LENGTH=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END),
                        Width=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END),
                        Area=(CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END),
                        vf.shapeid,JOBRATE.COMMRATE, JOBRATE.BONUS, JOBRATE.FinisherRate 
                        from OrderMaster OM 
                        inner join OrderDetail OD on OM.OrderId=OD.OrderId
                        inner join V_finisheditemdetail vf on Od.Item_finished_id=vf.item_finished_id
                        inner join unit u on OD.OrderUnitId=U.UnitId 
                        CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id,1," + DDProdunit.SelectedValue + @"," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
                        Where Om.orderid=" + DDorderNo.SelectedValue + "  order by OD.orderdetailid";
                }
            }


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DG.DataSource = ds.Tables[0].DefaultView;
            DG.DataBind();
        }
    }
    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        switch (Session["varcompanyId"].ToString())
        {
            case "28":
                chkpurchasefolio.Checked = false;
                chkexportsize.Checked = false;
                str = @"select Top 1 PURCHASEFLAG, EXPORTSIZEFLAG From OrderDetail Where OrderId=" + DDorderNo.SelectedValue;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["PURCHASEFLAG"].ToString() == "1")
                //    {
                //        //chkpurchasefolio.Checked = true;
                //    }
                //    if (ds.Tables[0].Rows[0]["EXPORTSIZEFLAG"].ToString() == "1")
                //    {
                //        //chkexportsize.Checked = true;
                //        //chkpurchasefolio.Checked = true;
                //    }
                //}
                break;
            case "44":
                if (DGOrderdetail.Rows.Count == 0)
                {
                    str = @"select top(1) OrderUnitId From OrderDetail Where OrderId=" + DDorderNo.SelectedValue+";select  PROCESS_NAME_ID,case when PROCESS_NAME='CUTTING' THEN 'DHARI' WHEN PROCESS_NAME='STITCHING' THEN 'CUSHION' END AS PROCESS_NAME from PROCESS_NAME_MASTER WHERE PROCESS_NAME IN('CUTTING','STITCHING')";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["orderunitid"].ToString()) != null)
                        {
                            ddunit.SelectedValue = ds.Tables[0].Rows[0]["orderunitid"].ToString();
                        }
                    }
                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    //    if (ddunit.Items.FindByValue(ds.Tables[1].Rows[0]["PROCESS_NAME_ID"].ToString()) != null)
                    //    {
                    //        ddunit.SelectedValue = ds.Tables[1].Rows[0]["orderunitid"].ToString();
                    //    }
                    //}
                }
                break;
            case "22":
                str = @"select REPLACE(CONVERT(Varchar(11),DispatchDate,106),' ','-') as DispatchDate From OrderMaster Where OrderId=" + DDorderNo.SelectedValue;
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    txttargetdate.Text = ds2.Tables[0].Rows[0]["DispatchDate"].ToString();
                }
                break;
        }
        FillissueDetails();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listWeaverName.Items.Remove(listWeaverName.SelectedItem);
        if (listWeaverName.Items.Count == 0)
        {
            hnEmpWagescalculation.Value = "";
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //if (txtloomid.Text == "" || txtloomid.Text == "0")
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "loomid", "alert('Please select Loom No.');", true);
        //    return;
        //}
        
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**************Sql Table
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("orderid", typeof(int));
        dtrecords.Columns.Add("orderdetailid", typeof(int));
        dtrecords.Columns.Add("Item_finished_id", typeof(int));
        dtrecords.Columns.Add("unitid", typeof(int));
        dtrecords.Columns.Add("LoomQty", typeof(int));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("Width", typeof(string));
        dtrecords.Columns.Add("Length", typeof(string));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("CommRate", typeof(float));
        dtrecords.Columns.Add("Bonus", typeof(float));
        dtrecords.Columns.Add("FinisherRate", typeof(float));
        dtrecords.Columns.Add("GSTTYPEID", typeof(int));
        dtrecords.Columns.Add("SGST", typeof(float));
        dtrecords.Columns.Add("CGST", typeof(float));
        dtrecords.Columns.Add("IGST", typeof(float));
       

        //**************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtloomqty = ((TextBox)DG.Rows[i].FindControl("txtloomqty"));
            if (Chkboxitem.Checked == true && (txtloomqty.Text != "" && txtloomqty.Text != "0"))
            {
                Label lblorderid = ((Label)DG.Rows[i].FindControl("lblorderid"));
                Label lblOrderdetailid = ((Label)DG.Rows[i].FindControl("lblOrderdetailid"));
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lblunitid"));
                TextBox txtrate = ((TextBox)DG.Rows[i].FindControl("txtrate"));
                TextBox txtwidth = ((TextBox)DG.Rows[i].FindControl("txtwidth"));
                TextBox txtlength = ((TextBox)DG.Rows[i].FindControl("txtlength"));
                Label lblarea = ((Label)DG.Rows[i].FindControl("lblarea"));
                TextBox txtcommrate = ((TextBox)DG.Rows[i].FindControl("txtcommrate"));
                TextBox TxtBonus = ((TextBox)DG.Rows[i].FindControl("TxtBonus"));
                TextBox TxtFinisherRate = ((TextBox)DG.Rows[i].FindControl("txtFinisherRate"));
                TextBox txtigst = ((TextBox)DG.Rows[i].FindControl("TXTIGST"));
                TextBox txtsgst = ((TextBox)DG.Rows[i].FindControl("TXTSGST"));
                TextBox txtcgst = ((TextBox)DG.Rows[i].FindControl("TXTCGST"));
                DropDownList DDGSType = (DropDownList)DG.Rows[i].FindControl("gsttype");

                //********Data Row
                DataRow dr = dtrecords.NewRow();
                dr["Orderid"] = lblorderid.Text;
                dr["orderdetailid"] = lblOrderdetailid.Text;
                dr["Item_finished_id"] = lblitemfinishedid.Text;
                dr["Unitid"] = lblunitid.Text;
                dr["LoomQty"] = txtloomqty.Text;
                dr["rate"] = txtrate.Text == "" ? "0" : txtrate.Text;
                dr["Width"] = txtwidth.Text;
                dr["Length"] = txtlength.Text;
                dr["area"] = lblarea.Text;
                dr["commrate"] = txtcommrate.Text == "" ? "0" : txtcommrate.Text;
                dr["Bonus"] = TxtBonus.Text == "" ? "0" : TxtBonus.Text;
                dr["FinisherRate"] = TxtFinisherRate.Text == "" ? "0" : TxtFinisherRate.Text;
                dr["GSTTYPEID"] = DDGSType.SelectedValue;
                dr["CGST"] = txtcgst.Text == "" ? "0" : txtcgst.Text;
                dr["SGST"] = txtsgst.Text == "" ? "0" : txtsgst.Text;
                dr["IGST"] = txtigst.Text == "" ? "0" : txtigst.Text;
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                //Get Empid 
                string StrEmpid = "";
                for (int i = 0; i < listWeaverName.Items.Count; i++)
                {
                    if (StrEmpid == "")
                    {
                        StrEmpid = listWeaverName.Items[i].Value;
                    }
                    else
                    {
                        StrEmpid = StrEmpid + "," + listWeaverName.Items[i].Value;
                    }
                }
                if (ddempname.SelectedValue.ToString() == "0")
                {
                    lblmessage.Text = "Plz Enter Weaver ID No...";
                    return;

                }
                else
                {
                    StrEmpid = ddempname.SelectedValue;
                }
                //Check Employee Entry
                //if (StrEmpid == "")
                //{
                //    lblmessage.Text = "Plz Enter Weaver ID No...";
                //    return;
                //}
                //******
                SqlCommand cmd = new SqlCommand("Pro_SaveProductionOrderonLoom_agni", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@issueorderid", SqlDbType.Int);
                cmd.Parameters["@issueorderid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@issueorderid"].Value = hnissueorderid.Value;
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductionUnit", 1);
                cmd.Parameters.AddWithValue("@LoomId",Convert.ToInt32(txtloomid.Text==""?"0":txtloomid.Text));
                cmd.Parameters.AddWithValue("@Empid", StrEmpid);
                cmd.Parameters.Add("@FolioNo", SqlDbType.VarChar, 100);
                cmd.Parameters["@FolioNo"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Issuedate", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@Targetdate", txttargetdate.Text);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@dtrecords", dtrecords);
                if (variable.VarProductionOrderPcsWise == "1" && ChkForPcsWise.Checked == true)
                {
                    hnordercaltype.Value = "1";
                }
                cmd.Parameters.AddWithValue("@Ordercaltype", (hnordercaltype.Value == "" ? "1" : hnordercaltype.Value));  //Pcs Wise
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Prefix", TxtPrefix.Text);
                cmd.Parameters.AddWithValue("@Postfix", TxtPostfix.Text);
               // cmd.Parameters.AddWithValue("@Purchaseflag", chkpurchasefolio.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@Purchaseflag",  "1");
                cmd.Parameters.AddWithValue("@Exportsizeflag", chkexportsize.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@Remarks", TxtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@Instruction", TxtInstructions.Text.Trim());
                cmd.Parameters.AddWithValue("@Tstockno", Tdstockno.Visible == true ? txtstockno.Text : "");
                cmd.Parameters.AddWithValue("@FlagFixOrWeight", ChkForFix.Checked == true ? 0 : 1);
                cmd.Parameters.AddWithValue("@TanaLotNo", TDTanaLotNo.Visible == true ? txtTanaLotNo.Text : "");
                cmd.Parameters.AddWithValue("@StockNoAttachWithoutMaterialIssue", ChkForStockNoAttachWithoutMaterialIssue.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@StockNoAttach", ChkForStockNoAttach.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
                cmd.Parameters.AddWithValue("@SHIPTO", ddlshipto.SelectedValue);
                
                if (TDDepartmentIssueNo.Visible == true)
                {
                    if (DDDepartmentIssueNo.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@DepartmentIssueOrderID", DDDepartmentIssueNo.SelectedValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DepartmentIssueOrderID", 0);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DepartmentIssueOrderID", 0);
                }
                if (TDChkForMaterialRate.Visible == true)
                {
                    cmd.Parameters.AddWithValue("@MaterialRate", ChkForMaterialRate.Checked == true ? "1" : "0");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MaterialRate", 0);
                }

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
                    DDCalType.Enabled = false;
                    txtfoliono.Text = cmd.Parameters["@FolioNo"].Value.ToString(); //param[5].Value.ToString();
                    hnissueorderid.Value = cmd.Parameters["@issueorderid"].Value.ToString();// param[0].Value.ToString();
                    FillGrid();
                    FillConsumptionQty();
                    Refreshcontrol();
                    disablecontrols();
                    if (Session["varcompanyid"].ToString() == "21")
                    {
                        chkforRateUpdate.Checked = false;
                    }
                }
                //******
                #region Comment on 07-Sep-2018
                //SqlParameter[] param = new SqlParameter[20];
                //param[0] = new SqlParameter("@issueorderid", SqlDbType.Int);
                //param[0].Value = hnissueorderid.Value;
                //param[0].Direction = ParameterDirection.InputOutput;
                //param[1] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                //param[2] = new SqlParameter("@ProductionUnit", DDProdunit.SelectedValue);
                //param[3] = new SqlParameter("@LoomId", txtloomid.Text);
                //param[4] = new SqlParameter("@Empid", StrEmpid);
                //param[5] = new SqlParameter("@FolioNo", SqlDbType.Int);
                //param[5].Direction = ParameterDirection.Output;
                //param[6] = new SqlParameter("@Issuedate", txtissuedate.Text);
                //param[7] = new SqlParameter("@Targetdate", txttargetdate.Text);
                //param[8] = new SqlParameter("@Userid", Session["varuserid"]);
                //param[9] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
                //param[10] = new SqlParameter("@dtrecords", dtrecords);
                //param[11] = new SqlParameter("@Ordercaltype", (hnordercaltype.Value == "" ? "1" : hnordercaltype.Value));  //Pcs Wise
                //param[12] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                //param[12].Direction = ParameterDirection.Output;
                //param[13] = new SqlParameter("@Prefix", TxtPrefix.Text);
                //param[14] = new SqlParameter("@Postfix", TxtPostfix.Text);
                //param[15] = new SqlParameter("@Purchaseflag", chkpurchasefolio.Checked == true ? "1" : "0");
                //param[16] = new SqlParameter("@Exportsizeflag", chkexportsize.Checked == true ? "1" : "0");
                //param[17] = new SqlParameter("@Remarks", TxtRemarks.Text.Trim());
                //param[18] = new SqlParameter("@Instruction", TxtInstructions.Text.Trim());
                //param[19] = new SqlParameter("@Tstockno", Tdstockno.Visible == true ? txtstockno.Text : "");
                ////*************
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveProductionOrderonLoom", param);
                //if (param[12].Value.ToString() != "")  ///IF DATA NOT SAVED
                //{
                //    lblmessage.Text = param[12].Value.ToString();
                //    Tran.Rollback();
                //}
                //else
                //{
                //    lblmessage.Text = "Data Saved Successfully.";
                //    Tran.Commit();
                //    txtfoliono.Text = param[5].Value.ToString();
                //    hnissueorderid.Value = param[0].Value.ToString();
                //    FillGrid();
                //    FillConsumptionQty();
                //    Refreshcontrol();
                //    disablecontrols();
                //    if (Session["varcompanyid"].ToString() == "21")
                //    {
                //        chkforRateUpdate.Checked = false;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void Refreshcontrol()
    {
        //DDProdunit.SelectedIndex = -1;
        //DDLoomNo.SelectedIndex = -1;
        //txtloomid.Text = "0";
        //txtloomno.Text = "";
        DDcustcode.SelectedIndex = -1;
        DDorderNo.SelectedIndex = -1;
        // listWeaverName.Items.Clear();
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillGrid()
    {
        TxtRemarks.Text = "";
        TxtInstructions.Text = "";
        string str = "";

//        if (Session["varcompanyid"].ToString() == "43")
//        {
//            str = @"Select Issue_Detail_Id,PM.issueorderid,
//                        VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHADECOLORNAME+' '+VF.SHAPENAME+ Space(2) + Case When PM.Unitid=1 Then VF.SizeMtr Else case When PM.unitid=6 Then VF.Sizeinch  Else  VF.SizeFt End End + Space(2) +' ('+
//                        Case When PM.Unitid=1 Then CS.MtSizeAToC Else case When PM.unitid=6 Then CS.InchSize  Else  CS.SizeNameAToC End End+')' ItemDescription,Length,Width,
//                        Length + 'x' + Width Size,ROund(Area*Qty,4) as Area,Rate,Comm,Qty,Amount,REPLACE(CONVERT(nvarchar(11),PM.AssignDate,106),' ','-') as AssignDate,REPLACE(CONVERT(nvarchar(11),PD.ReqbyDate,106),' ','-') as ReqbyDate,PM.Unitid,isnull(PM.Purchaseflag,0) as Purchaseflag,PM.Caltype ,
//                        isnull(PM.Remarks,'') as Remarks,isnull(Pm.instruction,'') as Instruction,pm.FlagFixOrWeight,isnull(PM.ChallanNo,'') as ChallanNo, 
//                        IsNull(PM.FlagStockNoAttachWithoutRawMaterialIssue, 0) FlagStockNoAttachWithoutRawMaterialIssue, 
//                        IsNull(PM.DEPARTMENTTYPE, 0) DEPARTMENTTYPE, IsNull(PM.DepartmentIssueOrderID, 0) DepartmentIssueOrderID, PD.Bonus,isnull(PD.Rate2,0) as FinisherRate 
//                        From PROCESS_ISSUE_MASTER_1 PM(NoLock) JOIN PROCESS_ISSUE_DETAIL_1 PD ON PM.IssueOrderid=PD.IssueOrderid
//                        JOIN V_FinishedItemDetail VF ON PD.Item_Finished_Id=Vf.Item_Finished_ID
//                        JOIN OrderMaster OM ON PD.ORDERID=OM.OrderId
//                        JOIN CustomerSize CS ON OM.CustomerId=CS.CustomerId and VF.SizeId=CS.Sizeid
//                        Where PM.IssueOrderid=" + hnissueorderid.Value + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
//        }
//        else
//        {
        str = @"Select Issue_Detail_Id,PM.issueorderid,ICM.Category_Name,ICM.Category_Name+' '+IM.Item_Name+' '+IPM.QDCS + Space(2) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End ItemDescription,Length,Width,
                        Length + 'x' + Width Size,ROund(Area*Qty,4) as Area,Rate,Comm,Qty,Amount,REPLACE(CONVERT(nvarchar(11),PM.AssignDate,106),' ','-') as AssignDate,REPLACE(CONVERT(nvarchar(11),PD.ReqbyDate,106),' ','-') as ReqbyDate,PM.Unitid,isnull(PM.Purchaseflag,0) as Purchaseflag,PM.Caltype ,
                        isnull(PM.Remarks,'') as Remarks,isnull(Pm.instruction,'') as Instruction,pm.FlagFixOrWeight,isnull(PM.ChallanNo,'') as ChallanNo, 
                        IsNull(PM.FlagStockNoAttachWithoutRawMaterialIssue, 0) FlagStockNoAttachWithoutRawMaterialIssue, 
                        IsNull(PM.DEPARTMENTTYPE, 0) DEPARTMENTTYPE, IsNull(PM.DepartmentIssueOrderID, 0) DepartmentIssueOrderID, PD.Bonus,isnull(PD.Rate2,0) as FinisherRate,isnull(PD.CGST,0) as CGST ,isnull(PD.SGST,0) as SGST, isnull(PD.IGST,0) as IGST,case when isnull(pd.GSTTYPE,0)=1 then pd.amount+((pd.amount*(isnull(pd.CGST,0)+isnull(pd.SGST,0))/100)) when isnull(pd.GSTTYPE,0)=2 then pd.amount+((pd.Amount*isnull(pd.IGST,0))/100) else pd.Amount end as totalamount  
                        From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
       // }

        //Employeedetail
        str = str + @" select Distinct Ei.Empid,EI.EmpCode+'-'+EI.EmpName as Empname,activestatus from Employee_ProcessOrderNo EMP 
                    inner Join EmpInfo EI on EMP.Empid=EI.EmpId 
                    Where Emp.ProcessId=1 and Emp.IssueOrderId=" + hnissueorderid.Value;
        if (hnEmployeeType.Value == "0")
        {
            str = str + @" Select Issue_Detail_Id,PM.issueorderid, VF.Category_Name + '  ' + VF.Item_Name + '  ' + VF.QualityName + '  ' + 
                    VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
                    Case When PM.Unitid=1 Then VF.SizeMtr Else Case When PM.UnitID = 6 Then VF.Sizeinch Else VF.SizeFt End End ItemDescription, LS.StockNo, LS.TstockNo 
                    From PROCESS_ISSUE_MASTER_1 PM 
                    JOIN PROCESS_ISSUE_DETAIL_1 PD ON PD.IssueOrderID = PM.IssueOrderID 
                    JOIN LoomStockNo LS ON LS.IssueOrderID = PD.IssueOrderID And LS.IssueDetailID = PD.Issue_Detail_ID And LS.ProcessID = 1 
                    JOIN V_FinishedItemDetail VF ON VF.Item_Finished_ID = PD.Item_Finished_ID 
                    Where PM.IssueOrderid = " + hnissueorderid.Value + " And VF.MasterCompanyId = " + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
        }
        //
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();

        if (chkEdit.Checked == true && (Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "28" || Session["varcompanyid"].ToString() == "42") && Convert.ToInt32(Session["usertype"]) > 2)
        {
            DGOrderdetail.Columns[5].Visible = false;
            DGOrderdetail.Columns[6].Visible = false;
            DGOrderdetail.Columns[7].Visible = false;
            DGOrderdetail.Columns[8].Visible = false;
            DGOrderdetail.Columns[9].Visible = false;
            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;

            DGOrderdetail.Columns[12].Visible = false;
            DGOrderdetail.Columns[13].Visible = false;
        }
        if (Session["varcompanyid"].ToString() == "42")
        {
            DGOrderdetail.Columns[12].Visible = true;

            //DGOrderdetail.Columns[11].Visible = false;
        }
        //if (chkEdit.Checked == true && Session["varcompanyid"].ToString() == "28" && Convert.ToInt32(Session["usertype"]) > 1)
        //{
        //    //DGOrderdetail.Columns[9].Visible = false;
        //    DGOrderdetail.Columns[10].Visible = false;
        //    DGOrderdetail.Columns[11].Visible = false;
        //}
        if (hnEmployeeType.Value == "0")
        {
            DGOrderdetailStockNo.DataSource = ds.Tables[2];
            DGOrderdetailStockNo.DataBind();
        }

        chkpurchasefolio.Checked = false;
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkpurchasefolio.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Purchaseflag"]);
            if (variable.VarProductionOrderPcsWise == "1")
            {
                ChkForPcsWise.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Caltype"]);
            }
            if (ds.Tables[0].Rows[0]["FlagFixOrWeight"].ToString() == "0")
            {
                ChkForFix.Checked = true;
            }
            else
            {
                ChkForFix.Checked = false;
            }
        }
        //
        if (chkEdit.Checked == true)
        {
            //Date
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissuedate.Text = ds.Tables[0].Rows[0]["assigndate"].ToString();
                txttargetdate.Text = ds.Tables[0].Rows[0]["Reqbydate"].ToString();
                //txtfoliono.Text = ds.Tables[0].Rows[0]["issueorderid"].ToString();
                txtfoliono.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["unitid"].ToString()) != null)
                {
                    ddunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
                }
                if (DDCalType.Enabled == true)
                {
                    hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                    DDCalType.SelectedValue = hnordercaltype.Value;
                }
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                TxtInstructions.Text = ds.Tables[0].Rows[0]["instruction"].ToString();

                if (variable.VarProductionOrderPcsWise == "1")
                {
                    ChkForPcsWise.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Caltype"]);
                }
                ChkForStockNoAttachWithoutMaterialIssue.Checked = false;
                if (ds.Tables[0].Rows[0]["FlagStockNoAttachWithoutRawMaterialIssue"].ToString() == "1")
                {
                    ChkForStockNoAttachWithoutMaterialIssue.Checked = true;
                }
            }
            //Employee
            if (ds.Tables[1].Rows.Count > 0)
            {
                listWeaverName.Items.Clear();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    listWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[i]["Empname"].ToString(), ds.Tables[1].Rows[i]["Empid"].ToString()));
                    if (ds.Tables[1].Rows[i]["activestatus"].ToString() == "0")
                    {
                        listWeaverName.Items[i].Attributes.Add("style", "background-color:red;");
                    }
                    else
                    {
                        listWeaverName.Items[i].Attributes.Add("style", "background-color:white;");
                    }
                }
            }
        }
    }
    protected void FillConsumptionQty()
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@issueorderid", hnissueorderid.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILLPRODUCTIONONLOOMCONSUMPTION", param);
        DGConsumption.DataSource = ds.Tables[0];
        DGConsumption.DataBind();


        //        string str = @"SELECT VF1.Item_Name+Space(2)+VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShapeName+Space(2)+
        //          CASE WHEN PM.UnitId=1 Then VF1.SizeMtr else VF1.SizeFt END+Space(2)+VF1.ShadeColorName ItemDescription,
        //          Isnull(Round(Sum(CASE WHEN OCD.ICalType=0 or OCD.ICalType=2 THEN CASE WHEN PM.UnitId=1 Then PD.Qty*PD.Area*OCD.IQTY*1.196 else PD.Qty*PD.Area*OCD.IQTY END ELSE 
        //          CASE WHEN PM.UnitId=1 Then PD.Qty*OCD.IQTY*1.196 else PD.Qty*OCD.IQTY END END),3),0) ConsmpQTY,OCD.IFinishedid,U.UnitName
        //          FROM PROCESS_ISSUE_MASTER_1 PM inner join PROCESS_ISSUE_DETAIL_1 PD on PM.IssueOrderId=PD.IssueOrderId
        //          inner join PROCESS_CONSUMPTION_DETAIL OCD on OCD.ISSUEORDERID=PM.IssueOrderId and OCD.ISSUE_DETAIL_ID=PD.Issue_Detail_Id and ocd.PROCESSID=1
        //          inner join V_FinishedItemDetail VF1 on Vf1.ITEM_FINISHED_ID=OCD.IFINISHEDID inner join Unit U on OCD.iunitid=U.Unitid Where OCD.Issueorderid=" + hnissueorderid.Value + @" 
        //          Group By VF1.Category_Name,VF1.Item_Name,VF1.QualityName,VF1.DesignName,VF1.ColorName,VF1.ShapeName,PM.UnitId,VF1.SizeMtr,VF1.SizeFt,
        //          VF1.ShadeColorName,OCD.IFINISHEDID,PM.Issueorderid,U.UnitName";

        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        DGConsumption.DataSource = ds.Tables[0];
        //        DGConsumption.DataBind();

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        switch (Session["varcompanyId"].ToString())
        {
            case "27":
            case "34":
                ReportOldForm();
                break;
            case "16":
                if (ChkForSlipPrint.Checked == true)
                {
                    SlipReport();
                    break;
                }
                else
                {
                    Report();
                    break;
                }
            case "28":
                if (ChkForSlipPrint.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoForSlipPrint.rpt";
                    SlipReport();
                    break;
                }
                else
                {
                    Report();
                    break;
                }
            default:
                Report();
                break;
        }

    }
    private void ReportOldForm()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[0].Value = hnissueorderid.Value;
        array[1].Value = 1;
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
            else if (Convert.ToInt32(Session["VarcompanyId"]) == 27 || Convert.ToInt32(Session["VarcompanyId"]) == 34)//For Antique Panipat
            {
                Session["rptFileName"] = "~\\Reports\\ProductionOrderNewAntique.rpt";
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
    private void Report()
    {
        //DataSet ds = new DataSet();
        //// string qry = "";
        //// string str = "";
        //SqlParameter[] array = new SqlParameter[3];
        //array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        //array[1] = new SqlParameter("@ProcessId", 1);
        //array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);

        //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrder", array);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_ForProductionOrder", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@IssueOrderId", hnissueorderid.Value);
        cmd.Parameters.AddWithValue("@ProcessId", 1);
        cmd.Parameters.AddWithValue("@UserName", Session["UserName"]);
        if (Session["varCompanyId"].ToString() == "44")
        {
            cmd.Parameters.AddWithValue("@TYPE", chksummary.Checked ? 2 : 0);
        }
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);

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
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    if (variable.VarLoomNoGenerated == "1")
                    {
                        if (ChkForWithoutRate.Checked == true)
                        {
                            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoWithoutRate.rpt";
                        }
                        else if (ChkForSlipPrint.Checked == true)
                        {
                            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoForSlipPrint.rpt";
                        }
                        else
                        {
                            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampo.rpt";
                        }
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWisechampo.rpt";
                    }
                    break;
                case "28":
                    //Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseWithoutrate.rpt";
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampo.rpt";
                    break;
                case "22":
                    if (variable.VarLoomNoGenerated == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockDiamond.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWise.rpt";
                    }
                    break;
                case "42":
                    if (variable.VarLoomNoGenerated == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockVikarm.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWise.rpt";
                    }
                    break;
                case "43":
                    if (variable.VarLoomNoGenerated == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockCarpet_International.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWise.rpt";
                    }
                    break;
                case "44":
                   
                        Session["rptFileName"] = "~\\Reports\\productionorderissagni.rpt";
                    
                    break;
                default:
                    if (variable.VarLoomNoGenerated == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStock.rpt";                        
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWise.rpt";
                    }
                    break;
            }

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionOrderLoomWise.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void SlipReport()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        array[1] = new SqlParameter("@ProcessId", 1);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrderSlipReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (variable.VarLoomNoGenerated == "1")
            {
                Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoForSlipPrint.rpt";
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionOrderLoomWiseStockSlipPrint.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            BtnOrderProcessToPNM.Visible = false;

            BtnChampoPanipat.Visible = false;
            BtnPanipatPNM1.Visible = false;
            BtnPanipatPNM2.Visible = false;
            BtnChampoHome.Visible = false;

            btnupdateconsmp.Visible = false;
            btnsave.Visible = false;
            btncancel.Visible = false;

            //DGOrderdetail.Columns[10].Visible = false;
            //DGOrderdetail.Columns[11].Visible = false;

            DGOrderdetail.Columns[12].Visible = false;
            DGOrderdetail.Columns[13].Visible = false;

            DGOrderdetailStockNo.Columns[3].Visible = false;
        }
       
    }
    protected void EditSelectedChange()
    {
        txteditempid.Text = "";
        txteditempcode.Text = "";
        txtfolionoedit.Text = "";
        ddunit.Enabled = true;
        enablecontrols();
        if (chkEdit.Checked == true)
        {
     //       TDEMPEDIT.Visible = true;
            TDFolioNo.Visible = true;
            TDFolioNotext.Visible = true;
            hnissueorderid.Value = "0";
            //btnsave.Visible = false;
            if (Session["varcompanyid"].ToString() == "16")
            {
                if (Convert.ToInt32(Session["usertype"]) > 2)
                {
                    btncancel.Visible = false;
                }
                else
                {
                    btncancel.Visible = true;
                }
            }
            else
            {
                   btncancel.Visible = true;
            }


            TDupdateemp.Visible = true;
            btnupdateconsmp.Visible = true;
           // TDLoomNoDropdown.Visible = true;
            TDLoomNotextbox.Visible = false;
            TDactiveemployee.Visible = true;
            ddunit.Enabled = false;
            if (Session["varcompanyno"].ToString() == "16" && Session["usertype"].ToString() == "1")
            {
                ChkForDyeingConsumption.Visible = true;
            }
            else
            {
                ChkForDyeingConsumption.Visible = false;
            }
            ChkForPcsWise.Enabled = false;

            if (Session["varCompanyNo"].ToString() == "22")
            {
                BtnUpdateTanaLotNo.Visible = true;
            }
            else
            {
                BtnUpdateTanaLotNo.Visible = false;
            }

            if (Session["varCompanyNo"].ToString() == "42")
            {
                BtnUpdateRemark.Visible = true;                
            }
            else
            {
                BtnUpdateRemark.Visible = false;
            }
        }
        else
        {
            ChkForPcsWise.Enabled = true;
            ChkForDyeingConsumption.Visible = false;
            TDEMPEDIT.Visible = false;
            btnsave.Visible = true;
            TDFolioNotext.Visible = false;
            TDFolioNo.Visible = false;
            hnissueorderid.Value = "0";
            btncancel.Visible = false;
            TDupdateemp.Visible = false;
            btnupdateconsmp.Visible = false;
            TDLoomNoDropdown.Visible = false;
           // TDLoomNotextbox.Visible = true;
            TDactiveemployee.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDProdunit, "select UnitsId,UnitName from Units order by UnitName", true, "--Plz Select--");
        }
        DDFolioNo.Items.Clear();
        listWeaverName.Items.Clear();
        txtfoliono.Text = "";
        DDProdunit.SelectedIndex = -1;
        DDLoomNo.SelectedIndex = -1;
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        //
        DG.DataSource = null;
        DG.DataBind();
        //
        DGConsumption.DataSource = null;
        DGConsumption.DataBind();
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        txtloomid.Text = DDLoomNo.SelectedValue;
        if (chkEdit.Checked == true)
        {
            str = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo 
                From Process_issue_master_1 PIM
                LEFT JOIN Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1   
                Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + " and PIM.LoomId=" + DDLoomNo.SelectedValue + @"
                And IsNull(PIM.BranchID, 0) = " + DDBranchName.SelectedValue;

            if (chkcomplete.Checked == true)
            {
                str = str + " and PIM.Status='Complete'";
            }
            else
            {
                str = str + " and PIM.Status='Pending'";
            }
            if (ddempname.SelectedIndex >0)
            {
                str = str + " and EMP.EMPID=" + ddempname.SelectedValue + "";
            }
            if (txtfolionoedit.Text != "")
            {
                str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

                ////str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
            }
            str = str + " order by PIM.IssueOrderId desc";
            UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
            if (DDFolioNo.Items.Count > 0)
            {
                DDFolioNo.SelectedIndex = 1;
                DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        //employee
        str = @"select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION'
        and EI.Status='P' and EI.Blacklist=0 order by Empname";
        UtilityModule.ConditionalComboFill(ref DDemployee, str, true, "--Plz select--");
    }
    protected void ShowCustomerCodeAndOrderNo()
    {
        string str = "";
        str = @"select OM.CustomerOrderNo,CI.CustomerCode from Process_issue_Detail_1 PID JOIN OrderMaster OM ON PID.OrderId=OM.OrderId
                JOIN customerinfo CI ON OM.CustomerId=CI.CustomerId                   
                 Where PID.IssueOrderId=" + DDFolioNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblCustomerCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
            lblCustomerOrderNo.Text = ds.Tables[0].Rows[0]["CustomerOrderNo"].ToString();
        }
        else
        {
            lblCustomerCode.Text = "";
            lblCustomerOrderNo.Text = "";
        }
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDFolioNo.SelectedValue;
        FillGrid();
        EnablecontrolwithGENERATESTOCKNOONTAGGING();
        DisablecontrolwithGENERATESTOCKNOONTAGGING();
        FillConsumptionQty();
        if (Session["VarCompanyId"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
        {
            ShowCustomerCodeAndOrderNo();
        }
    }
    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;
        FillGrid();
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblissuedetailid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissuedetailid");
            Label lblhqty = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblhqty");
            TextBox txtqty = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtqty");
            TextBox txtrategrid = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtrategrid");
            TextBox txtcommrategrid = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtcommrategrid");

            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@issuedetailid", lblissuedetailid.Text);
            param[2] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[3] = new SqlParameter("@Hqty", lblhqty.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Userid", Session["varuserid"]);
            param[6] = new SqlParameter("@rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);
            param[7] = new SqlParameter("@commrate", txtcommrategrid.Text == "" ? "0" : txtcommrategrid.Text);
            param[8] = new SqlParameter("@Remarks", TxtRemarks.Text.Trim());
            param[9] = new SqlParameter("@Instruction", TxtInstructions.Text.Trim());
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_UpdateproductionorderLoomWise", param);
            //*************
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            FillGrid();
            FillConsumptionQty();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@Userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CancelProductionorderLoomWise", param);
            //******
            if (param[1].Value.ToString() != "")
            {
                lblmessage.Text = param[1].Value.ToString();
            }
            else
            {
                lblmessage.Text = "Folio canceled successfully.";
                DGOrderdetail.DataSource = null;
                DGOrderdetail.DataBind();
                //*****************************
                DGConsumption.DataSource = null;
                DGConsumption.DataBind();
                DDLoomNo.SelectedIndex = -1;
                DDFolioNo.Items.Clear();
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnupdateemp_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
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
            for (int i = 0; i < listWeaverName.Items.Count; i++)
            {
                for (int j = 0; j < DGOrderdetail.Rows.Count; j++)
                {
                    Label lblissueorderid = ((Label)DGOrderdetail.Rows[j].FindControl("lblissueorderid"));
                    Label lblissuedetailid = ((Label)DGOrderdetail.Rows[j].FindControl("lblissuedetailid"));

                    DataRow dr = dtrecord.NewRow();
                    dr["processid"] = 1;
                    dr["issueorderid"] = lblissueorderid.Text;
                    dr["issuedetailid"] = lblissuedetailid.Text;
                    dr["empid"] = listWeaverName.Items[i].Value;
                    dtrecord.Rows.Add(dr);
                }
            }
            //
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", hnissueorderid.Value);
            param[1] = new SqlParameter("@processid", 1);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
            param[5] = new SqlParameter("@dtrecord", dtrecord);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFolioEmployee", param);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[3].Value.ToString() + "')", true);

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
        //********************
        #region
        //try
        //{
        //    if (DGOrderdetail.Rows.Count == 0)
        //    {
        //        Tran.Commit();
        //        return;
        //    }
        //    string str = "";
        //    //Delete And Update Existing record
        //    str = @"Delete from Employee_ProcessOrderNo Where IssueOrderId=" + hnissueorderid.Value + @" And ProcessId=1";

        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
        //    //

        //    for (int i = 0; i < listWeaverName.Items.Count; i++)
        //    {
        //        for (int j = 0; j < DGOrderdetail.Rows.Count; j++)
        //        {
        //            str = "Insert into Employee_ProcessOrderNo (ProcessId,IssueOrderId,IssueDetailId,Empid)values(1," + ((Label)DGOrderdetail.Rows[j].FindControl("lblissueorderid")).Text + "," + ((Label)DGOrderdetail.Rows[j].FindControl("lblissuedetailid")).Text + "," + listWeaverName.Items[i].Value + ")";
        //            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

        //        }
        //    }
        //    Tran.Commit();
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Employee updated successfully...')", true);

        //}
        //catch (Exception ex)
        //{
        //    lblmessage.Text = ex.Message;
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //}
        #endregion
    }
    protected void btnupdateconsmp_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@ChkForDyeingConsumption", ChkForDyeingConsumption.Checked == true ? "1" : "0");
            param[3] = new SqlParameter("@IssueDate", txtissuedate.Text);
            param[4] = new SqlParameter("@MasterCompanyId", Session["varcompanyNo"]);

            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatecurrentconsmpLoomWise", param);
            //******
            lblmessage.Text = param[1].Value.ToString();
            Tran.Commit();
            FillConsumptionQty();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DDemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (DDemployee.SelectedIndex > 0)
            {
                str = @"select EMp.Empid,Pm.IssueOrderId  from dbo.PROCESS_ISSUE_MASTER_1 PM inner join dbo.PROCESS_ISSUE_DETAIL_1 PD
                        on PM.IssueOrderId=Pd.IssueOrderId  and Pd.PQty>0
                        inner join  dbo.Employee_ProcessOrderNo EMP on EMP.IssueOrderId=PM.IssueOrderId and EMP.ProcessId=1
                        inner join EmpInfo EI on Ei.EmpId=EMP.Empid
                        And EI.empid=" + DDemployee.SelectedValue;


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Folio -" + ds.Tables[0].Rows[0]["IssueOrderId"] + " Already pending at this ID No..');", true);
                    return;
                }

                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");

                if (listWeaverName.Items.FindByValue(DDemployee.SelectedValue) == null)
                {

                    listWeaverName.Items.Add(new ListItem(DDemployee.SelectedItem.Text, DDemployee.SelectedValue));
                }

                //txtWeaverIdNo.Text = "";


                ds.Dispose();
            }
            // txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtgetvalue.Text != "")
        {
            FillWeaver();
        }
    }
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select EmpID 
                    From Empinfo(Nolock) Where EmpCode = '" + txteditempcode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            txteditempid.Text = ds.Tables[0].Rows[0]["EmpID"].ToString();
        }

        FIllProdUnit(sender);
    }
    protected void FIllProdUnit(object sender = null)
    {
        string str = @"select Distinct U.UnitsId,U.UnitName,PIm.CompanyId From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        Where PIM.Companyid=" + DDcompany.SelectedValue;

        string str1 = @" Select Top 1 PIM.CompanyID, PIM.Units, PIM.IssueOrderID From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        JOIN Empinfo EI(Nolock) ON EI.EmpID = EMP.EmpID 
                        Where PIM.Companyid=" + DDcompany.SelectedValue;

        if (txteditempid.Text != "")
        {
            str = str + " and EMP.EMPID=" + txteditempid.Text;
            str1 = str1 + " and EMP.EMPID=" + txteditempid.Text;
        }
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
            str1 = str1 + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

            ///str = str + " and EMP.Issueorderid=" + txtfolionoedit.Text;
        }

        if (txteditempcode.Text != "")
        {
            str1 = str1 + " And EI.EMPCode = '" + txteditempcode.Text + "'";
        }

        str = str + " order by Unitname ";
        str1 = str1 + " order by PIM.IssueOrderID Desc";
        str = str + str1;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 0, true, "--Plz Select--");

        //UtilityModule.ConditionalComboFill(ref DDProdunit, str, true, "--Plz Select--");
        if (DDProdunit.Items.Count > 0)
        {
            if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
            {
                DDProdunit.SelectedValue = ds.Tables[1].Rows[0]["Units"].ToString();
            }
            else
            {
                DDProdunit.SelectedIndex = 1;
            }
            DDProdunit_SelectedIndexChanged(sender, new EventArgs());
        }

    }
    protected void FillWeaver()
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNo.Text != "")
            {

                DataSet ds = null;

                if (Session["varCompanyId"].ToString() == "21")
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, 1 Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + txtgetvalue.Text;
                }
                else if (Session["varSubCompanyId"].ToString() == "283")
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, Case When " + ChkForPcsWise.Checked + @" == true Then 1 Else Case When EI.Employeetype = 1 Then 0 Else 1 End End Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + txtgetvalue.Text;
                }
                else
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, Case When EI.Employeetype = 1 Then 0 Else 1 End Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + txtgetvalue.Text;
                }

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (hnEmployeeType.Value == "0")
                    {
                        hnEmployeeType.Value = ds.Tables[0].Rows[0]["Emptype"].ToString();
                    }
                    else if (hnEmployeeType.Value != ds.Tables[0].Rows[0]["Emptype"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Please select same location employee');", true);
                        return;
                    }
                    if ((Session["varCompanyId"].ToString() == "28" || Session["varCompanyId"].ToString() == "16") && ds.Tables[0].Rows[0]["emptype"].ToString() == "0")
                    {
                        SqlParameter[] param = new SqlParameter[4];
                        param[0] = new SqlParameter("@CardNo", txtWeaverIdNoscan.Text);
                        param[1] = new SqlParameter("@UserID", Session["varuserid"]);
                        param[2] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);
                        param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                        param[3].Direction = ParameterDirection.Output;
                        //*************
                        DataSet dsnew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetEmployeeIsPresentOrNot", param);

                        if (dsnew.Tables[0].Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee is absent so please process attendance');", true);
                            //if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281") 
                            //{
                            //    txtWeaverIdNoscan.Text = "";
                            //    txtWeaverIdNoscan.Focus();
                            //    return;
                            //}
                        }
                    }
                    //***********CHECK LOCATION
                    Boolean addflag = true;
                    if (variable.VarINTERNALPRODUCTION_AREAWISE == "0")
                    {
                        switch (ds.Tables[0].Rows[0]["emptype"].ToString())
                        {
                            case "-1": //NO Location set
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employeeloc", "alert('Please Define Location of Employee first in Employee Master.');", true);
                                addflag = false;
                                break;
                            case "0":  // inside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                    if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "calinside", "alert('Employee Location Should be same in Employee Master.');", true);
                                        addflag = false;
                                    }
                                }
                                break;
                            case "1":  //Outside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                    if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Location Should be same in Employee Master.');", true);
                                        addflag = false;
                                    }

                                }
                                break;
                            default:
                                if (DDCalType.Enabled == true)
                                {
                                    hnordercaltype.Value = "1";
                                    DDCalType.SelectedValue = hnordercaltype.Value;
                                }
                                break;
                        }
                    }
                    if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
                    {
                        if (hnEmpWagescalculation.Value == "")
                        {
                            hnEmpWagescalculation.Value = ds.Tables[0].Rows[0]["Wagescalculation"].ToString();
                        }
                        else
                        {
                            if (hnEmpWagescalculation.Value.ToString() != ds.Tables[0].Rows[0]["Wagescalculation"].ToString())
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Wages Calculation Should be same in BIO DATA ENTRY.');", true);
                                addflag = false;
                            }

                        }
                    }

                    //*********END
                    if (addflag == true)
                    {
                        if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                        {
                            listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));

                            //if (Session["VarCompanyNo"].ToString() == "27" && hnEmployeeType.Value == "1")
                            if (hnEmployeeType.Value == "1")
                            {
                                hnEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                            }
                            else
                            {
                                hnEmpId = 0;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);

                }
                if (variable.VarINTERNALPRODUCTION_AREAWISE == "1")
                {
                    if (DDCalType.Enabled == true)
                    {
                        hnordercaltype.Value = "0";
                        DDCalType.SelectedValue = hnordercaltype.Value;
                    }
                }

                ds.Dispose();
                if (Session["varcompanyId"].ToString() == "22")
                {
                    tdweaverpedningstock.Visible = true;
                    //  string a = txtWeaverIdNoscan.Text;
                    string strweaver = @"SELECT  VF.DESIGNNAME,SUM(PID.QTY) AS QTY,SUM(PID.PQTY) AS PQTY,EI.EMPCODE
 FROM PROCESS_ISSUE_MASTER_1 PIM INNER JOIN PROCESS_ISSUE_DETAIL_1 PID ON PIM.ISSUEORDERID=PID.ISSUEORDERID  
 AND PIM.STATUS<>'CANCELED' AND PIM.EMPID=0   
 INNER JOIN V_FINISHEDITEMDETAIL VF ON PID.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID  
 CROSS APPLY (SELECT * FROM DBO.F_GETEMPLOYEEANDEMPTYPE(1,PIM.ISSUEORDERID)) AS EI WHERE PIM.COMPANYID=1 AND EMPCODE='" + txtWeaverIdNo.Text + "' and   PIM.Assigndate>DATEADD(M,-2,GETDATE()) GROUP BY EI.EMPCODE,VF.DESIGNNAME having SUM(PID.PQTY)>0 ";
                    DataSet dsweaver = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strweaver);

                    if (dsweaver != null)
                    {
                        if (dsweaver.Tables.Count > 0)
                        {
                            if (dsweaver.Tables[0].Rows.Count > 0)
                            {

                                grdpendingstock.DataSource = dsweaver;
                                grdpendingstock.DataBind();

                            }

                        }

                    }
                }



                txtWeaverIdNo.Text = "";
                DisablecontrolwithGENERATESTOCKNOONTAGGING();
            }

            txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void FillWeaverEMP()
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (Convert.ToInt32(ddempname.SelectedValue)>0)
            {

                DataSet ds = null;

                if (Session["varCompanyId"].ToString() == "21")
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, 1 Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + txtgetvalue.Text;
                }
                else if (Session["varSubCompanyId"].ToString() == "283")
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, Case When " + ChkForPcsWise.Checked + @" == true Then 1 Else Case When EI.Employeetype = 1 Then 0 Else 1 End End Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + txtgetvalue.Text;
                }
                else
                {
                    str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, Case When EI.Employeetype = 1 Then 0 Else 1 End Caltype, 
                            IsNull(EID.Wagescalculation, 0) Wagescalculation 
                            From EmpInfo EI(Nolock)
                            LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                            Where EI.Blacklist = 0 And EI.EmpID = " + ddempname.SelectedValue;
                }

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (hnEmployeeType.Value == "0")
                    {
                        hnEmployeeType.Value = ds.Tables[0].Rows[0]["Emptype"].ToString();
                    }
                    else if (hnEmployeeType.Value != ds.Tables[0].Rows[0]["Emptype"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Please select same location employee');", true);
                        return;
                    }
                    if ((Session["varCompanyId"].ToString() == "28" || Session["varCompanyId"].ToString() == "16") && ds.Tables[0].Rows[0]["emptype"].ToString() == "0")
                    {
                        SqlParameter[] param = new SqlParameter[4];
                        param[0] = new SqlParameter("@CardNo", txtWeaverIdNoscan.Text);
                        param[1] = new SqlParameter("@UserID", Session["varuserid"]);
                        param[2] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);
                        param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                        param[3].Direction = ParameterDirection.Output;
                        //*************
                        DataSet dsnew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetEmployeeIsPresentOrNot", param);

                        if (dsnew.Tables[0].Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee is absent so please process attendance');", true);
                            //if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281") 
                            //{
                            //    txtWeaverIdNoscan.Text = "";
                            //    txtWeaverIdNoscan.Focus();
                            //    return;
                            //}
                        }
                    }
                    //***********CHECK LOCATION
                    Boolean addflag = true;
                    if (variable.VarINTERNALPRODUCTION_AREAWISE == "0")
                    {
                        switch (ds.Tables[0].Rows[0]["emptype"].ToString())
                        {
                            case "-1": //NO Location set
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employeeloc", "alert('Please Define Location of Employee first in Employee Master.');", true);
                                addflag = false;
                                break;
                            case "0":  // inside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                    if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "calinside", "alert('Employee Location Should be same in Employee Master.');", true);
                                        addflag = false;
                                    }
                                }
                                break;
                            case "1":  //Outside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                    if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Location Should be same in Employee Master.');", true);
                                        addflag = false;
                                    }

                                }
                                break;
                            default:
                                if (DDCalType.Enabled == true)
                                {
                                    hnordercaltype.Value = "1";
                                   // DDCalType.SelectedValue = hnordercaltype.Value;
                                }
                                break;
                        }
                    }
                    if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
                    {
                        if (hnEmpWagescalculation.Value == "")
                        {
                            hnEmpWagescalculation.Value = ds.Tables[0].Rows[0]["Wagescalculation"].ToString();
                        }
                        else
                        {
                            if (hnEmpWagescalculation.Value.ToString() != ds.Tables[0].Rows[0]["Wagescalculation"].ToString())
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Wages Calculation Should be same in BIO DATA ENTRY.');", true);
                                addflag = false;
                            }

                        }
                    }

                    //*********END
                    if (addflag == true)
                    {
                        if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                        {
                            listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));

                            //if (Session["VarCompanyNo"].ToString() == "27" && hnEmployeeType.Value == "1")
                            if (hnEmployeeType.Value == "1")
                            {
                                hnEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                            }
                            else
                            {
                                hnEmpId = 0;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);

                }
                if (variable.VarINTERNALPRODUCTION_AREAWISE == "1")
                {
                    if (DDCalType.Enabled == true)
                    {
                        hnordercaltype.Value = "0";
                        //DDCalType.SelectedValue = hnordercaltype.Value;
                    }
                }

                ds.Dispose();
                if (Session["varcompanyId"].ToString() == "22")
                {
                    tdweaverpedningstock.Visible = true;
                    //  string a = txtWeaverIdNoscan.Text;
                    string strweaver = @"SELECT  VF.DESIGNNAME,SUM(PID.QTY) AS QTY,SUM(PID.PQTY) AS PQTY,EI.EMPCODE
 FROM PROCESS_ISSUE_MASTER_1 PIM INNER JOIN PROCESS_ISSUE_DETAIL_1 PID ON PIM.ISSUEORDERID=PID.ISSUEORDERID  
 AND PIM.STATUS<>'CANCELED' AND PIM.EMPID=0   
 INNER JOIN V_FINISHEDITEMDETAIL VF ON PID.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID  
 CROSS APPLY (SELECT * FROM DBO.F_GETEMPLOYEEANDEMPTYPE(1,PIM.ISSUEORDERID)) AS EI WHERE PIM.COMPANYID=1 AND EMPID='" + ddempname.SelectedValue + "' and   PIM.Assigndate>DATEADD(M,-2,GETDATE()) GROUP BY EI.EMPCODE,VF.DESIGNNAME having SUM(PID.PQTY)>0 ";
                    DataSet dsweaver = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strweaver);

                    if (dsweaver != null)
                    {
                        if (dsweaver.Tables.Count > 0)
                        {
                            if (dsweaver.Tables[0].Rows.Count > 0)
                            {

                                grdpendingstock.DataSource = dsweaver;
                                grdpendingstock.DataBind();

                            }

                        }

                    }
                }



                txtWeaverIdNo.Text = "";
                DisablecontrolwithGENERATESTOCKNOONTAGGING();
            }

            txtWeaverIdNo.Focus();

        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DisablecontrolwithGENERATESTOCKNOONTAGGING()
    {
        //*******Chages For Scanning Tagging Stock No
        if (variable.VarGENERATESTOCKNOONTAGGING == "1" && hnEmployeeType.Value == "0")
        {
            if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
            {
                if (chkEdit.Checked == true)
                {
                    Tdstockno.Visible = true;
                }
            }
            else
            {
                btnsave.Visible = false;
                DDcustcode.Enabled = false;
                DDorderNo.Enabled = false;
                Tdstockno.Visible = true;
            }

        }
        //********
    }
    protected void EnablecontrolwithGENERATESTOCKNOONTAGGING()
    {
        //*******Chages For Scanning Tagging Stock No

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
        }
        else
        {
            btnsave.Visible = true;
        }
        DDcustcode.Enabled = true;
        DDorderNo.Enabled = true;
        Tdstockno.Visible = false;

        //********
    }
    protected void FillWeaverWithBarcodescan()
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNoscan.Text != "")
            {
                DataSet ds = null;

                str = @"Select EI.Empid, EI.Empcode + '-' + EI.Empname EmpName, IsNull(EI.EmployeeType, 0) Emptype, 
                        Case When EI.Employeetype = 1 Then 0 Else 1 End Caltype, IsNull(EID.Wagescalculation, 0) Wagescalculation 
                        From EmpInfo EI(Nolock)
                        LEFT JOIN HR_EMPLOYEEINFORMATION EID(Nolock) ON EID.EMPID = EI.EMPID 
                        Where EI.Blacklist = 0 And EI.EmpCode = '" + txtWeaverIdNoscan.Text + "'";              

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (hnEmployeeType.Value == "0")
                    {
                        hnEmployeeType.Value = ds.Tables[0].Rows[0]["Emptype"].ToString();
                    }
                    else if (hnEmployeeType.Value != ds.Tables[0].Rows[0]["Emptype"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Please select same location employee');", true);
                        return;
                    }

                    if ((Session["varCompanyId"].ToString() == "28" || Session["varCompanyId"].ToString() == "16") && ds.Tables[0].Rows[0]["emptype"].ToString() == "0")
                    {
                        SqlParameter[] param = new SqlParameter[4];
                        param[0] = new SqlParameter("@CardNo", txtWeaverIdNoscan.Text);
                        param[1] = new SqlParameter("@UserID", Session["varuserid"]);
                        param[2] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);
                        param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                        param[3].Direction = ParameterDirection.Output;
                        //*************
                        DataSet dsnew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetEmployeeIsPresentOrNot", param);

                        if (dsnew.Tables[0].Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee is absent so please process attendance');", true);
                            //if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281")
                            //{
                            //    txtWeaverIdNoscan.Text = "";
                            //    txtWeaverIdNoscan.Focus();
                            //    return;
                            //}
                        }
                    }
                    //***********CHECK LOCATION
                    Boolean addflag = true;
                    if (variable.VarINTERNALPRODUCTION_AREAWISE == "0" && chkEdit.Checked == false)
                    {
                        switch (ds.Tables[0].Rows[0]["emptype"].ToString())
                        {
                            case "-1": //NO Location set
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employeeloc", "alert('Please Define Location of Employee first in Employee Master.');", true);
                                addflag = false;
                                break;
                            case "0":  // inside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                   
                                        if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "calinside", "alert('Employee Location Should be same in Employee Master.');", true);
                                            addflag = false;
                                        }
                                   
                                }
                                break;
                            case "1":  //Outside
                                if (hnordercaltype.Value == "")
                                {
                                    if (DDCalType.Enabled == true)
                                    {
                                        hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                                        DDCalType.SelectedValue = hnordercaltype.Value;
                                    }
                                }
                                else
                                {
                                    
                                        if (hnordercaltype.Value.ToString() != ds.Tables[0].Rows[0]["caltype"].ToString())
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Location Should be same in Employee Master.');", true);
                                            addflag = false;
                                        }
                                    
                                }
                                break;
                            default:
                                if (DDCalType.Enabled == true)
                                {
                                    hnordercaltype.Value = "1";
                                    DDCalType.SelectedValue = hnordercaltype.Value;
                                }
                                break;
                        }
                    }

                    if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
                    {
                        if (hnEmpWagescalculation.Value == "")
                        {
                            hnEmpWagescalculation.Value = ds.Tables[0].Rows[0]["Wagescalculation"].ToString();
                        }
                        else
                        {
                            if (hnEmpWagescalculation.Value.ToString() != ds.Tables[0].Rows[0]["Wagescalculation"].ToString())
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "caloutside", "alert('Employee Wages Calculation Should be same in BIO DATA ENTRY.');", true);
                                addflag = false;
                            }

                        }
                    }
                    //*********END
                    if (addflag == true)
                    {
                        if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                        {
                            listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                            hnEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                        }
                    }
                    txtWeaverIdNoscan.Text = "";

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                    txtWeaverIdNoscan.Text = "";
                }
                ds.Dispose();
            }
            if (variable.VarINTERNALPRODUCTION_AREAWISE == "1")
            {
                if (DDCalType.Enabled == true)
                {
                    hnordercaltype.Value = "0";
                    DDCalType.SelectedValue = hnordercaltype.Value;
                }
            }
            txtWeaverIdNoscan.Focus();
            DisablecontrolwithGENERATESTOCKNOONTAGGING();
        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
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
                dr["Processid"] = 1;
                dr["issueorderid"] = DDFolioNo.SelectedValue;
                dtrecord.Rows.Add(dr);
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@dtrecord", dtrecord);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Processid", 1);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
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
    protected void btnactiveemployee_Click(object sender, EventArgs e)
    {
        lblpopupmsg.Text = "";
        FillEmployeeForDeactive();
        ModalpopupextDeactivefolio.Show();
    }
    protected void FillEmployeeForDeactive()
    {
        string str = @"select Distinct EI.EmpName+'('+EI.EmpCode+')' as Employee,EMP.IssueOrderId,Emp.ActiveStatus,Ei.Empid From Employee_ProcessOrderNo EMP inner Join EmpInfo EI on Emp.Empid=Ei.EmpId
                   and EMP.ProcessId=1 and EMP.IssueOrderId=" + DDFolioNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
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
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        DDLoomNo.SelectedIndex = -1;
        DDFolioNo.SelectedIndex = -1;
        hnissueorderid.Value = "0";
        enablecontrols();
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
    }
    protected void TxtPrefix_TextChanged(object sender, EventArgs e)
    {
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFixLoom((TxtPrefix.Text).ToUpper()));
    }
    protected void TxtPostfix_TextChanged(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        string TStockNo = TxtPrefix.Text + TxtPostfix.Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select TStockNo from LoomstockNo(Nolock) Where TStockNo='" + TStockNo + "' And CompanyId=" + DDcompany.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            lblmessage.Text = "Stock No already exists....";
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFixLoom((TxtPrefix.Text).ToUpper()));
        }
    }
    protected void lnkdelClick(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lnkdel = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

            Label lblissueorderid = (Label)gvr.FindControl("lblissueorderid");
            Label lblissuedetailid = (Label)gvr.FindControl("lblissuedetailid");
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPRODUCTIONORDER", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillGrid();
            FillConsumptionQty();
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
    //protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    //{
    //    FillWeaverWithBarcodescan();
    //    //txtWeaverIdNoscan.Focus();
    //    //Page.SetFocus("CPH_Form_txtWeaverIdNoscan");
    //}
    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void Txtwidthlength_TextChanged(object sender, EventArgs e)
    {
        TextBox txtwidthlength = (TextBox)sender;
        GridViewRow gvr = (GridViewRow)txtwidthlength.NamingContainer;
        Check_Length_Width_Format(gvr.RowIndex);
    }
    private void Check_Length_Width_Format(int rowindex = 0)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";

        TextBox Txtlength = (TextBox)DG.Rows[rowindex].FindControl("txtlength");
        TextBox TxtWidth = (TextBox)DG.Rows[rowindex].FindControl("txtwidth");
        //Label lblunitid = ddunit.SelectedValue.ToArray;
        Label lblarea = (Label)DG.Rows[rowindex].FindControl("lblarea");
        // Label lblcaltype = (Label)DG.Rows[rowindex].FindControl("lblcaltype");
        Label lblshapeid = (Label)DG.Rows[rowindex].FindControl("lblshapeid");

        if (Txtlength.Text != "")
        {
            if (Convert.ToInt32(ddunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(Txtlength.Text));
                Txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    Txtlength.Text = "";
                    Txtlength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(ddunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (Txtlength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = Convert.ToInt16(lblshapeid.Text);

            if (Convert.ToInt32(ddunit.SelectedValue) == 1)
            {
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(hnordercaltype.Value), Shape));
            }
            if (Convert.ToInt32(ddunit.SelectedValue) == 2 || Convert.ToInt16(ddunit.SelectedValue) == 6)
            {
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(hnordercaltype.Value), Shape, UnitId: Convert.ToInt16(ddunit.SelectedValue)));
            }
        }
    }
    protected void disablecontrols()
    {
        txtWeaverIdNo.Enabled = false;
        txtWeaverIdNoscan.Enabled = false;
        btnDelete.Enabled = false;
    }
    protected void enablecontrols()
    {
        txtWeaverIdNo.Enabled = true;
        txtWeaverIdNoscan.Enabled = true;
        btnDelete.Enabled = true;
    }
    protected void EnabledRateTextBox()
    {
        if (chkforRateUpdate.Checked == true)
        {
            if (DG.Rows.Count > 0)
            {
                for (int k = 0; k < DG.Rows.Count; k++)
                {
                    TextBox txtrate = ((TextBox)DG.Rows[k].FindControl("txtrate"));
                    TextBox txtcommrate = ((TextBox)DG.Rows[k].FindControl("txtcommrate"));
                    txtrate.Enabled = true;
                    txtcommrate.Enabled = true;
                }
            }
        }
        else
        {
            if (DG.Rows.Count > 0)
            {
                for (int k = 0; k < DG.Rows.Count; k++)
                {
                    TextBox txtrate = ((TextBox)DG.Rows[k].FindControl("txtrate"));
                    TextBox txtcommrate = ((TextBox)DG.Rows[k].FindControl("txtcommrate"));
                    txtrate.Enabled = false;
                    txtcommrate.Enabled = false;
                    //txtrate.Text = "0";
                }
            }
        }

    }
    protected void chkforRateUpdate_CheckedChanged(object sender, EventArgs e)
    {

        if (chkforRateUpdate.Checked == true)
        {
            ChkUpdateRateFlag = "";
            ChkUpdateRateFlag = "True";
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            ChkUpdateRateFlag = "";
            EnabledRateTextBox();
        }


    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        if (variable.VarLoomProductionRateUpdatePwd == txtpwd.Text)
        {
            if (ChkUpdateRateFlag == "True")
            {
                EnabledRateTextBox();
            }
            Popup(false);
        }
        else
        {
            lblmessage.Visible = true;
            lblmessage.Text = "Please Enter Correct Password..";
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
        FillissueDetails();
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        FIllProdUnit(sender);
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

            array[0].Value = hnissueorderid.Value;
            array[1].Value = 1;
            array[2].Value = Session["varcompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionConsumptionOrderReport", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Session["VarcompanyId"]) == 27 || Convert.ToInt32(Session["VarcompanyId"]) == 34)//For Antique Panipat
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
            lblmessage.Text = ex.Message;
            lblmessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void txtstockno_TextChanged(object sender, EventArgs e)
    {
        if ((Session["varcompanyid"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28") && chkEdit.Checked == true)
        {
            StockNoTextChanged();
        }
        else
        {
            lblmessage.Text = "";
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@unitid", ddunit.SelectedValue);
                param[1] = new SqlParameter("@unitname", ddunit.SelectedItem.Text);
                param[2] = new SqlParameter("@Produnitid", DDProdunit.SelectedValue);
                param[3] = new SqlParameter("@ordercaltype", hnordercaltype.Value);
                param[4] = new SqlParameter("@Tstockno", txtstockno.Text);
                param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[5].Direction = ParameterDirection.Output;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETORDERDETAILWITHSTOCKNO", param);
                if (param[5].Value.ToString() != "")
                {
                    lblmessage.Text = param[5].Value.ToString();
                    DG.DataSource = null;
                    DG.DataBind();
                }
                else
                {
                    DG.DataSource = ds.Tables[0].DefaultView;
                    DG.DataBind();
                    for (int i = 0; i < DG.Rows.Count; i++)
                    {
                        CheckBox Chkboxitem = (CheckBox)DG.Rows[i].FindControl("Chkboxitem");
                        TextBox txtloomqty = (TextBox)DG.Rows[i].FindControl("txtloomqty");
                        Chkboxitem.Checked = true;
                        txtloomqty.Text = "1";
                    }
                    btnsave_Click(sender, new EventArgs());
                    DG.DataSource = null;
                    DG.DataBind();
                    txtstockno.Text = "";
                }
                txtstockno.Focus();
            }
            catch (Exception ex)
            {
                lblmessage.Text = ex.Message;
            }
        }
    }
    protected void StockNoTextChanged()
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@TStockNo", txtstockno.Text);
            param[1] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATESTOCKNOINPROCESSDETAIL", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillConsumptionQty();
            txtstockno.Text = "";
            txtstockno.Focus();
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
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");            
            if (txtqty != null)
            {
                if (hnordercaltype.Value == "1" && variable.VarGENERATESTOCKNOONTAGGING == "1")
                {
                    txtqty.Enabled = false;
                }
            }
            if (Convert.ToInt16(Session["usertype"]) > 2)
            {
                DGOrderdetail.Columns[12].Visible = false;
                DGOrderdetail.Columns[13].Visible = false;

                //DGOrderdetail.Columns[10].Visible = false;
                //DGOrderdetail.Columns[11].Visible = false;
            }

            for (int i = 0; i < DGOrderdetail.Columns.Count; i++)
            {
                if (DGOrderdetail.Columns[i].HeaderText == "Bonus" || DGOrderdetail.Columns[i].HeaderText == "Finisher Rate")
                {
                    if (Convert.ToInt32(Session["varcompanyId"]) == 42)
                    {
                        DGOrderdetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGOrderdetail.Columns[i].Visible = false;
                    }
                }
            }          
           
        }
       
        if (e.Row.RowType == DataControlRowType.DataRow && DGOrderdetail.EditIndex == e.Row.RowIndex)
        {
            TextBox txtrategrid = (TextBox)e.Row.FindControl("txtrategrid");
            TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");
            TextBox txtcommrategrid = (TextBox)e.Row.FindControl("txtcommrategrid");
            if (Session["VarCompanyNo"].ToString() == "22")
            {
                txtrategrid.Enabled = false;
            }
            else if (Session["VarCompanyNo"].ToString() == "42")
            {
                if (Convert.ToInt16(Session["usertype"]) == 1)
                {
                    txtrategrid.Enabled = true;
                    txtqty.Enabled = true;
                    txtcommrategrid.Enabled = true;
                }
                else
                {
                    txtrategrid.Enabled = false;
                    txtqty.Enabled = false;
                    txtcommrategrid.Enabled = false;
                }
            }
            else
            {
                if (Convert.ToInt16(Session["usertype"]) == 1)
                {
                    txtrategrid.Enabled = true;
                }
                else
                {
                    txtrategrid.Enabled = false;
                }
            }
        }        
    }
    protected void btnweaveridscan_Click(object sender, EventArgs e)
    {
        //*********Check Folio Pending
        switch (Session["varcompanyid"].ToString())
        {
            case "16":
            case "28":            
                string str = @"SELECT V.EMPID,V.issueorderid FROM V_FOLIOEMPID V INNER JOIN EMPINFO EI ON V.EMPID=EI.EMPID 
                            WHERE V.ACTIVESTATUS=1 AND V.FOLIO_STATUS='PENDING' and V.empcode='" + txtWeaverIdNoscan.Text + @"'
                            Select UserType From NewUserDetail Where UserID = " + Session["varuserid"];

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string msg = "Folio already pending on these employees i.e " + ds.Tables[0].Rows[0]["issueorderid"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('" + msg + "');", true);
                    if (Convert.ToInt32(ds.Tables[1].Rows[0]["UserType"]) > 2)
                    {
                        txtWeaverIdNoscan.Text = "";
                        txtWeaverIdNoscan.Focus();
                        return;
                    }
                }
                break;
            default:
                break;
        }
        //********
        if (txtWeaverIdNoscan.Text != "")
        {
            FillWeaverWithBarcodescan();
        }
    }
    protected void BtnUpdateTanaLotNo_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", hnissueorderid.Value);
            param[1] = new SqlParameter("@processid", 1);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
            param[5] = new SqlParameter("@TanaLotNo", txtTanaLotNo.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFolioTanaLotNo", param);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[3].Value.ToString() + "')", true);

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
        //********************

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DG.Columns.Count; i++)
            {
                switch (Session["varcompanyId"].ToString())
                {
                    case "27":
                    case "34":
                        TextBox txtloomqty = (TextBox)e.Row.FindControl("txtloomqty");
                        break;
                    case "42":
                        TextBox txtloomqty2 = (TextBox)e.Row.FindControl("txtloomqty");
                        txtloomqty2.Text = "";
                        TextBox txtwidth = (TextBox)e.Row.FindControl("txtwidth");
                        txtwidth.Enabled = false;
                        TextBox txtlength = (TextBox)e.Row.FindControl("txtlength");
                        txtlength.Enabled = false;
                        break;
                    default:
                        TextBox txtloomqty1 = (TextBox)e.Row.FindControl("txtloomqty");
                        txtloomqty1.Text = "";
                        break;
                }
            }
        }
    }
    protected void lnkStockNodelClick(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        if (DGOrderdetailStockNo.Rows.Count > 1)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                LinkButton lnkdel = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

                Label lblStockNo = (Label)gvr.FindControl("LblStockNo");
                Label lblissueorderid = (Label)gvr.FindControl("lblissueorderid");
                Label lblissuedetailid = (Label)gvr.FindControl("lblissuedetailid");

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
                param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
                param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;
                param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
                param[4] = new SqlParameter("@Userid", Session["varuserid"]);
                param[5] = new SqlParameter("@StockNo", lblStockNo.Text);

                //********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPRODUCTIONORDER", param);
                lblmessage.Text = param[2].Value.ToString();
                Tran.Commit();
                FillGrid();
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
    }
    protected void DGOrderdetailStockNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt16(Session["usertype"]) > 2)
            {
                DGOrderdetailStockNo.Columns[3].Visible = false;
            }
        }
    }
    protected void BtnEmpPhoto_Click(object sender, EventArgs e)
    {
        SqlParameter[] arr = new SqlParameter[2];
        DataSet DS;
        //for (int i = 0; i < listWeaverName.Items.Count; i++)
        //{
        //arr[0] = new SqlParameter("@empid", listWeaverName.Items[i].Value);

        arr[0] = new SqlParameter("@empid", listWeaverName.SelectedValue);
        arr[1] = new SqlParameter("@ReportType", 1);

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_BIODATA", arr);

        if (DS.Tables[0].Rows.Count > 0)
        {
            DS.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
            foreach (DataRow dr in DS.Tables[0].Rows)
            {

                if (Convert.ToString(dr["empphoto"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["empphoto"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["Image"] = img_Byte;
                    }
                }
            }

            Session["rptFilename"] = "Reports/Rpt_Hr_BioDataForm.rpt";
            Session["dsFilename"] = "~\\ReportSchema\\Rpt_Hr_BioDataForm.xsd";
            Session["GetDataset"] = DS;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        //}
    }
    protected void StockNoStatus_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        DataSet DS;

        param[0] = new SqlParameter("@IssueOrderID", hnissueorderid.Value);
        param[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        param[1].Direction = ParameterDirection.Output;

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_StockNoStatus", param);
        if (param[1].Value != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[1].Value + "');", true);
            return;
        }
        if (DS.Tables[0].Rows.Count > 0)
        {
            Session["rptFilename"] = "Reports/RptStockNoStatus.rpt";
            Session["dsFilename"] = "~\\ReportSchema\\RptRptStockNoStatus.xsd";
            Session["GetDataset"] = DS;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void BtnOrderProcessToPNM_Click(object sender, EventArgs e)
    {
        OrderProcessToAllCompany(0);
    }
    protected void BtnOrderProcessToChampoPanipat_Click(object sender, EventArgs e)
    {
        OrderProcessToAllCompany(1);
    }
    protected void BtnOrderProcessToChampoPanipatPNM1_Click(object sender, EventArgs e)
    {
        OrderProcessToAllCompany(2);
    }
    protected void BtnOrderProcessToChampoPanipatPNM2_Click(object sender, EventArgs e)
    {
        OrderProcessToAllCompany(3);
    }
    protected void BtnOrderProcessToChampoPanipatPNM3_Click(object sender, EventArgs e)
    {
        OrderProcessToAllCompany(4);
    }
    private void OrderProcessToAllCompany(int TypeFlag)
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
            param[0] = new SqlParameter("@ISSUEORDERID", hnissueorderid.Value);
            param[1] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MASTERCOMPANYID", 28);
            param[3] = new SqlParameter("@USERID", 1);
            param[4] = new SqlParameter("@POUFTYPECATEGORY", 0);
            param[5] = new SqlParameter("@TYPEFLAG", TypeFlag);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Save_ChampoProductionOrder_CreateCustomerOrderInPNMERP", param);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[1].Value + "')", true);
            Tran.Commit();
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
    protected void DDCalType_SelectedIndexChanged(object sender, EventArgs e)
    {
       // hnordercaltype.Value = DDCalType.SelectedValue;
    }
    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hnEmployeeType.Value == "0")
        {
            
            //DDcustcode.Enabled = false;
            DDorderNo.Enabled = false;

            if (DDDepartmentName.SelectedIndex == 0)
            {
                TDDepartmentIssueNo.Visible = false;
                string Str = "Select CustomerId, CustomerCode From customerinfo(Nolock) Where MasterCompanyId = " + Session["varCompanyId"] + @" order by Customercode";

                UtilityModule.ConditionalComboFill(ref DDcustcode, Str, true, "--Plz Select--");
                DDcustcode.Enabled = true;
                DDorderNo.Enabled = true;
            }
            else
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
                param[1] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
                param[2] = new SqlParameter("@DepartmentID", DDDepartmentName.SelectedValue);
                param[3] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_UpdateDepartmentStatus", param);
                
                UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 0, true, "select customer code");

                if (Session["VarCompanyNo"].ToString() == "39")
                {
                    DDcustcode.Enabled = true;
                    btnsave.Visible = true;
                }
                TDDepartmentIssueNo.Visible = true;
            }
            DDorderNo.Items.Clear();
            DG.DataSource = null;
            DG.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please select internal employee');", true);
            DDDepartmentName.SelectedIndex = 0;
        }
    }
    private void FillDepartmentIssueNo()
    {
        string Str = @"Select Distinct a.IssueOrderID, a.IssueOrderID a
                From ProcessIssueToDepartmentMaster a(Nolock) ";
        if (DDcustcode.SelectedIndex > 0)
        {
            Str = Str + @" JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID And OM.CustomerId = " + DDcustcode.SelectedValue;
        }
        Str = Str + @" Where a.Status = 'Pending' And a.CompanyID = " + DDcompany.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.DepartmentID = " + DDDepartmentName.SelectedValue + @" 
                    And a.MasterCompanyId = " + Session["varCompanyId"] + " Order By a.IssueOrderID";

        UtilityModule.ConditionalComboFill(ref DDDepartmentIssueNo, Str, true, "--Plz Select--");
    }
    protected void DDDepartmentIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDepartmentIssueNo.SelectedIndex > 0)
        {
//            string Str = @"Select Distinct CI.CustomerId, CI.CustomerCode 
//                From ProcessIssueToDepartmentMaster a(Nolock) 
//                JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
//                JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID 
//                JOIN Customerinfo CI(Nolock)  ON CI.CustomerId = OM.CustomerId 
//                Where a.CompanyID = " + DDcompany.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.DepartmentID = " + DDDepartmentName.SelectedValue + @" 
//                    And a.MasterCompanyId = " + Session["varCompanyId"] + @" And OM.status = '0' And a.IssueOrderID = " + DDDepartmentIssueNo.SelectedValue + @" 
//                order by CI.Customercode ";
//            UtilityModule.ConditionalComboFill(ref DDcustcode, Str, true, "--Plz Select--");
//            DDcustcode.SelectedIndex = 1;
//            DDcustcode_SelectedIndexChanged(sender, new EventArgs());
//            DDorderNo.SelectedIndex = 1;

            if (DDDepartmentName.SelectedIndex > 0 && DDDepartmentIssueNo.SelectedIndex > 0)
            {
                string str = @"Select Distinct OM.OrderId, OM.CustomerOrderNo 
                From ProcessIssueToDepartmentMaster a(Nolock) 
                JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID 
                Where a.Status = 'Pending' And a.CompanyID = " + DDcompany.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.DepartmentID = " + DDDepartmentName.SelectedValue + @" 
                    And a.MasterCompanyId = " + Session["varCompanyId"] + " And OM.status = '0' And OM.CustomerId = " + DDcustcode.SelectedValue + @" 
                    And a.IssueOrderID = " + DDDepartmentIssueNo.SelectedValue + @" 
                Order By OM.OrderID ";

                UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
            }
            DDorderNo.SelectedIndex = 1;
            DDorderNo_SelectedIndexChanged(sender, new EventArgs());
        }
    }

    protected void BtnUpdateRemark_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", hnissueorderid.Value);
            param[1] = new SqlParameter("@processid", 1);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
            param[5] = new SqlParameter("@Remarks", TxtRemarks.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFolioRemarks", param);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[3].Value.ToString() + "')", true);

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
        //********************

    }
    protected void gsttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        decimal qty = 0, amount = 0, total = 0, rate = 0;
        DropDownList DDGSType = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDGSType.Parent.Parent;
        TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        TextBox txtsgst = ((TextBox)row.FindControl("TXTSGST"));
        TextBox txtcgst = ((TextBox)row.FindControl("TXTCGST"));
        //TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        Label lblcategoryid = ((Label)row.FindControl("lblcategoryid"));
        Label lblitemid = ((Label)row.FindControl("lblitemid"));
        Label lblqualityid = ((Label)row.FindControl("lblqualityid"));
        TextBox txtqty = ((TextBox)row.FindControl("txtloomqty"));
        Label lblamount = ((Label)row.FindControl("lblamount"));
        Label lbltotalamt = ((Label)row.FindControl("lbltotalamt"));
        TextBox txtrate = (TextBox)row.FindControl("txtrate");

         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ProcessId", "9");
            param[1] = new SqlParameter("@CategoryId",Convert.ToInt32( string.IsNullOrEmpty(lblcategoryid.Text)?"0":lblcategoryid.Text));
            param[2] = new SqlParameter("@ItemId", Convert.ToInt32(string.IsNullOrEmpty(lblitemid.Text) ? "0" : lblitemid.Text));
            param[3] = new SqlParameter("@QualityId", Convert.ToInt32(string.IsNullOrEmpty(lblqualityid.Text) ? "0" : lblqualityid.Text));
            param[4] = new SqlParameter("@EffectiveDate", txtissuedate.Text);
            param[5] = new SqlParameter("@GSTType", DDGSType.SelectedValue);
            param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[10] = new SqlParameter("@BranchID", DDcompany.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

            if (DDGSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
                {
                    txtcgst.Text = param[6].Value.ToString();
                    txtsgst.Text = param[7].Value.ToString();
                    txtigst.Text = param[8].Value.ToString();
                   // fill_text();
                }
                else
                {
                    txtcgst.Text = "0";
                    txtsgst.Text = "0";
                    txtigst.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add GST/IGST regarding selected item";
                    return;
                }
            }
            else
            {
                txtcgst.Text = "0";
                txtsgst.Text = "0";
                txtigst.Text = "0";
               // fill_text();
            }

            Tran.Commit();
            qty = Convert.ToDecimal(txtqty.Text == "" ? "0" : txtqty.Text);
            rate = Convert.ToDecimal(txtrate.Text == "" ? "0" : txtrate.Text);
            amount = qty * rate;
            lblamount.Text = Convert.ToString(amount);
            if (Convert.ToInt32(DDGSType.SelectedValue) > 0)
            {
                if (DDGSType.SelectedValue == "1")
                {
                    total =amount+( (amount * (Convert.ToDecimal(txtcgst.Text) + Convert.ToDecimal(txtsgst.Text)))/100);


                }
                else { total =amount+( (amount * (Convert.ToDecimal(txtigst.Text)))/100); }

            }
            lbltotalamt.Text = Convert.ToString(total);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            //lblerrormessage.Text = ex.Message;
            con.Close();
        }


        //if (DDGSType.SelectedValue == "1")
        //{
            //TDCGST.Visible = true;
            //TDSGST.Visible = true;
            //TDIGST.Visible = false;
        //    FillGSTIGST(Convert.ToInt32(lblcategoryid.Text), Convert.ToInt32(lblitemid.Text), Convert.ToInt32(lblqualityid.Text),Convert.ToInt32(DDGSType.SelectedValue));
        //}
        //else if (DDGSType.SelectedValue == "2")
        //{
        //    //TDCGST.Visible = false;
        //    //TDSGST.Visible = false;
        //    //TDIGST.Visible = true;
        //    FillGSTIGST(Convert.ToInt32(lblcategoryid.Text), Convert.ToInt32(lblcategoryid.Text), Convert.ToInt32(lblcategoryid.Text), Convert.ToInt32(DDGSType.SelectedValue));
        //}
        //else
        //{
            //TDCGST.Visible = false;
            //TDSGST.Visible = false;
            //TDIGST.Visible = false;
            //txtCGST.Text = "0";
            //txtSGST.Text = "0";
            //txtIGST.Text = "0";
            //fill_text();
       // }
        //Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        //DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        //DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        //if (Session["VarcompanyNo"].ToString() == "22")
        //{
        //    SqlParameter[] array = new SqlParameter[4];
        //    array[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
        //    array[1] = new SqlParameter("@GodownId", ddlgodown.SelectedValue);
        //    array[2] = new SqlParameter("@item_finished_id", Ifinishedid.Text);
        //    array[3] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyNo"]);
        //    array[4] = new SqlParameter("@BinNo", variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "");

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DDBIND_LOTNO", array);
        //}
    }
    protected void txtrate_TextChanged(object sender, EventArgs e)
    {
        decimal qty=0,amount=0,total=0,rate=0;
        TextBox txtrate = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtrate.Parent.Parent;
        TextBox txtqty = ((TextBox)row.FindControl("txtloomqty"));
        Label lblamount = ((Label)row.FindControl("lblamount"));
        Label lbltotalamt = ((Label)row.FindControl("lbltotalamt"));
        TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        TextBox txtsgst = ((TextBox)row.FindControl("TXTSGST"));
        TextBox txtcgst = ((TextBox)row.FindControl("TXTCGST"));
         DropDownList DDGSType = (DropDownList)row.FindControl("gsttype");
        qty=Convert.ToDecimal(txtqty.Text==""?"0":txtqty.Text);
         rate=Convert.ToDecimal(txtrate.Text==""?"0":txtrate.Text);
        amount=qty*rate;
        lblamount.Text=Convert.ToString(amount);
        if(Convert.ToInt32(DDGSType.SelectedValue)>0)
        {
            if (DDGSType.SelectedValue == "1")
            {
                total = amount + ((amount * (Convert.ToDecimal(txtcgst.Text) + Convert.ToDecimal(txtsgst.Text))) / 100);


            }
            else { total = amount + ((amount * (Convert.ToDecimal(txtigst.Text))) / 100); }
            //if(DDGSType.SelectedValue=="1")
            //{
            //    total=amount*(Convert.ToDecimal(txtcgst.Text)+Convert.ToDecimal(txtsgst.Text));

            
            //}
            //else{ total=amount*(Convert.ToDecimal(txtigst.Text));}

        }
        lbltotalamt.Text =Convert.ToString(total);

    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {

      //  DropDownList ll=(DropDownList)(sender);
        string strf = string.Empty;
        //                UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo from ProductionLoomMaster PM 
        //                                            Left join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
        //                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
       // int val =Convert.ToInt32(ddempname.SelectedValue);
        if (chkEdit.Checked == true)
        {
            strf = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo 
                From Process_issue_master_1 PIM
                LEFT JOIN Employee_ProcessOrderNo EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMp.ProcessId=1   
                Where PIM.CompanyId=" + DDcompany.SelectedValue + @" 
                And IsNull(PIM.BranchID, 0) = " + DDBranchName.SelectedValue;

            if (chkcomplete.Checked == true)
            {
                strf = strf + " and PIM.Status='Complete'";
            }
            else
            {
                strf = strf + " and PIM.Status='Pending'";
            }
            if (ddempname.SelectedIndex >0)
            {
                strf = strf + " and EMP.EMPID=" + ddempname.SelectedValue + "";
            }
            if (txtfolionoedit.Text != "")
            {
                strf = strf + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";

                ////str = str + " and PIM.issueorderid=" + txtfolionoedit.Text + "";
            }
            strf = strf + " order by PIM.IssueOrderId desc";
            UtilityModule.ConditionalComboFill(ref DDFolioNo, strf, true, "--Plz Select--");
            if (DDFolioNo.Items.Count > 0)
            {
                DDFolioNo.SelectedIndex = 1;
                DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        else { FillWeaverEMP(); }
        //employee
//        strf = @"select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' as Empname from EmpInfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION'
//        and EI.Status='P' and EI.Blacklist=0 order by Empname";
//        UtilityModule.ConditionalComboFill(ref DDemployee, strf, true, "--Plz select--");





    }
    //private void FillGSTIGST(int categoryid,int itemid,int qualityid,int gstypeid)
    //{
    //    Label1.Visible = false;
    //    Label1.Text = "";
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    if (con.State == ConnectionState.Closed)
    //    {
    //        con.Open();
    //    }

    //    SqlTransaction Tran = con.BeginTransaction();
    //    try
    //    {
    //        SqlParameter[] param = new SqlParameter[11];
    //        param[0] = new SqlParameter("@ProcessId", "9");
    //        param[1] = new SqlParameter("@CategoryId",categoryid);
    //        param[2] = new SqlParameter("@ItemId", itemid);
    //        param[3] = new SqlParameter("@QualityId", qualityid);
    //        param[4] = new SqlParameter("@EffectiveDate", txtissuedate.Text);
    //        param[5] = new SqlParameter("@GSTType", gstypeid);
    //        param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
    //        param[6].Direction = ParameterDirection.Output;
    //        param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
    //        param[7].Direction = ParameterDirection.Output;
    //        param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
    //        param[8].Direction = ParameterDirection.Output;
    //        param[9] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
    //        param[10] = new SqlParameter("@BranchID", DDcompany.SelectedValue);

    //        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

    //        if (.SelectedIndex > 0)
    //        {
    //            if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
    //            {
    //                txtCGST.Text = param[6].Value.ToString();
    //                txtSGST.Text = param[7].Value.ToString();
    //                txtIGST.Text = param[8].Value.ToString();
    //                fill_text();
    //            }
    //            else
    //            {
    //                txtCGST.Text = "0";
    //                txtSGST.Text = "0";
    //                txtIGST.Text = "0";
    //                Label1.Visible = true;
    //                Label1.Text = "Please add GST/IGST regarding selected item";
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            txtCGST.Text = "0";
    //            txtSGST.Text = "0";
    //            txtIGST.Text = "0";
    //            fill_text();
    //        }

    //        Tran.Commit();
    //    }
    //    catch (Exception ex)
    //    {
    //        Tran.Rollback();
    //        //lblerrormessage.Text = ex.Message;
    //        con.Close();
    //    }
    //}
      protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;


        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            //str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            str = "select Designid,Designname From Design Where mastercompanyid=" + Session["varcompanyId"] + " order by designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            // str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            str = "select Colorid,colorname From color Where mastercompanyid=" + Session["varcompanyid"] + " order by colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapeid";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    //protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //UtilityModule.ConditionalComboFill(ref dddesign, "select Distinct Vf.designid,vf.designname From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and vf.designId>0 order by vf.designname", true, "--Plz Select");
    //}
    //protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref ddcolor, "select Distinct Vf.colorid,vf.ColorName From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and designId=" + dddesign.SelectedValue + " and vf.ColorId>0 order by colorname", true, "--Plz Select--");
    //}
    protected void Fillsize()
    {

        string str = null, size = null;
        if (variable.VarNewQualitySize == "1")
        {
            switch (ddunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "MtrSize";
                    }
                    else { size = "Production_Mt_Format"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From QualitySizeNew S Where S.shapeid=" + ddshape.SelectedValue;
            str = str + " order by S." + size;
        }
        else
        {
            switch (ddunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizemtr";
                    }
                    else { size = "Prodsizemtr"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From size S Where S.shapeid=" + ddshape.SelectedValue + "  and S.mastercompanyid=" + Session["varcompanyid"];
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    { }

    protected void chkwithoutorder_CheckedChanged(object sender, EventArgs e)
    {
        if (chkwithoutorder.Checked)
        {
            DDorderNo.Enabled = false;
            DDcustcode.Enabled = false;
            ddCatagory.Enabled = true;
            txtprodqty.Enabled = true;
            txtrateo.Enabled = true;
            txtamount.Enabled = true;
            ddltypegst.Enabled = true;
            TXTIGST.Enabled = true;
            TXTSGST.Enabled = true;
            TXTCGST.Enabled = true;
            DG.DataSource = null;
            DG.DataBind();
        }
        else {

            DDorderNo.Enabled = true;
            DDcustcode.Enabled = true;
            ddCatagory.Enabled = false;
            txtprodqty.Enabled = false;
            txtrateo.Enabled = false;
            txtamount.Enabled = false;
           ddltypegst.Enabled = false;
           TXTIGST.Enabled = false;
           TXTSGST.Enabled = false;
           TXTCGST.Enabled = false;

            ddCatagory.SelectedIndex = 0;
            ddCatagory_SelectedIndexChanged(sender, e);
        }

       // FillissueDetails();
    }

    protected void ddltypegst_SelectedIndexChanged(object sender, EventArgs e)
    { 
        decimal qty = 0, amount = 0, total = 0, rate = 0;
        DropDownList DDGSType = (DropDownList)sender;
       // GridViewRow row = (GridViewRow)DDGSType.Parent.Parent;
        //TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        //TextBox txtsgst = ((TextBox)row.FindControl("TXTSGST"));
        //TextBox txtcgst = ((TextBox)row.FindControl("TXTCGST"));
        //TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        //Label lblcategoryid = ((Label)row.FindControl("lblcategoryid"));
        //Label lblitemid = ((Label)row.FindControl("lblitemid"));
        //Label lblqualityid = ((Label)row.FindControl("lblqualityid"));
        //TextBox txtqty = ((TextBox)row.FindControl("txtloomqty"));
        //Label lblamount = ((Label)row.FindControl("lblamount"));
        //Label lbltotalamt = ((Label)row.FindControl("lbltotalamt"));
        //TextBox txtrate = (TextBox)row.FindControl("txtrate");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ProcessId", "9");
            param[1] = new SqlParameter("@CategoryId", Convert.ToInt32(string.IsNullOrEmpty(ddCatagory.SelectedValue) ? "0" : ddCatagory.SelectedValue));
            param[2] = new SqlParameter("@ItemId", Convert.ToInt32(string.IsNullOrEmpty(dditemname.SelectedValue) ? "0" : dditemname.SelectedValue));
            param[3] = new SqlParameter("@QualityId", Convert.ToInt32(string.IsNullOrEmpty(dquality.SelectedValue) ? "0" : dquality.SelectedValue));
            param[4] = new SqlParameter("@EffectiveDate", txtissuedate.Text);
            param[5] = new SqlParameter("@GSTType", ddltypegst.SelectedValue);
            param[6] = new SqlParameter("@CGSTRate", SqlDbType.Float);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@SGSTRate", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@IGSTRate", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[10] = new SqlParameter("@BranchID", DDcompany.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_GetCGST_SGST_IGST_Rate", param);

            if (DDGSType.SelectedIndex > 0)
            {
                if (param[6].Value.ToString() != "" && param[7].Value.ToString() != "" || param[8].Value.ToString() != "")
                {
                    TXTCGST.Text = param[6].Value.ToString();
                    TXTSGST.Text = param[7].Value.ToString();
                    TXTIGST.Text = param[8].Value.ToString();
                    // fill_text();
                }
                else
                {
                    TXTCGST.Text = "0";
                    TXTSGST.Text = "0";
                    TXTIGST.Text = "0";
                    Label1.Visible = true;
                    Label1.Text = "Please add GST/IGST regarding selected item";
                    return;
                }
            }
            else
            {
                TXTCGST.Text = "0";
                TXTSGST.Text = "0";
                TXTIGST.Text = "0";
                // fill_text();
            }

            Tran.Commit();
            qty = Convert.ToDecimal(txtprodqty.Text == "" ? "0" : txtprodqty.Text);
            rate = Convert.ToDecimal(txtrateo.Text == "" ? "0" : txtrateo.Text);
            amount = qty * rate;
            txtamount.Text = Convert.ToString(amount);
            if (Convert.ToInt32(DDGSType.SelectedValue) > 0)
            {
                if (DDGSType.SelectedValue == "1")
                {
                    total = amount + ((amount * (Convert.ToDecimal(TXTCGST.Text) + Convert.ToDecimal(TXTSGST.Text))) / 100);


                }
                else { total = amount + ((amount * (Convert.ToDecimal(TXTIGST.Text))) / 100); }

            }
            txtamount.Text = Convert.ToString(total);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            //lblerrormessage.Text = ex.Message;
            con.Close();
        }


    
    }
    protected void txtrateo_TextChanged(object sender, EventArgs e)
    {
        decimal qty = 0, amount = 0, total = 0, rate = 0;
        TextBox txtrate = (TextBox)sender;
        //GridViewRow row = (GridViewRow)txtrate.Parent.Parent;
        //TextBox txtqty = ((TextBox)row.FindControl("txtloomqty"));
        //Label lblamount = ((Label)row.FindControl("lblamount"));
        //Label lbltotalamt = ((Label)row.FindControl("lbltotalamt"));
        //TextBox txtigst = ((TextBox)row.FindControl("TXTIGST"));
        //TextBox txtsgst = ((TextBox)row.FindControl("TXTSGST"));
        //TextBox txtcgst = ((TextBox)row.FindControl("TXTCGST"));
        //DropDownList DDGSType = (DropDownList)row.FindControl("gsttype");
        qty = Convert.ToDecimal(txtprodqty.Text == "" ? "0" : txtprodqty.Text);
        rate = Convert.ToDecimal(txtrateo.Text == "" ? "0" : txtrateo.Text);
        amount = qty * rate;
        txtamount.Text = Convert.ToString(amount);
        if (Convert.ToInt32(ddltypegst.SelectedValue) > 0)
        {
            if (ddltypegst.SelectedValue == "1")
            {
                total = amount + ((amount * (Convert.ToDecimal(TXTCGST.Text) + Convert.ToDecimal(TXTSGST.Text))) / 100);


            }
            else { total = amount + ((amount * (Convert.ToDecimal(TXTIGST.Text))) / 100); }
            //if(DDGSType.SelectedValue=="1")
            //{
            //    total=amount*(Convert.ToDecimal(txtcgst.Text)+Convert.ToDecimal(txtsgst.Text));


            //}
            //else{ total=amount*(Convert.ToDecimal(txtigst.Text));}

        }
        txtamount.Text = Convert.ToString(total);

    }

}