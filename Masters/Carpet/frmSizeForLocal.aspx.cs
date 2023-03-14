using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class frmSizeForLocal : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddunit, "Select UnitId,UnitName From Unit Where UnitTypeId=1 Order By UnitId", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref ddshape, "select ShapeId,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order by ShapeName", true, "--Select--");
            ddunit.SelectedIndex = 1;
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
            }
            UnitDependControls();
            txtsize.Focus();
            txtheightFt.Text = "0";
            txtheightMtr.Text = "0";
            txtid.Text = "0";
            fill_grid();
            lablechange();
        }
        lbl.Visible = false;
        int VarCompanyType = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyType From MasterSetting"));
        if (VarCompanyType == 0)
        {
            Tr1.Visible = false;
            Tr2.Visible = false;
            Tr3.Visible = false;
            Tr4.Visible = false;
        }
        else if (VarCompanyType == 1)
        {
            //Tr1.Visible = true;
            //Tr2.Visible = true;
            //Tr3.Visible = true;
            //Tr4.Visible = false;
            Tr1.Visible = false;
            Tr2.Visible = false;
            Tr3.Visible = false;
            Tr4.Visible = false;
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblshapeyname.Text = ParameterList[3];
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
            string strsql = @"Select S.Sizeid as Sr_No,Sh.ShapeName,S.SizeFt As SizeType,S.WidthFt As Size
                            FROM Size S INNER JOIN Unit U ON S.UnitId=U.UnitId INNER JOIN Shape Sh ON 
                            S.Shapeid=Sh.ShapeId Where SH.Shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " Order By S.SizeFt";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmSizeForLocal.aspx");
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
        FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
    }
    protected void txtlengthFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
    }
    protected void txtheightFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
    }
    private void FtAreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarHeightNew, TextBox VarAreaNew, TextBox VarVolumeNew, int VarFactor)
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
        }
        InchHeight = (FootHeight * 12) + FootHeightInch;
        VarVolume = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth) * Convert.ToDouble(InchHeight)) / 1728, 4);
        VarVolumeNew.Text = Convert.ToString(VarVolume);
    }
    //------------------Mtr Format------------

    protected void txtwidthMtr_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
        {
            AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
        }
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        {
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        }
        txtlengthFt.Focus();
        txtlengthMtr.Focus();
    }
    protected void txtlengthMtr_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
        {
            AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
        }
        if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        {
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        }
    }

    private void AreaMtSq(double txtL, double txtW, TextBox VarAreaNew)
    {
        Double Area = (txtL * txtW) / 10000;
        VarAreaNew.Text = Convert.ToString(Area);
    }

    private void VolumeMtSq(double txtL, double txtW, double txtH)
    {
        Double Volume = Math.Round((txtL * txtW * txtH) / 1000000, 4);
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
        if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        {
            ConvertMtrToFt(txtwidthMtr, txtwidthFt);
            ConvertMtrToFt(txtlengthMtr, txtlengthFt);
            ConvertMtrToFt(txtheightMtr, txtheightFt);
            FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
            txtlengthInch.Text = (Math.Round(Convert.ToDouble(txtlengthMtr.Text) / 2.54, 0)).ToString();
            txtwidthInch.Text = (Math.Round(Convert.ToDouble(txtwidthMtr.Text) / 2.54, 0)).ToString();
            txtheightInch.Text = txtheightMtr.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightMtr.Text) / 2.54, 0)).ToString();
            VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(txtheightInch.Text));
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        {
            //CalculateArea();
            ConvertFtToMtr(txtlengthFt, txtlengthMtr);
            ConvertFtToMtr(txtwidthFt, txtwidthMtr);
            ConvertFtToMtr(txtheightFt, txtheightMtr);
            if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
            {
                AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
            }
            if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
            {
                VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
            }
            txtlengthInch.Text = (Math.Round(Convert.ToDouble(txtlengthMtr.Text) / 2.54, 0)).ToString();
            txtwidthInch.Text = (Math.Round(Convert.ToDouble(txtwidthMtr.Text) / 2.54, 0)).ToString();
            txtheightInch.Text = txtheightMtr.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightMtr.Text) / 2.54, 0)).ToString();
            VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
            VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(txtheightInch.Text));
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
        {
            txtlengthMtr.Text = (Math.Round(Convert.ToDouble(txtlengthInch.Text) * 2.54, 0)).ToString();
            txtwidthMtr.Text = (Math.Round(Convert.ToDouble(txtwidthInch.Text) * 2.54, 0)).ToString();
            txtheightMtr.Text = txtheightInch.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightInch.Text) * 2.54, 0)).ToString();
            if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
            {
                AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
            }
            if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
            {
                VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
            }
            ConvertMtrToFt(txtwidthMtr, txtwidthFt);
            ConvertMtrToFt(txtlengthMtr, txtlengthFt);
            ConvertMtrToFt(txtheightMtr, txtheightFt);
            FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        }
        Prod_Export_length_Width();
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
            txtwidthInch.Text = "";
            txtlengthInch.Text = "";
            txtheightInch.Text = "";
            txtAreaInch.Text = "";
            txtVolInch.Text = "";
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        {
            txtwidthMtr.Text = "";
            txtlengthMtr.Text = "";
            txtheightMtr.Text = "";
            txtAreaMtr.Text = "";
            txtVolMtr.Text = "";
            txtwidthInch.Text = "";
            txtlengthInch.Text = "";
            txtheightInch.Text = "";
            txtAreaInch.Text = "";
            txtVolInch.Text = "";
            txtwidthMtr.Focus();
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
        {
            txtwidthMtr.Text = "";
            txtlengthMtr.Text = "";
            txtheightMtr.Text = "";
            txtAreaMtr.Text = "";
            txtVolMtr.Text = "";
            txtwidthFt.Text = "";
            txtlengthFt.Text = "";
            txtheightFt.Text = "";
            txtAreaFt.Text = "";
            txtVolFt.Text = "";
            txtwidthInch.Focus();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
       // CHECKVALIDCONTROL();
        lblMessage.Text = "";
        Store_Data();
        btndelete.Visible = false;

        btnSave.Text = "Save";
        Session["ReportPath"] = "Reports/size.rpt";
        Session["CommanFormula"] = "";

    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddunit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtwidthFt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtlengthFt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtheightFt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtwidthMtr) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtlengthMtr) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtheightMtr) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtwidthInch) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtlengthInch) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtheightInch) == false)
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
    private void Store_Data()
    {
        // CheckDuplicateData();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string Str = "Select * from Size Where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];

            Str = Str + " And  WidthMtr=" + Convert.ToDouble(TxtLocalSize.Text) + " And SizeMtr='" + TxtSizeType.Text + "' And LengthMtr=" + Convert.ToDouble(TxtLocalSize.Text) + " And HeightMtr=" + Convert.ToDouble(TxtLocalSize.Text) + "";

            Str = Str + " And WidthFt=" + Convert.ToDouble(TxtLocalSize.Text) + " And LengthFt=" + Convert.ToDouble(TxtLocalSize.Text) + " And HeightFt=" + Convert.ToDouble(TxtLocalSize.Text) + "";

            Str = Str + " And WidthInch=" + Convert.ToDouble(TxtLocalSize.Text) + " And LengthInch=" + Convert.ToDouble(TxtLocalSize.Text) + " And HeightInch=" + Convert.ToDouble(TxtLocalSize.Text) + "";
         
            if (txtid.Text != "0")
            {
                Str = Str + " And Sizeid<>" + Convert.ToInt32(txtid.Text) + "";
            }
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblMessage.Text = "Size AllReady Exists";
                CalculateClear();
            }
            else
            {
                SqlParameter[] _arrPara = new SqlParameter[31];
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
                _arrPara[15] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[16] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[17] = new SqlParameter("@WidthInch", SqlDbType.Float);
                _arrPara[18] = new SqlParameter("@LengthInch", SqlDbType.Float);
                _arrPara[19] = new SqlParameter("@HeightInch", SqlDbType.Float);
                _arrPara[20] = new SqlParameter("@AreaInch", SqlDbType.Float);
                _arrPara[21] = new SqlParameter("@VolumeInch", SqlDbType.Float);
                _arrPara[22] = new SqlParameter("@SizeInch", SqlDbType.NVarChar, 50);
                _arrPara[23] = new SqlParameter("@ProdLengthFt", SqlDbType.Float);
                _arrPara[24] = new SqlParameter("@ProdWidthFt", SqlDbType.Float);
                _arrPara[25] = new SqlParameter("@ProdLengthMtr", SqlDbType.Float);
                _arrPara[26] = new SqlParameter("@ProdWidthMtr", SqlDbType.Float);
                _arrPara[27] = new SqlParameter("@ProdSizeFt", SqlDbType.NVarChar, 50);
                _arrPara[28] = new SqlParameter("@ProdSizeMtr", SqlDbType.NVarChar, 50);
                _arrPara[29] = new SqlParameter("@ProdAreaFt", SqlDbType.Float);
                _arrPara[30] = new SqlParameter("@ProdAreaMtr", SqlDbType.Float);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyno From MasterSetting"));
                if (Session["varCompanyId"] =="3")
                {
                    if (TxtLocalSize.Text == "0.00")
                    {
                        if ((TxtLocalSize.Text == "0" || TxtLocalSize.Text == "0.0" || TxtLocalSize.Text == "0.00") && (TxtLocalSize.Text == "0" || TxtLocalSize.Text == "0.0" || TxtLocalSize.Text == "0.00"))
                        {
                            _arrPara[1].Value = "";
                            _arrPara[2].Value = "";
                            _arrPara[22].Value = "";
                        }
                        else if (TxtLocalSize.Text == "0" || TxtLocalSize.Text == "0.0" || TxtLocalSize.Text == "0.00")
                        {
                            _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                            _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                            _arrPara[22].Value = TxtSizeType.Text.ToUpper();
                        }
                        else if (TxtLocalSize.Text == "0" || TxtLocalSize.Text == "0.0" || TxtLocalSize.Text == "0.00")
                        {
                            _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                            _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                            _arrPara[22].Value = TxtSizeType.Text.ToUpper();
                        }
                    }
                    else
                    {
                        _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                        _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                        _arrPara[22].Value = TxtSizeType.Text.ToUpper();
                    }
                }
                else if (Session["varCompanyId"] =="6")
                {
                    if (TxtLocalSize.Text == "0.00")
                    {
                        _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                        _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                    }
                    else
                    {
                        _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                        _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                    }
                }
                else
                {
                    _arrPara[1].Value = TxtSizeType.Text.ToUpper();
                    _arrPara[2].Value = TxtSizeType.Text.ToUpper();
                    _arrPara[22].Value = TxtSizeType.Text.ToUpper();
                }
                _arrPara[3].Value = ddunit.SelectedValue;
                _arrPara[4].Value = ddshape.SelectedValue;
                _arrPara[5].Value = Convert.ToDouble(TxtLocalSize.Text);
                _arrPara[6].Value = Convert.ToDouble(TxtLocalSize.Text);
                _arrPara[7].Value = TxtLocalSize.Text;
                _arrPara[8].Value = Convert.ToDouble(TxtLocalSize.Text);
                _arrPara[9].Value = Convert.ToDouble(TxtLocalSize.Text);
                _arrPara[10].Value = TxtLocalSize.Text;
                _arrPara[11].Value = txtAreaFt.Text != "" ? Convert.ToDouble(txtAreaFt.Text) : 0;
                _arrPara[12].Value = txtVolFt.Text != "" ? Convert.ToDouble(txtVolFt.Text) : 0;
                _arrPara[13].Value = txtAreaMtr.Text != "" ? Convert.ToDouble(txtAreaMtr.Text) : 0;
                _arrPara[14].Value = txtVolMtr.Text != "" ? Convert.ToDouble(txtVolMtr.Text) : 0; 
                _arrPara[15].Value = Session["varuserid"].ToString();
                _arrPara[16].Value = Session["varCompanyId"].ToString();
                _arrPara[17].Value = TxtLocalSize.Text;
                _arrPara[18].Value = TxtLocalSize.Text;
                _arrPara[19].Value = TxtLocalSize.Text;
                _arrPara[20].Value = txtAreaInch.Text != "" ? Convert.ToDouble(txtAreaInch.Text) : 0;
                _arrPara[21].Value = txtAreaInch.Text != "" ? Convert.ToDouble(txtAreaInch.Text) : 0;

                _arrPara[23].Value = TxtLengthProdFt.Text != "" ? Convert.ToDouble(TxtLengthProdFt.Text) : 0;
                _arrPara[24].Value = TxtWidthProdFt.Text != "" ? Convert.ToDouble(TxtWidthProdFt.Text) : 0;
                _arrPara[25].Value = TxtLengthCm.Text != "" ? Convert.ToDouble(TxtLengthCm.Text) : 0;
                _arrPara[26].Value = TxtWidthCm.Text != "" ? Convert.ToDouble(TxtWidthCm.Text) : 0;
                _arrPara[27].Value = TxtLocalSize.Text;
                _arrPara[28].Value = TxtLocalSize.Text;
                _arrPara[29].Value = TxtAreaProdSqYD.Text != "" ? Convert.ToDouble(TxtAreaProdSqYD.Text) : 0;
                _arrPara[30].Value = TxtAreaProdSqMtr.Text != "" ? Convert.ToDouble(TxtAreaProdSqMtr.Text) : 0;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_SIZE", _arrPara);
                UnitDependControls();
                txtid.Text = "0";
                lbl.Visible = true;
                lbl.Text = "Save Details...........";
            }
        }
        catch (Exception ex)
        { UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmSizeForLocal.aspx"); }
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
        string strsql = @"Select * from Size Where UnitId=" + ddunit.SelectedValue + " And Shapeid=" + ddshape.SelectedValue + " And Width='" + txtwidthFt.Text + "' And Length='" + txtlengthFt.Text + "' And HeightFt='" + VarHeight + "' and SizeID !='" + txtid.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
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
            TxtSizeType.Text="";
        TxtLocalSize.Text="";
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
    }
    protected void gdSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string id = gdSize.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Size WHERE SizeId=" + id + " And MasterCompanyid=" + Session["varCompanyId"] +"");
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
                txtAreaFt.Text = ds.Tables[0].Rows[0]["AreaFt"].ToString();
                txtVolFt.Text = ds.Tables[0].Rows[0]["VolumeMtr"].ToString();
                txtwidthMtr.Text = ds.Tables[0].Rows[0]["WidthMtr"].ToString();
                txtlengthMtr.Text = ds.Tables[0].Rows[0]["LengthMtr"].ToString();
                txtheightMtr.Text = ds.Tables[0].Rows[0]["HeightMtr"].ToString();
                txtAreaMtr.Text = ds.Tables[0].Rows[0]["AreaMtr"].ToString();
                txtVolMtr.Text = ds.Tables[0].Rows[0]["VolumeMtr"].ToString();
                txtwidthInch.Text = ds.Tables[0].Rows[0]["WidthInch"].ToString();
                txtlengthInch.Text = ds.Tables[0].Rows[0]["LengthInch"].ToString();
                txtheightInch.Text = ds.Tables[0].Rows[0]["HeightInch"].ToString();
                txtAreaInch.Text = ds.Tables[0].Rows[0]["AreaInch"].ToString();
                txtVolInch.Text = ds.Tables[0].Rows[0]["VolumeInch"].ToString();
                TxtLengthProdFt.Text = ds.Tables[0].Rows[0]["ProdLengthFt"].ToString();
                TxtWidthProdFt.Text = ds.Tables[0].Rows[0]["ProdWidthFt"].ToString();
                TxtLengthCm.Text = ds.Tables[0].Rows[0]["ProdLengthMtr"].ToString();
                TxtWidthCm.Text = ds.Tables[0].Rows[0]["ProdWidthMtr"].ToString();
                TxtAreaProdSqYD.Text = ds.Tables[0].Rows[0]["ProdAreaFt"].ToString();
                TxtAreaProdSqMtr.Text = ds.Tables[0].Rows[0]["ProdAreaMtr"].ToString();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmSizeForLocal.aspx");
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
        txtlengthFt.Text = "";
        txtwidthFt.Text = "";
        txtheightFt.Text = "0";
        txtAreaFt.Text = "";
        txtVolFt.Text = "";
        txtlengthMtr.Text = "";
        txtwidthMtr.Text = "";
        txtheightMtr.Text = "0";
        txtAreaMtr.Text = "";
        txtVolMtr.Text = "";
        txtlengthInch.Text = "";
        txtwidthInch.Text = "";
        txtheightInch.Text = "0";
        txtAreaInch.Text = "";
        txtVolInch.Text = "";
        txtAreaFt.Enabled = false;
        txtVolFt.Enabled = false;
        txtAreaMtr.Enabled = false;
        txtVolMtr.Enabled = false;
        TxtLengthProdFt.Text = "";
        TxtWidthProdFt.Text = "";
        TxtLengthCm.Text = "";
        TxtWidthCm.Text = "";
        TxtAreaProdSqYD.Text = "";
        TxtAreaProdSqMtr.Text = "";
        if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        {
            txtwidthMtr.Enabled = true;
            txtlengthMtr.Enabled = true;
            txtheightMtr.Enabled = true;
            txtwidthFt.Enabled = false;
            txtlengthFt.Enabled = false;
            txtheightFt.Enabled = false;
            txtwidthInch.Enabled = false;
            txtlengthInch.Enabled = false;
            txtheightInch.Enabled = false;
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
            txtwidthInch.Enabled = false;
            txtlengthInch.Enabled = false;
            txtheightInch.Enabled = false;
            txtwidthFt.Focus();
        }
        else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
        {
            txtwidthMtr.Enabled = false;
            txtlengthMtr.Enabled = false;
            txtheightMtr.Enabled = false;
            txtwidthFt.Enabled = false;
            txtlengthFt.Enabled = false;
            txtheightFt.Enabled = false;
            txtwidthInch.Enabled = true;
            txtlengthInch.Enabled = true;
            txtheightInch.Enabled = true;
            txtwidthInch.Focus();
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SIZE_ID from ITEM_PARAMETER_MASTER where MasterCompanyId=" +Session["varCompanyId"] + " And SIZE_ID=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Size where MasterCompanyid=" + Session["varCompanyid"] + " And   SizeId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Size'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                lbl.Visible = true;
                lbl.Text = "Value deleted...";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Value in Use...";

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmSizeForLocal.aspx");
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
        txtid.Text = "0";
        ClearAll();
    }
    protected void btnclose0_Click(object sender, EventArgs e)
    {

    }
    protected void txtwidthInch_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthInch.Text != "" && txtwidthInch.Text != "")
        {
            var VarHeight = txtheightInch.Text == "" ? "0" : txtheightInch.Text;
            VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(VarHeight));
        }
    }
    protected void txtlengthInch_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthInch.Text != "" && txtwidthInch.Text != "")
        {
            var VarHeight = txtheightInch.Text == "" ? "0" : txtheightInch.Text;
            VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(VarHeight));
        }
    }
    protected void txtheightInch_TextChanged(object sender, EventArgs e)
    {
        if (txtlengthInch.Text != "" && txtwidthInch.Text != "")
        {
            var VarHeight = txtheightInch.Text == "" ? "0" : txtheightInch.Text;
            VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(VarHeight));
        }
    }
    private void VolumeINCHSq(double txtL, double txtW, double txtH)
    {
        Double Area = (txtL * txtW);
        txtAreaInch.Text = Convert.ToString(Area);
        Double Volume = Math.Round((txtL * txtW * txtH), 4);
        txtVolInch.Text = Convert.ToString(Volume);
    }
    protected void TxtWidthProdFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //ConvertFtToMtr(TxtWidthProdFt, TxtWidthCm);
        //if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        //{
        //    AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        //}
    }
    protected void TxtLengthProdFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //ConvertFtToMtr(TxtLengthProdFt, TxtLengthCm);
        //if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        //{
        //    AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        //}
    }
    protected void TxtWidthCm_TextChanged(object sender, EventArgs e)
    {
        //if (TxtWidthCm.Text != "")
        //{
        //    ConvertMtrToFt(TxtWidthCm, TxtWidthProdFt);
        //    FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //}
        if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        {
            AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        }
    }
    protected void TxtLengthCm_TextChanged(object sender, EventArgs e)
    {
        //if (TxtLengthCm.Text != "")
        //{
        //    ConvertMtrToFt(TxtLengthCm, TxtLengthProdFt);
        //    FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //}
        if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        {
            AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        }
    }
    private void FtAreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarAreaNew, int VarFactor)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        double VarArea = 0;
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
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4);
        int VarProductionArea = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ProductionArea From MasterSetting"));
        if (VarProductionArea == 0)
        {
            VarAreaNew.Text = Convert.ToString(VarArea);
        }
        else
        {
            Str = string.Format("{0:#0.0000#}", VarArea);
            double IntegerValue = Convert.ToDouble(Str.Split('.')[0]);
            Str = (Convert.ToDouble(Str.Split('.')[1]) / (0.0625 * 10000)).ToString();
            Str = string.Format("{0:#0.0000#}", Convert.ToDouble(Str));
            string Str1 = string.Format("{0:#0.0000#}", Convert.ToDouble(Str.Split('.')[1]));
            double TotalGirhValue = 0;
            TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]);
            if (Convert.ToDouble(Str1) > 5999)
            {
                TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]) + 1;
            }
            double DecimalValue = 0;
            if (TotalGirhValue > 0)
            {
                DecimalValue = TotalGirhValue * 0.0625;
            }
            VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);
        }
    }
    private void Prod_Export_length_Width()
    {
        if (txtwidthFt.Text != "")
        {
            TxtWidthProdFt.Text = txtwidthFt.Text;
        }
        if (txtlengthFt.Text != "")
        {
            TxtLengthProdFt.Text = txtlengthFt.Text;
        }
        if (txtwidthMtr.Text != "")
        {
            TxtWidthCm.Text = txtwidthMtr.Text;
        }
        if (txtlengthMtr.Text != "")
        {
            TxtLengthCm.Text = txtlengthMtr.Text;
        }
        if (txtAreaFt.Text != "")
        {
            FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        }
        if (txtlengthMtr.Text != "")
        {
            TxtAreaProdSqMtr.Text = txtAreaMtr.Text;
        }
    }
    private void ConvertMtrToFt(TextBox VarWidthNew, TextBox VarReplaceWidthNew)
    {
        int i;
        string X, Y, Z;
        double a, b, VarWidth;
        if (VarWidthNew.Text != "")
        {
            VarWidth = Convert.ToDouble(VarWidthNew.Text);
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
            VarReplaceWidthNew.Text = Z;
        }
    }
    private void ConvertFtToMtr(TextBox VarWidthFtNew, TextBox VarReplaceWidthMtrNew)
    {
        string Str;
        int LengthMtr, LengthCm;
        double VarLength, TotalLengthCm;
        if (VarWidthFtNew.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthFtNew.Text));
            if (VarWidthFtNew.Text != "")
            {
                LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
                LengthCm = Convert.ToInt32(Str.Split('.')[1]);
                TotalLengthCm = (Convert.ToDouble(LengthMtr * 12) + Convert.ToDouble(LengthCm)) * 2.54;
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TotalLengthCm));
                VarLength = Convert.ToInt32(Str.Split('.')[0]);
                VarReplaceWidthMtrNew.Text = Convert.ToString(VarLength);
            }
        }
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