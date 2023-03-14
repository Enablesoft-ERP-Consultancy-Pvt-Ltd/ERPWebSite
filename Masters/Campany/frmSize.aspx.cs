using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class frmSize : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddunit, "select UnitId,UnitName from Unit  order by UnitName", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref ddshape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShapeName", true, "--Select--");
            ddunit.SelectedIndex = 1;
            UnitDependControls();
            txtsize.Focus();
            txtheightFt.Text = "0";
            txtheightMtr.Text = "0";
            txtid.Text = "0";
            //fill_grid();
        }
    }
    private void fill_grid()
    {
        gdSize.DataSource = Fill_Grid_Data();
        gdSize.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT S.*,Sh.ShapeName FROM Size S INNER JOIN Unit U ON S.UnitId=U.UnitId INNER JOIN Shape Sh ON 
                             S.Shapeid=Sh.ShapeId Where SH.Shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " Order by SizeId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSize.aspx");
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
    protected void txtwidthFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate();
    }
    protected void txtlengthFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate();
    }

    private void FtAreaCalculate()
    {
        int FootLength=0 ;
        int FootWidth=0;
        int FootHeight = 0;
        int FootLengthInch=0;
        int FootWidthInch=0;
        int FootHeightInch = 0;
        int InchLength=0;
        int InchWidth=0;
        int InchHeight = 0;
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";
        if (txtlengthFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
            if (txtlengthFt.Text != "")
            {
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootLengthInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                txtlengthFt.Text = "";
                txtlengthFt.Focus();
            }
        }
        if (txtwidthFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text));
            if (txtwidthFt.Text != "")
            {
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootWidthInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                txtwidthFt.Text = "";
                txtwidthFt.Focus();
            }
        }
        InchLength = (FootLength * 12) + FootLengthInch;
        InchWidth = (FootWidth * 12) + FootWidthInch;
        VarArea = Math .Round ((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144,4);
        txtAreaFt.Text = Convert.ToString (VarArea);
       if (txtheightFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtheightFt.Text));
            if (txtheightFt.Text != "")
            {
                FootHeight = Convert.ToInt32(Str.Split('.')[0]);
                FootHeightInch= Convert.ToInt32(Str.Split('.')[1]);
            }
            if (FootHeightInch > 11)
            {
                lblMessage.Text = "Inch value must be less than 12";
                txtheightFt .Text = "";
                txtheightFt.Focus();
            }
        }
        InchHeight = (FootHeight * 12) + FootHeightInch;
        VarVolume = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth) * Convert.ToDouble(InchHeight)) / 1728, 4);
        txtVolFt.Text = Convert.ToString(VarVolume);
    }
    protected void txtheightFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate();
    }
    //------------------Mtr Format------------
    protected void txtwidthMtr_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthMtr.Text !="" && txtwidthMtr.Text !="")
        {
            AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text));
        }
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        {
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        }
    }
    protected void txtlengthMtr_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
        {
            AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text));
        }
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        {
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        }
    }
    private void AreaMtSq(double txtL,double txtW)
    {
        Double Area=(txtL*txtW)/10000;
        txtAreaMtr.Text = Convert .ToString (Area);
    }
    private void VolumeMtSq(double txtL, double txtW, double txtH)
    {
        Double Volume = Math.Round((txtL * txtW * txtH) / 1000000,4);
        txtVolMtr.Text = Convert.ToString(Volume);
    }
    protected void txtheightMtr_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        {
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        }
    }
    protected void BtnCalCulate_Click(object sender, EventArgs e)
    {
        CalculateClear();
        if (txtwidthFt .Text =="")
        {
            ConvertMtrToFt();
            FtAreaCalculate();
        }
        else if (txtwidthMtr .Text == "" )
        {
            CalculateArea();
        }
    }
    protected void CalculateClear()
    {       
        if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        {
            txtwidthFt.Text = "";
            txtlengthFt.Text = "";
            txtheightFt.Text = "";
            txtAreaFt.Text = "";
            txtVolFt.Text = "";
            txtwidthFt.Focus();
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        {
            txtwidthMtr.Text = "";
            txtlengthMtr.Text = "";
            txtheightMtr.Text = "";
            txtAreaMtr.Text = "";
            txtVolMtr.Text = "";
            txtwidthMtr.Focus();
        }
    }
    private void ConvertMtrToFt()
    {
        int i;
        string X, Y, Z;
        double a, b, VarWidth, VarLength, VarHeight;

        //.......................For Width CalCulate
        if (txtwidthMtr.Text != "")
        {
            VarWidth = Convert.ToDouble(txtwidthMtr.Text);
            a = Math.Round(VarWidth / 2.54, 0);
            b = a / 12;
            X = Convert.ToString(b);
            i = Convert.ToInt32(X.Split('.')[0]);
            b = a % 12;
            if (b < 10)
            {
                Y = "0" + Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            else
            {
                Y = Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            txtwidthFt.Text = Z;
        }

        //.......................For Lenght CalCulate

        if (txtlengthMtr.Text != "")
        {
            VarLength = Convert.ToDouble(txtlengthMtr.Text);
            a = Math.Round(VarLength / 2.54, 0);
            b = a / 12;
            X = Convert.ToString(b);
            i = Convert.ToInt32(X.Split('.')[0]);
            b = a % 12;
            if (b < 10)
            {
                Y = "0" + Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            else
            {
                Y = Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            txtlengthFt.Text = Z;
        }
        //.......................For Height CalCulate
        if (txtheightMtr.Text != "")
        {
            VarHeight = Convert.ToDouble(txtheightMtr.Text);
            a = Math.Round(VarHeight / 2.54, 0);
            b = a / 12;
            X = Convert.ToString(b);
            i = Convert.ToInt32(X.Split('.')[0]);
            b = a % 12;
            if (b < 10)
            {
                Y = "0" + Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            else
            {
                Y = Convert.ToString(b);
                Z = Convert.ToString(i) + "." + Y;
            }
            txtheightFt.Text = Z;
        }
    }
    private void  CalculateArea()
    {
        string Str;
        int LengthMtr, LengthCm,WidthMtr, WidthCm;
        double VarLength, VarWidth, TotalLengthCm, TotalWidthCm;

        // -------------For --Length ------------------------
        if (txtlengthFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
            if (txtlengthFt.Text != "")
            {
                LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
                LengthCm = Convert.ToInt32(Str.Split('.')[1]);
                TotalLengthCm = (Convert.ToDouble(LengthMtr * 12) + Convert.ToDouble(LengthCm)) * 2.54;
                VarLength = Math.Round(TotalLengthCm, 0);
                txtlengthMtr.Text = Convert.ToString(VarLength);
            }
        }
        // -------------For -- Width ------------------------
        if (txtwidthFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text));
            if (txtwidthFt.Text != "")
            {
                WidthMtr = Convert.ToInt32(Str.Split('.')[0]);
                WidthCm = Convert.ToInt32(Str.Split('.')[1]);
                TotalWidthCm = (Convert.ToDouble(WidthMtr * 12) + Convert.ToDouble(WidthCm)) * 2.54;
                VarWidth = Math.Round(TotalWidthCm, 0);
                txtwidthMtr.Text = Convert.ToString(VarWidth);
            }
        }
        // -------------For -- Height ------------------------
        if (txtheightFt.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(txtheightFt.Text));
            if (txtheightFt.Text != "")
            {
                WidthMtr = Convert.ToInt32(Str.Split('.')[0]);
                WidthCm = Convert.ToInt32(Str.Split('.')[1]);
                TotalWidthCm = (Convert.ToDouble(WidthMtr * 12) + Convert.ToDouble(WidthCm)) * 2.54;
                VarWidth = Math.Round(TotalWidthCm, 0);
                txtheightMtr.Text = Convert.ToString(VarWidth);
            }
        }
        AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text));
        VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Store_Data();
        btndelete.Visible = false;
        btnSave.Text = "Save";
    }
    private void Store_Data()
    {
       // CheckDuplicateData();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                if (ddunit.SelectedIndex > 0 && ddshape.SelectedIndex > 0 && txtwidthFt.Text != "" && txtlengthFt.Text != "" && txtwidthMtr.Text != "" && txtlengthMtr.Text != "")
                {
                    string Str = "Select * from Size Where MasterCompanyId=" + Session["varCompanyId"] + " And  Unitid=" + ddunit.SelectedValue + " And Shapeid=" + ddshape.SelectedValue + " And WidthFt=" + Convert.ToDouble(txtwidthFt.Text) + " And LengthFt=" + Convert.ToDouble(txtlengthFt.Text) + " And  WidthMtr=" + Convert.ToDouble(txtwidthMtr.Text) + " And LengthMtr=" + Convert.ToDouble(txtlengthMtr.Text) + "";
                    if (txtid.Text !="0")
                    {
                        Str = Str + " And Sizeid<>" + Convert .ToInt32 (txtid.Text) + "";
                    }
                    DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        lblMessage.Text = "Size AllReady Exists";
                        CalculateClear();
                    }
                    else
                    {
                        SqlParameter[] _arrPara = new SqlParameter[15];
                        _arrPara[0] = new SqlParameter("@SizeId", SqlDbType.Int);
                        _arrPara[1] = new SqlParameter("@SizeFt", SqlDbType.NVarChar, 50);
                        _arrPara[2] = new SqlParameter("@SizeMtr", SqlDbType.NVarChar, 50);
                        _arrPara[3] = new SqlParameter("@UnitId", SqlDbType.Int);
                        _arrPara[4] = new SqlParameter("@Shapeid", SqlDbType.Int);
                        _arrPara[5] = new SqlParameter("@WidthFt", SqlDbType.Float);
                        _arrPara[6] = new SqlParameter("@LengthFt", SqlDbType.Float);
                        _arrPara[7] = new SqlParameter("@HeightFt", SqlDbType.Float);
                        _arrPara[8] = new SqlParameter("@WidthMtr", SqlDbType.Float);
                        _arrPara[9] = new SqlParameter("@LengthMtr", SqlDbType.Float);
                        _arrPara[10] = new SqlParameter("@HeightMtr", SqlDbType.Float);
                        _arrPara[11] = new SqlParameter("@AreaFt", SqlDbType.Float);
                        _arrPara[12] = new SqlParameter("@VolumeFt", SqlDbType.Float);
                        _arrPara[13] = new SqlParameter("@AreaMtr", SqlDbType.Float);                        
                        _arrPara[14] = new SqlParameter("@VolumeMtr", SqlDbType.Float);

                        _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                        _arrPara[1].Value = txtwidthFt.Text + 'x' + txtlengthFt.Text;
                        _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text; ;
                        _arrPara[3].Value = ddunit.SelectedValue;
                        _arrPara[4].Value = ddshape.SelectedValue;
                        _arrPara[5].Value = Convert.ToDouble(txtwidthFt.Text);
                        _arrPara[6].Value = Convert.ToDouble(txtlengthFt.Text);
                        _arrPara[7].Value = txtheightFt.Text != "" ? Convert.ToDouble(txtheightFt.Text) : 0;
                        _arrPara[8].Value = Convert.ToDouble(txtwidthMtr.Text);
                        _arrPara[9].Value = Convert.ToDouble(txtlengthMtr.Text);
                        _arrPara[10].Value = txtheightMtr.Text != "" ? Convert.ToDouble(txtheightMtr.Text) : 0;
                        _arrPara[11].Value = Convert.ToDouble(txtAreaFt.Text);
                        _arrPara[12].Value = Convert.ToDouble(txtVolFt.Text);
                        _arrPara[13].Value = Convert.ToDouble(txtAreaMtr.Text);
                        _arrPara[14].Value = Convert.ToDouble(txtVolMtr.Text);

                        SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_SIZE", _arrPara);
                        UnitDependControls();
                        txtid.Text = "0";
                        lblMessage.Text = "Data have been Saved Successfully";
                    }
                }
                else
                {
                    lblMessage.Text = "Some Field Are Mandatory..................";
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSize.aspx");
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
            fill_grid();
    }
    
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        Double VarHeight = txtheightFt.Text != "" ? Convert.ToInt32(txtheightFt.Text) : 0;
        string strsql = @"Select * from Size Where MasterCompanyId=" + Session["varCompanyId"] + " And UnitId=" + ddunit.SelectedValue + " And Shapeid=" + ddshape.SelectedValue + " And Width='" + txtwidthFt.Text + "' And Length='" + txtlengthFt.Text + "' And HeightFt='" + VarHeight + "' and SizeID !='" + txtid.Text + "'";
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl.Visible = true;
            lbl.Text = "Design AlReady Exits........";
            txtwidthFt.Text = "";
            txtlengthFt.Text = "";
            txtwidthFt.Focus();
        }
        else
        {
            lbl.Visible = false;
        }
    }

    private void ClearAll()
    {
        txtid.Text = "0";
        txtwidthFt.Text = "";
        ddunit.SelectedIndex = -1;
        ddshape.SelectedIndex = -1;
        txtlengthFt.Text = "";
        txtheightFt.Text = "";
        btnSave.Text = "Save";
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
    }    

