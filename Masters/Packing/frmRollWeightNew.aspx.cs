using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Packing_frmRollWeightNew : System.Web.UI.Page
{
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            TxtPKLNo.Text = Convert.ToString(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select Distinct TPackingNo from V_PackingMaster where PackingID = " + Request["ID"]));
            LoadRollWEight();           
        }
    }
    private void LoadRollWEight()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string sql = @"Select PI.Id,PI.RollNo,PI.PackingId,PI.RollFrom,PI.RollTo,isnull(PI.SinglePcsNetWt,0) as SinglePcsNetWt,isnull(PI.SinglePcsGrossWt,'') as SinglePcsGrossWt,
                        isnull(PI.L,0) as BaleLength,ISNULL(PI.W,0) as BaleWidth,isnull(PI.H,0) as BaleHeight,isnull(PI.CBM,0) as BaleCBM,isnull(PcsFrom,0) as PcsFrom,
                        isnull(PI.Pcs,0) as Pcs
				        From PACKINGINFORMATION PI where PI.PackingID = " + Request["ID"];

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
        GdvRollWeight.DataSource = ds.Tables[0];
        GdvRollWeight.DataBind();    

    }    
    protected void GdvRollWeight_RowEditing(object sender, GridViewEditEventArgs e)
    {

        //GdvRollWeight.EditIndex = e.NewEditIndex;
        //LoadRollWEight();
        ////GdvRollWeight.Rows[GdvRollWeight.EditIndex].Cells[6].Text = ViewState["CalArea"].ToString();
        ////ViewState["CalArea"] = GdvRollWeight.Rows[GdvRollWeight.EditIndex].Cells[6].Text;
        ////ViewState["IsEdited"] = "1";

    }
    protected void GdvRollWeight_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ////ViewState["IsEdited"] = "0";
        //GdvRollWeight.EditIndex = -1;
        //LoadRollWEight();
        ////CalcTotals();
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Strdetail = "";
        for (int i = 0; i < GdvRollWeight.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GdvRollWeight.Rows[i].FindControl("Chkboxitem"));
            Label lblId = ((Label)GdvRollWeight.Rows[i].FindControl("lblId"));
            TextBox TxtNetWt = ((TextBox)GdvRollWeight.Rows[i].FindControl("TxtNetWt"));
            TextBox TxtGrossswt = ((TextBox)GdvRollWeight.Rows[i].FindControl("TxtGrossswt"));
            TextBox TxtWidth = ((TextBox)GdvRollWeight.Rows[i].FindControl("TxtWidth"));
            TextBox TxtLength = ((TextBox)GdvRollWeight.Rows[i].FindControl("TxtLength"));
            TextBox TxtHeight = ((TextBox)GdvRollWeight.Rows[i].FindControl("TxtHeight"));
            Label lblCBM = (Label)GdvRollWeight.Rows[i].FindControl("lblCBM");
            Label lblPackingId = (Label)GdvRollWeight.Rows[i].FindControl("lblPackingId");
            Label lblNoOfBales = (Label)GdvRollWeight.Rows[i].FindControl("lblNoOfBales");
            Label lblPcs = (Label)GdvRollWeight.Rows[i].FindControl("lblPcs");


            if (Chkboxitem.Checked == true && (TxtNetWt.Text.Trim() != "" || TxtGrossswt.Text.Trim() != "" || TxtWidth.Text != "" || TxtLength.Text != "" || TxtHeight.Text != ""))
            {
                Strdetail = Strdetail + lblId.Text + '|' + TxtNetWt.Text + '|' + TxtGrossswt.Text + '|' + TxtWidth.Text + '|' + TxtLength.Text + '|' + TxtHeight.Text + '|' + lblCBM.Text + '|' + lblPackingId.Text + '|' + lblNoOfBales.Text + '|' + lblPcs.Text + '~';
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[4];            
            param[0] = new SqlParameter("@UserID", Session["varuserid"]);
            param[1] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[2] = new SqlParameter("@StringDetail", Strdetail);
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            param[3].Direction = ParameterDirection.Output;

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateInvoicePackingRollWeightNew", param);
            //*******************               

            //lblmessage.Text = param[6].Value.ToString();
            Msg = param[3].Value.ToString();
            MessageSave(Msg);
            Tran.Commit();
            LoadRollWEight();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            Msg = ex.Message;
            MessageSave(Msg);
            //lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void GdvRollWeight_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //string id = ((Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblId")).Text;        

        //TextBox TxtNetWt = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtNetWt");
        //TextBox TxtGrossswt = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtGrossswt");
      
        //TextBox TxtWidth = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtWidth");
        //TextBox TxtLength = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtLength");
        //TextBox TxtHeight = (TextBox)GdvRollWeight.Rows[e.RowIndex].FindControl("TxtHeight");

        //Label lblCBM = (Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblCBM");
        // Label lblPackingId = (Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblPackingId");
        // Label lblNoOfBales = (Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblNoOfBales");
        // Label lblPcs = (Label)GdvRollWeight.Rows[e.RowIndex].FindControl("lblPcs");

        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //try
        //{
        //    SqlParameter[] _param = new SqlParameter[10];
        //    _param[0] = new SqlParameter("@PackingID", lblPackingId.Text);
        //    _param[1] = new SqlParameter("@ID", id);
        //    _param[2] = new SqlParameter("@SinglePcsNetWt", TxtNetWt.Text);
        //    _param[3] = new SqlParameter("@SinglePcsGrossWt", TxtGrossswt.Text);
        //    _param[4] = new SqlParameter("@BaleWidth", TxtWidth.Text);
        //    _param[5] = new SqlParameter("@BaleLength", TxtLength.Text);
        //    _param[6] = new SqlParameter("@BaleHeight", TxtHeight.Text);
        //    // _param[7] = new SqlParameter("@Msg", CBM);
        //    _param[7] = new SqlParameter("@Msg", SqlDbType.NVarChar, 200);
        //    _param[7].Direction = ParameterDirection.Output;
        //    _param[8] = new SqlParameter("@NoOfBales", lblNoOfBales.Text);
        //    _param[9] = new SqlParameter("@Pcs", lblPcs.Text);

        //    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_UpdateRollWeightNew", _param);
        //    Msg = _param[7].Value.ToString();            
        //    MessageSave(Msg);
        //    GdvRollWeight.EditIndex = -1;
           
        //    LoadRollWEight();            
            
        //}
        //catch (Exception ex)
        //{
        //    UtilityModule.MessageAlert(ex.Message, "Masters/Packing/frmRollWeightNew.aspx");
        //    Msg = "Go to Catch in Grid RowUpdating";
        //    MessageSave(Msg);
        //    con.Close();
        //    con.Dispose();
        //}
    }
}