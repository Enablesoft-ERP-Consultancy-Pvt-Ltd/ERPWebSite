using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

public partial class Masters_Campany_AddBuyingHouse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }
        if (!IsPostBack)
        {
            fill_grid();

        }
    }
    protected void form_refresh()
    {
        txtnameofbuyinghouse.Text = "";
        txttypeofbuyinghouse.Text = "";
        txtaddress.Text = "";
        txtcontactno.Text = "";
        txtFaxNo.Text = "";
        txtEmail.Text = "";
        txtcontactperson.Text = "";
        txtemailofcontactperson.Text = "";
        txtmobilenumber.Text = "";

    }
    protected void fill_grid()
    {
        string strsql = @"select buyinghouseid,Name_buying_house,Type_buying_house,Address,Contactno,Faxno,Email,Contactperson,Emailofcontactperson,MobileNumber from buyinghouse Where
                         MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Gdbuyinghouse.DataSource = ds.Tables[0];
            Gdbuyinghouse.DataBind();
        }
        else
        {
            Gdbuyinghouse.DataSource = null;
            Gdbuyinghouse.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[13];
            _arrpara[0] = new SqlParameter("@Name_buying_house", SqlDbType.VarChar, 50);
            _arrpara[1] = new SqlParameter("@Type_buying_house", SqlDbType.VarChar, 50);
            _arrpara[2] = new SqlParameter("@Address", SqlDbType.VarChar, 50);
            _arrpara[3] = new SqlParameter("@Contactno", SqlDbType.VarChar, 50);
            _arrpara[4] = new SqlParameter("@Faxno", SqlDbType.VarChar, 50);
            _arrpara[5] = new SqlParameter("@Email", SqlDbType.VarChar, 50);
            _arrpara[6] = new SqlParameter("@Contactperson", SqlDbType.VarChar, 50);
            _arrpara[7] = new SqlParameter("@Emailofcontactperson", SqlDbType.VarChar, 50);
            _arrpara[8] = new SqlParameter("@MobileNumber", SqlDbType.VarChar, 50);
            _arrpara[9] = new SqlParameter("@userid", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrpara[11] = new SqlParameter("@buyinghouseId", SqlDbType.Int);
            _arrpara[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);

            _arrpara[0].Value = txtnameofbuyinghouse.Text.ToUpper();
            _arrpara[1].Value = txttypeofbuyinghouse.Text.ToUpper();
            _arrpara[2].Value = txtaddress.Text.ToUpper();
            _arrpara[3].Value = txtcontactno.Text.ToUpper();
            _arrpara[4].Value = txtFaxNo.Text.ToUpper();
            _arrpara[5].Value = txtEmail.Text.ToUpper();
            _arrpara[6].Value = txtcontactperson.Text.ToUpper();
            _arrpara[7].Value = txtemailofcontactperson.Text.ToUpper();
            _arrpara[8].Value = txtmobilenumber.Text.ToUpper();
            _arrpara[9].Value = Session["varuserid"].ToString();
            _arrpara[10].Value = Session["varcompanyid"].ToString();
            _arrpara[11].Direction = ParameterDirection.InputOutput;
            if (ViewState["buyinghouseId"] == null)
            {
                ViewState["buyinghouseId"] = 0;
            }
            _arrpara[11].Value = ViewState["buyinghouseId"];
            _arrpara[12].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_Buyinghouse", _arrpara);
            leblrr.Visible = true;
            leblrr.Text = _arrpara[12].Value.ToString();
            Tran.Commit();
            ViewState["buyinghouseId"] = 0;
            fill_grid();
        }
        catch (Exception ex)
        {
            leblrr.Visible = true;
            leblrr.Text = ex.Message;
            Tran.Rollback();
            Logs.WriteErrorLog("Master_Company_AddBuyinghouse|btnSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Masters_Campany_AddBuyingHouse.aspx");

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
        form_refresh();


    }
    protected void Gdbuyinghouse_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void Gdbuyinghouse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.Gdbuyinghouse, "Select$" + e.Row.RowIndex);
        }
    }
    protected void Gdbuyinghouse_SelectedIndexChanged(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"select buyinghouseid, Name_buying_house,Type_buying_house,Address,Contactno,Faxno,Email,Contactperson,Emailofcontactperson,MobileNumber from buyinghouse Where
                         MasterCompanyId=" + Session["varCompanyId"] + " And buyinghouseid=" + Gdbuyinghouse.SelectedDataKey.Value.ToString() + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["buyinghouseId"] = Gdbuyinghouse.SelectedDataKey.Value.ToString();

                txtnameofbuyinghouse.Text = ds.Tables[0].Rows[0]["Name_buying_house"].ToString();
                txttypeofbuyinghouse.Text = ds.Tables[0].Rows[0]["Type_buying_house"].ToString();
                txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtcontactno.Text = ds.Tables[0].Rows[0]["Contactno"].ToString();
                txtFaxNo.Text = ds.Tables[0].Rows[0]["Faxno"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtcontactperson.Text = ds.Tables[0].Rows[0]["Contactperson"].ToString();
                txtemailofcontactperson.Text = ds.Tables[0].Rows[0]["Emailofcontactperson"].ToString();
                txtmobilenumber.Text = ds.Tables[0].Rows[0]["MobileNumber"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters_Campany_AddBuyingHouse.aspx");
            Logs.WriteErrorLog("Master_Company_AddBuyinghouse|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnSave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select buyinghouseid from customerinfo where buyinghouseid=" + ViewState["buyinghouseId"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from buyinghouse where buyinghouseid=" + ViewState["buyinghouseId"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'buyinghouse'," + ViewState["buyinghouseId"].ToString() + ",getdate(),'Delete')");
                btnSave.Text = "Save";
                leblrr.Visible = true;
                leblrr.Text = "Value Deleted..............";
            }
            else
            {
                leblrr.Visible = true;
                leblrr.Text = "Value in Use...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters_Campany_AddBuyingHouse.aspx");
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
        form_refresh();

    }
   
}