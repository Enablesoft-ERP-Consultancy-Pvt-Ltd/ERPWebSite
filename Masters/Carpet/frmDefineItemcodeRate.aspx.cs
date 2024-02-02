using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Carpet_frmDefineItemcodeRate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtrate.Focus();
        }
    }
    protected void Fillgrid(int Itemfinishedid)
    {
        string str = "";
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int ItemFinishedid = 0;
            lblmsg.Text = "";
            if (Request.QueryString["Srno"] != null)
            {
                ItemFinishedid = Convert.ToInt32(Request.QueryString["Srno"]);
            }
            string str = "Insert into ItemCodeRate(Item_Finished_id,Rate,userid) values(" + ItemFinishedid + "," + txtrate.Text + "," + Session["varuserid"] + ")";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            lblmsg.Text = "Data saved successfully.";
            txtrate.Text = "";
            Fillgrid(ItemFinishedid);
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}
