using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_frmsampleordernew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                Select Distinct CI.Customerid,CI.Customercode  from CustomerInfo CI where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by customercode
                select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" and PROCESS_NAME='WEAVING'
                select unitid,unitname from unit where unitid in (1,2) order by unitid desc
                select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 2, true, "--Plz Select--");
            if (DDprocess.Items.Count > 0)
            {
                DDprocess.SelectedIndex = 1;
                DDprocess_SelectedIndexChanged(sender, new EventArgs());
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 4, true, "--Plz Select--");
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 1;
                ddCatagory_SelectedIndexChanged(sender, new EventArgs());
            }

            txtassigndate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtreqdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;


        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            //str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            str = "select Designid,Designname From Design Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            // str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            str = "select Colorid,colorname From color Where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapeid";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    //protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //UtilityModule.ConditionalComboFill(ref dddesign, "select Distinct Vf.designid,vf.designname From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and vf.designId>0 order by vf.designname", true, "--Plz Select");
    //}
    //protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref ddcolor, "select Distinct Vf.colorid,vf.ColorName From V_finisheditemdetail vf Where ITEM_ID=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and designId=" + dddesign.SelectedValue + " and vf.ColorId>0 order by colorname", true, "--Plz Select--");
    //}
    protected void Fillsize()
    {

        string str = null, size = null;
        if (variable.VarNewQualitySize == "1")
        {
            switch (DDunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "MtrSize";
                    }
                    else { size = "Production_Mt_Format"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Export_Format";
                    }
                    else { size = "Production_Ft_Format"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From QualitySizeNew S Where S.shapeid=" + ddshape.SelectedValue;
            str = str + " order by S." + size;
        }
        else
        {
            switch (DDunit.SelectedValue.ToString())
            {
                case "2":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
                case "1":
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizemtr";
                    }
                    else { size = "Prodsizemtr"; }
                    break;
                default:
                    if (chkexportsize.Checked == true)
                    {
                        size = "Sizeft";
                    }
                    else { size = "Prodsizeft"; }
                    break;
            }
            #region
            //// str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + "";
            // if (TDQuality.Visible == true && dquality.SelectedIndex > 0)
            // {
            //     str = str + " and vf.QualityId=" + dquality.SelectedValue;
            // }
            // if (TDDesign.Visible == true && dddesign.SelectedIndex > 0)
            // {
            //     str = str + " and vf.designId=" + dddesign.SelectedValue;
            // }
            // if (TDColor.Visible == true && ddcolor.SelectedIndex > 0)
            // {
            //     str = str + " and vf.colorid=" + ddcolor.SelectedValue;
            // }
            #endregion
            str = "select Distinct S.sizeid,S." + size + " From size S Where S.shapeid=" + ddshape.SelectedValue + "  and S.mastercompanyid=" + Session["varMasterCompanyIDForERP"];
            str = str + " order by S." + size;
        }
        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void Area()
    {
        txtarea.Text = "";
        SqlParameter[] _arrpara = new SqlParameter[9]; 
        _arrpara[0] = new SqlParameter("@size_Id", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@UnitTypeId", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@Length", SqlDbType.Float);
        _arrpara[3] = new SqlParameter("@width", SqlDbType.Float);
        _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
        _arrpara[5] = new SqlParameter("@ShapeId", SqlDbType.Int);
        _arrpara[6] = new SqlParameter("@ExportSizeFlag", SqlDbType.Int);        
        _arrpara[7] = new SqlParameter("@KhapLength", SqlDbType.Float);
        _arrpara[8] = new SqlParameter("@Khapwidth", SqlDbType.Float);

        _arrpara[0].Value = ddsize.SelectedValue;
        _arrpara[1].Value = DDunit.SelectedValue;
        _arrpara[2].Direction = ParameterDirection.Output;
        _arrpara[3].Direction = ParameterDirection.Output;
        _arrpara[4].Direction = ParameterDirection.Output;
        _arrpara[5].Direction = ParameterDirection.Output;
        _arrpara[6].Value = chkexportsize.Checked == true ? 1 : 0;
        _arrpara[7].Direction = ParameterDirection.Output;
        _arrpara[8].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_AreaNewSample", _arrpara);
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            switch (DDunit.SelectedValue)
            {
                case "2": //ft
                    txtlength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
                    txtwidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
                    txtKhapWidth.Text = string.Format("{0:#0.00}", _arrpara[7].Value);
                    txtKhapLength.Text = string.Format("{0:#0.00}", _arrpara[8].Value);
                    break;
                default:
                    txtlength.Text = _arrpara[2].Value.ToString();
                    txtwidth.Text = _arrpara[3].Value.ToString();
                    txtKhapWidth.Text = _arrpara[7].Value.ToString();
                    txtKhapLength.Text = _arrpara[8].Value.ToString();
                    break;
            }
        }
        else
        {
            txtlength.Text = string.Format("{0:#0.00}", _arrpara[2].Value);
            txtwidth.Text = string.Format("{0:#0.00}", _arrpara[3].Value);
            txtKhapWidth.Text = _arrpara[7].Value.ToString();
            txtKhapLength.Text = _arrpara[8].Value.ToString(); 

        }
        txtarea.Text = string.Format("{0:#0.0000}", _arrpara[4].Value);
        if (Convert.ToInt32(DDunit.SelectedValue) == 1)
        {
            txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ddshape.SelectedValue)));
        }
        if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
        {

            txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ddshape.SelectedValue), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: 0.7853));
        }
    }
    private void MASTER_RATECOMMWOOL()
    {
        SqlParameter[] _arrpara = new SqlParameter[9];
        _arrpara[0] = new SqlParameter("@QualityId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@DesignId", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@ColorId", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@UnitId", SqlDbType.Int);
        _arrpara[4] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);

        _arrpara[0].Value = dquality.SelectedValue;
        _arrpara[1].Value = dddesign.SelectedValue;
        _arrpara[2].Value = ddcolor.SelectedValue;
        _arrpara[3].Value = DDunit.SelectedValue;
        _arrpara[4].Value = Session["varMasterCompanyIDForERP"];

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_CONSUMPTION_DEFINE_QUALITYDETAILWISE_BAZAARTABLE_SAMPLE", _arrpara);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (chkedit.Checked == false)
                {
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "WOOL")
                    {
                        txtWool.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtWool.Text = "";
                    //}
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "COTTON")
                    {
                        txtCotton.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtCotton.Text = "";
                    //}
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "TAR")
                    {
                        txtTar.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtTar.Text = "";
                    //}
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "THARRI")
                    {
                        txtTharri.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtTharri.Text = "";
                    //}
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "MISC")
                    {
                        txtMisc.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtMisc.Text = "";
                    //}
                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "EXTRA")
                    {
                        txtExtra.Text = Ds.Tables[0].Rows[i]["Qty"].ToString();
                    }
                    //else
                    //{
                    //    txtExtra.Text = "";
                    //}

                    if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "Loss")
                    {
                        txtLoss.Text = string.Format("{0:#0.00}", Convert.ToDouble(Ds.Tables[0].Rows[i]["Qty"]));
                    }
                    //else
                    //{
                    //    txtLoss.Text = "";
                    //}
                }

                if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "Rate")
                {                    
                    txtrate.Text = string.Format("{0:#0.00}", Convert.ToDouble(Ds.Tables[0].Rows[i]["Qty"]));                    
                }
                if (Ds.Tables[0].Rows[i]["ItemName"].ToString() == "Comm")
                {
                    txtcomm.Text = string.Format("{0:#0.00}", Convert.ToDouble(Ds.Tables[0].Rows[i]["Qty"]));
                }
               
            }           
            
        }
    }
    
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Area();
        
            MASTER_RATECOMMWOOL();
        
    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (txtlength.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlength.Text));
                txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtlength.Text = "";
                    txtlength.Focus();
                }
            }
        }
        if (txtwidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidth.Text));
                txtwidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtwidth.Text = "";
                    txtwidth.Focus();
                }
            }
        }
        if (txtlength.Text != "" && txtwidth.Text != "")
        {
            int Shape = 0;

            Shape = Convert.ToInt32(ddshape.SelectedValue);

            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            }
        }
    }
    private void Check_Length_Width_Format2()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (txtChangeLength.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtChangeLength.Text));
                txtChangeLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtChangeLength.Text = "";
                    //txtChangeLength.Focus();
                }
            }
        }
        if (txtChangeWidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtChangeWidth.Text));
                txtChangeWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    txtChangeWidth.Text = "";
                    //txtwidth.Focus();
                }
            }
        }
        if (txtChangeLength.Text != "" && txtChangeWidth.Text != "")
        {
            int Shape = 0;

            Shape = Convert.ToInt32(ddshape.SelectedValue);

            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtChangeLength.Text), Convert.ToDouble(txtChangeWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtChangeLength.Text), Convert.ToDouble(txtChangeWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            }
        }
    }
    protected void AfterKhapSize()
    {
        double ActWF, ActWI, ActLF, ActLI, AWIDTH, ALENGTH;
            double W, L;
        string Str, AfterKhapSize2 = "";

            if(DDunit.SelectedValue=="2")
            {
                        Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidth.Text));
                            txtwidth.Text = Str;
                            ActWF =Convert.ToInt32(Str.Split('.')[0]);
                            ActWI = Convert.ToInt32(Str.Split('.')[1]);

                        Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlength.Text));
                            txtlength.Text = Str;
                            ActLF = Convert.ToInt32(Str.Split('.')[0]);
                            ActLI = Convert.ToInt32(Str.Split('.')[1]);

                        AWIDTH = (ActWF * 12 + ActWI);
                        ALENGTH = (ActLF * 12 + ActLI);
                        W = Convert.ToDouble(AWIDTH - Convert.ToDouble(txtKhapWidth.Text));
                        L = Convert.ToDouble(ALENGTH - Convert.ToDouble(txtKhapLength.Text));
            }
            else
            {
                AWIDTH = Convert.ToDouble(txtwidth.Text);
                ALENGTH = Convert.ToDouble(txtlength.Text);
                    W = Convert.ToDouble(AWIDTH - Math.Round(Convert.ToDouble(txtKhapWidth.Text) * 2.54));
                    L = Convert.ToDouble(ALENGTH - Math.Round(Convert.ToDouble(txtKhapLength.Text) * 2.54));
            }

            if (DDunit.SelectedValue == "2")
            {
                W =UtilityModule.ConvertInchesToFt(Convert.ToInt32(W));
                L = UtilityModule.ConvertInchesToFt(Convert.ToInt32(L));
                AfterKhapSize2 = (string.Format("{0:#0.00}",W) + 'x' + string.Format("{0:#0.00}",L)).ToString();
                txtAfterKhapSize.Text = (string.Format("{0:#0.00}", W) + 'x' + string.Format("{0:#0.00}", L)).ToString();
                txtChangeWidth.Text=(string.Format("{0:#0.00}", W));
                txtChangeLength.Text = (string.Format("{0:#0.00}", L));
            }
            else
            {
                AfterKhapSize2 = Convert.ToString(W + 'x' + L);
                txtAfterKhapSize.Text = Convert.ToString(W + 'x' + L);
                 txtChangeWidth.Text=(string.Format("{0:#0.00}", W));
                txtChangeLength.Text = (string.Format("{0:#0.00}", L));
            }

            Check_Length_Width_Format2();
       
    }
    protected void UpdateWidthLength(object sender)
    {       
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;

        Label lblissueorderid = (Label)gvRow.FindControl("lblissueorderid");     

        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblShapeId = (Label)gvRow.FindControl("lblShapeId"); 

        TextBox txtwidthedit = (TextBox)gvRow.FindControl("txtwidthedit");
        TextBox txtlengthedit = (TextBox)gvRow.FindControl("txtlengthedit");
        TextBox txtKhapwidthedit = (TextBox)gvRow.FindControl("txtKhapwidthedit");
        TextBox txtKhaplengthedit = (TextBox)gvRow.FindControl("txtKhaplengthedit");
        Label lblAfterKhapSizeOrder = (Label)gvRow.FindControl("lblAfterKhapSizeOrder");


        double ActWF, ActWI, ActLF, ActLI, AWIDTH, ALENGTH;
        double W, L;
        string Str2, AfterKhapSize2, ChangeWidth, ChangeLength,AfterKhapSizeOrder = "";

        if (DDunit.SelectedValue == "2")
        {
            Str2 = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthedit.Text));
            txtwidthedit.Text = Str2;
            ActWF = Convert.ToInt32(Str2.Split('.')[0]);
            ActWI = Convert.ToInt32(Str2.Split('.')[1]);

            Str2 = string.Format("{0:#0.00}", Convert.ToDouble(txtlengthedit.Text));
            txtlengthedit.Text = Str2;
            ActLF = Convert.ToInt32(Str2.Split('.')[0]);
            ActLI = Convert.ToInt32(Str2.Split('.')[1]);

            AWIDTH = (ActWF * 12 + ActWI);
            ALENGTH = (ActLF * 12 + ActLI);
            W = Convert.ToDouble(AWIDTH - Convert.ToDouble(txtKhapwidthedit.Text));
            L = Convert.ToDouble(ALENGTH - Convert.ToDouble(txtKhaplengthedit.Text));
        }
        else
        {
            AWIDTH = Convert.ToDouble(txtwidthedit.Text);
            ALENGTH = Convert.ToDouble(txtlengthedit.Text);
            W = Convert.ToDouble(AWIDTH - Math.Round(Convert.ToDouble(txtKhapwidthedit.Text) * 2.54));
            L = Convert.ToDouble(ALENGTH - Math.Round(Convert.ToDouble(txtKhaplengthedit.Text) * 2.54));
        }

        if (DDunit.SelectedValue == "2")
        {
            W = UtilityModule.ConvertInchesToFt(Convert.ToInt32(W));
            L = UtilityModule.ConvertInchesToFt(Convert.ToInt32(L));
            AfterKhapSize2 = (string.Format("{0:#0.00}", W) + 'x' + string.Format("{0:#0.00}", L)).ToString();
            lblAfterKhapSizeOrder.Text = (string.Format("{0:#0.00}", W) + 'x' + string.Format("{0:#0.00}", L)).ToString();
            ChangeWidth = (string.Format("{0:#0.00}", W));
            ChangeLength = (string.Format("{0:#0.00}", L));
        }
        else
        {
            AfterKhapSize2 = Convert.ToString(W + 'x' + L);
            lblAfterKhapSizeOrder.Text = Convert.ToString(W + 'x' + L);
            ChangeWidth = (string.Format("{0:#0.00}", W));
            ChangeLength = (string.Format("{0:#0.00}", L));
        }

        //Check_Length_Width_Format2();

        //int issue_detail_id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);

        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (ChangeLength != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(ChangeLength));
               // txtlengthedit.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    ChangeLength = "";
                    //txtlengthedit.Focus();
                }
            }
        }
        if (ChangeWidth != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(ChangeWidth));
                //txtwidthedit.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmsg.Text = "Inch value must be less than 12";
                    ChangeWidth = "";
                    //txtwidthedit.Focus();
                }
            }
        }
        if (ChangeLength != "" && ChangeWidth != "")
        {
            int Shape = 0;

            Shape = Convert.ToInt32(lblShapeId.Text);

            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(ChangeLength), Convert.ToDouble(ChangeWidth), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(ChangeLength), Convert.ToDouble(ChangeWidth), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            }
        }
    }
    
    protected void txtwidth_TextChanged(object sender, EventArgs e)
    {
        //Check_Length_Width_Format();
        AfterKhapSize();
    }
    protected void txtlength_TextChanged(object sender, EventArgs e)
    {
        //Check_Length_Width_Format();
        AfterKhapSize();
    }
    protected void txtKhapWidth_TextChanged(object sender, EventArgs e)
    {
        AfterKhapSize();
        //Check_Length_Width_Format();
    }
    protected void txtKhapLength_TextChanged(object sender, EventArgs e)
    {
        AfterKhapSize();
        //Check_Length_Width_Format();
    }
    protected void txtwidthedit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
    }
    protected void txtlengthedit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
    }
    protected void txtKhapwidthedit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
    }
    protected void txtKhaplengthedit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDvendor, "select EI.EmpId,EI.EmpName  From Empinfo EI inner join EmpProcess EP on EI.empid=EP.EmpId  and EP.ProcessId=" + DDprocess.SelectedValue + " and Ei.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by Ei.EmpName", true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[42];
            param[0] = new SqlParameter("@issueorderid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnissueorderid.Value;
            param[1] = new SqlParameter("@orderid", SqlDbType.Int);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Value = hnorderid.Value;
            param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            param[3] = new SqlParameter("@customerid", DDcustcode.SelectedValue);
            param[4] = new SqlParameter("@Processid", DDprocess.SelectedValue);
            param[5] = new SqlParameter("@empid", DDvendor.SelectedValue);
            param[6] = new SqlParameter("@assigndate", txtassigndate.Text);
            param[7] = new SqlParameter("@Reqdate", txtreqdate.Text);
            param[8] = new SqlParameter("@Unitid", DDunit.SelectedValue);
            param[9] = new SqlParameter("@caltype", DDcaltype.SelectedValue);
            param[10] = new SqlParameter("@Remarks", txtremarks.Text);
            int Item_finished_id = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            param[11] = new SqlParameter("@Item_finished_id", Item_finished_id);
            param[12] = new SqlParameter("@Width", txtwidth.Text);
            param[13] = new SqlParameter("@Length", txtlength.Text);
            param[14] = new SqlParameter("@Area", txtarea.Text);
            param[15] = new SqlParameter("@Rate", txtrate.Text == "" ? "0" : txtrate.Text);
            param[16] = new SqlParameter("@Commrate", txtcomm.Text == "" ? "0" : txtcomm.Text);
            param[17] = new SqlParameter("@Qty", txtpcs.Text);
            param[18] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[18].Direction = ParameterDirection.Output;
            param[19] = new SqlParameter("@userid", Session["varuserid"]);
            param[20] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

            param[21] = new SqlParameter("@Quality", dquality.SelectedItem.Text);
            param[22] = new SqlParameter("@Design", dddesign.SelectedItem.Text);
            param[23] = new SqlParameter("@color", ddcolor.SelectedItem.Text);

            param[24] = new SqlParameter("@QualityId", dquality.SelectedValue);
            param[25] = new SqlParameter("@Designid", dddesign.SelectedValue);
            param[26] = new SqlParameter("@Colorid", ddcolor.SelectedValue);
            param[27] = new SqlParameter("@varcarpetcompany", variable.Carpetcompany);
            param[28] = new SqlParameter("@Itemid", dditemname.SelectedValue);
            param[29] = new SqlParameter("@KhapWidth", txtKhapWidth.Text == "" ? "0" : txtKhapWidth.Text);
            param[30] = new SqlParameter("@KhapLength", txtKhapLength.Text == "" ? "0" : txtKhapLength.Text);
            param[31] = new SqlParameter("@Wool", txtWool.Text == "" ? "0" : txtWool.Text);
            param[32] = new SqlParameter("@CYarn", txtCotton.Text == "" ? "0" : txtCotton.Text);
            param[33] = new SqlParameter("@Tar", txtTar.Text == "" ? "0" : txtTar.Text);
            param[34] = new SqlParameter("@Tharri", txtTharri.Text == "" ? "0" : txtTharri.Text);
            param[35] = new SqlParameter("@Misc", txtMisc.Text == "" ? "0" : txtMisc.Text);
            param[36] = new SqlParameter("@Extra", txtExtra.Text == "" ? "0" : txtExtra.Text);
            param[37] = new SqlParameter("@Loss", txtLoss.Text == "" ? "0" : txtLoss.Text);
            param[38] = new SqlParameter("@Instruction", txtInstruction.Text);
            param[39] = new SqlParameter("@HSCode", txtHSCode.Text);
            param[40] = new SqlParameter("@OldQualityid", SqlDbType.Int);
            param[40].Direction = ParameterDirection.InputOutput;
            param[40].Value = hnQualityId.Value;
            param[41] = new SqlParameter("@AfterKhapSizeOrder", txtAfterKhapSize.Text == "" ? "0" : txtAfterKhapSize.Text);

            //*********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Savesampleordernew", param);
            //**************
            lblmsg.Text = param[18].Value.ToString();
            hnissueorderid.Value = param[0].Value.ToString();
            hnorderid.Value = param[1].Value.ToString();
            txtchallanNo.Text = hnissueorderid.Value;
            hnQualityId.Value = param[40].Value.ToString();
            ViewState["IssueOrderid"] = param[0].Value.ToString();
            ddsize.Focus();
            //**************
            Tran.Commit();
            refreshcontrol();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void refreshcontrol()
    {
        ddsize.SelectedIndex = -1;
        txtwidth.Text = "";
        txtlength.Text = "";
        txtarea.Text = "";
        txtrate.Text = "";
        txtcomm.Text = "";
        txtpcs.Text = "";
        txtKhapWidth.Text = "";
        txtKhapLength.Text = "";
    }
    //protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  //  Fillsize();
    //}
    private void Fill_Grid()
    {
        DGOrderdetail.DataSource = GetDetail();
        DGOrderdetail.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";

        //        sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch  Else  SizeFt End End Description,Length,Width,
        //                        Length + 'x' + Width Size,Qty*Area as Area,Rate,Qty,Amount From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
        //                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
        //                        Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
        //                        PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";

        if (variable.VarNewQualitySize == "1")
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QualityName+' '+IPM.designName+' '+IPM.ColorName+' ' +IPM.ShapeName+ Space(5) +
                    Case When PM.Unitid=1 Then IPM.SizeMtr Else case When PM.unitid=6 Then IPM.Sizeinch  Else  SizeFt End End Description,Length,Width,
                    Length + 'x' + Width Size,Qty*Area as TotalArea,Rate,Qty,Amount,PD.orderid,PD.Item_Finished_Id,Replace(CONVERT(nvarchar(11),PM.assigndate,106),' ','-') as assigndate,Replace(CONVERT(nvarchar(11),PD.ReqByDate,106),' ','-') as Reqbydate,
                    PM.UnitId,PM.CalType,PD.comm,PM.issueorderid,PM.remarks,PD.CommAmt, PD.Amount+PD.CommAmt as TotalAmount,IPM.QualityId,PM.Instruction,PM.HSCode,PD.Area,
                    PM.Wool,PM.CYarn,PM.Tar,PM.Tharri,PM.Misc,PM.Extra,PM.Loss,IPM.ShapeId,
PD.                 Khap, ISNULL(LEFT(Khap, CHARINDEX('x', Khap) - 1),0) as KhapWidth ,ISNULL(REPLACE(SUBSTRING(Khap, CHARINDEX('x', Khap), LEN(Khap)), 'x', ''),0) as KhapLength,PD.AfterKhapSizeOrder
                    From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
                    V_FinishedItemDetailNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                    Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.ITEM_FINISHED_ID 
                    And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                    PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";
        }
        else
        {
            sqlstr = @"Select Issue_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QualityName+' '+IPM.designName+' '+IPM.ColorName+' ' +IPM.ShapeName+ Space(5) +
                    Case When PM.Unitid=1 Then IPM.SizeMtr Else case When PM.unitid=6 Then IPM.Sizeinch  Else  SizeFt End End Description,Length,Width,
                    Length + 'x' + Width Size,Qty*Area as TotalArea,Rate,Qty,Amount,PD.orderid,PD.Item_Finished_Id,Replace(CONVERT(nvarchar(11),PM.assigndate,106),' ','-') as assigndate,Replace(CONVERT(nvarchar(11),PD.ReqByDate,106),' ','-') as Reqbydate,
                    PM.UnitId,PM.CalType,PD.comm,PM.issueorderid,PM.remarks,PD.CommAmt, PD.Amount+PD.CommAmt as TotalAmount,IPM.QualityId,PM.Instruction,PM.HSCode,PD.Area,
                    PM.Wool,PM.CYarn,PM.Tar,PM.Tharri,PM.Misc,PM.Extra,PM.Loss,IPM.ShapeId,
PD.                 Khap, ISNULL(LEFT(Khap, CHARINDEX('x', Khap) - 1),0) as KhapWidth ,ISNULL(REPLACE(SUBSTRING(Khap, CHARINDEX('x', Khap), LEN(Khap)), 'x', ''),0) as KhapLength,PD.AfterKhapSizeOrder
                    From PROCESS_ISSUE_MASTER_" + DDprocess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PD,
                    V_FinishedItemDetail IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                    Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.ITEM_FINISHED_ID 
                    And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                    PM.IssueOrderid=" + hnissueorderid.Value + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Issue_Detail_Id Desc";
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            if (chkedit.Checked == true)
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    txtchallanNo.Text = DDchallanNo.SelectedValue;
                    txtassigndate.Text = DS.Tables[0].Rows[0]["assigndate"].ToString();
                    txtreqdate.Text = DS.Tables[0].Rows[0]["Reqbydate"].ToString();
                    DDunit.SelectedValue = DS.Tables[0].Rows[0]["unitid"].ToString();
                    DDcaltype.SelectedValue = DS.Tables[0].Rows[0]["caltype"].ToString();
                    hnorderid.Value = DS.Tables[0].Rows[0]["orderid"].ToString();
                    txtremarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                    txtTotalPcs.Text = DS.Tables[0].Compute("sum(Qty)", "").ToString();
                    txtTotalArea.Text = DS.Tables[0].Compute("sum(TotalArea)", "").ToString();
                    txtTotalWeaving.Text = DS.Tables[0].Compute("sum(Rate)", "").ToString();
                    txtTotalComm.Text = DS.Tables[0].Compute("sum(CommAmt)", "").ToString();
                    txtTotalAmt.Text = DS.Tables[0].Compute("sum(TotalAmount)", "").ToString();
                    hnQualityId.Value = DS.Tables[0].Rows[0]["QualityId"].ToString();
                    txtInstruction.Text = DS.Tables[0].Rows[0]["Instruction"].ToString();
                    txtHSCode.Text = DS.Tables[0].Rows[0]["HSCode"].ToString();

                    txtWool.Text = DS.Tables[0].Rows[0]["Wool"].ToString();
                    txtCotton.Text = DS.Tables[0].Rows[0]["CYarn"].ToString();
                    txtTar.Text = DS.Tables[0].Rows[0]["Tar"].ToString();
                    txtTharri.Text = DS.Tables[0].Rows[0]["Tharri"].ToString();
                    txtExtra.Text = DS.Tables[0].Rows[0]["Extra"].ToString();
                    txtLoss.Text = DS.Tables[0].Rows[0]["Loss"].ToString();
                }
                else
                {
                    hnQualityId.Value = "0";
                    txtchallanNo.Text = "";
                    txtassigndate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    txtreqdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    txtTotalPcs.Text = "";
                    txtTotalArea.Text = "";
                    txtTotalWeaving.Text = "";
                    txtTotalComm.Text = "";
                    txtTotalAmt.Text = "";
                    txtWool.Text = "";
                    txtCotton.Text = "";
                    txtTar.Text = "";
                    txtTharri.Text = "";
                    txtMisc.Text = "";
                    txtLoss.Text = "";
                    txtExtra.Text = "";
                }
            }
            else
            {
                if (DS.Tables[0].Rows.Count > 0)
                {                    
                    txtTotalPcs.Text = DS.Tables[0].Compute("sum(Qty)", "").ToString();
                    txtTotalArea.Text = DS.Tables[0].Compute("sum(Area)", "").ToString();
                    txtTotalWeaving.Text = DS.Tables[0].Compute("sum(Rate)", "").ToString();
                    txtTotalComm.Text = DS.Tables[0].Compute("sum(CommAmt)", "").ToString();
                    txtTotalAmt.Text = DS.Tables[0].Compute("sum(TotalAmount)", "").ToString();
                }
                else
                {                   
                    txtTotalPcs.Text = "";
                    txtTotalArea.Text = "";
                    txtTotalWeaving.Text = "";
                    txtTotalComm.Text = "";
                    txtTotalAmt.Text = "";
                    txtWool.Text = "";
                    txtCotton.Text = "";
                    txtTar.Text = "";
                    txtTharri.Text = "";
                    txtMisc.Text = "";
                    txtLoss.Text = "";
                    txtExtra.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDChallanNO.Visible = false;
        DDvendor.SelectedIndex = -1;
        DDchallanNo.SelectedIndex = -1;
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        hnissueorderid.Value = "0";
        hnorderid.Value = "0";
        hnQualityId.Value = "0";
        TDeditchallanNo.Visible = false;

        if (chkedit.Checked == true)
        {
            TDChallanNO.Visible = true;
            TDeditchallanNo.Visible = true;
        }
    }
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {        
        string str = @"select Distinct PIM.IssueOrderId,PIM.IssueOrderId as Issueorderid1 From process_issue_master_" + DDprocess.SelectedValue + " PIM inner join Process_issue_Detail_" + DDprocess.SelectedValue + @" PID on PIM.Issueorderid=PID.issueorderid
                    inner join ordermaster OM on PID.orderid=OM.Orderid 
                    Where SampleNumber<>''  and PIM.Companyid=" + DDcompany.SelectedValue + @"
                    and PIM.Empid=" + DDvendor.SelectedValue;
        if (Chkcomplete.Checked==true)
        {
            str = str + " and PIM.Status='Complete'";
        }
        else
        {
            str = str + " and PIM.Status='Pending'";
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDcustcode.SelectedValue;
        }
        str = str + " order by Issueorderid1";

        UtilityModule.ConditionalComboFill(ref DDchallanNo, str, true, "--Plz Select--");

    }
    protected void DDchallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDchallanNo.SelectedValue;
        Fill_Grid();
    }
    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;
        Fill_Grid();
    }
    protected void txtchallanNoedit_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (DDprocess.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('Please Select Process.')", true);
            DDprocess.Focus();
            return;
        }
        string str = @"select  Distinct OM.CustomerId," + DDprocess.SelectedValue + @" as Processid,PIM.empid,PIM.IssueOrderId
                        From Process_Issue_Master_" + DDprocess.SelectedValue + " PIM inner join PROCESS_ISSUE_DETAIL_" + DDprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId 
                        inner join OrderMaster OM on PID.orderid=OM.OrderId
                        Where PIM.Companyid = " + DDcompany.SelectedValue + " And PIM.IssueOrderId=" + txtchallanNoedit.Text;
        if (Chkcomplete.Checked==true)
        {
            str = str + " and PIM.Status='Complete'";
        }
        else
        {
            str = str + " and PIM.Status='Pending'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDprocess.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
            DDprocess_SelectedIndexChanged(sender, new EventArgs());
            DDcustcode.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
            DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
            DDchallanNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
            DDchallanNo_SelectedIndexChanged(sender, new EventArgs());
            ViewState["IssueOrderid"] = ds.Tables[0].Rows[0]["issueorderid"].ToString();
        }
        else
        {
            lblmsg.Text = "Please enter proper Challan No. ";
            txtchallanNoedit.Focus();
            DDcustcode.SelectedIndex = -1;
            DDvendor.SelectedIndex = -1;
            DDchallanNo.SelectedIndex = -1;
            DGOrderdetail.DataSource = null;
            DGOrderdetail.DataBind();
            return;
        }
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int issue_detail_id = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
            TextBox txtqtyedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtqtyedit");
            TextBox txtrateedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtrateedit");
            TextBox txtcommrateedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtcommrateedit");
            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblitemfinishedid");
            Label lblcaltype = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblcaltype");
            TextBox txtwidthedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtwidthedit");
            TextBox txtlengthedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtlengthedit");
            Label lblArea = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblArea");
            TextBox txtKhapwidthedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtKhapwidthedit");
            TextBox txtKhaplengthedit = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtKhaplengthedit");
            Label lblAfterKhapSizeOrder = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblAfterKhapSizeOrder");
            //***************
            SqlParameter[] param = new SqlParameter[28];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@issue_Detail_id", issue_detail_id);
            param[2] = new SqlParameter("@Processid", DDprocess.SelectedValue);
            param[3] = new SqlParameter("@orderid", lblorderid.Text);
            param[4] = new SqlParameter("@Item_finished_id", lblitemfinishedid.Text);
            param[5] = new SqlParameter("@caltype", lblcaltype.Text);
            param[6] = new SqlParameter("@Qty", txtqtyedit.Text == "" ? "0" : txtqtyedit.Text);
            param[7] = new SqlParameter("@rate", txtrateedit.Text == "" ? "0" : txtrateedit.Text);
            param[8] = new SqlParameter("@commrate", txtcommrateedit.Text == "" ? "0" : txtcommrateedit.Text);
            param[9] = new SqlParameter("@userid", Session["varuserid"]);
            param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[12] = new SqlParameter("@Width", txtwidthedit.Text);
            param[13] = new SqlParameter("@Length", txtlengthedit.Text);
            param[14] = new SqlParameter("@Area", lblArea.Text);
            param[15] = new SqlParameter("@Wool", txtWool.Text=="" ? "0" : txtWool.Text);
            param[16] = new SqlParameter("@CYarn", txtCotton.Text == "" ? "0" : txtCotton.Text);
            param[17] = new SqlParameter("@Tar", txtTar.Text == "" ? "0" : txtTar.Text);
            param[18] = new SqlParameter("@Tharri", txtTharri.Text == "" ? "0" : txtTharri.Text);
            param[19] = new SqlParameter("@Misc", txtMisc.Text == "" ? "0" : txtMisc.Text);
            param[20] = new SqlParameter("@Extra", txtExtra.Text == "" ? "0" : txtExtra.Text);
            param[21] = new SqlParameter("@Loss", txtLoss.Text == "" ? "0" : txtLoss.Text);
            param[22] = new SqlParameter("@Remarks", txtremarks.Text);
            param[23] = new SqlParameter("@Instruction", txtInstruction.Text);
            param[24] = new SqlParameter("@HSCode", txtHSCode.Text);
            param[25] = new SqlParameter("@KhapWidth", txtKhapwidthedit.Text);
            param[26] = new SqlParameter("@KhapLength", txtKhaplengthedit.Text);
            param[27] = new SqlParameter("@AfterKhapSizeOrder", lblAfterKhapSizeOrder.Text); 
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateSampleorderNew", param);
            lblmsg.Text = param[10].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int issuedetailid = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);

            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblitemfinishedid");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Issue_detail_id", issuedetailid);
            param[1] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[2] = new SqlParameter("@orderid", lblorderid.Text);
            param[3] = new SqlParameter("@Item_finished_id", lblitemfinishedid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //*****
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteSampleOrderNew", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Report()
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

            array[0].Value = ViewState["IssueOrderid"];
            array[1].Value = DDprocess.SelectedValue;
            array[2].Value = Session["varMasterCompanyIDForERP"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForSampleProductionOrderReport", array);
           
            if (ds.Tables[0].Rows.Count > 0)
            {               
                    if (variable.VarNewConsumptionWise == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\SampleProductionOrderForMaltiRug.rpt";
                    }                    
             
                // Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\SampleProductionOrderForMaltiRug.xsd";
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frmsampleordernew.aspx");
            Tran.Rollback();
            lblmsg.Text = ex.Message;
            lblmsg.Visible = true;           
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();
        //string str = "select * From V_Sampleorder Where issueorderid=" + hnissueorderid.Value;
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    Session["rptFileName"] = "~\\Reports\\rptsampleorder.rpt";
        //    Session["Getdataset"] = ds;
        //    Session["dsFileName"] = "~\\ReportSchema\\rptsampleorder.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        //}
    }
    protected void chkexportsize_CheckedChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
}