using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_ProcessRawRecieve : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    static int MasterCompanyId;
    static string btnclickflag = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            DataSet DSQ = SqlHelper.ExecuteDataset(@"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname
                         Select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"  WHere PNM.ProcessType=1  order by PROCESS_NAME
                         Select VarProdCode From MasterSetting
                         DELETE TEMP_PROCESS_ISSUE_MASTER_NEW
                         Select Process_name_id from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" 
                         Select UnitsId,UnitName from Units order by UnitName 
                         Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @"
                         Select ConeType, ConeType From ConeMaster Order By SrNo");

            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 5, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "Select Process Name");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, DSQ, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 6, false, "");

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //ddCompName.SelectedIndex = 1;
            int VarProdCode = Convert.ToInt32(DSQ.Tables[2].Rows[0]["VarProdCode"]);
            switch (VarProdCode)
            {
                case 0:
                    procode.Visible = false;
                    break;
                case 1:
                    procode.Visible = true;
                    break;
            }
            switch (Session["varcompanyid"].ToString())
            {
                case "9":
                    txtChallanNo.ReadOnly = true;
                    ChkForWastedMaterial.Visible = true;
                    break;
                case "21":
                    TDProductionunit.Visible = true;
                    TDLoomNo.Visible = true;
                    //TDEmpName.Visible = false;
                    //TDBalanceReceive.Visible = true;
                    break;
                case "43":
                    Label19.Text = "UCN No";
                    break;
                default:
                    TDProductionunit.Visible = false;
                    TDLoomNo.Visible = false;
                    //TDEmpName.Visible = true;
                    //TDBalanceReceive.Visible = false;
                    ChkForWastedMaterial.Visible = false;
                    break;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            lablechange();
            //if (DSQ.Tables[3].Rows.Count > 0)
            //{
            //    for (int i = 0; i < DSQ.Tables[3].Rows.Count; i++)
            //    {
            //        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT PM.Companyid,OM.Customerid,PD.Orderid," + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid FROM PROCESS_ISSUE_MASTER_" + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid");
            //        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW (CompanyId,CustomerId,OrderId,PROCESSID,EmpId,IssueOrderId) SELECT DISTINCT PM.COMPANYID,OM.CUSTOMERID,PD.ORDERID," + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " PROCESSID,CASE WHEN PM.EMPID=0 THEN EMP.EMPID ELSE PM.EMPID END AS EMPID ,PM.ISSUEORDERID FROM PROCESS_ISSUE_MASTER_" + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " PM INNER JOIN PROCESS_ISSUE_DETAIL_" + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + " PD ON PM.ISSUEORDERID=PD.ISSUEORDERID INNER JOIN ORDERMASTER OM ON PD.ORDERID=OM.ORDERID LEFT JOIN EMPLOYEE_PROCESSORDERNO EMP ON PM.ISSUEORDERID=EMP.ISSUEORDERID AND PD.ISSUE_DETAIL_ID=EMP.ISSUEDETAILID AND EMP.PROCESSID=" + DSQ.Tables[3].Rows[i]["Process_Name_Id"] + "");
            //    }
            //}
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = 0;
        ProcessNameSelectedChange();
    }
    private void ProcessNameSelectedChange()
    {

        if (Session["varcompanyid"].ToString() == "21")
        {
            switch (ddProcessName.SelectedItem.Text.ToUpper())
            {
                case "WEAVING":
                    TDProductionunit.Visible = true;
                    TDLoomNo.Visible = true;
                    //TDEmpName.Visible = true;
                    ddempname.Enabled = false;
                    break;
                default:
                    TDProductionunit.Visible = false;
                    TDLoomNo.Visible = false;
                    //TDEmpName.Visible = true;
                    ddempname.Enabled = true;
                    FillFolioNo();
                    break;
            }

        }
        else
        {
            switch (ddProcessName.SelectedItem.Text.ToUpper())
            {
                case "WEAVING":
                    TDProductionunit.Visible = false;
                    TDLoomNo.Visible = false;
                    break;
                default:
                    TDProductionunit.Visible = false;
                    TDLoomNo.Visible = false;
                    TDEmpName.Visible = true;

                    //FillFolioNo();
                    break;
            }

        }

        string str = "";
        if (Session["varcompanyid"].ToString() == "21")
        {
            str = @"SELECT EMPID,EMPNAME FROM 
                    (SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                    and Isnull(EI.blacklist,0)=0
                    AND PIM.COMPANYID=" + ddCompName.SelectedValue + @" ";
            if (ddProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                str = str + " and PIM.IssueOrderId=" + ddPOrderNo.SelectedValue;
            }
            str = str + @" UNION 
                    SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME 
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPLOYEE_PROCESSORDERNO EMO ON PIM.ISSUEORDERID=EMO.ISSUEORDERID AND EMO.PROCESSID=" + ddProcessName.SelectedValue + @"
                    INNER JOIN EMPINFO EI ON EMO.EMPID=EI.EMPID and Isnull(EI.blacklist,0)=0
                    WHERE PIM.COMPANYID=" + ddCompName.SelectedValue + @" ";
            if (ddProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                str = str + " and PIM.IssueOrderId=" + ddPOrderNo.SelectedValue;
            }
            str = str + ") A ORDER BY EMPNAME";


        }
        else
        {
            str = @"SELECT EMPID,EMPNAME FROM 
                    (SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                    and Isnull(EI.blacklist,0)=0
                    AND PIM.COMPANYID=" + ddCompName.SelectedValue + @"
                    UNION
                    SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME 
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPLOYEE_PROCESSORDERNO EMO ON PIM.ISSUEORDERID=EMO.ISSUEORDERID AND EMO.PROCESSID=" + ddProcessName.SelectedValue + @"
                    INNER JOIN EMPINFO EI ON EMO.EMPID=EI.EMPID and Isnull(EI.blacklist,0)=0
                    WHERE PIM.COMPANYID=" + ddCompName.SelectedValue + ") A ORDER BY EMPNAME";
        }


        // UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct e.EmpId, e.EmpName  FROM  PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " pim INNER JOIN  EmpInfo e ON pim.Empid = e.EmpId ANd e.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Emp");
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "Select Emp");

        if (Session["varcompanyid"].ToString() == "21")
        {
            if (ddProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                if (ddempname.Items.Count > 0)
                {
                    ddempname.SelectedIndex = 1;
                }
            }
        }
    }

    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {

        //        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom from PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM inner join ProductionLoomMaster PL
        //                    on PM.LoomId=PL.UID and PM.Status='Pending'
        //                    And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + @" order by Loom,LoomNo
        //                    select Top(1) Godownid from Productionunitgodown Where Produnitid=" + DDProdunit.SelectedValue + " order by id desc";
        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,replace(loomno, '.', '')) Else 9999999 End as Loom from PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PM inner join ProductionLoomMaster PL
                    on PM.LoomId=PL.UID   And PL.companyid=" + ddCompName.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + @"  Left Join Employee_ProcessorderNo EMP on PM.issueorderid=EMP.issueorderid and EMP.processid=" + ddProcessName.SelectedValue + @"
                    Left join empinfo ei on EMp.empid=Ei.empid  ";
        if (ChKForEdit.Checked == false)
        {
            str = str + " Where PL.EnableDisableStatus=1";
        }
        else
        {
            str = str + " Where 1=1";
        }

        if (txtWeaverIdNoscan.Text != "")
        {
            str = str + " and Ei.empcode='" + txtWeaverIdNoscan.Text + "'";
        }
        str = str + " order by Loom,LoomNo";
        str = str + "  select Top(1) Godownid from Productionunitgodown Where Produnitid=" + DDProdunit.SelectedValue + " order by id desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDLoomNo, ds, 0, true, "--Plz Selec--");
        //*********
        //********
        if (ds.Tables[1].Rows.Count > 0)
        {
            hngodownid.Value = ds.Tables[1].Rows[0]["godownid"].ToString();
        }
        else
        {
            hngodownid.Value = "0";
        }
        if (DDLoomNo.Items.Count > 0)
        {
            DDLoomNo.SelectedIndex = DDLoomNo.Items.Count - 1;
            DDLoomNo_SelectedIndexChanged(sender, new EventArgs());
        }


    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DG.DataSource = null;
        //DG.DataBind();
        FillFolioNo(sender);

    }

    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = 0;
        EmpNameSelectedChange();
    }

    protected void FillFolioNo(object sender = null)
    {
        if (Session["VarCompanyId"].ToString() == "21")
        {
            if (ddProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                if (ddempname.Items.Count > 0)
                {
                    ddempname.SelectedIndex = 0;
                }
            }
        }

        string str = "";
        if (Session["varcompanyId"].ToString() == "9")
        {
            str = @"Select Distinct PRM.Prorderid,om.localOrder+'/'+cast(PRM.Prorderid as varchar(100)) as Prorderid1 
            From ProcessRawMaster PRM 
            inner join Process_issue_detail_" + ddProcessName.SelectedValue + @" PD on PRM.prorderid=Pd.issueorderid 
            inner join ordermaster OM on OM.orderid=PD.orderid  
            Where PRM.TypeFlag = 0 And PRM.CompanyId=" + ddCompName.SelectedValue + " And PRM.Processid=" + ddProcessName.SelectedValue + " And PRM.Empid=" + ddempname.SelectedValue + @" And 
            PRM.MasterCompanyId=" + Session["varCompanyId"] + " order by Prorderid1";
        }

        else
        {
            str = @"SELECT PRORDERID,ISSUEORDERID FROM 
                        (SELECT DISTINCT PM.PRORDERID,PM.FolioChallanNo AS ISSUEORDERID 
                        FROM PROCESSRAWMASTER PM INNER JOIN PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM ON PM.PRORDERID=PIM.ISSUEORDERID 
                        INNER JOIN EMPLOYEE_PROCESSORDERNO EMP ON PM.PRORDERID=EMP.ISSUEORDERID AND PM.PROCESSID=EMP.PROCESSID";
            if (Session["varcompanyId"].ToString() == "45")
            {
                str = str + " and pim.Empid=" + ddempname.SelectedValue;
                
            }
            else
            {
                str = str + " and pim.Empid=0";
            }

     str=str+"  Where PM.TypeFlag = 0 And PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + @"  and isnull(pim.FOLIOSTATUS,0)=0";
            if (TDProductionunit.Visible == true)
            {
                if (DDProdunit.SelectedIndex > 0)
                {
                    str = str + " and Units=" + DDProdunit.SelectedValue;
                }
                else
                {
                    str = str + " and Units=0";
                }
            }
            if (TDLoomNo.Visible == true)
            {
                if (DDLoomNo.SelectedIndex > 0)
                {
                    str = str + " and LoomId=" + DDLoomNo.SelectedValue;
                }
                else
                {
                    str = str + " and Loomid=0";
                }
            }
            if (TDEmpName.Visible == true)
            {
                if (ddempname.SelectedIndex > 0)
                {
                    str = str + " and EMP.Empid=" + ddempname.SelectedValue;
                }
                else
                {
                    str = str + " and EMP.Empid=0";
                }
            }
            //if (txtWeaverIdNoscan.Text != "")
            //{
            //    str = str + " and EI.empcode='" + txtWeaverIdNoscan.Text + "'";
            //}
            str = str + @" UNION 
                        SELECT DISTINCT PM.PRORDERID,PM.FolioChallanNo AS ISSUEORDERID 
                        FROM PROCESSRAWMASTER PM 
                        INNER JOIN PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM ON PM.PRORDERID=PIM.ISSUEORDERID
                        Where PM.TypeFlag = 0 And PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0";

            if (TDProductionunit.Visible == true)
            {
                if (DDProdunit.SelectedIndex > 0)
                {
                    str = str + " and Units=" + DDProdunit.SelectedValue;
                }
                else
                {
                    str = str + " and Units=0";
                }
            }
            if (TDLoomNo.Visible == true)
            {
                if (DDLoomNo.SelectedIndex > 0)
                {
                    str = str + " and LoomId=" + DDLoomNo.SelectedValue;
                }
                else
                {
                    str = str + " and Loomid=0";
                }
            }
            if (TDEmpName.Visible == true)
            {
                if (ddempname.SelectedIndex > 0)
                {
                    str = str + " and PM.Empid=" + ddempname.SelectedValue;
                }
                else
                {
                    str = str + " and PM.Empid=0";
                }
            }
            //if (txtWeaverIdNoscan.Text != "")
            //{
            //    str = str + " and EI.empcode='" + txtWeaverIdNoscan.Text + @"'";
            //}
            str = str + ") A";
            str = str + " ORDER BY ISSUEORDERID desc";
        }

        UtilityModule.ConditionalComboFill(ref ddPOrderNo, str, true, "--Plz Select--");
        if (ddPOrderNo.Items.Count > 0)
        {
            if (sender != null)
            {
                ddPOrderNo.SelectedIndex = 1;
                ddPOrderNo_SelectedIndexChanged(sender, new EventArgs());
            }

        }
    }

    private void EmpNameSelectedChange()
    {
        FillFolioNo();
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        ChKForEditChange();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
            DGMain.Columns[4].Visible = false;

        }
    }
    private void ChKForEditChange()
    {
        tdChallanNo.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            tdChallanNo.Visible = true;
            if (Session["varcompanyid"].ToString() == "21")
            {
                if (ddCompName.SelectedIndex > 0 && ddPOrderNo.SelectedIndex > 0 && ddProcessName.SelectedIndex > 0)
                {
                    UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId,replace(Str(Prmid)+'/'+ChalanNo,' ','') ChallanNo 
                    From ProcessRawMaster 
                    Where TypeFlag = 0 And TranType=1 And Companyid=" + ddCompName.SelectedValue + " And BranchID=" + DDBranchName.SelectedValue + @" And 
                    ProcessId=" + ddProcessName.SelectedValue + " And PROrderId=" + ddPOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
                }
            }
            else
            {
                if (ddCompName.SelectedIndex > 0 && ddempname.SelectedIndex > 0 && ddPOrderNo.SelectedIndex > 0 && ddProcessName.SelectedIndex > 0)
                {
                    if (Session["varcompanyid"].ToString() == "9")
                    {
                        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId,ChalanNo From ProcessRawMaster Where TypeFlag = 0 And TranType=1 And 
                        Companyid=" + ddCompName.SelectedValue + "  And BranchID=" + DDBranchName.SelectedValue + @" And ProcessId=" + ddProcessName.SelectedValue + @" And 
                        PROrderId=" + ddPOrderNo.SelectedValue + " And EmpId=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
                    }
                    else
                    {
                        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId,replace(Str(Prmid)+'/'+ChalanNo,' ','') ChallanNo 
                        From ProcessRawMaster Where TypeFlag = 0 And TranType=1 And Companyid=" + ddCompName.SelectedValue + "  And BranchID=" + DDBranchName.SelectedValue + @" And 
                        ProcessId=" + ddProcessName.SelectedValue + " And PROrderId=" + ddPOrderNo.SelectedValue + " And EmpId=" + ddempname.SelectedValue + @" And 
                        MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
                    }

                    if (DDChallanNo.Items.Count > 0)
                    {
                        DDChallanNo.SelectedIndex = 1;
                        ChallanSelectedChange();
                    }
                }
            }
        }
    }
    protected void ddPOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = 0;
        ChKForEditChange();
        Fill_GridForShowIssItem();
        Fill_POrderNoSelectedChange(sender);
        Fill_GridForBalanceQty();
        if (Session["VarCompanyId"].ToString() == "21")
        {
            if (ddProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
            {
                ProcessNameSelectedChange();
            }
        }
        FillCustomerCodeAndOrderNo();
    }

    private void FillCustomerCodeAndOrderNo()
    {
        if (ddProcessName.SelectedIndex > 0 && ddPOrderNo.SelectedIndex > 0)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct CI.CustomerCode + ' / ' + OM.CustomerOrderNo + ', ' 
                    From PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PID(Nolock) 
                    JOIN OrderMaster OM(Nolock) ON OM.OrderID = PID.OrderID 
                    JOIN Customerinfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                    Where PID.IssueOrderId = " + ddPOrderNo.SelectedValue + @" 
                    For XML Path('')");
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblCustomerCodeAndOrderNo.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }
    }
    private void Fill_GridForShowIssItem()
    {
        string strsql = @"Select  CATEGORY_NAME ,ITEM_NAME,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName Description,Sum(IssueQuantity) IssueQuantity
        From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
        Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TypeFlag = 0 And PRM.TranType=0 And PRM.PROrderId=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
        Group By CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGShowIssDetail.DataSource = ds;
        DGShowIssDetail.DataBind();
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanSelectedChange();
    }
    private void ChallanSelectedChange()
    {
        if (DDChallanNo.SelectedIndex > 0)
        {
            ViewState["Prmid"] = DDChallanNo.SelectedValue;
            if (DDChallanNo.SelectedItem.Text.Split('/').Length > 1)
            {

                txtChallanNo.Text = DDChallanNo.SelectedItem.Text.Split('/')[1];
            }
            else
            {
                txtChallanNo.Text = DDChallanNo.SelectedItem.Text;
            }
            Fill_GridForChallan();
        }
    }
    private void Fill_GridForChallan()
    {
        string strsql = "";
        if (variable.VarMANYFOLIORAWRECEIVE_SINGLECHALAN == "1")
        {
            strsql = @"Select PRT.PRTid,CATEGORY_NAME RecCategoryName,ITEM_NAME RecItemName,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName RecDescription,Sum(IssueQuantity) RecQty,isnull(PRM.Remark,'') as Remark,
                    Case When PRM.MasterCompanyId=9 Then Case When PRT.WastedMaterial=0 Then 'Fine' Else 'Wasted' End Else '' End as WastedMaterial
                    From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
                    Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 
                    And PRM.TypeFlag = 0 And PRM.chalanno='" + txtChallanNo.Text + "' And PRM.MasterCompanyId=" + Session["varCompanyId"] + @" ";

            if (ChKForEdit.Checked == true && ddPOrderNo.SelectedIndex > 0)
            {
                strsql = strsql + " and PRM.Prorderid=" + ddPOrderNo.SelectedValue + "";

                strsql = strsql + " Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark,PRM.MasterCompanyId,PRT.WastedMaterial";
            }
            else
            {
                strsql = strsql + " Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark,PRM.MasterCompanyId,PRT.WastedMaterial";
            }
        }
        else
        {
            strsql = @"Select PRT.PRTid,CATEGORY_NAME RecCategoryName,ITEM_NAME RecItemName,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName RecDescription,Sum(IssueQuantity) RecQty,isnull(PRM.Remark,'') as Remark,
                    Case When PRM.MasterCompanyId=9 Then Case When PRT.WastedMaterial=0 Then 'Fine' Else 'Wasted' End Else '' End as WastedMaterial
                    From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
                    Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 And PRM.TypeFlag = 0 And PRM.PRmId=" + ViewState["Prmid"] + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark,PRM.MasterCompanyId,PRT.WastedMaterial";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGMain.DataSource = ds;
        DGMain.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
        }
    }
    private void Fill_POrderNoSelectedChange(object sender = null)
    {
        string Qry = @" Select Distinct CATEGORY_ID,CATEGORY_NAME From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
        Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "";
        Qry = Qry + @" Select IsNull(DEPARTMENTTYPE, 0) DEPARTMENTTYPE, IsNull(DepartmentIssueOrderID, 0) DepartmentIssueOrderID From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " Where IssueOrderId = " + ddPOrderNo.SelectedValue;

        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "Select Catagory");
        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
            Fill_Category_SelectedChange();
        }
        if (Convert.ToInt32(MasterCompanyId) == 16 && Convert.ToInt32(DDBranchName.SelectedValue) == 2)
        {
            Qry = @" Select Distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + " Order by GodownName";
        }
        else if (Convert.ToInt32(DSQ.Tables[1].Rows[0]["DEPARTMENTTYPE"]) == 1)
        {
            Qry = @" Select Distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + " Order by GodownName";
        }
        else if (Convert.ToInt32(MasterCompanyId) == 42)
        {
            Qry = @" Select Distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + " Order by GodownName";
        }
        else if (Convert.ToInt32(MasterCompanyId) == 9)
        {
            Qry = @" Select Distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId 
                    Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + " and GM.GodownId in(2,4) Order by GodownName";
        }
        else
        {
            Qry = @" Select Distinct PRT.GodownID, GM.GodownName 
                From ProcessRawMaster PRM(Nolock)
                JOIN ProcessRawTran PRT(Nolock) ON PRT.PRMID = PRM.PRMID 
                JOIN GodownMaster GM(Nolock) ON GM.GoDownID = PRT.GodownID 
                JOIN Godown_Authentication GA(Nolock) ON GM.GodownId = GA.GodownId And GA.UserId = " + Session["varUserId"] + " and GA.MasterCompanyId = " + Session["varCompanyId"] + @" 
                Where PRM.Prorderid = " + ddPOrderNo.SelectedValue + @" 
                Order by GM.GodownName ";
        }
        UtilityModule.ConditionalComboFill(ref ddgodown, Qry, true, "Select Godown");

        if (ddgodown.Items.Count > 0)
        {
            ddgodown.SelectedIndex = 1;
            if (sender != null)
            {
                ddgodown_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Category_SelectedChange();
    }
    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct ITEM_ID,ITEM_NAME From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
            Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And VF.CATEGORY_ID=" + ddCatagory.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
            if (dditemname.Items.Count > 0)
            {
                dditemname.SelectedIndex = 1;
                ddlcategorycange();
                ItemName_SelectChange();
            }
        }
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        break;
                    case "3":
                        clr.Visible = true;
                        break;
                    case "4":
                        shp.Visible = true;
                        break;
                    case "5":
                        sz.Visible = true;
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemName_SelectChange();
    }
    private void ItemName_SelectChange()
    {
        if (dditemname.SelectedIndex > 0)
        {
            string Qry = @" SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"];
            Qry = Qry + @"  Select Distinct Qualityid,QualityName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF 
                        Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
            if (ddlunit.Items.Count > 0)
            {
                ddlunit.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 1, true, "Select Quality");

            if (dquality.Items.Count > 0)
            {
                dquality.SelectedIndex = 1;
                FillDropDowns();
                FillQuantity();
            }
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDropDowns();
        FillQuantity();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuantity();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuantity();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
    }
    private void ShapeSelectedChange()
    {
        if (sz.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct SizeId,SizeMtr From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF 
                        Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And VF.ShapeId=" + ddshape.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Size");
            if (ddsize.Items.Count > 0)
            {
                ddsize.SelectedIndex = 1;
            }
        }
        FillQuantity();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuantity();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuantity();
    }
    private void FillQuantity(object sender = null)
    {
        int quality = 0;
        int design = 0;
        int color = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
        {
            quality = 1;
        }
        if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
        {
            design = 1;
        }
        if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
        {
            color = 1;
        }
        if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
        {
            shape = 1;
        }
        if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
        {
            size = 1;
        }
        if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            txtlot.Text = "";
            txtissue.Text = "";
            ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, Txtprodcode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            hnitemfinishedid.Value = ItemFinishedId.ToString();
            FIllLotno(ItemFinishedId, sender);
            FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
        }
    }
    protected void FIllissuedqty(int ItemFinishedId)
    {
        txtissue.Text = "";
        string str = @"Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
                     Where PRM.PRMid=PRT.PRMid And TranType=0 And PRM.TypeFlag = 0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
        string str1 = @" Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
                     Where PRM.PRMid=PRT.PRMid And TranType=1 And PRM.TypeFlag = 0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
        if (DDlotno.SelectedIndex > 0)
        {
            str = str + "  and Prt.LotNo='" + DDlotno.SelectedItem.Text + "'";
            str1 = str1 + "  and Prt.LotNo='" + DDlotno.SelectedItem.Text + "'";
        }
        if (DDtagno.SelectedIndex > 0)
        {
            str = str + "  and Prt.Tagno='" + DDtagno.SelectedItem.Text + "'";
            str1 = str1 + "  and Prt.Tagno='" + DDtagno.SelectedItem.Text + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str + str1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtissue.Text = (Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]) - Convert.ToDouble(ds.Tables[1].Rows[0]["Qty"])).ToString();
        }
    }
    protected void FIllLotno(int ItemFinishedId, object sender = null)
    {
        DDlotno.SelectedIndex = -1;
        string str = @"Select Distinct Prt.LotNo,PRT.Lotno From ProcessRawMaster PRM,ProcessRawTran PRT 
        Where PRM.PRMid=PRT.PRMid And TranType=0 And PRM.TypeFlag = 0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDlotno, str, true, "--Plz Select--");
        if (DDlotno.Items.Count > 0)
        {
            DDlotno.SelectedIndex = 1;
            FIllTagno(ItemFinishedId, sender);
        }
    }
    protected void FIllTagno(int ItemFinishedId, object sender = null)
    {
        DDtagno.SelectedIndex = -1;
        string str = @"Select Distinct Prt.Tagno,PRT.Tagno From ProcessRawMaster PRM,ProcessRawTran PRT 
        Where PRM.PRMid=PRT.PRMid And TranType=0 And PRM.TypeFlag = 0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + " and prt.Lotno='" + DDlotno.SelectedItem.Text + "'";

        UtilityModule.ConditionalComboFill(ref DDtagno, str, true, "--Plz Select--");
        if (DDtagno.Items.Count > 0)
        {
            DDtagno.SelectedIndex = 1;
            if (sender != null)
            {
                DDtagno_SelectedIndexChanged(sender, new EventArgs());
            }

        }
    }
    protected void Txtprodcode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        // string Str;
        if (Txtprodcode.Text != "" && ddPOrderNo.SelectedIndex > 0)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * From ITEM_PARAMETER_MASTER IPM Where ProductCode='" + Txtprodcode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"] + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Str2 = @"Select VF.ITEM_FINISHED_ID,CATEGORY_ID,ITEM_ID,QualityId,DesignId,ColorId,ShapeId,SizeId,ShadecolorId 
                            From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
                            Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                            VF.ITEM_FINISHED_ID=" + ds.Tables[0].Rows[0]["Item_finished_id"] + " And PRM.MasterCompanyId=" + Session["varCompanyId"];

                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str2);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddCatagory.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    Fill_Category_SelectedChange();
                    dditemname.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    ItemName_SelectChange();
                }
                else
                {
                    LblError.Visible = true;
                    Txtprodcode.Focus();
                }
            }
        }
        else
        {
            LblError.Visible = true;
            Txtprodcode.Focus();

        }
    }
    protected void txtrecqty_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        double issuqty = Convert.ToDouble(txtissue.Text);
        double recqty = Convert.ToDouble(txtrecqty.Text);

        if (issuqty < recqty)
        {
            LblError.Text = "Pls Enter Correct Qty";
            txtrecqty.Text = "";
            txtrecqty.Focus();
        }
    }
    protected void SaveUpdateDetail()
    {
        CHECKVALIDCONTROL();

        if (LblError.Text == "")
        {
            if (variable.VarMANYFOLIORAWRECEIVE_SINGLECHALAN == "0")
            {
                DuplicateChallanNo();
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[30];

                arr[0] = new SqlParameter("@PrmID", SqlDbType.Int);
                arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                arr[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                arr[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                arr[4] = new SqlParameter("@OrderId", SqlDbType.Int);
                arr[5] = new SqlParameter("@IssueDate", SqlDbType.SmallDateTime);
                arr[6] = new SqlParameter("@ChalanNo", SqlDbType.NVarChar, 50);
                arr[7] = new SqlParameter("@TranType", SqlDbType.Int);
                arr[8] = new SqlParameter("@userid", SqlDbType.Int);
                arr[9] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                arr[10] = new SqlParameter("@Prtid", SqlDbType.Int);
                arr[11] = new SqlParameter("@CategoryId", SqlDbType.Int);
                arr[12] = new SqlParameter("@Itemid", SqlDbType.Int);
                arr[13] = new SqlParameter("@FinishedId", SqlDbType.Int);
                arr[14] = new SqlParameter("@GodownId", SqlDbType.Int);
                arr[15] = new SqlParameter("@IssueQuantity", SqlDbType.Float);
                arr[16] = new SqlParameter("@lotNo", SqlDbType.NVarChar, 50);
                arr[17] = new SqlParameter("@UnitId", SqlDbType.Int);
                arr[18] = new SqlParameter("@PrmIdOutPut", SqlDbType.Int);
                arr[19] = new SqlParameter("@PrtIdOutPut", SqlDbType.Int);
                arr[20] = new SqlParameter("@UpdateFlag", SqlDbType.Int);
                arr[21] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
                arr[22] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
                arr[23] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                arr[24] = new SqlParameter("@Remark", txtremark.Text);
                arr[25] = new SqlParameter("@FolioChallanNo", SqlDbType.VarChar, 50);
                arr[26] = new SqlParameter("@Conetype", SqlDbType.VarChar, 50);
                arr[27] = new SqlParameter("@Noofcone", SqlDbType.Int);
                arr[28] = new SqlParameter("@BranchID", SqlDbType.Int);
                arr[29] = new SqlParameter("@WastedMaterial", SqlDbType.Int);

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, Txtprodcode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));

                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = ddempname.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = ddPOrderNo.SelectedValue;
                arr[5].Value = txtdate.Text;
                arr[6].Value = txtChallanNo.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 1;
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varCompanyId"].ToString();
                arr[10].Value = 0;
                arr[20].Value = 0;
                if (btnsave.Text == "Update")
                {
                    arr[10].Value = DGMain.SelectedDataKey.Value;
                    arr[20].Value = 1;
                }
                arr[11].Value = ddCatagory.SelectedValue;
                arr[12].Value = dditemname.SelectedValue;
                arr[13].Value = Varfinishedid;
                arr[14].Value = ddgodown.SelectedValue;
                arr[15].Value = txtrecqty.Text;
                arr[16].Value = TDlotno.Visible == false ? "Without Lot No" : DDlotno.SelectedItem.Text;
                arr[17].Value = ddlunit.SelectedValue;
                arr[18].Direction = ParameterDirection.Output;
                arr[19].Direction = ParameterDirection.Output;
                arr[21].Value = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
                arr[22].Value = TDtagno.Visible == false ? "Without Tag No" : DDtagno.SelectedItem.Text;
                arr[23].Direction = ParameterDirection.Output;
                arr[25].Value = ddPOrderNo.SelectedIndex > 0 ? ddPOrderNo.SelectedItem.Text : "";
                arr[26].Value = DDconetype.SelectedItem.Text;
                arr[27].Value = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                arr[28].Value = DDBranchName.SelectedValue;
                arr[29].Value = ChkForWastedMaterial.Checked == true ? 1 : 0 ; 

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE", arr);

                Tran.Commit();
                txtChallanNo.Text = arr[6].Value.ToString();
                ViewState["Prmid"] = arr[18].Value;
                LblError.Visible = true;
                LblError.Text = arr[23].Value.ToString();
                Fill_GridForChallan();
                SaveReferece();
                btnsave.Text = "Save";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LblError.Visible = true;
                LblError.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        SaveUpdateDetail();
    }
    protected void DuplicateChallanNo()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtChallanNo.Text != "")
            {
                string str = "Select chalanNo From ProcessRawMaster Where ChalanNo<>'' And TranType=1 And TypeFlag = 0 And ChalanNo='" + txtChallanNo.Text + "' And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varCompanyId"];
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblError.Text = "Challan no. already exists.....";
                }
            }

        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void SaveReferece()
    {
        if (ddlshade.Items.Count > 0 && shd.Visible == true)
        {
            ddlshade.SelectedIndex = 0;
        }
        txtissue.Text = "";
        txtrecqty.Text = "";
        if (ChkForWastedMaterial.Visible == true)
        {
            ChkForWastedMaterial.Checked = false;
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblError.Text = "";
        LblError.Visible = true;
        if (UtilityModule.VALIDDROPDOWNLIST(ddCompName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddProcessName) == false)
        {
            goto a;
        }
        if (TDEmpName.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddempname) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddPOrderNo) == false)
        {
            goto a;
        }
        if (ChKForEdit.Checked == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDChallanNo) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(txtdate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddCatagory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(dditemname) == false)
        {
            goto a;
        }
        if (ql.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (dsn.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dddesign) == false)
            {
                goto a;
            }
        }
        if (clr.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (shp.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
            {
                goto a;
            }
        }
        if (sz.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
            {
                goto a;
            }
        }
        if (shd.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlshade) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlunit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddgodown) == false)
        {
            goto a;
        }
        if (TDlotno.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDlotno) == false)
            {
                goto a;
            }
        }
        if (TDtagno.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDtagno) == false)
            {
                goto a;
            }
        }
        if (TDBinNo.Visible == true)
        {

            if (UtilityModule.VALIDDROPDOWNLIST(DDBinNo) == false)
            {
                goto a;
            }

        }
        if (UtilityModule.VALIDTEXTBOX(txtrecqty) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblError);
    B: ;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("issueqty");
        Session.Remove("inhand");
        Session.Remove("finishedid");
        Session.Remove("prm");
    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;
        string sql = "SELECT COMPANYID,PROCESSID,EMPID,ISSUEORDERID FROM VIEW_PROCESS_ISSUE_MASTER Where COMPANYID = " + ddCompName.SelectedValue + " And ChallanNo='" + VarPOrderNo + "'";

        switch (Session["varcompanyid"].ToString())
        {
            case "28":
            case "16":
                sql = sql + " and processid=1";
                break;
            default:
                if (ddProcessName.SelectedIndex > 0)
                {
                    sql = sql + " and Processid=" + ddProcessName.SelectedValue;
                }
                break;
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
            ProcessNameSelectedChange();
            ddempname.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
            EmpNameSelectedChange();
            if (ddPOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["IssueOrderId"].ToString()) != null)
            {
                ddPOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                FillCustomerCodeAndOrderNo();
            }
            else
            {
                LblError.Text = "This Po No. does not exists or Sample Po.";
                LblError.Visible = true;
                return;

            }
            Fill_GridForShowIssItem();
            ChKForEditChange();
            Fill_POrderNoSelectedChange(sender);
            //Fill_Category_SelectedChange();
            //ItemName_SelectChange();
            //Fill_GodownSelectedChange();
            txtrecqty.Focus();
        }
        else
        {
            ddPOrderNo.SelectedIndex = 0;
            ddCatagory.SelectedIndex = 0;
            Fill_Category_SelectedChange();
            TxtPOrderNo.Text = "";
            TxtPOrderNo.Focus();
        }
    }
    protected void DGShowIssDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowIssDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGMain, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGMain.Columns.Count; i++)
            {
                if (DGMain.Columns[i].HeaderText.ToUpper() == "MATERIAL TYPE")
                {
                    if (Session["varCompanyNo"].ToString() == "9")
                    {
                        DGMain.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGMain.Columns[i].Visible = false;
                    }
                }
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
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
    protected void DGMain_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void DGMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPrtID = Convert.ToInt32(DGMain.DataKeys[e.RowIndex].Value);
            ViewState["Prmid"] = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select PrmId from ProcessRawTran Where PrtId=" + VarPrtID);
            SqlParameter[] arr = new SqlParameter[6];

            arr[0] = new SqlParameter("@PrtID", SqlDbType.Int);
            arr[1] = new SqlParameter("@RowCount", SqlDbType.Int);
            arr[2] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);

            arr[0].Value = VarPrtID;
            arr[1].Value = 2;
            arr[2].Value = 1;
            arr[3].Direction = ParameterDirection.Output;

            if (DGMain.Rows.Count == 1)
            {
                arr[1].Value = 1;
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE_RECEIVE_DELETE", arr);
            if (arr[3].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[3].Value.ToString() + "');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Fill_GridForChallan();
    }
    private void FillDropDowns()
    {
        if (dsn.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct designId,designName 
From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Design");
            if (dddesign.Items.Count > 0)
            {
                dddesign.SelectedIndex = 1;
            }
        }
        if (clr.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct ColorId,ColorName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Color");
            if (ddcolor.Items.Count > 0)
            {
                ddcolor.SelectedIndex = 1;
            }
        }
        if (shp.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct ShapeId,ShapeName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Shape");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ShapeSelectedChange();
            }
        }
        if (shd.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct ShadecolorId,ShadeColorName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.TypeFlag = 0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select ");
            if (ddlshade.Items.Count > 0)
            {
                ddlshade.SelectedIndex = 1;
            }
        }
    }
    private void CarpetInternationalFormatReport()
    {
        SqlParameter[] _array = new SqlParameter[3];
        _array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["Prmid"];
        _array[1].Value = ddProcessName.SelectedValue;
        _array[2].Value = 1; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeaverRawMaterialIssuedDetail_CarpetInternational", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialReceiveDetailCarpetInternational.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialReceiveDetailCarpetInternational.xsd";

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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "";
        switch (Session["varcompanyId"].ToString())
        {
            case "9":
                str = @"SELECT PRM.Date, PRM.ChalanNo,PRM.trantype, PRT.IssueQuantity,PRT.Lotno, GM.GodownName, EI.EmpName,
                    EI.Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, CI.CompAddr3, CI.CompTel,
                    Vf.ITEM_NAME, Vf.QualityName, Vf.designName, Vf.ColorName,Vf.ShadeColorName, Vf.ShapeName,Vf.SizeMtr, PNM.PROCESS_NAME,
                    PRM.Prorderid,U.UnitName,Om.LocalOrder,CI.GSTNO,EI.GSTNO as EmpGstno,isnull(PRT.TagNo,'') as TagNo,isnull(PRM.Remark,'') as Remark
                    FROM  ProcessRawMaster PRM  INNER JOIN  ProcessRawTran PRT ON PRM.PRMid=PRT.PRMid 
                    INNER JOIN GodownMaster GM ON GM.GoDownID=PRT.Godownid 
                    INNER JOIN EmpInfo EI ON PRM.Empid=EI.EmpId 
                    INNER JOIN CompanyInfo CI ON PRM.Companyid=CI.CompanyId 
                    INNER JOIN PROCESS_NAME_MASTER PNM ON PRM.Processid=PNM.PROCESS_NAME_ID
                    INNER JOIN V_FinishedItemDetail Vf ON PRT.Finishedid=Vf.ITEM_FINISHED_ID 
                    INNER JOIN Unit U ON U.UnitId=PRT.UnitId
                    inner join(select  Distinct IssueOrderId,Orderid from PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PID)as Process_issue_Detail
                    on Process_issue_Detail.IssueOrderId=prm.Prorderid
                    inner join OrderMaster OM on OM.OrderId=Process_issue_Detail.Orderid
                    Where PRM.TypeFlag = 0 And PRM.PrmId=" + ViewState["Prmid"];
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Session["rptFileName"] = "~\\Reports\\RptRawMaterialSlipforHafizia.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialSlipforHafizia.xsd";

                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }

                break;
            case "16":
                str = @" select PM.Date, PM.ChalanNo, PM.trantype, PT.IssueQuantity, 
                    PT.Lotno, GM.GodownName, EI.EmpName, EI.Address, CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                    '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                    vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                    PM.Prorderid, EI.GSTNo as empgstin, CI.GSTNo,PT.TagNo,PT.BinNo, BM.GstNo BranchGstNo, 
                    0 ReportType 
                    From ProcessRawMaster PM 
                    inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
                    JOIN BranchMaster BM ON BM.ID = PM.BranchID 
                    inner join CompanyInfo ci on PM.Companyid=ci.CompanyId
                    inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                    inner join GodownMaster GM on PT.Godownid=GM.GoDownID
                    inner join EmpInfo Ei on PM.Empid=ei.EmpId
                    inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                    Where PM.TypeFlag = 0 And PM.Prmid=" + ViewState["Prmid"];

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptRawIssueRecDuplicateNew.xsd";

                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }
                break;
            case "43":
                CarpetInternationalFormatReport();
                break;
            default:
                if (variable.VarMANYFOLIORAWRECEIVE_SINGLECHALAN == "1")
                {
                    Session["ReportPath"] = "Reports/RptRawIssueRecDuplicateManyfolioonsinglechalan.rpt";
                    Session["CommanFormula"] = "{ProcessRawMaster.chalanno}='" + txtChallanNo.Text + "' and {ProcessRawMaster.trantype}=1";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                }
                else
                {
                    Session["ReportPath"] = "Reports/RptRawIssueRecDuplicate.rpt";
                    Session["CommanFormula"] = "{ProcessRawMaster.PrmId}=" + ViewState["Prmid"];
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                }
                break;
        }
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, Txtprodcode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(ddgodown.SelectedValue), Varfinishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster where GODOWNID=" + ddgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            }
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT Top(1) EMP.ProcessId,EI.EmpId FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID WHERE EI.EMPCODE='" + txtWeaverIdNoscan.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddProcessName.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
            {
                ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
                if (sender != null)
                {
                    ddProcessName_SelectedIndexChanged(sender, new EventArgs());
                }

            }
            if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                ddempname.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                if (sender != null)
                {
                    ddempname_SelectedIndexChanged(sender, new EventArgs());
                }
            }

        }
        else
        {
            ddProcessName.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);

        }
    }
    protected void txtStockNoScan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_EmployeeStockNo(sender);

    }
    protected void FillProcess_EmployeeStockNo(object sender = null)
    {
        //string str = @"SELECT Top(1) EMP.ProcessId,EI.EmpId FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID WHERE EI.EMPCODE='" + txtWeaverIdNoscan.Text + "'";

        string str = @"SELECT Top(1) EMP.ProcessId,EI.EmpId,LS.IssueOrderId 
                        FROM EMPLOYEE_PROCESSORDERNO EMP 
                        INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID 
                        INNER JOIN LOOMSTOCKNO LS ON EMP.IssueOrderId=LS.Issueorderid and EMP.IssueDetailId=LS.IssueDetailid AND LS.ProcessID = " + ddProcessName.SelectedValue + @" 
                        WHERE LS.TStockNo='" + txtStockNoScan.Text + "' ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddProcessName.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
            {
                ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
                if (sender != null)
                {
                    ddProcessName_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                ddempname.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                if (sender != null)
                {
                    ddempname_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (ddPOrderNo.Items.FindByValue(ds.Tables[0].Rows[0]["IssueOrderId"].ToString()) != null)
            {
                ddPOrderNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                if (sender != null)
                {
                    ddPOrderNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }

        }
        else
        {
            ddProcessName.SelectedIndex = -1;
            ddPOrderNo.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Stock No')", true);

        }
    }
    protected void DDlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FIllTagno(Convert.ToInt32(hnitemfinishedid.Value), sender);
        FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
        // FIllConsumedqty(Convert.ToInt32(hnitemfinishedid.Value));

        //FIllAlreadyReceivedQty(Convert.ToInt32(hnitemfinishedid.Value));
    }

    protected void DDtagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
        //FIllConsumedqty(Convert.ToInt32(hnitemfinishedid.Value));

        //FIllAlreadyReceivedQty(Convert.ToInt32(hnitemfinishedid.Value));
    }
    protected void GDBalanceQty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDBalanceQty, "Select$" + e.Row.RowIndex);
        }
    }
    private void Fill_GridForBalanceQty()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@processid", ddProcessName.SelectedValue);
        param[1] = new SqlParameter("@IssueOrderId", ddPOrderNo.SelectedValue);
        param[2] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);

        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetBalanceQtyGridData", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GDBalanceQty.DataSource = ds;
            GDBalanceQty.DataBind();
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        //if (variable.VarWeaverRawReceiveSaveEditPassword == txtpwd.Text)
        //{
        //    if (btnclickflag == "BtnSaveUpdate")
        //    {
        //        SaveUpdateDetail();
        //    }
        //    Popup(false);
        //}
        //else
        //{
        //    LblError.Visible = true;
        //    LblError.Text = "Please Enter Correct Password..";
        //}
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

