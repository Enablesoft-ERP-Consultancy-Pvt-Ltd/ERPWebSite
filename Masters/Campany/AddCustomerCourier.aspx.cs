using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Campany_AddCustomerCourier : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            fill_grid();
        }

    }
    protected void fill_grid()
    {

        string strsql = "select couriername,AcNo,detailid from couriermaster Where MastercompanyId=" + Session["varMasterCompanyIDForERP"] + " And  CustomerId=" + Request.QueryString["a"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);


        if (ds.Tables[0].Rows.Count > 0)
        {
            DGcourier.DataSource = ds.Tables[0];
            DGcourier.DataBind();
        }
        else
        {
            DGcourier.DataSource = null;
            DGcourier.DataBind();
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
            SqlParameter[] _arrPara = new SqlParameter[7];
            _arrPara[0] = new SqlParameter("@customerid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@couriername", SqlDbType.VarChar, 50);
            _arrPara[2] = new SqlParameter("@AcNo", SqlDbType.VarChar, 50);
            _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@detailid", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);



            _arrPara[0].Value = Request.QueryString["a"].ToString();
            _arrPara[1].Value = txtcourier.Text.ToUpper();
            _arrPara[2].Value = txtacno.Text;
            _arrPara[3].Value = Session["varuserid"].ToString();
            _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[5].Direction = ParameterDirection.InputOutput;

            if (btnsave.Text == "Update")
            {
                _arrPara[5].Value = ViewState["detailid"];
            }
            else
            {
                _arrPara[5].Value = 0;
            }
            _arrPara[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_couriermaster", _arrPara);
            lblerr.Visible = true;
            lblerr.Text = _arrPara[6].Value.ToString();
            Tran.Commit();
            btnsave.Text = "Save";
            ViewState["detailid"] = 0;
            txtacno.Text = "";
            txtcourier.Text = "";

        }
        catch (Exception ex)
        {
            lblerr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

        btndelete.Visible = false;
        fill_grid();
    }
    protected void DGcourier_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGcourier_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGcourier, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGcourier_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["detailid"] = (int)DGcourier.SelectedDataKey.Value;
        int rowindex = DGcourier.SelectedIndex;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select couriername,AcNo,detailid from couriermaster Where detaiid=" + ViewState["detailid"]);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    txtcourier.Text = ds.Tables[0].Rows[0]["couriername"].ToString();
            //    txtacno.Text = ds.Tables[0].Rows[0]["AcNo"].ToString();




            //};
            Label lblcourierName = ((Label)DGcourier.Rows[rowindex].FindControl("lblCourierName"));
            Label lblAcNo = ((Label)DGcourier.Rows[rowindex].FindControl("lblAcNo"));
            txtcourier.Text = lblcourierName.Text; //CourierName
            txtacno.Text = lblAcNo.Text;//ac No

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomerCourier.aspx");
            Logs.WriteErrorLog("Masters_Campany_AddCustomerCourier|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
        btndelete.Visible = true;

    }
    protected void btndelete_Click(object sender, EventArgs e)
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
            str = "delete from couriermaster where detailid=" + ViewState["detailid"];
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            Tran.Commit();
            lblerr.Text = "Data Deleted Successfully...........";

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomerCourier.aspx");
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
        txtacno.Text = "";
        txtcourier.Text = "";
        btndelete.Visible = false;
        btnsave.Text = "Save";
    }
}


