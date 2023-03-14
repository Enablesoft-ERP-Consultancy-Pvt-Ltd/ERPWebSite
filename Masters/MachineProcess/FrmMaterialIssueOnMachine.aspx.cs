using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmMaterialIssueOnMachine : System.Web.UI.Page
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
            str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname 
                    select val,Type from SizeType Order by val
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=1 and MasterCompanyid=" + Session["varcompanyid"] + @" and Process_Name='WEAVING' order by PROCESS_NAME_ID
                    select UnitsId,UnitName from Units order by UnitName
                    select MachineNoId,MachineNoName From MachineNoMaster(Nolock) order by MachineNoName";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 1, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProductionUnit, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMachineNo, ds, 4, true, "--Plz Select--");

            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
            }

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //ViewState["MaterialIssueID"] = 0;
            hnMaterialIssueId.Value = "0";
            categorytype_change();
            //TagNo
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            //*************Bin No
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            //if (variable.Carpetcompany == "1")
            //{
            //    TRempcodescan.Visible = true;
            //}
            //*************
        }
    }
    protected void DDMachineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        MachineNoSelectedChanged();
    }
    private void MachineNoSelectedChanged()
    {
        //ViewState["MaterialIssueID"] = 0;
        hnMaterialIssueId.Value = "0";
        txtchalanno.Text = "";
        if (ChKForEdit.Checked == true)
        {
            string str = @"Select MaterialIssueId,IssueNo from MaterialIssueOnMachineMaster where CompanyId=" + ddCompName.SelectedValue + " and ProcessId=" + DDProcessName.SelectedValue + " and ProductionUnitId=" + DDProductionUnit.SelectedValue + " and MachineNoId=" + DDMachineNo.SelectedValue + "";

            str = str + " order by MaterialIssueId";
            UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "-Select Issue No-");
        }
    }

    private void categorytype_change()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select Distinct ic.CATEGORY_ID,ic.CATEGORY_NAME
                            from ITEM_CATEGORY_MASTER IC inner join CategorySeparate cs ON cs.Categoryid = ic.CATEGORY_ID 
                            Where ic.mastercompanyid=" + Session["varCompanyId"] + " and cs.id =1", true, "-Select Category-");
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
        UtilityModule.ConditionalComboFill(ref dditemname, @"select Distinct im.ITEM_ID,im.ITEM_NAME FROM ITEM_MASTER im Inner join 
            v_finisheditemdetail v On v.ITEM_ID=im.ITEM_ID Inner join 
            stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID 
            where v.CATEGORY_ID=" + ddCatagory.SelectedValue + " and im.MasterCompanyid=" + Session["varCompanyId"] + " order by im.ITEM_NAME", true, "--Select Item--");

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
                        ChkForMtr.Checked = false;
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
        item_change();

    }
    private void item_change()
    {
        UtilityModule.ConditionalComboFill(ref DDunit, "select Distinct U.UnitId,U.UnitName from Unit U inner join UNIT_TYPE_MASTER UT on U.UnitTypeID=UT.UnitTypeID inner join Item_master IM on Im.UnitTypeID=UT.UnitTypeID and Im.item_id=" + dditemname.SelectedValue + " order by unitname", true, "select unit");
        if (DDunit.Items.Count > 0)
        {
            DDunit.SelectedIndex = 1;
        }
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + " and v.item_id=" + dditemname.SelectedValue + " and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        fill_combo();
    }
    private void fill_combo()
    {
        if (dquality.Visible == true && varcombo < 1)
        {
            UtilityModule.ConditionalComboFill(ref dquality, @"select Distinct im.QualityId,im.QualityName FROM Quality im Inner join 
            v_finisheditemdetail v On v.QualityId=im.QualityId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.QualityName", true, "-Select Quality-");
            return;
        }
        if (dddesign.Visible == true && varcombo < 2)
        {
            UtilityModule.ConditionalComboFill(ref dddesign, @"select Distinct im.designId,im.designName FROM Design im Inner join 
                v_finisheditemdetail v On v.designId=im.designId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + "  order by im.designName", true, "-Select Design-");
            return;
        }
        if (ddcolor.Visible == true && varcombo < 3)
        {
            UtilityModule.ConditionalComboFill(ref ddcolor, @"select Distinct im.ColorId,im.ColorName FROM color im Inner join 
                v_finisheditemdetail v On v.ColorId=im.ColorId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.ColorName", true, "-Select Colour-");
            return;
        }
        if (ddshape.Visible == true && varcombo < 4)
        {
            UtilityModule.ConditionalComboFill(ref ddshape, @"select Distinct im.ShapeId,im.ShapeName FROM Shape im Inner join 
                v_finisheditemdetail v On v.ShapeId=im.ShapeId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.ShapeName", true, "-Select Shape-");
            return;
        }
        if (ddsize.Visible == true && varcombo < 5)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, @"select Distinct im.SizeId,im.SizeFt FROM Size im Inner join 
                v_finisheditemdetail v On v.SizeId=im.SizeId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.SizeFt", true, "-Select Size-");
            return;
        }
        if (ddlshade.Visible == true && varcombo < 6)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"select Distinct im.ShadecolorId,im.ShadeColorName FROM ShadeColor im Inner join 
                v_finisheditemdetail v On v.ShadecolorId=im.ShadecolorId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.ShadeColorName", true, "-Select Shade Colour-");
            return;
        }
        if (ddgodown.Visible == true && varcombo < 7)
        {
            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            ////UtilityModule.ConditionalComboFill(ref ddgodown, "select Distinct g.godownid,g.godownname from godownmaster g Inner join stock s On g.godownid=s.Godownid where ITEM_FINISHED_ID=" + varfinishedid + " And S.CompanyId=" + ddCompName.SelectedValue, true, "-Select Godown-");

            UtilityModule.ConditionalComboFill(ref ddgodown, "Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" 
                                                               JOIN Stock S ON GM.GodownID=S.GodownID Where S.CompanyId=" + ddCompName.SelectedValue + " And S.item_finished_id=" + varfinishedid + " And GM.MasterCompanyId=" + Session["varCompanyId"] + @"", true, "-Select Godown-");
            ViewState["finishedid"] = varfinishedid;
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
        fill_design();
    }
    private void fill_design()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.designId=" + dddesign.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
        if (dquality.Visible == true)
        {
            str = str + "and v.QualityId=" + dquality.SelectedValue + "";
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
            str = str + "and v.designId=" + dddesign.SelectedValue + "";
        }
        if (dquality.Visible == true)
        {
            str = str + "and v.QualityId=" + dquality.SelectedValue + "";
        }
        varcombo = 3;
        fill_combo();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
        //shape_change();
    }
    private void shape_change()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.ShapeId=" + ddshape.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
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
        varcombo = 4;
        fill_combo();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        size_change();
    }
    private void size_change()
    {
        str = "where v.CATEGORY_ID=" + ddCatagory.SelectedValue + "  and v.item_id=" + dditemname.SelectedValue + " and v.SizeId=" + ddsize.SelectedValue + "  and im.MasterCompanyid=" + Session["varCompanyId"] + "";
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
        varcombo = 5;
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
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
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
        UtilityModule.ConditionalComboFill(ref ddsize, @"select Distinct im.SizeId,im.SizeMtr FROM Size im Inner join 
                v_finisheditemdetail v On v.SizeId=im.SizeId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im.SizeMtr", true, "-Select Size-");
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,ShadecolorId from v_finisheditemdetail v Inner join stock s On v.ITEM_FINISHED_ID=s.ITEM_FINISHED_ID where ProductCode=" + TxtProdCode.Text + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            ddlcategorycange();
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            item_change();
            if (dquality.Visible == true)
            {
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                quality_change();
            }
            if (dddesign.Visible == true)
            {
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
                fill_design();
            }
            if (ddcolor.Visible == true)
            {
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                colour_change();
            }
            if (ddshape.Visible == true)
            {
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                FillShapeSelectedChange();
                //shape_change();
            }
            if (ddsize.Visible == true)
            {
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                size_change();
            }
            if (ddlshade.Visible == true)
            {
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
                shade_change();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('This Product Code Not Found In Stock......');", true);
        }
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        godown_change();
    }
    private void godown_change()
    {
        if (Convert.ToInt32(ViewState["finishedid"]) > 0)
        {
            if (ChKForEdit.Checked == true)
            {
                UtilityModule.ConditionalComboFill(ref ddlotno, "select distinct lotno,lotno from stock where ITEM_FINISHED_ID=" + ViewState["finishedid"] + "  and Godownid=" + ddgodown.SelectedValue + "  and companyid=" + ddCompName.SelectedValue + "", true, "-Select Lot No-");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddlotno, "select distinct lotno,lotno from stock where ITEM_FINISHED_ID=" + ViewState["finishedid"] + "  and Godownid=" + ddgodown.SelectedValue + "  and companyid=" + ddCompName.SelectedValue + " and Round(Qtyinhand,3)>0", true, "-Select Lot No-");
            }
            if (ddlotno.Items.Count > 0)
            {
                ddlotno.SelectedIndex = 1;
                ddlotno_SelectedIndexChanged(ddlotno, new EventArgs());
            }

        }
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        lot_change();
    }
    private void lot_change(object sender = null)
    {
        string str = "";
        DDTagNo.SelectedIndex = -1;
        if (TDTagNo.Visible == true)
        {
            if (Convert.ToInt32(ViewState["finishedid"]) > 0)
            {
                str = "select distinct TagNo,TagNo from stock where ITEM_FINISHED_ID=" + ViewState["finishedid"] + "  and Godownid=" + ddgodown.SelectedValue + "  and companyid=" + ddCompName.SelectedValue + " and LotNo='" + ddlotno.SelectedValue + "'";
                if (ChKForEdit.Checked == false)
                {
                    str = str + " and Round(Qtyinhand,3)>0";
                }

                UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "-Select Tag No-");
            }
            if (DDTagNo.Items.Count > 0)
            {
                DDTagNo.SelectedIndex = 1;
                DDTagNo_SelectedIndexChanged(DDTagNo, new EventArgs());
                txtissqty.Focus();

            }
        }
        else if (TDBinNo.Visible == true)
        {
            FillBinNo(sender);
        }
        else
        {
            txtstock.Text = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.SelectedValue, Convert.ToInt32(ViewState["finishedid"])).ToString();
            //txtstock.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Qtyinhand,0) from stock where ITEM_FINISHED_ID=" + ViewState["finishedid"] + " and lotno='" + ddlotno.SelectedValue + "'  and Godownid=" + ddgodown.SelectedValue + "  and companyid=" + ddCompName.SelectedValue));
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
            SqlParameter[] arr = new SqlParameter[18];
            arr[0] = new SqlParameter("@MaterialIssueID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
            arr[3] = new SqlParameter("@ProductionUnitId", SqlDbType.Int);
            arr[4] = new SqlParameter("@MachineNoId", SqlDbType.Int);
            arr[5] = new SqlParameter("@ISSUENO", SqlDbType.NVarChar, 50);
            arr[6] = new SqlParameter("@ISSUEDATE", SqlDbType.DateTime);
            arr[7] = new SqlParameter("@UserId", SqlDbType.Int);
            arr[8] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
            arr[9] = new SqlParameter("@MaterialIssueDetailId", SqlDbType.Int);
            arr[10] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
            arr[11] = new SqlParameter("@ISSUEQTY", SqlDbType.Float);
            arr[12] = new SqlParameter("@GODOWNID", SqlDbType.Int);
            arr[13] = new SqlParameter("@LotNo", SqlDbType.VarChar, 50);
            arr[14] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
            arr[15] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
            arr[16] = new SqlParameter("@UnitId", SqlDbType.Int);
            arr[17] = new SqlParameter("@msg", SqlDbType.VarChar, 200);



            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnMaterialIssueId.Value;
            //arr[0].Value = ViewState["MaterialIssueID"];
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDProductionUnit.SelectedValue;
            arr[4].Value = DDMachineNo.SelectedValue;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = txtchalanno.Text;
            arr[6].Value = txtdate.Text;
            arr[7].Value = Session["varuserid"];
            arr[8].Value = Session["varCompanyId"];
            arr[9].Value = 0;
            //if (btnsave.Text == "Update")
            //{
            //    //arr[9].Value = gvdetail.SelectedDataKey.Value;
            //}
            //else
            //{
            //    arr[9].Value = 0;
            //}
            arr[10].Value = ViewState["finishedid"];
            arr[11].Value = txtissqty.Text;
            arr[12].Value = ddgodown.SelectedValue;
            arr[13].Value = ddlotno.SelectedItem.Text;
            arr[14].Value = TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No";
            arr[15].Value = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
            arr[16].Value = DDunit.SelectedValue;
            arr[17].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_SAVEUPDATEMATERIALISSUEONMACHINE]", arr);
            //ViewState["MaterialIssueID"] = arr[0].Value;
            hnMaterialIssueId.Value = arr[0].Value.ToString();
            tran.Commit();
            if (arr[17].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[17].Value.ToString() + "');", true);
            }
            btnsave.Text = "Save";
            txtchalanno.Text = Convert.ToString(arr[5].Value);
            txtissqty.Text = "";
            txtstock.Text = "";
            FillstockQty(Convert.ToInt32(ViewState["finishedid"]));
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
    protected void FillstockQty(int varfinishedid)
    {
        string Lotno, TagNo = "";
        string BinNo = "";
        Lotno = ddlotno.SelectedItem.Text;
        if (TDTagNo.Visible == true)
        {
            TagNo = DDTagNo.SelectedItem.Text;
        }
        else
        {
            TagNo = "Without Tag No";
        }
        if (TDBinNo.Visible == true)
        {
            BinNo = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
        }
        txtstock.Text = Convert.ToString(UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, Lotno, varfinishedid, TagNo, BinNo: BinNo));
    }
    private void fill_grid()
    {
        string str = @"Select VF.CATEGORY_NAME+' '+VF.ITEM_NAME+' '+VF.QualityName+' '+VF.designName+' '+VF.ColorName+' '+VF.ShadeColorName+' '+VF.ShapeName+' '+VF.SizeFt +'   ' +isnull(u.unitname,'') as Itemdescription
                    ,GM.GodownName,MID.LotNo,MID.TagNo,MID.unitId,MID.FinishedId,MID.IssueQty,Replace(CONVERT(nvarchar(11),MI.IssueDate,106),' ','-') as IssueDate,MI.IssueNo,
                    MI.MaterialIssueId,MID.MaterialIssueDetailId,MID.BinNo,MI.ProcessId,MI.ProductionUnitId,MI.MachineNoID
                    from MaterialIssueOnMachineMaster MI JOIN MaterialIssueOnMachineDetail MID ON MI.MaterialIssueId=MID.MaterialIssueId
                    JOIN V_FinishedItemDetail VF ON MID.FinishedId=VF.ITEM_FINISHED_ID
                    JOIN GodownMaster GM ON MID.GodownId=GM.GoDownID
                    LEFT JOIN Unit U ON MID.UnitId=U.UnitID
                    Where MI.MaterialIssueId=" + hnMaterialIssueId.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (ChKForEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                txtdate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();

            }
            else
            {
                txtchalanno.Text = "";
                txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }

        }
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
        //e.Row.Cells[GetGridColumnId("TagNo")].Visible = false;
        //if (MySession.TagNowise == "1")
        //{
        //    e.Row.Cells[GetGridColumnId("TagNo")].Visible = true;
        //}

    }
    //protected int GetGridColumnId(string ColName)
    //{
    //    int columnid = -1;
    //    //foreach (DataControlField col in gvdetail.Columns)
    //    //{
    //    //    if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
    //    //    {
    //    //        columnid = gvdetail.Columns.IndexOf(col);
    //    //        break;
    //    //    }
    //    //}
    //    return columnid;
    //}
    protected void gvdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvdetail.EditIndex = e.NewEditIndex;
        fill_grid();

    }
    protected void gvdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvdetail.EditIndex = -1;
        fill_grid();
    }
    protected void gvdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    Label lblhqty = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblhqty");
        //    Label lblMaterialIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialIssueId");
        //    Label lblMaterialIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialIssueDetailId");           
        //    TextBox txtqty = (TextBox)gvdetail.Rows[e.RowIndex].FindControl("txtqty");
        //    Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");
        //    //**************
        //    SqlParameter[] param = new SqlParameter[8];
        //    param[0] = new SqlParameter("@MaterialIssueID", lblMaterialIssueId.Text);
        //    param[1] = new SqlParameter("@MaterialIssueDetailId", lblMaterialIssueDetailId.Text);           
        //    param[2] = new SqlParameter("@hqty", lblhqty.Text);
        //    param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    param[3].Direction = ParameterDirection.Output;
        //    param[4] = new SqlParameter("@IssueQty", txtqty.Text == "" ? "0" : txtqty.Text);
        //    param[5] = new SqlParameter("@processid", lblprocessid.Text);
        //    param[6] = new SqlParameter("@userid", Session["varuserid"]);
        //    param[7] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
        //    //*************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEMATERIALISSUEONMACHINE", param);
        //    lblmessage.Text = param[3].Value.ToString();
        //    Tran.Commit();
        //    gvdetail.EditIndex = -1;
        //    fill_grid();            
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
            Label lblMaterialIssueId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialIssueId");
            Label lblMaterialIssueDetailId = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblMaterialIssueDetailId");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@MaterialIssueID", lblMaterialIssueId.Text);
            param[1] = new SqlParameter("@MaterialIssueDetailId", lblMaterialIssueDetailId.Text);
            param[2] = new SqlParameter("@Processid", lblprocessid.Text);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMATERIALISSUEONMACHINE", param);
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


        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //SqlTransaction tran = con.BeginTransaction();
        //try
        //{
        //    SqlParameter[] arr = new SqlParameter[7];
        //    arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
        //    arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        //    arr[2] = new SqlParameter("@Count", SqlDbType.Int);
        //    arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
        //    arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    arr[5] = new SqlParameter("@userid", Session["varuserid"]);
        //    arr[6] = new SqlParameter("@mastercompanyid", Session["VarcompanyId"]);

        //    arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
        //    arr[1].Value = "Gateoutdetail";
        //    arr[2].Value = gvdetail.Rows.Count;
        //    arr[3].Direction = ParameterDirection.Output;
        //    arr[4].Direction = ParameterDirection.Output;

        //    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockGateOut", arr);
        //    tran.Commit();
        //    if (arr[4].Value.ToString() != "")
        //    {
        //        ScriptManager.RegisterStartupScript(Page, GetType(), "Del", "alert('" + arr[4].Value.ToString() + "');", true);
        //    }
        //    if (gvdetail.Rows.Count == 1)
        //    {

        //        ViewState["GATEOUT"] = 0;
        //        txtchalanno.Text = "";
        //        // ddgodown.SelectedIndex = 0;
        //    }
        //    fill_grid();
        //    LblErrorMessage.Text = "";
        //}
        //catch (Exception ex)
        //{
        //    LblErrorMessage.Text = ex.Message;
        //    LblErrorMessage.Visible = true;
        //    tran.Rollback();
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDIssueNo.Items.Clear();
        txtchalanno.Text = "";
        txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Td3.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            Td3.Visible = true;

            MachineNoSelectedChanged();
        }
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ViewState["MaterialIssueID"] = DDIssueNo.SelectedValue.ToString();
        hnMaterialIssueId.Value = DDIssueNo.SelectedValue.ToString();
        txtchalanno.Text = DDIssueNo.SelectedItem.Text;

        //string st = DDIssueNo.SelectedItem.Text;
        //string[] tt = st.Split('/');
        //txtchalanno.Text = tt[0].ToString();
        fill_grid();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@MaterialIssueId", hnMaterialIssueId.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MATERIALISSUEONMACHINEREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptMaterialIssueOnMachine.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialIssueOnMachine.xsd";
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
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
    }

    private void FillShapeSelectedChange()
    {
        string Size = "";
        string str = "";
        if (DDsizetype.SelectedValue == "0")
        {
            Size = "Sizeft";
        }
        else if (DDsizetype.SelectedValue == "1")
        {
            Size = "SizeMTR";
        }
        else
        {
            Size = "SizeInch";
        }
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
        UtilityModule.ConditionalComboFill(ref ddsize, @"select Distinct im.SizeId,im." + Size + @" FROM Size im Inner join 
                v_finisheditemdetail v On v.SizeId=im.SizeId Inner join stock s On s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID " + str + " order by im." + Size + "", true, "-Select Size-");

    }

    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            FillBinNo(sender);
        }
        else
        {
            txtstock.Text = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.SelectedValue, Convert.ToInt32(ViewState["finishedid"]), DDTagNo.SelectedValue).ToString();
        }
    }
    //protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref ddempname, "select EmpId,EmpName + ' (' + EmpCode + ')' EmpName From Empinfo Where MasterCompanyid=" + Session["varcompanyno"] + " and Departmentid=" + DDDept.SelectedValue + " order by EmpName", true, "--Plz Select--");

    //}
    protected void FillBinNo(object sender = null)
    {
        DDBinNo.SelectedIndex = -1;
        if (TDBinNo.Visible == true)
        {
            if (Convert.ToInt32(ViewState["finishedid"]) > 0)
            {
                str = "select distinct Binno,Binno from stock where ITEM_FINISHED_ID=" + ViewState["finishedid"] + "  and Godownid=" + ddgodown.SelectedValue + "  and companyid=" + ddCompName.SelectedValue + " and LotNo='" + ddlotno.SelectedValue + "'";
                if (ChKForEdit.Checked == false)
                {
                    str = str + " and Round(Qtyinhand,3)>0";
                }
                if (TDTagNo.Visible == true)
                {
                    str = str + " and Tagno='" + DDTagNo.SelectedValue + "'";
                }
                UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "-Select Tag No-");
            }
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
                if (sender != null)
                {
                    DDBinNo_SelectedIndexChanged(sender, new EventArgs());
                }
                txtissqty.Focus();
            }

        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstock.Text = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.SelectedValue, Convert.ToInt32(ViewState["finishedid"]), DDTagNo.SelectedValue, BinNo: (TDBinNo.Visible == true ? DDBinNo.SelectedValue : "")).ToString();
        txtissqty.Focus();
    }
    //protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    //{
    //    FillProcess_Employee(sender);
    //}
    //protected void FillProcess_Employee(object sender = null)
    //{
    //    string str = @"SELECT Top(1) EI.DepartmentID, EI.EmpId FROM EMPINFO EI WHERE EI.BlackList = 0 And EI.EMPCODE='" + txtWeaverIdNoscan.Text + "'";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        if (DDDept.Items.FindByValue(ds.Tables[0].Rows[0]["DepartmentID"].ToString()) != null)
    //        {
    //            DDDept.SelectedValue = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
    //            if (sender != null)
    //            {
    //                DDDept_SelectedIndexChanged(sender, new EventArgs());
    //            }
    //        }
    //        if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
    //        {
    //            ddempname.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
    //            if (sender != null)
    //            {
    //                ddempname_SelectedIndexChanged(sender, new EventArgs());
    //            }
    //        }
    //        ddCatagory.Focus();
    //    }
    //    else
    //    {
    //        DDDept.SelectedIndex = -1;
    //        ddempname.SelectedIndex = -1;
    //        txtWeaverIdNoscan.Text = "";
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);
    //    }
    //}
}
