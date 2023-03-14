using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data ;
using System.Data .SqlClient;

public partial class Masters_Carpet_frmParameterMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtParameterid.Text = "0";
            Fill_Grid();

        }
    }
     private void Fill_Grid()
    {
        gvParamerter.DataSource = Fill_Grid_Data();
        gvParamerter.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            string strsql = "select * from PARAMETER_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " order by PARAMETER_ID";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmParameterMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_PARAMETER_MASTER|Fill_Grid_Data|" + ex.Message);
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
    protected void gvParamerter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvParamerter, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvParamerter_SelectedIndexChanged(object sender, EventArgs e)
    {
         string id = gvParamerter.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PARAMETER_MASTER where PARAMETER_ID=" + id + " And MasterCompanyId=" + Session["varCompanyId"] +"");
   try
   {
    if (ds.Tables[0].Rows.Count == 1)
            
       {
           TxtParameterid.Text = ds.Tables[0].Rows[0]["PARAMETER_ID"].ToString();
           txtParameterName.Text = ds.Tables[0].Rows[0]["PARAMETER_NAME"].ToString();
          
        
        }
    }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmParameterMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_PARAMETER_MASTER|cmdSave_Click|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }
            if (con != null)
            {
                con.Dispose();
            }
         }
   

}
protected void BtnSave_Click(object sender, EventArgs e)
{

    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
     try
        {
            SqlParameter[] _arrPara = new SqlParameter[2];
            _arrPara[0] = new SqlParameter("@PARAMETER_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@PARAMETER_NAME", SqlDbType.VarChar, 50);
           
     
            _arrPara[0].Value = Convert.ToInt32(TxtParameterid.Text);
            _arrPara[1].Value = txtParameterName.Text;           

            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_PARAMETER_MASTER", _arrPara);
            TxtParameterid.Text = "0";
            txtParameterName.Text = "";          
            
     }
   catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/frmParameterMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_PARAMETER_MASTER|cmdSave_Click|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }
            if (con != null)
            {
                con.Dispose();
            }
        }
     Fill_Grid();
    }


protected void gvParamerter_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    gvParamerter.PageIndex = e.NewPageIndex;
    Fill_Grid();
}
protected void BtnNew_Click(object sender, EventArgs e)
{
    TxtParameterid.Text = "0";
    txtParameterName.Text = "";
    BtnSave.Text = "Save";
}







}



