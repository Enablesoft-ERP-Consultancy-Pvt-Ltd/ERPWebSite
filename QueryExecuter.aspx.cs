using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class QueryExecuter : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (TxtQuery.Text != "")
        {
            DataSet ds = null;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                string strsql = TxtQuery.Text;
                con.Open();
                if (ChkQuery.Checked == true)
                {
                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                    gdcolor.DataSource = ds;
                    gdcolor.DataBind();
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strsql);
                    LblErrorMessage.Text = "SuccessFully Execute";
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "QueryExecuter.aspx");
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * From SESSION");
        if (Ds.Tables[0].Rows.Count > 0)
        {

        }
    }
}