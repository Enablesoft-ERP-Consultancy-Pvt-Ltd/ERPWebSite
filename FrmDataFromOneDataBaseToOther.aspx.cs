using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class FrmDataFromOneDataBaseToOther : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void BtnDeleteData_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "exec [dbo].[Sp_DeleteAllRecordFromDB]'Delete from ?'");
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Insert into Import_NoOfTableUpdate Values(0)");
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('Data successfully deleted..!');", true);
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnPrepareData_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        try
        {
            string Str = @"Select Name from Sysobjects Where  Name not in ('ManagmentToDo','SamplingToDo','sysdiagrams') And xtype='U' Order By Name
                           Select Isnull(Max(ID),0) ID from NoOfTableUpdate";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            int VarID = Convert.ToInt32(Ds.Tables[1].Rows[0]["ID"]);
            int VarI = 0;
            SqlConnection cnn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

            for (int i = VarID; i < VarID + 50 && i < Ds.Tables[0].Rows.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + Ds.Tables[0].Rows[i]["Name"] + "", cnn);
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlDataReader rdr = cmd.ExecuteReader();
                SqlBulkCopy sbc = new SqlBulkCopy(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
                sbc.DestinationTableName = "Import_" + Ds.Tables[0].Rows[i]["Name"] + "";
                sbc.WriteToServer(rdr);
                if (Ds.Tables[0].Rows.Count == i)
                {
                    i = Ds.Tables[0].Rows.Count;
                }
                VarI = i;
                sbc.Close();
                rdr.Close();
                SqlHelper.ExecuteNonQuery(cnn, CommandType.Text, "Update NoOfTableUpdate Set Id=" + (i + 1));
            }
            cnn.Close();
            string Msg = "" + (VarID + 50) + " table data updated out of " + Ds.Tables[0].Rows.Count + " for more pls click again";
            if (VarI >= Ds.Tables[0].Rows.Count)
            {
                Msg = "Updated all table data";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update NoOfTableUpdate Set Id=0");
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('" + Msg + "..!');", true);
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
    }
    protected void BtnTranferData_Click(object sender, EventArgs e)
    {
        //// Establishing connection
        //SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
        //cb.DataSource = "EIT1-PC\MSSQLSERVER2008";
        //cb.InitialCatalog = "ExportERP2";
        //cb.IntegratedSecurity = true;
        //SqlConnection cnn = new SqlConnection(cb.ConnectionString);

        SqlConnection cnn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        // Getting source data
        SqlCommand cmd = new SqlCommand("SELECT * FROM CarpetNumber", cnn);
        cnn.Open();
        SqlDataReader rdr = cmd.ExecuteReader();

        // Initializing an SqlBulkCopy object
        //SqlBulkCopy sbc = new SqlBulkCopy("server=198.58.81.238;database=RudraRugs;Integrated Security=SSPI");
        SqlBulkCopy sbc = new SqlBulkCopy("Data Source=198.58.81.238;Initial Catalog=RudraRugs;Persist Security Info=True;User ID=rudra;Password=rajsaini218");

        // Copying data to destination
        sbc.DestinationTableName = "Import_CarpetNumber";
        sbc.WriteToServer(rdr);

        // Closing connection and the others
        sbc.Close();
        rdr.Close();
        cnn.Close();
    }
}