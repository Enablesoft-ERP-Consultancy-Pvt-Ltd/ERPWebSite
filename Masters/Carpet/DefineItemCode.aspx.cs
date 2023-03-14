using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Globalization;
public partial class DefineItemCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();
            if (Request.QueryString["ABC"] == "1")
            {
                zzz.Style.Add("display", "none");
            }
            lablechange();
            ddcategory.Focus();

            //if (Session["varcompanyId"].ToString() == "20" && variable.VarNewQualitySize == "1")
            //{
            //    dateforsize.Visible = true;
            //    tbSizeDate.Attributes.Add("readonly", "readonly");
            //    tbSizeDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    dateforsize.Visible = false;
            //}

            if (variable.VarNewQualitySize == "1")
            {
                btnaddsize.Visible = false;
                btnAddQualitySize.Visible = true;
            }
            else
            {
                btnaddsize.Visible = true;
                btnAddQualitySize.Visible = false;
            }
            string str = "";
            DataSet ds1;
            str = @"Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varCompanyId"] + @"
                  select val,Type from SizeType Order by val";
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddcategory, ds1, 0, true, "--Select--");
            if (Session["varcompanyId"].ToString() == "20")
            {
                trheader.Visible = false;
                ddshape.Enabled = false;
                DDsizetype.Enabled = false;
                ddsize.Enabled = false;

                if (ddcategory.Items.Count > 0)
                {
                    ddcategory.SelectedIndex = 1;
                    UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                    if (dditemname.Items.Count > 0)
                    {
                        dditemname.SelectedIndex = 1;
                        QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
                    }
                }
                Fill_Grid();

            }
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds1, 1, false, "");
            if (DDsizetype.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizetype.SelectedValue = variable.VarDefaultSizeId;
            }


            ds1.Dispose();


            txtitemcode.Text = Request.QueryString["ProdCode"];
            Prod_Code_Text_Change();
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            hncomp.Value = Session["varCompanyId"].ToString();
            if (Convert.ToInt16(Session["varCompanyId"]) == 2)
            {
                trweight.Visible = true;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from DutyWeightMaster");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblweight1.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    lblweight2.Text = ds.Tables[0].Rows[1]["Name"].ToString();
                    lblweight3.Text = ds.Tables[0].Rows[2]["Name"].ToString();
                }
            }
            switch (Convert.ToInt16(Session["varcompanyid"]))
            {
                case 4:
                    TrItemcode.Visible = true;
                    tdSearchDesign.Visible = false;
                    lblProductCode.Text = "Rug Id";
                    TDUPCNo.Visible = true;
                    break;
                case 9:
                    TrItemcode.Visible = false;
                    break;
                case 12:
                    lblDescp.Text = "Construction";
                    break;
                case 15:
                    TrItemcode.Visible = false;
                    break;
                case 30:
                    TRSKUNo.Visible = false;
                    break;
                case 39:
                case 40:
                    TrItemcode.Visible = false;
                    TRSKUNo.Visible = false;
                    break;
                default:
                    TrItemcode.Visible = false;
                    TRSKUNo.Visible = true;
                    TDUPCNo.Visible = false;
                    break;
            }
        }
    }
    private void Prod_Code_Text_Change()
    {
        DataSet ds;
        string Str;
        if (txtitemcode.Text != "")
        {
            ddcategory.SelectedIndex = 0;
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + txtitemcode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"] + "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddcategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                Fill_Grid();
                QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();

                if (variable.VarNewQualitySize == "1")
                {
                    if (Session["varcompanyId"].ToString() == "20")
                    {
                        Str = "SELECT SIZEID,Export_Format fROM QualitySizeNew Where QualityId=" + dquality.SelectedValue;
                    }
                    else
                    {
                        Str = "SELECT SIZEID,Export_Format fROM QualitySizeNew Where SHAPEID=" + ddshape.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and UpdateDate is null";
                    }
                }
                else
                {
                    Str = "SELECT SIZEID, SIZEFT fROM SIZE WhERE SHAPEID=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "";

                    if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
                    {
                        Str = Str + " order by sizeft, SizeID";
                    }
                }

                UtilityModule.ConditionalComboFill(ref ddsize, Str, true, "--ALL SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();

                string str2 = "select sku_no from sku_no where finished_id=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + " And MasterCompanyId=" + Session["varCompanyId"] + "";
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
                if (ds2.Tables[0].Rows.Count > 0)
                    txtsuk_no.Text = ds2.Tables[0].Rows[0]["sku_no"].ToString();
                DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select brass,iron,glass from MAIN_ITEM_IMAGE where finishedid=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + " And MasterCompanyId=" + Session["varCompanyId"] + "");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    Txtweight1.Text = ds3.Tables[0].Rows[0]["brass"].ToString();
                    Txtweight2.Text = ds3.Tables[0].Rows[0]["iron"].ToString();
                    Txtweight3.Text = ds3.Tables[0].Rows[0]["glass"].ToString();
                }
            }
            else
            {
                lblerror.Text = "ITEM CODE DOES NOT EXISTS....";
                ddcategory.SelectedIndex = 0;
                ddlcategorycange();
                txtitemcode.Text = "";
                txtitemcode.Focus();
            }
        }
        else
        {
            if (Session["varcompanyId"].ToString() == "20")
            {
                if (ddcategory.Items.Count > 0)
                {
                    ddcategory.SelectedIndex = 1;
                    UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                    if (dditemname.Items.Count > 0)
                    {
                        dditemname.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                ddcategory.SelectedIndex = 0;
                ddlcategorycange();

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
        Label1.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadename.Text = ParameterList[7];
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Gvdefineitem.Visible = true;
        ddlcategorycange();
        dditemname.Focus();
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        Shd.Visible = false;
        UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddcategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                        //UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Order  by DesignName", true, "--Select--");
                        break;
                    case "3":
                        clr.Visible = true;
                        //UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color", true, "--Select--");                        
                        break;
                    case "4":
                        shp.Visible = true;
                        //UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Order by Shapeid",true,"--Select--");
                        break;
                    case "5":
                        sz.Visible = true;
                        break;
                    case "6":
                        Shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality where item_id=" + dditemname.SelectedValue, true, "--Select--");
        QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));

        if (Session["varcompanyId"].ToString() == "20")
        {
            FillSize();
            Fill_Grid();
            // BtnRef_Click(sender, new EventArgs());


        }

        dquality.Focus();
        btnsave.Text = "Save";
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddshape.SelectedIndex > 0)
        {
            FillSize();
            //  Fill_Grid();
            chkbox.Focus();
        }
    }
    private void Fill_Grid()
    {
        DataSet ds = Fill_Grid_Data();
        if (ds.Tables[0].Rows.Count > 0)
        {
            Gvdefineitem.DataSource = ds;
            Gvdefineitem.DataBind();
        }
        else
        {
            Gvdefineitem.DataSource = null;
            Gvdefineitem.DataBind();
        }

    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int A = 2;
            if (chkbox.Checked == true)
            {
                A = 1;
            }

            string strsql;
            if (variable.VarNewQualitySize == "1")
            {

                strsql = @"Select Item_Finished_id as Sr_No,ICM.Category_Name CATEGORY,Im.Item_Name ITEMNAME,
             IsNull(QualityName,'') as Quality, IsNull(DesignName,'') as Design,IsNull(ColorName,'') as Color,IsNull(ShapeName,'') as Shape, Case When " + A + @"=2 then IsNull(Export_Format,'') else IsNull(MtrSize,'')  end as Size,
             IsNull(QualityName,'')+IsNull(DesignName,'')+IsNull(ColorName,'')+IsNull(ShapeName,'')+IsNull(ShadeColorName,'')+
            Case When " + A + @"=2 then IsNull(Export_Format,'') else IsNull(MtrSize,'') End  DESCRIPTION,ProductCode ProdCode From Item_Parameter_Master IPM inner join Item_Master IM ON IPM.Item_Id=IM.Item_Id 
            inner join ITEM_CATEGORY_MASTER ICM ON ICM.Category_Id=IM.Category_Id Left Outer Join Quality Q ON IPM.Quality_id=Q.Qualityid Left Outer Join Design D ON 
            IPM.Design_id=D.Designid Left Outer Join Color C ON IPM.Color_id=C.Colorid Left Outer Join Shape Sh ON IPM.Shape_id=Sh.Shapeid Left Outer Join QualitySizeNew QSN ON 
            IPM.Size_id=QSN.Sizeid Left Outer Join ShadeColor Sd ON IPM.SHADECOLOR_ID=Sd.ShadecolorId Left Outer Join  MAIN_ITEM_IMAGE MI on ipm.item_finished_id=MI.finishedid
            where IPM.ITEM_ID=" + dditemname.SelectedValue + "  And IPM.MasterCompanyId=" + Session["varCompanyId"];

            }
            else
            {
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    strsql = @"Select Item_Finished_id as Sr_No,ICM.Category_Name CATEGORY,Im.Item_Name ITEMNAME,
                IsNull(QualityName,'') as Quality, IsNull(DesignName,'') as Design,IsNull(ColorName,'') as Color,IsNull(ShapeName,'') as Shape, Case When " + A + @"=2 then cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end  else cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else '' end  end as Size,
                IsNull(QualityName,'')+IsNull(DesignName,'')+IsNull(ColorName,'')+IsNull(ShapeName,'')+IsNull(ShadeColorName,'')+
            Case When " + A + @"=2 then cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end else cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else '' end End  DESCRIPTION,ProductCode ProdCode From Item_Parameter_Master IPM inner join Item_Master IM ON IPM.Item_Id=IM.Item_Id 
            inner join ITEM_CATEGORY_MASTER ICM ON ICM.Category_Id=IM.Category_Id Left Outer Join Quality Q ON IPM.Quality_id=Q.Qualityid Left Outer Join Design D ON 
            IPM.Design_id=D.Designid Left Outer Join Color C ON IPM.Color_id=C.Colorid Left Outer Join Shape Sh ON IPM.Shape_id=Sh.Shapeid Left Outer Join Size S ON 
            IPM.Size_id=S.Sizeid Left Outer Join ShadeColor Sd ON IPM.SHADECOLOR_ID=Sd.ShadecolorId Left Outer Join  MAIN_ITEM_IMAGE MI on ipm.item_finished_id=MI.finishedid
            where IPM.ITEM_ID=" + dditemname.SelectedValue + "  And IPM.MasterCompanyId=" + Session["varCompanyId"];
                }
                else
                {

                    strsql = @"Select Item_Finished_id as Sr_No,ICM.Category_Name CATEGORY,Im.Item_Name ITEMNAME,
                IsNull(QualityName,'') as Quality, IsNull(DesignName,'') as Design,IsNull(ColorName,'') as Color,IsNull(ShapeName,'') as Shape, Case When " + A + @"=2 then IsNull(SizeFt,'') else IsNull(SizeMtr,'')  end as Size,
                IsNull(QualityName,'')+IsNull(DesignName,'')+IsNull(ColorName,'')+IsNull(ShapeName,'')+IsNull(ShadeColorName,'')+
            Case When " + A + @"=2 then IsNull(SizeFt,'') else IsNull(SizeMtr,'') End  DESCRIPTION,ProductCode ProdCode From Item_Parameter_Master IPM inner join Item_Master IM ON IPM.Item_Id=IM.Item_Id 
            inner join ITEM_CATEGORY_MASTER ICM ON ICM.Category_Id=IM.Category_Id Left Outer Join Quality Q ON IPM.Quality_id=Q.Qualityid Left Outer Join Design D ON 
            IPM.Design_id=D.Designid Left Outer Join Color C ON IPM.Color_id=C.Colorid Left Outer Join Shape Sh ON IPM.Shape_id=Sh.Shapeid Left Outer Join Size S ON 
            IPM.Size_id=S.Sizeid Left Outer Join ShadeColor Sd ON IPM.SHADECOLOR_ID=Sd.ShadecolorId Left Outer Join  MAIN_ITEM_IMAGE MI on ipm.item_finished_id=MI.finishedid
            where IPM.ITEM_ID=" + dditemname.SelectedValue + "  And IPM.MasterCompanyId=" + Session["varCompanyId"];


                }
            }
            if (dquality.Visible == true && dquality.SelectedIndex > 0)
            {
                strsql = strsql + @" And IPM.QUALITY_ID=" + dquality.SelectedValue + "";
            }

            if (Session["varcompanyId"].ToString() != "20")
            {

                if (dddesign.Visible == true && dddesign.SelectedIndex > 0)
                {
                    strsql = strsql + @" And IPM.DESIGN_ID=" + dddesign.SelectedValue + "";
                }
                if (ddcolor.Visible == true && ddcolor.SelectedIndex > 0)
                {
                    strsql = strsql + @" And IPM.COLOR_ID=" + ddcolor.SelectedValue + "";
                }
                if (ddshape.Visible == true && ddshape.SelectedIndex > 0)
                {
                    strsql = strsql + @" And IPM.SHAPE_ID=" + ddshape.SelectedValue + "";
                }
                if (ddsize.Visible == true && ddsize.SelectedIndex > 0)
                {
                    strsql = strsql + @" And IPM.SIZE_ID=" + ddsize.SelectedValue + "";
                }
                if (ddShade.Visible == true && ddShade.SelectedIndex > 0)
                {
                    strsql = strsql + @" And IPM.SHADECOLOR_ID=" + ddShade.SelectedValue + "";
                    //
                }
            }

            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/DefineItemCode.aspx");
            Logs.WriteErrorLog("DefineItemCode|Fill_Grid_Data|" + ex.Message);
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
    protected void Gvdefineitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        string id = Gvdefineitem.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        hnItemFinishedId.Value = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ipm.item_finished_id,ipm.quality_id,ipm.design_id,ipm.color_id,ipm.shape_id,ipm.size_id,ipm.description,ipm.item_id,ipm.productCode,im.CATEGORY_ID,isnull(ipm.OurCode,'') as OurCode from item_parameter_master ipm inner join ITEM_MASTER im on ipm.item_id = im.item_id Where item_finished_id=" + id + " And ipm.MasterCompanyId=" + Session["varCompanyId"] + "");
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                ddcategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();
                dquality.SelectedValue = ds.Tables[0].Rows[0]["quality_id"].ToString();
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["design_id"].ToString();
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["color_id"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["shape_id"].ToString();
                Txtdesc.Text = ds.Tables[0].Rows[0]["description"].ToString();
                FillSize();
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["size_id"].ToString();
                txtitemcode.Text = ds.Tables[0].Rows[0]["productCode"].ToString();
                if (Session["VarCompanyNo"].ToString() == "4")
                {
                    txtUpcNo.Text = ds.Tables[0].Rows[0]["OurCode"].ToString();
                }
                else
                {
                    txtUpcNo.Text = "";
                }

                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from MAIN_ITEM_IMAGE Where Finishedid=" + id + " And MasterCompanyId=" + Session["varCompanyId"] + "");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    //newPreview1.ImageUrl = "~/ImageHandler.ashx?Id=" + id + "&img=3";
                    newPreview1.ImageUrl = ds1.Tables[0].Rows[0]["photo"].ToString();
                    Txtweight1.Text = ds1.Tables[0].Rows[0]["brass"].ToString();
                    Txtweight2.Text = ds1.Tables[0].Rows[0]["iron"].ToString();
                    Txtweight3.Text = ds1.Tables[0].Rows[0]["glass"].ToString();
                    TxtWeight.Text = ds1.Tables[0].Rows[0]["NETWEIGHT"].ToString();
                }
                else
                {

                    Txtweight1.Text = "0";
                    Txtweight2.Text = "0";
                    Txtweight3.Text = "0";
                    TxtWeight.Text = "0";
                }
                DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Rtrim(sku_no) As sku_no  from sku_no where  Finished_id=" + id + " And MasterCompanyId=" + Session["varCompanyId"] + "");
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    txtsuk_no.Text = Convert.ToString(ds4.Tables[0].Rows[0][0]);
                }
                else
                {
                    txtsuk_no.Text = "";
                }
                BtnDelete.Visible = true;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/DefineItemCode.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        CHECKVALIDCONTROL();
        int VarFinishedid = 0;
        if (lblerror.Text == "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            int itemfinishedid = 0;
            try
            {
                string VarProdCode = TrItemcode.Visible == true ? txtitemcode.Text : "";
                SqlParameter[] _arrPara = new SqlParameter[16];
                _arrPara[0] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@QUALITY_ID", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@DESIGN_ID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@COLOR_ID", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@SHAPE_ID", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@SIZE_ID", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 100);
                _arrPara[7] = new SqlParameter("@ITEM_ID", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@ProCode", SqlDbType.NVarChar);
                _arrPara[9] = new SqlParameter("@SHADECOLOR_ID", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@VarOurCode", SqlDbType.VarChar, 100);
                _arrPara[11] = new SqlParameter("@ProdvalidMsg", SqlDbType.Int);
                _arrPara[12] = new SqlParameter("@Sku_No", SqlDbType.VarChar, 200);
                _arrPara[13] = new SqlParameter("@SkuMsg", SqlDbType.Int);
                _arrPara[14] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[15] = new SqlParameter("@VarUpdate", SqlDbType.Int);

                _arrPara[0].Direction = ParameterDirection.InputOutput;

                ///ddShadeColor
                _arrPara[0].Value = string.IsNullOrEmpty(hnItemFinishedId.Value) ? "0" : hnItemFinishedId.Value;

                itemfinishedid = Convert.ToInt32(_arrPara[0].Value);
                this.UploadPhoto(itemfinishedid);








                _arrPara[1].Value = dquality.Visible == true ? Convert.ToInt32(dquality.SelectedValue) : 0;
                _arrPara[2].Value = dddesign.Visible == true ? Convert.ToInt32(dddesign.SelectedValue) : 0;
                _arrPara[3].Value = ddcolor.Visible == true ? Convert.ToInt32(ddcolor.SelectedValue) : 0;
                _arrPara[4].Value = ddshape.Visible == true ? Convert.ToInt32(ddshape.SelectedValue) : 0;
                _arrPara[5].Value = ddsize.Visible == true ? Convert.ToInt32(ddsize.SelectedValue) : 0;
                _arrPara[6].Value = Txtdesc.Text;
                _arrPara[7].Value = dditemname.SelectedValue;
                _arrPara[8].Value = VarProdCode;
                _arrPara[9].Value = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;
                if (Session["VarCompanyNo"].ToString() == "4")
                {
                    _arrPara[10].Value = TDUPCNo.Visible == true ? txtUpcNo.Text : "";
                }
                else
                {
                    _arrPara[10].Value = "";
                }

                _arrPara[11].Direction = ParameterDirection.Output;
                _arrPara[12].Value = txtsuk_no.Text != "" ? txtsuk_no.Text : "";
                _arrPara[13].Direction = ParameterDirection.Output;
                _arrPara[14].Value = Session["varCompanyId"];
                if (hnItemFinishedId.Value != "")
                {
                    _arrPara[15].Value = 1;
                }
                else
                {
                    _arrPara[15].Value = 0;
                }
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_GET_ITEM_FINISHED_IDWithProdCode]", _arrPara);
                itemfinishedid = Convert.ToInt32(_arrPara[0].Value);

                if (_arrPara[11].Value.ToString() == "1")
                {
                    // ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('This Combination does not belong to Product Code...');", true);
                    lblerror.Text = "This Combination does not belong to Product Code...";
                }
                else if (_arrPara[11].Value.ToString() == "2")
                {
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('Product Code Already Exists...');", true);
                    lblerror.Text = "Product Code Already Exists...";
                }
                else if (_arrPara[11].Value.ToString() == "3")
                {
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('Product Code Already Exists...');", true);
                    lblerror.Text = "This Shade is already in used....";
                }
                else if (_arrPara[11].Value.ToString() == "4")
                {
                    // ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('This Combination does not belong to Product Code...');", true);
                    lblerror.Text = "This Combination already used so you can't not update...";
                }
                else if (_arrPara[11].Value.ToString() == "5")
                {
                    // ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('This Combination does not belong to Product Code...');", true);
                    lblerror.Text = "Data updated sucessfully...";
                }
                else if (_arrPara[11].Value.ToString() == "6")
                {
                    // ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('This Combination already exists so you can't not update...');", true);
                    lblerror.Text = "This Combination already exists so you can't not update...";
                }
                if (_arrPara[13].Value.ToString() == "1")
                {
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "Opn1", "alert('SKU_No Already Exists...');", true);
                    lblerror.Text = "SKU_No Already Exists...";
                }

                Tran.Commit();
                if (btnsave.Text == "Update")
                {
                    btnsave.Text = "Save";
                }
                if (itemfinishedid.ToString() != "0")
                {
                    // Save_Image(itemfinishedid);
                    savewight(itemfinishedid);
                }
                Fill_Grid();
                txtsuk_no.Text = "";
                Txtweight1.Text = "";
                Txtweight2.Text = "";
                Txtweight3.Text = "";
                TxtWeight.Text = "";
                hnItemFinishedId.Value = "";

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/DefineItemCode.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    public void UploadPhoto(int finishId)
    {
        if (finishId > 0)
        {

            string folderPath = ConfigurationManager.AppSettings["ImagePath"];
            if (PhotoImage.HasFiles)
            {
                int index = 1;
                foreach (HttpPostedFile uploadedFile in PhotoImage.PostedFiles)
                {
                    //Check whether Directory (Folder) exists.
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists. Create it.
                        Directory.CreateDirectory(folderPath);
                    }
                    string fileName = "product-" + index.ToString() + "-" + finishId.ToString() + "-" + DateTime.Now.Ticks.ToString() + "-img" + Path.GetExtension(uploadedFile.FileName);
                    string imgPath = Path.Combine(folderPath, fileName);
                    uploadedFile.SaveAs(imgPath);
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId) values(" + finishId + ",'" + fileName + "'," + Session["varCompanyId"] + ")");
                    index++;
                }
            }
        }
    }

    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }

    public void Save_Image(int VarFinishedid)
    {
        //SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //myConnection.Open();
        if (PhotoImage.FileName != "")
        {
            //SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "Delete MAIN_ITEM_IMAGE Where FINISHEDID=" + VarFinishedid);
            //int id = 0;
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT DISTINCT isnull(FINISHEDID,0) as FinishedID FROM MAIN_ITEM_IMAGE Where FINISHEDID=" + VarFinishedid + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
            if (id > 0)
            {
                string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
                string targetPath = Server.MapPath("../../Item_Image/" + VarFinishedid + "_Item.gif");
                string img = "~\\Item_Image\\" + VarFinishedid + "_Item.gif";
                //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
                Stream strm = PhotoImage.PostedFile.InputStream;
                var targetFile = targetPath;

                FileInfo TheFile = new FileInfo(Server.MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
                }
                if (PhotoImage.FileName != null && PhotoImage.FileName != "")
                {
                    GenerateThumbnails(0.3, strm, targetFile);
                }
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update MAIN_ITEM_IMAGE Set Photo='" + img + "' Where FINISHEDID= " + VarFinishedid + "");
                //storeimage = new SqlCommand("Update MAIN_ITEM_IMAGE SET PHOTO = @image WHERE FINISHEDID= "+ VarFinishedid + " And MasterCompanyId=" + Session["varCompanyId"] + ")", myConnection);
                //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
            }
            else
            {
                string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
                string targetPath = Server.MapPath("../../Item_Image/" + VarFinishedid + "_Item.gif");
                string img = "~\\Item_Image\\" + VarFinishedid + "_Item.gif";
                //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
                Stream strm = PhotoImage.PostedFile.InputStream;
                var targetFile = targetPath;

                FileInfo TheFile = new FileInfo(Server.MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
                }
                if (PhotoImage.FileName != null && PhotoImage.FileName != "")
                {
                    GenerateThumbnails(0.3, strm, targetFile);
                }
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId) values(" + VarFinishedid + ",'" + img + "'," + Session["varCompanyId"] + ")");
                //storeimage = new SqlCommand("Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId) values(" + VarFinishedid + ","+img+"," + Session["varCompanyId"] + ")", myConnection);
            }

        }
        //myConnection.Close();
        //myConnection.Dispose();
    }

    public void savewight(int VarFinishedid)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] _param = new SqlParameter[6];
            _param[0] = new SqlParameter("@Finishedid", VarFinishedid);
            _param[1] = new SqlParameter("@MasterCompanyid", Session["varCompanyId"]);
            _param[2] = new SqlParameter("@Brass", Txtweight1.Text);
            _param[3] = new SqlParameter("@Iron", Txtweight2.Text);
            _param[4] = new SqlParameter("@Glass", Txtweight3.Text);
            _param[5] = new SqlParameter("@Weight", TxtWeight.Text);
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_SaveWeight", _param);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }

    public void savewight1(int VarFinishedid)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(finishedid,0) from MAIN_ITEM_IMAGE where finishedid=" + VarFinishedid + " And MasterCompanyid=" + Session["varCompanyId"] + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "update MAIN_ITEM_IMAGE set brass='" + Txtweight1.Text + "',Iron='" + Txtweight2.Text + "',Glass='" + Txtweight3.Text + "' where finishedid=" + VarFinishedid + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            }
            else if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into MAIN_ITEM_IMAGE (finishedid,photo,remarkS,brass,Iron,Glass,MasterCompanyId) values(" + VarFinishedid + ",'','','" + Txtweight1.Text + "','" + Txtweight2.Text + "','" + Txtweight3.Text + "'," + Session["MasterCompanyId"] + ")");
            }
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblerror.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddcategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(dditemname) == false)
        {
            goto a;

        }
        if (dquality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (dddesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dddesign) == false)
            {
                goto a;
            }
        }
        if (ddcolor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (ddshape.Visible == true)
        {
            if (Session["varcompanyId"].ToString() == "20")
            {
                if (ddshape.SelectedIndex < 0)
                {
                    goto a;
                }
            }
            else
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
                {
                    goto a;
                }
            }
        }
        if (ddsize.Visible == true)
        {
            if (Session["varcompanyId"].ToString() == "20")
            {
                if (ddsize.SelectedIndex < 0)
                {
                    goto a;
                }
            }
            else
            {
                if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
                {
                    goto a;
                }
            }

        }
        if (Shd.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
            {
                goto a;
            }
        }
        //if (UtilityModule.VALIDTEXTBOX(txtitemcode) == false)
        //{
        //    goto a;
        //}
        //else
        //{
        //    goto B;
        //}
        goto B;
    a:
        UtilityModule.SHOWMSG(lblerror);
    B:;
    }
    protected void Gvdefineitem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gvdefineitem.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void chkbox_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
        Fill_Grid();
    }
    private void FillSize()
    {
        if (variable.VarNewQualitySize == "1")
        {
            string Str;
            string Size = "";
            switch (DDsizetype.SelectedValue)
            {
                case "0":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "Export_Format+'  '+'['+Production_Ft_Format+']'";
                            break;
                        default:
                            Size = "Export_Format";
                            break;
                    }
                    break;
                case "1":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "MtrSize+'  '+'['+Production_Mt_Format+']'";
                            break;
                        default:
                            Size = "MtrSize";
                            break;
                    }
                    break;
                case "2":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "MtrSize+'  '+'['+Production_Ft_Format+']'";
                            break;
                        default:
                            Size = "MtrSize";
                            break;
                    }
                    break;
                default:
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "Export_Format+'  '+'['+Production_Ft_Format+']'";
                            break;
                        default:
                            Size = "Export_Format";
                            break;
                    }
                    break;
            }
            if (Session["varcompanyId"].ToString() == "20")
            {
                //Str = "select sizeid," + Size + " as sizeft from QualitySizeNew where shapeid=" + ddshape.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and AddDate <= '" + String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(tbSizeDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : tbSizeDate.Text)) + "' AND (UpdateDate > '" + String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(tbSizeDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : tbSizeDate.Text)) + "' OR UpdateDate is null)  order by Export_Format";
                Str = "select sizeid," + Size + " as sizeft from QualitySizeNew where QualityTypeId=" + dditemname.SelectedValue + " and QualityId=" + dquality.SelectedValue + "   order by Export_Format";
            }
            else
            {
                Str = "select sizeid," + Size + " as sizeft from QualitySizeNew where shapeid=" + ddshape.SelectedValue + " and QualityId=" + dquality.SelectedValue;

                if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
                {
                    Str = Str + " order by sizeft, SizeID";
                }
                else
                {
                    Str = Str + " order by Export_Format";
                }
            }

            UtilityModule.ConditionalComboFill(ref ddsize, Str, true, " ALL SIZE");
        }
        else
        {
            string Str;
            string Size = "";
            switch (DDsizetype.SelectedValue)
            {
                case "0":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "sizeft+'  '+'['+Prodsizeft+']'";
                            break;

                        default:
                            Size = "sizeft";
                            break;
                    }
                    break;
                case "1":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "sizemtr+'  '+'['+ProdSizemtr+']'";
                            break;
                        default:
                            Size = "sizemtr";
                            break;
                    }
                    break;
                case "2":
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "sizeinch+'  '+'['+ProdSizeft+']'";
                            break;
                        default:
                            Size = "sizeinch";
                            break;
                    }
                    break;
                default:
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "4":
                            Size = "sizeft+'  '+'['+Prodsizeft+']'";
                            break;
                        default:
                            Size = "sizeft";
                            break;
                    }
                    break;
            }

            if (Session["varcompanyid"].ToString() == "44")
            {
                switch (DDsizetype.SelectedValue.ToString())
                {
                    case "0":
                        Str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                        break;
                    case "1":
                        Str = " Select Distinct S.sizeid,cast(s.WidthMtr as varchar)+'x'+cast(s.LengthMtr as varchar) +case when s.HeightMtr>0 then 'x'+cast(s.HeightMtr as varchar) else ''  end as  Sizemtr from Size S Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];

                        break;
                    case "2":
                        Str = "Select Distinct S.sizeid,cast(s.WidthInch as varchar)+'x'+cast(s.LengthInch as varchar) +case when s.HeightInch>0 then 'x'+cast(s.HeightInch as varchar) else ''  end as  Sizeinch from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                        break;
                    default:
                        Str = "Select Distinct S.sizeid,cast(s.WidthFt as varchar)+'x'+cast(s.LengthFt as varchar) +case when s.Heightft>0 then 'x'+cast(s.HeightFt as varchar) else ''  end as  SizeFt from Size S  Where S.shapeid=" + ddshape.SelectedValue + " and S.mastercompanyid=" + Session["varcompanyid"];
                        break;
                }
            }
            else

            {
                Str = "select sizeid," + Size + " as sizeft from size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " ";
            }
            if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
            {
                Str = Str + " order by sizeft, SizeID";
            }
            else if (Session["varcompanyId"].ToString() == "44")
            {
                Str = Str + " order by 1";
            }
            else
            {
                Str = Str + " order by sizeft";
            }

            UtilityModule.ConditionalComboFill(ref ddsize, Str, true, " Select Size");
        }
    }
    protected void Gvdefineitem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvdefineitem, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < Gvdefineitem.Columns.Count; i++)
            {
                if (Session["varcompanyId"].ToString() == "20")
                {
                    if (Gvdefineitem.Columns[i].HeaderText == "DESCRIPTION")
                    {
                        Gvdefineitem.Columns[i].Visible = false;
                    }
                }
                else
                {
                    if (Gvdefineitem.Columns[i].HeaderText == "Quality" || Gvdefineitem.Columns[i].HeaderText == "Design" || Gvdefineitem.Columns[i].HeaderText == "Color" || Gvdefineitem.Columns[i].HeaderText == "Shape" || Gvdefineitem.Columns[i].HeaderText == "Size")
                    {
                        Gvdefineitem.Columns[i].Visible = false;
                    }

                }
            }
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dquality.SelectedIndex > 0)
        {
            //txtitemcode.Text = dquality.SelectedItem.Text;
            // Fill_Grid();
            FillSize();
            dddesign.Focus();
            ddshape.Focus();
        }
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dddesign.SelectedIndex > 0)
        {
            // Fill_Grid();
            ddcolor.Focus();
        }
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddcolor.SelectedIndex > 0)
        {
            //Fill_Grid();
            ddshape.Focus();
        }
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddcategory, @"Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER", true, "Select Category");
        ddcategory.SelectedIndex = 0;
        ddlcategorycange();
        //dditemname.SelectedIndex = 0;
        if (Session["varcompanyId"].ToString() == "20")
        {
            Gvdefineitem.Visible = true;
        }
        else
        {
            Gvdefineitem.Visible = false;
        }

        btnsave.Text = "Save";
        txtsuk_no.Text = "";
        txtitemcode.Text = "";
        Txtweight1.Text = "";
        Txtweight2.Text = "";
        Txtweight3.Text = "";
    }
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcategory, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER where  MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_Id", true, "--SELECT--");
        if (Session["varcompanyId"].ToString() == "20")
        {
            if (ddcategory.Items.Count > 0)
            {
                ddcategory.SelectedIndex = 1;
                UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                if (dditemname.Items.Count > 0)
                {
                    dditemname.SelectedIndex = 1;
                }
            }

        }
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Item--");
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select qualityid,qualityname from quality Where Item_Id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname ", true, "--Select--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design where  MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName", true, "--Select--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color where  MasterCompanyId=" + Session["varCompanyId"] + " order by colorid", true, "--Select--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "20")
        {
            UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape where  MasterCompanyId=" + Session["varCompanyId"] + " Order by Shapeid", true, "--ALL Shape--");
        }
        else
        {

            UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape where  MasterCompanyId=" + Session["varCompanyId"] + " Order by Shapeid", true, "--Select--");
        }
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        string Str = "";
        if (variable.VarNewQualitySize == "1")
        {
            if (Session["varcompanyNo"].ToString() == "4")
            {
                Str = "select sizeid,Export_Format+'  '+'['+Production_Ft_Format+']' as Sizeft from QualitySizeNew where QualityId=" + dquality.SelectedValue + " and UpdateDate is null  order by sizeid ";
            }
            else if (Session["varcompanyId"].ToString() == "20")
            {
                Str = "select sizeid,Export_Format from QualitySizeNew where QualityId=" + dquality.SelectedValue + "  order by sizeid ";
            }
            else
            {
                Str = "select sizeid,Export_Format from QualitySizeNew where QualityId=" + dquality.SelectedValue + " and UpdateDate is null order by sizeid ";
            }
        }
        else
        {
            if (Session["varcompanyNo"].ToString() == "4")
            {
                Str = " select sizeid,sizeft+'  '+'['+ProdSizeft+']' as Sizeft from size where  MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid";
            }
            else
            {
                Str = "select sizeid,sizeft from size where  MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid ";
            }
        }

        UtilityModule.ConditionalComboFill(ref ddsize, Str, true, "--Select--");

    }
    protected void txtitemcode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (txtitemcode.Text != "")
        {
            ddcategory.SelectedIndex = 0;
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + txtitemcode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"] + "";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddcategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();


                if (variable.VarNewQualitySize == "1")
                {
                    if (Session["varcompanyId"].ToString() == "20")
                    {
                        Str = "SELECT SizeId,Export_Format fROM QualitySizeNew Where QualityId=" + dquality.SelectedValue;
                    }
                    else
                    {
                        Str = "SELECT SizeId,Export_Format fROM QualitySizeNew Where SHAPEID=" + ddshape.SelectedValue + " and QualityId=" + dquality.SelectedValue + " and UpdateDate is null ";
                    }
                }
                else
                {
                    Str = "SELECT SIZEID,SIZEFT fROM SIZE Where SHAPEID=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By SizeID";
                }

                UtilityModule.ConditionalComboFill(ref ddsize, Str, true, "--ALL SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();


                string str2 = "select sku_no from sku_no where finished_id=" + ds.Tables[0].Rows[0]["item_finished_id"].ToString() + " And MasterCompanyId=" + Session["varCompanyId"] + "";
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
                if (ds2.Tables[0].Rows.Count > 0)
                    txtsuk_no.Text = ds2.Tables[0].Rows[0]["sku_no"].ToString();
                Fill_Grid();
            }
            else
            {
                lblerror.Text = "ITEM CODE DOES NOT EXISTS....";
                ddcategory.SelectedIndex = 0;
                ddlcategorycange();

                txtitemcode.Focus();
            }
        }
        else
        {
            if (Session["varcompanyId"].ToString() == "20")
            {
                if (ddcategory.Items.Count > 0)
                {
                    ddcategory.SelectedIndex = 1;
                    UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddcategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                    if (dditemname.Items.Count > 0)
                    {
                        dditemname.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                ddcategory.SelectedIndex = 0;
                ddlcategorycange();

            }

        }
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        #region Author: Rajeev, Date: 26-Nov-12
        //UtilityModule.ConditionalComboFill(ref Quality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " Order By QUALITYID Desc", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE", true, "--SELECT--");
        //UtilityModule.ConditionalComboFill(ref Shade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor", true, "--SELECT--");

        #endregion

        string strsql = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME 
                           SELECT DESIGNID,DESIGNNAME from DESIGN where  MasterCompanyId=" + Session["varCompanyId"] + @" order by DesignName
                            SELECT COLORID,COLORNAME FROM COLOR where  MasterCompanyId=" + Session["varCompanyId"] + @" order by ColorName
                            SELECT SHAPEID,SHAPENAME FROM SHAPE where  MasterCompanyId=" + Session["varCompanyId"] + @" order by SHAPEID
                            SELECT ShadecolorId,ShadeColorName FROM ShadeColor  where  MasterCompanyId=" + Session["varCompanyId"] + " order by ShadecolorName";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        // int i = 0;
        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        if (Session["varcompanyId"].ToString() == "20")
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--ALL SHAPE--");
            if (Quality.Items.Count > 0)
            {
                Quality.SelectedIndex = 1;
                FillSize();
            }
            //if (Design.Items.Count > 0)
            //{
            //    Design.SelectedIndex = 1;
            //}
            //if (Color.Items.Count > 0)
            //{
            //    Color.SelectedIndex = 1;
            //}
        }
        else
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
            if (Session["varcompanyId"].ToString() == "30")
            {
                if (Shape.Items.Count > 0)
                {
                    Shape.SelectedIndex = 1;
                    if (dditemname.SelectedIndex > 0 && Shape.SelectedIndex > 0)
                    {
                        FillSize();
                    }
                }
            }

        }

        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");



    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            if (btnsave.Text == "Update")
            {
                SqlParameter[] parparam = new SqlParameter[4];
                parparam[0] = new SqlParameter("@id", Gvdefineitem.SelectedDataKey.Value);
                parparam[1] = new SqlParameter("@VarCompanyId", Session["VarCompanyId"]);
                parparam[2] = new SqlParameter("@VarUserId", Session["VarUserId"]);
                parparam[3] = new SqlParameter("@msg", SqlDbType.VarChar, 200);
                parparam[3].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteInDefineItemCode", parparam);
                lblerror.Text = parparam[3].Value.ToString();
            }
            txtitemcode.Text = "";
            BtnDelete.Visible = false;
            dquality.SelectedIndex = 0;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/DefineItemCode.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    //protected void Gvdefineitem_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }


    protected void close_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ABC"] == "1")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "CloseForm();", true);
        }
        else
        {
            Response.Redirect("~/main.aspx");
        }
    }
    protected void refreshControl()
    {
        //UtilityModule.ConditionalComboFill(ref ddcategory, @"Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER", true, "Select Category");
        ddcategory.SelectedIndex = 0;
        ddlcategorycange();
        //dditemname.SelectedIndex = 0;
        Gvdefineitem.Visible = false;
        btnsave.Text = "Save";
        txtsuk_no.Text = "";
        txtitemcode.Text = "";
        Txtweight1.Text = "";
        Txtweight2.Text = "";
        Txtweight3.Text = "";

    }

    protected void Gvdefineitem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (hnsst.Value == "true")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = Gvdefineitem.Rows[index];
            int ItemFinishedID = Convert.ToInt32(row.Cells[0].Text);
            // Response.Redirect("DefineItemCodeOther.aspx?SrNo=" + ItemFinishedID);
            if (e.CommandName == "AddRate")
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('frmDefineItemcodeRate.aspx?SrNo=" + ItemFinishedID + "', 'nwwin', 'toolbar=0, titlebar=1,  top=80px, left=80px, scrollbars=1, resizable = yes,width=500,height=300');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('DefineItemCodeOther.aspx?SrNo=" + ItemFinishedID + "', 'nwwin', 'toolbar=0, titlebar=1,  top=80px, left=80px, scrollbars=1, resizable = yes,width=920,height=360');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }

    }
    protected void BtnRef_Click(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void tbSizeDate_TextChanged(object sender, EventArgs e)
    {
        FillSize();
    }

    private void Fill_DesignGridWithQuality()
    {
        DataSet ds = Fill_DesignQuality_Grid_Data();
        if (ds.Tables[0].Rows.Count > 0)
        {
            TRDesignWithQuality.Visible = true;

            GVDesignWithQuality.DataSource = ds;
            GVDesignWithQuality.DataBind();
        }
        else
        {
            TRDesignWithQuality.Visible = false;
            GVDesignWithQuality.DataSource = null;
            GVDesignWithQuality.DataBind();
        }
    }

    private DataSet Fill_DesignQuality_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select Distinct D.DesignID, D.DesignName,Q.QualityId,Q.QualityName,IM.Item_Id,IM.Item_Name
                              from Item_Parameter_Master IPM 
                              JOIN Item_Master IM ON IPM.Item_Id=IM.Item_ID
                              LEFT JOIN Design D ON IPM.Design_ID=D.DesignId LEFT JOIN Quality Q ON IPM.QUALITY_ID=Q.QualityId 
                             where IPM.MasterCompanyId=" + Session["varCompanyId"];
            if (txtsearchdesign.Text != "")
            {
                strsql = strsql + " and D.Designname like '" + txtsearchdesign.Text + "%'";
            }
            strsql = strsql + " order by D.designname";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineItemCode");
            Logs.WriteErrorLog("Masters_Carpet_DefineItemCode|Fill_Grid_Data|" + ex.Message);
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
    protected void txtsearchdesign_TextChanged(object sender, EventArgs e)
    {
        if (txtsearchdesign.Text != "")
        {
            Fill_DesignGridWithQuality();
        }
    }

}