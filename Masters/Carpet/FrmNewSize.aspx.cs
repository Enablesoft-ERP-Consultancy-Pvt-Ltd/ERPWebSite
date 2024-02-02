using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Drawing;
using System.Text;

public partial class Masters_Carpet_FrmNewSize : System.Web.UI.Page
{
    public long varWinch;
    public long varLinch;
    public string MtrSize;
    public double Mtrarea;
    public double CalSquareYard2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //CalendarExtender1.StartDate = DateTime.Today;
            tbEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            BindUnit();
            BindQualityType();
            BindQualityName();
            BindShape();
            BindgdMasterSize();

            if (Session["varMasterCompanyIDForERP"].ToString() == "20" && variable.VarNewQualitySize == "1")
            {
                divattached.Visible = false;
                EffectiveDate.Visible = true;
                tbEffectiveDate.Attributes.Add("readonly", "readonly");
            }
            else
            {
                divattached.Visible = true;
                EffectiveDate.Visible = false;
            }

            //textbox1.Attributes.Add("onKeyDown", "ModifyEnterKeyPressAsTab();");           
        }
    }

    protected void BindUnit()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetUnit", param);

            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "UnitName";
            ddlUnit.DataSource = dt;
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, "--Select--");
            ddlUnit.Items[0].Value = "0";

            ddlUnit.SelectedValue = "2";

            tran.Commit();


        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }
    protected void BindQualityType()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetQualityType", param);

            ddlQualityType.DataValueField = "ITEM_ID";
            ddlQualityType.DataTextField = "ITEM_NAME";
            ddlQualityType.DataSource = dt;
            ddlQualityType.DataBind();
            ddlQualityType.Items.Insert(0, "--Select--");
            ddlQualityType.Items[0].Value = "0";

            ddlQualityTypeAttac.DataValueField = "ITEM_ID";
            ddlQualityTypeAttac.DataTextField = "ITEM_NAME";
            ddlQualityTypeAttac.DataSource = dt;
            ddlQualityTypeAttac.DataBind();
            ddlQualityTypeAttac.Items.Insert(0, "--Select--");
            ddlQualityTypeAttac.Items[0].Value = "0";

            tran.Commit();

            ViewState["CarpetType"] = ddlQualityType.SelectedValue;
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }
    protected void BindQualityName()
    {
        if (Convert.ToInt32(ddlQualityType.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", ddlQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQuality.DataValueField = "QualityID";
                ddlQuality.DataTextField = "QualityName";
                ddlQuality.DataSource = dt;
                ddlQuality.DataBind();
                ddlQuality.Items.Insert(0, "--Select--");
                ddlQuality.Items[0].Value = "0";

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
        else
        {
        }
    }
    protected void BindShape()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            Hashtable param = new Hashtable();
            param.Add("@mode", "Get");
            DataTable dt = DataAccess.fetch("Pro_GetShape", param);

            ddlShape.DataValueField = "ShapeId";
            ddlShape.DataTextField = "ShapeName";
            ddlShape.DataSource = dt;
            ddlShape.DataBind();
            ddlShape.Items.Insert(0, "--Select--");
            ddlShape.Items[0].Value = "0";

            tran.Commit();

            //ViewState["CarpetType"] = ddlShape.SelectedValue;
        }
        catch (Exception ex)
        {
            tran.Rollback();

            //lblmsg.Text = ex.Message;
            con.Close();
            throw ex;
        }
    }

    protected string FormatDecimalNumber(double varNumber, int varAfterDPoint)
    {
        string strTemp;
        string strTemp2;
        string strNumber = Convert.ToString(varNumber);

        string s = Convert.ToString(varNumber);
        string[] parts = s.Split('.');

        strTemp = parts[0];

        int k = parts.Length;
        if (k > 1)
        {
            strTemp2 = parts[1];
            if (Convert.ToInt32(strTemp2 == "" ? "0" : strTemp2) > 0)
            {
                strTemp = strTemp + "." + varAfterDPoint;
            }
            else
            {
                strTemp = strNumber;
            }
        }
        else
        {
            strTemp = strNumber;
        }

        return strTemp;
    }
    private void AreaFootSq(TextBox VarLengthNew, TextBox VarWidthNew)
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

        //Session["varcompanyNo"] = "9";

        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text == "" ? "0" : VarLengthNew.Text));

        //switch (Convert.ToString(Session["varcompanyNo"] == "9"))
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
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144, 4);
        tbAAreaSqFt.Text = Convert.ToString(VarArea);

    }
    private void AreaMtSq(double txtL, double txtW, TextBox VarAreaNew)
    {
        Double Area = (txtL * txtW) / 10000;
        if (Session["varcompanyNo"].ToString() == "9")
        {
            VarAreaNew.Text = Math.Round(Area, 4).ToString();
        }
        else
        {
            VarAreaNew.Text = Convert.ToString(Area);
        }
    }
    protected double CalSquareYard(int FASY)
    {
        double varSYarea;
        long varSYareaint;
        //int varSYareaint = 1;
        double varSYFloat;
        double varGrih;
        double CalSquareYard;

        //if (ViewState["varWinch"] != null || ViewState["varWinch"] != "0")
        if (Convert.ToInt32(ViewState["varWinch"]) > 0)
        {
            varWinch = (long)ViewState["varWinch"];
        }
        else
        {
            ViewState["varWinch"] = "0";
            //varWinch = (long)ViewState["varWinch"];
            varWinch = 0;
        }

        if (ViewState["varLinch"] == "0" || ViewState["varLinch"] == null)
        {
            ViewState["varLinch"] = 0;
            varLinch = 0;

        }
        else if (Convert.ToInt32(ViewState["varLinch"]) > 0)
        {
            varLinch = (long)ViewState["varLinch"];
        }


        varSYarea = Math.Round(Convert.ToDouble(varLinch * varWinch) / 1296, 4);

        if (FASY == 0)
        {
            tbPFullAreaSqYd.Text = Convert.ToString(varSYarea);
        }

        varSYareaint = (int)(varSYarea);
        string varSYFloat2 = string.Format("{0:#.000}", (varSYarea - varSYareaint));
        double varSYFloat3 = Convert.ToDouble(varSYFloat2);
        //string varSYFloat2 = string.Format("{0:#0.0000}", (varSYarea - varSYareaint));
        ////string varSYFloat2 = string.Format("{0:#,#.####}", (varSYarea - varSYareaint));
        varSYFloat = Convert.ToDouble(varSYFloat3);
        varGrih = (int)((varSYFloat / 0.0625));
        varGrih = (int)(varGrih) * 0.0625;
        CalSquareYard = Math.Round(varSYareaint + varGrih, 4);
        return CalSquareYard;

    }
    private void ConvertProduction_FtToMtr()
    {
        double PWidthCm;
        double PLengthCm;

        if (tbPWidthFt.Text != "" && tbPWidthIn.Text != "" && tbPLengthFt.Text != "" && tbPLengthIn.Text != "")
        {
            PWidthCm = ((Convert.ToDouble(tbPWidthFt.Text) * 12) + Convert.ToDouble(tbPWidthIn.Text)) * 2.54;

            tbPWidthCm.Text = FormatDecimalNumber(PWidthCm, 0);
            tbPWidthCm.Text = string.Format("{0:#0}", Convert.ToDouble(tbPWidthCm.Text));
            tbPWidthCm.Text = (tbPWidthCm.Text);

            txtPWidthCm();


            PLengthCm = ((Convert.ToDouble(tbPLengthFt.Text) * 12) + Convert.ToDouble(tbPLengthIn.Text)) * 2.54;

            tbPLengthCm.Text = FormatDecimalNumber(PLengthCm, 0);
            tbPLengthCm.Text = string.Format("{0:#0}", Convert.ToDouble(tbPLengthCm.Text));
            txtPLengthCm();
        }
    }

    private void CalculateFinishingSize(int WL)
    {
        int W;
        int L;

        //if (tbPWidthFt.Text != "" && tbPWidthIn.Text != "" && tbPLengthFt.Text != "" && tbPLengthIn.Text != "")
        if (tbPWidthFt.Text != "" && tbPWidthIn.Text != "" && tbPLengthFt.Text != "" && tbPLengthIn.Text != "")
        {
            if (WL == 1)
            {
                //for width
                lblMessage.Text = "";
                //if (tbPWidthFt.Text != "" && tbPWidthIn.Text != "")
                //{
                if (Convert.ToDouble(tbPKhapW.Text == "" ? "0" : tbPKhapW.Text) > Convert.ToDouble(tbPWidthIn.Text == "" ? "0" : tbPWidthIn.Text))
                {
                    double FWidthFt;
                    double FWidthIn;

                    FWidthFt = Math.Round(Convert.ToDouble(tbPWidthFt.Text) - 1, 0);
                    tbFWidthFt.Text = Convert.ToString(FWidthFt);


                    FWidthIn = Math.Round(Convert.ToDouble(Convert.ToDouble(tbPWidthIn.Text) + 12) - (Convert.ToDouble(tbPKhapW.Text)), 0);
                    //FWidthIn = Math.Round((Convert.ToDouble((tbPWidthIn.Text) + 12) - Convert.ToDouble(tbPKhapW.Text)), 0);
                    tbFWidthIn.Text = Convert.ToString(FWidthIn);
                    //txtFWidthIn();

                    if (hnSizeId.Value != null && btnSave.Text == "Update")
                    {
                        txtFWidthFt();
                        txtFWidthIn();
                        txtFLengthFt();
                        txtFLengthIn();
                    }
                    else
                    {
                        txtFWidthFt();
                        txtFWidthIn();
                    }

                }
                else
                {
                    double FWidthFt;
                    double FWidthIn;
                    FWidthFt = Math.Round(Convert.ToDouble(tbPWidthFt.Text), 0);
                    tbFWidthFt.Text = Convert.ToString(FWidthFt);
                    // txtFWidthFt();


                    //FWidthIn = Convert.ToDouble(Convert.ToInt32(tbPWidthIn.Text)) - (Convert.ToDouble(tbPKhapW.Text == "" ? "0" : tbPKhapW.Text));
                    // FWidthIn = Math.Round(Convert.ToDouble(tbPWidthIn.Text) - Convert.ToDouble(tbPKhapW.Text), 0);
                    FWidthIn = Math.Round(Convert.ToDouble(tbPWidthIn.Text) - Convert.ToDouble(tbPKhapW.Text == "" ? "0" : tbPKhapW.Text), 0);
                    tbFWidthIn.Text = Convert.ToString(FWidthIn);
                    // txtFWidthIn();

                    if (hnSizeId.Value != null && btnSave.Text == "Update")
                    {
                        txtFWidthFt();
                        txtFWidthIn();
                        txtFLengthFt();
                        txtFLengthIn();
                    }
                    else
                    {
                        txtFWidthFt();
                        txtFWidthIn();
                    }
                }
                //}
                //else
                //{
                //    lblMessage.Text = "Please fill the Production width(Ft) and width(In) values";
                //}

            }
            else if (WL == 2)
            {
                //for length
                lblMessage.Text = "";
                if (tbPLengthFt.Text != "" && tbPLengthIn.Text != "")
                {
                    if (Convert.ToDouble(tbPKhapL.Text == "" ? "0" : tbPKhapL.Text) > Convert.ToDouble(tbPLengthIn.Text))
                    {
                        double FLengthFt;
                        double FLengthIn;
                        FLengthFt = Math.Round(Convert.ToDouble(tbPLengthFt.Text) - 1, 0);
                        tbFLengthFt.Text = Convert.ToString(FLengthFt);
                        //txtFLengthFt();

                        FLengthIn = Math.Round(Convert.ToDouble(Convert.ToDouble(tbPLengthIn.Text) + 12) - (Convert.ToDouble(tbPKhapL.Text)), 0);
                        //FLengthIn = Math.Round(Convert.ToDouble(tbPLengthIn.Text) + 12 - Convert.ToDouble(tbPKhapL.Text), 0);
                        tbFLengthIn.Text = Convert.ToString(FLengthIn);
                        //txtFLengthIn();

                        if (hnSizeId.Value != null && btnSave.Text == "Update")
                        {
                            txtFWidthFt();
                            txtFWidthIn();
                            txtFLengthFt();
                            txtFLengthIn();
                        }
                        else
                        {
                            txtFLengthFt();
                            txtFLengthIn();
                        }
                    }
                    else
                    {
                        double FLengthFt;
                        double FLengthIn;
                        FLengthFt = Math.Round(Convert.ToDouble(tbPLengthFt.Text), 0);
                        tbFLengthFt.Text = Convert.ToString(FLengthFt);
                        //txtFLengthFt();

                        FLengthIn = Math.Round(Convert.ToDouble(tbPLengthIn.Text) - Convert.ToDouble(tbPKhapL.Text == "" ? "0" : tbPKhapL.Text), 0);
                        //FLengthIn = Math.Round(Convert.ToDouble(tbPLengthIn.Text) - Convert.ToDouble(tbPKhapL.Text), 0);
                        tbFLengthIn.Text = Convert.ToString(FLengthIn);
                        //txtFLengthIn();

                        if (hnSizeId.Value != null && btnSave.Text == "Update")
                        {
                            txtFWidthFt();
                            txtFWidthIn();
                            txtFLengthFt();
                            txtFLengthIn();
                        }
                        else
                        {
                            txtFLengthFt();
                            txtFLengthIn();
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Please fill the Production length(Ft) and length(In) values";
                }
            }
        }

    }
    private void ConvertFinishing_FtToMtr()
    {
        double FWidthCm;
        double FLengthCm;

        if (tbFWidthFt.Text != "" && tbFWidthIn.Text != "" && tbFLengthFt.Text != "" && tbFLengthCm.Text != "")
        {
            FWidthCm = ((Convert.ToDouble(tbFWidthFt.Text) * 12) + Convert.ToDouble(tbFWidthIn.Text)) * 2.54;

            tbFWidthCm.Text = FormatDecimalNumber(FWidthCm, 0);
            tbFWidthCm.Text = string.Format("{0:#0}", Convert.ToDouble(tbFWidthCm.Text));
            txtFWidthCm();

            FLengthCm = ((Convert.ToDouble(tbFLengthFt.Text) * 12) + Convert.ToDouble(tbFLengthIn.Text == "" ? "0" : tbFLengthIn.Text)) * 2.54;

            tbFLengthCm.Text = FormatDecimalNumber(FLengthCm, 0);
            tbFLengthCm.Text = string.Format("{0:#0}", Convert.ToDouble(tbFLengthCm.Text));
            txtFLengthCm();
        }
    }
    protected void txtPWidthFt()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double PWidthFt;
            double PWidthIn;
            PWidthFt = Convert.ToDouble(tbPWidthFt.Text == "" ? "0" : tbPWidthFt.Text) * 12;
            hnPWidthFt.Value = Convert.ToString(PWidthFt);
            PWidthIn = Convert.ToDouble(hnPWidthFt.Value) + Convert.ToDouble(tbPWidthIn.Text == "" ? "0" : tbPWidthIn.Text);
            varWinch = (long)PWidthIn;
            ViewState["varWinch"] = varWinch;
            double value = CalSquareYard(0);
            tbPAreaSqYd.Text = Convert.ToString(value);

            tbFWidthFt.Text = tbPWidthFt.Text;
            //tbFWidthFt_TextChanged(sender, new EventArgs());
            txtFWidthFt();

            ConvertProduction_FtToMtr();

            // tbPKhapW_TextChanged(sender, new EventArgs());
            CalculateFinishingSize(1);
            ConvertFinishing_FtToMtr();
        }
        else
        {
            lblMessage.Text = "Please select shape";
        }
    }
    protected void txtPWidthIn()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double PWidthIn;
            if (Convert.ToDouble(tbPWidthIn.Text == "" ? "0" : tbPWidthIn.Text) > 11)
            {
                tbPWidthIn.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Inch value must be less than 12');", true);
            }
            else
            {
                PWidthIn = Convert.ToDouble(hnPWidthFt.Value == "" ? "0" : hnPWidthFt.Value) + Convert.ToDouble(tbPWidthIn.Text == "" ? "0" : tbPWidthIn.Text);
                varWinch = (long)PWidthIn;
                ViewState["varWinch"] = varWinch;
                double value = CalSquareYard(0);
                tbPAreaSqYd.Text = Convert.ToString(value);

                tbFWidthIn.Text = tbPWidthIn.Text;
                //tbFWidthIn_TextChanged(sender, new EventArgs());
                txtFWidthIn();

                ConvertProduction_FtToMtr();

                ////tbPKhapW_TextChanged(sender, new EventArgs());
                //CalculateFinishingSize(1);
                //ConvertFinishing_FtToMtr();

            }
        }
    }
    protected void txtPLengthFt()
    {
        if (Convert.ToDouble(ddlShape.SelectedValue) > 0)
        {
            double PLengthFt;
            double PLengthIn;
            PLengthFt = Convert.ToDouble(tbPLengthFt.Text == "" ? "0" : tbPLengthFt.Text) * 12;
            hnPLengthFt.Value = Convert.ToString(PLengthFt);
            PLengthIn = Convert.ToDouble(hnPLengthFt.Value) + Convert.ToDouble(tbPLengthIn.Text == "" ? "0" : tbPLengthIn.Text);
            varLinch = (long)PLengthIn;
            ViewState["varLinch"] = varLinch;
            double value = CalSquareYard(0);
            tbPAreaSqYd.Text = Convert.ToString(value);

            tbFLengthFt.Text = tbPLengthFt.Text;
            //tbFLengthFt_TextChanged(sender, new EventArgs());
            txtFLengthFt();

            ConvertProduction_FtToMtr();

            // tbPKhapL_TextChanged(sender, new EventArgs());
            CalculateFinishingSize(2);
            ConvertFinishing_FtToMtr();

        }
        else
        {
            lblMessage.Text = "Please select shape";
        }
    }
    protected void txtPLengthIn()
    {
        if (Convert.ToDouble(ddlShape.SelectedValue) > 0)
        {
            double PLengthIn;
            if (Convert.ToDouble(tbPLengthIn.Text == "" ? "0" : tbPLengthIn.Text) > 11)
            {
                tbPLengthIn.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Inch value must be less than 12');", true);
            }
            else
            {
                PLengthIn = Convert.ToDouble(hnPLengthFt.Value == "" ? "0" : hnPLengthFt.Value) + Convert.ToDouble(tbPLengthIn.Text == "" ? "0" : tbPLengthIn.Text);
                varLinch = (long)PLengthIn;
                ViewState["varLinch"] = varLinch;
                double value = CalSquareYard(0);
                tbPAreaSqYd.Text = Convert.ToString(value);

                tbFLengthIn.Text = tbPLengthIn.Text;
                // tbFLengthIn_TextChanged(sender, new EventArgs());
                txtFLengthIn();

                ConvertProduction_FtToMtr();

                // tbPKhapL_TextChanged(sender, new EventArgs());
                CalculateFinishingSize(2);
                ConvertFinishing_FtToMtr();
            }
        }
    }

    protected void txtFWidthFt()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double FWidthFt;
            double FWidthIn;
            FWidthFt = Convert.ToDouble(tbFWidthFt.Text == "" ? "0" : tbFWidthFt.Text) * 12;
            hnFWidthFt.Value = Convert.ToString(FWidthFt);
            FWidthIn = Convert.ToDouble(hnFWidthFt.Value) + Convert.ToDouble(tbFWidthIn.Text == "" ? "0" : tbFWidthIn.Text);
            varWinch = (long)FWidthIn;
            ViewState["varWinch"] = varWinch;
            double value = CalSquareYard(1);
            tbFAreaSqYd.Text = Convert.ToString(value);
        }
        else
        {
            lblMessage.Text = "Please select shape";
        }
    }
    protected void txtFWidthIn()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double FWidthIn;
            if (Convert.ToDouble(tbFWidthIn.Text == "" ? "0" : tbFWidthIn.Text) > 11)
            {
                tbFWidthIn.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Inch value must be less than 12');", true);
            }
            else
            {
                FWidthIn = Convert.ToDouble(hnFWidthFt.Value == "" ? "0" : hnFWidthFt.Value) + Convert.ToDouble(tbFWidthIn.Text == "" ? "0" : tbFWidthIn.Text);
                varWinch = (long)FWidthIn;
                ViewState["varWinch"] = varWinch;
                double value = CalSquareYard(1);
                tbFAreaSqYd.Text = Convert.ToString(value);
            }
        }
    }
    protected void txtFLengthFt()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double FLengthFt;
            double FLengthIn;
            FLengthFt = Convert.ToDouble(tbFLengthFt.Text == "" ? "0" : tbFLengthFt.Text) * 12;
            hnFLengthFt.Value = Convert.ToString(FLengthFt);
            FLengthIn = Convert.ToDouble(hnFLengthFt.Value) + Convert.ToDouble(tbFLengthIn.Text == "" ? "0" : tbFLengthIn.Text);
            varLinch = (long)FLengthIn;
            ViewState["varLinch"] = varLinch;
            double value = CalSquareYard(1);
            tbFAreaSqYd.Text = Convert.ToString(value);
        }
        else
        {
            lblMessage.Text = "Please select shape";
        }

    }
    protected void txtFLengthIn()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            double FLengthIn;
            if (Convert.ToDouble(tbFLengthIn.Text == "" ? "0" : tbFLengthIn.Text) > 11)
            {
                tbFLengthIn.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "alrt", "alert('Inch value must be less than 12');", true);
            }
            else
            {
                FLengthIn = Convert.ToDouble(hnFLengthFt.Value == "" ? "0" : hnFLengthFt.Value) + Convert.ToDouble(tbFLengthIn.Text == "" ? "0" : tbFLengthIn.Text);
                varLinch = (long)FLengthIn;
                ViewState["varLinch"] = varLinch;
                double value = CalSquareYard(1);
                tbFAreaSqYd.Text = Convert.ToString(value);

            }
        }
    }
    protected void txtFWidthCm()
    {
        if (tbFLengthCm.Text == "" || tbFLengthCm.Text == "0" || tbFLengthCm.Text == null)
        {
            tbPAreaSqMt.Text = "0";
        }
        else if (tbFLengthCm.Text != "" || tbFLengthCm.Text != "0" || tbFLengthCm.Text != null)
        {
            double FAreaSqMt;
            FAreaSqMt = (Convert.ToDouble(tbFWidthCm.Text) * Convert.ToDouble(tbFLengthCm.Text)) / 10000;
            tbFAreaSqMt.Text = Convert.ToString(Math.Round((FAreaSqMt), 4));
        }
    }
    protected void txtFLengthCm()
    {
        if (tbFLengthCm.Text == "" || tbFLengthCm.Text == "0" || tbFLengthCm.Text == null)
        {
            tbFAreaSqMt.Text = "0";
        }
        else if (tbFLengthCm.Text != "" || tbFLengthCm.Text != "0" || tbFLengthCm.Text != null)
        {
            double FAreaSqMt;
            FAreaSqMt = (Convert.ToDouble(tbFWidthCm.Text) * Convert.ToDouble(tbFLengthCm.Text)) / 10000;
            tbFAreaSqMt.Text = Convert.ToString(Math.Round((FAreaSqMt), 4));
        }
    }
    protected void txtPWidthCm()
    {
        if (tbFLengthCm.Text == "" || tbFLengthCm.Text == "0" || tbFLengthCm.Text == null)
        {
            tbPAreaSqMt.Text = "0";
        }
        else if (tbFLengthCm.Text != "" || tbFLengthCm.Text != "0" || tbFLengthCm.Text != null)
        {
            double PAreaSqMt;
            PAreaSqMt = (Convert.ToDouble(tbPWidthCm.Text) * Convert.ToDouble(tbPLengthCm.Text)) / 10000;
            tbPAreaSqMt.Text = Convert.ToString(PAreaSqMt);
        }

        tbFWidthCm.Text = tbPWidthCm.Text;
        txtFWidthCm();
        //tbFWidthCm_TextChanged(sender, new EventArgs());
    }
    protected void txtPLengthCm()
    {
        if (tbPLengthCm.Text == "" || tbPLengthCm.Text == "0" || tbPLengthCm.Text == null)
        {
            tbPAreaSqMt.Text = "0";
        }
        else if (tbPLengthCm.Text != "" || tbPLengthCm.Text != "0" || tbPLengthCm.Text != null)
        {
            double PAreaSqMt;
            PAreaSqMt = (Convert.ToDouble(tbPWidthCm.Text) * Convert.ToDouble(tbPLengthCm.Text)) / 10000;
            tbPAreaSqMt.Text = Convert.ToString(PAreaSqMt);
        }

        tbFLengthCm.Text = tbPLengthCm.Text;
        txtFLengthCm();
        //tbFLengthCm_TextChanged(sender, new EventArgs());
    }
    protected void txtAWidthMtr()
    {
        if (tbAWidthMtr.Text != "" && tbALengthMtr.Text != "")
        {
            AreaMtSq(Convert.ToDouble(tbALengthMtr.Text), Convert.ToDouble(tbAWidthMtr.Text), tbAAreaSqMt);
        }
    }
    protected void txtALengthMtr()
    {
        if (tbAWidthMtr.Text != "" && tbALengthMtr.Text != "")
        {
            AreaMtSq(Convert.ToDouble(tbALengthMtr.Text), Convert.ToDouble(tbAWidthMtr.Text), tbAAreaSqMt);
        }
    }
    protected void BindgdMasterSize()
    {
        string where = "";

        if (ddlQualityType.SelectedIndex > 0)
        {
            where = where + " and QualityTypeId=" + ddlQualityType.SelectedValue;
        }
        if (ddlQuality.SelectedIndex > 0)
        {
            where = where + " and QualityId=" + ddlQuality.SelectedValue;
        }
        if (ddlShape.SelectedIndex > 0)
        {
            where = where + " and S.ShapeId=" + ddlShape.SelectedValue;
        }
        where = where + " and AddDate <='" + (tbEffectiveDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : tbEffectiveDate.Text) + "'";


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //if (btnSave.Text == "Save")
            //{
            //    hnSizeId.Value = "";                 
            //}

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@SizeId", "0");
            param[1] = new SqlParameter("@Where", where);

            //param[1] = new SqlParameter("@QualityTypeId", ddlQualityType.SelectedValue=="" ?"0" : ddlQualityType.SelectedValue);
            //param[2] = new SqlParameter("@QualityId", ddlQuality.SelectedValue == "" ? "0" : ddlQuality.SelectedValue);
            //param[3] = new SqlParameter("@ShapeId", ddlShape.SelectedValue);
            //param[4] = new SqlParameter("@AddDate", tbEffectiveDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : tbEffectiveDate.Text);


            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "pro_GetAllQualityMasterSize", param);

            //ds = SqlHelper.ExecuteDataset(con, CommandType.Text, where);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gdMasterSize.DataSource = ds;
                gdMasterSize.DataBind();
            }
            else
            {
                gdMasterSize.DataSource = null;
                gdMasterSize.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            //lblmsg.Text = ex.Message;
            con.Close();
        }

    }

    protected void ddlQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlQualityType.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", ddlQualityType.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQuality.DataValueField = "QualityID";
                ddlQuality.DataTextField = "QualityName";
                ddlQuality.DataSource = dt;
                ddlQuality.DataBind();
                ddlQuality.Items.Insert(0, "--Select--");
                ddlQuality.Items[0].Value = "0";

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }

            BindgdMasterSize();
        }
    }
    protected void ddlQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddlQuality.SelectedValue) > 0)
        //{
        BindgdMasterSize();
        // }
    }
    protected void ddlShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShape.SelectedIndex> 0)
        {
            BindgdMasterSize();
        }
    }
    protected void tbAWidthFt_TextChanged(object sender, EventArgs e)
    {
        AreaFootSq(tbALengthFt, tbAWidthFt);
        //tbALengthFt.Attributes.Add("(onfocus)", "this.selectAll()");


        // tbALengthFt.Focus();
    }
    protected void tbALengthFt_TextChanged(object sender, EventArgs e)
    {
        double Area;
        if (tbAWidthFt.Text != "" && tbALengthFt.Text != "")
        {
            var VarHeight = tbAWidthFt.Text == "" ? "0" : tbALengthFt.Text;

            AreaFootSq(tbALengthFt, tbAWidthFt);



            // Area =Convert.ToDouble(tbAWidthFt.Text) * Convert.ToDouble(tbALengthFt.Text) / 10000;
            //Area=Math.Round(Area,4);
            //tbAAreaSqFt.Text=Convert.ToString(Area);           
        }
    }
    protected void tbAWidthMtr_TextChanged(object sender, EventArgs e)
    {
        txtAWidthMtr();
    }
    protected void tbALengthMtr_TextChanged(object sender, EventArgs e)
    {
        txtALengthMtr();

    }
    protected void tbPWidthFt_TextChanged(object sender, EventArgs e)
    {
        if (hnSizeId.Value != null && btnSave.Text == "Update")
        {
            txtPWidthFt();
            txtPLengthFt();
        }
        else
        {
            txtPWidthFt();
        }

    }
    protected void tbPWidthIn_TextChanged(object sender, EventArgs e)
    {
        if (hnSizeId.Value != null && btnSave.Text == "Update")
        {
            txtPWidthIn();
            txtPLengthIn();
        }
        else
        {
            txtPWidthIn();
        }

    }

    protected void tbPLengthFt_TextChanged(object sender, EventArgs e)
    {
        if (hnSizeId.Value != null && btnSave.Text == "Update")
        {
            txtPWidthFt();
            txtPWidthIn();
            txtPLengthFt();
        }
        else
        {
            txtPLengthFt();
        }
    }
    protected void tbPLengthIn_TextChanged(object sender, EventArgs e)
    {
        if (hnSizeId.Value != null && btnSave.Text == "Update")
        {
            txtPWidthFt();
            txtPWidthIn();
            txtPLengthFt();
            txtPLengthIn();
            tbPKhapW_TextChanged(sender, new EventArgs());
        }
        else
        {
            txtPLengthIn();
            tbPKhapW_TextChanged(sender, new EventArgs());
        }
    }
    protected void tbPWidthCm_TextChanged(object sender, EventArgs e)
    {
        txtPWidthCm();

    }

    protected void tbPLengthCm_TextChanged(object sender, EventArgs e)
    {
        txtPLengthCm();

    }

    protected void tbFWidthFt_TextChanged(object sender, EventArgs e)
    {
        txtFWidthFt();
    }

    protected void tbFWidthIn_TextChanged(object sender, EventArgs e)
    {
        txtFWidthIn();
    }

    protected void tbFLengthFt_TextChanged(object sender, EventArgs e)
    {
        txtFLengthFt();
    }

    protected void tbFLengthIn_TextChanged(object sender, EventArgs e)
    {
        txtFLengthIn();
    }
    protected void tbFWidthCm_TextChanged(object sender, EventArgs e)
    {
        txtFWidthCm();
    }

    protected void tbFLengthCm_TextChanged(object sender, EventArgs e)
    {
        txtFLengthCm();
    }

    protected void tbPKhapW_TextChanged(object sender, EventArgs e)
    {
        CalculateFinishingSize(1);
        ConvertFinishing_FtToMtr();
    }
    protected void tbPKhapL_TextChanged(object sender, EventArgs e)
    {
        CalculateFinishingSize(2);
        ConvertFinishing_FtToMtr();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
        //return;
    }
    protected void Savedetail()
    {
        if (Convert.ToInt32(ddlShape.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                double Production_Area;
                double Finishing_Area;

                SqlParameter[] param = new SqlParameter[37];
                param[0] = new SqlParameter("@SizeId", SqlDbType.Int);
                param[1] = new SqlParameter("@Export_Format", SqlDbType.NVarChar, 50);
                param[2] = new SqlParameter("@Production_Mt_Format", SqlDbType.NVarChar, 50);
                param[3] = new SqlParameter("@Production_Ft_Format", SqlDbType.NVarChar, 50);
                param[4] = new SqlParameter("@UnitId", SqlDbType.Int);
                param[5] = new SqlParameter("@Export_Area", SqlDbType.Float);
                param[6] = new SqlParameter("@Production_MT_Area", SqlDbType.Float);
                param[7] = new SqlParameter("@Production_FT_Area", SqlDbType.Float);
                param[8] = new SqlParameter("@ShapeId", SqlDbType.Int);
                param[9] = new SqlParameter("@MtrSize", SqlDbType.NVarChar, 50);
                param[10] = new SqlParameter("@MtrArea", SqlDbType.Float);
                param[11] = new SqlParameter("@ProjFlag", SqlDbType.SmallInt);
                param[12] = new SqlParameter("@Finishing_Ft_Size", SqlDbType.NVarChar, 50);
                param[13] = new SqlParameter("@Finishing_Mt_Size", SqlDbType.NVarChar, 50);
                param[14] = new SqlParameter("@Finishing_Ft_Area", SqlDbType.Float);
                param[15] = new SqlParameter("@Finishing_Mt_Area", SqlDbType.Float);
                param[16] = new SqlParameter("@TypeS", SqlDbType.Int);
                param[17] = new SqlParameter("@KhapWidth", SqlDbType.Float);
                param[18] = new SqlParameter("@KhapLength", SqlDbType.Float);
                param[19] = new SqlParameter("@QualityTypeId", SqlDbType.Int);
                param[20] = new SqlParameter("@QualityId", SqlDbType.Int);

                param[21] = new SqlParameter("@SizeStatus", SqlDbType.TinyInt);
                param[22] = new SqlParameter("@ExpWidthMS_Ft", SqlDbType.NVarChar, 10);
                param[23] = new SqlParameter("@ExpLengthMS_Ft", SqlDbType.NVarChar, 10);
                param[24] = new SqlParameter("@FootWidth", SqlDbType.Int);
                param[25] = new SqlParameter("@InchWidthOfFoot", SqlDbType.Int);
                param[26] = new SqlParameter("@FootLength", SqlDbType.Int);
                param[27] = new SqlParameter("@InchLengthOfFoot", SqlDbType.Int);
                param[28] = new SqlParameter("@ProductionAreaSqYard", SqlDbType.Float);
                param[29] = new SqlParameter("@FinishingAreaSqYard", SqlDbType.Float);
                param[30] = new SqlParameter("@ProdWidth_MS", SqlDbType.Float);
                param[31] = new SqlParameter("@ProdLength_MS", SqlDbType.Float);
                param[32] = new SqlParameter("@ActualAreaSqYard", SqlDbType.Float);
                param[33] = new SqlParameter("@ActualFullAreaSqYard", SqlDbType.Float);
                param[34] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[35] = new SqlParameter("@AddDate", SqlDbType.SmallDateTime);



                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnSizeId.Value == "" ? "0" : hnSizeId.Value;

                string PWidthIn;
                if (tbPWidthIn.Text.Length > 1)
                {
                    PWidthIn = tbPWidthIn.Text;
                }
                else
                {
                    PWidthIn = "0" + tbPWidthIn.Text;
                }

                string PLengthIn;
                if (tbPLengthIn.Text.Length > 1)
                {
                    PLengthIn = tbPLengthIn.Text;
                }
                else
                {
                    PLengthIn = "0" + tbPLengthIn.Text;
                }

                string FWidthIn;
                if (tbFWidthIn.Text.Length > 1)
                {
                    FWidthIn = tbFWidthIn.Text;
                }
                else
                {
                    FWidthIn = "0" + tbFWidthIn.Text;
                }

                string FLengthIn;
                if (tbFLengthIn.Text.Length > 1)
                {
                    FLengthIn = tbFLengthIn.Text;
                }
                else
                {
                    FLengthIn = "0" + tbFLengthIn.Text;
                }

                string TempPWidth = Convert.ToDouble(tbPWidthFt.Text) + "." + (PWidthIn);
                string TempPLength = Convert.ToDouble(tbPLengthFt.Text) + "." + (PLengthIn);
                string Fini_Ft_Format = string.Format("{0:#0.00}", TempPWidth) + "x" + string.Format("{0:#0.00}", TempPLength);

                string TempFWidth = Convert.ToDouble(tbFWidthFt.Text) + "." + (FWidthIn);
                string TempFLength = Convert.ToDouble(tbFLengthFt.Text) + "." + (FLengthIn);
                string Fini_Ft_Format2 = string.Format("{0:#0.00}", TempFWidth) + "x" + string.Format("{0:#0.00}", TempFLength);

                if (cbProduction.Checked == true)
                {

                    Production_Area = Convert.ToDouble(tbPFullAreaSqYd.Text);
                }
                else
                {
                    Production_Area = Convert.ToDouble(tbPAreaSqYd.Text);
                }

                if (cbFinishing.Checked == true)
                {

                    Finishing_Area = Convert.ToDouble(tbPFullAreaSqYd.Text);
                }
                else
                {
                    Finishing_Area = Convert.ToDouble(tbPAreaSqYd.Text);
                }


                param[1].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbAWidthFt.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbALengthFt.Text));
                // param[2].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbPWidthCm.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbPLengthCm.Text));
                param[2].Value = string.Format("{0:#000}", Convert.ToDouble(tbPWidthCm.Text)) + 'x' + string.Format("{0:#000}", Convert.ToDouble(tbPLengthCm.Text));
                // param[3].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbPWidthFt.Text)) + '.' + string.Format("{0:#0.00}", Convert.ToDouble(tbPWidthIn.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbPLengthFt.Text)) + '.' + string.Format("{0:#0.00}", Convert.ToDouble(tbPLengthIn.Text));
                param[3].Value = Fini_Ft_Format;
                param[4].Value = ddlUnit.SelectedIndex > 0 ? ddlUnit.SelectedValue : "0";
                param[5].Value = tbAAreaSqFt.Text == "" ? "0" : tbAAreaSqFt.Text;
                param[6].Value = tbPAreaSqMt.Text == "" ? "0" : tbPAreaSqMt.Text;
                param[7].Value = tbPAreaSqYd.Text == "" ? "0" : tbPAreaSqYd.Text;
                param[8].Value = ddlShape.SelectedIndex > 0 ? ddlShape.SelectedValue : "0";
                //param[9].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbAWidthMtr.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbALengthMtr.Text));
                param[9].Value = (tbAWidthMtr.Text) + 'x' + (tbALengthMtr.Text);
                param[10].Value = tbAAreaSqMt.Text == "" ? "0" : tbAAreaSqMt.Text;
                param[11].Value = 0;
                // string text = tbPWidthFt.Text + '.' + tbPWidthIn.Text + 'x' + tbPLengthFt.Text + '.' + tbPLengthIn.Text;
                //param[12].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbFWidthFt.Text)) + '.' +string.Format("{0:#0.00}",Convert.ToDouble(tbFWidthIn.Text))  + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbFLengthFt.Text))+ '.' +string.Format("{0:#0.00}", Convert.ToDouble(tbFLengthIn.Text));
                param[12].Value = Fini_Ft_Format2;
                //param[13].Value = string.Format("{0:#0.00}", Convert.ToDouble(tbFWidthCm.Text)) + 'x' + string.Format("{0:#0.00}", Convert.ToDouble(tbFLengthCm.Text));
                param[13].Value = string.Format("{0:#000}", Convert.ToDouble(tbFWidthCm.Text)) + 'x' + string.Format("{0:#000}", Convert.ToDouble(tbFLengthCm.Text));
                param[14].Value = tbFAreaSqYd.Text == "" ? "0" : tbFAreaSqYd.Text;
                param[15].Value = tbFAreaSqMt.Text == "" ? "0" : tbFAreaSqMt.Text;

                if (lblStatus.Text == "No")
                {
                    param[16].Value = 0;
                }
                else
                {
                    param[16].Value = 1;
                }
                //param[16].Value = 1;
                param[17].Value = tbPKhapW.Text == "" ? "0" : tbPKhapW.Text;
                param[18].Value = tbPKhapL.Text == "" ? "0" : tbPKhapL.Text;
                param[19].Value = ddlQualityType.SelectedIndex > 0 ? ddlQualityType.SelectedValue : "0";
                param[20].Value = ddlQuality.SelectedIndex > 0 ? ddlQuality.SelectedValue : "0";

                param[21].Value = 1;
                param[22].Value = tbAWidthFt.Text == "" ? "0" : tbAWidthFt.Text;
                param[23].Value = tbALengthFt.Text == "" ? "0" : tbALengthFt.Text;
                param[24].Value = Convert.ToDouble(tbPWidthFt.Text == "" ? "0" : tbPWidthFt.Text);
                param[25].Value = Convert.ToDouble(tbPWidthIn.Text == "" ? "0" : tbPWidthIn.Text);
                param[26].Value = Convert.ToDouble(tbPLengthFt.Text == "" ? "0" : tbPLengthFt.Text);
                param[27].Value = Convert.ToDouble(tbPLengthIn.Text == "" ? "0" : tbPLengthIn.Text);

                //param[28].Value = tbPAreaSqYd.Text == "" ? "0" : tbPAreaSqYd.Text;
                //param[29].Value = tbPFullAreaSqYd.Text == "" ? "0" : tbPFullAreaSqYd.Text;

                param[28].Value = Production_Area;
                param[29].Value = Finishing_Area;

                param[30].Value = tbPWidthCm.Text == "" ? "0" : tbPWidthCm.Text;
                param[31].Value = tbPLengthCm.Text == "" ? "0" : tbPLengthCm.Text;
                param[32].Value = tbPAreaSqYd.Text == "" ? "0" : tbPAreaSqYd.Text;
                param[33].Value = tbPFullAreaSqYd.Text == "" ? "0" : tbPFullAreaSqYd.Text;
                param[34].Direction = ParameterDirection.Output;
                param[35].Value = tbEffectiveDate.Text;
                // param[34] .Value="";

                //**********
                if (Session["varMasterCompanyIDForERP"].ToString() == "20" && variable.VarNewQualitySize == "1")
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveCarpetMasterSizeMaltiRug", param);
                }
                else
                {

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveCarpetMasterSize", param);
                }
                lblMessage.Text = param[34].Value.ToString();
                Tran.Commit();
                btnSave.Text = "Save";
                BindgdMasterSize();
                Refreshcontrol();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "alert", "alert('Please select shape...')", true);
        }
    }
    protected void Refreshcontrol()
    {
        // ddlQualityType.SelectedValue = "0";
        //ddlQuality.SelectedValue = "0";
        //ddlShape.SelectedValue = "0";
        tbAWidthFt.Text = "";
        tbALengthFt.Text = "";
        tbAWidthMtr.Text = "";
        tbALengthMtr.Text = "";
        tbAAreaSqFt.Text = "";
        tbAAreaSqMt.Text = "";


        tbPWidthFt.Text = "";
        tbPWidthIn.Text = "";
        tbPLengthFt.Text = "";
        tbPLengthIn.Text = "";
        tbPWidthCm.Text = "";
        tbPLengthCm.Text = "";
        tbPAreaSqYd.Text = "";
        tbPAreaSqMt.Text = "";
        tbPFullAreaSqYd.Text = "";

        tbFWidthFt.Text = "";
        tbFWidthIn.Text = "";
        tbFLengthFt.Text = "";
        tbFLengthIn.Text = "";
        tbFWidthCm.Text = "";
        tbFLengthCm.Text = "";
        tbFAreaSqYd.Text = "";
        tbFAreaSqMt.Text = "";
        tbPKhapW.Text = "";
        tbPKhapL.Text = "";
        hnSizeId.Value = "";

    }
    protected void gdMasterSize_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = ((Label)e.Row.FindControl("lblStatus"));
            if (lblStatus.Text == "No")
            {
                //e.Row.BackColor = Color.Green;
                e.Row.BackColor = Color.FromName("#0080C0");
                //e.Row.Attributes["style"] = "background-color:#0080C0";
            }
            else
            {
                // e.Row.BackColor = Color.FromName("#E3E3E3");
                //e.Row.Attributes["style"] = "background-color:#E3E3E3";
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "20" && variable.VarNewQualitySize == "1")
            {

                gdMasterSize.HeaderRow.Cells[0].Visible = false;
                e.Row.Cells[0].Visible = false;

                gdMasterSize.HeaderRow.Cells[2].Visible = true;
                e.Row.Cells[2].Visible = true;

                gdMasterSize.HeaderRow.Cells[3].Visible = true;
                e.Row.Cells[3].Visible = true;
            }
            else
            {
                gdMasterSize.HeaderRow.Cells[0].Visible = true;
                e.Row.Cells[0].Visible = true;

                gdMasterSize.HeaderRow.Cells[2].Visible = false;
                e.Row.Cells[2].Visible = false;

                gdMasterSize.HeaderRow.Cells[3].Visible = false;
                e.Row.Cells[3].Visible = false;
            }


            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdMasterSize, "select$" + e.Row.RowIndex);

        }


    }
    protected void gdMasterSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(gdMasterSize.SelectedIndex.ToString());
        CheckBox chk = ((CheckBox)gdMasterSize.Rows[r].FindControl("Chkboxitem")) as CheckBox;

        if (chk.Checked == true)
        {

        }
        else
        {
            lblMessage.Text = "";

            btnSave.Text = "Save";

            string id = gdMasterSize.SelectedDataKey.Value.ToString();
            hnSizeId.Value = id;

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SizeId", id);

                //**********
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "pro_GetAllQualityMasterSize", param);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    // int designid = 0;
                    ddlQualityType.SelectedValue = ds.Tables[0].Rows[0]["QualityTypeId"].ToString();
                    BindQualityName();
                    ddlQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();

                    //hnQualityId.Value = ddlQualityName.SelectedValue;

                    ddlUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                    ddlShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();

                    //tbEffectiveDate.Text = ds.Tables[0].Rows[0]["AddDate"].ToString();
                    if (Session["varMasterCompanyIDForERP"].ToString() == "20" && variable.VarNewQualitySize == "1")
                    {
                        tbEffectiveDate.Text = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["AddDate"].ToString()).ToString("dd-MMM-yyyy"));
                    }


                    tbAWidthFt.ReadOnly = true;
                    tbALengthFt.ReadOnly = true;
                    tbAWidthMtr.ReadOnly = true;
                    tbALengthMtr.ReadOnly = true;

                    tbAWidthFt.Text = ds.Tables[0].Rows[0]["ExpWidthMS_Ft"].ToString();
                    tbALengthFt.Text = ds.Tables[0].Rows[0]["ExpLengthMS_Ft"].ToString();
                    tbAAreaSqFt.Text = ds.Tables[0].Rows[0]["Export_Area"].ToString();
                    tbAWidthMtr.Text = ds.Tables[0].Rows[0]["AWidthMtr"].ToString();
                    tbALengthMtr.Text = ds.Tables[0].Rows[0]["ALengthMtr"].ToString();
                    tbAAreaSqMt.Text = ds.Tables[0].Rows[0]["MtrArea"].ToString();

                    //tbPWidthFt.ReadOnly = true;
                    //tbPWidthIn.ReadOnly = true;
                    //tbPLengthFt.ReadOnly = true;
                    //tbPLengthIn.ReadOnly = true;
                    //tbPWidthCm.ReadOnly = true;
                    //tbPLengthCm.ReadOnly = true;

                    tbPWidthFt.Text = ds.Tables[0].Rows[0]["FootWidth"].ToString();
                    tbPWidthIn.Text = ds.Tables[0].Rows[0]["InchWidthOfFoot"].ToString();
                    tbPLengthFt.Text = ds.Tables[0].Rows[0]["FootLength"].ToString();
                    tbPLengthIn.Text = ds.Tables[0].Rows[0]["InchLengthOfFoot"].ToString();
                    tbPWidthCm.Text = ds.Tables[0].Rows[0]["ProdWidth_MS"].ToString();
                    tbPLengthCm.Text = ds.Tables[0].Rows[0]["ProdLength_MS"].ToString();

                    tbPKhapW.Text = ds.Tables[0].Rows[0]["KhapWidth"].ToString();
                    tbPKhapL.Text = ds.Tables[0].Rows[0]["KhapLength"].ToString();

                    tbPAreaSqYd.Text = ds.Tables[0].Rows[0]["ActualAreaSqYard"].ToString();
                    tbPFullAreaSqYd.Text = ds.Tables[0].Rows[0]["ActualFullAreaSqYard"].ToString();
                    tbPAreaSqMt.Text = ds.Tables[0].Rows[0]["Production_MT_Area"].ToString();

                    tbFWidthFt.ReadOnly = true;
                    tbFWidthIn.ReadOnly = true;
                    tbFLengthFt.ReadOnly = true;
                    tbFLengthIn.ReadOnly = true;

                    tbFWidthFt.Text = ds.Tables[0].Rows[0]["FWidthFt2"].ToString();
                    string FWidthIn = ds.Tables[0].Rows[0]["FWidthIn2"].ToString();
                    tbFWidthIn.Text = FWidthIn.TrimStart('0');

                    tbFLengthFt.Text = ds.Tables[0].Rows[0]["FLengthFt2"].ToString();
                    string FLengthIn = ds.Tables[0].Rows[0]["FLengthIn2"].ToString();
                    tbFLengthIn.Text = FLengthIn.TrimStart('0');

                    string FWidthCm = ds.Tables[0].Rows[0]["FWidthCm"].ToString();
                    tbFWidthCm.Text = FWidthCm.TrimStart('0');
                    string FLengthCm = ds.Tables[0].Rows[0]["FLengthCm"].ToString();
                    tbFLengthCm.Text = FLengthCm.TrimStart('0');

                    tbFAreaSqYd.Text = ds.Tables[0].Rows[0]["Finishing_Ft_Area"].ToString();
                    tbFAreaSqMt.Text = ds.Tables[0].Rows[0]["Finishing_Mt_Area"].ToString();

                    lblStatus.Text = ds.Tables[0].Rows[0]["TypeS"].ToString();
                }

                BindgdMasterSize();

                Tran.Commit();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            btnSave.Text = "Update";
        }
    }
    protected void gdMasterSize_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdMasterSize.PageIndex = e.NewPageIndex;
        BindgdMasterSize();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        Label lblSizeId = (Label)clickedRow.FindControl("lblSizeId");
        Label lblStatus = (Label)clickedRow.FindControl("lblStatus");

        if (lblStatus.Text == "Yes")
        {
            lblStatus.Text = "1";
        }
        else
        {
            lblStatus.Text = "0";
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@SizeId", Convert.ToInt32(lblSizeId.Text));
            param[1] = new SqlParameter("@TypeS", Convert.ToInt32(lblStatus.Text));
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateCarpetMasterSizeStatus", param);
            lblMessage.Text = param[2].Value.ToString();

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        BindgdMasterSize();
    }

    protected void BtnCalCulate_Click(object sender, EventArgs e)
    {
        if (tbAWidthFt.Text == "")
        {
            ConvertMtrToFt();
        }
        else if (tbAWidthMtr.Text == "" || tbAWidthMtr.Text != null)
        {
            //CalculateArea();
            CalculateArea(tbAWidthFt, tbALengthFt);
        }
        //else if(tbAWidthMtr.Text=="")
        //{
        //    //CalculateArea();
        //    CalculateArea(tbAWidthFt, tbALengthFt);
        //}

    }
    protected void ConvertMtrToFt()
    {
        int i;
        int j;
        string x;
        string y;
        double a;
        double b;

        a = Convert.ToDouble(tbAWidthMtr.Text == "" ? "0" : tbAWidthMtr.Text) / 2.54;
        b = Convert.ToDouble(tbALengthMtr.Text == "" ? "0" : tbALengthMtr.Text) / 2.54;
        i = Convert.ToInt32(a / 12);

        x = i + "." + string.Format("{0:#00}", a % 12);
        // x = i + "." + string.Format("{0#}",a);         
        j = Convert.ToInt32(b / 12);
        // y = j + "." + string.Format("{0#}",b);
        y = j + "." + string.Format("{0:#00}", b % 12);
        tbAWidthFt.Text = x;
        tbALengthFt.Text = y;

        AreaFootSq(tbALengthFt, tbAWidthFt);
    }
    protected void CalculateArea(TextBox VarWidthNew2, TextBox VarLengthNew2)
    {
        string WidthMtr = "";
        int WidthMtr2 = 0;
        string WidthCm = "";
        int WidthCm2 = 0;
        string LengthMtr = "";
        int LengthMtr2 = 0;
        string LengthCm = "";
        int LengthCm2 = 0;
        string TotalWidthCm = "";
        double TotalWidthCm2 = 0;
        string TotalLengthCm = "";
        double TotalLengthCm2 = 0;
        string Str = "";

        if (VarWidthNew2.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew2.Text));
            if (VarWidthNew2.Text != "")
            {
                WidthMtr2 = Convert.ToInt32(Str.Split('.')[0]);
                WidthCm2 = Convert.ToInt32(Str.Split('.')[1]);
            }

        }

        if (VarLengthNew2.Text != "")
        {
            Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew2.Text));
            if (VarLengthNew2.Text != "")
            {
                LengthMtr2 = Convert.ToInt32(Str.Split('.')[0]);
                LengthCm2 = Convert.ToInt32(Str.Split('.')[1]);
            }

        }

        TotalWidthCm2 = ((WidthMtr2 * 12) + WidthCm2) * 2.54;
        TotalLengthCm2 = ((LengthMtr2 * 12) + LengthCm2) * 2.54;

        TotalWidthCm = FormatDecimalNumber(TotalWidthCm2, 0);
        TotalWidthCm2 = Math.Round(Convert.ToDouble(TotalWidthCm), 0);
        tbAWidthMtr.Text = Convert.ToString(TotalWidthCm2);

        txtAWidthMtr();


        TotalLengthCm = FormatDecimalNumber(TotalLengthCm2, 0);
        TotalLengthCm2 = Math.Round(Convert.ToDouble(TotalLengthCm), 0);
        tbALengthMtr.Text = Convert.ToString(TotalLengthCm2);
        txtALengthMtr();

        //AreaMtSq1(tbALengthMtr, tbAWidthMtr);
        // AreaMtSq(tbALengthMtr, tbAWidthMtr, tbAAreaSqMt);

        AreaMtSq(Convert.ToDouble(tbALengthMtr.Text), Convert.ToDouble(tbAWidthMtr.Text), tbAAreaSqMt);

    }

    protected void cbProduction_CheckedChanged(object sender, EventArgs e)
    {
        lblProductionArea.Text = "Production Area";
        if (cbFinishing.Checked == true)
        {
            cbFinishing.Checked = false;
        }
    }
    protected void cbFinishing_CheckedChanged(object sender, EventArgs e)
    {
        lblProductionArea.Text = "Finishing Area";
        if (cbProduction.Checked == true)
        {
            cbProduction.Checked = false;
        }
    }

    protected void ddlQualityTypeAttac_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlQualityTypeAttac.SelectedValue) > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("@mode", "Get");
                param.Add("@Item_Id", ddlQualityTypeAttac.SelectedValue);
                DataTable dt = DataAccess.fetch("Pro_GetQualityName", param);

                ddlQualityAttac.DataValueField = "QualityID";
                ddlQualityAttac.DataTextField = "QualityName";
                ddlQualityAttac.DataSource = dt;
                ddlQualityAttac.DataBind();
                ddlQualityAttac.Items.Insert(0, "--Select--");
                ddlQualityAttac.Items[0].Value = "0";

                string selectedSub = ddlQuality.SelectedItem.Text;

                ddlQualityAttac.Items.FindByText(selectedSub).Enabled = false;

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                //lblmsg.Text = ex.Message;
                con.Close();
            }
        }
    }
    private void Save_AttachedSize()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("SizeId", typeof(int));
        dtrecords.Columns.Add("QualityTypeId", typeof(int));
        dtrecords.Columns.Add("QualityId", typeof(int));

        for (int i = 0; i < gdMasterSize.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)gdMasterSize.Rows[i].FindControl("Chkboxitem"));

            if (chkoutItem.Checked == true)
            {
                DataRow dr = dtrecords.NewRow();
                //***********
                Label lblSizeId = ((Label)gdMasterSize.Rows[i].FindControl("lblSizeId"));

                //**************
                dr["SizeId"] = lblSizeId.Text;
                dr["QualityTypeId"] = ddlQualityTypeAttac.SelectedValue == "" ? "0" : ddlQualityTypeAttac.SelectedValue;
                dr["QualityId"] = ddlQualityAttac.SelectedValue == "" ? "0" : ddlQualityAttac.SelectedValue;
                dtrecords.Rows.Add(dr);
            }
        }
        //********************
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@SizeId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = 0;
                param[1] = new SqlParameter("@QualityTypeId", ddlQualityTypeAttac.SelectedValue == "" ? "0" : ddlQualityTypeAttac.SelectedValue);
                param[2] = new SqlParameter("@QualityId", ddlQualityAttac.SelectedValue == "" ? "0" : ddlQualityAttac.SelectedValue);
                param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                param[4] = new SqlParameter("@dtrecords", dtrecords);
                //**********

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveAttachedCarpetMasterSize", param);
                //int rowscount;
                //rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveAttachedCarpetMasterSize", param);
                Tran.Commit();
                //string intid = param[0].Value.ToString();               
                lblMessage.Text = param[3].Value.ToString();
                BindgdMasterSize();
                Refreshcontrol2();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void btnAttached_Click(object sender, EventArgs e)
    {
        if (ddlQualityTypeAttac.SelectedIndex > 0 && ddlQualityAttac.SelectedIndex > 0)
        {
            lblMessage.Text = "";
            Save_AttachedSize();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please select Quality Type and Quality...')", true);
        }
    }
    protected void Refreshcontrol2()
    {
        ddlQualityAttac.SelectedValue = "0";
        ddlQualityTypeAttac.SelectedValue = "0";

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/RptSizeNew.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[4];
            _arrPara[0] = new SqlParameter("@SizeId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@QualityTypeId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@QualityId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@ShapeId", SqlDbType.Int);

            _arrPara[0].Value = "0";
            _arrPara[1].Value = ddlQualityType.SelectedValue == "" ? "0" : ddlQualityType.SelectedValue;
            _arrPara[2].Value = ddlQuality.SelectedValue == "" ? "0" : ddlQuality.SelectedValue;
            _arrPara[3].Value = ddlShape.SelectedValue;


            //**********         

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "pro_GetAllQualityMasterSizeReportData", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\RptSizeNew.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void tbEffectiveDate_TextChanged(Object sender, EventArgs e)
    {
        BindgdMasterSize();

    }
}