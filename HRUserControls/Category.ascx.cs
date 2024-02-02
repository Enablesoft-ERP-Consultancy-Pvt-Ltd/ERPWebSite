using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class HRUserControls_Category : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            FillGrid();
            TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@categoryid", SqlDbType.Int);
            param[0].Value = 0;
            param[1] = new SqlParameter("@category", txtcategory.Text.Trim());
            param[2] = new SqlParameter("@Dispseqno", txtseqno.Text == "" ? "0" : txtseqno.Text);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Date", TxtDate.Text);
            param[6] = new SqlParameter("@MinRate", TxtMinRate.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[HR_PRO_SAVECategory]", param);

            Tran.Commit();
            if (param[4].Value.ToString() != "")
            {
                lblmsg.Text = param[4].Value.ToString();

            }
            else
            {
                lblmsg.Text = "Category Saved !!!";

                refreshcontrol();
            }
            txtcategory.Focus();
            FillGrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void FillGrid()
    {
        string sql = @"Select a.Categoryid, a.Category, a.Dispseqno, REPLACE(CONVERT(NVARCHAR(11), b.FromDate, 106), ' ', '-') Date, b.MinimumRate MinRate 
                    From HR_categoryMaster a (Nolock)
                    JOIN HR_CategoryMinimumRateDefine b(Nolock) ON b.CategoryID = a.CATEGORYID 
                    Order By a.Dispseqno, a.Category";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        Dgdetail.DataSource = ds.Tables[0];
        Dgdetail.DataBind();

    }
    protected void refreshcontrol()
    {
        txtcategory.Text = "";
        txtseqno.Text = "";
        TxtMinRate.Text = "";
    }
    protected void Dgdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Dgdetail.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void Dgdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblcategoryid = (Label)Dgdetail.Rows[e.RowIndex].FindControl("lblcategoryid");
            TextBox txtcategorygrid = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtcategorygrid");
            TextBox txtdispseqnogrid = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtdispseqnogrid");
            TextBox txtDGDate = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtDGDate");
            TextBox txtDGminRate = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtMinRate");

            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@categoryid", lblcategoryid.Text);
            param[1] = new SqlParameter("@Category", txtcategorygrid.Text);
            param[2] = new SqlParameter("@Dispseqno", txtdispseqnogrid.Text == "" ? "0" : txtdispseqnogrid.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[6] = new SqlParameter("@Date", txtDGDate.Text);
            param[7] = new SqlParameter("@MinRate", txtDGminRate.Text);

            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "HR_PRO_UPDATECategory", param);
            //*************
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
            Dgdetail.EditIndex = -1;
            FillGrid();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void Dgdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        Dgdetail.EditIndex = -1;
        FillGrid();
    }

    protected void Dgdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblcategoryid = (Label)Dgdetail.Rows[e.RowIndex].FindControl("lblcategoryid");

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@categoryid", lblcategoryid.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "HR_PRO_DELETECategory", param);
            lblmsg.Text = param[1].Value.ToString();
            Tran.Commit();
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}
