using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_Campany_FrmAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Fill_Grid();
        }
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select AlertID as Sr_No,AlertName as AlertNAme from AlertName";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");
            Logs.WriteErrorLog("Masters_Campany_FrmAlert|Fill_Grid_Data|" + ex.Message);
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
    private void Fill_Grid()
    {
        gdalert.DataSource = Fill_Grid_Data();
        gdalert.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtDesign.Text != "" && Session["Id"].ToString()!="")
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update AlertNAme set alertname ='"+txtDesign.Text+"' Where alertid="+Session["id"]+"");
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saved Successfully');", true);
            Session["ID"] = "";
            txtDesign.Text = "";
            Fill_Grid();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please first select One Alert');", true);
        }
    }
    protected void gdalert_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = gdalert.SelectedIndex;
        Session["id"] = ((Label)gdalert.Rows[n].FindControl("lblid")).Text;
        string name = ((Label)gdalert.Rows[n].FindControl("lblalertname")).Text;
        txtDesign.Text = name;
        
    }
    protected void gdalert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdalert, "select$" + e.Row.RowIndex);
        }
    }

    protected void gdalert_RowCreated(object sender, GridViewRowEventArgs e)
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