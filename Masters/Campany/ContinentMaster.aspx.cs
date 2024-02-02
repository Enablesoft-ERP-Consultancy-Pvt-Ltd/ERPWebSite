using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_Campany_ContinentMaster : System.Web.UI.Page
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
        lblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        gdContinent.DataSource = Fill_Grid_Data();
        gdContinent.DataBind();
    }
    //private DataSet fill_grid_data()
    //{

    //    DataSet ds = null;
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    try
    //    {
    //        string strsql = "select ContinentId,ContinentName,ContinentCode from ContinentMaster where masterCompanyId=" + Session["varMasterCompanyIDForERP"];
    //        con.Open();
    //        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
    //    }
    //    catch (Exception ex)
    //    {
    //        UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
    //        Logs.WriteErrorLog("Masters_Campany_ContinentMaster|Fill_Grid_Data|" + ex.Message);
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //            con.Dispose();
    //        }
    //    }
    //    return ds;
    //}


    protected void gdContinent_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdContinent.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        txtid.Text = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from ContinentMaster where ContinentId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["ContinentId"].ToString();
                txtContinent.Text = ds.Tables[0].Rows[0]["Continentname"].ToString();
                txtContinentCode.Text = ds.Tables[0].Rows[0]["ContinentCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_ContinentMaster|Fill_Grid_Data|" + ex.Message);
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
    protected void gdContinent_RowCreated(object sender, GridViewRowEventArgs e)
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
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtContinent.Text != "" && txtContinentCode.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@ContinentId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ContinentName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@ContinentCode", SqlDbType.NVarChar, 50);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtContinent.Text.ToUpper();
                _arrPara[2].Value = txtContinentCode.Text.ToUpper();
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Continent", _arrPara);
                txtContinent.Text = "";
                txtContinentCode.Text = "";
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_ContinentMaster|cmdSave_Click|" + ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
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
                if (Request.QueryString["id"] != null)
                {
                    if (Request.QueryString["id"] == "1")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
                    }
                }
            }
            lblerr.Visible = true;
            lblerr.Text = "Save Details............";
        }
        else
        {
            if (lblerr.Text == "ContinentName already exists............")
            {
                lblerr.Visible = true;
                lblerr.Text = "ContinentName already exists............";
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Fill Details............";
            }
        }
        btnsave.Text = "Save";
        btndelete.Visible = false;
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
                strsql = "select ContinentName from ContinentMaster where ContinentId!='" + ViewState["id"].ToString() + "' and ContinentName='" + txtContinent.Text + "' And  masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select ContinentName from ContinentMaster where ContinentName='" + txtContinent.Text + "' And masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "ContinentName already exists............";
                txtContinent.Text = "";
                txtContinent.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Continentid from customerinfo where Continentid=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from ContinentMaster where ContinentId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'ContinentMaster'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                lblerr.Visible = true;
                lblerr.Text = "Value Deleted..............";
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
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
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
        txtContinent.Text = "";
        txtContinentCode.Text = "";

    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select ContinentId,ContinentName,ContinentCode from ContinentMaster where masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/ContinentMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_ContinentMaster|Fill_Grid_Data|" + ex.Message);
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



    protected void gdContinent_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdContinent, "select$" + e.Row.RowIndex);
        }

    }


}

