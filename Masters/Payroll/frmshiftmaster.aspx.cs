using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmshiftmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"SELECT CI.COMPANYID,CI.COMPANYNAME FROM COMPANYINFO CI INNER JOIN COMPANY_AUTHENTICATION CA ON CI.COMPANYID=CA.COMPANYID
                          WHERE CA.USERID=" + Session["varuserid"] + " ORDER BY COMPANYNAME";

            UtilityModule.ConditionalComboFill(ref DDcompany, str, false, "");

            if (DDcompany.Items.FindByValue(Session["dcompanyid"].ToString()) != null)
            {
                DDcompany.SelectedValue = Session["dcompanyid"].ToString();
            }
            switch (Session["usertype"].ToString())
            {
                case "1":
                case "2":
                    Tdedit.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[51];
            arr[0] = new SqlParameter("@SHIFTID", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnshiftid.Value;
            arr[1] = new SqlParameter("@COMPANYID", DDcompany.SelectedValue);
            arr[2] = new SqlParameter("@SHIFTCODE", txtshiftcode.Text);
            arr[3] = new SqlParameter("@SHIFTTYPE", (rdday.Checked == true ? "1" : (rdnight.Checked == true ? "2" : "0")));
            arr[4] = new SqlParameter("@INTIME", txtintime.Text);
            arr[5] = new SqlParameter("@OUTTIME", txtouttime.Text);
            arr[6] = new SqlParameter("@LUNCHBREAK", Rdlunchbreakyes.Checked == true ? "1" : (Rdlunchbreakno.Checked == true ? "0" : "-1"));
            arr[7] = new SqlParameter("@LUNCH_BREAKOUT", txtbreakout.Text);
            arr[8] = new SqlParameter("@LUNCH_BREAKIN", txtbreakin.Text);
            arr[9] = new SqlParameter("@LUNCH_BREAKOUTSTART", txtbreakoutstart.Text);
            arr[10] = new SqlParameter("@LUNCH_BREAKINEND", txtbreakinend.Text);
            arr[11] = new SqlParameter("@LUNCH_BREAKDEDUCTION", Rdbreakdeductionyes.Checked == true ? "1" : (Rdbreakdeductionno.Checked == true ? "0" : "-1"));
            arr[12] = new SqlParameter("@LUNCH_EXCESSBREAKDEDUCTION", Rdexcessbreakdeductionyes.Checked == true ? "1" : (Rdexcessbreakdeductionno.Checked == true ? "0" : "-1"));
            arr[13] = new SqlParameter("@TEA_STARTFIRST", txtteastartist.Text);
            arr[14] = new SqlParameter("@TEA_ENDFIRST", txtteaendist.Text);
            arr[15] = new SqlParameter("@TEA_STARTSECOND", txtteastartiind.Text);
            arr[16] = new SqlParameter("@TEA_ENDSECOND", txtteaendiind.Text);
            arr[17] = new SqlParameter("@OTBEFORE", rdotbeforeyes.Checked == true ? "1" : (rdotbeforeno.Checked == true ? "0" : "-1"));
            arr[18] = new SqlParameter("@OTBEFORE_TIME", txtotbeforehour.Text);
            arr[19] = new SqlParameter("@OTAFTER", rdotafteryes.Checked == true ? "1" : (rdotafterno.Checked == true ? "0" : "-1"));
            arr[20] = new SqlParameter("@OTAFTER_TIME", txtotafterhour.Text);
            arr[21] = new SqlParameter("@HALFDAYHRS", txthalfdayhrs.Text);
            arr[22] = new SqlParameter("@FULLDAYHRS", txtfulldayhrs.Text);
            arr[23] = new SqlParameter("@LATEARRIVALGRACE", txtlatearrivalgrace.Text);
            arr[24] = new SqlParameter("@ARRIVALGRACEDEDUCTION", rdarrivalgracedeductionyes.Checked == true ? "1" : (rdarrivalgracedeductionno.Checked == true ? "0" : "-1"));
            arr[25] = new SqlParameter("@EARLYDEPARTUREGRACE", txtearlydeparturegrace.Text);
            arr[26] = new SqlParameter("@DEPARTUREGRACEDEDUCTION", rddeparturegracedeductionyes.Checked == true ? "1" : (rddeparturegracedeductionno.Checked == true ? "0" : "-1"));
            arr[27] = new SqlParameter("@FULLSHIFT", rdfullshiftyes.Checked == true ? "1" : (rdfullshiftno.Checked == true ? "0" : "-1"));
            arr[28] = new SqlParameter("@HALFSHIFT", txthalfshift.Text);
            arr[29] = new SqlParameter("@INSTART", txtinstart.Text);
            arr[30] = new SqlParameter("@INEND", txtinend.Text);
            arr[31] = new SqlParameter("@OUTSTART", txtoutstart.Text);
            arr[32] = new SqlParameter("@OUTEND", txtoutend.Text);
            arr[33] = new SqlParameter("@WORKHOUR", txtworkhour.Text);
            arr[34] = new SqlParameter("@CROPBEFORE", txtcropbefore.Text);
            arr[35] = new SqlParameter("@CROPAFTER", txtcropafter.Text);
            arr[36] = new SqlParameter("@BEFORERAND", txtbeforerand.Text);
            arr[37] = new SqlParameter("@AFTERRAND", txtafterrand.Text);
            arr[38] = new SqlParameter("@TEABREAKHOURAFTERSHIFT", txtteabreakhouraftershift.Text);
            arr[39] = new SqlParameter("@DINNERBREAKDEDUCTAFTER", txtdinnerbreakdeductafter.Text);
            arr[40] = new SqlParameter("@HOUR", txtdinnerbreakdeductafterhour.Text);
            arr[41] = new SqlParameter("@USERID", Session["varuserid"]);
            arr[42] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            arr[42].Direction = ParameterDirection.Output;
            arr[43] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyId"]);
            arr[44] = new SqlParameter("@InTime_Relaxation", txtintimerelaxation.Text == "" ? "0" : txtintimerelaxation.Text);

            arr[45] = new SqlParameter("@ShiftMinIntime", TxtShiftMinIntime.Text == "" ? "00:00" : TxtShiftMinIntime.Text);
            arr[46] = new SqlParameter("@ShiftMaxIntime", TxtShiftMaxIntime.Text == "" ? "00:00" : TxtShiftMaxIntime.Text);
            arr[47] = new SqlParameter("@NextDay", TxtNextDay.Text == "" ? "0" : TxtNextDay.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Hr_Pro_Hrshiftmaster]", arr);
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + arr[42].Value.ToString() + "')", true);
            lblmsg.Text = arr[42].Value.ToString();
            //refreshcontrol();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void refreshcontrol()
    {
        txtshiftcode.Text = "";
        rdday.Checked = true;
        rdnight.Checked = false;
        txtintime.Text = "";
        txtouttime.Text = "";
        Rdlunchbreakyes.Checked = false;
        Rdlunchbreakno.Checked = false;
        txtbreakout.Text = "";
        txtbreakin.Text = "";
        txtbreakoutstart.Text = "";
        txtbreakinend.Text = "";
        Rdbreakdeductionyes.Checked = false;
        Rdbreakdeductionno.Checked = false;
        Rdexcessbreakdeductionyes.Checked = false;
        Rdexcessbreakdeductionno.Checked = false;
        txtteastartist.Text = "";
        txtteaendist.Text = "";
        txtteastartiind.Text = "";
        txtteaendiind.Text = "";
        rdotbeforeyes.Checked = false;
        rdotbeforeno.Checked = false;
        txtotbeforehour.Text = "";
        rdotafteryes.Checked = false;
        rdotafterno.Checked = false;
        txtotafterhour.Text = "";
        txthalfdayhrs.Text = "";
        txtfulldayhrs.Text = "";
        txtlatearrivalgrace.Text = "";
        rdarrivalgracedeductionyes.Checked = false;
        rdarrivalgracedeductionno.Checked = false;
        txtearlydeparturegrace.Text = "";
        rddeparturegracedeductionyes.Checked = false;
        rddeparturegracedeductionno.Checked = false;
        rdfullshiftyes.Checked = false;
        rdfullshiftno.Checked = false;
        txthalfshift.Text = "";
        txtinstart.Text = "";
        txtinend.Text = "";
        txtoutstart.Text = "";
        txtoutend.Text = "";
        txtworkhour.Text = "";
        txtcropbefore.Text = "";
        txtbeforerand.Text = "";
        txtcropafter.Text = "";
        txtafterrand.Text = "";
        txtteabreakhouraftershift.Text = "";
        txtdinnerbreakdeductafter.Text = "";
        txtdinnerbreakdeductafterhour.Text = "";
        TxtNextDay.Text = "0";
        TxtShiftMinIntime.Text = "";
        TxtShiftMaxIntime.Text = "";
    }

    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        Trshiftcode.Visible = false;
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            Trshiftcode.Visible = true;
            Fillshiftcode();
        }
    }
    protected void Fillshiftcode()
    {
        string str = "select ShiftId,shiftcode From HR_ShiftMaster(Nolock) Where Companyid=" + DDcompany.SelectedValue + " order by shiftcode";
        UtilityModule.ConditionalComboFill(ref DDshiftcode, str, true, "--Plz Select--");
    }

    protected void DDcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillshiftcode();
    }
    protected void DDshiftcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        refreshcontrol();
        hnshiftid.Value = DDshiftcode.SelectedValue;

        string str = @"select Companyid, shiftcode, shifttype, Intime, Outtime, Lunchbreak, Lunch_Breakout, Lunch_Breakin, Lunch_BreakOutstart, 
                      Lunch_BreakinEnd, Lunch_BreakDeduction, Lunch_ExcessBreakDeduction, Tea_Startfirst, Tea_endfirst, 
                      Tea_Startsecond, Tea_Endsecond, OTBefore, OTbefore_time, OTafter, OTafter_time, Halfdayhrs, Fulldayhrs, 
                      Latearrivalgrace, Arrivalgracededuction, EarlyDeparturegrace, Departuregracededuction, Fullshift, 
                      Halfshift, Instart, Inend, Outstart, Outend, Workhour, Cropbefore, cropafter, Beforerand, Afterrand, 
                      Teabreakhouraftershift, Dinnerbreakdeductafter, Hour,Intime_Relaxation, InStartNew, OutStartNew, NextDayEnd 
                      From Hr_Shiftmaster(Nolock) Where shiftid=" + hnshiftid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DDcompany.SelectedValue = ds.Tables[0].Rows[0]["companyid"].ToString();
            txtshiftcode.Text = ds.Tables[0].Rows[0]["shiftcode"].ToString();
            if (ds.Tables[0].Rows[0]["Shifttype"].ToString() == "1")
            {
                rdday.Checked = true;
            }
            else
            {
                rdnight.Checked = true;
            }
            txtintime.Text = ds.Tables[0].Rows[0]["Intime"].ToString();
            txtouttime.Text = ds.Tables[0].Rows[0]["Outtime"].ToString();
            txtintimerelaxation.Text = ds.Tables[0].Rows[0]["Intime_relaxation"].ToString();
            switch (ds.Tables[0].Rows[0]["Lunchbreak"].ToString())
            {
                case "1":
                    Rdlunchbreakyes.Checked = true;
                    break;
                case "0":
                    Rdlunchbreakno.Checked = true;
                    break;
                default:
                    break;
            }
            txtbreakout.Text = ds.Tables[0].Rows[0]["Lunch_breakout"].ToString();
            txtbreakin.Text = ds.Tables[0].Rows[0]["Lunch_breakin"].ToString();
            txtbreakoutstart.Text = ds.Tables[0].Rows[0]["Lunch_breakoutstart"].ToString();
            txtbreakinend.Text = ds.Tables[0].Rows[0]["Lunch_breakinEnd"].ToString();
            switch (ds.Tables[0].Rows[0]["Lunch_breakdeduction"].ToString())
            {
                case "1":
                    Rdbreakdeductionyes.Checked = true;
                    break;
                case "0":
                    Rdbreakdeductionno.Checked = true;
                    break;
                default:
                    break;
            }
            switch (ds.Tables[0].Rows[0]["Lunch_Excessbreakdeduction"].ToString())
            {
                case "1":
                    Rdexcessbreakdeductionyes.Checked = true;
                    break;
                case "0":
                    Rdexcessbreakdeductionno.Checked = true;
                    break;
                default:
                    break;
            }
            txtteastartist.Text = ds.Tables[0].Rows[0]["Tea_startfirst"].ToString();
            txtteaendist.Text = ds.Tables[0].Rows[0]["Tea_endfirst"].ToString();
            txtteastartiind.Text = ds.Tables[0].Rows[0]["Tea_startsecond"].ToString();
            txtteaendiind.Text = ds.Tables[0].Rows[0]["Tea_endsecond"].ToString();
            switch (ds.Tables[0].Rows[0]["Otbefore"].ToString())
            {
                case "1":
                    rdotbeforeyes.Checked = true;
                    break;
                case "0":
                    rdotbeforeno.Checked = true;
                    break;
                default:
                    break;
            }
            txtotbeforehour.Text = ds.Tables[0].Rows[0]["Otbefore_time"].ToString();

            switch (ds.Tables[0].Rows[0]["Otafter"].ToString())
            {
                case "1":
                    rdotafteryes.Checked = true;
                    break;
                case "0":
                    rdotbeforeno.Checked = true;
                    break;
                default:
                    break;
            }
            txtotafterhour.Text = ds.Tables[0].Rows[0]["Otafter_time"].ToString();
            txthalfdayhrs.Text = ds.Tables[0].Rows[0]["halfdayhrs"].ToString();
            txtfulldayhrs.Text = ds.Tables[0].Rows[0]["fulldayhrs"].ToString();
            txtlatearrivalgrace.Text = ds.Tables[0].Rows[0]["Latearrivalgrace"].ToString();
            switch (ds.Tables[0].Rows[0]["Arrivalgracededuction"].ToString())
            {
                case "1":
                    rdarrivalgracedeductionyes.Checked = true;
                    break;
                case "0":
                    rdarrivalgracedeductionno.Checked = true;
                    break;
                default:
                    break;
            }
            txtearlydeparturegrace.Text = ds.Tables[0].Rows[0]["Earlydeparturegrace"].ToString();
            switch (ds.Tables[0].Rows[0]["Departuregracededuction"].ToString())
            {
                case "1":
                    rddeparturegracedeductionyes.Checked = true;
                    break;
                case "0":
                    rddeparturegracedeductionno.Checked = true;
                    break;
                default:
                    break;
            }
            switch (ds.Tables[0].Rows[0]["Fullshift"].ToString())
            {
                case "1":
                    rdfullshiftyes.Checked = true;
                    break;
                case "0":
                    rdfullshiftno.Checked = true;
                    break;
                default:
                    break;
            }
            txthalfshift.Text = ds.Tables[0].Rows[0]["halfshift"].ToString();
            txtinstart.Text = ds.Tables[0].Rows[0]["Instart"].ToString();
            txtinend.Text = ds.Tables[0].Rows[0]["Inend"].ToString();
            txtoutstart.Text = ds.Tables[0].Rows[0]["Outstart"].ToString();
            txtoutend.Text = ds.Tables[0].Rows[0]["outend"].ToString();
            txtworkhour.Text = ds.Tables[0].Rows[0]["Workhour"].ToString();
            txtcropbefore.Text = ds.Tables[0].Rows[0]["cropbefore"].ToString();
            txtbeforerand.Text = ds.Tables[0].Rows[0]["beforerand"].ToString();
            txtcropafter.Text = ds.Tables[0].Rows[0]["cropafter"].ToString();
            txtafterrand.Text = ds.Tables[0].Rows[0]["afterrand"].ToString();
            txtteabreakhouraftershift.Text = ds.Tables[0].Rows[0]["Teabreakhouraftershift"].ToString();
            txtdinnerbreakdeductafter.Text = ds.Tables[0].Rows[0]["dinnerbreakdeductafter"].ToString();
            txtdinnerbreakdeductafterhour.Text = ds.Tables[0].Rows[0]["Hour"].ToString();
            TxtShiftMinIntime.Text = ds.Tables[0].Rows[0]["InStartNew"].ToString();
            TxtShiftMaxIntime.Text = ds.Tables[0].Rows[0]["OutStartNew"].ToString();
            TxtNextDay.Text = ds.Tables[0].Rows[0]["NextDayEnd"].ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altd", "alert('Invalid Shift Code !!!')", true);
        }
    }
}