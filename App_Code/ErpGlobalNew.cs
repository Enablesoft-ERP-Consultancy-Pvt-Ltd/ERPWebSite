using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for ErpGlobal
/// </summary>
public static class ErpGlobalNew
{
    static string _dbConnectionString = string.Empty;
    static ErpGlobalNew()
    {
        //
        // TODO: Add constructor logic here
        //
        _dbConnectionString = ConfigurationManager.ConnectionStrings["ExportERPConnectionString"].ToString();
    }
    public static string DBCONNECTIONSTRINGNEW
    {
        get { return _dbConnectionString; }

    }
}
