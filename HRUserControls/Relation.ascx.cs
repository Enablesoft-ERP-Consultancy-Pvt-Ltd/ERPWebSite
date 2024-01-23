using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class HRUserControls_Relation : System.Web.UI.UserControl
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@relationid", SqlDbType.Int);
            param[0].Value = 0;
            param[1] = new SqlParameter("@relation", txtrelation.Text.Trim());
            param[2] = new SqlParameter("@Dispseqno", txtseqno.Text == "" ? "0" : txtseqno.Text);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[HR_PRO_SAVERelation]", param);

            Tran.Commit();
            if (param[4].Value.ToString() != "")
            {
                lblmsg.Text = param[4].Value.ToString();

            }
            else
            {
                lblmsg.Text = "Relation Saved !!!";

                refreshcontrol();
            }
            txtrelation.Focus();
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
        string sql = "select Relationid,Relation,Dispseqno From HR_RelationMaster order by Dispseqno,Relation";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        Dgdetail.DataSource = ds.Tables[0];
        Dgdetail.DataBind();

    }
    protected void refreshcontrol()
    {
        txtrelation.Text = "";
        txtseqno.Text = "";

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
            Label lblrelationid = (Label)Dgdetail.Rows[e.RowIndex].FindControl("lblrelationid");
            TextBox txtrelationgrid = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtrelationgrid");
            TextBox txtdispseqnogrid = (TextBox)Dgdetail.Rows[e.RowIndex].FindControl("txtdispseqnogrid");


            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@relationid", lblrelationid.Text);
            param[1] = new SqlParameter("@relation", txtrelationgrid.Text);
            param[2] = new SqlParameter("@Dispseqno", txtdispseqnogrid.Text == "" ? "0" : txtdispseqnogrid.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "HR_PRO_UPDATERelation", param);
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

            Label lblLocationid = (Label)Dgdetail.Rows[e.RowIndex].FindControl("lblrelationid");

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Relationid", lblLocationid.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "HR_PRO_DELETERelation", param);
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