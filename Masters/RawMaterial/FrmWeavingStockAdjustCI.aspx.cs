using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_FrmWeavingStockAdjustCI : System.Web.UI.Page
{
    static int MasterCompanyId;
    static string TempLotNo = "";
    static string TempSelectedGodownId = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        DataSet DSQ = null; string Qry = "";
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            Qry = @" select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname
                     select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                     WHere PNM.ProcessType=1 and PNM.Process_Name='WEAVING'  order by PROCESS_NAME
                     Select VarProdCode,VarCompanyNo From MasterSetting 
                     Select ID, BranchName 
                        From BRANCHMASTER BM(nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                        Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                    Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 3, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            //UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 4, false, "");

            int VarProdCode = Convert.ToInt32(DSQ.Tables[2].Rows[0]["VarProdCode"]);
            int VarCompanyId = Convert.ToInt32(DSQ.Tables[2].Rows[0]["VarCompanyNo"]);
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //ddCompName.SelectedIndex = 1;
            switch (VarProdCode)
            {
                case 0:
                    procode.Visible = false;
                    break;
                case 1:
                    procode.Visible = true;
                    break;
            }
            lablechange();            
           
            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                txtchalanno.Enabled = false;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChange();
    }
    private void ProcessNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        //Fill_Grid();
        string str = "";
        if (variable.VarFinishingNewModuleWise == "1" && ddProcessName.SelectedValue != "1")
        {
            str = @"select EI.EmpId,EI.EmpName+' '+case when isnull(EI.empcode,'')='' Then '' ELse '['+ EI.empcode+']' End 
                    From Empinfo EI 
                    join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.Processid=" + ddProcessName.SelectedValue;
        }
        else
        {
            str = @"SELECT distinct e.EmpId, e.EmpName  
                        FROM  PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM 
                                            JOIN  EmpInfo e ON pim.Empid = e.EmpId And e.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                        UNION 
                        Select EI.EmpId, EI.EmpName 
                        From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(Nolock) 
                        JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = PIM.IssueOrderId And ProcessID = " + ddProcessName.SelectedValue + @" 
                        JOIN EmpInfo EI(Nolock) ON EI.EmpID = EPO.Empid ";
            //if (Session["varMasterCompanyIDForERP"].ToString() == "28")
            //{
            //    str = str + " Where PIM.FlagStockNoAttachWithoutRawMaterialIssue = 0";
            //}

            if (Session["varMasterCompanyIDForERP"].ToString() == "43")
            {
                str = str + " Where PIM.FlagStockNoAttachWithoutRawMaterialIssue = 0";
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "45")
            {
                str = str + " Where PIM.FlagStockNoAttachWithoutRawMaterialIssue = 0";
            }
            else
            {
                str = str + " Where PIM.FlagStockNoAttachWithoutRawMaterialIssue = 1";
            }
            str = str + " Order By EmpName";
        }
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Select--");
    }
    
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Prmid"] = 0;
        txtchalanno.Text = "";
        BindCarpetQuality();
        //EmpNameSelectedIndexChange();

        if (ChKForEdit.Checked == true)
        {
            ChKForEdit_CheckedChanged(sender,new EventArgs());
        }
    }
    private void BindCarpetQuality()
    {
        ViewState["Prmid"] = 0;
        string str = "";
        str = @"select Distinct VF.Qualityid, VF.Qualityname 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(NoLock) JOIN  PROCESS_ISSUE_Detail_" + ddProcessName.SelectedValue + @" PID(NoLock) ON PIM.IssueOrderId=PID.IssueOrderId
                JOIN V_FinishedItemDetail VF ON PID.Item_Finished_Id=VF.Item_Finished_Id
                join EmpInfo ei on PIM.Empid=ei.EmpId 
                WHERE --IsNull(PIM.DEPARTMENTTYPE, 0) = 0 And 
                PIM.Companyid=" + ddCompName.SelectedValue + " and PIM.Empid=" + ddempname.SelectedValue + " and isnull(pim.FOLIOSTATUS,0)=0 and isnull(PIM.samplenumber,'')='' and PIM.Status='Pending'";

        //if (ChkForCompleteStatus.Checked == true)
        //{
        //    str = str + @" And PIM.Status = 'Complete'";
        //}
        //else
        //{
        //    str = str + @" And PIM.Status <> 'Complete'";
        //}
        str = str + " UNION  ";
        str = str + @" select Distinct VF.Qualityid, VF.Qualityname 
            From Process_issue_Master_" + ddProcessName.SelectedValue + @" PIM(NoLock) JOIN  PROCESS_ISSUE_Detail_" + ddProcessName.SelectedValue + @" PID(NoLock) ON PIM.IssueOrderId=PID.IssueOrderId 
            JOIN V_FinishedItemDetail VF ON PID.Item_Finished_Id=VF.Item_Finished_Id
            join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=" + ddProcessName.SelectedValue + @" And pim.Empid=0 
            Where --IsNull(PIM.DEPARTMENTTYPE, 0) = 0 And 
            PIm.Companyid=" + ddCompName.SelectedValue + " and EMP.Empid=" + ddempname.SelectedValue + " and isnull(pim.FOLIOSTATUS,0)=0 and isnull(PIM.samplenumber,'')='' and PIM.Status='Pending'";

        //if (ChkForCompleteStatus.Checked == true)
        //{
        //    str = str + @" And PIM.Status = 'Complete'";
        //}
        //else
        //{
        //    str = str + @" And PIM.Status <> 'Complete'";
        //}
        UtilityModule.ConditionalComboFill(ref DDCarpetQuality, str, true, "---Select Series---");
    }
    private void BindRawMaterialCategory()
    {
        string str = "";
        str =str+ "0|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 0);
        cmd.Parameters.AddWithValue("@ParameterValue", str);       
       

        //cmd.Parameters.AddWithValue("@CategoryId", ddCatagory.SelectedIndex>0 ? ddCatagory.SelectedValue :"0");
        //cmd.Parameters.AddWithValue("@ItemId", dditemname.SelectedIndex > 0 ? dditemname.SelectedValue : "0");
        //cmd.Parameters.AddWithValue("@QualityId", dquality.SelectedIndex > 0 ? dquality.SelectedValue : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 0, true, "--Plz Select--");
    }

    protected void DDCarpetQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalanno.Text = "";
        BindRawMaterialCategory();
        //EmpNameSelectedIndexChange();
    }

    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        //EditCheckedChanged();

        //if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        //{
        //    btnsave.Visible = false;
        //    BtnUpdateRemark.Visible = false;
        //    gvdetail.Columns[8].Visible = false;
        //}
    }
    private void EditCheckedChanged()
    {
        if (ChKForEdit.Checked == true)
        {
            //if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28" || Session["varMasterCompanyIDForERP"].ToString() == "31")
            //{
            //    TDForCompleteStatus.Visible = true;
            //}
            Td7.Visible = true;
            
            string str = "";
            str = @"Select PRM.ChalanNo, PRM.ChalanNo as ChallanNo 
                    from ProcessRawMaster PRM
                    Where PRM.TypeFlag = 0 And PRM.TranType=0 And PRM.EmpId=" + ddempname.SelectedValue + " And PRM.CompanyID = " + ddCompName.SelectedValue + @" And 
                    PRM.BranchID = " + DDBranchName.SelectedValue + @" And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                    Group by ChalanNo 
                    order by cast(RIGHT(PRM.ChalanNo,charindex('/',reverse(PRM.ChalanNo),1)-1) as int) desc";

            UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "Select Challan No");

        }
        else
        {
            //TDForCompleteStatus.Visible = false;
            Td7.Visible = false;
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedIndexChange();
    }
    private void ChallanNoSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        txtchalanno.Text = "";
        if (DDChallanNo.SelectedIndex > 0)
        {
            //ViewState["Prmid"] = DDChallanNo.SelectedValue;
            //string strsql2 = "select PRMID,ChalanNo from ProcessRawMaster PRM where PRM.TypeFlag = 0 And PRM.Prmid=" + DDChallanNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];  
                     
            string strsql2 = "select PRMID,ChalanNo from ProcessRawMaster PRM where PRM.TypeFlag = 0 And PRM.ChalanNo='" + DDChallanNo.SelectedItem.Text + "' And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                     
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChalanNo"].ToString();
                ViewState["Prmid"] = ds2.Tables[0].Rows[0]["PRMID"].ToString();
            }

            ////if (DDChallanNo.SelectedItem.Text.Split('/').Length > 1)
            ////{
            ////    txtchalanno.Text = DDChallanNo.SelectedItem.Text.Split('/')[1];
            ////}
            ////else
            ////{
            ////    txtchalanno.Text = DDChallanNo.SelectedItem.Text;
            ////}           
            
            //string strsql = "select TransportName,BiltyNo,VehicleNo,Remark,isnull(EWayBillNo,'') as EWayBillNo from ProcessRawMaster PRM where PRM.TypeFlag = 0 And PRM.Prmid=" + DDChallanNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];                       

            //string strsql = "select TransportName,BiltyNo,VehicleNo,Remark,isnull(EWayBillNo,'') as EWayBillNo from ProcessRawMaster PRM where PRM.TypeFlag = 0 And PRM.ChalanNo='" + DDChallanNo.SelectedItem.Text + "' And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtTransportName.Text = ds.Tables[0].Rows[0]["TransportName"].ToString();
            //    txtBiltyNo.Text = ds.Tables[0].Rows[0]["BiltyNo"].ToString();
            //    txtVehicleNo.Text = ds.Tables[0].Rows[0]["VehicleNo"].ToString();
            //    txtremark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
            //    txtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            //}



        }        
    }
    private void BindRawMaterialItem()
    {  
        string str = "";
        str = str + ddCatagory.SelectedValue+ "|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 1);
        cmd.Parameters.AddWithValue("@ParameterValue", str);    

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref dditemname, ds, 0, true, "--Plz Select--");
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();
            BindRawMaterialItem();
        }
        
        //Fill_Category_SelectedChange();
    }
    private void BindRawMaterialDesign()
    {
        string str = "";
        str = str +" 0|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 3);
        cmd.Parameters.AddWithValue("@ParameterValue", str);
      
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref dddesign, ds, 0, true, "--Plz Select--");
    }
    private void BindRawMaterialColor()
    {
        string str = "";
        str = str + " 0|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 4);
        cmd.Parameters.AddWithValue("@ParameterValue", str);     

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref ddcolor, ds, 0, true, "--Plz Select--");
    }
    private void BindRawMaterialShape()
    {
        string str = "";
        str = str + " 0|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 5);
        cmd.Parameters.AddWithValue("@ParameterValue", str);  

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref ddshape, ds, 0, true, "--Plz Select--");
    }
    private void BindRawMaterialSize()
    {
        string str = "";
        str = str + " 0|0|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 6);
        cmd.Parameters.AddWithValue("@ParameterValue", str);       

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref ddsize, ds, 0, true, "--Plz Select--");
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
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
                    case "3":
                        clr.Visible = true;
                        if (hnissampleorder.Value == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddcolor, "select Colorid,Colorname From Color Where MasterCompanyId=" + ddcolor.SelectedValue + " order by ColorName", true, "--Select Color--");
                        }
                        else
                        {
                            BindRawMaterialColor();
//                            UtilityModule.ConditionalComboFill(ref ddcolor, @"SELECT DISTINCT dbo.color.ColorId, dbo.color.ColorName FROm dbo.ITEM_PARAMETER_MASTER INNER JOIN
//                        PROCESS_CONSUMPTION_DETAIL ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = PROCESS_CONSUMPTION_DETAIL.IFinishedId INNER JOIN
//                        dbo.color ON dbo.ITEM_PARAMETER_MASTER.COLOR_ID = dbo.color.ColorId
//                        where PROCESS_CONSUMPTION_DETAIL.issueorderid=" + ddOrderNo.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Color--");
                        }
                        break;
                    case "2":
                        dsn.Visible = true;
                        if (hnissampleorder.Value == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref dddesign, "select designId,designName From design  Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by designName", true, "--Select Design--");
                        }
                        else
                        {
                            BindRawMaterialDesign();
//                            UtilityModule.ConditionalComboFill(ref dddesign, @"SELECT DISTINCT dbo.Design.designId, dbo.Design.designName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
//                        PROCESS_CONSUMPTION_DETAIL ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = PROCESS_CONSUMPTION_DETAIL.IFinishedId INNER JOIN
//                        dbo.Design ON dbo.ITEM_PARAMETER_MASTER.DESIGN_ID = dbo.Design.designId
//                        where PROCESS_CONSUMPTION_DETAIL.issueorderid=" + ddOrderNo.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Design--");
                        }
                        break;
                    case "4":
                        shp.Visible = true;
                        if (hnissampleorder.Value == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName From Shape Where MasterCompanyid=" + ddshape.SelectedValue + " order by ShapeId", true, "--Select Shape--");
                        }
                        else
                        {
                            BindRawMaterialShape();
//                            UtilityModule.ConditionalComboFill(ref ddshape, @"SELECT DISTINCT dbo.Shape.ShapeId, dbo.Shape.ShapeName  FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
//                        PROCESS_CONSUMPTION_DETAIL ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = PROCESS_CONSUMPTION_DETAIL.IFinishedId INNER JOIN
//                        dbo.Shape ON dbo.ITEM_PARAMETER_MASTER.SHAPE_ID = dbo.Shape.ShapeId
//                        where PROCESS_CONSUMPTION_DETAIL.issueorderid=" + ddOrderNo.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Shape--");
                        }
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
                        if (hnissampleorder.Value == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select SizeId,SizeFt From Size WHere shapeid=" + ddshape.SelectedValue + "  and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " ", true, "Size in Ft");
                        }
                        else
                        {
                            BindRawMaterialSize();
//                            UtilityModule.ConditionalComboFill(ref ddsize, @"SELECT DISTINCT dbo.Size.SizeId, dbo.Size.SizeFt FROM dbo.ITEM_PARAMETER_MASTER INNER JOIN
//                        PROCESS_CONSUMPTION_DETAIL ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = PROCESS_CONSUMPTION_DETAIL.IFinishedId INNER JOIN
//                        dbo.Size ON dbo.ITEM_PARAMETER_MASTER.SIZE_ID = dbo.Size.SizeId
//                        where PROCESS_CONSUMPTION_DETAIL.issueorderid=" + ddOrderNo.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Ft");
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
    }
    private void BindRawMaterialQuality()
    {
        string str = "";
        str = str + ddCatagory.SelectedValue + "|"+dditemname.SelectedValue+ "|0|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 2);
        cmd.Parameters.AddWithValue("@ParameterValue", str);      

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 0, true, "--Plz Select--");
    }
    private void BindRawMaterialShadeColor()
    {
        string str = "";
        str = str + " 0|"+dditemname.SelectedValue+"|"+dquality.SelectedValue+"|0|0|0|0|0~";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindRawMaterialItemParameterDropDown", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", ddempname.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", ddProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@CarpetQualityId", DDCarpetQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@ChkForCompleteStatus", 0);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@Type", 7);
        cmd.Parameters.AddWithValue("@ParameterValue", str);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref ddlshade, ds, 0, true, "--Plz Select--");
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemName_SelectChange();
    }
    private void ItemName_SelectChange()
    {
        if (dditemname.SelectedIndex >= 0)
        {
            BindRawMaterialQuality();
            string Qry = "";
            Qry = @" SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
            //UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 1, true, "Select Quallity");
            if (dquality.Items.Count > 0)
            {
                if (dquality.Items.Count == 1)
                {
                    dquality.SelectedIndex = 0;
                }
                else
                {
                    dquality.SelectedIndex = 1;
                }

                QualitySelectedIndexChange();

            }
            if (ddlunit.Items.Count > 0)
            {
                ddlunit.SelectedIndex = 1;
            }
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        QualitySelectedIndexChange();
    }
    private void QualitySelectedIndexChange()
    {
        BindRawMaterialShadeColor();
        
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        TempLotNo = "";
        TempSelectedGodownId = "0";
       
    }


    private string CheckStockQty()
    {
        string str = "";
        try
        {
//            //int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "");
//            SqlParameter[] parparam = new SqlParameter[4];

//            int OrderID = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Orderid From PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + " Where IssueOrderId=" + ddOrderNo.SelectedValue));
//            parparam[0] = new SqlParameter("@OrderId", OrderID);
//            parparam[1] = new SqlParameter("@ProcessNo", ddProcessName.SelectedValue);
//            parparam[2] = new SqlParameter("@FinishedID", ViewState["FinishedID"]);
//            parparam[3] = new SqlParameter("@TxtQty", txtissue.Text);
//            parparam[4] = new SqlParameter("@Message", SqlDbType.NVarChar, 2);
//            parparam[4].Direction = ParameterDirection.Output;
//            parparam[5] = new SqlParameter("@PrtIdFlag", ddProcessName.SelectedValue);

//            string Str = @"Select Select ISNULL(Sum(IssueQuantity),0) From ProcessRawMaster PRM,ProcessRawTran PRT Where PRM.PRMid=PRT.PRMid And PRM.trantype=0 And PRM.TypeFlag = 0 And 
//            PRM.Processid=" + ddProcessName.SelectedValue + " And PRM.Prorderid in (Select IssueOrderid From PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" 
//            Where OrderId in (Select Orderid From PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + " Where IssueOrderId=" + ddOrderNo.SelectedValue + ")) And PRTid=0 And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

//            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_CheckStockQtyForProcessIssue", parparam);
//            if (parparam[4].Value.ToString() == "G")
//            {
//                LblError.Visible = true;
//                LblError.Text = "IssueQty should not be greater than stock";
//                str = "G";
//            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/RawMaterial/IndentRawIssue");
        }
        return str;
    }
    protected void DuplicateChallanNo()
    {
        LblError.Text = "";
        LblError.Visible = true;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtchalanno.Text != "")
            {
                string str = "Select ChalanNo From ProcessRawMaster Where ChalanNo<>'' And TranType=0 And TypeFlag = 0 And ChalanNo='" + txtchalanno.Text + "' and Empid>0 And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL(); 

        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[16];

                arr[0] = new SqlParameter("@ID", SqlDbType.Int);
                arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                arr[2] = new SqlParameter("@BranchId", SqlDbType.Int);
                arr[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                arr[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                arr[5] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                arr[6] = new SqlParameter("@ChallanNo", SqlDbType.NVarChar, 50);
                arr[7] = new SqlParameter("@FlagType", SqlDbType.Int);
                arr[8] = new SqlParameter("@FinishedIdQuality", SqlDbType.Int);
                arr[9] = new SqlParameter("@Item_Finished_Id", SqlDbType.Int);
                arr[10] = new SqlParameter("@Qty", SqlDbType.Float);
                arr[11] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 100);
                arr[12] = new SqlParameter("@UserId", SqlDbType.Int);
                arr[13] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                arr[14] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                arr[15] = new SqlParameter("@FinishedIdQualityName", SqlDbType.NVarChar, 40);

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                               
                //arr[0].Value = ViewState["ID"];
                arr[0].Value = 0;
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = DDBranchName.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = ddempname.SelectedValue;
                arr[5].Value = txtdate.Text;
                arr[6].Value = txtchalanno.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 0;
                arr[8].Value = DDCarpetQuality.SelectedValue;
                arr[9].Value = Varfinishedid;
                arr[10].Value = txtissue.Text;
                arr[11].Value = txtremark.Text;
                arr[12].Value = Session["varuserid"].ToString();
                arr[13].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[14].Direction = ParameterDirection.Output;
                arr[15].Value = DDCarpetQuality.SelectedItem.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEWEAVINGSTOCKADJUSTCI", arr);

                Tran.Commit();
                txtchalanno.Text = arr[6].Value.ToString();
                ViewState["Id"] = arr[0].Value;
                LblError.Visible = true;
                LblError.Text = arr[14].Value.ToString();
                //Fill_Grid();                
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
    private void SaveReferece()
    {       
        if (ddlshade.Items.Count > 0 && shd.Visible == true)
        {
            ddlshade.SelectedIndex = 0;
        }           
        txtissue.Text = "";

    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(ChalanNo,0) asd from ProcessRawMaster where TypeFlag = 0 And ChalanNo='" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]));
        if (ChalanNo != "")
        {
            txtchalanno.Text = "";
            txtchalanno.Focus();
            LblError.Visible = true;
            LblError.Text = "Challan No already exist";
        }
        else
        {
            LblError.Visible = false;
        }
    } 
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        //if (TxtProdCode.Text != "" && ddOrderNo.SelectedIndex > 0)
        if (TxtProdCode.Text != "" && DDCarpetQuality.SelectedIndex > 0)
        {

            //Str = "select IPM.*,IM.CATEGORY_ID  from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,PROCESS_CONSUMPTION_DETAIL PCD  WHERE IPM.ITEM_FINISHED_ID = PCD.IFINISHEDID and PCD.ISSUEORDERID =" + ddOrderNo.SelectedValue + " and IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            Str = "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Qry = @"select category_id,category_name from item_category_master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + " Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + "  select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ";
                Qry = Qry + "  SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorid";
                Qry = Qry + "  select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid  ";
                Qry = Qry + "  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd SHAPEID=" + ddshape.SelectedValue + "";
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "select");
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dditemname, DSQ, 1, true, "--Select Item--");
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "--Select Design--");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();

                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;

                }
                else
                {
                    ql.Visible = false;

                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
                {
                    dsn.Visible = true;
                }
                else
                {

                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;

                }
                else
                {
                    clr.Visible = false;
                }


                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }

                else
                {
                    shp.Visible = false;
                }

                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Unit");
            }
            else
            {
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCatagory.Items.Clear();
        }
        //fill_qty();
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
    private void Validated()
    {

    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("id");
        Session.Remove("finishedid");      
    }
    protected void txtissue_TextChanged(object sender, EventArgs e)
    {
        //double stockqty = Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
        //double TotalPendQty = Convert.ToDouble(TxtPendQty.Text == "" ? "0" : TxtPendQty.Text);

        //double Qty = Convert.ToDouble(txtissue.Text == "" ? "0" : txtissue.Text);
        //double coneweight = UtilityModule.Getconeweight(DDconetype.SelectedItem.Text, Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text));
        //Qty = Qty - coneweight;

        //if (Qty > stockqty || Qty > TotalPendQty)
        //{
        //    txtissue.Text = "";
        //    LblError.Text = "Pls Enter Correct Qty ";
        //    LblError.Visible = true;
        //    txtissue.Focus();
        //    return;
        //}
        //else
        //{
        //    LblError.Visible = false;
        //}       

    }   
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForMtr.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Mtr");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblError.Visible = true;
        LblError.Text = "";
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
        //if (UtilityModule.VALIDDROPDOWNLIST(ddOrderNo) == false)
        //{
        //    goto a;
        //}
        //if (TdDDItemDesignName.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDItemDesignName) == false)
        //    {
        //        goto a;
        //    }
        //}
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

        
        if (UtilityModule.VALIDTEXTBOX(txtissue) == false)
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        //CarpetInternationalFormatReport();

    }    
    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();
            //***********Sample
            if (hnissampleorder.Value == "1")
            {
                UtilityModule.ConditionalComboFill(ref dditemname, "select ITEM_ID,ITEM_NAME From Item_Master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " and Mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by ITEM_NAME", true, "--Select Item--");
            }
            else
            {
//                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select WithoutCottonMaterial 
//                    From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @"(Nolock) 
//                    Where IsNull(WithoutCottonMaterial, 0) = 1 And IssueOrderID = " + ddOrderNo.SelectedValue);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT VF.ITEM_ID, VF.ITEM_NAME 
//                    FROM PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
//                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFinishedId And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
//                        VF.ITEM_NAME not Like '%Cloth%'
//                    Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " and PCD.Processid = " + ddProcessName.SelectedValue + @" And 
//                    PCD.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + "", true, "--Select Item--");
//                }
//                else
//                {
//                    UtilityModule.ConditionalComboFill(ref dditemname, @"SELECT DISTINCT VF.ITEM_ID, VF.ITEM_NAME 
//                    FROM PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
//                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFinishedId And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
//                    Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " and PCD.Processid = " + ddProcessName.SelectedValue + @" And 
//                    PCD.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + "", true, "--Select Item--");
//                }
                
            }

            if (dditemname.Items.Count > 0)
            {
                dditemname.SelectedIndex = 1;
                ItemName_SelectChange();
            }
        }
    }    
    private void Fill_Temp_OrderNo()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_ISSUE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT PM.Companyid,OM.Customerid,PD.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid FROM PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid");
            }
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
       // FillProcess_Employee(sender);
    }  
    protected void ChkForCompleteStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (ddempname.Items.Count > 0)
        {
            ddempname.SelectedIndex = -1;
            //ddOrderNo.Items.Clear();
        }
    }    
}