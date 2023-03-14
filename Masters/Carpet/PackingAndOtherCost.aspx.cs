using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class OtherExpense : System.Web.UI.Page
{
  static  int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            lablechange();
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_Id", true, "--SELECT--");
            btnInnerPacking.Visible = false;
            btnMiddlePacking.Visible = false;
            btnMasterPackingCost.Visible = false;
            btnOtherPackingCost.Visible = false;
            btnContainerPackingCost.Visible = false;

            btnInnerpackingMatCost.Visible = false;
            btnMiddlepackingMatCost.Visible = false;
            btnMasterpackingMatCost.Visible = false;
            btnOtherpackingMatCost.Visible = false;
            btnContainerpackingMatCost.Visible = false;
            TxtProdCode.Text = Request.QueryString["ProdCode"];
            Prod_Code_Text_Change();
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
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadename.Text = ParameterList[7];

    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        Quality.Visible = false;
        Design.Visible = false;
        Color.Visible = false;
        Shape.Visible = false;
        Size.Visible = false;
        Shade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Quality.Visible = true;
                        break;
                    case "2":
                        Design.Visible = true;
                        break;
                    case "3":
                        Color.Visible = true;
                        break;
                    case "4":
                        Shape.Visible = true;
                        break;
                    case "5":
                        Size.Visible = true;
                        break;
                    case "6":
                        Shade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue));
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid)
    {
        UtilityModule.ConditionalComboFill(ref Quality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT FROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        Prod_Code_Text_Change();
    }
    private void Prod_Code_Text_Change()
    {
        DataSet ds;
        string Str;
        lblMessage.Text = "";
        if (TxtProdCode.Text != "")
        {
            ddCategoryName.SelectedIndex = 0;
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue));
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                FILLGRID();
                btnInnerpackingMatCost.Visible = true;
                btnMiddlepackingMatCost.Visible = true;
                btnMasterpackingMatCost.Visible = true;
                btnOtherpackingMatCost.Visible = true;
                btnContainerpackingMatCost.Visible = true;
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
                btnInnerpackingMatCost.Visible = false;
                btnMiddlepackingMatCost.Visible = false;
                btnMasterpackingMatCost.Visible = false;
                btnOtherpackingMatCost.Visible = false;
                btnContainerpackingMatCost.Visible = false;
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[11];
            _arrpara[0] = new SqlParameter("@PRMCID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@CALTYPE", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@INNERAMT", SqlDbType.Float);
            _arrpara[4] = new SqlParameter("@MIDDLEAMT", SqlDbType.Float);
            _arrpara[5] = new SqlParameter("@MASTERAMT", SqlDbType.Float);
            _arrpara[6] = new SqlParameter("@OTHERAMT", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@CONTAINERAMT", SqlDbType.Float);
            _arrpara[8] = new SqlParameter("@ID", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            TxtProdCode.Text = "";
            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            if (DG.SelectedValue != null)
            {
                _arrpara[0].Value = DG.SelectedValue;
            }
            else
            {
                _arrpara[0].Value = 0;
            }
            _arrpara[1].Value = Varfinishedid;
            _arrpara[2].Value = ddCalType.SelectedIndex;
            _arrpara[3].Value = txtInnerPackingCost.Text;
            _arrpara[4].Value = txtMiddlePackingCost.Text;
            _arrpara[5].Value = txtMasterPackingCost.Text;
            _arrpara[6].Value = txtOtherPackingCost.Text;
            _arrpara[7].Value = txtContainerPackingCost.Text;
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[9].Value = Session["varuserid"];
            _arrpara[10].Value = Session["varCompanyId"];
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_PACKING_AND_OTHERMATERIAL_COST]", _arrpara);
            int ID = Convert.ToInt32(_arrpara[8].Value);
            if (ID == 0)
            {
                lblMessage.Text = "DUPLICATE DATA EXISTS.....";
            }
            else if (ID == 1)
            {
                lblMessage.Text = "DATA SAVED SUCESSFULLY....";
                GET_FORMULAFEILD(Varfinishedid, Tran);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingAndOtherCost.aspx");
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        txtInnerPackingCost.Text = "";
        Save_Refresh();
        FILLGRID();
        //}
    }
    private void GET_FORMULAFEILD(int VARFINISHEDID, SqlTransaction Tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[2];
        _arrpara[0] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@AMT", SqlDbType.Float);
        _arrpara[0].Value = VARFINISHEDID;
        _arrpara[1].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_CAL_TOTAL_AMT]", _arrpara);
    }
    private void FILLGRID()
    {
        DG.DataSource = Fill_Grid_Data();
        DG.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string strsql = @"SELECT PRMCID Sr_No,INNERAMT,MIDDLEAMT,MASTERAMT,OTHERAMT PKG,CONTAINERAMT FRT FROM PACKING_AND_OTHERMATERIAL_COST WHERE FINISHEDID=" + Varfinishedid + " And MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingAndOtherCost.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
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
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;
        }
        if (ddQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (ddDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        if (ddShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
            {
                goto a;
            }
        }
        if (ddSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(txtInnerPackingCost) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Save_Refresh()
    {
        txtInnerPackingCost.Text = "";
        txtMiddlePackingCost.Text = "";
        txtMasterPackingCost.Text = "";
        txtOtherPackingCost.Text = "";
        txtContainerPackingCost.Text = "";
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        ddCategoryName.SelectedIndex = 0;
        CATEGORY_DEPENDS_CONTROLS();
        TxtProdCode.Text = "";
        FILLGRID();
        btnsave.Text = "Save";
    }
  
    protected void btnInnerpackingMatCost_Click(object sender, EventArgs e)
    {
        btnInnerPacking.Visible = true;
    }
    protected void btnMiddlepackingMatCost_Click(object sender, EventArgs e)
    {
        btnMiddlePacking.Visible = true;
    }
    protected void btnMasterpackingMatCost_Click(object sender, EventArgs e)
    {
        btnMasterPackingCost.Visible = true;
    }
    protected void btnOtherpackingMatCost_Click(object sender, EventArgs e)
    {
        btnOtherPackingCost.Visible = true;
    }
    protected void btnContainerpackingMatCost_Click(object sender, EventArgs e)
    {
        btnContainerPackingCost.Visible = true;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where MasterCompanyId=" + MasterCompanyId + " And  ProductCode Like  '" + prefixText + "%'";
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
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string str = "Select * from PACKING_AND_OTHERMATERIAL_COST PC,ITEM_PARAMETER_MASTER IPM Where IPM.ITEM_FINISHED_ID=PC.FINISHEDID And PRMCID=" + DG.SelectedDataKey.Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtProdCode.Text = ds.Tables[0].Rows[0]["PRODUCTCODE"].ToString();
                Prod_Code_Text_Change();
                txtInnerPackingCost.Text = ds.Tables[0].Rows[0]["INNERAMT"].ToString();
                txtMiddlePackingCost.Text = ds.Tables[0].Rows[0]["MIDDLEAMT"].ToString();
                txtMasterPackingCost.Text = ds.Tables[0].Rows[0]["MASTERAMT"].ToString();
                txtOtherPackingCost.Text = ds.Tables[0].Rows[0]["OTHERAMT"].ToString();
                txtContainerPackingCost.Text = ds.Tables[0].Rows[0]["CONTAINERAMT"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/PackingAndOtherCost.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
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
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
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