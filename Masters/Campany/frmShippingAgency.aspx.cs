using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmShippingAgency : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            //  Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            FillGrid();
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
            SqlParameter[] array = new SqlParameter[9];
            array[0] = new SqlParameter("@AgencyId", SqlDbType.Int);
            array[1] = new SqlParameter("@AgencyName", SqlDbType.VarChar, 50);
            array[2] = new SqlParameter("@Address", SqlDbType.VarChar, 250);
            array[3] = new SqlParameter("@PhoneNo", SqlDbType.VarChar, 50);
            array[4] = new SqlParameter("@FaxNo", SqlDbType.VarChar, 50);
            array[5] = new SqlParameter("@Email", SqlDbType.VarChar, 50);
            array[6] = new SqlParameter("@UserId", SqlDbType.Int);
            array[7] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[8] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);


            array[0].Direction = ParameterDirection.InputOutput;
            if (ViewState["AgencyId"] == null)
            {
                ViewState["AgencyId"] = 0;
            }
            array[0].Value = ViewState["AgencyId"];
            array[1].Value = txtAgencyName.Text;
            array[2].Value = txtAddress.Text;
            array[3].Value = txtPhoneNo.Text;
            array[4].Value = txtFaxno.Text;
            array[5].Value = txtEmail.Text;
            array[6].Value = Session["varUserId"];
            array[7].Value = Session["VarcompanyId"];
            array[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ShippingAgency", array);
            Tran.Commit();
            lblMsg.Text = array[8].Value.ToString();
            FillGrid();
            ClearAfterSave();



        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void FillGrid()
    {
        string str = "select AgencyId,AgencyName,Address,PhoneNo,FaxNo,Email From ShippingAgency";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gdShippingAgency.DataSource = ds;
        gdShippingAgency.DataBind();

    }
    protected void gdShippingAgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string str = "select * from shippingAgency Where AgencyId=" + gdShippingAgency.SelectedDataKey.Value.ToString() + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAgencyName.Text = ds.Tables[0].Rows[0]["AgencyName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtPhoneNo.Text = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                txtFaxno.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                ViewState["AgencyId"] = ds.Tables[0].Rows[0]["Agencyid"].ToString();
                btnsave.Text = "Update";
            }
            else
            {
                lblMsg.Text = "Plz Check Data...";
                btnsave.Text = "Save";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            btnsave.Text = "Save";
        }

    }
    protected void gdShippingAgency_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdShippingAgency.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void gdShippingAgency_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdShippingAgency, "select$" + e.Row.RowIndex);
        }
    }
    protected void ClearAfterSave()
    {
        btnsave.Text = "Save";
        txtAgencyName.Text = "";
        txtAddress.Text = "";
        txtPhoneNo.Text = "";
        txtFaxno.Text = "";
        txtEmail.Text = "";
    }
    protected void gdShippingAgency_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            int AgencyId = Convert.ToInt16(gdShippingAgency.DataKeys[e.RowIndex].Value);
            string str = "select Agencyid from Shipp Where AgencyId=" + AgencyId;
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Agency Already in Use..";
                Tran.Commit();
                return;
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from shippingAgency where AgencyId=" + AgencyId + "");
            lblMsg.Text = "Agency Deleted successfully.....";
            Tran.Commit();
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMsg.Text = ex.Message;

        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}