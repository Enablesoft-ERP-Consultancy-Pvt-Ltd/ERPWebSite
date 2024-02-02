using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Campany_AddWareHouseName : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            GetCustomerWareHouseCode();
            fill_grid();
        }

    }
    protected void GetCustomerWareHouseCode()
    {
        string strsql = "select WareHouseId,WareHouseCode,CI.CustomerCode,WHM.CustomerId from WareHouseMaster WHM JOIN CustomerInfo CI ON WHM.CustomerId=CI.CustomerId  Where WHM.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                        And  WareHouseId=" + Request.QueryString["a"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblCustomerCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
            lblWareHouseCode.Text = ds.Tables[0].Rows[0]["WareHouseCode"].ToString();
            lblCustomerId.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString();
        }
        else
        {
            lblCustomerCode.Text = "";
            lblWareHouseCode.Text = "";
        }
    }
    protected void fill_grid()
    {

        //string strsql = "select couriername,AcNo,detailid from couriermaster Where MastercompanyId=" + Session["varMasterCompanyIDForERP"] + " And  CustomerId=" + Request.QueryString["a"];
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);


        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DGWareHouseName.DataSource = ds.Tables[0];
        //    DGWareHouseName.DataBind();
        //}
        //else
        //{
        //    DGWareHouseName.DataSource = null;
        //    DGWareHouseName.DataBind();
        //}


        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@UserID", Session["VarUserId"]);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@WareHouseId", Request.QueryString["a"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILLWAREHOUSENAME", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGWareHouseName.DataSource = ds.Tables[0];
            DGWareHouseName.DataBind();
        }
        else
        {
            DGWareHouseName.DataSource = null;
            DGWareHouseName.DataBind();
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
            _arrPara[1] = new SqlParameter("@WareHouseId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@WareHouseNameByCode", SqlDbType.VarChar, 50);
            _arrPara[3] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@userid", SqlDbType.Int);           
            _arrPara[5] = new SqlParameter("@WareHouseNameId", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);


            _arrPara[0].Value = lblCustomerId.Text;
            _arrPara[1].Value = Request.QueryString["a"].ToString();
            _arrPara[2].Value = txtWareHouseName.Text.ToUpper();
            _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[4].Value =  Session["varuserid"].ToString();
            _arrPara[5].Direction = ParameterDirection.InputOutput;

            if (btnsave.Text == "Update")
            {
                _arrPara[5].Value = ViewState["WareHouseNameId"];
            }
            else
            {
                _arrPara[5].Value = 0;
            }
            _arrPara[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveWareHouseNameByCode", _arrPara);
            lblerr.Visible = true;
            lblerr.Text = _arrPara[6].Value.ToString();
            Tran.Commit();
            btnsave.Text = "Save";
            ViewState["WareHouseNameId"] = 0;
            txtWareHouseName.Text = "";

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
    protected void DGWareHouseName_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGWareHouseName_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGWareHouseName, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGWareHouseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["WareHouseNameId"] = (int)DGWareHouseName.SelectedDataKey.Value;
        int rowindex = DGWareHouseName.SelectedIndex;
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
            Label lblWareHouseNameByCode = ((Label)DGWareHouseName.Rows[rowindex].FindControl("lblWareHouseNameByCode"));

            txtWareHouseName.Text = lblWareHouseNameByCode.Text;
          

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddWareHouseName.aspx");
            Logs.WriteErrorLog("Masters_Campany_AddWareHouseName|Fill_Grid_Data|" + ex.Message);
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
        lblerr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            ////LinkButton lnkdel = sender as LinkButton;
            ////GridViewRow grv = lnkdel.NamingContainer as GridViewRow;
            //int BMID = Convert.ToInt32(GVBunkarMaster.DataKeys[e.RowIndex].Value);

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@WareHouseNameId", ViewState["WareHouseNameId"]);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEWAREHOUSENAMEBYCODE", param);
            lblerr.Text = param[3].Value.ToString();
            Tran.Commit();

            fill_grid();
            txtWareHouseName.Text = "";
            btndelete.Visible = false;
            btnsave.Text = "Save";


        }
        catch (Exception ex)
        {
            lblerr.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    string str = "";
        //    str = "delete from couriermaster where detailid=" + ViewState["detailid"];
        //    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        //    Tran.Commit();
        //    lblerr.Text = "Data Deleted Successfully...........";

        //}
        //catch (Exception ex)
        //{
        //    Tran.Rollback();
        //    UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddWareHouseName.aspx");
        //}
        //finally
        //{
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        //fill_grid();
        //txtacno.Text = "";
        //txtcourier.Text = "";
        //btndelete.Visible = false;
        //btnsave.Text = "Save";



        
    }
}