//    protected void DGShowIssDetail_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        if (Session["varCompanyNo"].ToString() == "22")
//        {
//            LblError.Text = "";
//            DataSet ds = null;
//            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//            try
//            {
//                con.Open();
//                ds = null;
//                string strsql = @"Select VF.CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,ShadecolorId,PRT.UnitId,Prt.Tagno,PRT.Godownid,PRT.Lotno,PRT.PRTid,
//            CATEGORY_NAME ,ITEM_NAME,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName Description
//            From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
//            Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TypeFlag = 0 And PRM.TranType=0 
//            And PRT.PRTid=" + DGShowIssDetail.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @" ";
//                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

//                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
//                Fill_Category_SelectedChange();
//                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
               
//                ItemName_SelectChange();
//                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
               
//                //ItemName_SelectChange();

//                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"], true, "Select Unit");
//                ddlunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();              

//                if (dsn.Visible == true)
//                {
//                    dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
//                    FillDropDowns();
//                }
//                if (clr.Visible == true)
//                {
//                    ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
//                    FillDropDowns();
//                }
//                if (shp.Visible == true)
//                {
//                    ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
//                    FillDropDowns();
//                }
//                if (shd.Visible == true)
//                {
//                    ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
//                    FillDropDowns();
//                }
//                if (sz.Visible == true)
//                {
//                    if (ddlunit.SelectedValue == "1")
//                    {
//                        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
//                    }
//                    else if (ddlunit.SelectedValue == "2")
//                    {
//                        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
//                    }
//                    ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
//                }
//                ddgodown.SelectedValue = ds.Tables[0].Rows[0]["Godownid"].ToString();
//                ddgodown_SelectedIndexChanged(sender, new EventArgs());

//                FillQuantity();
//                if (DDlotno.Items.FindByValue(ds.Tables[0].Rows[0]["Lotno"].ToString()) != null)
//                {
//                    DDlotno.SelectedValue = ds.Tables[0].Rows[0]["Lotno"].ToString();
//                    DDlotno_SelectedIndexChanged(sender, new EventArgs());
//                }
//                if (TDtagno.Visible == true)
//                {
//                    if (DDtagno.Items.FindByValue(ds.Tables[0].Rows[0]["Tagno"].ToString()) != null)
//                    {
//                        DDtagno.SelectedValue = ds.Tables[0].Rows[0]["Tagno"].ToString();
//                        DDtagno_SelectedIndexChanged(sender, new EventArgs());
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                LblError.Visible = true;
//                LblError.Text = ex.Message;
//            }
//            finally
//            {
//                if (con.State == ConnectionState.Open)
//                {
//                    con.Close();
//                }
//            }
//        }


//    }
}