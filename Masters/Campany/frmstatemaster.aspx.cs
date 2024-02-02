using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Campany_frmstatemaster : System.Web.UI.Page
{
    public static int StateId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            int Countryid = Convert.ToInt16(Request.QueryString["a"]);
            UtilityModule.ConditionalComboFill(ref ddCountry, "select CountryId,CountryName from CountryMaster Order by CountryName", true, "--Select--");
            if (ddCountry.Items.Count > 0)
            {
                ddCountry.SelectedValue = Countryid.ToString();
            }
            fill_grid();
        }
    }
    protected void fill_grid()
    {
        string strsql = "select StateID,StateName,CountryName,StateCode from State_master sm inner join Countrymaster cm on cm.Countryid=sm.Countryid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGState.DataSource = null;
        DGState.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGState.DataSource = ds.Tables[0];
            DGState.DataBind();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validate();
        if (ddCountry.Text != "" && txtStateName.Text != "")
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
                _arrPara[0] = new SqlParameter("@StateId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@StateName", SqlDbType.VarChar, 50);
                _arrPara[2] = new SqlParameter("@CountryID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);
                _arrPara[6] = new SqlParameter("@StateCode", SqlDbType.VarChar, 5);


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = StateId;
                _arrPara[1].Value = txtStateName.Text;
                _arrPara[2].Value = ddCountry.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[5].Direction = ParameterDirection.Output;
                _arrPara[6].Value = txtStateCode.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Statemaster", _arrPara);
                Tran.Commit();
                lblerr.Text = _arrPara[5].Value.ToString();
                StateId = 0;
                txtStateName.Text = "";
                ddCountry.Enabled = true;
                fill_grid();
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
                strsql = "select StateName from State_Master where statename='" + ViewState["id"].ToString() + "' and StateName='" + txtStateName.Text + "' And  masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select *  from State_Master where CountryName='" + txtStateName.Text + "' And masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "StateName already exists............";
                txtStateName.Text = "";
                ddCountry.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmstatemaster.aspx");
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



    protected void DGState_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGState, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGState_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
          //  e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }




    protected void btndelete_Click1(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Stateid from customerinfo where Stateid=" + StateId + ""));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "delete  from State_master where Stateid=" + StateId + "");
                DataSet dt = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'State_master'," + StateId + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                txtStateName.Text = "";
                lblerr.Visible = true;
                lblerr.Text = "Value Deleted..............";
                StateId = 0;

            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Value in Use...";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerr.Visible = true;
            lblerr.Text = ex.Message;
            
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmstatemaster.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        fill_grid();
        btndelete.Visible = false;
    }
    protected void DGState_SelectedIndexChanged(object sender, EventArgs e)
    {
        {
            ddCountry.Enabled = false;
            StateId = (int)DGState.SelectedDataKey.Value;
            //Session["id"] = id;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select StateName,CountryId,StateCode    from State_Master where StateId=" + StateId);
            try
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    txtStateName.Text = ds.Tables[0].Rows[0]["StateName"].ToString();
                    ddCountry.SelectedValue = ds.Tables[0].Rows[0]["CountryId"].ToString();
                    txtStateCode.Text = ds.Tables[0].Rows[0]["StateCode"].ToString();

                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmstateMaster.aspx");
                Logs.WriteErrorLog("Masters_Campany_frmstatemas|Fill_Grid_Data|" + ex.Message);
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
    
}
