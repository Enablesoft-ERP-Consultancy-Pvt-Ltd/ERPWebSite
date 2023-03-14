using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Data.SqlClient;

/// <summary>
/// Comman function for the whole project
/// </summary>
public class CommanFunction
{
    #region Author: Rajeev//For filling different combos By Dataset Date 27-Nov -12...

    public static void FillComboWithDS(DropDownList comboname, DataSet ds, int i)
    {

        try
        {

            comboname.DataSource = ds.Tables[i];
            comboname.DataValueField = ds.Tables[i].Columns[0].ToString();
            comboname.DataTextField = ds.Tables[i].Columns[1].ToString();
            comboname.DataBind();
            //comboname.Items.Insert(0, new ListItem("<----------Select---------->", "-1"));
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Error in Dropdown filling: " + comboname + " : FillComboWithDS: " + ex.Message);
        }
    }

    #endregion
    //For filling different combos
    public static void FillCombo(DropDownList comboname, string Query)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[1];
            _arrPara[0] = new SqlParameter("@Query", SqlDbType.VarChar, 500);

            _arrPara[0].Value = Query;

            DataSet ds;
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PRO_DROPDOWN_FILL", _arrPara);
            comboname.DataSource = ds.Tables[0];
            comboname.DataValueField = ds.Tables[0].Columns[0].ToString();
            comboname.DataTextField = ds.Tables[0].Columns[1].ToString();
            comboname.DataBind();
            //comboname.Items.Insert(0, new ListItem("<----------Select---------->", "-1"));
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Error in Dropdown filling: " + comboname + " : " + Query + " : " + ex.Message);
        }
        finally
        {
            //if (con.State == ConnectionState.Open)
            //{
            //    con.Close();

            //}
            //if (con != null)
            //{
            //    con.Dispose();
            //}
        }
    }
}