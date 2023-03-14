using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Process_DyeingRateDefine : System.Web.UI.Page
{
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
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname", true, "--SelectCompany--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDDyerName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=5 And EI.MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
            fill_grid();
        }
    }

    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
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
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
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
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyid=" + Session["varCompanyId"];
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
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItem.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDDesign, "select DesignId,DesignName from Design Where MasterCompanyid=" + Session["varCompanyId"] + " order by DesignId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColor, "select ColorId,ColorName from Color Where MasterCompanyid=" + Session["varCompanyId"] + " order by ColorId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDColorShade, "select ShadeColorId,ShadeColorName from shadecolor Where MasterCompanyid=" + Session["varCompanyId"] + " order by ShadeColorId", true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName from Shape Where MasterCompanyid=" + Session["varCompanyId"] + " order by ShapeId", true, "--Select--");
    }
    private void fill_size()
    {
        string st = null;
        if (ChkFt.Checked)
            st = "Select SizeId,SizeFt from  Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"];
        else
            st = "Select SizeId,SizeMtr from  Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"];
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
    protected void DGDyeingRateDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGDyeingRateDetail.PageIndex = e.NewPageIndex;
        //fill_grid();
    }
    private void fill_grid()
    {
        DGDyeingRateDetail.DataSource = Fill_Grid_Data();
        DGDyeingRateDetail.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        // Lblmessage.Text = "Data Saved..............";
        DataSet ds = null;
        try
        {
            string strsql = @"select DRateDetailId,CalType DyingType,EmpName,Item_Name+'/'+isnull(QualityName,'')+'/'+isnull(DesignName,'')+'/'+isnull(ColorName,'')+'/'+isnull(ShadeColorName,'')+'/'+isnull(ShapeName,'') ItemDescription,SizeFt,SizeMtr,FromoQty,ToQty,Rate
            from DyeingRateMaster DRM inner join EmpInfo EI on DRM.PartyId=EI.Empid inner join V_FinishedItemDetail VF on VF.Item_Finished_Id=DRM.FinishedId inner join Process_CalType DT on DT.CalId=DyeingTypeId where DRM.CompanyId=" + DDCompanyName.SelectedValue + " And EI.MasterCompanyid=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DyeingRateDefine.aspx");
        }
        return ds;
    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct Category_Id,Category_Name from Item_Category_Master ICM inner join CategorySeparate CS on ICM.Category_Id=CS.CategoryId where id=1 And ICM.MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
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
    private void Clear()
    {
        TxtToQty.Text = null;
        TxtFQty.Text = null;
        TxtRate.Text = null;
        DDItem.SelectedIndex = 0;
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDColorShade.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DyeingRateDefine.aspx");
            Lblmessage.Visible = true;
            Lblmessage.Text = "Some Importent fields are missing.....";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DGDyeingRateDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        try
        {
            string strsql = @"select DRateDetailId,DRateId,CompanyId,DyeingTypeId,PartyId,FromoQty,ToQty,Rate,Category_Id,IPM.Item_Id,
                    Quality_Id,Design_Id,Color_Id,ShadeColor_Id,Shape_Id,Size_Id 
                    From DyeingRateMaster 
                    inner join Item_Parameter_Master IPM on Item_Finished_Id=FinishedId 
                    inner join Item_Master IM on IPM.Item_Id=IM.Item_Id where CompanyId = " + DDCompanyName.SelectedValue + " And DRateDetailId = " + DGDyeingRateDetail.SelectedDataKey.Value + " And IPM.MasterCompanyid=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            UtilityModule.ConditionalComboFill(ref DDCategory, "select distinct Category_Id,Category_Name from Item_Category_Master ICM inner join CategorySeparate CS on ICM.Category_Id=CS.CategoryId where id=1 And ICM.MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDDyeingType, "select CalId,CalType from Process_CalType", true, "--Select--");
            DDDyerName.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
            DDDyeingType.SelectedValue = ds.Tables[0].Rows[0]["DyeingTypeId"].ToString();
            DDCategory.SelectedValue = ds.Tables[0].Rows[0]["Category_Id"].ToString();
            ddlcategorycange();
            UtilityModule.ConditionalComboFill(ref DDItem, "select Item_Id,Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyid=" + Session["varCompanyId"] + "", true, "--Select--");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DyeingRateDefine.aspx");
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"delete from DyeingRateMaster where DRateDetailId=" + Session["DRateDetailId"];
            con.Open();
            SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'DyeingRateMaster'," + Session["DRateDetailId"] + ",getdate(),'Delete')");
            fill_grid();
            Clear();
            Lblmessage.Text = "Data Deleted.............";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/DyeingRateDefine.aspx");
            Lblmessage.Text = "Error:- Data Not Deleted.............";
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
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void DGDyeingRateDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
}