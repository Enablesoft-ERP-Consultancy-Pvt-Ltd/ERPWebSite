using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class RptDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // FileInfo ReportPath=new FileInfo (Server.MapPath("CRDemo.Rpt"));
        ReportDocument cryRpt = new ReportDocument();
        cryRpt.Load(Server.MapPath("CRDemo.Rpt"));
        TableLogOnInfos tblLogonInfos = new TableLogOnInfos();
        TableLogOnInfo TblLogonInfo = new TableLogOnInfo();
        ConnectionInfo ConInfo = new ConnectionInfo();
        Tables Tbls;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        ConInfo.ServerName = con.DataSource;
        ConInfo.DatabaseName = con.Database;
        ConInfo.UserID = "sa";
        ConInfo.Password = "eit";
        Tbls = cryRpt.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table tbl1 in Tbls)
        {
            TblLogonInfo = tbl1.LogOnInfo;
            TblLogonInfo.ConnectionInfo = ConInfo;
            tbl1.ApplyLogOnInfo(TblLogonInfo);
        }
        CrystalReportViewer1.ReportSource = cryRpt;
        CrystalReportViewer1.RefreshReport();
    }
}