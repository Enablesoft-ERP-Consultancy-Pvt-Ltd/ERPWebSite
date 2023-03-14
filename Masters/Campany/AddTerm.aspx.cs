using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddTerm : CustomPage
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
        Label1.Visible = false;
    }
    private void Fill_Grid()
    {
        gdTerm.DataSource = Fill_Grid_Data();
        gdTerm.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select TermId SrNo,TermName from Term where MasterCompanyId=" + Session["varCompanyid"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmGoods|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTerm.aspx");
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtTerm.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@TermId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@TermName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtTerm.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_TERM", _arrPara);
                txtid.Text = "0";
                txtTerm.Text = "";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTerm.aspx");
                //Logs.WriteErrorLog("Masters_Campany_frmGoods|cmdSave_Click|" + ex.Message);
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
            Label1.Visible = true;
            Label1.Text = "Save Details............";
        }
        else
        {
            if (Label1.Text == "TermName already exists............")
            {
                Label1.Visible = true;
                Label1.Text = "TermName already exists............";
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Please Fill Details.........";
            }
            }
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtTerm.Text = "";
    }
    protected void gdTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtid.Text = gdTerm.SelectedRow.Cells[0].Text;
        //Session["id"] = txtid.Text;
        ViewState["id"] = txtid.Text;
        txtTerm.Text = gdTerm.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        btnsave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void gdTerm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdTerm.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdTerm_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdTerm, "select$" + e.Row.RowIndex);
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
                strsql = "select TermName from Term where TermId!='" + ViewState["id"].ToString() + "' and  TermName='" + txtTerm.Text + "' And MasterCompanyId=" + Session["varCompanyid"];
            }
            else
            {
                strsql = "select TermName from Term where TermName='" + txtTerm.Text + "' And MasterCompanyId=" + Session["varCompanyid"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label1.Visible = true;
                Label1.Text = "TermName already exists............";
                txtTerm.Text = "";
                txtTerm.Focus();
            }
            else
            {
                Label1.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTerm.aspx");
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
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Term where TermId=" + ViewState["id"].ToString());
            DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
            SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'TermId'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
            Label1.Visible = true;
            Label1.Text = "Value Deleted............";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTerm.aspx");
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
        txtTerm.Text = "";
        txtid.Text = "0";
    }
    protected void gdTerm_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
      //  if (e.Row.RowType == DataControlRowType.DataRow &&
               //   e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}