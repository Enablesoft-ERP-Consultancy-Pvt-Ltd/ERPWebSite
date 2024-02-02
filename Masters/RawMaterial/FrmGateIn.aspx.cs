using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_FrmGateIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "";
            str = @"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname";
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 45)
            {
                str = str + @" Select EI.EmpID, EI.EmpName + ' (' + EI.EmpCode + ')' EmpName1 
                    From Empinfo EI(Nolock) 
                    JOIN EmpinfoPageName EP(Nolock) ON EI.EmpID = EP.EmpID And EP.PageName = 'FrmGateIn'
                    Where EI.MastercompanyID = " + Session["varMasterCompanyIDForERP"] + @" Order By EmpName1 ";
            }
            else if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 8)
            {
                str = str + " Select Empid,empname from empinfo where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @" And PartyType=0 order by empname";
            }
            else
            {
                str = str + " Select Empid,EmpName + ' (' + EmpCode + ')' EmpName from empinfo where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by empname";
            }

            str = str + @" Select GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId Where GA.UserId=" + Session["varUserId"] + @" and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                           select DepartmentId,DepartmentName from Department order by DepartmentName 
                 Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddempname, ds, 1, true, "Select Employee");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 2, true, "Select Godown");
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDConeType, ds, 5, false, "--Plz Select--");

            if (ddgodown.Items.Count > 0)
            {
                ddgodown.SelectedIndex = 1;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["GateInMasterID"] = 0;
            categorytype_change();
            //Tagno
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            //Department
            switch (variable.Gatepassdeptwise)
            {
                case "1":
                    TDDept.Visible = true;
                    ddempname.Items.Clear();
                    break;
                default:
                    TDDept.Visible = false;
                    break;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "45")
            {
                TDIssQtyFromMWS.Visible = true;
                TDShadeRMStatus.Visible = true;
                TDFolioNo.Visible = true;
            }
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChange();
    }
    private void EmpSelectedChange()
    {
        ViewState["GateInMasterID"] = 0;
        TxtGateInNo.Text = "";
        string Str = @"Select GATEOUTID,cast(ISSUENO as varchar) +' / '+ replace(convert(varchar(11),ISSUEDATE,106),' ','-') as Date 
        From View_GateOutInDetail 
        Where CompanyId=" + ddCompName.SelectedValue + " And PartyID=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";

        if (TDDept.Visible == true)
        {
            Str = Str + " and deptid=" + DDDept.SelectedValue;
        }

        Str = Str + " Group By GATEOUTID,ISSUENO,ISSUEDATE Having Sum(IssQty)>Sum(RecQty) order by GATEOUTID ";

        if (ChKForEdit.Checked == true)
        {
            Str = Str + @" Select Distinct GIM.GateInID,cast(GIM.GateInNo as varchar) +' / '+ replace(convert(varchar(11),GIM.GateInDate,106),' ','-') as GateInNo 
            From GateInMaster GIM,GateInDetail GID 
            Where GIM.GateInID=GID.GateInID And CompanyID=" + ddCompName.SelectedValue + " And BranchID=" + DDBranchName.SelectedValue + " And MasterCompanyID=" + Session["varMasterCompanyIDForERP"] + @" And 
            PartyId=" + ddempname.SelectedValue + "";
            if (TDDept.Visible == true)
            {
                Str = Str + "  and GIM.Deptid=" + DDDept.SelectedValue;
            }
            Str = Str + " order by GIM.GateInID";
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDGatePassNo, Ds, 0, true, "-Select Gate Pass-");
        if (ChKForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDGateInNo, Ds, 1, true, "-Select Gate In No-");
        }
    }
    protected void ddlcatagorytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        categorytype_change();
    }
    private void categorytype_change()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select Distinct CATEGORY_ID,CATEGORY_NAME  From Item_Category_Master ICM,CategorySeparate CS Where ICM.CATEGORY_ID=CS.CategoryID And ICM.MasterCompanyID=" + Session["varMasterCompanyIDForERP"] + " And CS.ID=" + ddlcatagorytype.SelectedValue + " order by ICM.CATEGORY_NAME", true, "-Select Category-");
        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
            ddlcategorycange();
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        dditemname.Items.Clear();
        dquality.Items.Clear();
        dddesign.Items.Clear();
        ddcolor.Items.Clear();
        ddshape.Items.Clear();
        ddsize.Items.Clear();
        ddlshade.Items.Clear();
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME FROM ITEM_CATEGORY_PARAMETERS IPM inner join 
                          PARAMETER_MASTER PM on IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + @" And 
                          PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                          Select Distinct ITEM_ID,ITEM_NAME From Item_Master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME";

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
                        ChkForMtr.Checked = false;
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFillWithDS(ref dditemname, ds, 1, true, "--Select Item--");
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_combo();
    }
    private void fill_combo()
    {
        UtilityModule.ConditionalComboFill(ref DDunit, "select Distinct U.UnitId,U.UnitName from Unit U inner join UNIT_TYPE_MASTER UT on U.UnitTypeID=UT.UnitTypeID inner join Item_master IM on Im.UnitTypeID=UT.UnitTypeID and Im.item_id=" + dditemname.SelectedValue + " order by unitname", true, "select unit");
        if (DDunit.Items.Count > 0)
        {
            DDunit.SelectedIndex = 1;
        }
        if (dquality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref dquality, @"Select Distinct QualityId,QualityName FROM Quality Where Item_Id=" + dditemname.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order by QualityName", true, "-Select Quality-");
        }
        if (dddesign.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref dddesign, @"Select DesignID,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by DesignName", true, "-Select Design-");
        }
        if (ddcolor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"Select ColorID,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ColorName", true, "-Select Colour-");
        }
        if (ddshape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"Select ShapeID,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "-Select Shape-");
        }
        if (ddlshade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShadeColorName", true, "-Select Shade Colour-");
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    private void FillSize()
    {
        string Str = "Select SizeId,SizeFt";
        string StrNew = "SizeFt";
        if (ChkForMtr.Checked == true)
        {
            Str = "Select SizeId,SizeMtr";
            StrNew = "SizeMtr";
        }
        Str = Str + " From Size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And ShapeId=" + ddshape.SelectedValue + " Order by " + StrNew;
        UtilityModule.ConditionalComboFill(ref ddsize, Str, true, "-Select Size-");
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,ShadecolorId from v_finisheditemdetail v Inner join stock s On v.ITEM_FINISHED_ID=s.ITEM_FINISHED_ID where ProductCode=" + TxtProdCode.Text + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            ddlcategorycange();
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            fill_combo();
            if (dquality.Visible == true)
            {
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                fill_combo();
            }
            if (dddesign.Visible == true)
            {
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
            }
            if (ddcolor.Visible == true)
            {
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (ddshape.Visible == true)
            {
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
            }
            if (ddsize.Visible == true)
            {
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            if (ddlshade.Visible == true)
            {
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Pls enter correct product code....');", true);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[31];
            arr[0] = new SqlParameter("@GateInID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@PartyID", SqlDbType.Int);
            arr[3] = new SqlParameter("@GateInNo", SqlDbType.NVarChar, 50);
            arr[4] = new SqlParameter("@GateInDate", SqlDbType.SmallDateTime);
            arr[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[6] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[7] = new SqlParameter("@GateInDetailID", SqlDbType.Int);
            arr[8] = new SqlParameter("@FinishedID", SqlDbType.Int);
            arr[9] = new SqlParameter("@Qty", SqlDbType.Float);
            arr[10] = new SqlParameter("@GodownID", SqlDbType.Int);
            arr[11] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
            arr[12] = new SqlParameter("@CategoryType", SqlDbType.Int);
            arr[13] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
            arr[14] = new SqlParameter("@GateOutID", SqlDbType.Int);
            arr[15] = new SqlParameter("@Unitid", SqlDbType.Int);
            arr[16] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
            arr[17] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[18] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
            arr[19] = new SqlParameter("@DeptId", SqlDbType.Int);
            arr[20] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
            arr[21] = new SqlParameter("@Rate", SqlDbType.Float);
            arr[22] = new SqlParameter("@BranchID", SqlDbType.Int);
            arr[23] = new SqlParameter("@ConeType", SqlDbType.VarChar, 100);
            arr[24] = new SqlParameter("@NoofCone", SqlDbType.Int);
            arr[25] = new SqlParameter("@BellWeight", SqlDbType.Float);
            arr[26] = new SqlParameter("@GateInType", SqlDbType.Int);
            arr[27] = new SqlParameter("@OrderId", SqlDbType.Int);
            arr[28] = new SqlParameter("@IssQtyFromMWS", SqlDbType.Float);
            arr[29] = new SqlParameter("@ShadeStatus", SqlDbType.VarChar, 100);
            arr[30] = new SqlParameter("@FolioNo", SqlDbType.VarChar, 100);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["GateInMasterID"];
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = ddempname.SelectedValue;
            arr[3].Direction = ParameterDirection.InputOutput;
            arr[3].Value = TxtGateInNo.Text;
            arr[4].Value = txtdate.Text;
            arr[5].Value = Session["varMasterCompanyIDForERP"];
            arr[6].Value = Session["varuserid"];
            arr[7].Direction = ParameterDirection.InputOutput;
            if (btnsave.Text == "Update")
            {
                arr[7].Value = gvdetail.SelectedDataKey.Value;
            }
            else
            {
                arr[7].Value = 0;
            }
            arr[8].Value = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            arr[9].Value = TxtQty.Text;
            arr[10].Value = ddgodown.SelectedValue;
            arr[11].Value = TxtLotNo.Text == "" ? "Without Lot No" : TxtLotNo.Text;
            arr[12].Value = ddlcatagorytype.SelectedValue;
            arr[13].Value = txtremarks.Text;
            arr[14].Value = 0;
            arr[15].Value = DDunit.SelectedValue;
            arr[16].Value = txtchallanNo.Text;
            arr[17].Direction = ParameterDirection.Output;
            arr[18].Value = TDTagNo.Visible == true ? (txtTagno.Text == "" ? "Without Tag No" : txtTagno.Text) : "Without Tag No";
            arr[19].Value = TDDept.Visible == false ? "0" : DDDept.SelectedValue;
            arr[20].Value = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
            arr[21].Value = txtRate.Text == "" ? "0" : txtRate.Text;
            arr[22].Value = DDBranchName.SelectedValue;
            arr[23].Value = DDConeType.SelectedItem.Text;
            arr[24].Value = TxtNoofCone.Text == "" ? "0" : TxtNoofCone.Text;
            arr[25].Value = TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text;
            arr[26].Value = 0; ////1 for OrderWise 0 for Gatein form wise
            arr[27].Value = 0;
            if (TDIssQtyFromMWS.Visible == true)
            {
                arr[28].Value = TxtIssQtyFromMWS.Text;
            }
            else
            {
                arr[28].Value = 0;
            }
            if (TDShadeRMStatus .Visible == true)
            {
                arr[29].Value = TxtShadeRMStatus.Text;
            }
            else
            {
                arr[29].Value = TxtShadeRMStatus.Text;
            }
            if (TDFolioNo.Visible == true)
            {
                arr[30].Value = TxtFolioNo.Text;
            }
            else
            {
                arr[30].Value = 0;
            }

            if (DDGatePassNo.Items.Count > 0)
            {
                if (DDGatePassNo.SelectedIndex > 0)
                {
                    arr[14].Value = DDGatePassNo.SelectedValue;
                }
            }

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_GateIn]", arr);

            if (arr[17].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('" + arr[17].Value.ToString() + "');", true);
            }

            ViewState["GateInMasterID"] = arr[0].Value;

            tran.Commit();
            btnsave.Text = "Save";
            TxtQty.Text = "";
            TxtIssueQty.Text = "";
            TxtNoofCone.Text = "";
            TxtBellWeight.Text = "";
            TxtGateInNo.Text = Convert.ToString(arr[3].Value);
            txtremarks.Text = "";
            txtRate.Text = "";
            if (shd.Visible == true)
            {
                ddlshade.SelectedIndex = 0;
            }
            else if (ddsize.Visible == true)
            {
                ddsize.SelectedIndex = 0;
            }
            else if (ddshape.Visible == true)
            {
                ddshape.SelectedIndex = 0;
            }
            else if (ddcolor.Visible == true)
            {
                ddcolor.SelectedIndex = 0;
            }
            else if (dddesign.Visible == true)
            {
                dddesign.SelectedIndex = 0;
            }
            else if (dquality.Visible == true)
            {
                dquality.SelectedIndex = 0;
            }
            fill_grid();
            TxtLotNo.Enabled = true;
            txtTagno.Enabled = true;
            if (DDGatePassNo.SelectedIndex > 0)
            {
                Fill_ShowData();
            }
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
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select GateInDetailID,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt as description,
        GodownName,lotNo,QTY as Qty,Remark,GID.TagNo,GID.Rate From GateInMaster GIM,GateInDetail GID,V_finisheditemdetail VF,GodownMaster GM 
        Where GIM.GateInID=GID.GateInID And GID.FINISHEDID=VF.ITEM_FINISHED_ID And GM.GoDownID=GID.GoDownID And GIM.GateInID=" + ViewState["GateInMasterID"]);
        gvdetail.DataSource = ds;
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
        e.Row.Cells[GetGridColumnId("TagNo")].Visible = false;
        if (MySession.TagNowise == "1")
        {
            e.Row.Cells[GetGridColumnId("TagNo")].Visible = true;
        }

    }
    protected int GetGridColumnId(string ColName)
    {
        int columnid = -1;
        foreach (DataControlField col in gvdetail.Columns)
        {
            if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
            {
                columnid = gvdetail.Columns.IndexOf(col);
                break;
            }
        }
        return columnid;
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
            arr[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = "GateInDetail";
            arr[2].Value = gvdetail.Rows.Count;
            arr[3].Direction = ParameterDirection.Output;
            arr[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockAndDeleteGateIn", arr);
            tran.Commit();
            if (arr[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[4].Value.ToString() + "')", true);
            }
            else
            {

                if (gvdetail.Rows.Count == 1)
                {

                    ViewState["GateInMasterID"] = 0;
                    TxtGateInNo.Text = "";
                    ddgodown.SelectedIndex = 0;
                }
                fill_grid();
                LblErrorMessage.Text = "";

            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void gvdetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDGatePassNo.Items.Clear();
        TxtGateInNo.Text = "";
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Td3.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            Td3.Visible = true;
            EmpSelectedChange();
        }
    }
    protected void DDGatePassNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDGatePassNo.SelectedIndex > 0)
        {
            ddCatagory.Enabled = false;
            dditemname.Enabled = false;
            dquality.Enabled = false;
            dddesign.Enabled = false;
            ddcolor.Enabled = false;
            ddshape.Enabled = false;
            ddsize.Enabled = false;
            ddlshade.Enabled = false;
            TxtLotNo.Enabled = false;
            txtTagno.Enabled = false;
        }
        else
        {
            ddCatagory.Enabled = true;
            dditemname.Enabled = true;
            dquality.Enabled = true;
            dddesign.Enabled = true;
            ddcolor.Enabled = true;
            ddshape.Enabled = true;
            ddsize.Enabled = true;
            ddlshade.Enabled = true;
            TxtLotNo.Enabled = true;
            txtTagno.Enabled = true;
        }
        Fill_ShowData();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        if (MySession.TagNowise == "1")
        {
            if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 45)
            {
                Session["ReportPath"] = "Reports/RptGateInTagNoWiseMWS.rpt";
            }
            else
            {
                Session["ReportPath"] = "Reports/RptGateInTagNoWise.rpt";
            }
        }
        else
        {
            Session["ReportPath"] = "Reports/RptGateIn.rpt";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT c.CompanyName,BM.BranchAddress CompAddr1,'' CompAddr2,'' CompAddr3,
        c.CompFax,BM.PhoneNo CompTel,e.EmpName,e.Address,e.PhoneNo,e.Mobile,e.Fax,GIM.GateInNo,GIM.GateInDate,LotNo,Qty,GID.Remark,GM.GodownName,
        CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt as Description,
        ITEM_FINISHED_ID,gm.GoDownID,GIM.ChallanNo,GID.TagNo,DP.DepartmentName,BM.GSTNo GSTNO,e.GSTNO as EmpGstno,GID.Rate,isnull(NU.UserName,'') UserName, 
        GIM.MasterCompanyID, GID.IssQtyFromOther, GID.ShadeStatus, GID.FolioNo 
        FROM GateInMaster GIM(NoLock) 
        JOIN BranchMaster BM(NoLock) ON BM.ID = GIM.BranchID 
        INNER JOIN COMPANYINFO C(NoLock) ON C.CompanyId=GIM.COMPANYID 
        inner join empinfo e(NoLock) on e.empid=GIM.partyid 
        inner join GateInDetail GID(NoLock) On GIM.GateInID=GID.GateInID 
        Inner join GodownMaster GM(NoLock) On GM.GoDownID=GID.GoDownID 
        inner join V_finisheditemdetail vd(NoLock) On vd.item_finished_id=GID.finishedid 
        left join Department DP(NoLock) on GIM.DeptId=DP.DepartmentId 
        JOIN NewUserDetail NU(NoLock) ON GIM.UserID=NU.UserID
        Where GIM.GateInID=" + ViewState["GateInMasterID"]);

        Session["dsFileName"] = "~\\ReportSchema\\RptGateIn.xsd";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
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
    protected void DGShowData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //    e.Row.CssClass = "header";
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //          e.Row.RowState == DataControlRowState.Normal)
        //    e.Row.CssClass = "normal";
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //          e.Row.RowState == DataControlRowState.Alternate)
        //    e.Row.CssClass = "alternate";
    }
    protected void DGShowData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGShowData, "Select$" + e.Row.RowIndex);
        }
    }
    private void Fill_ShowData()
    {
        TDIssueQty.Visible = false;
        DivDGShowData.Visible = false;
        if (DDGatePassNo.SelectedIndex > 0)
        {
            TDIssueQty.Visible = true;
            DivDGShowData.Visible = true;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select ITEM_FINISHED_ID Finishedid,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeFt+' '+ShadeColorName As Description,Sum(IssQty) IssQty,Sum(RecQty) RecQty,VGD.Lotno,VGD.TagNo,VGD.unitid 
        From View_GateOutInDetail VGD,V_FinishedItemDetail VF Where VGD.Finishedid=VF.ITEM_FINISHED_ID  And GATEOUTID=" + DDGatePassNo.SelectedValue + @" Group By ITEM_FINISHED_ID,ITEM_NAME,QualityName,designName,ColorName,
        ShapeName,SizeFt,ShadeColorName,VGD.Lotno,VGD.TagNo,VGD.unitid");
        DGShowData.DataSource = ds;
        DGShowData.DataBind();
    }
    protected void DGShowData_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtIssueQty.Text = "0";
        GridViewRow row = DGShowData.SelectedRow;
        string lotno = ((Label)row.FindControl("lbllotno")).Text;
        string tagno = ((Label)row.FindControl("lbltagno")).Text;
        string unitid = ((Label)row.FindControl("lblunitid")).Text;


        string Str = @"Select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,ShadecolorId From V_FinishedItemDetail Where ITEM_FINISHED_ID=" + DGShowData.SelectedDataKey.Value + @"
                     Select Sum(IssQty)-Sum(RecQty) Qty From View_GateOutInDetail Where GateOutID=" + DDGatePassNo.SelectedValue + " And Finishedid=" + DGShowData.SelectedDataKey.Value + " and Lotno='" + lotno + "' and tagNo='" + tagno + "' and unitid=" + unitid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            ddlcategorycange();
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            fill_combo();
            if (dquality.Visible == true)
            {
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            }
            if (dddesign.Visible == true)
            {
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
            }
            if (ddcolor.Visible == true)
            {
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (ddshape.Visible == true)
            {
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                FillSize();
            }
            if (ddsize.Visible == true)
            {
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            if (ddlshade.Visible == true)
            {
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                TxtIssueQty.Text = ds.Tables[1].Rows[0]["Qty"].ToString();
            }
            if (DDunit.Items.FindByValue(unitid) != null)
            {
                DDunit.SelectedValue = unitid;
            }
            TxtLotNo.Enabled = false;
            txtTagno.Enabled = false;
            TxtLotNo.Text = lotno;
            txtTagno.Text = tagno;
        }
    }
    protected void DDGateInNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GateInNoSelectedChanged();
    }
    private void GateInNoSelectedChanged()
    {
        ViewState["GateInMasterID"] = DDGateInNo.SelectedValue;
        string st = DDGateInNo.SelectedItem.Text;
        string[] tt = st.Split('/');
        TxtGateInNo.Text = tt[0].ToString();
        txtdate.Text = tt[1].ToString();
        fill_grid();
    }
    protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Empid,EmpName + ' (' + EmpCode + ')' EmpName  from empinfo where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + "";
        if (Session["varMasterCompanyIDForERP"].ToString() == "8")
        {
            str = str + "  and PartyType=0";
        }
        if (TDDept.Visible == true)
        {
            str = str + " and Departmentid=" + DDDept.SelectedValue;
        }
        str = str + " order by empname";
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Plz Select--");
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(ddgodown.SelectedValue), Varfinishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BInId,BInNo From Binmaster Where GODOWNID=" + ddgodown.SelectedValue + " order by BinNo", true, "--Plz Select--");
            }
        }
    }
}