using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_Productionunitmaster : System.Web.UI.UserControl
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State==ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Uid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnUid.Value;
            param[1] = new SqlParameter("@unitName", txtunitname.Text);
            param[2] = new SqlParameter("@address", txtaddress.Text);
            param[3] = new SqlParameter("@Unitcode", txtunitcode.Text);
            param[4] = new SqlParameter("@userid",Session["varuserid"]);
            param[5] = new SqlParameter("@mastercompanyid",Session["varcompanyNo"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveProductionUnit", param);
            Tran.Commit();
            lblmsg.Text = param[6].Value.ToString();
            FillGrid();
            //**********
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
        string str = "select UId,UnitName,Address,Unitcode from ProductionUnitmaster";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }

}