using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Order_FrmItemConsumption : System.Web.UI.Page
{
    static int MasterCompanyid;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyid = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            lablechange();
            ddCategoryName.Focus();
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedIndex = 1;
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.Focus();
            }
            UtilityModule.ConditionalComboFill(ref ddProcessName, "Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by PROCESS_NAME", true, "--Select--");
            if (ddProcessName.Items.Count > 0)
            {
                ddProcessName.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFill(ref ddOrderUnit, "SELECT * from Unit Where UnitTypeId=1 Order By UnitName", true, "--SELECT--");
            if (ddOrderUnit.Items.Count > 0)
            {
                ddOrderUnit.SelectedIndex = 1;
            }
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            switch (VarProdCode)
            {
                case 0:
                    TDItemCode.Visible = false;
                    break;
                case 1:
                    TDItemCode.Visible = true;
                    break;
            }
            rdoUnitWise.Checked = true;
            DataDeleteFromGrid(99999);
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblCategoryname.Text = ParameterList[5];
        lblItemname.Text = ParameterList[6];
        lblQualityname.Text = ParameterList[0];
        lblDesignname.Text = ParameterList[1];
        lblColorname.Text = ParameterList[2];
        lblShapename.Text = ParameterList[3];
        lblSizename.Text = ParameterList[4];
        lblShade.Text = ParameterList[7];
    }

    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        lblMessage.Text = "";
        Quality.Visible = false;
        Design.Visible = false;
        Color.Visible = false;
        Shape.Visible = false;
        Size.Visible = false;
        Shade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "SELECT--");
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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid, int Type_Flag)
    {
        lblMessage.Text = "";
        if (Quality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Quality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QUALITYNAME", true, "--SELECT--");
        }
        if (Design.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--SELECT--");
        }
        if (Color.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--SELECT--");
        }
        if (Shape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SHAPENAME", true, "--SELECT--");
            if (Shape.Items.Count > 0)
            {
                Shape.SelectedIndex = 1;
                OrderUnitSelectedChanged();
            }
        }
        if (Shade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Shade, "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SHADECOLORNAME", true, "--SELECT--");
        }
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderUnitSelectedChanged();
    }
    private void OrderUnitSelectedChanged()
    {
        lblMessage.Text = "";
        if (ddShape.SelectedIndex > 0)
        {
            if (Convert.ToInt32(ddOrderUnit.SelectedValue) == 6)
            {
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,Sizeinch Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Sizeinch", true, "--SELECT--");
            }
            else if (Convert.ToInt32(ddOrderUnit.SelectedValue) == 2)
            {
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeFt Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SizeFt", true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SizeId,SizeMtr Size_Name from Size where shapeid=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by SizeMtr", true, "--SELECT--");
            }
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        lblMessage.Text = "";
        if (TxtProdCode.Text != "")
        {
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            Str = "Select IPM.*,IM.CATEGORY_ID,IsNull(MII.REMARKS,'') REMARKS from ITEM_MASTER IM,CategorySeparate CS,ITEM_PARAMETER_MASTER IPM Left Outer Join MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  And Id=0 and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                OrderUnitSelectedChanged();
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
                ddCategoryName.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtArea.Text = FillArea().ToString();
    }
    public double FillArea()
    {
        string Str = "";
        double VarArea = 0;
        if (ddSize.SelectedIndex > 0)
        {
            if (Convert.ToInt32(ddOrderUnit.SelectedValue) == 6)
            {
                Str = "SELECT AreaINCH from Size where sizeid=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            }
            else if (Convert.ToInt32(ddOrderUnit.SelectedValue) == 2)
            {
                Str = "SELECT AreaFt from Size where sizeid=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                Str = "SELECT AreaMtr from Size where sizeid=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            }
            VarArea = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str));
        }
        return VarArea;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[7];
                _arrpara[0] = new SqlParameter("@TableID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@CalType", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@UnitId", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@ItemFinishedId", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@Qty", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@Area", SqlDbType.Float);

                _arrpara[0].Direction = ParameterDirection.Output;
                _arrpara[1].Value = ddProcessName.SelectedValue;
                if (rdoPcWise.Checked == true)
                {
                    _arrpara[2].Value = 1;
                }
                else
                {
                    _arrpara[2].Value = 0;
                }
                _arrpara[3].Value = ddOrderUnit.SelectedValue;
                int ItemFinishedId = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                _arrpara[4].Value = ItemFinishedId;
                _arrpara[5].Value = (TxtQty.Text == "" ? 0 : Convert.ToDouble(TxtQty.Text));
                _arrpara[6].Value = (TxtArea.Text == "" ? 0 : Convert.ToDouble(TxtArea.Text));

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_ItemConsumptionMaster", _arrpara);
                tran.Commit();
                string vara = _arrpara[0].Value.ToString();
                UtilityModule.ITEM_CONSUMPTION_DEFINE(Convert.ToInt32(ddProcessName.SelectedValue), ItemFinishedId, Convert.ToInt32(_arrpara[0].Value));
                FillGrid();
                SaveClear();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmItemConsumption.aspx");
                tran.Rollback();
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void SaveClear()
    {
        TxtArea.Text = "";
        TxtQty.Text = "";
        if (Shade.Visible == true)
        {
            ddShade.SelectedIndex = 0;
            ddShade.Focus();
        }
        else if (Size.Visible == true)
        {
            ddSize.SelectedIndex = 0;
            ddSize.Focus();
        }
        else if (Shape.Visible == true)
        {
            ddShape.SelectedIndex = 0;
            ddShape.Focus();
        }
        else if (Color.Visible == true)
        {
            ddColor.SelectedIndex = 0;
            ddColor.Focus();
        }
        else if (Design.Visible == true)
        {
            ddDesign.SelectedIndex = 0;
            ddDesign.Focus();
        }
    }
    private void FillGrid()
    {
        string strsql = "";
        DataSet ds;
        strsql = @"Select Distinct ICM.ID,IsNull(ICD.ID,0) ConsmptionID,CATEGORY_NAME CATEGORY,ITEM_NAME ITEMNAME,QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+
                   ShapeName+'  '+Case When ICM.UNITID=1 Then SizeMtr Else SizeFt End Description,Qty,Area From V_FinishedItemDetail VF,ItemConsumptionMaster ICM 
                   Left Outer Join ITEM_CONSUMPTION_DETAIL ICD ON ICM.ID=ICD.ID Where ICM.FinishedId=VF.Item_Finished_id And VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By ICM.Id";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGDetail.DataSource = ds;
        DGDetail.DataBind();
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOrderUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;
        }
        if (Quality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (Design.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (Color.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        if (Shape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
            {
                goto a;
            }
        }
        if (Size.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (Shade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
            {
                goto a;
            }
        }
        if (TxtArea.Visible == true)
        {

            if (UtilityModule.VALIDTEXTBOX(TxtArea) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtQty) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=1 And ProductCode Like '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyid + "";
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
    protected void TxtQty_TextChanged(object sender, EventArgs e)
    {
        if (ddSize.SelectedIndex > 0 && TxtQty.Text != "")
        {
            TxtArea.Text = (FillArea() * Convert.ToDouble(TxtQty.Text)).ToString();
        }
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblMessage.Text = "";
        int VarDetailId = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
        DataDeleteFromGrid(VarDetailId);
    }
    private void DataDeleteFromGrid(int VarDetailId)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[1];
            _arrpara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrpara[0].Value = VarDetailId;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_ItemConsumptionDelete", _arrpara);
            tran.Commit();
            FillGrid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmItemConsumption.aspx");
            tran.Rollback();
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string CellValue = e.Row.Cells[6].Text;
            if (CellValue == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Green;
                e.Row.Cells[1].BackColor = System.Drawing.Color.Green;
            }
            e.Row.Cells[6].Visible = false;
        }
    }
    protected void BtnReport_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/RptItemConsumptionDetail.rpt";
        Session["CommanFormula"] = "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    }
    protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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