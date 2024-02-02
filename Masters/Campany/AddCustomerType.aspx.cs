using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddCustomerType : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            Fill_Grid();
        }
        lbl.Visible = false;
    }
  
    protected void gdCarriage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdCarriage.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdCarriage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdCarriage, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //  Validated();
        if (txtCustomerType.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@CustomerTypeId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@TypeName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);

                if (btnsave.Text == "Update")
                {
                    _arrPara[0].Value = ViewState["id"];
                }
                else
                {
                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrPara[1].Value = txtCustomerType.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[4].Direction = ParameterDirection.Output;
                
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CustomerType", _arrPara);
                Tran.Commit();
                lbl.Visible = true;
                lbl.Text = _arrPara[4].Value.ToString();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lbl.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_AddCusomerType|cmdSave_Click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCarriage.aspx");
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            Fill_Grid();
        }

        txtid.Text = "0";
        txtCustomerType.Text = "";
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    private void Fill_Grid()
    {
        gdCarriage.DataSource = Fill_Grid_Data();
        gdCarriage.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select * from CustomerType Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerTypeId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            gdCarriage.DataSource = ds;
            gdCarriage.DataBind();
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_AddCustomerType|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCarriage.aspx");
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
    private void Validated()
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //try
        //{
        //    string strsql;
        //    if (btnsave.Text == "Update")
        //    {
        //        strsql = "select CarriageName from Carriage where CarriageId!='" + ViewState["id"].ToString() + "' and CarriageName='" + txtCarriage.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        //    }
        //    else
        //    {
        //        strsql = "select CarriageName from Carriage where CarriageName='" + txtCarriage.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        //    }
        //    con.Open();
        //    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        lbl.Visible = true;
        //        lbl.Text = "Carriage already exists............";
        //        txtCustomerType.Text = "";
        //        txtCustomerType.Focus();
        //    }
        //    else
        //    {
        //        lbl.Text = "";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCarriage.aspx");
        //}
        //finally
        //{
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PreCarriageBy from customerinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  PreCarriageBy=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Carriage where CarriageId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'PreCarriageBy'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                lbl.Visible = true;
                lbl.Text = "Value Deleted...........";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Value in Use...";

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCarriage.aspx");
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
        txtCustomerType.Text = "";
        txtid.Text = "0";
    }
    protected void gdCarriage_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
                //  e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
    protected void gdCarriage_SelectedIndexChanged(object sender, EventArgs e)
    {

        string id = gdCarriage.SelectedDataKey.Value.ToString();
        // Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CustomerTypeId,TypeName from Customertype where CustomerTypeid=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CustomerTypeId"].ToString();
                txtCustomerType.Text = ds.Tables[0].Rows[0]["TypeName"].ToString();
            }
        }
        catch (Exception ex)
        {
            //Logs.WriteErrorLog("Masters_Campany_frmpenality|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCarriage.aspx");
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
}