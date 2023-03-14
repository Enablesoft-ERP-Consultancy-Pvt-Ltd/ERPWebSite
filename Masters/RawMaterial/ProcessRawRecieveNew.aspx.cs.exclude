using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_ProcessRawRecieveNew : System.Web.UI.Page
{
    int ItemFinishedId = 0;
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
            ViewState["Prmid"] = 0;
            DataSet DSQ = SqlHelper.ExecuteDataset(@"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname
                         select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"  WHere PNM.ProcessType=1  order by PROCESS_NAME
                         Select VarProdCode From MasterSetting
                         DELETE TEMP_PROCESS_ISSUE_MASTER_NEW
                         Select Process_name_id from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"]);

            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "Select Comp Name");
            if (ddCompName.Items.FindByValue(Session["dcompanyid"].ToString()) != null)
            {
                ddCompName.SelectedValue = Session["dcompanyid"].ToString();
            }
            else
            {
                ddCompName.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "Select Process Name");

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
        string str = @"SELECT EMPID,EMPNAME FROM 
                    (SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                    and Isnull(EI.blacklist,0)=0
                    AND PIM.COMPANYID=" + ddCompName.SelectedValue + @"
                    UNION
                    SELECT DISTINCT EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN  ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME 
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM INNER JOIN EMPLOYEE_PROCESSORDERNO EMO ON PIM.ISSUEORDERID=EMO.ISSUEORDERID AND EMO.PROCESSID=" + ddProcessName.SelectedValue + @"
                    INNER JOIN EMPINFO EI ON EMO.EMPID=EI.EMPID and Isnull(EI.blacklist,0)=0
                    WHERE PIM.COMPANYID=" + ddCompName.SelectedValue + ") A ORDER BY EMPNAME";

        // UtilityModule.ConditionalComboFill(ref ddempname, "SELECT distinct e.EmpId, e.EmpName  FROM  PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " pim INNER JOIN  EmpInfo e ON pim.Empid = e.EmpId ANd e.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Emp");
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "Select Emp");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = 0;
        EmpNameSelectedChange();
    }
    private void EmpNameSelectedChange()
    {
        if (Session["varcompanyId"].ToString() == "9")
        {
            UtilityModule.ConditionalComboFill(ref ddPOrderNo, "Select Distinct PRM.Prorderid,om.localOrder+'/'+cast(PRM.Prorderid as varchar(100)) as Prorderid1 From ProcessRawMaster PRM inner join Process_issue_detail_" + ddProcessName.SelectedValue + " pD on PRM.prorderid=Pd.issueorderid inner join ordermaster OM on OM.orderid=PD.orderid  Where PRM.CompanyId=" + ddCompName.SelectedValue + " And PRM.Processid=" + ddProcessName.SelectedValue + " And PRM.Empid=" + ddempname.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + " order by Prorderid1", true, "Select POrder No.");
        }
        else
        {
//            string str = @"SELECT PRORDERID,ISSUEORDERID FROM 
//                        (SELECT DISTINCT PM.PRORDERID,PM.PRORDERID AS ISSUEORDERID FROM PROCESSRAWMASTER PM INNER JOIN EMPLOYEE_PROCESSORDERNO EMP ON PM.PRORDERID=EMP.ISSUEORDERID AND PM.PROCESSID=EMP.PROCESSID
//                        Where PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + " And EMP.Empid=" + ddempname.SelectedValue + @"
//                        UNION
//                        SELECT DISTINCT PM.PRORDERID,PM.PRORDERID AS ISSUEORDERID FROM PROCESSRAWMASTER PM INNER JOIN PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM ON PM.PRORDERID=PIM.ISSUEORDERID
//                        Where PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + " And PM.Empid=" + ddempname.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0) A
//                        ORDER BY ISSUEORDERID desc";

            string str = @"SELECT PRORDERID,ISSUEORDERID FROM 
                        (SELECT DISTINCT PM.PRORDERID,PM.FolioChallanNo AS ISSUEORDERID FROM PROCESSRAWMASTER PM INNER JOIN PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM ON PM.PRORDERID=PIM.ISSUEORDERID
                        INNER JOIN EMPLOYEE_PROCESSORDERNO EMP ON PM.PRORDERID=EMP.ISSUEORDERID AND PM.PROCESSID=EMP.PROCESSID and pim.Empid=0
                        Where PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + " And EMP.Empid=" + ddempname.SelectedValue + @" and isnull(pim.FOLIOSTATUS,0)=0
                        UNION
                        SELECT DISTINCT PM.PRORDERID,PM.FolioChallanNo AS ISSUEORDERID FROM PROCESSRAWMASTER PM INNER JOIN PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM ON PM.PRORDERID=PIM.ISSUEORDERID
                        Where PM.CompanyId=" + ddCompName.SelectedValue + " And PM.Processid=" + ddProcessName.SelectedValue + " And PM.Empid=" + ddempname.SelectedValue +@" and isnull(pim.FOLIOSTATUS,0)=0) A
                        ORDER BY ISSUEORDERID desc";
            UtilityModule.ConditionalComboFill(ref ddPOrderNo, str, true, "Select POrder No.");
            // UtilityModule.ConditionalComboFill(ref ddPOrderNo, "Select Distinct Prorderid,Prorderid From ProcessRawMaster Where CompanyId=" + ddCompName.SelectedValue + " And Processid=" + ddProcessName.SelectedValue + " And Empid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
        }
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        ChKForEditChange();
    }
    private void ChKForEditChange()
    {
        tdChallanNo.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            tdChallanNo.Visible = true;
            if (ddCompName.SelectedIndex > 0 && ddempname.SelectedIndex > 0 && ddPOrderNo.SelectedIndex > 0 && ddProcessName.SelectedIndex > 0)
            {
                if (Session["varcompanyid"].ToString() == "9")
                {
                    UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select PrmId,ChalanNo From ProcessRawMaster Where TranType=1 And Companyid=" + ddCompName.SelectedValue + " And ProcessId=" + ddProcessName.SelectedValue + " And PROrderId=" + ddPOrderNo.SelectedValue + " And EmpId=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select PrmId,replace(Str(Prmid)+'/'+ChalanNo,' ','') ChallanNo From ProcessRawMaster Where TranType=1 And Companyid=" + ddCompName.SelectedValue + " And ProcessId=" + ddProcessName.SelectedValue + " And PROrderId=" + ddPOrderNo.SelectedValue + " And EmpId=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select POrder No.");
                }


                if (DDChallanNo.Items.Count > 0)
                {
                    DDChallanNo.SelectedIndex = 1;
                    ChallanSelectedChange();
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
    }
    private void Fill_GridForShowIssItem()
    {
        string strsql = @"Select CATEGORY_NAME ,ITEM_NAME,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName Description,Sum(IssueQuantity) IssueQuantity
        From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=0 And PRM.PROrderId=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
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
            strsql = @"Select PRT.PRTid,CATEGORY_NAME RecCategoryName,ITEM_NAME RecItemName,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName RecDescription,Sum(IssueQuantity) RecQty,isnull(PRM.Remark,'') as Remark
                    From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 
                    And PRM.chalanno='" + txtChallanNo.Text + "' And PRM.MasterCompanyId=" + Session["varCompanyId"] + @" ";

            if (ChKForEdit.Checked == true && ddPOrderNo.SelectedIndex > 0)
            {
                strsql = strsql + " and PRM.Prorderid=" + ddPOrderNo.SelectedValue + "";

                strsql = strsql + " Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark";
            }
            else
            {
                strsql = strsql + " Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark";
            }
        }
        else
        {
            strsql = @"Select PRT.PRTid,CATEGORY_NAME RecCategoryName,ITEM_NAME RecItemName,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName RecDescription,Sum(IssueQuantity) RecQty,isnull(PRM.Remark,'') as Remark
                    From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 And PRM.PRmId=" + ViewState["Prmid"] + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName,PRM.Remark";
        }

        //        string strsql = @"Select PRT.PRTid,CATEGORY_NAME RecCategoryName,ITEM_NAME RecItemName,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeMtr+' '+ShadeColorName RecDescription,Sum(IssueQuantity) RecQty
        //        From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 And PRM.PRmId=" + ViewState["Prmid"] + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
        //        Group By PRT.PRTid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,SizeMtr,ShadeColorName";
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
        string Qry = @"Select Distinct CATEGORY_ID,CATEGORY_NAME From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF 
        Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "";
        Qry = Qry + @"   SELECT DISTINCT GM.GoDownID,GM.GodownName FROM GodownMaster GM  order by GodownName";

        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "Select Catagory");
        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
            Fill_Category_SelectedChange();
        }
        UtilityModule.ConditionalComboFillWithDS(ref ddgodown, DSQ, 1, true, "Select Godown");
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
            Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And VF.CATEGORY_ID=" + ddCatagory.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
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
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
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
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
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

            //            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select LotNo,PRT.TagNo From ProcessRawMaster PRM,ProcessRawTran PRT Where PRM.PRMid=PRT.PRMid And TranType=0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + @"
            //            Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
            //            Where PRM.PRMid=PRT.PRMid And TranType=0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"]);
            //            if (Ds.Tables[0].Rows.Count > 0)
            //            {
            //                txtlot.Text = Ds.Tables[0].Rows[0]["LotNo"].ToString();
            //                txttagno.Text = Ds.Tables[0].Rows[0]["Tagno"].ToString();
            //            }
            //            if (Ds.Tables[0].Rows.Count > 0)
            //            {
            //                txtissue.Text = Ds.Tables[1].Rows[0]["Qty"].ToString();
            //            }
            FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
        }
    }
    protected void FIllissuedqty(int ItemFinishedId)
    {
        txtissue.Text = "";
        string str = @"Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
                     Where PRM.PRMid=PRT.PRMid And TranType=0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
        if (DDlotno.SelectedIndex > 0)
        {
            str = str + "  and Prt.LotNo='" + DDlotno.SelectedItem.Text + "'";
        }
        if (DDtagno.SelectedIndex > 0)
        {
            str = str + "  and Prt.Tagno='" + DDtagno.SelectedItem.Text + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtissue.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
        }
    }
    protected void FIllLotno(int ItemFinishedId, object sender = null)
    {
        DDlotno.SelectedIndex = -1;
        string str = "Select Distinct Prt.LotNo,PRT.Lotno From ProcessRawMaster PRM,ProcessRawTran PRT Where PRM.PRMid=PRT.PRMid And TranType=0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"];
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
        string str = "Select Distinct Prt.Tagno,PRT.Tagno From ProcessRawMaster PRM,ProcessRawTran PRT Where PRM.PRMid=PRT.PRMid And TranType=0 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddPOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + " and prt.Lotno='" + DDlotno.SelectedItem.Text + "'";

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
                string Str2 = @"Select VF.ITEM_FINISHED_ID,CATEGORY_ID,ITEM_ID,QualityId,DesignId,ColorId,ShapeId,SizeId,ShadecolorId From ProcessRawMaster PRM,
                               ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=30 And VF.ITEM_FINISHED_ID=" + ds.Tables[0].Rows[0]["Item_finished_id"] + " And PRM.MasterCompanyId=" + Session["varCompanyId"];

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
        double issuqty = Convert.ToDouble(txtissue.Text);
        double recqty = Convert.ToDouble(txtrecqty.Text);
        if (issuqty < recqty)
        {
            LblError.Text = "Pls Enter Correct Qty";
            txtrecqty.Text = "";
            txtrecqty.Focus();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
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
                SqlParameter[] arr = new SqlParameter[26];

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

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE", arr);

                // UtilityModule.StockStockTranTableUpdate(Varfinishedid, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(ddCompName.SelectedValue), txtlot.Text.ToString(), Convert.ToDouble(txtrecqty.Text), Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "ProcessRawTran", Convert.ToInt32(arr[19].Value), Tran, 1, true, 1, 0, unitid: Convert.ToInt16(ddlunit.SelectedValue));

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
                string str = "Select chalanNo From ProcessRawMaster Where ChalanNo<>'' And TranType=1 And ChalanNo='" + txtChallanNo.Text + "' And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varCompanyId"];
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
        if (UtilityModule.VALIDDROPDOWNLIST(ddempname) == false)
        {
            goto a;
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
        string sql = "SELECT COMPANYID,PROCESSID,EMPID,ISSUEORDERID FROM VIEW_PROCESS_ISSUE_MASTER Where ChallanNo='" + VarPOrderNo + "'";

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
            ddCompName.SelectedValue = Ds.Tables[0].Rows[0]["CompanyId"].ToString();
            ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
            ProcessNameSelectedChange();
            ddempname.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
            EmpNameSelectedChange();
            ddPOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
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
        //        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,
        //        ShadecolorId,IssueQuantity,Godownid,Lotno,UnitId From ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And 
        //        PRT.Finishedid=VF.ITEM_FINISHED_ID And PRM.TranType=1 And PRT.PRTId=" + DGMain.SelectedDataKey.Value + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "");
        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            ddCatagory.SelectedValue = Ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //            Fill_Category_SelectedChange();
        //            dditemname.SelectedValue = Ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
        //            ItemName_SelectChange();
        //            if (ql.Visible == true)
        //            {
        //                dquality.SelectedValue = Ds.Tables[0].Rows[0]["QualityId"].ToString();
        //            }
        //            if (dsn.Visible == true)
        //            {
        //                dddesign.SelectedValue = Ds.Tables[0].Rows[0]["designId"].ToString();
        //            }
        //            if (clr.Visible == true)
        //            {
        //                ddcolor.SelectedValue = Ds.Tables[0].Rows[0]["ColorId"].ToString();
        //            }
        //            if (shp.Visible == true)
        //            {
        //                ddshape.SelectedValue = Ds.Tables[0].Rows[0]["ShapeId"].ToString();
        //            }
        //            if (sz.Visible == true)
        //            {
        //                ddsize.SelectedValue = Ds.Tables[0].Rows[0]["SizeId"].ToString();
        //            }
        //            if (shd.Visible == true)
        //            {
        //                ddlshade.SelectedValue = Ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
        //            }
        //            ddgodown.SelectedValue = Ds.Tables[0].Rows[0]["Godownid"].ToString();
        //            txtlot.Text = Ds.Tables[0].Rows[0]["Lotno"].ToString();
        //            txtrecqty.Text = Ds.Tables[0].Rows[0]["IssueQuantity"].ToString();
        //            FillQuantity();
        //        }
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
            UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct designId,designName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Design");
            if (dddesign.Items.Count > 0)
            {
                dddesign.SelectedIndex = 1;
            }
        }
        if (clr.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct ColorId,ColorName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Color");
            if (ddcolor.Items.Count > 0)
            {
                ddcolor.SelectedIndex = 1;
            }
        }
        if (shp.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct ShapeId,ShapeName From ProcessRawMaster PRM,ProcessRawTran PRT,
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
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
                        V_FinishedItemDetail VF Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=VF.ITEM_FINISHED_ID And TranType=0 And PRM.Prorderid=" + ddPOrderNo.SelectedValue + @" And 
                        VF.ITEM_ID=" + dditemname.SelectedValue + " And VF.Qualityid=" + dquality.SelectedValue + " And PRM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select ");
            if (ddlshade.Items.Count > 0)
            {
                ddlshade.SelectedIndex = 1;
            }
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
                    PRM.Prorderid,U.UnitName,Om.LocalOrder,CI.GSTNO,EI.GSTNO as EmpGstno FROM  ProcessRawMaster PRM  INNER JOIN  ProcessRawTran PRT ON PRM.PRMid=PRT.PRMid 
                    INNER JOIN GodownMaster GM ON GM.GoDownID=PRT.Godownid 
                    INNER JOIN EmpInfo EI ON PRM.Empid=EI.EmpId 
                    INNER JOIN CompanyInfo CI ON PRM.Companyid=CI.CompanyId 
                    INNER JOIN PROCESS_NAME_MASTER PNM ON PRM.Processid=PNM.PROCESS_NAME_ID
                    INNER JOIN V_FinishedItemDetail Vf ON PRT.Finishedid=Vf.ITEM_FINISHED_ID 
                    INNER JOIN Unit U ON U.UnitId=PRT.UnitId
                    inner join(select  Distinct IssueOrderId,Orderid from PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PID)as Process_issue_Detail
                    on Process_issue_Detail.IssueOrderId=prm.Prorderid
                    inner join OrderMaster OM on OM.OrderId=Process_issue_Detail.Orderid
                    Where PRM.PrmId=" + ViewState["Prmid"];
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
                             PT.Lotno, GM.GodownName, EI.EmpName, EI.Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, 
                             CI.CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                             vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                             PM.Prorderid, EI.GSTNo as empgstin, CI.GSTNo,PT.TagNo,PT.BinNo From ProcessRawMaster PM inner join ProcessRawTran PT on PM.PRMid=PT.PRMid
                             inner join CompanyInfo ci on PM.Companyid=ci.CompanyId
                             inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                             inner join GodownMaster GM on PT.Godownid=GM.GoDownID
                             inner join EmpInfo Ei on PM.Empid=ei.EmpId
                             inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID Where PM.Prmid=" + ViewState["Prmid"];

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

        string str = @"SELECT Top(1) EMP.ProcessId,EI.EmpId,LS.IssueOrderId FROM EMPLOYEE_PROCESSORDERNO EMP INNER JOIN EMPINFO EI ON EMP.EMPID=EI.EMPID 
                        INNER JOIN LOOMSTOCKNO LS ON EMP.IssueOrderId=LS.Issueorderid and EMP.IssueDetailId=LS.IssueDetailid
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
    }

    protected void DDtagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
    }
}