using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmPaymentDetail : CustomPage
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
    private void Fill_Grid()
    {
        gdPayment.DataSource = Fill_Grid_Data();
        gdPayment.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = "SELECT PaymentId as SrNo,	PaymentName,PaymentDescription from Payment Where MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmPayment|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPaymentDetail.aspx");
        }
        return ds;
    }
    protected void gdPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdPayment.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["PaymentId"] = id;
        DataSet ds = SqlHelper.ExecuteDataset("select * from Payment where PaymentId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["PaymentId"].ToString();
                txtPayment.Text = ds.Tables[0].Rows[0]["PaymentName"].ToString();
                txtdescription.Text = ds.Tables[0].Rows[0]["PaymentDescription"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPaymentDetail.aspx");
            //Logs.WriteErrorLog("Masters_Campany_frmpenality|Fill_Grid_Data|" + ex.Message);
        }
        btndelete.Visible = true;
        btnsave.Text = "Update";
    }
    protected void gdPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdPayment.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdPayment, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtPayment.Text != "")
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
                _arrPara[0] = new SqlParameter("@PaymentId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@PaymentName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@PaymentDescription", SqlDbType.NVarChar, 200);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                if (btnsave.Text == "Update")
                {
                    _arrPara[0].Value = ViewState["PaymentId"];
                }
                {
                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrPara[1].Value = txtPayment.Text.ToUpper();
                _arrPara[2].Value = txtdescription.Text.ToUpper();
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varCompanyId"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PAYMENT", _arrPara);
                Tran.Commit();
                lbl.Visible = true;
                lbl.Text = "Save Details............";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lbl.Visible = true;
                lbl.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_frmPayment|cmdSave_Click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPaymentDetail.aspx");
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
            if (lbl.Text == "Payment Name already exists............")
            {
                lbl.Visible = true;
                lbl.Text = "Payment Name already exists............";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Please Fill Details.....";
            }
        }
        txtid.Text = "0";
        txtPayment.Text = "";
        txtdescription.Text = "";
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select PaymentName from Payment where PaymentId!='" + ViewState["PaymentId"].ToString() + "' and  PaymentName='" + txtPayment.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select PaymentName from Payment where PaymentName='" + txtPayment.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl.Visible = true;
                lbl.Text = "Payment Name already exists............";
                txtPayment.Text = "";
                txtPayment.Focus();
            }
            else
            {
                lbl.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPaymentDetail.aspx");
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
            _array[0] = new SqlParameter("@PaymentId", ViewState["PaymentId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletePayment", _array);
            Tran.Commit();
            lbl.Visible = true;
            lbl.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lbl.Visible = true;
            lbl.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmPaymentDetail.aspx");
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
        txtPayment.Text = "";
        txtdescription.Text = "";
        txtid.Text = "0";
    }
   
}