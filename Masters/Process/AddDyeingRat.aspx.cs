using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Process_DyeingRateDefine : System.Web.UI.Page
{
    static int Finishedid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["DRateDetailId"] = 0;
            Session["DRateId"] = 0;
            ParameteLabel();
            UtilityModule.ConditionalComboFill(ref DDCompanyName, @"select Distinct CI.CompanyId,CI.Companyname 
            From CompanyInfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" 
            Order by Companyname", true, "--SelectCompany--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            if (variable.Carpetcompany == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDDyerName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " inner join Process_name_master Pnm on EP.ProcessId=Pnm.PROCESS_NAME_ID and Pnm.PROCESS_NAME='DYEING' order by Empname", true, "--Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDDyerName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            }

            Fill_Detail(Request.QueryString["a1"], Request.QueryString["a2"], Request.QueryString["a3"], Request.QueryString["a4"]);
            Finishedid = Convert.ToInt32(Request.QueryString["a3"]);
            fill_grid();
        }
    }
    private void Fill_Detail(string a1, string a2, string a3, string a4)
    {
        //DDCompanyName.SelectedValue = a1;
        DDDyerName.SelectedValue = a2;
        UtilityModule.ConditionalComboFill(ref DDCategory, "select Category_Id,Category_Name from Item_category_Master Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");

        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select IPM.*,ICM.Category_Id from Item_Parameter_Master IPM inner Join Item_Master IM on IPM.Item_Id=IM.Item_ID inner join Item_Category_Master ICM on  ICM.Category_Id=IM.Category_Id where Item_Finished_Id=" + a3 + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            DDCategory.SelectedValue = ds.Tables[0].Rows[0]["Category_Id"].ToString();
            ddlcategorycange();
            UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDItem.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
            UtilityModule.ConditionalComboFill(ref DDQuality, "select  QualityId,QualityName from Quality where Item_Id=" + DDItem.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["Quality_Id"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDColor, "select ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDColorShade, "select ShadeColorId,ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Design_Id"].ToString();
            DDColor.SelectedValue = ds.Tables[0].Rows[0]["Color_Id"].ToString();
            DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_Id"].ToString();
            DDShape.SelectedValue = ds.Tables[0].Rows[0]["Shape_Id"].ToString();
            UtilityModule.ConditionalComboFill(ref DDSize, "select SizeId,SizeMtr from Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["Size_Id"].ToString();
            UtilityModule.ConditionalComboFill(ref DDDyeingType, "select CalId,CalType from Process_CalType", true, "--Select--");
            DDDyeingType.SelectedValue = a4;
            fill_grid();
            TxtFQty.Focus();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/AddDyeingRat.aspx");
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

    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));//);
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
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
    }
    private void ddlcategorycange()
    {
        TdQuality.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;

        string strsql = @"SELECT distinct IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fill_size();
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_combo();
    }
    private void fill_combo()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItem.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " order by DesignId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColor, "select ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by ColorId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColorShade, "select ShadeColorId,ShadeColorName from shadecolor Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShadeColorId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShapeId", true, "--Select--");
    }
    private void fill_size()
    {
        string st = null;
        if (ChkFt.Checked)
            st = "Select SizeId,SizeFt from  Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
        else
            st = "Select SizeId,SizeMtr from  Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDSize, st, true, "--Select--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_size();
    }
    protected void DGDyeingRateDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDyeingRateDetail, "select$" + e.Row.RowIndex);
        }
    }

    private void fill_grid()
    {
        DGDyeingRateDetail.DataSource = Fill_Grid_Data();
        DGDyeingRateDetail.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"select distinct DRateDetailId,CalType DyingType,EmpName,Item_Name+'/'+isnull(QualityName,'')+'/'+isnull(DesignName,'')+'/'+isnull(ColorName,'')
                            +'/'+isnull(ShadeColorName,'')+'/'+isnull(ShapeName,'') ItemDescription,SizeFt,SizeMtr,FromoQty,ToQty,Rate
                            from DyeingRateMaster DRM inner join EmpInfo EI on DRM.PartyId=EI.Empid 
                            inner join V_FinishedItemDetail VF on VF.Item_Finished_Id=DRM.FinishedId 
                            inner join Process_CalType DT on DT.CalId=DyeingTypeId Where DRM.CompanyId=" + DDCompanyName.SelectedValue;
            if (DDDyerName.SelectedIndex > 0)
            {
                strsql = strsql + " and EI.EMpid=" + DDDyerName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                strsql = strsql + " and vf.category_id=" + DDCategory.SelectedValue;
            }
            if (DDDyeingType.SelectedIndex > 0)
            {
                strsql = strsql + " and calid=" + DDDyeingType.SelectedValue;
            }
            if (DDItem.SelectedIndex > 0)
            {
                strsql = strsql + " and vf.Item_id=" + DDItem.SelectedValue;
            }
            if (Finishedid > 0)
            {
                strsql = strsql + " and Finishedid=" + Finishedid;
            }

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/AddDyeingRat.aspx");
        }

        return ds;
    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct Category_Id,Category_Name from Item_Category_Master ICM inner join CategorySeparate CS on ICM.Category_Id=CS.CategoryId where id=1 And ICM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDDyeingType, "select CalId,CalType from Process_CalType", true, "--Select--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {

        if (Convert.ToDouble(TxtFQty.Text) < Convert.ToDouble(TxtToQty.Text))
        {
            Save_Date();
        }
        else
        {
            TxtToQty.Text = null;
            TxtToQty.Focus();
            Lblmessage.Text = "Error:- ToQty must greated than fromQty .............";
        }
    }
    private void Save_Date()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[13];
            _arrpara[0] = new SqlParameter("@DRateDetailId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@DRateId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@FinishedId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@DyeingTypeId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@PartyId", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@FromoQty", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@ToQty", SqlDbType.Float);
            _arrpara[8] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[9] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            con.Open();

            SqlTransaction Tran = con.BeginTransaction();
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = Session["DRateDetailId"];
            _arrpara[1].Direction = ParameterDirection.InputOutput;
            _arrpara[1].Value = Session["DRateId"];
            _arrpara[2].Value = DDCompanyName.SelectedValue;
            _arrpara[3].Value = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            _arrpara[4].Value = DDDyeingType.SelectedValue;
            _arrpara[5].Value = DDDyerName.SelectedValue;
            _arrpara[6].Value = TxtFQty.Text;
            _arrpara[7].Value = TxtToQty.Text;
            _arrpara[8].Value = TxtRate.Text;
            _arrpara[9].Value = Session["varuserId"];
            _arrpara[10].Value = Session["varCompanyId"];
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ValidateDyeingRate", _arrpara);

            if (Convert.ToInt32(_arrpara[0].Value) == 0)
            {
                _arrpara[0].Value = Session["DRateDetailId"];
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DyeingRateDefine", _arrpara);
                Session["DRateId"] = _arrpara[1].Value;
                Session["DRateDetailId"] = 0;
                Session["ReportPath"] = "";
                // Session["CommanFormula"] = "{V_PurchaseReceiveReport.ReceiveNo} ='" "'";
                BtnPreview.Enabled = true;
                Tran.Commit();
                Lblmessage.Text = "Data Saved..........";
                fill_grid();
                Clear();
                BtnDelete.Visible = false;
                BtnSave.Text = "Save";
            }
            else
            {
                Lblmessage.Text = "Error: Range selection already exist or incorrect ........";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/AddDyeingRat.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = "Some Importent fields are missing.....";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Clear()
    {
        TxtToQty.Text = null;
        TxtFQty.Text = null;
        TxtRate.Text = null;
        DDColorShade.SelectedValue = "0";
        DDColorShade.Focus();
    }

    protected void DGDyeingRateDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        try
        {
            string strsql = @"select DRateDetailId,DRateId,CompanyId,DyeingTypeId,PartyId,FromoQty,ToQty,Rate,Category_Id,IPM.Item_Id,
                    Quality_Id,Design_Id,Color_Id,ShadeColor_Id,Shape_Id,Size_Id 
                    from DyeingRateMaster 
                    inner join Item_Parameter_Master IPM on Item_Finished_Id=FinishedId 
                    inner join Item_Master IM on IPM.Item_Id=IM.Item_Id 
                    where CompanyId = " + DDCompanyName.SelectedValue + @" And DRateDetailId=" + DGDyeingRateDetail.SelectedDataKey.Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct Category_Id,Category_Name from Item_Category_Master ICM inner join CategorySeparate CS on ICM.Category_Id=CS.CategoryId where id=1 And ICM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDDyeingType, "select CalId,CalType from Process_CalType", true, "--Select--");
            DDDyerName.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
            DDDyeingType.SelectedValue = ds.Tables[0].Rows[0]["DyeingTypeId"].ToString();
            DDCategory.SelectedValue = ds.Tables[0].Rows[0]["Category_Id"].ToString();
            ddlcategorycange();
            UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            DDItem.SelectedValue = ds.Tables[0].Rows[0]["Item_Id"].ToString();
            fill_combo();
            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["Quality_Id"].ToString();
            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Design_Id"].ToString();
            DDColor.SelectedValue = ds.Tables[0].Rows[0]["Color_Id"].ToString();
            DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_Id"].ToString();
            DDShape.SelectedValue = ds.Tables[0].Rows[0]["Shape_Id"].ToString();
            fill_size();
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["Size_Id"].ToString();
            TxtFQty.Text = ds.Tables[0].Rows[0]["FromoQty"].ToString();
            TxtToQty.Text = ds.Tables[0].Rows[0]["ToQty"].ToString();
            TxtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
            BtnDelete.Visible = true;
            BtnSave.Text = "Update";
            Session["DRateDetailId"] = ds.Tables[0].Rows[0]["DRateDetailId"].ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/AddDyeingRat.aspx");
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string strsql = @"delete from DyeingRateMaster where DRateDetailId=" + Session["DRateDetailId"];
            SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            fill_grid();
            Clear();
            Lblmessage.Text = "Data Deleted.............";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/AddDyeingRat.aspx");
            Lblmessage.Text = "Error:- Data Not Deleted.............";
        }
    }
    protected void DDDyeingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    //protected void DGDyeingRateDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void lnkgetdata_Click(object sender, EventArgs e)
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;

        if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
        {
            quality = 1;
        }
        if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
        {
            design = 1;
        }
        if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
        {
            color = 1;
        }
        if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
        {
            shape = 1;
        }
        if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
        {
            size = 1;
        }
        if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
        {
            shadeColor = 1;
        }
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1 && DDCategory.SelectedIndex > 0)
        {
            Finishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TextItemCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

        }
        fill_grid();
    }
    protected void DyerRate_Excel()
    {
        Lblmessage.Text = "";
        int Row;
        //DataSet DS = new DataSet();
        String sQry = " ";
        string FilterBy = "Filter By-";
        try
        {

            if (DDDyerName.SelectedIndex > 0)
            {
                sQry = sQry + " and DRM.PartyId = " + DDDyerName.SelectedValue;
                FilterBy = FilterBy + ",PartyName - " + DDDyerName.SelectedItem.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                sQry = sQry + " and VF.Category_Id = " + DDCategory.SelectedValue;
                FilterBy = FilterBy + ",CategoryName - " + DDCategory.SelectedItem.Text;
                //strsql = strsql + " and vf.category_id=" + DDCategory.SelectedValue;
            }
            if (DDItem.SelectedIndex > 0)
            {
                sQry = sQry + " and VF.Item_Id = " + DDItem.SelectedValue;
                FilterBy = FilterBy + ",ItemName - " + DDItem.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                sQry = sQry + " and VF.QualityId = " + DDQuality.SelectedValue;
                FilterBy = FilterBy + ",QualityName - " + DDQuality.SelectedItem.Text;
            }
            if (DDColorShade.SelectedIndex > 0)
            {
                sQry = sQry + " and VF.Shadecolorid = " + DDColorShade.SelectedValue;
                FilterBy = FilterBy + ",ShadeColorName - " + DDColorShade.SelectedItem.Text;
            }
            if (DDDyeingType.SelectedIndex > 0)
            {
                sQry = sQry + " and DRM.DyeingTypeId = " + DDDyeingType.SelectedValue;
                FilterBy = FilterBy + ",DyeingType - " + DDDyeingType.SelectedItem.Text;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] array = new SqlParameter[4];
                array[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                array[1] = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                array[2] = new SqlParameter("@UserId", SqlDbType.Int);
                array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

                array[0].Value = DDCompanyName.SelectedValue;
                array[1].Value = sQry;
                array[2].Value = Session["VarUserId"];
                array[3].Value = Session["varCompanyId"];

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDyerRateDetailForExcelReport", array);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";

                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("Dyer Rate Summary");
                    //*************
                    sht.Range("A1:G1").Merge();
                    sht.Range("A1:G1").Style.Font.FontSize = 11;
                    sht.Range("A1:G1").Style.Font.Bold = true;
                    sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1").SetValue("DYER RATE SUMMARY");
                    sht.Row(1).Height = 21.75;
                    //
                    sht.Range("A2:G2").Merge();
                    sht.Range("A2:G2").Style.Font.FontSize = 11;
                    sht.Range("A2:G2").Style.Font.Bold = true;
                    sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A2").SetValue(FilterBy.TrimStart(','));
                    sht.Row(2).Height = 21.75;
                    //Header
                    sht.Range("A3:G3").Style.Font.FontSize = 11;
                    sht.Range("A3:G3").Style.Font.Bold = true;
                    sht.Range("A3:G3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Row(3).Height = 18.00;
                    //
                    sht.Range("A3").SetValue("Dyer Name");
                    sht.Range("B3").SetValue("Dyeing Type");
                    sht.Range("C3").SetValue("Item Description");
                    sht.Range("D3").SetValue("Shade Color");
                    sht.Range("E3").SetValue("From Qty");
                    sht.Range("F3").SetValue("To Qty");
                    sht.Range("G3").SetValue("Rate");
                    Row = 4;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + Row + ":F" + Row).Style.Font.FontSize = 11;

                        sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                        sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["DyingType"]);
                        sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                        sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                        sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["FromoQty"]);
                        sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["ToQty"]);
                        sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                        Row = Row + 1;
                    }
                    //**********
                    sht.Columns(1, 10).AdjustToContents();
                    //**************Save
                    //******SAVE FILE
                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("DyerRateSummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                    Path = Server.MapPath("~/Tempexcel/" + filename);
                    xapp.SaveAs(Path);
                    xapp.Dispose();
                    //Download File
                    Response.ClearContent();
                    Response.ClearHeaders();
                    // Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.WriteFile(Path);
                    // File.Delete(Path);
                    Response.End();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
                }
            }
            catch (Exception ex)
            {
                Lblmessage.Text = ex.ToString();
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        catch (Exception ex)
        {
            Lblmessage.Text = ex.Message;
        }
    }
    protected void BtnPreviewExcel_Click(object sender, EventArgs e)
    {
        if (DDCompanyName.SelectedIndex > 0)
        {
            DyerRate_Excel();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('Please select company name!');", true);
        }
    }
}