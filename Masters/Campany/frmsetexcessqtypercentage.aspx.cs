using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmsetexcessqtypercentage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void ddpercentageexcessqtyfor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtpercentage.Text = "";
        lblmsg.Text = "";
        string column = "";
        TrPPNo.Visible = false;
        switch (ddpercentageexcessqtyfor.SelectedValue)
        {
            case "1":
                column = "PercentageExecssQty";
                break;
            case "2":
                column = "PercentageExecssQtyForIndent";
                break;
            case "3":
                column = "PercentageExecssQtyForIndentRawRec";
                break;
            case "4":
                column = "PercentageExecssQtyForProcessIss";
                break;
            default:
                break;
        }
        if (column != "")
        {
            string str = "select " + column + " as value From Mastersetting";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtpercentage.Text = ds.Tables[0].Rows[0]["value"].ToString();
            }
        }
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" && column == "PercentageExecssQtyForIndent")
        {
            TrPPNo.Visible = true;
            if (column == "PercentageExecssQtyForIndent")
            {
                FillPPNo();
            }
        }
    }
    protected void btnupdate_Click(object sender, EventArgs e)
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
            string column = "";
            switch (ddpercentageexcessqtyfor.SelectedValue)
            {
                case "1":
                    column = "PercentageExecssQty";
                    break;
                case "2":
                    column = "PercentageExecssQtyForIndent";
                    break;
                case "3":
                    column = "PercentageExecssQtyForIndentRawRec";
                    break;
                case "4":
                    column = "PercentageExecssQtyForProcessIss";
                    break;
                default:
                    break;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@columnname", column);
            param[1] = new SqlParameter("@value", txtpercentage.Text == "" ? "0" : txtpercentage.Text);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyId", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            if (TrPPNo.Visible == true)
            {
                param[5] = new SqlParameter("@PPID", DDProcessProgramNo.SelectedValue);
            }
            
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEEXCESSPERCENTAGE", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
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

    protected void DDProcessProgramNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select PercentageExecssQtyForIndent 
            From ProcessProgramExcessPercentage(Nolock) 
            Where PPID = " + DDProcessProgramNo.SelectedValue + @" And MasterCompanyID = " + Session["varMasterCompanyIDForERP"]);
        txtpercentage.Text = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtpercentage.Text = ds.Tables[0].Rows[0]["PercentageExecssQtyForIndent"].ToString();
        }
    }
    protected void ChkForAllPPNo_CheckedChanged(object sender, EventArgs e)
    {
        FillPPNo();
    }
    private void FillPPNo()
    {
        string Str = @"Select Distinct PP.PPID, PP.ChallanNo 
                    From ProcessProgram PP(Nolock) 
                    JOIN OrderMaster OM(Nolock) ON OM.OrderID = PP.Order_ID";
        if (ChkForAllPPNo.Checked == false)
        {
            Str = Str + " And OrderDate > GetDate() - 90 ";
        }
        Str = Str + " Where PP.Process_ID = 5 Order By PP.PPID Desc";

        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, Str, true, "--SELECT--");
    }
}