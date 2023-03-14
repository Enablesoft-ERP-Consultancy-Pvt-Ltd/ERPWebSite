using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_FrmGatePass : System.Web.UI.Page
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
                    select Empid,EmpName + ' (' + EmpCode + ')' EmpName from empinfo where mastercompanyid=" + Session["varCompanyId"] + @" order by empname
                    select DepartmentId,DepartmentName from Department order by DepartmentName
                    select val,Type from SizeType Order by val 
                 Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @"
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
            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDConeType, ds, 5, false, "--Plz Select--");

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["GATEOUT"] = 0;
            categorytype_change();
            //TagNo
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
            //*************Bin No
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            //*************
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChange();
    }
    private void EmpSelectedChange()
    {
        ViewState["GATEOUT"] = 0;
        txtchalanno.Text = "";
        if (ChKForEdit.Checked == true)
        {
            string str = @"Select GATEOUTID,cast(ISSUENO as varchar) +' / '+ cast(GATEOUTID as nvarchar),replace(convert(varchar(11),ISSUEDATE,106),' ','-') as date 
            From GateOutMaster Where CompanyID = " + ddCompName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + " And PARTYID=" + ddempname.SelectedValue;
            if (TDDept.Visible == true)
            {
                str = str + " and    Deptid=" + DDDept.SelectedValue;
            }
            str = str + " order by Gateoutid";
            UtilityModule.ConditionalComboFill(ref DDGatePassNo, str, true, "-Select Issue No-");
        }
    }
    protected void ddlcatagorytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        categorytype_change();
    }
    private void categorytype_change()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"select Distinct ic.CATEGORY_ID,ic.CATEGORY_NAME FROM dbo.V_finisheditemdetail im INNER JOIN dbo.Stock s ON im.ITEM_FINISHED_ID = s.ITEM_FINISHED_ID  inner join  
        ITEM_CATEGORY_MASTER ic On im.CATEGORY_ID=ic.CATEGORY_ID inner join CategorySeparate cs ON cs.Categoryid = ic.CATEGORY_ID Where ic.mastercompanyid=" + Session["varCompanyId"] + " and cs.id =" + ddlcatagorytype.SelectedValue + " order by CATEGORY_NAME", true, "-Select Category-");
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
            SqlParameter[] arr = new SqlParameter[32];
            arr[0] = new SqlParameter("@GATEOUTID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ISSUENO", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@PARTYID", SqlDbType.Int);
            arr[3] = new SqlParameter("@ISSUEDATE", SqlDbType.SmallDateTime);
            arr[4] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
            arr[5] = new SqlParameter("@GATEOUTDETAILID", SqlDbType.Int);
            arr[6] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
            arr[7] = new SqlParameter("@ISSUEQTY", SqlDbType.Float);
            arr[8] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
            arr[9] = new SqlParameter("@GODOWNID", SqlDbType.Int);
            arr[10] = new SqlParameter("@CategoryType", SqlDbType.Int);
            arr[11] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
            arr[12] = new SqlParameter("@VarUserId", SqlDbType.Int);
            arr[13] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[14] = new SqlParameter("@flagsize", SqlDbType.Int);
            arr[15] = new SqlParameter("@UnitId", SqlDbType.Int);
            arr[16] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);
            arr[17] = new SqlParameter("@Userid", SqlDbType.Int);
            arr[18] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[19] = new SqlParameter("@DeptId", SqlDbType.Int);
            arr[20] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
            arr[21] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 15);
            arr[22] = new SqlParameter("@BranchID", SqlDbType.Int);
            arr[23] = new SqlParameter("@GateOutType", SqlDbType.Int);
            arr[24] = new SqlParameter("@Rate", SqlDbType.Float);
            arr[25] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 100);
            arr[26] = new SqlParameter("@DriverName", SqlDbType.VarChar, 100);
            arr[27] = new SqlParameter("@GstType", SqlDbType.Int);
            arr[28] = new SqlParameter("@OrderId", SqlDbType.Int);
            arr[29] = new SqlParameter("@ConeType", SqlDbType.VarChar, 100);
            arr[30] = new SqlParameter("@NoofCone", SqlDbType.Int);
            arr[31] = new SqlParameter("@BellWeight", SqlDbType.Float);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = ViewState["GATEOUT"];
            arr[1].Direction = ParameterDirection.InputOutput;
            arr[1].Value = txtchalanno.Text;
            arr[2].Value = ddempname.SelectedValue;
            arr[3].Value = txtdate.Text;
            arr[4].Value = Session["varCompanyId"];
            arr[5].Direction = ParameterDirection.InputOutput;
            if (btnsave.Text == "Update")
            {
                arr[5].Value = gvdetail.SelectedDataKey.Value;
            }
            else
            {
                arr[5].Value = 0;
            }
            arr[6].Value = ViewState["finishedid"];
            arr[7].Value = txtissqty.Text;
            arr[8].Value = ddlotno.SelectedItem.Text;
            arr[9].Value = ddgodown.SelectedValue;
            arr[10].Value = ddlcatagorytype.SelectedValue;
            arr[11].Value = txtremarks.Text;
            arr[12].Value = Session["varuserid"];
            arr[13].Value = ddCompName.SelectedValue;
            arr[14].Value = DDsizetype.SelectedIndex < 0 ? "0" : DDsizetype.SelectedValue;
            arr[15].Value = DDunit.SelectedValue;
            arr[16].Value = TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No";
            arr[17].Value = Session["varuserid"];
            arr[18].Direction = ParameterDirection.Output;
            arr[19].Value = TDDept.Visible == false ? "0" : DDDept.SelectedValue;
            arr[20].Value = TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "";
            arr[21].Value = TxtEWayBillNo.Text;
            
            arr[22].Value = DDBranchName.SelectedValue;

            arr[23].Value = 0;  //1 for OrderWise 0 for GateOut form wise
            arr[24].Value = 0;
            arr[25].Value = "";
            arr[26].Value = "";
            arr[27].Value = 0;
            arr[28].Value = 0;
            arr[29].Value = DDConeType.SelectedItem.Text;
            arr[30].Value = TxtNoofCone.Text == ""? "0": TxtNoofCone.Text;
            arr[31].Value = TxtBellWeight.Text == ""? "0": TxtBellWeight.Text;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_GateOut]", arr);
            ViewState["GATEOUT"] = arr[0].Value;
            //  UtilityModule.StockStockTranTableUpdate(Convert.ToInt32(arr[6].Value), Convert.ToInt32(arr[9].Value), Convert.ToInt32(arr[13].Value), Convert.ToString(arr[8].Value), Convert.ToDouble(arr[7].Value), Convert.ToString(arr[3].Value), DateTime.Now.ToString("dd-MMM-yyyy"), "GateOutDetail", Convert.ToInt32(arr[5].Value), tran, 0, false, Convert.ToInt32(ddlcatagorytype.SelectedValue), 0);
            //UtilityModule.StockStockTranTableUpdateNew(Convert.ToInt32(arr[6].Value), Convert.ToInt32(arr[9].Value), Convert.ToInt32(arr[13].Value), Convert.ToString(arr[8].Value), Convert.ToDouble(arr[7].Value), Convert.ToString(arr[3].Value), DateTime.Now.ToString("dd-MMM-yyyy"), "GateOutDetail", Convert.ToInt32(arr[5].Value), tran, 0, false, Convert.ToInt32(ddlcatagorytype.SelectedValue), 0);
            tran.Commit();
            if (arr[18].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[18].Value.ToString() + "');", true);
            }
            btnsave.Text = "Save";
            txtchalanno.Text = Convert.ToString(arr[1].Value);
            txtissqty.Text = "";
            txtremarks.Text = "";
            txtstock.Text = "";
            TxtNoofCone.Text = "";
            TxtBellWeight.Text = "";

            //ddgodown.Items.Clear();
            //ddlotno.Items.Clear();
            //DDTagNo.Items.Clear();
            //if (shd.Visible == true)
            //{
            //    ddlshade.SelectedIndex = 0;
            //}
            //else if (ddsize.Visible == true)
            //{
            //    ddsize.SelectedIndex = 0;
            //}
            //else if (ddshape.Visible == true)
            //{
            //    ddshape.SelectedIndex = 0;
            //}
            //else if (ddcolor.Visible == true)
            //{
            //    ddcolor.SelectedIndex = 0;
            //}
            //else if (dddesign.Visible == true)
            //{
            //    dddesign.SelectedIndex = 0;
            //}
            //else if (dquality.Visible == true)
            //{
            //    dquality.SelectedIndex = 0;
            //}
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
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeFt +'   ' +isnull(u.unitname,'') as description,GodownName,lotno,ISSUEQTY as qty,Remark,ITEM_FINISHED_ID,
        CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,gd.GoDownID,GATEOUTDETAILID,gom.GATEOUTID ,CategoryType,Remark,ISSUENO,ISSUEDATE,ProductCode,gd.flagsize,gd.unitid,Gd.TagNo,gd.BinNo,isnull(gom.EWayBillNo,'') as EWayBillNo
        From Gateoutdetail gd inner join GateOutMaster gom On gom.GATEOUTID=gd.GATEOUTID inner join V_finisheditemdetail vd On gd.FINISHEDID=vd.ITEM_FINISHED_ID Inner join GodownMaster gm On gm.GoDownID=gd.GoDownID left join unit u on u.unitid=gd.unitid
        Where gom.GATEOUTID=" + ViewState["GATEOUT"] + " order by GATEOUTDETAILID");
        gvdetail.DataSource = ds;
        gvdetail.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
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
            SqlParameter[] arr = new SqlParameter[7];
            arr[0] = new SqlParameter("@Prtid", SqlDbType.Int);
            arr[1] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
            arr[2] = new SqlParameter("@Count", SqlDbType.Int);
            arr[3] = new SqlParameter("@FlagDeleteOrNot", SqlDbType.Int);
            arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[5] = new SqlParameter("@userid", Session["varuserid"]);
            arr[6] = new SqlParameter("@mastercompanyid", Session["VarcompanyId"]);

            arr[0].Value = gvdetail.DataKeys[e.RowIndex].Value;
            arr[1].Value = "Gateoutdetail";
            arr[2].Value = gvdetail.Rows.Count;
            arr[3].Direction = ParameterDirection.Output;
            arr[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_UpdateStockGateOut", arr);
            tran.Commit();
            if (arr[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Del", "alert('" + arr[4].Value.ToString() + "');", true);
            }
            if (gvdetail.Rows.Count == 1)
            {

                ViewState["GATEOUT"] = 0;
                txtchalanno.Text = "";
                // ddgodown.SelectedIndex = 0;
            }
            fill_grid();
            LblErrorMessage.Text = "";
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

    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsave.Text = "Update";
        int n = gvdetail.SelectedIndex;
        txtdate.Text = Convert.ToDateTime(((Label)gvdetail.Rows[n].FindControl("ISSUEDATE")).Text).ToString("dd-MMM-yyyy");
        TxtProdCode.Text = ((Label)gvdetail.Rows[n].FindControl("ProductCode")).Text;
        ddlcatagorytype.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("CategoryType")).Text;
        categorytype_change();
        ddCatagory.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("CATEGORY_ID")).Text;
        ddlcategorycange();
        dditemname.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("ITEM_ID")).Text;
        string flagsize = ((Label)gvdetail.Rows[n].FindControl("lblflagsize")).Text;
        item_change();
        DDunit.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("lblunitid")).Text; ;
        if (dquality.Visible == true)
        {
            dquality.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("QualityId")).Text;
            quality_change();
        }
        if (dddesign.Visible == true)
        {
            dddesign.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("designId")).Text;
            fill_design();
        }
        if (ddcolor.Visible == true)
        {
            ddcolor.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("ColorId")).Text;
            colour_change();
        }
        if (ddshape.Visible == true)
        {
            ddshape.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("ShapeId")).Text;
            //shape_change();
            FillShapeSelectedChange();
        }
        if (ddsize.Visible == true)
        {
            DDsizetype.SelectedValue = flagsize;
            ddsize.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("SizeId")).Text;
            size_change();
        }
        if (ddlshade.Visible == true)
        {
            ddlshade.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("ShadecolorId")).Text;
            shade_change();
        }
        if (ddgodown.Items.Count > 0)
        {
            ddgodown.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("GoDownID")).Text;
            godown_change();
        }
        if (ddlotno.Items.Count > 0)
        {
            ddlotno.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("lotno")).Text;
            lot_change();
        }
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("tagno")).Text;
            DDTagNo_SelectedIndexChanged(sender, e);
        }
        if (TDBinNo.Visible == true)
        {
            DDBinNo.SelectedValue = ((Label)gvdetail.Rows[n].FindControl("lblbinno")).Text;
            DDBinNo_SelectedIndexChanged(sender, new EventArgs());
        }
        txtissqty.Text = ((Label)gvdetail.Rows[n].FindControl("qty")).Text;
        if (txtissqty.Text != "" && txtstock.Text != "")
        {
            txtstock.Text = Convert.ToString(Convert.ToDouble(txtissqty.Text) + Convert.ToDouble(txtstock.Text));
        }
        txtremarks.Text = ((Label)gvdetail.Rows[n].FindControl("Remark")).Text;
        TxtEWayBillNo.Text = ((Label)gvdetail.Rows[n].FindControl("lblEWayBillNo")).Text;
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDGatePassNo.Items.Clear();
        txtchalanno.Text = "";
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
        TxtEWayBillNo.Text = "";
        ViewState["GATEOUT"] = DDGatePassNo.SelectedValue;
        string st = DDGatePassNo.SelectedItem.Text;
        string[] tt = st.Split('/');
        txtchalanno.Text = tt[0].ToString();
        fill_grid();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        if (MySession.TagNowise == "1")
        {
            Session["ReportPath"] = "Reports/RptGateOutTagWise.rpt";
        }
        else
        {
            Session["ReportPath"] = "Reports/RptGateOut.rpt";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT c.CompanyName,BM.BranchAddress CompAddr1,'' CompAddr2,'' CompAddr3,
        c.CompFax,BM.PhoneNo CompTel,e.EmpName,e.Address,e.PhoneNo,e.Mobile,e.Fax,gom.ISSUENO,gom.ISSUEDATE,LotNo,ISSUEQTY,god.Remark,god.CategoryType,gm.godownname,
        CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+ case When god.flagsize=0 then SizeFt else case when god.flagsize=1 then sizemtr else sizeinch end end as Description,
        ITEM_FINISHED_ID,gm.GoDownID,U.unitname,god.TagNo," + MySession.TagNowise + @" as TagNoWise,DP.DepartmentName,IsNull(BM.GSTNo, '') GSTIN,isnull(e.gstno,'') as EMPGSTIN,
        isnull(gom.EWayBillNo,'') as EWayBillNo,isnull(vd.hsncode,'') as HSNCode,Isnull(VD.ItemCode,'') as ItemCode ,isnull(NU.Username,'') as UserName,gom.MASTERCOMPANYID 
        FROM GateOutMaster gom 
        JOIN BranchMaster BM ON BM.ID = gom.BranchID 
        INNER JOIN COMPANYINFO C ON c.CompanyId=gom.COMPANYID 
        inner join empinfo e on e.empid=gom.partyid 
        inner join Gateoutdetail god On god.GATEOUTID=gom.GATEOUTID 
        Inner join GodownMaster gm On gm.GoDownID=god.GoDownID 
        inner join V_finisheditemdetail vd On vd.item_finished_id=god.finishedid 
        left join Unit U on god.unitid=u.unitid 
        left join Department DP on GOM.Deptid=DP.DepartmentId 
        JOIN NewUserDetail NU(NoLock) ON GOM.USERID=NU.UserId
        where gom.GATEOUTID=" + ViewState["GATEOUT"]);

        Session["dsFileName"] = "~\\ReportSchema\\RptGateOut.xsd";
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
    protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddempname, "select EmpId,EmpName + ' (' + EmpCode + ')' EmpName From Empinfo Where MasterCompanyid=" + Session["varcompanyno"] + " and Departmentid=" + DDDept.SelectedValue + " order by EmpName", true, "--Plz Select--");

    }
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
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT Top(1) EI.DepartmentID, EI.EmpId FROM EMPINFO EI WHERE EI.BlackList = 0 And EI.EMPCODE='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDDept.Items.FindByValue(ds.Tables[0].Rows[0]["DepartmentID"].ToString()) != null)
            {
                DDDept.SelectedValue = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
                if (sender != null)
                {
                    DDDept_SelectedIndexChanged(sender, new EventArgs());
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
            ddCatagory.Focus();
        }
        else
        {
            DDDept.SelectedIndex = -1;
            ddempname.SelectedIndex = -1;
            txtWeaverIdNoscan.Text = "";
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);
        }
    }
}
