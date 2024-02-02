using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddPaymentDetail : CustomPage
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
     private void Fill_Grid()
    {
        gdPayment.DataSource = Fill_Grid_Data();
        gdPayment.DataBind();
    }
     private DataSet Fill_Grid_Data()
     {
         DataSet ds = null;
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         try
         {
             string strsql = "SELECT PaymentId as SrNo,	PaymentName,PaymentDescription from Payment where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
             con.Open();
             ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
         }
         catch (Exception ex)
         {
             Logs.WriteErrorLog("Masters_Campany_frmPayment|Fill_Grid_Data|" + ex.Message);
             UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddPaymentDetail.aspx");
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
    protected void gdPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdPayment.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Payment where PaymentId=" + id);
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
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddPaymentDetail.aspx");
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
                    _arrPara[0].Value = ViewState["id"];
                }
                {
                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrPara[1].Value = txtPayment.Text.ToUpper();
                _arrPara[2].Value = txtdescription.Text.ToUpper();
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_PAYMENT", _arrPara);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Masters_Campany_frmPayment|cmdSave_Click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddPaymentDetail.aspx");
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
            lbl.Visible = true;
            lbl.Text = "Save Details............";
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select PaymentName from Payment where PaymentId!='" + ViewState["id"].ToString() + "' and  PaymentName='" + txtPayment.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select PaymentName from Payment where PaymentName='" + txtPayment.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
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
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddPaymentDetail.aspx");
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
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Payment where PaymentId=" + ViewState["id"].ToString());
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'PaymentId'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
            lbl.Visible = true;
            lbl.Text = "Value Deleted.....";
            
          }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddPaymentDetail.aspx");
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
    protected void gdPayment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
                  //e.Row.RowState == DataControlRowState.Alternate)
            //e.Row.CssClass = "alternate";
    }
}