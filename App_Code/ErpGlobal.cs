using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for ErpGlobal
/// </summary>
public static class ErpGlobal
{
    static string _dbConnectionString = string.Empty;
    static ErpGlobal()
    {
        //
        // TODO: Add constructor logic here
        //
        _dbConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
    }
    public static string DBCONNECTIONSTRING
    {
        get { return _dbConnectionString; }

    }
}
