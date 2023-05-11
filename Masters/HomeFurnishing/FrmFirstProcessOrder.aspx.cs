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

public partial class Masters_HomeFurnishing_FrmFirstProcessOrder : System.Web.UI.Page
{
    static int hnEmpId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            hnEmployeeType.Value = "0";
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                From Companyinfo CI
                JOIN Company_Authentication CA ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName 
                Select Distinct CI.CustomerId, CI.CustomerCode 
                From CustomerInfo CI (Nolock) 
                JOIN OrderMaster OM(Nolock) ON OM.CustomerId = CI.CustomerId 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.PoufTypeCategory = 1 
                Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By Customercode 
                Select UnitId, UnitName From Unit(Nolock) Where Unitid in (1,2,6) 
                Select PROCESS_NAME_ID, PROCESS_NAME From PROCESS_NAME_MASTER PNM(Nolock) 
                Where PROCESS_NAME = 'STITCHING' And MasterCompanyID = " + Session["varCompanyId"] + @" Order By Process_Name ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 3, false, "");

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            hnissueorderid.Value = "0";
            if (ddunit.Items.FindByValue(variable.VarDefaultProductionunit) != null)
            {
                ddunit.SelectedValue = variable.VarDefaultProductionunit;
            }
            DDcaltype.SelectedValue = "1";
            hnordercaltype.Value = DDcaltype.SelectedValue;
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    txtWeaverIdNoscan.Visible = true;
                    txtWeaverIdNo.Visible = false;
                    break;
                case "44":
                     txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    ddunit.Enabled = false;
                    break;
                default:
                    txtWeaverIdNo.Visible = true;
                    txtWeaverIdNoscan.Visible = false;
                    break;
            }
        }
    }
    protected void FillFolioNo()
    {
        string Str = "";
        if (chkEdit.Checked == true)
        {
            Str = @"Select Distinct a.IssueOrderId, a.CHALLANNO 
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock) 
                JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderID = a.IssueOrderID And EPO.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = EPO.EmpID 
                Where a.Units is null And a.CompanyID = " + DDcompany.SelectedValue;
            if (chkcomplete.Checked == true)
            {
                Str = Str + " And a.[Status] = 'Complete'";
            }
            else
            {
                Str = Str + " And a.[Status] = 'Pending' ";
            }
            if (Session["varcompanyNo"].ToString() == "16")
            {
                Str = Str + " And a.AssignDate > '2021-11-30'";
            }
            if (txteditempid.Text != "")
            {
                Str = Str + " And EI.EMPID = " + txteditempid.Text + "";
            }
            if (txteditempcode.Text != "")
            {
                Str = Str + " And EI.EMPCode = '" + txteditempcode.Text + "'";
            }
            if (txtfoliono.Text != "")
            {
                Str = Str + " And a.CHALLANNO = '" + txtfoliono.Text + "' ";
            }
            Str = Str + " Order By a.IssueOrderId Desc";

            UtilityModule.ConditionalComboFill(ref DDFolioNo, Str, true, "--Plz Select--");
        }
    }

    protected void FillWeaverWithBarcodescan()
    {
        string str = "";
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
                            if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281")
                            {
                                txtWeaverIdNoscan.Text = "";
                                txtWeaverIdNoscan.Focus();
                                return;
                            }
                        }
                    }

                    Boolean addflag = true;
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
                            hnEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                            listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
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
            txtWeaverIdNoscan.Focus();
        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
    }
    protected void FillWeaver()
    {
        string str = "";
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
                            if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281")
                            {
                                txtWeaverIdNoscan.Text = "";
                                txtWeaverIdNoscan.Focus();
                                return;
                            }
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
                                    hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
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
                                    hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
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
                                hnordercaltype.Value = "1";
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

                            //if (Session["VarCompanyNo"].ToString() == "27" && hnEmployeeType.Value == "1")
                            //{
                            //    //hnEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                            //}
                            //else
                            //{
                            //    //hnEmpId = 0;
                            //}
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                }

                ds.Dispose();
                txtWeaverIdNo.Text = "";
            }

            txtWeaverIdNo.Focus();
        }
        catch (Exception ex)
        {
            lblmessage.Visible = true;
            lblmessage.Text = ex.Message;
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "", view = "";
        view = "V_ORDERBALITEMTOBEORDERED_TAGGINGWITHINTERALPROD";
        if (hnEmployeeType.Value == "1")
        {
            view = "V_ORDERBALITEMTOBEORDERED_PREPRODASSIGNEDQTY";
        }
        str = @"select Distinct OM.OrderId, OM.CustomerOrderNo OrderNo 
            From OrderMaster OM(Nolock) 
            JOIN OrderDetail OD(Nolock) ON OD.OrderId = OM.OrderId 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.ITEM_FINISHED_ID And VF.PoufTypeCategory = 1 
            Where OM.CompanyId = " + DDcompany.SelectedValue + " And OM.Status = 0 And OM.CustomerId = " + DDcustcode.SelectedValue + @" 
            Order By OM.OrderId";

        UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
    }
    protected void FillissueDetails()
    {
        string str = "";

        if (hnordercaltype.Value != "")
        {
            string length = "", Width = "", Area = "", ColumnName = "";

            ColumnName = "Areamtr";
            switch (hnordercaltype.Value)
            {
                case "1": //Pcs Wise
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            length = "LengthMtr";
                            Width = "Widthmtr";
                            Area = ColumnName;
                            break;
                        case "2":
                            length = "Lengthft";
                            Width = "Widthft";
                            Area = "Actualfullareasqyd";
                            break;
                        default:
                            length = "Lengthft";
                            Width = "Widthft";
                            Area = "Actualfullareasqyd";
                            break;
                    }
                    break;
                default:
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            {
                                length = "PRODLENGTHMTR";
                                Width = "PRODWIDTHMTR";
                                Area = "PRODAREAMTR";
                            }
                            break;
                        case "2":
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                        default:
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                    }
                    break;
            }
            string Function = string.Empty;
            string Qtyrequired = "VJ.INTERNALPRODASSIGNEDQTY";
            if (Session["varcompanyId"].ToString() == "44")
            {
                if (hnEmployeeType.Value == "1")
                {
                    Qtyrequired = "vj.preprodassignedqty";
                }
                Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL_NEW_AGNI]";
            }
            else { Function = "[F_GETPRODUCTIONORDERQTY_INTERNAL_NEW]"; }

            if (hnEmployeeType.Value == "1")
            {
                Qtyrequired = "vj.preprodassignedqty";
                Function = "[F_GETPRODUCTIONORDERQTY_ExterNal_NEW]";
            }
            if (Session["varcompanyId"].ToString() == "44")
            {
                str = @"select Om.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
            case When " + hnordercaltype.Value + " = 1 Then VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME +' '+Case When OD.OrderUnitId = 1 Then vf.LWHMtr  When OD.OrderUnitId = 6 Then VF.LWHInch Else VF.LWHFt End  ELse dbo.F_getItemDescription(OD.Item_Finished_Id, Case when " + ddunit.SelectedValue + " = 1 Then 1 ELse case when " + ddunit.SelectedValue + " = 2 Then 0 Else Od.flagsize ENd ENd) END ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
            " + Qtyrequired + @" QtyRequired, dbo." + Function + @"(OM.OrderId, OD.Item_Finished_Id) OrderedQty, JOBRATE.RATE, 
            LENGTH=Case When OD.OrderUnitId = 1 Then LengthMtr  When OD.OrderUnitId = 6 THEN LengthINCH ELSE LengthFt END,
                            Width=Case When OD.OrderUnitId = 1 Then WidthMtr  When OD.OrderUnitId = 6 THEN WidthINCH ELSE WidthFt END,
                            Area=Case When OD.OrderUnitId = 1 Then AreaMtr  When OD.OrderUnitId = 6 THEN AreaInch ELSE AreaFt END,
            vf.shapeid,JOBRATE.COMMRATE 
            From OrderMaster OM(Nolock) 
            JOIN OrderDetail OD(Nolock) on OM.OrderId=OD.OrderId
            JOIN V_finisheditemdetail vf(Nolock) on Od.Item_finished_id=vf.item_finished_id AND VF.PoufTypeCategory = 1 
            JOIN V_JOBASSIGNSQTY VJ(Nolock) on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id
            JOIN Unit U(Nolock) on OD.OrderUnitId=U.UnitId 
            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id, " + DDProcessName.SelectedValue + "," + ddunit.SelectedValue + "," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + ">0  order by OD.orderdetailid";



            }
            else
            {
                str = @"select Om.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
            case When " + hnordercaltype.Value + " = 1 Then VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + case when " + ddunit.SelectedValue + @" = 1 Then Vf.Sizemtr Else vf.sizeft End ELse
            dbo.F_getItemDescription(OD.Item_Finished_Id, Case when " + ddunit.SelectedValue + " = 1 Then 1 ELse case when " + ddunit.SelectedValue + " = 2 Then 0 Else Od.flagsize ENd ENd) END ItemDescription,'" + ddunit.SelectedItem.Text + @"' as UnitName,
            " + Qtyrequired + @" QtyRequired, dbo." + Function + @"(OM.OrderId, OD.Item_Finished_Id) OrderedQty, JOBRATE.RATE, 
            LENGTH = case when " + hnordercaltype.Value + @" = 1 Then (CASE WHEN " + ddunit.SelectedValue + " = 1 THEN cast(vf." + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @" = 2 THEN cast(vf." + length + " as varchar(20)) ELSE cast(vf." + length + @" as varchar(20)) END)  Else 
            (CASE WHEN " + ddunit.SelectedValue + " = 1 THEN cast(" + length + " as varchar(20)) WHEN " + ddunit.SelectedValue + @" = 2 THEN cast(" + length + " as varchar(20)) ELSE cast(" + length + @" as varchar(20)) END) END, 
            Width = case when " + hnordercaltype.Value + @" = 1 Then  (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(vf." + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(vf." + Width + " as varchar(20)) ELSE cast(vf." + Width + @" as varchar(20)) END) Else
            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN cast(" + Width + " as varchar(20)) WHEN " + ddunit.SelectedValue + @"=2 THEN cast(" + Width + " as varchar(20)) ELSE cast(" + Width + @" as varchar(20)) END) END,
            Area=case when " + hnordercaltype.Value + @"=1 Then (CASE WHEN " + ddunit.SelectedValue + "=1 THEN vf." + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN vf." + Area + " ELSE vf." + Area + @" END) else
            (CASE WHEN " + ddunit.SelectedValue + "=1 THEN " + Area + " WHEN " + ddunit.SelectedValue + @"=2 THEN " + Area + " ELSE " + Area + @" END) END,
            vf.shapeid,JOBRATE.COMMRATE 
            From OrderMaster OM(Nolock) 
            JOIN OrderDetail OD(Nolock) on OM.OrderId=OD.OrderId
            JOIN V_finisheditemdetail vf(Nolock) on Od.Item_finished_id=vf.item_finished_id AND VF.PoufTypeCategory = 1 
            JOIN V_JOBASSIGNSQTY VJ(Nolock) on OD.orderid=VJ.orderid and OD.item_finished_id=VJ.Item_finished_id
            JOIN Unit U(Nolock) on OD.OrderUnitId=U.UnitId 
            CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id, " + DDProcessName.SelectedValue + "," + ddunit.SelectedValue + "," + hnordercaltype.Value + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE
            Where Om.orderid=" + DDorderNo.SelectedValue + " and " + Qtyrequired + ">0  order by OD.orderdetailid";
            }
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }

    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        str = @"select top(1) OrderUnitId From OrderDetail Where OrderId=" + DDorderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["orderunitid"].ToString()) != null)
            {
                ddunit.SelectedValue = ds.Tables[0].Rows[0]["orderunitid"].ToString();
            }
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string StrDetail = "";

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

                decimal Rate = 0, CommRate = 0;
                if (txtrate.Text != "")
                {
                    Rate = Convert.ToDecimal(txtrate.Text);
                }

                CommRate = 0;
                if (txtcommrate.Text != "")
                {
                    CommRate = Convert.ToDecimal(txtcommrate.Text);
                }

                if (StrDetail == "")
                {
                    StrDetail = lblorderid.Text + "|" + lblOrderdetailid.Text + "|" + lblitemfinishedid.Text + "|" + txtloomqty.Text + "|" + Rate + "|" + CommRate + "~";
                }
                else
                {
                    StrDetail = StrDetail + lblorderid.Text + "|" + lblOrderdetailid.Text + "|" + lblitemfinishedid.Text + "|" + txtloomqty.Text + "|" + Rate + "|" + CommRate + "~";
                }
            }
        }
        if (StrDetail != "")
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
                //Check Employee Entry
                if (StrEmpid == "")
                {
                    lblmessage.Text = "Plz Enter Weaver ID No...";
                    return;
                }
                //******
                SqlCommand cmd = new SqlCommand("PRO_SAVE_FIRST_PROCESS_ORDER", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@issueorderid", SqlDbType.Int);
                cmd.Parameters["@issueorderid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@issueorderid"].Value = hnissueorderid.Value;
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
                cmd.Parameters.AddWithValue("@Empid", StrEmpid);
                cmd.Parameters.Add("@FolioNo", SqlDbType.VarChar, 100);
                cmd.Parameters["@FolioNo"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Issuedate", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@Targetdate", txttargetdate.Text);
                cmd.Parameters.AddWithValue("@UnitID", ddunit.SelectedValue);
                cmd.Parameters.AddWithValue("@CalType", DDcaltype.SelectedValue);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@DetailData", StrDetail);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Remarks", TxtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@Instruction", TxtInstructions.Text.Trim());
                cmd.Parameters.AddWithValue("@TStockNo", Tdstockno.Visible == true ? txtstockno.Text : "");

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
                    txtfoliono.Text = cmd.Parameters["@FolioNo"].Value.ToString(); //param[5].Value.ToString();
                    hnissueorderid.Value = cmd.Parameters["@issueorderid"].Value.ToString();// param[0].Value.ToString();
                    FillGrid();
                    Refreshcontrol();
                    disablecontrols();
                }
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
        DDcustcode.SelectedIndex = -1;
        DDorderNo.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillGrid()
    {
        TxtRemarks.Text = "";
        TxtInstructions.Text = "";
        string str = @"Select Issue_Detail_Id,PM.issueorderid,ICM.Category_Name+' '+IM.Item_Name+' '+IPM.QDCS + Space(2) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End ItemDescription,Length,Width,
                Length + 'x' + Width Size,ROund(Area*Qty,4) as Area,Rate,Comm,Qty,Amount,REPLACE(CONVERT(nvarchar(11),PM.AssignDate,106),' ','-') as AssignDate,REPLACE(CONVERT(nvarchar(11),PD.ReqbyDate,106),' ','-') as ReqbyDate,PM.Unitid,isnull(PM.Purchaseflag,0) as Purchaseflag,PM.Caltype ,
                isnull(PM.Remarks,'') as Remarks,isnull(Pm.instruction,'') as Instruction,pm.FlagFixOrWeight,isnull(PM.ChallanNo,'') as ChallanNo, 
                IsNull(PM.FlagStockNoAttachWithoutRawMaterialIssue, 0) FlagStockNoAttachWithoutRawMaterialIssue 
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
                ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
        //Employeedetail
        str = str + @" select Distinct Ei.Empid,EI.EmpCode+'-'+EI.EmpName as Empname,activestatus from Employee_ProcessOrderNo EMP 
                inner Join EmpInfo EI on EMP.Empid=EI.EmpId 
                Where Emp.ProcessId=" + DDProcessName.SelectedValue + " and Emp.IssueOrderId=" + hnissueorderid.Value;

        str = str + @" Select Issue_Detail_Id,PM.issueorderid, VF.Category_Name + '  ' + VF.Item_Name + '  ' + VF.QualityName + '  ' + 
                VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
                Case When PM.Unitid=1 Then VF.SizeMtr Else Case When PM.UnitID = 6 Then VF.Sizeinch Else VF.SizeFt End End ItemDescription, LS.StockNo, LS.TstockNo 
                From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM 
                JOIN PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PD.IssueOrderID = PM.IssueOrderID 
                JOIN LoomStockNo LS ON LS.IssueOrderID = PD.IssueOrderID And LS.IssueDetailID = PD.Issue_Detail_ID And LS.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN V_FinishedItemDetail VF ON VF.Item_Finished_ID = PD.Item_Finished_ID 
                Where PM.IssueOrderid = " + hnissueorderid.Value + " And VF.MasterCompanyId = " + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();

        if (chkEdit.Checked == true && (Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "28") && Convert.ToInt32(Session["usertype"]) > 2)
        {
            DGOrderdetail.Columns[5].Visible = false;
            DGOrderdetail.Columns[6].Visible = false;
            DGOrderdetail.Columns[7].Visible = false;
            DGOrderdetail.Columns[8].Visible = false;
            DGOrderdetail.Columns[9].Visible = false;
            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;
        }
        DGOrderdetailStockNo.DataSource = ds.Tables[2];
        DGOrderdetailStockNo.DataBind();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        array[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrder", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["VarCompanyId"].ToString())
            {

                case "16":
                case "28":
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoWithoutRate_barcode.rpt";
                    break;
                case "44":
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockwithrateagni.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampoWithoutRate.rpt";
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

    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();
        FillFolioNo();
        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;

            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;

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
            TDEMPEDIT.Visible = true;
            TDFolioNo.Visible = true;
            TDFolioNotext.Visible = true;
            hnissueorderid.Value = "0";
            ddunit.Enabled = false;
        }
        else
        {
            TDEMPEDIT.Visible = false;
            btnsave.Visible = true;
            TDFolioNotext.Visible = false;
            TDFolioNo.Visible = false;
            hnissueorderid.Value = "0";
        }
        DDFolioNo.Items.Clear();
        listWeaverName.Items.Clear();
        txtfoliono.Text = "";
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        DG.DataSource = null;
        DG.DataBind();
    }

    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDFolioNo.SelectedValue;
        FolioSelectedChanged();
        FillGrid();
    }
    protected void FolioSelectedChanged()
    {
        string str = @"Select PM.issueorderid,REPLACE(CONVERT(nvarchar(11),PM.AssignDate,106),' ','-') AssignDate,
        REPLACE(CONVERT(nvarchar(11),PD.ReqbyDate,106),' ','-') ReqbyDate,PM.Unitid,isnull(PM.Purchaseflag,0) Purchaseflag, PM.Caltype, 
        isnull(PM.ChallanNo,'') ChallanNo, isnull(PM.Remarks,'') Remarks, isnull(Pm.instruction,'') Instruction  
        From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD 
        Where PM.IssueOrderid=PD.IssueOrderid And PM.IssueOrderid=" + hnissueorderid.Value;

        str = str + @" select Distinct Ei.Empid,EI.EmpCode+'-'+EI.EmpName as Empname,activestatus from Employee_ProcessOrderNo EMP 
                inner Join EmpInfo EI on EMP.Empid=EI.EmpId 
                Where Emp.ProcessId=" + DDProcessName.SelectedValue + " and Emp.IssueOrderId=" + hnissueorderid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (chkEdit.Checked == true)
        {
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
                hnordercaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                TxtInstructions.Text = ds.Tables[0].Rows[0]["instruction"].ToString();
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
                DGOrderdetail.Columns[10].Visible = false;
                DGOrderdetail.Columns[11].Visible = false;
            }
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
            TextBox txtrategrid = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtrategrid");
            TextBox txtcommrategrid = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtcommrategrid");

            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@issuedetailid", lblissuedetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            param[5] = new SqlParameter("@rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);
            param[6] = new SqlParameter("@commrate", txtcommrategrid.Text == "" ? "0" : txtcommrategrid.Text);
            param[7] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATE_FIRST_PROCESS_ORDER", param);

            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            FillGrid();
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
            //param[2] = new SqlParameter("@ChkForDyeingConsumption", ChkForDyeingConsumption.Checked == true ? "1" : "0");
            param[3] = new SqlParameter("@IssueDate", txtissuedate.Text);
            param[4] = new SqlParameter("@MasterCompanyId", Session["varcompanyNo"]);

            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatecurrentconsmpLoomWise", param);
            //******
            lblmessage.Text = param[1].Value.ToString();
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
        DDFolioNo.SelectedIndex = -1;
        hnissueorderid.Value = "0";
        enablecontrols();
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
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
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            param[5] = new SqlParameter("@StockNo", 0);
            param[6] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_FIRST_PROCESS_ORDER", param);
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
    protected void lnkStockNodelClick(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        if (DGOrderdetailStockNo.Rows.Count > 0)
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
                param[6] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

                //********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_FIRST_PROCESS_ORDER", param);
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
                //param[2] = new SqlParameter("@Produnitid", DDProdunit.SelectedValue);
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
                    DG.DataSource = ds.Tables[0];
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

    protected void btnweaveridscan_Click(object sender, EventArgs e)
    {
        //*********Check Folio Pending
        switch (Session["varcompanyid"].ToString())
        {
            case "16":
            case "28":
                string str = @"SELECT DISTINCT EMP.EMPID, PIM.ISSUEORDERID 
                        FROM PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(Nolock)
                        JOIN EMPLOYEE_PROCESSORDERNO EMP(Nolock) ON PIM.ISSUEORDERID=EMP.ISSUEORDERID AND EMP.PROCESSID = " + DDProcessName.SelectedValue + @" 
                        JOIN EMPINFO EI(Nolock) ON EMP.EMPID=EI.EMPID And EI.Empcode = '" + txtWeaverIdNoscan.Text + @"' 
                        WHERE PIM.STATUS <> 'CANCELED'And EMP.ACTIVESTATUS = 1 AND PIM.STATUS = 'PENDING' Order BY PIM.IssueOrderID Desc 
                        Select UserType From NewUserDetail(Nolock) Where UserID = " + Session["varuserid"];

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
            //param[5] = new SqlParameter("@TanaLotNo", txtTanaLotNo.Text);
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
                    default:
                        TextBox txtloomqty1 = (TextBox)e.Row.FindControl("txtloomqty");
                        txtloomqty1.Text = "";
                        break;
                }

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
    protected void BtnOrderProcessToPNM_Click(object sender, EventArgs e)
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
            param[0] = new SqlParameter("@ISSUEORDERID", hnissueorderid.Value);
            param[1] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MASTERCOMPANYID", 28);
            param[3] = new SqlParameter("@USERID", 1);

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
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnordercaltype.Value = DDcaltype.SelectedValue;
    }
}