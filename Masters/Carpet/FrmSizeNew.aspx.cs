using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_FrmSizeNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select UnitTypeID,UnitType from UNIT_TYPE_MASTER
                           select UnitId,UnitName from unit where unittypeid =1
                          select ShapeId,ShapeName from shape where mastercompanyid=" + Session["vARcompanyid"] + "";
           DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddunittype, ds,0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds,1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddshape,ds,2 , true, "--Select Shape--");
            txtwidthFt.Text = "";
            txtlengthFt.Text = "";
            txtheightFt.Text = "";
            txtAreaFt.Text = "";
            txtVolFt.Text = "";
        }
    }
    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddunittype.SelectedValue != "1")
        {
            trsize.Visible = false;
            tdUnit.Visible = false;
            tdUnit1.Visible = false;
        }
        else 
        {
            trsize.Visible = true;
            tdUnit.Visible = true;
            tdUnit1.Visible = true;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lblMessage.Text == "")
        {
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@SizeId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Length", SqlDbType.Float);
            _arrPara[2] = new SqlParameter("@Width", SqlDbType.Float);
            _arrPara[3] = new SqlParameter("@Height", SqlDbType.Float);
            _arrPara[4] = new SqlParameter("@Area", SqlDbType.Float);
            _arrPara[5] = new SqlParameter("@Volume", SqlDbType.Float);
            _arrPara[6] = new SqlParameter("@SizeName", SqlDbType.NVarChar, 250);
            _arrPara[7] = new SqlParameter("@UnitTypeid", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@Unitid", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@ShapeId", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@userid", SqlDbType.Int);
            _arrPara[11] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
            if (btnSave.Text == "Save")
            {
                _arrPara[0].Value = 0;
            }
            else
            {
                _arrPara[0].Value = Session["Sizeid"];
            }
            _arrPara[1].Value = txtlengthFt.Text != "" ? txtlengthFt.Text : "0";
            _arrPara[2].Value = txtwidthFt.Text != "" ? txtwidthFt.Text : "0";
            _arrPara[3].Value = txtheightFt.Text != "" ? txtheightFt.Text : "0";
            _arrPara[4].Value = txtAreaFt.Text != "" ? txtAreaFt.Text : "0";
            _arrPara[5].Value = txtVolFt.Text != "" ? txtVolFt.Text : "0";
            _arrPara[6].Value = txtsizename.Text;
            _arrPara[7].Value = ddunittype.SelectedValue;
            _arrPara[8].Value = ddunit.SelectedValue;
            _arrPara[9].Value = ddshape.SelectedValue;
            _arrPara[10].Value = Session["varuserid"];
            _arrPara[11].Value = Session["varcompanyid"];
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SIZENew", _arrPara);
            FillGrid();
            refresh();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        refresh();
    }
    private void refresh()
    {
        txtwidthFt.Text = "";
        txtlengthFt.Text = "";
        txtheightFt.Text = "";
        txtAreaFt.Text = "";
        txtVolFt.Text = "";
        txtsizename.Text = "";
        Session["SIZEID"] = 0;
        btnSave.Text = "Save";
        lblMessage.Text = "";
    }
    private void FtinchAreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarHeightNew, TextBox VarAreaNew, TextBox VarVolumeNew, int VarFactor)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootHeight = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int FootHeightInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        int InchHeight = 0;
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";
        if (VarLengthNew.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text));
            if (VarLengthNew.Text != "")
            {
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootLengthInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                VarLengthNew.Text = "";
                VarLengthNew.Focus();
            }
        }
        if (VarWidthNew.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
            if (VarWidthNew.Text != "")
            {
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootWidthInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                VarWidthNew.Text = "";
                VarWidthNew.Focus();
            }
        }
            InchLength = (FootLength * 12) + FootLengthInch;
            InchWidth = (FootWidth * 12) + FootWidthInch;
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);
        VarAreaNew.Text = Convert.ToString(VarArea);

        if (VarHeightNew.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarHeightNew.Text));
            if (VarHeightNew.Text != "")
            {
                FootHeight = Convert.ToInt32(Str.Split('.')[0]);
                FootHeightInch = Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootHeightInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                VarHeightNew.Text = "";
                VarHeightNew.Focus();
            }
            InchHeight = (FootHeight * 12) + FootHeightInch;
            VarVolume = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth) * Convert.ToDouble(InchHeight)) / 1728, 4);
            VarVolumeNew.Text = Convert.ToString(VarVolume);
            txtheightFt.Focus();
        }
    }
    private void AreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarHeightNew, TextBox VarAreaNew, TextBox VarVolumeNew, int VarFactor)
    {
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";
        if (VarLengthNew.Text != "" && VarWidthNew.Text != "")
        {
            VarArea = Math.Round((Convert.ToDouble(VarLengthNew.Text) * Convert.ToDouble(VarWidthNew.Text)), 4);
            VarAreaNew.Text = Convert.ToString(VarArea);
        }
        else
        {
            VarAreaNew.Text = "0";
        }
        if (VarLengthNew.Text != "" && VarWidthNew.Text != "" && VarHeightNew.Text != "")
        {
            VarVolume = Math.Round((Convert.ToDouble(VarLengthNew.Text) * Convert.ToDouble(VarWidthNew.Text) * Convert.ToDouble(VarHeightNew.Text)), 4);
            VarVolumeNew.Text = Convert.ToString(VarVolume);
        }
        else
        {
            VarVolumeNew.Text = "0";
        }
        txtheightFt.Focus();
    }
    protected void txtwidthFt_TextChanged(object sender, EventArgs e)
        {
        if (ddunit.SelectedValue == "3")
        {
            lblMessage.Text = "";
            FtinchAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
        else
        {
            AreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
    }
    protected void txtlengthFt_TextChanged(object sender, EventArgs e)
    {
        if (ddunit.SelectedValue == "3")
        {
            lblMessage.Text = "";
            FtinchAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
        else
        {
            AreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
    }
    protected void txtheightFt_TextChanged(object sender, EventArgs e)
    {
        if (ddunit.SelectedValue == "3")
        {
            lblMessage.Text = "";
            FtinchAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
        else
        {
            AreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
    }
    private void FillGrid()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT SizeID,Length,Width,Height,Area,Volume,SizeName,UnitType,UnitName,ShapeName 
        FROM SIZENEW S INNER JOIN UNIT_TYPE_MASTER UT ON UT.UnitTypeID=S.UnitTypeid INNER JOIN UNIT U ON U.UNITID=U.UNITID inner join shape sh On sh.shapeid=s.shapeid  where u.unitid=" +ddunit.SelectedValue+" and s.shapeid="+ddshape.SelectedValue+"");
        gdSize.DataSource = ds;
        
        {
            gdSize.DataBind();
        }
    }
    protected void ddunit_SelectedIndexChanged1(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        FillGrid();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void gdSize_RowCreated(object sender, GridViewRowEventArgs e)
    {//Add CSS class on header row.
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
    protected void gdSize_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdSize, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gdSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hnsst.Value == "true")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Length,Width,Height,Area,Volume,SizeName,UnitTypeid,Unitid,ShapeId,userid,Mastercompanyid from SIZENEW where sizeid=" + gdSize.SelectedDataKey.Value + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddunittype.SelectedValue = ds.Tables[0].Rows[0]["UnitTypeid"].ToString();
                ddunit.SelectedValue = ds.Tables[0].Rows[0]["Unitid"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                txtlengthFt.Text = ds.Tables[0].Rows[0]["Length"].ToString();
                txtwidthFt.Text = ds.Tables[0].Rows[0]["Width"].ToString();
                txtheightFt.Text = ds.Tables[0].Rows[0]["Height"].ToString();
                txtAreaFt.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                txtVolFt.Text = ds.Tables[0].Rows[0]["Volume"].ToString();
                txtsizename.Text = ds.Tables[0].Rows[0]["SizeName"].ToString();
                btnSave.Text = "UPDATE";
                Session["SIZEID"] = gdSize.SelectedDataKey.Value;
            }
        }
    }
    protected void gdSize_RowDeleting1(object sender, GridViewDeleteEventArgs e)
    {
        if (hnsst.Value == "true")
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete from sizenew where sizeid=" + gdSize.DataKeys[e.RowIndex].Value + "");
            FillGrid();
        }
    }
}