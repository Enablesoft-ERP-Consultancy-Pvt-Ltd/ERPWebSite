using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_FrmMachineNoMaster : CustomPage
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
    protected void gdMachineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdMachineNo.SelectedDataKey.Value.ToString();
        // Session["id"] = id;
        ViewState["MachineNoId"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select MachineNoId,MachineNoName from MachineNoMaster where MachineNoId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["MachineNoId"].ToString();
                txtMachineNo.Text = ds.Tables[0].Rows[0]["MachineNoName"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/FrmMachineNoMaster.aspx");
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
    protected void gdMachineNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdMachineNo.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdMachineNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdMachineNo, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtMachineNo.Text != "")
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
                _arrPara[0] = new SqlParameter("@MachineNoId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@MachineNoName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@UserID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtMachineNo.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_MachineNoMaster", _arrPara);
                Tran.Commit();
                lbl.Visible = true;
                lbl.Text = "Data Saved Sucessfully";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/FrmMachineNoMaster.aspx");
                lbl.Visible = true;
                lbl.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_FrmMachineNoMaster|cmdSave_Click|" + ex.Message);
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
            if (lbl.Text == "MachineNo already exists............")
            {
                lbl.Visible = true;
                lbl.Text = "MachineNo already exists............";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Please Fill Details............";
            }
        }
        txtid.Text = "0";
        txtMachineNo.Text = "";
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    private void Fill_Grid()
    {
        gdMachineNo.DataSource = Fill_Grid_Data();
        gdMachineNo.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select * from MachineNoMaster Where MasterCompanyId=" + Session["varCompanyId"] + " order by MachineNoid";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns["MachineNoId"].ColumnName = "MachineNoId";
            ds.Tables[0].Columns["MachineNoName"].ColumnName = "MachineNoName";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/FrmMachineNoMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_FrmMachineNoMaster|Fill_Grid_Data|" + ex.Message);
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
                strsql = "select MachineNoName from MachineNoMaster where MachineNoId!='" + ViewState["MachineNoId"].ToString() + "' and MachineNoName='" + txtMachineNo.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select MachineNoName from MachineNoMaster where MachineNoName='" + txtMachineNo.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl.Visible = true;
                lbl.Text = "MachineNo already exists............";
                txtMachineNo.Text = "";
                txtMachineNo.Focus();
            }
            else
            {
                lbl.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/FrmMachineNoMaster.aspx");
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
            _array[0] = new SqlParameter("@MachineNoId", ViewState["MachineNoId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteMachineNoMaster", _array);
            lbl.Visible = true;
            lbl.Text = _array[1].Value.ToString();
            Tran.Commit();

        }
        catch (Exception ex)
        {
            lbl.Visible = true;
            lbl.Text = ex.Message;
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/FrmMachineNoMaster.aspx");
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
        txtMachineNo.Text = "";
        txtid.Text = "0";
    }

}
