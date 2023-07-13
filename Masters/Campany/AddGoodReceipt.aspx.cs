using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddGoodReceipt : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack==false)
        {
            txtid.Text = "0";
            Fill_Grid();
        }
        Label1.Visible = false;
    }
    private void Fill_Grid()
    {
        gdGoods.DataSource = Fill_Grid_Data();
        gdGoods.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select GoodsreceiptId as Sr_No,StationName from GoodsReceipt Where MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmGoods|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddGoodReceipt.aspx");
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtStationname.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@GoodsreceiptId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@StationName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                if (btnsave.Text == "Update")
                {
                    _arrPara[0].Value = ViewState["id"];
                }
                else
                {
                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrPara[1].Value = txtStationname.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_GOODS", _arrPara);
                txtid.Text = "0";
                txtStationname.Text = "";
            }
            catch(Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddGoodReceipt.aspx");
                //Logs.WriteErrorLog("Masters_Campany_frmGoods|cmdSave_Click|" + ex.Message);
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
            }
            Fill_Grid();
            Label1.Visible = true;
            Label1.Text = "Save Details............";
        }
        else
        {
            if (Label1.Text == "StationName already exists............")
            {
                Label1.Visible = true;
                Label1.Text = "StationName already exists............";
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Please Fill Details.................";
            }
        }
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtStationname.Text = "";
    }
    protected void gdGoods_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdGoods.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        txtid.Text = ViewState["id"].ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from GoodsReceipt where GoodsReceiptId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
               // txtid.Text = ds.Tables[0].Rows[0]["GoodsReceiptId"].ToString();
                txtStationname.Text = ds.Tables[0].Rows[0]["Stationname"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddGoodReceipt.aspx");
            //Logs.WriteErrorLog("Masters_Campany_frmpenality|Fill_Grid_Data|" + ex.Message);
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
    protected void gdGoods_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdGoods.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdGoods_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdGoods, "select$" + e.Row.RowIndex);
        }
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select StationName from GoodsReceipt where GoodsreceiptId!='" + ViewState["id"].ToString() + "' and StationName='" + txtStationname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select StationName from GoodsReceipt where StationName='" + txtStationname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label1.Visible = true;
                Label1.Text = "StationName already exists............";
                txtStationname.Text = "";
                txtStationname.Focus();
            }
            else
            {
                Label1.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddGoodReceipt.aspx");
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
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select RecieptAtByPreCarrier ,PortOfLoading from customerinfo where MasterCompanyId=" + Session["varCompanyId"] + "  And PortOfLoading='" + ViewState["id"].ToString() + "' or PortOfLoading='" + ViewState["id"].ToString() + "'"));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from GoodsReceipt where GoodsreceiptId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'GoodsReceipt'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                Label1.Visible = true;
                Label1.Text = "Value Deleted..........";

            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Value in Use...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddGoodReceipt.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        Fill_Grid();
        btndelete.Visible = false;
        btnsave.Text = "Save";
        txtStationname.Text = "";
        txtid.Text = "0";
    }
    protected void gdGoods_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
         //   e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}