using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;
public partial class Masters_Purchase_PurchaseIndent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            #region Author:Rajeev, Date:09-12-12,...
            ViewState["PIndentId"] = "0";
            ViewState["PIndentDetailId"] = "0";
            string Qry = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                        Select DepartmentId,DepartmentName from Department order by DepartmentName
                        select distinct ci.customerid,ci.Customercode from customerinfo ci inner join OrderMaster om on om.customerid=ci.customerid Order BY Ci.Customercode
                        Select Distinct OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Order By LocalOrder+ ' / ' +CustomerOrderNo ";
            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            CommanFunction.FillComboWithDS(DDCompanyName, DSQ, 0);

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedIndexChanged();
            }

            
            ParameteLabel();
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, DSQ, 1, true, "--Select Department--");
            if (Session["varcompanyno"].ToString() != "12")
            {
                UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, DSQ, 2, true, "Select CustomerCode");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddcustomer, "select distinct ci.customerid,ci.CompanyName+' '+ci.Customercode from customerinfo ci inner join OrderMaster om on om.customerid=ci.customerid ", true, "Select Buyer Name");
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddorder, DSQ, 3, true, "-Select Order");

            //            CommanFunction.FillCombo(DDCompanyName, "Select CompanyId,CompanyName from Companyinfo order by CompanyName");
            //            ParameteLabel();
            //            UtilityModule.ConditionalComboFill(ref DDDepartment, "Select DepartmentId,DepartmentName from Department order by DepartmentName", true, "--Select Department--");
            //            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select distinct ci.customerid,ci.Customercode from customerinfo ci 
            //                inner join OrderMaster om on om.customerid=ci.customerid Order BY ci.Customercode ", true, "Select CustomerCode");
            //            UtilityModule.ConditionalComboFill(ref ddorder, "Select Distinct OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Order By LocalOrder+ ' / ' +CustomerOrderNo ", true, "-Select Order");
            #endregion
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtreqdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["ReportPath"] = "Reports/PGenrateIndent.rpt";
            Session["CommanFormula"] = "";
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            // Size Type
            UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
            //
            switch (VarProdCode)
            {
                case 0:
                    itmcod.Visible = false;
                    break;
                case 1:
                    itmcod.Visible = true;
                    break;
            }
            switch (Session["varcompanyId"].ToString())
            {
                case "12":
                    lblvend.Text = "Supplier Name";
                    lblcustcode.Text = "Buyer Name";
                    tdemp.Visible = false;
                    break;
                case "6":
                    ChKForOrder.Checked = true;
                    ChKForOrder.Enabled = false;
                    Chkfororder_check();
                    break;
            }
        }
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        //DDCompanyName.SelectedValue = Session["varCompanyId"].ToString();
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorychange1();
        //ddlcategorycange();
        //UtilityModule.ConditionalComboFill(ref DDItem, "select Item_id,Item_Name from Item_Master where Category_Id="+DDCategory.SelectedValue,true,"--Select Item--");
    }
    private void ddlcategorychange1()
    {
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref DDItem, "select Item_id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " Order By Item_Name ", true, "--Select Item--");
    }
    private void ddlcategorycange()
    {
        TdQuality.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                        FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                        IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TdQuality.Visible = true;
                        break;
                    case "2":
                        TdDesign.Visible = true;
                        break;
                    case "3":
                        TdColor.Visible = true;
                        break;
                    case "6":
                        TdColorShade.Visible = true;
                        break;
                    case "4":
                        TdShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        departmentchange();
        //if (ChkEditOrder.Checked == true)
        //{
        //    UtilityModule.ConditionalComboFill(ref DDPartyName, "select EmpId,EmpName from EmpInfo e,PurchaseIndentMaster p where p.partyid=e.empid and p.flagapproval<>1 ", true, "--Select Employee--");
        //    UtilityModule.ConditionalComboFill(ref DDEmp, "select EmpId,EmpName from EmpInfo e,PurchaseIndentMaster p Where p.empid=e.empid and p.flagapproval<>1 departmentid=" + DDDepartment.SelectedValue + " and ", true, "--Select Employee--");
        //}
        //else
        //{
        //    UtilityModule.ConditionalComboFill(ref DDPartyName, "select EmpId,EmpName from EmpInfo ", true, "--Select Employee--");
        //    UtilityModule.ConditionalComboFill(ref DDEmp, "select EmpId,EmpName from EmpInfo Where departmentid=" + DDDepartment.SelectedValue + "", true, "--Select Employee--");
        //}
    }
    private void departmentchange()
    {
        DataSet DSQ = null;
        string Qry = "";

        if (ChkEditOrder.Checked == true)
        {
            #region Author:Rajeev, Date:09-12-12..
            Qry = @"select Distinct e.EmpId,e.EmpName from EmpInfo e,PurchaseIndentMaster p where p.partyid=e.empid and p.flagapproval<>1 And p.PindentId=" + ddindentno.SelectedValue + @" and blacklist=0 Order By e.EmpName 
                    select Distinct e.EmpId,e.EmpName from EmpInfo e,PurchaseIndentMaster p Where p.empid=e.empid and p.flagapproval<>1 and p.departmentid=" + DDDepartment.SelectedValue + " And  p.PindentId=" + ddindentno.SelectedValue + " and blacklist=0 Order By e.EmpName  ";
        }
        else
        {
            switch (variable.Carpetcompany)
            {
                case "1":
                    if (variable.VarPURCHASEORDER_INDENTOTHERVENDOR == "1")
                    {
                        Qry = @"select Distinct EmpId,EmpName from EmpInfo where departmentid=" + DDDepartment.SelectedValue + @" and blacklist=0 and Partytype=1 Order By EmpName
                            select Distinct EmpId,EmpName from EmpInfo Where departmentid=" + DDDepartment.SelectedValue + " and blacklist=0 and Partytype=1 Order By EmpName ";
                    }
                    else
                    {
                        Qry = @"select Distinct EmpId,EmpName from EmpInfo where departmentid=" + DDDepartment.SelectedValue + @" and blacklist=0 Order By EmpName
                            select Distinct EmpId,EmpName from EmpInfo Where departmentid=" + DDDepartment.SelectedValue + " and blacklist=0 Order By EmpName ";
                    }
                    break;
                default:
                    Qry = @"select Distinct EmpId,EmpName from EmpInfo where blacklist=0 Order By EmpName
                    select Distinct EmpId,EmpName from EmpInfo Where departmentid=" + DDDepartment.SelectedValue + " and blacklist=0 Order By EmpName ";
                    break;
            }
            
        }
        DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref DDPartyName, DSQ, 0, true, "--Select Employee--");
        UtilityModule.ConditionalComboFillWithDS(ref DDEmp, DSQ, 1, true, "--Select Employee--");
            #endregion
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarPURCHASEORDER_INDENTOTHERVENDOR=="1")
        {
            DDEmp.SelectedValue = DDPartyName.SelectedValue;
        }
        partychange();
        //UtilityModule.ConditionalComboFill(ref DDCategory, "Select Category_Id,Category_NAme from Item_Category_Master ICM,CategorySeparate CS where CS.Id=1 and CS.CategoryId=ICM.Category_Id", true, "--Select Category--");
    }
    private void partychange()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "Select Distinct Category_Id,Category_Name from Item_Category_Master ICM,CategorySeparate CS where CS.Id=1 and CS.CategoryId=ICM.Category_Id Order By Category_Name", true, "--Select Category--");
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        itemchange();
        //UtilityModule.ConditionalComboFill(ref DDQuality,"Select QualityId,QualityName from Quality where Item_Id="+DDItem.SelectedValue,true,"--select--");
        //UtilityModule.ConditionalComboFill(ref DDDesign,"Select DesignId,DesignName from Design",true,"--select--");
        //UtilityModule.ConditionalComboFill(ref DDColor,"Select ColorId,ColorName from Color",true,"--select--");
        //UtilityModule.ConditionalComboFill(ref DDColorShade,"select ShadeColorId,ShadeColorName from ShadeColor ",true,"--select--");
        //UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape", true, "--select--");
        //CommanFunction.FillCombo(DDUnit, "select distinct UnitId,UnitName from Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId and IM.Item_Id=" + DDItem.SelectedValue);
    }
    private void itemchange()
    {
        #region Author:Rajeev, Date:09-12-12
        string Qry = @"Select QualityId,QualityName from Quality where Item_Id=" + DDItem.SelectedValue + @" Order BY QualityName 
                    Select DesignId,DesignName from Design  Order BY DesignName
                    Select ColorId,ColorName from Color Order By ColorName
                    select ShadeColorId,ShadeColorName from ShadeColor Order BY ShadeColorName 
                    select ShapeId,ShapeName from Shape Order BY ShapeName
                    select distinct UnitId,UnitName from Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId and IM.Item_Id=" + DDItem.SelectedValue + "Order BY UnitName ";
        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref DDQuality, DSQ, 0, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, DSQ, 1, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColor, DSQ, 2, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, DSQ, 3, true, "--select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDShape, DSQ, 4, true, "--select--");
        CommanFunction.FillComboWithDS(DDUnit, DSQ, 5);
        #endregion
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    private void FillSize()
    {
        if (DDsizetype.SelectedValue == "0")
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeFt from Size where ShapeId=" + DDShape.SelectedValue + " Order BY SizeFt ", true, "--select--");
        }
        else if (DDsizetype.SelectedValue == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr from Size where ShapeId=" + DDShape.SelectedValue + " Order BY SizeMtr ", true, "--select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeInch from Size where ShapeId=" + DDShape.SelectedValue + " Order BY SizeMtr ", true, "--select--");
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SaveIndent();
    }
    private void SaveIndent()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            int varFinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            TxtIndentNo.Text = "";

            if (Session["VarcOmpanyNo"].ToString() == "6")
            {
                // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PindentDetailId from PurchaseindentMaster PM,PurchaseindentDetail PD Where PM.PIndentId=PD.PindentId And OrderId=" + ddorder.SelectedValue + " And Finishedid=" + varFinishedid + " And PIndentDetailId<>" + ViewState["PIndentDetailId"] + "");
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select V.OrderId from PurchaseIndentMaster PM,PurchaseIndentDetail PD,V_ConsumptionQtyAndPurchaseIndentQtyForArtIndia V Where PM.PindentId=PD.Pindentid And V.OrderId=pM.OrderId And PD.FinishedId=V.Finishedid And V.OrderId=" + ddorder.SelectedValue + " And V.Finishedid=" + varFinishedid + " And PIndentDetailId<>" + ViewState["PIndentDetailId"] + " group by V.OrderId Having Sum(consumptionqty)<Sum(purchaseqty)");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Item Already issued for this Order');", true);
                    tran.Commit();
                    return;
                }
            }
            else
            {
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PindentDetailId from PurchaseindentDetail Where PIndentId=" + ViewState["PIndentId"] + " And Finishedid=" + varFinishedid + "  And PIndentDetailId<>" + ViewState["PIndentDetailId"] + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Dublicate Enrty');", true);
                    tran.Commit();
                    return;
                }
            }
            SqlParameter[] _arrpara = new SqlParameter[21];
            _arrpara[0] = new SqlParameter("@PIndentId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@DepartmentId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@PartyId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@PIndentNo", SqlDbType.NVarChar, 50);
            _arrpara[5] = new SqlParameter("@Date", SqlDbType.DateTime);
            _arrpara[6] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@PIndentDetailId", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@FinishedId", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Qty", SqlDbType.Float);
            _arrpara[11] = new SqlParameter("@UnitId", SqlDbType.Int);
            _arrpara[12] = new SqlParameter("@Remark", SqlDbType.NVarChar, 4000);
            _arrpara[13] = new SqlParameter("@empid", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@itemRemark", SqlDbType.VarChar, 8000);
            _arrpara[15] = new SqlParameter("@CustomerCode", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@flagsize", SqlDbType.Int);
            _arrpara[18] = new SqlParameter("@Reqdate", SqlDbType.DateTime);
            _arrpara[19] = new SqlParameter("@Rate", SqlDbType.Float);

            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["PIndentId"];
            _arrpara[1].Value = DDCompanyName.SelectedValue;
            _arrpara[2].Value = DDDepartment.SelectedValue;
            _arrpara[3].Value = DDPartyName.SelectedValue;
            _arrpara[4].Direction = ParameterDirection.InputOutput;
            _arrpara[4].Value = TxtIndentNo.Text.ToUpper();
            _arrpara[5].Value = TxtDate.Text;
            _arrpara[6].Value = Session["varuserid"];
            _arrpara[7].Value = Session["varCompanyId"];
            _arrpara[8].Direction = ParameterDirection.InputOutput;
            if (ChkEditOrder.Checked == true)
                _arrpara[8].Value = ViewState["PIndentDetailId"];
            else
                _arrpara[8].Value = ViewState["PIndentDetailId"];
            _arrpara[9].Value = varFinishedid;
            _arrpara[10].Value = TxtQty.Text;
            _arrpara[11].Value = DDUnit.SelectedValue;
            _arrpara[12].Value = txtremarks.Text;
            if (DDEmp.Visible == true)
            {
                _arrpara[13].Value = DDEmp.SelectedValue;
            }
            else
            {
                _arrpara[13].Value = 0;
            }
            _arrpara[14].Value = txtitemremark.Text;
            _arrpara[15].Value = ddcustomer.SelectedIndex != -1 ? Convert.ToInt32(ddcustomer.SelectedValue) : 0;
            if (ddorder.Items.Count > 0 && ddorder.SelectedIndex != -1)
            {
                _arrpara[16].Value = ddorder.SelectedIndex != -1 ? Convert.ToInt32(ddorder.SelectedValue) : 0;
            }
            else
            {
                _arrpara[16].Value = 0;
            }
            _arrpara[17].Value = TdSize.Visible == true ? DDsizetype.SelectedValue : "0";
            _arrpara[18].Value = txtreqdate.Text;
            _arrpara[19].Value = txtrate.Text == "" ? "0" : txtrate.Text;
            //_arrpara[16].Value = ddorder.SelectedIndex != -1 ? "0" : ddorder.SelectedValue;
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_PIndent", _arrpara);
            tran.Commit();
            ViewState["PIndentId"] = _arrpara[0].Value;
            ViewState["PIndentDetailId"] = 0;
            TxtIndentNo.Text = _arrpara[4].Value.ToString();
            Lblmessage.Visible = true;
            Lblmessage.Text = "Data Successfully Saved..";
            Session["CommanFormula"] = "{V_PurchaseIndentReport.PIndentNo}=" + TxtIndentNo.Text;
            fill_grid();
            DDItem.SelectedValue = null;
            DDQuality.SelectedValue = null;
            DDDesign.SelectedValue = null;
            DDColor.SelectedValue = null;
            DDShape.SelectedValue = null;
            DDColorShade.SelectedValue = null; ;
            DDSize.SelectedValue = null;
            TxtQty.Text = "";
            txtrate.Text = "";
            txtreqdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtremarks.Text = "";
            txtitemremark.Text = "";
            if (ChKForOrder.Checked == true)
            {
                fill_grid_show();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseIndent.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void fill_grid()
    {
        DGPIndentDetail.DataSource = Fill_Grid_Data();
        DGPIndentDetail.DataBind();
        Session["ReportPath"] = "Reports/PGenrateIndent.rpt";
        Session["CommanFormula"] = "{V_PurchaseIndentReport.PIndentNo}='" + TxtIndentNo.Text + "'";
    }
    private DataSet Fill_Grid_Data()
    {

        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select PIndentDetailId,pim.PIndentId,PIndentNo,D.DepartmentName,EmpName PartyName,Category_Name+'  '+Item_Name+'  '+ QualityName+'  '+isnull(DesignName,'')+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'  '+isnull(ShapeName,'')  ItemDescription,SizeMtr,SizeFt,Qty,UnitName,pim.remark as orderremark,pid.itemremark as remark,pid.finishedid,pid.Rate as rate,pid.flagsize
                            From PurchaseIndentMaster PIM inner Join PurchaseIndentDetail PID  on PIM.PIndentId=PID.PIndentId inner join 
                            V_Companyinfo VC on PIM.CompanyId=VC.CompanyId inner join V_Employeeinfo VE ON VE.EmpId=PIM.PartyId inner join
                            Department D on D.DepartmentId=PIM.DepartmentId inner join V_ItemDetail VI on VI.Item_Finished_Id=PID.FinishedId Left outer Join 
                            Unit on Unit.UnitId=PID.UnitId where PIM.PIndentId=" + ViewState["PIndentId"] + " Order by PIndentDetailId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseIndent.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (DDPreviewType.SelectedValue == "0")
        {
            Report();
        }
        else
        {
            Report1();
        }
    }
    private void Report()
    {
        string qry = @" SELECT V_PurchaseIndentReport.ItemDescription,V_PurchaseIndentReport.PIndentNo,V_PurchaseIndentReport.Date,V_PurchaseIndentReport.Qty,
        V_PurchaseIndentReport.UnitName,V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,
        V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,ci.customername,
        V_PurchaseIndentReport.orderid,ci.customercode,om.customerorderno,localorder,V_PurchaseIndentReport.iremark,V_PurchaseIndentReport.remark,
        V_PurchaseIndentReport.Rate,V_PurchaseIndentReport.Reqdate,V_PurchaseIndentReport.flagsize,V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN
        FROM   V_PurchaseIndentReport INNER JOIN 
        V_Companyinfo ON V_PurchaseIndentReport.CompanyId=V_Companyinfo.CompanyId INNER JOIN 
        V_EmployeeInfo ON V_PurchaseIndentReport.PartyID=V_EmployeeInfo.EmpId left outer join 
        Customerinfo ci On V_PurchaseIndentReport.customercode=ci.customerid left outer join 
        Ordermaster om on V_PurchaseIndentReport.orderid=om.orderid
        Where V_PurchaseIndentReport.pindentid=" + ViewState["PIndentId"] + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\PGenrateIndentNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void Report1()
    {
        string qry = @" SELECT V_PurchaseIndentReport.ItemDescription,V_PurchaseIndentReport.PIndentNo,V_PurchaseIndentReport.Date,V_PurchaseIndentReport.Qty,
        V_PurchaseIndentReport.UnitName,V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,
        V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,ci.customername,
        V_PurchaseIndentReport.orderid,ci.customercode,om.customerorderno,localorder,V_PurchaseIndentReport.iremark,V_PurchaseIndentReport.remark,
        V_PurchaseIndentReport.Rate,V_PurchaseIndentReport.Reqdate,V_PurchaseIndentReport.PIndentDetailId,Replace(Replace(V_PurchaseIndentReport.ImageName,'~\',''),'\','/') Photo,
        V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN
        FROM   V_PurchaseIndentReport INNER JOIN 
        V_Companyinfo ON V_PurchaseIndentReport.CompanyId=V_Companyinfo.CompanyId INNER JOIN 
        V_EmployeeInfo ON V_PurchaseIndentReport.PartyID=V_EmployeeInfo.EmpId left outer join 
        Customerinfo ci On V_PurchaseIndentReport.customercode=ci.customerid left outer join 
        Ordermaster om on V_PurchaseIndentReport.orderid=om.orderid
        Where V_PurchaseIndentReport.pindentid=" + ViewState["PIndentId"] + "";
        SqlDataAdapter sda = new SqlDataAdapter(qry, ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ds.Tables[0].Columns.Add("ImageName", typeof(System.Byte[]));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (Convert.ToString(dr["Photo"]) != "")
            {
                FileInfo TheFile = new FileInfo(Server.MapPath("~/PurchaseImage/") + dr["PIndentDetailId"] + "_PindentImage.gif");
                if (TheFile.Exists)
                {
                    string img = dr["Photo"].ToString();
                    img = Server.MapPath(@"~\" + img);
                    Byte[] img_Byte = File.ReadAllBytes(img);
                    dr["ImageName"] = img_Byte;
                }
            }
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\PGenrateIndentPhotoNEW.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\PGenrateIndentNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    public void OpenNewWidow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../Main.aspx");
    }
    protected void DGPIndentDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGPIndentDetail.PageIndex = e.NewPageIndex;
        fill_grid();
    }
    protected void DGPIndentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPIndentDetail, "select$" + e.Row.RowIndex);
        }
    }
    protected void TxtItemCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds1;
        string Str;
        if (TxtItemCode.Text != "")
        {
            DDCategory.SelectedIndex = -1;
            Str = @"select IPM.*,IM.CATEGORY_ID,cs.id from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate cs where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid
                 and ProductCode='" + TxtItemCode.Text + "'";
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                #region Author:Rajeev, Date:09-12-12...
                string Qry = @"SELECT Distinct dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                 dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID Order BY ITEM_CATEGORY_MASTER.CATEGORY_NAME 
                Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + @" Order BY Item_Name 
                select qualityid,qualityname from quality where item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + @" Order BY  qualityname
                select distinct Designid,DesignName from Design Order  by DesignName
                SELECT ColorId,ColorName FROM Color Order By ColorName
                select Shapeid,ShapeName from Shape Order by ShapeName
                SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString()) + @" Order BY SIZEFT
                select ShadeColorId,ShadeColorName from ShadeColor Order BY  ShadeColorName";
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                UtilityModule.ConditionalComboFillWithDS(ref DDCategory, DSQ, 0, true, "Select Catagory");
                DDCategory.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDItem, DSQ, 1, true, "--Select Item--");
                DDItem.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDQuality, DSQ, 2, true, "Select Quallity");
                DDQuality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDDesign, DSQ, 3, true, "Select Design");
                DDDesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDColor, DSQ, 4, true, "--Select Color--");
                DDColor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDShape, DSQ, 5, true, "--Select Shape--");
                DDShape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDSize, DSQ, 6, true, "--SELECT SIZE--");
                DDSize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref DDColorShade, DSQ, 7, true, "--select--");
                DDColorShade.SelectedValue = ds1.Tables[0].Rows[0]["shadecolor_id"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDCategory, @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                //                 dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID Order BY ITEM_CATEGORY_MASTER.CATEGORY_NAME ", true, "Select Catagory");
                //                DDCategory.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDItem, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " Orser BY Item_Name ", true, "--Select Item--");
                //                DDItem.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDQuality, "select qualityid,qualityname from quality where item_id=" + DDItem.SelectedValue + " Order BY  qualityname", true, "Select Quallity");
                //                DDQuality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDDesign, "select distinct Designid,DesignName from Design Order  by DesignName ", true, "Select Design");
                //                DDDesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDColor, "SELECT ColorId,ColorName FROM Color Order By ColorName ", true, "--Select Color--");
                //                DDColor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDShape, "select Shapeid,ShapeName from Shape Order by ShapeName", true, "--Select Shape--");
                //                DDShape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + DDShape.SelectedValue + " Order BY SIZEFT ", true, "--SELECT SIZE--");
                //                DDSize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();
                //                UtilityModule.ConditionalComboFill(ref DDColorShade, "select ShadeColorId,ShadeColorName from ShadeColor Ordr BY  ShadeColorName ", true, "--select--");
                //                DDColorShade.SelectedValue = ds1.Tables[0].Rows[0]["shadecolor_id"].ToString();
                #endregion
                Session["finishedid"] = ds1.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(DDQuality.SelectedValue) > 0)
                {
                    TdQuality.Visible = true;
                }
                else
                {
                    TdQuality.Visible = false;
                }
                if (Convert.ToInt32(DDDesign.SelectedValue) > 0)
                {
                    TdDesign.Visible = true;
                }
                else
                {
                    TdDesign.Visible = false;
                }
                int c = (DDColor.SelectedIndex > 0 ? Convert.ToInt32(DDColor.SelectedValue) : 0);
                if (c > 0)
                {
                    TdColor.Visible = true;
                }
                else
                {
                    TdColor.Visible = false;
                }
                int s = (DDShape.SelectedIndex > 0 ? Convert.ToInt32(DDShape.SelectedValue) : 0);
                if (s > 0)
                {
                    TdShape.Visible = true;
                }
                else
                {
                    TdShape.Visible = false;
                }
                int si = (DDSize.SelectedIndex > 0 ? Convert.ToInt32(DDSize.SelectedValue) : 0);
                if (si > 0)
                {
                    TdSize.Visible = true;
                }
                else
                {
                    TdSize.Visible = false;
                }
                int sc = (DDColorShade.SelectedIndex > 0 ? Convert.ToInt32(DDColorShade.SelectedValue) : 0);
                if (sc > 0)
                {
                    TdColorShade.Visible = true;
                }
                else
                {
                    TdColorShade.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref DDUnit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + DDItem.SelectedValue + " Order BY u.UnitName ", true, "Select Unit");
                Lblmessage.Visible = false;
            }
            else
            {
                Lblmessage.Visible = true;
                Lblmessage.Text = "ProdCode doesn't exist";
                TxtItemCode.Focus();
            }
        }
        else
        {
            Lblmessage.Visible = true;
            Lblmessage.Text = "Plz fill ProdCode";
            TxtItemCode.Focus();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%'";
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
    //protected void DGPIndentDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            tdindno.Visible = true;
            UtilityModule.ConditionalComboFill(ref ddindentno, "select pindentid,pindentno from PurchaseIndentMaster where flagapproval<>1 and  CompanyId="+DDCompanyName.SelectedValue+" Order BY pindentno ", true, "-Select-");
        }
        else
        {
            tdindno.Visible = false;
            TxtIndentNo.Text = "";
        }
        refresh();

    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        if (ChkEditOrder.Checked == true)
        {
            tdindno.Visible = true;
            UtilityModule.ConditionalComboFill(ref ddindentno, "select pindentid,pindentno from PurchaseIndentMaster where flagapproval<>1 and  CompanyId=" + DDCompanyName.SelectedValue + " Order BY pindentno ", true, "-Select-");
            refresh();
            DGPIndentDetail.DataSource = null;
            DGPIndentDetail.DataBind();
        }
    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        DataSet ds;
        UtilityModule.ConditionalComboFill(ref DDDepartment, "Select distinct d.DepartmentId,d.DepartmentName from Department d,PurchaseIndentMaster p where p.departmentid=d.DepartmentId and p.flagapproval<>1 And P.PindentId=" + ddindentno.SelectedValue + " Order BY d.DepartmentName ", true, "--Select Department--");
        UtilityModule.ConditionalComboFill(ref ddcustomer, "select distinct ci.customerid,ci.Customercode from customerinfo ci inner join OrderMaster om on om.customerid=ci.customerid inner join PurchaseIndentMaster PM on PM.OrderId=Om.OrderId And PM.PindentId=" + ddindentno.SelectedValue + " order BY ci.Customercode", true, "--Select Customer Code--");
        ViewState["PIndentId"] = ddindentno.SelectedValue;
        //TxtIndentNo.Text = ddindentno.SelectedValue;
        TxtIndentNo.Text = ddindentno.SelectedItem.Text;

        fill_grid();
    }
    public void refresh()
    {
        if (ChkEditOrder.Checked == false)
        {
            DDCompanyName.SelectedIndex = 0;
        }
       
        if (ddindentno.Items.Count > 0)
        {
            ddindentno.SelectedIndex = -1;
        }
        DDDepartment.Items.Clear();
        DDEmp.Items.Clear();
        TxtIndentNo.Text = "";
        TxtItemCode.Text = "";
        TxtDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        DDCategory.Items.Clear();
        DDItem.Items.Clear();
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDColorShade.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        txtremarks.Text = "";
        txtitemremark.Text = "";
    }
    protected void DGPIndentDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select departmentid,partyid,pindentno,empid,replace(convert(varchar(11),date,106),' ','-') as date,remark,customercode,orderid,replace(convert(varchar(11),Reqdate,106),' ','-') as reqdate from PurchaseIndentMaster where pindentid=" + ddindentno.SelectedValue + "");
        DDDepartment.SelectedValue = ds.Tables[0].Rows[0]["departmentid"].ToString();
        departmentchange();
        DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["partyid"].ToString();
        TxtIndentNo.Text = ds.Tables[0].Rows[0]["pindentno"].ToString();
        DDEmp.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
        TxtDate.Text = ds.Tables[0].Rows[0]["date"].ToString();
        txtremarks.Text = ds.Tables[0].Rows[0]["remark"].ToString();
        txtreqdate.Text = ds.Tables[0].Rows[0]["reqdate"].ToString();
        if (ds.Tables[0].Rows[0]["customercode"].ToString() != "0")
        {
            ddcustomer.SelectedValue = ds.Tables[0].Rows[0]["customercode"].ToString();
            UtilityModule.ConditionalComboFill(ref ddorder, "Select Distinct OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where  CustomerId=" + ddcustomer.SelectedValue + " Order BY LocalOrder+ ' / ' +CustomerOrderNo ", true, "-Select Order");
            ddorder.SelectedValue = ds.Tables[0].Rows[0]["orderid"].ToString();
            ChKForOrder.Checked = true;
            tdcust.Visible = true;
            tdorder.Visible = true;
        }
        partychange();
        int n = DGPIndentDetail.SelectedIndex;
        txtitemremark.Text = ((Label)DGPIndentDetail.Rows[n].FindControl("lblitemremark")).Text;
        string finishedid = ((Label)DGPIndentDetail.Rows[n].FindControl("lblfinishedid")).Text;
        string pindentdetailid = ((Label)DGPIndentDetail.Rows[n].FindControl("lblPIndentDetailId")).Text;
        ViewState["PIndentDetailId"] = pindentdetailid;
        TxtQty.Text = ((Label)DGPIndentDetail.Rows[n].FindControl("lblqty")).Text;
        txtrate.Text = ((Label)DGPIndentDetail.Rows[n].FindControl("lblrate")).Text;
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select category_id,item_id,qualityid,designid,colorid,shadecolorid,shapeid,sizeid from V_FinishedItemDetail where item_finished_id=" + finishedid + " ");
        DDCategory.SelectedValue = ds1.Tables[0].Rows[0]["category_id"].ToString();
        ddlcategorychange1();
        DDItem.SelectedValue = ds1.Tables[0].Rows[0]["item_id"].ToString();
        itemchange();
        if (DDQuality.Items.Count > 0)
            DDQuality.SelectedValue = ds1.Tables[0].Rows[0]["qualityid"].ToString();
        if (DDDesign.Items.Count > 0)
            DDDesign.SelectedValue = ds1.Tables[0].Rows[0]["designid"].ToString();
        if (DDColor.Items.Count > 0)
            DDColor.SelectedValue = ds1.Tables[0].Rows[0]["colorid"].ToString();
        if (DDColorShade.Items.Count > 0)
            DDColorShade.SelectedValue = ds1.Tables[0].Rows[0]["shadecolorid"].ToString();
        if (DDShape.Items.Count > 0)
            DDShape.SelectedValue = ds1.Tables[0].Rows[0]["shapeid"].ToString();
        DDsizetype.SelectedValue = ((Label)DGPIndentDetail.Rows[n].FindControl("lblflagsize")).Text;
        FillSize();
        if (DDSize.Items.Count > 0)
            DDSize.SelectedValue = ds1.Tables[0].Rows[0]["sizeid"].ToString();

    }
    protected void ChKForOrder_CheckedChanged(object sender, EventArgs e)
    {
        Chkfororder_check();
    }
    private void Chkfororder_check()
    {
        if (ChKForOrder.Checked == true)
        {
            tdcust.Visible = true;
            tdorder.Visible = true;
            fill_grid_show();
        }
        else
        {
            tdcust.Visible = false;
            tdorder.Visible = false;
            TDGridShow.Visible = false;
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddorder, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,PurchaseIndentMaster PM Where  OM.CustomerId=" + ddcustomer.SelectedValue + " And PM.OrderId=OM.OrderId And PindentId=" + ddindentno.SelectedValue + " and OM.Status=0  Order BY LocalOrder+ ' / ' +CustomerOrderNo", true, "-Select Order");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddorder, "Select Distinct OM.OrderId,OM.LocalOrder+ ' / ' +OM.CustomerOrderNo as OrderNo from OrderMaster OM Where  OM.CustomerId=" + ddcustomer.SelectedValue + " and OM.status=0 Order BY OrderNo", true, "-Select Order");
        }
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DGPIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[4];
            arr[0] = new SqlParameter("@Pindentdetailid", SqlDbType.Int);
            arr[1] = new SqlParameter("@pindentid", SqlDbType.Int);
            arr[2] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.NVarChar, 250);

            arr[0].Value = DGPIndentDetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = ((Label)DGPIndentDetail.Rows[e.RowIndex].FindControl("lblpindentid")).Text;
            arr[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_purchaseIndentdelete", arr);
            tran.Commit();

            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + arr[2].Value + "');", true);
            fill_grid();
        }
        catch (Exception ex)
        {
            tran.Rollback();
        }
    }
    private void fill_grid_show()
    {
        DataSet ds;
        switch (Convert.ToInt16(Session["varcompanyId"]))
        {
            case 6:
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,(isnull(Sum(consumptionqty),0)-isnull(sum(purchaseqty),0)) as qty,vc.finishedid,'0' as ISizeflag
        from V_ConsumptionQtyAndPurchaseIndentQtyForArtIndia vc inner join v_finisheditemdetail v On vc.finishedid=v.ITEM_FINISHED_ID
        where orderid=" + ddorder.SelectedValue + @"
        group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,vc.finishedid
        having  round(isnull(Sum(consumptionqty),0),2)>round(isnull(sum(purchaseqty),0),2)");
                break;
            default:
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT Category_Name+'  '+VF.ITEM_NAME +'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
                           CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End Description,sum(Qty) As Qty,
                           VF.Item_Finished_Id as finishedid,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,SizeId,'0' as ISizeflag FROM OrderLocalConsumption OD Inner JOIN V_FinishedItemDetail VF ON 
                           OD.FinishedId=VF.Item_Finished_Id  INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id 
                           Where OrderId=" + ddorder.SelectedValue + " And VF.MasterCompanyId=" + Session["varcompanyId"] + @" Group by Category_Name,ITEM_NAME,QualityName,DesignName,ColorName,ShadeColorName,
                           ShapeName,SizeUnit,SizeMtr,SizeFt, VF.Item_Finished_Id,Qualityid,Colorid,designid,shapeid,shadecolorid,category_id,vf.item_id,SizeId");
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,(isnull(Sum(consumptionqty),0)-isnull(sum(purchaseqty),0)) as qty,vc.finishedid,Max(ISizeflag) as ISizeflag
        from V_ConsumptionQtyAndPurchaseIndentQty vc inner join v_finisheditemdetail v On vc.finishedid=v.ITEM_FINISHED_ID
        where orderid=" + ddorder.SelectedValue + @"
        group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,vc.finishedid
        having  round(isnull(Sum(consumptionqty),0),2)>round(isnull(sum(purchaseqty),0),2)");
                }
                break;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGShowConsumption.DataSource = ds;
            DGShowConsumption.DataBind();
            TDGridShow.Visible = true;
        }
        else
        {
            TDGridShow.Visible = false;
        }
    }
    protected void ddorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        fill_grid_show();
    }
    protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DGShowConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowConsumption, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGShowConsumption_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(DGShowConsumption.SelectedIndex.ToString());
        string category = ((Label)DGShowConsumption.Rows[r].FindControl("lblcategoryid")).Text;
        string Item = ((Label)DGShowConsumption.Rows[r].FindControl("lblitem_id")).Text;
        string Quality = ((Label)DGShowConsumption.Rows[r].FindControl("lblQualityid")).Text;
        string Color = ((Label)DGShowConsumption.Rows[r].FindControl("lblColorid")).Text;
        string design = ((Label)DGShowConsumption.Rows[r].FindControl("lbldesignid")).Text;
        string shape = ((Label)DGShowConsumption.Rows[r].FindControl("lblshapeid")).Text;
        string shadecolor = ((Label)DGShowConsumption.Rows[r].FindControl("lblshadecolorid")).Text;
        string size = ((Label)DGShowConsumption.Rows[r].FindControl("lblsizeid")).Text;
        string sizeflag = ((Label)DGShowConsumption.Rows[r].FindControl("lblsizeflag")).Text;
        string Qty = ((Label)DGShowConsumption.Rows[r].FindControl("lblqty")).Text;
        string OrderedQty = ((Label)DGShowConsumption.Rows[r].FindControl("lblorderedqty")).Text;
        Qty = Qty == "" ? "0" : Qty;
        OrderedQty = OrderedQty == "" ? "0" : OrderedQty;
        if (DDCategory.Visible == true)
        {
            DDCategory.SelectedValue = category;
            ddlcategorychange1();
        }
        if (DDItem.Visible == true)
        {
            DDItem.SelectedValue = Item;
            itemchange();
        }
        if (DDQuality.Visible == true)
        {
            DDQuality.SelectedValue = Quality;
        }
        if (DDDesign.Visible == true)
        {
            DDDesign.SelectedValue = design;
        }
        if (DDColor.Visible == true)
        {
            DDColor.SelectedValue = Color;
        }
        if (DDColorShade.Visible == true)
        {
            DDColorShade.SelectedValue = shadecolor;
        }
        if (DDShape.Visible == true)
        {
            DDShape.SelectedValue = shape;
            DDsizetype.SelectedValue = sizeflag;
            FillSize();
        }
        if (DDSize.Visible == true)
        {
            DDSize.SelectedValue = size;
        }
        TxtQty.Text = (Convert.ToDouble(Qty) - Convert.ToDouble(OrderedQty)).ToString();
    }

    protected void DGPIndentDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      
    }
    public string getgiven(string strval)
    {
        string val = "0";
        DataSet ds;
        if (ChKForOrder.Checked == true)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(Qty),0) from PurchaseIndentDetail piit,PurchaseIndentMaster pii where pii.PindentId=piit.PindentId and  finishedid=" + strval + "  and pii.orderid=" + ddorder.SelectedValue + " And pii.MasterCompanyid=" + Session["varCompanyId"] + "");
            val = ds.Tables[0].Rows[0][0].ToString();
        }

        return val;
    }

    protected void BtnAddImage_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string CommandName = btn.CommandName;
        string CommandArgument = btn.CommandArgument;
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("window.open('../Carpet/AddPhotoRefImage1.aspx?SrNo=" + DGPIndentDetail.DataKeys[index].Value + "&img=pp', 'nwwin', 'toolbar=0, titlebar=1,  top=200px, left=100px, scrollbars=1, resizable = yes,width=550px,Height=200px');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
}