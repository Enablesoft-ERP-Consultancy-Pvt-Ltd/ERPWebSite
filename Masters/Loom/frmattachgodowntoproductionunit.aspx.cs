using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Loom_frmattachgodowntoproductionunit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select UnitsId,UnitName from Units order by UnitName
                           select GoDownID,GodownName from  GodownMaster order by GodownName";
                           
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);            
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 1, true, "--Plz Select--");            
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
            SqlParameter [] param=new SqlParameter[5];
            param[0] = new SqlParameter("@ID",SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@Produnitid", DDProdunit.SelectedValue);
            param[2] = new SqlParameter("@Godownid",DDgodown.SelectedValue);
            param[3] = new SqlParameter("@userid",Session["varuserid"]);
            param[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveProductiongodown", param);
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void Fillgrid()
    {
        string str = @"select PU.ID,U.UnitName as productionunit,Gm.GodownName from Productionunitgodown PU inner join Units U on PU.Produnitid=U.UnitsId
                    inner join GodownMaster GM on PU.Godownid=GM.GoDownID Where 1=1";
        if (DDProdunit.SelectedIndex>0)
        {
            str = str + " and PU.Produnitid=" + DDProdunit.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        Gddetail.DataSource = ds.Tables[0];
        Gddetail.DataBind();

    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void Gddetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblId = (Label)Gddetail.Rows[e.RowIndex].FindControl("lblid");
        string str = "Delete from Productionunitgodown Where Id=" + lblId.Text;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        lblmsg.Text = "Row Delete successfully.";
        Fillgrid();
    }
}