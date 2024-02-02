using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_frmcalculatecbm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["b"] != null)
            {
                lblsize.Text = "Size : " + Request.QueryString["b"] + "   " + Request.QueryString["c"];
                Fillgrid();
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[14];
            param[0] = new SqlParameter("@Sizeid", SqlDbType.Int);
            param[1] = new SqlParameter("@BoxSize", SqlDbType.VarChar, 50);
            param[2] = new SqlParameter("@Length", SqlDbType.VarChar, 50);
            param[3] = new SqlParameter("@Width", SqlDbType.VarChar, 50);
            param[4] = new SqlParameter("@Height", SqlDbType.VarChar, 50);
            param[5] = new SqlParameter("@Pc_Box", SqlDbType.Int);
            param[6] = new SqlParameter("@Boxply", SqlDbType.Float);
            param[7] = new SqlParameter("@CBM_Pc", SqlDbType.Float);
            param[8] = new SqlParameter("@Rate_Cbm", SqlDbType.Float);
            param[9] = new SqlParameter("@Rate_pc", SqlDbType.Float);
            param[10] = new SqlParameter("@Boxprice", SqlDbType.Float);
            param[11] = new SqlParameter("@Box_Pc", SqlDbType.Float);
            param[12] = new SqlParameter("@sizeflag", SqlDbType.TinyInt);
            param[13] = new SqlParameter("@userid", SqlDbType.Int);
            //**************************
            param[0].Value = Request.QueryString["a"];
            param[1].Value = txtBoxsize.Text;
            param[2].Value = txtLength.Text;
            param[3].Value = txtWidth.Text;
            param[4].Value = txtHeight.Text;
            param[5].Value = txtpcperbox.Text == "" ? "0" : txtpcperbox.Text;
            param[6].Value = txtboxply.Text == "" ? "0" : txtboxply.Text;
            param[7].Value = txtcbm_pc.Text == "" ? "0" : txtcbm_pc.Text;
            param[8].Value = txtrate_cbm.Text == "" ? "0" : txtrate_cbm.Text;
            param[9].Value = txtrate_pc.Text == "" ? "0" : txtrate_pc.Text;
            param[10].Value = txtboxprice.Text == "" ? "0" : txtboxprice.Text;
            param[11].Value = txtbox_pc.Text == "" ? "0" : txtbox_pc.Text;
            param[12].Value = Request.QueryString["d"];
            param[13].Value = Session["varuserid"];
            //*************************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_saveBoxcosting", param);
            lblmessgae.Text = "Data saved successfully...";
            Tran.Commit();
            Refreshcontrol();
        }
        catch (Exception ex)
        {

            Tran.Rollback();
            lblmessgae.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        txtBoxsize.Text = "";
        txtLength.Text = "";
        txtWidth.Text = "";
        txtHeight.Text = "";
        txtpcperbox.Text = "";
        txtboxply.Text = "";
        txtcbm_pc.Text = "";
        txtrate_cbm.Text = "";
        txtrate_pc.Text = "";
        txtboxprice.Text = "";
        txtbox_pc.Text = "";


    }
    protected void Fillgrid()
    {
        string str = "select BoxSize, Length, Width, Height, PC_Box, Boxply, CBM_Pc, Rate_Cbm, Rate_Pc, BoxPrice,Box_Pc, Sizeflag, userid, Dateadded from Boxcosting where 1=1";
        if (Request.QueryString["a"] != null)
        {
            str = str + " and  sizeid=" + Request.QueryString["a"];
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
          txtBoxsize.Text=ds.Tables[0].Rows[0]["boxsize"].ToString();
          txtLength.Text = ds.Tables[0].Rows[0]["length"].ToString();
          txtWidth.Text = ds.Tables[0].Rows[0]["width"].ToString();
          txtHeight.Text = ds.Tables[0].Rows[0]["Height"].ToString();
          txtpcperbox.Text = ds.Tables[0].Rows[0]["pc_box"].ToString();
          txtboxply.Text = ds.Tables[0].Rows[0]["boxply"].ToString();
          txtcbm_pc.Text = ds.Tables[0].Rows[0]["Cbm_pc"].ToString();
          txtrate_cbm.Text = ds.Tables[0].Rows[0]["Rate_cbm"].ToString();
          txtrate_pc.Text = ds.Tables[0].Rows[0]["rate_pc"].ToString();
          txtboxprice.Text = ds.Tables[0].Rows[0]["Boxprice"].ToString();
          txtbox_pc.Text = ds.Tables[0].Rows[0]["box_pc"].ToString(); 
        }
    }
}