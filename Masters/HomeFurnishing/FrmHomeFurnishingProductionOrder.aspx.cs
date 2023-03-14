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

public partial class Masters_HomeFurnishing_FrmHomeFurnishingProductionOrder : System.Web.UI.Page
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
            hnEmpWagescalculation.Value = "";
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock)
                        JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                        Select Distinct CI.CustomerId, CI.CustomerCode 
                        From OrderMaster OM(Nolock)
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                        JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.PoufTypeCategory = 1 
                        Where OM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                        Order By CI.CustomerCode 
                        Select UnitsId, UnitName From Units(Nolock) Order By UnitName 
                        Select UnitId, UnitName From Unit(Nolock) Where Unitid in(1, 2, 6)
                        Select PROCESS_NAME_ID, PROCESS_NAME From Process_Name_Master(Nolock) Where Process_Name_ID = 1";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 4, false, "");

            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds, 3, true, "--Plz Select--");
            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            hnissueorderid.Value = "0";
            if (Session["varcompanyNo"].ToString() != "44")
            {
                if (ddunit.Items.FindByValue(variable.VarDefaultProductionunit) != null)
                {
                    ddunit.SelectedValue = variable.VarDefaultProductionunit;
                }
            }
            if (Session["varcompanyNo"].ToString() == "16")
            {
                BtnOrderProcessToPNM.Visible = true;
                BtnChampoPanipat.Visible = true;
                BtnPanipatPNM1.Visible = true;
                BtnPanipatPNM2.Visible = true;
                BtnChampoHome.Visible = true;
            }
            else if (Session["varcompanyNo"].ToString() == "44")
            {
            TDLoomNotextbox.Visible=false;
            ddunit.Enabled = false;
            Page.Title = "PRODUCTION ORDER";
            }
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtloomid.Text = "0";
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        listWeaverName.Items.Clear();
        hnissueorderid.Value = "0";
        enablecontrols();
        //*********************
        AutoCompleteExtenderloomno.ContextKey = "0#" + DDcompany.SelectedValue + "#" + DDProdunit.SelectedValue;
         if (Session["varcompanyNo"].ToString() == "44" )
                    {
                        BindFolioNo(sender,e);
                    }
                    else
                    {
                        if (TDLoomNoDropdown.Visible == true)
                        {
                            if (chkEdit.Checked == true)
                            {
                                string str = @"select Distinct PLM.UID,PLM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo,case when ISNUMERIC(loomno)=1 Then CONVERT(int,loomno) Else 9999999 End as Loom 
                            from HomeFurnishingOrderMaster PIM 
                            inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                            Left join ITEM_MASTER Im on PLm.Itemid=IM.ITEM_ID 
                            left join Employee_HomeFurnishingOrderMaster EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
                            Where Plm.CompanyId=" + DDcompany.SelectedValue + " and PLm.UnitId=" + DDProdunit.SelectedValue + "";

                                string str1 = @"select Top 1 PIM.IssueOrderID, PIM.LoomId 
                            from HomeFurnishingOrderMaster PIM 
                            inner join ProductionLoomMaster PLM on PIM.LoomId=PLM.UID
                            join Employee_HomeFurnishingOrderMaster EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
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
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.LoomNo+'/'+isnull(IM.ITEM_NAME,'') as LoomNo from ProductionLoomMaster PM 
                                            Left join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
                            }
                        }
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
                    From OrderMaster OM(Nolock) 
                    JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.PoufTypeCategory = 1 
                    Where OM.Status = 0 And OM.CompanyID = " + DDcompany.SelectedValue + " AND OM.CustomerId = " + DDcustcode.SelectedValue + @" 
                    Order By OM.CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
    }
    protected void FillissueDetails()
    {
        string str = "";

        if (DDorderNo.SelectedIndex > 0 && DDcaltype.SelectedValue != "")
        {
            string length = "", Width = "", height = "", Area = "";
            switch (DDcaltype.SelectedValue)
            {
                case "1": //Pcs Wise
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            length = "LengthMtr";
                            Width = "WidthMtr";
                            Area = "AreaMtr";
                            height = "HeightMtr";
                            break;
                        case "2":
                            length = "LengthFt";
                            Width = "WidthFt";
                            height = "HeightFt";
                            Area = "AreaFt";
                            break;
                        default:
                            length = "Lengthft";
                            Width = "Widthft";
                            height = "HeightFt";
                            Area = "AreaFt";
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
                                height = "HeightMtr";
                                Area = "Areamtr";
                            }
                            else
                            {
                                length = "PRODLENGTHMTR";
                                Width = "PRODWIDTHMTR";
                                Area = "PRODAREAMTR";
                            }
                            break;
                        case "2":
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                height = "HeightFt";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                        default:
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                height = "HeightFt";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                    }
                    break;
            }
                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        //                str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                        //                " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
                        //                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                        //                Case When " + ddunit.SelectedValue + @" = 1 Then vf.LWHMtr Else Case When " + ddunit.SelectedValue + @" = 6 Then VF.LWHInch Else VF.LWHFt End End ItemDescription, 
                        //                '" + ddunit.SelectedItem.Text + @"' UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                        //                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, " + length + " [Length], " + Width + " [Width]," + height + " [height],  " + Area + @" Area, VF.ShapeID, JOBRATE.COMMRATE 
                        //                From OrderMaster OM(Nolock) 
                        //                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                        //                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                        //                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        //                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                        //                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                        //                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                        //                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                        //                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(ODD.OrderDetailDetail_Item_Finished_Id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                        //                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";

                        str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                od.OrderUnitId OrderUnitId, OD.flagsize, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                Case When OD.OrderUnitId = 1 Then vf.LWHMtr Else Case When OD.OrderUnitId = 6 Then VF.LWHInch Else VF.LWHFt End End ItemDescription, 
                Case When OD.OrderUnitId = 1 Then 'Mtr'  When OD.OrderUnitId = 6 THEN 'INCH' ELSE 'Ft' END UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, Case When OD.OrderUnitId = 1 Then LengthMtr  When OD.OrderUnitId = 6 THEN LengthINCH ELSE LengthFt END   [Length], Case When OD.OrderUnitId = 1 Then WidthMtr  When OD.OrderUnitId = 6 THEN WidthINCH ELSE WidthFt END  [Width],Case When OD.OrderUnitId = 1 Then HeightMtr  When OD.OrderUnitId = 6 THEN HeightINCH ELSE HeightFt END [height],   Case When OD.OrderUnitId = 1 Then AreaMtr  When OD.OrderUnitId = 6 THEN AreaInch ELSE AreaFt END  Area, VF.ShapeID, JOBRATE.COMMRATE 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(ODD.OrderDetailDetail_Item_Finished_Id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";
                    }
                    else
                    {
                        str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                Case When " + ddunit.SelectedValue + @" = 1 Then VF.Sizemtr Else Case When " + ddunit.SelectedValue + @" = 6 Then VF.SizeInch Else VF.sizeft End End ItemDescription, 
                '" + ddunit.SelectedItem.Text + @"' UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, " + length + " [Length], " + Width + " [Width]," + height + " [height],  " + Area + @" Area, VF.ShapeID, JOBRATE.COMMRATE 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";
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
                    DG.DataSource = ds.Tables[0];
                    DG.DataBind();
            }
        
    }
    protected void FillissueDetailscustom()
    {
        string str = "";

        if (DDorderNo.SelectedIndex > 0 && DDcaltype.SelectedValue != "")
        {
            string length = "", Width = "", height = "", Area = "";
            switch (DDcaltype.SelectedValue)
            {
                case "1": //Pcs Wise
                    switch (ddunit.SelectedValue)
                    {
                        case "1":
                            length = "LengthMtr";
                            Width = "WidthMtr";
                            Area = "AreaMtr";
                            height = "HeightMtr";
                            break;
                        case "6":
                            length = "LengthInch";
                            Width = "WidthInch";
                            height = "HeightInch";
                            Area = "AreaInch";
                            break;
                        default:
                            length = "Lengthft";
                            Width = "Widthft";
                            height = "HeightFt";
                            Area = "AreaFt";
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
                                height = "HeightMtr";
                                Area = "Areamtr";
                            }
                            else
                            {
                                length = "PRODLENGTHMTR";
                                Width = "PRODWIDTHMTR";
                                Area = "PRODAREAMTR";
                            }
                            break;
                        case "2":
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                height = "HeightFt";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                        default:
                            if (chkexportsize.Checked == true)
                            {
                                length = "Lengthft";
                                Width = "Widthft";
                                height = "HeightFt";
                                Area = "Actualfullareasqyd";
                            }
                            else
                            {
                                length = "PRODLENGTHFT";
                                Width = "PRODWIDTHFT";
                                Area = "PRODAREAFT";
                            }
                            break;
                    }
                    break;
            }


                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                Case When " + ddunit.SelectedValue + @" = 1 Then vf.LWHMtr Else Case When " + ddunit.SelectedValue + @" = 6 Then VF.LWHInch Else VF.LWHFt End End ItemDescription, 
                '" + ddunit.SelectedItem.Text + @"' UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, " + length + " [Length], " + Width + " [Width]," + height + " [height],  " + Area + @" Area, VF.ShapeID, JOBRATE.COMMRATE 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(ODD.OrderDetailDetail_Item_Finished_Id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";

                        //                str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                        //                " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
                        //                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                        //                Case When OD.OrderUnitId = 1 Then vf.LWHMtr Else Case When OD.OrderUnitId = 6 Then VF.LWHInch Else VF.LWHFt End End ItemDescription, 
                        //                '" + ddunit.SelectedItem.Text + @"' UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                        //                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, Case When OD.OrderUnitId = 1 Then LengthMtr  When OD.OrderUnitId = 6 THEN LengthINCH ELSE LengthFt END   [Length], Case When OD.OrderUnitId = 1 Then WidthMtr  When OD.OrderUnitId = 6 THEN WidthINCH ELSE WidthFt END  [Width],Case When OD.OrderUnitId = 1 Then HeightMtr  When OD.OrderUnitId = 6 THEN HeightINCH ELSE HeightFt END [height],   Case When OD.OrderUnitId = 1 Then AreaMtr  When OD.OrderUnitId = 6 THEN AreaInch ELSE AreaFt END  Area, VF.ShapeID, JOBRATE.COMMRATE 
                        //                From OrderMaster OM(Nolock) 
                        //                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                        //                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                        //                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        //                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                        //                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                        //                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                        //                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                        //                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(ODD.OrderDetailDetail_Item_Finished_Id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                        //                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";
                    }
                    else
                    {
                        str = @"Select OM.OrderId, OD.OrderDetailId, OD.Item_Finished_Id, ODD.OrderDetailDetailID, ODD.OrderDetailDetail_Item_Finished_Id, 
                " + ddunit.SelectedValue + @" OrderUnitId, OD.flagsize, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                Case When " + ddunit.SelectedValue + @" = 1 Then VF.Sizemtr Else Case When " + ddunit.SelectedValue + @" = 6 Then VF.SizeInch Else VF.sizeft End End ItemDescription, 
             .   '" + ddunit.SelectedItem.Text + @"' UnitName, (J.INTERNALPRODASSIGNEDQTY + J.PreProdAssignedQty) QtyRequired, 
                IsNull(VHFOD.Qty, 0) OrderedQty, JOBRATE.RATE, " + length + " [Length], " + Width + " [Width]," + height + " [height],  " + Area + @" Area, VF.ShapeID, JOBRATE.COMMRATE 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OM.OrderId=OD.OrderId 
                JOIN ORDERDETAILDETAIL ODD(Nolock) ON ODD.OrderDetailID = OD.OrderDetailId And ODD.OrderID = OD.OrderID 
                LEFT JOIN V_HomeFurnishingOrderDetail VHFOD(Nolock) ON VHFOD.OrderID = ODD.OrderID And VHFOD.Order_FinishedID = OD.ITEM_FINISHED_ID And 
                        VHFOD.OrderDetailDetail_FinishedID = ODD.OrderDetailDetail_Item_Finished_Id And VHFOD.ProcessID = " + DDProcessName.SelectedValue + @" 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                JOIN JobAssigns J(Nolock) ON J.OrderID = OD.OrderID And J.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN Unit U(nolock) ON U.UnitId = OD.OrderUnitId 
                CROSS APPLY(SELECT * FROM DBO.F_GETJOBRATE_COMM(OD.item_finished_id, " + DDProcessName.SelectedValue + "," + DDProdunit.SelectedValue + @"," + DDcaltype.SelectedValue + @"," + hnEmployeeType.Value + @"," + hnEmpId + @",OM.OrderCategoryId)) JOBRATE 
                Where Om.orderid = " + DDorderNo.SelectedValue + " Order By OD.OrderDetailID";
                    }

                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    DG.DataSource = ds.Tables[0];
                    DG.DataBind();
            }
        
    }

    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        if (Session["varcompanyNo"].ToString() != "44")
        {
            if (txtloomid.Text == "" || txtloomid.Text == "0")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "loomid", "alert('Please select Loom No.');", true);
                return;
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string strOrderDetail = "";

        int CancelQty = 0;
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
                TextBox txtheight = ((TextBox)DG.Rows[i].FindControl("txtheight"));
                Label lblarea = ((Label)DG.Rows[i].FindControl("lblarea"));
                TextBox txtcommrate = ((TextBox)DG.Rows[i].FindControl("txtcommrate"));
                Label LblOrderDetailDetailID = ((Label)DG.Rows[i].FindControl("LblOrderDetailDetailID"));
                Label LblOrderDetailDetail_Item_Finished_Id = ((Label)DG.Rows[i].FindControl("LblOrderDetailDetail_Item_Finished_Id"));
                if (strOrderDetail == "")
                {
                    strOrderDetail = lblorderid.Text + "|" + lblitemfinishedid.Text + "|" + LblOrderDetailDetail_Item_Finished_Id.Text + "|" + txtlength.Text + "|" +
                        txtwidth.Text + "|" + txtheight.Text + "|" + lblarea.Text + "|" + txtrate.Text + "|" + txtloomqty.Text + "|" + txtcommrate.Text + "|" + CancelQty + "~";
                }
                else
                {
                    strOrderDetail = strOrderDetail + lblorderid.Text + "|" + lblitemfinishedid.Text + "|" + LblOrderDetailDetail_Item_Finished_Id.Text + "|" + txtlength.Text + "|" +
                        txtwidth.Text + "|" + txtheight.Text + "|" + lblarea.Text + "|" + txtrate.Text + "|" + txtloomqty.Text + "|" + txtcommrate.Text + "|" + CancelQty + "~";
                }
            }
        }
        if (strOrderDetail != "")
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

                SqlCommand cmd = new SqlCommand("PRO_SAVEHOMEFURNISHINGPRODUCTIONORDER", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@ISSUEORDERID", SqlDbType.Int);
                cmd.Parameters["@ISSUEORDERID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@ISSUEORDERID"].Value = hnissueorderid.Value;
                cmd.Parameters.AddWithValue("@COMPANYID", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@PROCESSID", DDProcessName.SelectedValue);
                cmd.Parameters.AddWithValue("@ISSUEDATE", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@TARGETDATE", txttargetdate.Text);
                cmd.Parameters.AddWithValue("@PRODUCTIONUNIT", DDProdunit.SelectedValue);
                cmd.Parameters.AddWithValue("@UNITID", ddunit.SelectedValue);
                cmd.Parameters.AddWithValue("@LOOMID", txtloomid.Text);
                cmd.Parameters.AddWithValue("@CALTYPE", DDcaltype.SelectedValue);
                cmd.Parameters.AddWithValue("@USERID", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MASTERCOMPANYID", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@REMARKS", TxtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@INSTRUCTION", TxtInstructions.Text.Trim());
                cmd.Parameters.AddWithValue("@FLAGFIXORWEIGHT", 1);
                cmd.Parameters.AddWithValue("@EXPORTSIZEFLAG", chkexportsize.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@EWAYBILLNO", chkexportsize.Checked == true ? "1" : "0");
                cmd.Parameters.Add("@FOLIONO", SqlDbType.VarChar, 100);
                cmd.Parameters["@FOLIONO"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@MSG", SqlDbType.VarChar, 100);
                cmd.Parameters["@MSG"].Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("@EMPIDS", StrEmpid);
                cmd.Parameters.AddWithValue("@OrderDetail", strOrderDetail);

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
                    DDcaltype.Enabled = false;
                    txtfoliono.Text = cmd.Parameters["@ISSUEORDERID"].Value.ToString(); //param[5].Value.ToString();
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
        DDorderNo.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillGrid()
    {
        TxtRemarks.Text = "";
        TxtInstructions.Text = "";
         string str=string.Empty;

         if (Session["varcompanyNo"].ToString() == "44")
         {
             str = @"Select a.IssueOrderId, b.IssueDetailId, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                                Case When a.UNITID = 1 Then VF.LWHMtr Else Case When a.UNITID = 6 Then VF.LWHInch Else VF.LWHFt End End ItemDescription, 
                b.Width, b.Length,b.height, b.Qty, b.Rate, b.Comm, b.Area, b.Amount, REPLACE(CONVERT(NVARCHAR(11), a.assigndate, 106), ' ', '-') assigndate, 
                REPLACE(CONVERT(NVARCHAR(11), b.Reqbydate, 106), ' ', '-') Reqbydate, a.ChallanNo, a.unitid, a.caltype, a.Remarks, a.instruction 
                From HomeFurnishingOrderMaster a(Nolock) 
                JOIN HomeFurnishingOrderDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.OrderDetailDetail_FinishedID 
                Where a.ISSUEORDERID = " + hnissueorderid.Value + " And a.MasterCompanyId = " + Session["varCompanyId"] + " Order By b.IssueDetailId Desc";
         }
         else
         {
             str = @"Select a.IssueOrderId, b.IssueDetailId, 
                VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                                Case When a.UNITID = 1 Then VF.SizeMtr Else Case When a.UNITID = 6 Then VF.SizeInch Else VF.SizeFt End End ItemDescription, 
                b.Width, b.Length,b.height, b.Qty, b.Rate, b.Comm, b.Area, b.Amount, REPLACE(CONVERT(NVARCHAR(11), a.assigndate, 106), ' ', '-') assigndate, 
                REPLACE(CONVERT(NVARCHAR(11), b.Reqbydate, 106), ' ', '-') Reqbydate, a.ChallanNo, a.unitid, a.caltype, a.Remarks, a.instruction 
                From HomeFurnishingOrderMaster a(Nolock) 
                JOIN HomeFurnishingOrderDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.OrderDetailDetail_FinishedID 
                Where a.ISSUEORDERID = " + hnissueorderid.Value + " And a.MasterCompanyId = " + Session["varCompanyId"] + " Order By b.IssueDetailId Desc";
         }
        //Employeedetail
        str = str + @" Select Distinct EI.Empid, EI.EmpCode + '-' + EI.EmpName EmpName, activestatus 
                    From Employee_HomeFurnishingOrderMaster EMP(Nolock)
                    Join EmpInfo EI(Nolock) ON EI.EmpId = EMP.Empid 
                    Where Emp.ProcessId = " + DDProcessName.SelectedValue + " And Emp.IssueOrderId = " + hnissueorderid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();
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
                DDcaltype.SelectedValue = ds.Tables[0].Rows[0]["caltype"].ToString();
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
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        array[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        array[3] = new SqlParameter("@ReportType", 1);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrder", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyid"].ToString())
            {               
                case "44":
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockagni.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampo.rpt";
                    break;
            }

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionOrderLoomWise.xsd";

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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;
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
            if((Session["varcompanyid"].ToString()=="44"))
            {
            TDLoomNoDropdown.Visible = false;
            }
            else
            {
             TDLoomNoDropdown.Visible = true;
            }
            TDLoomNotextbox.Visible = false;
            //TDactiveemployee.Visible = true;
            ddunit.Enabled = false;
            BtnUpdateConsumption.Visible = true;
        }
        else
        {
            TDEMPEDIT.Visible = false;
            btnsave.Visible = true;
            TDFolioNotext.Visible = false;
            TDFolioNo.Visible = false;
            hnissueorderid.Value = "0";
            TDLoomNoDropdown.Visible = false;
            TDLoomNotextbox.Visible = true;
            TDactiveemployee.Visible = false;
            BtnUpdateConsumption.Visible = false
                ;
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
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        txtloomid.Text = DDLoomNo.SelectedValue;
        if (chkEdit.Checked == true)
        {
            str = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo from HomeFurnishingOrderMaster PIM
                   LEFT JOIN Employee_HomeFurnishingOrderMaster EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
                   
                   Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + " and PIM.LoomId=" + DDLoomNo.SelectedValue;
            if (chkcomplete.Checked == true)
            {
                str = str + " and PIM.Status='Complete'";
            }
            else
            {
                str = str + " and PIM.Status='Pending'";
            }
            if (txteditempid.Text != "")
            {
                str = str + " and EMP.EMPID=" + txteditempid.Text + "";
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

    protected void BindFolioNo(object sender, EventArgs e)
    {
        string str;
        txtloomid.Text = DDLoomNo.SelectedValue;
        if (chkEdit.Checked == true)
        {
            str = @"select Distinct PIM.IssueOrderId,PIM.ChallanNo from HomeFurnishingOrderMaster PIM
                   LEFT JOIN Employee_HomeFurnishingOrderMaster EMP on PIM.IssueOrderId=EMP.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
                   
                   Where PIM.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue;
            if (chkcomplete.Checked == true)
            {
                str = str + " and PIM.Status='Complete'";
            }
            else
            {
                str = str + " and PIM.Status='Pending'";
            }
            if (txteditempid.Text != "")
            {
                str = str + " and EMP.EMPID=" + txteditempid.Text + "";
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
        str = @"select OM.CustomerOrderNo,CI.CustomerCode from HomeFurnishingOrderDetail PID JOIN OrderMaster OM ON PID.OrderId=OM.OrderId
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

        if (Session["VarCompanyId"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
        {
            ShowCustomerCodeAndOrderNo();
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
                str = @"select EMp.Empid,Pm.IssueOrderId  from dbo.HomeFurnishingOrderMaster PM inner join dbo.HomeFurnishingOrderDetail PD
                        on PM.IssueOrderId=Pd.IssueOrderId  and Pd.PQty>0
                        inner join  dbo.Employee_HomeFurnishingOrderMaster EMP on EMP.IssueOrderId=PM.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
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
        string str = @"select Distinct U.UnitsId,U.UnitName,PIm.CompanyId 
                    From HomeFurnishingOrderMaster PIM 
                    inner Join  Units U on PIM.Units=U.UnitsId
                    inner join Employee_HomeFurnishingOrderMaster EMP on PIM.Issueorderid = EMP.IssueOrderId And EMP.ProcessId = " + DDProcessName.SelectedValue + @"
                    Where PIM.Companyid=" + DDcompany.SelectedValue;

        string str1 = @" Select Top 1 PIM.CompanyID, PIM.Units, PIM.IssueOrderID 
                From HomeFurnishingOrderMaster PIM 
                inner Join  Units U on PIM.Units=U.UnitsId
                inner join Employee_HomeFurnishingOrderMaster EMP on PIM.Issueorderid = EMP.IssueOrderId and EMP.ProcessId = " + DDProcessName.SelectedValue + @"
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

                            if (Session["VarCompanyNo"].ToString() == "27" && hnEmployeeType.Value == "1")
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
        finally
        {
            con.Dispose();
            con.Close();
        }
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
        string str = @"select Distinct EI.EmpName+'('+EI.EmpCode+')' as Employee,EMP.IssueOrderId,Emp.ActiveStatus,Ei.Empid 
                From Employee_HomeFurnishingOrderMaster EMP inner Join EmpInfo EI on Emp.Empid=Ei.EmpId
                   and EMP.ProcessId = " + DDProcessName.SelectedValue + " and EMP.IssueOrderId=" + DDFolioNo.SelectedValue;
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
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            param[5] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEHOMEFURNISHINGORDER", param);
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
    //protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    //{
    //    FillWeaverWithBarcodescan();
    //    //txtWeaverIdNoscan.Focus();
    //    //Page.SetFocus("CPH_Form_txtWeaverIdNoscan");
    //}
    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillissueDetailscustom();
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
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(ddunit.SelectedValue) == 2 || Convert.ToInt16(ddunit.SelectedValue) == 6)
            {
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(ddunit.SelectedValue)));
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
                param[3] = new SqlParameter("@ordercaltype", DDcaltype.SelectedValue);
                //param[4] = new SqlParameter("@Tstockno", txtstockno.Text);
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
                }
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
            //param[0] = new SqlParameter("@TStockNo", txtstockno.Text);
            param[1] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATESTOCKNOINPROCESSDETAIL", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
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
                if (DDcaltype.SelectedValue == "1" && variable.VarGENERATESTOCKNOONTAGGING == "1")
                {
                    txtqty.Enabled = false;
                }
            }
            if (Convert.ToInt16(Session["usertype"]) > 2)
            {
                DGOrderdetail.Columns[10].Visible = false;
                DGOrderdetail.Columns[11].Visible = false;
            }
            if (Convert.ToString(Session["varcompanyid"]) == "44")
            {
                DGOrderdetail.Columns[3].Visible = true;
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
                string str = @"SELECT DISTINCT PIM.ISSUEORDERID, EMP.EMPID 
                    FROM HomeFurnishingOrderMaster PIM(Nolock) 
                    INNER JOIN Employee_HomeFurnishingOrderMaster EMP(Nolock) ON PIM.ISSUEORDERID=EMP.ISSUEORDERID AND EMP.PROCESSID = " + DDProcessName.SelectedValue + @" 
                    INNER JOIN EMPINFO EI(Nolock) ON EMP.EMPID=EI.EMPID 
                    WHERE PIM.STATUS = 'PENDING' AND EMP.ACTIVESTATUS = 1 AND EI.EMPCODE = '" + txtWeaverIdNoscan.Text + @"' 
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
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            param[4] = new SqlParameter("@rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);
            param[5] = new SqlParameter("@commrate", txtcommrategrid.Text == "" ? "0" : txtcommrategrid.Text);
            param[6] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATE_HOMEFURNISHINGORDER", param);

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
    protected void BtnUpdateConsumption_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEHOMEFURNISHINGPRODUCTIONORDERCONSUMPTION", param);
            if (param[2].Value.ToString() != "")
            {
                lblmessage.Text = param[2].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Consumption successfully updated')", true);
                Tran.Commit();
            }
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
            param[4] = new SqlParameter("@POUFTYPECATEGORY", 1);
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
}
