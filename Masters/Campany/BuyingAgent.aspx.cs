using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Masters_Campany_BuyingAgent : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtAgentName.Focus();
            txtid.Text = "0";
            Fill_Grid();
            UtilityModule.ConditionalComboFill(ref ddbuyinghouse, "select buyinghouseid,Name_buying_house from buyinghouse Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"order by Name_buying_house", true, "--Select Buyinghouse Name--");
        }
        Label2.Visible = false;
    }
    private void Fill_Grid()
    {
        gdBuyingAgent.Visible = true;
        gdBuyingAgent.DataSource = Fill_Grid_Data();
        gdBuyingAgent.DataBind();
        Session["ReportPath"] = "Reports/BuyingAgentNew.rpt";
        Session["CommanFormula"] = "";
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string str = "select BuyeingAgentId,BuyeingAgentName,Address,PhoneNo,Email,isnull(buyinghouseid,0) as buyinghouseid from BuyingAgent Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/BuyingAgent.aspx");
        }
        return ds;
    }
    protected void gdBuyingAgent_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtid.Text = ((Label)gdBuyingAgent.Rows[gdBuyingAgent.SelectedIndex].FindControl("lblBuyeingAgentId")).Text;
        // Session["id"] = txtid.Text;
        ViewState["BuyingAgentid"] = txtid.Text;
        txtAgentName.Text = gdBuyingAgent.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        txtAddress.Text = gdBuyingAgent.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        txtPhone.Text = gdBuyingAgent.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        txtEmail.Text = gdBuyingAgent.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        ddbuyinghouse.SelectedValue = ((Label)gdBuyingAgent.Rows[gdBuyingAgent.SelectedIndex].FindControl("lblBuyingHouseId")).Text;
        btndelete.Visible = true;
        btnsave.Text = "Update";
    }
    protected void gdBuyingAgent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdBuyingAgent, "select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtAgentName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[9];
                _arrpara[0] = new SqlParameter("@BuyeingAgentId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@BuyeingAgentName", SqlDbType.NVarChar, 150);
                _arrpara[2] = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
                _arrpara[3] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
                _arrpara[4] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
                _arrpara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@buyinghouseid", SqlDbType.Int);

                if (btnsave.Text == "Update")
                {
                    _arrpara[0].Value = ViewState["BuyingAgentid"];
                }
                else
                {
                    _arrpara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrpara[1].Value = txtAgentName.Text.ToUpper();
                _arrpara[2].Value = txtAddress.Text.ToUpper();
                _arrpara[3].Value = txtPhone.Text.ToUpper();
                _arrpara[4].Value = txtEmail.Text.ToUpper();
                _arrpara[5].Value = Session["varuserid"].ToString();
                _arrpara[6].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrpara[7].Value = ddbuyinghouse.SelectedValue;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_BUYEING", _arrpara);
                Tran.Commit();
                Label2.Visible = true;
                Label2.Text = "Save Details.............";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Label2.Visible = true;
                Label2.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/BuyingAgent.aspx");
            }
            finally
            {
                con.Close();
            }
            Fill_Grid();
            txtid.Text = "0";
            txtAgentName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

        }
        else
        {
            if (Label2.Text == "BuyeingAgentName already exists............")
            {
                Label2.Visible = true;
                Label2.Text = "BuyeingAgentName already exists............";
            }
            else
            {
                Label2.Visible = true;
                Label2.Text = "Please Fill Details.............";
            }
        }
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void gdBuyingAgent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdBuyingAgent.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select BuyeingAgentName from BuyingAgent where BuyeingAgentId!='" + ViewState["BuyingAgentid"].ToString() + "' and BuyeingAgentName='" + txtAgentName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select BuyeingAgentName from BuyingAgent where BuyeingAgentName='" + txtAgentName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label2.Visible = true;
                Label2.Text = "BuyeingAgentName already exists............";
                txtAgentName.Text = "";
                txtAgentName.Focus();
            }
            else
            {
                Label2.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/BuyingAgent.aspx");
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
            _array[0] = new SqlParameter("@BuyingAgentID", ViewState["BuyingAgentid"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteBuyingAgent", _array);
            Tran.Commit();
            Label2.Visible = true;
            Label2.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Label2.Visible = true;
            Label2.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/BuyingAgent.aspx");
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
        txtAgentName.Text = "";
        txtAddress.Text = "";
        txtPhone.Text = "";
        txtEmail.Text = "";
        txtid.Text = "0";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        txtAddress.Text = "";
        txtAgentName.Text = "";
        txtEmail.Text = "";
        txtid.Text = "0";
        txtPhone.Text = "";
        btndelete.Visible = false;
        btnsave.Text = "Save";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = @"SELECT BuyeingAgentId,BuyeingAgentName,BA.Address,PhoneNo,BA.Email,BH.Name_buying_house FROM BuyingAgent BA left outer join BuyingHouse BH on BH.buyinghouseid=BA.buyinghouseid  where BA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\BuyingAgentNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\BuyingAgentNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void gdBuyingAgent_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }


    protected void btnbuyinghouseClose_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddbuyinghouse, "select buyinghouseid,Name_buying_house from buyinghouse Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"order by Name_buying_house", true, "--select--");

    }
   
}