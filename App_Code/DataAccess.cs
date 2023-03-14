using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Reflection.Emit;
using System.Data.Common;
/// <summary>
/// Summary description for command
/// </summary>
public class DataAccess
{

    public DataAccess()
    {
        //
        // TODO: Add constructor logic here
        //
    }   

    /// <summary>
    /// Execute queries for Select
    /// </summary>
    /// <param name="name"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static DataTable fetch(string name, Hashtable param)
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["QmsConnectionString"].ConnectionString);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            Hashtable ht = new Hashtable();
            ht = param;
            foreach (DictionaryEntry entry in ht)
            {
                //object o = entry.Value;
                cmd.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
            }
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally 
        { 
            con.Close();
        }
    }

    /// <summary>
    /// Execute queries like insert,update,delete
    /// </summary>
    /// <param name="name"></param>
    /// <param name="param"></param>
    public static void execute(string name, Hashtable param)
    {
      //  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["QmsConnectionString"].ConnectionString);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            Hashtable ht = new Hashtable();
            ht = param;
            foreach (DictionaryEntry entry in ht)
            {
                //object o = entry.Value;
                cmd.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);                
            }
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally 
        { 
            con.Close();
        }
    }
}