using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddShipping : CustomPage
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
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "9":
                    lblAgent.Text = "Company Name";
                    break;
                default:
                    lblAgent.Text = "Agent Name";
                    break;
            }
            UtilityModule.ConditionalComboFill(ref DDbankinformation, "select bankid,bankname from bank where mastercompanyid=" + Session["varMasterCompanyIDForERP"] + "order by bankname", true, "--plz select bank");
            UtilityModule.ConditionalComboFill(ref DDAgencyName, "select Agencyid,AgencyName From ShippingAgency Order by AgencyName", true, "--Select Agency Name--");
        }
        Label1.Visible = false;
    }
    private void Fill_Grid()
    {
        gdShippingMaster.DataSource = Fill_Grid_Data();
        gdShippingMaster.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            //string str = "select AgentId,AgentName,Address,S.ContectPerson As ContactPerson,S.PhoneNo,Mobile,Fax, S.Email,definecompany,modeoftranaction,S.bankid,B.BankName from shipp S left outer join Bank B on S.BankId=B.BankId Where s.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Agentid";
            //con.Open();
            //ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            string str = "select AgentId,AgentName,Address,S.ContectPerson As ContactPerson,S.PhoneNo,Mobile,Fax, S.Email,definecompany,modeoftranaction,S.bankid,B.BankName,AgencyId from shipp S left outer join Bank B on S.BankId=B.BankId Where s.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (DDAgencyName.SelectedIndex > 0)
            {
                str = str + " And AgencyId=" + DDAgencyName.SelectedValue;

            }
            str = str + "order by Agentid";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddShipping.aspx");
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
    protected void gdShippingMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdShippingMaster.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Shipp where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And AgentId=" + id);
        try
        {
            txtid.Text = ds.Tables[0].Rows[0]["AgentId"].ToString();
            txtCompanyName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
            txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            txtContactPerson.Text = ds.Tables[0].Rows[0]["ContectPerson"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            DDmodeoftranaction.SelectedItem.Text = ds.Tables[0].Rows[0]["modeoftranaction"].ToString();
            DDdefinecompany.SelectedItem.Text = ds.Tables[0].Rows[0]["definecompany"].ToString();
            DDbankinformation.SelectedValue = ds.Tables[0].Rows[0]["bankid"].ToString();
            DDAgencyName.SelectedValue = ds.Tables[0].Rows[0]["AgencyId"].ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddShipping.aspx");
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
    protected void gdShippingMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdShippingMaster, "select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtCompanyName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[14];
                _arrpara[0] = new SqlParameter("@Agentid", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@AgentName", SqlDbType.NVarChar, 50);
                _arrpara[2] = new SqlParameter("@Address", SqlDbType.NVarChar, 50);
                _arrpara[3] = new SqlParameter("@ContectPerson", SqlDbType.NVarChar, 50);
                _arrpara[4] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
                _arrpara[5] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
                _arrpara[6] = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
                _arrpara[7] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
                _arrpara[8] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@definecompany", SqlDbType.VarChar, 50);
                _arrpara[11] = new SqlParameter("@modeoftranaction", SqlDbType.VarChar, 50);
                _arrpara[12] = new SqlParameter("@bankid", SqlDbType.Int);
                _arrpara[13] = new SqlParameter("@AgencyId", SqlDbType.Int);

                if (btnsave.Text == "Update")
                {
                    _arrpara[0].Value = ViewState["id"];
                }
                else
                {
                    _arrpara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrpara[1].Value = txtCompanyName.Text.ToUpper();
                _arrpara[2].Value = txtAddress.Text.ToUpper();
                _arrpara[3].Value = txtContactPerson.Text.ToUpper();
                _arrpara[4].Value = txtPhone.Text.ToUpper();
                _arrpara[5].Value = txtMobile.Text.ToUpper();
                _arrpara[6].Value = txtFax.Text.ToUpper();
                _arrpara[7].Value = txtEmail.Text.ToUpper();
                _arrpara[8].Value = Session["varuserid"].ToString();
                _arrpara[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrpara[10].Value = DDdefinecompany.SelectedItem.Text.ToUpper();
                _arrpara[11].Value = DDmodeoftranaction.SelectedItem.Text.ToUpper();
                _arrpara[12].Value = DDbankinformation.SelectedValue.ToUpper();
                _arrpara[13].Value = DDAgencyName.SelectedValue;
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_SHIPPING", _arrpara);
                Fill_Grid();
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddShipping.aspx");
            }
            finally
            {
                con.Close();
            }
            Label1.Visible = true;
            Label1.Text = "Save Details............";
        }
        else
        {
            if (Label1.Text == "Agent already exists............")
            {
                Label1.Visible = true;
                Label1.Text = "Agent already exists............";
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Please Fill Details............";
            }
        }
        txtid.Text = "0";
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";
    }
    protected void gdShippingMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdShippingMaster.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select AgentName from Shipp where AgentId!='" + ViewState["AgentId"].ToString() + "' and  AgentName='" + txtCompanyName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And AgencyId=" + DDAgencyName.SelectedValue;
            }
            else
            {
                strsql = "select AgentName from Shipp where AgentName='" + txtCompanyName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And AgencyId=" + DDAgencyName.SelectedValue;
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label1.Visible = true;
                Label1.Text = "Agent already exists............";
                txtCompanyName.Text = "";
                txtCompanyName.Focus();
            }
            else
            {
                Label1.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddShipping.aspx");
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
            _array[0] = new SqlParameter("@ShippingAgentID", ViewState["AgentId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteShipp", _array);
            Tran.Commit();
            Label1.Visible = true;
            Label1.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label1.Visible = true;
            Label1.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ShippingMaster.aspx");
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
        txtid.Text = "0";
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";

        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //try
        //{
        //    int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ShippingAgent from customerinfo where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And ShippingAgent=" + ViewState["id"].ToString()));
        //    if (id <= 0)
        //    {
        //        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from shipp where AgentId=" + ViewState["id"].ToString());
        //        DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
        //        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'shipp'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
        //        Label1.Visible = true;
        //        Label1.Text = "Value Deleted.......";
        //    }
        //    else
        //    {
        //        Label1.Visible = true;
        //        Label1.Text = "Value in Use......";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
        //}
        //finally
        //{
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        //Fill_Grid();
        //btndelete.Visible = false;
        //btnsave.Text = "Save";
        //txtid.Text = "0";
        //txtCompanyName.Text = "";
        //txtAddress.Text = "";
        //txtContactPerson.Text = "";
        //txtPhone.Text = "";
        //txtMobile.Text = "";
        //txtEmail.Text = "";
        //txtFax.Text = "";
    }
    protected void gdShippingMaster_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void btnAgencyClose_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDAgencyName, "select AgencyId,AgencyName from ShippingAgency Order by AgencyName", true, "--Plz Select Agency--");
    }
    protected void DDAgencyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
  
}