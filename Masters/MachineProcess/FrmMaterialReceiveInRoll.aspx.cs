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
using System.Drawing;
using System.Drawing.Drawing2D;
using ClosedXML.Excel;

public partial class Masters_MachineProcess_FrmMaterialReceiveInRoll : System.Web.UI.Page
{
    string str = "";
    int varcombo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(nolock)
            JOIN Company_Authentication CA(nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CI.Companyname 

            Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From MaterialIssueOnMachineMaster MIMM(Nolock) 
            JOIN PROCESS_NAME_MASTER PNM(nolock) ON PNM.PROCESS_NAME_ID = MIMM.ProcessId 
            Where MIMM.MasterCompanyid = " + Session["varcompanyid"] + @" Order By MIMM.ProcessID 

            Select Distinct U.UnitsId, U.UnitName 
            From MaterialIssueOnMachineMaster MIMM(Nolock) 
            JOIN Units U(Nolock) ON U.UnitsId = MIMM.ProductionUnitId 
            Where MIMM.MasterCompanyid = " + Session["varcompanyid"] + @"
            Order By U.UnitName

            Select Distinct MIMM.MachineNoId, MNM.MachineNoName 
            From MaterialIssueOnMachineMaster MIMM(Nolock) 
            JOIN MachineNoMaster MNM(Nolock) ON MNM.MachineNoID = MIMM.MachineNoId 
            Where MIMM.MasterCompanyid = " + Session["varcompanyid"] + @" Order By MNM.MachineNoName 

            Select Distinct ic.CATEGORY_ID, ic.CATEGORY_NAME
            From ITEM_CATEGORY_MASTER IC(Nolock) 
            JOIN CategorySeparate CS(Nolock) ON CS.Categoryid = IC.CATEGORY_ID 
            Where cs.id = 0 And IC.MasterCompanyID = " + Session["varcompanyid"] + @"

            Select Distinct OM.CustomerID, CI.CustomerCode 
            From OrderMaster OM(Nolock) 
            JOIN CustomerInfo CI(Nolock) ON CI.CustomerID = OM.CustomerID 
            JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderID 
            Where OM.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
            Order By CI.CustomerCode

            Select UnitID, UnitName 
            From Unit(Nolock) 
            Where UnitID in (1, 2, 6) ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProductionUnit, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMachineNo, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 6, false, "");

            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
            }

            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                ddlcategorycange();
            }

            txtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnMaterialReceiveID.Value = "0";
        }
    }
    private void ddlcategorycange()
    {
        dditemname.Items.Clear();
        dquality.Items.Clear();
        dddesign.Items.Clear();
        ddcolor.Items.Clear();
        ddshape.Items.Clear();
        ddlshade.Items.Clear();
        ddsize.Items.Clear();
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        shd.Visible = false;
        sz.Visible = false;
        UtilityModule.ConditionalComboFill(ref dditemname, @"select Distinct im.ITEM_ID, im.ITEM_NAME 
            FROM ITEM_MASTER im 
            join v_finisheditemdetail v On v.ITEM_ID = im.ITEM_ID And v.CATEGORY_ID=" + ddCatagory.SelectedValue + @" 
            where im.MasterCompanyid=" + Session["varCompanyId"] + " Order by im.ITEM_NAME", true, "--Select Item--");

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM 
                      join PARAMETER_MASTER PM on IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] 
                      where [CATEGORY_ID] = " + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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

    protected void DDMachineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        MachineNoSelectedChanged();
    }
    private void MachineNoSelectedChanged()
    {
        HnMaterialReceiveID.Value = "0";
        txtReceiveno.Text = "";
        string str = @"Select MaterialIssueId, IssueNo 
            From MaterialIssueOnMachineMaster(Nolock) 
            Where CompanyId = " + ddCompName.SelectedValue + " And ProcessId = " + DDProcessName.SelectedValue + @" And 
            ProductionUnitId = " + DDProductionUnit.SelectedValue + " And MachineNoId = " + DDMachineNo.SelectedValue + @"
            Order By MaterialIssueId";

        if (ChKForEdit.Checked == true)
        {
            str = str + @" Select a.MaterialReceiveInPcsID, a.ReceiveNo 
                From MaterialReceiveInPcsMaster a(Nolock) 
                Where a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + " And a.ProdUnitID = " + DDProductionUnit.SelectedValue + @" And 
                a.MachineNoID = " + DDMachineNo.SelectedValue + " Order By a.MaterialReceiveInPcsID";
        }

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");
        if (ChKForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDReceiveNo, ds, 1, true, "-Select Issue No-");
        }
    }

    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }

    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedChange();
    }
    private void ItemNameSelectedChange()
    {
        txtReceiveno.Text = "";
        HnMaterialReceiveID.Value = "0";
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        fill_combo();
    }

    private void fill_combo()
    {
        txtReceiveno.Text = "";
        HnMaterialReceiveID.Value = "0";
        if (dquality.Visible == true && varcombo < 1)
        {
            UtilityModule.ConditionalComboFill(ref dquality, @"select Distinct im.QualityId,im.QualityName 
            FROM Quality im 
            join v_finisheditemdetail v On v.QualityId=im.QualityId 
            " + str + " order by im.QualityName", true, "-Select Quality-");
            return;
        }
        if (dddesign.Visible == true && varcombo < 2)
        {
            UtilityModule.ConditionalComboFill(ref dddesign, @"select Distinct im.designId,im.designName 
                FROM Design im 
                join v_finisheditemdetail v On v.designId=im.designId 
                " + str + "  order by im.designName", true, "-Select Design-");
            return;
        }
        if (ddcolor.Visible == true && varcombo < 3)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"select Distinct im.ColorId,im.ColorName 
            FROM color im 
            join v_finisheditemdetail v On v.ColorId=im.ColorId 
            " + str + " order by im.ColorName", true, "-Select Colour-");
            return;
        }
        if (ddshape.Visible == true && varcombo < 4)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"select Distinct im.ShapeId,im.ShapeName 
            FROM Shape im 
            join v_finisheditemdetail v On v.ShapeId=im.ShapeId 
            " + str + " order by im.ShapeName", true, "-Select Shape-");
            return;
        }
        if (ddsize.Visible == true && varcombo < 5)
        {
            FillSize();
            return;
        }
        if (ddlshade.Visible == true && varcombo < 6)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"select Distinct im.ShadecolorId,im.ShadeColorName 
            FROM ShadeColor im 
            join v_finisheditemdetail v On v.ShadecolorId=im.ShadecolorId 
            " + str + " order by im.ShadeColorName", true, "-Select Shade Colour-");
            return;
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        quality_change();
    }
    private void quality_change()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.QualityId=" + dquality.SelectedValue + " and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        varcombo = 1;
        fill_combo();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Design_change();
    }
    private void Design_change()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.designId=" + dddesign.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        if (dquality.Visible == true)
        {
            str = str + " and v.QualityId=" + dquality.SelectedValue + "";
        }
        varcombo = 2;
        fill_combo();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        colour_change();
    }
    private void colour_change()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.ColorId=" + ddcolor.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        if (dddesign.Visible == true)
        {
            str = str + " and v.designId=" + dddesign.SelectedValue + "";
        }
        if (dquality.Visible == true)
        {
            str = str + " and v.QualityId=" + dquality.SelectedValue + "";
        }
        varcombo = 3;
        fill_combo();
    }

    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        shade_change();
    }
    private void shade_change()
    {
        varcombo = 6;
        fill_combo();
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderNO();
    }
    protected void FillOrderNO()
    {
        string str = @"Select Distinct OM.OrderId, OM.CustomerOrderNo 
        From OrderMaster OM
        JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
        Where OM.Status = 0 And OM.CompanyId = " + ddCompName.SelectedValue + " And OM.CustomerId = " + DDCustomerCode.SelectedValue + @" Order By OM.OrderId";
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, str, true, "--Plz Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderDescription();
    }
    protected void FillOrderDescription()
    {
        TxtOrderLength.Text = "";
        TxtOrderWidth.Text = "";
        string str = @"Select OD.Item_Finished_Id, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
        Case When OD.OrderUnitId = 1 Then VF.SizeMtr Else Case When OD.OrderUnitId = 2 Then VF.SizeFt Else VF.SizeInch End End [Description] 
        From OrderMaster OM(nolock)
        JOIN OrderDetail OD(nolock) ON OD.OrderID = OM.OrderID 
        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
        Where OM.OrderID = " + DDCustomerOrderNumber.SelectedValue + @" 
        Order By VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
        Case When OD.OrderUnitId = 1 Then VF.SizeMtr Else Case When OD.OrderUnitId = 2 Then VF.SizeFt Else VF.SizeInch End End ";

        UtilityModule.ConditionalComboFill(ref DDOrderDescription, str, true, "--Plz Select--");
    }

    protected void DDOrderDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtOrderQty.Text = "";
        TxtPendingQty.Text = "";
        if (TxtSplitWidth.Text == "")
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please fill split width";
            DDOrderDescription.SelectedIndex = 0;
            return;
        }
        if (TxtSplitLength.Text == "")
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please fill split length";
            DDOrderDescription.SelectedIndex = 0;
            return;
        }
        OrderDescriptionSelectedIndexChanged();
    }

    protected void OrderDescriptionSelectedIndexChanged()
    {
        Decimal Width = TxtSplitWidth.Text == "" ? 0 : Convert.ToDecimal(TxtSplitWidth.Text);
        Decimal Length = TxtSplitLength.Text == "" ? 0 : Convert.ToDecimal(TxtSplitLength.Text);
//        string str = @"Select OD.QtyRequired, 
//                OD.QtyRequired - IsNull((Select Sum(IsNull(Qty, 0)) From MaterialReceiveInPcsDetail MRPD(Nolock) Where MRPD.ORDERID = OM.OrderID And MRPD.ITEM_FINISHED_ID = OD.Item_Finished_Id), 0) PendingQty, 
//                FLOOR(" + Width + " / Case When " + DDUnit.SelectedValue + " = 1 Then VF.WidthMtr Else Case When " + DDUnit.SelectedValue + @" = 2 Then VF.WidthFt Else VF.WidthINCH End End) * 
//                FLOOR(" + Length + " / Case When " + DDUnit.SelectedValue + " = 1 Then VF.LengthMtr Else Case When " + DDUnit.SelectedValue + @" = 2 Then VF.LengthFt Else VF.LengthINCH End End) RecQty 
//                From OrderMaster OM(nolock)
//                JOIN OrderDetail OD(nolock) ON OD.OrderID = OM.OrderID 
//                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
//                Where OM.OrderID = " + DDCustomerOrderNumber.SelectedValue + " And OD.Item_Finished_Id = " + DDOrderDescription.SelectedValue;
        string str = @"Select OD.QtyRequired, 
                OD.QtyRequired - IsNull((Select Sum(IsNull(Qty, 0)) From MaterialReceiveInPcsDetail MRPD(Nolock) Where MRPD.ORDERID = OM.OrderID And MRPD.ITEM_FINISHED_ID = OD.Item_Finished_Id), 0) PendingQty 
                From OrderMaster OM(nolock)
                JOIN OrderDetail OD(nolock) ON OD.OrderID = OM.OrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                Where OM.OrderID = " + DDCustomerOrderNumber.SelectedValue + " And OD.Item_Finished_Id = " + DDOrderDescription.SelectedValue;

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtOrderQty.Text = Ds.Tables[0].Rows[0]["QtyRequired"].ToString();
            TxtPendingQty.Text = Ds.Tables[0].Rows[0]["PendingQty"].ToString();
            //TxtRecQty.Text = Ds.Tables[0].Rows[0]["RecQty"].ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        TextBox TxtProdCode = new TextBox();
        string strWeight = "0", strNoofRoll = "1";

        int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

        if (TxtMainRollWeight.Text != "")
        {
            strWeight = TxtMainRollWeight.Text;
        }
        if (TxtNoofRoll.Text != "")
        {
            strNoofRoll = TxtNoofRoll.Text;
        }

        string MasterData = ddCompName.SelectedValue + "|" + DDProcessName.SelectedValue + "|" + DDProductionUnit.SelectedValue + "|" + DDMachineNo.SelectedValue + "|";
        MasterData = MasterData + txtReceiveDate.Text + "|" + DDUnit.SelectedValue + "|" + varfinishedid + "|" + strWeight;

        string DetailData = DDIssueNo.SelectedValue + "|" + DDCustomerOrderNumber.SelectedValue + "|" + DDOrderDescription.SelectedValue + "|" + TxtSplitWidth.Text + "|";
        DetailData = DetailData + TxtSplitLength.Text + "|" + TxtRecQty.Text + "|" + strNoofRoll;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[22];
            arr[0] = new SqlParameter("@MaterialReceiveInPcsID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("MasterData", SqlDbType.NVarChar);
            arr[3] = new SqlParameter("DetailData", SqlDbType.NVarChar);
            arr[4] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnMaterialReceiveID.Value;
            arr[1].Direction = ParameterDirection.InputOutput;
            arr[1].Value = txtReceiveno.Text;
            arr[2].Value = MasterData;
            arr[3].Value = DetailData;
            arr[4].Value = Session["varuserid"];
            arr[5].Value = Session["varCompanyId"];
            arr[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveMaterialReceiveInRoll]", arr);

            if (arr[6].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[6].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnMaterialReceiveID.Value = arr[0].Value.ToString();
                txtReceiveno.Text = Convert.ToString(arr[1].Value);
                tran.Commit();
            }
            TxtOrderWidth.Text = "";
            TxtOrderLength.Text = "";
            TxtRecQty.Text = "";
            TxtNoofRoll.Text = "";
            fill_grid();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void fill_grid()
    {
        string str = @"Select a.MaterialReceiveInPcsID, b.MaterialReceiveInPcsDetailID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate, 
            c.IssueNo, CI.CustomerCode + ' ' + OM.CustomerOrderNo OrderNo, 
            VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End ItemDescription, b.Qty, 
            VF1.CATEGORY_ID CategoryID, VF1.ITEM_ID ItemID, VF1.QualityID, VF1.DesignID, VF1.ColorID, VF1.ShapeId, VF1.SizeId, a.UnitID, b.SplitWidth, b.SplitLength 
            From MaterialReceiveInPcsMaster a(Nolock) 
            JOIN MaterialReceiveInPcsDetail b(Nolock) ON b.MaterialReceiveInPcsID = a.MaterialReceiveInPcsID 
            JOIN MaterialIssueOnMachineMaster c(Nolock) ON c.MaterialIssueId = b.MaterialIssueID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID 
            JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = a.MainRollFinishedID 
            Where a.MaterialReceiveInPcsID = " + HnMaterialReceiveID.Value + @" 
            Order By b.MaterialReceiveInPcsDetailID Desc ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
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
            Label lblMaterialReceiveInPcsID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialReceiveInPcsID");
            Label lblMaterialReceiveInPcsDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialReceiveInPcsDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@MaterialReceiveInPcsID", lblMaterialReceiveInPcsID.Text);
            param[1] = new SqlParameter("@MaterialReceiveInPcsDetailID", lblMaterialReceiveInPcsDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteMaterialReceiveInRoll", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
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
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDReceiveNo.Items.Clear();
        txtReceiveno.Text = "";
        txtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Td3.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            Td3.Visible = true;
            MachineNoSelectedChanged();
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        string Str = @"Select a.MaterialReceiveInPcsID RollNo,b.MaterialReceiveInPcsDetailID as SubRollNo, CI.CompanyName, CI.CompAddr1 + ', ' + CI.CompAddr2 + ', ' + CI.CompAddr3 CompanyAddress, 
            CI.CompTel, CI.GSTNo, PNM.PROCESS_NAME ProcessName, U.UnitName ProdUnit, MNM.MachineNoName, a.ReceiveNo, 
            REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate, U1.UnitName, 
            VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName MainRollDescription, 
            OM.CustomerOrderNo OrderNo, VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.ShadeColorName SubRollDescription, 
            Cast(b.SplitWidth as Nvarchar) + 'x' + Cast(b.SplitLength as Nvarchar) SubRollSize, MIMM.IssueNo RawMaterialIssueNo, 
            Case When a.UnitID = 1 Then VF1.SizeMtr Else Case When a.UnitID = 2 Then VF1.SizeFt Else VF1.SizeInch End End OrderSize, b.Qty OrderQty, a.RollWeight 
            From MaterialReceiveInPcsMaster a(Nolock)
            JOIN MaterialReceiveInPcsDetail b(Nolock) ON b.MaterialReceiveInPcsID = a.MaterialReceiveInPcsID 
            JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            JOIN Units U(Nolock) ON U.UnitsId = a.ProdUnitID 
            JOIN Unit U1(Nolock) ON U1.UnitId = a.UnitID 
            JOIN MachineNoMaster MNM(Nolock) ON MNM.MachineNoID = a.MachineNoID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.MainRollFinishedID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = b.Item_Finished_ID 
            JOIN MaterialIssueOnMachineMaster MIMM(Nolock) ON MIMM.MaterialIssueId = b.MaterialIssueID 
            Where a.MaterialReceiveInPcsID = " + HnMaterialReceiveID.Value + @" 
            Order BY b.MaterialReceiveInPcsDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptMaterialReceiveInRollForm.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialReceiveInRollForm.xsd";
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
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        HnMaterialReceiveID.Value = DDReceiveNo.SelectedValue;
        ReceiveNoSelectedIndexChanged();
        fill_grid();
    }
    private void ReceiveNoSelectedIndexChanged()
    {
        if (ChKForEdit.Checked == true)
        {
            string str = @"Select a.MaterialReceiveInPcsID, b.MaterialReceiveInPcsDetailID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate, 
            VF1.CATEGORY_ID CategoryID, VF1.ITEM_ID ItemID, VF1.QualityID, VF1.DesignID, VF1.ColorID, VF1.ShapeId, VF1.SizeId, a.UnitID, b.SplitWidth, b.SplitLength 
            From MaterialReceiveInPcsMaster a(Nolock) 
            JOIN MaterialReceiveInPcsDetail b(Nolock) ON b.MaterialReceiveInPcsID = a.MaterialReceiveInPcsID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = a.MainRollFinishedID 
            Where a.MaterialReceiveInPcsID = " + HnMaterialReceiveID.Value + @" 
            Order By b.MaterialReceiveInPcsDetailID Desc ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitID"].ToString();
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CategoryID"].ToString();
                ddlcategorycange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ItemID"].ToString();
                ItemNameSelectedChange();
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityID"].ToString();
                quality_change();
                if (dsn.Visible == true)
                {
                    dddesign.SelectedValue = ds.Tables[0].Rows[0]["DesignID"].ToString();
                    Design_change();
                }
                if (clr.Visible == true)
                {
                    ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorID"].ToString();
                    colour_change();
                }
                if (shp.Visible == true)
                {
                    ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeID"].ToString();
                    FillShapeSelectedChange();
                }
                if (sz.Visible == true)
                {
                    ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeID"].ToString();
                }

                TxtSplitWidth.Text = ds.Tables[0].Rows[0]["SplitWidth"].ToString();
                TxtSplitLength.Text = ds.Tables[0].Rows[0]["SplitLength"].ToString();

                txtReceiveno.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
                txtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            }
            else
            {
                txtReceiveno.Text = "";
                txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        HnMaterialReceiveID.Value = DDReceiveNo.SelectedValue;
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
    }
    private void FillShapeSelectedChange()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        if (dddesign.Visible == true)
        {
            str = str + "and v.designId=" + dddesign.SelectedValue + "";
        }
        if (dquality.Visible == true)
        {
            str = str + "and v.QualityId=" + dquality.SelectedValue + "";
        }
        if (ddcolor.Visible == true)
        {
            str = str + "and v.ColorId=" + ddcolor.SelectedValue + "";
        }
        if (ddshape.Visible == true)
        {
            str = str + "and v.ShapeId=" + ddshape.SelectedValue + "";
        }
        varcombo = 4;
        fill_combo();
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddshape.Items.Count > 0)
        {
            ddshape.SelectedIndex = 0;
        }
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        FillSize();
        if (ddsize.Items.Count > 0)
        {
            ddsize.SelectedIndex = 1;
            SizeSelectedChanged();
        }
    }
    private void FillSize()
    {
        UtilityModule.ConditionalComboFill(ref ddsize, @"select Distinct im.SizeId, 
            Case When " + DDUnit.SelectedValue + @" = 1 Then im.SizeMtr Else Case When " + DDUnit.SelectedValue + @" = 2 Then im.SizeFt Else im.SizeInch End End Size
            From Size im 
            Where im.ShapeID = " + ddshape.SelectedValue + " order by im.SizeId Desc", true, "-Select Size-");
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeSelectedChanged();
    }
    private void SizeSelectedChanged()
    {
        if (DDOrderDescription.Items.Count > 0)
        {
            DDOrderDescription.SelectedIndex = 0;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
        @"Select Case When " + DDUnit.SelectedValue + @" = 1 Then s.WidthMtr Else Case When " + DDUnit.SelectedValue + @" = 2 Then s.WidthFt Else WidthInch End End Width, 
        Case When " + DDUnit.SelectedValue + @" = 1 Then s.LengthMtr Else Case When " + DDUnit.SelectedValue + @" = 2 Then s.LengthFt Else LengthInch End End Length
        From Size s Where s.SizeID = " + ddsize.SelectedValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtSplitLength.Text = ds.Tables[0].Rows[0]["Length"].ToString();
            TxtSplitWidth.Text = ds.Tables[0].Rows[0]["Width"].ToString();
        }
    }
    protected void TxtOrderWidth_TextChanged(object sender, EventArgs e)
    {
        if (TxtSplitWidth.Text == "")
        {
            TxtOrderWidth.Text = "";
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please fill split width";
            TxtSplitWidth.Focus();
            return;
        }
        OrderWidthLengthChanged();
    }
    protected void TxtOrderLength_TextChanged(object sender, EventArgs e)
    {
        if (TxtSplitLength.Text == "")
        {
            TxtOrderLength.Text = "";
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please fill split length";
            TxtSplitLength.Focus();
            return;
        }
        OrderWidthLengthChanged();
    }
    private void OrderWidthLengthChanged()
    {
        TxtRecQty.Text = "";
        Decimal Width = TxtSplitWidth.Text == "" ? 0 : Convert.ToDecimal(TxtSplitWidth.Text);
        Decimal Length = TxtSplitLength.Text == "" ? 0 : Convert.ToDecimal(TxtSplitLength.Text);

        Decimal OrderWidth = TxtOrderWidth.Text == "" ? 0 : Convert.ToDecimal(TxtOrderWidth.Text);
        Decimal OrderLength = TxtOrderLength.Text == "" ? 0 : Convert.ToDecimal(TxtOrderLength.Text);

        if (OrderLength > 0 && OrderWidth > 0)
        {
            TxtRecQty.Text = (Convert.ToInt32(Math.Truncate(Width / OrderWidth)) * Convert.ToInt32(Math.Truncate(Length / OrderLength))).ToString();
        }
    }
}
