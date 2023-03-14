using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmCarriage : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
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
    protected void gdCarriage_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdCarriage.SelectedDataKey.Value.ToString();
        // Session["id"] = id;
        ViewState["CarriageId"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CarriageId,CarriageName from Carriage where CarriageId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CarriageId"].ToString();
                txtCarriage.Text = ds.Tables[0].Rows[0]["CarriageName"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/frmCarriage.aspx");
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
        Validated();
        if (txtCarriage.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@CarriageId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@CarriageName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@companyid", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtCarriage.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Carriage", _arrPara);
                Tran.Commit();
                lbl.Visible = true;
                lbl.Text = "Data Saved Sucessfully";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/frmCarriage.aspx");
                lbl.Visible = true;
                lbl.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_frmCarriage|cmdSave_Click|" + ex.Message);
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
            
        }
        else
        {
            if (lbl.Text == "Carriage already exists............")
            {
                lbl.Visible = true;
                lbl.Text = "Carriage already exists............";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Please Fill Details............";
            }
        }
        txtid.Text = "0";
        txtCarriage.Text = "";
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
            string strsql = "select * from Carriage Where MasterCompanyId=" + Session["varCompanyId"] + " order by carriageid";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns["CarriageId"].ColumnName = "CarriageId";
            ds.Tables[0].Columns["CarriageName"].ColumnName = "CarriageName";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/frmCarriage.aspx");
            Logs.WriteErrorLog("Masters_Campany_frmCarriage|Fill_Grid_Data|" + ex.Message);
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select CarriageName from Carriage where CarriageId!='" + ViewState["CarriageId"].ToString() + "' and CarriageName='" + txtCarriage.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select CarriageName from Carriage where CarriageName='" + txtCarriage.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl.Visible = true;
                lbl.Text = "Carriage already exists............";
                txtCarriage.Text = "";
                txtCarriage.Focus();
            }
            else
            {
                lbl.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/frmCarriage.aspx");
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
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@CarriageId", ViewState["CarriageId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteCarriage", _array);
            lbl.Visible = true;
            lbl.Text = _array[1].Value.ToString();
            Tran.Commit();

        }
        catch (Exception ex)
        {
            lbl.Visible = true;
            lbl.Text = ex.Message;
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/frmCarriage.aspx");
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
        txtCarriage.Text = "";
        txtid.Text = "0";
    }

}
