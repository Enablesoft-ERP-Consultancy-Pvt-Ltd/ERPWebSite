using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_AddSize : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["varcompanyno"].ToString() != "6")
            {
                UtilityModule.ConditionalComboFill(ref ddunit, "Select UnitId,UnitName From Unit Where UnitTypeId=1 Order By UnitId", true, "--Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddunit, "Select UnitId,UnitName From Unit Where UnitTypeId=1 and unitid<>2 Order By UnitId", true, "--Select--");
            }
            UtilityModule.ConditionalComboFill(ref ddshape, "select ShapeId,ShapeName from Shape order by ShapeId", true, "--Select--");
            ddunit.SelectedIndex = 1;
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
            }

            if (Request.QueryString["ShapeID"] != "0")
            {
                ddshape.SelectedValue = Request.QueryString["ShapeID"];
            }
            if (Request.QueryString["UnitID"] != "0")
            {
                ddunit.SelectedValue = Request.QueryString["UnitID"];
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
            Tr1.Visible = true;
            Tr2.Visible = true;
            Tr3.Visible = true;
            Tr4.Visible = true;
        }
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "9":
                lblMtrWidth.Text = "CM Width";
                lblmtrlength.Text = "CM Length";
                lblmtrheight.Text = "CM Height";
                break;
        }
    }
    public void lablechange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string shape = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id  and  ps.parameter_id='5'").ToString();
            lblshapeyname.Text = shape;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            //LblError.Visible = true;
            //LblError.Text = "Data base errer..................";
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
            string strsql;
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                strsql = @"Select S.Sizeid as Sr_No,Sh.ShapeName,S.SizeFt,S.SizeMtr As SizeCM,S.WidthFt,S.LengthFt,S.AreaFt,S.WidthMtr as WidthCM,S.LengthMtr as LengthCM,S.AreaMtr,
                            S.HeightFt,S.VolumeFt,S.HeightMtr as HeightCM,S.VolumeMtr,isnull(SizeCode,'') as SizeCode FROM Size S INNER JOIN Unit U ON S.UnitId=U.UnitId INNER JOIN Shape Sh ON 
                            S.Shapeid=Sh.ShapeId Where SH.Shapeid=" + ddshape.SelectedValue + " Order By S.SizeFt";
            }
            else
            {
                strsql = @"Select S.Sizeid as Sr_No,Sh.ShapeName,S.SizeFt,S.SizeMtr,S.WidthFt,S.LengthFt,S.AreaFt,S.WidthMtr,S.LengthMtr,S.AreaMtr,
                            S.HeightFt,S.VolumeFt,S.HeightMtr,S.VolumeMtr,isnull(SizeCode,'') as SizeCode FROM Size S INNER JOIN Unit U ON S.UnitId=U.UnitId INNER JOIN Shape Sh ON 
                            S.Shapeid=Sh.ShapeId Where SH.Shapeid=" + ddshape.SelectedValue + " Order By S.SizeFt";
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddSize.aspx");
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
        #region
        //if (VarLengthNew.Text != "")
        //{
        //    Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text));
        //    if (Session["varcompanyno"].ToString() != "6")
        //    {
        //        if (VarLengthNew.Text != "")
        //        {
        //            FootLength = Convert.ToInt32(Str.Split('.')[0]);
        //            FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
        //        }
        //        if (FootLengthInch > 11)
        //        {
        //            lblMessage.Text = "Inch value must be less than 12";
        //            VarLengthNew.Text = "";
        //            VarLengthNew.Focus();
        //        }
        //    }
        //}
        //if (VarWidthNew.Text != "")
        //{
        //    if (Session["varcompanyno"].ToString() != "6")
        //    {
        //        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
        //        if (VarWidthNew.Text != "")
        //        {
        //            FootWidth = Convert.ToInt32(Str.Split('.')[0]);
        //            FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
        //        }
        //        if (FootWidthInch > 11)
        //        {
        //            lblMessage.Text = "Inch value must be less than 12";
        //            VarWidthNew.Text = "";
        //            VarWidthNew.Focus();
        //        }
        //    }
        //}
        //if (Session["varcompanyno"].ToString() != "6")
        //{
        //    InchLength = (FootLength * 12) + FootLengthInch;
        //    InchWidth = (FootWidth * 12) + FootWidthInch;
        //}
        //else
        //{
        //    InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
        //    InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
        //}
        #endregion
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
            case "12":
                InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
                InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
                break;
            default:
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
                break;
        }

        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);
        //VarAreaNew.Text = Convert.ToString(VarArea);

        if (variable.VarAreaFtRound == "0")////Without Round
        {
            VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(VarArea, variable.VarRoundFtFlag).ToString();
        }
        else
        {
            VarAreaNew.Text = Convert.ToString(VarArea);
        }  

        if (VarHeightNew.Text != "")
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "6":
                case "12":
                    InchHeight = Convert.ToInt32(Convert.ToDouble(VarHeightNew.Text) * 12);
                    break;
                default:
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
                    break;
            }
            VarVolume = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth) * Convert.ToDouble(InchHeight)) / 1728, 4);
            VarVolumeNew.Text = Convert.ToString(VarVolume);
            txtheightFt.Focus();
        }
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
    private void AreaMtSq(double txtL, double txtW, TextBox VarAreaNew, string ShapeName = "")
    {
        ////Double Area = (txtL * txtW) / 10000;
        ////VarAreaNew.Text = Convert.ToString(Area);

        Double Area = 0;
        if (ShapeName.Trim() == "ROUND")
        {
            Area = ((txtL * txtW) / 10000) * Convert.ToDouble(variable.VarMfactor);
            //if (variable.VarAreaMtrRound == "0") // No Rounding            {
            //{
            //    VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(Area, variable.VarRoundMtrFlag).ToString();
            //}
            //else
            //{
            //    VarAreaNew.Text = Math.Round(Area, variable.VarRoundMtrFlag).ToString();

            //}
        }
        else
        {
            Area = (txtL * txtW) / 10000;

        }
        if (variable.VarAreaMtrRound == "0")
        {
            VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(Area, variable.VarRoundMtrFlag).ToString();
        }
        else
        {
            VarAreaNew.Text = Math.Round(Area, variable.VarRoundMtrFlag).ToString();
        }

        if (Session["varcompanyNo"].ToString() == "9")
        {
            VarAreaNew.Text = Math.Round(Area, 4).ToString();
        }
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
    private void CheckDuplicateSizeCode()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Size Where SizeCode='" + txtSizeCode.Text + "' and SizeId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Size Code AlReady Exits........";
            txtSizeCode.Text = "";
            txtSizeCode.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        CHECKVALIDCONTROL();
        if (Session["varMasterCompanyIDForERP"].ToString() == "21")
        {
            if (txtSizeCode.Text != "")
            {
                CheckDuplicateSizeCode();
            }
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
        }
        else
        {
            Store_Data();
        }
       
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
            string Str = "Select * from Size Where Shapeid=" + ddshape.SelectedValue + "";
            if (Convert.ToInt32(ddunit.SelectedValue) == 1)
            {
                Str = Str + " And  WidthMtr=" + Convert.ToDouble(txtwidthMtr.Text) + " And LengthMtr=" + Convert.ToDouble(txtlengthMtr.Text) + " And HeightMtr=" + Convert.ToDouble(txtheightMtr.Text) + "";
            }
            else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
            {
                Str = Str + " And WidthFt=" + Convert.ToDouble(txtwidthFt.Text) + " And LengthFt=" + Convert.ToDouble(txtlengthFt.Text) + " And HeightFt=" + Convert.ToDouble(txtheightFt.Text) + "";
            }
            else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
            {
                Str = Str + " And WidthInch=" + Convert.ToDouble(txtwidthInch.Text) + " And LengthInch=" + Convert.ToDouble(txtlengthInch.Text) + " And HeightInch=" + Convert.ToDouble(txtheightInch.Text) + "";
            }
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
                SqlParameter[] _arrPara = new SqlParameter[34];
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
                _arrPara[31] = new SqlParameter("@SizeCode", SqlDbType.VarChar, 10);
                _arrPara[32] = new SqlParameter("@ProdRoundOvelAreaFt", SqlDbType.Float);
                _arrPara[33] = new SqlParameter("@ProdRoundOvelAreaMtr", SqlDbType.Float);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyno From MasterSetting"));
                if (VarCompanyNo == 3)
                {
                    if (txtheightFt.Text == "0.00")
                    {
                        if ((txtwidthFt.Text == "0" || txtwidthFt.Text == "0.0" || txtwidthFt.Text == "0.00") && (txtlengthFt.Text == "0" || txtlengthFt.Text == "0.0" || txtlengthFt.Text == "0.00"))
                        {
                            _arrPara[1].Value = "";
                            _arrPara[2].Value = "";
                            _arrPara[22].Value = "";
                        }
                        else if (txtwidthFt.Text == "0" || txtwidthFt.Text == "0.0" || txtwidthFt.Text == "0.00")
                        {
                            _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
                            _arrPara[2].Value = txtlengthMtr.Text;
                            _arrPara[22].Value = txtlengthInch.Text;
                        }
                        else if (txtlengthFt.Text == "0" || txtlengthFt.Text == "0.0" || txtlengthFt.Text == "0.00")
                        {
                            _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text));
                            _arrPara[2].Value = txtwidthMtr.Text;
                            _arrPara[22].Value = txtwidthInch.Text;
                        }
                        else if ((txtwidthFt.Text != "0" || txtwidthFt.Text != "0.0" || txtwidthFt.Text != "0.00") && (txtlengthFt.Text != "0" || txtlengthFt.Text != "0.0" || txtlengthFt.Text != "0.00"))
                        {
                            _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
                            _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text;
                            _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text;
                        }
                    }
                    else
                    {
                        _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtheightFt.Text));
                        _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text + 'x' + txtheightMtr.Text;
                        _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text + 'x' + txtheightInch.Text;
                    }
                }
                else if (VarCompanyNo == 6 || VarCompanyNo == 12)
                {
                    if (txtheightFt.Text == "0.00" || txtheightFt.Text == "0")
                    {
                        _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
                        _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text;
                        _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text;
                    }
                    else
                    {
                        _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtheightFt.Text));
                        _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text + 'x' + txtheightMtr.Text;
                        _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text + 'x' + txtheightInch.Text;
                    }
                }
                else if (VarCompanyNo == 16 || VarCompanyNo == 28)
                {
                    _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
                    if (Convert.ToDouble(txtheightFt.Text) > 0)
                    {
                        _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtheightFt.Text));
                    }
                    _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text;
                    if (Convert.ToDouble(txtheightMtr.Text) > 0)
                    {
                        _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text + 'x' + txtheightMtr.Text;
                    }
                    _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text;
                    if (Convert.ToDouble(txtheightInch.Text) > 0)
                    {
                        _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text + 'x' + txtheightInch.Text;
                    }
                }
                else
                {
                    _arrPara[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(txtwidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(txtlengthFt.Text));
                    //_arrPara[1].Value = Math.Round(Convert.ToDouble(txtwidthFt.Text), 2).ToString() + 'x' + txtlengthFt.Text;
                    _arrPara[2].Value = txtwidthMtr.Text + 'x' + txtlengthMtr.Text;
                    _arrPara[22].Value = txtwidthInch.Text + 'x' + txtlengthInch.Text;
                }
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
                _arrPara[15].Value = Session["varuserid"].ToString();
                _arrPara[16].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[17].Value = Convert.ToDouble(txtwidthInch.Text);
                _arrPara[18].Value = Convert.ToDouble(txtlengthInch.Text);
                _arrPara[19].Value = txtheightInch.Text != "" ? Convert.ToDouble(txtheightInch.Text) : 0;
                _arrPara[20].Value = Convert.ToDouble(txtAreaInch.Text);
                _arrPara[21].Value = Convert.ToDouble(txtVolInch.Text);

                _arrPara[23].Value = Convert.ToDouble(TxtLengthProdFt.Text);
                _arrPara[24].Value = Convert.ToDouble(TxtWidthProdFt.Text);
                _arrPara[25].Value = Convert.ToDouble(TxtLengthCm.Text);
                _arrPara[26].Value = Convert.ToDouble(TxtWidthCm.Text);
                _arrPara[27].Value = TxtWidthProdFt.Text + 'x' + TxtLengthProdFt.Text;
                _arrPara[28].Value = TxtWidthCm.Text + 'x' + TxtLengthCm.Text;
                _arrPara[29].Value = Convert.ToDouble(TxtAreaProdSqYD.Text);
                _arrPara[30].Value = Convert.ToDouble(TxtAreaProdSqMtr.Text);
                _arrPara[31].Value = txtSizeCode.Text;
                _arrPara[32].Value = TxtRoundOvelSqYDArea.Text;
                _arrPara[33].Value = TxtRoundOvelAreaProdSqMtr.Text;

                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_SIZE", _arrPara);
                UnitDependControls();
                txtid.Text = "0";
                lbl.Visible = true;
                lbl.Text = "Save Details...........";
            }
        }
        catch (Exception ex)
        { UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddSize.aspx"); }
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
        string strsql = @"Select * from Size Where UnitId=" + ddunit.SelectedValue + " And Shapeid=" + ddshape.SelectedValue + " And Width='" + txtwidthFt.Text + "' And Length='" + txtlengthFt.Text + "' And HeightFt='" + VarHeight + "' and SizeID !='" + txtid.Text + "'";
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
    protected void gdSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnUpdateCode.Visible = true;
        lblMessage.Text = "";
        string str;
        string id = gdSize.SelectedDataKey.Value.ToString();
        Session["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        str = @"select * from Size WHERE SizeId=" + id + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                select SIZE_ID from ITEM_PARAMETER_MASTER Where size_id=" + id + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,str);
        try
        {
            //if size is in used
            if (ds.Tables[1].Rows.Count > 0)
            {
                //if (Session["varcompanyNo"].ToString() != "9")
                //{
                //    txtSizeCode.Text = ds.Tables[0].Rows[0]["SizeCode"].ToString();
                //    lblMessage.Text = "Size is already in used...";
                //    UnitDependControls();
                //    return;
                //}
            }
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
                txtSizeCode.Text = ds.Tables[0].Rows[0]["SizeCode"].ToString();
                TxtRoundOvelSqYDArea.Text = ds.Tables[0].Rows[0]["ProdRoundOvelAreaFt"].ToString();
                TxtRoundOvelAreaProdSqMtr.Text = ds.Tables[0].Rows[0]["ProdRoundOvelAreaMtr"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddSize.aspx");
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
        //txtSizeCode.Text = "";
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
        TxtRoundOvelSqYDArea.Text = "";
        TxtRoundOvelAreaProdSqMtr.Text = "";
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SIZE_ID from ITEM_PARAMETER_MASTER where SIZE_ID=" + Session["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Size where SizeId=" + Session["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Size'," + Session["id"].ToString() + ",getdate(),'Delete')");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddSize.aspx");
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
        Productioncm(TxtWidthProdFt, TxtWidthCm);
        TxtWidthCm_TextChanged(sender, e);
    }
    protected void TxtLengthProdFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //ConvertFtToMtr(TxtLengthProdFt, TxtLengthCm);
        //if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        //{
        //    AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        //}

        Productioncm(TxtLengthProdFt, TxtLengthCm);
        TxtLengthCm_TextChanged(sender, e);
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
            AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr, ddshape.SelectedItem.Text.ToUpper());
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
            AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr, ddshape.SelectedItem.Text.ToUpper());
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
        int vargirh;
        //if (VarLengthNew.Text != "")
        //{
        //    if (Session["varcompanyno"].ToString() != "6")
        //    {
        //        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text));
        //        if (VarLengthNew.Text != "")
        //        {
        //            FootLength = Convert.ToInt32(Str.Split('.')[0]);
        //            FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
        //        }
        //        if (FootLengthInch > 11)
        //        {
        //            lblMessage.Text = "Inch value must be less than 12";
        //            VarLengthNew.Text = "";
        //            VarLengthNew.Focus();
        //        }
        //    }
        //}
        //if (VarWidthNew.Text != "")
        //{
        //    if (Session["varcompanyno"].ToString() != "6")
        //    {
        //        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
        //        if (VarWidthNew.Text != "")
        //        {
        //            FootWidth = Convert.ToInt32(Str.Split('.')[0]);
        //            FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
        //        }
        //        if (FootWidthInch > 11)
        //        {
        //            lblMessage.Text = "Inch value must be less than 12";
        //            VarWidthNew.Text = "";
        //            VarWidthNew.Focus();
        //        }
        //    }
        //}
        //if (Session["varcompanyno"].ToString() != "6")
        //{
        //    InchLength = (FootLength * 12) + FootLengthInch;
        //    InchWidth = (FootWidth * 12) + FootWidthInch;
        //}
        //else
        //{
        //    InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
        //    InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
        //}
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
            case "12":
                InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
                InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
                break;
            default:
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
                break;
        }

        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / (144 * VarFactor), 4);
        int VarProductionArea = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ProductionArea From MasterSetting"));
        if (VarProductionArea == 0)
        {
            if (variable.VarAreaFtRound == "0")
            {
                VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(VarArea, variable.VarRoundFtFlag).ToString();
            }
            else
            {
                VarAreaNew.Text = Convert.ToString(VarArea);
            }            
        }
        else
        {

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select vargirh from Mastersetting");
            //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            vargirh = Convert.ToInt16(ds.Tables[0].Rows[0]["vargirh"]);
            Str = string.Format("{0:#0.0000#}", VarArea);
            double IntegerValue = Convert.ToDouble(Str.Split('.')[0]);
            Str = (Convert.ToDouble(Str.Split('.')[1]) / (0.0625 * 10000)).ToString();
            Str = string.Format("{0:#0.0000#}", Convert.ToDouble(Str));
            string Str1 = string.Format("{0:#0.0000#}", Convert.ToDouble(Str.Split('.')[1]));
            double TotalGirhValue = 0;
            TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]);
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                case "28":
                case "15":
                case "43":
                    break;
                default:
                    if (vargirh == 0)
                    {
                        if (Convert.ToDouble(Str1) > 5999)
                        {
                            TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]) + 1;
                        }
                    }
                    break;
            }

            double DecimalValue = 0;
            if (TotalGirhValue > 0)
            {
                DecimalValue = TotalGirhValue * 0.0625;
            }
            //VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);

            if (variable.VarAreaFtRound == "0")
            {
                VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding((IntegerValue + DecimalValue), variable.VarRoundFtFlag).ToString();
            }
            else
            {
                VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);
            }  

            ////Str = string.Format("{0:#0.0000#}", VarArea);
            ////double IntegerValue = Convert.ToDouble(Str.Split('.')[0]);
            ////Str = (Convert.ToDouble(Str.Split('.')[1]) / (0.0625 * 10000)).ToString();
            ////Str = string.Format("{0:#0.0000#}", Convert.ToDouble(Str));
            ////string Str1 = string.Format("{0:#0.0000#}", Convert.ToDouble(Str.Split('.')[1]));
            ////double TotalGirhValue = 0;
            ////TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]);
            ////if (Convert.ToDouble(Str1) > 5999)
            ////{
            ////    TotalGirhValue = Convert.ToDouble(Str.Split('.')[0]) + 1;
            ////}
            ////double DecimalValue = 0;
            ////if (TotalGirhValue > 0)
            ////{
            ////    DecimalValue = TotalGirhValue * 0.0625;
            ////}
            ////VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);
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
            switch (Session["varcompanyNo"].ToString())
            {
                case "6":
                case "12":
                    a = Math.Round(Convert.ToDouble(VarWidth / (30.48)), 2);
                    Z = Convert.ToString(a);
                    break;
                default:
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
                    break;
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
                switch (Session["varcompanyNo"].ToString())
                {
                    case "6":
                    case "12":
                        LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
                        LengthCm = Convert.ToInt32(Str.Split('.')[1]);
                        TotalLengthCm = Math.Round(Convert.ToDouble(Convert.ToDouble(VarWidthFtNew.Text) * 30.48), 0, 0);
                        Str = string.Format("{0:#0.00}", Convert.ToDouble(TotalLengthCm));
                        VarLength = Convert.ToInt32(Str.Split('.')[0]);
                        VarReplaceWidthMtrNew.Text = Convert.ToString(VarLength);
                        break;
                    default:
                        LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
                        LengthCm = Convert.ToInt32(Str.Split('.')[1]);
                        TotalLengthCm = (Convert.ToDouble(LengthMtr * 12) + Convert.ToDouble(LengthCm)) * 2.54;
                        Str = string.Format("{0:#0.00}", Convert.ToDouble(TotalLengthCm));
                        VarLength = Convert.ToInt32(Str.Split('.')[0]);
                        VarReplaceWidthMtrNew.Text = Convert.ToString(VarLength);
                        break;
                }
            }
        }
    }

    protected void Productioncm(TextBox VarWidthFtNew, TextBox VarReplaceWidthMtrNew)
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
    protected void BtnUpdateCode_Click(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "21")
        {
            if (txtSizeCode.Text != "")
            {
                CheckDuplicateSizeCode();
            }
        }
        if (lblMessage.Visible == false)
        {
            if (txtSizeCode.Text != "")
            {
                lblMessage.Text = "";
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrpara = new SqlParameter[4];
                    _arrpara[0] = new SqlParameter("@SizeId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@UserId", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                    _arrpara[3] = new SqlParameter("@SizeCode", SqlDbType.VarChar, 10);

                    _arrpara[0].Value = Session["id"].ToString();
                    _arrpara[1].Value = Session["varuserid"].ToString();
                    _arrpara[2].Value = Session["varMasterCompanyIDForERP"].ToString();
                    _arrpara[3].Value = txtSizeCode.Text;
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Update_SizeCode", _arrpara);
                    lblMessage.Visible = true;
                    lblMessage.Text = "Update SizeCode...........";
                    txtSizeCode.Text = "";
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmSize.aspx");
                    lblMessage.Visible = true;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                fill_grid();
                BtnUpdateCode.Visible = false;
            }
        }
    }
    //protected void gdSize_RowCreated(object sender, GridViewRowEventArgs e)
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
}