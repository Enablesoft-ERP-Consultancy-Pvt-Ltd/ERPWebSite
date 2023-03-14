using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_FrmQCParaMeterName : System.Web.UI.Page
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
            ViewState["ParaId"] = "0";
            TxtSerialNumber.Attributes.Add("onkeypress", "return isNumberKey(event)");
            Fill_grid();
        }
    }
    private void Fill_grid()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ParaID,ParaName ParameterName,SName ShortName,SrNo SerialNumber,Specification Specified,Method Method From QCParameter Where CategoryID=" + Request.QueryString["Category"] + " And ProcessId=" + Request.QueryString["Proc"] + " Order by SrNo");
        DG.DataSource = Ds;
        DG.DataBind();
    }
    
    protected void btnsave_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[11];

            arr[0] = new SqlParameter("@ParaID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ParaName", SqlDbType.NVarChar, 100);
            arr[2] = new SqlParameter("@SName", SqlDbType.NVarChar, 20);
            arr[3] = new SqlParameter("@SrNo", SqlDbType.Int);
            arr[4] = new SqlParameter("@Specification", SqlDbType.NVarChar, 50);
            arr[5] = new SqlParameter("@Method", SqlDbType.NVarChar, 50);
            arr[6] = new SqlParameter("@CategoryID", SqlDbType.Int);
            arr[7] = new SqlParameter("@ProcessId", SqlDbType.Int);
            arr[8] = new SqlParameter("@varuserid", SqlDbType.Int);
            arr[9] = new SqlParameter("@varcompanyid", SqlDbType.Int);
            arr[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
           

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = Convert.ToInt32(txtid.Text);
            arr[1].Value = txtParameterName.Text.ToUpper();
            arr[2].Value = TxtShortName.Text.ToUpper();
            arr[3].Value = TxtSerialNumber.Text;
            arr[4].Value = txtSpecified.Text.ToUpper();
            arr[5].Value = txtMethod.Text.ToUpper();
            arr[6].Value = Request.QueryString["Category"];
            arr[7].Value = Request.QueryString["Proc"];
            arr[8].Value = Session["varuserid"].ToString();
            arr[9].Value = Session["varCompanyId"].ToString();
            arr[10].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_QCPARAMETER]", arr);
            Tran.Commit();
            LblErrorMessage.Text = arr[10].Value.ToString();
            Fill_grid();
            AfterSaveClear();
            txtid.Text = "0";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmQCParaMeterName.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void AfterSaveClear()
    {
        txtParameterName.Text = "";
        TxtSerialNumber.Text = "";
        TxtShortName.Text = "";
        txtSpecified.Text = "";
        txtMethod.Text = "";
        txtParameterName.Focus();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarParaID = Convert.ToInt32(DG.DataKeys[e.RowIndex].Value);
            SqlParameter[] arr = new SqlParameter[2];

            arr[0] = new SqlParameter("@ParaID", SqlDbType.Int);
            arr[1] = new SqlParameter("@RetID", SqlDbType.Int);

            arr[0].Value = VarParaID;
            arr[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_QCPARAMETER_DELETE]", arr);
            Tran.Commit();
            Fill_grid();
            LblErrorMessage.Visible = true;
            if (Convert.ToInt32(arr[1].Value) == 1)
            {
                LblErrorMessage.Text = "AlReady Used";
            }
            else
            {
                LblErrorMessage.Text = "Successfully Deleted...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmQCParaMeterName.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = DG.SelectedDataKey.Value.ToString();
        // Session["id"] = id;
        ViewState["ParaId"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ParaID,ParaName,SName,SrNo,Specification,Method from QCPARAMETER where ParaId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["ParaID"].ToString();
                txtParameterName.Text = ds.Tables[0].Rows[0]["ParaName"].ToString();
                TxtSerialNumber.Text = ds.Tables[0].Rows[0]["SrNo"].ToString();
                TxtShortName.Text = ds.Tables[0].Rows[0]["SName"].ToString();
                txtSpecified.Text = ds.Tables[0].Rows[0]["Specification"].ToString();
                txtMethod.Text = ds.Tables[0].Rows[0]["Method"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmQCParaMeterName.aspx");
            //Logs.WriteErrorLog("Masters_Campany_frmpenality|Fill_Grid_Data|" + ex.Message);
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
        //btndelete.Visible = true;
    }
   
}