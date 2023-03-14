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
using System.Text;
using System.Globalization;
public partial class Master_Carpet_FrmCreateRecipe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            lablechange();
            ddcategory.Focus();
            string str = @"Select PNM.Process_Name_ID, PNM.Process_Name From Process_Name_Master PNM(Nolock) Order By PNM.Process_Name
                    Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varCompanyId"] + @"
                    Select ID, Name From RecipeMaster(Nolock) Where ProcessID = 29
                    Select UnitID, UnitName From Unit(Nolock) Order By UnitId
                    Select Val, Type From SizeType(Nolock) Order By Val";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddcategory, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDRecipeName, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 4, false, "");

            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedValue = "29";
                Process_NameSelectedChanged();
            }
            if (ddcategory.Items.Count > 0)
            {
                ddcategory.SelectedValue = "3";
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
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Process_NameSelectedChanged();
    }
    private void Process_NameSelectedChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDRecipeName, @"Select ID, Name From RecipeMaster(Nolock) Where ProcessID = " + DDProcessName.SelectedValue + " And MasterCompanyId = " + Session["varCompanyId"], true, "--Select--");
    }
    protected void DDRecipeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        if (DDProcessName.SelectedIndex > 0 && DDRecipeName.SelectedIndex > 0)
        {
            string Str = @"Select a.ID, VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.designName + '  ' + VF.ColorName + '  ' + VF.ShadeColorName + '  ' + VF.ShapeName + '  ' + VF.SizeMtr Description, 
                        U.UnitName, a.ConsmpQty 
                        From RecipeDetail a(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                        JOIN Unit U(Nolock) ON U.UnitID = a.UnitID 
                        Where a.ProcessID = " + DDProcessName.SelectedValue + " And a.ReceipeNameID = " + DDRecipeName.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGRecipe.DataSource = ds.Tables[0];
            DGRecipe.DataBind();
        }
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        Shd.Visible = false;
        dditemname.Focus();
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
                        Shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(dquality, dddesign, ddcolor, ddshape, ddShade, Convert.ToInt32(dditemname.SelectedValue));
        dquality.Focus();
        btnsave.Text = "Save";
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddshape.SelectedIndex > 0)
        {
            FillSize();
        }
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
                Str = "select sizeid," + Size + " as sizeft from QualitySizeNew where shapeid=" + ddshape.SelectedValue + " and QualityId=" + dquality.SelectedValue + "  order by Export_Format";
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
            Str = "select sizeid," + Size + " as sizeft from size where shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by sizeft";

            UtilityModule.ConditionalComboFill(ref ddsize, Str, true, " Select Size");
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dquality.SelectedIndex > 0)
        {
            FillSize();
            dddesign.Focus();
            ddshape.Focus();
        }
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && dddesign.SelectedIndex > 0)
        {
            ddcolor.Focus();
        }
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditemname.SelectedIndex > 0 && ddcolor.SelectedIndex > 0)
        {
            ddshape.Focus();
        }
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        string strsql = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME 
                           SELECT DESIGNID,DESIGNNAME from DESIGN where  MasterCompanyId=" + Session["varCompanyId"] + @" order by DesignName
                            SELECT COLORID,COLORNAME FROM COLOR where  MasterCompanyId=" + Session["varCompanyId"] + @" order by ColorName
                            SELECT SHAPEID,SHAPENAME FROM SHAPE where  MasterCompanyId=" + Session["varCompanyId"] + @" order by SHAPEID
                            SELECT ShadecolorId,ShadeColorName FROM ShadeColor  where  MasterCompanyId=" + Session["varCompanyId"] + " order by ShadecolorName";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void tbSizeDate_TextChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (lblerror.Text == "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            int itemfinishedid = 0;
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[16];
                _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@RecipeNameID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@Item_Finished_ID", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@UnitID", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@ConsmpQty", SqlDbType.Float);
                _arrPara[6] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);

                itemfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = 0;
                _arrPara[1].Value = DDProcessName.SelectedValue;
                _arrPara[2].Value = DDRecipeName.SelectedValue;
                _arrPara[3].Value = itemfinishedid;
                _arrPara[4].Value = DDUnit.SelectedValue;
                _arrPara[5].Value = TxtConsmp.Text;
                _arrPara[6].Value = Session["varuserid"];
                _arrPara[7].Value = Session["varCompanyId"];
                _arrPara[8].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVE_RECIPEDETAIL]", _arrPara);

                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", " alert('" + _arrPara[8].Value + "');", true);

                TxtConsmp.Text = "";
                DDUnit.SelectedIndex = 0;
                dquality.SelectedIndex = 0;
                dquality.Focus();
                Fill_Grid();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/save/frmcreateRecipe.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void RefreshRecipeName_Click(object sender, EventArgs e)
    {
        Process_NameSelectedChanged();
    }
    protected void DGRecipe_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRecipe, "Select$" + e.Row.RowIndex);
        }
    }

    protected void DGRecipe_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (lblerror.Text == "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[16];
                _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@RecipeNameID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = DGRecipe.DataKeys[e.RowIndex].Value;
                _arrPara[1].Value = DDProcessName.SelectedValue;
                _arrPara[2].Value = DDRecipeName.SelectedValue;
                _arrPara[3].Value = Session["varuserid"];
                _arrPara[4].Value = Session["varCompanyId"];
                _arrPara[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_DELETE_RECIPEDETAIL]", _arrPara);

                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", " alert('" + _arrPara[5].Value + "');", true);
                Fill_Grid();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Delete/frmcreateRecipe.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}