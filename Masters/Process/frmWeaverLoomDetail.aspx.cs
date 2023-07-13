using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Process_frmWeaverLoomDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }
        if (!IsPostBack)
        {
            fillgrid();
            hnEditflag.Value = Request.QueryString["C"];
        }

    }
    protected void fillgrid()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string str = @"select PM.IssueOrderId,QualityName As Quality,DesignName As Design,
                     case When Unitid=1 Then SizeMtr Else Case When UnitId=2 Then Sizeft Else
                     case When UnitId=6 Then Sizeinch Else Sizeft End End End As Size
                     ,Sum(Qty) As Qty," + Request.QueryString["b"] + @" ProcessId,V.QualityId,V.DesignId,V.Sizeid,PM.UnitId
                     from Process_Issue_Master_" + Request.QueryString["b"] + " PM,Process_Issue_Detail_" + Request.QueryString["b"] + @" PD,V_FinishedItemDetail V
                     where PM.IssueOrderId=PD.IssueOrderId And PD.Item_Finished_id=v.Item_Finished_id
                    And PM.IssueOrderId=" + Request.QueryString["a"] + " And PM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                     group by PM.IssueOrderId,QualityName,DesignName,UnitId,SizeMtr,Sizeinch,Sizeft,V.QualityId,V.DesignId,V.Sizeid";

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
            GDLoomDetail.DataSource = ds;
            GDLoomDetail.DataBind();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[12];
            _array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _array[1] = new SqlParameter("@QualityId", SqlDbType.Int);
            _array[2] = new SqlParameter("@DesignId", SqlDbType.Int);
            _array[3] = new SqlParameter("@SizeId", SqlDbType.Int);
            _array[4] = new SqlParameter("@Qty", SqlDbType.Int);
            _array[5] = new SqlParameter("@Loom", SqlDbType.Int);
            _array[6] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[7] = new SqlParameter("@UserId", SqlDbType.Int);
            _array[8] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[9] = new SqlParameter("@UnitId", SqlDbType.Int);
            _array[10] = new SqlParameter("@DetailId", SqlDbType.Int);
            _array[11] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);

            //Delete existing Detail
            // SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from LoomDetail where IssueOrderId=" + Request.QueryString["a"] + "");
            //
            string str = "";

            for (int i = 0; i < GDLoomDetail.Rows.Count; i++)
            {
                _array[0].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblIssueOrderId")).Text;
                _array[1].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblQualityId")).Text;
                _array[2].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblDesignId")).Text;
                _array[3].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblSizeId")).Text;
                _array[4].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblQty")).Text;
                _array[5].Value = ((TextBox)GDLoomDetail.Rows[i].FindControl("txtloomDetail")).Text;
                _array[6].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblProcessId")).Text; ;
                _array[7].Value = Session["varuserId"];
                _array[8].Value = Session["varcompanyId"];
                _array[9].Value = ((Label)GDLoomDetail.Rows[i].FindControl("lblUnitId")).Text;
                if (ViewState["DetailId"] == null)
                {
                    ViewState["DetailId"] = "0";
                }
                _array[10].Direction = ParameterDirection.InputOutput;
                _array[10].Value = ViewState["DetailId"];
                _array[11].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveLoomDetail", _array);
                ViewState["DetailId"] = _array[10].Value.ToString();
            }
            Tran.Commit();
            lblErrorMessage.Text = _array[11].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}