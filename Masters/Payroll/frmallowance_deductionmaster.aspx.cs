using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Payroll_frmallowance_deductionmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"]==null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State==ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param=new SqlParameter[6];
            param[0] = new SqlParameter("@ID",SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = "0";
            param[1] = new SqlParameter("@Typeid",DDtype.SelectedValue);
            param[2] = new SqlParameter("@Parametername", txtparametername.Text.Trim());
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar,100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveHrAllowancesmaster", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
            FillGrid();
            txtparametername.Text = "";
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
    protected void FillGrid()
    {
        string str = @"select ID,ParameterName From HR_AllowancesMaster WHere 1=1";
        if (DDtype.SelectedIndex>0)
        {
            str = str + " and Typeid=" + DDtype.SelectedValue;
        }
        str = str + " order by Parametername";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGDetail.DataSource = ds.Tables[0];
        DGDetail.DataBind();
    }
    protected void DDtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DGDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGDetail.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void DGDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGDetail.EditIndex = -1;
        FillGrid();
    }
    protected void DGDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State==ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid=(Label)DGDetail.Rows[e.RowIndex].FindControl("lblid");
            TextBox txtparameter=(TextBox)DGDetail.Rows[e.RowIndex].FindControl("txteditparametername");

            SqlParameter[] param=new SqlParameter[4];
            param[0] = new SqlParameter("@ID",lblid.Text);
            param[1] = new SqlParameter("@parametername", txtparameter.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateHrAllowancesmaster", param);
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            DGDetail.EditIndex = -1;
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
}