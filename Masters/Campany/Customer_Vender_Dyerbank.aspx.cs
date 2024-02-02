using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_Customer_vender_Dyerbank : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDbank, "select B.bankid,B.bankname +'/'+ SwiftCode as bankname  from bank B,BankCategory_Detail BC where B.BankId=Bc.BankId And BC.CategoryId=1  And  B.mastercompanyid=" + Session["varMasterCompanyIDForERP"] + "order by B.bankname", true, "--plz select bank");
            UtilityModule.ConditionalComboFill(ref DDcurrency, "select currencyid, currencyname from currencyinfo where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + "order by currencyname", true, "--plz select currency");
            fill_grid();
        }
    }
    protected void fill_grid()
    {
        if (Request.QueryString["b"] == "1")
        {
            string strsql = "select pb.Iban,DetailId,CustomerName,BankName +'/'+ SwiftCode as BankName,C.CurrencyName,pb.AcNo, case when actypeid=1 then 'Current' else case when actypeid=2 then 'saving' else '' end end AcType from partybank pb inner join Customerinfo ci on ci.Customerid=pb.Customer_vender_dyerid inner join bank b on b.bankid=pb.bankid left outer join currencyinfo C on C.currencyid=pb.currencyid Where TypeId=1 And Customer_vender_dyerid=" + Request.QueryString["a"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            Gvcvd.DataSource = null;
            Gvcvd.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Gvcvd.DataSource = ds.Tables[0];
                Gvcvd.DataBind();
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[11];
            _arrpara[0] = new SqlParameter("@Bankid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Actypeid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Currencyid", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@Acno", SqlDbType.VarChar, 50);
            _arrpara[4] = new SqlParameter("@customer_vender_dyerid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@userid", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Typeid", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _arrpara[9] = new SqlParameter("@Detailid", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Iban", SqlDbType.VarChar, 100);

            _arrpara[0].Value = DDbank.SelectedValue;
            _arrpara[1].Value = DDactype.SelectedValue;
            _arrpara[2].Value = DDcurrency.SelectedValue;
            _arrpara[3].Value = txtacno.Text.ToUpper();
            _arrpara[4].Value = Request.QueryString["a"].ToString();
            _arrpara[5].Value = Session["varuserid"].ToString();
            _arrpara[6].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrpara[7].Value = Request.QueryString["b"].ToString();
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[9].Direction = ParameterDirection.InputOutput;
            if (ViewState["DetailId"] == null)
            {
                ViewState["DetailId"] = 0;
            }
            _arrpara[9].Value = ViewState["DetailId"];
            _arrpara[10].Value = txtibanno.Text.ToUpper();
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Customer_Vender_Dyer_bank", _arrpara);
            lblErr.Visible = true;
            lblErr.Text = _arrpara[8].Value.ToString();
            Tran.Commit();
            Data_refresh();
            ViewState["DetailId"] = 0;
            BtnSave.Text = "Save";
            fill_grid();
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
            Tran.Rollback();
            Logs.WriteErrorLog("Master_Company_Customer_Vender_Dyerbank|BtnSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master_Company_Customer_Vender_Dyerbank.aspx");
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
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["id"] == "1")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "OnLoad", "onSuccess();", true);

                }
            }
        }
        BtnSave.Text = "Save";
        Btndelete.Visible = false;
        fill_grid();
    }
    protected void Data_refresh()
    {
        DDbank.SelectedIndex = 0;
        DDactype.SelectedIndex = -1;
        DDcurrency.SelectedIndex = 0;
        txtacno.Text = "";
        txtibanno.Text = "";

    }

    protected void Gvcvd_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void Gvcvd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gvcvd, "Select$" + e.Row.RowIndex);
        }
    }

    protected void Gvcvd_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            int DetailId = (int)Gvcvd.SelectedDataKey.Value;
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"select Iban,DetailId,Customer_vender_dyerid,b.Bankid,c.Currencyid,pb.AcNo,AcTypeid from partybank pb inner join Customerinfo ci on ci.Customerid=pb.Customer_vender_dyerid inner join bank b on b.bankid=pb.bankid left outer join currencyinfo C on C.currencyid=pb.currencyid where Customer_vender_dyerid=" + Request.QueryString["a"] + "  And DetailId=" + DetailId + "");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["DetailId"] = DetailId;
                DDbank.SelectedValue = ds.Tables[0].Rows[0]["Bankid"].ToString();
                DDactype.SelectedValue = ds.Tables[0].Rows[0]["Actypeid"].ToString();
                DDcurrency.SelectedValue = ds.Tables[0].Rows[0]["Currencyid"].ToString();
                txtacno.Text = ds.Tables[0].Rows[0]["AcNo"].ToString();
                txtibanno.Text = ds.Tables[0].Rows[0]["Iban"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Customer_Vender_Dyerbank.aspx");
            Logs.WriteErrorLog("Master_Company_Customer_Vender_Dyerbank|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        BtnSave.Text = "Update";
        Btndelete.Visible = true;

    }
    protected void Btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string str = "";
            str = "Delete from partybank where DetailId=" + ViewState["DetailId"];
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            Tran.Commit();
            lblErr.Text = "Data Deleted successfully........";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Customer_Vender_Dyerbank.aspx");
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
        Data_refresh();
        Btndelete.Visible = false;
        BtnSave.Text = "Save";
    }

}