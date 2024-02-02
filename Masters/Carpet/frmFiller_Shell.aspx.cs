using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_frmFiller_Shell : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDcurrency, "select CurrencyId,CurrencyName from currencyinfo order by currencyName", true, "--Plz Select--");
            GetDetail(Convert.ToInt16(Request.QueryString["id"]));
        }
    }
    protected void GetDetail(int costingid)
    {
        string str;
        DataSet ds;
        str = "select Margin,Currency,Overheads,Finance,Commission,TotalAmount,Quote,isnull(currencyId,0) as Currencyid from costing_FillerShell where costingid=" + Request.QueryString["id"];
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txttotalamount.Text = ds.Tables[0].Rows[0]["Totalamount"].ToString();
            txtmarginshell.Text = ds.Tables[0].Rows[0]["Margin"].ToString();
            txtCurrencyShell.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
            txtoverheadsshell.Text = ds.Tables[0].Rows[0]["Overheads"].ToString();
            txtfinanceshell.Text = ds.Tables[0].Rows[0]["Finance"].ToString();
            txtcommissionShell.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
            txtQuote.Text = ds.Tables[0].Rows[0]["quote"].ToString();
            if(DDcurrency.Items.FindByValue(ds.Tables[0].Rows[0]["Currencyid"].ToString())!=null)
            {
                DDcurrency.SelectedValue = ds.Tables[0].Rows[0]["Currencyid"].ToString();
            }
        }
        //********Current costing amount
        str = "select isnull(sum(amount),0) as Amount from v_costingdetail where CostingMID=" + costingid;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {            
            lblCurrentCosting.Text = "Current Total Costing amount : " + ds.Tables[0].Rows[0]["Amount"];
            if (txttotalamount.Text == "")
            {
                txttotalamount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
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
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@costingid", SqlDbType.Int);
            param[1] = new SqlParameter("@Margin", SqlDbType.Float);
            param[2] = new SqlParameter("@currency", SqlDbType.Float);
            param[3] = new SqlParameter("@Overheads", SqlDbType.Float);
            param[4] = new SqlParameter("@Finance", SqlDbType.Float);
            param[5] = new SqlParameter("@Commission", SqlDbType.Float);
            param[6] = new SqlParameter("@quote", SqlDbType.Float);
            param[7] = new SqlParameter("@totalamt", SqlDbType.Float);
            param[8] = new SqlParameter("@userid", SqlDbType.Int);
            param[9] = new SqlParameter("CurrencyId",SqlDbType.Int);
            //***********************
            param[0].Value = Request.QueryString["id"];
            param[1].Value = txtmarginshell.Text == "" ? "0" : txtmarginshell.Text;
            param[2].Value = txtCurrencyShell.Text == "" ? "0" : txtCurrencyShell.Text;
            param[3].Value = txtoverheadsshell.Text == "" ? "0" : txtoverheadsshell.Text;
            param[4].Value = txtfinanceshell.Text == "" ? "0" : txtfinanceshell.Text;
            param[5].Value = txtcommissionShell.Text == "" ? "0" : txtcommissionShell.Text;
            param[6].Value = txtQuote.Text == "" ? "0" : txtQuote.Text;
            param[7].Value = txttotalamount.Text == "" ? "0" : txttotalamount.Text;
            param[8].Value = Session["varuserid"];
            param[9].Value = DDcurrency.SelectedValue;
            //***********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_SaveCostingFillershell", param);
            Tran.Commit();
            lblmessage.Text = "Data saved successfully...";
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}