protected void  gdSize_SelectedIndexChanged(object sender, EventArgs e)
{
    lblMessage.Text = "";
    string id = gdSize.SelectedDataKey.Value.ToString();
    //Session["id"] = id;
    ViewState["id"] = id;
    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Size WHERE MasterCompanyId=" + Session["varCompanyId"] + " And SizeId=" + id);
    try
    {
        if (ds.Tables[0].Rows.Count == 1)
        {
            txtid.Text = ds.Tables[0].Rows[0]["SizeId"].ToString();
            ddunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
            ddshape.SelectedValue = ds.Tables[0].Rows[0]["Shapeid"].ToString();

            txtwidthFt.Text = ds.Tables[0].Rows[0]["WidthFt"].ToString();            
            txtlengthFt.Text = ds.Tables[0].Rows[0]["LengthFt"].ToString();
            txtheightFt.Text = ds.Tables[0].Rows[0]["HeightFt"].ToString();

            txtwidthMtr.Text = ds.Tables[0].Rows[0]["WidthMtr"].ToString();
            txtlengthMtr.Text = ds.Tables[0].Rows[0]["LengthMtr"].ToString();
            txtheightMtr.Text = ds.Tables[0].Rows[0]["HeightMtr"].ToString();

            txtAreaFt.Text = ds.Tables[0].Rows[0]["AreaFt"].ToString();
            txtVolFt.Text = ds.Tables[0].Rows[0]["VolumeMtr"].ToString();
            txtAreaMtr.Text = ds.Tables[0].Rows[0]["AreaMtr"].ToString();
            txtVolMtr.Text = ds.Tables[0].Rows[0]["VolumeMtr"].ToString();
        }
    }
    catch (Exception ex)
    {
        UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSize.aspx");
    }
    finally
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
    }
    btndelete.Visible = true;
    btnSave.Text = "Update";
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

    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtid.Text = "0";
        UnitDependControls();
    }    

    private void UnitDependControls()
    {
        txtheightFt.Text = "0";
        txtheightMtr.Text = "0";
        txtwidthFt.Text = "";
        txtlengthFt.Text = "";        
        txtAreaFt.Text = "";
        txtVolFt.Text = "";
        txtwidthMtr.Text = "";
        txtlengthMtr.Text = "";        
        txtAreaMtr.Text = "";
        txtVolMtr.Text = "";
        
        txtAreaFt.Enabled = false;
        txtVolFt.Enabled = false;
        txtAreaMtr.Enabled = false;
        txtVolMtr.Enabled = false;
        if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        {
            txtwidthFt.Enabled = false;
            txtlengthFt.Enabled = false;
            txtheightFt.Enabled = false;
            txtwidthMtr.Enabled = true;
            txtlengthMtr.Enabled = true;
            txtheightMtr.Enabled = true;
            txtwidthMtr.Focus();
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        {
            txtwidthMtr.Enabled = false;
            txtlengthMtr.Enabled = false;
            txtheightMtr.Enabled = false;
            txtwidthFt.Enabled = true;
            txtlengthFt.Enabled = true;
            txtheightFt.Enabled = true;
            txtwidthFt.Focus();
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtid.Text = "0";
        fill_grid();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SIZE_ID from ITEM_PARAMETER_MASTER where MasterCompanyId=" + Session["varCompanyId"] + " And SIZE_ID=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Size where SizeId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Size'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
            }
            else
            {
                lbl.Text = "Value in Use...";
                
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSize.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        fill_grid();
        btndelete.Visible = false;
        btnSave.Text = "Save";
    }
    protected void gdSize_RowCreated(object sender, GridViewRowEventArgs e)
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