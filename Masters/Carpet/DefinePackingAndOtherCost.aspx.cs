using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_DefinePackingAndOtherCost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT COMPANY NAME--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                CompanySelectedChange();
                DDCompanyName.Enabled = false;
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerName, "SELECT CI.CUSTOMERID,CI.CUSTOMERNAME FROM CUSTOMERINFO CI,ORDERMASTER OM WHERE CI.CUSTOMERID=OM.CUSTOMERID AND OM.COMPANYID=" + DDCompanyName.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT CUSTOMER NAME--");
    }
    protected void DDCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "SELECT DISTINCT OM.ORDERID,OM.CUSTOMERORDERNO FROM ORDERMASTER OM WHERE OM.CUSTOMERID=" + DDCustomerName.SelectedValue + "", true, "--SELECT ORDER NUMBER--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        //if (lblMessage.Text == "")
        //{
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[5];
            _arrpara[0] = new SqlParameter("@PRMCID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@CALTYPE", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@AMT", SqlDbType.Float);
            _arrpara[4] = new SqlParameter("@ID", SqlDbType.Int);

            _arrpara[0].Value = 0;
            _arrpara[1].Value = DDOrderNo.SelectedValue;
            _arrpara[2].Value = ddCalType.SelectedIndex;
            _arrpara[3].Value = txtPackingCost.Text;
            _arrpara[4].Direction = ParameterDirection.Output;
            //Select ORDERID,CALTYPE,AMT from PACKING_AND_OTHERMATERIAL_COST
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[dbo].[PRO_PACKING_AND_OTHERMATERIAL_COST]", _arrpara);
            int ID = Convert.ToInt32(_arrpara[4].Value);
            if (ID == 0)
            {
                lblMessage.Text = "DUPLICATE DATA EXISTS.....";
            }
            else if (ID == 1)
            {
                lblMessage.Text = "DATA SAVED SUCESSFULLY....";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefinePackingAndOtherCost.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        txtPackingCost.Text = "";
        Save_Refresh();
        FILLGRID();
        //}
    }
    private void FILLGRID()
    {
        DG.DataSource = Fill_Grid_Data();
        DG.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"=" + DDOrderNo.SelectedValue + "";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefinePackingAndOtherCost.aspx");
            Logs.WriteErrorLog("Charge|Fill_Grid_Data|" + ex.Message);
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
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDOrderNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtPackingCost) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Save_Refresh()
    {
        DDOrderNo.SelectedIndex = 0;
    }
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}