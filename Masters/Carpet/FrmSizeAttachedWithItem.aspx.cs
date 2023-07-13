using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_FrmSizeAttachedWithItem : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = "select IC.CATEGORY_ID,IC.CATEGORY_NAME from ITEM_CATEGORY_MASTER IC JOIN CategorySeparate CS ON IC.CATEGORY_ID=CS.Categoryid and CS.id=0  And IC.MasterCompanyId=" + Session["varCompanyId"] + @" Order By IC.CATEGORY_NAME";

            UtilityModule.ConditionalComboFill(ref DDCategory, str, true, "--Select--");
            if (DDCategory.Items.Count > 0)
            {
                DDCategory.SelectedIndex = 1;

                DDCategory_SelectedIndexChanged(sender, new EventArgs());
            }
            UtilityModule.ConditionalComboFill(ref ddshape, "select ShapeId,ShapeName from Shape where MasterCompanyId=" + Session["varCompanyId"] + " order by ShapeId", true, "--Select--");

            
            hnSizeId.Value = "0";
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
            Tr5.Visible = false;
            Tr6.Visible = false;
            Tr7.Visible = false;
        }
        else if (VarCompanyType == 1)
        {
            Tr1.Visible = true;
            Tr2.Visible = true;
            Tr3.Visible = true;
            Tr4.Visible = false;

            if (Session["VarCompanyNo"].ToString() == "43")
            {
                Tr5.Visible = true;
                Tr6.Visible = true;
                Tr7.Visible = true;
                TDQuality.Visible = true;
                TDDDQuality.Visible = true;
            }
            else
            {
                Tr5.Visible = false;
                Tr6.Visible = false;
                Tr7.Visible = false;
                TDQuality.Visible = false;
                TDDDQuality.Visible = false;
            }
        }
        
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select distinct IM.ITEM_ID,IM.ITEM_NAME from ITEM_MASTER IM JOIN ITEM_CATEGORY_MASTER IC ON IM.CATEGORY_ID=IC.CATEGORY_ID
                        Where IM.CATEGORY_ID=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varCompanyId"] + @"  Order by IM.ITEM_NAME";

        UtilityModule.ConditionalComboFill(ref DDItem, str, true, "--Select--");
    }

    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select distinct Q.QualityId,Q.QualityName from Quality Q JOIN ITEM_MASTER IM ON Q.Item_Id=IM.ITEM_ID 
                    Where Q.Item_Id=" + DDItem.SelectedValue + " and Q.MasterCompanyid=" + Session["varCompanyId"] + @"  Order by Q.QualityName";

        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--Select--");
    }

    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        // txtid.Text = "0";
        //fill_grid();

        string strsql = @"Select S.Sizeid,S.SizeFt FROM Size S INNER JOIN Unit U ON S.UnitId=U.UnitId INNER JOIN Shape Sh ON 
                            S.Shapeid=Sh.ShapeId Where SH.Shapeid=" + ddshape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " Order By S.SizeFt";

        UtilityModule.ConditionalComboFill(ref DDSize, strsql, true, "--Select--");

    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtid.Text = "0";

        string strsql = @"select S.SizeId,S.UnitId,S.Shapeid,S.WidthFt,S.LengthFt,S.HeightFt,S.WidthMtr,S.LengthMtr,S.HeightMtr,S.AreaFt,S.AreaMtr,S.SizeFt,S.SizeMtr 
                            from Size S Where S.SizeId=" + DDSize.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " Order By S.SizeFt";
        DataSet ds = SqlHelper.ExecuteDataset(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtwidthFt.Text = ds.Tables[0].Rows[0]["WidthFt"].ToString();
            txtlengthFt.Text = ds.Tables[0].Rows[0]["LengthFt"].ToString();
            txtheightFt.Text = ds.Tables[0].Rows[0]["HeightFt"].ToString();
            txtAreaFt.Text = ds.Tables[0].Rows[0]["AreaFt"].ToString();
            BtnCalCulate_Click(sender, new EventArgs());
        }

        fill_grid();
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        //lblshapeyname.Text = ParameterList[3];
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
            string strsql = "";
            strsql = @"Select SA.SizeAttachedWithItemId as Sr_No, SA.Sizeid ,SA.CategoryId,SA.ItemId,ICM.CATEGORY_NAME,IM.ITEM_NAME, Sh.ShapeName,
                            SA.ItemProdSizeft,SA.ItemProdSizemtr,S.SizeFt ,SA.ItemProdAreaFt,SA.ItemProdAreaMtr,Sa.QualityId,Q.QualityName,
                            isnull(SA.ItemFinishingSizeft,0) as ItemFinishingSizeft,isnull(SA.ItemFinishingSizemtr,0) as ItemFinishingSizemtr,
                            isnull(SA.ItemFinishingAreaFt,0) as ItemFinishingAreaFt,isnull(SA.ItemFinishingAreaMtr,0) as ItemFinishingAreaMtr
                            FROM SizeAttachedWithItem SA(NoLock) JOIN Size S(NoLock) ON SA.SizeId=S.SizeId 
                            INNER JOIN Unit U(NoLock) ON S.UnitId=U.UnitId 
                            INNER JOIN Shape Sh(NoLock) ON S.Shapeid=Sh.ShapeId 
                            JOIN ITEM_CATEGORY_MASTER ICM(NoLock) ON SA.CategoryId=ICM.CATEGORY_ID
                            JOIN ITEM_MASTER IM(NoLock) ON SA.ItemId=IM.ITEM_ID
                            LEFT JOIN Quality Q(NoLock) ON SA.QualityId=Q.QualityId
                            Where SA.CategoryId=" + DDCategory.SelectedValue + "  and SH.Shapeid=" + ddshape.SelectedValue + " And SA.MasterCompanyId=" + Session["varCompanyId"] + " ";
            if (DDItem.SelectedIndex > 0)
            {
                strsql = strsql + " and SA.ItemId=" + DDItem.SelectedValue + "";
            }
            if (DDSize.SelectedIndex > 0)
            {
                strsql = strsql + " and SA.SizeId=" + DDSize.SelectedValue + " ";
            }
            if (DDQuality.SelectedIndex > 0)
            {
                strsql = strsql + " and SA.QualityId=" + DDQuality.SelectedValue + " ";
            }
            strsql = strsql + "  Order By SA.ItemProdSizeft ";

            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmSizeAttachedWithItem.aspx");
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

        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text == "" ? "0" : VarLengthNew.Text));
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

        if (variable.VarAreaFtRound == "0")////Without Round
        {
            VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(VarArea, variable.VarRoundFtFlag).ToString();
        }
        else
        {
            VarAreaNew.Text = Convert.ToString(VarArea);
        }  
        
        //VarAreaNew.Text = Convert.ToString(VarArea);

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
        txtheightMtr.Focus();
    }
    private void AreaMtSq(double txtL, double txtW, TextBox VarAreaNew, string ShapeName = "")
    {
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


        ////CalculateArea();
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

        //if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        //{
        //    ConvertMtrToFt(txtwidthMtr, txtwidthFt);
        //    ConvertMtrToFt(txtlengthMtr, txtlengthFt);
        //    ConvertMtrToFt(txtheightMtr, txtheightFt);
        //    FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        //    txtlengthInch.Text = (Math.Round(Convert.ToDouble(txtlengthMtr.Text) / 2.54, 0)).ToString();
        //    txtwidthInch.Text = (Math.Round(Convert.ToDouble(txtwidthMtr.Text) / 2.54, 0)).ToString();
        //    txtheightInch.Text = txtheightMtr.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightMtr.Text) / 2.54, 0)).ToString();
        //    VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(txtheightInch.Text));
        //}
        //else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        //{
        //    //CalculateArea();
        //    ConvertFtToMtr(txtlengthFt, txtlengthMtr);
        //    ConvertFtToMtr(txtwidthFt, txtwidthMtr);
        //    ConvertFtToMtr(txtheightFt, txtheightMtr);
        //    if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
        //    {
        //        AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
        //    }
        //    if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        //    {
        //        VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        //    }
        //    txtlengthInch.Text = (Math.Round(Convert.ToDouble(txtlengthMtr.Text) / 2.54, 0)).ToString();
        //    txtwidthInch.Text = (Math.Round(Convert.ToDouble(txtwidthMtr.Text) / 2.54, 0)).ToString();
        //    txtheightInch.Text = txtheightMtr.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightMtr.Text) / 2.54, 0)).ToString();
        //    VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        //    VolumeINCHSq(Convert.ToDouble(txtlengthInch.Text), Convert.ToDouble(txtwidthInch.Text), Convert.ToDouble(txtheightInch.Text));
        //}
        //else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
        //{
        //    if (Session["varcompanyNo"].ToString() == "9")
        //    {
        //        txtlengthMtr.Text = (Math.Round(Convert.ToDouble(txtlengthInch.Text) * 2.54, 2)).ToString();
        //        txtwidthMtr.Text = (Math.Round(Convert.ToDouble(txtwidthInch.Text) * 2.54, 2)).ToString();
        //    }
        //    else
        //    {
        //        txtlengthMtr.Text = (Math.Round(Convert.ToDouble(txtlengthInch.Text) * 2.54, 0)).ToString();
        //        txtwidthMtr.Text = (Math.Round(Convert.ToDouble(txtwidthInch.Text) * 2.54, 0)).ToString();
        //    }

        //    txtheightMtr.Text = txtheightInch.Text == "" ? "0" : (Math.Round(Convert.ToDouble(txtheightInch.Text) * 2.54, 0)).ToString();
        //    if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "")
        //    {
        //        AreaMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), txtAreaMtr);
        //    }
        //    if (txtlengthMtr.Text != "" && txtwidthMtr.Text != "" && txtheightMtr.Text != "")
        //    {
        //        VolumeMtSq(Convert.ToDouble(txtlengthMtr.Text), Convert.ToDouble(txtwidthMtr.Text), Convert.ToDouble(txtheightMtr.Text));
        //    }
        //    ConvertMtrToFt(txtwidthMtr, txtwidthFt);
        //    ConvertMtrToFt(txtlengthMtr, txtlengthFt);
        //    ConvertMtrToFt(txtheightMtr, txtheightFt);
        //    FtAreaCalculate(txtlengthFt, txtwidthFt, txtheightFt, txtAreaFt, txtVolFt, 1);
        //}
        Prod_Export_length_Width(sender);
    }
    protected void CalculateClear()
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
        TxtWidthProdFt.Text = "";
        TxtLengthProdFt.Text = "";
        TxtWidthCm.Text = "";
        TxtLengthCm.Text = "";

        TxtWidthFinishingFt.Text = "";
        TxtLengthFinishingFt.Text = "";
        TxtWidthFinishingCm.Text = "";
        TxtLengthFinishingCm.Text = "";

        
    }    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        if (lblMessage.Text == "")
        {
            Store_Data();
        }
       

        btndelete.Visible = false;

        btnSave.Text = "Save";
        //Session["ReportPath"] = "Reports/size.rpt";
        //Session["CommanFormula"] = "";

    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(ddunit) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItem) == false)
        {
            goto a;
        }
        if (DDQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDTEXTBOX(TxtWidthFinishingFt) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDTEXTBOX(TxtLengthFinishingFt) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDTEXTBOX(TxtWidthFinishingCm) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDTEXTBOX(TxtLengthFinishingCm) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtWidthProdFt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLengthProdFt) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtWidthCm) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLengthCm) == false)
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
        if (hnSizeId.Value == "0")
        {
            CheckDuplicateData();
        }        
         if (lbl.Text == "")
         {
             try
             {
                 string Str = @"Select SA.SizeAttachedWithItemId,SA.SizeId,SA.CategoryId,SA.ItemId,S.AreaFt,SA.ItemProdLengthFt,SA.ItemProdWidthFt,SA.ItemProdLengthMtr,SA.ItemProdWidthMtr,
		                    SA.ItemProdSizeFt,SA.ItemProdSizeMtr,SA.ItemProdAreaFt,SA.ItemProdAreaMtr,SA.Actualfullareasqyd,SA.userid,Sa.MasterCompanyid,S.Shapeid 
                            from SizeAttachedWithItem SA JOIN SIZE S ON S.SizeId=SA.SizeId
                            Where SA.CategoryId=" + DDCategory.SelectedValue + " and SA.ItemId=" + DDItem.SelectedValue + " and S.Shapeid=" + ddshape.SelectedValue + @" 
                            and SA.sizeid=" + DDSize.SelectedValue + " and SA.QualityId=" + DDQuality.SelectedValue + "  And SA.MasterCompanyId=" + Session["varCompanyId"];

                 Str = Str + "  And SA.ItemProdWidthft=" + Convert.ToDouble(TxtWidthProdFt.Text) + " and SA.ItemProdLengthft= " + Convert.ToDouble(TxtLengthProdFt.Text) + "";

                 if (txtid.Text != "0")
                 {
                     Str = Str + " And SA.SizeAttachedWithItemId<>" + Convert.ToInt32(txtid.Text) + "";
                 }
              
                 DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                 switch (Session["varcompanyNo"].ToString())
                 {

                     default:
                         if (Ds.Tables[0].Rows.Count > 0)
                         {
                             lblMessage.Text = "Production Size already exists";
                             CalculateClear();
                             return;
                         }
                         break;
                 }

                 SqlParameter[] _arrPara = new SqlParameter[24];
                 _arrPara[0] = new SqlParameter("@SizeAttachedWithItemId", SqlDbType.Int);
                 _arrPara[1] = new SqlParameter("@SizeId", SqlDbType.Int);
                 _arrPara[2] = new SqlParameter("@CategoryId", SqlDbType.Int);
                 _arrPara[3] = new SqlParameter("@ItemId", SqlDbType.Int);
                 _arrPara[4] = new SqlParameter("@AreaFt", SqlDbType.Float);

                 _arrPara[5] = new SqlParameter("@ItemProdLengthFt", SqlDbType.Float);
                 _arrPara[6] = new SqlParameter("@ItemProdWidthFt", SqlDbType.Float);
                 _arrPara[7] = new SqlParameter("@ItemProdLengthMtr", SqlDbType.Float);
                 _arrPara[8] = new SqlParameter("@ItemProdWidthMtr", SqlDbType.Float);
                 _arrPara[9] = new SqlParameter("@ItemProdSizeFt", SqlDbType.NVarChar, 50);
                 _arrPara[10] = new SqlParameter("@ItemProdSizeMtr", SqlDbType.NVarChar, 50);
                 _arrPara[11] = new SqlParameter("@ItemProdAreaFt", SqlDbType.Float);
                 _arrPara[12] = new SqlParameter("@ItemProdAreaMtr", SqlDbType.Float);
                 _arrPara[13] = new SqlParameter("@UserId", SqlDbType.Int);
                 _arrPara[14] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

                 _arrPara[15] = new SqlParameter("@QualityID", SqlDbType.Int);
                 _arrPara[16] = new SqlParameter("@ItemFinishingLengthFt", SqlDbType.Float);
                 _arrPara[17] = new SqlParameter("@ItemFinishingWidthFt", SqlDbType.Float);
                 _arrPara[18] = new SqlParameter("@ItemFinishingLengthMtr", SqlDbType.Float);
                 _arrPara[19] = new SqlParameter("@ItemFinishingWidthMtr", SqlDbType.Float);
                 _arrPara[20] = new SqlParameter("@ItemFinishingSizeFt", SqlDbType.NVarChar, 50);
                 _arrPara[21] = new SqlParameter("@ItemFinishingSizeMtr", SqlDbType.NVarChar, 50);
                 _arrPara[22] = new SqlParameter("@ItemFinishingAreaFt", SqlDbType.Float);
                 _arrPara[23] = new SqlParameter("@ItemFinishingAreaMtr", SqlDbType.Float);


                 _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                 _arrPara[1].Value = DDSize.SelectedValue;
                 _arrPara[2].Value = DDCategory.SelectedValue;
                 _arrPara[3].Value = DDItem.SelectedValue;
                 _arrPara[4].Value = Convert.ToDouble(txtAreaFt.Text);

                 _arrPara[5].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtLengthProdFt.Text));
                 _arrPara[6].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidthProdFt.Text));
                 _arrPara[7].Value = Convert.ToDouble(TxtLengthCm.Text);
                 _arrPara[8].Value = Convert.ToDouble(TxtWidthCm.Text);
                 _arrPara[9].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidthProdFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(TxtLengthProdFt.Text)); ;
                 _arrPara[10].Value = TxtWidthCm.Text + 'x' + TxtLengthCm.Text;
                 _arrPara[11].Value = Convert.ToDouble(TxtAreaProdSqYD.Text);
                 _arrPara[12].Value = Convert.ToDouble(TxtAreaProdSqMtr.Text);
                 _arrPara[13].Value = Session["VarUserId"];
                 _arrPara[14].Value = Session["VarCompanyNo"];
                
                  _arrPara[15].Value = TDQuality.Visible == true ? DDQuality.SelectedIndex > 0 ? DDQuality.SelectedValue : "0" : "0";
                  _arrPara[16].Value = TDQuality.Visible == true ? string.Format("{0:#0.00}", Convert.ToDouble(TxtLengthFinishingFt.Text)) : "0";
                  _arrPara[17].Value = TDQuality.Visible == true ? string.Format("{0:#0.00}", Convert.ToDouble(TxtWidthFinishingFt.Text)) : "0";
                  _arrPara[18].Value = TDQuality.Visible == true ? Convert.ToDouble(TxtLengthFinishingCm.Text) : 0;
                  _arrPara[19].Value = TDQuality.Visible == true ? Convert.ToDouble(TxtWidthFinishingCm.Text) : 0;
                  _arrPara[20].Value = TDQuality.Visible == true ? string.Format("{0:#0.00}", Convert.ToDouble(TxtWidthFinishingFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(TxtLengthFinishingFt.Text)) : "0"; 
                  _arrPara[21].Value = TDQuality.Visible == true ? TxtWidthFinishingCm.Text + 'x' + TxtLengthFinishingCm.Text : "0";
                  _arrPara[22].Value = TDQuality.Visible == true ? Convert.ToDouble(TxtAreaFinishingSqYD.Text) : 0;
                  _arrPara[23].Value = TDQuality.Visible == true ? Convert.ToDouble(TxtAreaFinishingSqMtr.Text) : 0;
                
                 SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SIZE_ATTACHED_WITH_ITEM", _arrPara);
                 UnitDependControls();
                 txtid.Text = "0";
                 lbl.Visible = true;
                 lbl.Text = "Save Details...........";

             }
             catch (Exception ex)
             {
                 UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmSizeAttachedWithItem.aspx");
             }

             fill_grid();
         }

        
    }
    private void CheckDuplicateData()
    {
        lbl.Text = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //Double VarHeight = txtheightFt.Text != "" ? Convert.ToInt32(txtheightFt.Text) : 0;
        string strsql = @"Select SA.SizeAttachedWithItemId,SA.SizeId,SA.CategoryId,SA.ItemId,S.AreaFt,SA.ItemProdLengthFt,SA.ItemProdWidthFt,SA.ItemProdLengthMtr,SA.ItemProdWidthMtr,
		                    SA.ItemProdSizeFt,SA.ItemProdSizeMtr,SA.ItemProdAreaFt,SA.ItemProdAreaMtr,SA.Actualfullareasqyd,SA.userid,Sa.MasterCompanyid,S.Shapeid 
                            from SizeAttachedWithItem SA JOIN SIZE S ON S.SizeId=SA.SizeId
                            Where SA.CategoryId=" + DDCategory.SelectedValue + " and SA.ItemId=" + DDItem.SelectedValue + " and S.Shapeid=" + ddshape.SelectedValue + @" 
                            and SA.sizeid=" + DDSize.SelectedValue + " and SA.QualityId=" + DDQuality.SelectedValue + "  And SA.MasterCompanyId=" + Session["varCompanyId"];

        //string strsql = @"Select * from Size Where UnitId=" + ddunit.SelectedValue + " And Shapeid=" + ddshape.SelectedValue + " And Width='" + txtwidthFt.Text + "' And Length='" + txtlengthFt.Text + "' And HeightFt='" + VarHeight + "' and SizeID !='" + txtid.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl.Visible = true;
            lbl.Text = "One Production Size AlReady Exits With Same Item and Size........";
            txtwidthFt.Text = "";
            txtlengthFt.Text = "";
            txtwidthFt.Focus();
            return;
        }
        else
        {
            lbl.Visible = false;
        }
    }
    private void ClearAll()
    {
        txtid.Text = "0";
        DDCategory.SelectedIndex = 0;
        DDItem.SelectedIndex = 0;
        DDSize.SelectedIndex = 0;
        txtwidthFt.Text = "";
        ddshape.SelectedIndex = -1;
        txtlengthFt.Text = "";
        txtheightFt.Text = "";
        TxtWidthProdFt.Text = "";
        TxtLengthProdFt.Text = "";
        TxtWidthCm.Text = "";
        TxtLengthCm.Text = "";
        btnSave.Text = "Save";

        TxtWidthFinishingFt.Text = "";
        TxtLengthFinishingFt.Text = "";
        TxtWidthFinishingCm.Text = "";
        TxtLengthFinishingCm.Text = "";
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
    }
    protected void gdSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BtnUpdateCode.Visible = true;
        lblMessage.Text = "";
        string str;
        string id = gdSize.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;

        string lblSizeId = (gdSize.SelectedRow.FindControl("lblSizeId") as Label).Text;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        str = @"select SA.SizeAttachedWithItemId,SA.SizeId,SA.CategoryId,SA.ItemId,S.AreaFt,SA.ItemProdLengthFt,SA.ItemProdWidthFt,SA.ItemProdLengthMtr,SA.ItemProdWidthMtr,
		            SA.ItemProdSizeFt,SA.ItemProdSizeMtr,SA.ItemProdAreaFt,SA.ItemProdAreaMtr,SA.Actualfullareasqyd,SA.userid,Sa.MasterCompanyid,S.Shapeid,
		            S.WidthFt,S.LengthFt,S.HeightFt,S.VolumeFt,isnull(SA.QualityId,0) as QualityId,isnull(SA.ItemFinishingLengthFt,0) as ItemFinishingLengthFt,
                    isnull(SA.ItemFinishingWidthFt,0) as ItemFinishingWidthFt,isnull(SA.ItemFinishingLengthMtr,0) as ItemFinishingLengthMtr,
                    isnull(SA.ItemFinishingWidthMtr,0) as ItemFinishingWidthMtr,isnull(SA.ItemFinishingSizeFt,0) as ItemFinishingSizeFt,
                    isnull(SA.ItemFinishingSizeMtr,0) as ItemFinishingSizeMtr,isnull(SA.ItemFinishingAreaFt,0) as ItemFinishingAreaFt,isnull(SA.ItemFinishingAreaMtr,0) as ItemFinishingAreaMtr
		            from SizeAttachedWithItem SA JOIN Size S ON SA.SizeId=S.SizeId 
                    WHERE SA.SizeAttachedWithItemId=" + id + " And SA.MasterCompanyId=" + Session["varCompanyId"] + @"
                select SIZE_ID from ITEM_PARAMETER_MASTER Where size_id=" + lblSizeId + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        try
        {
            //if size is in used
            if (ds.Tables[1].Rows.Count > 0)
            {
                lblMessage.Text = "Size is already in used...";
                UnitDependControls();
                return;

            }
            if (ds.Tables[0].Rows.Count == 1)
            {
                hnSizeId.Value = ds.Tables[0].Rows[0]["SizeId"].ToString();

                txtid.Text = ds.Tables[0].Rows[0]["SizeAttachedWithItemId"].ToString();
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CategoryId"].ToString();
                DDItem.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["Shapeid"].ToString();
                txtwidthFt.Text = ds.Tables[0].Rows[0]["WidthFt"].ToString();
                txtlengthFt.Text = ds.Tables[0].Rows[0]["LengthFt"].ToString();
                txtheightFt.Text = ds.Tables[0].Rows[0]["HeightFt"].ToString();
                txtAreaFt.Text = ds.Tables[0].Rows[0]["AreaFt"].ToString();
                txtVolFt.Text = ds.Tables[0].Rows[0]["VolumeFt"].ToString();               
                TxtLengthProdFt.Text = ds.Tables[0].Rows[0]["ItemProdLengthFt"].ToString();
                TxtWidthProdFt.Text = ds.Tables[0].Rows[0]["ItemProdWidthFt"].ToString();
                TxtLengthCm.Text = ds.Tables[0].Rows[0]["ItemProdLengthMtr"].ToString();
                TxtWidthCm.Text = ds.Tables[0].Rows[0]["ItemProdWidthMtr"].ToString();
                TxtAreaProdSqYD.Text = ds.Tables[0].Rows[0]["ItemProdAreaFt"].ToString();
                TxtAreaProdSqMtr.Text = ds.Tables[0].Rows[0]["ItemProdAreaMtr"].ToString();

                if (TDQuality.Visible == true)
                {
                    DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                    TxtLengthFinishingFt.Text = ds.Tables[0].Rows[0]["ItemFinishingLengthFt"].ToString();
                    TxtWidthFinishingFt.Text = ds.Tables[0].Rows[0]["ItemFinishingWidthFt"].ToString();
                    TxtLengthFinishingCm.Text = ds.Tables[0].Rows[0]["ItemFinishingLengthMtr"].ToString();
                    TxtWidthFinishingCm.Text = ds.Tables[0].Rows[0]["ItemFinishingWidthMtr"].ToString();
                    TxtAreaFinishingSqYD.Text = ds.Tables[0].Rows[0]["ItemFinishingAreaFt"].ToString();
                    TxtAreaFinishingSqMtr.Text = ds.Tables[0].Rows[0]["ItemFinishingAreaMtr"].ToString();
                }
                else
                {
                    DDQuality.SelectedValue = "0";
                    TxtLengthFinishingFt.Text = "0";
                    TxtWidthFinishingFt.Text = "0";
                    TxtLengthFinishingCm.Text = "0";
                    TxtWidthFinishingCm.Text = "0";
                    TxtAreaFinishingSqYD.Text = "0";
                    TxtAreaFinishingSqMtr.Text = "0";
                }

                DDCategory.Enabled = false;
                DDItem.Enabled = false;
                ddshape.Enabled = false;
                DDSize.Enabled = false;
                DDQuality.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmSizeAttachedWithItem.aspx");
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

            for (int i = 0; i < gdSize.Columns.Count; i++)
            {
                //if (DGOrderDetail.Columns[i].HeaderText == "Finished_ID")
                //{
                //    DGOrderDetail.Columns[i].Visible = false;
                //}
                //if (DGOrderDetail.Columns[i].HeaderText == "FlagSize")
                //{
                //    DGOrderDetail.Columns[i].Visible = false;
                //}

                if (Session["varcompanyId"].ToString() == "43")
                {
                    if (gdSize.Columns[i].HeaderText == "Finishing SizeFt" || gdSize.Columns[i].HeaderText == "Finishing SizeMtr" || gdSize.Columns[i].HeaderText == "Finishing AreaFt" || gdSize.Columns[i].HeaderText == "Finishing AreaMtr")
                    {
                        gdSize.Columns[i].Visible = true;
                    }

                }
                else
                {
                    if (gdSize.Columns[i].HeaderText == "Finishing SizeFt" || gdSize.Columns[i].HeaderText == "Finishing SizeMtr" || gdSize.Columns[i].HeaderText == "Finishing AreaFt" || gdSize.Columns[i].HeaderText == "Finishing AreaMtr")
                    {
                        gdSize.Columns[i].Visible = false;
                    }

                }
            }
        }
    }
    //protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtid.Text = "0";
    //    UnitDependControls();
    //}
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

        TxtWidthFinishingFt.Text = "";
        TxtLengthFinishingFt.Text = "";
        TxtWidthFinishingCm.Text = "";
        TxtLengthFinishingCm.Text = "";
        TxtAreaFinishingSqYD.Text = "";
        TxtAreaFinishingSqMtr.Text = "";

        //if (Convert.ToInt32(ddunit.SelectedValue) == 1)
        //{
        //    txtwidthMtr.Enabled = true;
        //    txtlengthMtr.Enabled = true;
        //    txtheightMtr.Enabled = true;
        //    txtwidthFt.Enabled = false;
        //    txtlengthFt.Enabled = false;
        //    txtheightFt.Enabled = false;
        //    txtwidthInch.Enabled = false;
        //    txtlengthInch.Enabled = false;
        //    txtheightInch.Enabled = false;
        //    txtwidthMtr.Focus();
        //}
        //else if (Convert.ToInt32(ddunit.SelectedValue) == 2)
        //{
        //    txtwidthMtr.Enabled = false;
        //    txtlengthMtr.Enabled = false;
        //    txtheightMtr.Enabled = false;
        //    txtwidthFt.Enabled = true;
        //    txtlengthFt.Enabled = true;
        //    txtheightFt.Enabled = true;
        //    txtwidthInch.Enabled = false;
        //    txtlengthInch.Enabled = false;
        //    txtheightInch.Enabled = false;
        //    txtwidthFt.Focus();
        //    switch (Session["varcompanyid"].ToString())
        //    {
        //        case "16":
        //        case "28":
        //            txtwidthMtr.Enabled = true;
        //            txtlengthMtr.Enabled = true;
        //            txtheightMtr.Enabled = true;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //else if (Convert.ToInt32(ddunit.SelectedValue) == 6)
        //{
        //    txtwidthMtr.Enabled = false;
        //    txtlengthMtr.Enabled = false;
        //    txtheightMtr.Enabled = false;
        //    txtwidthFt.Enabled = false;
        //    txtlengthFt.Enabled = false;
        //    txtheightFt.Enabled = false;
        //    txtwidthInch.Enabled = true;
        //    txtlengthInch.Enabled = true;
        //    txtheightInch.Enabled = true;
        //    txtwidthInch.Focus();
        //}
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] parparam = new SqlParameter[4];
            parparam[0] = new SqlParameter("@id", ViewState["id"].ToString());
            parparam[1] = new SqlParameter("@varCompanyId", Session["varCompanyId"].ToString());
            parparam[2] = new SqlParameter("@varuserid", Session["varuserid"].ToString());
            parparam[3] = new SqlParameter("@SizeId", hnSizeId.Value);

            int id = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteSizeAttachedWithItem", parparam);
            if (id > 0)
            {
                lbl.Visible = true;
                lbl.Text = "Record(s) has been deleted!";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Value in Use...";
            }
            #region Author: Rajeev, Date: 26-Nov-12, Converted into proc..
            //int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SIZE_ID from ITEM_PARAMETER_MASTER where SIZE_ID=" + ViewState["id"].ToString()));
            //if (id <= 0)
            //{
            //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Size where SizeId=" + ViewState["id"].ToString());
            //    DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            //    SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Size'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
            //    lbl.Visible = true;
            //    lbl.Text = "Value deleted...";
            //}
            //else
            //{
            //    lbl.Visible = true;
            //    lbl.Text = "Value in Use...";

            //}
            #endregion
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmSizeAttachedWithItem.aspx");
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


    protected void TxtWidthFinishingFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(TxtLengthFinishingFt,TxtWidthFinishingFt,TxtAreaFinishingSqYD, 9);
        //ConvertFtToMtr(TxtWidthProdFt, TxtWidthCm);
        //if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        //{
        //    AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        //}
        Productioncm(TxtWidthFinishingFt, TxtWidthFinishingCm);
        TxtWidthFinishingCm_TextChanged(sender, e);
    }
    protected void TxtLengthFinishingFt_TextChanged(object sender, EventArgs e)
    {
        FtAreaCalculate(TxtLengthFinishingFt, TxtWidthFinishingFt, TxtAreaFinishingSqYD, 9);
        //ConvertFtToMtr(TxtLengthProdFt, TxtLengthCm);
        //if (TxtLengthCm.Text != "" && TxtWidthCm.Text != "")
        //{
        //    AreaMtSq(Convert.ToDouble(TxtLengthCm.Text), Convert.ToDouble(TxtWidthCm.Text), TxtAreaProdSqMtr);
        //}
        Productioncm(TxtLengthFinishingFt,TxtLengthFinishingCm);
        TxtLengthFinishingCm_TextChanged(sender, e);
    }
    protected void TxtWidthFinishingCm_TextChanged(object sender, EventArgs e)
    {
        //if (TxtWidthCm.Text != "")
        //{
        //    ConvertMtrToFt(TxtWidthCm, TxtWidthProdFt);
        //    FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //}
        if (TxtLengthFinishingCm.Text != "" && TxtWidthFinishingCm.Text != "")
        {
            AreaMtSq(Convert.ToDouble(TxtLengthFinishingCm.Text), Convert.ToDouble(TxtWidthFinishingCm.Text), TxtAreaFinishingSqMtr, ddshape.SelectedItem.Text.ToUpper());
        }
    }
    protected void TxtLengthFinishingCm_TextChanged(object sender, EventArgs e)
    {
        //if (TxtLengthCm.Text != "")
        //{
        //    ConvertMtrToFt(TxtLengthCm, TxtLengthProdFt);
        //    FtAreaCalculate(TxtLengthProdFt, TxtWidthProdFt, TxtAreaProdSqYD, 9);
        //}
        if (TxtLengthFinishingCm.Text != "" && TxtWidthFinishingCm.Text != "")
        {
            AreaMtSq(Convert.ToDouble(TxtLengthFinishingCm.Text), Convert.ToDouble(TxtWidthFinishingCm.Text), TxtAreaFinishingSqMtr, ddshape.SelectedItem.Text.ToUpper());
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
            if (variable.VarAreaFtRound == "0")////Without Round
            {
                VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding(VarArea, variable.VarRoundFtFlag).ToString();
            }
            else
            {
                VarAreaNew.Text =Convert.ToString(VarArea);
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
            ////VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);

            if (variable.VarAreaFtRound == "0")////Without Round
            {
                VarAreaNew.Text = UtilityModule.DecimalvalueUptoWithoutRounding((IntegerValue + DecimalValue), variable.VarRoundFtFlag).ToString();
            }
            else
            {
                VarAreaNew.Text = Convert.ToString(IntegerValue + DecimalValue);
            }  
        }
    }
    private void Prod_Export_length_Width(object sender = null)
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
            if (sender != null)
            {
                TxtLengthCm_TextChanged(sender, new EventArgs());
            }

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
                        if (variable.VarFt_To_Mtr_SizeRound == "1")
                        {
                            TotalLengthCm = Math.Round(TotalLengthCm, MidpointRounding.AwayFromZero);
                        }
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
    protected void ProductionLengthcm()
    {
        string Str;
        int LengthMtr, LengthCm;
        double VarLength, TotalLengthCm;

        Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidthProdFt.Text));
        if (TxtWidthProdFt.Text != "")
        {
            LengthMtr = Convert.ToInt32(Str.Split('.')[0]);
            LengthCm = Convert.ToInt32(Str.Split('.')[1]);
            TotalLengthCm = (Convert.ToDouble(LengthMtr * 12) + Convert.ToDouble(LengthCm)) * 2.54;
            Str = string.Format("{0:#0.00}", Convert.ToDouble(TotalLengthCm));
            VarLength = Convert.ToInt32(Str.Split('.')[0]);
            TxtWidthCm.Text = Convert.ToString(VarLength);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[1] = new SqlParameter("@ItemId", DDItem.SelectedIndex>0 ? DDItem.SelectedValue : "0");
        param[2] = new SqlParameter("@QualityId", DDQuality.SelectedIndex > 0 ? DDQuality.SelectedValue : "0");
       
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SIZEATTACHED_WITHITEM_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptSizeAttachedWithItemReport.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptSizeAttachedWithItemReport.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

//        string qry = @" SELECT SizeAttachedWithItemId as Sr_No,SizeId, ShapeName,Unitname,SizeFt as ExportSizeFt,CategoryId,CATEGORY_NAME,ItemId,ITEM_NAME,
//                        ItemProdLengthFt,ItemProdWidthFt,ItemProdLengthMtr,ItemProdWidthMtr,ItemProdSizeFt,ItemProdSizeMtr,ItemProdAreaFt,ItemProdAreaMtr,
//                        Actualfullareasqyd,userid,MasterCompanyid
//                FROM V_SizeAttachedWithItem Where MasterCompanyId=" + Session["varCompanyId"];
//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            Session["rptFileName"] = "~\\Reports\\RptSizeAttachedWithItemReport.rpt";
//            //Session["rptFileName"] = Session["ReportPath"];
//            Session["GetDataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\RptSizeAttachedWithItemReport.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
//        }
    }
      

}