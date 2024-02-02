using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Campany_FrmSubcontinentMaster : System.Web.UI.Page
{
    public static int SubcontinentID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            int ContinentId = Convert.ToInt16(Request.QueryString["a"]);
            UtilityModule.ConditionalComboFill(ref ddContinent, "select ContinentId,ContinentName from ContinentMaster Order by ContinentName", true, "--Select--");
            if (ddContinent.Items.Count > 0)
            {
                ddContinent.SelectedValue = ContinentId.ToString();
            }
            fill_grid();
        }
    }
    protected void fill_grid()
    {
        string strsql = "select SubcontinentID,SubcontinentName,ContinentName from Subcontinentmaster sm inner join Continentmaster cm on cm.Continentid=sm.Continentid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGSubcontinent.DataSource = null;
        DGSubcontinent.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGSubcontinent.DataSource = ds.Tables[0];
            DGSubcontinent.DataBind();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validate();
        if (ddContinent.Text != "" && txtsubcontinentName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@SubcontinentId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@SubcontinentName", SqlDbType.VarChar, 50);
                _arrPara[2] = new SqlParameter("@ContinentID", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = SubcontinentID;
                _arrPara[1].Value = txtsubcontinentName.Text;
                _arrPara[2].Value = ddContinent.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[5].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_Subcontinentmaster", _arrPara);
                Tran.Commit();
                lblerr.Text = _arrPara[5].Value.ToString();
                SubcontinentID = 0;
                txtsubcontinentName.Text = "";
                ddContinent.Enabled = true;
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
                strsql = "select subcontinentName from Subcontinentmaster where Subcontinentname='" + ViewState["id"].ToString() + "' and SubcontinentName='" + txtsubcontinentName.Text + "' And  masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select  from Subcontinentmaster where ContinentName='" + txtsubcontinentName.Text + "' And masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Subcontinentname already exists............";
                txtsubcontinentName.Text = "";
                ddContinent.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSubcontinentMaster.aspx");
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Subcontinentid from customerinfo where Subcontinentid=" + SubcontinentID + ""));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "delete  from SubcontinentMaster where Subcontinentid=" + SubcontinentID + "");
                DataSet dt = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'SubcontinentMaster'," + SubcontinentID + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                txtsubcontinentName.Text = "";
                lblerr.Visible = true;
                lblerr.Text = "Value Deleted..............";
                SubcontinentID = 0;

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

    protected void DGSubcontinent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSubcontinent, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGSubcontinent_RowCreated(object sender, GridViewRowEventArgs e)
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
                  //e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
    protected void DGSubcontinent_SelectedIndexChanged(object sender, EventArgs e)
    {
        {
            ddContinent.Enabled = false;
            SubcontinentID = (int)DGSubcontinent.SelectedDataKey.Value;
            //Session["id"] = id;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SubcontinentName,ContinentId   from Subcontinentmaster where SubcontinentId=" + SubcontinentID);
            try
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    txtsubcontinentName.Text = ds.Tables[0].Rows[0]["SubcontinentName"].ToString();
                    ddContinent.SelectedValue = ds.Tables[0].Rows[0]["ContinentId"].ToString();

                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmSubcontinentMaster.aspx");
                Logs.WriteErrorLog("Masters_Campany_frmSubcontinentMsater|Fill_Grid_Data|" + ex.Message);
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